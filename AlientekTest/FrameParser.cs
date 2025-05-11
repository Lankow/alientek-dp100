using System;

namespace AlientekTest
{
    public static class FrameParser
    {
        public static Frame FromByteArray(byte[] rawData)
        {
            if (rawData == null || rawData.Length < 6)
                throw new ArgumentException("Invalid raw frame data");

            byte deviceAddress = rawData[0];
            byte functionType = rawData[1];
            byte sequence = rawData[2];
            byte dataLen = rawData[3];


            byte[] data = new byte[dataLen];
            if (dataLen > 0)
            {
                Array.Copy(rawData, 4, data, 0, dataLen);
            }

            ushort receivedCrc = Utils.ReadUInt16(rawData, 4 + dataLen);
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

        public static byte[] ToByteArray(Frame frame)
        {
            byte[] frameBuffer = new byte[4 + frame.Data.Length + 2 + 1];

            frameBuffer[0] = 0x00; // TODO: Handle Report ID
            frameBuffer[1] = frame.DeviceAddress;
            frameBuffer[2] = (byte)frame.FunctionType;
            frameBuffer[3] = frame.Sequence;
            frameBuffer[4] = frame.DataLen;

            Buffer.BlockCopy(frame.Data, 0, frameBuffer, 5, frame.Data.Length);
            var checksum = Crc16Modbus(frameBuffer, 1, frameBuffer.Length - 2);

            Utils.WriteUInt16(frameBuffer, frameBuffer.Length - 2, checksum);

            return frameBuffer;
        }

        private static ushort Crc16Modbus(byte[] data, int offset, int length)
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
    }

}
