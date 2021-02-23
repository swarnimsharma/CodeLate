using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Common
{
    public class EnumSection
    {
        enum InterestForCandidates
        {
            Pending = 0,
            Approved=1,
            Closed=2,
            OnHold=3
        }

       public enum CandidateJobType
        {
            Both = 1,
            FullTime = 2,
            Contractual = 3
        }

        public enum PlanDuration
        {
            Monthly=1,
            Quarterly=2,
            Yearly = 3
        }
    }
}