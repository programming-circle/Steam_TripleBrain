using FluentValidation;
using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Command.Order
{
    public class UpdateOrderCommand : IRequest<Result<Unit>>
    {

        public Guid Id { get; set; }
        public List<OrderItemViewProfile> Items { get; set; } = new();
    }
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderValidator()
        {
            RuleFor(x => x.Items).NotEmpty().WithMessage("Need at least one item to update order");
            //RuleFor(x => x.TotalPrice).NotEmpty().WithMessage("Must be price");
            //RuleFor(x => x.TotalPrice).GreaterThan(-1).WithMessage("Must be valid (not negative)"); //Check later if that works
            //RuleFor(x => x.Items).NotEmpty().WithMessage("Items can't be null");
        }
    }

}
