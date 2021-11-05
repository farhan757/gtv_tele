using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace gtv_tele.Models
{
    public class SenderView
    {
        public string number { get; set; }
        public string Cycle { get; set; }
        public int Part { get; set; }
        public int Project { get; set; }
        public int Customer { get; set; }
        [Required]
        [Display(Name = "TxtFile")]
        public HttpPostedFileBase TxtFile { get; set; }
    }

    public class SenderModel
    {
        public string NoReceipt { get; set; }
        public string NamaReceipt { get; set; }
        public string Message { get; set; }
    }
}