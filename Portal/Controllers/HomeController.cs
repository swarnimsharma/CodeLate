using log4net;
using Portal.Common;
using Portal.Filters;
using Portal.PortalBL.AdminBL;
using Portal.PortalBL.Candidate;
using Portal.PortalBL.DropDown;
using Portal.PortalBL.EmailBL;
using Portal.PortalBL.RecruiterBL;
using Portal.PortalBL.UserBL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Portal.Controllers
{
    //[UserAuthenticationFilter]
    public class HomeController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HomeController));
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index(string title = null, string locality = null, int? experience = null)
        {
            ViewBag.query_title = title;
            ViewBag.locality = locality;
            ViewBag.experience = experience;
            ViewBag.RoleID = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserRoleID]);
            if (ViewBag.RoleID == 2)
            {
                return RedirectToAction("Profile", "Vendor", new { area = "Vendor" });
            }

            if (ViewBag.RoleID == 1)
            {
                return RedirectToAction("Profile", "Admin", new { area = "Admin" });
            }

            if (ViewBag.RoleID == 4)
            {
                return RedirectToAction("Profile", "Candidate", new { area = "Candidate" });
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel userViewModel)
        {
            ResponseOut responseOut = new ResponseOut();

            IUserBL userBL = new UserEngine();
            try
            {
                
                if (userViewModel != null)
                {
                    responseOut = userBL.Authentication(userViewModel.user_name, userViewModel.password);
                    if (responseOut.status == "SUCCESS")
                    {
                        //int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserRoleID]);
                        //return RedirectToAction("Index", "Home");
                        responseOut.message = ActionMessage.SuccessLogin;
                        responseOut.status = ActionStatus.Success;
                    }
                    //return View("Index");
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in Home Controller/Login", ex);
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CandidateProfiles(string[] title = null, string locality = null, int? experience = null)
        {
            ViewBag.search_title = title;
            ViewBag.search_locality = locality;
            ViewBag.search_experience = experience;
            return View();
        }

        [HttpPost]
        public JsonResult SaveInterestedToCandidate(InterestedToCandidateViewModel model)
        {
            ResponseOut responseOut = new ResponseOut();
            IRecruiterBL recruiterBL = new RecruiterEngine();
            try
            {
                
                int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                responseOut = recruiterBL.SaveInterestedCandidate(model, user_id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in Home Controller/SaveInterestedToCandidate", ex);
                throw ex;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("Index");
        }

        public ActionResult Jobs()
        {

            return PartialView();
        }

        public JsonResult SearchFilters(string type, string search)
        {
            IDropDownBL dropdown = new DropDownEngine();
            List<DropDownViewModal> expertiseViewModel = new List<DropDownViewModal>();
            try
            {
                string type_string = "";
                if (type == "expertise")
                {
                    type_string = "expertise_dropdown";
                }
                if (type == "locality")
                {
                    type_string = "city_dropdown";
                }
                expertiseViewModel = dropdown.GetList(type_string, null,null, search);
            }
            catch (Exception ex)
            {
                Log.Error("Error in Home Controller/SearchFilters", ex);
            }
            return Json(expertiseViewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PostYourRequirement(PostYourRequirement candidateViewModel)
        {
            ResponseOut responseOut = new ResponseOut();

            ICandidateBL candidateBL = new CandidateEngine();
            try
            {

                if (candidateViewModel != null)
                {
                    int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);

                    responseOut = candidateBL.PostRequirement(candidateViewModel, user_id);

                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }
            }
            catch (Exception ex)
            {

                Log.Error("Error in Home Controller/PostYourRequirement", ex);
            
            responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OurBenefits()
        {
            return View();
        }

        public ActionResult ProcessStep()
        {
            return View();
        }

        public ActionResult WhatClientSays()
        {
            List<WhatClientSays> feedback = new List<WhatClientSays>();
            IAdminBL adminBL = new AdminEngine();
            try
            {
                feedback = adminBL.GetClientFeedback(null);
                feedback = feedback.Where(x => x.is_published).ToList();
            }
           
            catch (Exception ex)
            {
                Log.Error("Error in Home Controller/WhatClientSays", ex);
                throw ex;
            }
            return View(feedback);
        }

        public ActionResult AboutUs()
        {
            ViewBag.RoleID = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserRoleID]);
            if (ViewBag.RoleID == 2)
            {
                return RedirectToAction("Profile", "Vendor", new { area = "Vendor" });
            }

            if (ViewBag.RoleID == 1)
            {
                return RedirectToAction("Profile", "Admin", new { area = "Admin" });
            }
            return View();
        }

        public ActionResult WhoWeAre()
        {
            return View();
        }

        public ActionResult Team()
        {
            return View();
        }

        public ActionResult OurPartners()
        {
            return View();
        }

        public ActionResult CaseStudy()
        {
            ViewBag.RoleID = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserRoleID]);
            if (ViewBag.RoleID == 2)
            {
                return RedirectToAction("Profile", "Vendor", new { area = "Vendor" });
            }

            if (ViewBag.RoleID == 1)
            {
                return RedirectToAction("Profile", "Admin", new { area = "Admin" });
            }
            return View();
        }

        public ActionResult Careers()
        {
            ViewBag.RoleID = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserRoleID]);
            if (ViewBag.RoleID == 2)
            {
                return RedirectToAction("Profile", "Vendor", new { area = "Vendor" });
            }

            if (ViewBag.RoleID == 1)
            {
                return RedirectToAction("Profile", "Admin", new { area = "Admin" });
            }
            return View();
        }

        [HttpPost]
        public JsonResult ContactUs(EmailContactUsViewModel data)
        {
            ResponseOut responseOut = new ResponseOut();
            IEmailBL emailBL = new EmailEngine();
            try
            {
                //int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                responseOut = emailBL.ContactUsEmail(data);
            }
            catch (Exception ex)
            {
                Log.Error("Error in Home Controller/ContactUs", ex);
                throw ex;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerifyStatus(string hash)
        {
            ResponseOut responseOut = new ResponseOut();
            IUserBL userBL = new UserEngine();
            try
            {
                //int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
                responseOut = userBL.VerifyUserEmail(hash);
                ViewBag.Status = responseOut.message;
            }
            catch (Exception ex)
            {
                Log.Error("Error in Home Controller/VerifyStatus", ex);
                throw ex;
            }
            return View();
        }

    }
}