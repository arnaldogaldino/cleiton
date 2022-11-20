function ReloadGrid(selectorGrid, param) {
    if (param != null) {
        param.url = $("#hdfUrlRoot").val() + param.url;
        selectorGrid.setGridParam({ url: param.url, datatype: 'json', postData: param.data }).trigger('reloadGrid');
    }
    else {
        selectorGrid.setGridParam({ datatype: 'json' }).trigger('reloadGrid');
    }
}

function PegarGrid(selector) {
    var ids = selector.jqGrid('getDataIDs');
    var list = [];
    for (id in ids) {
        var row = selector.jqGrid('getRowData', id);
        if (row) {
            list.push(row);
        }
    }
    return list;
}