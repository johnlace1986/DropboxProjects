$(document).ready(function () {
    $('#divBigDayTabs').tabs();
});

function setGuestVegetarian(guestId, isVegetarian) {

    setElementDisabled($('#divVegetarianGuest' + guestId), true);

    $('#divVegetarianGuestLoadingCell' + guestId).loading({
        loadingImage: '../../Content/Images/loading.gif',
        successImage: '../../Content/Images/tick.png',
        errorImage: '../../Content/Images/cross.png',
        load: function (onSuccess, onError) {
            $.ajax({
                url: 'BigDay/SetGuestVegetarian',
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=uft-8",
                data: JSON.stringify({
                    guestId: guestId,
                    isVegetarian: isVegetarian
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
            setElementDisabled($('#divVegetarianGuest' + guestId), false);
        },
        error: function (response, textStatus, errorThrown) {

            checkBox($('#chkVegetarianGuest' + guestId), !isVegetarian);
            setElementDisabled($('#divVegetarianGuest' + guestId), false);
        }
    });

}

function setGuestPlusOneVegetarian(guestId, isVegetarian) {

    setElementDisabled($('#divVegetarianGuestPlusOne' + guestId), true);

    $('#divVegetarianGuestPlusOneLoadingCell' + guestId).loading({
        loadingImage: '../../Content/Images/loading.gif',
        successImage: '../../Content/Images/tick.png',
        errorImage: '../../Content/Images/cross.png',
        load: function (onSuccess, onError) {
            $.ajax({
                url: 'BigDay/SetGuestPlusOneVegetarian',
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=uft-8",
                data: JSON.stringify({
                    guestId: guestId,
                    isVegetarian: isVegetarian
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
            setElementDisabled($('#divVegetarianGuestPlusOne' + guestId), false);
        },
        error: function (response, textStatus, errorThrown) {

            checkBox($('#chkVegetarianGuestPlusOne' + guestId), !isVegetarian);
            setElementDisabled($('#divVegetarianGuestPlusOne' + guestId), false);
        }
    });

}

function setReserveSandholeRoom() {

    var reserveSandholeRoom = $('#chkReserveRoom')[0].checked;

    setElementDisabled($('.big_day_reserve_room'), true);

    $('.big_day_reserve_room_loading').loading({
        loadingImage: '../../Content/Images/loading.gif',
        successImage: '../../Content/Images/tick.png',
        errorImage: '../../Content/Images/cross.png',
        load: function (onSuccess, onError) {
            $.ajax({
                url: 'BigDay/SetReserveSandholeRoom',
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=uft-8",
                data: JSON.stringify({
                    reserveSandholeRoom: reserveSandholeRoom
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
            setElementDisabled($('.big_day_reserve_room'), false);
        },
        error: function (response, textStatus, errorThrown) {

            checkBox($('#chkReserveRoom'), !reserveSandholeRoom);
            setElementDisabled($('.big_day_reserve_room'), false);
        }
    });
}