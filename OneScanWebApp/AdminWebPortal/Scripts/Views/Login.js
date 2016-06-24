function pollTimeout(scanned) {
    var info = "";
    if (scanned) info = "Scanning";

    $("#resultlabel").html(info);

    if (!CancelCurrentAction)
        setTimeout($("#hiddenStatusCheckBtn").click(), 500);
}

function ScanFailed() {
    $("#resultlabel").html("Login Failed");
    $("#hiddenNewQRBtn").click();
}

