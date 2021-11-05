using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gtv_tele.DataAccess
{
    public class History_Read
    {
        public int History_ReadId { get; set; }
        public int Master_Upload_DetailId { get; set; }
        public string Ip { get; set; }
        public DateTime Read_at { get; set; }
    }

    public class Verify_Code
    {
        public Guid Verify_CodeId { get; set; }
        public int Master_Upload_DetailId {  get; set; }
    }
}