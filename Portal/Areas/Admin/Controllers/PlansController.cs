using Portal.Common;
using Portal.PortalBL.PlansBL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace Portal.Areas.Admin.Controllers
{
   
    public class PlansController : Controller
    {
        // GET: Admin/Plans
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlansController));
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Plans()
        {
            return View();
        }

        public ActionResult VendorPlans()
        {
            return View();
        }

       
        public ActionResult CandidatePlans()
        {
            return View();
        }

        public PartialViewResult _PlansList()
        {
            IPlansBL planBL = new PlansEngine();
            List<PlansViewModel> PlansList = new List<PlansViewModel>();
            try
            {
                PlansList = planBL.GetPlans();
                return PartialView(PlansList);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PlansController/_PlansList", ex);
                return null;

            }
        }

        public PartialViewResult _VendorPlansList() {

            IPlansBL planBL = new PlansEngine();
            List<VendorPlansViewModel> VendorPlansList = new List<VendorPlansViewModel>();
            try {
                VendorPlansList = planBL.GetVendorPlans();
                return PartialView(VendorPlansList);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PlansController/_VendorPlansList", ex);
                return null;
            }
        }

        [HttpPost]
        public ActionResult AddPlanType(PlansViewModel data)
        {
            IPlansBL planBL = new PlansEngine();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (data.pk_plan_id == 0)
                {
                    responseOut = planBL.AddPlans(data);
                }
                else
                {
                    responseOut = planBL.UpdatePlans(data);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in PlansController/AddPlanType", ex);
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPlansById(int id)
        {
            IPlansBL planBL = new PlansEngine();
            try
            {
                var data = planBL.GetPlansById(id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PlansController/GetPlansById", ex);
                throw ex;
            }
        }

        public ActionResult AddVendorPlan(VendorPlansViewModel data)
        {
            IPlansBL planBL = new PlansEngine();
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (data.pk_vendor_plan_id == 0)
                {
                    responseOut = planBL.AddVendorPlans(data);
                }
                else
                {
                    responseOut = planBL.UpdateVendorPlans(data);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in PlansController/AddVendorPlan", ex);
                responseOut.message = ActionMessage.ApplicationException;
                responseOut.status = ActionStatus.Fail;
            }
            return Json(responseOut, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVendorPlansById(int id)
        {
            IPlansBL planBL = new PlansEngine();
            try
            {
                var data = planBL.GetVendorPlansById(id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PlansController/GetVendorPlansById", ex);
                throw;
            }
        }

    }
}