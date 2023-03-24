using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsgInnerNet.Common
{
    [Serializable]
    public class TransferMsg
    { 
        public String Model { get; set; } 
        public String Content { get; set; }
        public String Remark { get; set; }
    }
}
