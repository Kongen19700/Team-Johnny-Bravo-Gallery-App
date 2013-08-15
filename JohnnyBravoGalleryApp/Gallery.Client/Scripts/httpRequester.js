/// <reference path="jquery-2.0.3.js" />

var httpRequester = (function () {

    function getJSON(url, successHandler, errorHandler) {
        $.ajax({
            url: url,
            type: "GET",
            contentType: "application/json",
            timeout: 5000,
            success: successHandler,
            error: errorHandler,
        });
    }

    function postJSON(url, data, successHandler, errorHandler) {
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            timeout: 5000,
            data: JSON.stringify(data),
            success: successHandler,
            error: errorHandler,
        });
    }

    return {
        getJSON: getJSON,
        postJSON: postJSON,
    }
})();