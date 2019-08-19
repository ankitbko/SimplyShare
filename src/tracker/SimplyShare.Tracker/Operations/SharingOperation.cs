using FluentValidation;
using Microsoft.Extensions.Options;
using SimplyShare.Common.Models;
using SimplyShare.Tracker.Exceptions;
using SimplyShare.Tracker.Models;
using SimplyShare.Tracker.Repository;
using SimplyShare.Tracker.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplyShare.Tracker.Operations
{
    public class SharingOperation : ISharingOperation
    {
        private readonly ISharingContextRepository _sharingRepository;
        private readonly SharingOption _sharingOptions;

        public SharingOperation(ISharingContextRepository sharingRepository, IOptions<SharingOption> sharingOptions)
        {
            _sharingRepository = sharingRepository;
            _sharingOptions = sharingOptions.Value;
        }

        public async Task StartSharing(ShareRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var validator = new ShareRequestValidator();
            await validator.ValidateAndThrowAsync(request, "default,write");

            var context = SharingContext.Create(request);
            var existing = await _sharingRepository.GetSharingContextForUserByInfoHash(request.User.Id, context.InfoHash);

            if (existing != default)
            {
                throw new DuplicateSharingContextException();
            }

            SanitizeSharingContext(context);

            await _sharingRepository.CreateSharingContext(context);
        }

        protected virtual void SanitizeSharingContext(SharingContext context)
        {
            if (context.SharingConfiguration.Expiry > _sharingOptions.MaxExpiry
                || context.SharingConfiguration.Expiry < _sharingOptions.MinExpiry)
            {
                context.SharingConfiguration.Expiry = _sharingOptions.DefaultExpiry;
            }
        }
    }
}
