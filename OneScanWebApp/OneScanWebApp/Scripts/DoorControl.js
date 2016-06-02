

var sessionID;

function httpGetAsync(theUrl, callback) {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function () {
        alert("state: " + xmlHttp.readyState + ", status: " + xmlHttp.status);
        if (xmlHttp.readyState == 4) //&& xmlHttp.status == 200)
            callback(xmlHttp.responseText);
    }
    xmlHttp.open("GET", theUrl, true); // true for asynchronous 
    xmlHttp.send(null);
}

function httpResponseCallback(response) {
    alert("Callback");
    alert(response);
}

function pollOneScan(_sessionID) {
    sessionID = _sessionID;
    alert("SessionID: " + _sessionID);
    httpGetAsync("https://liveservice.ensygnia.net/api/PartnerGateway/1/CheckOnescanSessionStatus?OnescanSessionID=" + sessionID, function (response) {
        alert(response);
        alert("Callback");
    });

    alert("sent2");
}




