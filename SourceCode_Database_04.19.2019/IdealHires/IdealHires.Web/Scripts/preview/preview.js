function GetPreviewPreferenceOnModel() {
    debugger;
    $.ajax({
        url: "../Candidate/PreviewAddPreferences",
        type: "GET",
        contentType: "application/json",
        async: true,
        success: function (data) {
            $('#PreviewModelContentJobPreference').empty().html(data);
            $("#PreviewJobPreferenceModal").modal('show');            
            $("#PreferenceOption").text("Preview");
        },
        error: function (xHr, status, res) {
            $('#pWarningMessage').empty().html(res);
            $('#CommonWarningModel').modal('show');
        }
    });
}

function GetPreviewPreference() {
    debugger;
    $.ajax({
        url: "../Candidate/PreviewPreferenceDetails",
        type: "GET",
        contentType: "application/json",
        async: true,
        success: function (data) {
            $('#PreviewPreferenceDetailsDiv').empty().html(data);
        },
        error: function (xHr, status, res) {
            $('#pWarningMessage').empty().html(res);
            $('#CommonWarningModel').modal('show');
        }
    });
}
