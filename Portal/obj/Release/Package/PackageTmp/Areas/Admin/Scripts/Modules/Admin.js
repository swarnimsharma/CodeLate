var what_client_say_id = 0;
var Admin = {
    OpenPostApprovedModal: function (id) {
        Admin.ResetRequirementParams();
        $('#post_requirement_no').text(id);
        $("#approv_reject_panel").slideDown("slow");
        Admin.ShowOnStatusChange(0);
        $('#hidden_post_requirement_id').val(id);

    },
    SubmitPostRequirement: function () {
        $.ajax({
            url: "../Admin/SubmitRequirementStatus",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(Admin.GetRequirementParams()),
            contentType: 'application/json',
            success: function (data) {
                if (data.status == "SUCCESS") {
                    $("#approv_reject_panel").slideUp("slow");
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
                Admin.ResetRequirementParams();
            }
        });
    },
    GetRequirementParams: function () {
        var vendor_selection = [];
        var comma_seperate_vendors = "";
        $.each($("#post_requirement_vendors option:selected"), function () {
            vendor_selection.push($(this).val());
        });
        if (vendor_selection.length > 0) {
            comma_seperate_vendors = vendor_selection.join(",")
        }
        var data = {
            post_id: $('#hidden_post_requirement_id').val(),
            vendor_ids: vendor_selection,
            status: $('#post_requirement_status').val(),
            reason_status: $('#post_requirement_reason').val()
        };
        return data;
    },
    ResetRequirementParams: function () {
        $('#hidden_post_requirement_id').val('');
        $('#post_requirement_status').val('');
        $('#post_requirement_reason').val('');
    },
    CancelPostRequirement: function () {
        $("#approv_reject_panel").slideUp("slow");
    },
    ShowOnStatusChange: function (val) {
        if (val == 1) {
            $('#hidden_requirement_vendors').slideDown();
        }
        else {
            $('#hidden_requirement_vendors').slideUp();
        }
    },
    OnAddUser: function () {
        $('#user_add_reset_profile_btn').attr('onclick', 'Admin.CloseModal()')
        $('#user_header_text').text('User Details!');
        $('#user_type_formgroup').show();
        $('#user_is_active_section').show();

        $('#user_pic_section').hide();
        $('#user_username_formgroup').show();
        $('#user_password_formgroup').show();
        $('#user_main_section').removeClass('col-md-9');
        $('#user_main_section').addClass('col-md-12');
        $('#user_password_section').hide();
        $('.form-group').removeClass('col-md-6');
        $('.form-group').addClass('col-md-3');
        $('#user_text_area_section').removeClass('col-md-3');
        $('#user_text_area_section').addClass('col-md-12');
        $('#user_main_info_section').removeClass('col-md-8');
        $('#user_main_info_section').addClass('col-md-12');
    },
    LoadUserList: function () {
        $.ajax({
            url: "../Admin/GetAllUserList",
            cache: false,
            type: "GET",
            dataType: "html",
            contentType: 'application/html; charset=utf-8',
            success: function (result) {
                $('#user_list_div').html(result);
            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {
                Admin.ResetRequirementParams();
                $('#table_get_all_userlist').DataTable();
            }
        });

    },
    OpenUserModal: function (val) {
        User.ResetUserFields();
        $('#user_is_paid_section').hide();
        $("#user_profile_add_section").slideDown();
        System.LoadDropDown("county_dropdown", "user_fk_country_id");
        User.ResetUserFields();
        if (val == 0) {
            $('#user_btn_text').text('Add User');
            $('#user_add_update_profile_btn').attr('onclick', 'User.AddUserProfile()');
        }
        else {
            Admin.OnEditModal(val);
            $('#user_add_update_profile_btn').attr('onclick', 'User.UpdateProfile()');
            $('#user_btn_text').text('Update User');
        }
    },
    CloseModal: function () {
        $("#user_profile_add_section").slideUp();
        User.ResetUserFields();
    },
    OnEditModal: function (id) {
        User.GetProfile(id);
    },
    OpenImportModal: function () {
        $("#import_file_modal").modal('show');
    },
    UploadVendorCandidateFile: function () {
        $(".divLoading").addClass('show');
        $.ajax({
            url: "/ImportFile/UploadFile",
            cache: false,
            type: "Post",
            dataType: "json",
            data: { name: $('#hidden_import_file_name').val(), type: $('#upload_file_type').val() },
            success: function (result) {
                if (result == "Success") {
                    toastr8.success("File has been uploaded successfully");
                }
                else {
                    toastr8.warning("Something went wrong with your file!");
                }

                //$('#user_list_div').html(result);
                
            },
            error: function (err) {
                //ShowModel("Error", err)
                toastr8.warning("Something went wrong with your file!");
                $(".divLoading").removeClass('show');
            },
            complete: function () {
                $('#import_upload_file').val('');
                $('#import_upload_file').html('');

                $(".divLoading").removeClass('show');
            }
        });
    },
    WhatsClientSays: function () {
        $.ajax({
            url: "../Admin/WhatClientSays",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(Admin.GetParamWhatClientSays()),
            contentType: 'application/json',
            success: function (data) {
                if (data.status == "SUCCESS") {
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
    GetParamWhatClientSays: function () {
        var data = {
            id: what_client_say_id,
            title: $('#what_client_title').val(),
            description: $('#what_client_description').val(),
            client_name: $('#what_client_name').val(),
            is_published: $('#what_client_is_published').is(':checked'),
            client_image: $('#hidden_client_pic').val()
        };

        return data;
    },
    OpenWhatsSayModal: function () {
        this.ResetWhatClientSays();
        $("#what_clients_say_modal").modal('show');
    },
    GetWhatsClientSaysByID: function (id) {
        this.ResetWhatClientSays();
        what_client_say_id = id;
        $.ajax({
            url: "../Admin/WhatClientSaysDetail?id=" + id,
            cache: false,
            type: "Get",
            dataType: "json",
            contentType: 'application/json',
            success: function (data) {
                if (data != null) {
                    $('#what_client_title').val(data.title);
                    $('#what_client_description').val(data.description);
                    $('#what_client_name').val(data.client_name);
                    if (data.is_published) {
                        $('#what_client_is_published').prop('checked', true);
                    }
                    else {
                        $('#what_client_is_published').prop('checked', false);
                    }
                    if (data.client_image != null && data.client_image != "") {
                        $('#hidden_client_pic').val(data.client_image)
                        $("#client_image_preview").attr('src', "../../Assets/Images/" + data.client_image);
                    }
                    else {
                        $('#hidden_client_pic').val()
                        $("#client_image_preview").attr('src', "http://simpleicon.com/wp-content/uploads/account.png");
                    }
                }
                else {
                    //ShowModel("Error", data.message)
                }
            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {
                $("#what_clients_say_modal").modal('show');
            }
        });
    },
    ResetWhatClientSays: function () {
        $('#what_client_title').val(''),
        $('#what_client_description').val(''),
        $('#what_client_name').val(''),
         $('#what_client_is_published').is(':checked'),
         $('#hidden_client_pic').val('')
        $('#what_client_is_published').prop('checked', false);
        $("#client_image_preview").attr('src', "http://simpleicon.com/wp-content/uploads/account.png");
    }
};