using EnvironmentSurvey.WebAPI.BusinessLogic;
using EnvironmentSurvey.WebAPI.ClientSide.Common;
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
    //[Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHostingEnvironment hostingEnvironment;

        public UserController(IUserService userService, IHostingEnvironment hostingEnv)
        {
            _userService = userService;
            hostingEnvironment = hostingEnv;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponsePagedModel> getAll(SearchModel model, [FromQuery] PaginationClientModel paginationClientModel)
        {
            var listUser = await _userService.GetAll(model, paginationClientModel);
            return listUser;
        }
        [HttpPost]
        [Route("search")]
        [Authorize(Roles = "ADMIN")]
        public async Task<List<UserModel>> Search(SearchModel model)
        {
            return await _userService.Search(model.Search_key);
        }
        [HttpGet("{id:int}")]
        [Authorize(Roles = "ADMIN,EMPLOYEE,STUDENT")]
        public async Task<IActionResult> getUserByID(int id)
        {
            var user = await _userService.GetUserByID(id);
            return Ok(user);
        }
        [HttpPost]
        [Route("searchByUsername")]
        [Authorize(Roles = "ADMIN,EMPLOYEE,STUDENT")]
        public async Task<IActionResult> getUserByUsername(SearchModel model)
        {
            var userModel = await _userService.GetUserByName(model.Username);
            if(userModel != null)
            {
                return Ok(userModel);
            }
            else
            {
                return BadRequest(new { error = "can not find user info" });
            }
            
        }
        [HttpPut]
        [Route("changeProfile")]
        [Authorize(Roles = "ADMIN,EMPLOYEE,STUDENT")]
        public async Task<IActionResult> update(UserModel model)
        {
            var response = await _userService.Update(model);
            return Ok(response);
            //var formCollection = await Request.ReadFormAsync();
            //int count = formCollection.Count();
            //var dict = formCollection.ToDictionary(x => x.Key, x => x.Value.ToString());
            //string imgPath = null;
            //if (count == 7)
            //{
            //    var file = formCollection.Files.First();
            //    if (file != null)
            //    {
            //        FileInfo fi = new FileInfo(file.FileName);
            //        var newfilename = "Image_" + DateTime.Now.TimeOfDay.Milliseconds + fi.Extension;
            //        var path = Path.Combine("", hostingEnvironment.WebRootPath + "\\Images\\" + newfilename);
            //        using (var stream = new FileStream(path, FileMode.Create))
            //        {
            //            file.CopyTo(stream);
            //            imgPath = newfilename;
            //        }
                   
            //    }
            //}
            
            //var response = await _userService.Update(dict, imgPath);
            //return Ok(response);
        }
        [HttpPut]
        [Route("changeAvatar")]
        [Authorize(Roles = "ADMIN,EMPLOYEE,STUDENT")]
        public async Task<IActionResult> changeAvatar(IFormCollection data)
        {
            var model = new ChangeAvatarModel
            {
                ID = int.Parse(data["ID"])
            };
            if (data.Files.Count() != 0)
            {
                model.File = data.Files.First();
            }
            var response = await _userService.changeAvatar(model);
            return Ok(response);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<string> delete(int id)
        {
            return await _userService.Delete(id);
        }
        [HttpPost]
        [Route("checkUserExists")]
        public async Task<IActionResult> checkUserExists(string username)
        {
            var data  = await _userService.checkUserExists(username);
            return Ok(data);
        }
        [HttpPost]
        [Route("checkEmailExists")]
        public async Task<IActionResult> checkEmailExists(string email)
        {
            var data = await _userService.checkEmailExists(email);
            return Ok(data);
        }
        [HttpPost]
        [Route("checkTelExists")]
        public async Task<IActionResult> checkTelExists(string tel)
        {
            var data = await _userService.checkTelExists(tel);
            return Ok(data);
        }
        [HttpPost]
        [Route("checkIdNumberExists")]
        public async Task<IActionResult> checkIdNumberExists(string idnum)
        {
            var data = await _userService.checkIdnumberExists(idnum);
            return Ok(data);
        }

        [HttpPost]
        [Route("verifyAccoount")]
        public async Task<IActionResult> verifyAccount(string username)
        {
            var data = await _userService.VerifyAccount(username);
            return Ok(data);
        }
    }
}
