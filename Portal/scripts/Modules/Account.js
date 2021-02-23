var Account = {
    UserLogin: function () {
        var data = Account.LoginParams();
        if (data != null) {
            $('#Required_fields_message').hide();
            $.ajax({
                url: "/Home/Login",
                cache: false,
                type: "POST",
                dataType: "json",
                beforeSend: function () {
                    $('#login_btn_spinner').show();
                    $('#login_btn').attr('disabled', 'disabled');
                },
                data: JSON.stringify(Account.LoginParams()),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status == "SUCCESS") {
                        $('#login_alert_message').addClass('show');
                        $('#login_alert_message').removeClass('alert-warning');
                        $('#login_alert_message').addClass('alert-success');
                        $('#login_status_message').text(data.message);
                        setTimeout(function () {
                            window.location.reload();
                        }, 1000);
                        //$('#candidate_profile_modal').modal('hide');
                    }
                    else {
                        $('#login_alert_message').addClass('show');
                        $('#login_alert_message').removeClass('alert-success');
                        $('#login_alert_message').addClass('alert-warning');
                        $('#login_status_message').text(data.message);
                        $('#login_btn_spinner').hide();
                    }
                },
                error: function (err) {
                    //ShowModel("Error", err)
                },
                complete: function () {

                    $('#login_btn').removeAttr('disabled');
                }
            });
        }
    },
    LoginParams: function () {
        $('input').removeClass('required_fields_border');
        var required_fields = [];
        var data = {
            user_name: $('#login_user_name').val(),
            password: $('#login_user_password').val()
        }
        if (data.user_name == null || data.user_name == '') {
            required_fields.push('login_user_name');
        }
        if (data.password == null || data.password == '') {
            required_fields.push('login_user_password');
        }

        if (required_fields.length > 0) {
            Account.RequiredValidation(required_fields);
            $('#Required_fields_message').show();
        }
        else {
            return data;
        }
        return null;
    },
    RequiredValidation: function (data) {
        $.each(data, function (index, value) {
            $('#' + value).addClass('required_fields_border');
        });
    },
    ResetLogin: function () {
        $('#login_btn_spinner').hide();
        $('#Required_fields_message').hide();
        $('input').removeClass('required_fields_border');
        $('#login_alert_message').removeClass('show');
        $('#login_alert_message').removeClass('alert-warning');
        $('#login_alert_message').removeClass('alert-success');
        $('#login_status_message').text('');
        $('#login_user_name').val('');
        $('#login_user_password').val('');
        $('#user_required_fields_message').hide();
    },
    UserRegister: function () {
        var data = Account.UserRegisterParams();
        if (data != null) {
            $('#register_required_fields_message').hide();
            $.ajax({
                url: "/User/RegisterUser",
                cache: false,
                type: "POST",
                dataType: "json",
                beforeSend: function () {
                    $('#register_btn_spinner').show();
                    $('#register_btn').attr('disabled', 'disabled');
                },
                data: JSON.stringify(Account.UserRegisterParams()),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status == "SUCCESS") {
                        $('#register_alert_message').addClass('show');
                        $('#register_alert_message').removeClass('alert-warning');
                        $('#register_alert_message').addClass('alert-success');
                        $('#register_status_message').text(data.message + " " + "Please verify your email, We have sent a link to your register email address!!");
                        setTimeout(function () {
                            window.location.reload();
                            $('#register_modal').modal('hide');
                        }, 5000);
                        //$('#candidate_profile_modal').modal('hide');
                    }
                    else {
                        $('#register_alert_message').addClass('show');
                        $('#register_alert_message').removeClass('alert-success');
                        $('#register_alert_message').addClass('alert-warning');
                        $('#register_status_message').text(data.message);
                        $('#register_btn_spinner').hide();
                    }
                },
                error: function (err) {
                    //ShowModel("Error", err)
                },
                complete: function () {

                    $('#register_btn').removeAttr('disabled');
                }
            });
        }
    },
    UserRegisterParams: function () {
        $('input').removeClass('required_fields_border');
        $('select').removeClass('required_fields_border');

        var required_fields = [];
        var data = {
            user_type: $('#register_user_type').val(),
            fullname: $('#register_full_name').val(),
            username: $('#register_user_name').val(),
            email: $('#register_user_email').val(),
            password: $('#register_user_password').val(),
            contact: $('#register_user_contact').val(),
        }
        if (data.user_type == null || data.user_type == '') {
            required_fields.push('register_user_type');
        }
        if (data.fullname == null || data.fullname == '') {
            required_fields.push('register_full_name');
        }

        if (data.username == null || data.username == '') {
            required_fields.push('register_user_name');
        }

        if (data.email == null || data.email == '') {
            required_fields.push('register_user_email');
        }
        else {
            if (!(Account.IsValidEmail(data.email))) {
                Account.RequiredValidation(required_fields);
                //$('#register_required_fields_message').show();
                //$('#spnEmailrequired').show();
                alert('Invalid Email Format');
                return null;
            }
        }

        if (data.password == null || data.password == '') {
            required_fields.push('register_user_password');
        }

        if (data.contact == null || data.contact == '') {
            required_fields.push('register_user_contact');
        }

        if (required_fields.length > 0) {
            Account.RequiredValidation(required_fields);
            $('#register_required_fields_message').show();
        }
        else {
            return data;
        }
        return null;
    },
    IsValidEmail: function (email) {
        debugger;
        var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        var a=regex.test(email);
        if (!regex.test(email)) {
            return false;
        } else {
            return true;
        }
    },
   
    ResetRegister: function () {
        $('#register_btn_spinner').hide();
        $('#register_required_fields_message').hide();
        $('input').removeClass('required_fields_border');
        $('#register_alert_message').removeClass('show');
        $('#register_alert_message').removeClass('alert-warning');
        $('#register_alert_message').removeClass('alert-success');
        $('#register_status_message').text('');
        $('#register_user_type').val(""),
            $('#register_full_name').val(""),
            $('#register_user_name').val(""),
            $('#register_user_email').val(""),
            $('#register_user_password').val(""),
            $('#register_user_contact').val("")
        $('#spnEmailrequired').hide();
        $('#user_required_fields_message').hide();
    }
}