function setExpensesContainerHeight() {
    var availableSpace = getWindowHeight() - 24;    //vertical padding

    availableSpace -= $('.admin_layout_navigation').height() + 72;    //48px vertical padding + 24px vertical margin
    availableSpace -= $('.logged_in_layout_footer').height() + 96;        //48px vertical padding + 48px vertical margin
    availableSpace -= $('.admin_expenses_add_expense').height() + 24;   //24px bottom margin
    availableSpace -= $('.admin_expenses_total').height() + 26;   //24px bottom margin + 1px top and bottom border

    availableSpace -= 50; //24px bottom margin + 12px top and bottom padding + 1px top and bottom border

    if (availableSpace < 240)
        availableSpace = 240;

    $('.admin_expenses_list').css('height', availableSpace + 'px');
}

function addExpense(expenseId, name, cost, paid) {
    $('#tblExpenses tbody').append($('<tr data-expenseId="' + expenseId + '">' +
            '<td></td>' +
            '<td name></td>' +
            '<td cost></td>' +
            '<td paid></td>' +
            '<td remaining></td>' +
            '<td>' +
            '    <ul class="admin_expenses_list_buttons">' +
            '        <li><input type="button" class="button admin_expenses_list_buttons_edit" title="Edit Expense" onclick="editExpense(' + expenseId + ');" /></li>' +
            '        <li><input type="button" class="button admin_expenses_list_buttons_delete" title="Delete Expense" onclick="deleteExpense(' + expenseId + ');" /></li>' +
            '        <li><div class="admin_expenses_list_buttons_loading"></div>' +
            '    </ul>' +
            '</td>' +
        '</tr>'));

    updateExpense(expenseId, name, cost, paid);
}

function updateExpense(expenseId, name, cost, paid) {
    cost = parseFloat(cost.toString());
    paid = parseFloat(paid.toString());

    var $row = $('.admin_expenses_list tbody tr[data-expenseId=' + expenseId + ']');
    $row.find('td[name]').html(name);
    $row.find('td[cost]').html('£' + cost.toFixed(2));
    $row.find('td[paid]').html('£' + paid.toFixed(2));
    $row.find('td[remaining]').html('£' + (cost - paid).toFixed(2));

    if (cost <= paid) {
        $row.find('td[name]').addClass('admin_expenses_list_nothing_to_pay');
        $row.find('td[cost]').addClass('admin_expenses_list_nothing_to_pay');
        $row.find('td[paid]').addClass('admin_expenses_list_nothing_to_pay');
        $row.find('td[remaining]').addClass('admin_expenses_list_nothing_to_pay');
    }
    else {
        $row.find('td[name]').removeClass('admin_expenses_list_nothing_to_pay');
        $row.find('td[cost]').removeClass('admin_expenses_list_nothing_to_pay');
        $row.find('td[paid]').removeClass('admin_expenses_list_nothing_to_pay');
        $row.find('td[remaining]').removeClass('admin_expenses_list_nothing_to_pay');
    }
}

function addNewExpense() {
    editExpense(-1);
}

function editExpense(expenseId) {
    var name = '';
    var cost = '';
    var paid = '';

    var $row = $('.admin_expenses_list tbody tr[data-expenseId=' + expenseId + ']');

    if ($row.length == 1) {
        name = $row.find('td[name]').html();
        cost = $row.find('td[cost]').html().substr(1);
        paid = $row.find('td[paid]').html().substr(1);
    }

    $('#txtName').val(name);
    $('#txtCost').val(cost);
    $('#txtPaid').val(paid);
    
    $('#divExpense').dialog({
        dialogClass: "no-close",
        modal: true,
        width: "292px",
        buttons: [
            {
                text: 'OK',
                click: function () {
                    saveExpense(expenseId);
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

function saveExpense(expenseId) {

    $('#lblSaveExpenseError').html('&nbsp;');

    var name = $('#txtName').val().trim();

    if (name == '') {
        $('#lblSaveExpenseError').html('Please give the expense a name.');
        $('#txtName').focus();
        return;
    }

    var cost = $('#txtCost').val() == '' ? 0 : parseFloat(replaceAll($('#txtCost').val(), '_', '0'));
    var paid = $('#txtPaid').val() == '' ? 0 : parseFloat(replaceAll($('#txtPaid').val(), '_', '0'));

    $('#divExpense').dialog("close");

    var $row = $('.admin_expenses_list tbody tr[data-expenseId=' + expenseId + ']');

    $('.admin_expenses_add_expense_error').html('');

    var $loadingCell;
    var $disabledElements;

    if (expenseId == -1) {
        $loadingCell = $('.admin_expenses_add_expense_loading');
        $disabledElements = $('#btnAddExpense');
    }
    else {
        $loadingCell = $row.find('.admin_expenses_list_buttons_loading');
        $disabledElements = $row.find('.admin_expenses_list_buttons input[type="button"]');
    }
    
    setElementDisabled($disabledElements, true);

    $loadingCell.loading({
        loadingImage: '../../Content/Images/loading.gif',
        successImage: '../../Content/Images/tick.png',
        errorImage: '../../Content/Images/cross.png',
        load: function (onSuccess, onError) {
            $.ajax({
                url: "/Admin/SaveExpense",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=uft-8",
                data: JSON.stringify({
                    expenseId: expenseId,
                    name: name,
                    cost: cost,
                    paid: paid
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
            if (expenseId == -1) {
                expenseId = parseInt(data);
                addExpense(expenseId, name, cost, paid);
            }
            else {
                updateExpense(expenseId, name, cost, paid);
            }

            loadTotals();

            setElementDisabled($disabledElements, false);
        },
        error: function () {
            setElementDisabled($disabledElements, false);
            $('.admin_expenses_add_expense_error').html('An error has occurred. Please check the error logs.');
        }
    });
}

function deleteExpense(expenseId) {

    var $row = $('.admin_expenses_list tbody tr[data-expenseId=' + expenseId + ']');
    setElementDisabled($row.find('.admin_expenses_list_buttons input[type="button"]'), true);
    
    $row.find('.admin_expenses_list_buttons_loading').loading({
        loadingImage: '../../Content/Images/loading.gif',
        successImage: '../../Content/Images/tick.png',
        errorImage: '../../Content/Images/cross.png',
        load: function (onSuccess, onError) {
            $.ajax({
                url: "/Admin/DeleteExpense",
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=uft-8",
                data: JSON.stringify({
                    expenseId: expenseId
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
            $row.remove();
            loadTotals();
        },
        error: function () {
            setElementDisabled($row.find('.admin_expenses_list_buttons input[type="button"]'), false);
            $('.admin_expenses_add_expense_error').html('An error has occurred. Please check the error logs.');
        }
    });
}

function toggleRowEnabled(type) {
    var $row = $('tr[' + type + ']');

    if ($row.hasClass('admin_expenses_food_drink_disabled'))
        $row.removeClass('admin_expenses_food_drink_disabled');
    else
        $row.addClass('admin_expenses_food_drink_disabled');
}

function loadTotals() {

    $.ajax({
        url: "/Admin/LoadExpensesTotals",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=uft-8",
        success: function (data) {
            $('.admin_expenses_total td[total]').html('£' + data.total.toFixed(2));
            $('.admin_expenses_total td[paid]').html('£' + data.paid.toFixed(2));
            $('.admin_expenses_total td[totalToPay]').html('£' + data.totalToPay.toFixed(2));
        }
    });
}