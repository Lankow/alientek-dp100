namespace Alientek_DP100
{
    /// <summary>
    /// Enumerates function types for frames exchanged with the Alientek DP100 device.
    /// </summary>
    public enum FrameFunctionType : byte
    {
        /// <summary>
        /// Request or response for general device information.
        /// </summary>
        FRAME_DEVICE_INFO = 0x10,

        /// <summary>
        /// Request or response for firmware version information.
        /// </summary>
        FRAME_FIRM_INFO = 17,

        /// <summary>
        /// Indicates the start of a data transmission sequence.
        /// </summary>
        FRAME_START_TRANS = 18,

        /// <summary>
        /// Represents an in-progress data transmission frame.
        /// </summary>
        FRAME_DATA_TRANS = 19,

        /// <summary>
        /// Marks the end of a data transmission sequence.
        /// </summary>
        FRAME_END_TRANS = 20,

        /// <summary>
        /// Used for initiating a firmware or system upgrade.
        /// </summary>
        FRAME_DEV_UPGRADE = 21,

        /// <summary>
        /// Requests or returns basic real-time operational data (e.g., voltage, current).
        /// </summary>
        FRAME_BASIC_INFO = 48,

        /// <summary>
        /// Requests or confirms basic settings changes (e.g., voltage/current limits).
        /// </summary>
        FRAME_BASIC_SET = 53,

        /// <summary>
        /// Requests or returns system-level information/configuration.
        /// </summary>
        FRAME_SYSTEM_INFO = 0x40,

        /// <summary>
        /// Sends system-level settings to the device.
        /// </summary>
        FRAME_SYSTEM_SET = 69,

        /// <summary>
        /// Initiates or responds to an output scan command.
        /// </summary>
        FRAME_SCAN_OUT = 80,

        /// <summary>
        /// Transmits serial output data.
        /// </summary>
        FRAME_SERIAL_OUT = 85,

        /// <summary>
        /// Used to signal or perform a disconnection operation.
        /// </summary>
        FRAME_DISCONNECT = 0x80,

        /// <summary>
        /// Represents an undefined or unused frame type.
        /// </summary>
        NONE = 0xFF
    }

    /// <summary>
    /// Represents a structured communication frame used in HID communication with the Alientek DP100.
    /// </summary>
    public class Frame
    {
        /// <summary>
        /// Gets or sets the address of the target device (usually fixed).
        /// </summary>
        public byte DeviceAddress { get; set; }

        /// <summary>
        /// Gets or sets the function type indicating the purpose of the frame.
        /// </summary>
        public FrameFunctionType FunctionType { get; set; }

        /// <summary>
        /// Gets or sets the sequence number for multi-part communication or tracking.
        /// </summary>
        public byte Sequence { get; set; }

        /// <summary>
        /// Gets or sets the length of the data payload.
        /// </summary>
        public byte DataLen { get; set; }

        /// <summary>
        /// Gets or sets the payload data of the frame.
        /// </summary>
        public byte[] Data { get; set; }
    }
}
