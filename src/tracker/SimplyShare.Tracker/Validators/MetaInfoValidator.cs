using FluentValidation;
using SimplyShare.Core;
using System.Linq;

namespace SimplyShare.Tracker.Validators
{
    public class MetaInfoValidator: AbstractValidator<MetaInfo>
    {
        public MetaInfoValidator()
        {
            RuleFor(meta => meta.Announce).NotNull().Unless(meta => meta?.AnnounceList?.Count > 0);
            RuleFor(meta => meta.Info.PieceLength).NotEmpty();
            RuleFor(meta => meta.Info.Pieces.Length).NotEmpty().Must(len => len % 20 == 0);  // Must be multiple of 20
            RuleFor(meta => (meta.Info as SingleFileInfo).Length).NotEmpty().When(meta => meta.Info is SingleFileInfo);
            RuleFor(meta => (meta.Info as SingleFileInfo).Name).NotEmpty().When(meta => meta.Info is SingleFileInfo);
            RuleFor(meta => (meta.Info as MultipleFileInfo).Name).NotEmpty().When(meta => meta.Info is MultipleFileInfo);
            RuleFor(meta => (meta.Info as MultipleFileInfo).Files)
                .NotEmpty()
                .When(meta => meta.Info is MultipleFileInfo)
                .DependentRules(() =>
                {
                    RuleForEach(meta => (meta.Info as MultipleFileInfo).Files)
                        .Must(file => file.Length > 0).WithMessage("Length cannot be empty or 0")
                        .Must(file => file.Path.Count > 0).WithMessage("Files cannot be empty")
                        .Must(file => file.Path.Any(path => string.IsNullOrEmpty(path)) == false).WithMessage("Files cannot be empty");
                });
                
        }
    }
}
