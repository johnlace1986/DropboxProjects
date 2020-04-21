function checkBox(checkbox, checked) {
    if (checked)
        $(checkbox).prop('checked', 'checked');
    else
        $(checkbox).removeProp('checked');
}

function setElementDisabled(element, disabled) {
    if (disabled) {
        $(element).prop('disabled', true);
        $(element).addClass('disabledElement');
    }
    else {
        $(element).removeProp('disabled');
        $(element).removeClass('disabledElement');
    }
}

function getWindowHeight() {

    var viewportheight;

    if (typeof window.innerHeight != 'undefined') {
        viewportheight = window.innerHeight;
    }
    else if (
        typeof document.documentElement != 'undefined' &&
        typeof document.documentElement.clientHeight != 'undefined' &&
        document.documentElement.clientHeight != 0) {
        viewportheight = document.documentElement.clientHeight;
    }
    else {
        viewportheight = document.getElementsByTagName('body')[0].clientHeight;
    }

    return viewportheight;
}

function escapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}

function replaceAll(string, find, replace) {
    return string.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}

function getDateFromServerParts(dateParts) {

    var date = new Date();
    date.setUTCFullYear(dateParts.split('|')[0]);
    date.setUTCMonth(parseInt(dateParts.split('|')[1]) - 1);
    date.setUTCDate(dateParts.split('|')[2]);
    date.setUTCHours(dateParts.split('|')[3]);
    date.setUTCMinutes(dateParts.split('|')[4]);
    date.setUTCSeconds(dateParts.split('|')[5]);

    return date;
}

function formatUtcDate(date) {

    var utcDate = new Date();
    utcDate.setUTCFullYear(date.getFullYear());
    utcDate.setUTCMonth(date.getMonth());
    utcDate.setUTCDate(date.getDate());
    utcDate.setUTCHours(date.getHours());
    utcDate.setUTCMinutes(date.getMinutes());
    utcDate.setUTCSeconds(date.getSeconds());

    return utcDate.toLocaleString();
}

function makeStringWebSafe(value) {

    value = replaceAll(value, '&#39;', '\'');
    value = replaceAll(value, '&quot;', '"');
    value = replaceAll(value, '&amp;', '&');
    value = replaceAll(value, '&#163;', '£');
    value = replaceAll(value, '&#172;', '¬');

    return value;
}