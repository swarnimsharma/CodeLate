using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portal.Common;
using Portal.ViewModels;
using Portal.DAL;

namespace Portal.PortalBL.RecruiterBL
{
    public class RecruiterEngine : IRecruiterBL
    {
        public ResponseOut AddUpdateRecruiter(RecruiterViewModel model, int user_id)
        {
            ResponseOut _responseOut = new ResponseOut();

            using (PortalEntities _context = new PortalEntities())
            {

                try
                {
                    portal_recruiter_profile _recruiter;
                    if (model.pk_resource_profile_id == 0)
                    {
                        _recruiter = new portal_recruiter_profile();
                        _recruiter.is_active = true;
                        _recruiter.is_deleted = false;
                        _recruiter.created_date = DateTime.Now;
                    }
                    else
                    {
                        _recruiter = _context.portal_recruiter_profile.Find(model.pk_resource_profile_id);
                        _recruiter.is_active = model.is_active;
                        _recruiter.is_deleted = model.is_deleted;
                        _recruiter.updated_date = DateTime.Now;
                    }

                    if(_recruiter.job_type!=null && _recruiter.job_type!=0)
                    {

                    }
                    _recruiter.firstname = model.firstname;
                    _recruiter.lastname = model.lastname;
                    _recruiter.minimum_tenure = model.minimum_tenure;
                    if (model.fk_country_id != 0)
                    {
                        _recruiter.fk_country_id = model.fk_country_id;
                    }
                    if (model.fk_state_id != 0)
                    {
                        _recruiter.fk_state_id = model.fk_state_id;
                    }
                    if (model.fk_city_id != 0)
                    {
                        _recruiter.fk_city_id = model.fk_city_id;
                    }
                    _recruiter.fk_experience_level = model.fk_experience_level;
                    _recruiter.expertise_profession = model.expertise_profession;
                    _recruiter.contact_no = model.contact_no;
                    _recruiter.email_id = model.email_id;
                    _recruiter.profile_pic = model.profile_pic;
                    _recruiter.about_us = model.about_us;
                    _recruiter.availablity = model.availability;
                    _recruiter.fk_vendor_id = user_id;
                    if (model.pk_resource_profile_id == 0)
                    {
                        _context.portal_recruiter_profile.Add(_recruiter);
                        _context.SaveChanges();
                        _responseOut.status = ActionStatus.Success;
                        _responseOut.message = ActionMessage.ProfileCreatedSuccess;
                    }
                    else
                    {
                        _context.SaveChanges();
                        _responseOut.status = ActionStatus.Success;
                        _responseOut.message = ActionMessage.ProfileUpdatedSuccess;
                    }

                }
                catch (Exception ex)
                {
                    _responseOut.status = ActionStatus.Fail;
                    _responseOut.message = ActionMessage.ApplicationException;
                }
                return _responseOut;
            }
        }

        public List<RecruiterResultSet> GetRecruitersList(int? user_id, string candidate_name = null, string candidate_technology = null, string candidate_experience = null) 
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var data = _context.portal_get_candidates_by_vendor(user_id, candidate_name, candidate_technology, candidate_experience).AsEnumerable().Select(x => new RecruiterResultSet
                {
                    pk_resource_profile_id = x.pk_resource_profile_id,
                    contact = x.contact_no,
                    created_datetime = x.created_date != null ? x.created_date.Value.ToString("dd/MM/yyyy") : "",
                    email = x.email_id,
                    experience = x.experience_years + " (" + x.level + ")",
                    fullname = x.firstname + " " + x.lastname,
                    is_active = x.is_active ? "Active" : "Not Active",
                    availablity = x.availablity,
                    expertise = x.expertise_text,
                    country = x.country_name,
                    state = x.state_name,
                    city = x.city_name,
                    about_us = x.about_us,
                    is_already_interested = x.pk_candidate_interested_id != null ? true : false,
                    job_type = x.job_type != null ? ((EnumSection.CandidateJobType)x.job_type).ToString() : ""
                }).ToList();
                if (!string.IsNullOrEmpty(candidate_technology))
                {
                    data = data.Where(x => x.expertise.ToLower().Contains(candidate_technology.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(candidate_experience))
                {
                    data = data.Where(x => x.experience.ToLower().Contains(candidate_experience.ToLower())).ToList();
                }
                return data;
            }
        }

        public RecruiterViewModel GetCandidateProfile(int pk_candidate_id)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var data = _context.portal_recruiter_profile.AsEnumerable().Where(x => x.pk_resource_profile_id == pk_candidate_id).Select(x => new RecruiterViewModel
                {
                    pk_resource_profile_id = x.pk_resource_profile_id,
                    firstname = x.firstname,
                    lastname = x.lastname,
                    email_id = x.email_id,
                    contact_no = x.contact_no,
                    minimum_tenure = x.minimum_tenure,
                    expertise_profession = x.expertise_profession,
                    fk_city_id = Convert.ToInt32(x.fk_city_id),
                    fk_country_id = Convert.ToInt32(x.fk_country_id),
                    fk_experience_level = Convert.ToInt32(x.fk_experience_level),
                    fk_state_id = Convert.ToInt32(x.fk_state_id),
                    is_active = x.is_active,
                    is_deleted = x.is_deleted,
                    about_us = x.about_us,
                    profile_pic = x.profile_pic,
                    availability=x.availablity,
                    job_type=Convert.ToInt32(x.job_type)
                }).FirstOrDefault();

                return data;
            }
        }

        public ResponseOut SaveInterestedCandidate(InterestedToCandidateViewModel model, int user_id)
        {
            ResponseOut responseOut = new ResponseOut();
            using (PortalEntities _context = new PortalEntities())
            {
                try
                {
                    var vendor_id = _context.portal_recruiter_profile.Where(x => x.pk_resource_profile_id == model.candidate_id).Select(x => x.fk_vendor_id).FirstOrDefault();
                    portal_for_interested_Candidate _interested = new portal_for_interested_Candidate();
                    _interested.fk_user_id = user_id;
                    _interested.requested_date = DateTime.Now;
                    _interested.requirement_title_select = model.requirement_title_select;
                    _interested.requirement_title = model.requirement_title;
                    _interested.fk_candidate_id = model.candidate_id;
                    _interested.is_active = true;
                    if (vendor_id != 0 && vendor_id!=null)
                    {
                        _interested.fk_vendor_id = vendor_id;
                    }
                    _context.portal_for_interested_Candidate.Add(_interested);
                    int return_code = _context.SaveChanges();
                    if (return_code > 0)
                    {
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.SaveInterested;
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

        public List<InterestedToCandidateViewModel> GetInterestedUsers(int user_id)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var data = _context.portal_for_interested_Candidate.AsEnumerable().Where(x => x.is_active == true && x.fk_vendor_id == user_id).Select(x => new InterestedToCandidateViewModel
                {
                    pk_interest_id = x.pk_candidate_interested_id,
                    candidate_id = Convert.ToInt32(x.fk_candidate_id),
                    candidate_name = x.fk_candidate_id != null ? (x.portal_recruiter_profile.firstname + " " + x.portal_recruiter_profile.lastname) : "",
                    requirement_title = x.requirement_title,
                    requested_date = x.requested_date.ToString("dd/MM/yyyy"),
                    no_of_request = _context.portal_for_interested_Candidate.Where(y => y.fk_candidate_id == x.fk_candidate_id).Count(),
                    fk_user_id = Convert.ToInt32(x.fk_user_id)
                }).ToList();
                return data;
            }
        }

    }
}