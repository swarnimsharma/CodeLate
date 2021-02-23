using Portal.Common;
using Portal.DAL;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Web;

namespace Portal.PortalBL.Candidate
{
    public class CandidateEngine : ICandidateBL
    {
        public List<CandidatesListViewModel> GetCandidatesList(string[] title, string locality = null, int? experience = null, int? user_id = null, string sort_by_level = null, string availibility = null, string sort_by_new_old = null, string sort_by_profile = null, string job_type = null)
        {
            //if (!string.IsNullOrEmpty(title[0]))
            //{
            //    title = title[0].ToString().Split(',');
            //}
            //else
            //{

            //}

            if (job_type == "")
            {
                job_type = null;
            }

            using (PortalEntities _context = new PortalEntities())
            {
                List<CandidatesListViewModel> finalResult = new List<CandidatesListViewModel>();
                if (string.IsNullOrEmpty(locality))
                {
                    locality = null;
                }
                if (experience == 0)
                {
                    experience = null;
                }
                int job_title = Convert.ToInt32(job_type);

                var data = _context.portal_get_candidates(title[0], locality, experience, user_id, job_title).AsEnumerable().Select(x => new CandidatesListViewModel
                {
                    pk_resource_profile_id = Convert.ToInt32(x.pk_resource_profile_id),
                    contact = x.contact_no,
                    created_datetime = x.created_date != null ? x.created_date.Value.ToString("dd/MM/yyyy") : "",
                    added_date_time = x.created_date != null ? x.created_date : null,
                    email = x.email_id,
                    experience = x.experience_years + " (" + x.level + ")",
                    fullname = x.firstname + " " + x.lastname,
                    is_active = x.is_Active == true ? "Active" : "Not Active",
                    availablity = x.availablity,
                    expertise = x.expertise_text,
                    country = x.country_name,
                    state = x.state_name,
                    city = x.city_name,
                    about_us = x.about_us,
                    experience_id = Convert.ToInt32(x.fk_experience_level),
                    is_already_interested = x.pk_candidate_interested_id != null ? true : false,
                    //is_already_interested = _context.portal_for_interested_Candidate.Where(y => user_id != null && user_id != 0 && y.fk_user_id == user_id && y.fk_candidate_id == x.pk_resource_profile_id).Any(),
                    profile_pic = x.profile_pic,
                    job_type = x.job_type != null ? ((EnumSection.CandidateJobType)x.job_type).ToString() : ""

                }).ToList();

                //foreach (var str in title)
                //{
                //    List<CandidatesListViewModel> tempData = data.Where(x => x.expertise != null && x.expertise.ToLower().Contains(str.ToLower())).ToList();
                //    finalResult.AddRange(tempData);
                //}

                //finalResult = finalResult.Distinct().ToList();

                if (sort_by_level == "ord_by_desc_exp")
                {
                    data = data.OrderByDescending(x => x.experience_id).ToList();
                }
                if (sort_by_level == "ord_by_asc_exp")
                {
                    data = data.OrderBy(x => x.experience_id).ToList();
                }
                if (sort_by_new_old == "ord_by_new")
                {
                    data = data.Where(x => x.added_date_time != null).OrderByDescending(x => x.added_date_time).ToList();
                }
                if (sort_by_new_old == "ord_by_old")
                {
                    data = data.Where(x => x.added_date_time != null).OrderBy(x => x.added_date_time).ToList();
                }
                if (sort_by_profile == "with_pic")
                {
                    data = data.Where(x => x.profile_pic != null).ToList();
                }
                if (!string.IsNullOrEmpty(availibility))
                {
                    if (availibility == "Immediate")
                    {
                        data = data.Where(x => x.availablity != null && x.availablity == availibility).ToList();
                    }
                    if (availibility == "7")
                    {
                        data = data.Where(x => x.availablity != null && (x.availablity == "7" || x.availablity == "Immediate")).ToList();
                    }
                    if (availibility == "15")
                    {
                        data = data.Where(x => x.availablity != null && (x.availablity == "7" || x.availablity == "Immediate" || x.availablity == availibility)).ToList();
                    }
                    if (availibility == "20")
                    {
                        data = data.Where(x => x.availablity != null && (x.availablity == "7" || x.availablity == "15" || x.availablity == "Immediate" || x.availablity == availibility)).ToList();
                    }
                    if (availibility == "25")
                    {
                        data = data.Where(x => x.availablity != null && (x.availablity == "7" || x.availablity == "15" || x.availablity == "20" || x.availablity == "Immediate" || x.availablity == availibility)).ToList();
                    }
                }
                return data.ToList();
            }
        }

        public string SplitExpertise(string expertise)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                string str_expert = "";
                if (!string.IsNullOrEmpty(expertise))
                {
                    string[] number = expertise.Split(',').ToArray();

                    foreach (var item in number)
                    {
                        str_expert += _context.portal_experise.AsEnumerable().Where(x => x.pk_expertise_id == Convert.ToInt32(item)).Select(x => x.expertise_name).FirstOrDefault() + ",";
                    }
                    if (str_expert.Contains(","))
                    {
                        str_expert = str_expert.Substring(0, str_expert.Length - 1);
                    }
                }

                return str_expert;
            }
        }

        public string[] ConvertStringToArray(string str, char delimeter)
        {
            if (!String.IsNullOrEmpty(str))
            {
                string[] temp = str.Split(delimeter).ToArray();
                return temp;
            }
            return null;
        }

        public ResponseOut PostRequirement(PostYourRequirement model, int id)
        {
            ResponseOut responseOut = new ResponseOut();

            using (PortalEntities _context = new PortalEntities())
            {

                try
                {
                    portal_post_requirement requirement = new portal_post_requirement();
                    if (id != 0)
                    {
                        var data = _context.portal_user.Where(x => x.pk_user_id == id).FirstOrDefault();
                        model.email_id = data.email;
                        model.fullname = data.firstname + " " + data.lastname;
                        model.mobile_no = data.contact;
                        requirement.fk_client_id = id;
                    }

                    requirement.email_id = model.email_id;
                    requirement.fullname = model.fullname;
                    requirement.mobile_no = model.mobile_no;
                    requirement.requirement_title = model.requirement_title;
                    requirement.subject = model.subject;
                    requirement.message = model.message;
                    requirement.added_date = DateTime.Now;
                    requirement.project_type = model.project_type;
                    _context.portal_post_requirement.Add(requirement);
                    int result = _context.SaveChanges();
                    if (result > 0)
                    {
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.RequirementPost;
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    var outputLines = new List<string>();
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        outputLines.Add(string.Format(
                            "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                            DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            outputLines.Add(string.Format(
                                "- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage));
                        }
                    }
                    System.IO.File.AppendAllLines(@"c:\temp\errors.txt", outputLines);

                }
                catch (Exception ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                }
                return responseOut;
            }
        }

        public List<PostYourRequirement> GetApprovedRequirements(int user_id, string requirement = null, string status = null)
        {
            using (PortalEntities _context = new PortalEntities())
            {

                var data = from map in _context.portal_requirement_vendor_mapping
                           join vendor in _context.portal_user
                           on map.fk_vendor_id equals vendor.pk_user_id
                           join post in _context.portal_post_requirement
                           on map.fk_post_id equals post.pk_requirement_id
                           //where map.fk_vendor_id == user_id
                           select new PostYourRequirement
                           {
                               created_Date = map.map_date,
                               requirement_title = post.requirement_title,
                               subject = post.subject,
                               fullname = vendor.firstname + " " + vendor.lastname,
                               post_id = post.pk_requirement_id,
                               status = post.approved_status,
                               message = post.message,
                               status_by_codelate = post.status_by_codelate
                           };
                var final_result = data.AsEnumerable().Select(x => new PostYourRequirement
                {
                    added_datetime = x.created_Date != null ? x.created_Date.Value.ToString("dd/MM/yyyy") : "",
                    requirement_title = x.requirement_title,
                    subject = x.subject,
                    fullname = x.fullname,
                    post_id = x.post_id,
                    status = x.status,
                    status_by_codelate = x.status_by_codelate,
                    message = x.message
                });

                if (!string.IsNullOrEmpty(requirement))
                {
                    final_result = final_result.Where(x => x.requirement_title == requirement).ToList();
                }
                if (!string.IsNullOrEmpty(status))
                {
                    final_result = final_result.Where(x => x.status == Convert.ToInt32(status)).ToList();
                }
                return final_result.ToList();
            }
        }

        public List<InterestedToCandidateViewModel> GetInterestedUsers(int user_id)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var data = _context.get_candidate_show_interest_by_client(user_id).AsEnumerable().Select(x => new InterestedToCandidateViewModel
                {
                    pk_interest_id = Convert.ToInt32(x.pk_candidate_interested_id),
                    requirement_title = x.requirement_title,
                    requested_date = x.requested_date.ToString("dd/MM/yyyy"),
                    experience = x.experience_years + " (" + x.level + ")",
                    expertise = x.expertise,
                    status = x.status
                }).ToList();
                return data;
            }
        }

        public List<PostYourRequirement> GetAllPostedRequirement(int user_id)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var user_info = _context.portal_user.Where(x => x.pk_user_id == user_id).Select(x => new
                {
                    x.email,
                    x.contact
                }).FirstOrDefault();
                var data = _context.portal_post_requirement.AsEnumerable().Where(x => (x.fk_client_id == user_id) || (x.mobile_no == user_info.contact) || x.email_id == user_info.email).Select(x => new PostYourRequirement
                {
                    post_id = x.pk_requirement_id,
                    added_datetime = x.added_date != null ? x.added_date.Value.ToString("dd/MM/yyyy") : "",
                    email_id = x.email_id,
                    fullname = x.fullname,
                    message = x.message,
                    mobile_no = x.mobile_no,
                    project_type = x.project_type,
                    requirement_title = x.requirement_title,
                    status = x.approved_status,
                    subject = x.subject,
                    status_reason = x.status_reason,
                    client_reason = x.client_reason
                }).ToList();

                return data;
            }
        }
        public PostYourRequirement GetSingleApprovedRequirement(int post_id)
        {
            using (PortalEntities _context = new PortalEntities())
            {

                var data = from map in _context.portal_requirement_vendor_mapping
                           join vendor in _context.portal_user
                           on map.fk_vendor_id equals vendor.pk_user_id
                           join post in _context.portal_post_requirement
                           on map.fk_post_id equals post.pk_requirement_id
                           where post.pk_requirement_id == post_id
                           select new PostYourRequirement
                           {
                               created_Date = map.map_date,
                               requirement_title = post.requirement_title,
                               subject = post.subject,
                               fullname = vendor.firstname + " " + vendor.lastname,
                               email_id = post.email_id,
                               mobile_no = post.mobile_no,
                               post_id = post.pk_requirement_id,
                               message = post.message,
                               project_type = post.project_type,
                           };
                var final_result = data.AsEnumerable().Select(x => new PostYourRequirement
                {
                    added_datetime = x.created_Date != null ? x.created_Date.Value.ToString("dd/MM/yyyy") : "",
                    requirement_title = x.requirement_title,
                    subject = x.subject,
                    fullname = x.fullname,
                    project_type = x.project_type,
                    email_id = x.email_id,
                    message = x.message,
                    post_id = x.post_id,
                    mobile_no = x.mobile_no
                });
                return data.FirstOrDefault();
            }
        }

        public ResponseOut AddUpdateCandidateProfile(CandidateProfile model)
        {
            ResponseOut responseOut = new ResponseOut();
            using (PortalEntities _context = new PortalEntities())
            {
                try
                {
                    portal_user_candidate_info candidate_Info = new portal_user_candidate_info();
                    var is_Candidate_exist = _context.portal_user_candidate_info.Where(x => x.fk_user_id == model.fk_user_id).Any();
                    if (is_Candidate_exist)
                    {
                        candidate_Info = _context.portal_user_candidate_info.Where(x => x.fk_user_id == model.fk_user_id).FirstOrDefault();
                    }
                    portal_user portal_User = _context.portal_user.Where(x => x.pk_user_id == model.fk_user_id).FirstOrDefault();
                    candidate_Info.availability = model.availability;
                    candidate_Info.fk_user_id = model.fk_user_id;
                    candidate_Info.resume = model.resume;
                    candidate_Info.technologies = model.selected_technologies_name;
                    candidate_Info.preferred_city = model.city_listing;
                    candidate_Info.preferred_country = model.country_listing;
                    candidate_Info.preferred_state = model.state_listing;
                    candidate_Info.head_line = model.head_line;
                    if (!is_Candidate_exist)
                    {
                        _context.portal_user_candidate_info.Add(candidate_Info);
                    }

                    portal_User.fk_country_id = model.candidate_fk_country_id;
                    portal_User.fk_state_id = model.candidate_fk_state_id;
                    portal_User.fk_city_id = model.candidate_fk_city_id;
                    portal_User.profile_pic = model.profile_pic;
                    portal_User.firstname = model.first_name;
                    portal_User.lastname = model.last_name;
                    portal_User.email = model.email;
                    portal_User.contact = model.contact;
                    int result = _context.SaveChanges();
                    if (result > 0)
                    {
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.RequirementPost;
                    }
                }
                catch (EntityException ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                }
                catch (Exception ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                }

            }

            return responseOut;
        }

        public CandidateProfile GetCandidateProfile(int user_id)
        {
            using (PortalEntities _context = new PortalEntities())   
            {
                CandidateProfile candidate_data = new CandidateProfile();
                bool candidate = _context.portal_user_candidate_info.Where(x => x.fk_user_id == user_id).Any();
                if (candidate)
                {
                    candidate_data = _context.portal_user_candidate_info.Where(x => x.fk_user_id == user_id).AsEnumerable().Select(x => new CandidateProfile
                    {
                        fk_user_id = x.portal_user.pk_user_id,
                        about_us = x.portal_user.about_us,
                        email = x.portal_user.email,
                        first_name = x.portal_user.firstname,
                        last_name = x.portal_user.lastname,
                        candidate_fk_city_id = Convert.ToInt32(x.portal_user.fk_city_id),
                        candidate_fk_state_id = Convert.ToInt32(x.portal_user.fk_state_id),
                        candidate_fk_country_id = Convert.ToInt32(x.portal_user.fk_country_id),
                        availability = x.availability,
                        city_listing = x.preferred_city,
                        country_listing = x.preferred_country,
                        state_listing = x.preferred_state,
                        resume = x.resume,
                        profile_headline = x.head_line,
                        profile_pic = x.portal_user.profile_pic,
                        contact = x.portal_user.contact,
                        selected_technologies_name = x.technologies,
                    }).FirstOrDefault();
                }
                else
                {
                    candidate_data.fk_user_id = user_id;
                }

                return candidate_data;
            }
        }
    }
}