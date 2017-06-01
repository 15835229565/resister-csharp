namespace HFWifi
{
    partial class SerialSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SerialNum = new System.Windows.Forms.ComboBox();
            this.Baudrate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "串口号";
            // 
            // SerialNum
            // 
            this.SerialNum.FormattingEnabled = true;
            this.SerialNum.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM18",
            "COM19",
            "COM20"});
            this.SerialNum.Location = new System.Drawing.Point(90, 44);
            this.SerialNum.Name = "SerialNum";
            this.SerialNum.Size = new System.Drawing.Size(100, 20);
            this.SerialNum.TabIndex = 1;
            // 
            // Baudrate
            // 
            this.Baudrate.FormattingEnabled = true;
            this.Baudrate.Items.AddRange(new object[] {
            "9600",
            "115200"});
            this.Baudrate.Location = new System.Drawing.Point(92, 84);
            this.Baudrate.Name = "Baudrate";
            this.Baudrate.Size = new System.Drawing.Size(98, 20);
            this.Baudrate.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "波特率";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(90, 139);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SerialSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 202);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Baudrate);
            this.Controls.Add(this.SerialNum);
            this.Controls.Add(this.label1);
            this.Name = "SerialSetting";
            this.Text = "SerialSetting";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SerialNum;
        private System.Windows.Forms.ComboBox Baudrate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}