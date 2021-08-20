using EnvironmentSurvey.WebAPI.BusinessLogic;
using EnvironmentSurvey.WebAPI.ClientSide.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class FAQsController : ControllerBase
    {
        private readonly IFAQsService _faqsSevice;

        public FAQsController(IFAQsService faqsSevice)
        {
            _faqsSevice = faqsSevice;
        }

        [HttpGet]
        public async Task<List<FAQsModel>> GetAll()
        {
            var listFAQs = await _faqsSevice.GetAll();
            return listFAQs;
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<FAQsModel> getFAQByID(int id)
        {
            var faq = await _faqsSevice.GetByID(id);
            return faq;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(FAQsModel model)
        {
            var response = await _faqsSevice.Create(model);
            if (response)
                return Ok("Success");
            return BadRequest("Error");
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(FAQsModel model)
        {
            var response = await _faqsSevice.Update(model);
            if (response)
                return Ok("Success");
            return BadRequest("Error");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int Id)
        {
            var response = await _faqsSevice.Delete(Id);
            if (response)
                return Ok("Success");
            return BadRequest("Error");
        }
    }
}
