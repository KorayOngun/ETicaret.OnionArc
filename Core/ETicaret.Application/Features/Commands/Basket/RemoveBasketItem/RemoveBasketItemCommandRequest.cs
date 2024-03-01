using MediatR;

namespace ETicaret.Application.Features.Commands.Basket
{
    public class RemoveBasketItemCommandRequest : IRequest<RemoveBasketItemCommandResponse>
    {
        public  string BasketItemId { get; set; }
    }
}