using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using EventContracts;

namespace masstransit_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MassTransitController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<ActionResult> Post(string value)
        {
            await _publishEndpoint.Publish<IValueEntered>(new
            {
                Value = value
            });

            return Ok();
        }
    }
}
