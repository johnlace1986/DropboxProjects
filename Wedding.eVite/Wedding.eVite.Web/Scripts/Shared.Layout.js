var fadeImageCount = 0;
var doScrollToContent = true;

function setContentHeight() {

    var contentHeight = getWindowHeight() - 24; //vertical padding
    contentHeight -= $('.logged_in_layout_footer').height() + 72; //48 px vertical padding + 24px vertical padding

    $('.layout_onscreen').css('minHeight', contentHeight + 'px');
}

function getUnreadMessageCount(inviteId, actionUrl) {

    $.ajax({
        url: actionUrl,
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=uft-8",
        data: JSON.stringify({
            inviteId: inviteId
        }),
        success: function (data) {

            if (data.unreadMessageCount == 0) {
                setUnreadMessageCount(0);
            }
            else {
                setUnreadMessageCount(data.unreadMessageCount);
            }
        },
        error: function (response, status, errorThrown) {
            alert(response);
            setUnreadMessageCount(0);
        }
    });
}

function setUnreadMessageCount(unreadMessageCount) {
    if (unreadMessageCount == 0)
        $('.logged_in_layout_footer_contact_us_unread_message_count').html('');
    else
        $('.logged_in_layout_footer_contact_us_unread_message_count').html(unreadMessageCount);
}

function fadeInImages() {

    fadeImageCount = $('.layout_images').find('div[data-imageFadeIndex]').length;
    fadeInImage(0, 750);
}

function fadeInImage(imageIndex, timeout) {

    var div = $('.layout_images').find('div[data-imageFadeIndex=' + imageIndex + ']').find('img:eq(0)');
    div.fadeIn(timeout);

    if (imageIndex == (fadeImageCount - 1)) {
        if (doScrollToContent)
            scrollToContent(1000);
    }
    else {
        setTimeout(function () {
            fadeInImage(imageIndex + 1, timeout);
        }, timeout);
    }
}

function scrollToContent(interval) {

    var offset = $(".layout_onscreen").offset().top + 12;

    if (interval == 0) {
        $(window).scrollTop(offset);
    }
    else {
        setTimeout(function () {
            $('html, body').animate({
                scrollTop: offset
            }, interval);
        }, 1000);
    }
}

function cancelScrollToContent() {
    doScrollToContent = false;
}