using FluentValidation;
using SimplyShare.Common.Models;
using System.Net;

namespace SimplyShare.Tracker.Validators
{
    public class ShareRequestValidator : AbstractValidator<ShareRequest>
    {
        public ShareRequestValidator()
        {
            RuleFor(request => request.User.SecretHash).NotNull();
            RuleFor(request => request.User.Id).NotNull();
            RuleFor(request => request.User.UserAddress.Addresses).NotEmpty()
                .ForEach(addressRule => addressRule
                    .Must(address => address.Port > 0 && address.Port < 65536)
                    .Must(address => IPAddress.TryParse(address.Host, out var _)));

            RuleFor(request => request.MetaInfo).SetValidator(new MetaInfoValidator());
        }
    }
}
