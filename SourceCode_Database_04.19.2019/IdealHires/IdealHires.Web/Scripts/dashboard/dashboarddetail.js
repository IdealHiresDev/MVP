"use strict";

function GetJobDetails(Id) {
    debugger;
    $.ajax({
        url: "../Employer/GetJobDetails/" + Id,
        type: "GET",
        contentType: "application/json",
        async: true,
        // dataType: 'json',
        success: function (html) {
            var url = '@ Url.Content("~/Employer/PostJob?NestId =' + Id + '")'
            window.location.href = url; 
         
        },
        error: function (xHr, status, res) {

            $('#pWarningMessage').empty().html(res);
            $('#CommonWarningModel').modal('show');
        }
    });
}
function DeleteJobDetails(Id) {
    debugger;
    DeleteConfirmation('btnWorkExpDelete');
    $('#btnWorkExpDelete').click(function () {
        $.ajax({
            url: "../Employer/DeleteJobDetails/" + Id,
            type: "GET",
            contentType: "application/json",
            async: true,
            success: function (data) {
                if (data == 'DeletedWork') {
                    $('#CommonDeleteModel').modal('hide');
                    window.location.reload();
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