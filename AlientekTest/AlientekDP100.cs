using Device.Net;
using HidSharp;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public bool Connect()
        {
            return true;
        }
        public void Disconnect()
        {
        }

        public bool GetVoltageCurrent(out float voltage, out float current)
        {
            voltage = float.NaN;
            current = float.NaN;

            return true;
        }

        public void SetState(bool state)
        {

        }

        public void SetVoltage(float voltage)
        {

        }

        public void GetBasicInfo()
        {
            if (_device.TryOpen(out HidStream stream))
            {
                var frame = new Frame
                {
                    DeviceAddress = 251,
                    FunctionType = FrameFunctionType.FRAME_BASIC_INFO,
                    Sequence = 0,
                    DataLen = 0,
                    Data = Array.Empty<byte>()
                };

                var buffer = FrameParser.FrameToBytes(frame);

                stream.Write(buffer);
                Console.WriteLine("Data written to HID device.");

                int inputReportLength = _device.GetMaxInputReportLength();
                byte[] inputReport = new byte[inputReportLength];

                try
                {
                    int bytesRead = stream.Read(inputReport, 0, inputReport.Length);
                    Console.WriteLine($"Received {bytesRead} bytes: {BitConverter.ToString(inputReport)}");
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Timed out waiting for response.");
                }

                stream.Close();
            }
            else
            {
                Console.WriteLine("Could not open HID stream.");
            }
        }
    }
}
