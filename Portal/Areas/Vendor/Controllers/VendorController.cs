using Portal.Common;
using Portal.DAL;
using Portal.Filters;
using Portal.PortalBL.PlansBL;
using Portal.PortalBL.UserBL;
using Portal.PortalBL.VendorBL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace Portal.Areas.Vendor.Controllers
{
    [UserAuthenticationFilter]
    public class VendorController : Controller

    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(VendorController));
        // GET: Vendor/Vendor
        public ActionResult Profile()
        {
            ViewBag.current_user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult UpdateUserProfile(UserViewModel userViewModel)
        {
            ResponseOut responseOut = new ResponseOut();

            IUserBL userBL = new UserEngine();
            try
            {

                if (userViewModel != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                    responseOut = userBL.UpdateUserProfile(userViewModel, user_id);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in VendorController/UpdateUserProfile", ex);
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatePassword(PasswordResetViewModel passwordViewModel)
        {
            ResponseOut responseOut = new ResponseOut();

            IUserBL userBL = new UserEngine();
            try
            {

                if (passwordViewModel != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                    responseOut = userBL.ResetUserPassword(passwordViewModel, user_id);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in VendorController/UpdatePassword", ex);
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetVendorProfile()
        {
            UserViewModel userProfile = new UserViewModel();
            IUserBL userBL = new UserEngine();
            int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
            try
            {
                userProfile = userBL.GetUserProfile(user_id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in VendorController/GetVendorProfile", ex);
                throw ex;
            }
            return Json(userProfile, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Plans()
        {
           
            return View();
        }

        public PartialViewResult _VendorPlan()
        {
            IPlansBL planBL = new PlansEngine();
            try
            {
                int? user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                var data = planBL.GetVendorPricingPlans(user_id);
                return PartialView(data);
            }
            catch (Exception ex)
            {
                Log.Error("Error in VendorController/Plans", ex);
                throw ex;
            }
            //return PartialView();
        }

        public JsonResult ShowInterestOnPlan(string plan)
        {
            SingleResponseOut<UserViewModel> responseOut = new SingleResponseOut<UserViewModel>();
            IVendorBL vendorBL = new VendorEngine();
            int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
            responseOut = vendorBL.ShowInterestOnPlan(user_id);
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   

            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplates/VendorInterestPlan.html")))
            {
                body = reader.ReadToEnd();
                if(plan=="Walker")
                {
                    body = body.Replace("{plan}", plan);
                }
                if (plan == "Runner")
                {
                    body = body.Replace("{plan}", plan);
                }
                if (plan == "NotSure")
                {
                    body = body.Replace("{plan}", plan);
                }
                body = body.Replace("{name}", responseOut.Model.firstname);
                body = body.Replace("{mobile}", responseOut.Model.contact);
                body = body.Replace("{Email}", responseOut.Model.email);
                bool status = Utilities.SendEmail("", "anish.sharma@infoxen.com", "Codelate | Interest Request from Vendor!", body, 587, true, "smtp.gmail.com", "", null, null);
                if (status)
                {
                    responseOut.message = "Email Sent !";
                    responseOut.status = ActionStatus.Success;
                }
                else
                {
                    responseOut.message = "Email Not Sent !";
                    responseOut.status = ActionStatus.Fail;
                }
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AddUpdatePlanSelectedByVendor(string plan_id)
        {
            int id = Convert.ToInt32(plan_id);
            ResponseOut responseOut = new ResponseOut();
            IPlansBL planBL = new PlansEngine();
            try
            {

                if (id != 0)
                {
                    int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                    responseOut = planBL.AddUpdateSelectPlanByVendor(id, user_id);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in VendorController/AddUpdatePlanSelectedByVendor", ex);
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }
    }
}