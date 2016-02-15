using Lego.EV3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;
using Microsoft.ServiceBus;
using System.Threading;

namespace Lego.EV3.ConsoleTest
{
    class Program
    {
        static Brick _brick;
        static void Main(string[] args) {

            Test2();

            //Test3();

            //Task t = Test();
            //t.Wait();
            //System.Console.ReadKey();

           


        }


        static async Task Test()
        {
            //_brick = new Brick(new UsbCommunication());
            _brick = new Brick(new BluetoothCommunication("COM6"));
            //_brick = new Brick(new NetworkCommunication("192.168.2.237"));

            _brick.BrickChanged += _brick_BrickChanged;

            System.Console.WriteLine("Connecting...");
            await _brick.ConnectAsync();

            // volume (0-100), frequency, duration
            await _brick.DirectCommand.PlayToneAsync(100, 40000, 300);

            //System.Console.WriteLine("Connected...Turning motor...");

            

            ////mueve la rueda derecha
            //await _brick.DirectCommand.TurnMotorAtSpeedForTimeAsync(OutputPort.C, 0x50, 1000, false);

            //System.Console.WriteLine("Motor turned...beeping...");
            //await _brick.DirectCommand.PlayToneAsync(100, 50000, 500);

            //System.Console.WriteLine("Beeped...done!");

            System.Console.WriteLine("Getting result...");

            //Timer tmr = new Timer();           
            //tmr.Interval = 3000; // 6 second
            //tmr.Elapsed += Tmr_Elapsed;
            //tmr.Start(); // The countdown is launched!


           


        }

        private static async void Tmr_Elapsed(object sender, ElapsedEventArgs e)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://legoev3api.azurewebsites.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



                var response = await client.GetAsync("api/values");
                if (response.IsSuccessStatusCode)
                {
                    var movement = await response.Content.ReadAsAsync<int>();

                    if (movement == 1)
                    {
                        await _brick.DirectCommand.TurnMotorAtSpeedForTimeAsync(OutputPort.B, 0x25, 1000, false);
                        await _brick.DirectCommand.TurnMotorAtSpeedForTimeAsync(OutputPort.C, 0x25, 1000, false);
                    }
                    else if (movement == 2)
                    {
                        await _brick.DirectCommand.TurnMotorAtSpeedForTimeAsync(OutputPort.B, 0x25, 1000, false);
                    }
                    else if (movement == 4)
                    {
                        await _brick.DirectCommand.TurnMotorAtSpeedForTimeAsync(OutputPort.C, 0x25, 1000, false);
                    }
                    else if (movement == 3)
                    {
                        await _brick.DirectCommand.TurnMotorAtSpeedForTimeAsync(OutputPort.B, -0x25, 1000, false);
                        await _brick.DirectCommand.TurnMotorAtSpeedForTimeAsync(OutputPort.C, -0x25, 1000, false);
                    }
                }



            }
        }


        static void _brick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            System.Console.WriteLine(e.Ports[InputPort.Four].SIValue);
        }

        static  void Test2() {

            try
            {
                CancellationTokenSource source = new CancellationTokenSource();
                string connectionString = ConfigurationManager.AppSettings["TopicConnectionString"];
            
                 var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);


                if (!namespaceManager.TopicExists("legotopic"))
                {
                    namespaceManager.CreateTopic("legotopic");
                }

                ReceiveMessage(connectionString, "legotopic", "legosubs", source.Token);

                Console.ReadKey();
                source.Cancel();
                Console.ReadKey();
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.WriteLine($"{ex.Message}");
            }
    
        }

        static public async void ReceiveMessage(string connectionString, string topicPath, string subscriptionName, CancellationToken token )
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var subscriptionClient = SubscriptionClient.CreateFromConnectionString(connectionString, topicPath, subscriptionName);
                    var brokeredMessage = await subscriptionClient.ReceiveAsync(TimeSpan.FromSeconds(1));
                    if (brokeredMessage != null)
                    {
                        var message = brokeredMessage.GetBody<string>();
                        Console.WriteLine(message);
                        await brokeredMessage.CompleteAsync();
                        //var message = brokeredMessage.GetBody<TestMessage>();
                    }
                }
                catch (Exception ex)
                {

                    System.Diagnostics.Trace.WriteLine($"{ex.Message}");
                }
            }            
            
        }
    }
}
