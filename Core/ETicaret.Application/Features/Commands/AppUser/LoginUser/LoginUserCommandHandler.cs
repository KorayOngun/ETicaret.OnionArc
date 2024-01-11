using ETicaret.Application.Abstractions.Token;
using ETicaret.Application.Exceptions;
using ETicaret.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly SignInManager<Domain.Entities.Identity.AppUser> _singInManager;
        private readonly ITokenHandler _tokenHandler;
        public LoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, SignInManager<Domain.Entities.Identity.AppUser> singInManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var appUser = await _userManager.FindByNameAsync(request.UsernameOrEmail)  ??
                          await _userManager.FindByEmailAsync(request.UsernameOrEmail) ??
                          throw new NotFoundUserException();
            
            SignInResult result = await _singInManager.CheckPasswordSignInAsync(appUser, request.Password,false);
            
            if (result.Succeeded)
            {
                var token = _tokenHandler.CreateAccessToken(5);
                return new()
                {
                    Token = token
                };
            }
           
           throw new AuthenticationErrorException();
        }
       
    }
}
