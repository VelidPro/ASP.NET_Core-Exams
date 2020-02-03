

function DodajAjaxEvente() {
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

        if ($(this).data("requestValidator"))
            return;

        $(this).data("requestValidator", true);

        $(this).attr("ajax-poziv", "dodan");
        event.preventDefault();
        var urlZaPoziv1 = $(this).attr("ajax-url");
        var urlZaPoziv2 = $(this).attr("href");
        var divZaRezultat = $(this).attr("ajax-rezultat");
        var ajaxRemove = $(this).attr("ajax-remove");
        var ajaxRemoveId = $(this).attr("ajax-remove-id");

        var urlZaPoziv;

        if (urlZaPoziv1 instanceof String)
            urlZaPoziv = urlZaPoziv1;
        else
            urlZaPoziv = urlZaPoziv2;

        var thisTemp = $(this);

        $.get(urlZaPoziv, function (data, status) {
            $("#" + divZaRezultat).html(data);

            if (ajaxRemove === "da")
                $("#" + ajaxRemoveId).remove();
        }).done(function() {
            $(thisTemp).data("requestValidator", false);

        });
    });

    $("form[ajax-poziv='da']").submit(function (event) {

        if ($(this).data("requestValidator"))
            return;

        $(this).data("requestValidator", true);


        event.preventDefault();
        var urlZaPoziv1 = $(this).attr("ajax-url");
        var urlZaPoziv2 = $(this).attr("action");
        var divZaRezultat = $(this).attr("ajax-rezultat");
        var ajaxRezultatAppend = $(this).attr("ajax-rezultat-append");

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
                if (ajaxRezultatAppend === "da")
                    $("#" + divZaRezultat).append(data);
                else
                    $("#" + divZaRezultat).html(data);
            },
            error: function(req, status, error) {

            }
        }).complete(function() {
            $(form).data("requestValidator", false);
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


