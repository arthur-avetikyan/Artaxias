using Artaxias.Core.Constants.Domain;
using Artaxias.Core.Enums;
using Artaxias.Core.Exceptions;
using Artaxias.Data;
using Artaxias.Data.Models.Feadback;
using Artaxias.Web.Common.DataTransferObjects.Feadback;
using Artaxias.Web.Common.DataTransferObjects.Organization;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Feedbacks
{
    public class FeedbackManager : IFeedbackManager
    {
        private readonly IRepository<Feedback> _feedbackRepository;
        private readonly IRepository<ReviewerReviewee> _reviewerRevieweeRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<AnswerOptionType> _answerOptionTypeRepository;
        private readonly ITemplateManager _templateManager;

        public FeedbackManager(IRepository<Feedback> feedbackRepository,
                               ITemplateManager templateManager,
                               IRepository<ReviewerReviewee> reviewerRevieweeRepository,
                               IRepository<Question> questionRepository,
                               IRepository<AnswerOptionType> answerOptionTypeRepository)
        {
            _feedbackRepository = feedbackRepository;
            _templateManager = templateManager;
            _reviewerRevieweeRepository = reviewerRevieweeRepository;
            _questionRepository = questionRepository;
            _answerOptionTypeRepository = answerOptionTypeRepository;
        }

        public async Task<ProvideFeedbackResponse> GetFeedbackResponseAsync(int reviewerRevieweeId)
        {
            ProvideFeedbackResponse requestFeedback = await _reviewerRevieweeRepository.Get(rf => rf.Id == reviewerRevieweeId)
                .Select(rf => new ProvideFeedbackResponse
                {
                    Id = rf.Id,
                    Reviewer = new EmployeeInfo
                    {
                        Id = rf.RevieweeId,
                        FirstName = rf.Reviewer.User.FirstName,
                        LastName = rf.Reviewer.User.LastName
                    },
                    Reviewee = new EmployeeInfo
                    {
                        Id = rf.RevieweeId,
                        FirstName = rf.Reviewee.User.FirstName,
                        LastName = rf.Reviewee.User.LastName
                    },
                    Template = new TemplateResponse { Id = rf.Review.FeedbackTemplateId }
                })
                .FirstOrDefaultAsync();

            requestFeedback.Template = (await _templateManager.GetAsync(requestFeedback.Template.Id));
            return requestFeedback;
        }

        public async Task<ProvideFeedbackResponse> CreateFeedbackAsync(ProvideFeedbackRequest request)
        {
            ReviewerReviewee reviewerReviewee = await _reviewerRevieweeRepository.Get(rr => rr.Id == request.ReviewerRevieweeId).Include(rr => rr.Feadback).FirstOrDefaultAsync();
            if (reviewerReviewee == null)
            {
                throw new ArgumentException("Data is not found.");
            }

            if (reviewerReviewee.FeedbackId != null)
            {
                throw new DomainException("You have already provided feedback for this review.");
            }

            Feedback feedback = new Feedback
            {
                ReviewerRevieweeId = reviewerReviewee.Id,
                ProvidedAt = DateTime.UtcNow
            };
            foreach (ProvideFeedbackAnswerRequest answer in request.FeedbackAnswerRequests)
            {
                Question question = _questionRepository.Get(q => q.Id == answer.QuestionId).Include(q => q.AnswerOption.OptionType).FirstOrDefault();
                List<AnswerOptionType> answerTypes = _answerOptionTypeRepository.Get().ToList();

                if (question == null)
                {
                    throw new ArgumentException("Question is not found.");
                }

                if (question.AnswerOption.OptionType.Description != answer.OptionType.Description)
                {
                    throw new ArgumentException("Answer Option Type is incorrect.");
                }

                FeedbackAnswer feedbackAnswer = new FeedbackAnswer { QuestionId = answer.QuestionId };
                if (answer.OptionType.Description == AnswerOptionTypes.OpenText)
                {
                    feedbackAnswer.OpenTextValue = answer.OpenTextValue;
                }
                else
                {
                    foreach (int answerValueId in answer.AnswerValueIds)
                    {
                        feedbackAnswer.FeedbackAnswerValues.Add(new FeedbackAnswerValue { AnswerValueId = answerValueId });
                    }
                }

                feedback.FeedbackAnswers.Add(feedbackAnswer);
            }
            _feedbackRepository.Insert(feedback);
            await _feedbackRepository.SaveChangesAsync();

            int feedbackId = _feedbackRepository.Get(f => f.ReviewerRevieweeId == reviewerReviewee.Id).Select(f => f.Id).FirstOrDefault();
            reviewerReviewee.FeedbackId = feedbackId;
            reviewerReviewee.DomainStateId = (int)EDomainState.Completed;
            _reviewerRevieweeRepository.Update(reviewerReviewee);
            await _reviewerRevieweeRepository.SaveChangesAsync();

            return new ProvideFeedbackResponse
            {
                Id = reviewerReviewee.Id,
                Reviewee = new EmployeeInfo { Id = reviewerReviewee.RevieweeId },
                Reviewer = new EmployeeInfo { Id = reviewerReviewee.ReviewerId },
                Template = new TemplateResponse { Id = reviewerReviewee.Review.FeedbackTemplateId }
            };
        }

        public async Task<List<FeedbackAboutEmployeeResponse>> GetFeedbackAboutEmployee(int employeeId, int reviewId)
        {
            List<FeedbackAboutEmployeeResponse> feedbacks = await _feedbackRepository.Get(f => f.ReviewerReviewee.RevieweeId == employeeId)
                                                     .Where(f => f.ReviewerReviewee.ReviewId == reviewId)
                                                     .Select(f => new FeedbackAboutEmployeeResponse
                                                     {
                                                         Id = f.Id,
                                                         ReviewerRevieweeId = f.ReviewerRevieweeId,
                                                         Reviewer = new EmployeeInfo
                                                         {
                                                             Id = f.ReviewerReviewee.RevieweeId,
                                                             FirstName = f.ReviewerReviewee.Reviewer.User.FirstName,
                                                             LastName = f.ReviewerReviewee.Reviewer.User.LastName
                                                         },
                                                         Reviewee = new EmployeeInfo
                                                         {
                                                             Id = f.ReviewerReviewee.RevieweeId,
                                                             FirstName = f.ReviewerReviewee.Reviewee.User.FirstName,
                                                             LastName = f.ReviewerReviewee.Reviewee.User.LastName
                                                         },
                                                         QuestionAnswers = f.FeedbackAnswers.Select(a => new FeedbackQuestionAnswerResponse
                                                         {
                                                             Title = a.Question.Title,
                                                             Description = a.Question.Description,
                                                             AnswerValues = a.OpenTextValue == null ?
                                                                             a.FeedbackAnswerValues.Select(v => v.AnswerValue.Value).ToList() :
                                                                             new List<string> { a.OpenTextValue }
                                                         }).ToList()
                                                     })
                                                     .ToListAsync();
            return feedbacks;
        }

        public async Task<List<FeedbackDetails>> GetListAsync(int pageSize, int pageNumber)
        {
            List<FeedbackDetails> feedbacks = await _feedbackRepository.Get()
                                                     .Skip(pageNumber * pageSize)
                                                     .Take(pageSize)
                                                     .OrderByDescending(f => f.ProvidedAt)
                                                     .Select(f => new FeedbackDetails
                                                     {
                                                         ReviewId = f.ReviewerReviewee.ReviewId,
                                                         ReviewTitle = f.ReviewerReviewee.Review.Title,
                                                         ReviewerRevieweeId = f.ReviewerRevieweeId,
                                                         ProvidedDate = f.ProvidedAt,
                                                         Reviewer = new EmployeeInfo
                                                         {
                                                             Id = f.ReviewerReviewee.RevieweeId,
                                                             FirstName = f.ReviewerReviewee.Reviewer.User.FirstName,
                                                             LastName = f.ReviewerReviewee.Reviewer.User.LastName
                                                         },
                                                         Reviewee = new EmployeeInfo
                                                         {
                                                             Id = f.ReviewerReviewee.RevieweeId,
                                                             FirstName = f.ReviewerReviewee.Reviewee.User.FirstName,
                                                             LastName = f.ReviewerReviewee.Reviewee.User.LastName
                                                         },
                                                     })
                                                     .ToListAsync();
            return feedbacks;
        }
    }
}
