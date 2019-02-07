export class WorkDetails {

display: any = 'none'; //default Variable
 modal() {
    alert("Hello");
}
}

//function AddWorkExp() {
//    debugger;
//    $.ajax({
//        url: "../Candidate/AddWorkExp",
//        type: "GET",
//        contentType: "application/json",
//        async: true,
//        // dataType: 'json',
//        success: function (html) {
//            $("#modelContentWorkExp").empty().html(html);
//            $("#WorkExpModal").modal('show');
//            $('#StartAt').datepicker({ minDate: new Date(2007, 1 - 1, 1) });
//            $('#EndAt').datepicker({ defaultDate: +7 });
//            $.validator.unobtrusive.parse(document);
//        },
//        error: function (xHr, status, res) {
//            $('#pWarningMessage').empty().html(res);
//            $('#CommonWarningModel').modal('show');
//        }
//    });
//}

//function GetWorkExp(id) {
//    AddWorkExp();
//    alert(id);
//}
//function modal() {
//    $("#WorkExpModal").modal('show');
//}