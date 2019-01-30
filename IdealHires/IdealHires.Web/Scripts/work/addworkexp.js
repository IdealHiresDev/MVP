"use strict";

$(document).ready(function () {
    $("#exampleSwitch").change(function () {
        if ($("#exampleSwitch").is(":checked")) {
            $('#IsCurrent').val('True');
            $('#EndAtDiv').hide();
        }
        else {
            $('#IsCurrent').val('False');
            $('#EndAtDiv').show();
        }
    });
});
function WorkExperienceSuccess(data) {
    if (data == "WorkExpSuccess") {
        $("#WorkExpModal").modal('hide');
        LoadWorkData();
    } else {
        alert("Failure");
    }
}

$("#btnAddWorkSubmit").on("click", function (e) {
    var formId = e.delegateTarget.form.id;
    $.validator.unobtrusive.parse(formId);
    $("#" + formId).validate();
    var res = $("#" + formId).valid();
    if ($("#" + formId).valid()) {
        $("#" + formId).submit();
    }
    else {
        return false;
    }
});

function LoadWorkData() {
    $.ajax({
        url: "../Candidate/WorkDetails",
        type: "GET",
        contentType: "application/json",
        async: true,
        // dataType: 'json',
        success: function (data) {
            $('#WorkDetailsDiv').empty().html(data);
        },
        error: function (xHr, status, res) {
            alert('Failure');
        }
    });
}