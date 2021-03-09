using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.ViewModels;

namespace TestApp.Services.Interface
{
    public interface IAccountService
    {
        public Task<bool> RegisterAsync(RegisterViewModel model);
        public Task<bool> LoginAsync(LoginViewModel model);
        public Task LogoutAsync();
    }
}
