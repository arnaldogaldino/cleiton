$(function () {
    AddBotaNovo();
})
function CriarBotoesClassificacao(valorDaCelula) {
    return '<a href="#" ><img src="../../Content/images/buttons/editar.png"  onclick=" EditarClassificacao(' + valorDaCelula + ')" width="16" height="16" /></a>' +
      '<a href="#" ><img src="../../Content/images/buttons/cancelar.png"  onclick=" DeletarClassificacao(' + valorDaCelula + ')" width="16" height="16" /></a>';
}

function DeletarClassificacao(id) {
    var parametros = {
        Url: 'Classificacao/Deletar',
        Dados: { Id: id },
        Done: Finalizar
    };

    $.postAjax(parametros);
}
function Finalizar(json)
{
    alert(json.msg);
    $('#gridClassificacao').trigger("reloadGrid");
}
function EditarClassificacao(id) {
    window.location = $("#hdfUrlRoot").val() + "Classificacao/Editar/" + id;
}
function NovoClassificacao() {
    window.location = $("#hdfUrlRoot").val() + "Classificacao/Novo";
}
function AddBotaNovo()
{
    var img = '<a href="#" ><img src="../../Content/images/buttons/adicionar.png"  onclick=" NovoClassificacao()" width="24" height="24" /> </a>'
    $('#t_gridClassificacao').append(img);
}