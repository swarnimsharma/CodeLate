using Portal.Common;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PortalBL.RequirementBL
{
    interface IRequirementBL
    {
        ResponseOut ChangeStatusByUser(int post_id,int status, string message);
        ResponseOut PostRequirementUpload(PostRequirementImportViewModel data);
    }
}
