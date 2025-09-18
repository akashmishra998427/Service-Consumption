using FluentValidation;
using ServiceConsumption.Models.Entity;

namespace ServiceConsumption.Models.Validation
{
    public class PostApiEntityValidations : AbstractValidator<PostApiEntity>
    {
        public PostApiEntityValidations()
        {
            RuleFor(y => y.Title).NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
            RuleFor(y => y.Body).NotEmpty().WithMessage("Body is required")
                .MinimumLength(10).WithMessage("Body must be at least 10 characters long");
            RuleFor(y => y.UserID).NotEmpty().WithMessage("UserID is required")
                .GreaterThan(0).WithMessage("UserID must be a positive integer");
        }
    }
}
