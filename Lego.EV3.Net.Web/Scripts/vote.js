$(function () {

  
    $.ajaxSetup({ cache: false });
   

    $("button").click(function () {
        
        //send the current button value
        $.getJSON("http://localhost:22640/home/SendMovement/?" + $.param({ movement: this.value }), "", function () { })
    });
}());