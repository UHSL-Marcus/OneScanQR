var url;

function httpGetAsync(url, callback) {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function () {
        if (xmlHttp.readyState == 4 && xmlHttp.status == 200)
            callback(xmlHttp.responseText);
    }
    xmlHttp.open("GET", url, true); // true for asynchronous 
    xmlHttp.send(null);
}

function httpResponseCallback(response) {
    alert(response);
    if (response < 2)
        setTimeout(function () { httpGetAsync(url, httpResponseCallback) }, 1000);
    else if (response == 2) {
        document.getElementById("hiddenPostBackBtn").click();
    }
    else if (response == 3) {
        document.getElementById("hiddenNewQRBtn").click();
    }
}

function pollOneScan(_url) {
    url = _url;
    alert("Url: " + url);
    httpGetAsync(url, httpResponseCallback);

    alert("sent");
}