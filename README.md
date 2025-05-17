# Alientek-DP100

A .NET library for communicating with the **Alientek ATK-MDP100** programmable power supply over USB using the HID protocol. This library provides basic control and monitoring capabilities such as reading voltage/current, setting output state, and configuring limits.

> **Note**: This project is based on [scottbez1/webdp100](https://github.com/scottbez1/webdp100), which served as the foundation for understanding the communication protocol and device behavior.

---

## Features

- Connect/disconnect from ATK-MDP100 device via HID
- Read real-time output voltage and current
- Turn output on or off
- Set voltage and current limit values

---

## Supported Device

- **Product Name**: ATK-MDP100  
- **Vendor ID**: `0x2E3C`  
- **Communication**: USB HID

---

## Example Usage

```csharp
using Alientek_DP100;

var device = new AlientekDP100();

if (device.Connect())
{
    Console.WriteLine("Connected!");

    device.SetVoltage(5.0f);          // Set output voltage to 5V
    device.SetCurrentLimit(1.5f);     // Set current limit to 1.5A
    device.SetState(true);            // Enable output

    if (device.GetVoltageCurrent(out float voltage, out float current))
    {
        Console.WriteLine($"Voltage: {voltage} V, Current: {current} A");
    }

    device.Disconnect();
}
else
{
    Console.WriteLine("Device not found.");
}
```

---

## Project Structure

```
Alientek_DP100/
├── AlientekDP100.cs        // Main device interface
├── BasicInfo.cs            // Structure for real-time monitoring data
├── BasicSet.cs             // Structure for configuration commands
├── Frame.cs                // Data frame definition
├── FrameParser.cs          // Frame serialization & deserialization
├── Utils.cs                // Byte array helpers
└── FrameFunctionType.cs    // Enum of supported frame command types
```

---

## Acknowledgments

- Based on [webdp100](https://github.com/scottbez1/webdp100) by [@scottbez1](https://github.com/scottbez1)
- [HidSharp](https://github.com/mikeobrien/HidSharp) for cross-platform USB HID support
- Alientek for the ATK-MDP100 hardware

---

## Contributing

Pull requests, bug reports, and feature suggestions are welcome.

---

## License

This project is licensed under the [MIT License](LICENSE).