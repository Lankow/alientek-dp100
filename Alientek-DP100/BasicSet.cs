using System;

namespace Alientek_DP100
{
    /// <summary>
    /// Represents the configuration settings to be applied to the Alientek DP100 device.
    /// </summary>
    public class BasicSet
    {
        /// <summary>
        /// Gets or sets the index or configuration slot. May include a write flag.
        /// </summary>
        public byte Index { get; set; }

        /// <summary>
        /// Gets or sets the output state. 1 = ON, 0 = OFF.
        /// </summary>
        public byte State { get; set; }

        /// <summary>
        /// Gets or sets the desired output voltage setting in millivolts.
        /// </summary>
        public ushort VoSet { get; set; }

        /// <summary>
        /// Gets or sets the desired output current limit in milliamperes.
        /// </summary>
        public ushort IoSet { get; set; }

        /// <summary>
        /// Gets or sets the over-voltage protection threshold in millivolts.
        /// </summary>
        public ushort OvpSet { get; set; }

        /// <summary>
        /// Gets or sets the over-current protection threshold in milliamperes.
        /// </summary>
        public ushort OcpSet { get; set; }

        /// <summary>
        /// Creates a <see cref="BasicSet"/> instance by parsing a device frame.
        /// </summary>
        /// <param name="frame">The frame containing basic set data from the device.</param>
        /// <returns>A populated <see cref="BasicSet"/> object.</returns>
        /// <exception cref="ArgumentException">Thrown when the frame is not of the correct type or data is insufficient.</exception>
        public static BasicSet FromFrame(Frame frame)
        {
            if (frame.FunctionType != FrameFunctionType.FRAME_BASIC_SET)
                throw new ArgumentException("Invalid frame function type for BasicSet.");

            if (frame.Data == null || frame.Data.Length < 10 || frame.DataLen < 10)
                throw new ArgumentException("Frame data is too short.");

            var data = frame.Data;

            return new BasicSet
            {
                Index = data[0],
                State = data[1],
                VoSet = Utils.ReadUInt16(data, 2),
                IoSet = Utils.ReadUInt16(data, 4),
                OvpSet = Utils.ReadUInt16(data, 6),
                OcpSet = Utils.ReadUInt16(data, 8)
            };
        }

        /// <summary>
        /// Converts the <see cref="BasicSet"/> object into a byte array suitable for transmission to the device.
        /// </summary>
        /// <returns>A byte array representing the current configuration settings.</returns>
        public byte[] ToByteArray()
        {
            var data = new byte[10];

            data[0] = Index;
            data[1] = State;
            Utils.WriteUInt16(data, 2, VoSet);
            Utils.WriteUInt16(data, 4, IoSet);
            Utils.WriteUInt16(data, 6, OvpSet);
            Utils.WriteUInt16(data, 8, OcpSet);

            return data;
        }
    }
}
