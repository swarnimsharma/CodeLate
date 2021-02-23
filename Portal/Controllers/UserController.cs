using Portal.Common;
using Portal.PortalBL.EmailBL;
using Portal.PortalBL.UserBL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace Portal.Controllers
{
    public class UserController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HomeController));
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddUserProfile(UserViewModel userViewModel)
        {
            ResponseOut responseOut = new ResponseOut();

            IUserBL userBL = new UserEngine();
            try
            {

                if (userViewModel != null)
                {
                    responseOut = userBL.AddUserProfile(userViewModel);
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in UserController/AddUserProfile", ex);
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
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
                    if (userViewModel.pk_user_id != 0)
                    {
                        user_id = userViewModel.pk_user_id;
                    }
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
                Log.Error("Error in UserController/UpdateUserProfile", ex);
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
                Log.Error("Error in UserController/UpdatePassword", ex);
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserProfile(int? id = null)
        {

            UserViewModel userProfile = new UserViewModel();
            IUserBL userBL = new UserEngine();
            int user_id = Convert.ToInt32(HttpContext.Session[SessionKey.CurrentUserID]);
            if (id != null && id != 0)
            {
                user_id = Convert.ToInt32(id);
            }
            try
            {
                userProfile = userBL.GetUserProfile(user_id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in UserController/GetUserProfile", ex);
                throw ex;
            }
            return Json(userProfile, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegisterUser(RegisterUserViewModel userViewModel)
        {
            ResponseOut responseOut = new ResponseOut();

            IUserBL userBL = new UserEngine();
            IEmailBL emailBL = new EmailEngine();
            try
            {

                if (userViewModel != null)
                {
                    responseOut = userBL.RegisterUser(userViewModel);
                    string body = this.createEmailBody(responseOut.hash);
                    bool status = Utilities.SendEmail("", responseOut.email, "Codelate | Verify your Email!", body, 587, true, "smtp.gmail.com", "", null, null);
                    if (status)
                    {

                    }
                }
                else
                {
                    responseOut.message = ActionMessage.ProbleminData;
                    responseOut.status = ActionStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in UserController/RegisterUser", ex);
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }


        private string createEmailBody(string hash)
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   

            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplates/EmailVerify.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{action_url}}", "http://codelate.com/Home/VerifyStatus?hash=" + hash); //replacing the required things  
            return body;
        }

    }
}