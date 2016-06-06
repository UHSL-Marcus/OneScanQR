function pollTimeout() {
    alert("timout");
    setTimeout($("#hiddenStatusCheckBtn").click(), 1000);
}

function ScanFinish(success) {
    if (success)
        $("#hiddenPostBackBtn").click();
    else
        $("#hiddenNewQRBtn").click();
}

