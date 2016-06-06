

$(document).ready(function () {
    adjustForNav();
});

$(window).resize(function () { adjustForNav(); });

function adjustForNav() {
    var navWidth = $('#sidebar').css('width');
    $('#MainContent').css('padding-left', navWidth);
}