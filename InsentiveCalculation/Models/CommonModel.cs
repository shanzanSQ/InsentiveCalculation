using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.Models
{
    public class CommonModel
    {
        public int BuyerId { get; set; }
        public string BuyerName { get; set; }
        public int CatagoryId { get; set; }
        public string CatagoryName { get; set; }

        public int StyleId { get; set; }
        public string StyleName { get; set; }

        public int BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }

        public int OperationId { get; set; }
        public int OperationNumber { get; set; }
        public string OperationName { get; set; }
        public string OperationSection { get; set; }
        public float OperationSMV { get; set; }

        public List<CommonModel> OperationList { get; set; }
    }
}