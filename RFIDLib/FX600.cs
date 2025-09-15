using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Net;

namespace RFIDLib
{
    static public class FX600
    {

        static private List<int> UART_RX_BUF = new List<int>();
        static private System.IO.Ports.SerialPort serialPort = new SerialPort();
        static private bool isOpen = false;
        static private bool flag_init = false;
        static private bool FLAG_UART_RX = false;
        static public string _COM = "COM1";
        static public bool IsOpen
        {
            get
            {
                return isOpen;
            }
            private set
            {
                isOpen = value;
            }
        }

        /// <summary>
        /// 初始化序列埠監聽 (Initialize SerialPort)
        /// </summary>
        /// <remarks>
        /// 此方法用於初始化序列埠設定，並註冊 <c>DataReceived</c> 事件處理程序，
        /// 以便在有資料進入時能觸發回呼函式 <c>SerialPort_DataReceived</c>。  
        ///
        /// <b>邏輯流程：</b>
        /// 1. 檢查是否已經初始化 (<c>flag_init</c>)。  
        /// 2. 若尚未初始化，將 <c>SerialPort_DataReceived</c> 綁定至 <c>serialPort.DataReceived</c> 事件。  
        /// 3. 儲存傳入的 COM 埠名稱至內部變數 <c>_COM</c>。  
        /// 4. 將 <c>flag_init</c> 設為 <c>true</c>，避免重複註冊事件。  
        ///
        /// <b>注意事項：</b>  
        /// - 此方法僅註冊事件並記錄 COM 埠，並未真正開啟序列埠連線。  
        /// - 若要啟用通訊，需後續呼叫 <c>SerialPortOpen()</c> 開啟序列埠。  
        /// - 多次呼叫僅會在第一次註冊事件，其餘僅更新 <c>_COM</c> 值。  
        /// 
        /// </remarks>
        /// <param name="COM">指定的序列埠名稱 (例如: "COM3")。</param>
        /// <param name="baudrate">鮑率設定，預設為 9600 (此方法目前僅接收參數，未直接套用至 SerialPort)。</param>
        static public void init(string COM, int baudrate = 9600)
        {
            if (flag_init == false) serialPort.DataReceived += SerialPort_DataReceived;
            _COM = COM;
            flag_init = true;
        }

        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int byte2read = serialPort.BytesToRead;
            for (int i = 0; i < byte2read; i++)
            {
                UART_RX_BUF.Add(serialPort.ReadByte());
            }
            FLAG_UART_RX = true;
        }
        static public bool SerialPortOpen(int baudrate = 9600)
        {
            return SerialPortOpen(_COM, baudrate);
        }
        static public bool SerialPortOpen(string COM, int baudrate = 9600)
        {
            if (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.PortName = COM;
                    serialPort.BaudRate = baudrate;
                    serialPort.DataBits = 8;
                    serialPort.Parity = System.IO.Ports.Parity.None;
                    serialPort.StopBits = System.IO.Ports.StopBits.One;

                    serialPort.Open();

                    Console.WriteLine($"[INFO] Serial port {COM} opened at {baudrate} baud.");
                }
                catch (Exception ex)
                {
                    IsOpen = false;
                    Console.WriteLine($"[ERROR] Failed to open serial port {COM} at {baudrate} baud. Exception: {ex.Message}");
                    return false;
                }
            }
            else
            {
                Console.WriteLine($"[WARN] Serial port {COM} is already open.");
            }

            IsOpen = true;
            return true;
        }
        static public void SerialPortClose()
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                    Console.WriteLine("[INFO] Serial port closed.");
                }
                else
                {
                    Console.WriteLine("[WARN] Serial port was not open.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to close serial port. Exception: {ex.Message}");
            }
            finally
            {
                IsOpen = false;
            }
        }
        static public ushort Get_CRC16(byte[] pDataBytes)
        {
            ushort crc = 0xffff;
            ushort polynom = 0xA001;

            for (int i = 0; i < pDataBytes.Length; i++)
            {
                crc ^= pDataBytes[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x01) == 0x01)
                    {
                        crc >>= 1;
                        crc ^= polynom;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }

            return crc;
        }
        static public void Get_CRC16(byte[] pDataBytes, ref byte L_byte, ref byte H_byte)
        {
            ushort value = Get_CRC16(pDataBytes);
            L_byte = (byte)(value);
            H_byte = (byte)(value >> 8);
        }
        static public string ByteToStringHex(this byte[] value)
        {
            return BitConverter.ToString(value);
        }
        /// <summary>
        /// 讀取 7 Byte 卡片 ID (Get 7-Byte Card ID Command)
        /// </summary>
        /// <remarks>
        /// 此方法會透過序列埠向指定 <c>station</c> 發送「讀取卡號」命令，
        /// 嘗試獲取裝置回傳的 7 Byte 卡片 ID。  
        ///
        /// <b>邏輯流程：</b>
        /// 1. 檢查序列埠是否開啟，若未開啟則立即回傳 <c>false</c>。  
        /// 2. 組合 MODBUS RTU 封包：功能碼 <c>0x03</c>，起始位址 <c>0x0000</c>，長度 <c>0x0004</c>。  
        /// 3. 計算 CRC16 並附加於封包尾端。  
        /// 4. 發送封包，等待回應。  
        /// 5. 驗證回應長度 (13 bytes) 並檢查：  
        ///    - 位址是否一致。  
        ///    - 功能碼是否為 0x03。  
        ///    - 資料長度是否為 0x08。  
        ///    - CRC16 校驗是否正確。  
        /// 6. 若驗證成功，解析出 7 Byte 的卡號，轉換為 16 進制字串 (無 "-" 分隔)，存入 <c>ID</c>。  
        /// 7. 若超時或驗證失敗則重試，最多 5 次。  
        ///
        /// <b>Console.WriteLine 訊息：</b>  
        /// - [INFO]：發送指令與重試次數。  
        /// - [SUCCESS]：卡號讀取成功，並顯示卡號。  
        /// - [WARN]：回應 CRC 檢查失敗。  
        /// - [TIMEOUT]：等待回應超時。  
        /// - [FAIL]：達到最大重試次數仍失敗。  
        /// - [ERROR]：序列埠異常或例外錯誤。  
        ///
        /// <b>參數：</b>
        /// <param name="station">目標站號 (Station ID)，通常為裝置地址。</param>
        /// <param name="ID">
        /// 參考傳入的字串變數，用於接收卡號 (成功時為 7 Byte 卡號的十六進制字串，失敗時為 "0")。
        /// </param>
        ///
        /// <b>回傳：</b>
        /// <returns>
        /// - 成功讀取並驗證卡號時，回傳 <c>true</c> 並更新 <c>ID</c>。  
        /// - 若序列埠未開啟、超時、CRC 驗證失敗或例外狀況，則回傳 <c>false</c>。  
        /// </returns>
        /// </remarks>

        static public bool Command_Get_7CardID(int station, ref string ID)
        {
            bool flag_OK = false;

            if (!SerialPortOpen())
            {
                Console.WriteLine("[ERROR] Serial port is not open, cannot send Command_Get_7CardID.");
                return false;
            }

            ID = "0";
            byte CRC_L = 0;
            byte CRC_H = 0;

            // 組封包
            List<byte> list_byte = new List<byte>();
            list_byte.Add((byte)station);
            list_byte.Add((byte)(0x03));
            list_byte.Add(0x00);
            list_byte.Add(0x00);
            list_byte.Add(0x00);
            list_byte.Add(0x04);
            Get_CRC16(list_byte.ToArray(), ref CRC_L, ref CRC_H);
            list_byte.Add(CRC_L);
            list_byte.Add(CRC_H);

            int retry = 0;
            int maxRetry = 5;
            int timeoutMs = 100;

            while (retry <= maxRetry)
            {
                try
                {
                    UART_RX_BUF.Clear();
                    FLAG_UART_RX = false;

                    serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                    Console.WriteLine($"[INFO] Sent Command_Get_7CardID to station {station}, attempt {retry + 1}.");

                    var sw = System.Diagnostics.Stopwatch.StartNew();

                    while (sw.ElapsedMilliseconds < timeoutMs)
                    {
                        if (FLAG_UART_RX)
                        {
                            if (UART_RX_BUF.Count == 13)
                            {
                                int cnt_OK = 0;
                                byte[] bytes = new byte[11];

                                if (UART_RX_BUF[0] == (byte)station) cnt_OK++;
                                if (UART_RX_BUF[1] == 0x03) cnt_OK++;
                                if (UART_RX_BUF[2] == 0x08) cnt_OK++;

                                for (int i = 0; i < bytes.Length; i++)
                                    bytes[i] = (byte)UART_RX_BUF[i];

                                Get_CRC16(bytes, ref CRC_L, ref CRC_H);
                                if (UART_RX_BUF[11] == CRC_L) cnt_OK++;
                                if (UART_RX_BUF[12] == CRC_H) cnt_OK++;

                                if (cnt_OK == 5)
                                {
                                    List<byte> hex_bytes = new List<byte>();
                                    hex_bytes.Add((byte)UART_RX_BUF[3]);
                                    hex_bytes.Add((byte)UART_RX_BUF[4]);
                                    hex_bytes.Add((byte)UART_RX_BUF[5]);
                                    hex_bytes.Add((byte)UART_RX_BUF[6]);
                                    hex_bytes.Add((byte)UART_RX_BUF[7]);
                                    hex_bytes.Add((byte)UART_RX_BUF[8]);
                                    hex_bytes.Add((byte)UART_RX_BUF[9]);

                                    ID = ByteToStringHex(hex_bytes.ToArray()).Replace("-", "");
                                    Console.WriteLine($"[SUCCESS] Command_Get_7CardID station {station}, ID={ID}");
                                    return true;
                                }
                                else
                                {
                                    Console.WriteLine($"[WARN] Command_Get_7CardID station {station}, CRC check failed.");
                                }
                            }
                        }
                        System.Threading.Thread.Sleep(1);
                    }

                    Console.WriteLine($"[TIMEOUT] Command_Get_7CardID station {station}, attempt {retry + 1} timed out.");
                    retry++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Exception in Command_Get_7CardID: {ex.Message}");
                    return false;
                }
            }

            Console.WriteLine($"[FAIL] Command_Get_7CardID station {station} failed after {maxRetry} retries.");
            return flag_OK;
        }
        /// <summary>
        /// 發送蜂鳴器控制命令 (Set Beep Command)
        /// </summary>
        /// <remarks>
        /// 此方法會透過序列埠向指定 <c>station</c> 發送蜂鳴器控制命令，
        /// 驗證裝置是否正確回覆，以確認命令執行狀態。
        ///
        /// <b>邏輯流程：</b>
        /// 1. 檢查序列埠是否開啟，若未開啟則回傳 <c>false</c>。  
        /// 2. 組合 MODBUS RTU 指令封包 (功能碼 0x06，寄存器 0x0004，數值 0x0002)。  
        /// 3. 計算 CRC16 並附加於封包後方。  
        /// 4. 傳送封包至目標站號，等待回應。  
        /// 5. 驗證回應長度 (8 bytes) 並逐位比對與送出封包是否一致。  
        /// 6. 若回應完全正確，回傳 <c>true</c>；若超時或比對失敗，重試最多 5 次。  
        ///
        /// <b>Console.WriteLine 訊息：</b>  
        /// - [INFO]：發送指令與重試次數。  
        /// - [SUCCESS]：回應正確，命令成功。  
        /// - [WARN]：有回應，但資料不一致。  
        /// - [TIMEOUT]：等待超時。  
        /// - [FAIL]：達到最大重試次數仍失敗。  
        /// - [ERROR]：序列埠異常或例外錯誤。  
        ///
        /// <b>參數：</b>
        /// <param name="station">目標站號 (Station ID)，通常為裝置地址。</param>
        ///
        /// <b>回傳：</b>
        /// <returns>
        /// 若命令成功執行並收到正確回覆，回傳 <c>true</c>；  
        /// 若序列埠未開啟、超時、資料不符或發生錯誤，則回傳 <c>false</c>。
        /// </returns>
        /// </remarks>
        static public bool Command_Set_Beep(int station)
        {
            bool flag_OK = false;

            if (!SerialPortOpen())
            {
                Console.WriteLine("[ERROR] Serial port is not open, cannot send Command_Set_Beep.");
                return false;
            }

            byte CRC_L = 0;
            byte CRC_H = 0;
            List<byte> list_byte = new List<byte>();
            list_byte.Add((byte)station);
            list_byte.Add((byte)(0x06));
            list_byte.Add(0x00);
            list_byte.Add(0x04);
            list_byte.Add(0x00);
            list_byte.Add(0x02);
            Get_CRC16(list_byte.ToArray(), ref CRC_L, ref CRC_H);
            list_byte.Add(CRC_L);
            list_byte.Add(CRC_H);

            int retry = 0;
            int maxRetry = 5;
            int timeoutMs = 200;

            while (retry <= maxRetry)
            {
                try
                {
                    UART_RX_BUF.Clear();
                    FLAG_UART_RX = false;

                    serialPort.Write(list_byte.ToArray(), 0, list_byte.Count);
                    Console.WriteLine($"[INFO] Sent Command_Set_Beep to station {station}, attempt {retry + 1}.");

                    // 用 Stopwatch 模擬超時控制
                    var sw = System.Diagnostics.Stopwatch.StartNew();

                    while (sw.ElapsedMilliseconds < timeoutMs)
                    {
                        if (FLAG_UART_RX)
                        {
                            if (UART_RX_BUF.Count == 8)
                            {
                                int cnt_OK = 0;
                                for (int i = 0; i < 8; i++)
                                {
                                    if (UART_RX_BUF[i] == list_byte[i]) cnt_OK++;
                                }

                                if (cnt_OK == 8)
                                {
                                    Console.WriteLine($"[SUCCESS] Command_Set_Beep station {station} succeeded on attempt {retry + 1}.");
                                    return true;
                                }
                                else
                                {
                                    Console.WriteLine($"[WARN] Command_Set_Beep station {station} got response but mismatch.");
                                }
                            }
                        }
                        System.Threading.Thread.Sleep(1);
                    }

                    Console.WriteLine($"[TIMEOUT] Command_Set_Beep station {station}, attempt {retry + 1} timed out.");
                    retry++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Exception in Command_Set_Beep: {ex.Message}");
                    return false;
                }
            }

            Console.WriteLine($"[FAIL] Command_Set_Beep station {station} failed after {maxRetry} retries.");
            return false;
        }

        static public void serch_UID_session(string API_Server, string UID, ref string ID, ref string Name)
        {
            string url = $"{API_Server}/api/person_page/get_all";
            string json_in = "{}";

            Console.WriteLine("======================================");
            //Console.WriteLine($"[INFO] 呼叫 API → {url}");
            Console.WriteLine($"[INFO] 查詢 UID → {UID}");

            string json_out = WEBApiPostJson(url, json_in, false);
            Console.WriteLine($"[DEBUG] API 回傳 JSON 長度 = {json_out.Length}");

             FindIdAndNameByUID(json_out, UID , ref ID , ref Name);

            if (!string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(Name))
            {
                Console.WriteLine($"[SUCCESS] 找到 UID={UID}, ID={ID}, Name={Name}");
            }
            else
            {
                Console.WriteLine($"[WARN] 找不到 UID={UID} 對應的資料");
            }

            Console.WriteLine("======================================");
            return;
        }

        /// <summary>
        /// 在 JSON 字串中找出指定 UID 對應的 ID 與 Name (純字串解析，不使用 JSON 函式庫)
        /// </summary>
        /// <param name="json">完整 JSON 字串</param>
        /// <param name="targetUID">要查找的 UID</param>
        /// <returns>若找到回傳 (ID, Name)，否則回傳 (null, null)</returns>
        private static void FindIdAndNameByUID(string json, string targetUID , ref string ID , ref string Name)
        {
            string target_text = $"\"UID\":\"{targetUID}\"";
            int uidIndex = json.IndexOf(target_text, StringComparison.OrdinalIgnoreCase);
            if (uidIndex < 0) ;

            // 找出該物件範圍
            int objStart = json.LastIndexOf("{", uidIndex);
            int objEnd = json.IndexOf("}", uidIndex);
            if (objStart < 0 || objEnd < 0 || objEnd <= objStart) ;

            string objText = json.Substring(objStart, objEnd - objStart);

            // 擷取 ID
            string id = ExtractValue(objText, "\"ID\":");
            // 擷取 Name
            string name = ExtractValue(objText, "\"name\":");
            ID = id;
            Name = name;
            return;
        }
        /// <summary>
        /// 從物件片段裡解析某個 key 的字串值
        /// </summary>
        private static string ExtractValue(string objText, string key)
        {
            int keyIndex = objText.IndexOf(key, StringComparison.OrdinalIgnoreCase);
            if (keyIndex < 0) return null;

            int startQuote = objText.IndexOf("\"", keyIndex + key.Length);
            int endQuote = objText.IndexOf("\"", startQuote + 1);

            if (startQuote >= 0 && endQuote > startQuote)
            {
                return objText.Substring(startQuote + 1, endQuote - startQuote - 1);
            }
            return null;
        }

        public class MyTimerBasic
        {
            private bool OnTick = false;
            public double TickTime = 0;
            static private Stopwatch stopwatch = new Stopwatch();
            public MyTimerBasic()
            {
                stopwatch.Start();
                StartTickTime(999999);
            }
            public MyTimerBasic(double TickTime)
            {
                stopwatch.Start();
                StartTickTime(TickTime);
            }

            private double CycleTime_start;
            public void StartTickTime(double TickTime)
            {
                TickTime = TickTime;
                if (!OnTick)
                {
                    CycleTime_start = stopwatch.Elapsed.TotalMilliseconds;
                    OnTick = true;
                }
            }

            public void StartTickTime()
            {
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
                OnTick = false;
            }
            public bool IsTimeOut()
            {
                //if (OnTick == false) return false;
                if ((stopwatch.Elapsed.TotalMilliseconds - CycleTime_start) >= TickTime)
                {
                    OnTick = false;
                    return true;
                }
                else return false;
            }



            public override string ToString()
            {
                return ToString(true);
            }
            public string ToString(bool retick)
            {
                string text = GetTickTime().ToString("0.000") + "ms";
                if (retick)
                {
                    TickStop();
                    StartTickTime(999999);
                }
                return text;

            }

        }

        public class HttpContentType
        {
            public const string TEXT_PLAIN = "text/plain";
            public const string APPLICATION_JSON = "application/json";
            public const string APPLICATION_OCTET_STREAM = "application/octet-stream";
            public const string WWW_FORM_URLENCODED = "application/x-www-form-urlencoded";
            public const string WWW_FORM_URLENCODED_GB2312 = "application/x-www-form-urlencoded;charset=gb2312";
            public const string WWW_FORM_URLENCODED_UTF8 = "application/x-www-form-urlencoded;charset=utf-8";
            public const string MULTIPART_FORM_DATA = "multipart/form-data";
        }
        /// <summary>
        /// 呼叫 Web API (POST JSON) - 適用於 .NET Framework 3.5
        /// </summary>
        public static string WEBApiPostJson(string url, string value, bool debug)
        {
            string responseBody = "";
            if (string.IsNullOrEmpty(url))
            {
                Console.WriteLine("網址不得為空");
                return null;
            }

            try
            {
                MyTimerBasic myTimerBasic = new MyTimerBasic();
                myTimerBasic.StartTickTime(50000);

                // 忽略 SSL 憑證驗證 (適用於自簽憑證)
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                byte[] data = Encoding.UTF8.GetBytes(value);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json; charset=UTF-8";
                request.ContentLength = data.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseBody = reader.ReadToEnd();
                }

                return responseBody;
            }
            catch (Exception ex)
            {
                if (debug)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                return responseBody;
            }
        }

 
    }
}
