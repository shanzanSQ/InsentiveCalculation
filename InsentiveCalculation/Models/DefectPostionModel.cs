using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.Models
{
    public class DefectPostionModel
    {
        public int DefectPoistionId { get; set; }
        public int GridNo { get; set; }
        public int CategoryId { get; set; }
        public string DefectPositionName { get; set; }
        public string DefectColor { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public List<DefectPostionModel> DefectList { get; set; }
    }
}