$(document).ready(function () {

    setGuestsContainerHeight();

    $('.admin_guests_filter').find('input[type=radio]').on('click', function () {
        filterGuests();
    });

    $('#txtFilter').watermark('Filter...', { useNative: false, className: 'watermark' });
    filterGuests();

    $('td[data-dateOfRsvpYear]').each(function (cellIndex, cell) {
        var year = parseInt($(cell).attr('data-dateOfRsvpYear'));
        var month = parseInt($(cell).attr('data-dateOfRsvpMonth')) - 1;
        var day = parseInt($(cell).attr('data-dateOfRsvpDay'));
        var hour = parseInt($(cell).attr('data-dateOfRsvpHour'));
        var minute = parseInt($(cell).attr('data-dateOfRsvpMinute'));
        var second = parseInt($(cell).attr('data-dateOfRsvpSecond'));

        var dateOfRsvp = new Date(year, month, day, hour, minute, second);
        $(cell).html(formatUtcDate(dateOfRsvp));
    });

    $('td[data-isAttending]').each(function (cellIndex, cell) {

        var isAttending = $(cell).attr('data-isAttending');

        if (isAttending != '') {
            displayGuestIsAttending(cell, isAttending == 'true');
        }
    });

    $('#tblGuests').tablesorter({
        textExtraction: {
            3: function (node, table, cellIndex) {

                if (node.hasAttribute('data-dateOfRsvpYear')) {
                    var year = $(node).attr('data-dateOfRsvpYear');
                    var month = $(node).attr('data-dateOfRsvpMonth');
                    var day = $(node).attr('data-dateOfRsvpDay');
                    var hour = $(node).attr('data-dateOfRsvpHour');
                    var minute = $(node).attr('data-dateOfRsvpMinute');
                    var second = $(node).attr('data-dateOfRsvpSecond');

                    return year + '-' + month + '-' + day + ' ' + hour + ':' + minute + ':' + second;
                }

                return node.innerHTML;
            }
        },
        cancelSelection: false,
        sortList: [[1, 0]],
        headers: {
            0: {
                sorter: false
            },
            4: {
                sorter: false
            },
            5: {
                sorter: false
            },
            6: {
                sorter: false
            },
            7: {
                sorter: false
            },
            8: {
                sorter: false
            }
        }
    });

    window.onresize = function () {
        setGuestsContainerHeight();
    };
});

function displayGuestIsAttending(cell, isAttending) {
    if (isAttending) {
        $(cell).html('<img src="../../Content/Images/tick.png" title="Guest is attending" />');
    }
    else {
        $(cell).html('<img src="../../Content/Images/cross.png" title="Guest is not attending" />');
    }
}

function setGuestsContainerHeight() {
    var availableSpace = getWindowHeight() - 24;    //vertical padding

    availableSpace -= $('.admin_layout_navigation').height() + 72;    //48px vertical padding + 24px vertical margin
    availableSpace -= $('.logged_in_layout_footer').height() + 96;        //48px vertical padding + 48px vertical margin
    availableSpace -= $('.admin_guests_filter').height() + 12;     //12px bottom margin
    availableSpace -= $('.admin_guests_summary').height() + 24;   //24px top margin

    availableSpace -= 26; //vertical padding on the guests container + 1px top and bottom border

    if (availableSpace < 240)
        availableSpace = 240;

    $('.admin_guests_list').css('height', availableSpace + 'px');
}

function filterGuests() {

    var attending = $('.admin_guests_filter').find('input[name=chkAttending]:checked').val();
    var attendingCeremony = $('.admin_guests_filter').find('input[name=chkAttendingCeremony]:checked').val();
    var age = $('.admin_guests_filter').find('input[name=chkAge]:checked').val();
    var vegetarian = $('.admin_guests_filter').find('input[name=chkVegetarian]:checked').val();
    var stayingAtSandhole = $('.admin_guests_filter').find('input[name=chkStayingAtSandhole]:checked').val();
    var filter = $('#txtFilter').val();

    var totalGuests = 0;

    $('#tbGuests').find('tr').each(function (rowIndex, row) {

        if (attending != 'all') {
            if ($(row).attr('data-attending') != attending) {
                $(row).hide();
                return true;
            }
        }

        if (attendingCeremony != 'all') {
            if ($(row).attr('data-attendingCeremony') != attendingCeremony) {
                $(row).hide();
                return true;
            }
        }

        if (age != 'all') {
            if ($(row).attr('data-age') != age) {
                $(row).hide();
                return true;
            }
        }

        if (vegetarian != 'all') {
            if ($(row).attr('data-vegetarian') != vegetarian) {
                $(row).hide();
                return true;
            }
        }

        if (stayingAtSandhole != 'all') {
            if ($(row).attr('data-stayingAtSandhole') != stayingAtSandhole) {
                $(row).hide();
                return true;
            }
        }

        if (filter != '') {
            if (row.outerText.toLowerCase().indexOf(filter.toLowerCase()) == -1) {
                $(row).hide();
                return true;
            }
        }

        $(row).show();
        totalGuests++;
    });

    $('#lblTotalGuests').html(totalGuests);
}

function showNotes(notes) {

    $('#lblNotes').html(notes);

    $('#divNotes').dialog({
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

function rsvpGuests() {

    var guests = [];

    $('.admin_guests_list_guest_checkbox:checked').each(function (checkboxIndex, checkbox) {
        guests.push({
            inviteId: parseInt($(checkbox).attr('data-inviteId')),
            guestId: parseInt($(checkbox).attr('data-guestId'))
        });
    });

    if (guests.length > 0) {

        checkBox($('#rdoRsvpGuestsIsAttendingYes'), true);

        $('#divRsvpGuests').dialog({
            dialogClass: "no-close",
            modal: true,
            width: "300px",
            buttons: [
                {
                    text: 'OK',
                    click: function () {

                        var isAttending = $('#rdoRsvpGuestsIsAttendingYes')[0].checked;

                        $.ajax({
                            url: '/Admin/RsvpGuests',
                            type: "POST",
                            dataType: "json",
                            data: JSON.stringify({
                                guests: guests,
                                isAttending: isAttending
                            }),
                            contentType: "application/json; charset=uft-8",
                            success: function (data) {

                                var dateOfRsvp = getDateFromServerParts(data);

                                formatDatePart = function (datePart) {
                                    datePart = datePart.toString();

                                    if (datePart.length == 1)
                                        datePart = '0' + datePart;

                                    return datePart;
                                };

                                $.each(guests, function (guestIndex, guest) {

                                    $('#tbGuests tr[data-inviteId="' + guest.inviteId + '"][data-guestId="' + guest.guestId + '"]').attr('data-attending', isAttending ? 'yes' : 'no');

                                    var $dateOfRsvpCell = $('td[data-inviteId="' + guest.inviteId + '"][data-guestId="' + guest.guestId + '"][data-dateOfRsvp]');
                                    $dateOfRsvpCell.attr('data-dateOfRsvpYear', dateOfRsvp.getFullYear());
                                    $dateOfRsvpCell.attr('data-dateOfRsvpMonth', formatDatePart(dateOfRsvp.getMonth() + 1));
                                    $dateOfRsvpCell.attr('data-dateOfRsvpDay', formatDatePart(dateOfRsvp.getDate()));
                                    $dateOfRsvpCell.attr('data-dateOfRsvpHour', formatDatePart(dateOfRsvp.getHours()));
                                    $dateOfRsvpCell.attr('data-dateOfRsvpMinute', formatDatePart(dateOfRsvp.getMinutes()));
                                    $dateOfRsvpCell.attr('data-dateOfRsvpSecond', formatDatePart(dateOfRsvp.getSeconds()));

                                    $dateOfRsvpCell.html(dateOfRsvp.toLocaleString());

                                    var $isAttendingCell = $('td[data-inviteId="' + guest.inviteId + '"][data-guestId="' + guest.guestId + '"][data-isAttending]');
                                    displayGuestIsAttending($isAttendingCell, isAttending);
                                });

                                $("#tblGuests").trigger("update");
                                $("#tblGuests").trigger("appendCache");

                                $('.admin_guests_list_guest_checkbox:checked').each(function (checkboxIndex, checkbox) {
                                    checkBox($(checkbox), false);
                                });

                                filterGuests();
                                $('#divRsvpGuests').dialog("close");
                            }
                        });
                    }
                },
                {
                    text: 'Cancel',
                    click: function () {
                        $('#divRsvpGuests').dialog("close");
                    }
                }
            ]
        });
    }
}