using EnvironmentSurvey.WebAPI.BusinessLogic;
using EnvironmentSurvey.WebAPI.ClientSide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeminarController : Controller
    {
        private readonly ISeminarService _seminarSevice;

        public SeminarController(ISeminarService seminarSevice)
        {
            _seminarSevice = seminarSevice;
        }

        [HttpPost]
        //[Authorize(Roles = "ADMIN,STUDENT,EMPLOYEE")]
        //[Authorize(Roles ="ADMIN")]
        public async Task<ResponsePagedModel> getAll(SearchModel model, [FromQuery] PaginationClientModel paginationClientModel)
        {
            var listUser = await _seminarSevice.GetAll(model, paginationClientModel);
            return listUser;
        }

        [HttpGet("{id:int}")]
        //[Authorize(Roles = "ADMIN,EMPLOYEE,STUDENT")]
        public async Task<SeminarModel> getSeminarByID(int id)
        {
            var user = await _seminarSevice.GetByID(id);
            return user;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<List<SeminarModel>> GetListSeminar()
        {
            var listSeminar = await _seminarSevice.GetListSeminar();
            return listSeminar;
        }

        [HttpGet("Manage/{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<SeminarModel> getSeminarByIDManage(int id)
        {
            var seminar = await _seminarSevice.GetByIDManage(id);
            return seminar;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(IFormCollection data)
        {
            var model = new SeminarModel
            {
                Name = data["Name"],
                Description = data["Description"],
                File = data.Files.First(),
                Location = data["Location"],
                Author = data["Author"],
                SubjectId = int.Parse( data["SubjectId"]),
                StartDate = data["StartTime"],
                EndDate = data["EndTime"],
                forUser = int.Parse(data["forUser"])
            };
            var response = await _seminarSevice.Create(model);
            if (response)
                return Ok("Success");
            return BadRequest("Error");
        }

        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(IFormCollection data)
        {
            var model = new SeminarModel
            {
                ID = int.Parse(data["Id"]),
                Name = data["Name"],
                Description = data["Description"],
                Location = data["Location"],
                Author = data["Author"],
                SubjectId = int.Parse(data["SubjectId"]),
                StartDate = data["StartTime"],
                EndDate = data["EndTime"],
                forUser = int.Parse(data["forUser"])
            };
            if(data.Files.Count() != 0)
            {
                model.File = data.Files.First();
            }
            var response = await _seminarSevice.Update(model);
            if (response)
                return Ok("Success");
            return BadRequest("Error");
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int Id)
        {
            var response = await _seminarSevice.Delete(Id);
            if (response)
                return Ok("Success");
            return BadRequest("Error");
        }
    }
}
