using InsentiveCalculation.DAL;
using InsentiveCalculation.Models;
using SQIndustryThree.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace InsentiveCalculation.Controllers
{
    public class AccountController : Controller
    {
        AccountDAL accountDAL = new AccountDAL();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TestView()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckLogin(string UserEmail, string UserPassword)
        {
            ResultResponse result = new ResultResponse();
            UserInformation users = accountDAL.CheckUserLogin(UserEmail, UserPassword);
            if (users.Empty)
            {
                result.isSuccess = true;
                result.msg = "Wrong Username Or Password";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {

                List<ModuleClassModel> moduleList = new List<ModuleClassModel>();
                moduleList = accountDAL.GetModuleByuUser(users.UserInformationId);
                if (moduleList.Count <= 0)
                {
                    result.isSuccess = true;
                    result.msg = "You Don't Have Permission To This System";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.isSuccess = false;
                    result.msg = Url.Action(moduleList[0].ModuleValue, moduleList[0].ModuleController);
                    Session["IncentiveUserId"] = users.UserInformationId;
                    Session["IncentiveUserName"] = users.UserInformationName;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }

        }
        public ActionResult Logout()
        {
            bool result = true;
            Session.Abandon();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProfileView()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }
        public ActionResult GetUserInformation()
        {

            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userID = Convert.ToInt32(Session["IncentiveUserId"].ToString());
            UserInformation users = accountDAL.GetUserInformation(userID);
            return Json(users, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RecoveryPassword()
        {

            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userID = Convert.ToInt32(Session["IncentiveUserId"].ToString());
            bool result = accountDAL.RecoveryPassword(userID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangePassword(string email, string oldpass, string newpass)
        {
            bool result = false;
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userID = Convert.ToInt32(Session["IncentiveUserId"].ToString());
            UserInformation users = accountDAL.CheckUserLogin(email, oldpass);
            if (users.Empty)
            {
                result = false;
            }
            else
            {
                result = accountDAL.changePassword(userID, newpass);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadPermissionMenu()
        {
            if (Session["IncentiveUserId"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            int userid = Convert.ToInt32(Session["IncentiveUserId"]);
            List<ModuleClassModel> moduleList = new List<ModuleClassModel>();
            moduleList = accountDAL.GetModuleByuUser(userid);
            return Json(moduleList, JsonRequestBehavior.AllowGet);
        }
    }
}