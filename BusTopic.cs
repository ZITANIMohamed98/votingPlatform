using System;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace VotingPlatform
{
    public class BusTopic
    {
        private readonly ILogger<BusTopic> _logger;

        private static readonly HttpClient client = new HttpClient();
        public BusTopic(ILogger<BusTopic> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// the function receives a message from Service Bus and processes it and update the count on the Table Storage
        /// </summary>
        class votes
        {
            public int number { get; set; }
        }
        [Function(nameof(BusTopic))]
        public async Task Run(
            [ServiceBusTrigger("rainy", "testsubscription", Connection = "ServiceBusConnectionString")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            List<string> presidents = new List<string> { "Mandela", "Trump", "Obama", "Tebboune", "Bouteflika" };
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
            

            // Complete the message
            await messageActions.CompleteMessageAsync(message);

            // Initialize a stringcontent to send a putAsync to the Table Storage - Need to work on this..
            // 
            //votes votesdata = new votes { number = 0 };
            //StringContent currentvotenumber = new StringContent(votesdata);

            string President = message.Body.ToString();
            string partition = "";
            if (President != null) {
                if (presidents.Contains(message.Body.ToString()))
                {
                    switch (President)
                    {
                        case "Mandela":
                            partition = "mndl";
                            HttpResponseMessage mndlresponse  = await client.GetAsync("https://testevenhubtriggergbed8.table.core.windows.net/votingdata(PartitionKey=" + partition + ", RowKey='')");
                            _logger.LogInformation($"Fetched the latest vote number for {President} which is {mndlresponse}");
                            //await client.PutAsync("https://testevenhubtriggergbed8.table.core.windows.net/votingdata(PartitionKey=" + partition + ", RowKey='myRowKey')", votesnumber);

                            break;

                        case "Trump":
                            partition = "trmp";
                            _logger.LogInformation($"Voted for {President}");
                            HttpResponseMessage trmpresponse = await client.GetAsync("https://testevenhubtriggergbed8.table.core.windows.net/votingdata(PartitionKey=" + partition + ", RowKey='')");
                            _logger.LogInformation($"Fetched the latest vote number for {President} which is {trmpresponse}");
                            // Get the current Vote number coreesponding to each President
                            //HttpResponseMessage trmpresponse = await client.GetAsync("https://testevenhubtriggergbed8.table.core.windows.net/votingdata(PartitionKey=" + partition + ", RowKey='myRowKey')");

                            //trmpresponse.EnsureSuccessStatusCode();
                            //votesnumber = Int32.Parse(await trmpresponse.Content.ReadAsStringAsync());
                            //votesnumber = +1;
                            //StringContent currentvotenumber 

                            //await client.PutAsync("https://testevenhubtriggergbed8.table.core.windows.net/votingdata(PartitionKey=" + partition + ", RowKey='myRowKey')", votesnumber);
                            break;

                        case "Obama":
                            partition = "obm";
                            _logger.LogInformation($"Voted for {President}");
                            HttpResponseMessage obmresponse = await client.GetAsync("https://testevenhubtriggergbed8.table.core.windows.net/votingdata(PartitionKey='mndl',RowKey='1')?sp=r&st=2024-12-30T11:52:40Z&se=2025-01-31T11:52:00Z&spr=https&sv=2022-11-02&sig=zpfbb9F0rgEO4hOcaEhgGV%2BnoIxnkS1soYs85lKAZ%2FI%3D&tn=votingdata");
                            //.EnsureSuccessStatusCode();
                            string result = await obmresponse.Content.ReadAsStringAsync();
                            _logger.LogInformation($"Fetched the latest vote number for {President} which is{result}");
                            //await client.PutAsync("https://testevenhubtriggergbed8.table.core.windows.net/votingdata(PartitionKey=" + partition + ", RowKey='myRowKey')", votesnumber);
                            break;

                        case "Tebboune":
                            partition = "tbn";
                            _logger.LogInformation($"Voted for {President}");
                            HttpResponseMessage tbnresponse = await client.GetAsync("https://testevenhubtriggergbed8.table.core.windows.net/votingdata(PartitionKey=" + partition + ", RowKey='')");
                            _logger.LogInformation($"Fetched the latest vote number for {President} which is {tbnresponse}");

                            //await client.PutAsync("https://testevenhubtriggergbed8.table.core.windows.net/votingdata(PartitionKey=" + partition + ", RowKey='myRowKey')",votesnumber);

                            break;

                        case "Bouteflika":
                            partition = "btflk";
                            _logger.LogInformation($"Voted for {President}");
                            HttpResponseMessage btflkresponse = await client.GetAsync("https://testevenhubtriggergbed8.table.core.windows.net/votingdata(PartitionKey=" + partition + ", RowKey='')");
                            _logger.LogInformation($"Fetched the latest vote number for {President} which is {btflkresponse}");

                            //await client.PutAsync("https://testevenhubtriggergbed8.table.core.windows.net/votingdata(PartitionKey=" + partition + ", RowKey='myRowKey')");
                            break;

                        default:
                            throw new Exception("Oops, Something went wrong");
                    }
                }
            }
            
        }
    }
}
