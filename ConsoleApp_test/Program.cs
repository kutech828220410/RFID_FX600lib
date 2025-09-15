using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_test
{
    class Program
    {
        static void Main(string[] args)
        {
            RFIDLib.FX600.init("COM4");
            RFIDLib.FX600.Command_Set_Beep(1);
            string ID = "";
            RFIDLib.FX600.Command_Get_7CardID(1, ref ID);
            Console.ReadKey();
        }
    }
}
