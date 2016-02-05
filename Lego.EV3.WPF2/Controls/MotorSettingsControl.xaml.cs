using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Lego.EV3.Core;

namespace Lego.EV3.WPF2.Controls
{
    /// <summary>
    /// Interaction logic for MotorSettingsControl.xaml
    /// </summary>
    public partial class MotorSettingsControl : UserControl
    {
        public event EventHandler<MotorSettingsEventArgs> SaveSettings;

        public MotorSettingsControl()
        {
            InitializeComponent();
        }

        private void RoundButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveSettings != null)
            {
                SaveSettings.Invoke(sender, new MotorSettingsEventArgs
                {
                    MotorMovementType = (MotorMovementTypes)MovementStyle.SelectedIndex,

                    DegreeMovement = (int)DegreeSlider.Value,
                    TimeToMoveInSeconds = (int)PowerTimerSlider.Value,
                    PowerRatingMovement = (int)PowerSlider.Value,
                });
            }

            Visibility = Visibility.Collapsed;
        }

        public void Show(InputPort port, DeviceType deviceType, MotorMovementTypes movementType, int degreeMovement, int powerRatingMovement, int timeToMoveInSeconds)
        {
            PortName.Text = port.ToString();
            ObjectName.Text = deviceType.ToString();

            MovementStyle.SelectedIndex = (int)movementType;
            DegreeSlider.Value = degreeMovement;

            PowerTimerSlider.Value = timeToMoveInSeconds;
            PowerSlider.Value = powerRatingMovement;

            Visibility = Visibility.Visible;
        }
    }
}
