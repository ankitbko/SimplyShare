using FluentValidation;
using SimplyShare.Core;
using SimplyShare.Core.Models;
using System.Net;

namespace SimplyShare.Tracker.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Id).NotNull().NotEmpty();
            RuleFor(user => user.UserAddress).NotNull()
                .DependentRules(() =>
                {
                    RuleFor(user => user.UserAddress.Addresses)
                        .NotNull()
                        .NotEmpty()
                        .ForEach(addressRule => addressRule
                        .Must(address => address.Port > 0 && address.Port < 65536)
                        .Must(address => IPAddress.TryParse(address.Host, out var _)));
                });

            RuleSet("write", () =>
            {
                RuleFor(user => user.SecretHash).NotNull().NotEmpty();
            });
        }
    }
}
