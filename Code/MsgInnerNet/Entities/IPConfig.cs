using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Common
{
    [Serializable]
    public class IPConfig
    {
        public double Index { get; set; }
        public String Key { get; set; }

        public string FormatKey 
        { 
            get 
            {
                string result = "";
                if (!String.IsNullOrWhiteSpace(Key)) 
                {
                    result= Key.ToLower();
                }
                return result;
            } 
        }

        public String Name { get; set; } 
        public String IPAddress { get; set; }
        public int Port { get; set; }
        public String Remark { get; set; }



    }
}
