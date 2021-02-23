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
            candidate_id:global_candidate_id
        }

        return data;
    },
    OpenInterestedModal: function (id) {
        debugger;
        global_candidate_id = id;
        $('#candidate_interested_modal').modal('show');
    },
    SearchCandidates: function () {
        var data = Candidate.GetCandidateSearchParams();
        $.ajax({
            url: "/Candidate/CandidateProfileList",
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
    }
}