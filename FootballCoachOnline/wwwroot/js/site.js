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
