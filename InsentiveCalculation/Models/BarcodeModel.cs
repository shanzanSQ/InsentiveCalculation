using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.Models
{
    public class BarcodeModel
    {
        public int BarcodeId { get; set; }
        public int BarcodeNumber { get; set; }
        public double StandardMinuteValue { get; set; }
        public int WorkerId { get; set; }
        public string WorkingDate { get; set; }
    }
}