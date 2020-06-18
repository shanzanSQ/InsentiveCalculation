using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.Models
{
    public class ReportModel
    {
        public string EmployeeId { get; set;}
        public string NoOFBarcode { get; set;}
        public string TotalSMV { get; set;}
        public string DirectoryName { get; set;}
        public string FileName { get; set;}
        public int FileNameId { get; set; }
    }
}