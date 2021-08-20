using EnvironmentSurvey.WebAPI.BusinessLogic;
using EnvironmentSurvey.WebAPI.ClientSide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController:Controller
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<List<SubjectModel>> GetListSeminar()
        {
            var listSubject =  _subjectService.getAll();
            return listSubject;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<Boolean> Create(SubjectModel model)
        {

            var subject = _subjectService.Create(model);
            if(subject != null)
            {
                return true;
            }
            return false;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<Boolean> Update(SubjectModel model)
        {
            try
            {
                var subject = _subjectService.Update(model);
                if (subject != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<Boolean> Delete(int id)
        {

            try
            {
                return _subjectService.Delete(id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpPost]
        [Route("search")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<Boolean> Search(SubjectModel model)
        {

            return _subjectService.Search(model);
        }



    }
}
