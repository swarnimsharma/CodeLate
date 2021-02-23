using Portal.PortalBL.DropDown;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace Portal.Controllers
{
    public class DropDownController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DropDownController));
        [HttpGet]
        public JsonResult GetDropdown(string type, int? id = null, string ids = null,string search=null)
        {
            List<DropDownViewModal> dropDownList = new List<DropDownViewModal>();
            IDropDownBL dropdownBL = new DropDownEngine();
            try
            {
                dropDownList = dropdownBL.GetList(type, id,ids,search);
            }
            catch (Exception ex)
            {
                Log.Error("Error in DropDownController/GetDropdown", ex);
                throw ex;
            }
            return Json(dropDownList, JsonRequestBehavior.AllowGet);
        }
    }
}