var Vendor = {
    LoadProfileImageHandler: function () {
        $("#user_profile_image").change(function () {
            var data = new FormData();
            var files = $("#user_profile_image").get(0).files;
            if (files.length > 0) {
                data.append("Files", files[0]);
            }
            $.ajax({
                url: "/UploadHandler/UploadFile",
                type: "POST",
                processData: false,
                contentType: false,
                data: data,
                success: function (response) {
                    //code after success                    
                    $("#preview_profile_image").attr('src', '/Assets/Images/' + response);
                },
                error: function (er) {
                    alert(er);
                }

            });
        })
    },
    UpdateProfile: function () {
        $.ajax({
            url: "../Vendor/UpdateUserProfile",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(Vendor.GetProfileParams()),
            contentType: 'application/json',
            success: function (data) {
                if (data.status == "SUCCESS") {
                    System.LoadNotification("personal_notification_text", "success", data.message, "personal_notification_block");
                }
                else {
                    System.LoadNotification("personal_notification_text", "warning", data.message, "personal_notification_block");
                }
                $('#personal_notification_block').show();
            },
            error: function (err) {
                //ShowModel("Error", err)
            }
        });
    },
    GetProfileParams: function () {
        var data = {
            firstname: $('#user_first_name').val(),
            lastname: $('#user_last_name').val(),
            email: $('#user_email').val(),
            profile_pic: $('#hidden_user_pic').val(),
            about_us: $('#user_about_us').val(),
            contact: $('#user_contact').val(),
            fk_country_id: $('#user_fk_country_id').val(),
            fk_state_id: $('#user_fk_state_id').val(),
            fk_city_id: $('#user_fk_city_id').val(),
        }
        return data;
    },
    UpdatePassword: function () {
        debugger
        var data = Vendor.GetPasswordParams();
        if (data != null) {
            $.ajax({
                url: "../Vendor/UpdatePassword",
                cache: false,
                type: "POST",
                dataType: "json",
                data: JSON.stringify(Vendor.GetPasswordParams()),
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
        $.ajax({
            type: "GET",
            url: "../Vendor/GetVendorProfile",
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

                $('#user_about_us').val(data.about_us);
                System.LoadDropDown("county_dropdown", "user_fk_country_id", null, data.fk_country_id);

                if (data.profile_pic != null && data.profile_pic != "") {
                    $("#profile_image_preview").attr('src', "../../Assets/Images/" + data.profile_pic);
                    $('#hidden_user_pic').val(data.profile_pic);
                }
                else {
                    $("#profile_image_preview").attr('src', "http://simpleicon.com/wp-content/uploads/account.png");
                }
                if (data.fk_country_id != null && data.fk_country_id != 0) {
                    Vendor.LoadState(data.fk_country_id, data.fk_state_id);
                }
                if (data.fk_state_id != null && data.fk_state_id != 0) {
                    Vendor.LoadCity(data.fk_state_id, data.fk_city_id);
                }
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
    GetAllLeads: function () {
        var data = Vendor.GetLeadSearchFilter();
        $.ajax({
            url: "../Recruiter/GetAllLeads",
            cache: false,
            type: "GET",
            dataType: "html",
            data: { requirement: data.requirement, status: data.status },
            contentType: 'application/html; charset=utf-8',
            success: function (result) {
                $('#append_all_leads').html(result);
            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {
            }
        });
    },
    GetLeadSearchFilter: function () {
        var data = {
            requirement: $('#lead_requirement_title_search').val(),
            status: $('#lead_status_search').val(),
        }

        return data;
    },
    ShowInterestOnPlan: function (plan) {
        $(".divLoading").addClass('show');
        $.ajax({
            url: "../Vendor/ShowInterestOnPlan?plan=" + plan,
            cache: false,
            type: "GET",
            dataType: "json",
            success: function (data) {
                if (data.status == "SUCCESS") {
                    toastr8.success(data.message);
                }

            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {
                $(".divLoading").removeClass('show');
            }
        });
    }
};

var VendorPlan = {
    SelectVendorPlan: function (id) {
        var PlanID = $('#hdVendorPlanID').val();
        var helpText = "Do you wish to continue with this plan?";
        if (PlanID != '') {
            helpText = 'You already have a plan,Do you wish to continue with this plan?'
        }

        bootbox.confirm({
            message: helpText,
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    $.ajax({
                        url: "../Vendor/AddUpdatePlanSelectedByVendor?plan_id="+id,
                        cache: false,
                        type: "GET",
                        dataType: "json",
                        data: { plan_id: id },
                        contentType: 'application/json',
                        success: function (data) {
                            if (data.status == "SUCCESS") {
                                $('#hdVendorPlanID').val('');
                                $.ajax({
                                    method: 'GET',
                                    url: '../Vendor/_VendorPlan',
                                    success: function (data) {
                                        $('#divVendorPlan').html(data);
                                    }
                                });

                            }
                            else {

                            }
                        },
                        error: function (err) {
                            //ShowModel("Error", err)
                        }
                    });
                }
            }
        });
       
    }
};