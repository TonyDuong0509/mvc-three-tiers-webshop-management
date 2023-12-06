


$(document).ready(function () {
    ShowCount();
    $('body').on('click', '.btnAddToCart', function (event) {
        event.preventDefault();
        var id = $(this).data('id');
        var quantity = 1;
        var tQuantity = $('#quantity_value').text();
        if (tQuantity != '') {
            quantity = parseInt(tQuantity);
        }
        $.ajax({
            url: '/ShoppingCart/AddToCart',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (result) {
                if (result.success) {
                    $('#checkout_items').html(result.Count);
                    alert(result.msg);
                }
            }
        });
    });


    $('body').on('click', '.btnDelete', function (event) {
        event.preventDefault();
        var id = $(this).data('id');
        var conf = confirm('Click OK to Delete !!!');
        if (conf == true) {
            $.ajax({
                url: '/ShoppingCart/Delete',
                type: 'POST',
                data: { id, id },
                success: function (result) {
                    if (result.success) {
                        $('#checkout_items').html(result.Count);
                        $('#trow_' + id).remove();
                        LoadCart();
                    }
                }
            });
        }
    });

    $('body').on('click', '.btnDeleteAll', function (event) {
        event.preventDefault();
        var conf = confirm('Click OK to Delete All !!!');
        if (conf == true) {
            DeleteAll();
        }
    });

    $('body').on('click', '.btnUpdate', function (event) {
        event.preventDefault();
        var id = $(this).data('id');
        var quantity = $('#Quantity_' + id).val();
        Update(id, quantity);
    }); 
});


// --------------------------------------------------------------


function ShowCount() {
    $.ajax({
        url: '/ShoppingCart/ShowCount',
        type: 'GET',
        success: function (result) {
            $('#checkout_items').html(result.Count);
        }
    });
}

function LoadCart() {
    $.ajax({
        url: '/ShoppingCart/Partial_Item_Cart',
        type: 'GET',
        success: function (result) {
            $('#load_data').html(result);
        }
    });
}

function DeleteAll() {
    $.ajax({
        url: '/ShoppingCart/DeleteAll',
        type: 'POST',
        success: function (result) {
            if (result.success) {
                LoadCart();
            }
        }
    });
}

function Update(id, quantity) {
    $.ajax({
        url: '/ShoppingCart/Update',
        type: 'POST',
        data: { id: id, quantity: quantity },
        success: function (result) {
            if (result.success) {
                LoadCart();
            }
        }
    });
}


