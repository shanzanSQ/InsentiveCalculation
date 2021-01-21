using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.Models
{
    public class ImageGalleryModel
    {
        public int SilhouetteId { get; set; }
        public string SilhouetteName { get; set; }
        public List<StyleUploadModal> ImageList { get; set; }
    }
}