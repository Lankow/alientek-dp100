namespace DP100App
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ConnectButton = new Button();
            NumericVoltage = new NumericUpDown();
            NumericCurrent = new NumericUpDown();
            VoltageSetLabel = new Label();
            currentSetLabel = new Label();
            TextVoltage = new TextBox();
            VoltageLabel = new Label();
            TextCurrent = new TextBox();
            currentLabel = new Label();
            TurnOnButton = new Button();
            TurnOffButton = new Button();
            ((System.ComponentModel.ISupportInitialize)NumericVoltage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericCurrent).BeginInit();
            SuspendLayout();
            // 
            // ConnectButton
            // 
            ConnectButton.Font = new Font("Segoe UI", 11F);
            ConnectButton.Location = new Point(12, 137);
            ConnectButton.Name = "ConnectButton";
            ConnectButton.Size = new Size(150, 37);
            ConnectButton.TabIndex = 1;
            ConnectButton.Text = "Connect";
            ConnectButton.UseVisualStyleBackColor = true;
            ConnectButton.Click += ConnectButton_Click;
            // 
            // NumericVoltage
            // 
            NumericVoltage.DecimalPlaces = 2;
            NumericVoltage.Enabled = false;
            NumericVoltage.Location = new Point(12, 33);
            NumericVoltage.Name = "NumericVoltage";
            NumericVoltage.Size = new Size(150, 23);
            NumericVoltage.TabIndex = 2;
            // 
            // NumericCurrent
            // 
            NumericCurrent.DecimalPlaces = 2;
            NumericCurrent.Enabled = false;
            NumericCurrent.Location = new Point(12, 93);
            NumericCurrent.Name = "NumericCurrent";
            NumericCurrent.Size = new Size(150, 23);
            NumericCurrent.TabIndex = 3;
            // 
            // VoltageSetLabel
            // 
            VoltageSetLabel.AutoSize = true;
            VoltageSetLabel.Font = new Font("Segoe UI", 12F);
            VoltageSetLabel.Location = new Point(9, 9);
            VoltageSetLabel.Margin = new Padding(0);
            VoltageSetLabel.Name = "VoltageSetLabel";
            VoltageSetLabel.Size = new Size(152, 21);
            VoltageSetLabel.TabIndex = 4;
            VoltageSetLabel.Text = "Voltage  Setpoint [V]";
            // 
            // currentSetLabel
            // 
            currentSetLabel.AutoSize = true;
            currentSetLabel.Font = new Font("Segoe UI", 12F);
            currentSetLabel.Location = new Point(12, 69);
            currentSetLabel.Margin = new Padding(0);
            currentSetLabel.Name = "currentSetLabel";
            currentSetLabel.Size = new Size(149, 21);
            currentSetLabel.TabIndex = 5;
            currentSetLabel.Text = "Current Setpoint [A]";
            // 
            // TextVoltage
            // 
            TextVoltage.Enabled = false;
            TextVoltage.Location = new Point(190, 33);
            TextVoltage.Name = "TextVoltage";
            TextVoltage.ReadOnly = true;
            TextVoltage.Size = new Size(150, 23);
            TextVoltage.TabIndex = 6;
            // 
            // VoltageLabel
            // 
            VoltageLabel.AutoSize = true;
            VoltageLabel.Font = new Font("Segoe UI", 12F);
            VoltageLabel.Location = new Point(190, 9);
            VoltageLabel.Margin = new Padding(0);
            VoltageLabel.Name = "VoltageLabel";
            VoltageLabel.Size = new Size(86, 21);
            VoltageLabel.TabIndex = 7;
            VoltageLabel.Text = "Voltage [V]";
            // 
            // TextCurrent
            // 
            TextCurrent.Enabled = false;
            TextCurrent.Location = new Point(190, 93);
            TextCurrent.Name = "TextCurrent";
            TextCurrent.ReadOnly = true;
            TextCurrent.Size = new Size(150, 23);
            TextCurrent.TabIndex = 8;
            // 
            // currentLabel
            // 
            currentLabel.AutoSize = true;
            currentLabel.Font = new Font("Segoe UI", 12F);
            currentLabel.Location = new Point(190, 69);
            currentLabel.Margin = new Padding(0);
            currentLabel.Name = "currentLabel";
            currentLabel.Size = new Size(87, 21);
            currentLabel.TabIndex = 9;
            currentLabel.Text = "Current [A]";
            // 
            // TurnOnButton
            // 
            TurnOnButton.Enabled = false;
            TurnOnButton.Font = new Font("Segoe UI", 11F);
            TurnOnButton.Location = new Point(190, 137);
            TurnOnButton.Name = "TurnOnButton";
            TurnOnButton.Size = new Size(75, 37);
            TurnOnButton.TabIndex = 10;
            TurnOnButton.Text = "ON";
            TurnOnButton.UseVisualStyleBackColor = true;
            TurnOnButton.Click += TurnOnButton_Click;
            // 
            // TurnOffButton
            // 
            TurnOffButton.Enabled = false;
            TurnOffButton.Font = new Font("Segoe UI", 11F);
            TurnOffButton.Location = new Point(265, 137);
            TurnOffButton.Name = "TurnOffButton";
            TurnOffButton.Size = new Size(75, 37);
            TurnOffButton.TabIndex = 11;
            TurnOffButton.Text = "OFF";
            TurnOffButton.UseVisualStyleBackColor = true;
            TurnOffButton.Click += TurnOffButton_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(354, 197);
            Controls.Add(TurnOffButton);
            Controls.Add(TurnOnButton);
            Controls.Add(currentLabel);
            Controls.Add(TextCurrent);
            Controls.Add(VoltageLabel);
            Controls.Add(TextVoltage);
            Controls.Add(currentSetLabel);
            Controls.Add(VoltageSetLabel);
            Controls.Add(NumericCurrent);
            Controls.Add(NumericVoltage);
            Controls.Add(ConnectButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "MainForm";
            Text = "Alientek-DP100";
            ((System.ComponentModel.ISupportInitialize)NumericVoltage).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericCurrent).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button ConnectButton;
        private NumericUpDown NumericVoltage;
        private NumericUpDown NumericCurrent;
        private Label VoltageSetLabel;
        private Label currentSetLabel;
        private TextBox TextVoltage;
        private Label VoltageLabel;
        private TextBox TextCurrent;
        private Label currentLabel;
        private Button TurnOnButton;
        private Button TurnOffButton;
    }
}
