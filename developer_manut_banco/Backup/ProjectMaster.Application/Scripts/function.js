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

    for (i = 0; i < (href.length - 1); i++) {
        result += href[i] + '/';
    }
    return result;
}

function ShowLoad() {
    var html = "<div id='ShowLoad' style='z-index: 99991; margin: 0px; position:absolute; display:block' class='ui-widget-overlay'><div id='load' class='load'><table width='100%' border='0' cellpadding='0' cellspacing='0'><tr><td width='25%' align='right' valign='middle'><img alt='Carregando' src='/Content/images/load.gif' width='16' height='16' />&nbsp;&nbsp;</td><td width='75%' align='left' valign='middle'><span>Carregando... </span></td></tr></table></div> </div>";
    var html2 = "<div id='ShowLoad' style='z-index: 99991; margin: 0px; position:absolute; display:block' class='ui-widget-overlay'><div id='load' class='load'><img alt='Carregando' src='/Content/images/load.gif' width='16' height='16' />&nbsp;&nbsp;&nbsp;<span>Carregando... </span></div> </div>";

    $("html").append(html);
}

function HideLoad() {
    $("#ShowLoad").fadeOut('slow');
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

function LoockPessoa(marca, razao_social, id) {
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
                var msgHtml = "<div id='dialog-lookup' title='Pesquisa de Pessoas'>" +
                          "    <iframe src='../Pessoa/LookupScreen' width='100%' height='100%' frameborder='0'></iframe>" +
                          "</div> ";
//                var msgHtml = "<div id='dialog-lookup' title='Pesquisa de Pessoas'><object data='../Pessoa/LookupScreen' type='text/html' id='ob'  width='100%' height='100%'></object></div>";

                $("html").append(msgHtml);

                $("#dialog-lookup").dialog({
                    modal: true,
                    width: 800,
                    height: 600,
                    closeOnEscape: false,
                    close: function (event, ui) {
                        $("#dialog-lookup").remove();
                    }
                });
                HideLoad();
            }
        } else {
            var msgHtml = "<div id='dialog-lookup' title='Pesquisa de Pessoas'>" +
                          "    <iframe src='../Pessoa/LookupScreen' width='100%' height='100%' frameborder='0'></iframe>" +
                          "</div> ";
            //                var msgHtml = "<div id='dialog-lookup' title='Pesquisa de Pessoas'><object data='../Pessoa/LookupScreen' type='text/html' id='ob'  width='100%' height='100%'></object></div>";

            $("html").append(msgHtml);

            $("#dialog-lookup").dialog({
                modal: true,
                width: 800,
                height: 600,
                closeOnEscape: false,
                close: function (event, ui) {
                    $("#dialog-lookup").remove();
                }
            });
            HideLoad();


//            HideLoad();
//            var msgHtml = "<div id='dialog-message' title='Pesquisa de Pessoas'>" +
//                            "    <p>" +
//                            "        <span class='ui-icon ui-icon-alert' style='float: left; margin: 0 7px 50px 0;'></span>" +
//                            "        <b>" + json.error +
//                            "    </b></p>" +
//                            "</div> ";

//            $("html").append(msgHtml);

//            $("#dialog-message").dialog({
//                modal: true,
//                buttons: {
//                    Ok: function () {
//                        $(this).dialog("close");
//                    }
//                }
//            });
        }
    });
}

function LoockProduto(cprod, xprod, id) {
    ShowLoad();

    $.getJSON('/Produto/Lookup', { cprod: $("#" + cprod).val() }, function (json) {

        $("#" + cprod).val("");
        $("#" + xprod).val("");
        $("#" + id).val("");

        if (json.success) {
            //            if (json.list.length == 1) {
            //                HideLoad();
            //                $('#' + marca).val(json.list[0].ds_marca);
            //                $('#' + razao_social).val(json.list[0].ds_razao_social);
            //                $('#' + id).val(json.list[0].id_pessoa);
            //            } else if (json.list.length > 1) {

            var msgHtml = "<div id='dialog-lookup' title='Pesquisa de Produto'>" +
                          "    <iframe src='../Produto/LookupScreen' width='100%' height='100%' frameborder='0'></iframe>" +
                          "</div> ";
//            var msgHtml = "<div id='dialog-lookup' title='Pesquisa de Produto'><object data='../Produto/LookupScreen' type='text/html' id='ob'  width='100%' height='100%'></object></div>";

            $("html").append(msgHtml);

            $("#dialog-lookup").dialog({
                modal: true,
                width: 800,
                height: 600,
                closeOnEscape: false,
                close: function (event, ui) {
                    $("#dialog-lookup").remove();
                }
            });
            HideLoad();
            //            }
        } else {
            HideLoad();
            var msgHtml = "<div id='dialog-message' title='Pesquisa de Produto'>" +
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
        }
    });
}


function calcular_total_produto(campo_valor, campo_qtd, campo_total) {
    var valor_unitario = $("#" + campo_valor).val().replace(".", "").replace(",", ".");
    var qtd = $("#" + campo_qtd).val();
    var total = (valor_unitario * qtd);

    $("#" + campo_total).val(total.toFixed(2).replace('.', ','));
    $("#" + campo_total).setMask();
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