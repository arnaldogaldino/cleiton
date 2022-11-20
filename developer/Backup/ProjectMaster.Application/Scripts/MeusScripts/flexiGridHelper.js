
function Grid(seletorGrid, funcaoEditar, funcaoDeletar) {
    this.seletorGrid = seletorGrid
    if (funcaoEditar) { var editar = funcaoEditar; }
    if (funcaoDeletar) { var deletar = funcaoDeletar; }
    this.Montar = function (parametros) {
        seletorGrid.flexigrid({
            url: $("#hdfUrlRoot").val() + parametros.Url,
            dataType: 'json',
            colModel: parametros.ColModels,
            buttons: [{ name: 'Novo', bclass: 'add', onpress: this.Novo },
                      { name: 'Editar', bclass: 'edit', onpress: this.Editar },
                      { name: 'Delete', bclass: 'delete', onpress: this.Deletar}],
            sortname: "Id",
            sortorder: "asc",
            nameGrid: parametros.NomeGrid,
            usepager: true,
            useRp: false,
            rp: 10,
        });
    }
    this.Novo = function () {
        alert('Não definido');
    }
    this.Deletar = function (com, grid) {
        $('.trSelected', grid).each(function () {
            var id = $(this).attr('Id');
            id = id.substring(id.lastIndexOf('row') + 3);
            deletar(id);
        });
    }
    this.Editar = function (com, grid) {
        $('.trSelected', grid).each(function () {
            var id = $(this).attr('Id');
            id = id.substring(id.lastIndexOf('row') + 3);
            editar(id);
        });
    }
    this.Carregar = function (data) {
        seletorGrid.flexAddData(data);
    }
}