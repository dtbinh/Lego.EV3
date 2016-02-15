using Lego.EV3.Shared;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Lego.EV3.Net.Web.Hubs
{
    public class VoteDriveHub: Hub
    {

        public void SendDriveCommand(DriveCommand command)
        {
            Trace.TraceInformation("SendDriveCommand: " + command.ToString());
        }

    }
}