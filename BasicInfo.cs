using System;

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
            Vin = ToUInt16LE(data, 0),
            Vout = ToUInt16LE(data, 2),
            Iout = ToUInt16LE(data, 4),
            VoMax = ToUInt16LE(data, 6),
            Temp1 = ToUInt16LE(data, 8),
            Temp2 = ToUInt16LE(data, 10),
            Dc5V = ToUInt16LE(data, 12),
            OutMode = data[14],
            WorkSt = data[15]
        };
    }

    private static ushort ToUInt16LE(byte[] data, int offset)
    {
        return (ushort)(data[offset] | (data[offset + 1] << 8));
    }
}
