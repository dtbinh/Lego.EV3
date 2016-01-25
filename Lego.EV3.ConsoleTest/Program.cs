using Lego.EV3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lego.EV3.ConsoleTest
{
    class Program
    {
        static Brick _brick;
        static void Main(string[] args) {

            Task t = Test();
            t.Wait();
            System.Console.ReadKey();

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

            System.Console.WriteLine("Connected...Turning motor...");

            //mueve la rueda izquierda
            await _brick.DirectCommand.TurnMotorAtSpeedForTimeAsync(OutputPort.B, 0x25, 1000, false);

            //mueve la rueda derecha
            await _brick.DirectCommand.TurnMotorAtSpeedForTimeAsync(OutputPort.C, 0x50, 1000, false);

            System.Console.WriteLine("Motor turned...beeping...");
            await _brick.DirectCommand.PlayToneAsync(100, 50000, 500);

            System.Console.WriteLine("Beeped...done!");

        }

        static void _brick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            System.Console.WriteLine(e.Ports[InputPort.Four].SIValue);
        }
    }
}
