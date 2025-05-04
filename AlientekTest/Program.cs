using System;

namespace AlientekTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AlientekDP100 alientek = new AlientekDP100();

            Console.WriteLine("START");
            alientek.GetBasicInfo();
            Console.WriteLine("STOP");
        }
    }
}
