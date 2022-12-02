using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using Basic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace RFID_FX600lib
{
    public partial class RFID_ESP32_UI : UserControl
    {
        [Category("UDP Config")]
        public int ServerPort
        {
            get
            {
                return serverport;
            }
            set
            {
                this.serverport = value;
            }
        }
        private List<string> _UDP_Ports = new List<string>();
        [Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
        [ReadOnly(false), Browsable(true), Category("UDP Config"), Description(""), DefaultValue("")]
        public List<string> UDP_Ports
        {
            get { return _UDP_Ports; }
            set
            {
                _UDP_Ports = value;
            }
        }
        private int serverport = 29999;

        public enum emun_command_IP
        {
            IP_Adress = (byte)'2',
            Subnet = (byte)'3',
            Gateway = (byte)'4',
            DNS = (byte)'5',
            Server_IP_Adress = (byte)'6',
            RFID_Enable = (byte)'Z',
        }
        public enum emun_command_Port
        {
            Local_Port = (byte)'7',
            Server_Port = (byte)'8',
        }
        private enum command
        {
            SSID = (byte)'9',
            Password = (byte)'A',
            Restart = (byte)'B',
            Set_Beep = (byte)'C',
            Get_7CardID = (byte)'D',
            UDP_Test = (byte)'E',
        }
        public enum RFID
        {
            編號,
            IP,
            station,
            ID,
            StartTime,
            Time,
            State,
        }
        public List<object[]> List_RFID = new List<object[]>();
        public String IP_Adress
        {
            get
            {
                int[] temp = new int[4];
                int.TryParse(this.textBox_IP_Adress_A.Text, out temp[0]);
                int.TryParse(this.textBox_IP_Adress_B.Text, out temp[1]);
                int.TryParse(this.textBox_IP_Adress_C.Text, out temp[2]);
                int.TryParse(this.textBox_IP_Adress_D.Text, out temp[3]);
                return temp[0].ToString() + "." + temp[1].ToString() + "." + temp[2].ToString() + "." + temp[3].ToString();
            }
            set
            {
                string[] str = value.Split('.');
                if (str.Length == 4)
                {
                    bool flag_OK = true;
                    int[] temp = new int[4];
                    if (!int.TryParse(str[0], out temp[0])) flag_OK = false;
                    if (!int.TryParse(str[1], out temp[1])) flag_OK = false;
                    if (!int.TryParse(str[2], out temp[2])) flag_OK = false;
                    if (!int.TryParse(str[3], out temp[3])) flag_OK = false;
                    if (flag_OK)
                    {
                        this.textBox_IP_Adress_A.Text = temp[0].ToString();
                        this.textBox_IP_Adress_B.Text = temp[1].ToString();
                        this.textBox_IP_Adress_C.Text = temp[2].ToString();
                        this.textBox_IP_Adress_D.Text = temp[3].ToString();
                    }
                }
            }
        }
        public String Subnet
        {
            get
            {
                int[] temp = new int[4];
                int.TryParse(this.textBox_Subnet_A.Text, out temp[0]);
                int.TryParse(this.textBox_Subnet_B.Text, out temp[1]);
                int.TryParse(this.textBox_Subnet_C.Text, out temp[2]);
                int.TryParse(this.textBox_Subnet_D.Text, out temp[3]);
                return temp[0].ToString() + "." + temp[1].ToString() + "." + temp[2].ToString() + "." + temp[3].ToString();
            }
            set
            {
                string[] str = value.Split('.');
                if (str.Length == 4)
                {
                    bool flag_OK = true;
                    int[] temp = new int[4];
                    if (!int.TryParse(str[0], out temp[0])) flag_OK = false;
                    if (!int.TryParse(str[1], out temp[1])) flag_OK = false;
                    if (!int.TryParse(str[2], out temp[2])) flag_OK = false;
                    if (!int.TryParse(str[3], out temp[3])) flag_OK = false;
                    if (flag_OK)
                    {
                        this.textBox_Subnet_A.Text = temp[0].ToString();
                        this.textBox_Subnet_B.Text = temp[1].ToString();
                        this.textBox_Subnet_C.Text = temp[2].ToString();
                        this.textBox_Subnet_D.Text = temp[3].ToString();
                    }
                }
            }
        }
        public String Gateway
        {
            get
            {
                int[] temp = new int[4];
                int.TryParse(this.textBox_Gateway_A.Text, out temp[0]);
                int.TryParse(this.textBox_Gateway_B.Text, out temp[1]);
                int.TryParse(this.textBox_Gateway_C.Text, out temp[2]);
                int.TryParse(this.textBox_Gateway_D.Text, out temp[3]);
                return temp[0].ToString() + "." + temp[1].ToString() + "." + temp[2].ToString() + "." + temp[3].ToString();
            }
            set
            {
                string[] str = value.Split('.');
                if (str.Length == 4)
                {
                    bool flag_OK = true;
                    int[] temp = new int[4];
                    if (!int.TryParse(str[0], out temp[0])) flag_OK = false;
                    if (!int.TryParse(str[1], out temp[1])) flag_OK = false;
                    if (!int.TryParse(str[2], out temp[2])) flag_OK = false;
                    if (!int.TryParse(str[3], out temp[3])) flag_OK = false;
                    if (flag_OK)
                    {
                        this.textBox_Gateway_A.Text = temp[0].ToString();
                        this.textBox_Gateway_B.Text = temp[1].ToString();
                        this.textBox_Gateway_C.Text = temp[2].ToString();
                        this.textBox_Gateway_D.Text = temp[3].ToString();
                    }
                }
            }
        }
        public String DNS
        {
            get
            {
                int[] temp = new int[4];
                int.TryParse(this.textBox_DNS_A.Text, out temp[0]);
                int.TryParse(this.textBox_DNS_B.Text, out temp[1]);
                int.TryParse(this.textBox_DNS_C.Text, out temp[2]);
                int.TryParse(this.textBox_DNS_D.Text, out temp[3]);
                return temp[0].ToString() + "." + temp[1].ToString() + "." + temp[2].ToString() + "." + temp[3].ToString();
            }
            set
            {
                string[] str = value.Split('.');
                if (str.Length == 4)
                {
                    bool flag_OK = true;
                    int[] temp = new int[4];
                    if (!int.TryParse(str[0], out temp[0])) flag_OK = false;
                    if (!int.TryParse(str[1], out temp[1])) flag_OK = false;
                    if (!int.TryParse(str[2], out temp[2])) flag_OK = false;
                    if (!int.TryParse(str[3], out temp[3])) flag_OK = false;
                    if (flag_OK)
                    {
                        this.textBox_DNS_A.Text = temp[0].ToString();
                        this.textBox_DNS_B.Text = temp[1].ToString();
                        this.textBox_DNS_C.Text = temp[2].ToString();
                        this.textBox_DNS_D.Text = temp[3].ToString();
                    }
                }
            }
        }
        public String Server_IP_Adress
        {
            get
            {
                int[] temp = new int[4];
                int.TryParse(this.textBox_Server_IP_Adress_A.Text, out temp[0]);
                int.TryParse(this.textBox_Server_IP_Adress_B.Text, out temp[1]);
                int.TryParse(this.textBox_Server_IP_Adress_C.Text, out temp[2]);
                int.TryParse(this.textBox_Server_IP_Adress_D.Text, out temp[3]);
                return temp[0].ToString() + "." + temp[1].ToString() + "." + temp[2].ToString() + "." + temp[3].ToString();
            }
            set
            {
                string[] str = value.Split('.');
                if (str.Length == 4)
                {
                    bool flag_OK = true;
                    int[] temp = new int[4];
                    if (!int.TryParse(str[0], out temp[0])) flag_OK = false;
                    if (!int.TryParse(str[1], out temp[1])) flag_OK = false;
                    if (!int.TryParse(str[2], out temp[2])) flag_OK = false;
                    if (!int.TryParse(str[3], out temp[3])) flag_OK = false;
                    if (flag_OK)
                    {
                        this.textBox_Server_IP_Adress_A.Text = temp[0].ToString();
                        this.textBox_Server_IP_Adress_B.Text = temp[1].ToString();
                        this.textBox_Server_IP_Adress_C.Text = temp[2].ToString();
                        this.textBox_Server_IP_Adress_D.Text = temp[3].ToString();
                    }
                }
            }
        }
        public String Local_Port
        {
            get
            {
                UInt16 temp = 0;
                UInt16.TryParse(this.textBox_Local_Port.Text, out temp);
                return temp.ToString();
            }
            set
            {
                UInt16 temp = 0;
                if (UInt16.TryParse(value, out temp))
                {
                    this.textBox_Local_Port.Text = value;
                }
            }
        }
        public String Server_Port
        {
            get
            {
                UInt16 temp = 0;
                UInt16.TryParse(this.textBox_Server_Port.Text, out temp);
                return temp.ToString();
            }
            set
            {
                UInt16 temp = 0;
                if (UInt16.TryParse(value, out temp))
                {
                    this.textBox_Server_Port.Text = value;
                }
            }
        }
        public String SSID
        {
            get
            {
                return this.textBox_SSID.Text;
            }
            set
            {
                this.textBox_SSID.Text = value;
            }
        }
        public String Password
        {
            get
            {
                return this.textBox_Password.Text;
            }
            set
            {
                this.textBox_Password.Text = value;
            }
        }
        public String Station
        {
            get
            {
                UInt16 temp = 0;
                UInt16.TryParse(this.textBox_Station.Text, out temp);
                return temp.ToString();
            }
            set
            {
                UInt16 temp = 0;
                if (UInt16.TryParse(value, out temp))
                {
                    this.textBox_Station.Text = value;
                }
            }
        }
        public String RFID_Enable
        {
            set
            {
                int temp = 0;
                int.TryParse(value, out temp);
                if(this.IsHandleCreated)
                {
                    this.Invoke(new Action(delegate
                    {
                        this.checkBox_RFID_Enable_01.Checked = myConvert.Int32GetBit(temp, 0);
                        this.checkBox_RFID_Enable_02.Checked = myConvert.Int32GetBit(temp, 1);
                        this.checkBox_RFID_Enable_03.Checked = myConvert.Int32GetBit(temp, 2);
                        this.checkBox_RFID_Enable_04.Checked = myConvert.Int32GetBit(temp, 3);
                        this.checkBox_RFID_Enable_05.Checked = myConvert.Int32GetBit(temp, 4);
                        this.checkBox_RFID_Enable_06.Checked = myConvert.Int32GetBit(temp, 5);
                        this.checkBox_RFID_Enable_07.Checked = myConvert.Int32GetBit(temp, 6);
                        this.checkBox_RFID_Enable_08.Checked = myConvert.Int32GetBit(temp, 7);
                        this.checkBox_RFID_Enable_09.Checked = myConvert.Int32GetBit(temp, 8);
                        this.checkBox_RFID_Enable_10.Checked = myConvert.Int32GetBit(temp, 9);
                        this.checkBox_RFID_Enable_11.Checked = myConvert.Int32GetBit(temp, 10);
                        this.checkBox_RFID_Enable_12.Checked = myConvert.Int32GetBit(temp, 11);
                        this.checkBox_RFID_Enable_13.Checked = myConvert.Int32GetBit(temp, 12);
                        this.checkBox_RFID_Enable_14.Checked = myConvert.Int32GetBit(temp, 13);
                        this.checkBox_RFID_Enable_15.Checked = myConvert.Int32GetBit(temp, 14);
                        this.checkBox_RFID_Enable_16.Checked = myConvert.Int32GetBit(temp, 15);
                    }));
                }
        


            }
            get
            {
                int temp = 0;
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_01.Checked, temp, 0);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_02.Checked, temp, 1);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_03.Checked, temp, 2);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_04.Checked, temp, 3);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_05.Checked, temp, 4);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_06.Checked, temp, 5);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_07.Checked, temp, 6);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_08.Checked, temp, 7);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_09.Checked, temp, 8);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_10.Checked, temp, 9);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_11.Checked, temp, 10);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_12.Checked, temp, 11);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_13.Checked, temp, 12);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_14.Checked, temp, 13);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_15.Checked, temp, 14);
                temp = myConvert.Int32SetBit(this.checkBox_RFID_Enable_16.Checked, temp, 15);
                return temp.ToString();
            }
        }
        private MyThread MyThread_SqlDataRefrsh;
        private MyConvert myConvert = new MyConvert();
        private List<UDP_Class> List_UDP_Server = new List<UDP_Class>();


        private bool FLAG_UART_RX = false;
        private List<int> UART_RX_BUF = new List<int>();
        private Stopwatch stopwatch = new Stopwatch();
        #region StreamIO
        [Serializable]
        private class SavePropertyFile
        {
            public string COMName = "";
            public string Baudrate = "";
            public string DataBits = "";
            public string Parity = "";
            public string StopBits = "";
        }
        private SavePropertyFile savePropertyFile = new SavePropertyFile();
        private void SaveProperties()
        {
            IFormatter binFmt = new BinaryFormatter();
            Stream stream = null;
            savePropertyFile.COMName = textBox_COM.Text.DeepClone();
            try
            {
                stream = File.Open(this.Name + ".pro", FileMode.Create);
                binFmt.Serialize(stream, savePropertyFile);
            }
            finally
            {
                if (stream != null) stream.Close();
            }
        }
        private void LoadProperties()
        {
            IFormatter binFmt = new BinaryFormatter();
            Stream stream = null;
            try
            {
                if (File.Exists(".\\" + this.Name + ".pro"))
                {
                    stream = File.Open(".\\" + this.Name + ".pro", FileMode.Open);
                    try { savePropertyFile = (SavePropertyFile)binFmt.Deserialize(stream); }
                    catch { }

                }
                this.textBox_COM.Text = savePropertyFile.COMName.DeepClone();
            }
            finally
            {
                if (stream != null) stream.Close();
            }
        }
        #endregion
        public void Init()
        {
            this.sqL_DataGridView_RFID_List.Init();
            this.FindForm().FormClosing += this.FormClosing;
            this.LoadProperties();
            for (int i = 0; i < this.UDP_Ports.Count; i++)
            {
                UDP_Class uDP_Class = new UDP_Class("0.0.0.0", this.UDP_Ports[i].StringToInt32());
                uDP_Class.DataReciveEvent += UDP_Server_DataReciveEvent;
                List_UDP_Server.Add(uDP_Class);
            }

            this.textBox_IP_Adress_A.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_IP_Adress_B.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_IP_Adress_C.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_IP_Adress_D.KeyPress += this.textBox_KeyPress_NumBox;

            this.textBox_Subnet_A.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_Subnet_B.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_Subnet_C.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_Subnet_D.KeyPress += this.textBox_KeyPress_NumBox;

            this.textBox_Gateway_A.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_Gateway_B.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_Gateway_C.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_Gateway_D.KeyPress += this.textBox_KeyPress_NumBox;

            this.textBox_DNS_A.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_DNS_B.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_DNS_C.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_DNS_D.KeyPress += this.textBox_KeyPress_NumBox;

            this.textBox_Server_IP_Adress_A.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_Server_IP_Adress_B.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_Server_IP_Adress_C.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_Server_IP_Adress_D.KeyPress += this.textBox_KeyPress_NumBox;

            this.textBox_Local_Port.KeyPress += this.textBox_KeyPress_NumBox;
            this.textBox_Server_Port.KeyPress += this.textBox_KeyPress_NumBox;

            this.textBox_IP_Adress_A.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_IP_Adress_B.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_IP_Adress_C.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_IP_Adress_D.TextChanged += this.textBox_TextChanged_NumBox;

            this.textBox_Subnet_A.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_Subnet_B.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_Subnet_C.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_Subnet_D.TextChanged += this.textBox_TextChanged_NumBox;

            this.textBox_Gateway_A.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_Gateway_B.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_Gateway_C.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_Gateway_D.TextChanged += this.textBox_TextChanged_NumBox;

            this.textBox_DNS_A.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_DNS_B.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_DNS_C.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_DNS_D.TextChanged += this.textBox_TextChanged_NumBox;

            this.textBox_Server_IP_Adress_A.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_Server_IP_Adress_B.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_Server_IP_Adress_C.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_Server_IP_Adress_D.TextChanged += this.textBox_TextChanged_NumBox;

            this.textBox_Local_Port.TextChanged += this.textBox_TextChanged_NumBox;
            this.textBox_Server_Port.TextChanged += this.textBox_TextChanged_NumBox;

            this.sqL_DataGridView_RFID_List.DataGridRefreshEvent += SqL_DataGridView_RFID_List_DataGridRefreshEvent;

            this.MyThread_SqlDataRefrsh = new MyThread();
            this.MyThread_SqlDataRefrsh.AutoRun(true);
            this.MyThread_SqlDataRefrsh.Add_Method(sub_SqlDataRefrsh);
            this.MyThread_SqlDataRefrsh.SetSleepTime(100);
            this.stopwatch.Start();

        }

        private void SqL_DataGridView_RFID_List_DataGridRefreshEvent()
        {
            string 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_RFID_List.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_RFID_List.dataGridView.Rows[i].Cells[(int)RFID.State].Value.ToString();
                if (狀態 == "OK")
                {
                    this.sqL_DataGridView_RFID_List.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.sqL_DataGridView_RFID_List.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == "NG")
                {
                    this.sqL_DataGridView_RFID_List.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    this.sqL_DataGridView_RFID_List.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        public RFID_ESP32_UI()
        {
            InitializeComponent();
        }
        #region Function
        public bool SerialPortOpen()
        {
            this.serialPort.Close();
            try
            {
                this.serialPort.PortName = this.textBox_COM.Text;
                this.serialPort.BaudRate = 115200;
                this.serialPort.DataBits = 8;
                this.serialPort.Parity = System.IO.Ports.Parity.None;
                this.serialPort.StopBits = System.IO.Ports.StopBits.One;
                this.serialPort.Open();
            }
            catch
            {
                MessageBox.Show("COM Port open failed!");
                return false;
            }
            return true;

        }
        public void SerialPortClose()
        {
            this.serialPort.Close();
        }
        public bool Command_Get_Setting()
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {

                int retry = 0;
                this.UART_RX_BUF.Clear();
                this.FLAG_UART_RX = false;
                byte[] bytes = new byte[3] { 2, (byte)'0', 3 };
                this.serialPort.Write(bytes, 0, 3);
                while (true)
                {
                    if (retry >= 1000)
                    {
                        this.UART_RX_BUF.Clear();
                        this.FLAG_UART_RX = false;
                        MessageBox.Show("Receive data failed!");
                        flag_OK = false;
                        break;
                    }
                    if (this.FLAG_UART_RX)
                    {
                        if (this.UART_RX_BUF[0] == 2 && this.UART_RX_BUF[this.UART_RX_BUF.Count - 1] == 3)
                        {
                            string str = "";
                            for (int i = 1; i < (this.UART_RX_BUF.Count - 1); i++)
                            {
                                str += (char)this.UART_RX_BUF[i];
                            }
                            string[] str_array = str.Split(',');
                            if (str_array.Length == 11)
                            {
                                this.Invoke(new Action(delegate
                                {
                                    this.IP_Adress = str_array[0];
                                    this.Subnet = str_array[1];
                                    this.Gateway = str_array[2];
                                    this.DNS = str_array[3];
                                    this.Server_IP_Adress = str_array[4];
                                    this.Local_Port = str_array[5];
                                    this.Server_Port = str_array[6];
                                    this.SSID = str_array[7];
                                    this.Password = str_array[8];
                                    this.Station = str_array[9];
                                    this.RFID_Enable = str_array[10];
                                }));

                                MessageBox.Show("Receive data sucessed!");
                                flag_OK = true;
                                break;
                            }
                            else
                            {

                                MessageBox.Show("Receive data lenth error!");
                                flag_OK = false;
                                break;
                            }

                        }
                    }

                    retry++;
                    System.Threading.Thread.Sleep(1);
                }
            }
            this.SerialPortClose();
            return flag_OK;
        }
        public bool Command_Set_Station(int station)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte checksum = 0;
                byte[] bytes = new byte[5];
                bytes[0] = 2;
                bytes[1] = (byte)'1';
                bytes[2] = (byte)station;
                bytes[3] = (byte)(station >> 8);
                bytes[4] = 3;
                for (int i = 0; i < bytes.Length; i++)
                {
                    checksum += bytes[i];
                }
                int retry = 0;
                this.UART_RX_BUF.Clear();
                this.FLAG_UART_RX = false;
                serialPort.Write(bytes, 0, bytes.Length);
                while (true)
                {
                    if (retry >= 1000)
                    {
                        flag_OK = false;
                        break;
                    }
                    if (this.FLAG_UART_RX)
                    {
                        if (this.CheckSum(checksum))
                        {
                            flag_OK = true;
                            break;
                        }

                    }
                    retry++;
                    System.Threading.Thread.Sleep(1);
                }
            }
            this.SerialPortClose();
            return flag_OK;
        }
        public bool Command_Set_Port(int station, emun_command_Port command_Port, int port)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte checksum = 0;
                byte[] bytes = new byte[5];
                bytes[0] = 2;
                bytes[1] = (byte)command_Port;
                bytes[2] = (byte)port;
                bytes[3] = (byte)(port >> 8);
                bytes[4] = 3;
                for (int i = 0; i < bytes.Length; i++)
                {
                    checksum += bytes[i];
                }
                int retry = 0;
                this.UART_RX_BUF.Clear();
                this.FLAG_UART_RX = false;
                serialPort.Write(bytes, 0, bytes.Length);
                while (true)
                {
                    if (retry >= 1000)
                    {
                        flag_OK = false;
                        break;
                    }
                    if (this.FLAG_UART_RX)
                    {
                        if (this.CheckSum(checksum))
                        {
                            flag_OK = true;
                            break;
                        }

                    }
                    retry++;
                    System.Threading.Thread.Sleep(1);
                }
            }
            this.SerialPortClose();
            return flag_OK;
        }
        public bool Command_Set_IP_Adress(int station, emun_command_IP command_IP, string IP)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte checksum = 0;
                byte[] bytes = new byte[7];
                byte[] bytes_IP = this.IP2Bytes(IP);
                bytes[0] = 2;
                bytes[1] = (byte)command_IP;
                bytes[2] = bytes_IP[0];
                bytes[3] = bytes_IP[1];
                bytes[4] = bytes_IP[2];
                bytes[5] = bytes_IP[3];
                bytes[6] = 3;
                for (int i = 0; i < bytes.Length; i++)
                {
                    checksum += bytes[i];
                }
                int retry = 0;
                this.UART_RX_BUF.Clear();
                this.FLAG_UART_RX = false;
                serialPort.Write(bytes, 0, bytes.Length);
                while (true)
                {
                    if (retry >= 1000)
                    {
                        flag_OK = false;
                        break;
                    }
                    if (this.FLAG_UART_RX)
                    {
                        if (this.CheckSum(checksum))
                        {
                            flag_OK = true;
                            break;
                        }

                    }
                    retry++;
                    System.Threading.Thread.Sleep(1);
                }
            }
            this.SerialPortClose();
            return flag_OK;
        }
        public bool Command_Set_SSID(int station, string ssid)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte checksum = 0;
                char[] chars = ssid.ToCharArray();
                List<byte> list_byte = new List<byte>();
                list_byte.Add(2);
                list_byte.Add((byte)(command.SSID));
                for (int i = 0; i < chars.Length; i++)
                {
                    list_byte.Add((byte)chars[i]);
                }
                list_byte.Add(3);
                for (int i = 0; i < list_byte.Count; i++)
                {
                    checksum += list_byte[i];
                }
                int retry = 0;
                this.UART_RX_BUF.Clear();
                this.FLAG_UART_RX = false;
                serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                while (true)
                {
                    if (retry >= 1000)
                    {
                        flag_OK = false;
                        break;
                    }
                    if (this.FLAG_UART_RX)
                    {
                        if (this.CheckSum(checksum))
                        {
                            flag_OK = true;
                            break;
                        }

                    }
                    retry++;
                    System.Threading.Thread.Sleep(1);
                }
            }
            this.SerialPortClose();
            return flag_OK;
        }
        public bool Command_Set_Password(int station, string password)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte checksum = 0;
                char[] chars = password.ToCharArray();
                List<byte> list_byte = new List<byte>();
                list_byte.Add(2);

                list_byte.Add((byte)(command.Password));
                for (int i = 0; i < chars.Length; i++)
                {
                    list_byte.Add((byte)chars[i]);
                }
                list_byte.Add(3);
                for (int i = 0; i < list_byte.Count; i++)
                {
                    checksum += list_byte[i];
                }
                int retry = 0;
                this.UART_RX_BUF.Clear();
                this.FLAG_UART_RX = false;
                serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                while (true)
                {
                    if (retry >= 1000)
                    {
                        flag_OK = false;
                        break;
                    }
                    if (this.FLAG_UART_RX)
                    {
                        if (this.CheckSum(checksum))
                        {
                            flag_OK = true;
                            break;
                        }

                    }
                    retry++;
                    System.Threading.Thread.Sleep(1);
                }
            }
            this.SerialPortClose();
            return flag_OK;
        }
        public bool Command_Set_RFID_Enable()
        {
            return this.Command_Set_RFID_Enable(this.RFID_Enable.StringToInt32());
        }
        public bool Command_Set_RFID_Enable(int RFID_Enable)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte checksum = 0;
                List<byte> list_byte = new List<byte>();
                list_byte.Add(2);
                list_byte.Add((byte)(emun_command_IP.RFID_Enable));
                list_byte.Add(((byte)RFID_Enable));
                list_byte.Add(((byte)(RFID_Enable >> 8)));
                list_byte.Add(3);
                for (int i = 0; i < list_byte.Count; i++)
                {
                    checksum += list_byte[i];
                }
                int retry = 0;
                this.UART_RX_BUF.Clear();
                this.FLAG_UART_RX = false;
                serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                while (true)
                {
                    if (retry >= 1000)
                    {
                        flag_OK = false;
                        break;
                    }
                    if (this.FLAG_UART_RX)
                    {
                        if (this.CheckSum(checksum))
                        {
                            flag_OK = true;
                            break;
                        }

                    }
                    retry++;
                    System.Threading.Thread.Sleep(1);
                }
            }
            this.SerialPortClose();
            return flag_OK;
        }

        public bool Command_Restart()
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte checksum = 0;
                List<byte> list_byte = new List<byte>();
                list_byte.Add(2);
                list_byte.Add((byte)(command.Restart));       
                list_byte.Add(3);
                for (int i = 0; i < list_byte.Count; i++) checksum += list_byte[i];

                int retry = 0;
                this.UART_RX_BUF.Clear();
                this.FLAG_UART_RX = false;
                serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                while (true)
                {
                    if (retry >= 1000)
                    {
                        flag_OK = false;
                        break;
                    }
                    if (this.FLAG_UART_RX)
                    {
                        if (this.CheckSum(checksum))
                        {
                            flag_OK = true;
                            break;
                        }

                    }
                    retry++;
                    System.Threading.Thread.Sleep(1);
                }
            }
            this.SerialPortClose();
            return flag_OK;
        }
        public bool Command_Set_Beep(int station)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte checksum = 0;
                List<byte> list_byte = new List<byte>();
                list_byte.Add(2);
                list_byte.Add((byte)(command.Set_Beep));
                list_byte.Add((byte)(station));
                list_byte.Add(3);
                for (int i = 0; i < list_byte.Count; i++) checksum += list_byte[i];

                int retry = 0;
                this.UART_RX_BUF.Clear();
                this.FLAG_UART_RX = false;
                serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                while (true)
                {
                    if (retry >= 1000)
                    {
                        flag_OK = false;
                        break;
                    }
                    if (this.FLAG_UART_RX)
                    {
                        if (this.CheckSum(checksum))
                        {
                            flag_OK = true;
                            break;
                        }

                    }
                    retry++;
                    System.Threading.Thread.Sleep(1);
                }
            }
            this.SerialPortClose();
            return flag_OK;
        }
        public bool Command_Get_7CardID(int station ,ref string ID)
        {
            bool flag_OK = true;
            ID = "";
            if (this.SerialPortOpen())
            {
                byte checksum = 0;
                List<byte> list_byte = new List<byte>();
                list_byte.Add(2);
                list_byte.Add((byte)(command.Get_7CardID));
                list_byte.Add((byte)(station));
                list_byte.Add(3);
                for (int i = 0; i < list_byte.Count; i++) checksum += list_byte[i];

                int retry = 0;
                this.UART_RX_BUF.Clear();
                this.FLAG_UART_RX = false;
                serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                while (true)
                {
                    if (retry >= 1000)
                    {
                        flag_OK = false;
                        break;
                    }
                    if (this.FLAG_UART_RX)
                    {
                        if(this.UART_RX_BUF.Count == 16)
                        {
                            for (int i = 0; i < (this.UART_RX_BUF.Count - 1); i++)
                            {
                                ID += (char)this.UART_RX_BUF[i];
                            }
                            return true;
                        }
                      

                    }
                    retry++;
                    System.Threading.Thread.Sleep(1);
                }
            }
            this.SerialPortClose();
            return flag_OK;
        }
        public bool Command_UDP_Test()
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte checksum = 0;
                List<byte> list_byte = new List<byte>();
                list_byte.Add(2);
                list_byte.Add((byte)(command.UDP_Test));
                list_byte.Add(3);
                for (int i = 0; i < list_byte.Count; i++) checksum += list_byte[i];

                int retry = 0;
                this.UART_RX_BUF.Clear();
                this.FLAG_UART_RX = false;
                serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                while (true)
                {
                    if (retry >= 1000)
                    {
                        flag_OK = false;
                        break;
                    }
                    if (this.FLAG_UART_RX)
                    {
                        if (this.CheckSum(checksum))
                        {
                            flag_OK = true;
                            break;
                        }

                    }
                    retry++;
                    System.Threading.Thread.Sleep(1);
                }
            }
            this.SerialPortClose();
            return flag_OK;
        }
        #endregion
        private bool CheckSum(byte checksum)
        {
            if (this.UART_RX_BUF.Count == 3 && this.FLAG_UART_RX)
            {
                string str = "";
                str += (char)this.UART_RX_BUF[0];
                str += (char)this.UART_RX_BUF[1];
                str += (char)this.UART_RX_BUF[2];
                byte temp = 0;

                byte.TryParse(str, out temp);
                if (temp == checksum) return true;
            }
            return false;
        }
        private byte[] IP2Bytes(string IP)
        {
            byte[] bytes = new byte[4];
            string[] str = IP.Split('.');
            if (str.Length == 4)
            {
                byte.TryParse(str[0], out bytes[0]);
                byte.TryParse(str[1], out bytes[1]);
                byte.TryParse(str[2], out bytes[2]);
                byte.TryParse(str[3], out bytes[3]);
            }
            return bytes;
        }

        #region Event
        private void textBox_KeyPress_NumBox(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar <= 57 && (int)e.KeyChar >= 48) || (e.KeyChar == (char)Keys.Back)) // 8 > BackSpace
            {

                e.Handled = false;
                return;
            }
            else
            {
                e.Handled = true;
                return;
            }
        }
        private void textBox_TextChanged_NumBox(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int temp = 0;
            int MaxNum;
            int.TryParse(textBox.Text, out temp);
            if (textBox.Name == "textBox_Local_Port" || textBox.Name == "textBox_Server_Port") MaxNum = 65535;
            else MaxNum = 255;
            if (temp > MaxNum)
            {
                temp = MaxNum;
                textBox.Text = "";
                return;
            }
        }
        private void button_Read_Click(object sender, EventArgs e)
        {
            this.Command_Get_Setting();
        }
        private void button_Write_Click(object sender, EventArgs e)
        {
            int station = 0;
            int.TryParse(this.Station, out station);
            int localport = 0;
            int.TryParse(this.Local_Port, out localport);
            int serverport = 0;
            int.TryParse(this.Server_Port, out serverport);
            bool flag_OK = true;
            if (!this.Command_Set_IP_Adress(station, emun_command_IP.IP_Adress, this.IP_Adress)) flag_OK = false;
            if (!this.Command_Set_IP_Adress(station, emun_command_IP.Subnet, this.Subnet)) flag_OK = false;
            if (!this.Command_Set_IP_Adress(station, emun_command_IP.Gateway, this.Gateway)) flag_OK = false;
            if (!this.Command_Set_IP_Adress(station, emun_command_IP.Server_IP_Adress, this.Server_IP_Adress)) flag_OK = false;
            if (!this.Command_Set_IP_Adress(station, emun_command_IP.DNS, this.DNS)) flag_OK = false;
            if (!this.Command_Set_Port(station, emun_command_Port.Local_Port, localport)) flag_OK = false;
            if (!this.Command_Set_Port(station, emun_command_Port.Server_Port, serverport)) flag_OK = false;

            if (!this.Command_Set_SSID(station, this.SSID)) flag_OK = false;
            if (!this.Command_Set_Password(station, this.Password)) flag_OK = false;
            if(!this.Command_Set_RFID_Enable()) flag_OK = false;
            if (flag_OK)
            {
                MessageBox.Show("Write data sucessed!");
            }
            else
            {
                MessageBox.Show("Write data failed!");
            }
        }
        private void button_Station_Write_Click(object sender, EventArgs e)
        {
            int temp = 0;
            int.TryParse(this.Station, out temp);
            if (this.Command_Set_Station(temp))
            {
                MessageBox.Show(string.Format("Change station to {0}", temp.ToString()));
            }
        }
        private void button_Restart_Click(object sender, EventArgs e)
        {
            if(this.Command_Restart())
            {
                MessageBox.Show("EPS32 Restart!");
            }
        }
        private void button_SetBeep_Click(object sender, EventArgs e)
        {
            int station = this.textBox_Control_station.Text.StringToInt32();
            if (station < 0 || station >=256) MessageBox.Show("Station num error!");
            if (this.Command_Set_Beep(station))
            {
                MessageBox.Show("Beep sucess!");
            }
        }
        private void button_Get_7_Card_ID_Click(object sender, EventArgs e)
        {
            int station = this.textBox_Control_station.Text.StringToInt32();
            if (station < 0 || station >= 256) MessageBox.Show("Station num error!");
            string ID = "";
            if (this.Command_Get_7CardID(station , ref ID))
            {
                MessageBox.Show(string.Format("sucess! {0}" , ID));
            }
        }
        private void button_UDP_Test_Click(object sender, EventArgs e)
        {
            if (this.Command_UDP_Test())
            {
                MessageBox.Show(string.Format("Done!"));
            }
        }
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int byte2read = this.serialPort.BytesToRead;
            for (int i = 0; i < byte2read; i++)
            {
                this.UART_RX_BUF.Add(this.serialPort.ReadByte());
            }
            this.FLAG_UART_RX = true;
        }
        private void FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveProperties();
        }
        MyTimer MyTimer_UDP = new MyTimer();
        private void UDP_Server_DataReciveEvent(string IP, int Port, string readline)
        {

            string[] str_array = myConvert.分解分隔號字串(readline, ',', StringSplitOptions.RemoveEmptyEntries);
            string station = "";
            string ID = "";
            double CycleTime_start = 0;
            if (str_array.Length > 0)
            {
                station = str_array[0];
                List<object[]> List_RFID_buf = new List<object[]>();
                List_RFID_buf = (from value in List_RFID
                                 where value[(int)RFID.IP].ObjectToString() == IP
                                 where value[(int)RFID.station].ObjectToString() == station
                                 select value).ToList();
                if (str_array.Length == 2)
                {

                    ID = str_array[1];

                    if (List_RFID_buf.Count > 0)
                    {
                        List_RFID_buf[0][(int)RFID.ID] = ID;
                        double.TryParse(List_RFID_buf[0][(int)RFID.StartTime].ObjectToString(), out CycleTime_start);
                        List_RFID_buf[0][(int)RFID.StartTime] = stopwatch.Elapsed.TotalMilliseconds.ToString("0.000000");
                        List_RFID_buf[0][(int)RFID.Time] = (stopwatch.Elapsed.TotalMilliseconds - CycleTime_start).ToString("0.000000");
                        List_RFID_buf[0][(int)RFID.State] = "OK";
                    }
                    else
                    {
                        this.List_RFID.Add(new object[] {"0", IP, station, ID, stopwatch.Elapsed.TotalMilliseconds.ToString("0.000000"), "0", "OK" });
                    }
                }
                else
                {
                    if (List_RFID_buf.Count > 0)
                    {
                        List_RFID_buf[0][(int)RFID.State] = "NG";
                    }
                }
            }






        }

        #endregion
        private void sub_SqlDataRefrsh()
        {
            List<object[]> list_rfid = this.List_RFID.DeepClone();
            list_rfid.Sort(new ICP_RFID());
            for (int i = 0; i < list_rfid.Count; i++)
            {
                list_rfid[i][(int)RFID.編號] = (i + 1).ToString("00");
            }
            this.sqL_DataGridView_RFID_List.RefreshGrid(list_rfid);
        }
        public class ICP_RFID : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string IP_0 = x[(int)RFID.IP].ObjectToString();
                string IP_1 = y[(int)RFID.IP].ObjectToString();
                string station_0 = x[(int)RFID.station].ObjectToString();
                string station_1 = y[(int)RFID.station].ObjectToString();
                string[] IP_0_Array = IP_0.Split('.');
                string[] IP_1_Array = IP_1.Split('.');
                IP_0 = "";
                IP_1 = "";
                for (int i = 0; i < 4; i++)
                {
                    if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];
                    if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];
                    if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];

                    if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];
                    if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];
                    if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];

                    IP_0 += IP_0_Array[i];
                    IP_1 += IP_1_Array[i];
                }
                int cmp = IP_0_Array[3].CompareTo(IP_1_Array[3]);
                if (cmp > 0)
                {
                    return 1;
                }
                else if (cmp < 0)
                {
                    return -1;
                }
                else
                {
                    if (station_0.StringToInt32() > station_1.StringToInt32()) return 1;
                    else if (station_0.StringToInt32() < station_1.StringToInt32()) return -1;
                    else
                    {
                        return 0;
                    }
                }
             
         

            }
        }
        public class MyTimer
        {
            private bool OnTick = false;
            private double TickTime = 0;
            static private Stopwatch stopwatch = new Stopwatch();
            public MyTimer()
            {
                stopwatch.Start();
            }
            private double CycleTime_start;
            public void StartTickTime(double TickTime)
            {
                this.TickTime = TickTime;
                if (!OnTick)
                {
                    CycleTime_start = stopwatch.Elapsed.TotalMilliseconds;
                    OnTick = true;
                }
            }
            public void StartTickTime(int vlaue)
            {
                this.TickTime = vlaue;
                if (!OnTick)
                {
                    CycleTime_start = stopwatch.Elapsed.TotalMilliseconds;
                    OnTick = true;
                }
            }
            public double GetTickTime()
            {
                return stopwatch.Elapsed.TotalMilliseconds - CycleTime_start;
            }
            public void TickStop()
            {
                this.OnTick = false;
            }
            public bool IsTimeOut()
            {
                if ((stopwatch.Elapsed.TotalMilliseconds - CycleTime_start) >= TickTime)
                {
                    OnTick = false;
                    return true;
                }
                else return false;
            }
        }

    }
}
