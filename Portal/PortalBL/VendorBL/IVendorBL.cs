using Portal.Common;
using Portal.DAL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PortalBL.VendorBL
{
    interface IVendorBL
    {
        SingleResponseOut<UserViewModel> ShowInterestOnPlan(int id);
    }
}
