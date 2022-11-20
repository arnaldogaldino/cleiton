$(function () {
    idContaCorrente = 0;
    $('#btnSalvar').click(SalvarPessoa);
    $('#nr_ie').keypress(function () { mascaraInteiro(this) });
    $('#nr_inscricao_suframa').keypress(function () { mascaraInteiro(this) });
    $('#ds_fisico_juridico').change(function () { TrocarMascaraDocumento(true) });
    TrocarMascaraDocumento();
});
function TrocarMascaraDocumento(change) {
    var selector = $('#nr_documento');
    if (change) selector.val("");
    selector.unbind('keypress');
    selector.unbind('blur');
    if ($('#ds_fisico_juridico').val() == "C") {
        selector.attr('MaxLength', '14');
        selector.keypress(function () { MascaraCPF(this) });
        selector.blur(function () { ValidarCPF(this) });
    }
    else {
        selector.attr('MaxLength', '18');
        selector.keypress(function () { MascaraCNPJ(this) });
        selector.blur(function () { ValidarCNPJ(this) });
    }
}
function CriarPessoa() {
    var pessoa = {
        id_pessoa: $("#id_pessoa").val(),
        ds_marca: $("#ds_marca").val(),
        ds_razao_social: $("#ds_razao_social").val(),
        ds_nome_fantasia: $("#ds_nome_fantasia").val(),
        dm_tipo_pessoa: $("#dm_tipo_pessoa").val(),
        ds_fisico_juridico: $("#ds_fisico_juridico").val(),
        nr_documento: $("#nr_documento").val(),
        nr_ie: $("#nr_ie").val(),
        nr_inscricao_suframa: $("#nr_inscricao_suframa").val(),
        dm_dias_limite_credito: $("#dm_dias_limite_credito").val(),
        dm_venda_agendada: $("#dm_venda_agendada").val(),
        enderecos: PegarGrid($("#gridEndereco")),
        telefones: PegarGrid($("#gridTelefone")),
        contas_correntes: PegarGrid($("#gridContaCorrente"))
    }
    return pessoa;
}

function SalvarPessoa() {
    var pessoa = CriarPessoa();
    if (ValidarPessoa(pessoa)) {
        var parametros = {
            Url: 'Produto/Salvar',
            Dados: pessoa,
            Done: Finalizar
        };
        $.postAjax(parametros);
    }
}
function ValidarPessoa(pessoa) {
    if ($("#formProduto").valid()) {
        
        if (pessoa.enderecos.length == 0) {
            alert("Pelo menos um endereço deve ser cadastrado.");
            return false;
        }
        if (pessoa.telefones.length == 0) {
            alert("Pelo menos um telefone deve ser cadastrado.");
            return false;
        }
        if (pessoa.contas_correntes.length == 0) {
            alert("Pelo menos uma conta corrente deve ser cadastrada.");
            return false;
        }
        return true;
    }
    alert("Alguns campos obrigatórios não foram preenchidos.");
    return false;
}
function Finalizar(json) {
    $("#id_pessoa").val();
    alert(json.msg);
    window.location = $("#hdfUrlRoot").val() + "Produto/Editar/" + json.idPessoa;
}
