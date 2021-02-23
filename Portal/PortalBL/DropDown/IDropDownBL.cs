using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.PortalBL.DropDown
{
    public interface IDropDownBL
    {
        List<DropDownViewModal> GetList(string type, int? id, string ids=null, string search=null);
    }
}