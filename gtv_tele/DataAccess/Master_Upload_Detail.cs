using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gtv_tele.DataAccess
{
    public class Master_Upload_Detail
    {
        public int Master_Upload_DetailId { get; set; }
        public int Master_UploadId { get; set; } 
        public string NoHP { get;set;  } 
        public string Nama { get; set; } 
        public string Account { get; set; } 
        public string Body_Message { get; set; }
        public string Short_Link { get; set; } 
        public string Path_PDF { get; set; } 
        public int Send { get; set; } 
        public DateTime Send_at { get; set; } 
        public int Status { get; set; } 
        public string Message_Status { get; set; } 
        public int Read_Total { get; set; } 
        public DateTime Read_at { get; set; } 
        public string Keterangan { get; set; } 
    }
}