$(document).ready(function () {
    $.ajax({
        url: '/User/ProductListByName',
        type: 'get',
        success: function (data) {
            console.log(data)
            $('#searchid').autocomplete({
                source:data
            })
        }
    })
});

$(document).ready(function () {
    $.ajax({
        url: '/User/NotificationBadge',
        type: 'get',
        success: function (data) {
            if (data === 0) {
                $('#badge').hide() 
            }
            else {
                $('#badge').html(data);
            }
          
        }
    })
});

