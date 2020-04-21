function setInviteGuestAttending(guestId, attending) {

    var divAttendingCell = $('#divAttendingCell' + guestId);
    var divLoadingCell = $('#divLoadingCell' + guestId);
    var divGuestPlusOneAttendingCell = $('#divGuestPlusOneAttendingCell' + guestId);

    //disable the cell so the user can't change the attending option while the web service call is being made
    setElementDisabled(divAttendingCell, true);
    setElementDisabled(divGuestPlusOneAttendingCell, true);

    divLoadingCell.loading({
        loadingImage: '../../Content/Images/loading.gif',
        successImage: '../../Content/Images/tick.png',
        errorImage: '../../Content/Images/cross.png',
        load: function (onSuccess, onError) {

            $.ajax({
                url: 'RSVP/SetGuestAttending',
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=uft-8",
                data: JSON.stringify({
                    guestId: guestId,
                    attending: attending
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
            setElementDisabled(divAttendingCell, false);
            setElementDisabled(divGuestPlusOneAttendingCell, false);

            document.getElementById('spanGuest' + guestId).classList.remove('rsvp_invite_guest_name_no_rsvp');

            if (attending) {
                $('#trGuestPlusOneAttendingCell' + guestId).show();
            }
            else {
                $('#trGuestPlusOneAttendingCell' + guestId).hide();
            }
        },
        error: function (response, textStatus, errorThrown) {
            setElementDisabled(divAttendingCell, false);
            setElementDisabled(divGuestPlusOneAttendingCell, false);
        }
    });
}

function showHidePlusOneName(guestId, show) {
    if (show) {
        editGuestPlusOneDetails(guestId, '', '', true);
    }
    else {
        setGuestPlusOneDetails(guestId, false, '', '');
    }
}

function editGuestPlusOneDetails(guestId, plusOneForename, plusOneSurname, untickIsBringingPlusOneOnCancel) {

    if (document.getElementById('divGuestPlusOneAttendingCell' + guestId).hasAttribute('disabled'))
        return;

    $('#txtPlusOneForename').val(plusOneForename);
    $('#txtPlusOneSurname').val(plusOneSurname);
    $('#lblEditGuestPlusOneDetailsError').html('');

    $('#divEditGuestPlusOneDetails').dialog({
        dialogClass: "no-close",
        modal: true,
        width: "322px",
        buttons: [
            {
                text: "OK",
                click: function () {

                    $('#lblEditGuestPlusOneDetailsError').html('');

                    plusOneForename = $('#txtPlusOneForename').val();

                    if (plusOneForename == '') {
                        $('#lblEditGuestPlusOneDetailsError').html('Please enter the forename of your guest.');
                        $('#txtPlusOneForename').focus();
                        return;
                    }

                    plusOneSurname = $('#txtPlusOneSurname').val();

                    if (plusOneSurname == '') {
                        $('#lblEditGuestPlusOneDetailsError').html('Please enter the surname of your guest.');
                        $('#txtPlusOneSurname').focus();
                        return;
                    }

                    var divGuestPlusOneAttendingCell = $('#divGuestPlusOneAttendingCell' + guestId);
                    var divGuestPlusOneLoadingCell = $('#divGuestPlusOneLoadingCell' + guestId);

                    //disable the cell so the user can't change the attending option while the web service call is being made
                    setElementDisabled(divGuestPlusOneAttendingCell, true);

                    var isBringingPlusOne = $('#chkIsBringingPlusOne' + guestId)[0].checked;

                    divGuestPlusOneLoadingCell.loading({
                        loadingImage: '../../Content/Images/loading.gif',
                        successImage: '../../Content/Images/tick.png',
                        errorImage: '../../Content/Images/cross.png',
                        load: function (onSuccess, onError) {
                            $.ajax({
                                url: 'RSVP/SetGuestPlusOneDetails',
                                type: "POST",
                                dataType: "json",
                                contentType: "application/json; charset=uft-8",
                                data: JSON.stringify({
                                    guestId: guestId,
                                    isBringingPlusOne: isBringingPlusOne,
                                    plusOneForename: plusOneForename,
                                    plusOneSurname: plusOneSurname
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

                            $('#divGuestPlusOneName' + guestId).attr('data-plusOneForename', plusOneForename);
                            $('#divGuestPlusOneName' + guestId).attr('data-plusOneSurname', plusOneSurname);

                            var plusOneFullName = (plusOneForename + ' ' + plusOneSurname).trim();

                            $('#divGuestPlusOneName' + guestId).html(plusOneFullName);

                            setElementDisabled(divGuestPlusOneAttendingCell, false);
                        },
                        error: function (response, textStatus, errorThrown) {

                            checkBox($('#chkIsBringingPlusOne' + guestId), !isBringingPlusOne);
                            setElementDisabled(divGuestPlusOneAttendingCell, false);
                        }
                    });

                    $(this).dialog('close');
                }
            },
            {
                text: "Cancel",
                click: function () {
                    $(this).dialog('close');

                    if (untickIsBringingPlusOneOnCancel)
                        checkBox($('#chkIsBringingPlusOne' + guestId), false);
                }
            }
        ]
    });
}