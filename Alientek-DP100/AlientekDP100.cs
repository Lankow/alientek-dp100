using HidSharp;
using System;
using System.Linq;

namespace Alientek_DP100
{
    public class AlientekDP100 : IDisposable
    {
        private const string ProductName = "ATK-MDP100";

        private const int VendorId = 0x2e3c;
        private const int DeviceAddress = 251;

        private const byte ReportIdOffset = 1;
        private const byte StateOn = 0x01;
        private const byte StateOff = 0x00;
        private const byte IndexWriteFlag = 0x20;

        private const int VoltageCurrentScaler = 1000;

        private readonly byte[] DefaultBasicSetData = { 0x80 };


        private HidDevice _device;
        private HidStream _stream;

        private bool _isConnected = false;

        public bool Connect()
        {
            var devices = DeviceList.Local.GetHidDevices();

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
                return false;
            }

            _isConnected = _device.TryOpen(out _stream);
            return _isConnected;
        }

        public void Disconnect()
        {
            _stream?.Close();
            _stream?.Dispose();
            _stream = null;
            _device = null;
            _isConnected = false;
        }

        public bool GetVoltageCurrent(out float voltage, out float current)
        {
            voltage = float.NaN;
            current = float.NaN;

            var basicInfo = GetBasicInfo();

            if (basicInfo == null) return false;

            voltage = (float)basicInfo.Vout / VoltageCurrentScaler;
            current = (float)basicInfo.Iout / VoltageCurrentScaler;

            return true;
        }

        public void SetState(bool state)
        {
            var basicSet = GetBasicSet();
            basicSet.State = state ? StateOn : StateOff;

            SetBasic(basicSet);
        }

        public void SetVoltage(float voltage)
        {
            var basicSet = GetBasicSet();
            basicSet.VoSet = (ushort)(voltage * VoltageCurrentScaler);

            SetBasic(basicSet);
        }

        public void SetCurrentLimit(float voltage)
        {
            var basicSet = GetBasicSet();
            basicSet.IoSet = (ushort)(voltage * VoltageCurrentScaler);

            SetBasic(basicSet);
        }

        private BasicInfo GetBasicInfo()
        {
            var frame = new Frame
            {
                DeviceAddress = DeviceAddress,
                FunctionType = FrameFunctionType.FRAME_BASIC_INFO,
                Sequence = 0,
                DataLen = 0,
                Data = Array.Empty<byte>()
            };

            var response = WriteFrameAwaitResponse(frame, FrameFunctionType.FRAME_BASIC_INFO);

            if (response != null) return BasicInfo.FromFrame(response);

            return null;
        }

        private BasicSet GetBasicSet()
        {
            var frame = new Frame
            {
                DeviceAddress = DeviceAddress,
                FunctionType = FrameFunctionType.FRAME_BASIC_SET,
                Sequence = 0,
                DataLen = (byte)DefaultBasicSetData.Length,
                Data = DefaultBasicSetData
            };

            var response = WriteFrameAwaitResponse(frame, FrameFunctionType.FRAME_BASIC_SET);

            return BasicSet.FromFrame(response);
        }

        private bool SetBasic(BasicSet basicSet)
        {
            var copy = new BasicSet
            {
                Index = (byte)(basicSet.Index | IndexWriteFlag),
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

            var response = WriteFrameAwaitResponse(frame, FrameFunctionType.FRAME_BASIC_SET);
            return response.Data[0] == 1;
        }

        private Frame WriteFrameAwaitResponse(Frame frame, FrameFunctionType expectedFrameFunctionType)
        {
            WriteFrame(frame);
            var response = ReadFrame(expectedFrameFunctionType);

            return response;
        }

        private void WriteFrame(Frame frame)
        {
            if (!_isConnected || _stream == null) return;

            var frameBuffer = FrameParser.ToByteArray(frame);

            _stream.Write(frameBuffer, 0, frameBuffer.Length);
        }

        private Frame ReadFrame(FrameFunctionType expectedFrameFunctionType)
        {
            if (!_isConnected || _stream == null) return null;

            var frameBuffer = new byte[_device.GetMaxInputReportLength()];
            var count = _stream.Read(frameBuffer, 0, frameBuffer.Length);

            if (count > 0)
            {
                var frame = FrameParser.FromByteArray(frameBuffer.Skip(ReportIdOffset).ToArray());
                if (frame != null && frame.FunctionType == expectedFrameFunctionType)
                {
                    return frame;
                }
            }

            return null;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
