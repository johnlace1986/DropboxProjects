var EMAIL_TYPE_NONE;
var EMAIL_TYPE_INVITE;
var EMAIL_TYPE_UPDATE;

var isAdmin;

function setInviteContainerHeight() {
    var availableSpace = getWindowHeight() - 24;    //vertical padding

    availableSpace -= $('.admin_layout_navigation').height() + 72;    //48px vertical padding + 24px vertical margin
    availableSpace -= $('.logged_in_layout_footer').height() + 96;        //48px vertical padding + 48px vertical margin
    availableSpace -= $('.admin_invite_details').height() + 18;     //18px bottom margin
    availableSpace -= $('.admin_invite_options').height() + 24;   //12px top margin

    availableSpace -= 26; //vertical padding on the guests container + 1px top and bottom border

    if (availableSpace < 240)
        availableSpace = 240;

    $('.admin_invite_guests').css('height', availableSpace + 'px');
}

function isAdminChanged(newIsAdmin) {

    isAdmin = newIsAdmin;

    if (isAdmin) {
        setElementDisabled($('#chkReserveSandholeRoom'), true);
        checkBox($('#chkReserveSandholeRoom'), true);

        setElementDisabled($('input[disableForAdmin]'), true);
        checkBox($('input[disableForAdmin]'), false);
    }
    else {
        setElementDisabled($('#chkReserveSandholeRoom'), false);

        setElementDisabled($('input[disableForAdmin]'), false);
    }
}

function addGuest(guestId, forename, surname, isAttending, dateOfRsvp, isChild, isVegetarian, canBringPlusOne, tableId, roomId, notes) {

    var index = $('#tblGuests').find('tr[data-guestId]').length;

	$tr = $(
		'<tr data-guestId="' + guestId + '" data-index="' + index + '" data-isAttending="' + (isAttending == null ? 'NA' : isAttending) + '" data-dateOfRsvp="' + (dateOfRsvp == null ? 'NA' : dateOfRsvp.toString()) + '" data-tableId="' + (tableId == null ? 'NA' : tableId) + '" data-roomId="' + (roomId == null ? 'NA' : roomId) + '">' +
			'<td><input type="text" class="textbox" forename value="' + forename + '" /></td>' +
			'<td><input type="text" class="textbox" surname value="' + surname + '" /></td>' +
			'<td><input type="checkbox" isChild' + (isChild ? ' checked' : '') + (isAdmin ? ' disabled' : '') + ' disableForAdmin /></td>' +
			'<td><input type="checkbox" isVegetarian' + (isVegetarian ? ' checked' : '') + ' /></td>' +
			'<td><input type="checkbox" canBringPlusOne' + (canBringPlusOne ? ' checked' : '') + (isAdmin ? ' disabled' : '') + ' disableForAdmin /></td>' +
			'<td style="padding: 0px 6px;"><input type="button" class="button admin_invite_guests_notes_button" title="Notes" onclick="editNotes(' + index + ');" /></td>' +
			'<td style="padding: 0px 6px;"><input type="button" class="button admin_invite_guests_delete_button" title="Delete Guest" onclick="deleteGuest(' + index + ');" /></td>' +
		'</tr>');

	$tr.data('notes', notes);

	$tr.insertBefore($('.admin_invite_guests_new_guest_row'));
}

function editNotes(index) {

    var $row;
    
    if (index == -1)
        $row = $('#trNewGuest');
    else
        $row = $('#tblGuests').find('tr[data-index=' + index + ']');

    $('#txtNotes').val($row.data('notes'));

    $('#divNotes').dialog({
        dialogClass: "no-close",
        modal: true,
        width: "300px",
        buttons: [
            {
                text: 'OK',
                click: function () {

                    $row.data('notes', $('#txtNotes').val());
                    $(this).dialog("close");
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

function deleteGuest(index) {

	$('#divDeleteGuest').dialog({
		dialogClass: "no-close",
		modal: true,
		width: "300px",
		buttons: [
            {
            	text: 'OK',
            	click: function () {

            	    var $row = $('#tblGuests').find('tr[data-index=' + index + ']');
            		$row.remove();

            		$(this).dialog("close");
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

function addNewGuestFromKeyboard(keyCode) {
    if (keyCode == 13) {
        if (addNewGuest())
            $('#txtForenameNew').focus();
    }
}

function addNewGuest() {

    $('.admin_invite_options_error').html('');

    var guestId = -1;
    var forename = $('#txtForenameNew').val();
    var surname = $('#txtSurnameNew').val();
    var isChild = $('#chkIsChildNew')[0].checked;
    var isVegetarian = $('#chkIsVegetarianNew')[0].checked;
    var canBringPlusOne = $('#chkCanBringPlusOne')[0].checked;

    if (forename == '') {
        $('.admin_invite_options_error').html('Please enter the forename of the guest.');
        $('#txtForenameNew').focus();
        return false;
    }

    if (surname == '') {
        $('.admin_invite_options_error').html('Please enter the surname of the guest.');
        $('#txtSurnameNew').focus();
        return false;
    }

    var notes = $('#trNewGuest').data('notes');

    if (!notes)
        notes = '';

    addGuest(guestId, forename, surname, null, null, isChild, isVegetarian, canBringPlusOne, null, null, notes);

    $('#txtForenameNew').val('');
    $('#txtSurnameNew').val('');
    checkBox($('#chkIsChildNew'), false);
    checkBox($('#chkIsVegetarianNew'), false);
    checkBox($('#chkCanBringPlusOne'), false);
    $('#trNewGuest').data('notes', '');

    return true;
}

function saveInvite(inviteId, actionUrl, onFinished) {

    $('.admin_invite_options_error').html('');

    var emailAddress = $('#txtEmailAddress').val().trim().toLowerCase();

    if (emailAddress == '') {
        $('.admin_invite_options_error').html('Please enter the email address.');
        $('#txtEmailAddress').focus();
        return;
    }

    var includesCeremony = $('#chkIncludesCeremony')[0].checked;
    var reserveSandholeRoom = $('#chkReserveSandholeRoom')[0].checked;
    var isAdmin = $('#chkIsAdmin')[0].checked;
    
    if (($('#txtForenameNew').val() != '') ||
        ($('#txtSurnameNew').val() != '')) {

        if (!addNewGuest()) {
            return;
        }
    }

    var guests = [];
    var valid = true;

    $('#tblGuests').find('tr[data-guestId]').each(function (index, tr) {

        var guestId = parseInt($(tr).attr('data-guestId'));
        
        var dateOfRsvp;
        
        if ($(tr).attr('data-dateOfRsvp') == 'NA')
            dateOfRsvp = null;
        else {
            dateOfRsvp = new Date($(tr).attr('data-dateOfRsvp'));
        }

        var isAttending;

        if (isAdmin) {
            isAttending = true;

            if (!dateOfRsvp)
                dateOfRsvp = new Date();
        }
        else {
            if ($(tr).attr('data-isAttending') == 'NA')
                isAttending = null;
            else {
                isAttending = $(tr).attr('data-isAttending') == 'true';
            }
        }

        $txtForename = $(tr).find('input[type="text"][forename]');

        if ($txtForename.val() == '') {
            $('.admin_invite_options_error').html('Please enter the forename of the guest.');
            $txtForename.focus();
            valid = false;
            return false;
        }

        $txtSurname = $(tr).find('input[type="text"][surname]');

        if ($txtSurname.val() == '') {
            $('.admin_invite_options_error').html('Please enter the surname of the guest.');
            $txtSurname.focus();
            valid = false;
            return false;
        }

        var isChild = $(tr).find('input[type="checkbox"][isChild]')[0].checked;
        var isVegetarian = $(tr).find('input[type="checkbox"][isVegetarian]')[0].checked;
        var canBringPlusOne = $(tr).find('input[type="checkbox"][canBringPlusOne]')[0].checked;

        var tableId;

        if ($(tr).attr('data-tableId') == 'NA')
            tableId = null;
        else {
            tableId = parseInt($(tr).attr('data-tableId'));
        }

        var roomId;

        if ($(tr).attr('data-roomId') == 'NA')
            roomId = null;
        else {
            roomId = parseInt($(tr).attr('data-roomId'));
        }

        var notes = $(tr).data('notes');

        guests.push({
            guestId: guestId,
            forename: $txtForename.val(),
            surname: $txtSurname.val(),
            isAttending: isAttending,
            dateOfRsvp: (dateOfRsvp ? dateOfRsvp.toUTCString() : null),
            isChild: isChild,
            isVegetarian: isVegetarian,
            canBringPlusOne: canBringPlusOne,
            tableId: tableId,
            roomId: roomId,
            notes: notes
        });
    });

    if (guests.length == 0) {
        $('.admin_invite_options_error').html('Please add at least one guest to the invite.');
        return;
    }

    if (isAdmin) {
        if (guests.length != 1) {
            $('.admin_invite_options_error').html('Ad admin invitation can only have one guest.');
            return;
        }
    }

    if (valid) {
        if (inviteId == -1) {
            saveInviteAfterWarning(inviteId, emailAddress, includesCeremony, reserveSandholeRoom, isAdmin, guests, EMAIL_TYPE_INVITE, actionUrl, onFinished);
        }
        else {
            $('#divSendEmail').dialog({
                dialogClass: "no-close",
                modal: true,
                width: "394px",
                buttons: [
                    {
                        text: 'Send Invite Email',
                        width: '163px',
                        click: function () {
                            $(this).dialog("close");
                            saveInviteAfterWarning(inviteId, emailAddress, includesCeremony, reserveSandholeRoom, isAdmin, guests, EMAIL_TYPE_INVITE, actionUrl, onFinished);
                        }
                    },
                    {
                        text: 'Send Update Email',
                        width: '163px',
                        click: function () {
                            $(this).dialog("close");
                            saveInviteAfterWarning(inviteId, emailAddress, includesCeremony, reserveSandholeRoom, isAdmin, guests, EMAIL_TYPE_UPDATE, actionUrl, onFinished);
                        }
                    },
                    {
                        text: 'Don\'t Send Email',
                        width: '163px',
                        click: function () {
                            $(this).dialog("close");
                            saveInviteAfterWarning(inviteId, emailAddress, includesCeremony, reserveSandholeRoom, isAdmin, guests, EMAIL_TYPE_NONE, actionUrl, onFinished);
                        }
                    },
                    {
                        text: 'Cancel',
                        width: '163px',
                        click: function () {
                            $(this).dialog("close");
                        }
                    }
                ]
            });
        }
    }
}

function saveInviteAfterWarning(inviteId, emailAddress, includesCeremony, reserveSandholeRoom, isAdmin, guests, emailType, actionUrl, onFinished) {

    setElementDisabled($('.admin_invite_details'), true);
    setElementDisabled($('.admin_invite_guests'), true);
    setElementDisabled($('.admin_invite_options').find('input[type=button]'), true);

    $('.admin_invite_options_loading').loading({
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
                    inviteId: inviteId,
                    emailAddress: emailAddress,
                    includesCeremony: includesCeremony,
                    reserveSandholeRoom: reserveSandholeRoom,
                    isAdmin: isAdmin,
                    guests: JSON.stringify(guests),
                    emailType: emailType
                }),
                success: function (data) {
                    if (data.result) {
                        onSuccess(data);
                    }
                    else {
                        onError(null, null, data.errorMessage);
                    }
                },
                error: function (response, textStatus, errorThrown) {
                    onError(response, textStatus, errorThrown);
                }
            });
        },
        success: function (data) {
            setElementDisabled($('.admin_invite_details'), false);
            setElementDisabled($('.admin_invite_guests'), false);
            setElementDisabled($('.admin_invite_options').find('input[type=button]'), false);

            if (onFinished)
                onFinished();
        },
        error: function (response, textStatus, errorThrown) {

            if (!errorThrown || errorThrown == '') {
                errorThrown = 'Unable to save invite. Please check errors.';
            }

            setElementDisabled($('.admin_invite_details'), false);
            setElementDisabled($('.admin_invite_guests'), false);
            setElementDisabled($('.admin_invite_options').find('input[type=button]'), false);

            $('.admin_invite_options_error').html(errorThrown);
        }
    });
}