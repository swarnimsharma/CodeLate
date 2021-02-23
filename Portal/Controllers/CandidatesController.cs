using Portal.Common;
using Portal.PortalBL.Candidate;
using Portal.PortalBL.RecruiterBL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace Portal.Controllers
{
    public class CandidatesController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CandidatesController));
        // GET: Candidate
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CandidateProfileList(string[] title = null, string locality = null, int? experience = null, string sort_by_level = null, string sort_by_availability = null, string sort_by_new_old = null, string sort_by_profile_pic = null, string job_type = null)
        {
            List<CandidatesListViewModel> candidatesResultSet = new List<CandidatesListViewModel>();
            ICandidateBL candidateBL = new CandidateEngine();

            try
            {
                
                int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                candidatesResultSet = candidateBL.GetCandidatesList(title, locality, experience, user_id, sort_by_level, sort_by_availability, sort_by_new_old, sort_by_profile_pic, job_type);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CandidateController/CandidateProfileList", ex);
                throw ex;
            }
            return PartialView(candidatesResultSet);
        }

        public ActionResult ShowCandidatesInterestedByUser()
        {
            return View();
        }
    }
}