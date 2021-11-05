using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gtv_tele.DataAccess
{
    public class VariableToBody
    {
        public int VariableToBodyId {get;set;}
        public string VariableToBodyName { get; set; }        
    }

    public class VariableToBodyDetail
    {
        public int VariableToBodyDetailId { get; set; }
        public int VariableToBodyId { get; set; }
        public string VariableName { get; set; }
        public string VariableField { get; set; }
    }
}