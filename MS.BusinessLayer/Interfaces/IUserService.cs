using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Cookies;
using MS.BusinessLayer.DTO;
using MS.BusinessLayer.Infrastructure;

namespace MS.BusinessLayer.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDto userDto);

        Task<ClaimsIdentity>   Authenticate(UserDto userDto);

        ICookieAuthenticationProvider Provider { get; }
    }
}
