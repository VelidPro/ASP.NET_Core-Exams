

function DodajAjaxEvente() {
    $("select[ajax-event='da']").change(function(event) {
        $(this).attr("ajax-poziv", "dodan");
        event.preventDefault();
        var urlZaPoziv = $(this).attr("href");
        var removeElementClass = $(this).attr("ajax-remove-class");
        var ajaxNotify = $(this).attr("ajax-notify");
        var ajaxMessage = $(this).attr("ajax-message");
        var divZaRezultat = $(this).attr("ajax-rezultat");
        var ajaxEvidencijaRez = $(this).attr("ajax-evidencija-rezultata");

        var ajaxGetValue = $(this).attr("ajax-get-value");
        var appendUrl = $(this).attr('ajax-append-url');



        if (ajaxGetValue === "da") {
            urlZaPoziv += appendUrl + $(this).val();
        }

        $.get(urlZaPoziv,
            function(data, status) {
                var tempDiv = $("#" + divZaRezultat);
                if (ajaxEvidencijaRez === "da") {
                    if (data.isReferentna) {
                        tempDiv.removeClass("danger-border");
                    } else {
                        tempDiv.addClass("danger-border");

                    }
                } else {
                    tempDiv.html(data);
                }

                if (ajaxNotify === "da")
                    alertify.success(ajaxMessage);

            });
    });

    $("input[ajax-event='da']").change(function (event) {
        $(this).attr("ajax-poziv", "dodan");
        event.preventDefault();
        var urlZaPoziv = $(this).attr("href");
        var removeElementClass = $(this).attr("ajax-remove-class");
        var ajaxNotify = $(this).attr("ajax-notify");
        var ajaxMessage = $(this).attr("ajax-message");
        var divZaRezultat = $(this).attr("ajax-rezultat");
        var ajaxEvidencijaRez = $(this).attr("ajax-evidencija-rezultata");

        var ajaxGetValue = $(this).attr("ajax-get-value");
        var appendUrl = $(this).attr('ajax-append-url');




        if (ajaxGetValue === "da") 
            urlZaPoziv += appendUrl + $(this).val();

        $.get(urlZaPoziv,
            function (data, status) {
                var tempDiv = $("#" + divZaRezultat);
                if (ajaxEvidencijaRez === "da") {
                    if (data.isReferentna) {
                        tempDiv.removeClass("danger-border");
                    } else {
                        tempDiv.addClass("danger-border");
                    }
                } else {
                    tempDiv.html(data);
                }

                if (ajaxNotify === "da")
                    alertify.success(ajaxMessage);

            });
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
        var thisParent = $(this).parent();
        $(this).attr("ajax-poziv", "dodan");
        event.preventDefault();
        var urlZaPoziv1 = $(this).attr("ajax-url");
        var urlZaPoziv2 = $(this).attr("href");
        var divZaRezultat = $(this).attr("ajax-rezultat");
        var ajaxNotify = $(this).attr("ajax-notify");
        var ajaxMessage = $(this).attr("ajax-message");
        var removeElement = $(this).attr("ajax-remove");
        var removeElementClass = $(this).attr("ajax-remove-class");

        var lockSelects = $(this).attr("ajax-lock-selects");
        var lockInputs = $(this).attr("ajax-lock-inputs");
        var lockSelectsClass = $(this).attr("ajax-selects-class");
        var lockInputsClass = $(this).attr("ajax-inputs-class");


        var urlZaPoziv;

        if (urlZaPoziv1 instanceof String)
            urlZaPoziv = urlZaPoziv1;
        else
            urlZaPoziv = urlZaPoziv2;

        $.get(urlZaPoziv, function (data, status) {
            $("#" + divZaRezultat).html(data);

            if (divZaRezultat.includes("Datum"))
                thisParent.html("Zakljucano");

            if (lockSelects === "da") {
                var tempSelects = $("." + lockSelectsClass);
                tempSelects.each(function() {
                    var text = $("option:selected", $(this)).text();
                    $(this).replaceWith(`<div class='${$(this).attr("class")}'>${text}</div>`);
                });
                lockSelects = "ne";
            }

            else if (lockInputs === "da")
                $("." + lockInputsClass).attr("readonly",true);

            if (ajaxNotify === "da")
                alertify.success(ajaxMessage);
        }).error(function() {
            alertify.error("Greska.");
        }).done(function() {
            if (removeElement === "da")
                $("." + removeElementClass).remove();
        });
    });

    $("form[ajax-poziv='da']").submit(function (event) {
        $(this).attr("ajax-poziv", "dodan");
        event.preventDefault();
        var urlZaPoziv1 = $(this).attr("ajax-url");
        var urlZaPoziv2 = $(this).attr("action");
        var divZaRezultat = $(this).attr("ajax-rezultat");

        var ajaxNotify = $(this).attr("ajax-notify");
        var ajaxMessage = $(this).attr("ajax-message");

        var ajaxEvidencijaRez = $(this).attr("ajax-evidencija-rezultata");

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
                var tempDiv = $("#" + divZaRezultat);
                if (ajaxEvidencijaRez === "da") {
                    if (data.isReferentna) {
                        tempDiv.html(data.vrijednost);
                        tempDiv.removeClass("text-danger");
                    } else {
                        tempDiv.html(data.vrijednost);
                        tempDiv.addClass("text-danger");

                    }
                }
                else {
                   tempDiv.html(data);
                }

                if (ajaxNotify === "da")
                    alertify.success(ajaxMessage);
            },
            error: function(request, status, error) {
                if (request.responseText.length>0 && request.responseText.length<35) {
                    alertify.error(request.responseText);
                }
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
