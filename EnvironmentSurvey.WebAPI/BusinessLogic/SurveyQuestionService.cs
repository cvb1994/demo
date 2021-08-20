using EnvironmentSurvey.WebAPI.ClientSide.Models;
using EnvironmentSurvey.WebAPI.DataAccess.Domains;
using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.BusinessLogic
{
    public interface ISurveyQuestionService
    {
        bool Create(List<SurveyQuestionModel> model);
        List<SurveyQuestionModel> GetAll();
        SurveyQuestionModel GetById(int Id);
        bool Update(List<SurveyQuestionModel> model);
        bool Delete(int Id);
    }
    public class SurveyQuestionService : ISurveyQuestionService
    {
        private readonly IRepository<SurveyQuestion> _surveyQuestionRepository;
        private readonly IRepository<UserAnswer> _userAnswerRepository;
        public SurveyQuestionService(IRepository<SurveyQuestion> surveyQuestionRepository,
            IRepository<UserAnswer> userAnswerRepository
            )
        {
            _surveyQuestionRepository = surveyQuestionRepository;
            _userAnswerRepository = userAnswerRepository;
        }
        public bool Create(List<SurveyQuestionModel> model)
        {
            foreach (var item in model)
            {
                var surveyQuestion = new SurveyQuestion
                {
                    SurveyId = item.SurveyId,
                    QuestionId = item.QuestionId
                };
                _surveyQuestionRepository.Insert(surveyQuestion);
                item.Id = surveyQuestion.Id;
            }
            return true;
        }

        public bool Delete(int Id)
        {
            SurveyQuestion surveyQuestion = _surveyQuestionRepository.Get(Id);
            if (surveyQuestion == null)
                throw new Exception("Survey Question not found");
            else
            {
                _surveyQuestionRepository.Delete(surveyQuestion);
                return true;
            }
        }

        public List<SurveyQuestionModel> GetAll()
        {
            var listUserAnswer = _userAnswerRepository.GetAll().ToList();
            List<UserAnswerModel> listUserAnswerModel = new();
            if (listUserAnswer.Count > 0)
            {
                listUserAnswerModel = listUserAnswer.Select(x => new UserAnswerModel
                {
                    Id = x.Id,
                    SurveyQuestionId = x.SurveyQuestionId,
                    AnswerId = x.AnswerId,
                    UserId = x.UserId
                }).ToList();
            }

            var listSurveyQuestion = _surveyQuestionRepository.GetAll().ToList();
            if (listSurveyQuestion.Count == 0)
                throw new Exception("There is no SurveyQuestion existed");
            var result = listSurveyQuestion.Select(x => new SurveyQuestionModel
            {
                Id = x.Id,
                SurveyId = x.SurveyId,
                QuestionId = x.QuestionId,
                UserAnswers = listUserAnswerModel.Count > 0 ? listUserAnswerModel.Where(y => y.SurveyQuestionId == x.Id).ToList() : null,
            }).ToList();
            return result;
        }

        public SurveyQuestionModel GetById(int Id)
        {
            SurveyQuestionModel model = new();
            SurveyQuestion surveyQuestion = _surveyQuestionRepository.Get(Id);
            List<UserAnswer> listUserAnswer = _userAnswerRepository.GetAll().Where(x => x.SurveyQuestionId == Id).ToList();
            List<UserAnswerModel> listUserAnswerModel = new();
            if (listUserAnswer.Count > 0)
            {
                listUserAnswerModel = listUserAnswer.Select(x => new UserAnswerModel
                {
                    Id = x.Id,
                    SurveyQuestionId = x.SurveyQuestionId,
                    AnswerId = x.AnswerId,
                    UserId = x.UserId
                }).ToList();
            }
            if (surveyQuestion == null)
                throw new Exception("Survey Question not found");
            else
            {
                model.Id = surveyQuestion.Id;
                model.SurveyId = surveyQuestion.SurveyId;
                model.QuestionId = surveyQuestion.QuestionId;
                model.UserAnswers = listUserAnswerModel;
            }
            return model;
        }

        public bool Update(List<SurveyQuestionModel> model)
        {
            int surveyId = 0;
            if (model.GroupBy(x => x.SurveyId).Select(t => t.Key).ToList().Count == 1)
            {
                surveyId = model.GroupBy(x => x.SurveyId).Select(t => t.Key).FirstOrDefault();
            }
            List<SurveyQuestion> listSurveyQuestionDomains = _surveyQuestionRepository.GetAll().Where(x => x.SurveyId == surveyId).ToList();
            if (listSurveyQuestionDomains.Count == 0)
                throw new Exception("There are no question for this survey");
            else
            {
                var newQuestion = model.Where(t => !listSurveyQuestionDomains.Any(x => x.QuestionId == t.QuestionId)).ToList();
                if (newQuestion.Count > 0)
                {
                    foreach (var item in newQuestion)
                    {
                        var surveyQuestion = new SurveyQuestion
                        {
                            SurveyId = item.SurveyId,
                            QuestionId = item.QuestionId
                        };
                        _surveyQuestionRepository.Insert(surveyQuestion);
                    }
                }
                var removedQuestion = listSurveyQuestionDomains.Where(t => !model.Any(x => x.QuestionId == t.QuestionId)).ToList();
                if (removedQuestion.Count > 0)
                {
                    _surveyQuestionRepository.DeleteRange(removedQuestion);
                }
            }
            return true;
        }
    }
}
