﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplyShare.Core;
using SimplyShare.Core.Models;

namespace SimplyShare.Tracker.Controllers
{
    [Route("api/announce")]
    [ApiController]
    public class AnnounceController : ControllerBase
    {
        [HttpGet]
        public ActionResult<AnnounceResponse> Get([FromQuery] AnnounceRequest request)
        {
            return new AnnounceResponse();
        }
    }
}