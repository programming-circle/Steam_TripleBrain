using FluentValidation;
using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Command.Order
{
    public class CreateOrderCommand : IRequest<Result<OrderViewProfile>>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemViewProfile> Items { get; set; } = new();

        public decimal TotalPrice { get; set; }
    }

    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator() { 
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User not found , can't create order");
            RuleFor(x => x.CreatedAt).NotEmpty().WithMessage("Date must be");
            RuleFor(x => x.TotalPrice).NotEmpty().WithMessage("Must be price");
            RuleFor(x => x.TotalPrice).GreaterThan(-1).WithMessage("Must be valid (not negative)"); //Check later if that works
            RuleFor(x => x.Items).NotEmpty().WithMessage("Items can't be null");
        }
    }
}
