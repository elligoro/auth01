// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.addEventListener('load', function (e) {
    if (window.token) {
        var existingToken = window.localStorage.getItem('access_token');
        if (!existingToken || existingToken != window.token)
            window.localStorage.setItem('access_token', window.token);
    }
});

function sendAuth() {
    $.ajax({
        url: "Home/GetAuthUrl",
        method: "POST",
        dataType: 'json',
        success: (json) => {
            if (!json)
                return;

            location.href = `${json.url}?${setPropsToQuery(json.body)}`;
        }
    });
}

function setPropsToQuery(obj) {
    var query = "";
    for (var prop in obj) {
        query += `${prop}=${obj[prop]}&`;
    }
    return query;
}

function getResource() {
    $.ajax({
        url: "Home/GetProtectedResource?token=" + localStorage.getItem('access_token'),
        method: "GET",
        dataType: "json",
        success: (json) => {
            if (!json)
                return;

            console.log(json);
        }
    });
}