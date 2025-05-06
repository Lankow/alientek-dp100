using HidSharp;
using System;

namespace AlientekTest
{
    public class AlientekDP100
    {
        private const string ProductName = "ATK-MDP100";
        private const int VendorId = 0x2e3c;

        private readonly HidDevice _device;
        private readonly HidStream _stream;

        public AlientekDP100()
        {
            var devices = DeviceList.Local.GetHidDevices();
            var deviceList = DeviceList.Local;

            _device = deviceList.GetHidDeviceOrNull(vendorID: VendorId);

            foreach (var device in devices)
            {
                if (device.GetProductName() == ProductName && device.VendorID == VendorId)
                {
                    Console.WriteLine($"Found {device.GetProductName()}, Vendor ID: {device.VendorID}, Product ID: {device.ProductID}");
                    _device = device;
                    break;
                }
            }

            if (_device == null && !_device.TryOpen(out _stream))
            {
                throw new InvalidOperationException($"Unable to open {ProductName} device.");
            }

            Console.WriteLine(_stream == null);
        }

        public void GetBasicInfo()
        {
            var frame = new Frame
            {
                DeviceAddress = 251,
                FunctionType = FrameFunctionType.FRAME_BASIC_INFO,
                Sequence = 0,
                DataLen = 0,
                Data = Array.Empty<byte>()
            };
        }

        private Frame SendFrameForResponse()
        {
            return new Frame();
        }

        private void SendFrame(Frame frame)
        {

        }
    }
}
