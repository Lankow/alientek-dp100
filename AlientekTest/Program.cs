using System;

namespace AlientekTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AlientekDP100 alientek = new AlientekDP100();
            var basicInfo = alientek.GetBasicInfo().Result;

            if (basicInfo != null)
            {
                Console.WriteLine("Yes");
            }
            else
            {
                Console.WriteLine("No");
            }
        }
    }
}
