function validatePasswords() {
    var password = $('#txtPassword').val();
    var reEnteredPassword = $('#txtReEnteredPassword').val();
    $('.log_in_error').html('');

    if (password == '') {
        $('.log_in_error').html('Please enter a password');
        $('#txtPassword').focus();
        return false;
    }

    if (reEnteredPassword == '') {
        $('.log_in_error').html('Please re-enter a password');
        $('#txtReEnteredPassword').focus();
        return false;
    }

    if (password != reEnteredPassword) {
        $('.log_in_error').html('Passwords do not match');
        $('#txtReEnteredPassword').focus();
        return false;
    }

    return true;
}