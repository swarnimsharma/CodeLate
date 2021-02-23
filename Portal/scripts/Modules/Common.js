var Common = {
    LoadDropDown: function (type, bind_control, filter_id, selected_id) {
        $("#" + bind_control).empty();
        $("#" + bind_control).append($("<option></option>").val(0).html("Loading.."));
        var bind_data = { type: type, id: filter_id }
        $.ajax({
            type: "GET",
            url: "/DropDown/GetDropdown",
            data: bind_data,
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#" + bind_control).empty();
                if (bind_control == 'search_experience')
                {
                    $("#" + bind_control).append($("<option></option>").val(0).html("Select Experience"));
                }
                else
                {
                    $("#" + bind_control).append($("<option></option>").val(0).html("Select"));
                }
                $.each(data, function (i, item) {
                    $("#" + bind_control).append($("<option></option>").val(item.value).html(item.name));
                });
                if (selected_id == null || selected_id == undefined || selected_id == "undefined") {

                }
                else {
                    $("#" + bind_control).val(selected_id);
                }
            },
            complete: function () {
                if (type == "expertise_dropdown") {
                    $('#' + bind_control).multiselect();
                }
                if (type == "expertise_dropdown" && selected_id != null) {
                    var expert_array = selected_id.split(',');
                    $("#" + bind_control).val(expert_array);
                    $("#" + bind_control).multiselect("refresh");
                    //$('#candidate_expertise').multiselect();
                }
            },
            error: function (Result) {
                $("#" + bind_control).append($("<option></option>").val(0).html("Searching"));
            }
        });
    },
    LoadNotification: function (bind_id, bg_type, message, notification_id) {
        $("#" + notification_id).addClass(bg_type);
        $("#" + bind_id).text(message);
        setTimeout(function () {
            $('#' + notification_id).fadeOut('slow');
            $('#' + bind_id).text('');
        }, 3000);
    },
    LoadCandidateImage: function () {
        $("#candidate_profile_image").change(function () {
            var data = new FormData();
            var files = $("#candidate_profile_image").get(0).files;
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
                    $("#profile_image_preview").attr('src', "../../Assets/Images/" + response);
                    $('#hidden_candidate_pic').val(response);
                },
                error: function (er) {
                    alert(er);
                }

            });
        });
    }
}