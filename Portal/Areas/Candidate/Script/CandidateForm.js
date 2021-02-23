
var CandidateForm = {

    LoadState: function (id, selected_id) {
        System.LoadDropDown("state_dropdown", "candidate_fk_state_id", id, selected_id);
    },

    MultiLoadState: function (id, selected_id) {
        System.LoadDropDown("state_dropdown", "candidate_state", id, selected_id);
    },

    LoadCity: function (id, selected_id) {
        System.LoadDropDown("city_dropdown", "candidate_fk_city_id", id, selected_id);
    },

    MultiLoadCity: function (id, selected_id) {
        System.LoadDropDown("city_dropdown", "candidate_city", id, selected_id);
    },

   
    LoadExpertise: function (selected_id) {
        System.LoadDropDown("expertise_dropdown", "candidate_fk_technology", null, selected_id)
    },

    GetPreferredSelectedCountries: function (callback) {
    
        var selected_countries = $('#preferred_country_id').select2("val");
        if (selected_countries != null && selected_countries != undefined && selected_countries != "") {
            selected_countries = selected_countries.toString();
        }
        else {
            selected_countries = "";
        }
        System.LoadDropDown("multi_state_dropdown", "preferred_state_id", null, null, selected_countries);
        if (callback != null && callback != undefined) {
            callback();
        }
    },
    GetPreferredSelectedStates: function (callback) {
       
        var selected_states = $('#preferred_state_id').select2("val");
        if (selected_states != null && selected_states != undefined && selected_states != "") {
            selected_states = selected_states.toString();
        }
        else {
            selected_states = "";
        }
        System.LoadDropDown("multi_city_dropdown", "preferred_city_id", null, null, selected_states);
        if (callback != null && callback != undefined) {
            callback();
        }
    },

    GetProfile: function (id) {
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
                    CandidateForm.LoadState(data.fk_country_id, data.fk_state_id);
                }
                if (data.fk_state_id != null && data.fk_state_id != 0) {
                    CandidateForm.LoadCity(data.fk_state_id, data.fk_city_id);
                }
                User.OnSelectUser(data.fk_user_type);
            },
            complete: function () {
            },
            error: function (Result) {
            }
        });
    },

    UpdateCandidateProfile: function () {
        var data = CandidateForm.GetCandidateParams();
        if (data != null) {
        $.ajax({
            url: "../Candidate/AddUpdateCandidateProfile",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(CandidateForm.GetCandidateParams()),
            contentType: 'application/json',
            success: function (data) {
                if (data.status == "SUCCESS") {
                    alert('Saved Successfully');
                    CandidateForm.GetCandidateProfile();
                }
                else {
                }
            },
            error: function (err) {
            },
            });
        }
    },

    GetCandidateParams: function () {
        var expertise_selection = [];
        var comma_seperate_experts = "";
        $.each($("#candidate_fk_technology option:selected"), function () {
            expertise_selection.push($(this).val());
        });
        if (expertise_selection.length > 0) {
            comma_seperate_experts = expertise_selection.join(",")
        }

        var selected_countries = $('#preferred_country_id').select2("val");
        var selected_states = $('#preferred_state_id').select2("val");
        var selected_cities = $('#preferred_city_id').select2("val");
        var selected_countries_name = $('#preferred_country_id').select2('data').text;
        var selected_states_name = $('#preferred_state_id').select2('data').text;
        var selected_cities_name = $('#preferred_city_id').select2('data').text;

        var profile_pic = $('#hidden_candidate_pic').val();
        var candidate_fk_country_id = $('#candidate_fk_country_id').val();
        var candidate_fk_state_id = $('#candidate_fk_state_id').val();
        var candidate_fk_city_id = $('#candidate_fk_city_id').val();
        var profile_headline = $('#profile_headline').val();
        var fk_user_id = $('#candidate_pk_id').val();
        var resume = $('#hidden_candidate_cv').val();
        var availability = $('#candidate_availability').val();

        var first_name = $('#candidate_first_name').val();
        var last_name = $('#candidate_last_name').val();
        var email = $('#candidate_email').val();
        var contact = $('#candidate_contact').val();
        // Get Countries
        if (selected_countries != null && selected_countries != undefined && selected_countries != "") {
            selected_countries = selected_countries.toString();
        }
        else {
            selected_countries = "";
        }

        //Get States
        if (selected_states != null && selected_states != undefined && selected_states != "") {
            selected_states = selected_states.toString();
        }
        else {
            selected_states = "";
        }

        //Get States
        if (selected_cities != null && selected_cities != undefined && selected_cities != "") {
            selected_cities = selected_cities.toString();
        }
        else {
            selected_cities = "";
        }

        ////Get Technologies
        //if (selected_technologies != null && selected_technologies != undefined && selected_technologies != "") {
        //    selected_technologies = selected_technologies.toString();
        //}
        //else {
        //    selected_technologies = "";
        //}

        var required_fields = [];
        var param = {
            head_line: profile_headline,
            candidate_fk_country_id: candidate_fk_country_id,
            candidate_fk_state_id: candidate_fk_state_id,
            candidate_fk_city_id: candidate_fk_city_id,
            country_listing: selected_countries,
            state_listing: selected_states,
            city_listing: selected_cities,
            fk_user_id: fk_user_id,
            profile_pic: profile_pic,
            resume: resume,
            availability: availability,
            selected_technologies_name: comma_seperate_experts,
            first_name: first_name,
            last_name: last_name,
            email: email,
            contact: contact
        }

        if (param.head_line == null || param.head_line == '' || param.head_line == '0') {
            required_fields.push('profile_headline');
        }

        if (param.email == null || param.email == '' || param.email == '0') {
            required_fields.push('candidate_email');
        }

        if (param.first_name == null || param.first_name == '' || param.first_name == '0') {
            required_fields.push('candidate_first_name');
        }

        if (param.last_name == null || param.last_name == '' || param.last_name == '0') {
            required_fields.push('candidate_last_name');
        }

        if (param.contact == null || param.contact == '' || param.contact == '0') {
            required_fields.push('candidate_contact');
        }

        if (param.selected_technologies_name == null || param.selected_technologies_name == '' || param.selected_technologies_name == '0') {
            required_fields.push('candidate_fk_technology');
        }

        if (param.availability == null || param.availability == '' || param.availability == '0') {
            required_fields.push('candidate_availability');
        }

        
        if (param.candidate_fk_city_id == null || param.candidate_fk_city_id == '' || param.candidate_fk_city_id =='0') {
            required_fields.push('candidate_fk_city_id');
        }

        if (param.candidate_fk_country_id == null || param.candidate_fk_country_id == '' || param.candidate_fk_country_id == '0') {
            required_fields.push('candidate_fk_country_id');
        }

        if (param.candidate_fk_state_id == null || param.candidate_fk_state_id == '' || param.candidate_fk_state_id == '0') {
            required_fields.push('candidate_fk_state_id');
        }

        if (param.country_listing == null || param.country_listing == '' || param.country_listing == '0') {
            required_fields.push('preferred_country_id');
        }

        if (param.state_listing == null || param.state_listing == '' || param.state_listing == '0') {
            required_fields.push('preferred_state_id');
        }

        if (param.city_listing == null || param.city_listing == '' || param.city_listing == '0') {
            required_fields.push('preferred_city_id');
        }

        if (required_fields.length >0) {
            User.RequiredValidation(required_fields);
            $('#candidate_required_fields_message').show();
        }
        else
        {
            $('#candidate_required_fields_message').hide();
            return param;
        }
        return null;
    },

    GetCandidateProfile: function (id) {
        $.ajax({
            type: "GET",
            url: "../Candidate/GetCandidateProfile?id=" + id,
            data: {},
            dataType: "json",
            asnc: false,
            success: function (data) {
                debugger;
                $('#profile_headline').val(data.profile_headline);
                $('#candidate_pk_id').val(data.fk_user_id),
                $('#candidate_first_name').val(data.first_name);
                $('#candidate_last_name').val(data.last_name);
                $('#candidate_email').val(data.email);
                $('#candidate_contact').val(data.contact_no);
                $('#candidate_availability').val(data.availability);
                $('#hidden_candidate_cv').val(data.resume);
                $('#candidate_contact').val(data.contact);
                debugger;
                if (data.country_listing != null && data.country_listing != "") {
                    var country_array = data.country_listing.split(',');
                    $('#preferred_country_id').val(country_array).trigger('change');
                }
                else {
                    $('#preferred_country_id').val(null).trigger('change');
                }
                debugger;
                if (data.state_listing != null && data.state_listing != "") {
                    CandidateForm.GetPreferredSelectedCountries(function () {
                        setTimeout(function () {
                            var state_array = data.state_listing.split(',');
                            $('#preferred_state_id').val(state_array).trigger('change');
                            if (data.city_listing != null && data.city_listing != "") {
                                CandidateForm.GetPreferredSelectedStates(function () {
                                    setTimeout(function () {
                                        var city_array = data.city_listing.split(',');
                                        $('#preferred_city_id').val(city_array).trigger('change');
                                    }, 1000);
                                });
                            }
                            else {
                                CandidateForm.GetPreferredSelectedStates();
                                $('#preferred_city_id').val(null).trigger('change');
                            }
                        }, 1000);
                    });
                }
                else {
                    CandidateForm.GetPreferredSelectedCountries();
                    $('#preferred_state_id').val(null).trigger('change');
                }

                if (data.candidate_fk_country_id != null && data.candidate_fk_country_id != 0) {
                    System.LoadDropDown("county_dropdown", "candidate_fk_country_id", null, data.candidate_fk_country_id);
                    CandidateForm.LoadState(data.candidate_fk_country_id, data.candidate_fk_state_id)
                }
                if (data.candidate_fk_city_id != null && data.candidate_fk_city_id != 0) {
                    CandidateForm.LoadCity(data.candidate_fk_state_id, data.candidate_fk_city_id);
                }
                if (data.selected_technologies_name != null) {
                    CandidateForm.LoadExpertise(data.selected_technologies_name)
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
            error: function (Result) {
            }
        });
    },

    SelectCandidatePlan: function (id) {
        var PlanID = $('#hdPlanID').val();
        var helpText = "Do you wish to continue with this plan?";
        if (PlanID != '') {
            helpText = 'You already have a plan,Do you wish to continue with this plan?'
        }

        var data = {
            pk_plan_id: id
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
                        url: "../Candidate/AddUpdatePlanSelectedByCandidate",
                        cache: false,
                        type: "POST",
                        dataType: "json",
                        data: JSON.stringify(data),
                        contentType: 'application/json',
                        success: function (data) {
                            if (data.status == "SUCCESS") {
                                $('#hdPlanID').val('');
                                $.ajax({
                                    method: 'GET',
                                    url: '../Candidate/_UserPlan',
                                    success: function (data) {
                                        $('#divUserPlan').html(data);
                                    }
                                });
                            }
                            else {

                            }
                        },
                        error: function (err) {
                        }
                    });
                }
            }
        });

    }
}
