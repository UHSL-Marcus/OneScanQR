function pollTimeout() {
    alert("timout");
    setTimeout($("#hiddenStatusCheckBtn").click(), 1000);
}

function ScanFailed() {
    alert("failed");
    $("#hiddenNewQRBtn").click();
}

