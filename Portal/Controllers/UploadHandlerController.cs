using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using log4net;

namespace Portal.Controllers
{
    public class UploadHandlerController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UploadHandlerController));
        // GET: UploadHandler
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UploadFile()
        {
            string imgname = string.Empty;
            string hostWithPortNumber = HttpContext.Request.Url.Authority;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                try
                {

                    var pic = System.Web.HttpContext.Current.Request.Files["Files"];
                    string _comPath = "";
                    if (pic.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(pic.FileName);
                        var _ext = Path.GetExtension(pic.FileName);
                        imgname = Guid.NewGuid().ToString();

                        if (pic.FileName.ToLower().Contains("xls"))
                        {
                            _comPath = Path.Combine(Server.MapPath("~/Assets/Files/") + imgname + _ext);
                        }
                        else
                        {
                            _comPath = Path.Combine(Server.MapPath("~/Assets/Images/") + imgname + _ext);
                        }

                        ViewBag.Msg = _comPath;
                        var path = _comPath;

                        // Saving Image in Original Mode
                        pic.SaveAs(path);
                        imgname = imgname + _ext;
                        // resizing image
                        //if(!pic.FileName.ToLower().Contains("xls"))
                        //{
                        //    MemoryStream ms = new MemoryStream();
                        //    WebImage img = new WebImage(_comPath);

                        //    if (img.Width > 200)
                        //        img.Resize(200, 200);
                        //    img.Save(_comPath);
                        //}

                        if ((pic.FileName.ToLower().Contains("jpg")) || (pic.FileName.ToLower().Contains("jpeg")) || (pic.FileName.ToLower().Contains("png")))
                        {
                            MemoryStream ms = new MemoryStream();
                            WebImage img = new WebImage(_comPath);

                            if (img.Width > 200)
                                img.Resize(200, 200);
                            img.Save(_comPath);
                        }
                        // end resize
                    }
                    return Json(Convert.ToString(imgname), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    Log.Error("Error in UploadHandlerController/UploadFile", ex);
                    return Json(Convert.ToString(ex.HResult + " " + ex.Message + " " + ex.StackTrace), JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }
    }
}