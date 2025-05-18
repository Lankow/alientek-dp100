using Timer = System.Windows.Forms.Timer;

namespace DP100App
{
    public partial class MainForm : Form
    {
        private readonly Alientek_DP100.AlientekDP100 _device = new Alientek_DP100.AlientekDP100();
        private readonly Timer _timer;

        private const int CycleTimeMs = 25; // update cycle time in milliseconds

        private bool _isConnected = false;
        private bool _isUpdating = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeControls();

            _timer = new Timer
            {
                Interval = CycleTimeMs
            };

            _timer.Tick += TimerCyclic;
        }

        private void InitializeControls()
        {
            ConnectButton.Text = "Connect";

            NumericVoltage.ValueChanged += (s, e) =>
            {
                if (_isConnected)
                    _device.SetVoltage((float)NumericVoltage.Value);
            };

            NumericCurrent.ValueChanged += (s, e) =>
            {
                if (_isConnected)
                    _device.SetCurrentLimit((float)NumericCurrent.Value);
            };
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (!_isConnected)
            {
                _isConnected = _device.Connect();
                if (_isConnected)
                {
                    ConnectButton.Text = "Disconnect";
                    _timer.Start();

                    SetInputsEnabled(true);
                    _device.GetInputVoltageCurrent(out float voltage, out float current);
                    NumericVoltage.Value = (decimal)voltage;
                    NumericCurrent.Value = (decimal)current;
                }
            }
            else
            {
                _timer.Stop();
                _device.Disconnect();
                _isConnected = false;
                ConnectButton.Text = "Connect";
                SetInputsEnabled(false);
            }
        }

        private void TurnOnButton_Click(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                _device.SetState(true);
            }
        }

        private void TurnOffButton_Click(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                _device.SetState(false);
            }
        }

        private void SetInputsEnabled(bool isEnabled)
        {
            TurnOffButton.Enabled = isEnabled;
            TurnOnButton.Enabled = isEnabled;
            NumericVoltage.Enabled = isEnabled;
            NumericCurrent.Enabled = isEnabled;
            TextVoltage.BackColor = isEnabled ? Color.White : SystemColors.Control;
            TextCurrent.BackColor = isEnabled ? Color.White : SystemColors.Control;
        }

        private void TimerCyclic(object sender, EventArgs e)
        {
            if (_isUpdating || !_isConnected) return;

            _isUpdating = true;

            if (_device.GetOutputVoltageCurrent(out float voltage, out float current))
            {
                BeginInvoke(new Action(() =>
                {
                    TextVoltage.Text = voltage.ToString("F2");
                    TextCurrent.Text = current.ToString("F2");
                }));
            }

            _isUpdating = false;
        }
    }
}