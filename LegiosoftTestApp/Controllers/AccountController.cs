using LegiosoftTestApp.Services.Interfaces;
using LegiosoftTestApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LegiosoftTestApp.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return Ok(new RegisterViewModel() { UserName = "vasya", Password = "12345Zz@" });
        }

        [HttpPost]
        [Route("register")]
        public async Task<string> RegisterAsync([FromBody] RegisterViewModel user)
        {
            IdentityUser newUser = new IdentityUser() { UserName = user.UserName, PasswordHash = user.Password };
            bool result = await _accountService.RegisterAsync(newUser);
            if (result)
                return "yes";
            else
                return "no";
        }

        [HttpPost]
        [Route("login")]
        public async Task<string> Login([FromBody] LoginViewModel user)
        {
            IdentityUser loginUser = new IdentityUser() { UserName = user.UserName, PasswordHash = user.Password };
            bool result = await _accountService.LoginAsync(loginUser,user.RememberMe);
            if (result)
                return "yes";
            else
                return "no";
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {            
            await _accountService.LogoutAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet]        
        [Route("info")]
        public IActionResult Info()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
