$(function() {
    $("#message a.close-notify").click(function() {
        $("#message").fadeOut("slow");
        return false;
    });
    $.ajaxSetup({ cache: false });
});

$(document).everyTime(15000, function() {
    checkNotifications();
}, 0);

function checkNotifications() {
    //Check AJAX query
    $.getJSON("/Home/GetNotifications", function (notification) {  //$("#notifyForm").serialize()
            if (null != notification) {
            $("#notifyText").html(notification[0].Message);
            $("#message").fadeIn("slow");
        }
        //$("notifyHyperLink").href(notification.HyperLink);
    });

//    $.ajax({
//        url: "/Home/GetNotifications",
//        cache: false,
//        type: "GET",
//        dataType: "json",
//        processData: false,
//        success: function (notification) {
//            $("#notifyText").html(notification[0].Message);
//            $("#message").fadeIn("slow");
//        }
//    });

}

