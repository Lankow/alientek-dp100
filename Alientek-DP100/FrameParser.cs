using System;

namespace Alientek_DP100
{
    /// <summary>
    /// Provides functionality to serialize and deserialize communication frames for the Alientek DP100 device.
    /// </summary>
    public static class FrameParser
    {
        /// <summary>
        /// Parses a byte array into a <see cref="Frame"/> object, verifying data integrity via CRC-16 (Modbus).
        /// </summary>
        /// <param name="rawData">The raw byte array representing a received frame.</param>
        /// <returns>A <see cref="Frame"/> object containing the parsed data.</returns>
        /// <exception cref="ArgumentException">Thrown if the input data is null or too short.</exception>
        /// <exception cref="InvalidOperationException">Thrown if CRC validation fails.</exception>
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

        /// <summary>
        /// Converts a <see cref="Frame"/> object into a byte array suitable for transmission,
        /// including a CRC-16 checksum and a report ID prefix.
        /// </summary>
        /// <param name="frame">The <see cref="Frame"/> to serialize.</param>
        /// <returns>A byte array representing the frame with CRC and header.</returns>
        public static byte[] ToByteArray(Frame frame)
        {
            byte[] frameBuffer = new byte[4 + frame.Data.Length + 2 + 1];

            frameBuffer[0] = 0x00; // Report ID (0x00 for HID raw transfer)
            frameBuffer[1] = frame.DeviceAddress;
            frameBuffer[2] = (byte)frame.FunctionType;
            frameBuffer[3] = frame.Sequence;
            frameBuffer[4] = frame.DataLen;

            Buffer.BlockCopy(frame.Data, 0, frameBuffer, 5, frame.Data.Length);
            var checksum = Crc16Modbus(frameBuffer, 1, frameBuffer.Length - 3);

            Utils.WriteUInt16(frameBuffer, frameBuffer.Length - 2, checksum);

            return frameBuffer;
        }

        /// <summary>
        /// Computes the CRC-16 checksum using the Modbus polynomial (0xA001).
        /// </summary>
        /// <param name="data">The byte array to compute the CRC on.</param>
        /// <param name="offset">The offset in the array to begin CRC computation.</param>
        /// <param name="length">The number of bytes to include in the computation.</param>
        /// <returns>The computed CRC-16 value.</returns>
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
