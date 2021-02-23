using Portal.Common;
using Portal.DAL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PortalBL.PlansBL
{
    interface IPlansBL
    {
        ResponseOut AddPlans(PlansViewModel model);
        ResponseOut UpdatePlans(PlansViewModel model);
        ResponseOut AddVendorPlans(VendorPlansViewModel model);
        ResponseOut VendorPlansPricing(VendorPlansPricing model);
        List<PlansViewModel> GetPlans();
        PlansViewModel GetPlansById(int id);
        List<VendorPlansViewModel> GetVendorPlans();
        VendorPlansViewModel GetVendorPlansById(int id);
        List<VendorPlansPricing> GetVendorPricingPlans(int? user_id);
        VendorPlansPricing GetVendorPricingPlansById(int id);
        ResponseOut UpdateVendorPlans(VendorPlansViewModel model);
        ResponseOut AddUpdateSelectPlanByVendor(int plan_id, int vendor_id);
        List<VendorPlansPricing> GetCandidatePricingPlans(int? user_id);
        ResponseOut AddUpdateSelectPlanByCandidate(PlansViewModel model);
        List<getIsApprovedCandidatePlans_Result> GetApprovedCandidatePlan();
        ResponseOut UpdatePlanByPlanIDByAdmin(int planId);
        List<CandidateInterestReceivedLimit> CandidateInterestReceivedLimits(int? user_id);
    }
}
