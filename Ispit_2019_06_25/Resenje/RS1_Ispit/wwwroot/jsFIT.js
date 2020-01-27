

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
        $(this).attr("ajax-poziv", "dodan");
        event.preventDefault();
        var urlZaPoziv1 = $(this).attr("ajax-url");
        var urlZaPoziv2 = $(this).attr("href");
        var divZaRezultat = $(this).attr("ajax-rezultat");

        var urlZaPoziv;

        var ajaxNotify = $(this).attr("ajax-notify");
        var ajaxMessage = $(this).attr("ajax-message");

        var ajaxToggler = $(this).attr("ajax-toggler");
        var ajaxRezultatId = $(this).attr("ajax-toggle-rezultat");


        if (urlZaPoziv1 instanceof String)
            urlZaPoziv = urlZaPoziv1;
        else
            urlZaPoziv = urlZaPoziv2;

        $.get(urlZaPoziv, function (data, status) {
            $("#" + divZaRezultat).html(data);

            if (ajaxToggler === "da") {
                if(data==="True")
                    $("#" + ajaxRezultatId).html("Pristupio");
                else if(data==="False")
                    $("#" + ajaxRezultatId).html("Nije pristupio");
            }

            if (ajaxNotify === "da")
                alertify.success(ajaxMessaege);
        });
    });

    $("form[ajax-poziv='da']").submit(function (event) {
        $(this).attr("ajax-poziv", "dodan");
        event.preventDefault();
        var urlZaPoziv1 = $(this).attr("ajax-url");
        var urlZaPoziv2 = $(this).attr("action");
        var divZaRezultat = $(this).attr("ajax-rezultat");

        var ajaxInputRezultat = $(this).attr("ajax-input-rezultat");
        var urlZaPoziv;
        if (urlZaPoziv1 instanceof String)
            urlZaPoziv = urlZaPoziv1;
        else
            urlZaPoziv = urlZaPoziv2;


        var ajaxNotify = $(this).attr("ajax-notify");
        var ajaxMessage = $(this).attr("ajax-message");

        var form = $(this);

        $.ajax({
            type: "POST",
            url: urlZaPoziv,
            data: form.serialize(),
            success: function (data) {
                if (ajaxInputRezultat === "da")
                    $("#" + divZaRezultat).val(data);
                else
                    $("#" + divZaRezultat).html(data);

                if (ajaxNotify === "da")
                    alertify.success(ajaxMessage);
            },
            error: function(request, status, error) {
                if (request.responseText.length)
                    alertify.error(request.responseText);
            }
        });
    });

    $("input[ajax-poziv='da']").change(function(event) {
        event.preventDefault();

        var urlZaPoziv1 = $(this).attr("ajax-url");

        var ajaxNotify = $(this).attr("ajax-notify");
        var ajaxMessage = $(this).attr("ajax-message");

        var appendParameter = $(this).attr("ajax-append-parameter");
        var appendParameterName = $(this).attr("ajax-append-parameter-name");

        if (appendParameter == "da")
            urlZaPoziv1 += "&" + appendParameterName + "=" + $(this).val();

        $.get(urlZaPoziv1, function (data, status) {
            if (ajaxNotify === "da")
                alertify.success(ajaxMessage);
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
