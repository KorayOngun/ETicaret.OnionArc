using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.Abstractions.Token;
using ETicaret.Application.DTOs;
using ETicaret.Application.Exceptions;
using ETicaret.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserService _userService;
        public AuthService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration, SignInManager<AppUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _signInManager = signInManager;
            _userService = userService;
        }

        private async Task<Token> CreateUserExternalAsync(AppUser? user,string email,string name,UserLoginInfo userLoginInfo,int accessTokenLifeTime)
        {

            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        NameSurname = name,
                    };
                }
                var identityResult = await _userManager.CreateAsync(user);
                result = identityResult.Succeeded;
            }
            if (result)
            {
                await _userManager.AddLoginAsync(user, userLoginInfo);
                
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime,user);

                await _userService.UpdateRefreshToken(token.RefreshToken,user,token.Expiration,5);

                return token;
            }
            throw new Exception("Invalid External Authenticatin");

        }


        public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>
                {
                    _configuration["ExternalLoginSettings:Google:Client_ID"]
                }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");

            //AppUser
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            return await CreateUserExternalAsync(user, payload.Email, payload.Name, info,accessTokenLifeTime);
        }

        public async Task<Application.DTOs.Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
        {
            var appUser = await _userManager.FindByNameAsync(usernameOrEmail) ??
                          await _userManager.FindByEmailAsync(usernameOrEmail) ??
                          throw new NotFoundUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(appUser, password, false);

            if (result.Succeeded)
            {
                var token = _tokenHandler.CreateAccessToken(accessTokenLifeTime,appUser);
                await _userService.UpdateRefreshToken(token.RefreshToken, appUser, token.Expiration, 300);
                return token;
            }

            throw new AuthenticationErrorException();
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenExpiryDate > DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateAccessToken(20,user);
                await _userService.UpdateRefreshToken(token.RefreshToken,user,token.Expiration,300);
                return token;
            }
            throw new NotFoundUserException();
        }
    }
}
