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
using SimplyShare.Core.Models;
using SimplyShare.Tracker.Exceptions;
using SimplyShare.Tracker.Operations;

namespace SimplyShare.Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShareController : ControllerBase
    {
        private readonly ISharingOperation _sharingOperation;
        private readonly ILogger<ShareController> _logger;

        public ShareController(ISharingOperation sharingOperation, ILogger<ShareController> logger)
        {
            _sharingOperation = sharingOperation;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var request = new ShareRequest
            {
                MetaInfo = new MetaInfo(
                    new SingleFileInfo(
                        500,
                        string.Join("", Enumerable.Range(0, 20).Select(_ => "a")),
                        "name",
                        5000),
                    "http://announce"),
                User = new User
                {
                    Id = "userid",
                    SecretHash = "secret",
                    UserAddress = new UserAddress
                    {
                        Addresses = new List<Address>
                        {
                            new Address() { Host = "192.0.0.1", Port = 5770, Type = AddressType.Internal }
                        }
                    }
                },
                SharingConfiguration = new SharingConfiguration
                {
                    Expiry = TimeSpan.FromDays(5),
                    SharingScope = SharingScope.Internal
                }
            };
            await _sharingOperation.StartSharing(request);
            return Ok();
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