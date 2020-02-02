

function DodajAjaxEvente() {
    $("input[ajax-trigger='da']").change(function (event) {
        event.preventDefault();
        $(this).parent().submit();
    });

    $("button[ajax-poziv='da']").click(function (event) {
        $(this).attr("ajax-poziv", "dodan");

        event.preventDefault();
        var urlZaPoziv = $(this).attr("ajax-url");
        var divZaRezultat = $(this).attr("ajax-rezultat");

        $.get(urlZaPoziv, function (data, status) {
            $("#" + divZaRezultat).html(data);
        });
    });

    $("a[ajax-poziv='da']").click(function (event) {
        var me = $(this);

        $(this).attr("ajax-poziv", "dodan");
        event.preventDefault();
        var urlZaPoziv1 = $(this).attr("ajax-url");
        var urlZaPoziv2 = $(this).attr("href");
        var divZaRezultat = $(this).attr("ajax-rezultat");
        var ajaxDanger = $(this).attr("ajax-danger");
        var ajaxRemoveDanger = $(this).attr("ajax-remove-danger");
        var ajaxRemove = $(this).attr("ajax-remove");
        var ajaxRemoveClass = $(this).attr("ajax-remove-class");
        var ajaxZakljucajButton = $(this).attr("ajax-zakljucaj-button");

        if (me.data("requestRunning"))
            return;

        me.data("requestRunning", true);

        var urlZaPoziv;

        if (urlZaPoziv1 instanceof String)
            urlZaPoziv = urlZaPoziv1;
        else
            urlZaPoziv = urlZaPoziv2;

        $.get(urlZaPoziv, function(data, status) {
            var temp = $("#" + divZaRezultat);
            temp.html(data);
            if (ajaxRemoveDanger === "da") {
                temp.removeClass("text-danger");
                temp.addClass("text-success");
            }

            if (ajaxDanger === "da") {
                temp.addClass("text-danger");
                temp.removeClass("text-success");
            }
            if (ajaxZakljucajButton === "da")
                $("#zakljucaj").html("<div>Zakljucano</div>");
            if (ajaxRemove === "da")
                $("." + ajaxRemoveClass).remove();
        }).done(function() {
            me.data("requestRunning", false);
        });
    });

    $("form[ajax-poziv='da']").submit(function (event) {
        $(this).attr("ajax-poziv", "dodan");
        event.preventDefault();
        var urlZaPoziv1 = $(this).attr("ajax-url");
        var urlZaPoziv2 = $(this).attr("action");
        var divZaRezultat = $(this).attr("ajax-rezultat");

        var urlZaPoziv;
        if (urlZaPoziv1 instanceof String)
            urlZaPoziv = urlZaPoziv1;
        else
            urlZaPoziv = urlZaPoziv2;

        var form = $(this);

        $.ajax({
            type: "POST",
            url: urlZaPoziv,
            data: form.serialize(),
            success: function (data) {
                $("#" + divZaRezultat).html(data);
            },
            error: function(req, status, error) {

            }
        });
    });
}
$(document).ready(function () {
    // izvršava nakon što glavni html dokument bude generisan
    DodajAjaxEvente();
});

$(document).ajaxComplete(function () {
    // izvršava nakon bilo kojeg ajax poziva
    DodajAjaxEvente();
});
