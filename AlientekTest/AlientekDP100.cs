using HidSharp;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlientekTest
{
    public class AlientekDP100
    {
        private const string ProductName = "ATK-MDP100";
        private const int VendorId = 0x2e3c;
        private const int DeviceAddress = 251;

        private readonly HidDevice _device;
        private HidStream _stream;

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

            if (_device == null)
            {
                throw new InvalidOperationException($"Unable to open {ProductName} device.");
            }

            _device.TryOpen(out _stream);
        }

        public async Task<BasicInfo> GetBasicInfo()
        {

            var frame = new Frame
            {
                DeviceAddress = DeviceAddress,
                FunctionType = FrameFunctionType.FRAME_BASIC_INFO,
                Sequence = 0,
                DataLen = 0,
                Data = Array.Empty<byte>()
            };

            await WriteFrame(frame);

            var response = await ReadFrame(FrameFunctionType.FRAME_BASIC_INFO);

            if (response != null) return BasicInfo.FromFrame(response);

            return null;
        }

        private async Task WriteFrame(Frame frame)
        {
            var frameBuffer = FrameParser.ToByteArray(frame);
            await _stream.WriteAsync(frameBuffer, 0, frameBuffer.Length);
        }

        private async Task<Frame> ReadFrame(FrameFunctionType expectedFrameFunctionType)
        {
            var frameBuffer = new byte[_device.GetMaxInputReportLength()];
            var count = await _stream.ReadAsync(frameBuffer, 0, frameBuffer.Length);

            if (count > 0)
            {
                var frame = FrameParser.FromByteArray(frameBuffer.Skip(1).ToArray()); // skip report ID
                if (frame != null && frame.FunctionType == expectedFrameFunctionType)
                {
                    return frame;
                }
            }

            return null;
        }
    }
}
