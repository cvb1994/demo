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
    public class SupportInfoController : ControllerBase
    {
        private readonly ISupportInfoService _supportInfoService;

        public SupportInfoController(ISupportInfoService supportInfoService)
        {
            _supportInfoService = supportInfoService;
        }

        [HttpGet]
        public async Task<SupportInfoModel> GetSupportInfomation()
        {
            var supportInfo = await _supportInfoService.GetSuportInfo();
            return supportInfo;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(SupportInfoModel model)
        {
            var response = await _supportInfoService.Create(model);
            if (response)
                return Ok("Success");
            return BadRequest("Error");
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(SupportInfoModel model)
        {
            var response = await _supportInfoService.Update(model);
            if (response)
                return Ok("Success");
            return BadRequest("Error");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int Id)
        {
            var response = await _supportInfoService.Delete(Id);
            if (response)
                return Ok("Success");
            return BadRequest("Error");
        }
    }
}
