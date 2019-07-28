using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplyShare.Core;

namespace SimplyShare.Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShareController : ControllerBase
    {
        [HttpPost]
        public ActionResult StartSharing(PrimarySeeder request)
        {
            return NotFound();
        }
    }
}