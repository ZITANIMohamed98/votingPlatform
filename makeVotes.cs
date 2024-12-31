
/*
  Voting Platform v1.0
  
  makeVotes function:
    Responsible of taking Http requests and creating messages and events to Service Bus Topic and Queue and Event Hub.
    Returns the multi-output binding object MyOutputType. 
    https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=hostbuilder%2Clinux#multiple-output-bindings
    for the event Grid output binding I couldn't send just a string in the body as it always expects a json format, sending just a string "Bouteflika" results in the following error:
    System.Text.Json.JsonReaderException: 'B' is an invalid start of a value. LineNumber: 0 | BytePositionInLine: 0.
    https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-event-grid-output?tabs=python-v2%2Cisolated-process%2Cnodejs-v4%2Cextensionv3&pivots=programming-language-csharp
    

    So if someone finds how to fix this it would be great, as of now I am not using the Event Grid output binding :)
    
 */


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace VotingPlatform
{
    public class makeVotes
    {
        private readonly ILogger<makeVotes> _logger;
        public record President(string Name);
        public makeVotes(ILogger<makeVotes> logger)
        {
            _logger = logger;
        }

        [Function("makeVotes")]
        public MyOutputType Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            List<string> presidents = new List<string> { "Mandela", "Trump", "Obama", "Tebboune", "Bouteflika" };
            
            string president = (req.Query["president"]).ToString(); 
            _logger.LogInformation($"Validating Input...{president}");

            if (presidents.Contains(president))
                {
                _logger.LogInformation($"Right! input validated, Voted for {president}");
                var myObject = new MyOutputType
                {
                    MessageBusQueue = president,
                    Result = new OkObjectResult($"No way!! You voted for {president}"),
                    MessageBusTopic = president,
                    //MessageEventGrid = president,
                    MessageEventHub = president

                };
                return myObject;
            }
            else {
               throw new Exception("an invalid input was introduced.");
            }
        }
        public class MyOutputType
        {
            [HttpResult]
            public IActionResult Result { get; set; }

            [ServiceBusOutput("queue", Connection = "ServiceBusConnectionString")]
            public string MessageBusQueue { get; set; }

            [ServiceBusOutput("rainy", Connection = "ServiceBusConnectionString")]
            public string MessageBusTopic { get; set; }

            //[EventGridOutput(TopicEndpointUri = "MyEventGridTopicUriSetting", TopicKeySetting = "MyEventGridTopicKeySetting")]
            //public string MessageEventGrid { get; set; }
           
            [EventHubOutput("testeventhub",Connection = "EventHubConnection")] // Event hub output binding
            public string MessageEventHub { get; set; }
        }
        
    }
}
