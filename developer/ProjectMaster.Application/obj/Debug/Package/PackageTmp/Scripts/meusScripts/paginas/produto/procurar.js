$(function () {
    AddBotaNovo();
})
function CriarBotoesProduto(valorDaCelula) {
    return '<a href="#" ><img src="../../Content/images/buttons/editar.png"  onclick=" EditarProduto(' + valorDaCelula + ')" width="16" height="16" /></a>' +
      '<a href="#" ><img src="../../Content/images/buttons/cancelar.png"  onclick=" DeletarProduto(' + valorDaCelula + ')" width="16" height="16" /></a>';
}

function DeletarProduto(id) {
    var parametros = {
        Url: 'Produto/Deletar',
        Dados: { Id: id },
        Done: Finalizar
    };

    $.postAjax(parametros);
}
function Finalizar(json)
{
    alert(json.msg);
    $('#gridProduto').trigger("reloadGrid");
}
function EditarProduto(id) {
    window.location = $("#hdfUrlRoot").val() + "Produto/Editar/" + id;
}
function NovaProduto() {
    window.location = $("#hdfUrlRoot").val() + "Produto/Novo";
}
function AddBotaNovo()
{
    var img = '<a href="#" ><img src="../../Content/images/buttons/adicionar.png"  onclick=" NovaProduto()" width="24" height="24" /> </a>'
    $('#t_gridProduto').append(img);
}