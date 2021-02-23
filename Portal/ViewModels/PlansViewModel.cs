using System;
using System.Collections.Generic;

namespace Portal.ViewModels
{
    public class PlansViewModel
    {
        public int pk_plan_id { get; set; }
        public string plan_name { get; set; }
        public bool is_active { get; set; }
        public int pk_user_id { get; set; }
        public DateTime plan_date_time { get; set; }
        public int pk_vendor_plan_id { get; set; }
    }

    public class VendorPlansViewModel{
        public int pk_vendor_plan_id { get; set; }
        public int fk_plan_id { get; set; }
        public int candidate_listing_limit { get; set; }
        public int interest_received_limit { get; set; }
        public int lead_access_limit { get; set; }
        public int technology_limit { get; set; }
        public int? per_technology_limit { get; set; }
        public bool is_active { get; set; }
        public string plan_name { get; set; }
        public string country_listing { get; set; }
        public string state_listing { get; set; }
        public string city_listing { get; set; }
        public string country_listing_names { get; set; }
        public string state_listing_names { get; set; }
        public string city_listing_names { get; set; }
        public double plan_price { get; set; }
        public int plan_duration_type { get; set; }
        public int user_role_id { get; set; }
        public int ? bold_listing { get; set; }
        public int ? top_ranking { get; set; }
    }

    public class VendorPlansPricing
    {
        public int pk_vendor_pricing_id { get; set; }
        public double per_candidate_listing_price { get; set; }
        public double per_interest_Received_price { get; set; }
        public double per_lead_access_price { get; set; }
        public double per_technology_price { get; set; }
        public bool is_active { get; set; }
        public double plan_price_amount { get; set; }
        public string plan_duration { get; set; }
        public string plan_name { get; set; }
        public int candidate_listing { get; set; }
        public int interest_recieved_listing { get; set; }
        public string country_listing { get; set; }
        public string state_listing { get; set; }
        public string city_listing { get; set; }
        public int lead_access_limit { get; set; }
        public int? selected_user_plan { get; set; }
        public int? technology_count { get; set; }
        public int? bold_listing { get; set; }
        public int? top_ranking { get; set; }
        public bool?  is_plan_activated_by_admin { get; set; }

        public int? userplan_id { get; set; }
    }

    public class CandidatePlansViewModel
    {
        public int pk_vendor_plan_id { get; set; }
        public int fk_plan_id { get; set; }
        public int candidate_listing_limit { get; set; }
        public int interest_received_limit { get; set; }
        public int lead_access_limit { get; set; }
        public int technology_limit { get; set; }
        public int? per_technology_limit { get; set; }
        public bool is_active { get; set; }
        public string plan_name { get; set; }
        public string country_listing { get; set; }
        public string state_listing { get; set; }
        public string city_listing { get; set; }
        public string country_listing_names { get; set; }
        public string state_listing_names { get; set; }
        public string city_listing_names { get; set; }
    }
    public class CandidateInterestReceivedLimit
    {
        public int pk_candidate_interested_id { get; set; }
        public string requirement_title_select{ get; set; }
        public string requirement_title { get; set; } 
        public DateTime requested_date { get; set; }
        public string vendor_name { get; set; }

    }

}