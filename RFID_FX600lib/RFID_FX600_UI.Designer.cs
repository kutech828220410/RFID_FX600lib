namespace RFID_FX600lib
{
    partial class RFID_FX600_UI
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox_COM = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.textBox_New_Station = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Old_Station = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.button_Station_Write_Station = new System.Windows.Forms.Button();
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox_Station = new System.Windows.Forms.ListBox();
            this.label_CycleTime = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label_CycleTime);
            this.panel1.Controls.Add(this.textBox_COM);
            this.panel1.Controls.Add(this.label26);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(359, 43);
            this.panel1.TabIndex = 4;
            // 
            // textBox_COM
            // 
            this.textBox_COM.Location = new System.Drawing.Point(45, 8);
            this.textBox_COM.Name = "textBox_COM";
            this.textBox_COM.Size = new System.Drawing.Size(69, 22);
            this.textBox_COM.TabIndex = 40;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("新細明體", 9F);
            this.label26.Location = new System.Drawing.Point(5, 14);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(34, 12);
            this.label26.TabIndex = 39;
            this.label26.Text = "COM:";
            // 
            // textBox_New_Station
            // 
            this.textBox_New_Station.Location = new System.Drawing.Point(284, 77);
            this.textBox_New_Station.Name = "textBox_New_Station";
            this.textBox_New_Station.Size = new System.Drawing.Size(72, 22);
            this.textBox_New_Station.TabIndex = 42;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(215, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 12);
            this.label1.TabIndex = 41;
            this.label1.Text = "New Station :";
            // 
            // textBox_Old_Station
            // 
            this.textBox_Old_Station.Location = new System.Drawing.Point(284, 49);
            this.textBox_Old_Station.Name = "textBox_Old_Station";
            this.textBox_Old_Station.Size = new System.Drawing.Size(72, 22);
            this.textBox_Old_Station.TabIndex = 2;
            this.textBox_Old_Station.Text = "5";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(215, 55);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(63, 12);
            this.label25.TabIndex = 1;
            this.label25.Text = "Old Station :";
            // 
            // button_Station_Write_Station
            // 
            this.button_Station_Write_Station.Location = new System.Drawing.Point(267, 108);
            this.button_Station_Write_Station.Name = "button_Station_Write_Station";
            this.button_Station_Write_Station.Size = new System.Drawing.Size(89, 39);
            this.button_Station_Write_Station.TabIndex = 6;
            this.button_Station_Write_Station.Text = "Write Station";
            this.button_Station_Write_Station.UseVisualStyleBackColor = true;
            this.button_Station_Write_Station.Click += new System.EventHandler(this.button_Station_Write_Station_Click);
            // 
            // serialPort
            // 
            this.serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox_Station);
            this.groupBox1.Location = new System.Drawing.Point(9, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 98);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Station List";
            // 
            // listBox_Station
            // 
            this.listBox_Station.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_Station.FormattingEnabled = true;
            this.listBox_Station.ItemHeight = 12;
            this.listBox_Station.Location = new System.Drawing.Point(3, 18);
            this.listBox_Station.Name = "listBox_Station";
            this.listBox_Station.Size = new System.Drawing.Size(194, 77);
            this.listBox_Station.TabIndex = 8;
            // 
            // label_CycleTime
            // 
            this.label_CycleTime.AutoSize = true;
            this.label_CycleTime.Location = new System.Drawing.Point(247, 18);
            this.label_CycleTime.Name = "label_CycleTime";
            this.label_CycleTime.Size = new System.Drawing.Size(105, 12);
            this.label_CycleTime.TabIndex = 41;
            this.label_CycleTime.Text = "CycleTime : 0.000ms";
            // 
            // RFID_FX600_UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.textBox_New_Station);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_Station_Write_Station);
            this.Controls.Add(this.textBox_Old_Station);
            this.Controls.Add(this.label25);
            this.Name = "RFID_FX600_UI";
            this.Size = new System.Drawing.Size(359, 161);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox_COM;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button button_Station_Write_Station;
        private System.Windows.Forms.TextBox textBox_Old_Station;
        private System.Windows.Forms.Label label25;
        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.TextBox textBox_New_Station;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox_Station;
        private System.Windows.Forms.Label label_CycleTime;
    }
}
