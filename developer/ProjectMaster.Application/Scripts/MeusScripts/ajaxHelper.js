$.ajaxSetup({
    beforeSend: function () {
        MostrarCarregando();
    },
    complete: function () {
        EsconderCarregando();
    },
    cache: false,
    global: true
});

function MostrarCarregando() {
    $('#divLoading').fadeIn();
}

function EsconderCarregando() {
    $('#divLoading').fadeOut();
}
$.postAjax = function (parametros) {
    $.ajax({
        type: "POST",
        url: $("#hdfUrlRoot").val() + parametros.Url,
        cache: false,
        data: JSON.stringify(parametros.Dados),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    }).done(function (json) {
        parametros.Done(json);  
    });;
};
