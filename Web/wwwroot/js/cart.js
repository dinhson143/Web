var CartController = function () {
    this.initialize = function () {
        loadData();
        registerEvent();
    }

    function registerEvent() {
        $('body').on('click', '.btn-plus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) + 1;
            updateCart(id, quantity);
        })

        $('body').on('click', '.btn-minus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) - 1;
            updateCart(id, quantity);
        })

        $('body').on('click', '.btn-remove', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = 0;
            updateCart(id, quantity);
        })
    }

    function updateCart(id, quantity) {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: 'POST',
            url: "/" + culture + '/Cart/UpdateCart',
            data: {
                id: id,
                quantity: quantity
            },
            dataType: 'json',
            success: function (res) {
                $("#lbl_numberItem_layout").text(res.length);
                $("#lbl_numberItem_sidebar").text(res.length);
                loadData();
            },
            error: function (err) {
                console.log(err);
            }
        })
    }

    function loadData() {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: 'GET',
            url: "/" + culture + '/Cart/GetListItem',
            dataType: 'json',
            success: function (res) {
                if (res.length == 0) {
                    $("#tbl_cart").hide();
                }
                var html = '';
                var total = 0;
                $.each(res, function (i, item) {
                    console.log(item);
                    const amount = item.price * item.quantity;
                    html += "<tr>\r\n           <td> <img width=\"60\" src=\"" + item.image + "\" alt=\"\" \/><\/td>\r\n                <td>" + item.name + "<br \/>Size : " + item.nameSize + "<\/td>\r\n                <td>\r\n                    <div class=\"input-append\"><input class=\"span1\" style=\"max-width:34px\" placeholder=\"" + item.quantity + "\" id=\"txt_quantity_" + item.productId + "\" value=\"" + item.quantity + "\" size=\"16\" type=\"text\"><button class=\"btn btn-minus\" data-id=\"" + item.productId + "\" type=\"button\"><i class=\"icon-minus\"><\/i><\/button><button class=\"btn btn-plus\" data-id=\"" + item.productId + "\" type=\"button\"><i class=\"icon-plus\"><\/i><\/button><button class=\"btn btn-danger btn-remove\" data-id=\"" + item.productId + "\" type=\"button\"><i class=\"icon-remove icon-white\"><\/i><\/button>\t\t\t\t<\/div>\r\n                <\/td>\r\n                <td>" + numberWithCommas(item.price) + "<\/td>\r\n                <td>" + numberWithCommas(amount) + "<\/td>\r\n            <\/tr>";
                    total += amount;
                })
                $("#cart_body").html(html);
                $("#lbl_total").text(numberWithCommas(total));
                $("#lbl_numberItem").text(res.length);
                $("#lbl_numberItem_sidebar").text(res.length);
                $("#total_sidebar").text(numberWithCommas(total));
            },
            error: function (err) {
                console.log(err);
            }
        })
    }
}