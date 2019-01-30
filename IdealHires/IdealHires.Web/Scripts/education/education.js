"use strict";

$(document).ready(function () {
    $("#editEducationDiv a").click(function () {
        var Id = $(this).attr('id');
        $.ajax({
            url: "../Candidate/GetEducationDetails/" + Id,
            type: "GET",
            contentType: "application/json",
            async: true,
            success: function (html) {
                $("#modelContentEducation").empty().html(html);
                $("#AcademicsModel").modal('show');
                $('#StartAt').datepicker({ minDate: new Date(2007, 1 - 1, 1) });
                $('#EndAt').datepicker({ defaultDate: +7 });
                $.validator.unobtrusive.parse(document);
            },
            error: function (xHr, status, res) {
                $('#pWarningMessage').empty().html(res);
                $('#CommonWarningModel').modal('show');
            }
        });
    });

    $("#deleteEducationDiv a").click(function () {        
        $('#CommonDeleteModel').modal('show');
    });

    $('#btnCommonDeleteModel').on("click", function (e) {
        var Id = $("#deleteEducationDiv a").attr('id');       
        $.ajax({
            url: "../Candidate/DeleteEducation/" + Id,
            type: "GET",
            contentType: "application/json",
            async: true,
            success: function (data) {                
                if (data == 'EducationDeleted') {
                    $('#CommonDeleteModel').modal('hide');
                    LoadEducationData1('Deleted');
                }
                else if (data =='EducationDeleteFailure') {
                    $('#CommonDeleteModel').modal('hide');
                    LoadEducationData1('Failure');
                }
            },
            error: function (xHr, status, res) {
                $('#pWarningMessage').empty().html(res);
                $('#CommonWarningModel').modal('show');
            }
        });
    });    
});

$("#btnAddEducation").on("click", function (e) {
    $.ajax({
        url: "../Candidate/AddEducation",
        type: "GET",
        contentType: "application/json",
        async: true,
        success: function (html) {            
            $("#modelContentEducation").empty().html(html);
            $("#AcademicsModel").modal('show');
            $('#StartAt').datepicker({ minDate: new Date(2007, 1 - 1, 1) });
            $('#EndAt').datepicker({ defaultDate: +7 });
            $.validator.unobtrusive.parse(document);
        },
        error: function (xHr, status, res) {           
            $('#pWarningMessage').empty().html(res);
            $('#CommonWarningModel').modal('show');
        }
    });
});

function LoadEducationData1(message) {   
    $.ajax({
        url: "../Candidate/EducationDetails",
        type: "GET",
        contentType: "application/json",
        async: true,       
        success: function (data) {
            $('#EducationDetailsDiv').empty().html(data);
            if (message === 'Deleted') {
                ToastMessageDelete();
            }
            else {
                $('#pWarningMessage').empty().html('There was an issue deleting data !');
                $('#CommonWarningModel').modal('show');
            }
        },
        error: function (xHr, status, res) {
            $('#pWarningMessage').empty().html(res);
            $('#CommonWarningModel').modal('show');
        }
    });
}