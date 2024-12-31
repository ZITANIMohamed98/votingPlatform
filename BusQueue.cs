using System;
using System.Threading.Tasks;
using System.Xml;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using System.Net.Http;

namespace VotingPlatform
{
    public class BusQueue
    {
        private readonly ILogger<BusQueue> _logger;
        private static readonly HttpClient client = new HttpClient();
        public BusQueue(ILogger<BusQueue> logger)
        {
            _logger = logger;
        }
        

        [Function(nameof(BusQueue))]
        public async Task Run(
            [ServiceBusTrigger("queue", Connection = "ServiceBusConnectionString")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions,HttpClient httpClient)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);


            //var response = await client.PutAsync("https://myaccount.table.core.windows.net/mytable(PartitionKey='myPartitionKey', RowKey='myRowKey')", content);
            // Complete the message
            await messageActions.CompleteMessageAsync(message);

        }
    }
    
}
