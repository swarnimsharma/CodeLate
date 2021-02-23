var global_user_id = 0
var User = {
    UpdateProfile: function () {
        $('input').removeClass('required_fields_border');
        $('select').removeClass('required_fields_border');
        var data = User.GetProfileParams();
        if (data != null) {
            $.ajax({
                url: "/User/UpdateUserProfile",
                cache: false,
                type: "POST",
                dataType: "json",
                data: JSON.stringify(User.GetProfileParams()),
                contentType: 'application/json',
                beforeSend: function () {
                    $('#user_btn_spinner').removeClass('hidden')
                    $('#user_add_update_profile_btn').attr('disabled', 'disabled');
                },
                success: function (data) {
                    if (data.status == "SUCCESS") {
                        $('#user_profile_alert_message').addClass('show');
                        $('#user_profile_alert_message').removeClass('alert-warning');
                        $('#user_profile_alert_message').addClass('alert-success');
                        $('#user_status_message').text(data.message);
                       
                        setTimeout(function () {
                            $('#user_profile_alert_message').removeClass('show');
                            $('#user_btn_spinner').addClass('hidden')
                            $('#user_add_update_profile_btn').removeAttr('disabled');
                        }, 4000);
                        if (window.location.href.indexOf("Admin/Admin/User") > -1) {
                            Admin.LoadUserList();
                            $("#user_profile_add_section").slideUp();
                            User.ResetUserFields();
                        }
                        //System.LoadNotification("personal_notification_text", "success", data.message, "personal_notification_block");
                    }
                    else {
                        $('#user_profile_alert_message').addClass('show');
                        $('#user_profile_alert_message').removeClass('alert-success');
                        $('#user_profile_alert_message').addClass('alert-warning');
                        $('#user_status_message').text(data.message);
                        $('#user_btn_spinner').addClass('hidden');
                        $('#user_add_update_profile_btn').removeAttr('disabled');
                    }
                },
                error: function (err) {
                    //ShowModel("Error", err)
                }
            });
        }
    },
    GetProfileParams: function () {
        debugger;
        var required_fields = [];
        var data = {
            pk_user_id: global_user_id,
            firstname: $('#user_first_name').val(),
            lastname: $('#user_last_name').val(),
            email: $('#user_email').val(),
            profile_pic: $('#hidden_user_pic').val(),
            about_us: $('#user_about_us').val(),
            contact: $('#user_contact').val(),
            fk_country_id: $('#user_fk_country_id').val(),
            fk_state_id: $('#user_fk_state_id').val(),
            fk_city_id: $('#user_fk_city_id').val(),
            fk_user_type: $('#user_fk_user_id').val(),
            user_name: $('#user_username').val(),
            password: $('#user_userpassword').val(),
            is_active: $('#user_is_active').is(":checked"),
            is_paid_user: $('#user_is_paid').is(":checked")
        }
        if (data.firstname == null || data.firstname == '') {
            required_fields.push('user_first_name');
        }
        if (data.lastname == null || data.lastname == '') {
            required_fields.push('user_last_name');
        }
        if (data.email == null || data.email == '') {
            required_fields.push('user_email');
        }

        if ($('#hidden_user_type').val() == 'admin') {

            if ((data.password == null || data.password == "") && global_user_id == 0) {
                required_fields.push('user_userpassword');
            }
            if (data.user_name == null || data.user_name == "") {
                required_fields.push('user_username');
            }
            if (data.fk_user_type == null || data.fk_user_type == "") {
                required_fields.push('user_fk_user_id');
            }
        }

        if (required_fields.length > 0) {
            User.RequiredValidation(required_fields);
            $('#user_required_fields_message').show();
        }
        else {
            return data;
        }
        return null;
    },
    UpdatePassword: function () {
        debugger
        var data = User.GetPasswordParams();
        if (data != null) {
            $.ajax({
                url: "/User/UpdatePassword",
                cache: false,
                type: "POST",
                dataType: "json",
                data: JSON.stringify(User.GetPasswordParams()),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status == "SUCCESS") {
                        debugger;
                        System.LoadNotification("password_notification_text", "success", data.message, "password_notification_block");
                    }
                    else {
                        System.LoadNotification("password_notification_text", "warning", data.message, "password_notification_block");
                    }
                    $('#password_notification_block').show();
                },
                error: function (err) {
                    //ShowModel("Error", err)
                }
            });
        }
        else {
            $('#password_status_text').text('New Password and Confirm Password Mismatched')
        }
    },
    GetPasswordParams: function () {
        if ($('#user_password_new').val() == $('#user_password_confirm').val()) {
            var data = {
                old_password: $('#user_password_old').val(),
                new_password: $('#user_password_new').val()
            };
            return data;
        }
        else {
            return null;
        }

    },
    GetProfile: function (id) {
        debugger;
        if (id != null && id != 0 && id != undefined) {
            global_user_id = id;
        }
        $.ajax({
            type: "GET",
            url: "/User/GetUserProfile?id=" + id,
            data: {},
            dataType: "json",
            asnc: false,
            success: function (data) {
                debugger;
                $('#user_pk_id').val(id),
                $('#user_first_name').val(data.firstname);
                $('#user_last_name').val(data.lastname);
                $('#user_email').val(data.email);
                $('#user_contact').val(data.contact);
                $('#user_fk_state_id').val(data.fk_state_id);
                $('#user_fk_city_id').val(data.fk_city_id);
                $('#user_fk_user_id').val(data.fk_user_type);
                $('#user_about_us').val(data.about_us);
                $('#user_username').val(data.user_name);
                if (data.is_active) {
                    $('#user_is_active').prop('checked', true);
                }
                else {
                    $('#user_is_active').prop('checked', false);
                }
                if (data.is_paid_user) {
                    $('#user_is_paid').prop('checked', true);
                }
                else {
                    $('#user_is_paid').prop('checked', false);
                }
                System.LoadDropDown("county_dropdown", "user_fk_country_id", null, data.fk_country_id);

                if (data.profile_pic != null && data.profile_pic != "") {
                    $("#profile_image_preview").attr('src', "../../Assets/Images/" + data.profile_pic);
                    $('#hidden_user_pic').val(data.profile_pic);
                }
                else {
                    $("#profile_image_preview").attr('src', "http://simpleicon.com/wp-content/uploads/account.png");
                }
                if (data.fk_country_id != null && data.fk_country_id != 0) {
                    User.LoadState(data.fk_country_id, data.fk_state_id);
                }
                if (data.fk_state_id != null && data.fk_state_id != 0) {
                    User.LoadCity(data.fk_state_id, data.fk_city_id);
                }
                User.OnSelectUser(data.fk_user_type);
            },
            complete: function () {
                //Candidate.OpenCandidateModal(id);
            },
            error: function (Result) {
                //$("#" + bind_control).append($("<option></option>").val(0).html("Loading Problem.."));
            }
        });
    },
    LoadState: function (id, selected_id) {
        System.LoadDropDown("state_dropdown", "user_fk_state_id", id, selected_id);
    },
    LoadCity: function (id, selected_id) {
        System.LoadDropDown("city_dropdown", "user_fk_city_id", id, selected_id);
    },
    AddUserProfile: function () {
        $('input').removeClass('required_fields_border');
        $('select').removeClass('required_fields_border');
        var data = User.GetProfileParams();
        if (data != null) {
            $.ajax({
                url: "/User/AddUserProfile",
                cache: false,
                type: "POST",
                dataType: "json",
                data: JSON.stringify(User.GetProfileParams()),
                beforeSend: function () {
                    $('#user_btn_spinner').removeClass('hidden')
                    $('#user_add_update_profile_btn').attr('disabled', 'disabled');
                },
                data: JSON.stringify(User.GetProfileParams()),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status == "SUCCESS") {
                        $('#user_profile_alert_message').addClass('show');
                        $('#user_profile_alert_message').removeClass('alert-warning');
                        $('#user_profile_alert_message').addClass('alert-success');
                        $('#user_status_message').text(data.message);
                        Admin.LoadUserList();
                        User.ResetUserFields();
                        setTimeout(function () {
                            $('#user_profile_alert_message').removeClass('show');
                            $('#user_btn_spinner').addClass('hidden');
                            $('#user_add_update_profile_btn').removeAttr('disabled');
                            $("#user_profile_add_section").slideUp();
                        }, 4000);
                    }
                    else {
                        if (data.message == 'EmailID is Already Exist !') {
                            $('#user_email').addClass('required_fields_border');
                        }
                        if (data.message == 'Username is Already Exist !') {
                            $('#user_username').addClass('required_fields_border');
                        }
                        $('#user_profile_alert_message').addClass('show');
                        $('#user_profile_alert_message').removeClass('alert-success');
                        $('#user_profile_alert_message').addClass('alert-warning');
                        $('#user_status_message').text(data.message);
                        $('#user_btn_spinner').addClass('hidden');
                        $('#user_add_update_profile_btn').removeAttr('disabled');
                    }
                },
                error: function (err) {
                    //ShowModel("Error", err)
                },
                complete: function () {

                }
            });
        }
    },
    ResetUserFields: function () {
        $('input').removeClass('required_fields_border');
        $('select').removeClass('required_fields_border');
        $('#user_first_name').val(''),
        $('#user_last_name').val(''),
        $('#user_email').val(''),
        $('#hidden_user_pic').val(''),
        $('#user_about_us').val(''),
        $('#user_contact').val(''),
        $('#user_fk_country_id').val(''),
        $('#user_fk_state_id').val(''),
        $('#user_fk_city_id').val(''),
        $('#user_fk_user_id').val(''),
        $('#user_username').val(''),
        $('#user_userpassword').val(''),
         $('#user_is_active').prop('checked', false);
        $('#user_is_paid').prop('checked', false);
    },
    RequiredValidation: function (data) {
        $.each(data, function (index, value) {
            $('#' + value).addClass('required_fields_border');
        });
    },
    OnSelectUser:function(val)
    {
        debugger;
        if (val == 2 || val == '2')
        {
            $('#user_is_paid_section').show();
        }
        else {
            $('#user_is_paid_section').hide();
        }
    }
}