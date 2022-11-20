$(function () {
    AddBotaNovo();
})
function CriarBotoesMoeda(valorDaCelula) {
    return '<a href="#" ><img src="../../Content/images/buttons/editar.png"  onclick=" EditarMoeda(' + valorDaCelula + ')" width="16" height="16" /></a>' +
      '<a href="#" ><img src="../../Content/images/buttons/cancelar.png"  onclick=" DeletarMoeda(' + valorDaCelula + ')" width="16" height="16" /></a>';

}

function DeletarMoeda(id) {
    var parametros = {
        Url: 'Moeda/Deletar',
        Dados: { Id: id },
        Done: Finalizar
    };

    $.postAjax(parametros);
}
function Finalizar(json)
{
    alert(json.msg);
    $('#gridMoeda').trigger("reloadGrid");
}
function EditarMoeda(id) {
    window.location = $("#hdfUrlRoot").val() + "Moeda/Editar/" + id;
}
function NovaMoeda() {
    window.location = $("#hdfUrlRoot").val() + "Moeda/Novo";
}
function AddBotaNovo()
{
    var img = '<a href="#" ><img src="../../Content/images/buttons/adicionar.png"  onclick=" NovaMoeda()" width="24" height="24" /> </a>'
    $('#t_gridMoeda').append(img);
}