using Portal.Common;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PortalBL.EmailBL
{
    interface IEmailBL
    {
        ResponseOut ContactUsEmail(EmailContactUsViewModel data);
    }
}
