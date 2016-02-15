using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ServiceBus.Messaging;
using System.Text;
using System.Configuration;
using Lego.EV3.Shared;

namespace Lego.EV3.Net.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async System.Threading.Tasks.Task<JsonResult> SendMovement(int movement)
        {
            try
            {
                //Put all of the data into a single class
                EventHubData ehd = new EventHubData() { Movement = movement, EntryTime= DateTime.UtcNow};

                //serialize it
                var serializedString = JsonConvert.SerializeObject(ehd);

                ////Create the Event Data object so the data and partition are known
                EventData data = new EventData(Encoding.UTF8.GetBytes(serializedString))
                {
                    PartitionKey = "0"
                };


                ////Get a reference to the event hub
                EventHubClient client = EventHubClient.CreateFromConnectionString(ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"], "legodemoeventhub");


                ////Send the data
                await client.SendAsync(data);


                //Return to the client

                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }



        }
    }


}