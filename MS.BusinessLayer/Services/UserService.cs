using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using MS.BusinessLayer.DTO;
using MS.BusinessLayer.Infrastructure;
using MS.BusinessLayer.Interfaces;
using MS.DataLayer.Entities;
using MS.DataLayer.Identity;
using MS.DataLayer.Interfaces;

namespace MS.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork Database { get; }

        public UserService(IUnitOfWork uow) => Database = uow;

        public async Task<OperationDetails> Create(UserDto userDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);

            if (user == null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<UserDto, ApplicationUser>());
                user = Mapper.Map<UserDto, ApplicationUser>(userDto);

                var result = await Database.UserManager.CreateAsync(user, userDto.Password);

                if (result.Errors.Any())
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }

                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                
                Mapper.Initialize(cfg => cfg.CreateMap<UserDto, UserProfile>());
                UserProfile clientProfile = Mapper.Map<UserDto, UserProfile>(userDto);

                Database.Users.Create(clientProfile);
                await Database.SaveAsync();

                return new OperationDetails(true, "Success", "");
            }

            return new OperationDetails(false, "User with that email already exist", "Email");
        }

        public async Task<ClaimsIdentity> Authenticate(UserDto userDto)
        {
            ClaimsIdentity claim = null;

            // находим пользователя
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);

            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
            {
                claim = await Database.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }

            return claim;
        }

        public ICookieAuthenticationProvider Provider
        {
            get
            {
                return new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity =
                        SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                            validateInterval: TimeSpan.FromMinutes(30),
                            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                };
            }
        }

        public void Dispose() => Database.Dispose();
    }
}
