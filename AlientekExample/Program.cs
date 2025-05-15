using Alientek_DP100;
using System;

namespace AlientekExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AlientekDP100 alientek = new AlientekDP100();
            alientek.Connect();
            alientek.SetVoltage(3.0f);
            alientek.SetCurrentLimit(2.2f);
            alientek.SetState(true);

            alientek.GetVoltageCurrent(out float voltage, out float current);
            Console.WriteLine($"Voltage: {voltage}, Current: {current}");

            alientek.Disconnect();
        }
    }
}
