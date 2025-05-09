using System;

namespace AlientekTest
{
    public class BasicSet
    {
        public byte Index { get; set; }
        public byte State { get; set; }
        public ushort VoSet { get; set; }
        public ushort IoSet { get; set; }
        public ushort OvpSet { get; set; }
        public ushort OcpSet { get; set; }

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
