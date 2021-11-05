using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gtv_tele.DataAccess
{
    public class Menus
    {
        public int id {  get; set; }
        public int order { get; set; }
        public string nama_menu { get; set; }
        public int parent { get; set; }
        public bool active { get; set; }
        public string desc { get; set; }
        public string controller { get; set; }
        public string method { get; set; }
        public string icon { get; set; }
    }
}