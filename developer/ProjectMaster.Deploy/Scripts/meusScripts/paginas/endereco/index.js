var idEndereco;
$(function () {
    idEndereco = $('#end_proximoId').val();
    $('#btnAddEndereco').click(AddEndereco);
    $('#btnEdtEndereco').hide();
    $('#btnEdtEndereco').click(EdtEndereco);
    $('#nr_cep').attr('MaxLength', '10')
    $('#nr_cep').keypress(function () { MascaraCep(this) });
    $('#nr_cep').blur(function () { ValidaCep(this) });
    $('#nr_numero').keypress(function () { mascaraInteiro(this) });
});
function AddEndereco() {
    if ($("#formEndereco").valid()) {
        var datarow = CriarEndereço(idEndereco);
        $("#gridEndereco").jqGrid('addRowData', datarow.id, datarow);
        idEndereco++;
        LimparFormEndereco();
    }
}
function EdtEndereco() {
    var form = $("#formEndereco");
    if (form.valid()) {
        var datarow = CriarEndereço($("#id_edicao_endereco").val());
        $("#gridEndereco").jqGrid('setRowData', datarow.id, datarow);
        $('#btnEdtEndereco').hide();
        $("#id_edicao_endereco").val("");
        LimparFormEndereco();
    }
}
function LimparFormEndereco() {
    Limpar($("#formEndereco"));
}

function CriarEndereço(id) {
    var endereco = {
        id: id,
        dm_tipo_endereco: $("#dm_tipo_endereco").val(),
        ds_tipo_endereco: $("#dm_tipo_endereco  option:selected").text(),
        nr_cep: $("#nr_cep").val(),
        nm_endereco: $("#nm_endereco").val(),
        nr_numero: $("#nr_numero").val(),
        ds_complemento: $("#ds_complemento").val(),
        ds_bairro: $("#ds_bairro").val(),
        ds_cidade: $("#ds_cidade").val(),
        ds_uf: $("#ds_uf").val(),
        id_pais: $("#id_pais").val(),
        ds_pais: $("#id_pais  option:selected").text(),
        acoes: id
    };

    return endereco;
}


function CriarBotoesEndereco(valorDaCelula) {
    return '<a href="#" ><img src="../../Content/images/buttons/editar.png"  onclick=" EditarEndereco(' + valorDaCelula + ')" width="16" height="16" /></a>' +
     '<a href="#" ><img src="../../Content/images/buttons/cancelar.png"  onclick=" DeletarEndereco(' + valorDaCelula + ')" width="16" height="16" /></a>';

}

function EditarEndereco(id) {
    var gr = $("#gridEndereco").jqGrid('getRowData', id);
    $("#id_edicao_endereco").val(gr.id);
    $("#dm_tipo_endereco").val(gr.dm_tipo_endereco);
    $("#nr_cep").val(gr.nr_cep);
    $("#nm_endereco").val(gr.nm_endereco);
    $("#nr_numero").val(gr.nr_numero);
    $("#ds_complemento").val(gr.ds_complemento);
    $("#ds_bairro").val(gr.ds_bairro);
    $("#ds_cidade").val(gr.ds_cidade);
    $("#ds_uf").val(gr.ds_uf);
    $("#id_pais").val(gr.id_pais);
    $('#btnEdtEndereco').show();
}

function DeletarEndereco(id) {
    $("#gridEndereco").jqGrid('delRowData', id);
}
