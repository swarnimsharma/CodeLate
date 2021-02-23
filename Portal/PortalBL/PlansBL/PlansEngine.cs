using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Web;
using Portal.Common;
using Portal.DAL;
using Portal.ViewModels;

namespace Portal.PortalBL.PlansBL
{
    public class PlansEngine : IPlansBL
    {

        public ResponseOut AddPlans(PlansViewModel model)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (PortalEntities _context = new PortalEntities())
                {

                    portal_plan plan = new portal_plan();
                    plan.is_active = model.is_active;
                    plan.is_deleted = false;
                    plan.plan_name = model.plan_name;
                    _context.portal_plan.Add(plan);
                    int result = _context.SaveChanges();
                    if (result > 0)
                    {
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.PlansCreatedSuccess;
                    }
                }
            }
            catch (Exception ex)
            {
                responseOut.status = ActionStatus.Fail;
                responseOut.message = ActionMessage.ApplicationException;
            }

            return responseOut;
        }

        public ResponseOut UpdatePlans(PlansViewModel model)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (PortalEntities _context = new PortalEntities())
                {

                    portal_plan plan = _context.portal_plan.Where(x => x.pk_plan_type_id == model.pk_plan_id).FirstOrDefault();
                    plan.is_active = model.is_active;
                    plan.is_deleted = false;
                    plan.plan_name = model.plan_name;
                    int result = _context.SaveChanges();
                    if (result > 0)
                    {
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.PlansCreatedSuccess;
                    }
                }
            }
            catch (Exception ex)
            {
                responseOut.status = ActionStatus.Fail;
                responseOut.message = ActionMessage.ApplicationException;
            }

            return responseOut;
        }

        public ResponseOut AddVendorPlans(VendorPlansViewModel model)
        {
            string temp_country_name = "";
            string temp_state_name = "";
            string temp_city_name = "";

            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (PortalEntities _context = new PortalEntities())
                {
                    if (!string.IsNullOrEmpty(model.country_listing))
                    {
                        var country_names = model.country_listing.Split(',');
                        foreach (var c_id in country_names)
                        {
                            temp_country_name += _context.portal_country.AsEnumerable().Where(x => x.pk_country_id == Convert.ToInt32(c_id)).Select(x => x.country_name).FirstOrDefault() + ",";
                        }
                        if (!string.IsNullOrEmpty(temp_country_name))
                        {
                            temp_country_name = temp_country_name.Remove(temp_country_name.Length - 1, 1);
                        }
                    }

                    if (!string.IsNullOrEmpty(model.state_listing))
                    {
                        var country_names = model.state_listing.Split(',');
                        foreach (var s_id in country_names)
                        {
                            temp_state_name += _context.portal_state.AsEnumerable().Where(x => x.pk_state_id == Convert.ToInt32(s_id)).Select(x => x.state_name).FirstOrDefault() + ",";
                        }
                        if (!string.IsNullOrEmpty(temp_state_name))
                        {
                            temp_state_name = temp_state_name.Remove(temp_state_name.Length - 1, 1);
                        }
                    }

                    if (!string.IsNullOrEmpty(model.city_listing))
                    {
                        var country_names = model.city_listing.Split(',');
                        foreach (var c_id in country_names)
                        {
                            temp_city_name += _context.portal_city.AsEnumerable().Where(x => x.pk_city_id == Convert.ToInt32(c_id)).Select(x => x.city_name).FirstOrDefault() + ",";
                        }
                        if (!string.IsNullOrEmpty(temp_city_name))
                        {
                            temp_city_name = temp_city_name.Remove(temp_city_name.Length - 1, 1);
                        }
                    }

                    model.country_listing_names = temp_country_name;
                    model.state_listing_names = temp_state_name;
                    model.city_listing_names = temp_city_name;
                    portal_vendor_plan plan = new portal_vendor_plan();
                    plan.is_active = model.is_active;
                    plan.is_deleted = false;
                    plan.lead_access_limit = model.lead_access_limit;
                    plan.candidate_listing_limit = model.candidate_listing_limit;
                    plan.technology_count = model.technology_limit;
                    plan.interest_recived_limit = model.interest_received_limit;
                    plan.fk_plan_id = model.fk_plan_id;
                    plan.added_date = DateTime.Now;
                    plan.per_technology_count = model.per_technology_limit;
                    plan.country_listing = model.country_listing;
                    plan.state_listing = model.state_listing;
                    plan.city_listing = model.city_listing;
                    plan.country_listing_names = model.country_listing_names;
                    plan.state_listing_names = model.state_listing_names;
                    plan.city_listing_names = model.city_listing_names;
                    plan.custom_plan_price = model.plan_price;
                    plan.plan_duration_type = model.plan_duration_type;
                    plan.fk_role_id = model.user_role_id;
                    plan.bold_listing = model.bold_listing;
                    plan.top_ranking = model.top_ranking;
                    _context.portal_vendor_plan.Add(plan);
                    int result = _context.SaveChanges();
                    if (result > 0)
                    {
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.PlansCreatedSuccess;
                    }
                }
            }
            catch (Exception ex)
            {
                responseOut.status = ActionStatus.Fail;
                responseOut.message = ActionMessage.ApplicationException;
            }
            return responseOut;
        }
        public ResponseOut UpdateVendorPlans(VendorPlansViewModel model)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (PortalEntities _context = new PortalEntities())
                {
                    string temp_country_name = "";
                    string temp_state_name = "";
                    string temp_city_name = "";
                    if (!string.IsNullOrEmpty(model.country_listing))
                    {
                        var country_names = model.country_listing.Split(',');
                        foreach (var c_id in country_names)
                        {
                            temp_country_name += _context.portal_country.AsEnumerable().Where(x => x.pk_country_id == Convert.ToInt32(c_id)).Select(x => x.country_name).FirstOrDefault() + ",";
                        }
                        if (!string.IsNullOrEmpty(temp_country_name))
                        {
                            temp_country_name = temp_country_name.Remove(temp_country_name.Length - 1, 1);

                        }
                    }

                    if (!string.IsNullOrEmpty(model.state_listing))
                    {
                        var country_names = model.state_listing.Split(',');
                        foreach (var s_id in country_names)
                        {
                            temp_state_name += _context.portal_state.AsEnumerable().Where(x => x.pk_state_id == Convert.ToInt32(s_id)).Select(x => x.state_name).FirstOrDefault() + ",";
                        }
                        if (!string.IsNullOrEmpty(temp_state_name))
                        {
                            temp_state_name = temp_state_name.Remove(temp_state_name.Length - 1, 1);
                        }
                    }

                    if (!string.IsNullOrEmpty(model.city_listing))
                    {
                        var country_names = model.city_listing.Split(',');
                        foreach (var c_id in country_names)
                        {
                            temp_city_name += _context.portal_city.AsEnumerable().Where(x => x.pk_city_id == Convert.ToInt32(c_id)).Select(x => x.city_name).FirstOrDefault() + ",";
                        }
                        if (!string.IsNullOrEmpty(temp_city_name))
                        {
                            temp_city_name = temp_city_name.Remove(temp_city_name.Length - 1, 1);
                        }
                    }

                    model.country_listing_names = temp_country_name;
                    model.state_listing_names = temp_state_name;
                    model.city_listing_names = temp_city_name;

                    portal_vendor_plan plan = _context.portal_vendor_plan.Where(x => x.pk_vendor_plan_id == model.pk_vendor_plan_id).FirstOrDefault();
                    plan.is_active = model.is_active;
                    plan.is_deleted = false;
                    plan.lead_access_limit = model.lead_access_limit;
                    plan.technology_count = model.technology_limit;
                    plan.candidate_listing_limit = model.candidate_listing_limit;
                    plan.interest_recived_limit = model.interest_received_limit;
                    plan.fk_plan_id = model.fk_plan_id;
                    plan.per_technology_count = model.per_technology_limit;
                    plan.country_listing = model.country_listing;
                    plan.state_listing = model.state_listing;
                    plan.city_listing = model.city_listing;
                    plan.country_listing_names = model.country_listing_names;
                    plan.state_listing_names = model.state_listing_names;
                    plan.city_listing_names = model.city_listing_names;
                    plan.custom_plan_price = model.plan_price;
                    plan.plan_duration_type = model.plan_duration_type;
                    plan.fk_role_id = model.user_role_id;
                    plan.bold_listing = model.bold_listing;
                    plan.top_ranking = model.top_ranking;

                    int result = _context.SaveChanges();
                    if (result > 0)
                    {
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.PlansCreatedSuccess;
                    }
                }
            }
            catch (Exception ex)
            {
                responseOut.status = ActionStatus.Fail;
                responseOut.message = ActionMessage.ApplicationException;
            }
            return responseOut;
        }

        public List<PlansViewModel> GetPlans()
        {
            try
            {
                using (PortalEntities _context = new PortalEntities())
                {
                    var data = _context.portal_plan.AsEnumerable().Where(x => x.is_deleted == false).Select(x => new PlansViewModel
                    {
                        pk_plan_id = x.pk_plan_type_id,
                        is_active = Convert.ToBoolean(x.is_active),
                        plan_name = x.plan_name
                    }).ToList();

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PlansViewModel GetPlansById(int id)
        {
            try
            {
                using (PortalEntities _context = new PortalEntities())
                {
                    var data = _context.portal_plan.AsEnumerable().Where(x => x.pk_plan_type_id == id).Select(x => new PlansViewModel
                    {
                        pk_plan_id = x.pk_plan_type_id,
                        plan_name = x.plan_name,
                        is_active = Convert.ToBoolean(x.is_active)
                    }).FirstOrDefault();

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VendorPlansViewModel> GetVendorPlans()
        {
            try
            {
                using (PortalEntities _context = new PortalEntities())
                {
                    var data = _context.portal_vendor_plan.AsEnumerable().Where(x => x.is_deleted == false).Select(x => new VendorPlansViewModel
                    {
                        pk_vendor_plan_id = x.pk_vendor_plan_id,
                        is_active = Convert.ToBoolean(x.is_active),
                        fk_plan_id = Convert.ToInt32(x.fk_plan_id),
                        plan_name = x.fk_plan_id != null ? x.portal_plan.plan_name : "",
                        candidate_listing_limit = Convert.ToInt32(x.candidate_listing_limit),
                        interest_received_limit = Convert.ToInt32(x.interest_recived_limit),
                        lead_access_limit = Convert.ToInt32(x.lead_access_limit),
                        per_technology_limit = Convert.ToInt32(x.per_technology_count),
                        technology_limit = Convert.ToInt32(x.technology_count),
                        country_listing_names = x.country_listing_names,
                        state_listing_names = x.state_listing_names,
                        city_listing_names = x.city_listing_names
                    }).ToList();

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public VendorPlansViewModel GetVendorPlansById(int id)
        {
            try
            {
                int a;
                using (PortalEntities _context = new PortalEntities())
                {
                    var data = _context.portal_vendor_plan.AsEnumerable().Where(x => x.pk_vendor_plan_id == id).Select(x => new VendorPlansViewModel
                    {
                        pk_vendor_plan_id = x.pk_vendor_plan_id,
                        is_active = Convert.ToBoolean(x.is_active),
                        fk_plan_id = Convert.ToInt32(x.fk_plan_id),
                        candidate_listing_limit = Convert.ToInt32(x.candidate_listing_limit),
                        interest_received_limit = Convert.ToInt32(x.interest_recived_limit),
                        lead_access_limit = Convert.ToInt32(x.lead_access_limit),
                        per_technology_limit = Convert.ToInt32(x.per_technology_count),
                        technology_limit = Convert.ToInt32(x.technology_limit),
                        country_listing = x.country_listing,
                        state_listing = x.state_listing,
                        city_listing = x.city_listing,
                        country_listing_names = x.country_listing_names,
                        city_listing_names = x.city_listing_names,
                        state_listing_names = x.state_listing_names,
                        plan_price = Convert.ToDouble(x.custom_plan_price),
                        plan_duration_type = Convert.ToInt32(x.plan_duration_type),
                        user_role_id = Convert.ToInt32(x.fk_role_id),
                        bold_listing = Convert.ToInt32(x.bold_listing),
                        top_ranking = Convert.ToInt32(x.top_ranking)
                    }).FirstOrDefault();

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VendorPlansPricing> GetVendorPricingPlans(int? user_id)
        {
            try
            {
                using (PortalEntities _context = new PortalEntities())
                {
                    var data = _context.portal_vendor_plan.AsEnumerable().Where(x => x.is_active == true && x.is_deleted == false && x.fk_role_id == 2).Select(x => new VendorPlansPricing
                    {
                        pk_vendor_pricing_id = x.pk_vendor_plan_id,
                        is_active = Convert.ToBoolean(x.is_active),
                        plan_price_amount = x.custom_plan_price,
                        plan_duration = x.plan_duration_type != null ? Enum.GetName(typeof(EnumSection.PlanDuration), x.plan_duration_type) : "",
                        plan_name = x.portal_plan.plan_name,
                        candidate_listing = Convert.ToInt32(x.candidate_listing_limit),
                        interest_recieved_listing = Convert.ToInt32(x.interest_recived_limit),
                        lead_access_limit = Convert.ToInt32(x.lead_access_limit),
                        country_listing = Utilities.ConvertCommaSeperateToBadge(x.country_listing_names),
                        state_listing = Utilities.ConvertCommaSeperateToBadge(x.state_listing_names),
                        city_listing = Utilities.ConvertCommaSeperateToBadge(x.city_listing_names),
                        selected_user_plan = x.portal_userplan_mapping.Where(y => y.user_id == user_id).Select(y => y.plan_id).FirstOrDefault(),
                        is_plan_activated_by_admin = x.portal_userplan_mapping.Where((a => a.user_id == user_id)).Select(y => Convert.ToBoolean(y.is_active)).FirstOrDefault(),
                    }).ToList();

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Getting issue while get vendor price plan");
            }
        }

        public List<VendorPlansPricing> GetCandidatePricingPlans(int? user_id)
        {
            try
            {
                using (PortalEntities _context = new PortalEntities())
                {
                    bool IsCurrentPlan = true;
                    var data = _context.portal_vendor_plan.AsEnumerable().Where(x => x.is_active == true && x.is_deleted == false && x.fk_role_id == 4).Select(x => new VendorPlansPricing
                    {
                        pk_vendor_pricing_id = x.pk_vendor_plan_id,
                        is_active = Convert.ToBoolean(x.is_active),
                        plan_price_amount = x.custom_plan_price,
                        plan_duration = x.plan_duration_type != null ? Enum.GetName(typeof(EnumSection.PlanDuration), x.plan_duration_type) : "",
                        plan_name = x.portal_plan.plan_name,
                        candidate_listing = Convert.ToInt32(x.candidate_listing_limit),
                        interest_recieved_listing = Convert.ToInt32(x.interest_recived_limit),
                        lead_access_limit = Convert.ToInt32(x.lead_access_limit),
                        country_listing = Utilities.ConvertCommaSeperateToBadge(x.country_listing_names),
                        state_listing = Utilities.ConvertCommaSeperateToBadge(x.state_listing_names),
                        city_listing = Utilities.ConvertCommaSeperateToBadge(x.city_listing_names),
                        technology_count = x.technology_count,
                        selected_user_plan = x.portal_userplan_mapping.Where((y => y.user_id == user_id)).Select(y => y.plan_id).FirstOrDefault(),
                        is_plan_activated_by_admin = x.portal_userplan_mapping.Where((a => a.user_id == user_id)).Select(y => Convert.ToBoolean(y.is_active)).FirstOrDefault(),
                        top_ranking = x.top_ranking,
                        bold_listing = x.bold_listing
                    }).ToList();

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Getting issue while get vendor price plan");
            }
        }

        public VendorPlansPricing GetVendorPricingPlansById(int id)
        {
            throw new NotImplementedException();
        }

        public ResponseOut VendorPlansPricing(VendorPlansPricing model)
        {
            throw new NotImplementedException();
        }

        public ResponseOut AddUpdateSelectPlanByVendor(int plan_id, int vendor_id)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (PortalEntities _context = new PortalEntities())
                {
                    portal_userplan_mapping vendor_plan_mapping = null;

                    vendor_plan_mapping = _context.portal_userplan_mapping.Where(x => x.user_id == vendor_id).FirstOrDefault();
                    if (vendor_plan_mapping != null)
                    {
                        vendor_plan_mapping.plan_id = plan_id;
                        vendor_plan_mapping.is_active = false;
                    }
                    else
                    {
                        vendor_plan_mapping = new portal_userplan_mapping();
                        vendor_plan_mapping.user_id = vendor_id;
                        vendor_plan_mapping.plan_id = plan_id;
                        vendor_plan_mapping.plan_time = DateTime.Now;
                        vendor_plan_mapping.is_active = false;
                        _context.portal_userplan_mapping.Add(vendor_plan_mapping);
                    }
                    int result = _context.SaveChanges();
                    if (result > 0)
                    {
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.PlansSelectedSuccess;
                    }
                }
            }
            catch (Exception ex)
            {
                responseOut.status = ActionStatus.Fail;
                responseOut.message = ActionMessage.ApplicationException;
            }

            return responseOut;
        }

        public ResponseOut AddUpdateSelectPlanByCandidate(PlansViewModel model)
        {
            ResponseOut responseOut = new ResponseOut();
            using (PortalEntities _context = new PortalEntities())
            {
                try
                {
                    portal_userplan_mapping portal_Userplan = null;
                    portal_plan_duration_left portal_Plan_Duration_Left = _context.portal_plan_duration_left.Where(x => x.fk_user_id == model.pk_user_id).FirstOrDefault();
                    portal_vendor_plan portal_Vendor_Plan;
                    portal_Vendor_Plan = _context.portal_vendor_plan.Where(x => x.pk_vendor_plan_id == model.pk_plan_id).FirstOrDefault();

                    if (portal_Plan_Duration_Left != null)
                    {
                        portal_Plan_Duration_Left.fk_plan_id = portal_Vendor_Plan.pk_vendor_plan_id;
                        portal_Plan_Duration_Left.candidate_listing_limit = portal_Vendor_Plan.candidate_listing_limit;
                        portal_Plan_Duration_Left.interest_recived_limit = portal_Vendor_Plan.interest_recived_limit;
                        portal_Plan_Duration_Left.lead_access_limit = portal_Vendor_Plan.lead_access_limit;
                        portal_Plan_Duration_Left.technology_limit = portal_Vendor_Plan.technology_limit;
                        portal_Plan_Duration_Left.technology_count = portal_Vendor_Plan.technology_count;
                        portal_Plan_Duration_Left.added_date = DateTime.Now;
                        portal_Plan_Duration_Left.bold_listing = portal_Vendor_Plan.bold_listing;
                        portal_Plan_Duration_Left.top_ranking = portal_Vendor_Plan.top_ranking;
                        portal_Plan_Duration_Left.fk_user_id = model.pk_user_id;
                    }
                    else
                    {
                        portal_Plan_Duration_Left = new portal_plan_duration_left();
                        portal_Plan_Duration_Left.fk_plan_id = portal_Vendor_Plan.pk_vendor_plan_id;
                        portal_Plan_Duration_Left.candidate_listing_limit = portal_Vendor_Plan.candidate_listing_limit;
                        portal_Plan_Duration_Left.interest_recived_limit = portal_Vendor_Plan.interest_recived_limit;
                        portal_Plan_Duration_Left.lead_access_limit = portal_Vendor_Plan.lead_access_limit;
                        portal_Plan_Duration_Left.technology_limit = portal_Vendor_Plan.technology_limit;
                        portal_Plan_Duration_Left.technology_count = portal_Vendor_Plan.technology_count;
                        portal_Plan_Duration_Left.added_date = DateTime.Now;
                        portal_Plan_Duration_Left.bold_listing = portal_Vendor_Plan.bold_listing;
                        portal_Plan_Duration_Left.top_ranking = portal_Vendor_Plan.top_ranking;
                        portal_Plan_Duration_Left.fk_user_id = model.pk_user_id;
                        _context.portal_plan_duration_left.Add(portal_Plan_Duration_Left);
                    }

                    portal_Userplan = _context.portal_userplan_mapping.Where(x => x.user_id == model.pk_user_id).FirstOrDefault();
                    if (portal_Userplan != null)
                    {
                        portal_Userplan.plan_id = model.pk_plan_id;
                        portal_Userplan.is_active = false;
                    }
                    else
                    {
                        portal_Userplan = new portal_userplan_mapping();
                        portal_Userplan.user_id = model.pk_user_id;
                        portal_Userplan.plan_id = model.pk_plan_id;
                        portal_Userplan.plan_time = DateTime.Now;
                        portal_Userplan.is_active = false;
                        _context.portal_userplan_mapping.Add(portal_Userplan);
                    }

                    int result = _context.SaveChanges();
                    if (result > 0)
                    {
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.PlansSelectedSuccess;
                    }
                }
                catch (EntityException ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                }
                catch (Exception ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                }
            }
            return responseOut;
        }

        public List<getIsApprovedCandidatePlans_Result> GetApprovedCandidatePlan()
        {
            try
            {
                using (PortalEntities _context = new PortalEntities())
                {
                    var data = _context.getIsApprovedCandidatePlans().ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResponseOut UpdatePlanByPlanIDByAdmin(int planId)
        {
            ResponseOut responseOut = new ResponseOut();
            using (PortalEntities _context = new PortalEntities())
            {
                try
                {
                    portal_userplan_mapping portal_Userplan = null;
                    portal_Userplan = _context.portal_userplan_mapping.Where(x => x.userplan_id == planId).FirstOrDefault();
                    if (portal_Userplan != null)
                    {
                        portal_Userplan.is_active = true;
                    }
                    int result = _context.SaveChanges();
                    if (result > 0)
                    {
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.PlansSelectedSuccess;
                    }
                }
                catch (EntityException ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                }
                catch (Exception ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                }
            }
            return responseOut;
        }
        public List<VendorPlansPricing> CandidateInterestReceivedLimit(int? user_id)
        {
            try
            {
                using (PortalEntities _context = new PortalEntities())
                {
                    bool IsCurrentPlan = true;
                    var data = _context.portal_vendor_plan.AsEnumerable().Where(x => x.is_active == true && x.is_deleted == false && x.fk_role_id == 4)
                        .Select(x => new VendorPlansPricing
                        {
                            pk_vendor_pricing_id = x.pk_vendor_plan_id,
                            is_active = Convert.ToBoolean(x.is_active),
                            plan_price_amount = x.custom_plan_price,
                            plan_duration = x.plan_duration_type != null ? Enum.GetName(typeof(EnumSection.PlanDuration), x.plan_duration_type) : "",
                            plan_name = x.portal_plan.plan_name,
                            candidate_listing = Convert.ToInt32(x.candidate_listing_limit),
                            interest_recieved_listing = Convert.ToInt32(x.interest_recived_limit),
                            lead_access_limit = Convert.ToInt32(x.lead_access_limit),
                            country_listing = Utilities.ConvertCommaSeperateToBadge(x.country_listing_names),
                            state_listing = Utilities.ConvertCommaSeperateToBadge(x.state_listing_names),
                            city_listing = Utilities.ConvertCommaSeperateToBadge(x.city_listing_names),
                            technology_count = x.technology_count,
                            selected_user_plan = x.portal_userplan_mapping.Where(y => y.user_id == user_id).Select(y => y.plan_id).FirstOrDefault(),
                            top_ranking = x.top_ranking,
                            bold_listing = x.bold_listing
                        }).ToList();

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Getting issue while get vendor price plan");
            }
        }
        public portal_vendor_plan AddUpdateSelectPlanDurationLeftByCandidate(int pk_plan_id)
        {
            //var data = _context.portal_vendor_plan.AsEnumerable().Where(x => x.is_active == true && x.is_deleted == false && x.fk_role_id == 4).Select(x => new VendorPlansPricing
            portal_vendor_plan responseOut = new portal_vendor_plan();
            using (PortalEntities _context = new PortalEntities())
            {
                try
                {
                    responseOut = _context.portal_vendor_plan.Where(x => x.pk_vendor_plan_id == pk_plan_id).FirstOrDefault();

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return responseOut;
        }

        public List<CandidateInterestReceivedLimit> CandidateInterestReceivedLimits(int? user_id)
        {
            try
            {
                using (PortalEntities _context = new PortalEntities())
                {
                    var data = _context.portal_for_interested_Candidate.AsEnumerable().Where(x => x.fk_candidate_id == user_id).Select(x => new CandidateInterestReceivedLimit
                    {
                        pk_candidate_interested_id = x.pk_candidate_interested_id,
                        requirement_title_select = x.requirement_title_select,
                        requirement_title = x.requirement_title,
                        //pk_vendor_plan_id = x.requirement_title,
                        requested_date = x.requested_date,
                        vendor_name=_context.portal_user.Where(p=>p.pk_user_id==x.fk_user_id).FirstOrDefault().ToString()
                    }).ToList();

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
          
    }
}