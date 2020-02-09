function DodajAjaxEvente() {
    $("button[ajax-poziv='da']").click(function (event) {
        event.preventDefault();
        var urlZaPoziv = $(this).attr("ajax-url");
        var divZaRezultat = $(this).attr("ajax-rezultat");

        $.ajax({
            type: "GET",
            cache: false,
            url: urlZaPoziv,
            success: function (data) {
                $("#" + divZaRezultat).html(data);
            }
        }).complete(function () {
            $(this).data("requestValidator", false);
        });
    });

    $("a[ajax-poziv='da']").click(function (event) {
        if ($(this).data("requestValidator"))
            return;

        $(this).data("requestValidator", true);

        event.preventDefault();
        var urlZaPoziv1 = $(this).attr("ajax-url");
        var urlZaPoziv2 = $(this).attr("href");
        var divZaRezultat = $(this).attr("ajax-rezultat");
        var ajaxReplaceRezultat = $(this).attr("ajax-rezultat-replace");

        var urlZaPoziv;

        if (urlZaPoziv1 instanceof String)
            urlZaPoziv = urlZaPoziv1;
        else
            urlZaPoziv = urlZaPoziv2;

        $.ajax({
            type: "GET",
            cache: false,
            url: urlZaPoziv,
            success: function (data) {
                if (ajaxReplaceRezultat === "da")
                    $("#" + divZaRezultat).replaceWith(data);
                else
                    $("#" + divZaRezultat).html(data);
            }
        }).complete(function () {
            $(this).data("requestValidator", false);
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
        var ajaxReplaceRezultat = $(this).attr("ajax-rezultat-replace");

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
                if (ajaxReplaceRezultat === "da")
                    $("#" + divZaRezultat).replaceWith(data);
                else
                    $("#" + divZaRezultat).html(data);
            }
        }).complete(function () {
            $(this).data("requestValidator", false);
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