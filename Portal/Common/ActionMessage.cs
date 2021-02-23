using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Common
{
    public class ActionMessage
    {
        #region UserProfile
        public const String ProfileCreatedSuccess = "Profile Created Successfully !";
        public const String ProfileUpdatedSuccess = "Profile Updated Successfully !";
        public const String ProfileRemovedSuccess = "Profile Removed Successfully !";
        public const String UsernameAlreadyExist =  "Username is Already Exist !";
        public const String EmailAlreadyExist = "EmailID is Already Exist !";
        #endregion

        #region Exception
        public const String ApplicationException = "Error occured in application. Please contact administrator";
        public const String SessionException = "Session Expired!!!";
        public const String AuthenticationException = "User not authenticated";
        public const String AccessDenied = "Access Denied";
        public const String ProbleminData = "Problem in Data";
        #endregion

        #region Password
        public const String PasswordUpdated = "Password Updated Successfully !";
        public const String PasswordOldMismatch = "Old Password Wrong !";
        #endregion

        #region Login
        public const String FailedLogin = "Log In Failed: Username / Password is incorrect.!";
        public const String SuccessLogin = "Log In Succesfully";
        public const String InActiveLogin = "you are not activated by Admin, Please contact to Admin";
        public const String EmailNotVerified = "Please verify your email or contact to Admin";
        #endregion

        #region Interested
        public const String SaveInterested = "Thanks for showing your interested!";
        #endregion

        #region Post Requirement
        public const String Mandatory = "Please fill mandatory fields";
        public const String RequirementPost = "Thanks for your requirement";
        public const String RequirementApprovedStatus = "Successfully Approved";
        public const String RequirementRejectedStatus = "Successfully Rejected";
        #endregion
        #region Feedback
        public const String FeedbackCreated = "Successfully Created Feedback";
        public const String FeedbackUpdated = "Successfully Updated Feedback";
        #endregion

        #region common
        public const String RecordSaved = "Record Saved";
        #endregion

        #region
        public const String PlansCreatedSuccess = "Created Successfully !";
        public const String PlansUpdatedSuccess = "Updated Successfully !";
        public const String PlansDeletedSuccess = "Deleted Successfully !";
        public const String PlansSelectedSuccess = "Plan Selected Successfully !";
        #endregion

    }
}