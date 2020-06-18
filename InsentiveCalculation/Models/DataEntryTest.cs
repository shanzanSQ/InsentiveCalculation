using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.Models
{
    public class DataEntryTest
    {
        public int RequestId { get; set; }
        public int RequestFor { get; set; }
        public int BusinessUnitId { get; set; }
        public int BuyerId { get; set; }
        public string Priority { get; set; }
        public string Instructions { get; set; }

    }
}