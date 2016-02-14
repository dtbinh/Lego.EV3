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
        // POST api/values
        public void Post([FromBody]int value)
        {
            //the only admitted values are from 1 to 4
            if(value>=1 && value <= 4)
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
           


        }
        public int Get()
        {
           
            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            Microsoft.Azure.CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist.
            queue.CreateIfNotExists();

            // Fetch the queue attributes.
            queue.FetchAttributes();

            // Retrieve the cached approximate message count.
            int? cachedMessageCount = queue.ApproximateMessageCount;
            //number of messages that are going to be processed
            int numberOfMessages = 0;
            //resulting movement of the Lego
            int movement = 0;
            //movement dequeued
            int dequeuedMessage = 0;
            //if there are messages in the queue, calculate the movement from the votes
            if (cachedMessageCount != 0)
            {
                //if there are less than 32 messsages there will be all processed
                if (cachedMessageCount < 32)
                {
                    numberOfMessages = (int)cachedMessageCount;
                }
                //if there are more than 32 messages, 32 will be processed
                else
                {
                    numberOfMessages = 32;
                }
                //array for votes
                int[] votes = new int[4];
                //calculate the votation
                foreach (CloudQueueMessage message in queue.GetMessages(numberOfMessages, TimeSpan.FromSeconds(30)))
                {
                    // Process  messages in less than 30 second, deleting each message after processing.
                    int.TryParse(message.AsString, out dequeuedMessage);
                    votes[dequeuedMessage-1]++;                  
                    queue.DeleteMessage(message);
                }
               movement = calculateVotes(votes);
              
            }
            //if there are not messaged in the queue the movement will be 0, not a movement
            else
            {
                movement = 0;
            }

           return movement;
        }



        //method for calculating the most voted movement
        private int calculateVotes(int[] votes)
        {
            int maxNumberOfVotes = votes[0];
            int result = 0;
            for(int i = 1; i < votes.Length; i++)
            {
                if (votes[i] > maxNumberOfVotes)
                {
                    result = i;
                }
            }
               
            return result+1;
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