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
        private const int DeviceAddress = 251;

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

            if (_device == null)
            {
                throw new InvalidOperationException($"Unable to open {ProductName} device.");
            }

            _device.TryOpen(out _stream);
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
            current= float.NaN;

            return true;
        }

        public void SetState(bool state)
        {
            var basicSet = GetBasicInfo().GetAwaiter().GetResult();
        }

        public void SetVoltage(bool state)
        {

        }

        public void SetCurrentLimit(bool state)
        {

        }

        public void SetDefault(bool outState, float voltage, float current)
        {

        }

        private async Task<BasicInfo> GetBasicInfo()
        {
            var frame = new Frame
            {
                DeviceAddress = DeviceAddress,
                FunctionType = FrameFunctionType.FRAME_BASIC_INFO,
                Sequence = 0,
                DataLen = 0,
                Data = Array.Empty<byte>()
            };

            var response = await WriteFrameAwaitResponse(frame, FrameFunctionType.FRAME_BASIC_INFO);

            if (response != null) return BasicInfo.FromFrame(response);

            return null;
        }

        private async Task<BasicSet> GetBasicSet()
        {
            var frameData = new byte[] { (byte)(0x80) };
            var frame = new Frame
            {
                DeviceAddress = DeviceAddress,
                FunctionType = FrameFunctionType.FRAME_BASIC_SET,
                Sequence = 0,
                DataLen = (byte)frameData.Length,
                Data = frameData
            };

            var response = await WriteFrameAwaitResponse(frame, FrameFunctionType.FRAME_BASIC_SET);

            return BasicSet.FromFrame(response);
        }


        private async Task<bool> SetBasic(BasicSet basicSet)
        {
            var copy = new BasicSet
            {
                Index = (byte)(basicSet.Index | 0x20),
                State = basicSet.State,
                VoSet = basicSet.VoSet,
                IoSet = basicSet.IoSet,
                OvpSet = basicSet.OvpSet,
                OcpSet = basicSet.OcpSet
            };

            var frameData = copy.ToByteArray();
            var frame = new Frame
            {
                DeviceAddress = DeviceAddress,
                FunctionType = FrameFunctionType.FRAME_BASIC_SET,
                Sequence = 0,
                DataLen = (byte)frameData.Length,
                Data = frameData
            };

            var response = await WriteFrameAwaitResponse(frame, FrameFunctionType.FRAME_BASIC_SET);
            return response.Data[0] == 1;
        }

        private async Task<Frame> WriteFrameAwaitResponse(Frame frame, FrameFunctionType expectedFrameFunctionType)
        {
            await WriteFrame(frame);
            var response = await ReadFrame(expectedFrameFunctionType);

            return response;
        }

        private async Task WriteFrame(Frame frame)
        {
            // TODO: CRC Calculation is done wrong. Fix it. 
            var frameBuffer = FrameParser.ToByteArray(frame);
            //var frameBuffer = new byte[] { 0, 251, 48, 0, 0, 49, 15 };
            Console.WriteLine($"WRITE FRAME: {BitConverter.ToString(frameBuffer)}");

            await _stream.WriteAsync(frameBuffer, 0, frameBuffer.Length);
        }

        private async Task<Frame> ReadFrame(FrameFunctionType expectedFrameFunctionType)
        {
            var frameBuffer = new byte[_device.GetMaxInputReportLength()];
            var count = await _stream.ReadAsync(frameBuffer, 0, frameBuffer.Length);
            Console.WriteLine($"READ FRAME: {BitConverter.ToString(frameBuffer)}");

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
