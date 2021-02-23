var is_project_outsource = true;

var Home = {
    LoadCandidateProfile: function () {
        $.ajax({
            url: "/Home/ShowCandidateProfiles",
            data: { search: $('#home_search').val() },
            dataType: "html",
            type: "GET",
            error: function (err) {
                $("#show_candidate_profile").html("");
                $("#show_candidate_profile").html(err);
            },
            success: function (data) {
                $("#show_candidate_profile").html("");
                $("#show_candidate_profile").html(data);
            }
        });
    },
    SearchForJobs: function () {
        debugger;
        var append_expertise_tag = "";
        //var tags = $("#search_skills").tagit("assignedTags");
        var datasource = expertise_dataSource;
        var tag_ids = $("#search_skills").attr('data-id');

        if (tag_ids != null && tag_ids != "") {
            debugger
            var temp_array_tag = tag_ids.split(',');

            $.each(temp_array_tag, function (index, value) {
                append_expertise_tag += datasource.find(x => x.value === value).name + ',';
            });
            if (append_expertise_tag.length > 0) {
                debugger;
                append_expertise_tag = append_expertise_tag.substring(0, append_expertise_tag.length - 1);
            }
        }

        var data = {
            title: append_expertise_tag,
            locality: $('#search_location').val(),
            experience: $('#search_experience').val()
        };

        if (data != null && data.title != "") {
            $('#search_skills').removeClass('required_fields_border');
            $('#search_validation_message').hide();
            var url = "/Home/CandidateProfiles?title=" + data.title + "&locality=" + data.locality + "&experience=" + data.experience;
            window.location.href = url;
        }
        else {
            $('#search_skills').addClass('required_fields_border');
            $('#search_validation_message').show();
            setTimeout(function () {
                $('#search_skills').removeClass('required_fields_border');
                $('#search_validation_message').hide();
            }, 5000);
        }
       
    },
    PostYourRequirement: function () {
        var data = Home.GetRequirementParams();
        if (data != null) {
            $("#post_requirement_btn").attr("disabled", true);

            $.ajax({
                url: "/Home/PostYourRequirement",
                cache: false,
                type: "POST",
                dataType: "json",
                beforeSend: function () {
                    $('#post_btn_spinner').removeClass('hidden');
                    $('#post_requirement_btn').attr('disabled', 'disabled');
                },
                data: JSON.stringify(Home.GetRequirementParams()),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status == "SUCCESS") {
                        $('#post_alert_message').addClass('show');
                        $('#post_alert_message').removeClass('alert-warning');
                        $('#post_alert_message').addClass('alert-success');
                        $('#post_status_message').text(data.message);
                        Home.ResetRequirementForm();
                        //System.LoadNotification("requirement_notification_text", "success", data.message, "requirement_notification_block");
                        setTimeout(function () {
                            $("#post_requirement_btn").attr("disabled", false);
                            $('#post_requirement_modal').modal('hide');
                            $('#post_btn_spinner').addClass('hidden');
                        }, 3000);
                    }
                    else {
                        $('#post_alert_message').addClass('show');
                        $('#post_alert_message').removeClass('alert-success');
                        $('#post_alert_message').addClass('alert-warning');
                        $('#post_status_message').text(data.message);
                        $('#post_btn_spinner').addClass('hidden');
                        //System.LoadNotification("requirement_notification_text", "warning", data.message, "requirement_notification_block");
                    }
                    //$('#requirement_notification_block').show();
                },
                error: function (err) {
                    System.LoadNotification("requirement_notification_text", "warning", data.message, "requirement_notification_block");
                },
                complete: function () {
                    Home.ResetRequirementForm();
                    setTimeout(function () {
                        $("#post_requirement_btn").removeAttr("disabled");
                    }, 3000);
                }
            });
        }
    },
    GetRequirementParams: function () {
        $('input').removeClass('required_fields_border');
        $('select').removeClass('required_fields_border');
        var required_fields = [];
        var data = {
            requirement_title: $('#post_requirement_title').val(),
            fullname: $('#post_requirement_name').val(),
            email_id: $('#post_requirement_email').val(),
            mobile_no: $('#post_requirement_mobile').val(),
            message: $('#post_requirement_description').val(),
            subject: $('#post_requirement_subject').val(),
            project_type: $('#project_type').val()
        };

        if ($('#hidden_current_user_id').val == null || $('#hidden_current_user_id').val == '' || $('#hidden_current_user_id').val == undefined) {
            if (data.fullname == null || data.fullname == "") {
                required_fields.push('post_requirement_name');
            }
            if (data.email_id == null || data.email_id == "") {
                required_fields.push('post_requirement_email');
            }
            if (data.mobile_no == null || data.mobile_no == "") {
                required_fields.push('post_requirement_mobile');
            }
        }

        if (data.subject == null || data.subject == "") {
            required_fields.push('post_requirement_subject');
        }
        if ((data.project_type == null || data.project_type == "") && is_project_outsource) {
            required_fields.push('post_requirement_project_type');
        }
        if (data.requirement_title == null || data.requirement_title == "") {
            required_fields.push('post_requirement_title');
        }
        if (required_fields.length > 0) {
            Account.RequiredValidation(required_fields);
            $('#post_required_fields_message').show();
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
    ResetRequirementForm: function () {
        $('#post_requirement_title').val('');
        $('#post_requirement_name').val('');
        $('#post_requirement_email').val('');
        $('#post_requirement_mobile').val('');
        $('#post_requirement_description').val('');
        $('#post_requirement_subject').val('');
        $('#project_type').val('');
        $('input').removeClass('required_fields_border');
        $('select').removeClass('required_fields_border');
        $('#post_btn_spinner').addClass('hidden');
        $('#post_requirement_btn').removeAttr('disabled', 'disabled');
        $('#post_required_fields_message').hide();
    },
    ProjectOutSource: function () {
        is_project_outsource = true;
        Home.ResetRequirementForm();
        $('#post_requirement_modal').modal('show');
        $('#project_type_section').show()
        $('#post_requirement_title').val('Project_Outsource');
        $('#post_requirement_title').attr("disabled", true);
    },
    OnOpenPostRequirement: function () {
        is_project_outsource = false;
        Home.ResetRequirementForm();
        $('#project_type_section').hide();
        $('#post_requirement_project_type').val('');
        $('#post_requirement_title').removeAttr("disabled");
    },
    ContactEnquiryRequest: function () {
        $.ajax({
            url: "/Home/ContactUs",
            cache: false,
            type: "POST",
            dataType: "json",
            beforeSend: function () {
                //$('#post_btn_spinner').removeClass('hidden');
                $('#enquiry_send_button').attr('disabled', 'disabled');
            },
            data: JSON.stringify(Home.GetEnquiryParameters()),
            contentType: 'application/json',
            success: function (data) {
                console.log(data);
                $('#contact_name').val('');
                $('#contact_email').val('');
                $('#contact_contact').val('');
                $('#contact_message').val('');
                toastr8.success('Your enquiry has been sent to our customer support !');
            },
            complete: function () {
                $('#enquiry_send_button').removeAttr('disabled');
            }
        });
    },
    GetEnquiryParameters: function () {
        var data = {
            name: $('#contact_name').val(),
            email: $('#contact_email').val(),
            contact_no: $('#contact_contact').val(),
            message: $('#contact_message').val()
        };

        return data;
    },
    LoadOurClients: function () {
        $.ajax({
            url: "/Home/WhatClientSays",
            dataType: "html",
            type: "GET",
            error: function (err) {
                $("#append_what_client_says").html("");
                $("#append_what_client_says").html(err);
            },
            success: function (data) {
                $("#append_what_client_says").html("");
                $("#append_what_client_says").html(data);
            }
        });
    }
}