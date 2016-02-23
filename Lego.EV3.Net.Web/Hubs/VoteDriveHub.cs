using Lego.EV3.Shared;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Timers;

namespace Lego.EV3.Net.Web.Hubs
{
    public class VoteDriveHub: Hub
    {
        private VoteCounter instance;

        public VoteDriveHub() : this(VoteCounter.Instance) { }

        public VoteDriveHub(VoteCounter instance)
        {
            this.instance = instance;
        }

        public void SendDriveCommand(DriveCommand command)
        {
            instance.UpdateVoteCounter(command);
            Trace.TraceInformation("SendDriveCommand: " + command.ToString());
        }

        public override Task OnConnected()
        {
            Clients.All.updateVotesCounter(instance.CurrentVotesCount);
            return base.OnConnected();
        }

    }
}