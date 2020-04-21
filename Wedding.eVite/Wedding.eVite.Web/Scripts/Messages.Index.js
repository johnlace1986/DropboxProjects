function setMessagesContainerHeight() {
    var availableSpace = getWindowHeight() - 24;

    availableSpace -= $('.layout_header').height() + 12;        //12px top padding
    availableSpace -= $('.layout_navigation').height() + 72;    //48px vertical padding + 24px vertical margin
    availableSpace -= $('.logged_in_layout_footer').height() + 96;        //48px vertical padding + 48px vertical margin
    availableSpace -= $('.messages_new_message').height() + 26; //24px top margin + 2px mystery

    availableSpace -= 24; //messages container padding

    if (availableSpace < 240)
        availableSpace = 240;

    $('.messages_container').css('height', availableSpace + 'px');
}

function initMessages(inviteId) {
    $('.messages_container').messages({
        inviteMessageClass: 'messages_received_message',
        adminMessageClass: 'messages_sent_message',
        load: function (onSuccess) {
            loadMessages(inviteId, onSuccess);
        },
        refresh: function (onSuccess) {
            refreshMessages(inviteId, onSuccess);
        },
        textBox: $('#txtNewMessage'),
        watermark: 'Write your message here...',
        watermarkClass: 'watermark',
        sendButton: $('#btnSendMessage'),
        beginSend: function (onSuccess) {
            $('.message_new_message_error').html('');
            onSuccess();
        },
        send: function (messageBody, onSuccess) {
            sendMessage(inviteId, messageBody, onSuccess);
        }
    });
}

function loadMessages(inviteId, onSuccess) {
    $.ajax({
        url: 'Messages/GetMessages',
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            inviteId: inviteId
        }),
        contentType: "application/json; charset=uft-8",
        success: function (data) {
            onSuccess(data);
        },
        error: function (response, textStatus, errorThrown) {
            $('.message_new_message_error').html('Unable to get messages. Please try again later.');

            setElementDisabled($('#txtNewMessage'), true);
            setElementDisabled($('#btnSendMessage'), true);
            setElementDisabled($('.message_new_message_email_messages'), true);
        }
    });
}

function refreshMessages(inviteId, onSuccess) {
    $.ajax({
        url: 'Messages/GetUnreadMessages',
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            inviteId: inviteId
        }),
        contentType: "application/json; charset=uft-8",
        success: function (data) {
            onSuccess(data);
        },
        error: function (response, textStatus, errorThrown) {

            $('.message_new_message_error').html('Unable to get messages. Please try again later.');

            setElementDisabled($('#txtNewMessage'), true);
            setElementDisabled($('#btnSendMessage'), true);
            setElementDisabled($('.message_new_message_email_messages'), true);
        }
    });
}

function sendMessage(inviteId, messageBody, onSuccess) {

    setElementDisabled($('#txtNewMessage'), true);
    setElementDisabled($('#btnSendMessage'), true);
    setElementDisabled($('.message_new_message_email_messages'), true);

    $('.messages_new_message_loading').loading({
        loadingImage: '../../Content/Images/loading.gif',
        successImage: '../../Content/Images/tick.png',
        errorImage: '../../Content/Images/cross.png',
        load: function (onSendSuccess, onSendError) {
            sendMessageFromInvite(inviteId, messageBody, onSendSuccess, onSendError);
        },
        beginSuccess: function (data) {
            onSuccess(data);

            setElementDisabled($('#txtNewMessage'), false);
            setElementDisabled($('#btnSendMessage'), false);
            setElementDisabled($('.message_new_message_email_messages'), false);

            $('#txtNewMessage').val('');
            $('#txtNewMessage').focus();
        },
        error: function (response, textStatus, errorThrown) {
            $('.message_new_message_error').html('Unable to send message. Please try again later.');
        }
    });
}

function sendMessageFromInvite(inviteId, messageBody, onSuccess, onError) {

    $.ajax({
        url: 'Messages/SendMessageFromInvite',
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=uft-8",
        data: JSON.stringify({
            inviteId: inviteId,
            body: messageBody
        }),
        success: function (data) {
            onSuccess(data);
        },
        error: function (response, textStatus, errorThrown) {
            onError(response, textStatus, errorThrown);
        }
    });
}

function setInviteEmailMessages(emailMessages) {

    $('.message_new_message_error').html('');

    setElementDisabled($('#txtNewMessage'), true);
    setElementDisabled($('#btnSendMessage'), true);
    setElementDisabled($('.message_new_message_email_messages'), true);

    $('.messages_new_message_loading').loading({
        loadingImage: '../../Content/Images/loading.gif',
        successImage: '../../Content/Images/tick.png',
        errorImage: '../../Content/Images/cross.png',
        load: function (onSuccess, onError) {
            $.ajax({
                url: 'Messages/SetInviteEmailMessages',
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=uft-8",
                data: JSON.stringify({
                    emailMessages: emailMessages
                }),
                success: function (data) {
                    onSuccess(data);
                },
                error: function (response, textStatus, errorThrown) {
                    onError(response, textStatus, errorThrown);
                }
            });
        },
        beginSuccess: function (data) {

            setElementDisabled($('#txtNewMessage'), false);
            setElementDisabled($('#btnSendMessage'), false);
            setElementDisabled($('.message_new_message_email_messages'), false);
        },
        error: function (response, textStatus, errorThrown) {
            $('.message_new_message_error').html('An error has occurred. Please try again later.');
        }
    });
}
