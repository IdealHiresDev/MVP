"use strict";

$(document).ready(function () {
    $("#exampleSwitch").change(function () {
        if ($("#exampleSwitch").is(":checked")) {
            $('#IsDegreeOrCertification').val('True');           
        }
        else {
            $('#IsDegreeOrCertification').val('False');            
        }
    });
});
function EducationSuccess(data) {
    if (data === "EducationSuccess") {
        $("#AcademicsModel").modal('hide');
        LoadData();
    } else {
        alert("Failure");
    }
}

$("#btnAddWducationSubmit").on("click", function (e) {
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

function LoadData() {
    $.ajax({
        url: "../Candidate/EducationDetails",
        type: "GET",
        contentType: "application/json",
        async: true,
        // dataType: 'json',
        success: function (data) {
            $('#EducationDetailsDiv').empty().html(data);
        },
        error: function (xHr, status, res) {
            alert('Failure');
        }
    });
}