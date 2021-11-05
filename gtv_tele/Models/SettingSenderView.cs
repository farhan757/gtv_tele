using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace gtv_tele.Models
{
    public class SettingSenderView
    {                
        [Display(Name = "Number Sender")]
        public string Number_Sender { get; set; }
        [Display(Name = "Hash Code")]
        public string Hash_Code { get; set; }
        [Display(Name = "Code OTP")]
        public string Code_OTP { get; set; }
    }
}