using System;

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

        var data = frame.Data;
        return new BasicSet
        {
            Index = data[0],
            State = data[1],
            VoSet = ToUInt16LE(data, 2),
            IoSet = ToUInt16LE(data, 4),
            OvpSet = ToUInt16LE(data, 6),
            OcpSet = ToUInt16LE(data, 8)
        };
    }

    public byte[] ToByteArray()
    {
        var result = new byte[10];
        result[0] = Index;
        result[1] = State;
        FromUInt16LE(result, 2, VoSet);
        FromUInt16LE(result, 4, IoSet);
        FromUInt16LE(result, 6, OvpSet);
        FromUInt16LE(result, 8, OcpSet);
        return result;
    }

    private static ushort ToUInt16LE(byte[] data, int offset)
    {
        return (ushort)(data[offset] | (data[offset + 1] << 8));
    }

    private static void FromUInt16LE(byte[] buffer, int offset, ushort value)
    {
        buffer[offset] = (byte)(value & 0xFF);
        buffer[offset + 1] = (byte)((value >> 8) & 0xFF);
    }
}
