using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class ImportViewModel
    {
        
    }

    public class ImportVendorCandidateViewModel
    {
        public string vendor_name { get; set; }
        public string vendor_user_name { get; set; }
        public string vendor_password { get; set; }
        public string candidate_firstname { get; set; }
        public string candidate_lastname { get; set; }
        public string candidate_technology { get; set; }
        public string candidate_one_liner_headline { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string availability { get; set; }
        public string experience_level { get; set; }
    }
}