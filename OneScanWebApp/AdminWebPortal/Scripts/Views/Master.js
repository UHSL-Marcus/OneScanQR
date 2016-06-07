

$(document).ready(function () {
    adjustForNav();

    var pathname = window.location.pathname;
    $("#navList").children("li").each(function (index, element) {
        $(this).removeClass("active");
        if ($(this).children("a").attr('href') == pathname) $(this).addClass("active");
    });
});

$(window).resize(function () { adjustForNav(); });

function adjustForNav() {
    var navWidth = $('#sidebar').css('width');
    $('#MainContent').css('padding-left', navWidth);
}