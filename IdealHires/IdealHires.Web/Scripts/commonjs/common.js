"use strict";

function ToastMessageDelete() {    
    var x = document.getElementById("ToastDeleteDiv");
    x.className = "alert alert-danger alert-dismissible fade fade show";
    setTimeout(function () { x.className = x.className.replace("alert alert-danger alert-dismissible fade fade show", "alert alert-danger alert-dismissible fade"); }, 3000);
}

function ToastMessageSuccess() {   
    var x = document.getElementById("ToastSuccessDiv");
    x.className = "alert alert-success alert-dismissible fade show";
    setTimeout(function () { x.className = x.className.replace("alert alert-success alert-dismissible fade show", "alert alert-success alert-dismissible fade"); }, 3000);
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