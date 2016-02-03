using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Configuration;

namespace Lego.EV3.API.Controllers
{
    public class ValuesController : ApiController
    {
        public string Get()
        {
            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            Microsoft.Azure.CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Get the next message
            CloudQueueMessage retrievedMessage = queue.GetMessage();

            //Process the message in less than 30 seconds, and then delete the message
            queue.DeleteMessage(retrievedMessage);

            return retrievedMessage.AsString;


        }
        // POST api/values
        public void Post([FromBody]int value)
        {
            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            Microsoft.Azure.CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist.
            queue.CreateIfNotExists();

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(value.ToString());
            queue.AddMessage(message);


        }

        //// GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
//// Create the queue if it doesn't already exist
//if(await queue.CreateIfNotExistsAsync())
//{
//    Console.WriteLine("Queue '{0}' Created", queue.Name);
//}
//else
//{
//    Console.WriteLine("Queue '{0}' Exists", queue.Name);
//}

//// Create a message to put in the queue
//CloudQueueMessage cloudQueueMessage = new CloudQueueMessage("My message");

//// Async enqueue the message
//await queue.AddMessageAsync(cloudQueueMessage);
//Console.WriteLine("Message added");

//// Async dequeue the message
//CloudQueueMessage retrievedMessage = await queue.GetMessageAsync();
//Console.WriteLine("Retrieved message with content '{0}'", retrievedMessage.AsString);

//// Async delete the message
//await queue.DeleteMessageAsync(retrievedMessage);
//Console.WriteLine("Deleted message");