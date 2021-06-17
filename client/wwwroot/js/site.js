// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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
