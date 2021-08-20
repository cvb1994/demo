using EnvironmentSurvey.WebAPI.ClientSide.Common;
using EnvironmentSurvey.WebAPI.ClientSide.Models;
using EnvironmentSurvey.WebAPI.DataAccess;
using EnvironmentSurvey.WebAPI.DataAccess.Domains;
using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.BusinessLogic
{
    public interface IResultService
    {
        Task<ResponsePagedModel> showResultUser(PaginationClientModel paginationClientModel, int userId);
        ResultModel SaveResult(SaveUserAnswerModel model);
        Task<ResponsePagedModel> showResultBySurveyId(PaginationClientModel paginationClientModel,int surveyId);
        Task<List<InforTakeSurveyModel>> TakeInfor(int seminarId);
    }
    public class ResultService : IResultService
    {
        private readonly ESContext _context;

        private readonly IRepository<Answer> _answerRespository;
        private readonly IRepository<SurveyQuestion> _surveyQuestionRepository;
        private readonly IRepository<Result> _resultRepository;
        private readonly IRepository<User> _userRespository;
        private readonly IRepository<Survey> _surveyRespository;
        public ResultService(ESContext context,
            IRepository<Answer> answerRespository,
            IRepository<SurveyQuestion> surveyQuestionRepository,
            IRepository<Result> resultRepository,
            IRepository<User> userRespository,
            IRepository<Survey> surveyRespository)
        {
            _context = context;
            _answerRespository = answerRespository;
            _surveyQuestionRepository = surveyQuestionRepository;
            _resultRepository = resultRepository;
            _userRespository = userRespository;
            _surveyRespository = surveyRespository;
        }

        public async Task<ResponsePagedModel> showResultUser(PaginationClientModel paginationClientModel, int userId)
        {
            var lsUserResultTotal = await _context.Results.Where(r => r.UserId == userId).ToListAsync();
            int totalPage = (int)Math.Ceiling(lsUserResultTotal.Count() / (double)paginationClientModel.PageSize);
            var lsUserResult = await _context.Results.Where(r => r.UserId == userId)
                                .Skip((paginationClientModel.PageNumber - 1) * paginationClientModel.PageSize)
                                .Take(paginationClientModel.PageSize)
                                .ToListAsync();
            List<ResultModel> lsUserResultModel = new List<ResultModel>();
            if(lsUserResult.Count() > 0)
            {
                foreach (var userResult in lsUserResult)
                {
                    var listResult = await _context.Results.Where(r => r.SurveyId == userResult.SurveyId).OrderByDescending(r => r.Point).ThenBy(r => r.SubmitTime).ThenBy(r => r.CreatedDate).ToListAsync();
                    for (int i = 0; i < listResult.Count; i++)
                    {
                        var r = listResult[i];
                        if (userResult.UserId == r.UserId)
                        {
                            ResultModel result = new ResultModel
                            {
                                Id = r.Id,
                                surveyName = _surveyRespository.Get(r.SurveyId).Name,
                                point = r.Point,
                                SubmitTime = r.SubmitTime,
                                //FullName = _userRespository.Get(r.UserId).FirstName + " " + _userRespository.Get(r.UserId).LastName,
                                Ranked = i + 1,
                                //NameSeminar = _surveyRespository.Get(r.SurveyId).Serminar.Name
                            };
                            lsUserResultModel.Add(result);
                        }

                    }
                }
            }
            var responsePagedModel = new ResponsePagedModel
            {
                listResult = lsUserResultModel.ToList(),
                PageNumber = paginationClientModel.PageNumber,
                TotalPage = totalPage
            };
            return responsePagedModel;
        }

        public ResultModel SaveResult(SaveUserAnswerModel model)
        {
            if (model.ListUserAnserModel.Count == 0)
                throw new Exception("Invalid input list model");
            //get all user anwers
            var listAnswer = _answerRespository.GetAll().ToList();
            var listAnswerModel = new List<AnswerModel>();
            foreach (var item in model.ListUserAnserModel)
            {
                var AnswerDomain = listAnswer.Where(x => x.Id == item.AnswerId).FirstOrDefault();
                if (AnswerDomain != null)
                {
                    AnswerModel answerModel = new()
                    {
                        Id = AnswerDomain.Id,
                        Answer = AnswerDomain.Answer1,
                        IsCorrect = (IsCorrect)AnswerDomain.IsCorrect,
                        QuestionId = AnswerDomain.QuestionId,
                    };
                    listAnswerModel.Add(answerModel);
                }
            }
            //count point
            int point = 0;
            if (listAnswerModel.Count > 0)
            {
                foreach (var item in listAnswerModel)
                {
                    if (item.IsCorrect == (IsCorrect)1)
                    {
                        point++;
                    };
                }
            }
            //Get userId
            int userId = 0;
            var countUser = model.ListUserAnserModel.GroupBy(x => x.UserId).Select(t => t.Key).ToList();
            if (countUser.Count == 1)
            {
                userId = countUser.FirstOrDefault();
            }
            //Get SurveyId
            int surveyId = 0;
            var a = model.ListUserAnserModel.FirstOrDefault();
            var surveyQuestionModel = _surveyQuestionRepository.Get(a.Id);
            if (surveyQuestionModel != null)
            {
                surveyId = surveyQuestionModel.SurveyId;
            }
            //Save result
            Result res = new()
            {
                Point = point,
                SubmitTime = model.SubmitTime,
                UserId = userId,
                User = _userRespository.Get(userId),
                SurveyId = surveyId,
                Survey = _surveyRespository.Get(surveyId)
            };
            _resultRepository.Insert(res);

            var result = new ResultModel()
            {
                Id = res.Id,
                point = res.Point,
                SubmitTime = res.SubmitTime,
                UserId = res.UserId,
                surveyId = res.SurveyId,
            };
            return result;
        }

        public async Task<ResponsePagedModel> showResultBySurveyId(PaginationClientModel paginationClientModel, int surveyId)
        {
            var listResultTotal = await _context.Results.Where(r => r.SurveyId == surveyId).OrderByDescending(r => r.Point).ThenBy(r => r.SubmitTime).ThenBy(r => r.CreatedDate).ToListAsync();
            int totalPage = (int)Math.Ceiling(listResultTotal.Count() / (double)paginationClientModel.PageSize);

            var listResult = await _context.Results
                        .Where(r => r.SurveyId == surveyId)
                        .OrderByDescending(r => r.Point)
                        .ThenBy(r => r.SubmitTime).ThenBy(r => r.CreatedDate)
                        .Skip((paginationClientModel.PageNumber - 1) * paginationClientModel.PageSize)
                        .Take(paginationClientModel.PageSize)
                        .ToListAsync();
            List<ResultModel> lsresultModel = new List<ResultModel>();

            if (listResult.Count() > 0)
            {
                for (int i = 0; i < listResult.Count; i++)
                {
                    var r = listResult[i];
                    ResultModel result = new ResultModel
                    {
                        Id = r.Id,
                        surveyName = _surveyRespository.Get(r.SurveyId).Name,
                        point = r.Point,
                        SubmitTime = r.SubmitTime,
                        FullName = _userRespository.Get(r.UserId).FirstName + " " + _userRespository.Get(r.UserId).LastName,
                        Ranked = i + 1
                    };
                    lsresultModel.Add(result);
                }
            }
            var responsePagedModel = new ResponsePagedModel
            {
                listResult = lsresultModel.ToList(),
                PageNumber = paginationClientModel.PageNumber,
                TotalPage = totalPage
            };
            return responsePagedModel;
        }

        public async Task<List<InforTakeSurveyModel>> TakeInfor(int seminarId)
        {
            var requestSeminars = await _context.UserSeminars.Where(s=> s.SeminarId == seminarId).Where(s=>s.Status ==(int)Status.ACCEPTED).ToListAsync();
            var totalRquestSeminars = requestSeminars.Count();
            var listSurvey = await _context.Surveys.Where(s => s.SerminarId == seminarId).ToListAsync();
            List<InforTakeSurveyModel> listResult = new List<InforTakeSurveyModel>();
            if(listSurvey.Count() > 0)
            {
                foreach(var survey in listSurvey)
                {
                    InforTakeSurveyModel model = new InforTakeSurveyModel
                    {
                        SurveyId = survey.Id,
                        TotalRegister = totalRquestSeminars,
                        SeminarName = _context.Seminars.Where(s=> s.Id == seminarId).FirstOrDefault().Name,
                        SurveyName = survey.Name,
                        TotalTakeSurvey = _resultRepository.GetAll().Where(r=> r.SurveyId == survey.Id).ToList().Count()
                    };
                    listResult.Add(model);
                }
            }
            return listResult;
        }
    }
}
