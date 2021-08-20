using EnvironmentSurvey.WebAPI.ClientSide.Common;
using EnvironmentSurvey.WebAPI.ClientSide.Models;
using EnvironmentSurvey.WebAPI.DataAccess;
using EnvironmentSurvey.WebAPI.DataAccess.Domains;
using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnvironmentSurvey.WebAPI.BusinessLogic
{
    public interface IQuestionService
    {
        bool Create(QuestionModel model);
        List<QuestionModel> GetAll();
        List<QuestionModel> GetAllQuestionBySurveyId(int surveyId);
        QuestionModel GetById(int Id);
        QuestionModel GetByIdForAdmin(int Id);
        bool Update(QuestionModel model);
        bool Delete(int Id);
        int GetNumsQuestionBySubject(int id);

        List<int> getRandomQuestion(int num);
    }
    public class QuestionService : IQuestionService
    {
        private readonly ESContext _context;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<SurveyQuestion> _surveyQuestionRepository;
        public QuestionService(
            IRepository<Question> questionRepository,
            IRepository<Answer> answerRepository,
            IRepository<SurveyQuestion> surveyQuestionRepository, ESContext context)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _surveyQuestionRepository = surveyQuestionRepository;
            _context = context;
        }

        public bool Create(QuestionModel model)
        {
            if (model.Answers.Count < 4)
            {
                throw new Exception("Not enough answers for this question");
            }
            else
            {
                var question = new Question
                {
                    Question1 = model.Question,
                    SubjectId = model.SubjectId
                };
                _questionRepository.Insert(question);
                model.Id = question.Id;
                foreach (var item in model.Answers)
                {
                    Answer answer = new()
                    {
                        Answer1 = item.Answer,
                        IsCorrect = (int)item.IsCorrect,
                        QuestionId = model.Id,
                    };
                    _answerRepository.Insert(answer);
                }
            }
            return true;
        }

        public bool Delete(int Id)
        {
            Question question = _questionRepository.Get(Id);
            if (question == null)
                throw new Exception("Question not found");
            else
            {
                _questionRepository.Delete(question);
                var listAnswer = _answerRepository.GetAll().Where(x => x.QuestionId == Id).ToList();
                _answerRepository.DeleteRange(listAnswer);
            }
            return true;
        }

        public List<QuestionModel> GetAll()
        {
            var listAnswer = _answerRepository.GetAll().ToList();
            List<AnswerModel> listAnswerModel = new();
            if (listAnswer.Count > 0)
            {
                listAnswerModel = listAnswer.Select(x => new AnswerModel
                {
                    Id = x.Id,
                    Answer = x.Answer1,
                    IsCorrect = null,
                    QuestionId = x.QuestionId
                }).ToList();
            }

            var listSurveyQuestion = _surveyQuestionRepository.GetAll().ToList();
            List<SurveyQuestionModel> listSurveyQuestionModel = new();
            if (listSurveyQuestion.Count > 0)
            {
                listSurveyQuestionModel = listSurveyQuestion.Select(x => new SurveyQuestionModel
                {
                    Id = x.Id,
                    SurveyId = x.SurveyId,
                    QuestionId = x.QuestionId,
                }).ToList();
            }

            var listQuestion = _questionRepository.GetAll().ToList();
            if (listQuestion.Count == 0)
                throw new Exception("There is no question existed");

            var result = listQuestion.Select(x => new QuestionModel
            {
                Id = x.Id,
                Question = x.Question1,
                Answers = listAnswerModel.Count > 0 ? listAnswerModel.Where(y => y.QuestionId == x.Id).ToList() : null,
                SurveyQuestions = listSurveyQuestionModel.Count > 0 ? listSurveyQuestionModel.Where(y => y.QuestionId == x.Id).ToList() : null,
            }).ToList();
            return result;
        }

        public List<QuestionModel> GetAllQuestionBySurveyId(int Id)
        {
            var listAnswer = _answerRepository.GetAll().ToList();
            List<AnswerModel> listAnswerModel = new();
            if (listAnswer.Count > 0)
            {
                listAnswerModel = listAnswer.Select(x => new AnswerModel
                {
                    Id = x.Id,
                    Answer = x.Answer1,
                    IsCorrect = null,
                    QuestionId = x.QuestionId
                }).ToList();
            }
            var listQuestion = _questionRepository.GetAll().ToList();

            var listSurveyQuestion = _surveyQuestionRepository.GetAll().Where(x => x.SurveyId == Id).ToList();
            if (listSurveyQuestion.Count == 0)
                throw new Exception("There is no Question of this Survey existed");
            var result = listQuestion.Where(x => listSurveyQuestion.Select(y => y.QuestionId).Contains(x.Id)).ToList();

            List<QuestionModel> listQuestionModel = new();
            if (result.Count > 0)
            {
                listQuestionModel = result.Select(x => new QuestionModel
                {
                    Id = x.Id,
                    Question = x.Question1,
                    Answers = listAnswerModel.Count > 0 ? listAnswerModel.Where(y => y.QuestionId == x.Id).ToList() : null,
                    SurveyQuestionId = x.SurveyQuestions.GroupBy(x => x.Id).Select(t => t.Key).ToList().Count == 1
                        ? x.SurveyQuestions.GroupBy(x => x.Id).Select(t => t.Key).ToList().FirstOrDefault() : null,
                }).ToList();
            }
            return listQuestionModel;
        }

        public QuestionModel GetById(int Id)
        {
            var listAnswer = _answerRepository.GetAll().Where(x => x.QuestionId == Id).ToList();
            List<AnswerModel> listAnswerModel = new();
            if (listAnswer.Count > 0)
            {
                listAnswerModel = listAnswer.Select(x => new AnswerModel
                {
                    Id = x.Id,
                    Answer = x.Answer1,
                    IsCorrect = null,
                    QuestionId = x.QuestionId
                }).ToList();
            }

            QuestionModel model = new();
            Question question = _questionRepository.Get(Id);
            if (question == null)
                throw new Exception("Question not found");
            else
            {
                model.Id = question.Id;
                model.Question = question.Question1;
                model.Answers = listAnswerModel;
            }
            return model;
        }

        public QuestionModel GetByIdForAdmin(int Id)
        {
            var listAnswer = _answerRepository.GetAll().Where(x => x.QuestionId == Id).ToList();
            List<AnswerModel> listAnswerModel = new();
            if (listAnswer.Count > 0)
            {
                listAnswerModel = listAnswer.Select(x => new AnswerModel
                {
                    Id = x.Id,
                    Answer = x.Answer1,
                    IsCorrect = (IsCorrect?)x.IsCorrect,
                    QuestionId = x.QuestionId
                }).ToList();
            }

            QuestionModel model = new();
            Question question = _questionRepository.Get(Id);
            if (question == null)
                throw new Exception("Question not found");
            else
            {
                model.Id = question.Id;
                model.Question = question.Question1;
                model.SubjectId = question.SubjectId;
                model.Answers = listAnswerModel;
            }
            return model;
        }

        public int GetNumsQuestionBySubject(int id)
        {
            var list = _context.Questions.Where(q => q.SubjectId == id).ToList();
            return list.Count();
        }

        public List<int> getRandomQuestion(int num)
        {
            var list = _context.Questions.OrderBy(q => Guid.NewGuid()).Take(num);
            List<int> listQuestionId = new List<int>();
            foreach(Question question in list)
            {
                listQuestionId.Add(question.Id);
            }
            return listQuestionId;
        }

        public bool Update(QuestionModel model)
        {
            Question question = _questionRepository.Get(model.Id);
            if (question == null)
                throw new Exception("Question not found");
            else
            {
                question.Question1 = model.Question;
                question.SubjectId = model.SubjectId;
                _questionRepository.Update(question);
                var listAnswer = _answerRepository.GetAll().Where(x => x.QuestionId == model.Id).ToList();
                foreach (var item in model.Answers)
                {
                    var answer = listAnswer.Where(x => x.Id == item.Id).FirstOrDefault(); ;
                    if (answer == null)
                        throw new Exception("Answer not found");
                    else
                    {
                        answer.Answer1 = item.Answer;
                        answer.IsCorrect = (int)item.IsCorrect;
                        _answerRepository.Update(answer);
                    };
                }
            }
            return true;
        }
    }
}
