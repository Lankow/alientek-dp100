using System;

namespace AlientekTest
{
    public static class FrameParser
    {
        public static byte[] FrameToBytes(Frame frame)
        {
            int totalLength = 4 + frame.DataLen + 2;
            byte[] buffer = new byte[totalLength];

            buffer[0] = frame.DeviceAddress;
            buffer[1] = (byte)frame.FunctionType;
            buffer[2] = frame.Sequence;
            buffer[3] = frame.DataLen;

            if (frame.Data != null && frame.Data.Length > 0)
            {
                Array.Copy(frame.Data, 0, buffer, 4, frame.Data.Length);
            }

            ushort crc = Crc16Modbus(buffer, 0, 4 + frame.DataLen);

            buffer[4 + frame.DataLen] = (byte)(crc & 0xFF);
            buffer[5 + frame.DataLen] = (byte)((crc >> 8) & 0xFF);

            return buffer;
        }

        public static ushort Crc16Modbus(byte[] data, int offset, int length)
        {
            const ushort polynomial = 0xA001;
            ushort crc = 0xFFFF;

            for (int i = offset; i < offset + length; i++)
            {
                crc ^= data[i];
                for (int j = 0; j < 8; j++)
                {
                    bool lsb = (crc & 0x0001) != 0;
                    crc >>= 1;
                    if (lsb)
                        crc ^= polynomial;
                }
            }
            return crc;
        }

        internal static Frame ParseInputReport(byte[] rawData)
        {
            if (rawData == null || rawData.Length < 6)
                throw new ArgumentException("Invalid raw frame data");

            byte deviceAddress = rawData[0];
            byte functionType = rawData[1];
            byte sequence = rawData[2];
            byte dataLen = rawData[3];

            if (rawData.Length != 4 + dataLen + 2)
                throw new ArgumentException("Incorrect frame length");

            byte[] data = new byte[dataLen];
            if (dataLen > 0)
            {
                Array.Copy(rawData, 4, data, 0, dataLen);
            }

            ushort receivedCrc = (ushort)(rawData[4 + dataLen] | (rawData[5 + dataLen] << 8));
            ushort computedCrc = Crc16Modbus(rawData, 0, 4 + dataLen);

            if (receivedCrc != computedCrc)
                throw new InvalidOperationException($"CRC mismatch: received 0x{receivedCrc:X4}, computed 0x{computedCrc:X4}");

            return new Frame
            {
                DeviceAddress = deviceAddress,
                FunctionType = (FrameFunctionType)functionType,
                Sequence = sequence,
                DataLen = dataLen,
                Data = data
            };
        }
    }

}
