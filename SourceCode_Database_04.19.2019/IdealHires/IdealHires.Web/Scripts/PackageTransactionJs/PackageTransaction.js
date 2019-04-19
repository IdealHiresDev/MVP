$(document).ready(function () {
    $("#userTable").DataTable({
        "processing": true,
        "serverSide": true,
        "searching": false,
        "sorting": false,
        "createdRow": function (row, data, dataIndex) {
            $(row).addClass('job-items');
        },
        "ajax": {
            "url": "../PackageTansaction/GetPackage",
            "type": "POST",
            "datatype": "json"
        },

        "columns": [
            {
                "data": "Price", "name": "Price", "autoWidth": true, "searchable": false,
                "sortable": false
            },
            {
                "data": "Discount", "name": "Discount", "autoWidth": true, "searchable": false,
                "sortable": false,
            },
            {
                "data": "Duration", "name": "Duration", "autoWidth": true, "searchable": false,
                "sortable": false,
            },
            {
                "data": "JobCredit", "name": "JobCredit", "autoWidth": true, "searchable": false,
                "sortable": false,
            },
            {
                "data": "Description", "name": "Description", "autoWidth": true, "searchable": false,
                "sortable": false,
            },
            {
                "className": "action",
                'render': function (data, type, full, meta) {
                    debugger;
                    return ' <a href="../PackageTansaction/Details?id=' + full.Id + '" class="preview" title="Preview"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-eye"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path><circle cx="12" cy="12" r="3"></circle></svg></a>'
                        + '<a href="../PackageTansaction/PackageTansactionManages?id=' + full.Id + '" class="edit" title="Edit"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-edit"><path d="M20 14.66V20a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h5.34"></path><polygon points="18 2 22 6 12 16 8 16 8 12 18 2"></polygon></svg></a>'
                        + '<a onclick="Deleteuser(' + full.Id + ')" class="remove" title="Delete"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-trash-2"><polyline points="3 6 5 6 21 6"></polyline><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path><line x1="10" y1="11" x2="10" y2="17"></line><line x1="14" y1="11" x2="14" y2="17"></line></svg></a>'
                },
                "searchable": false,
                "sortable": false,
            }
        ]

    });
});


function Deleteuser(Id) {
    DeleteConfirmation('btnPackageDelete');
    $('#btnPackageDelete').click(function () {
        $.ajax({
            url: "../PackageTansaction/Remove/?Id=" + Id,
            type: "GET",
            contentType: "application/json",
            async: true,
            dataType: "json",
            success: function (data) {
                if (data.Message == 'DeleteSuccess') {
                    $('#CommonDeleteModel').modal('hide');
                    ToastMessageDelete();
                    setTimeout(function () { window.location.href = data.url; }, 4000);
                }
                else if (data.Message == 'DeleteFailure') {
                    $('#pWarningMessage').empty().html('There was an error deleting information !');
                    $('#CommonWarningModel').modal('show');
                }
            },
            error: function (xHr, status, res) {
                $('#pWarningMessage').empty().html(res);
                $('#CommonWarningModel').modal('show');
            }
        });
    })
};

