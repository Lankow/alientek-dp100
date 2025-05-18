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
            buttonConnect = new Button();
            numericVoltage = new NumericUpDown();
            numericCurrent = new NumericUpDown();
            voltageSetLabel = new Label();
            currentSetLabel = new Label();
            textVoltage = new TextBox();
            voltageLabel = new Label();
            textBox2 = new TextBox();
            currentLabel = new Label();
            turnOnButton = new Button();
            turnOffButton = new Button();
            ((System.ComponentModel.ISupportInitialize)numericVoltage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericCurrent).BeginInit();
            SuspendLayout();
            // 
            // buttonConnect
            // 
            buttonConnect.Font = new Font("Segoe UI", 11F);
            buttonConnect.Location = new Point(12, 137);
            buttonConnect.Name = "buttonConnect";
            buttonConnect.Size = new Size(150, 37);
            buttonConnect.TabIndex = 1;
            buttonConnect.Text = "Connect";
            buttonConnect.UseVisualStyleBackColor = true;
            buttonConnect.Click += buttonConnect_Click;
            // 
            // numericVoltage
            // 
            numericVoltage.DecimalPlaces = 2;
            numericVoltage.Location = new Point(12, 33);
            numericVoltage.Name = "numericVoltage";
            numericVoltage.Size = new Size(150, 23);
            numericVoltage.TabIndex = 2;
            // 
            // numericCurrent
            // 
            numericCurrent.DecimalPlaces = 2;
            numericCurrent.Location = new Point(12, 93);
            numericCurrent.Name = "numericCurrent";
            numericCurrent.Size = new Size(150, 23);
            numericCurrent.TabIndex = 3;
            // 
            // voltageSetLabel
            // 
            voltageSetLabel.AutoSize = true;
            voltageSetLabel.Font = new Font("Segoe UI", 12F);
            voltageSetLabel.Location = new Point(9, 9);
            voltageSetLabel.Margin = new Padding(0);
            voltageSetLabel.Name = "voltageSetLabel";
            voltageSetLabel.Size = new Size(152, 21);
            voltageSetLabel.TabIndex = 4;
            voltageSetLabel.Text = "Voltage  Setpoint [V]";
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
            // textVoltage
            // 
            textVoltage.Location = new Point(190, 33);
            textVoltage.Name = "textVoltage";
            textVoltage.Size = new Size(150, 23);
            textVoltage.TabIndex = 6;
            // 
            // voltageLabel
            // 
            voltageLabel.AutoSize = true;
            voltageLabel.Font = new Font("Segoe UI", 12F);
            voltageLabel.Location = new Point(190, 9);
            voltageLabel.Margin = new Padding(0);
            voltageLabel.Name = "voltageLabel";
            voltageLabel.Size = new Size(86, 21);
            voltageLabel.TabIndex = 7;
            voltageLabel.Text = "Voltage [V]";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(190, 93);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(150, 23);
            textBox2.TabIndex = 8;
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
            // turnOnButton
            // 
            turnOnButton.Font = new Font("Segoe UI", 11F);
            turnOnButton.Location = new Point(190, 137);
            turnOnButton.Name = "turnOnButton";
            turnOnButton.Size = new Size(75, 37);
            turnOnButton.TabIndex = 10;
            turnOnButton.Text = "ON";
            turnOnButton.UseVisualStyleBackColor = true;
            turnOnButton.Click += turnOnButton_Click;
            // 
            // turnOffButton
            // 
            turnOffButton.Font = new Font("Segoe UI", 11F);
            turnOffButton.Location = new Point(265, 137);
            turnOffButton.Name = "turnOffButton";
            turnOffButton.Size = new Size(75, 37);
            turnOffButton.TabIndex = 11;
            turnOffButton.Text = "OFF";
            turnOffButton.UseVisualStyleBackColor = true;
            turnOffButton.Click += turnOffButton_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(354, 197);
            Controls.Add(turnOffButton);
            Controls.Add(turnOnButton);
            Controls.Add(currentLabel);
            Controls.Add(textBox2);
            Controls.Add(voltageLabel);
            Controls.Add(textVoltage);
            Controls.Add(currentSetLabel);
            Controls.Add(voltageSetLabel);
            Controls.Add(numericCurrent);
            Controls.Add(numericVoltage);
            Controls.Add(buttonConnect);
            Name = "MainForm";
            Text = "Alientek-DP100";
            ((System.ComponentModel.ISupportInitialize)numericVoltage).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericCurrent).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button buttonConnect;
        private NumericUpDown numericVoltage;
        private NumericUpDown numericCurrent;
        private Label voltageSetLabel;
        private Label currentSetLabel;
        private TextBox textVoltage;
        private Label voltageLabel;
        private TextBox textBox2;
        private Label currentLabel;
        private Button turnOnButton;
        private Button turnOffButton;
    }
}
