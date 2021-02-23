using Portal.Common;
using Portal.PortalBL.Candidate;
using Portal.PortalBL.RequirementBL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Areas.Client.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client/Client
        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult SentInterests()
        { 
            List<InterestedToCandidateViewModel> interestedUserList = new List<InterestedToCandidateViewModel>();
            ICandidateBL candidateBL = new CandidateEngine();
            try
            {
                int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                interestedUserList = candidateBL.GetInterestedUsers(user_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(interestedUserList);
        }

        public ActionResult PostedRequirements()
        {
            List<PostYourRequirement> postedRequirements = new List<PostYourRequirement>();
            ICandidateBL candidateBL = new CandidateEngine();
            try
            {
                int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                postedRequirements = candidateBL.GetAllPostedRequirement(user_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(postedRequirements);
        }
        [HttpGet]
        public ActionResult ChangeRequirementStatus(int post_id,int status,string message)
        {
            ResponseOut responseOut = new ResponseOut();

            IRequirementBL requirementBL = new RequirementEngine();
            try
            {
                    responseOut = requirementBL.ChangeStatusByUser(post_id, status,message);
            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }
    }
}