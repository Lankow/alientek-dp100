using HidSharp;
using System;
using System.Linq;

namespace Alientek_DP100
{
    /// <summary>
    /// Represents a driver for the Alientek DP100 programmable power supply.
    /// </summary>
    public class AlientekDP100
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

        /// <summary>
        /// Static constructor for the <see cref="AlientekDP100"/> class.
        /// Initializes the embedded assembly resolver to allow loading of dependencies
        /// directly from embedded resources within the assembly.
        /// </summary>
        static AlientekDP100()
        {
            EmbeddedAssemblyLoader.Attach();
        }

        /// <summary>
        /// Connects to the Alientek DP100 device via USB HID.
        /// </summary>
        /// <returns><c>true</c> if the device is successfully connected; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Disconnects from the Alientek DP100 device and releases associated resources.
        /// </summary>
        public void Disconnect()
        {
            _stream?.Close();
            _stream?.Dispose();
            _stream = null;
            _device = null;
            _isConnected = false;
        }

        /// <summary>
        /// Gets the current voltage and current output of the device.
        /// </summary>
        /// <param name="voltage">The voltage output in volts.</param>
        /// <param name="current">The current output in amperes.</param>
        /// <returns><c>true</c> if successful; otherwise, <c>false</c>.</returns>
        public bool GetOutputVoltageCurrent(out float voltage, out float current)
        {
            voltage = float.NaN;
            current = float.NaN;

            var basicInfo = GetBasicInfo();

            if (basicInfo == null) return false;

            voltage = (float)basicInfo.Vout / VoltageCurrentScaler;
            current = (float)basicInfo.Iout / VoltageCurrentScaler;

            return true;
        }

        /// <summary>
        /// Gets the current voltage and current input of the device.
        /// </summary>
        /// <param name="voltage">The voltage input in volts.</param>
        /// <param name="current">The current input in amperes.</param>
        /// <returns><c>true</c> if successful; otherwise, <c>false</c>.</returns>
        public bool GetInputVoltageCurrent(out float voltage, out float current)
        {
            voltage = float.NaN;
            current = float.NaN;

            var basicSet = GetBasicSet();

            if (basicSet == null) return false;

            voltage = (float)basicSet.VoSet / VoltageCurrentScaler;
            current = (float)basicSet.IoSet / VoltageCurrentScaler;

            return true;
        }

        /// <summary>
        /// Turns the power output of the device on or off.
        /// </summary>
        /// <param name="state"><c>true</c> to turn on; <c>false</c> to turn off.</param>
        public void SetState(bool state)
        {
            var basicSet = GetBasicSet();
            basicSet.State = state ? StateOn : StateOff;

            SetBasic(basicSet);
        }

        /// <summary>
        /// Sets the output voltage of the device.
        /// </summary>
        /// <param name="voltage">The voltage in volts.</param>
        public void SetVoltage(float voltage)
        {
            var basicSet = GetBasicSet();
            basicSet.VoSet = (ushort)(voltage * VoltageCurrentScaler);

            SetBasic(basicSet);
        }

        /// <summary>
        /// Sets the current limit of the device.
        /// </summary>
        /// <param name="voltage">The current limit in amperes.</param>
        public void SetCurrentLimit(float current)
        {
            var basicSet = GetBasicSet();
            basicSet.IoSet = (ushort)(current * VoltageCurrentScaler);

            SetBasic(basicSet);
        }

        /// <summary>
        /// Retrieves basic information from the device such as voltage and current output.
        /// </summary>
        /// <returns>A <see cref="BasicInfo"/> object, or <c>null</c> if the operation fails.</returns>
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
            return response != null ? BasicInfo.FromFrame(response) : null;
        }

        /// <summary>
        /// Retrieves the current basic settings from the device.
        /// </summary>
        /// <returns>A <see cref="BasicSet"/> object, or <c>null</c> if the operation fails.</returns>
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
            return response != null ? BasicSet.FromFrame(response) : null;
        }

        /// <summary>
        /// Sends updated settings to the device.
        /// </summary>
        /// <param name="basicSet">The settings to apply.</param>
        /// <returns><c>true</c> if the settings were applied successfully; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Sends a frame to the device and waits for a corresponding response.
        /// </summary>
        /// <param name="frame">The frame to send.</param>
        /// <param name="expectedFrameFunctionType">The type of response expected.</param>
        /// <returns>The response frame, or <c>null</c> if none received or mismatched type.</returns>
        private Frame WriteFrameAwaitResponse(Frame frame, FrameFunctionType expectedFrameFunctionType)
        {
            WriteFrame(frame);
            return ReadFrame(expectedFrameFunctionType);
        }

        /// <summary>
        /// Sends a frame to the device.
        /// </summary>
        /// <param name="frame">The frame to send.</param>
        private void WriteFrame(Frame frame)
        {
            if (!_isConnected || _stream == null) return;

            var frameBuffer = FrameParser.ToByteArray(frame);
            _stream.Write(frameBuffer, 0, frameBuffer.Length);
        }

        /// <summary>
        /// Reads a response frame from the device.
        /// </summary>
        /// <param name="expectedFrameFunctionType">The type of frame expected.</param>
        /// <returns>The received frame, or <c>null</c> if none or if the type does not match.</returns>
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
    }
}
