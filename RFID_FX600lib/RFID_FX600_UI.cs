using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using Basic;
using System.Diagnostics;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MyUI;
namespace RFID_FX600lib
{
    public partial class RFID_FX600_UI : UserControl
    {
        private bool IsOpen = false;
        private MyTimer myTimer = new MyTimer();
        private MyConvert Myconvert = new MyConvert();
        public int baudrate = 9600;
        [Serializable]
        public class RFID_Device
        {
            public int station = -1;
            public string UID = ""; 
        }
        private List<RFID_Device> List_RFID_Device = new List<RFID_Device>();
        private bool FLAG_UART_RX = false;
        private List<int> UART_RX_BUF = new List<int>();
        #region 自訂屬性
        private int _從站數量 = 1;
        [ReadOnly(false), Browsable(true), Category("自訂屬性"), Description(""), DefaultValue("")]
        public virtual int 從站數量
        {
            get { return this._從站數量; }
            set
            {
                this._從站數量 = value;
            }
        }
        private int _掃描速度 = 1;
        [ReadOnly(false), Browsable(true), Category("自訂屬性"), Description(""), DefaultValue("")]
        public virtual int 掃描速度
        {
            get { return this._掃描速度; }
            set
            {
                this._掃描速度 = value;
            }
        }

        private bool _是否自動通訊 = false;
        [ReadOnly(false), Browsable(true), Category("自訂屬性"), Description(""), DefaultValue("")]
        public virtual bool 是否自動通訊
        {
            get { return this._是否自動通訊; }
            set
            {
                this._是否自動通訊 = value;
            }
        }
        #endregion

        private int NumOfRFID = 1;
        private Basic.MyThread MyThread_RS485;
        private Basic.MyThread MyThread_RefreshUI;
        private bool flag_Init = false;
        private bool flag_IsFormClosing = false;
        public enum enum_Mode
        {
            Mobus = 0x01,
            OutputToDEC = 0x02,
            OutputToHEX = 0x03,
            OutputTo_7_HEX = 0x05,
        }
        public enum Baudrate
        {
            _4800  = 4800,
            _9600 = 9600,
            _115200 = 115200,
        }
        public RFID_FX600_UI()
        {
            InitializeComponent();
        }
        public void Init(int NumOfRFID, Baudrate baudrate, string COM_Name)
        {
            this.Init(baudrate, NumOfRFID, COM_Name);
        }
        public void Init(string COM_Name)
        {
            this.Init(Baudrate._9600, this.從站數量, COM_Name);
        }
        public void Init(int NumOfRFID, string COM_Name)
        {
            this.Init(Baudrate._9600, NumOfRFID, COM_Name);
        }
        public void Init(Baudrate baudrate)
        {
            this.Init(baudrate, this.從站數量, "");
        }
        public void Init(int NumOfRFID)
        {

            this.Init(Baudrate._9600, NumOfRFID, "");
        }
        public void Init()
        {
            this.Init(Baudrate._9600, this.從站數量, "");
        }
        public void Init(Baudrate baudrate , int NumOfRFID, string COM_Name)
        {
            this.LoadProperties();
            if (!COM_Name.StringIsEmpty()) this.textBox_COM.Text = COM_Name;
            this.從站數量 = NumOfRFID;
            this.NumOfRFID = this.從站數量;
            this.FindForm().FormClosing += this.FormClosing;
            this.ChangeBaudrate(baudrate);
            this.SerialPortOpen();
            this.MyThread_RS485 = new MyThread(this.FindForm());
            this.MyThread_RS485.AutoRun(true);
            this.MyThread_RS485.SetSleepTime(this.掃描速度);
            this.MyThread_RS485.Add_Method(this.Run);
            this.MyThread_RS485.Trigger();

            this.MyThread_RefreshUI = new MyThread(this.FindForm());
            this.MyThread_RefreshUI.AutoRun(true);
            this.MyThread_RefreshUI.SetSleepTime(10);
            this.MyThread_RefreshUI.Add_Method(this.RefreshUI);
            this.MyThread_RefreshUI.Trigger();

          
        }
        public void ChangeBaudrate(Baudrate baudrate)
        {
            if (baudrate == Baudrate._4800) this.baudrate = 4800;
            else if (baudrate == Baudrate._9600) this.baudrate = 9600;
            else if (baudrate == Baudrate._115200) this.baudrate = 115200;
            this.SerialPortClose();
            this.SerialPortOpen();
        }
        private void RefreshUI()
        {
            if(this.flag_Init)
            {
                for (int i = 0; i < this.List_RFID_Device.Count; i++)
                {
                    this.Invoke(new Action(delegate
                    {
                        this.listBox_Station.Items[i] = string.Format("<{0}> : {1}", this.List_RFID_Device[i].station.ToString("00"), this.List_RFID_Device[i].UID);
                    }));
                }
            }
          
        }
        private void Run()
        {
            myTimer.TickStop();
            myTimer.StartTickTime(50000);
            if (this.IsOpen)
            {
                if (!flag_Init)
                {
                    this.Check_All_Staion(this.NumOfRFID);
                    for (int i = 0; i < this.List_RFID_Device.Count; i++)
                    {
                        this.Command_Set_AutoBeep(this.List_RFID_Device[i].station, true);
                        this.Command_Set_Mode(this.List_RFID_Device[i].station, enum_Mode.Mobus);
                    }
                    flag_Init = true;
                }


                if(this.是否自動通訊)
                {
                    for (int i = 0; i < this.List_RFID_Device.Count; i++)
                    {

                        string ID = "";
                        this.Command_Get_7CardID(this.List_RFID_Device[i].station, ref ID);
                        this.List_RFID_Device[i].UID = ID;

                    }
                }

          
            }
            this.Invoke(new Action(delegate
            {
                this.label_CycleTime.Text = $"CycleTime :{myTimer.GetTickTime()}";
            }));

        }
        public List<RFID_Device> Get_RFID()
        {
            List<RFID_Device> list_RFID_Devices = new List<RFID_Device>();
            for (int i = 0; i < this.List_RFID_Device.Count; i++)
            {
                string UID = this.Get_RFID_UID(this.List_RFID_Device[i].station);
                int temp = 0;
                int.TryParse(UID, out temp);
                if (UID.StringToInt32() != 0 && UID != "")
                {
                    RFID_Device rFID_Device = new RFID_Device();
                    rFID_Device.station = this.List_RFID_Device[i].station;
                    rFID_Device.UID = UID;
                    list_RFID_Devices.Add(rFID_Device);
                }
            }
            return list_RFID_Devices;
        }
        public string Get_RFID_UID(int station)
        {
           string ID = null;
           for(int i = 0; i < this.List_RFID_Device.Count; i++)
            {
                if(this.List_RFID_Device[i].station == station)
                {
                    ID = this.List_RFID_Device[i].UID.DeepClone();
                    this.List_RFID_Device[i].UID = "";
                    return ID;
                }
            }
            return ID;
        }
        public bool Command_Set_AutoBeep(int station, bool enable)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte CRC_L = 0;
                byte CRC_H = 0;
                List<byte> list_byte = new List<byte>();
                list_byte.Add((byte)station);
                list_byte.Add((byte)(0x06));
                list_byte.Add(0x00);
                list_byte.Add(0x05);
                list_byte.Add(0x00);
                list_byte.Add(enable ? (byte)(0x01) : (byte)(0x00));
                Basic.MyConvert.Get_CRC16(list_byte.ToArray(), ref CRC_L, ref CRC_H);
                list_byte.Add(CRC_L);
                list_byte.Add(CRC_H);
                MyTimer MyTimer_UART_TimeOut = new MyTimer();
                int retry = 0;
                int cnt = 0;
                while (true)
                {
                    if (cnt == 0)
                    {
                        if (retry > 5)
                        {
                            flag_OK = false;
                            break;
                        }
                        this.UART_RX_BUF.Clear();
                        this.FLAG_UART_RX = false;
                        serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                        MyTimer_UART_TimeOut.TickStop();
                        MyTimer_UART_TimeOut.StartTickTime(100);
                        cnt++;
                    }
                    else if (cnt == 1)
                    {
                        if (MyTimer_UART_TimeOut.IsTimeOut())
                        {
                            retry++;
                            cnt = 0;
                        }
                        if (this.FLAG_UART_RX)
                        {
                            if (this.UART_RX_BUF.Count == 8)
                            {
                                int cnt_OK = 0;
                                if (this.UART_RX_BUF[0] == list_byte[0]) cnt_OK++;
                                if (this.UART_RX_BUF[1] == list_byte[1]) cnt_OK++;
                                if (this.UART_RX_BUF[2] == list_byte[2]) cnt_OK++;
                                if (this.UART_RX_BUF[3] == list_byte[3]) cnt_OK++;
                                if (this.UART_RX_BUF[4] == list_byte[4]) cnt_OK++;
                                if (this.UART_RX_BUF[5] == list_byte[5]) cnt_OK++;
                                if (this.UART_RX_BUF[6] == list_byte[6]) cnt_OK++;
                                if (this.UART_RX_BUF[7] == list_byte[7]) cnt_OK++;
                                if (cnt_OK == 8)
                                {
                                    flag_OK = true;
                                    break;
                                }

                            }

                        }
                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
            return flag_OK;
        }
        public bool Command_Set_Baudrate(int station, Baudrate baudrate)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte CRC_L = 0;
                byte CRC_H = 0;
                List<byte> list_byte = new List<byte>();
                list_byte.Add((byte)station);
                list_byte.Add((byte)(0x06));
                list_byte.Add(0x07);
                list_byte.Add(0xD1);
                list_byte.Add(0x00);
                list_byte.Add((byte)baudrate);
                Basic.MyConvert.Get_CRC16(list_byte.ToArray(), ref CRC_L, ref CRC_H);
                list_byte.Add(CRC_L);
                list_byte.Add(CRC_H);
                MyTimer MyTimer_UART_TimeOut = new MyTimer();
                int retry = 0;
                int cnt = 0;
                while (true)
                {
                    if (cnt == 0)
                    {
                        if (retry > 5)
                        {
                            flag_OK = false;
                            break;
                        }
                        this.UART_RX_BUF.Clear();
                        this.FLAG_UART_RX = false;
                        serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                        MyTimer_UART_TimeOut.TickStop();
                        MyTimer_UART_TimeOut.StartTickTime(100);
                        cnt++;
                    }
                    else if (cnt == 1)
                    {
                        if (MyTimer_UART_TimeOut.IsTimeOut())
                        {
                            retry++;
                            cnt = 0;
                        }
                        if (this.FLAG_UART_RX)
                        {
                            if (this.UART_RX_BUF.Count == 8)
                            {
                                int cnt_OK = 0;
                                if (this.UART_RX_BUF[0] == list_byte[0]) cnt_OK++;
                                if (this.UART_RX_BUF[1] == list_byte[1]) cnt_OK++;
                                if (this.UART_RX_BUF[2] == list_byte[2]) cnt_OK++;
                                if (this.UART_RX_BUF[3] == list_byte[3]) cnt_OK++;
                                if (this.UART_RX_BUF[4] == list_byte[4]) cnt_OK++;
                                if (this.UART_RX_BUF[5] == list_byte[5]) cnt_OK++;
                                if (this.UART_RX_BUF[6] == list_byte[6]) cnt_OK++;
                                if (this.UART_RX_BUF[7] == list_byte[7]) cnt_OK++;
                                if (cnt_OK == 8)
                                {
                                    flag_OK = true;
                                    break;
                                }

                            }

                        }
                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
            return flag_OK;
        }
        public bool Command_Set_Beep(int station)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte CRC_L = 0;
                byte CRC_H = 0;
                List<byte> list_byte = new List<byte>();
                list_byte.Add((byte)station);
                list_byte.Add((byte)(0x06));
                list_byte.Add(0x00);
                list_byte.Add(0x04);
                list_byte.Add(0x00);
                list_byte.Add(0x02);
                Basic.MyConvert.Get_CRC16(list_byte.ToArray(), ref CRC_L, ref CRC_H);
                list_byte.Add(CRC_L);
                list_byte.Add(CRC_H);
                MyTimer MyTimer_UART_TimeOut = new MyTimer();
                int cnt = 0;
                int retry = 0;
                while (true)
                {
                    if (cnt == 0)
                    {
                        if (retry > 5)
                        {
                            flag_OK = false;
                            break;
                        }
                        this.UART_RX_BUF.Clear();
                        this.FLAG_UART_RX = false;
                        serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                        MyTimer_UART_TimeOut.TickStop();
                        MyTimer_UART_TimeOut.StartTickTime(200);
                        cnt++;
                    }
                    else if (cnt == 1)
                    {
                        if (MyTimer_UART_TimeOut.IsTimeOut())
                        {
                            cnt = 0;
                            retry++;
                            Console.WriteLine($"Command_Set_Beep station :{station} failed!");
                        }
                        if (this.FLAG_UART_RX)
                        {
                            if (this.UART_RX_BUF.Count == 8)
                            {
                                int cnt_OK = 0;
                                if (this.UART_RX_BUF[0] == list_byte[0]) cnt_OK++;
                                if (this.UART_RX_BUF[1] == list_byte[1]) cnt_OK++;
                                if (this.UART_RX_BUF[2] == list_byte[2]) cnt_OK++;
                                if (this.UART_RX_BUF[3] == list_byte[3]) cnt_OK++;
                                if (this.UART_RX_BUF[4] == list_byte[4]) cnt_OK++;
                                if (this.UART_RX_BUF[5] == list_byte[5]) cnt_OK++;
                                if (this.UART_RX_BUF[6] == list_byte[6]) cnt_OK++;
                                if (this.UART_RX_BUF[7] == list_byte[7]) cnt_OK++;
                                if (cnt_OK == 8)
                                {
                                    Console.WriteLine($"Command_Set_Beep station :{station} sucessed!");
                                    flag_OK = true;
                                    break;
                                }

                            }

                        }
                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
            return flag_OK;
        }
        public bool Command_Set_Mode(int station, enum_Mode enum_Mode)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte CRC_L = 0;
                byte CRC_H = 0;
                List<byte> list_byte = new List<byte>();
                list_byte.Add((byte)station);
                list_byte.Add((byte)(0x06));
                list_byte.Add(0x00);
                list_byte.Add(0x09);
                list_byte.Add(0x00);
                list_byte.Add((byte)enum_Mode);
                Basic.MyConvert.Get_CRC16(list_byte.ToArray(), ref CRC_L, ref CRC_H);
                list_byte.Add(CRC_L);
                list_byte.Add(CRC_H);
                MyTimer MyTimer_UART_TimeOut = new MyTimer();
                int retry = 0;
                int cnt = 0;
                while (true)
                {
                    if (cnt == 0)
                    {
                        if (retry > 5)
                        {
                            flag_OK = false;
                            break;
                        }
                        this.UART_RX_BUF.Clear();
                        this.FLAG_UART_RX = false;
                        serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                        MyTimer_UART_TimeOut.TickStop();
                        MyTimer_UART_TimeOut.StartTickTime(100);
                        cnt++;
                    }
                    else if (cnt == 1)
                    {
                        if (MyTimer_UART_TimeOut.IsTimeOut())
                        {
                            cnt = 0;
                            retry++;
                        }
                        if (this.FLAG_UART_RX)
                        {
                            if (this.UART_RX_BUF.Count == 8)
                            {
                                int cnt_OK = 0;
                                if (this.UART_RX_BUF[0] == list_byte[0]) cnt_OK++;
                                if (this.UART_RX_BUF[1] == list_byte[1]) cnt_OK++;
                                if (this.UART_RX_BUF[2] == list_byte[2]) cnt_OK++;
                                if (this.UART_RX_BUF[3] == list_byte[3]) cnt_OK++;
                                if (this.UART_RX_BUF[4] == list_byte[4]) cnt_OK++;
                                if (this.UART_RX_BUF[5] == list_byte[5]) cnt_OK++;
                                if (this.UART_RX_BUF[6] == list_byte[6]) cnt_OK++;
                                if (this.UART_RX_BUF[7] == list_byte[7]) cnt_OK++;
                                if (cnt_OK == 8)
                                {
                                    flag_OK = true;
                                    break;
                                }
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
            return flag_OK;
        }
        public bool Command_Set_Station(int old_station ,int new_station)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                byte CRC_L = 0;
                byte CRC_H = 0;
                List<byte> list_byte = new List<byte>();
                list_byte.Add((byte)old_station);
                list_byte.Add((byte)(0x06));
                list_byte.Add(0x07);
                list_byte.Add(0xD0);
                list_byte.Add(0x00);
                list_byte.Add((byte)new_station);
                Basic.MyConvert.Get_CRC16(list_byte.ToArray(), ref CRC_L, ref CRC_H);
                list_byte.Add(CRC_L);
                list_byte.Add(CRC_H);
                MyTimer MyTimer_UART_TimeOut = new MyTimer();
                int cnt = 0;
                while (true)
                {
                    if (cnt == 0)
                    {
                        this.UART_RX_BUF.Clear();
                        this.FLAG_UART_RX = false;
                        serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                        MyTimer_UART_TimeOut.TickStop();
                        MyTimer_UART_TimeOut.StartTickTime(500);
                        cnt++;
                    }
                    else if (cnt == 1)
                    {
                        if (MyTimer_UART_TimeOut.IsTimeOut())
                        {
                            if (this.FLAG_UART_RX)
                            {
                                if (this.UART_RX_BUF.Count == 8)
                                {
                                    int cnt_OK = 0;
                                    if (this.UART_RX_BUF[0] == list_byte[0]) cnt_OK++;
                                    if (this.UART_RX_BUF[1] == list_byte[1]) cnt_OK++;
                                    if (this.UART_RX_BUF[2] == list_byte[2]) cnt_OK++;
                                    if (this.UART_RX_BUF[3] == list_byte[3]) cnt_OK++;
                                    if (this.UART_RX_BUF[4] == list_byte[4]) cnt_OK++;
                                    if (this.UART_RX_BUF[5] == list_byte[5]) cnt_OK++;
                                    if (this.UART_RX_BUF[6] == list_byte[6]) cnt_OK++;
                                    if (this.UART_RX_BUF[7] == list_byte[7]) cnt_OK++;
                                    if (cnt_OK == 8)
                                    {
                                        flag_OK = true;
                                        break;
                                    }
                                    else
                                    {
                                        flag_OK = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    flag_OK = false;
                                    break;
                                }
                            }
                            else
                            {
                                flag_OK = false;
                                break;
                            }
                        }

                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
            return flag_OK;
        }
        public bool Command_Get_7CardID(int station , ref string ID)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                ID = "0";
                byte CRC_L = 0;
                byte CRC_H = 0;
                List<byte> list_byte = new List<byte>();
                list_byte.Add((byte)station);
                list_byte.Add((byte)(0x03));
                list_byte.Add(0x00);
                list_byte.Add(0x00);
                list_byte.Add(0x00);
                list_byte.Add(0x04);
                Basic.MyConvert.Get_CRC16(list_byte.ToArray(), ref CRC_L, ref CRC_H);
                list_byte.Add(CRC_L);
                list_byte.Add(CRC_H);
                MyTimer MyTimer_UART_TimeOut = new MyTimer();
                int cnt = 0;
                int retry = 0;
                while (!this.flag_IsFormClosing)
                {
                    if (cnt == 0)
                    {
                        if(retry > 5)
                        {
                            flag_OK = false;
                            break;
                        }
                        this.UART_RX_BUF.Clear();
                        this.FLAG_UART_RX = false;
                        this.serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                        MyTimer_UART_TimeOut.TickStop();
                        MyTimer_UART_TimeOut.StartTickTime(100);
                        cnt++;
                    }
                    else if (cnt == 1)
                    {
                        if (MyTimer_UART_TimeOut.IsTimeOut())
                        {
                            cnt = 0;
                            retry++;
                        }
                        if (this.FLAG_UART_RX)
                        {
                            if (this.UART_RX_BUF.Count == 13)
                            {
                                int cnt_OK = 0;
                                byte[] bytes = new byte[11];
                                if (this.UART_RX_BUF[0] == (byte)station) cnt_OK++;
                                if (this.UART_RX_BUF[1] == 0x03) cnt_OK++;
                                if (this.UART_RX_BUF[2] == 0x08) cnt_OK++;
                                for (int i = 0; i < bytes.Length; i++)
                                {
                                    bytes[i] = (byte)this.UART_RX_BUF[i];
                                }

                                Basic.MyConvert.Get_CRC16(bytes, ref CRC_L, ref CRC_H);
                                if (this.UART_RX_BUF[11] == CRC_L) cnt_OK++;
                                if (this.UART_RX_BUF[12] == CRC_H) cnt_OK++;



                                if (cnt_OK == 5)
                                {
                                    List<byte> hex_bytes = new List<byte>();
                                    hex_bytes.Add((byte)this.UART_RX_BUF[3]);
                                    hex_bytes.Add((byte)this.UART_RX_BUF[4]);
                                    hex_bytes.Add((byte)this.UART_RX_BUF[5]);
                                    hex_bytes.Add((byte)this.UART_RX_BUF[6]);
                                    hex_bytes.Add((byte)this.UART_RX_BUF[7]);
                                    hex_bytes.Add((byte)this.UART_RX_BUF[8]);
                                    hex_bytes.Add((byte)this.UART_RX_BUF[9]);
                                    ID = Myconvert.ByteToStringHex(hex_bytes.ToArray());
                                    ID = ID.Replace("-", "");
                                    flag_OK = true;
                                    break;
                                }

                            }
                       
                        }
                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
            return flag_OK;
        }
        public bool Command_Get_4CardID(int station, ref string ID)
        {
            bool flag_OK = true;
            if (this.SerialPortOpen())
            {
                ID = "00000000";
                byte CRC_L = 0;
                byte CRC_H = 0;
                List<byte> list_byte = new List<byte>();
                list_byte.Add((byte)station);
                list_byte.Add((byte)(0x03));
                list_byte.Add(0x00);
                list_byte.Add(0x00);
                list_byte.Add(0x00);
                list_byte.Add(0x02);
                Basic.MyConvert.Get_CRC16(list_byte.ToArray(), ref CRC_L, ref CRC_H);
                list_byte.Add(CRC_L);
                list_byte.Add(CRC_H);
                MyTimer MyTimer_UART_TimeOut = new MyTimer();
                int retry = 0;
                int cnt = 0;
                while (true)
                {
                    if (cnt == 0)
                    {
                        if (retry > 5)
                        {
                            flag_OK = false;
                            break;
                        }
                        this.UART_RX_BUF.Clear();
                        this.FLAG_UART_RX = false;
                        serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                        MyTimer_UART_TimeOut.TickStop();
                        MyTimer_UART_TimeOut.StartTickTime(100);
                        cnt++;
                    }
                    else if (cnt == 1)
                    {
                        if (MyTimer_UART_TimeOut.IsTimeOut())
                        {
                            cnt = 0;
                            retry++;
                        }
                        if (this.FLAG_UART_RX)
                        {
                            if (this.UART_RX_BUF.Count == 9)
                            {
                                int cnt_OK = 0;
                                byte[] bytes = new byte[7];
                                if (this.UART_RX_BUF[0] == (byte)station) cnt_OK++;
                                if (this.UART_RX_BUF[1] == 0x03) cnt_OK++;
                                if (this.UART_RX_BUF[2] == 0x04) cnt_OK++;
                                for (int i = 0; i < bytes.Length; i++)
                                {
                                    bytes[i] = (byte)this.UART_RX_BUF[i];
                                }
                                Basic.MyConvert.Get_CRC16(bytes, ref CRC_L, ref CRC_H);
                                if (this.UART_RX_BUF[7] == CRC_L) cnt_OK++;
                                if (this.UART_RX_BUF[8] == CRC_H) cnt_OK++;
                                if (cnt_OK == 5)
                                {
                                    List<byte> hex_bytes = new List<byte>();
                                    hex_bytes.Add((byte)this.UART_RX_BUF[3]);
                                    hex_bytes.Add((byte)this.UART_RX_BUF[4]);
                                    hex_bytes.Add((byte)this.UART_RX_BUF[5]);
                                    hex_bytes.Add((byte)this.UART_RX_BUF[6]);
                                    ID = Myconvert.ByteToStringHex(hex_bytes.ToArray());
                                    ID = ID.Replace("-", "");

                                    flag_OK = true;
                                    break;
                                }
                            }

                        }

                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
            return flag_OK;
        }
        public void Check_All_Staion(int num)
        {
            for (int i = 0; i < num; i++)
            {
                if (this.Command_Set_Beep(i))
                {
                    RFID_Device rFID_Device = new RFID_Device();
                    rFID_Device.station = i;
                    rFID_Device.UID = "00000000";
                    this.List_RFID_Device.Add(rFID_Device);
                    Console.WriteLine($"[RFID_FX600] check station {i} sucess ");
                }
                else
                {
                    Console.WriteLine($"[RFID_FX600] check station {i} failed ");
                }
            }
            System.Threading.Thread.Sleep(500);
            this.Invoke(new Action(delegate
            {
                this.listBox_Station.Items.Clear();
                for (int i = 0; i < this.List_RFID_Device.Count; i ++)
                {
                    this.listBox_Station.Items.Add(this.List_RFID_Device[i].station.ToString("00"));
                }
               
            }));
        }
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
        public bool SerialPortOpen()
        {
            if (!this.serialPort.IsOpen)
            {
                try
                {
                    this.serialPort.PortName = this.textBox_COM.Text;
                    this.serialPort.BaudRate = this.baudrate;
                    this.serialPort.DataBits = 8;
                    this.serialPort.Parity = System.IO.Ports.Parity.None;
                    this.serialPort.StopBits = System.IO.Ports.StopBits.One;
                    this.serialPort.Open();
                }
                catch
                {
                    this.IsOpen = false;
                    MessageBox.Show("COM Port open failed! RFID-FX600");
                    return false;
                }
            }
            this.IsOpen = true;
            return true;

        }
        public void SerialPortClose()
        {
            this.serialPort.Close();
            this.IsOpen = false;
        }

        #region Event
        private void button_Station_Write_Station_Click(object sender, EventArgs e)
        {
            int O_station = this.textBox_Old_Station.Text.StringToInt32();
            int N_station = this.textBox_New_Station.Text.StringToInt32();
            if (O_station == -1 || N_station == -1) MyMessageBox.ShowDialog("Old Station or New Staion is valid num!");
            if(this.Command_Set_Station(O_station, N_station))
            {
                MyMessageBox.ShowDialog(string.Format("Station change to <{0}> !", N_station.ToString("00")));
            }
            else
            {
                MyMessageBox.ShowDialog(string.Format("Station change failed!"));
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
            this.flag_IsFormClosing = true;
            this.SaveProperties();
            this.MyThread_RefreshUI.Stop();
            this.MyThread_RS485.Stop();
        }
        #endregion


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
