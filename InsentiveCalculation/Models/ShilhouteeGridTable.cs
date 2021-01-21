using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.Models
{
    public class ShilhouteeGridTable
    {
        public int SilhouteeGridId { get; set; }
        public int GridNo { get; set; }
        public int DefectPositionId { get; set; }
        public int MasterSilhouteeId { get; set; }
        public int FrontorBack { get; set; }
        public string DefectPositionName { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
    }
}