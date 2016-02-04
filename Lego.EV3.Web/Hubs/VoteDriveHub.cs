using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNet.SignalR;
using System.Diagnostics;
using Lego.EV3.Shared;
using System.Threading.Tasks;

namespace Lego.EV3.Web.Hubs
{
    public class VoteDriveHub : Hub
    {

        public void SendDriveCommand(DriveCommand command)
        {
            Trace.TraceInformation("SendDriveCommand: " + command.ToString());

            //call to BeAPI
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://legoev3api.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsJsonAsync("api/values", (int)command).Result;
                if (response.IsSuccessStatusCode)
                {
                    var a = response.Headers.Location;
                }
            }


        }

    }
}