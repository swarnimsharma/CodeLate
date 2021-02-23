var temp_countries_array_names = [];
var temp_states_array_names = [];
var temp_cities_array_names = [];

var AdminPlans = {
    OpenPlanModal: function () {
        AdminPlans.ResetPlansForm();
        $('#plan_modal').modal('show');
    },
    OpenVendorPlanModal: function () {
        System.LoadDropDown("plan_dropdown", "vendor_plan_type");
        AdminPlans.ResetVendorPlanForm();
        $('#vendor_plan_modal').modal('show');
    },
    AddUpdatePlanType: function () {
        $.ajax({
            url: "../Plans/AddPlanType",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(AdminPlans.GetPlanTypeParams()),
            contentType: 'application/json',
            success: function (data) {
                if (data.status == "SUCCESS") {
                    $("#plan_modal").modal("hide");
                    //location.reload(true);
                }
                else {
                    //ShowModel("Error", data.message)
                }
            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {
                AdminPlans.LoadPlanList();
            }
        });
    },
    GetPlanTypeParams: function () {

        var param = {
            pk_plan_id: $('#hidden_pk_plan_id').val(),
            plan_name: $('#plan_type_name').val(),
            is_active: $('#plan_type_active').is(":checked")
        };
        return param;
    },
    OpenPlanGetById: function (id) {
        AdminPlans.ResetPlansForm();
        $.ajax({
            url: "../Plans/GetPlansById?id=" + id,
            cache: false,
            type: "Get",
            dataType: "json",
            contentType: 'application/json',
            success: function (data) {
                if (data != null) {
                    $('#plan_type_name').val(data.plan_name);
                    if (data.is_active) {
                        $('#plan_type_active').prop('checked', true);
                    }
                    else {
                        $('#plan_type_active').prop('checked', false);
                    }
                    $('#hidden_pk_plan_id').val(data.pk_plan_id);
                }
                else {
                    //ShowModel("Error", data.message)
                }
            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {
                $("#plan_modal").modal('show');
            }
        });
    },
    LoadPlanList: function () {
        $.ajax({
            url: "../Plans/_PlansList",
            cache: false,
            type: "GET",
            dataType: "html",
            contentType: 'application/html; charset=utf-8',
            success: function (result) {
                $('#plan_list_div').html(result);
            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {
                $('#table_get_all_planlist').DataTable();
            }
        });
    },
    DeletePlanType: function () {

    },
    ResetPlansForm: function () {
        $('#hidden_pk_plan_id').val('');
        $('#plan_type_name').val('');
        $('#plan_type_active').prop('checked', true);
    },
    AddUpdateVendorPlans: function () {
        $.ajax({
            url: "../Plans/AddVendorPlan",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(AdminPlans.GetVendorPlansParams()),
            contentType: 'application/json',
            success: function (data) {
                if (data.status == "SUCCESS") {
                    $("#vendor_plan_modal").modal("hide");
                    //$('.alert').addClass('alert-success');
                    //location.reload(true);
                }
                else {
                    //ShowModel("Error", data.message)
                }
            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {
                AdminPlans.ResetVendorPlanForm();
                AdminPlans.LoadVendorPlanList();
            }
        });
    },
    DeleteVendorPlan: function (id) {
        $.ajax({
            type: "POST",
            url: '@Url.Action("Delete")',
            data: JSON.stringify({ id: id }), //use id here
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function () {
                // alert("Data has been deleted.");
                LoadData();
            },
            error: function () {
                alert("Error while deleting data");
            }
        });
    },
    GetVendorPlansParams: function () {
        var selected_countries = $('#vendor_plan_country_limit').select2("val");
        var selected_states = $('#vendor_plan_state_limit').select2("val");
        var selected_cities = $('#vendor_plan_city_limit').select2("val");

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

        var param = {
            pk_vendor_plan_id: $('#hidden_pk_vendor_plan_id').val(),
            fk_plan_id: $('#vendor_plan_type').val(),
            is_active: $('#vendor_plan_type_active').is(":checked"),
            candidate_listing_limit: $('#vendor_plan_candidate_listing').val(),
            interest_received_limit: $('#vendor_plan_interest_received_limit').val(),
            lead_access_limit: $('#vendor_plan_lead_access_limit').val(),
            technology_count: $('#vendor_plan_technology_limit').val(),
            country_listing: selected_countries,
            state_listing: selected_states,
            city_listing: selected_cities
        };
        return param;
    },
    OpenVendorPlanGetById: function (id) {
        AdminPlans.ResetVendorPlanForm();
        $.ajax({
            url: "../Plans/GetVendorPlansById?id=" + id,
            cache: false,
            type: "Get",
            dataType: "json",
            contentType: 'application/json',
            success: function (data) {
                if (data != null) {
                    System.LoadDropDown("plan_dropdown", "vendor_plan_type", null, data.fk_plan_id);
                    $('#hidden_pk_vendor_plan_id').val(data.pk_vendor_plan_id);
                    $('#vendor_plan_candidate_listing').val(data.candidate_listing_limit);
                    $('#vendor_plan_interest_received_limit').val(data.interest_received_limit);
                    $('#vendor_plan_lead_access_limit').val(data.lead_access_limit);
                    $('#vendor_plan_technology_limit').val(data.technology_limit);
                    if (data.is_active) {
                        $('#vendor_plan_type_active').prop('checked', true);
                    }
                    else {
                        $('#vendor_plan_type_active').prop('checked', false);
                    }

                    if (data.country_listing != null && data.country_listing != "") {
                        var country_array = data.country_listing.split(',');
                        $('#vendor_plan_country_limit').val(country_array).trigger('change');
                    }
                    else {
                        $('#vendor_plan_country_limit').val(null).trigger('change');
                    }

                    if (data.state_listing != null && data.state_listing != "") {
                        AdminPlans.GetSelectedCountries(function () {
                            setTimeout(function () {
                                var state_array = data.state_listing.split(',');
                                $('#vendor_plan_state_limit').val(state_array).trigger('change');
                                if (data.city_listing != null && data.city_listing != "") {
                                    AdminPlans.GetSelectedStates(function () {
                                        setTimeout(function () {
                                            debugger;
                                            var city_array = data.city_listing.split(',');
                                            $('#vendor_plan_city_limit').val(city_array).trigger('change');
                                        }, 1000);
                                    });
                                }
                                else {
                                    AdminPlans.GetSelectedStates();
                                    $('#vendor_plan_city_limit').val(null).trigger('change');
                                }
                            }, 1000);
                        });
                    }
                    else {
                        AdminPlans.GetSelectedCountries();
                        $('#vendor_plan_state_limit').val(null).trigger('change');
                    }

                    

                    //$('#vendor_plan_state_limit').val(null).trigger('change');
                    //$('#vendor_plan_city_limit').val(null).trigger('change');
                    //System.LoadDropDown("multi_country_dropdown", "vendor_plan_country_limit", null, null, null, data.country_listing);
                    //System.LoadDropDown("multi_state_dropdown", "vendor_plan_state_limit", null, null, data.country_listing, data.state_listing);
                    //System.LoadDropDown("multi_city_dropdown", "vendor_plan_city_limit", null, null, data.state_listing, data.city_listing);
                }
                else {
                    //ShowModel("Error", data.message)
                }
            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {
                $("#vendor_plan_modal").modal('show');
            }
        });
    },
    AddVendorPlanPricing: function () {

    },
    LoadVendorPlanList: function () {
        $.ajax({
            url: "../Plans/_VendorPlansList",
            cache: false,
            type: "GET",
            dataType: "html",
            contentType: 'application/html; charset=utf-8',
            success: function (result) {
                $('#vendor_plan_list_div').html(result);
            },
            error: function (err) {
                //ShowModel("Error", err)
            },
            complete: function () {
                $('#table_get_all_vendor_planlist').DataTable();
            }
        });
    },
    ResetVendorPlanForm: function () {
        $('#hidden_pk_vendor_plan_id').val('');
        $('#plan_type_name').val('');
        $('#vendor_plan_candidate_listing').val('');
        $('#vendor_plan_interest_received_limit').val('');
        $('#vendor_plan_lead_access_limit').val('');
        $('#vendor_plan_technology_limit').val('');
        $('#vendor_plan_country_limit').val(null).trigger('change');
        $('#vendor_plan_state_limit').val(null).trigger('change');
        $('#vendor_plan_city_limit').val(null).trigger('change');
    },
    GetSelectedCountries: function (callback) {
        var selected_countries = $('#vendor_plan_country_limit').select2("val");
        if (selected_countries != null && selected_countries != undefined && selected_countries != "") {
            selected_countries = selected_countries.toString();
        }
        else {
            selected_countries = "";
        }
        System.LoadDropDown("multi_state_dropdown", "vendor_plan_state_limit", null, null, selected_countries);
        if (callback != null && callback != undefined) {
            callback();
        }
    },
    GetSelectedStates: function (callback) {
        var selected_states = $('#vendor_plan_state_limit').select2("val");
        if (selected_states != null && selected_states != undefined && selected_states != "") {
            selected_states = selected_states.toString();
        }
        else {
            selected_states = "";
        }
        System.LoadDropDown("multi_city_dropdown", "vendor_plan_city_limit", null, null, selected_states);
        if (callback != null && callback != undefined) {
            callback();
        }
    },
    AppendCountriesToShow: function () {
        var country_badge = "";
        $('#vendor_plan_show_countries_list').empty();
        $.each(temp_countries_array_names, function (index, val) {
            country_badge += '<span class="pull-left badge bg-red" style="margin-right:2px !important" >' + val + '</span >';
        });
        $('#vendor_plan_show_countries_list').append(country_badge);
    }
};