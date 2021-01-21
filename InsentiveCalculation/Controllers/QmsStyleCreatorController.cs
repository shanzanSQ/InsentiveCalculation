using InsentiveCalculation.DAL;
using InsentiveCalculation.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsentiveCalculation.Controllers
{
    public class QmsStyleCreatorController : Controller
    {
        // GET: QmsStyleCreator
        QmsAppDAL qmsDAL = new QmsAppDAL();
        public ActionResult StyleCreator()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }
        public ActionResult SilhoutteUploadMenu()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }
        public ActionResult UploadSilhoutte()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            string silhoutteName = "";int catagoryId = 0;
            List<StyleUploadModal>  fileModelList = new List<StyleUploadModal>();
            if (Request.Files.Count > 0)
            {
                var files = Request.Files;
                silhoutteName= Request.Form["SilhoutteName"];
                catagoryId=Convert.ToInt32(Request.Form["CatagoryId"]);
                int count = 0;
                foreach (string str in files)
                {
                    count++;
                    HttpPostedFileBase file = Request.Files[str] as HttpPostedFileBase;
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        Stream strm = file.InputStream;
                        using (var image = System.Drawing.Image.FromStream(strm))
                        {
                            int newWidth = 766; // New Width of Image in Pixel  
                            int newHeight = 588; // New Height of Image in Pixel  
                            var thumbImg = new Bitmap(newWidth, newHeight);
                            var thumbGraph = Graphics.FromImage(thumbImg);
                            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            var imgRectangle = new Rectangle(0, 0, newWidth, newHeight);
                            thumbGraph.DrawImage(image, imgRectangle);
                            var currentmilse = DateTime.Now.Ticks;
                            var InputFileName = Path.GetFileNameWithoutExtension(file.FileName);
                            var InputFileExtention = Path.GetExtension(file.FileName);
                            var FullFileWithext = InputFileName + currentmilse + InputFileExtention;
                            var ServerSavePath = Path.Combine(Server.MapPath("~/StyleUpload/") + FullFileWithext);
                            thumbImg.Save(ServerSavePath, image.RawFormat);
                            StyleUploadModal fileUploadModel = new StyleUploadModal();
                            fileUploadModel.SilhoutteImageName = file.FileName.ToString();
                            fileUploadModel.SilhoutteImageDirectory = ServerSavePath;
                            fileUploadModel.ServerFileName = FullFileWithext;
                            fileUploadModel.FrontBack = count;
                            fileModelList.Add(fileUploadModel);
                        }
                    }
                }

            }
            ResultResponse result = qmsDAL.UploadSilhoutte(silhoutteName,catagoryId,fileModelList);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetImageGallery(int CategoryType)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            return PartialView("_imageGallery", qmsDAL.GetImageForgalary(CategoryType));
        }
        public ActionResult GetCategoryType()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return Json(qmsDAL.GetCatagoryType(userid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserUnits()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return Json(qmsDAL.GetUnitsfromDatabase(userid), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllDefectPosition(int Status,int CategoryId)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return Json(qmsDAL.GetAllDefectPosition(Status,CategoryId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDefectByPosition(string PositionId)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return Json(qmsDAL.GetDefectByPosition(PositionId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegisterStyle(int BusinessUnitId,int BuyerId,int StyleId,int SilhoutteId,int CategoryID)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            ResultResponse result = new ResultResponse();
            result = qmsDAL.RegisterSilhoutte(BusinessUnitId, BuyerId, StyleId, SilhoutteId,CategoryID,userid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CopyRegisteredStyle(int StyleId,int MasterSilhoutteId)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            ResultResponse result = new ResultResponse();
            result = qmsDAL.CopySilhoutteToStyle(StyleId, MasterSilhoutteId,userid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInformationMasterTable(int MasterId)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            return PartialView("_silhoutteGridInformation", qmsDAL.GetMasterSilhoutteInformation(MasterId));
        }
        public ActionResult GetsilhoutteList()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return PartialView("_masterSilhoutteListData", qmsDAL.GetSilhoutteTableList(userid));
        }
        [HttpPost]
        public ActionResult ChangeDivView(int status)
        {
            //string viewName = "";
            //switch (status)
            //{
            //    //case 0:
            //    //    viewName = "_addPertialStyle";
            //    //    break;
                
            //}
            return PartialView("_createStyleRegistration");

        }

        public ActionResult DefectEntry()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }
        public ActionResult AllQmsUserInfo()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return View(qmsDAL.AllQmsUserInformation(userid));
        }
        public ActionResult AddOrDeleteDefect(int DefectKey,string DefectName,int CatagoryId)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return Json(qmsDAL.AddOrDeleteDefect(DefectKey,DefectName, CatagoryId, userid),JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddDefectToPosition(DefectPostionModel DefectPostionModel)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return Json(qmsDAL.AddDefectToPosition(DefectPostionModel,userid), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AssignPositionToSilhoutee(List<ShilhouteeGridTable> silhouteeGridList)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return Json(qmsDAL.AssignDefectPosition(silhouteeGridList,userid), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ShowGridByDefectPosition(int DefectPositionId,int MasterSilhouetteId,int FrontOrBack)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return Json(qmsDAL.ShowGridByDefectPosition(DefectPositionId,MasterSilhouetteId,FrontOrBack), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeletePositionFromGrid(int DefectPositionId, int MasterSilhouetteId, int FrontOrBack)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return Json(qmsDAL.DeletePositionFromGrid(DefectPositionId, MasterSilhouetteId, FrontOrBack), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteRegisteredStyle(int MasterSilhouetteId)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return Json(qmsDAL.DeleteRegisteredStyle(MasterSilhouetteId), JsonRequestBehavior.AllowGet);
        }
    }
}