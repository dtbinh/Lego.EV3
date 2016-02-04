$(function () {
    
    // Reference the auto-generated proxy for the hub.
    var voteHub = $.connection.voteDriveHub;


    voteHub.client.commandToRun = function (cmd) {
        console.log("cmdToRun:" + cmd);
    }

    //client function to receive
    voteHub.client.sendState = function (state) {
        var dist = state.SensorDistance;
        console.log(state);

        //use jQuery to pull values
        $("#sensorDistance").text("Distance" + ": " + dist);
        $("#first").text(lookup(state.AllVotes[0].Command) + ": " + state.AllVotes[0].Count);
        $("#second").text(lookup(state.AllVotes[1].Command) + ": " + state.AllVotes[1].Count);
        $("#third").text(lookup(state.AllVotes[2].Command) + ": " + state.AllVotes[2].Count);
        $("#fourth").text(lookup(state.AllVotes[3].Command) + ": " + state.AllVotes[3].Count);

        console.log(state);
    };

    //lookup to display a friendly label versus the enum value
    function lookup(id) {
        if (id == 0)
            return "Forward";
        if (id == 1)
            return "Back";
        if (id == 2)
            return "Left";
        if (id == 3)
            return "Right";
    }

    // Start the connection.
    $.connection.hub.start().done(function () {
        $("button").button().click(function () {
            //send the current button value
            voteHub.server.sendDriveCommand(this.value);
        });
    });

});