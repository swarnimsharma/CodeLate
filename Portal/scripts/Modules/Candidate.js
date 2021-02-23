var global_candidate_id = 0;

var Candidate = {
    SaveInterested: function () {
        $.ajax({
            url: "/Home/SaveInterestedToCandidate",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(Candidate.GetInterestParams()),
            contentType: 'application/json',
            success: function (data) {
                $('#candidate_interested_modal').modal('hide');
                window.location.reload();
            },
            error: function (err) {
                //ShowModel("Error", err)
            }
        });
    },
    GetInterestParams: function () {
        var data = {
            requirement_title_select: $('#requirement_title_select').val(),
            requirement_title: $('#requirement_title').val(),
            requirement_description: $('#requirement_description').val(),
            candidate_id: global_candidate_id
        }

        return data;
    },
    OpenInterestedModal: function (id) {
        //debugger;
        global_candidate_id = id;
        $('#candidate_interested_modal').modal('show');
    },
    SearchCandidates: function () {
        var data = Candidate.GetCandidateSearchParams();
        $.ajax({
            url: "/Candidates/CandidateProfileList",
            cache: false,
            type: "GET",
            dataType: "html",
            data: { title: data.title, locality: data.locality, experience: data.experience, sort_by_level: data.sort_by_level, sort_by_availability: data.availability, sort_by_new_old: data.sort_by_new_old, sort_by_profile_pic: data.sort_by_profile_pic, job_type: data.job_type },
            contentType: 'application/html; charset=utf-8',
            success: function (result) {
                $('#candidate_profile_list').html(result);
            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {
            }
        });
    },
    GetCandidateSearchParams: function () {
        var title = Candidate.GetUrlParameter('title');
        var data = {
            sort_by_level: $('#sort_by_expertise').val(),
            availability: $('#sort_by_availability').val(),
            sort_by_new_old: $('#sort_by_time').val(),
            sort_by_profile_pic: $('#sort_by_profile').val(),
            title: title,
            locality: $('#candidate_profiles_locality').val(),
            experience: $('#candidate_profiles_experience').val(),
            job_type: $('#sort_by_job_type').val()
        };
        return data;
    },
    GetUrlParameter: function (sParam) {
        var sPageURL = window.location.search.substring(1),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
            }
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
                //$('#user_pk_id').val(id),
                //    $('#user_first_name').val(data.firstname);
                //$('#user_last_name').val(data.lastname);
                //$('#user_email').val(data.email);
                //$('#user_contact').val(data.contact);
                //$('#user_fk_state_id').val(data.fk_state_id);
                //$('#user_fk_city_id').val(data.fk_city_id);
                //$('#user_fk_user_id').val(data.fk_user_type);
                //$('#user_about_us').val(data.about_us);
                //$('#user_username').val(data.user_name);
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
        System.LoadDropDown("state_dropdown", "candidate_fk_state_id", id, selected_id);
    },
    LoadCity: function (id, selected_id) {
        System.LoadDropDown("city_dropdown", "candidate_fk_city_id", id, selected_id);
    },

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
                    $('#candidate_btn_spinner').removeClass('hidden')
                    $('#candidate_add_update_profile_btn').attr('disabled', 'disabled');
                },
                success: function (data) {
                    if (data.status == "SUCCESS") {
                        $('#candidate_profile_alert_message').addClass('show');
                        $('#candidate_profile_alert_message').removeClass('alert-warning');
                        $('#candidate_profile_alert_message').addClass('alert-success');
                        //$('#user_status_message').text(data.message);

                        setTimeout(function () {
                            $('#candidate_profile_alert_message').removeClass('show');
                            $('#candidate_btn_spinner').addClass('hidden')
                            $('#candidate_add_update_profile_btn').removeAttr('disabled');
                        }, 4000);
                        //if (window.location.href.indexOf("Admin/Admin/User") > -1) {
                        //    Admin.LoadUserList();
                        //    $("#user_profile_add_section").slideUp();
                        //    User.ResetUserFields();
                        //}
                        //System.LoadNotification("personal_notification_text", "success", data.message, "personal_notification_block");
                    }
                    else {
                        $('#candidate_profile_alert_message').addClass('show');
                        $('#candidate_profile_alert_message').removeClass('alert-success');
                        $('#candidate_profile_alert_message').addClass('alert-warning');
                        //$('#user_status_message').text(data.message);
                        $('#candidate_btn_spinner').addClass('hidden');
                        $('#candidate_add_update_profile_btn').removeAttr('disabled');
                    }
                },
                error: function (err) {
                    //ShowModel("Error", err)
                }
            });
        }
    },
    GetProfileParams: function () {
        //debugger;
        var required_fields = [];
        var data = {
            pk_user_id: global_candidate_id,
            technology: $('#candidate_fk_technology').val(),
            current_location: $('#candidate_current_location').val(),
            fk_country_id: $('#candidate_fk_country_id').val(),
            fk_state_id: $('#candidate_fk_state_id').val(),
            fk_city_id: $('#candidate_fk_city_id').val(),
            fk_user_type: $('#candidate_available_within').val(),
            //user_name: $('#user_username').val(),
            //password: $('#user_userpassword').val(),
            //is_active: $('#user_is_active').is(":checked"),
            //is_paid_user: $('#user_is_paid').is(":checked")
        }
        if (data.technology == null || data.technology == '') {
            required_fields.push('candidate_fk_technology');
        }
        if (data.current_location == null || data.current_location == '') {
            required_fields.push('candidate_current_location');
        }
        if (data.email == null || data.email == '') {
            required_fields.push('user_email');
        }
        if (required_fields.length > 0) {
            User.RequiredValidation(required_fields);
            $('#candidate_required_fields_message').show();
        }
        else {
            return data;
        }
        return null;
    },
}