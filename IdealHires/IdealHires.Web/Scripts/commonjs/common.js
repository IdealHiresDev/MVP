"use strict";

function ToastMessageDelete() {
    var x = document.getElementById("ToastDeleteDiv");
    x.className = "alert alert-danger alert-dismissible fade fade show";
    setTimeout(function () { x.className = x.className.replace("show", "alert alert-danger alert-dismissible fade"); }, 3000);
}

function ToastMessageSuccess() {
    var x = document.getElementById("ToastSuccessDiv");
    x.className = "alert alert-success alert-dismissible fade show";
    setTimeout(function () { x.className = x.className.replace("show", "alert alert-success alert-dismissible fade"); }, 3000);
}

function ToastWarningSuccess() {
    var x = document.getElementById("ToastWarningDiv");
    x.className = "alert alert-success alert-dismissible fade show";
    setTimeout(function () { x.className = x.className.replace("show", "alert alert-success alert-dismissible fade"); }, 3000);
}