using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gtv_tele.DataAccess
{
    public class Body_Message
    {
        public int Body_MessageId {  get; set; }
        public int VariableToBodyId { get; set; }
        public String Nama_Body { get;set;  }
        public String Body { get; set; }
    }
}