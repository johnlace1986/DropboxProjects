function setNotifyGiftWebsite(notifyGiftWebsite) {
    setElementDisabled($('#divNotifyGiftWebsite'), true);

    $('#divNotifyGiftWebsiteLoadingCell').loading({
        loadingImage: '../../Content/Images/loading.gif',
        successImage: '../../Content/Images/tick.png',
        errorImage: '../../Content/Images/cross.png',
        load: function (onSuccess, onError) {
            $.ajax({
                url: 'Gift/SetNotifyGiftWebsite',
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=uft-8",
                data: JSON.stringify({
                    notifyGiftWebsite: notifyGiftWebsite
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
            setElementDisabled($('#divNotifyGiftWebsite'), false);
        },
        error: function (response, textStatus, errorThrown) {

            checkBox($('#chkNotifyGiftWebsite'), !notifyGiftWebsite);
            setElementDisabled($('#divNotifyGiftWebsite'), false);
        }
    });
}