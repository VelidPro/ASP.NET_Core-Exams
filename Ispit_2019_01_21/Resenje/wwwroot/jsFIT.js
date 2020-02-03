

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
        var ajaxRezultatReplace = $(this).attr("ajax-rezultat-replace");
        var urlZaPoziv;

        if (urlZaPoziv1 instanceof String)
            urlZaPoziv = urlZaPoziv1;
        else
            urlZaPoziv = urlZaPoziv2;

        var tempThis = $(this);

        $.get(urlZaPoziv, function (data, status) {
            if (ajaxRezultatReplace === "da")
                $("#" + divZaRezultat).replaceWith(data);
            else
                $("#" + divZaRezultat).html(data);
        }).done(function () {
            $(tempThis).data("requestValidator", false);
        });;
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
        var rezultatAppend = $(this).attr("ajax-append-rezultat");
        var ajaxRezultatReplace = $(this).attr("ajax-rezultat-replace");


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
                if (rezultatAppend === "da")
                    $("#" + divZaRezultat).append(data);
                else if (ajaxRezultatReplace==="da")
                    $("#" + divZaRezultat).replaceWith(data);
                else
                    $("#" + divZaRezultat).html(data);

            }
        }).done(function() {
            $(form).data("requestValidator", false);
        });
    });
}
$(document).ready(function () {
    // izvršava nakon što glavni html dokument bude generisan
    DodajAjaxEvente();

    // The node to be monitored
    var target = $("table")[0];

    // Create an observer instance
    var observer = new MutationObserver(function (mutations) {
        DodajAjaxEvente();
    });

    // Configuration of the observer:
    var config = {
        attributes: true,
        childList: true,
        characterData: true
    };

    // Pass in the target node, as well as the observer options
    observer.observe(target, config);
});

$(document).ajaxComplete(function () {
    // izvršava nakon bilo kojeg ajax poziva
    DodajAjaxEvente();
});
