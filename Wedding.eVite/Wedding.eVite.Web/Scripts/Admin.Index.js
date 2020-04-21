function setInvitesContainerHeight() {
    var availableSpace = getWindowHeight() - 24;

    availableSpace -= $('.admin_layout_navigation').height() + 72;            //48px vertical padding + 24px vertical margin
    availableSpace -= $('.logged_in_layout_footer').height() + 96;      //48px vertical padding + 48px vertical margin
    availableSpace -= $('.admin_index_invite_summary').height() + 24;   //24px bottom margin
    availableSpace -= $('.admin_index_add_invite').height();

    availableSpace -= 50;   //12px vertical padding, 1px vertical border and 24px bottom margin

    if (availableSpace < 240)
        availableSpace = 240;

    $('.admin_index_invites').css('height', availableSpace + 'px');
}

function loadSummary() {
    $.ajax({
        url: 'Admin/GetInvitesSummary',
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=uft-8",
        success: function (data) {

            $('.admin_index_invite_summary').find('[data-totalGuests]').html(data.totalGuests);
            $('.admin_index_invite_summary').find('[data-guestsAttending]').html(data.guestsAttending);
            $('.admin_index_invite_summary').find('[data-guestsNotAttending]').html(data.guestsNotAttending);
            $('.admin_index_invite_summary').find('[data-guestsNoRSVP]').html(data.guestsNoRSVP);
        },
        error: function (response, textStatus, errorThrown) {
        }
    });
}

function filterInvites(filter) {
    $('.admin_index_invites').find('tbody tr').each(function (trIndex, tr) {
        if (filter == '' || (tr.outerText.toLowerCase().indexOf(filter.toLowerCase()) > -1)) {
            $(tr).show();
        }
        else {
            $(tr).hide();
        }
    });
}

function sendInviteEmail(inviteId) {
    
    $('.admin_index_send_email_message').html('Sending email...');

    $('#divSendEmail').dialog({
        dialogClass: "no-close",
        modal: true,
        width: "420px",
        open: function () {
            
            setElementDisabled($('div[aria-describedby="divSendEmail"]').find('.ui-dialog-buttonset'), true);

            $('.admin_index_send_email_loading').loading({
                loadingImage: '../../Content/Images/loading.gif',
                successImage: '../../Content/Images/tick.png',
                errorImage: '../../Content/Images/cross.png',
                load: function (onSuccess, onError) {
                    $.ajax({
                        url: 'Admin/SendInviteEmail',
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=uft-8",
                        data: JSON.stringify({
                            inviteId: inviteId
                        }),
                        success: function (data) {
                            onSuccess(data);
                        },
                        error: function (response, textStatus, errorThrown) {
                            onError(response, textStatus, errorThrown);
                        }
                    });
                },
                success: function (data) {
                    $('#divSendEmail').dialog("close");
                },
                error: function () {
                    
                    setElementDisabled($('div[aria-describedby="divSendEmail"]').find('.ui-dialog-buttonset'), false);
                    $('.admin_index_send_email_message').html('Unable to send email. Please check errors.');
                }
            });
        },
        buttons: [
            {
                text: 'OK',
                click: function () {
                    $('#divSendEmail').dialog("close");
                }
            }
        ]
    });
}

function deleteInvite(inviteId, guestList) {

    $('#lblDeleteInviteName').text(guestList);

    $('#divDeleteInvite').dialog({
        dialogClass: "no-close",
        modal: true,
        width: "420px",
        buttons: [
            {
                text: 'OK',
                click: function () {

                    setElementDisabled($('div[aria-describedby="divDeleteInvite"]').find('.ui-dialog-buttonset'), true);

                    $('.admin_index_delete_invite_loading').loading({
                        loadingImage: '../../Content/Images/loading.gif',
                        successImage: '../../Content/Images/tick.png',
                        errorImage: '../../Content/Images/cross.png',
                        load: function (onSuccess, onError) {
                            $.ajax({
                                url: 'Admin/DeleteInvite',
                                type: "POST",
                                dataType: "json",
                                contentType: "application/json; charset=uft-8",
                                data: JSON.stringify({
                                    inviteId: inviteId
                                }),
                                success: function (data) {
                                    onSuccess(data);
                                },
                                error: function (response, textStatus, errorThrown) {
                                    onError(response, textStatus, errorThrown);
                                }
                            });
                        },
                        success: function (data) {
                            $('#trInvite' + inviteId).remove();
                            $('#divDeleteInvite').dialog("close");

                            loadSummary();
                        },
                        error: function (response, textStatus, errorThrown) {
                            setElementDisabled($('div[aria-describedby="divDeleteInvite"]').find('.ui-dialog-buttonset'), false);
                        },
                        errorClick: function () {
                            $('#divError').dialog({
                                dialogClass: "no-close",
                                modal: true,
                                width: "300px",
                                buttons: [
                                    {
                                        text: 'OK',
                                        click: function () {
                                            $(this).dialog("close");
                                        }
                                    }
                                ]
                            });
                        }
                    });
                }
            },
            {
                text: 'Cancel',
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}

function sendGiftWebsiteEmails() {
    $('#divSendGiftWebsiteEmails').dialog({
        dialogClass: "no-close",
        modal: true,
        width: "420px",
        buttons: [
            {
                text: 'OK',
                click: function () {
                    setElementDisabled($('div[aria-describedby="divSendGiftWebsiteEmails"]').find('.ui-dialog-buttonset'), true);

                    $('.admin_index_send_gift_notification_emails_loading').loading({
                        loadingImage: '../../Content/Images/loading.gif',
                        successImage: '../../Content/Images/tick.png',
                        errorImage: '../../Content/Images/cross.png',
                        load: function (onSuccess, onError) {
                            $.ajax({
                                url: 'Admin/SendGiftWebsiteEmails',
                                type: "POST",
                                dataType: "json",
                                contentType: "application/json; charset=uft-8",
                                success: function (data) {
                                    onSuccess(data);
                                },
                                error: function (response, textStatus, errorThrown) {
                                    onError(response, textStatus, errorThrown);
                                }
                            });
                        },
                        success: function (data) {
                            $('#divSendGiftWebsiteEmails').dialog("close");
                        },
                        error: function (response, textStatus, errorThrown) {
                            setElementDisabled($('div[aria-describedby="divSendGiftWebsiteEmails"]').find('.ui-dialog-buttonset'), false);
                        },
                        errorClick: function () {
                            $('#divError').dialog({
                                dialogClass: "no-close",
                                modal: true,
                                width: "300px",
                                buttons: [
                                    {
                                        text: 'OK',
                                        click: function () {
                                            $(this).dialog("close");
                                        }
                                    }
                                ]
                            });
                        }
                    });
                }
            },
            {
                text: 'Cancel',
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
}