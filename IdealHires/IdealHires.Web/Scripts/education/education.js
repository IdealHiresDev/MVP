"use strict";

function AddEducation() {
    $.ajax({
        url: "../Candidate/AddEducation",
        type: "GET",
        contentType: "application/json",
        async: true,
        // dataType: 'json',
        success: function (html) {
            $("#modelContentEducation").empty().html(html);
            $("#AcademicsModel").modal('show');
            $('#StartAt').datepicker({ minDate: new Date(2007, 1 - 1, 1) });
            $('#EndAt').datepicker({ defaultDate: +7 });
            $.validator.unobtrusive.parse(document);
        },
        error: function (xHr, status, res) {
            alert('Failure');
        }
    });
}

function GetWorkExp(Id) {
    $.ajax({
        url: "../Candidate/GetEducationDetails/" + Id,
        type: "GET",
        contentType: "application/json",
        async: true,
        // dataType: 'json',
        success: function (html) {
            $("#modelContentEducation").empty().html(html);
            $("#AcademicsModel").modal('show');
            $('#StartAt').datepicker({ minDate: new Date(2007, 1 - 1, 1) });
            $('#EndAt').datepicker({ defaultDate: +7 });
            $.validator.unobtrusive.parse(document);
        },
        error: function (xHr, status, res) {
            alert('Failure');
        }
    });
}
function DeleteWorkExp(Id) {
    if (confirm("Are you sure?")) {
        $.ajax({
            url: "../Candidate/DeleteEducation/" + Id,
            type: "GET",
            contentType: "application/json",
            async: true,
            // dataType: 'json',
            success: function (data) {
                if (data == 'Deleted') {
                    LoadData();
                }
                else {
                    LoadData();
                }
            },
            error: function (xHr, status, res) {
                alert('Failure');
            }
        });
    }
    return false;
}

function LoadData() {
    $.ajax({
        url: "../Candidate/EducationDetails",
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