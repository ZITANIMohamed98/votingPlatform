// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using Azure.Messaging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace VotingPlatform
{
    public class RG_actions
    {
        private readonly ILogger<RG_actions> _logger;

        public RG_actions(ILogger<RG_actions> logger)
        {
            _logger = logger;
        }

        [Function(nameof(RG_actions))]
        public void Run([EventGridTrigger] CloudEvent cloudEvent)
        {
            _logger.LogInformation("Event type: {type}, Event subject: {subject}", cloudEvent.Type, cloudEvent.Subject);
        }
    }
}
