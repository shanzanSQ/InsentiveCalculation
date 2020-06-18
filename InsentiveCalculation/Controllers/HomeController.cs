using InsentiveCalculation.DAL;
using InsentiveCalculation.Models;
using SQIndustryThree.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsentiveCalculation.Controllers
{
    public class HomeController : Controller
    {
        AccountDAL accountDAL = new AccountDAL();
        HomeDAL homeDAL = new HomeDAL();
        public ActionResult ReadQRCode()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            //int permission = accountDAL.ModulePermission(7, userid);
            //if (permission != 1)
            //{
            //    return RedirectToAction("RequestPermission", "Home");
            //}
            return View();
        }
        public ActionResult Report()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return View();
        }

        public ActionResult SearchReportAll(string Workdate,int BusinessUnitId)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return PartialView("_ReportPartialView", homeDAL.SearchReportByDate(Workdate,BusinessUnitId));
        }

        public ActionResult AddNewWorkerId(string EmployeeId,int FileId)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            return Json(homeDAL.AddEmployeeByFileID(FileId,EmployeeId),JsonRequestBehavior.AllowGet);
        }

        public ActionResult RequestPermission()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }

        public ActionResult WorkerSticker()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }

        public ActionResult GenarateStickers()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            //int permission = accountDAL.ModulePermission(5, userid);
            //if (permission != 1)
            //{
            //    return RedirectToAction("RequestPermission", "Home");
            //}
            return View();
        }

        public ActionResult AddInformation()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            //int permission = accountDAL.ModulePermission(6, userid);
            //if (permission != 1)
            //{
            //    return RedirectToAction("RequestPermission", "Home");
            //}
            return View();
        }
        public ActionResult ReadQRfromSource(String selectdate)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            int result=homeDAL.ReadQrCode(selectdate);
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        public void DownloadFile(string filename)
        {
            string fname = Path.GetFileName(filename);
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + fname);
            string aaa = Server.MapPath("~/GenaratePDF/" + fname);
            Response.TransmitFile(Server.MapPath("~/GenaratePDF/" + fname));
            Response.End();
        }
        [HttpPost]
        public ActionResult AddStyletOdatabase(String StyleName,int BuyerId)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            bool result=homeDAL.SaveStyleToDataBAse(StyleName,BuyerId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddPoToDatabase(string PoNumber,int StyleId,int numberProduction,int maxNumber)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            bool result = homeDAL.SavePotoDatabase(PoNumber,StyleId,numberProduction,maxNumber,userid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadPoFromDatabase(int StyleID)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            List<PurchaseOrderModel> purchaseOrderList = new List<PurchaseOrderModel>();
            purchaseOrderList = homeDAL.GetPoFromDatabaseByStyle(StyleID,userid);
            return Json(purchaseOrderList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadBuyer()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            List<CommonModel> buyerlist = new List<CommonModel>();
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            buyerlist = homeDAL.GetBuyer(userid);
            return Json(buyerlist,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LoadStyle(int BuyerId)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            List<CommonModel> stylelist = new List<CommonModel>();
            stylelist = homeDAL.GetStyle(BuyerId);
            return Json(stylelist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetWorkerSearch(string BusinessUnitId, string Department, string Section, string Subsection, string Line)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            List<UserInformation> userInformation = new List<UserInformation>();
            userInformation = homeDAL.GetWorkersSearch(BusinessUnitId,Department,Section,Subsection,Line);
            return PartialView("_workerPertialView", userInformation);
        }
        [HttpPost]
        public ActionResult GenarateBarcodeWorker(List<UserInformation> alocatuserlist)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            ResultResponse resultResponse = homeDAL.GenerateWorkerBarcode(alocatuserlist);
            return Json(resultResponse,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadOperations()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            List<CommonModel> operationList = new List<CommonModel>();
            operationList = homeDAL.GetOperations();
            return Json(operationList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LoadBusinessUnit()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<CommonModel> Businessunitlist = new List<CommonModel>();
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            Businessunitlist = homeDAL.GetBusinessUnit(userid);
            return Json(Businessunitlist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadStyleWiseOper(String StyleId)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<CommonModel> loadoperationList = new List<CommonModel>();
            if (StyleId =="0")
            {
                loadoperationList = homeDAL.GetOperations();
            }
            else
            {
                loadoperationList = homeDAL.LoadStyleWiseOperation(StyleId);
            }
            
            return Json(loadoperationList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GenarateBarcodesFromInput(BarcodeGenarateModal barcodeGenarateModal)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            ResultResponse resultResponse = new ResultResponse();
            resultResponse = homeDAL.SaveBarcodes(barcodeGenarateModal,userid);
            return Json(resultResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveStylewiseOperation(CommonModel commonModel)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            ResultResponse resultResponse = new ResultResponse();
            resultResponse = homeDAL.SaveOperaTionToStyle(commonModel, userid);
            return Json(resultResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteStyleOperation(CommonModel commonModel)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            ResultResponse resultResponse = new ResultResponse();
            resultResponse = homeDAL.DeleteOperationFromStyle(commonModel, userid);
            return Json(resultResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteStyle(int StyleId)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            bool result = homeDAL.DeleteStyle(StyleId, userid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PoInformationByID(int PoID)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            PurchaseOrderModel poinformation = homeDAL.GetPOInformationById(PoID, userid);
            return Json(poinformation, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeDivView(int status)
        {
            string viewName = "";
            switch (status)
            {
                case 0:
                    viewName = "_addPertialStyle";
                    break;
                case 1:
                    viewName = "_addPartialOperation";
                    break;
                case 2:
                    viewName = "_addPartialSection";
                    break;
                case 3:
                    viewName = "_addOperationToStyle";
                    break;
                case 4:
                    viewName = "_addPurchaseOrder";
                    break;
            }
            return PartialView(viewName);

        }
        [HttpPost]
        public ActionResult SearchEmployeeBYCode(string EmployeeCode,int BusinessUnitId)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            List<UserInformation> userInformation = new List<UserInformation>();
            userInformation = homeDAL.GetWorkerByCode(EmployeeCode,userid,BusinessUnitId);
            return PartialView("_workerPertialView", userInformation);
        }

    }
}