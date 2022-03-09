using Artaxias.Core.Constants.Domain;
using Artaxias.Data;
using Artaxias.Data.Models.Feadback;
using Artaxias.Web.Common.DataTransferObjects.Feadback;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaxias.BusinessLogic.Feedbacks
{
    public class TemplateManager : ITemplateManager
    {
        private readonly IRepository<FeedbackTemplate> _feadbackTemplateRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<AnswerValue> _answerValueRepository;
        private readonly IRepository<AnswerOptionType> _answerOptionTypeRepository;

        public TemplateManager(
            IRepository<FeedbackTemplate> feadbackTemplateRepository,
            IRepository<AnswerValue> answerValueRepository,
            IRepository<Question> questionRepository,
            IRepository<AnswerOptionType> answerOptionTypeRepository)
        {
            _feadbackTemplateRepository = feadbackTemplateRepository;
            _answerValueRepository = answerValueRepository;
            _questionRepository = questionRepository;
            _answerOptionTypeRepository = answerOptionTypeRepository;
        }

        public async Task<TemplateResponse> CreateAsync(TemplateRequest request)
        {
            FeedbackTemplate template = new FeedbackTemplate
            {
                Title = request.Title,
                CreatedByUserId = request.CreatedByUserId,
                Questions = new List<Question>()
            };
            foreach (QuestionRequest questionRequest in request.Questions)
            {
                Question newQuestion = new Question
                {
                    Title = questionRequest.Title,
                    Description = questionRequest.Description,
                    AnswerOption = new AnswerOption { AnswerOptionTypeId = questionRequest.Answer.OptionType.Id }
                };
                if (questionRequest.Answer.OptionType.Description != AnswerOptionTypes.OpenText)
                {
                    foreach (AnswerValueRequest answer in questionRequest.Answer.Values)
                    {
                        AnswerValue answerValue = new AnswerValue { Value = answer.Value };
                        newQuestion.AnswerOption.Values.Add(answerValue);
                    }
                }
                template.Questions.Add(newQuestion);
            }
            _feadbackTemplateRepository.Insert(template);
            await _feadbackTemplateRepository.SaveChangesAsync();

            return new TemplateResponse { Id = template.Id };
        }

        public async Task<TemplateResponse> GetAsync(int key)
        {
            TemplateResponse template = await _feadbackTemplateRepository.Get(t => t.Id == key)
                   .Select(t => new TemplateResponse
                   {
                       Id = t.Id,
                       CreatedByUserId = t.CreatedByUserId,
                       CreatorName = $"{t.CreatedByUser.FirstName} {t.CreatedByUser.LastName}",
                       Title = t.Title,
                       InUse = t.Reviews.Any(r => r.ReviewerReviewees.Any(rr => rr.FeedbackId != null)),
                       Questions = t.Questions.Select(q => new QuestionResponse
                       {
                           Id = q.Id,
                           Title = q.Title,
                           Description = q.Description,
                           Answer = new AnswerResponse
                           {
                               Id = q.AnswerOption.Id,
                               OptionType = new AnswerOptionTypeDetails { Id = q.AnswerOption.OptionType.Id, Description = q.AnswerOption.OptionType.Description },
                               Values = q.AnswerOption.Values.Select(v => new AnswerValueResponse
                               {
                                   Id = v.Id,
                                   Value = v.Value
                               })
                               .ToList()
                           }
                       }).ToList()
                   }).FirstOrDefaultAsync();

            return template;

        }

        public async Task<List<TemplateResponse>> GetListAsync(int pageSize = 10, int currentPage = 0)
        {
            List<TemplateResponse> templates = await _feadbackTemplateRepository.Get()
                  .Skip(currentPage * pageSize)
                  .Take(pageSize)
                  .Select(t => new TemplateResponse
                  {
                      Id = t.Id,
                      CreatedByUserId = t.CreatedByUserId,
                      CreatorName = $"{t.CreatedByUser.FirstName} {t.CreatedByUser.LastName}",
                      Title = t.Title,
                      InUse = t.Reviews.Any(r => r.ReviewerReviewees.Any(rr => rr.FeedbackId != null)),
                      Questions = t.Questions.Select(q => new QuestionResponse
                      {
                          Id = q.Id,
                          Title = q.Title,
                          Description = q.Description,
                          Answer = new AnswerResponse
                          {
                              Id = q.AnswerOption.Id,
                              OptionType = new AnswerOptionTypeDetails { Id = q.AnswerOption.OptionType.Id, Description = q.AnswerOption.OptionType.Description },
                              Values = q.AnswerOption.Values.Select(v => new AnswerValueResponse
                              {
                                  Id = v.Id,
                                  Value = v.Value
                              }).ToList()
                          }
                      }).ToList()
                  }).ToListAsync();

            return templates;
        }

        public async Task<TemplateResponse> UpdateAsync(int id, TemplateRequest request)
        {
            List<AnswerOptionType> optionTypes = _answerOptionTypeRepository.Get().ToList();
            bool inUse = _feadbackTemplateRepository.Get(t => t.Id == id).Any(a => a.Reviews.Any(r => r.ReviewerReviewees.Any(rr => rr.FeedbackId != null)));
            if (inUse)
            {
                throw new ArgumentException("You cannot modify template if it is used to provide feedback.");
            }

            FeedbackTemplate template = await _feadbackTemplateRepository.Get(t => t.Id == id)
                .Include(t => t.Questions)
                .ThenInclude(q => q.AnswerOption)
                .ThenInclude(ao => ao.Values)
                .FirstOrDefaultAsync();
            if (template == null || inUse)
            {
                throw new ArgumentException("Feedback Template was not found.");
            }

            template.Title = request.Title;
            template.CreatedByUserId = request.CreatedByUserId;

            foreach (Question question in template.Questions)
            {
                if (!request.Questions.Any(q => q.Id == question.Id))
                {
                    _questionRepository.Delete(question);
                }
            }
            await _questionRepository.SaveChangesAsync();

            foreach (QuestionRequest questionRequest in request.Questions)
            {
                Question newQuestion = template.Questions.FirstOrDefault(q => q.Id == questionRequest.Id && q.Id > 0);
                if (newQuestion == null)
                {
                    newQuestion = new Question();
                }

                newQuestion.Title = questionRequest.Title;
                newQuestion.Description = questionRequest.Description;
                newQuestion.AnswerOption.AnswerOptionTypeId = questionRequest.Answer.OptionType.Id;
                AnswerOptionType optionType = optionTypes.FirstOrDefault(t => t.Id == questionRequest.Answer.OptionType.Id);
                if (optionType == null)
                {
                    throw new ArgumentException("Answer Option Type is incorrect.");
                }

                foreach (AnswerValue item in newQuestion.AnswerOption.Values)
                {
                    _answerValueRepository.Delete(item);
                }
                await _answerValueRepository.SaveChangesAsync();

                if (optionType.Description != AnswerOptionTypes.OpenText)
                {
                    foreach (AnswerValueRequest answer in questionRequest.Answer.Values)
                    {
                        AnswerValue answerValue = new AnswerValue { Value = answer.Value };
                        newQuestion.AnswerOption.Values.Add(answerValue);
                    }
                }
                if (newQuestion.Id == 0)
                {
                    template.Questions.Add(newQuestion);
                }
            }
            _feadbackTemplateRepository.Update(template);
            await _feadbackTemplateRepository.SaveChangesAsync();

            return new TemplateResponse { Id = template.Id };
        }

        public async Task DeleteAsync(int key)
        {
            FeedbackTemplate template = await _feadbackTemplateRepository.Get(template => template.Id == key).FirstOrDefaultAsync();
            if (template == null)
            {
                throw new ArgumentException("Feedback Template was not found.");
            }

            _feadbackTemplateRepository.Delete(template);
            await _feadbackTemplateRepository.SaveChangesAsync();
        }

        public async Task<List<AnswerOptionTypeDetails>> GetAllAnswerOptionTypes()
        {
            List<AnswerOptionTypeDetails> answerOptionTypes = await _answerOptionTypeRepository.Get()
                                                                     .Select(t => new AnswerOptionTypeDetails
                                                                     {
                                                                         Id = t.Id,
                                                                         Description = t.Description
                                                                     }).ToListAsync();
            return answerOptionTypes;
        }
    }
}
