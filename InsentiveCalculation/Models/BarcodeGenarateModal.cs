using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.Models
{
    public class BarcodeGenarateModal
    {
        public int StyleID { get; set; }
        public string StyleName { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public int BusinessUnitId { get; set; }
        public string SelectDate { get; set; }
        public int PONumber { get; set; }
        public string BundleSize { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public int CutNo { get; set; }
        public int ShadeNO { get; set; }
        public string ProductName { get; set; }
        public int NoOfBundle { get; set; }
        public List<CommonModel> OprationList { get; set; }
    }
}