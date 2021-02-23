using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class PostRequirementImportViewModel
    {
        public string engagement_model { get; set; }
        public string requirement_title { get; set; }
        public string requirement_description { get; set; }
        public string client_name { get; set; }
        public string email_id { get; set; }
        public string location { get; set; }
        public string contact_details { get; set; }
    }
}