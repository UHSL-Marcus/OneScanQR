

$(document).ready(function () {
    //adjustForNav();

    var pathname = window.location.pathname;
    $("#navList").children("li").each(function (index, element) {
        $(this).removeClass("active");
        if ($(this).children("a").get(0).href.indexOf(pathname) > -1) $(this).addClass("active");
    });
});

//$(window).resize(function () { adjustForNav(); });

function adjustForNav() {
    var navWidth = $('#sidebar').css('width');
    $('#MainContent').css('padding-left', navWidth);
}