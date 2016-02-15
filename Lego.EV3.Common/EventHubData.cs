using System;


namespace Lego.EV3.Shared
{
    public class EventHubData
    {
        public int Movement { get; set; }
        public DateTime EntryTime { get; set; }
    }


    public class EventHubVotes
    {
 
        public DriveCommand Movement { get; set; }
   
        public int Votes { get; set; }

    }
}
