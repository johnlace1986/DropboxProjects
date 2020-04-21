function setErrorsContainerHeight() {
    var availableSpace = getWindowHeight() - 24;    //vertical padding

    availableSpace -= $('.admin_layout_navigation').height() + 72;            //48px vertical padding + 24px vertical margin
    availableSpace -= $('.logged_in_layout_footer').height() + 96;      //48px vertical padding + 48px vertical margin

    if (availableSpace < 240)
        availableSpace = 240;

    $('.admin_errors').css('height', availableSpace + 'px');
}

function displayError(errorId, message, stackTrace) {
        
    var $error = $('#divError' + errorId);
    var $detail = $error.find('.admin_error_detail');

    var $message = $detail.find('.admin_error_detail_message');
    message = replaceAll(message, '&lt;', '<');
    message = replaceAll(message, '&gt;', '>');
    $message.html(message);

    var $stackTrace = $detail.find('.admin_error_detail_stack_trace');
    stackTrace = replaceAll(stackTrace, '&lt;', '<');
    stackTrace = replaceAll(stackTrace, '&gt;', '>');
    $stackTrace.html(stackTrace);

    var $showHideStackTrace = $detail.find('.admin_error_detail_show_stack_trace');
    $showHideStackTrace.html('Show stack trace...');

    $showHideStackTrace.on('click', function () {
        $stackTrace.slideToggle({
            complete: function () {
                if ($showHideStackTrace.html() == 'Show stack trace...')
                    $showHideStackTrace.html('Hide stack trace...');
                else
                    $showHideStackTrace.html('Show stack trace...');
            }
        });
    });
}

function deleteError(errorId, actionUrl) {

    var $error = $('#divError' + errorId);

    setElementDisabled($error.find('.admin_error_delete_button'), true);

    $error.find('.admin_error_options_loading').loading({
        loadingImage: '../../Content/Images/loading.gif',
        successImage: '../../Content/Images/tick.png',
        errorImage: '../../Content/Images/cross.png',
        load: function (onSuccess, onError) {
            $.ajax({
                url: actionUrl,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=uft-8",
                data: JSON.stringify({
                    errorId: errorId
                }),
                success: function (data) {
                    onSuccess(data);
                },
                error: function (response, textStatus, errorThrown) {
                    alert(JSON.stringify(response));
                    onError(response, textStatus, errorThrown);
                }
            });
        },
        success: function (data) {
            $error.remove();

            if ($('.admin_error').length == 0) {
                $('.admin_errors').append('<p>No errors to show.</p>');
            }
        },
        error: function (response, textStatus, errorThrown) {
            setElementDisabled($('.admin_error_delete_button'), false);
        },
        errorClick: function () {
            $('#divCouldNotDeleteError').dialog({
                dialogClass: "no-close",
                modal: true,
                width: "300px",
                buttons: [
                    {
                        text: 'OK',
                        click: function () {
                            $error.find('.admin_error_options_loading').empty();
                            $(this).dialog("close");
                        }
                    }
                ]

            });
        }
    });
}