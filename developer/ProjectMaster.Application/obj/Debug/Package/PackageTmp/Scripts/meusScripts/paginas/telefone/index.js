
var idTelefone;
$(function () {
    CarregarGrid();
    idTelefone = $('#tell_proximoId').val();
    $('#btnAddTelefone').click(AddTelefone);
    $('#btnEdtTelefone').click(EdtTelefone);
    $('#btnEdtTelefone').hide();
    ConfigNrTelefone();
});
function ConfigNrTelefone() {
    $('#nr_telefone').attr('MaxLength', '14');
    $('#nr_telefone').keypress(function () {
        MascaraTelefone(this)
    });
    $('#nr_telefone').blur(function () {
        ValidaTelefone(this)
    });

}
function CarregarGrid() {
    var idPessoa = $("#id_pessoa").val();
    if (idPessoa != undefined && idPessoa > 0) {
        var param = {
            url: 'Telefone/Procurar',
            data: { idPessoa: idPessoa }
        };
        ReloadGrid($("#gridTelefone"), param);
    }
}
function AddTelefone() {
    if ($("#formTelefone").valid()) {
        var datarow = CriarTelefone(idTelefone);
        $("#gridTelefone").jqGrid('addRowData', datarow.id, datarow);
        idTelefone++;
        LimparFormTelefone();
    }
}
function EdtTelefone() {
    var form = $("#formTelefone");
    if (form.valid()) {
        var datarow = CriarTelefone($("#id_edicao").val());
        $("#gridTelefone").jqGrid('setRowData', datarow.id, datarow);
        $('#btnEdtTelefone').hide();
        $("#id_edicao").val("");
        LimparFormTelefone();
    }
} 
function LimparFormTelefone()
{
    Limpar($("#formTelefone")) ;
}
function CriarTelefone(idTelefone) {
    var tell = {
        id: idTelefone,
        dm_tipo_telefone: $("#dm_tipo_telefone").val(),
        ds_tipo_telefone: $("#dm_tipo_telefone  option:selected").text(),
        nr_telefone: $("#nr_telefone").val(),
        nm_contato: $("#nm_contato").val(),
        acoes: idTelefone
    };
    return tell;
}

function CriarBotoesTelefone(valorDaCelula) {
    return '<a href="#" ><img src="../../Content/images/buttons/editar.png"  onclick=" EditarTelefone(' + valorDaCelula + ')" width="16" height="16" /></a>' +
     '<a href="#" ><img src="../../Content/images/buttons/cancelar.png"  onclick=" DeletarTelefone(' + valorDaCelula + ')" width="16" height="16" /></a>';

}

function EditarTelefone(id) {
    var gr = $("#gridTelefone").jqGrid('getRowData', id);
    $("#id_edicao").val(gr.id);
    $("#dm_tipo_telefone").val(gr.dm_tipo_telefone);
    $("#nr_telefone").val(gr.nr_telefone);
    $("#nm_contato").val(gr.nm_contato);
    $('#btnEdtTelefone').show();
}

function DeletarTelefone(id) {
    $("#gridTelefone").jqGrid('delRowData', id);
}
