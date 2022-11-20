$(function () {
    idContaCorrente = 0;
    $('#btnSalvar').click(SalvarCentroDeCusto);
    if ($('#id_centro_de_custo').val() > 0) {
        $("#ds_codigo").attr('readonly', 'true');
    }
});

function SalvarCentroDeCusto() {
    var form = $("#formCentroDeCusto");
    if (form.valid()) {
        var CentroDeCusto = CriarCentroDeCusto();
        var parametros = {
            Url: 'CentroDeCusto/Salvar',
            Dados: CentroDeCusto,
            Done: Finalizar
        };
        $.postAjax(parametros);
    }
}
function CriarCentroDeCusto() {
    var centroDeCusto = {
        id_centro_de_custo: $("#id_centro_de_custo").val(),
        ds_codigo: $("#ds_codigo").val(),
        ds_descricao: $("#ds_descricao").val()
    }
    return centroDeCusto;
}

function Finalizar(json) {
    alert(json.msg);
    if (!json.erro) {
        window.location = $("#hdfUrlRoot").val() + "CentroDeCusto/Editar/" + json.id;
    }
}