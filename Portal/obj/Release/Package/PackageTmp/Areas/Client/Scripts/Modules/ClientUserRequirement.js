var global_post_id=0
var ClientUserRequirment = {
    ChangeRequirementStatus: function () {
        $.ajax({
            url: "../Client/ChangeRequirementStatus",
            cache: false,
            type: "Get",
            dataType: "json",
            data: { post_id: global_post_id, status: $('#posted_requirement_status').val(), message: $('#client_reason').val() },
            contentType: 'application/json',
            success: function (data) {
                debugger;
                if (data.status == "SUCCESS") {
                    $("#change_status_div").slideUp("slow");
                    location.reload(true);
                }
                else {
                    //ShowModel("Error", data.message)
                }
            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {

            }
        });
    },
    ShowChangeStatusSection: function (id) {
        $("#change_status_div").slideDown("slow");
        global_post_id = id;
        $('#post_requirement_no').text(id);
    },
    CloseRequirementStatus: function () {
        $("#change_status_div").slideUp("slow");
    }
}