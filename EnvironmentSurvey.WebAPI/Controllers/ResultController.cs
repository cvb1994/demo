using EnvironmentSurvey.WebAPI.BusinessLogic;
using EnvironmentSurvey.WebAPI.ClientSide.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;
        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpGet]
        [Route("getUserResult")]
        public async Task<ResponsePagedModel> getResult([FromQuery] PaginationClientModel paginationClientModel,int userId)
        {
            return await _resultService.showResultUser(paginationClientModel , userId);
        }

        [HttpGet("getResultBySurveyId")]
        public async Task<ResponsePagedModel> getResultBySurveyId([FromQuery] PaginationClientModel paginationClientModel, int surveyId)
        {
            return await _resultService.showResultBySurveyId(paginationClientModel, surveyId);
        }

        [HttpGet("getInfor")]
        public async Task<List<InforTakeSurveyModel>> getInfor( int seminarId)
        {
            return await _resultService.TakeInfor(seminarId);
        }
    }
}
