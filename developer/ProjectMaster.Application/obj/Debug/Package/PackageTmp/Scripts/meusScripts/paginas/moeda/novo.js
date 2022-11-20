$(function () {
    idContaCorrente = 0;
    $('#btnSalvar').click(SalvarMoeda);
    $('#nr_valor').priceFormat({
        prefix: '', centsSeparator: ',', thousandsSeparator: ''
    });
    $('#dt_cotacao ').keypress(function () { MascaraData(this) });
    $('#dt_cotacao').blur(function () { ValidaData(this) });
    $("#dt_cotacao").attr('MaxLength', '10');
    $("#nr_valor").attr('MaxLength', '6');
    if ($('#id_moeda').val() > 0) {
        $("#dt_cotacao").attr('readonly', 'true');
        $("#dt_cotacao").attr('readonly', 'true');
    }
    else {
        $("#dt_cotacao").datepicker({ dateFormat: 'dd/mm/yy' });
    }
});

function SalvarMoeda() {
    var form = $("#formMoeda");
    if (form.valid()) {
        var moeda = CriarMoeda();
        var parametros = {
            Url: 'Moeda/Salvar',
            Dados: moeda,
            Done: Finalizar
        };
        $.postAjax(parametros);
    }
}
function CriarMoeda() {
    var moeda = {
        id_moeda: $("#id_moeda").val(),
        id_tipo_moeda: $("#id_tipo_moeda").val(),
        dt_cotacao: $("#dt_cotacao").val(),
        nr_valor: $("#nr_valor").val()
    }
    return moeda;
}

function Finalizar(json) {
    alert(json.msg);
    if (!json.erro) {
        window.location = $("#hdfUrlRoot").val() + "Moeda/Editar/" + json.id_moeda;
    }
}