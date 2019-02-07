"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var WorkDetails = /** @class */ (function () {
    function WorkDetails() {
        this.display = 'none'; //default Variable
    }
    WorkDetails.prototype.modal = function () {
        alert("Hello");
    };
    return WorkDetails;
}());
exports.WorkDetails = WorkDetails;
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
//# sourceMappingURL=workdetaillist.js.map