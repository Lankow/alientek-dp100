using System.Buffers.Binary;

namespace Alientek_DP100;

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

        var data = frame.Data;
        return new BasicInfo
        {
            Vin = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(0)),
            Vout = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(2)),
            Iout = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(4)),
            VoMax = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(6)),
            Temp1 = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(8)),
            Temp2 = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(10)),
            Dc5V = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(12)),
            OutMode = data[14],
            WorkSt = data[15]
        };
    }
}