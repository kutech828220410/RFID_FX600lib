namespace TEST_FORM
{
    partial class Form1
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

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button_TEST = new System.Windows.Forms.Button();
            this.textBox_Station = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Set_Baudrate = new System.Windows.Forms.Button();
            this.comboBox_Baudrate = new System.Windows.Forms.ComboBox();
            this.comboBox_COM_Baudrate = new System.Windows.Forms.ComboBox();
            this.button_ChangeBaudrate = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rfiD_FX600_UI1 = new RFID_FX600lib.RFID_FX600_UI();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rfiD_ESP32_UI1 = new RFID_FX600lib.RFID_ESP32_UI();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_TEST
            // 
            this.button_TEST.Location = new System.Drawing.Point(183, 28);
            this.button_TEST.Name = "button_TEST";
            this.button_TEST.Size = new System.Drawing.Size(75, 46);
            this.button_TEST.TabIndex = 1;
            this.button_TEST.Text = "蜂鳴測試";
            this.button_TEST.UseVisualStyleBackColor = true;
            this.button_TEST.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox_Station
            // 
            this.textBox_Station.Location = new System.Drawing.Point(18, 21);
            this.textBox_Station.Name = "textBox_Station";
            this.textBox_Station.Size = new System.Drawing.Size(100, 22);
            this.textBox_Station.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_Station);
            this.groupBox1.Location = new System.Drawing.Point(26, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(149, 56);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Station";
            // 
            // button_Set_Baudrate
            // 
            this.button_Set_Baudrate.Location = new System.Drawing.Point(157, 31);
            this.button_Set_Baudrate.Name = "button_Set_Baudrate";
            this.button_Set_Baudrate.Size = new System.Drawing.Size(75, 46);
            this.button_Set_Baudrate.TabIndex = 5;
            this.button_Set_Baudrate.Text = "Baudrate";
            this.button_Set_Baudrate.UseVisualStyleBackColor = true;
            this.button_Set_Baudrate.Click += new System.EventHandler(this.button_Set_Baudrate_Click);
            // 
            // comboBox_Baudrate
            // 
            this.comboBox_Baudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Baudrate.FormattingEnabled = true;
            this.comboBox_Baudrate.Items.AddRange(new object[] {
            "4800",
            "9600",
            "115200"});
            this.comboBox_Baudrate.Location = new System.Drawing.Point(21, 45);
            this.comboBox_Baudrate.Name = "comboBox_Baudrate";
            this.comboBox_Baudrate.Size = new System.Drawing.Size(121, 20);
            this.comboBox_Baudrate.TabIndex = 6;
            this.comboBox_Baudrate.SelectedIndexChanged += new System.EventHandler(this.comboBox_Baudrate_SelectedIndexChanged);
            // 
            // comboBox_COM_Baudrate
            // 
            this.comboBox_COM_Baudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_COM_Baudrate.FormattingEnabled = true;
            this.comboBox_COM_Baudrate.Items.AddRange(new object[] {
            "4800",
            "9600",
            "115200"});
            this.comboBox_COM_Baudrate.Location = new System.Drawing.Point(6, 38);
            this.comboBox_COM_Baudrate.Name = "comboBox_COM_Baudrate";
            this.comboBox_COM_Baudrate.Size = new System.Drawing.Size(121, 20);
            this.comboBox_COM_Baudrate.TabIndex = 7;
            // 
            // button_ChangeBaudrate
            // 
            this.button_ChangeBaudrate.Location = new System.Drawing.Point(133, 24);
            this.button_ChangeBaudrate.Name = "button_ChangeBaudrate";
            this.button_ChangeBaudrate.Size = new System.Drawing.Size(114, 46);
            this.button_ChangeBaudrate.TabIndex = 8;
            this.button_ChangeBaudrate.Text = "ChangeBaudrate";
            this.button_ChangeBaudrate.UseVisualStyleBackColor = true;
            this.button_ChangeBaudrate.Click += new System.EventHandler(this.button_ChangeBaudrate_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox_COM_Baudrate);
            this.groupBox2.Controls.Add(this.button_ChangeBaudrate);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(262, 84);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "設定主機端波特率";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBox_Baudrate);
            this.groupBox4.Controls.Add(this.button_Set_Baudrate);
            this.groupBox4.Location = new System.Drawing.Point(26, 83);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(247, 100);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "設定波特率";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.button_TEST);
            this.groupBox3.Location = new System.Drawing.Point(6, 263);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(370, 192);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "RFID Slave端";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1084, 694);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.rfiD_FX600_UI1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1076, 668);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "RS485";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // rfiD_FX600_UI1
            // 
            this.rfiD_FX600_UI1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rfiD_FX600_UI1.Location = new System.Drawing.Point(6, 96);
            this.rfiD_FX600_UI1.Name = "rfiD_FX600_UI1";
            this.rfiD_FX600_UI1.Size = new System.Drawing.Size(370, 161);
            this.rfiD_FX600_UI1.TabIndex = 2;
            this.rfiD_FX600_UI1.從站數量 = 3;
            this.rfiD_FX600_UI1.掃描速度 = 1;
            this.rfiD_FX600_UI1.是否自動通訊 = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.rfiD_ESP32_UI1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1076, 668);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ESP32-WIFI";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // rfiD_ESP32_UI1
            // 
            this.rfiD_ESP32_UI1.DNS = "0.0.0.0";
            this.rfiD_ESP32_UI1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rfiD_ESP32_UI1.Gateway = "0.0.0.0";
            this.rfiD_ESP32_UI1.IP_Adress = "0.0.0.0";
            this.rfiD_ESP32_UI1.Local_Port = "0";
            this.rfiD_ESP32_UI1.Location = new System.Drawing.Point(3, 3);
            this.rfiD_ESP32_UI1.Name = "rfiD_ESP32_UI1";
            this.rfiD_ESP32_UI1.Password = "";
            this.rfiD_ESP32_UI1.RFID_Enable = "0";
            this.rfiD_ESP32_UI1.Server_IP_Adress = "0.0.0.0";
            this.rfiD_ESP32_UI1.Server_Port = "0";
            this.rfiD_ESP32_UI1.ServerPort = 29999;
            this.rfiD_ESP32_UI1.Size = new System.Drawing.Size(1070, 662);
            this.rfiD_ESP32_UI1.SSID = "";
            this.rfiD_ESP32_UI1.Station = "0";
            this.rfiD_ESP32_UI1.Subnet = "0.0.0.0";
            this.rfiD_ESP32_UI1.TabIndex = 0;
            this.rfiD_ESP32_UI1.UDP_Ports = ((System.Collections.Generic.List<string>)(resources.GetObject("rfiD_ESP32_UI1.UDP_Ports")));
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 694);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "RFID調試軟體";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_TEST;
        private RFID_FX600lib.RFID_FX600_UI rfiD_FX600_UI1;
        private System.Windows.Forms.TextBox textBox_Station;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_Set_Baudrate;
        private System.Windows.Forms.ComboBox comboBox_Baudrate;
        private System.Windows.Forms.ComboBox comboBox_COM_Baudrate;
        private System.Windows.Forms.Button button_ChangeBaudrate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private RFID_FX600lib.RFID_ESP32_UI rfiD_ESP32_UI1;
    }
}

