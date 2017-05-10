function SetEditAjax(selector, url, paramname) {
    $(document).on('click', selector, function (event) {
        var paramval = $(this).data(paramname);
        $("#tempmessage").removeClass("alert-success");
        $("#tempmessage").removeClass("alert-danger");
        $("#tempmessage").html('');
        $.get(url, { id: paramval }, function (data) {
            $("#tempmessage").addClass(data.success ? "alert-success" : "alert-danger");
            $("#tempmessage").addClass("panel-body");
            $("#tempmessage").html(data.message);
        });
    });
}

function SetAddAjax(selector, url) {
    $(document).on('click', selector, function (event) {
        event.preventDefault();
        var teamid = $(this).data('teamid');
        var playerid = $(this).data('playerid');
        var app = $('#a-' + playerid).is(":checked");
        var sub = $('#s-' + playerid).is(":checked");
        var goals = $('#g-' + playerid).val();
        var goalsConceded = $('#gc-' + playerid).val();
        var yellowCard = $('#y-' + playerid).is(":checked");
        var redCard = $('#r-' + playerid).is(":checked");
        var mid = $('#matchId').val();
        console.log(app);
        $.post(url, { playerId: playerid, teamId: teamid, app: app, sub: sub, goals: goals, goalsConceded: goalsConceded, yellowCard: yellowCard, redCard: redCard }, function (data) {
            console.log(data.message);
        });
    });
}
