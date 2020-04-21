$(document).ready(function () {
    
    setContentHeight();

    if ($('#txtUsername').val() == '') {
        $('#txtUsername').focus();
    }
    else {
        $('#txtPassword').focus();
    }

    window.onresize = function () {
        setContentHeight();
    };
});

function setContentHeight() {
    var height = getWindowHeight() - 96;    //48px top and bottom padding

    $('.log_in').css('min-height', height + 'px');
}