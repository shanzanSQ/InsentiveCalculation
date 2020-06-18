using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.Models
{
    public class PurchaseOrderModel
    {
        public int PoID { get; set; }
        public string PONumeber { get; set; }
        public int POQuantity { get; set; }
        public int PoPercentage { get; set; }
        public int TotalQuantity { get; set; }
        public int RemainingQuantity { get; set; }
    }
}