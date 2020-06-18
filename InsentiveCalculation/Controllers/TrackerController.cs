using InsentiveCalculation.DAL;
using InsentiveCalculation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsentiveCalculation.Controllers
{
    public class TrackerController : Controller
    {
        HomeDAL homeDal = new HomeDAL();
        // GET: Tracker
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateTracker()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }
        public ActionResult ShowCoOrdinateView()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }
        public ActionResult DataEntryView()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }
        [HttpPost]
        public ActionResult SubmitValueToMerchandise(DataEntryTest dataEntryTest)
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            int primarykey = homeDal.SaveDataEntryTable(dataEntryTest, userid);
            return Json(primarykey,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ShowPartialView()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            return PartialView("_ShowListData");
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
            Businessunitlist = homeDal.GetBusinessUnit(userid);
            return Json(Businessunitlist, JsonRequestBehavior.AllowGet);
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
            buyerlist = homeDal.GetBuyer(userid);
            return Json(buyerlist, JsonRequestBehavior.AllowGet);
        }
    }
}