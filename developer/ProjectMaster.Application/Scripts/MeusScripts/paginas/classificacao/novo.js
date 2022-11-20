$(function () {
    idContaCorrente = 0;
    $('#btnSalvar').click(SalvarClassificacao);
    if ($('#id_classificacao').val() > 0) {
        $("#ds_codigo").attr('readonly', 'true');
    }
});

function SalvarClassificacao() {
    var form = $("#formClassificacao");
    if (form.valid()) {
        var classificacao = CriarClassificacao();
        var parametros = {
            Url: 'Classificacao/Salvar',
            Dados: classificacao,
            Done: Finalizar
        };
        $.postAjax(parametros);
    }
}
function CriarClassificacao() {
    var classificacao = {
        id_classificacao: $("#id_classificacao").val(),
        ds_codigo: $("#ds_codigo").val(),
        ds_descricao: $("#ds_descricao").val()
    }
    return classificacao;
}

function Finalizar(json) {
    alert(json.msg);
    if (!json.erro) {
        window.location = $("#hdfUrlRoot").val() + "Classificacao/Editar/" + json.id;
    }
}