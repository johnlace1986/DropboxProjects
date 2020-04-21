function setSeatingPlanContainerHeight() {
    var availableSpace = getWindowHeight() - 24;    //vertical padding

    availableSpace -= $('.admin_layout_navigation').height() + 72;    //48px vertical padding + 24px vertical margin
    availableSpace -= $('.logged_in_layout_footer').height() + 96;        //48px vertical padding + 48px vertical margin
    
    if (availableSpace < 240)
        availableSpace = 240;

    $('.admin_seating_plan').css('height', availableSpace + 'px');
}

function makeUnassignedGuestContainerDroppable() {

    $('.admin_seating_plan_unassigned_guests').droppable({
        drop: function (event, ui) {
            $(this).removeClass('admin_seating_plan_unassigned_guests_hover');

            var guestId = $(ui.helper).attr('data-guestId');
            var fullName = $(ui.helper).attr('data-fullName');
            var plusOneFullName = $(ui.helper).attr('data-plusOneFullName');
            var isAttending = $(ui.helper).attr('data-isAttending');
            
            if ($('.admin_seating_plan_unassigned_guests_container').find('div[data-guestId=' + guestId + ']').length == 0) {
                setGuestTableId(guestId, null, function (data) {
                    addUnassignedGuest(guestId, fullName, plusOneFullName, isAttending);
                    $('.admin_seating_plan_tables').find('div[data-guestId=' + guestId + ']').remove();
                });
            }
        },
        over: function (event, ui) {
            $(this).addClass('admin_seating_plan_unassigned_guests_hover');
        },
        out: function (event, ui) {
            $(this).removeClass('admin_seating_plan_unassigned_guests_hover');
        }
    });

}

function makeNewTableContainerDroppable() {
    $('.admin_seating_plan_tables_add').droppable({
        drop: function (event, ui) {
            $('.admin_seating_plan_tables_add').removeClass('admin_seating_plan_tables_add_hover');

            addNewTable(function (tableId) {
                var guestId = $(ui.helper).attr('data-guestId');
                var fullName = $(ui.helper).attr('data-fullName');
                var plusOneFullName = $(ui.helper).attr('data-plusOneFullName');
                var isAttending = $(ui.helper).attr('data-isAttending');

                var onSuccess = function () {
                    addGuestToTable(tableId, guestId, fullName, plusOneFullName, isAttending);
                };

                var onError = function () {
                    deleteTable(tableId);
                };

                setGuestTableId(guestId, tableId, onSuccess, onError);
            });
        },
        over: function () {
            $('.admin_seating_plan_tables_add').addClass('admin_seating_plan_tables_add_hover');
        },
        out: function () {
            $('.admin_seating_plan_tables_add').removeClass('admin_seating_plan_tables_add_hover');
        }
    });
}

function addNewTable(onSuccess) {

    $.ajax({
        url: '/Admin/AddNewTable',
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=uft-8",
        success: function (data) {
            var tableId = data;
            addTable(tableId);

            if (onSuccess)
                onSuccess(tableId);
        }
    });

}

function addUnassignedGuest(guestId, fullName, plusOneFullName, isAttending) {

    $guest = $('<div class="admin_seating_plan_unassigned_guest ' + (isAttending == 'true' ? '' : 'admin_seating_plan_guest_not_attending') + '" data-guestId="' + guestId + '" data-fullName="' + fullName + '" data-plusOneFullName="' + plusOneFullName + '" data-isAttending="' + isAttending + '"></div>');

    $guest.append('<p>' + fullName + '</p>');

    if (plusOneFullName != '') {
        $guest.append('<p>' + plusOneFullName + '</p>');
    }

    $guest.draggable({
        helper: 'clone',
        appendTo: '.admin_seating_plan',
        revert: true,
        revertDuration: 0,
        scroll: false,
        start: function (event, ui) {
            $(ui.helper).addClass('admin_seating_plan_unassigned_guest_dragging')
        }
    });

    $('.admin_seating_plan_unassigned_guests_container').append($guest);
}

function filterUnassignedGuests(filter) {
    $('.admin_seating_plan_unassigned_guest').each(function (divIndex, div) {
        if (filter == '' || ($(div).find('p').html().toLowerCase().indexOf(filter.toLowerCase()) > -1)) {
            $(div).show();
        }
        else {
            $(div).hide();
        }
    });
}

function addTable(tableId) {

    var tableCount = $('.admin_seating_plan_table').length;

    var $table = $(
        '<div class="admin_seating_plan_table" data-tableId="' + tableId + '">' +
            '<div class="admin_seating_plan_table_header">' +
                '<h5>Table ' + (tableCount + 1) + '</h5>' +
                '<div class="admin_seating_plan_table_header_delete_button">&nbsp;</div>' +
            '</div>' +
        '</div>');

    $table.find('.admin_seating_plan_table_header_delete_button').on('click', function () {
        deleteTable(tableId);
    });

    $table.insertBefore('.admin_seating_plan_tables_add');

    $table.droppable({
        drop: function (event, ui) {
            $(this).removeClass('admin_seating_plan_table_hover');

            var guestId = $(ui.helper).attr('data-guestId');
            var fullName = $(ui.helper).attr('data-fullName');
            var plusOneFullName = $(ui.helper).attr('data-plusOneFullName');
            var isAttending = $(ui.helper).attr('data-isAttending');

            if ($('.admin_seating_plan_tables').find('div[data-tableId=' + tableId + '] div[data-guestId=' + guestId + ']').length == 0) {
                setGuestTableId(guestId, tableId, function (data) {
                    addGuestToTable(tableId, guestId, fullName, plusOneFullName, isAttending);
                });
            }
        },
        over: function (event, ui) {
            var draggableTableId = parseInt(ui.draggable.attr('data-guestTableId'))
            var droppableTableId = parseInt($(this).attr('data-tableId'));

            if (draggableTableId != droppableTableId)
                $(this).addClass('admin_seating_plan_table_hover');
        },
        out: function (event, ui) {
            $(this).removeClass('admin_seating_plan_table_hover');
        }
    });
}

function deleteTable(tableId) {

    var $table = $('div[data-tableId=' + tableId + ']');

    $.ajax({
        url: '/Admin/DeleteTable',
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            tableId: tableId
        }),
        contentType: "application/json; charset=uft-8",
        success: function (data) {
            $table.find('div[data-guestId]').each(function (guestIndex, div) {
                var guestId = $(div).attr('data-guestId');
                var fullName = $(div).attr('data-fullName');
                var plusOneFullName = $(div).attr('data-plusOneFullName');
                var isAttending = $(div).attr('data-isAttending');

                addUnassignedGuest(guestId, fullName, plusOneFullName, isAttending);
            });

            $table.remove();

            $('.admin_seating_plan_tables').find('.admin_seating_plan_table').each(function (index, div) {
                $(div).find('.admin_seating_plan_table_header h5').html('Table ' + (index + 1));
            });
        }
    });
}

function addGuestToTable(tableId, guestId, fullName, plusOneFullName, isAttending) {

    $guest = $('<div class="admin_seating_plan_table_guest ' + (isAttending == 'true' ? '' : 'admin_seating_plan_guest_not_attending') + '" data-guestTableId="' + tableId + '" data-guestId="' + guestId + '" data-fullName="' + fullName + '" data-plusOneFullName="' + plusOneFullName + '", data-isAttending="' + isAttending + '"></div>');

    $guest.append('<p>' + fullName + '</p>');

    if (plusOneFullName != '') {
        $guest.append('<p>' + plusOneFullName + '</p>');
    }

    $guest.draggable({
        helper: 'clone',
        appendTo: '.admin_seating_plan',
        revert: true,
        revertDuration: 0,
        scroll: false,
        start: function (event, ui) {
            $(ui.helper).addClass('admin_seating_plan_table_guest_dragging')
        }
    });

    $('.admin_seating_plan_tables').find('div[data-tableId=' + tableId + ']').append($guest);

    $('.admin_seating_plan_unassigned_guests_container').find('div[data-guestId=' + guestId + ']').remove();
    $('.admin_seating_plan_tables').find('div[data-tableId!=' + tableId + '] div[data-guestId=' + guestId + ']').remove();
}

function setGuestTableId(guestId, tableId, onSuccess, onError) {

    $.ajax({
        url: '/Admin/SetGuestTableId',
        type: "POST",
        dataType: "json",
        data: JSON.stringify({
            guestId: guestId,
            tableId: tableId
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