using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gtv_tele.DataAccess
{
    public class Master_Upload
    {
        public int Master_UploadId { get;set;  }
        public int CustomerId { get;set; }
        public int ProjectId { get; set; }
        public string Cycle { get; set; }
        public int Part { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int UserId { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}