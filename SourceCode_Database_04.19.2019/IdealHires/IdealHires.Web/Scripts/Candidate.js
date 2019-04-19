$(document).ready(function () {
    //  //  alert('hi');
    //  alert('hi');

});
//General tab
$('#btnSaveCandidate').click(function () {
   
    //var url = '/Candidate/SaveProfile';
    //var model = {
    //    JobTitle: $('#txtDesiredJobTitle').val(),
    //    ResumeFile: $('#txtUploadResume').val(),
    //    FirstName: $('#txtFirstName').val(),
    //    LastName: $('#txtLastName').val()

    //};
    //$.post(url, model, function (res) {

    //});
    var fileUpload = $("#txtUploadResume").get(0);
    var files = fileUpload.files;
    startPositionTts = $('#txtUploadResume').val();
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }

    //validating image file
    var FileUploadPath = fileUpload.value;
    if (FileUploadPath !== '') {
        var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

        if (Extension === "pdf" || Extension === "doc" || Extension === "docx") {
            // ajax call 
        }
        else {
            alert('Please choose .pdf, .doc, .docx extention only');
            return;
        }
    }

    let dataModel = [];
    dataModel.push({
        JobTitle: $('#txtDesiredJobTitle').val(),
        ResumeFile: FileUploadPath,
        FirstName: $('#txtFirstName').val(),
        LastName: $('#txtLastName').val()
    });
    if (dataModel.length === 0) {
        return;
    }
    else {
        let dataString = JSON.stringify(dataModel);
        $.ajax({
            url: '/Candidate/SaveProfile?generalDetails=' + encodeURIComponent(dataString),
            content: "application/json; charset=utf-8",
            type: "POST",
            dataType: "JSON",
            contentType: false,
            processData: false,
            data: fileData,
            async: false,
            success: function (data) {
                //$('[href="#Address"]').tab('show');
            },
            error: function (xhr, textStatus, errorThrown) {
                // TODO: Show error
            }
        });
    }

});
$('#btnContactCandidate').click(function (e) {

    var url = '/Candidate/SaveContact';
    var model = {
        StreetAddress1: $('#txtStreetAddress').val(),
        StreetAddress2: $('#txtAddress2').val(),
        City: $('#txtCity').val(),
        State: $('#txtState').val(),
        Country: $('#txtCountry').val(),
        ZipCode: $('#txtZipCode').val(),
        Phone1: $('#txtPhone1').val(),
        Phone2: $('#txtPhone2').val(),
        //Email: $('#txtEmailId').val()

    };
    $.post(url, model, function (res) {

    });

    $('#btnWorkCandidate').click(function (e) {
        debugger;
        var url = '../Candidate/SaveWork';
        var model = {
            CompanyName: $('#txtCompany').val(),
            PositionName: $('#txtPosition').val(),
            StartAt: $('#txtFrom').val(),
            Salary: $('#txtSalary').val()



            //Email: $('#txtEmailId').val()

        };
        $.post(url, model, function (res) {

        });


        $('#btnSaveJobCandidate').click(function (e) {
            debugger;
            var url = '/Candidate/SaveKeywords';
            var model = {
                Keywords: $('#txtKeywords').val(),
                JobType: $('#customCheck').val(),
                JobCategory: $('#txtJobCategory').val(),
                Objective: $('#txtResumetips').val()



                //Email: $('#txtEmailId').val()

            };
            $.post(url, model, function (res) {

            });


            //$('#btnPreferenceCandidate').click(function (e) {

            //    var url = '/Candidate/SavePreference';
            //    var model = {
            //        Keywords: $('#txtKeywords').val(),
            //        JobType: $('#txtAddress2').val(),
            //        JobCategory: $('#txtJobCategory').val(),
            //        Objective: $('#txtObjective').val(),



            //    };
            //    $.post(url, model, function (res) {

            //    });









            //

            //});

            //$('#btnSaveCandidate').click(function (e) {
            //    debugger;
            //    let dataModel = [];
            //    dataModel.push({
            //        Id: $("#txtId").val(),
            //        JobTitle: $("#txtDesiredJobTitle").val(),
            //        ResumeFile: $("#txtUploadResume").val(),
            //        FirstName: $("#txtFirstName").val(),
            //        LastName: $("#txtLastName").val()
            //    });
            //    if (dataModel.length === 0) {
            //        return;
            //    }
            //    else {
            //        $.ajax({
            //            type: "POST",
            //            url:  "/Candidate/SaveProfile",
            //            content: "application/json; charset=utf-8",
            //            dataType: "JSON",
            //            data: { candidateBasic: JSON.stringify(dataModel) },
            //            success: function (data) {
            //                if (data == 'Saved') {
            //                    alert(data);

            //                    $('#dvGeneral').removeClass("active");
            //                    $('#fldGeneral').hide();

            //                    $('#fldContact').show();
            //                    $("#dvContact").addClass('active');
            //                }
            //                else {

            //                };
            //                //debugger;
            //                //if (data.IsSucceeded) {

            //                //}
            //                //else {
            //                //    WarningAlert('Error', data.Message);
            //                //}
            //            },
            //            error: function (xhr, textStatus, errorThrown) {
            //                // TODO: Show error
            //            }
            //        });
            //    }
        });
    });
});
$("#btnContactCandidate").click(function () {
    var url = '/Candidate/GetJobType';
    $.ajax({
        type: "GET",
        url: url,
        dataType: "json",
        contentType: "application/json",
        success: function (res) {
            //$("#ddlInsureLinxState").val(res.State);
            //$("#chkIncludedpolicy").attr("checked", res.IncludedInPremium);
            //$("#chkJobCosting").attr("checked", res.UseJob);
            //$("#txtClassCode").val(res.ClassCode);
        }
    });
})