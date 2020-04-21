(function ($) {

    $.fn.loading = function (options) {
        return $(this).each(function (index, container) {

            var actualOptions = $.extend({
                fadeInTime: 1,
                fadeOutTime: 1.5,
                loadingImage: '',
                successImage: '',
                errorImage: '',
                load: null,
                beginSuccess: null,
                success: null,
                error: null,
                errorClick: null
            }, options);

            if (actualOptions.load) {
                fadeInImage(container, actualOptions.fadeInTime, actualOptions.loadingImage, function () {

                    $(container).find('img').css('cursor', '');

                    actualOptions.load(
                        function (data, textStatus, response) {
                            //fade out loading image
                            $(container).find('img').fadeOut(actualOptions.fadeInTime * 1000);

                            //wait for loading image fade out to complete
                            setTimeout(function () {

                                if (actualOptions.beginSuccess)
                                    actualOptions.beginSuccess(data, textStatus, response);

                                //fade in success image
                                fadeInImage(container, actualOptions.fadeInTime, actualOptions.successImage, function () {

                                    //wait and then fade out the success image
                                    setTimeout(function () {
                                        fadeOutImage(container, actualOptions.fadeOutTime, function () {
                                            $(container).html('');

                                            if (actualOptions.success)
                                                actualOptions.success(data, textStatus, response);
                                        });
                                    }, actualOptions.fadeInTime * 1000);
                                });
                            }, actualOptions.fadeInTime * 1000);
                        },
                        function (response, textStatus, errorThrown) {
                            //fade out loading image
                            $(container).find('img').fadeOut(actualOptions.fadeInTime * 1000);

                            //wait for loading image fade out to complete
                            setTimeout(function () {

                                //fade in error image
                                fadeInImage(container, actualOptions.fadeInTime, actualOptions.errorImage, function () {

                                    if (actualOptions.errorClick) {
                                        var img = $(container).find('img');
                                        img.css('cursor', 'pointer');
                                        img.off('click').on('click', function () {
                                            actualOptions.errorClick();
                                        });
                                    }

                                    if (actualOptions.error)
                                        actualOptions.error(response, textStatus, errorThrown);
                                });
                            }, actualOptions.fadeInTime * 1000);
                        });
                });
            }
        });
    }

    function fadeInImage(container, fadeInTime, src, success) {
        //add the image to the cell
        $(container).html('<img src="' + src + '" />');

        //immediately hide the image and then fade it in
        $(container).find('img').hide().fadeIn(fadeInTime * 1000);

        //call the success function after the fade in has completed
        if (success) {
            setTimeout(success, fadeInTime * 1000);
        }
    }

    function fadeOutImage(container, fadeOutTime, success) {
        $(container).find('img').fadeOut(fadeOutTime * 1000);

        //call the success function after the fade in has completed
        if (success) {
            setTimeout(success, fadeOutTime * 1000);
        }
    }

})(jQuery);