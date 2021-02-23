using Portal.Common;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PortalBL.AdminBL
{
    interface IAdminBL
    {
        List<PostYourRequirement> GetAllPostRequirement(string title = null);
        ResponseOut SubmitPostStatus(SubmitYourRequirement data);
        ResponseOut SubmitClientFeedback(WhatClientSays status);
        List<WhatClientSays> GetClientFeedback(string title = null);
        WhatClientSays GetSingleClientFeedback(int id);
    }
}
