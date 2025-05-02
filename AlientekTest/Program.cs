using HidSharp;
using System;

namespace AlientekTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var devices = DeviceList.Local.GetHidDevices();
            var productName = "ATK-MDP100";
            var vendorId = 0x2e3c;

            foreach (var device in devices)
            {
                if (device.GetProductName() == productName && device.VendorID == vendorId)
                {
                    Console.WriteLine($"Found: {device.GetProductName()}, Vendor: {device.VendorID}, Product: {device.ProductID}");
                }
            }

        }
    }
}
