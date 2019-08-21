using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplyShare.Core.Models;
using SimplyShare.Tracker.Operations;

namespace SimplyShare.Tracker.Controllers
{
    [Route("api/announce")]
    [ApiController]
    public class AnnounceController : ControllerBase
    {
        private readonly ITracker _tracker;
        private readonly ILogger<AnnounceController> _logger;

        public AnnounceController(ITracker tracker, ILogger<AnnounceController> logger)
        {
            _tracker = tracker;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<AnnounceResponse>> Get([FromQuery] AnnounceRequest request, string userId)
        {
            try
            {
                var result = await _tracker.Announce(request, userId);
                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
            }
        }
    }
}