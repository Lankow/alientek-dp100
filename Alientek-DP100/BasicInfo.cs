using System;

namespace Alientek_DP100
{
    /// <summary>
    /// Represents real-time information retrieved from the Alientek DP100 device.
    /// </summary>
    public class BasicInfo
    {
        /// <summary>
        /// Gets or sets the input voltage in millivolts.
        /// </summary>
        public ushort Vin { get; set; }

        /// <summary>
        /// Gets or sets the output voltage in millivolts.
        /// </summary>
        public ushort Vout { get; set; }

        /// <summary>
        /// Gets or sets the output current in milliamperes.
        /// </summary>
        public ushort Iout { get; set; }

        /// <summary>
        /// Gets or sets the maximum output voltage setting in millivolts.
        /// </summary>
        public ushort VoMax { get; set; }

        /// <summary>
        /// Gets or sets the temperature reading from sensor 1.
        /// </summary>
        public ushort Temp1 { get; set; }

        /// <summary>
        /// Gets or sets the temperature reading from sensor 2.
        /// </summary>
        public ushort Temp2 { get; set; }

        /// <summary>
        /// Gets or sets the internal 5V DC rail voltage in millivolts.
        /// </summary>
        public ushort Dc5V { get; set; }

        /// <summary>
        /// Gets or sets the output mode (e.g., constant voltage/current).
        /// </summary>
        public byte OutMode { get; set; }

        /// <summary>
        /// Gets or sets the device working state or status code.
        /// </summary>
        public byte WorkSt { get; set; }

        /// <summary>
        /// Creates a <see cref="BasicInfo"/> instance from a raw device frame.
        /// </summary>
        /// <param name="frame">The frame received from the device.</param>
        /// <returns>A <see cref="BasicInfo"/> object populated with the parsed values.</returns>
        /// <exception cref="ArgumentException">Thrown when the frame is of the wrong type or contains insufficient data.</exception>
        public static BasicInfo FromFrame(Frame frame)
        {
            if (frame.FunctionType != FrameFunctionType.FRAME_BASIC_INFO)
                throw new ArgumentException("Invalid frame function type for BasicInfo.");

            if (frame.Data == null || frame.Data.Length < 16 || frame.DataLen < 16)
                throw new ArgumentException("Frame data is too short.");

            var data = frame.Data;

            return new BasicInfo
            {
                Vin = Utils.ReadUInt16(data, 0),
                Vout = Utils.ReadUInt16(data, 2),
                Iout = Utils.ReadUInt16(data, 4),
                VoMax = Utils.ReadUInt16(data, 6),
                Temp1 = Utils.ReadUInt16(data, 8),
                Temp2 = Utils.ReadUInt16(data, 10),
                Dc5V = Utils.ReadUInt16(data, 12),
                OutMode = data[14],
                WorkSt = data[15],
            };
        }
    }
}
