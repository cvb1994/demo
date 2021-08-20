using EnvironmentSurvey.WebAPI.ClientSide.Common;
using EnvironmentSurvey.WebAPI.ClientSide.Models;
using EnvironmentSurvey.WebAPI.DataAccess;
using EnvironmentSurvey.WebAPI.DataAccess.Domains;
using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.BusinessLogic
{
    public interface ISurveyService
    {
        List<SurveyModel> GetAllSurveyBySeminarId(int seminarId);
        SurveyModel Create(SurveyModel model);
        bool Update(SurveyModel model);
        SurveyModel GetById(int Id);
        bool Delete(int Id);
    }
    public class SurveyService : ISurveyService
    {
        private readonly IRepository<Survey> _surveyRespository;
        private readonly IRepository<Result> _resultRespository;
        private readonly IRepository<Seminar> _seminartRespository;
        private readonly ESContext _context;
        public SurveyService(IRepository<Survey> surveyRespository, IRepository<Result> resultRespository, 
            IRepository<Seminar> seminartRespository, ESContext context)
        {
            _surveyRespository = surveyRespository;
            _resultRespository = resultRespository;
            _seminartRespository = seminartRespository;
            _context = context;
        }

        public SurveyModel Create(SurveyModel model)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var survey = new Survey
                {
                    Name = model.Name,
                    StartDate = Convert.ToDateTime(model.StartDate),
                    EndTime = Convert.ToDateTime(model.EndDate),
                    Status = (int)model.Status,
                    SerminarId = model.SeminarId,
                    Serminar = _seminartRespository.Get(model.SeminarId),
                    Description = model.Description,
                };
                _surveyRespository.Insert(survey);
                model.Id = survey.Id;

                var seminar = _seminartRespository.Get(model.SeminarId);
                int maxQuestion = _context.Questions.Where(q => q.SubjectId == seminar.SubjectId).ToList().Count();

                if (maxQuestion < model.NumberOfQuestion) throw new Exception("Invalid input param");
                var listRandomQuestion = _context.Questions.OrderBy(q => Guid.NewGuid()).Take(model.NumberOfQuestion);
                foreach (Question question in listRandomQuestion)
                {
                    SurveyQuestion obj = new SurveyQuestion();
                    obj.SurveyId = survey.Id;
                    obj.QuestionId = question.Id;
                    _context.Add(obj);
                }
                _context.SaveChanges();
                transaction.Commit();
                return model;
            } catch (Exception e)
            {
                transaction.Rollback();
            }
            return null;
            
        }

        public bool Update(SurveyModel model)
        {
            Survey survey = _surveyRespository.Get(model.Id);
            if (survey == null)
                throw new Exception("Survey not found");
            else
            {
                survey.Name = model.Name;
                survey.StartDate = Convert.ToDateTime(model.StartDate);
                survey.EndTime = Convert.ToDateTime(model.EndDate);
                survey.Status = (int)model.Status;
                survey.SerminarId = model.SeminarId;
                survey.Description = model.Description;
                _surveyRespository.Update(survey);
            }
            return true;
        }

        public SurveyModel GetById(int Id)
        {
            var listResult = _resultRespository.GetAll().ToList();
            List<ResultModel> listResultModel = new();
            if (listResult.Count > 0)
            {
                listResultModel = listResult.Select(x => new ResultModel
                {
                    Id = x.Id,
                    surveyId = x.SurveyId,
                    point = x.Point,
                    SubmitTime = x.SubmitTime,
                    UserId = x.UserId
                }).ToList();
            }
            SurveyModel surveyModel = new();
            Survey survey = _surveyRespository.Get(Id);
            if (survey == null)
                throw new Exception("Survey not found");
            else
            {
                surveyModel.Id = survey.Id;
                surveyModel.Name = survey.Name;
                surveyModel.StartDate = survey.StartDate.ToString();
                surveyModel.EndDate = survey.EndTime.ToString();
                surveyModel.Status = (SurveyStatus)survey.Status;
                surveyModel.SeminarId = survey.SerminarId;
                surveyModel.Description = survey.Description;
                surveyModel.Results = listResultModel.Count > 0 ? listResultModel.Where(x => x.surveyId == survey.Id).ToList() : null;
            };
            return surveyModel;
        }

        public bool Delete(int Id)
        {
            Survey survey = _surveyRespository.Get(Id);
            if (survey == null)
                throw new Exception("Survey not found");
            else
            {
                var listResult = _resultRespository.GetAll().Where(x => x.SurveyId == Id).ToList();
                _resultRespository.DeleteRange(listResult);
                _surveyRespository.Delete(survey);
                return true;
            }
        }

        public List<SurveyModel> GetAllSurveyBySeminarId(int seminarId)
        {
            var listResult = _resultRespository.GetAll().ToList();
            List<ResultModel> listResultModel = new();
            if (listResult.Count > 0)
            {
                listResultModel = listResult.Select(x => new ResultModel
                {
                    Id = x.Id,
                    surveyId = x.SurveyId,
                    point = x.Point,
                    SubmitTime = x.SubmitTime,
                    UserId = x.UserId
                }).ToList();
            }
            var listSurvey = _surveyRespository.GetAll().Where(x => x.SerminarId == seminarId).ToList();
            List<SurveyModel> result = new List<SurveyModel>();
            if (listSurvey.Count > 0) 
            {
                result = listSurvey.Select(x => new SurveyModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartDate = x.StartDate.ToString(),
                    EndDate = x.EndTime.ToString(),
                    Status = (SurveyStatus)x.Status,
                    SeminarId = x.SerminarId,
                    Description = x.Description,
                    Results = listResultModel.Count > 0 ? listResultModel.Where(y => y.surveyId == x.Id).ToList() : null,
                }).ToList();
            }
                
            
            return result;
        }
    }
}

