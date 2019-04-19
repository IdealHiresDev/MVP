$(document).ready(function () {
    $("#btnAddEducation").on("click", function (e) {
        $.ajax({
            url: "../Candidate/AddEducation",
            type: "GET",
            contentType: "application/json",
            async: true,
            success: function (html) {
                $("#modelContentEducation").empty().html(html);
                $("#AcademicsModel").modal('show');
                $('#StartAt').datepicker({ format: 'mm/dd/yyyy' });
                $('#EndAt').datepicker({ format: 'mm/dd/yyyy' });
                $.validator.unobtrusive.parse(document);
            },
            error: function (xHr, status, res) {
                $('#pWarningMessage').empty().html(res);
                $('#CommonWarningModel').modal('show');
            }
        });
    });
});

function LoadEducationData(message) {   
    $.ajax({
        url: "../Candidate/EducationDetails",
        type: "GET",
        contentType: "application/json",
        async: true,       
        success: function (data) {           
            $('#EducationDetailsDiv').empty().html(data);
            $('#EducationDetailsDiv #StartAt').datepicker({ minDate: new Date(2007, 1 - 1, 1) });
            $('#EducationDetailsDiv #EndAt').datepicker({ defaultDate: +7 });
            if (message == 'Deleted') {
                ToastMessageDelete();
            } else if (message == "Saved") {
                ToastMessageSuccess();
            }
            else if (message == "Updated") {
                ToastMessageUpdate();
            }
            else {
                $('#pWarningMessage').empty().html('There was an issue while performing action !');
                $('#CommonWarningModel').modal('show');
            }
        },
        error: function (xHr, status, res) {
            $('#pWarningMessage').empty().html(res);
            $('#CommonWarningModel').modal('show');
        }
    });
}

function LoadPreviewEducationData(message) {
    debugger;
    $.ajax({
        url: "../Candidate/PreviewEducationDetails",
        type: "GET",
        contentType: "application/json",
        async: true,
        success: function (data) {
          
            $("#PreviewAcademicsModel").modal('hide');
            $('#PreviewEducationDetailsDiv').empty().html(data);
            
            if (message == "Updated") {
                ToastMessageUpdate();
            } else if (message == "Deleted") {
                ToastMessageDelete();
            } else {
                $('#pWarningMessage').empty().html('There was an issue while performing action !');
                $('#CommonWarningModel').modal('show');
            }
        },
        error: function (xHr, status, res) {
            $('#pWarningMessage').empty().html(res);
            $('#CommonWarningModel').modal('show');
        }
    });
}

function DeleteEducation(Id) {
    DeleteConfirmation('btnEducationDelete');
    $('#btnEducationDelete').click(function () {
        debugger;
        $.ajax({
            url: "../Candidate/DeleteEducation/" + Id,
            type: "GET",
            contentType: "application/json",
            async: true,
            success: function (data) {
                if (data == 'EducationDeleted') {
                    $('#CommonDeleteModel').modal('hide');
                    LoadEducationData('Deleted');
                }
                else if (data == 'EducationDeleteFailure') {
                    $('#CommonDeleteModel').modal('hide');
                    LoadEducationData('Failure');
                }
            },
            error: function (xHr, status, res) {                
                $('#pWarningMessage').empty().html(res);
                $('#CommonWarningModel').modal('show');
            }
        });
    })
};

function DeletePreviewEducation(Id) {
    DeleteConfirmation('btnEducationDelete');
    $('#btnEducationDelete').click(function () {
        $.ajax({
            url: "../Candidate/DeleteEducation/" + Id,
            type: "GET",
            contentType: "application/json",
            async: true,
            success: function (data) {
                if (data == 'EducationDeleted') {
                    $('#CommonDeleteModel').modal('hide');
                    LoadPreviewEducationData('Deleted');
                }
                else if (data == 'EducationDeleteFailure') {
                    $('#CommonDeleteModel').modal('hide');
                    LoadEducationData('Failure');
                }
            },
            error: function (xHr, status, res) {
                $('#pWarningMessage').empty().html(res);
                $('#CommonWarningModel').modal('show');
            }
        });
    })
};

function EditEducation(Id) {    
    $.ajax({
        url: "../Candidate/GetEducationDetails/" + Id,
        type: "GET",
        contentType: "application/json",
        async: true,
        success: function (html) {
            $("#modelContentEducation").empty().html(html);
            $("#AcademicsModel").modal('show');
            $('#StartAt').datepicker({ format: 'mm/dd/yyyy' });
            $('#EndAt').datepicker({ format: 'mm/dd/yyyy' });
            $.validator.unobtrusive.parse(document);
            Materialize.updateTextFields(document);
        },
        error: function (xHr, status, res) {
            $('#pWarningMessage').empty().html(res);
            $('#CommonWarningModel').modal('show');
        }
    });
};

function GetPreviewEditEducation(Id) {
    $.ajax({
        url: "../Candidate/GetEducationDetails/" + Id,
        type: "GET",
        contentType: "application/json",
        async: true,
        success: function (html) {
            $("#PreviewModelContentEducation").empty().html(html);
            $("#PreviewAcademicsModel").modal('show');
            $('#PreviewModelContentEducation #StartAt').datepicker({ minDate: new Date(2007, 01 - 01, 01) });
            $('#PreviewModelContentEducation #EndAt').datepicker({ defaultDate: +7 });            
            $(".lblEducationHeader").text("Edit Education");
            $("#EduOption").text("Preview");
            $.validator.unobtrusive.parse(document);
            Materialize.updateTextFields(document);
        },
        error: function (xHr, status, res) {
            $('#pWarningMessage').empty().html(res);
            $('#CommonWarningModel').modal('show');
        }
    });
}