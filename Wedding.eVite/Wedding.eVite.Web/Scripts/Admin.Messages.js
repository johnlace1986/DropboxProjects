function setMessagesContainerHeight() {
    var availableSpace = getWindowHeight() - 24;    //vertical padding

    availableSpace -= $('.admin_layout_navigation').height() + 72;    //48px vertical padding + 24px vertical margin
    availableSpace -= $('.logged_in_layout_footer').height() + 96;        //48px vertical padding + 48px vertical margin
    availableSpace -= $('.admin_messages_recipient').height() + 24;     //12px bottom margin
    availableSpace -= $('.messages_new_message').height() + 26; //24px top margin + 2px mystery

    availableSpace -= 48; //vertical padding on the messages container

    if (availableSpace < 240)
        availableSpace = 240;

    $('.messages_container').css('height', availableSpace + 'px');
}

function initMessages(inviteId) {
    $('.messages_container').messages({
        inviteMessageClass: 'messages_sent_message',
        adminMessageClass: 'messages_received_message',
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
        url: '/Admin/GetMessages',
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
        url: '/Admin/GetUnreadMessages',
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
        url: '/Admin/SendMessageFromInvite',
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
                url: '/Admin/SetInviteEmailMessages',
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

