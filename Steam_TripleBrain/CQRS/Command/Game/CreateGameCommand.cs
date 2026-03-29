using FluentValidation;
using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Command.Game
{
    public class CreateGameCommand : IRequest<Result<GameViewProfile>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ImageUrlViewProfile Poster { get; set; }

        public List<ImageUrlViewProfile>? Images { get; set; }

        public double Rating { get; set; }

        public string Description { get; set; }
        public List<GenreViewProfile> Genres { get; set; }

        public List<TagViewProfile>? Tags { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        public Guid Author { get; set; }

        public List<DLCViewProfile>? DLCs { get; set; }
    }

    public class CreateGameValidator : AbstractValidator<CreateGameCommand>
    {
        //Section for validation rules
        public CreateGameValidator() { 
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Name).MinimumLength(3).WithMessage("Name must be at least 3 characters long.");
            RuleFor(x => x.Name).MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
            RuleFor(x => x.Poster).NotNull().WithMessage("Poster is required.");
            RuleFor(x => x.Rating).InclusiveBetween(0, 10).WithMessage("Rating must be between 0 and 10.");
            RuleFor(x => x.Description).MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");
            RuleFor(x => x.Genres).NotEmpty().WithMessage("At least one genre is required.");
            RuleFor(x => x.Genres).Must(g => g.Count <= 10).WithMessage("No more than 10 genres are allowed.");
            RuleFor(x => x.Tags).NotEmpty().WithMessage("At least one tag is required.");
            RuleFor(x => x.Tags).Must(t => t.Count <= 20).WithMessage("No more than 20 tags are allowed.");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be a positive value.");
            RuleFor(x => x.Price).LessThanOrEqualTo(9000).WithMessage("Price must not exceed 9000.");
            RuleFor(x => x.Discount).InclusiveBetween(0, 100).WithMessage("Discount must be between 0 and 100.");
            RuleFor(x => x.Author).NotEmpty().WithMessage("Author is required.");
        }
    }
}
