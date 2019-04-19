"use strict";

function ToastMessageDelete() {
    var x = document.getElementById("ToastDeleteDiv");
    x.className = "alert alert-danger alert-dismissible fade fade show";
    setTimeout(function () { x.className = x.className.replace("alert alert-danger alert-dismissible fade fade show", "alert alert-danger alert-dismissible fade"); }, 3000);
}

function ToastMessageSuccess() {
    var x = document.getElementById("ToastSuccessDiv");
    x.className = "alert alert-success alert-dismissible fade show";
    setTimeout(function () { x.className = x.className.replace("alert alert-success alert-dismissible fade show", "alert alert-success alert-dismissible fade"); }, 6000);
}

function ToastMessageUpdate() {
    var x = document.getElementById("ToastUpdateDiv");
    x.className = "alert alert-success alert-dismissible fade show";
    setTimeout(function () { x.className = x.className.replace("alert alert-success alert-dismissible fade show", "alert alert-success alert-dismissible fade"); }, 3000);
}

function ToastWarningSuccess() {
    var x = document.getElementById("ToastWarningDiv");
    x.className = "alert alert-success alert-dismissible fade show";
    setTimeout(function () { x.className = x.className.replace("alert alert-success alert-dismissible fade show", "alert alert-success alert-dismissible fade"); }, 3000);
}

function DeleteConfirmation(buttonId) {
    $("#CommonDeleteModel").modal('show');    
    $("#CommonDeleteModelDiv").html('<button type="button" class="btn btn-light box-shadow-none mr-3" data-dismiss="modal">Cancel</button>&nbsp;<button type="button" class="btn btn-bg-danger" id=' + buttonId +'>Delete</button>');
}
function DeleteConfirmation1() {
    $("#CommonDeleteModel").modal('show');
    $("#CommonDeleteModelDiv").html('<button type="button" class="btn btn-light box-shadow-none mr-3" data-dismiss="modal">Delete</button>');
}
function DeleteConfirmation2(buttonId,deleteid) {   
    $("#CommonDeleteModel").modal('show');
    $("#CommonDeleteModelDiv").html('<button type="button" class="btn btn-light box-shadow-none mr-3" data-dismiss="modal">Cancel</button>&nbsp;<button type="button" class="btn btn-bg-danger" id=' + buttonId + ' data-val=' + deleteid + '>Delete</button>');
}
function ToastMessageUserUnableDelete() {
    var x = document.getElementById("ToastWarningUserDiv");
    x.className = "alert alert-danger alert-dismissible fade fade show";
    setTimeout(function () { x.className = x.className.replace("alert alert-danger alert-dismissible fade fade show", "alert alert-danger alert-dismissible fade"); }, 3000);
}

function ToastMessagePaymentSuccess() {
    var x = document.getElementById("ToastMessagePaymentDiv");
    x.className = "alert alert-success alert-dismissible fade show";
    setTimeout(function () { x.className = x.className.replace("alert alert-success alert-dismissible fade show", "alert alert-success alert-dismissible fade"); }, 3000);
}
function ToastMessagePaymentFailure() {
    var x = document.getElementById("ToastMessagePaymentFailureDiv");
    x.className = "alert alert-danger alert-dismissible fade show";
    setTimeout(function () { x.className = x.className.replace("alert alert-danger alert-dismissible fade show", "alert alert-danger alert-dismissible fade"); }, 3000);
}