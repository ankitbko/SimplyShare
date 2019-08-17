using Microsoft.Extensions.Options;
using SimplyShare.Common.Models;
using SimplyShare.Tracker.Exceptions;
using SimplyShare.Tracker.Models;
using SimplyShare.Tracker.Repository;
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

        public Task StartSharing(ShareRequest request)
        {
            var context = SharingContext.Create(request);
            var existing = _sharingRepository.GetSharingContextForUserByInfoHash(request.User.Id, context.InfoHash);

            if (existing != default)
            {
                throw new DuplicateSharingContextException();
            }

            if (context.SharingConfiguration.Expiry > _sharingOptions.MaxExpiry
                || context.SharingConfiguration.Expiry < _sharingOptions.MinExpiry)
            {
                context.SharingConfiguration.Expiry = _sharingOptions.DefaultExpiry;
            }

            return _sharingRepository.CreateSharingContext(context);
        }
    }
}
