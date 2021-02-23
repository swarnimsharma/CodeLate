using Portal.PortalBL.ImportExcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace Portal.Controllers
{
    public class ImportFileController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ImportFileController));
        // GET: ImportFile
        [HttpPost]
        public ActionResult UploadFile(string name, string type=null)
        {
            string result = "";
            try
            {
                IImportBL importBL = new ImportEngine();
                result = importBL.ImportData(null, type, name);
            }
            catch (Exception ex)
            {
                Log.Error("Error in ImportFileController/UploadFile", ex);
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}