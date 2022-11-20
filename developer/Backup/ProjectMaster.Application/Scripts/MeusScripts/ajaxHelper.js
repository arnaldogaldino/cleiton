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

