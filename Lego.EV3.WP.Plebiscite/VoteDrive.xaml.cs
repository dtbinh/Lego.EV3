using Lego.EV3.WP.Plebiscite.Common;
using System;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.AspNet.SignalR.Client;
using Lego.EV3.Shared;
using Lego.EV3.Core;
using System.Diagnostics;
using Microsoft.WindowsAzure.Messaging;
using System.Text;
using System.IO;
using Windows.Data.Xml.Dom;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Lego.EV3.WP.Plebiscite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VoteDrive : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        //SignalR Hub proxy
        private IHubProxy _hubProxy;
        private LegoWrapper _lego = new LegoWrapper();
        private int _distance;


        public VoteDrive()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Ya no consumimos con SignalR

            ////Sets up connection and events from SignalR server
            //HubConnection connection = new HubConnection(App.LegoAPIUrl);
            //_hubProxy = connection.CreateHubProxy("VoteDriveHub");
            //_hubProxy.On<DriveCommand>("commandToRun", RunCommand);
            //await connection.Start();
            try
            {

                var topic = new Topic("legotopic", App.connectionTopicString);
                var subs= new Subscription(topic.Path, "legosubs", App.connectionTopicString);
                subs.OnMessage((m) => {
                    try {
                        System.Runtime.Serialization.Json.DataContractJsonSerializer s;
                        var input = m.GetBody<string>();
                     //   var bytes = m.GetBody<Stream>();
                        //string s;
                        //using (var reader = new StreamReader(bytes, Encoding.UTF8))
                        //{
                        //    s = reader.ReadToEnd();
                        //}
                        //var s = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                        //System.Diagnostics.Debug.WriteLine(m);
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });

            }
            catch (Exception ex)
            {

                throw;
            }
       
            
                //_lego.BrickChanged += _lego_BrickChanged;
            //await _lego.Connect();



        }

        async void _lego_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            try
            {
                //Only send if >= 2 centimeter change
                if (Math.Abs(_distance - (int)e.Ports[InputPort.Four].SIValue) >= 2)
                {
                    _distance = (int)e.Ports[InputPort.Four].SIValue;

                    //Send the new distance to the SignalRHub 
                    await _hubProxy.Invoke("SendUpdatedSensorData", _distance);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("exception:" + ex);
            }
        }

        private void RunCommand(DriveCommand command)
        {     
        
            Action action = async () =>            {

                txtCommand.Text = command.ToString();
                await _lego.SendCommand(command);
            };



        }
    }
}
