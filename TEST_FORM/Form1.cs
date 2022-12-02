using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basic;
namespace TEST_FORM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int station = this.textBox_Station.Text.StringToInt32();
            if (station > 0)
            {
                this.rfiD_FX600_UI1.Command_Set_Beep(station);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.comboBox_COM_Baudrate.SelectedIndex = 1;
            this.comboBox_Baudrate.SelectedIndex = 1;
            this.rfiD_FX600_UI1.Init(RFID_FX600lib.RFID_FX600_UI.Baudrate._9600);

            this.rfiD_ESP32_UI1.Init();
        }

        private void button_Set_Baudrate_Click(object sender, EventArgs e)
        {
            int station = this.textBox_Station.Text.StringToInt32();
            if (station > 0)
            {
                if (this.rfiD_FX600_UI1.Command_Set_Baudrate(station, (RFID_FX600lib.RFID_FX600_UI.Baudrate)this.comboBox_Baudrate.SelectedIndex + 1))
                {
                    MessageBox.Show("OK");
                }
                else
                {
                    MessageBox.Show("NG");
                }
            }
         
        }

        private void button_ChangeBaudrate_Click(object sender, EventArgs e)
        {
            this.rfiD_FX600_UI1.ChangeBaudrate((RFID_FX600lib.RFID_FX600_UI.Baudrate)this.comboBox_COM_Baudrate.SelectedIndex + 1);
            MessageBox.Show("OK");
        }

        private void comboBox_Baudrate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            List<object[]> list = this.rfiD_ESP32_UI1.List_RFID;
        }
    }
}
