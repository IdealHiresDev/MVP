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
        LoadWorkData('Saved');
    } else if (data == "WorkExpEditSuccess") {
        $("#WorkExpModal").modal('hide');
        LoadWorkData('Updated');
    } else {
        $('#pWarningMessage').empty().html('There was an issue saving/updating data !');
        $('#CommonWarningModel').modal('show');
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

function LoadWorkData(message) {
    $.ajax({
        url: "../Candidate/WorkDetails",
        type: "GET",
        contentType: "application/json",
        async: true,
        success: function (data) {
            $('#WorkDetailsDiv').empty().html(data);
            if (message == "Saved") {
                ToastMessageSuccess();
            } else if (message == 'Deleted') {
                ToastMessageDelete();
            } else if (message == 'Updated') {
                ToastMessageUpdate();
            }
        },
        error: function (xHr, status, res) {
            $('#pWarningMessage').empty().html(res);
            $('#CommonWarningModel').modal('show');
        }
    });
}