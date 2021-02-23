using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class CandidatesListViewModel
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
        public int experience_id { get; set; }
        public string[] expertise_array { get; set; }
        public DateTime? added_date_time { get; set; }
        public string job_type { get; set; }
    }

    public class PostYourRequirement
    {
        public int post_id { get; set; }
        public string requirement_title { get; set; }
        public string fullname { get; set; }
        public string email_id { get; set; }
        public string mobile_no { get; set; }
        public string message { get; set; }
        public string subject { get; set; }
        public int status { get; set; }
        public string project_type { get; set; }
        public string added_datetime { get; set; }
        public DateTime? created_Date { get; set; }
        public string status_reason { get; set; }
        public string client_reason { get; set; }
        public int? status_by_codelate { get; set; }
    }

    public class SubmitYourRequirement
    {
        public int post_id { get; set; }
        public int[] vendor_ids { get; set; }
        public int status { get; set; }
        public string reason_status { get; set; }
    }

    public class CandidateProfile
    {
        public int fk_user_id { get; set; }
        public string profile_headline { get; set; }
        public int candidate_fk_country_id { get; set; }
        public int candidate_fk_state_id { get; set; }
        public int candidate_fk_city_id { get; set; }
        public string country_listing { get; set; }
        public string state_listing { get; set; }
        public string city_listing { get; set; }
        public string resume{ get; set; }
        public string availability { get; set; }
        public string selected_technologies_name { get; set; }
        public string profile_pic { get; set; }
        public string head_line { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string about_us { get; set; }
        public string contact { get; set; }
    }

}