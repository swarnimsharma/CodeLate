using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portal.Common;
using Portal.ViewModels;
using Portal.DAL;
using System.Data.Entity.Validation;

namespace Portal.PortalBL.UserBL
{
    public class UserEngine : IUserBL
    {
        public ResponseOut ResetUserPassword(PasswordResetViewModel model, int user_id)
        {
            ResponseOut _responseOut = new ResponseOut();

            string old_encrypt_password = Utilities.MD5Hash(model.old_password);
            string new_encrypt_password = Utilities.MD5Hash(model.new_password);

            using (PortalEntities _context = new PortalEntities())
            {
                try
                {
                    portal_user _user = _context.portal_user.Find(user_id);
                    if (_user.password == old_encrypt_password)
                    {
                        _user.password = new_encrypt_password;
                        int return_code = _context.SaveChanges();
                        if (return_code > 0)
                        {
                            _responseOut.status = ActionStatus.Success;
                            _responseOut.message = ActionMessage.PasswordUpdated;
                        }
                    }
                    else
                    {
                        _responseOut.status = ActionStatus.Fail;
                        _responseOut.message = ActionMessage.PasswordOldMismatch;
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

        public ResponseOut UpdateUserProfile(UserViewModel model, int user_id)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                ResponseOut responseOut = new ResponseOut();
                try
                {
                    portal_user _user = _context.portal_user.Find(user_id);
                    _user.firstname = model.firstname;
                    _user.lastname = model.lastname;
                    _user.updated_date = DateTime.Now;
                    _user.email = model.email;
                    _user.contact = model.contact;
                    _user.fk_country_id = model.fk_country_id;
                    _user.fk_city_id = model.fk_city_id;
                    _user.fk_state_id = model.fk_state_id;
                    _user.profile_pic = model.profile_pic;
                    _user.about_us = model.about_us;
                    _user.is_active = model.is_active;
                    _user.is_paid_user = model.is_paid_user;
                    int return_code = _context.SaveChanges();
                    if (return_code == 1)
                    {
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.ProfileUpdatedSuccess;
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

        public UserViewModel GetUserProfile(int id)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var data = _context.portal_user.AsEnumerable().Where(x => x.pk_user_id == id).Select(x => new UserViewModel
                {
                    pk_user_id = x.pk_user_id,
                    about_us = x.about_us,
                    contact = x.contact,
                    email = x.email,
                    firstname = x.firstname,
                    fk_city_id = Convert.ToInt32(x.fk_city_id),
                    fk_country_id = Convert.ToInt32(x.fk_country_id),
                    fk_state_id = Convert.ToInt32(x.fk_state_id),
                    lastname = x.lastname,
                    profile_pic = x.profile_pic,
                    user_name = x.user_name,
                    is_active = x.is_active,
                    is_paid_user = x.is_paid_user,
                    fk_user_type = x.portal_user_role_mapping.Where(y => y.fk_user_id == x.pk_user_id).Select(y => Convert.ToInt32(y.fk_role_id)).FirstOrDefault(),
                    city_name = x.fk_city_id != null ? x.portal_city.city_name : ""
                }).FirstOrDefault();

                return data;
            }
        }

        public ResponseOut Authentication(string user_name, string password)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                ResponseOut responseOut = new ResponseOut();
                try
                {
                    var data = _context.portal_user.AsEnumerable().Where(x => x.user_name == user_name && x.password == Utilities.MD5Hash(password)).Select(x => new
                    {
                        x.pk_user_id,
                        x.portal_user_role_mapping.FirstOrDefault().fk_role_id,
                        full_name = x.firstname + " " + x.lastname,
                        x.user_name,
                        x.is_paid_user,
                        x.is_active,
                        x.is_email_verified
                    }).FirstOrDefault();
                    if (data != null)
                    {
                        if(data.is_email_verified)
                        {
                            if (data.is_active)
                            {
                                AppSession.SetSessionValue(SessionKey.CurrentUserID, data.pk_user_id);
                                AppSession.SetSessionValue(SessionKey.CurrentUserName, data.full_name);
                                AppSession.SetSessionValue(SessionKey.CurrentUserRoleID, data.fk_role_id);
                                AppSession.SetSessionValue(SessionKey.CurrentUserPremium, data.is_paid_user);
                                responseOut.message = ActionMessage.SuccessLogin;
                                responseOut.status = ActionStatus.Success;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.InActiveLogin;
                                responseOut.status = ActionStatus.Fail;
                            }
                        }
                        else
                        {
                            responseOut.message = ActionMessage.EmailNotVerified;
                            responseOut.status = ActionStatus.Fail;
                        }

                    }
                    else
                    {
                        responseOut.message = ActionMessage.FailedLogin;
                        responseOut.status = ActionStatus.Fail;
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

        public List<UserViewModel> GetUserList(int? user_id = null)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                var data = _context.portal_user.AsEnumerable().Select(x => new UserViewModel
                {
                    pk_user_id = x.pk_user_id,
                    firstname = x.firstname,
                    lastname = x.lastname,
                    user_name = x.user_name,
                    email = x.email,
                    user_role = x.portal_user_role_mapping.Where(y => y.fk_user_id == x.pk_user_id).Select(y => y.portal_role.role_name).FirstOrDefault(),
                    city_name = x.fk_city_id != null ? x.portal_city.city_name : "",
                    created_date = x.created_date.ToString("dd/MM/yyyy"),
                    active_or_not = x.is_active ? "Active" : "In Active",
                    is_paid_user = x.is_paid_user
                }).ToList();

                return data;
            }
        }

        public ResponseOut AddUserProfile(UserViewModel model)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                ResponseOut responseOut = new ResponseOut();
                try
                {
                    var check_user_name = _context.portal_user.AsEnumerable().Where(x => x.user_name.ToLower() == model.user_name.ToLower()).Any();
                    var check_email = _context.portal_user.AsEnumerable().Where(x => (model.email != null && model.email != "") && x.email.ToLower() == model.email.ToLower()).Any();

                    if (check_user_name)
                    {
                        responseOut.status = ActionStatus.Fail;
                        responseOut.message = ActionMessage.UsernameAlreadyExist;
                        return responseOut;
                    }

                    if (check_email)
                    {
                        responseOut.status = ActionStatus.Fail;
                        responseOut.message = ActionMessage.EmailAlreadyExist;
                        return responseOut;
                    }

                    portal_user _user = new portal_user();
                    _user.firstname = model.firstname;
                    _user.lastname = model.lastname;
                    _user.updated_date = DateTime.Now;
                    _user.user_hash = Guid.NewGuid().ToString();
                    _user.email = model.email;
                    _user.contact = model.contact;
                    _user.password = Utilities.MD5Hash(model.password);
                    _user.user_name = model.user_name;
                    if (model.fk_country_id != 0)
                    {
                        _user.fk_country_id = model.fk_country_id;
                    }
                    else
                    {
                        _user.fk_country_id = null;
                    }
                    _user.fk_city_id = model.fk_city_id;
                    _user.fk_state_id = model.fk_state_id;
                    _user.profile_pic = model.profile_pic;
                    _user.about_us = model.about_us;
                    _user.is_active = model.is_active;
                    _context.portal_user.Add(_user);
                    int return_code = _context.SaveChanges();
                    if (return_code == 1)
                    {
                        portal_user_role_mapping _role_map = new portal_user_role_mapping();
                        _role_map.fk_role_id = model.fk_user_type;
                        _role_map.fk_user_id = _user.pk_user_id;
                        _context.portal_user_role_mapping.Add(_role_map);
                        _context.SaveChanges();
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.ProfileCreatedSuccess;

                    }
                    responseOut.trnId = _user.pk_user_id;
                    responseOut.email = _user.email;
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                }
                return responseOut;
            }
        }

        public ResponseOut RegisterUser(RegisterUserViewModel user)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                ResponseOut responseOut = new ResponseOut();
                try
                {
                    var check_user_name = _context.portal_user.Where(x => x.user_name.ToLower() == user.username.ToLower()).Any();
                    var check_email = _context.portal_user.AsEnumerable().Where(x => (user.email != null && user.email != "" && x.email != null) && x.email.ToLower() == user.email.ToLower()).Any();
                    var hash = Guid.NewGuid().ToString();

                    if (check_user_name)
                    {
                        responseOut.status = ActionStatus.Fail;
                        responseOut.message = ActionMessage.UsernameAlreadyExist;
                        return responseOut;
                    }

                    if (check_email)
                    {
                        responseOut.status = ActionStatus.Fail;
                        responseOut.message = ActionMessage.EmailAlreadyExist;
                        return responseOut;
                    }

                    portal_user _user = new portal_user();
                    _user.firstname = user.fullname;
                    _user.created_date = DateTime.Now;
                    _user.email = user.email;
                    _user.contact = user.contact;
                    _user.password = Utilities.MD5Hash(user.password);
                    _user.user_name = user.username;
                    _user.is_active = true;
                    _user.user_hash = hash;
                    _context.portal_user.Add(_user);
                    int return_code = _context.SaveChanges();
                    if (return_code == 1)
                    {
                        portal_user_role_mapping _role_map = new portal_user_role_mapping();
                        _role_map.fk_role_id = user.user_type;
                        _role_map.fk_user_id = _user.pk_user_id;
                        _context.portal_user_role_mapping.Add(_role_map);
                        _context.SaveChanges();
                        responseOut.status = ActionStatus.Success;
                        responseOut.message = ActionMessage.ProfileCreatedSuccess;
                    }
                    responseOut.trnId = _user.pk_user_id;
                    responseOut.hash = _user.user_hash;
                    responseOut.email = _user.email;
                }
                catch (Exception ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                }
                return responseOut;
            }
        }

        public ResponseOut VerifyUserEmail(string hash)
        {
            using (PortalEntities _context = new PortalEntities())
            {
                ResponseOut responseOut = new ResponseOut();
                try
                {
                    string status = "Verified";
                    var check_user_status = _context.portal_user.Where(x => x.user_hash == hash).FirstOrDefault();
                    if (check_user_status != null)
                    {
                        if (check_user_status.is_email_verified)
                        {
                            status = "Already Verified";
                        }
                        else
                        {
                            check_user_status.is_email_verified = true;
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        status = "Invalid";
                    }
                    responseOut.status = ActionStatus.Success;
                    responseOut.message = status;
                }
                catch (Exception ex)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ex.Message;
                }
                return responseOut;
            }
        }

    }
}