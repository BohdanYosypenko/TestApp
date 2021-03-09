using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestApp.Models;
using TestApp.Services.Interface;
using TestApp.ViewModels;

namespace TestApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAccountService _accountService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
        }

        /// <summary>
        /// Add new user to database
        /// </summary>
        /// <param name="model">new user</param>
        /// <returns>returns status of operation</returns>
        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var result = await _accountService.RegisterAsync(model);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Login in app
        /// </summary>
        /// <param name="model">required username and password</param>
        /// <returns>status of operation</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var result = await _accountService.LoginAsync(model);
            if (result)
                return Ok();
            else
                return BadRequest();

        }
        /// <summary>
        /// logout from app
        /// </summary>
        /// <returns></returns>
        [HttpPost("/logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return Ok();
        }
    }
}

