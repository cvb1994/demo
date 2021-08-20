using EnvironmentSurvey.WebAPI.ClientSide.Models;
using EnvironmentSurvey.WebAPI.DataAccess.Domains;
using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.BusinessLogic
{
    public interface IUserAnswerService
    {
        List<UserAnswerModel> GetAll();
        List<UserAnswerModel> GetByUserSurvey(int userId, int surveyId);
        List<UserAnswerModel> Create(List<UserAnswerModel> model);
        bool Delete(int Id);
    }
    public class UserAnswerService : IUserAnswerService
    {
        private readonly IRepository<UserAnswer> _userAnswerRepository;
        private readonly IRepository<SurveyQuestion> _surveyQuestionRepository;
        public UserAnswerService(IRepository<UserAnswer> userAnswerRepository, IRepository<SurveyQuestion> surveyQuestionRepository)
        {
            _userAnswerRepository = userAnswerRepository;
            _surveyQuestionRepository = surveyQuestionRepository;
        }
        public List<UserAnswerModel> Create(List<UserAnswerModel> model)
        {
            foreach (var item in model)
            {
                var userAnswer = new UserAnswer
                {
                    SurveyQuestionId = item.SurveyQuestionId,
                    AnswerId = item.AnswerId,
                    UserId = item.UserId
                };
                _userAnswerRepository.Insert(userAnswer);
                item.Id = userAnswer.Id;
            }
            return model;
        }
        public bool Delete(int Id)
        {
            UserAnswer userAnswer = _userAnswerRepository.GetAll().Where(x => x.Id == Id).FirstOrDefault();
            if (userAnswer == null)
                throw new Exception("UserAnswer not found");
            else
            {
                _userAnswerRepository.Delete(userAnswer);
                return true;
            }
        }
        public List<UserAnswerModel> GetAll()
        {
            var listUserAnswer = _userAnswerRepository.GetAll().ToList();
            if (listUserAnswer.Count == 0)
                throw new Exception("There is no UserAnswer existed");
            var result = listUserAnswer.Select(x => new UserAnswerModel
            {
                Id = x.Id,
                SurveyQuestionId = x.SurveyQuestionId,
                AnswerId = x.AnswerId,
                UserId = x.UserId,
            }).ToList();
            return result;
        }
        public List<UserAnswerModel> GetByUserSurvey(int userId, int surveyId)
        {
            var listSurveyQuestion = _surveyQuestionRepository.GetAll().Where(x => x.SurveyId == surveyId).ToList(); ;
            var listUserAnswer = _userAnswerRepository.GetAll().Where(x => x.UserId == userId).ToList();
            var listUserSurvey = listUserAnswer.Join(listSurveyQuestion, userAnswer => userAnswer.SurveyQuestionId, surveyQuestion => surveyQuestion.Id,
                (userAnswer, surveyQuestion) => new { userAnswer, surveyQuestion }).ToList();
            if (listUserSurvey.Count == 0)
                throw new Exception("There is no UserAnswer existed");
            var result = listUserSurvey.Select(x => new UserAnswerModel
            {
                Id = x.userAnswer.Id,
                SurveyQuestionId = x.userAnswer.SurveyQuestionId,
                AnswerId = x.userAnswer.AnswerId,
                UserId = x.userAnswer.UserId,
            }).ToList();
            return result;
        }
    }
}
