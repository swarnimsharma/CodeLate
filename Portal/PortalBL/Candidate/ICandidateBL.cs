using Portal.Common;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.PortalBL.Candidate
{
    public interface ICandidateBL
    {
        List<CandidatesListViewModel> GetCandidatesList(string[] title, string locality = null, int? experience = null, int? user_id = null, string sort_by_level = null, string availibility = null, string sort_by_new_old = null, string sort_by_profile = null, string job_type=null);
        ResponseOut PostRequirement(PostYourRequirement model, int id);
        List<PostYourRequirement> GetApprovedRequirements(int user_id, string requirement = null, string status = null);
        List<InterestedToCandidateViewModel> GetInterestedUsers(int user_id);
        List<PostYourRequirement> GetAllPostedRequirement(int user_id);
        PostYourRequirement GetSingleApprovedRequirement(int post_id);
        ResponseOut AddUpdateCandidateProfile(CandidateProfile model);
        CandidateProfile GetCandidateProfile(int user_id);
    }
}