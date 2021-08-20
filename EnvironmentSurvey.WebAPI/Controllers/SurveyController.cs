using EnvironmentSurvey.WebAPI.BusinessLogic;
using EnvironmentSurvey.WebAPI.ClientSide.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EnvironmentSurvey.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        [HttpGet("/api/Seminar/{id:int}/[controller]")]
        public ActionResult<List<SurveyModel>> GetAllSurveyBySeminarId(int id)
        {
            return _surveyService.GetAllSurveyBySeminarId(id);
        }
        [HttpGet("{id:int}")]
        public ActionResult<SurveyModel> GetById(int id)
        {
            return _surveyService.GetById(id);
        }
        [HttpPost]
        public ActionResult<bool> Create(SurveyModel model)
        {
            SurveyModel modelCreated = _surveyService.Create(model);
            if (modelCreated != null) return true;
            return false;
        }
        [HttpPut]
        public ActionResult<bool> Update(SurveyModel model)
        {
            return _surveyService.Update(model);
        }
        [HttpDelete("{id:int}")]
        public ActionResult<bool> Delete(int id)
        {
            return _surveyService.Delete(id);
        }
    }
}
