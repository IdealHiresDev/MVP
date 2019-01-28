"use strict";

function AddWorkExp() {
    $.ajax({
        url: "../Candidate/AddWorkExp",
        type: "GET",
        contentType: "application/json",
        async: true,
        // dataType: 'json',
        success: function (html) {
            $("#modelContentWorkExp").empty().html(html);
            $("#WorkExpModal").modal('show');
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
        url: "../Candidate/GetWorkDetails/" + Id,
        type: "GET",
        contentType: "application/json",
        async: true,
        // dataType: 'json',
        success: function (html) {
            $("#modelContentWorkExp").empty().html(html);
            $("#WorkExpModal").modal('show');
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
            url: "../Candidate/DeleteWorkItem/" + Id,
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