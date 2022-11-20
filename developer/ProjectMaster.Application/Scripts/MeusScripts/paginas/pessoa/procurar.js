$(function () {
    AddBotaNovo();
})
function CriarBotoesPessoa(valorDaCelula) {
    return '<a href="#" ><img src="../../Content/images/buttons/editar.png"  onclick=" EditarPessoa(' + valorDaCelula + ')" width="16" height="16" /></a>' +
      '<a href="#" ><img src="../../Content/images/buttons/cancelar.png"  onclick=" DeletarPessoa(' + valorDaCelula + ')" width="16" height="16" /></a>';

}

function DeletarPessoa(id) {
    var parametros = {
        Url: 'Pessoa/Deletar',
        Dados: { Id: id },
        Done: Finalizar
    };

    $.postAjax(parametros);
}
function Finalizar(json)
{
    alert(json.msg);
    $('#gridPessoa').trigger("reloadGrid");
}
function EditarPessoa(id) {
    window.location = $("#hdfUrlRoot").val() + "Pessoa/Editar/" + id;
}
function NovaPessoa() {
    window.location = $("#hdfUrlRoot").val() + "Pessoa/Novo";
}
function AddBotaNovo()
{
    var img = '<a href="#" ><img src="../../Content/images/buttons/adicionar.png"  onclick=" NovaPessoa()" width="24" height="24" /> </a>'
    $('#t_gridPessoa').append(img);
}