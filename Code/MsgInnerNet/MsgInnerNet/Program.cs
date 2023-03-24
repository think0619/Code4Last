using MsgInnerNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MsgInnerNet
{
    internal static class Program
    {

        public static List<IPConfig> IPConfigList = null; //IP

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false); 

            string ipdata = System.IO.File.ReadAllText(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"Data\msgip.json"), Encoding.Default);
            List<IPConfig> ipdataList = JsonHelper.JsonHelper.DeserializeJsonToList<IPConfig>(ipdata);
            IPConfigList = ipdataList;

            Application.Run(new Form1());
        }
    }
}
