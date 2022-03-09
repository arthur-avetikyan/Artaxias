using Artaxias.BusinessLogic.FileManagement;
using Artaxias.Core.Configurations;
using Artaxias.Core.Enums;
using Artaxias.Data;
using Artaxias.Data.Models.Feadback;
using Artaxias.Mailing;
using Artaxias.Models;
using Artaxias.Web.Common.DataTransferObjects.Feadback;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Feedbacks
{
    public class ReviewManager : IReviewManager
    {
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IFileManager _fileManager;
        private readonly IEmailProcessor _emailProcessor;

        private readonly IRepository<FeedbackTemplate> _feadbackTemplateRepository;
        private readonly IRepository<Review> _reviewRepository;
        private readonly IRepository<ReviewerReviewee> _reviewerRevieweeRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<ReviewDepartment> _reviewDepartmentRepository;

        public ReviewManager(IOptions<ApplicationConfiguration> applicationConfiguration,
                             IRepository<FeedbackTemplate> feadbackTemplateRepository,
                             IRepository<Review> reviewRepository,
                             IEmailProcessor emailProcessor,
                             IFileManager fileManager,
                             IRepository<User> userRepository,
                             IRepository<ReviewerReviewee> reviewerRevieweeRepository,
                             IRepository<ReviewDepartment> reviewDepartmentRepository)
        {
            _applicationConfiguration = applicationConfiguration.Value;
            _feadbackTemplateRepository = feadbackTemplateRepository;
            _reviewRepository = reviewRepository;
            _emailProcessor = emailProcessor;
            _fileManager = fileManager;
            _userRepository = userRepository;
            _reviewerRevieweeRepository = reviewerRevieweeRepository;
            _reviewDepartmentRepository = reviewDepartmentRepository;
        }

        public async Task<ReviewResponse> CreateAsync(ReviewRequest request, string requesterUserName)
        {
            FeedbackTemplate template = await _feadbackTemplateRepository.Get(t => t.Id == request.TemplateId).FirstOrDefaultAsync();
            User user = await _userRepository.Get(u => u.UserName == requesterUserName).FirstOrDefaultAsync();
            if (template == null || user == null)
            {
                throw new ArgumentException("Data is missing or incorrect");
            }

            Review review = new Review
            {
                Title = request.Title,
                CreatedDateTimeUTC = DateTime.UtcNow,
                Deadline = request.Deadline,
                FeedbackTemplateId = template.Id,
                CreatedByUserId = user.Id
            };

            foreach (ReviewerRevieweeInfo item in request.ReviewerReviewees)
            {
                review.ReviewerReviewees.Add(new ReviewerReviewee
                {
                    RevieweeId = item.Reviewee.Id,
                    ReviewerId = item.Reviewer.Id,
                    DomainStateId = (int)EDomainState.Pending
                });
            }

            foreach (int item in request.DepartmentIds)
            {
                review.Departments.Add(new ReviewDepartment { DepartmentId = item });
            }

            _reviewRepository.Insert(review);
            await _reviewRepository.SaveChangesAsync();

            foreach (ReviewerReviewee reviewerReviewee in review.ReviewerReviewees)
            {
                await NotifyReviewersByEmail(reviewerReviewee.Id);
            }

            return new ReviewResponse();
        }

        public async Task<ReviewResponse> UpdateAsync(int reviewId, ReviewRequest request)
        {
            FeedbackTemplate template = await _feadbackTemplateRepository.Get(t => t.Id == request.TemplateId).FirstOrDefaultAsync();
            Review review = await _reviewRepository.Get(r => r.Id == reviewId).Include(r => r.ReviewerReviewees).FirstOrDefaultAsync();
            if (template == null || review == null)
            {
                throw new ArgumentException("Data is missing or incorrect");
            }

            review.Title = request.Title;
            review.Deadline = request.Deadline;
            review.FeedbackTemplateId = template.Id;

            foreach (ReviewerReviewee item in review.ReviewerReviewees)
            {
                if (item.DomainStateId > 1)
                {
                    continue;
                }

                _reviewerRevieweeRepository.Delete(item);
            }
            await _reviewerRevieweeRepository.SaveChangesAsync();

            foreach (ReviewerRevieweeInfo item in request.ReviewerReviewees)
            {
                if (!review.ReviewerReviewees.Any(rr => rr.ReviewerId == item.Reviewer.Id && rr.RevieweeId == item.Reviewee.Id))
                {
                    review.ReviewerReviewees.Add(new ReviewerReviewee
                    {
                        RevieweeId = item.Reviewee.Id,
                        ReviewerId = item.Reviewer.Id,
                        DomainStateId = (int)EDomainState.Pending
                    });
                }
            }

            IEnumerable<int> newDepartments = request.DepartmentIds.Where(departmentId => !review.Departments.Any(s => s.DepartmentId == departmentId));
            IEnumerable<ReviewDepartment> formerDepartments = review.Departments.Where(reviewDepartment => !request.DepartmentIds.Contains(reviewDepartment.DepartmentId));

            foreach (int id in newDepartments)
            {
                review.Departments.Add(new ReviewDepartment { DepartmentId = id });
            }

            foreach (ReviewDepartment item in formerDepartments)
            {
                _reviewDepartmentRepository.Delete(item);
            }

            _reviewRepository.Update(review);
            await _reviewRepository.SaveChangesAsync();
            await _reviewDepartmentRepository.SaveChangesAsync();

            foreach (ReviewerReviewee reviewerReviewee in review.ReviewerReviewees.Where(rr => rr.DomainStateId != (int)EDomainState.Completed))
            {
                await NotifyReviewersByEmail(reviewerReviewee.Id);
            }

            //TODO mapping
            return new ReviewResponse { Id = review.Id };
        }

        private async Task NotifyReviewersByEmail(int reviewerRevieweeId)
        {
            var reviewerReviewee = await _reviewerRevieweeRepository.Get(rr => rr.Id == reviewerRevieweeId)
                                                                    .Where(rr => rr.DomainStateId > 1)
                                                                    .Select(rr => new
                                                                    {
                                                                        Id = rr.Id,
                                                                        Deadline = rr.Review.Deadline,
                                                                        RequesterFullName = $"{rr.Review.CreatedByUser.FirstName} {rr.Review.CreatedByUser.LastName}",
                                                                        ReviewerUserName = rr.Reviewer.User.UserName,
                                                                        ReviewerFullName = $"{rr.Reviewer.User.FirstName} {rr.Reviewer.User.LastName}",
                                                                        ReviewerEmail = rr.Reviewer.User.Email,
                                                                        RevieweeFullName = $"{rr.Reviewee.User.FirstName} {rr.Reviewee.User.LastName}"
                                                                    })
                                                                    .FirstOrDefaultAsync();
            if (reviewerReviewee == null)
            {
                return;
            }

            EmailConfigurationParameters emailMessage = new EmailConfigurationParameters();
            string emailTemplate = _fileManager.ReadFile(
                Path.Combine(_applicationConfiguration.EmailTemplatesFilePath, "ProvideFeedbackEmail.template.html"));
            ProvideFeedbackParameters provideFeedbackEmailParameters = new ProvideFeedbackParameters
            {
                CallbackUrl = $"{_applicationConfiguration.Url}/feedbacks/{reviewerReviewee.ReviewerUserName}/{reviewerReviewee.Id}",
                Template = emailTemplate,
                FeedbackId = reviewerReviewee.Id,
                Deadline = reviewerReviewee.Deadline,
                Requester = reviewerReviewee.RequesterFullName,
                Recipient = reviewerReviewee.ReviewerEmail,
                Reviewer = reviewerReviewee.ReviewerFullName,
                Reviewee = reviewerReviewee.RevieweeFullName
            };
            emailMessage.BuildProvideFeedbackEmail(provideFeedbackEmailParameters);
            emailMessage.DestinationAddresses.Add(reviewerReviewee.ReviewerEmail);
            try
            {
                await _emailProcessor.SendAsync(emailMessage);
            }
            catch (Exception)
            {
                //_logger.LogInformation("New user email failed: Body: {0}, Error: {1}", emailMessage.Body, ex.Message);
            }
        }

        public async Task DeleteAsync(int key)
        {
            Review review = await _reviewRepository.Get(r => r.Id == key).Include(r => r.ReviewerReviewees).FirstOrDefaultAsync();
            if (review == null)
            {
                throw new ArgumentException("Review not found.");
            }

            if (review.ReviewerReviewees.Any(rr => rr.FeedbackId != null))
            {
                throw new AccessViolationException("You can not delete this review. Employees have already given feedbacks.");
            }

            foreach (ReviewerReviewee item in review.ReviewerReviewees)
            {
                _reviewerRevieweeRepository.Delete(item);
            }

            await _reviewerRevieweeRepository.SaveChangesAsync();

            _reviewRepository.Delete(review);
            await _reviewRepository.SaveChangesAsync();
        }

        public async Task<ReviewResponse> GetAsync(int key)
        {
            ReviewResponse review = await _reviewRepository.Get(review => review.Id == key)
                                                .Select(review => new ReviewResponse
                                                {
                                                    Id = review.Id,
                                                    Title = review.Title,
                                                    Created = review.CreatedDateTimeUTC,
                                                    Deadline = review.Deadline,
                                                    Template = new TemplateResponse
                                                    {
                                                        Id = review.FeedbackTemplate.Id,
                                                        CreatedByUserId = review.FeedbackTemplate.CreatedByUserId,
                                                        CreatorName = $"{review.FeedbackTemplate.CreatedByUser.FirstName} {review.FeedbackTemplate.CreatedByUser.LastName}",
                                                        Title = review.FeedbackTemplate.Title
                                                    },
                                                    ReviewerReviewees = new List<ReviewerRevieweeInfo>(),
                                                    Departments = new List<DepartmentInfo>()
                                                })
                                                .FirstOrDefaultAsync();

            review.ReviewerReviewees = await _reviewerRevieweeRepository.Get(reviewerReviewee => reviewerReviewee.ReviewId == key)
                                                                        .Select(reviewerReviewee => new ReviewerRevieweeInfo
                                                                        {
                                                                            Reviewee = new EmployeeInfo
                                                                            {
                                                                                Id = reviewerReviewee.RevieweeId,
                                                                                FirstName = reviewerReviewee.Reviewee.User.FirstName,
                                                                                LastName = reviewerReviewee.Reviewee.User.LastName,
                                                                            },
                                                                            Reviewer = new EmployeeInfo
                                                                            {
                                                                                Id = reviewerReviewee.ReviewerId,
                                                                                FirstName = reviewerReviewee.Reviewer.User.FirstName,
                                                                                LastName = reviewerReviewee.Reviewer.User.LastName,
                                                                            },
                                                                            StateId = reviewerReviewee.DomainStateId
                                                                        })
                                                                        .ToListAsync();

            review.Departments = await _reviewDepartmentRepository.Get(d => d.ReviewId == key)
                                                                  .Select(r => new DepartmentInfo
                                                                  {
                                                                      DepartmentId = r.DepartmentId,
                                                                      DepartmentName = r.Department.Name
                                                                  })
                                                                  .ToListAsync();

            SetReviewState(review);
            return review;
        }

        public async Task<List<ReviewResponse>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            List<ReviewResponse> reviews = await _reviewRepository.Get()
                                                 .Skip(currentPage * pageSize)
                                                 .Take(pageSize)
                                                 .OrderByDescending(r => r.CreatedDateTimeUTC)
                                                 .Select(r => new ReviewResponse
                                                 {
                                                     Id = r.Id,
                                                     Title = r.Title,
                                                     Created = r.CreatedDateTimeUTC,
                                                     Deadline = r.Deadline,
                                                     Template = new TemplateResponse
                                                     {
                                                         Id = r.FeedbackTemplate.Id,
                                                         CreatedByUserId = r.FeedbackTemplate.CreatedByUserId,
                                                         CreatorName = $"{r.FeedbackTemplate.CreatedByUser.FirstName} {r.FeedbackTemplate.CreatedByUser.LastName}",
                                                         Title = r.FeedbackTemplate.Title
                                                     },
                                                     Departments = r.Departments.Select(d => new DepartmentInfo
                                                     {
                                                         DepartmentId = d.DepartmentId,
                                                         DepartmentName = d.Department.Name
                                                     }).ToList()
                                                 })
                                                 .ToListAsync();
            foreach (ReviewResponse review in reviews)
            {
                SetReviewState(review);
            }

            return reviews;
        }

        public async Task<List<ReviewAboutEmployeeResponse>> GetReviewsAboutEmployee(int employeeId)
        {
            List<ReviewAboutEmployeeResponse> feedbacks = await _reviewerRevieweeRepository.Get(r => r.RevieweeId == employeeId)
                                                             .Where(r => r.FeedbackId != null)
                                                             .Select(r => new ReviewAboutEmployeeResponse
                                                             {
                                                                 ReviewId = r.ReviewId,
                                                                 ReviewTitle = r.Review.Title,
                                                                 ProvidedDate = r.Feadback.ProvidedAt
                                                             })
                                                             .ToListAsync();
            return feedbacks;
        }

        private void SetReviewState(ReviewResponse review)
        {
            List<Data.Models.DomainState> statesOfReview = _reviewerRevieweeRepository.Get(r => r.ReviewId == review.Id).Select(rr => rr.DomainState).ToList();
            if (statesOfReview.All(rr => rr.Id == (int)EDomainState.Pending))
            {
                review.StateId = (int)EDomainState.Pending;
            }
            else if (statesOfReview.All(rr => rr.Id == (int)EDomainState.Completed))
            {
                review.StateId = (int)EDomainState.Completed;
            }
            else
            {
                review.StateId = (int)EDomainState.Processing;
            }
        }


        public Task<ReviewResponse> CreateAsync(ReviewRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
