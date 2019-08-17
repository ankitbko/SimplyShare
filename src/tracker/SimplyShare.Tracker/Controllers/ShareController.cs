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
            var validationResult = await _shareRequestValidator.ValidateAsync(request);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation Failed. StartSharing. {validationResult}", validationResult);
                return BadRequest(validationResult);
            }

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
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return StatusCode(500, exception.Message);
            }
        }
    }
}