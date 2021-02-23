
var Candidate = {
    OpenCandidateModal: function (id) {
        $('#candidate_profile_modal').modal('show');
        if (id != null && id != 0) {
            $('#candidate_pk_id').val(id);

        }
        else {
            Candidate.ResetForm();
            //$("#candidate_expertise option:selected").prop("selected", false);
            $("#hidden_candidate_pic").val('');
            $("#profile_image_preview").attr('src', "http://simpleicon.com/wp-content/uploads/account.png");
            System.LoadDropDown("county_dropdown", "candidate_country");
            Candidate.LoadExpertise();
            Candidate.LoadExperience();
            Candidate.LoadState();
            Candidate.LoadCity();
        }

    },
    GetCandidateParams: function () {
        var expertise_selection = [];
        var comma_seperate_experts = "";
        $.each($("#candidate_expertise option:selected"), function () {
            expertise_selection.push($(this).val());
        });
        if (expertise_selection.length > 0) {
            comma_seperate_experts = expertise_selection.join(",")
        }

        var data = {
            pk_resource_profile_id: $('#candidate_pk_id').val(),
            firstname: $('#candidate_first_name').val(),
            lastname: $('#candidate_last_name').val(),
            email_id: $('#candidate_email').val(),
            contact_no: $('#candidate_contact').val(),
            fk_state_id: $('#candidate_state').val(),
            fk_city_id: $('#candidate_city').val(),
            fk_country_id: $('#candidate_country').val(),
            is_active: $('#candidate_active').is(":checked"),
            minimum_tenure: $('#candidate_tenure').val(),
            fk_experience_level: $('#candidate_experience').val(),
            expertise_profession: comma_seperate_experts,
            profile_pic: $('#hidden_candidate_pic').val(),
            about_us: $('#candidate_about_us').val(),
            availability: $('#candidate_availability').val(),
            job_type: $('#candidate_job_type').val()
        };
        return data;
    },

    SaveUpdateCandidate: function () {
        $.ajax({
            url: "/Recruiter/AddEditRecruiterProfile",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(Candidate.GetCandidateParams()),
            contentType: 'application/json',
            success: function (data) {
                if (data.status == "SUCCESS") {
                    $('#candidate_profile_modal').modal('hide');
                }
                else {
                    //ShowModel("Error", data.message)
                }
            },
            error: function (err) {
                //ShowModel("Error", err)
            }, complete: function () {
                //location.reload();
                Candidate.LoadCandidateList();
            }
        });
    },

    LoadState: function (id, selected_id) {
        System.LoadDropDown("state_dropdown", "candidate_state", id, selected_id);
    },
    LoadCity: function (id, selected_id) {
        System.LoadDropDown("city_dropdown", "candidate_city", id, selected_id);
    },
    LoadExperience: function (id) {
        System.LoadDropDown("experience_dropdown", "candidate_experience", null, id)
    },
    GetCandidateProfile: function (id) {
        Candidate.ResetForm();
        $.ajax({
            type: "GET",
            url: "/Recruiter/GetCandidateProfile?id=" + id,
            data: {},
            dataType: "json",
            asnc: false,
            success: function (data) {
                $('#candidate_pk_id').val(id),
                    $('#candidate_first_name').val(data.firstname);
                $('#candidate_last_name').val(data.lastname);
                $('#candidate_email').val(data.email_id);
                $('#candidate_contact').val(data.contact_no);
                $('#candidate_state').val(data.fk_state_id);
                $('#candidate_city').val(data.fk_city_id);
                $('#candidate_country').val(data.fk_country_id);
                //$('#candidate_active').val(data.is_active);
                $('#candidate_tenure').val(data.minimum_tenure);
                $('#candidate_experience').val(data.fk_experience_level);
                $('#candidate_about_us').val(data.about_us);
                if (data.is_active) {
                    $("#candidate_active").prop('checked', true);
                }
                else {
                    $("#candidate_active").prop('checked', false);
                }
                if (data.expertise_profession != null) {
                    Candidate.LoadExpertise(data.expertise_profession)
                }
                Candidate.LoadExperience(data.fk_experience_level);
                if (data.fk_country_id != null && data.fk_country_id != 0) {
                    System.LoadDropDown("county_dropdown", "candidate_country", null, data.fk_experience_level);
                    Candidate.LoadState(data.fk_country_id, data.fk_state_id)
                }
                if (data.fk_city_id != null && data.fk_city_id != 0) {
                    Candidate.LoadCity(data.fk_state_id, data.fk_city_id);
                }

                if (data.profile_pic != null) {
                    $("#profile_image_preview").attr('src', "../../Assets/Images/" + data.profile_pic);
                    $('#hidden_candidate_pic').val(data.profile_pic);
                }
                else {
                    $("#hidden_candidate_pic").val('');
                    $("#profile_image_preview").attr('src', "http://simpleicon.com/wp-content/uploads/account.png");
                }
                $('#candidate_availability').val(data.availability);
                if (data.job_type != 0) {
                    $('#candidate_job_type').val(data.job_type);
                }
                else {
                    $('#candidate_job_type').val('');
                }
            },
            complete: function () {
                Candidate.OpenCandidateModal(id);
            },
            error: function (Result) {
                //$("#" + bind_control).append($("<option></option>").val(0).html("Loading Problem.."));
            }
        });
    },

    LoadExpertise: function (selected_id) {
        System.LoadDropDown("expertise_dropdown", "candidate_expertise", null, selected_id)
    },
    ResetForm: function () {
        $('#candidate_pk_id').val(0),
            $('#candidate_first_name').val('');
        $('#candidate_last_name').val('');
        $('#candidate_email').val('');
        $('#candidate_contact').val('');
        $('#candidate_active').val('');
        $('#candidate_tenure').val('');
        $('#candidate_experience').val('');
        $('#candidate_about_us').val('');
        $('#candidate_availability').val('');
        $("#candidate_active").prop('checked', false);
        $('#candidate_job_type').val('');

        //$("#candidate_expertise").multiselect("refresh");
        //$("#candidate_expertise option:selected").prop("selected", false);
        //$('#candidate_expertise').multiselect('deselectAll', false);
        //$("#candidate_expertise").multiselect('refresh');
    },
    ShowPostRequirementDetail: function (post_id) {
        $.ajax({
            url: "../Recruiter/GetSingleApprovedRequirement?post_id=" + post_id,
            cache: false,
            type: "GET",
            dataType: "html",
            contentType: 'application/html; charset=utf-8',
            success: function (result) {
                $('#show_post_details').html(result);
            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {
                $('#show_post_details').slideDown();
            }
        });

    },
    CloseRequirementDetail: function () {
        $('#show_post_details').slideUp();
    },
    ShowInterestedUserDetail: function (id) {
        $.ajax({
            url: "../Recruiter/GetInterestedUserDetail?user_id=" + id,
            cache: false,
            type: "GET",
            dataType: "html",
            contentType: 'application/html; charset=utf-8',
            success: function (result) {
                $('#interested_user_section').html(result);
            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {
                $('#interested_user_section').slideDown();
            }
        });

    },
    CloseInterestedUserDetail: function () {
        $('#interested_user_section').slideUp();
    },
    LoadCandidateList: function () {
        $(".divLoading").addClass('show');
        var data = Candidate.CandidateSearchFilter();


        //$('#candidate_list').DataTable({
        //    processing: true,
        //    serverSide: true,
        //    searching: true,
        //    ordering: false,
        //    paging: false,
        //    "ajax": {
        //        "url": "/Recruiter/GetTest",
        //        "type": "Get",
        //        "datatype": "json"
        //    },
        //    "columnDefs":
        //        [{
        //            "targets": [0],
        //            "visible": false,
        //            "searchable": false
        //        }],  
        //    columns: [
               
        //    ]
        //});
          
        $.ajax({
            url: "../Recruiter/GetRecruitersList",
            cache: false,
            type: "GET",
            dataType: "html",
            data: { candidate_name: data.candidate_name, candidate_technology: data.candidate_technology, candidate_experience: data.candidate_experience },
            contentType: 'application/html; charset=utf-8',
            success: function (result) {
                $('#append_candidate_list').html(result);
                //$("#candidate_list").DataTable();
                //$("#candidate_list").DataTable();
            },
            error: function (err) {
                alert(err);
                //ShowModel("Error", err);
            },
            complete: function () {
                $('#interested_user_section').slideDown();
                $("#candidate_list").DataTable();
                $(".divLoading").removeClass('show');

            }
        });
    },
    CandidateSearchFilter: function () {
        var data = {
            candidate_name: $('#candidate_name_search').val(),
            candidate_technology: $('#candidate_technology_search').val(),
            candidate_experience: $('#candidate_experience_search').val(),
        }

        return data;
    }
}