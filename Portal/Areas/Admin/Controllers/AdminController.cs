using Portal.Common;
using Portal.Filters;
using Portal.PortalBL.AdminBL;
using Portal.PortalBL.UserBL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using Portal.PortalBL.PlansBL;
using Portal.DAL;

namespace Portal.Areas.Admin.Controllers
{
    [UserAuthenticationFilter]
    public class AdminController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AdminController));
        // GET: Admin/Admin
        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult AllPostRequirement(string title = null)
        {
            List<PostYourRequirement> postRequirement = new List<PostYourRequirement>();
            IAdminBL adminBL = new AdminEngine();
            try
            {
                int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                postRequirement = adminBL.GetAllPostRequirement(title);
            }
            catch (Exception ex)
            {
                Log.Error("Error in AdminController/AllPostRequirement", ex);
                throw ex;
            }
            return View(postRequirement);
        }

        [HttpPost]
        public ActionResult SubmitRequirementStatus(SubmitYourRequirement statusViewModel)
        {
            ResponseOut responseOut = new ResponseOut();

            IAdminBL adminBL = new AdminEngine();
            try
            {

                if (statusViewModel != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                    responseOut = adminBL.SubmitPostStatus(statusViewModel);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in AdminController/SubmitRequirementStatus", ex);
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        public ActionResult User()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult GetAllUserList()
        {
            List<UserViewModel> user = new List<UserViewModel>();
            IUserBL userBL = new UserEngine();
            try
            {
                user = userBL.GetUserList();
            }
            catch (Exception ex)
            {
                Log.Error("Error in AdminController/GetAllUserList", ex);
                throw ex;
            }
            return PartialView(user);
        }

        [HttpGet]
        public ActionResult WhatClientSays()
        {
            List<WhatClientSays> feedback = new List<WhatClientSays>();
            IAdminBL adminBL = new AdminEngine();
            try
            {
                feedback = adminBL.GetClientFeedback(null);
            }
            catch (Exception ex)
            {
                Log.Error("Error in AdminController/WhatClientSays", ex);
                throw ex;
            }
            return View(feedback);
        }

        [HttpGet]
        public ActionResult WhatClientSaysDetail(int id)
        {
            WhatClientSays feedback = new WhatClientSays();
            IAdminBL adminBL = new AdminEngine();
            try
            {
                feedback = adminBL.GetSingleClientFeedback(id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in AdminController/WhatClientSaysDetail", ex);
                throw ex;
            }
            return Json(feedback, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult WhatClientSays(WhatClientSays feedback)
        {
            ResponseOut responseOut = new ResponseOut();

            IAdminBL adminBL = new AdminEngine();
            try
            {

                if (feedback != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                    responseOut = adminBL.SubmitClientFeedback(feedback);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in AdminController/WhatClientSays", ex);
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApprovedCandidatePlans()
        {
            return View();
        }

        public PartialViewResult _ApprovedCandidatePlanList()
        {
            IPlansBL planBL = new PlansEngine();
            List<getIsApprovedCandidatePlans_Result> isActivatedPlansList = new List<getIsApprovedCandidatePlans_Result>();
            try
            {
                isActivatedPlansList = planBL.GetApprovedCandidatePlan();
                return PartialView(isActivatedPlansList);
            }
            catch (Exception ex)
            {
                Log.Error("Error in AdminController/_ApprovedCandidatePlanList", ex);
                return null;
            }
        }
        [HttpGet]
        public JsonResult UpdatePlanByPlanID(string Id)
        {
            IPlansBL planBL = new PlansEngine();
            ResponseOut response = new ResponseOut();
            try
            {
                int planID = Convert.ToInt32(Id);
                response=planBL.UpdatePlanByPlanIDByAdmin(planID);
            }
            catch (Exception ex)
            {
                Log.Error("Error in AdminController/UpdatePlanByPlanID", ex);
                response.message = ActionMessage.ApplicationException;
                response.status = ActionStatus.Fail;
            }
            return Json(response,JsonRequestBehavior.AllowGet);
        }
    }
}