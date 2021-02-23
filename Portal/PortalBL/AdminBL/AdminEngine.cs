using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portal.ViewModels;
using Portal.DAL;
using Portal.Common;

namespace Portal.PortalBL.AdminBL
{
    public class AdminEngine : IAdminBL
    {
        public List<PostYourRequirement> GetAllPostRequirement(string title = null)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var data = _context.portal_get_post_requirement(title).AsEnumerable().Select(x => new PostYourRequirement
                {
                    post_id = x.pk_requirement_id,
                    email_id = x.email_id,
                    fullname = x.fullname,
                    message = x.message,
                    mobile_no = x.mobile_no,
                    requirement_title = x.requirement_title,
                    subject = x.subject,
                    status = x.approved_status,
                    added_datetime = x.added_date != null ? x.added_date.Value.ToString("dd/MM/yyyy") : ""
                }).ToList();
                return data;
            }
        }

        public ResponseOut SubmitPostStatus(SubmitYourRequirement status)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                ResponseOut responseOut = new ResponseOut();
                try
                {
                    if (status.status == 1)
                    {
                        foreach (var val in status.vendor_ids)
                        {
                            portal_requirement_vendor_mapping vendor = new portal_requirement_vendor_mapping();
                            vendor.fk_post_id = status.post_id;
                            vendor.fk_vendor_id = val;
                            vendor.map_date = DateTime.Now;
                            _context.portal_requirement_vendor_mapping.Add(vendor);
                            _context.SaveChanges();
                        }
                        var data = _context.portal_post_requirement.Where(x => x.pk_requirement_id == status.post_id).FirstOrDefault();
                        data.approved_status = status.status;
                        data.status_reason = status.reason_status;
                        int result = _context.SaveChanges();
                        if (result > 0)
                        {
                            responseOut.message = ActionMessage.RequirementApprovedStatus;
                            responseOut.status = ActionStatus.Success;
                        }
                    }
                    else
                    {
                        var data = _context.portal_post_requirement.Where(x => x.pk_requirement_id == status.post_id).FirstOrDefault();
                        data.approved_status = status.status;
                        data.status_reason = status.reason_status;
                        int result = _context.SaveChanges();
                        if (result > 0)
                        {
                            responseOut.message = ActionMessage.RequirementRejectedStatus;
                            responseOut.status = ActionStatus.Success;
                        }

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

        public ResponseOut SubmitClientFeedback(WhatClientSays status)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                ResponseOut responseOut = new ResponseOut();
                try
                {
                    if (status.id == 0)
                    {
                        portal_what_client_says _client = new portal_what_client_says();
                        _client.title = status.title;
                        _client.client_name = status.client_name;
                        _client.discription = status.description;
                        _client.added_datetime = DateTime.Now;
                        _client.is_published_by_admin = status.is_published;
                        _client.image = status.client_image;
                        _context.portal_what_client_says.Add(_client);
                        int result = _context.SaveChanges();
                        if (result > 0)
                        {
                            responseOut.status = ActionStatus.Success;
                            responseOut.message = ActionMessage.FeedbackCreated;
                        }
                    }
                    else
                    {
                        portal_what_client_says _client = _context.portal_what_client_says.Where(x=>x.pk_client_what_says_id==status.id).FirstOrDefault();
                        _client.title = status.title;
                        _client.client_name = status.client_name;
                        _client.discription = status.description;
                        _client.added_datetime = DateTime.Now;
                        _client.is_published_by_admin = status.is_published;
                        _client.image = status.client_image;
                        int result = _context.SaveChanges();
                        if (result > 0)
                        {
                            responseOut.status = ActionStatus.Success;
                            responseOut.message = ActionMessage.FeedbackUpdated;
                        }
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

        public List<WhatClientSays> GetClientFeedback(string title = null)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var data = _context.portal_what_client_says.AsEnumerable().Select(x => new WhatClientSays
                {
                    id = x.pk_client_what_says_id,
                    title = x.title,
                    description = x.discription,
                    client_name = x.client_name,
                    is_published = Convert.ToBoolean(x.is_published_by_admin),
                    client_image = x.image,
                    added_datetime = x.added_datetime != null ? x.added_datetime.Value.ToString("dd/MM/yyyy") : ""
                }).ToList();
                return data;
            }
        }

        public WhatClientSays GetSingleClientFeedback(int id)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var data = _context.portal_what_client_says.Where(x => x.pk_client_what_says_id == id).AsEnumerable().Select(x => new WhatClientSays
                {
                    id = x.pk_client_what_says_id,
                    title = x.title,
                    description = x.discription,
                    client_name = x.client_name,
                    is_published = Convert.ToBoolean(x.is_published_by_admin),
                    client_image = x.image,
                }).FirstOrDefault();

                return data;
            }
        }
    }
}