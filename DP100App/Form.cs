namespace DP100App
{
    public partial class MainForm : Form
    {
        private readonly Alientek_DP100.AlientekDP100 _device = new Alientek_DP100.AlientekDP100();
        private const int cycleTime = 1000; // update cycle time in milliseconds

        private bool _isConnected = false;
        private bool _isUpdating = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            buttonConnect.Text = "Connect";

            numericVoltage.ValueChanged += (s, e) =>
            {
                if (_isConnected)
                    _device.SetVoltage((float)numericVoltage.Value);
            };

            numericCurrent.ValueChanged += (s, e) =>
            {
                if (_isConnected)
                    _device.SetCurrentLimit((float)numericCurrent.Value);
            };
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (!_isConnected)
            {
                _isConnected = _device.Connect();
                if (_isConnected)
                {
                    buttonConnect.Text = "Disconnect";
                }
            }
            else
            {
                _device.Disconnect();
                _isConnected = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void turnOnButton_Click(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                _device.SetState(true);
            }
        }

        private void turnOffButton_Click(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                _device.SetState(false);
            }
        }
    }
}
