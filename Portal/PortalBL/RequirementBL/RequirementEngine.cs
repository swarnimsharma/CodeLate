using Portal.Common;
using Portal.DAL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.PortalBL.RequirementBL
{
    public class RequirementEngine : IRequirementBL
    {
        public ResponseOut ChangeStatusByUser(int post_id, int status, string message)
        {
            ResponseOut responseOut = new ResponseOut();
            using (PortalEntities _context = new PortalEntities())
            {
                try
                {
                    var data = _context.portal_post_requirement.Where(x => x.pk_requirement_id == post_id).FirstOrDefault();
                    data.approved_status = status;
                    data.client_reason = message;
                    int result = _context.SaveChanges();
                    if (result > 0)
                    {
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.RequirementPost;
                    }
                }

                catch (Exception ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                }
                return responseOut;
            }
        }

        public ResponseOut PostRequirementUpload(PostRequirementImportViewModel data)
        {
            ResponseOut responseOut = new ResponseOut();
            using (PortalEntities _context=new PortalEntities())
            {
                try
                {
                    portal_post_requirement _requirement = new portal_post_requirement();
                    int user_id = 0;
                    if (user_id == 0)
                    {
                        user_id = _context.portal_user.Where(x => !string.IsNullOrEmpty(data.email_id) && x.email.ToLower() == data.email_id.ToLower()).Select(x => x.pk_user_id).FirstOrDefault();
                    }
                    if (user_id == 0)
                    {
                        user_id = _context.portal_user.Where(x => !string.IsNullOrEmpty(data.contact_details) && x.contact == data.contact_details).Select(x => x.pk_user_id).FirstOrDefault();
                    }
                    _requirement.mobile_no = data.contact_details;
                    _requirement.email_id = data.email_id;
                    _requirement.fullname = data.client_name;
                    _requirement.added_date = DateTime.Now;
                    _requirement.approved_status = 0;
                    if (user_id != 0)
                    {
                        _requirement.fk_client_id = user_id;
                    }
                    _requirement.subject = data.requirement_title;
                    _requirement.message = data.requirement_description;

                    _context.portal_post_requirement.Add(_requirement);
                    int result = _context.SaveChanges();
                    if (result > 0)
                    {
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.RequirementPost;
                    }
                }
                catch (Exception ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                }
                return responseOut;
            }
        }
    }
}