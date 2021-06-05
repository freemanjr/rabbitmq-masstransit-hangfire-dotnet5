using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventContracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace masstransit_api.EventConsumer
{
    public class ValueEnteredEventConsumer : IConsumer<IValueEntered>
    {
        private readonly ILogger<ValueEnteredEventConsumer> _logger;
        public ValueEnteredEventConsumer(ILogger<ValueEnteredEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IValueEntered> context)
        {
            _logger.LogInformation("Value: {Value}", context.Message.Value);

            return Task.CompletedTask;
        }
    }
}
