
var CancelCurrentAction = false;

$(document).ready(function () {
    //adjustForNav();

    var pathname = window.location.pathname;
    $("#navList").children("li").each(function (index, element) {
        $(this).removeClass("active");
        if ($(this).children("a").get(0).href.indexOf(pathname) > -1) $(this).addClass("active");
    });

    $(".SideBarNavLink").click(function (handler) {
        doCancelActions();
        return true;
    });
});

//$(window).resize(function () { adjustForNav(); });

function adjustForNav() {
    var navWidth = $('#sidebar').css('width');
    $('#MainContent').css('padding-left', navWidth);
}

function doCancelActions() {
    CancelCurrentAction = true;

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (prm.get_isInAsyncPostBack()) {
        prm.abortPostBack();
    }
}