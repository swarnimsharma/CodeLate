using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class RecruiterViewModel
    {
        public int pk_resource_profile_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email_id { get; set; }
        public string contact_no { get; set; }
        public int fk_state_id { get; set; }
        public int fk_city_id { get; set; }
        public int fk_country_id { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        public string minimum_tenure { get; set; }
        public int fk_experience_level { get; set; }
        public string expertise_profession { get; set; }
        public string profile_pic { get; set; }
        public string about_us { get; set; }
        public string availability { get; set; }
        public int job_type { get; set; }

    }

    public class RecruiterResultSet
    {
        public int pk_resource_profile_id { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        public string experience { get; set; }
        public string is_active { get; set; }
        public string expertise { get; set; }
        public string created_datetime { get; set; }
        public string availablity { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string about_us { get; set; }
        public bool is_already_interested { get; set; }
        public string profile_pic { get; set; }
        public string job_type { get; set; }

    }

    public class InterestedToCandidateViewModel
    {
        public int pk_interest_id { get; set; }
        public string requirement_title_select { get; set; }
        public string requirement_title { get; set; }
        public string requirement_description { get; set; }
        public int candidate_id { get; set; }
        public int interested_user_count { get; set; }

        public string candidate_name { get; set; }
        public string requested_date { get; set; }
        public string experience { get; set; }
        public string expertise { get; set; }
        public int status { get; set; }
        public int no_of_request { get; set; }
        public int fk_user_id { get; set; }
    }

}