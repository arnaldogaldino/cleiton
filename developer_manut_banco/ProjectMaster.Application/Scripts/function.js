function login_event() {

    if ($('#login').val() != "" && $('#senha').val() != "" && $('#empresa').val() != "" && $('#filial').val() != "") {
        $('#formulario_entrar').submit();
    } else {
        if ($('#campos_empresa').css('display') == 'block') {
            if ($('#empresa').val() == "" || $('#filial').val() == "") {
                $('#msg_error').html("Informe a empresa ou filial <br /> para continuar.");
                return;
            }
        }
    }

    ShowLoad();
    $.getJSON('/Usuario/GetEmpresaByUsuarioSenha', { user: $('#login').val(), password: $('#senha').val() }, function (json) {

        if (json.login_falha != '') {
            $('#msg_error').html(json.login_falha);
            HideLoad();
        } else {
            $('#msg_error').html("");
        }

        if (json.success) {

            var empresa = json.results;
            var count_empresa = 0;
            var id_empresa = 0;
            var count_filial = 0;
            var id_filial = 0;

            if (empresa != null) {
                $('#campos_empresa').css('display', 'block');
                $('#campos_filial').css('display', 'block');

                $('#login').attr('readonly', 'true');
                $('#senha').attr('readonly', 'true');

                if (empresa != null) {
                    if (empresa.length > 1) {
                        var options = '<option value="">Selecione...</option>';
                        for (var i = 0; i < empresa.length; i++) {
                            options += '<option value="' + empresa[i].id_empresa + '">' + empresa[i].apelido + '</option>';
                        }
                        $('#empresa').html(options).show();
                    } else {
                        for (var i = 0; i < empresa.length; i++) {
                            options += '<option value="' + empresa[i].id_empresa + '">' + empresa[i].apelido + '</option>';
                        }
                    }
                }

                if (empresa.length == 1) {
                    $('#campos_empresa').css('display', 'none');
                    $('#campos_filial').css('display', 'none');

                    $('#empresa').change();
                    $('#formulario_entrar').submit();
                    return;
                }

                $('#empresa').change(function () {
                    ShowLoad();

                    if ($('#empresa').val() == "") {
                        var options = '<option value="">Selecione...</option>';
                        $('#filial').html(options);
                        HideLoad();
                        return;
                    }

                    $.getJSON('/Usuario/GetFilialByUsuarioSenha', { user: $('#login').val(), password: $('#senha').val(), id_empresa: $('#empresa').val() }, function (json) {
                        if (json.results != null) {
                            if (json.results.length > 1) {
                                var options = '<option value="">Selecione...</option>';
                                for (var i = 0; i < json.results.length; i++) {
                                    options += '<option value="' + json.results[i].id_filial + '">' + json.results[i].apelido + '</option>';
                                }
                                $('#filial').html(options).show();
                            } else {
                                for (var i = 0; i < json.results.length; i++) {
                                    options += '<option value="' + json.results[i].id_filial + '">' + json.results[i].apelido + '</option>';
                                }
                            }

                        }
                        HideLoad();
                    });
                });

            }

            HideLoad();
        }
        else {
            HideLoad();
        }
    });
}

function baseUrl() {
    var href = window.location.href.split('/');
    var result = "";

    for (i = 0; i < (href.length - 1) ; i++) {
        result += href[i] + '/';
    }
    return result;
}

function ShowLoad() {
    var html = "<div id='ShowLoad' style='z-index: 99991; margin: 0px; position:absolute; display:block' class='ui-widget-overlay'></div><div id='load' class='load'><table width='100%' border='0' cellpadding='0' cellspacing='0'><tr><td width='25%' align='right' valign='middle'><img alt='Carregando' src='/Content/images/load.gif' width='16' height='16' />&nbsp;&nbsp;</td><td width='75%' align='left' valign='middle'><span>Carregando... </span></td></tr></table></div>";
    var html2 = "<div id='ShowLoad' style='z-index: 99991; margin: 0px; position:absolute; display:block' class='ui-widget-overlay'></div><div id='load' class='load'><img alt='Carregando' src='/Content/images/load.gif' width='16' height='16' />&nbsp;&nbsp;&nbsp;<span>Carregando... </span></div>";

    $("html").append(html);
}

function HideLoad() {
    $("#load").fadeOut('slow');
    $("#ShowLoad").fadeOut('slow');

    $("#load").remove();
    $("#load").remove();
    $("#load").remove();
    $("#load").remove();
    $("#ShowLoad").remove();    
}

function delete_file(id, controller, action) {
    $(function () {
        $("#dialog-confirm").dialog({
            resizable: false,
            height: 140,
            modal: true,
            buttons: {
                "Sim": function () {
                    $(this).dialog("close");
                    if (action == "rows") {
                        $("#lin" + id).fadeOut('slow');
                    }
                    ShowLoad();

                    $.getJSON('/' + controller + '/Delete', { id: id }, function (json) {
                        if (json.success) {
                            if (action == "rows") {
                                $("#lin" + id).remove();
                            } else {
                                window.location = baseUrl() + "Index"
                            }
                            HideLoad();
                            return;
                        } else {
                            if (action == "rows") {
                                $("#lin" + id).fadeIn('slow');
                            }
                            HideLoad();

                            var msgHtml = "" +
"<div id='dialog-message' title='Download complete'>" +
"    <p>" +
"        <span class='ui-icon ui-icon-alert' style='float: left; margin: 0 7px 50px 0;'></span>" +
"        <b>" + json.error +
                            "    </b></p>" +
"</div> ";

                            $("html").append(msgHtml);

                            $("#dialog-message").dialog({
                                modal: true,
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                    }
                                }
                            });

                            return;
                        }
                    });

                    HideLoad();

                    return;
                },
                "Não": function () {
                    $(this).dialog("close");
                }
            }
        });
    });
}

function LookupPessoa(marca, razao_social, id) {
    ShowLoad();

    $.getJSON('/Pessoa/Lookup', { ds_marca: $("#" + marca).val() }, function (json) {

        $("#" + marca).val("");
        $("#" + razao_social).val("");
        $("#" + id).val("");

        if (json.success) {
            if (json.list.length == 1) {
                HideLoad();
                $('#' + marca).val(json.list[0].ds_marca);
                $('#' + razao_social).val(json.list[0].ds_razao_social);
                $('#' + id).val(json.list[0].id_pessoa);

            } else if (json.list.length > 1) {
                var msgHtml = "<div id='dialog-lookup-cliente' title='Pesquisa de Pessoas' style='overflow: hidden'>" +
                          "    <iframe src='../Pessoa/LookupScreen' width='100%' height='100%' frameborder='0'></iframe>" +
                          "</div> ";

                $("html").append(msgHtml);

                $("#dialog-lookup-cliente").dialog({
                    modal: true,
                    width: 800,
                    height: 600,
                    closeOnEscape: false,
                    close: function (event, ui) {
                        $("#dialog-lookup-cliente").remove();
                    }
                });
                HideLoad();
            }
        } else {
            var msgHtml = "<div id='dialog-lookup-cliente' title='Pesquisa de Pessoas' style='overflow: hidden'>" +
                          "    <iframe src='../Pessoa/LookupScreen' width='100%' height='100%' frameborder='0'></iframe>" +
                          "</div> ";

            $("html").append(msgHtml);

            $("#dialog-lookup-cliente").dialog({
                modal: true,
                width: 800,
                height: 600,
                closeOnEscape: true,
                close: function (event, ui) {
                    $("#dialog-lookup-cliente").remove();
                }
            });
            HideLoad();
        }
    });
}

function LoockProduto(cprod, xprod, id, valor_unitario, tp_embalagem) {
    ShowLoad();

    $.getJSON('/Produto/Lookup', { cprod: $("#" + cprod).val() }, function (json) {

        $("#" + cprod).val("");
        $("#" + xprod).val("");

        if (valor_unitario != "")
            $("#" + valor_unitario).val("");

        if (tp_embalagem != "")
            $("#" + tp_embalagem).val("");

        if (json.success) {
            if (json.list.length == 1) {
                HideLoad();
                $('#' + cprod).val(json.list[0].cprod);
                $('#' + xprod).val(json.list[0].xprod);

                if (id != "")
                    $('#' + id).val(json.list[0].id_produto);

                if (valor_unitario != "")
                    $("#" + valor_unitario).val(json.list[0].valor_unitario.toFixed(2).replace('.', ','));

                if (tp_embalagem != "")
                    $("#" + tp_embalagem).val(json.list[0].tp_embalagem);

                if (valor_unitario != "")
                    $("#quantidade").focus();

            } else if (json.list.length > 1) {
                var msgHtml = "<div id='dialog-lookup-produto' title='Pesquisa de Produtos' style='overflow: hidden'>" +
                          "    <iframe src='../Produto/LookupScreen' width='100%' height='100%' frameborder='0'></iframe>" +
                          "</div> ";

                $("html").append(msgHtml);

                $("#dialog-lookup-produto").dialog({
                    modal: true,
                    width: 800,
                    height: 600,
                    closeOnEscape: false,
                    close: function (event, ui) {
                        $("#dialog-lookup-produto").remove();
                    }
                });
                HideLoad();
            }
        } else {
            var msgHtml = "<div id='dialog-lookup-produto' title='Pesquisa de Produtos' style='overflow: hidden'>" +
                          "    <iframe src='../Produto/LookupScreen' width='100%' height='100%' frameborder='0'></iframe>" +
                          "</div> ";

            $("html").append(msgHtml);

            $("#dialog-lookup-produto").dialog({
                modal: true,
                width: 800,
                height: 600,
                closeOnEscape: true,
                close: function (event, ui) {
                    $("#dialog-lookup-produto").remove();
                }
            });
            HideLoad();
        }
    });
}

function LoockProdutoEstoqueHistorico(id) {
    ShowLoad();

    var msgHtml = "<div id='dialog-lookup-produto-estoque_historico' title='Histórico de Estoque de Produtos' style='overflow: hidden'>" +
                  "    <iframe src='../Estoque/ShowHistoricoEstoque?id_produto=" + id + "' width='100%' height='100%' frameborder='0'></iframe>" +
                  "</div> ";

    $("html").append(msgHtml);        

    $("#dialog-lookup-produto-estoque_historico").dialog({
        modal: true,
        width: 800,
        height: 600,
        closeOnEscape: false,
        close: function (event, ui) {
            $("#dialog-lookup-produto-estoque_historico").remove();
        }
    });

    HideLoad();
}

function calcular_total_produto(campo_valor, campo_qtd, campo_total) {
    var valor_unitario = $("#" + campo_valor).val().replace(".", "").replace(",", ".");
    var qtd = $("#" + campo_qtd).val().replace(".", "").replace(",", ".");
    var total = (valor_unitario * qtd);

    $("#" + campo_total).val(total.toFixed(2).replace('.', ','));
    $("#" + campo_total).setMask();
}

function GetTipoEmpalagem(tipo) {
    switch (tipo) {
        case "CX": return "Caixa";
        case "SC": return "Saco";
        case "KG": return "Kilo";
        case "UN": return "Unidade";
        case "DC": return "Descartável";
        case "PP": return "Papelão";
        case "GR": return "Granel";
        default: "";
    }
}

function GetTipoTelefone(tipo) {
    switch (tipo) {
        case "C" : return "Comercial";
        case "R" : return "Residencial";
        case "M": return "Móvel";
        default: "";
    }
}

function GetTipoEndereco(tipo) {
    switch (tipo) {
        case "C": return "Comercial";
        case "R": return "Residencial";
        case "E": return "Entrega";
        default: "";
    }
}

var __nonMSDOMBrowser = (window.navigator.appName.toLowerCase().indexOf('explorer') == -1);

function WebForm_FireDefaultButton(event, target) {
    if (event.keyCode == 13) {
        var src = event.srcElement || event.target;
        if (src &&
            ((src.tagName.toLowerCase() == "input") &&
            (src.type.toLowerCase() == "submit" || src.type.toLowerCase() == "button")) ||
            ((src.tagName.toLowerCase() == "a") &&
            (src.href != null) && (src.href != "")) ||
            (src.tagName.toLowerCase() == "textarea")) {
            return true;
        }
        var defaultButton;
        if (__nonMSDOMBrowser) {
            defaultButton = document.getElementById(target);
        }
        else {
            defaultButton = document.all[target];
        }
        if (defaultButton) {
            return WebForm_SimulateClick(defaultButton, event);
        }
    }
    return true;
}

function WebForm_SimulateClick(element, event) {
    var clickEvent;
    if (element) {
        if (element.click) {
            element.click();
        } else {
            clickEvent = document.createEvent("MouseEvents");
            clickEvent.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
            if (!element.dispatchEvent(clickEvent)) {
                return true;
            }
        }
        event.cancelBubble = true;
        if (event.stopPropagation) {
            event.stopPropagation();
        }
        return false;
    }
    return true;
}