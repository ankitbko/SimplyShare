using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplyShare.Common.Models;
using SimplyShare.Core;
using SimplyShare.Tracker.Exceptions;
using SimplyShare.Tracker.Operations;

namespace SimplyShare.Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShareController : ControllerBase
    {
        private readonly ISharingOperation _sharingOperation;
        private readonly IValidator<ShareRequest> _shareRequestValidator;
        private readonly ILogger<ShareController> _logger;

        public ShareController(ISharingOperation sharingOperation, IValidator<ShareRequest> shareRequestValidator, ILogger<ShareController> logger)
        {
            _sharingOperation = sharingOperation;
            _shareRequestValidator = shareRequestValidator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> StartSharing(ShareRequest request)
        {
            try
            {
                await _sharingOperation.StartSharing(request);
                return Ok();
            }
            catch (DuplicateSharingContextException exception)
            {
                _logger.LogError(exception, exception.Message);
                return BadRequest("File is already shared by this user");
            }
            catch (ValidationException exception)
            {
                _logger.LogWarning("Validation Failed. StartSharing");
                _logger.LogError(exception, exception.Message);
                return BadRequest(exception.Errors);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return StatusCode(500, exception.Message);
            }
        }
    }
}