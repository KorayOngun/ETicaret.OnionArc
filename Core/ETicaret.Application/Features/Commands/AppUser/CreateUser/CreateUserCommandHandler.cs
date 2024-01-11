using ETicaret.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaret.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                Email = request.Email,
                NameSurname = request.NameSurname,
            },request.Password);

            CreateUserCommandResponse response = new()
            {
                Succeded = result.Succeeded
            };

            if (result.Succeeded)           
                response.Message = "kullanıcı başarıyla kaydedilmiştir";
            else
                foreach (var item in result.Errors)
                    response.Message+= $"{item.Code} --> {item.Description} \n\r";
                
            return response;
            
            //throw new UserCreateFailedException();

        }
    }
}
