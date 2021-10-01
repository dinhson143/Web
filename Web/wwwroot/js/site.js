var SiteController = function () {
    this.initialize = function () {
        registerEvent();
        loadCart();
    }
    function loadCart() {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: 'GET',
            url: "/" + culture + '/Cart/GetListItem',
            dataType: 'json',
            success: function (res) {
                $("#lbl_numberItem_layout").text(res.length);
                $("#lbl_numberItem_sidebar").text(res.length);
            },
            error: function (err) {
                console.log(err);
            }
        })
    }
    function registerEvent() {
        $('body').on('click', '.btn-add-card', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const culture = $('#hidCulture').val();
            $.ajax({
                type: 'POST',
                url: "/" + culture + '/Cart/AddToCart',
                data: { id: id },
                dataType: 'json',
                success: function (res) {
                    $("#lbl_numberItem_layout").text(res.length);
                    $("#lbl_numberItem_sidebar").text(res.length);
                    /*$("#total_sidebar").text(numberWithCommas(total));*/
                },
                error: function (err) {
                    console.log(err);
                }
            })
        })
    }
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}