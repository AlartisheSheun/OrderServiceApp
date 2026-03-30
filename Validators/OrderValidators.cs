using FluentValidation;
using OrderAPI.DTOs;

namespace OrderAPI.Validators;

// CreateOrderDto
public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("Customer name is required")
            .MaximumLength(100).WithMessage("Customer name must not exceed 100 characters");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero");
    }
}

// UpdateOrderDto
public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
{
    public UpdateOrderDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid order status");
    }
}
