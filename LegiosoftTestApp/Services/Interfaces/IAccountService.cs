using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LegiosoftTestApp.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<bool> RegisterAsync(IdentityUser user);
        public Task<bool> LoginAsync(IdentityUser user, bool rememberMe);
        public Task LogoutAsync();
    }
}
