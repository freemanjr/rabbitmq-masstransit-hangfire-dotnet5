using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using EventContracts;
using System.ComponentModel;
using Hangfire;

namespace masstransit_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MassTransitController : ControllerBase
    {
        private readonly IMessageScheduler _messageScheduler;

        public MassTransitController(IMessageScheduler messageScheduler)
        {
            _messageScheduler = messageScheduler;
        }

        [HttpPost]
        public async Task<ActionResult> Post(string value)
        {
            await _messageScheduler.SchedulePublish<IValueEntered>(DateTime.UtcNow, new
            {
                Value = value
            });

            return Ok();
        }
    }
}
