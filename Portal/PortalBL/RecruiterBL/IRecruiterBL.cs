using Portal.Common;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PortalBL.RecruiterBL
{
    interface IRecruiterBL
    {
        ResponseOut AddUpdateRecruiter(RecruiterViewModel model, int user_id);
        List<RecruiterResultSet> GetRecruitersList(int? user_id, string candidate_name = null, string candidate_technology = null, string candidate_experience = null);
        RecruiterViewModel GetCandidateProfile(int pk_candidate_id);
        ResponseOut SaveInterestedCandidate(InterestedToCandidateViewModel model, int user_id);
        List<InterestedToCandidateViewModel> GetInterestedUsers(int user_id);
    }
}
