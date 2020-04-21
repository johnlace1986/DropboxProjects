function setAccommodationContainerHeight() {
    var availableSpace = getWindowHeight() - 24;    //vertical padding

    availableSpace -= $('.admin_layout_navigation').height() + 72;    //48px vertical padding + 24px vertical margin
    availableSpace -= $('.logged_in_layout_footer').height() + 96;        //48px vertical padding + 48px vertical margin
    
    if (availableSpace < 240)
        availableSpace = 240;

    $('.admin_accommodation').css('height', availableSpace + 'px');
}

function makeUnassignedGuestContainerDroppable() {

    $('.admin_accommodation_unassigned_guests').droppable({
        drop: function (event, ui) {
            $(this).removeClass('admin_accommodation_unassigned_guests_hover');

            var guestId = $(ui.helper).attr('data-guestId');
            var fullName = $(ui.helper).attr('data-fullName');
            var plusOneFullName = $(ui.helper).attr('data-plusOneFullName');
            var isAttending = $(ui.helper).attr('data-isAttending');
            
            if ($('.admin_accommodation_unassigned_guests_container').find('div[data-guestId=' + guestId + ']').length == 0) {
                setGuestRoomId(guestId, null, function (data) {
                    addUnassignedGuest(guestId, fullName, plusOneFullName, isAttending);
                    $('.admin_accommodation_rooms').find('div[data-guestId=' + guestId + ']').remove();
                });
            }
        },
        over: function (event, ui) {
            $(this).addClass('admin_accommodation_unassigned_guests_hover');
        },
        out: function (event, ui) {
            $(this).removeClass('admin_accommodation_unassigned_guests_hover');
        }
    });

}

function makeNewRoomContainerDroppable() {
    $('.admin_accommodation_rooms_add').droppable({
        drop: function (event, ui) {
            $('.admin_accommodation_rooms_add').removeClass('admin_accommodation_rooms_add_hover');

            addNewRoom(function (roomId) {
                var guestId = $(ui.helper).attr('data-guestId');
                var fullName = $(ui.helper).attr('data-fullName');
                var plusOneFullName = $(ui.helper).attr('data-plusOneFullName');
                var isAttending = $(ui.helper).attr('data-isAttending');

                var onSuccess = function () {
                    addGuestToRoom(roomId, guestId, fullName, plusOneFullName, isAttending, parseInt($(ui.draggable[0]).attr('data-guestRoomId')));
                };

                var onError = function () {
                    deleteRoom(roomId);
                };

                setGuestRoomId(guestId, roomId, onSuccess, onError);
            });
        },
        over: function () {
            $('.admin_accommodation_rooms_add').addClass('admin_accommodation_rooms_add_hover');
        },
        out: function () {
            $('.admin_accommodation_rooms_add').removeClass('admin_accommodation_rooms_add_hover');
        }
    });
}

function addNewRoom(onSuccess) {
    
    editRoom(-1, '', 0, onSuccess);
}

function editRoom(roomId, name, beds, onSuccess) {

    $('#txtName').val(name);
    $('#txtBeds').val(beds);

    $('#divRoom').dialog({
        dialogClass: "no-close",
        modal: true,
        width: "292px",
        buttons: [
            {
                text: 'OK',
                click: function () {

                    $('#lblSaveExpenseError').html('');

                    name = $('#txtName').val().trim();

                    if (name == '') {
                        $('#lblSaveExpenseError').html('Please give the room a name.');
                        return;
                    }

                    beds = parseInt($('#txtBeds').val().trim());

                    if (isNaN(beds)) {
                        beds = 0;
                    }

                    if (beds == 0) {
                        $('#lblSaveExpenseError').html('Please enter the number of beds in the room.');
                        return;
                    }

                    $.ajax({
                        url: '/Admin/SaveRoom',
                        type: "POST",
                        dataType: "json",
                        data: JSON.stringify({
                            roomId: roomId,
                            name: name,
                            beds: beds
                        }),
                        contentType: "application/json; charset=uft-8",
                        success: function (data) {

                            $('#divRoom').dialog("close");

                            if (roomId == -1) {
                                roomId = data;
                                addRoom(roomId, name, beds);
                            }
                            else {

                                var $room = $('div[data-roomId=' + roomId + ']');
                                $room.attr('data-beds', beds);

                                var $roomHeader = $room.find('.admin_accommodation_room_header h5');
                                $roomHeader.html(name);
                                $roomHeader.attr('title', name);
                                
                                var guestsIdsToDelete = [];

                                $room.find('.admin_accommodation_room_guest').each(function (guestIndex, guest) {
                                    if (guestIndex >= beds) {
                                        guestsIdsToDelete.push(parseInt($(guest).attr('data-guestId')));
                                    }
                                });

                                $.each(guestsIdsToDelete, function (guestIndex, guestId) {

                                    var $guest = $room.find('div[data-guestId=' + guestId + ']')

                                    var guestId = $guest.attr('data-guestId');
                                    var fullName = $guest.attr('data-fullName');
                                    var plusOneFullName = $guest.attr('data-plusOneFullName');
                                    var isAttending = $guest.attr('data-isAttending');

                                    addUnassignedGuest(guestId, fullName, plusOneFullName, isAttending);
                                    $guest.remove();
                                });
                            }

                            if (onSuccess)
                                onSuccess(roomId);
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

function addRoom(roomId, name, beds) {

    var $room = $(
        '<div class="admin_accommodation_room admin_accommodation_room_empty" data-roomId="' + roomId + '" data-beds="' + beds + '">' +
            '<div class="admin_accommodation_room_header">' +
                '<h5 title="' + name + '">' + name + '</h5>' +
                '<div class="admin_accommodation_room_header_delete_button">&nbsp;</div>' +
                '<div class="admin_accommodation_room_header_edit_button">&nbsp;</div>' +
            '</div>' +
        '</div>');

    $room.find('.admin_accommodation_room_header_edit_button').on('click', function () {
        editRoom(roomId, name, beds);
    });

    $room.find('.admin_accommodation_room_header_delete_button').on('click', function () {
        deleteRoom(roomId);
    });

    $room.insertBefore('.admin_accommodation_rooms_add');

    $room.droppable({
        drop: function (event, ui) {
            $(this).removeClass('admin_accommodation_room_hover');

            var guestId = $(ui.helper).attr('data-guestId');
            var fullName = $(ui.helper).attr('data-fullName');
            var plusOneFullName = $(ui.helper).attr('data-plusOneFullName');
            var isAttending = $(ui.helper).attr('data-isAttending');

            if ($('.admin_accommodation_rooms').find('div[data-roomId=' + roomId + '] div[data-guestId=' + guestId + ']').length == 0) {
                setGuestRoomId(guestId, roomId, function (data) {
                    addGuestToRoom(roomId, guestId, fullName, plusOneFullName, isAttending, parseInt($(ui.draggable[0]).attr('data-guestRoomId')));
                });
            }
        },
        over: function (event, ui) {
            var draggableRoomId = parseInt(ui.draggable.attr('data-guestRoomId'))
            var droppableRoomId = parseInt($(this).attr('data-roomId'));

            if (draggableRoomId != droppableRoomId)
                $(this).addClass('admin_accommodation_room_hover');
        },
        out: function (event, ui) {
            $(this).removeClass('admin_accommodation_room_hover');
        }
    });
}

function addUnassignedGuest(guestId, fullName, plusOneFullName, isAttending) {

    $guest = $('<div class="admin_accommodation_unassigned_guest ' + (isAttending == 'true' ? '' : 'admin_accommodation_guest_not_attending') + '" data-guestId="' + guestId + '" data-fullName="' + fullName + '" data-plusOneFullName="' + plusOneFullName + '" data-isAttending="' + isAttending + '"></div>');

    $guest.append('<p>' + fullName + '</p>');

    if (plusOneFullName != '') {
        $guest.append('<p>' + plusOneFullName + '</p>');
    }

    $guest.draggable({
        helper: 'clone',
        appendTo: '.admin_accommodation',
        revert: true,
        revertDuration: 0,
        scroll: false,
        start: function (event, ui) {
            $(ui.helper).addClass('admin_accommodation_unassigned_guest_dragging')
        }
    });

    $('.admin_accommodation_unassigned_guests_container').append($guest);
}

function filterUnassignedGuests(filter) {
    $('.admin_accommodation_unassigned_guest').each(function (divIndex, div) {
        if (filter == '' || ($(div).find('p').html().toLowerCase().indexOf(filter.toLowerCase()) > -1)) {
            $(div).show();
        }
        else {
            $(div).hide();
        }
    });
}

function addGuestToRoom(roomId, guestId, fullName, plusOneFullName, isAttending, draggedRoomId) {
    
    var $room = $('.admin_accommodation_rooms').find('div[data-roomId=' + roomId + ']');

    var maxBeds = parseInt($room.attr('data-beds'));
    var currentBeds = $room.find('.admin_accommodation_room_guest').length;

    if (currentBeds < maxBeds) {
        $guest = $('<div class="admin_accommodation_room_guest ' + (isAttending == 'true' ? '' : 'admin_accommodation_guest_not_attending') + '" data-guestRoomId="' + roomId + '" data-guestId="' + guestId + '" data-fullName="' + fullName + '" data-plusOneFullName="' + plusOneFullName + '" data-isAttending="' + isAttending + '"></div>');

        $guest.append('<p>' + fullName + '</p>');

        if (plusOneFullName != '') {
            $guest.append('<p>' + plusOneFullName + '</p>');
        }

        $guest.draggable({
            helper: 'clone',
            appendTo: '.admin_accommodation',
            revert: true,
            revertDuration: 0,
            scroll: false,
            start: function (event, ui) {
                $(ui.helper).addClass('admin_accommodation_room_guest_dragging')
            }
        });

        $room.append($guest);
        $room.removeClass('admin_accommodation_room_empty');

        $('.admin_accommodation_unassigned_guests_container').find('div[data-guestId=' + guestId + ']').remove();
        $('.admin_accommodation_rooms').find('div[data-roomId!=' + roomId + '] div[data-guestId=' + guestId + ']').remove();

        if (draggedRoomId) {
            var $draggedRoom = $('.admin_accommodation_rooms').find('div[data-roomId=' + draggedRoomId + ']');

            if ($draggedRoom.find('.admin_accommodation_room_guest[data-guestId!=' + guestId + ']').length == 0) {
                $('.admin_accommodation_rooms').find('div[data-roomId=' + draggedRoomId + ']').addClass('admin_accommodation_room_empty');
            }
        }
    }
    else {

    }
}

function deleteRoom(roomId) {

    var $room = $('div[data-roomId=' + roomId + ']');

    $.ajax({
        url: '/Admin/DeleteRoom',
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            roomId: roomId
        }),
        contentType: "application/json; charset=uft-8",
        success: function (data) {
            $room.find('div[data-guestId]').each(function (guestIndex, div) {
                var guestId = $(div).attr('data-guestId');
                var fullName = $(div).attr('data-fullName');
                var plusOneFullName = $(div).attr('data-plusOneFullName');
                var isAttending = $(div).attr('data-isAttending');

                addUnassignedGuest(guestId, fullName, plusOneFullName, isAttending);
            });

            $room.remove();
        }
    });
}

function setGuestRoomId(guestId, roomId, onSuccess, onError) {

    $.ajax({
        url: '/Admin/SetGuestRoomId',
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            guestId: guestId,
            roomId: roomId
        }),
        contentType: "application/json; charset=uft-8",
        success: function (data) {
            if (onSuccess)
                onSuccess(data);
        },
        error: function (response, textStatus, errorThrown) {
            if (onError)
                onError(response, textStatus, errorThrown);
        }
    });
}