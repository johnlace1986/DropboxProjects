
(function ($) {

    $.fn.slideshow = function (options) {

        return $(this).each(function (index, container) {

            if ($(container).find('img').length > 1) {

                var actualOptions = $.extend({
                    fadePeriod: 3,
                    slidePeriod: 5,
                    fadeImmediately: false
                }, options);

                $(container).find('img').css('position', 'absolute');
                $(container).find('img:gt(0)').hide();

                if (actualOptions.fadeImmediately) {
                    doFade(container, actualOptions)
                }
                else {
                    //set the timeout to initiate the first fade after the slide period
                    setTimeout(function () {
                        doFade(container, actualOptions);
                    }, actualOptions.slidePeriod * 1000);
                }
            }
        });
    };

    function doFade(container, actualOptions) {

        fade(container, actualOptions.fadePeriod);

        //set regular intervals to fade between pictures after the slide period + the fade period
        setInterval(function () {
            fade(container, actualOptions.fadePeriod);
        }, (actualOptions.slidePeriod + actualOptions.fadePeriod) * 1000);

    }
    
	function fade(container, fadePeriod) {
		//fade out first image
		$(container).find(':first-child').fadeOut(fadePeriod * 1000)
			//fade in next image
			.next('img').fadeIn(fadePeriod * 1000)
			//extract first image and append to the end of the stack
			.end().appendTo($(container));
	}

})(jQuery);