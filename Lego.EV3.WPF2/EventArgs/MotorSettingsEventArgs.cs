using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lego.EV3
{
    public class MotorSettingsEventArgs : EventArgs
    {
        public MotorMovementTypes MotorMovementType { get; set; }
        public int DegreeMovement { get; set; }
        public int TimeToMoveInSeconds { get; set; }
        public int PowerRatingMovement { get; set; }
    }
}
