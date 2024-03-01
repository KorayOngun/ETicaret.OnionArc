using ETicaret.Application.Abstractions.Services.Authentication;
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
        private readonly IInternalAuthentication _internalAuthentication;

        public LoginUserCommandHandler(IInternalAuthentication internalAuthentication)
        {
            _internalAuthentication = internalAuthentication;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
          var token = await _internalAuthentication.LoginAsync(request.UsernameOrEmail, request.Password, 900);
          return new() 
          {
            Token = token
          };       
        }
       
    }
}
