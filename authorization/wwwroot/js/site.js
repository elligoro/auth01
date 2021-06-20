// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

window.addEventListener('DOMContentLoaded', authorizeQueryHandle, false);
// Write your JavaScript code.
var queryObj = {};

function authorizeQueryHandle(e) {

    var params = window.location.search;
    var paramHandler = new URLSearchParams(params);
    queryObj.client_id = paramHandler.get("client_id");
    queryObj.redirect_uri = paramHandler.get("redirect_uri");
    queryObj.response_type = paramHandler.get("response_type");
    queryObj.state = paramHandler.get("state");

    var clientIdUi = document.querySelector("#client_id_span");
    clientIdUi.innerHTML = queryObj.client_id;

    var approveAuth = document.getElementById("approveAuth");
    approveAuth.addEventListener("click", approveAuthCallback, false);
}

function approveAuthCallback(e) {
    $.ajax({
        method: 'GET',
        url: `code/${queryObj.client_id}?state=${queryObj.state}`,
        success: (json) => {
            location.href = `${queryObj.redirect_uri}?code=${json.code}&state_req=${queryObj.state}&state_res=${json.state}`;
        }
    });
}