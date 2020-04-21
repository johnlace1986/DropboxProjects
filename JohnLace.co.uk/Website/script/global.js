function FillScreen() {
    var wrapper = document.getElementById('wrapper');
    var footer = document.getElementById('footer');
    var window_height = get_window_height();

    while (wrapper.offsetHeight < window_height)
        footer.style.height = (footer.offsetHeight + 1) + 'px';
}

function SetFooterSize() {
    var footer = document.getElementById('footer_div');
    var latestTweetDiv = document.getElementById('latest_tweet_div');

    if (latestTweetDiv.offsetHeight > 270)
        footer.style.minHeight = (latestTweetDiv.offsetHeight - 24) + 'px';
    else
        footer.style.minHeight = '270px';
}

function CentrePopup(id) {
    var popup = document.getElementById(id);
    var window_width = get_window_width();
    var window_height = get_window_height();

    popup.style.left = ((window_width / 2) - (popup.offsetWidth / 2)) + 'px';
    popup.style.top = ((window_height / 2) - (popup.offsetHeight / 2)) + 'px';
}

function get_window_height() {

    var viewportheight;

    if (typeof window.innerHeight != 'undefined') {
        viewportheight = window.innerHeight;
    }
    else if (typeof document.documentElement != 'undefined' &&
                    typeof document.documentElement.clientHeight != 'undefined' &&
                    document.documentElement.clientHeight != 0) {
        viewportheight = document.documentElement.clientHeight;
    }
    else {
        viewportheight = document.getElementsByTagName('body')[0].clientHeight;
    }

    return viewportheight;
}

function get_window_width() {

    var viewportwidth;

    if (typeof window.innerWidth != 'undefined') {
        viewportwidth = window.innerWidth;
    }
    else if (typeof document.documentElement != 'undefined' &&
                    typeof document.documentElement.clientWidth != 'undefined' &&
                    document.documentElement.clientWidth != 0) {
        viewportwidth = document.documentElement.clientWidth;
    }
    else {
        viewportwidth = document.getElementsByTagName('body')[0].clientWidth;
    }

    return viewportwidth;
}

function ValidateEmailAddress(address) {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    if (reg.test(address) == false) {
        return false;
    }

    return true;
}

function SetVisibility(id, visible) {
    var lblError = document.getElementById(id);

    if (visible)
        lblError.style.visibility = 'visible';
    else
        lblError.style.visibility = 'hidden';
}