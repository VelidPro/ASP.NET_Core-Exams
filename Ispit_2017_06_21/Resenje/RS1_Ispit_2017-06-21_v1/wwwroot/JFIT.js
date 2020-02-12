function DodajAjaxEvente() {
    $("input[ajax-change-trigger='da']").change(function(event) {
        event.preventDefault();

        $(this).parent().submit();
    })

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
        var ajaxReplace = $(this).attr("ajax-replace-rezultat");

        var urlZaPoziv;

        if (urlZaPoziv1 instanceof String)
            urlZaPoziv = urlZaPoziv1;
        else
            urlZaPoziv = urlZaPoziv2;

        $.get(urlZaPoziv, function (data, status) {
            if (ajaxReplace === "da")
                $("#" + divZaRezultat).replaceWith(data);
            else
                $("#" + divZaRezultat).html(data);
        }).done(function() {
            $(this).data("requestValidator", false);
        });
    });

    $("form[ajax-poziv='da']").submit(function (event) {

        if ($(this).data("requestValidator"))
            return;

        $(this).data("requestValidator", true);

        $(this).attr("ajax-poziv", "dodan");
        event.preventDefault();
        var urlZaPoziv1 = $(this).attr("ajax-url");
        var urlZaPoziv2 = $(this).attr("action");
        var divZaRezultat = $(this).attr("ajax-rezultat");
        var ajaxReplace = $(this).attr("ajax-replace-rezultat");

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
                if (ajaxReplace === "da")
                    $("#" + divZaRezultat).replaceWith(data);
                else
                    $("#" + divZaRezultat).html(data);
            }
        }).complete(function() {
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