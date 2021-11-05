using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gtv_tele.DataAccess
{
    public class SettingSender
    {
        public int SettingSenderId { get; set; }
        public string SettingSenderName { get; set; }
        public string Number_Sender { get; set; }
        public int ApiId_Sender { get; set; }
        public string ApiHash_Sender { get; set; }        
    }
}