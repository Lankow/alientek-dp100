using System;
using System.Collections.Concurrent;
using System.IO.Ports;
using System.Threading.Tasks;

public class AlientekDP100 : IDisposable
{
    private readonly SerialPort _serialPort;
    private readonly ConcurrentQueue<Func<Task>> _taskQueue = new ConcurrentQueue<Func<Task>>();
    private bool _runningTask = false;

    private TaskCompletionSource<Frame> _pendingResponse;
    private FrameFunctionType _expectedFunctionType;

    public AlientekDP100(string portName, int baudRate = 9600)
    {
        _serialPort = new SerialPort(portName, baudRate)
        {
            ReadTimeout = 1000,
            WriteTimeout = 1000
        };
        _serialPort.DataReceived += SerialPort_DataReceived;
        _serialPort.Open();
    }

    private void Enqueue(Func<Task> task)
    {
        _taskQueue.Enqueue(task);
        _ = ServiceQueue();
    }

    private async Task ServiceQueue()
    {
        if (_runningTask) return;

        _runningTask = true;
        try
        {
            while (_taskQueue.TryDequeue(out var task))
            {
                await task();
            }
        }
        finally
        {
            _runningTask = false;
        }
    }

    public async Task<Frame> SendFrameAndAwaitResponse(Frame frame, Func<Frame, bool> matchResponse)
    {
        var tcs = new TaskCompletionSource<Frame>();
        Enqueue(async () =>
        {
            _pendingResponse = tcs;
            _expectedFunctionType = frame.FunctionType;

            var sentData = FrameParser.FrameToBytes(frame);
            Console.WriteLine($"[SEND]: {BitConverter.ToString(sentData)}");

            _serialPort.Write(sentData, 0, sentData.Length);

            await Task.CompletedTask;
        });
        return await tcs.Task;
    }

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        try
        {
            var bytesToRead = _serialPort.BytesToRead;
            if (bytesToRead < 6) return;

            var buffer = new byte[bytesToRead];
            _serialPort.Read(buffer, 0, bytesToRead);

            ReceiveFrame(buffer);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] SerialPort_DataReceived: {ex.Message}");
        }
    }

    public void ReceiveFrame(byte[] rawData)
    {
        try
        {
            var frame = FrameParser.ParseInputReport(rawData);
            if (frame != null && _pendingResponse != null)
            {
                Console.WriteLine($"[RECV]: {BitConverter.ToString(rawData)}");
                _pendingResponse.TrySetResult(frame);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[RECV ERROR]: {ex.Message}");
        }
    }

    public async Task<BasicInfo> GetBasicInfo()
    {
        var frame = new Frame
        {
            DeviceAddress = 251,
            FunctionType = FrameFunctionType.FRAME_BASIC_INFO,
            Sequence = 0,
            DataLen = 0,
            Data = new byte[0]
        };
        var response = await SendFrameAndAwaitResponse(frame, f => f.FunctionType == FrameFunctionType.FRAME_BASIC_INFO);
        return BasicInfo.FromFrame(response);
    }

    public async Task<BasicSet> GetCurrentBasic()
    {
        var frameData = new byte[] { (byte)(0x80) };
        var frame = new Frame
        {
            DeviceAddress = 251,
            FunctionType = FrameFunctionType.FRAME_BASIC_SET,
            Sequence = 0,
            DataLen = (byte)frameData.Length,
            Data = frameData
        };
        var response = await SendFrameAndAwaitResponse(frame, f => f.FunctionType == FrameFunctionType.FRAME_BASIC_SET);
        return BasicSet.FromFrame(response);
    }

    public async Task<bool> SetBasic(BasicSet basicSet)
    {
        var copy = new BasicSet
        {
            Index = (byte)(basicSet.Index | 0x20),
            State = basicSet.State,
            VoSet = basicSet.VoSet,
            IoSet = basicSet.IoSet,
            OvpSet = basicSet.OvpSet,
            OcpSet = basicSet.OcpSet
        };

        var frameData = copy.ToByteArray();
        var frame = new Frame
        {
            DeviceAddress = 251,
            FunctionType = FrameFunctionType.FRAME_BASIC_SET,
            Sequence = 0,
            DataLen = (byte)frameData.Length,
            Data = frameData
        };
        var response = await SendFrameAndAwaitResponse(frame, f => f.FunctionType == FrameFunctionType.FRAME_BASIC_SET);
        return response.Data[0] == 1;
    }

    public void Dispose()
    {
        if (_serialPort.IsOpen)
        {
            _serialPort.DataReceived -= SerialPort_DataReceived;
            _serialPort.Close();
        }
        _serialPort.Dispose();
    }
}
