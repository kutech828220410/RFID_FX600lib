using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using Basic;
using System.Diagnostics;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Drawing.Drawing2D;


namespace RFID_FX600lib
{
    public class UDP_Class
    {
        public delegate void DataReciveEventHandler(string IP, int Port, string readline);
        public event DataReciveEventHandler DataReciveEvent;

        MyThread MyThread_Program;
        IPEndPoint remoteIP;
        Socket Client;
        byte[] data = new byte[4096]; //存放接收的資料

        public bool IsDataRecive
        {
            get
            {
                return (_readline != "");
            }
        }
        private string _readline = "";
        public string readline
        {
            get
            {

                return this._readline;
            }
            set
            {
                this._readline = value;
            }
        }
        public UDP_Class(string IP, int port)
        {
            try
            {
                this.remoteIP = new IPEndPoint(IPAddress.Parse(IP), port); //定義一個位址 (伺服器位址)
                this.Client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); //傳輸模式為UDP
                IPEndPoint IPEnd = new IPEndPoint(IPAddress.Any, port);
                this.Client.Bind(IPEnd);
                this.Client.SendBufferSize = 4096;
            }
            catch
            {
                return;
            }
            this.MyThread_Program = new MyThread();
            this.MyThread_Program.Add_Method(sub_ReadByte);
            this.MyThread_Program.AutoRun(true);
            this.MyThread_Program.SetSleepTime(10);
            this.MyThread_Program.Trigger();
        }
        public void WriteByte(byte[] value)
        {
            this.Client.SendTo(value, remoteIP); //送出的資料跟目的          
        }
        void sub_ReadByte()
        {
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0); //定一個空端點(唯讀)
            EndPoint Remote = (EndPoint)sender;
            try
            {
                int recv = Client.ReceiveFrom(data, ref Remote); //(收到的資料,來自哪個IP放進Remote) 不能放IPEndPoint 好像是它唯獨的關係 這時候sender已經變成跟remoteIP一樣
                this.readline = Encoding.UTF8.GetString(data, 0, recv);
                if (this.DataReciveEvent != null) this.DataReciveEvent(((IPEndPoint)(Remote)).Address.ToString(), ((IPEndPoint)(Remote)).Port, Encoding.UTF8.GetString(data, 0, recv));
            }
            catch
            {

            }

        }
        public void Dispose()
        {
            if (MyThread_Program != null) MyThread_Program.Stop();
            if (Client != null)
            {
                Client.Close();
                Client.Dispose();
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
        public MyTimer(string adress)
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
