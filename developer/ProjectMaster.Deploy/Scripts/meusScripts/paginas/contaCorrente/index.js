var idContaCorrente;
$(function () {
    idContaCorrente = $('#cc_proximoId').val();
    $('#btnAddContaCorrente').click(AddContaCorrente);
    $('#btnEdtContaCorrente').click(EdtContaCorrente);
    $('#btnEdtContaCorrente').hide();
});
function AddContaCorrente() {
    if ($("#formContaCorrente").valid()) {
        var datarow = CriarContaCorrente(idContaCorrente);
        $("#gridContaCorrente").jqGrid('addRowData', datarow.id, datarow);
        idContaCorrente++;
        LimparFormContaCorrente();
    }
}
function EdtContaCorrente() {
    var form = $("#formContaCorrente");
    if (form.valid()) {
        var datarow = CriarContaCorrente($("#cc_id_edicao").val());
        $("#gridContaCorrente").jqGrid('setRowData', datarow.id, datarow);
        $('#btnEdtContaCorrente').hide();
        $("#cc_id_edicao").val("");
        LimparFormContaCorrente();
    }
} 
function LimparFormContaCorrente()
{
    Limpar($("#formContaCorrente")) ;
}
function CriarContaCorrente(id) {
    return {
        id: id,
        id_banco: $("#id_banco").val(),
        ds_banco: $("#id_banco  option:selected").text(),
        ds_agencia: $("#ds_agencia").val(),
        ds_numero_conta_corrente: $("#ds_numero_conta_corrente").val(),
        ds_emitente: $("#ds_emitente").val(),
        ds_conta_corrente: $("#ds_conta_corrente").val(),
        acoes: id
    };
}

function CriarBotoesContaCorrente(valorDaCelula) {
    return '<a href="#" ><img src="../../Content/images/buttons/editar.png"  onclick=" EditarContaCorrente(' + valorDaCelula + ')" width="16" height="16" /></a>' +
     '<a href="#" ><img src="../../Content/images/buttons/cancelar.png"  onclick=" DeletarContaCorrente(' + valorDaCelula + ')" width="16" height="16" /></a>';

}

function EditarContaCorrente(id) {
    var gr = $("#gridContaCorrente").jqGrid('getRowData', id);
    $("#cc_id_edicao").val(gr.id);
    $("#id_banco").val(gr.id_banco);
    $("#ds_agencia").val(gr.ds_agencia);
    $("#ds_numero_conta_corrente").val(gr.ds_numero_conta_corrente);
    $("#ds_emitente").val(gr.ds_emitente);
    $("#ds_conta_corrente").val(gr.ds_conta_corrente);
    $('#btnEdtContaCorrente').show();
}

function DeletarContaCorrente(id) {
    $("#gridContaCorrente").jqGrid('delRowData', id);
}
