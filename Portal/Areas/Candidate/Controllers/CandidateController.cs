using Portal.Common;
using Portal.PortalBL.Candidate;
using Portal.ViewModels;
using System;
using System.Web.Mvc;
using log4net;
using Portal.PortalBL.PlansBL;
using Portal.Filters;

namespace Portal.Areas.Candidate.Controllers
{
    [UserAuthenticationFilter]
    public class CandidateController : Controller
    {
        // GET: Candidate/Candidate
        private static readonly ILog Log = LogManager.GetLogger(typeof(CandidateController));
        public ActionResult Profile()
        {
            ViewBag.user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
            return View();
        }

      
        [HttpPost]
        public ActionResult AddUpdateCandidateProfile(CandidateProfile data)
        {
            ICandidateBL candidateBL = new CandidateEngine();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                 responseOut = candidateBL.AddUpdateCandidateProfile(data);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CandidateController/AddUpdateCandidateProfile", ex);
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCandidateProfile(int? id = null)
        {

            CandidateProfile candidate;
            ICandidateBL candidateBL = new CandidateEngine();
            int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
            if (id != null && id != 0)
            {
                user_id = Convert.ToInt32(id);
            }
            try
            {
                candidate = candidateBL.GetCandidateProfile(user_id);
                ViewBag.user_id = user_id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CandidateController/GetCandidateProfile", ex);
                throw ex;
            }
            return Json(candidate, JsonRequestBehavior.AllowGet);
        }

       
        public ActionResult CandidatePlans()
        {
            return View();
        }

        public PartialViewResult _UserPlan()
        {
            IPlansBL planBL = new PlansEngine();
            try
            {
                int? user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);

                var data = planBL.GetCandidatePricingPlans(user_id);
                return PartialView(data);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CandidateController/_UserPlan", ex);
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult AddUpdatePlanSelectedByCandidate(PlansViewModel model)
        {
            ResponseOut responseOut = new ResponseOut();
            IPlansBL planBL = new PlansEngine();
            try
            {
                if (model.pk_plan_id != 0)
                {
                    model.pk_user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                    responseOut = planBL.AddUpdateSelectPlanByCandidate(model);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in Candidate Controller/AddUpdatePlanSelectedByVendor", ex);
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CandidatePlanResult()
        {
            return View();
        }

        public PartialViewResult _UserPlanRemaining()
        {
            IPlansBL planBL = new PlansEngine();
            try
            {
                int? user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                var data = planBL.CandidateInterestReceivedLimits(user_id);
                return PartialView("_UserPlan", data);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CandidateController/_UserPlanRemaining", ex);
                throw ex;
            }
        }
        public ActionResult CandidateLead()
        {
            return View();
        }

        public ActionResult CandidateInterestReceived()
        {
            return View();
        }
        public PartialViewResult _CandidateInterestReceivedLimit()
        {
            IPlansBL planBL = new PlansEngine();
            try
            {
                int? user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                var data = planBL.CandidateInterestReceivedLimits(user_id);
                return PartialView( data);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CandidateController/_CandidateInterestReceivedLimit", ex);
                throw ex;
            }
        }

    }
}