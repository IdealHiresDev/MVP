"use strict";
function AddWorkExp() {    
    $.ajax({
        url: "../Candidate/AddWorkExp",
        type: "GET",
        contentType: "application/json",
        async: true,        
        success: function (html) {
            $("#modelContentWorkExp").empty().html(html);
            $("#WorkExpModal").modal('show');
            $('#StartAt').datepicker({ format: 'mm/dd/yyyy' });
            $('#EndAt').datepicker({ format: 'mm/dd/yyyy' });
            $.validator.unobtrusive.parse(document);
        },
        error: function (xHr, status, res) {           
            $('#pWarningMessage').empty().html(res);
            $('#CommonWarningModel').modal('show');
        }
    });
}

function GetWorkExp(Id) { 
    
    $.ajax({
        url: "../Candidate/GetWorkDetails/" + Id,
        type: "GET",
        contentType: "application/json",
        async: true,        
        success: function (html) {            
            $("#modelContentWorkExp").empty().html(html);
            $("#WorkExpModal").modal('show');
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
}

function GetPreviewWorkExp(Id) {    
    $.ajax({
        url: "../Candidate/GetWorkDetails/" + Id,
        type: "GET",
        contentType: "application/json",
        async: true,      
        success: function (html) {           
            $("#PreviewModelContentWorkExp").empty().html(html);
            $("#PreviewWorkExpModal").modal('show');
            $("#WorkOption").text("Preview");
            $(".lblHeader").text("Edit Work Experience");
            $('#PreviewModelContentWorkExp #StartAt').datepicker({ minDate: new Date(2007, 1 - 1, 1) });
            $('#PreviewModelContentWorkExp #EndAt').datepicker({ defaultDate: +7 });
            $.validator.unobtrusive.parse(document);
            Materialize.updateTextFields(document);
        },
        error: function (xHr, status, res) {
            $('#pWarningMessage').empty().html(res);
            $('#CommonWarningModel').modal('show');
        }
    });
}
function DeleteWorkExp(Id) {
    debugger;
    DeleteConfirmation('btnWorkExpDelete'); 
    $('#btnWorkExpDelete').click(function () {
        $.ajax({
            url: "../Candidate/DeleteWorkItem/" + Id,
            type: "GET",
            contentType: "application/json",
            async: true,           
            success: function (data) {
                if (data == 'DeletedWork') {
                    $('#CommonDeleteModel').modal('hide');
                    LoadWorkData('Deleted');
                    LoadWorkDataPreview('Updated');
                }
                else if (data == 'DeleteWorkFailure') {
                    $('#CommonDeleteModel').modal('hide');
                    LoadWorkData('Failure');
                }
            },
            error: function (xHr, status, res) {
                $('#pWarningMessage').empty().html(res);
                $('#CommonWarningModel').modal('show');
            }
        });
    });            
}

function DeletePreviewWorkExp(Id) {
    DeleteConfirmation('btnWorkExpDelete');
    $('#btnWorkExpDelete').click(function () {
        $.ajax({
            url: "../Candidate/DeleteWorkItem/" + Id,
            type: "GET",
            contentType: "application/json",
            async: true,
            success: function (data) {
                if (data == 'DeletedWork') {
                    $('#CommonDeleteModel').modal('hide');                  
                }
                else if (data == 'DeleteWorkFailure') {
                    $('#CommonDeleteModel').modal('hide');                 
                }
                LoadWorkDataPreview('Deleted');
                
            },
            error: function (xHr, status, res) {
                $('#pWarningMessage').empty().html(res);
                $('#CommonWarningModel').modal('show');
            }
        });
    });
}

function LoadWorkData(message) {
    $.ajax({
        url: "../Candidate/WorkDetails",
        type: "GET",
        contentType: "application/json",
        async: true,
        success: function (data) {
            $('#WorkDetailsDiv').empty().html(data);
            $('#WorkDetailsDiv #StartAt').datepicker({ minDate: new Date(2007, 1 - 1, 1) });
            $('#WorkDetailsDiv #EndAt').datepicker({ defaultDate: +7 });
            if (message == "Saved") {
                ToastMessageSuccess();
            } else if (message == 'Deleted') {
                ToastMessageDelete();
            } else if (message == 'Updated') {
                ToastMessageUpdate();
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

function LoadWorkDataPreview(message) {   
    $.ajax({
        url: "../Candidate/PreviewWorkDetails",
        type: "GET",
        contentType: "application/json",
        async: true,
        success: function (data) {               
            $("#previewWorkDetailsDiv").empty().html(data);
            $('#previewWorkDetailsDiv #StartAt').datepicker({ minDate: new Date(2007, 1 - 1, 1) });
            $('#previewWorkDetailsDiv #EndAt').datepicker({ defaultDate: +7 });
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




