using System;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace VotingPlatform
{
    public class EventHub
    {
        private readonly ILogger<EventHub> _logger;

        private static readonly HttpClient client = new HttpClient();
        public EventHub(ILogger<EventHub> logger)
        {
            _logger = logger;
        }

        [Function(nameof(EventHub))]
        public async Task Run([EventHubTrigger("testeventhub", Connection = "EventHubConnection")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                _logger.LogInformation("Event Body: {body}", @event.Body);
                _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
                _logger.LogInformation("writing the data to the Table");
               // var response = await client.PutAsync("https://myaccount.table.core.windows.net/mytable(PartitionKey='myPartitionKey', RowKey='myRowKey')", content);
            }
        }
    }
}
