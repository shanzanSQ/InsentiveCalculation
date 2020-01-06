using InsentiveCalculation.DAL;
using InsentiveCalculation.Models;
using SQIndustryThree.Models;
using System;
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
                result.isSuccess = false;
                result.msg = Url.Action("ReadQRCode", "Home");
                Session["IncentiveUserId"] = users.UserInformationId;
                Session["IncentiveUserName"] = users.UserInformationName;
                Session["IncentiveUserDes"] = users.DesignationId;
                return Json(result, JsonRequestBehavior.AllowGet);
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
    }



}