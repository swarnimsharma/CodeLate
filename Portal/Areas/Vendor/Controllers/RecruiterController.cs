using Portal.Common;
using Portal.Filters;
using Portal.PortalBL.Candidate;
using Portal.PortalBL.RecruiterBL;
using Portal.PortalBL.UserBL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Areas.Vendor.Controllers
{
    [UserAuthenticationFilter]
    public class RecruiterController : Controller
    {
        // GET: Vendor/Recruiter
        public ActionResult Recruiters()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEditRecruiterProfile(RecruiterViewModel recruiterViewModel)
        {
            ResponseOut responseOut = new ResponseOut();

            IRecruiterBL recruiterBL = new RecruiterEngine();
            try
            {

                if (recruiterViewModel != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                    responseOut = recruiterBL.AddUpdateRecruiter(recruiterViewModel, user_id);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRecruitersList(string candidate_name = null, string candidate_technology = null, string candidate_experience = null)
        {
            List<RecruiterResultSet> recruitersResultSet = new List<RecruiterResultSet>();
            IRecruiterBL recruiterBL = new RecruiterEngine();
            try
            {
                int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                recruitersResultSet = recruiterBL.GetRecruitersList(user_id, candidate_name, candidate_technology, candidate_experience);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return Json(recruitersResultSet, JsonRequestBehavior.AllowGet);
            return PartialView(recruitersResultSet);
        }

        [HttpGet]
        public JsonResult GetCandidateProfile(int id)
        {
            RecruiterViewModel candidateProfile = new RecruiterViewModel();
            IRecruiterBL recruiterBL = new RecruiterEngine();
            try
            {
                candidateProfile = recruiterBL.GetCandidateProfile(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(candidateProfile, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RequestForCandidate()
        {
            return View();
        }

        public ActionResult InterestredUsers()
        {
            List<InterestedToCandidateViewModel> interestedUserList = new List<InterestedToCandidateViewModel>();
            IRecruiterBL recruiterBL = new RecruiterEngine();
            try
            {
                int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                interestedUserList = recruiterBL.GetInterestedUsers(user_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(interestedUserList);
        }

        public ActionResult GetApprovedRequirement()
        {
            return View();
        }

        public ActionResult GetAllLeads(string requirement = null, string status = null)
        {
            List<PostYourRequirement> requirementList = new List<PostYourRequirement>();
            ICandidateBL candidateBL = new CandidateEngine();
            try
            {
                int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                requirementList = candidateBL.GetApprovedRequirements(user_id, requirement, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(requirementList);
        }
        [HttpGet]
        public PartialViewResult GetSingleApprovedRequirement(int post_id)
        {
            PostYourRequirement requirementDetail = new PostYourRequirement();
            ICandidateBL candidateBL = new CandidateEngine();
            try
            {
                requirementDetail = candidateBL.GetSingleApprovedRequirement(post_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(requirementDetail);
        }

        public PartialViewResult GetInterestedUserDetail(int user_id)
        {
            UserViewModel userDetail = new UserViewModel();
            IUserBL userBL = new UserEngine();
            try
            {
                userDetail = userBL.GetUserProfile(user_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(userDetail);
        }

        [HttpGet]
        public ActionResult GetTest()
        {
            return Json("ok");
        }

    }
}