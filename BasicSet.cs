using System.Buffers.Binary;

namespace Alientek_DP100;

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
            VoSet = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(2)),
            IoSet = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(4)),
            OvpSet = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(6)),
            OcpSet = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(8))
        };
    }

    public byte[] ToByteArray()
    {
        var result = new byte[10];
        result[0] = Index;
        result[1] = State;
        BinaryPrimitives.WriteUInt16LittleEndian(result.AsSpan(2), VoSet);
        BinaryPrimitives.WriteUInt16LittleEndian(result.AsSpan(4), IoSet);
        BinaryPrimitives.WriteUInt16LittleEndian(result.AsSpan(6), OvpSet);
        BinaryPrimitives.WriteUInt16LittleEndian(result.AsSpan(8), OcpSet);
        return result;
    }
}
