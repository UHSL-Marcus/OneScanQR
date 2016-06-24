function pollTimeout(scanned) {
    var info = "";
    if (scanned) info = "Scanning";

    $("#resultlabel").html(info);

    if (!CancelCurrentAction)
        setTimeout($("#hiddenStatusCheckBtn").click(), 500);
}

function RegistrationFinish(success) {
    $("#hiddenQRCompleteBtn").click();

    var info = "Failed";
    if (success) info = "Complete";

    $("#resultlabel").html("Registration " + info);
}
