using FluentValidation;
using ServiceConsumption.Models.Entity;

namespace ServiceConsumption.Models.Validation
{
    public class AddProductValidation:AbstractValidator<AddProductsEntity>
    {
        public AddProductValidation()
        {
            RuleFor(AP => AP.ID).NotEmpty().WithMessage("Id is required id can't Empty")
                                .GreaterThan(0).WithMessage("Id can't be be zero or negative number it should alwyas be the grater than 0 and a positive number");
            RuleFor(AP => AP.Price).NotEmpty().WithMessage("Price Can't be Empty")
                                 .GreaterThan(50).WithMessage("Price Should be grater 50")
                                 .LessThan(999).WithMessage("Price Should't less than 999");
            RuleFor(AP => AP.Image).NotEmpty().WithMessage("Image URL should't be empty");
            RuleFor(AP => AP.Title).NotEmpty().WithMessage("Title can't be empty");
            RuleFor(Ap => Ap.category).NotEmpty().WithMessage("Category can't be Empty");
            RuleFor(AP => AP.Description).NotEmpty().WithMessage("Description Can't be Empty")
                                            .MinimumLength(10).WithMessage("Description should be alwyas grater than 10 charachters");
        }
    }
}
