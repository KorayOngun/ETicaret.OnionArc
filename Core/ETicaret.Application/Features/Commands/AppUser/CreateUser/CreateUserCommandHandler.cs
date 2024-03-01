using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaret.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly IUserService _userService;
        public CreateUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var response = await _userService.CreateAsync(new()
            {
                Email = request.Email,
                NameSurname = request.NameSurname,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
                UserName = request.UserName
            });

            return new() 
            { 
                Message = response.Message,
                Succeded = response.Succeded,
            };
            
            //throw new UserCreateFailedException();

        }
    }
}
