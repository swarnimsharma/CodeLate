using Portal.Common;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PortalBL.UserBL
{
    interface IUserBL
    {
        ResponseOut UpdateUserProfile(UserViewModel model, int user_id);
        ResponseOut ResetUserPassword(PasswordResetViewModel model, int user_id);
        ResponseOut Authentication(string user_name, string password);
        UserViewModel GetUserProfile(int id);
        List<UserViewModel> GetUserList(int? user_id=null);
        ResponseOut AddUserProfile(UserViewModel model);
        ResponseOut RegisterUser(RegisterUserViewModel user);
        ResponseOut VerifyUserEmail(string hash);
    }
}
