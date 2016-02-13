$(function () {

    $("button").button().click(function () {
        //send the current button value
        $.getJSON("http://localhost:22640/home/SendMovement/?" + $.param({ movement: 1 }), "", function () { })
    });
}());