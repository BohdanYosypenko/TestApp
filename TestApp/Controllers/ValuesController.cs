using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Controllers
{
    public class ValuesController : Controller
    {
        /// <summary>
        /// show what user is Logged in 
        /// </summary>
        /// <returns>retuns Username of logged user</returns>
        [Authorize]
        [Route("getlogin")]
        [HttpGet]
        public IActionResult GetLogin()
        {
            return Ok($"Your Username is {User.Identity.Name}");
        }
    }
}
