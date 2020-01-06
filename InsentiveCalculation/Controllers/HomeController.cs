using InsentiveCalculation.DAL;
using InsentiveCalculation.Models;
using System;
using System.Collections.Generic;
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
            int permission = accountDAL.ModulePermission(6, userid);
            if (permission != 1)
            {
                return RedirectToAction("RequestPermission", "Home");
            }
            return View();
        }

        public ActionResult RequestPermission()
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
            int permission = accountDAL.ModulePermission(5, userid);
            if (permission != 1)
            {
                return RedirectToAction("RequestPermission", "Home");
            }
            return View();
        }

        public ActionResult AddInformation()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            int permission = accountDAL.ModulePermission(7, userid);
            if (permission != 1)
            {
                return RedirectToAction("RequestPermission", "Home");
            }
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
        [HttpPost]
        public ActionResult ShowStylesFromDatabase()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            List<ProductionModel> productionStyleList = new List<ProductionModel>();
            productionStyleList = homeDAL.showStyleList();
            return PartialView("_showStyle", productionStyleList);
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
            }
            return PartialView(viewName);

        }
    }
}