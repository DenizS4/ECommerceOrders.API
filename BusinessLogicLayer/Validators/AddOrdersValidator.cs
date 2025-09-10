using System;
using System.Linq;
using BusinessLogicLayer.DTO;
using FluentValidation;

namespace BusinessLogicLayer.Validators
{
    public class AddOrdersValidator : AbstractValidator<AddOrderDto>
    {
        public AddOrdersValidator()
        {
            // Core
            RuleFor(x => x.BuyerID)
                .NotEmpty();

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3)
                .Must(s => s.All(char.IsLetter))
                .WithMessage("Currency must be ISO-4217 alpha-3 (e.g., USD, EUR, TRY).")
                .Must(s => s.ToUpperInvariant() == s)
                .WithMessage("Currency must be uppercase ISO code.");

            // Addresses
            RuleFor(x => x.BillingAddress)
                .NotNull()
                .SetValidator(new AddressDtoValidator());

            RuleFor(x => x.ShippingAddress)
                .NotNull()
                .SetValidator(new AddressDtoValidator());

            // Buyer snapshot (optional but validate if present)
            When(x => x.BuyerSnapshot != null, () =>
            {
                RuleFor(x => x.BuyerSnapshot)
                    .SetValidator(new BuyerSnapshotDtoValidator());
            });

            // Items
            RuleFor(x => x.Items)
                .NotNull()
                .NotEmpty()
                .WithMessage("At least one order item is required.")
                .Must(items => items.Count <= 500)
                .WithMessage("Too many items. Max 500 per order.");

            RuleForEach(x => x.Items)
                .SetValidator(new AddOrderItemDtoValidator());

            // Optional totals from client (if you accept them)
            // Ensure non-negative numbers
            RuleFor(x => x.Subtotal).GreaterThanOrEqualTo(0).When(x => x.Subtotal.HasValue);
            RuleFor(x => x.DiscountTotal).GreaterThanOrEqualTo(0).When(x => x.DiscountTotal.HasValue);
            RuleFor(x => x.ShippingTotal).GreaterThanOrEqualTo(0).When(x => x.ShippingTotal.HasValue);
            RuleFor(x => x.TaxTotal).GreaterThanOrEqualTo(0).When(x => x.TaxTotal.HasValue);
            RuleFor(x => x.GrandTotal).GreaterThanOrEqualTo(0).When(x => x.GrandTotal.HasValue);

            // If all totals are provided, verify the arithmetic relation:
            // GrandTotal == Subtotal - DiscountTotal + ShippingTotal + TaxTotal
            When(x =>
                x.Subtotal.HasValue &&
                x.DiscountTotal.HasValue &&
                x.ShippingTotal.HasValue &&
                x.TaxTotal.HasValue &&
                x.GrandTotal.HasValue,
            () =>
            {
                RuleFor(x => x)
                    .Must(x =>
                    {
                        var expected = x.Subtotal!.Value - x.DiscountTotal!.Value + x.ShippingTotal!.Value + x.TaxTotal!.Value;
                        return NearlyEqual(expected, x.GrandTotal!.Value);
                    })
                    .WithMessage("GrandTotal must equal Subtotal - DiscountTotal + ShippingTotal + TaxTotal.");
            });
        }

        private static bool NearlyEqual(decimal a, decimal b, decimal epsilon = 0.01m)
            => Math.Abs(a - b) <= epsilon;
    }

    public class AddOrderItemDtoValidator : AbstractValidator<AddOrderItemDto>
    {
        public AddOrderItemDtoValidator()
        {
            RuleFor(x => x.ProductRefID)
                .NotEmpty();

            RuleFor(x => x.Sku)
                .NotEmpty()
                .MaximumLength(64);

            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.Category)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Quantity)
                .GreaterThan(0);

            RuleFor(x => x.LineDiscount)
                .GreaterThanOrEqualTo(0)
                .When(x => x.LineDiscount.HasValue);

            RuleFor(x => x.LineTax)
                .GreaterThanOrEqualTo(0)
                .When(x => x.LineTax.HasValue);

            // Optional consistency check if client sends LineTotal
            // LineTotal == UnitPrice * Quantity - LineDiscount + LineTax
            When(x => x.LineTotal != default, () =>
            {
                RuleFor(x => x)
                    .Must(x =>
                    {
                        var discount = x.LineDiscount;
                        var tax = x.LineTax;
                        var expected = (x.UnitPrice * x.Quantity)
                                       - (discount ?? 0m)
                                       + (tax ?? 0m);
                        return Math.Abs(expected - (decimal)x.LineTotal!) <= 0.01m;
                    })
                    .WithMessage("LineTotal must equal UnitPrice*Quantity - LineDiscount + LineTax (±0.01).");
            });
        }
    }

    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(32);

            RuleFor(x => x.Line1)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.Line2)
                .MaximumLength(256)
                .When(x => !string.IsNullOrWhiteSpace(x.Line2));

            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(x => x.Zip)
                .NotEmpty()
                .MaximumLength(32);

            RuleFor(x => x.Country)
                .NotEmpty()
                .Length(2)
                .Must(s => s.All(char.IsLetter))
                .WithMessage("Country must be ISO-3166-1 alpha-2 code (e.g., US, DE, TR).")
                .Must(s => s.ToUpperInvariant() == s)
                .WithMessage("Country must be uppercase ISO code.");
        }
    }

    public class BuyerSnapshotDtoValidator : AbstractValidator<BuyerSnapshotDto>
    {
        public BuyerSnapshotDtoValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Name)
                .MaximumLength(128)
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            RuleFor(x => x.Company)
                .MaximumLength(128)
                .When(x => !string.IsNullOrWhiteSpace(x.Company));
        }
    }
}
