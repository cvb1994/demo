using EnvironmentSurvey.WebAPI.BusinessLogic;
using EnvironmentSurvey.WebAPI.ClientSide.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EnvironmentSurvey.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        [HttpGet]
        public ActionResult<List<QuestionModel>> GetAll()
        {
            return _questionService.GetAll();
        }
        [HttpGet("{id}")]
        public ActionResult<QuestionModel> GetById(int id)
        {
            return _questionService.GetById(id);
        }

        [HttpGet("/api/Admin/Question/{id:int}")]
        public ActionResult<QuestionModel> GetByIdForAdmin(int id)
        {
            return _questionService.GetByIdForAdmin(id);
        }

        [HttpGet("/api/Survey/{id:int}/[controller]")]
        public ActionResult<List<QuestionModel>> GetAllQuestionBySurveyId(int id)
        {
            return _questionService.GetAllQuestionBySurveyId(id);
        }
        [HttpPost]
        public ActionResult<bool> Create(QuestionModel model)
        {
            return _questionService.Create(model);
        }
        [HttpPut]
        public ActionResult<bool> Update(QuestionModel model)
        {
            return _questionService.Update(model);
        }
        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(int id)
        {
            return _questionService.Delete(id);
        }

        [HttpGet("/api/Subject/{id:int}/[controller]")]
        public ActionResult<int> GetNumQuestionBySubject(int id)
        {
            return _questionService.GetNumsQuestionBySubject(id);
        }
    }
}
