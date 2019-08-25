using FluentValidation;
using SimplyShare.Common.Models;

namespace SimplyShare.Tracker.Validators
{
    public class ShareRequestValidator : AbstractValidator<ShareRequest>
    {
        public ShareRequestValidator()
        {
            RuleFor(request => request.User).SetValidator(new UserValidator());
            RuleFor(request => request.MetaInfo).SetValidator(new MetaInfoValidator());
        }
    }
}
