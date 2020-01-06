using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.Models
{
    public class ProductionModel
    {
        public int StyleId { get; set; }
        public string StyleName{get; set; }
        public int OperationId { get; set; }
        public string OperationName { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public string SMV { get; set; }
        public int EmployeeCode { get; set; }
        public string WorkDate { get; set; }
        public int BarcodeNo { get; set; }
    }
}