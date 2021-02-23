using Portal.Common;
using Portal.DAL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.PortalBL.VendorBL
{
    public class VendorEngine : IVendorBL
    {
        public SingleResponseOut<UserViewModel> ShowInterestOnPlan(int id)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                SingleResponseOut<UserViewModel> responseOut = new SingleResponseOut<UserViewModel>();
                var data = _context.portal_user.Where(x => x.pk_user_id == id).Select(x => new UserViewModel
                {
                   user_name=x.user_name,
                   firstname= x.firstname,
                   contact= x.contact,
                   email= x.email
                }).FirstOrDefault();

                responseOut.Model = data;

                return responseOut;
            }
        }
    }
}