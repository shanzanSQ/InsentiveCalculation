using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.Models
{
    public class MasterSilhoteeTable
    {
        public int MasterSilhouteeId { get; set; }
        public int BuyerId { get; set; }
        public int StyleId { get; set; }
        public int CatagoryId { get; set; }
        public int BusinessUnitId { get; set; }
        public string CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int UserId { get; set; }
        public int ShiloteeCreated { get; set; }
        public string StyleName { get; set; }
        public string BuyerName { get; set; }
        public string UnitName { get; set; }
        public string SilhouetteName { get; set; }
        public List<StyleUploadModal> Imagelist { get; set; }
        public List<DefectPostionModel> FrontPositionList { get; set; }
        public List<DefectPostionModel> BackPostionList { get; set; }
    }
}