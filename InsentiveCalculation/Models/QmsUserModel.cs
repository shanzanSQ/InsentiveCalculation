using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsentiveCalculation.Models
{
    public class QmsUserModel
    {
        public int QmsUserId { get;set;}
        public string BusinessUnitName { get;set;}
        public string PunitName { get;set;}
        public string FloorModuleName { get;set;}
        public string LineName { get;set;}
        public string UserName { get;set;}
    }
}