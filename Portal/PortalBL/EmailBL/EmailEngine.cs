using Portal.Common;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Portal.PortalBL.EmailBL
{
    public class EmailEngine : IEmailBL
    {
       public string server_email = null;
       public string server_password = null;

        public EmailEngine()
        {
            server_email = ConfigurationManager.AppSettings["ServerEmailID"];
            server_password = ConfigurationManager.AppSettings["ServerEmailPassword"];
        }

        public ResponseOut ContactUsEmail(EmailContactUsViewModel data)
        {
            ResponseOut responseOut = new ResponseOut();
            string mailBody = null;
            mailBody += "Hi Neel,  <br/><br/>";
            mailBody += "you got a new enquiry from website. Details of enquiry is as followed:";
            mailBody += "<br />";
            mailBody += "Name: " + data.name;
            mailBody += "<br/>";
            mailBody += "Contact: " + data.contact_no;
            mailBody += "<br />";
            mailBody += "Email: " + data.email;
            mailBody += "<br />";
            mailBody += "Description: " + data.message;
            mailBody += "<br/><br/>";

            bool status = Utilities.SendEmail(server_email, "anish.sharma@infoxen.com", "Enquiry from Website!", mailBody, 587, true, "smtp.gmail.com", "Welcome@1234", null, null);
            if (status)
            {

                responseOut.status = ActionStatus.Success;
                return responseOut;
            }
            responseOut.status = ActionStatus.Fail;
            return responseOut;
        }
    }
}