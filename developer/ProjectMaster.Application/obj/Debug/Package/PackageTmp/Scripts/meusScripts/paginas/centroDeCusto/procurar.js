$(function () {
    AddBotaNovo();
})
function CriarBotoesCentroDeCusto(valorDaCelula) {
    return '<a href="#" ><img src="../../Content/images/buttons/editar.png"  onclick=" EditarCentroDeCusto(' + valorDaCelula + ')" width="16" height="16" /></a>' +
      '<a href="#" ><img src="../../Content/images/buttons/cancelar.png"  onclick=" DeletarCentroDeCusto(' + valorDaCelula + ')" width="16" height="16" /></a>';
}

function DeletarCentroDeCusto(id) {
    var parametros = {
        Url: 'CentroDeCusto/Deletar',
        Dados: { Id: id },
        Done: Finalizar
    };

    $.postAjax(parametros);
}
function Finalizar(json)
{
    alert(json.msg);
    $('#gridCentroDeCusto').trigger("reloadGrid");
}
function EditarCentroDeCusto(id) {
    window.location = $("#hdfUrlRoot").val() + "CentroDeCusto/Editar/" + id;
}
function NovoCentroDeCusto() {
    window.location = $("#hdfUrlRoot").val() + "CentroDeCusto/Novo";
}
function AddBotaNovo()
{
    var img = '<a href="#" ><img src="../../Content/images/buttons/adicionar.png"  onclick=" NovoCentroDeCusto()" width="24" height="24" /> </a>'
    $('#t_gridCentroDeCusto').append(img);
}