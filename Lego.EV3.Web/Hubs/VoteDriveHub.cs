using Microsoft.AspNet.SignalR;
using System.Diagnostics;
using Lego.EV3.Shared;

namespace Lego.EV3.Web.Hubs
{
    public class VoteDriveHub : Hub
    {

        public void SendDriveCommand(DriveCommand command)
        {
            Trace.TraceInformation("SendDriveCommand: " + command.ToString());
     
        }

    }
}