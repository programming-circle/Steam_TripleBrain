using FluentValidation;
using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Command.OrderItem
{
    public class CreateOrderItemCommand : IRequest<Result<OrderItemViewProfile>>
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid? GameId { get; set; }

        // public Guid? DLCId { get; set; }

        public decimal PriceOfItem { get; set; }
    }

    public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemCommand>
    {
        public CreateOrderItemValidator() {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderId Empty. ");
            RuleFor(x => x.GameId).NotEmpty().WithMessage("GameId empty. ");
            RuleFor(x => x.PriceOfItem).NotEmpty().WithMessage("Price is empty");
        }
    }
}
