using Microsoft.Extensions.Logging;
using SimplyShare.Core;
using SimplyShare.Core.Models;
using SimplyShare.Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using SimplyShare.Common;

namespace SimplyShare.Tracker.Operations
{
    public class Tracker : ITracker
    {
        private readonly ISharingOperation _sharingOperation;
        private readonly ILogger<Tracker> _logger;

        public Tracker(ISharingOperation sharingOperation, ILogger<Tracker> logger)
        {
            _sharingOperation = sharingOperation;
            _logger = logger;
        }

        public async Task<AnnounceResponse> Announce(AnnounceRequest request, string userId)
        {
            var eventId = request.GenerateEventId();
            _logger.LogInformation(eventId, "announceRequest: {announceRequest} userId: {userId}", request, userId);
            if (request == null)
            {
                _logger.LogError(eventId, "announceRequest is null. userId: {userId}", userId);
                return CreateResponseFromError($"The incoming request is in bad state. EventId: {eventId}");
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogError(eventId, "UserId is null. announceRequest: {announceRequest}", request);
                return CreateResponseFromError($"The incoming request is in bad state. EventId: {eventId}");
            }

            try
            {
                var context = await _sharingOperation.GetSharingContext(userId, request.Info_Hash);

                if (context == null)
                {
                    _logger.LogWarning(eventId, "sharing context not found");
                    return CreateResponseFromError($"This file no longer exists. EventId: {eventId}");
                }

                var response = CreateResponseFromContext(context);
                response.TrackerId = request.TrackerId ?? Guid.NewGuid().ToString();
                return response;
            }
            catch (Exception exception)
            {
                _logger.LogError(eventId, exception, exception.Message);
                return CreateResponseFromError($"Something went wrong while fetching the announce information. EventId: {eventId}");
            }

        }

        private AnnounceResponse CreateResponseFromContext(SharingContext context)
        {
            var response = new AnnounceResponse();
            response.Complete = 1;
            response.Incomplete = 0;
            response.Peers = context.User.UserAddress.Addresses.Select(address => new Peer(null, address.Host, address.Port));
            response.Interval = TimeSpan.FromHours(1).Seconds;
            return response;
        }

        private AnnounceResponse CreateResponseFromError(string message) => new AnnounceResponse(message);
    }
}
