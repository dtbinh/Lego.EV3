using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Lego.EV3.WP.Core;
using Lego.EV3.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Lego.EV3.WP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {
        private Brick _brick;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }


        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _brick = new Brick(new BluetoothCommunication());
                //_brick = new Brick(new NetworkCommunication("192.168.2.237"));
                _brick.BrickChanged += brick_BrickChanged;

                //Conect
                //await _brick.ConnectAsync();
                //Connected
                //Output.Text = "Connected";
            }
            catch (Exception ex)
            {
                //Error message
                //MessageBox.Show("Failed to connect: " + ex);
            }
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
        }



        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        void brick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
          //  InputPorts.DataContext = e.Ports;
        }
    }
}
