using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gtv_tele.DataAccess
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }

    public class Project
    {
        public int ProjectId { get; set; }
        public int CustomerId { get; set; }
        public int Body_MessageId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDesc { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}