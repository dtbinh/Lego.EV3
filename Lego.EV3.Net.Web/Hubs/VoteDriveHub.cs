using Lego.EV3.Shared;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Lego.EV3.Net.Web.Hubs
{
    public class VoteDriveHub: Hub
    {

        public static Dictionary<DriveCommand, int> CurrentVotesCount = new Dictionary<DriveCommand, int>()
        {
            {DriveCommand.Left, 0 },
            {DriveCommand.Forward, 0 },
            {DriveCommand.Right, 0 },
            {DriveCommand.Back, 0 },
        };

        public void SendDriveCommand(DriveCommand command)
        {
            if(CurrentVotesCount[command] < Int32.MaxValue)
            {
                CurrentVotesCount[command]++;
            }
            Clients.All.updateVotesCounter(CurrentVotesCount);
            Trace.TraceInformation("SendDriveCommand: " + command.ToString());
        }

        public override Task OnConnected()
        {
            Clients.All.updateVotesCounter(CurrentVotesCount);
            return base.OnConnected();
        }

    }
}