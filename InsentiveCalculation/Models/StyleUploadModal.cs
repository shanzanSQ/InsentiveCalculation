using System;

namespace InsentiveCalculation.Models
{
    public class StyleUploadModal
    {
        public int SilhoutteImageId { get; set; }
        public string SilhoutteImageName { get; set; }
        public string SilhoutteImageDirectory { get; set; }
        public string ServerFileName { get; set; }
        public int FrontBack { get; set; }
        public int SilhouteeId { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
    }
}