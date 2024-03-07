using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.DTOs.User;
using ETicaret.Application.Exceptions;
using ETicaret.Application.Features.Commands.AppUser.CreateUser;
using ETicaret.Application.Helpers;
using ETicaret.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Persistence.Services
{
    public class UserService : IUserService
    {

        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser createUser)
        {
            
            var result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = createUser.UserName,
                Email = createUser.Email,
                NameSurname = createUser.NameSurname,
            }, createUser.Password);

            CreateUserResponse response = new()
            {
                Succeded = result.Succeeded
            };

            if (result.Succeeded)
                response.Message = "kullanıcı başarıyla kaydedilmiştir";
            else
                foreach (var item in result.Errors)
                    response.Message += $"{item.Code} --> {item.Description} \n\r";
            return response;
        }

        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPasssword)
        {

            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                resetToken = resetToken.UrlDecode();
                IdentityResult result = await _userManager.ResetPasswordAsync(user,resetToken,newPasssword);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                }
                else
                {
                    throw new PasswordChangeFailedException();
                }
            }

        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
                return;
            }
            throw new NotFoundUserException();
        }
    }
}
