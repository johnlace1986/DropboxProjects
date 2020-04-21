(function ($) {

    $.fn.messages = function (options) {

        return $(this).each(function (index, container) {

            var actualOptions = $.extend({
                inviteMessageClass: '',
                adminMessageClass: '',
                load: null,
                refresh: null,
                refreshInterval: 5,
                textBox: null,
                watermark: 'Enter text here...',
                watermarkClass: null,
                sendButton: null,
                beginSend: null,
                send: null
            }, options);

            if (actualOptions.textBox) {

                if (actualOptions.watermark && $(actualOptions.textBox).watermark) {
                    $(actualOptions.textBox).watermark(actualOptions.watermark, { useNative: false, className: actualOptions.watermarkClass });
                }

                if (actualOptions.sendButton) {
                    $(actualOptions.sendButton).off('click').on('click', function () {

                        var messageBody = $(actualOptions.textBox).val().trim();

                        if (messageBody != '') {

                            if (actualOptions.beginSend) {
                                actualOptions.beginSend(function () {
                                    if (actualOptions.send) {
                                        actualOptions.send(
                                            messageBody,
                                            function (data, textStatus, response) {
                                                displayMessage(container, data, actualOptions.inviteMessageClass, actualOptions.adminMessageClass, 1000);
                                            });
                                    }
                                });
                            }
                        }
                    });
                }
            }

            if (actualOptions.load) {
                actualOptions.load(
                    function (data, textStatus, response) {
                        $.each(data, function (index, message) {
                            displayMessage(container, message, actualOptions.inviteMessageClass, actualOptions.adminMessageClass, 0);
                        });

                        refreshMessages(actualOptions.refresh, actualOptions.refreshInterval * 1000, container, actualOptions.inviteMessageClass, actualOptions.adminMessageClass);
                    },
                    function (response, textStatus, errorThrown) {
                        if (actualOptions.loadError)
                            actualOptions.loadError(response, textStatus, errorThrown);
                    });
            }
        });
    }

    function displayMessage(container, message, inviteMessageClass, adminMessageClass, fadeInterval) {

        message.Body = makeStringWebSafe(message.Body);

        var messageSenderEnumInvite = 0;
        var messageSenderEnumAdmin = 1;

        var messageClass = '';

        switch (message.Sender) {
            case messageSenderEnumInvite:
                messageClass = inviteMessageClass;
                break;

            case messageSenderEnumAdmin:
                messageClass = adminMessageClass;
                break;
        }

        var dateSent = getDateFromServerParts(message.DateSent);

        var newMessage = $(container).append(
            '<div>' +
                '<div id="divMessage' + message.Id + '" class="' + messageClass + '">' +
                    '<div class="message_body">' +
                        '<div class="messages_message">' + message.Body + '</div>' +
                    '</div>' +
                    '<div id="divMessageDateSent' + message.Id + '" class="messages_date_sent">' + formatUtcDate(dateSent) + '</div>' +
                '</div>' +
            '</div>');

        if (fadeInterval == 0) {
            $(container).scrollTop($(container)[0].scrollHeight);
        }
        else {
            $('#divMessage' + message.Id).css('opacity', '0');
            $('#divMessage' + message.Id).hide();

            $('#divMessage' + message.Id).show("slide", { direction: "up" }, fadeInterval);

            $('#divMessage' + message.Id).animate({
                opacity: 1
            }, fadeInterval);

            $(container).animate({
                scrollTop: $(container)[0].scrollHeight
            }, fadeInterval);
        }
    }
    
    function refreshMessages(refresh, interval, container, inviteMessageClass, adminMessageClass, onError) {
        
        if (refresh) {
            setTimeout(function () {
                refresh(
                    function (data, textStatus, response) {
                        $.each(data, function (index, message) {
                            displayMessage(container, message, inviteMessageClass, adminMessageClass, 1000);
                        });

                        refreshMessages(refresh, interval, container, inviteMessageClass, adminMessageClass)
                    });
            }, interval);
        }
    }

})(jQuery);
