using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Services.Interface;
using TestApp.ViewModels;

namespace TestApp.Services.Realization
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            IdentityUser user = new IdentityUser { PhoneNumber = model.PhoneNumber, UserName = model.Username };            
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {                
                await _signInManager.SignInAsync(user, false);
                return true;
            }

            return false;
        }

        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {                
                return true;
            }
            return false;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
