using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class UserViewModel
    {
        public int pk_user_id { get; set; }
        public string user_name { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string profile_pic { get; set; }
        public string about_us { get; set; }
        public string contact { get; set; }
        public bool is_active { get; set; }
        public bool is_delete { get; set; }
        public int fk_country_id { get; set; }
        public int fk_state_id { get; set; }
        public int fk_city_id { get; set; }
        public int fk_user_type { get; set; }
        public string user_role { get; set; }
        public string state_name { get; set; }
        public string city_name { get; set; }
        public string created_date { get; set; }
        public string active_or_not { get; set; }
        public bool is_paid_user { get; set; }
    }

    public class PasswordResetViewModel
    {
        public string old_password { get; set; }
        public string new_password { get; set; }
    }

    public class LoginViewModel
    {
        public string user_name { get; set; }
        public string password { get; set; }
    }

    public class RegisterUserViewModel
    {
        public int user_type { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string contact { get; set; }
    }
}