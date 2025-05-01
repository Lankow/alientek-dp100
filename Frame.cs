public enum FrameFunctionType : byte
{
    FRAME_DEVICE_INFO = 0x10,
    FRAME_FIRM_INFO = 17,
    FRAME_START_TRANS = 18,
    FRAME_DATA_TRANS = 19,
    FRAME_END_TRANS = 20,
    FRAME_DEV_UPGRADE = 21,
    FRAME_BASIC_INFO = 48,
    FRAME_BASIC_SET = 53,
    FRAME_SYSTEM_INFO = 0x40,
    FRAME_SYSTEM_SET = 69,
    FRAME_SCAN_OUT = 80,
    FRAME_SERIAL_OUT = 85,
    FRAME_DISCONNECT = 0x80,
    NONE = 0xFF
}

public class Frame
{
    public byte DeviceAddress { get; set; }
    public FrameFunctionType FunctionType { get; set; }
    public byte Sequence { get; set; }
    public byte DataLen { get; set; }
    public byte[] Data { get; set; }
}
