function login_event() {

    if ($('#login').val() != "" && $('#senha').val() != "" && $('#empresa').val() != "" && $('#filial').val() != "") {
        $('#formulario_entrar').submit();
    } else {
        if ( $('#campos_empresa').css('display') == 'block' ) {
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

function ShowLoad() {
    var html = "<div id='ShowLoad' style='z-index: 99991; margin: 0px; position:absolute; display:block' class='ui-widget-overlay'><div id='load' class='load'><table width='100%' border='0' cellpadding='0' cellspacing='0'><tr><td width='25%' align='right' valign='middle'><img alt='Carregando' src='/Content/images/load.gif' width='16' height='16' />&nbsp;&nbsp;</td><td width='75%' align='left' valign='middle'><span>Carregando... </span></td></tr></table></div> </div>";
    var html2 = "<div id='ShowLoad' style='z-index: 99991; margin: 0px; position:absolute; display:block' class='ui-widget-overlay'><div id='load' class='load'><img alt='Carregando' src='/Content/images/load.gif' width='16' height='16' />&nbsp;&nbsp;&nbsp;<span>Carregando... </span></div> </div>";

    $("html").append(html);
}

function HideLoad() {
    $("#ShowLoad").fadeOut('slow');
    $("#ShowLoad").remove();
}

function delete_menu(id)
{
    $(function () {
        $("#dialog-confirm").dialog({
            resizable: false,
            height: 140,
            modal: true,
            buttons: {
                "Sim": function () {
                    $(this).dialog("close");

                    $("#lin" + id).fadeOut('slow');

                    ShowLoad();

                    $.getJSON('/Menu/MenuDelete', { id_menu: id }, function (json) {
                        if (json.success) {
                            $("#lin" + id).remove();
                            HideLoad();
                            return;
                        } else {
                            $("#lin" + id).fadeIn('slow');
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