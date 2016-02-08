using Lego.EV3.Core;
using Lego.EV3.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Lego.EV3.WP.Plebiscite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        LegoWrapper _lego = new LegoWrapper();

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            _lego.BrickChanged += _lego_BrickChanged;
            await _lego.Connect();
        }

        void _lego_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            txtDistance.Text = e.Ports[InputPort.Two].SIValue.ToString();
            txtTouch.Text = e.Ports[InputPort.Four].SIValue.ToString();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var name = ((Button)sender).Name;
            DriveCommand cmd = (DriveCommand)Enum.Parse(typeof(DriveCommand), name);
            await _lego.SendCommand(cmd);

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
    }
}
