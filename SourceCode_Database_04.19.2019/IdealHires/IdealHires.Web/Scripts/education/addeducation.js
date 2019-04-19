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
    debugger;
    if (data == "EducationSuccess") {
        $("#AcademicsModel").modal('hide');
        LoadEducationData();
        LoadPreviewEducationData('Updated');
    } else {
        $('#pWarningMessage').empty().html('There was an issue saving data !');
        $('#CommonWarningModel').modal('show');
    }
}

$("#btnAddEducationSubmit").on("click", function (e) {
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

//function LoadEducationData() {
//    $.ajax({
//        url: "../Candidate/EducationDetails",
//        type: "GET",
//        contentType: "application/json",
//        async: true,
//        // dataType: 'json',
//        success: function (data) {
//            $('#EducationDetailsDiv').empty().html(data);
//            ToastMessageSuccess();
//        },
//        error: function (xHr, status, res) {
//            $('#pWarningMessage').empty().html(res);
//            $('#CommonWarningModel').modal('show');
//        }
//    });
//}