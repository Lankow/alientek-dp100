using System;

namespace Alientek_DP100
{
    /// <summary>
    /// Provides utility methods.
    /// </summary>
    internal class Utils
    {
        /// <summary>
        /// Reads a 16-bit unsigned integer (UInt16) from a byte array starting at the specified offset.
        /// </summary>
        /// <param name="data">The byte array containing the data.</param>
        /// <param name="offset">The starting index from which to read the 16-bit value.</param>
        /// <returns>The 16-bit unsigned integer read from the byte array.</returns>
        public static ushort ReadUInt16(byte[] data, int offset)
        {
            return BitConverter.ToUInt16(data, offset);
        }

        /// <summary>
        /// Writes a 16-bit unsigned integer (UInt16) to a byte array starting at the specified offset in little-endian format.
        /// </summary>
        /// <param name="data">The byte array to write to.</param>
        /// <param name="offset">The starting index at which to write the 16-bit value.</param>
        /// <param name="value">The 16-bit unsigned integer to write.</param>
        public static void WriteUInt16(byte[] data, int offset, ushort value)
        {
            data[offset] = (byte)(value & 0xFF);               // Low byte
            data[offset + 1] = (byte)((value >> 8) & 0xFF);     // High byte
        }
    }
}
