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
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }
        [HttpGet]
        public ActionResult<List<AnswerModel>> GetAll()
        {
            return _answerService.GetAll();
        }
        [HttpGet("{id}")]
        public ActionResult<AnswerModel> GetById(int id)
        {
            return _answerService.GetById(id);
        }
        [HttpPost]
        public ActionResult<AnswerModel> Create(AnswerModel model)
        {
            return _answerService.Create(model);
        }
        [HttpPut]
        public ActionResult<AnswerModel> Update(AnswerModel model)
        {
            return _answerService.Update(model);
        }
        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(int id)
        {
            return _answerService.Delete(id);
        }
    }
}
