
// JavaScript Document
//adiciona mascara de cnpj
function MascaraCNPJ(cnpj) {
    if (mascaraInteiro(cnpj) == false) {
        event.returnValue = false;
    }
    return formataCampo(cnpj, '00.000.000/0000-00', event);
}

//adiciona mascara de cep
function MascaraCep(cep) {
    if (mascaraInteiro(cep) == false) {
        event.returnValue = false;
    }
    return formataCampo(cep, '00.000-000', event);
}

//adiciona mascara de data
function MascaraDecimal(data) {
    if (mascaraInteiro(data) == false) {
        event.returnValue = false;
    }
    return formataCampo(data, '00.00', event);
}

//adiciona mascara de data
function MascaraData(data) {
    if (mascaraInteiro(data) == false) {
        event.returnValue = false;
    }
    return formataCampo(data, '00/00/0000', event);
}

//adiciona mascara ao telefone
function MascaraTelefone(tel) {
    if (mascaraInteiro(tel) == false) {
        event.returnValue = false;
    }
    return formataCampo(tel, '(00) 0000-0000', event);
}

//adiciona mascara ao CPF
function MascaraCPF(cpf) {
    if (mascaraInteiro(cpf) == false) {
        event.returnValue = false;
    }
    return formataCampo(cpf, '000.000.000-00', event);
}

//valida telefone
function ValidaTelefone(tel) {
    if (tel.value != "") {
        exp = /\(\d{2}\)\ \d{4}\-\d{4}/
        if (!exp.test(tel.value)) {
            tel.value = "";
            alert('Numero de Telefone Invalido!');
        }
    }
}

//valida CEP
function ValidaCep(cep) {
    if (cep.value != "") {
        exp = /\d{2}\.\d{3}\-\d{3}/
        if (!exp.test(cep.value)) {
            cep.value = "";
            alert('Numero de CEP Invalido!');
        }
    }
}

//valida data
function ValidaData(data) {
    if (data.value != "") {
        exp = /\d{2}\/\d{2}\/\d{4}/
        if (!exp.test(data.value)) {
            data.value = "";
            alert('Data Invalida!');
        }
    }
}

function ValidarCPF(Objcpf) {
    CPF = Objcpf.value;
    if (CPF == "") {
        return true;
    }

    var value = Objcpf.value.replace(/\./gi, "").replace(/-/gi, "");
    var isValid = true;
    var Soma;
    var Resto;
    Soma = 0;

    for (i = 1; i <= 9; i++) {
        Soma = Soma + parseInt(value.substring(i - 1, i)) * (11 - i);
    }
    Resto = (Soma * 10) % 11;
    if ((Resto == 10) || (Resto == 11)) {
        Resto = 0;
    }
    if (Resto != parseInt(value.substring(9, 10))) {
        isValid = false;
    }
    Soma = 0;
    for (i = 1; i <= 10; i++) {
        Soma = Soma + parseInt(value.substring(i - 1, i)) * (12 - i);
    }
    Resto = (Soma * 10) % 11;
    if ((Resto == 10) || (Resto == 11)) {
        Resto = 0;
    }
    if (Resto != parseInt(value.substring(10, 11))) {
        isValid = false;
    }
    if (!isValid) {
        alert('CPF Invalido!');
        Objcpf.value = "";

        return isValid;
    }
    else {
        return isValid;
    }
}

//valida numero inteiro com mascara
function mascaraInteiro() {
    if (event.keyCode < 48 || event.keyCode > 57) {
        event.returnValue = false;
        return false;
    }
    return true;
}

//valida o CNPJ digitado
function ValidarCNPJ(ObjCnpj) {
    if (ObjCnpj.value != "") {
        var cnpj = ObjCnpj.value;
        var valida = new Array(6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2);
        var dig1 = new Number;
        var dig2 = new Number;

        exp = /\.|\-|\//g
        cnpj = cnpj.toString().replace(exp, "");
        var digito = new Number(eval(cnpj.charAt(12) + cnpj.charAt(13)));

        for (i = 0; i < valida.length; i++) {
            dig1 += (i > 0 ? (cnpj.charAt(i - 1) * valida[i]) : 0);
            dig2 += cnpj.charAt(i) * valida[i];
        }
        dig1 = (((dig1 % 11) < 2) ? 0 : (11 - (dig1 % 11)));
        dig2 = (((dig2 % 11) < 2) ? 0 : (11 - (dig2 % 11)));

        if (((dig1 * 10) + dig2) != digito) {
            ObjCnpj.value = "";
            alert('CNPJ Invalido!');
        }
    }
}
//formata de forma generica os campos
function formataCampo(campo, Mascara, evento) {
    var boleanoMascara;

    var Digitato = evento.keyCode;
    exp = /\-|\.|\/|\(|\)| /g
    campoSoNumeros = campo.value.toString().replace(exp, "");

    var posicaoCampo = 0;
    var NovoValorCampo = "";
    var TamanhoMascara = campoSoNumeros.length;;

    if (Digitato != 8) { // backspace 
        for (i = 0; i <= TamanhoMascara; i++) {
            boleanoMascara = ((Mascara.charAt(i) == "-") || (Mascara.charAt(i) == ".")
                                                    || (Mascara.charAt(i) == "/"))
            boleanoMascara = boleanoMascara || ((Mascara.charAt(i) == "(")
                                                    || (Mascara.charAt(i) == ")") || (Mascara.charAt(i) == " "))
            if (boleanoMascara) {
                NovoValorCampo += Mascara.charAt(i);
                TamanhoMascara++;
            } else {
                NovoValorCampo += campoSoNumeros.charAt(posicaoCampo);
                posicaoCampo++;
            }
        }
        campo.value = NovoValorCampo;
        return true;
    } else {
        return true;
    }
}
function Limpar(ele) {

    ele.find(':input').each(function () {
        switch (this.type) {
            case 'password':
            case 'select-multiple':
            case 'select-one':
            case 'text':
            case 'textarea':
                $(this).val('');
                break;
            case 'checkbox':
            case 'radio':
                this.checked = false;
        }
    });

}
jQuery.extend(jQuery.validator.methods, {
    date: function (value, element) {
        return this.optional(element) || /^\d\d?\.\d\d?\.\d\d\d?\d?$/.test(value);
    },
    number: function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
    }
});