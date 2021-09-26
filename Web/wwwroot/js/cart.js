var CartController = function () {
    this.initialize = function () {
        loadData();
    }
    function loadData() {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: 'GET',
            url: "/" + culture + '/Cart/GetListItem',
            dataType: 'json',
            success: function (res) {
                var html = '';
                var total = 0;
                $.each(res, function (i, item) {
                    const amount = item.price * item.quantity;
                    html += "<tr>\r\n           <td> <img width=\"60\" src=\"" + item.image + "\" alt=\"\" \/><\/td>\r\n                <td>" + item.name + "<br \/>Color : black, Material : metal<\/td>\r\n                <td>\r\n                    <div class=\"input-append\"><input class=\"span1\" style=\"max-width:34px\" placeholder=\"" + item.quantity + "\" id=\"appendedInputButtons\" size=\"16\" type=\"text\"><button class=\"btn\" type=\"button\"><i class=\"icon-minus\"><\/i><\/button><button class=\"btn\" type=\"button\"><i class=\"icon-plus\"><\/i><\/button><button class=\"btn btn-danger\" type=\"button\"><i class=\"icon-remove icon-white\"><\/i><\/button>\t\t\t\t<\/div>\r\n                <\/td>\r\n                <td>" + numberWithCommas(item.price) + "<\/td>\r\n                <td>" + numberWithCommas(amount) + "<\/td>\r\n            <\/tr>";
                    total += amount;
                })
                $("#cart_body").html(html);
                $("#lbl_total").text(numberWithCommas(total));
                $("#lbl_numberItem").text(res.length);
            },
            error: function (err) {
                console.log(err);
            }
        })
    }
}