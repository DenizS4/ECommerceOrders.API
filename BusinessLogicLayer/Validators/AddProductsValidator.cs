using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators;

public class AddOrdersValidator : AbstractValidator<AddOrderDto>
{
    public AddOrdersValidator()
    {

    }
}