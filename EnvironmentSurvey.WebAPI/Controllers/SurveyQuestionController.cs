using EnvironmentSurvey.WebAPI.BusinessLogic;
using EnvironmentSurvey.WebAPI.ClientSide.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EnvironmentSurvey.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyQuestionController : ControllerBase
    {
        private readonly ISurveyQuestionService _surveyQuestionService;
        public SurveyQuestionController(ISurveyQuestionService surveyQuestionService)
        {
            _surveyQuestionService = surveyQuestionService;
        }
        [HttpGet]
        public ActionResult<List<SurveyQuestionModel>> GetAll()
        {
            return _surveyQuestionService.GetAll();
        }
        [HttpPost]
        public ActionResult<bool> Create(List<SurveyQuestionModel> model)
        {
            return _surveyQuestionService.Create(model);
        }
        [HttpPut]
        public ActionResult<bool> Update(List<SurveyQuestionModel> model)
        {
            return _surveyQuestionService.Update(model);
        }
    }
}
