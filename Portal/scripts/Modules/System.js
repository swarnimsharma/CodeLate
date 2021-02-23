var expertise_dataSource = [];
var param_experience = "";
$(function () {
    System.LoadDropDown('experience_dropdown', 'search_experience', null, null);
    var param_locality = System.GetParameterValue('locality');
    param_experience = System.GetParameterValue('experience');
    if (param_locality != null && param_locality != '') {
        $('#search_location').val(param_locality);
    }
    
});

var System = {
    LoadDropDown: function (type, bind_control, filter_id, selected_id, ids, selected_ids) {
        $("#" + bind_control).empty();
        $("#" + bind_control).append($("<option></option>").val(0).html("Loading.."));
        var bind_data = { type: type, id: filter_id, ids: ids };
        $.ajax({
            type: "GET",
            url: "/DropDown/GetDropdown",
            data: bind_data,
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#" + bind_control).empty();
                if (type != "expertise_dropdown") {
                    if (bind_control == 'search_experience') {
                        $("#" + bind_control).append($("<option></option>").val(0).html("Select Experience"));
                        selected_id = param_experience;
                    }
                    else {
                        $("#" + bind_control).append($("<option></option>").val(0).html("Select"));
                    }
                }

                $.each(data, function (i, item) {
                    $("#" + bind_control).append($("<option></option>").val(item.value).html(item.name));
                });
                if (selected_id == null || selected_id == undefined || selected_id == "undefined") {
                    //
                }
                else {
                    $("#" + bind_control).val(selected_id);
                }
                if (type == "multi_country_dropdown") {
                    //
                    $("#" + bind_control).select2({
                        placeholder: "Select Countries"
                    });
                }
                if (type == "multi_state_dropdown") {
                    //
                    $("#" + bind_control).select2({
                        placeholder: "Select States"
                    });
                }
                if (type == "multi_city_dropdown") {
                    //
                    $("#" + bind_control).select2({
                        placeholder: "Select Cities"
                    });
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
                if (type == "vendor_dropdown") {
                    $('#post_requirement_vendors').multiselect();
                }
                if (type == "multi_country_dropdown" && selected_ids != null && selected_ids != undefined) {
                    //
                    var countries_array = selected_ids.split(",");
                    //$("#" + bind_control).select2(['1', '2']);
                    $("#" + bind_control).select2(countries_array);
                    $("#" + bind_control).trigger('change');
                }
                if (type == "multi_state_dropdown" &&  selected_ids != null && selected_ids != undefined) {
                    //
                    var states_array = selected_ids.split(",");
                    $("#" + bind_control).select2(states_array);
                    $("#" + bind_control).trigger('change');
                }
                if (type == "multi_city_dropdown" && selected_ids != null && selected_ids != undefined) {
                    //
                    var cities_array = selected_ids.split(",");
                    $("#" + bind_control).select2(cities_array);
                    $("#" + bind_control).trigger('change');
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
    },
    LoadSkillsOnKeySearch: function () {
        $("#search_skills").autocomplete({
            minLength: 2,
            delay: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Home/SearchFilters",
                    type: "GET",
                    dataType: "json",
                    data: { search: request.term },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                value: item.value,
                                name: item.name
                            };
                        }))
                    }
                })
            },
            focus: function (event, ui) {
                $("#search_skills").val(ui.item.name);
                return false;
            },
            select: function (event, ui) {
                $("#search_skills").val(ui.item.name);
                $("#hidden_search_skills").val(ui.item.value);
                //GetShopSubProcessList(ui.item.value);
                return false;
            },
            change: function (event, ui) {
                if (ui.item == null) {
                    $("#search_skills").val("");
                    $("#hidden_search_skills").val("0");

                    //ShowModel("Alert", "Please select Shop No In List.")

                }
                return false;
            }

        })
       .autocomplete("instance")._renderItem = function (ul, item) {
           return $("<li>")
             .append("<div>" + item.name + "</div>")
             .appendTo(ul);
       };
    },
    LoadSkillsTagit: function () {

        $.getJSON("/Home/SearchFilters?type=expertise", {},
                function (data) {
                    $.map(data, function (item) {
                        expertise_dataSource.push(item);

                    });

                    console.log(data);
                    $('#search_skills').magicsearch({
                        dataSource: expertise_dataSource,
                        fields: ['name'],
                        id: 'value',
                        format: '%name%',
                        multiple: true,
                        multiField: 'name',
                        multiStyle: {
                            space: 5,
                            width: 80
                        }
                    });
                    var param_title = System.GetParameterValue('title');
                    if (param_title != null && param_title!="") {

                        var append_expertise_tag_ids = "";
                        var split_search_titles = param_title.split(',');
                        $.each(split_search_titles, function (index, name) {
                            append_expertise_tag_ids+= expertise_dataSource.find(x => x.name === name).value + ',';
                        });
                        if (append_expertise_tag_ids.length > 0) {
                            append_expertise_tag_ids = append_expertise_tag_ids.substring(0, append_expertise_tag_ids.length - 1);
                        }
                        $('#search_skills').trigger('set', { id: append_expertise_tag_ids });
                    }

                }).fail(function (jqXHR, textStatus, errorThrown) { alert("fail " + errorThrown); });

        //$('#search_skills').trigger('set', { value: 'Tim,Eric' });

        //$("#search_skills").tagit({
        //    allowSpaces: true,
        //    minLength: 10,
        //    //removeConfirmation: true,
        //    placeholderText: "Skills",
        //    tagSource: function (request, response) {
        //        $.ajax({
        //            url: "/Home/SearchFilters",
        //            type: "GET",
        //            dataType: "json",
        //            data: { type: "expertise", search: request.term },
        //            success: function (data) {
        //                response($.map(data, function (item) {
        //                    return {
        //                        value: item.name,
        //                        name: item.name
        //                    };
        //                }))
        //            }
        //        })
        //    },
        //    autocomplete: {
        //        delay: 0,
        //        minLength: 10
        //    }
        //});
    },
    LoadLocalityOnKeySearchNew: function () {
        $("#search_location").autocomplete({
            minLength: 1,
            delay: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Home/SearchFilters",
                    type: "GET",
                    dataType: "json",
                    data: { type: "locality", search: request.term },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                value: item.value,
                                name: item.name
                            };
                        }));
                    }
                })
            },
            focus: function (event, ui) {
                $("#search_location").val(ui.item.name);
                return false;
            },
            select: function (event, ui) {
                $("#search_location").val(ui.item.name);
                //$("#hidden_search_skills").val(ui.item.value);
                //GetShopSubProcessList(ui.item.value);
                return false;
            },
            change: function (event, ui) {
                if (ui.item == null) {
                    $("#search_location").val("");
                    //ShowModel("Alert", "Please select Shop No In List.")
                }
                return false;
            }
        })
       .autocomplete("instance")._renderItem = function (ul, item) {
           return $("<li>")
             .append("<div>" + item.name + "</div>")
             .appendTo(ul);
       };
    },
    LoadLocalityOnKeySearch: function () {
        $("#search_location").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/Home/SearchFilters",
                    type: "GET",
                    dataType: "json",
                    data: { type: "locality", search: request.term }
                }).done(function (data) {
                    if (data != null) {
                        response($.map(data, function (item) {
                            return item.name;
                        }));
                    } else {

                    }
                });
            },
            delay: 500,
            minLength: 1,
            response: function (event, ui) {
                if (!ui.content.length) {
                    var noResult = { value: "", label: "No results found" };
                    ui.content.push(noResult);
                    //$("#message").text("No results found");
                } else {
                    $("#message").empty();
                }
            }
        });
    },
    GetParameterValue: function (param) {
        var url_string = window.location.href; // www.test.com?filename=test
        var url = new URL(url_string);
        var paramValue = url.searchParams.get(param);
        return paramValue;
    }
}