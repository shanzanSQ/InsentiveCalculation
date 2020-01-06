using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.Models
{
    public class QRReaderValue
    {
        public int QRreadPrimary { get; set; }
        public int QRNo { get; set; }
        public string Style { get; set; }
        public string Operation { get; set; }
        public string PurchaseOrder { get; set; }
        public string Quantity { get; set; }
        public double StandardMinuteValue { get; set; }
        public string ColorCode { get; set; }
        public string Size { get; set; }
        public int WorkerId { get; set; }
        public string Date { get; set; }
        public string datasource { get; set; }
    }
}