$.connection.hub.start()
    .done(function () {
        console.log("IT WORKED!")
        $.connection.myHub.server.test("Connected!");
    })
    .fail(function () { alert("ERROR" )});

$.connection.myHub.client.test = function (message) {
    $("#welcome-messages").append(message);
}