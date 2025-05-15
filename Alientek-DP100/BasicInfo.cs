using System;

namespace Alientek_DP100
{
    public class BasicInfo
    {
        public ushort Vin { get; set; }
        public ushort Vout { get; set; }
        public ushort Iout { get; set; }
        public ushort VoMax { get; set; }
        public ushort Temp1 { get; set; }
        public ushort Temp2 { get; set; }
        public ushort Dc5V { get; set; }
        public byte OutMode { get; set; }
        public byte WorkSt { get; set; }

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
