jQuery.validator.unobtrusive.adapters.add
    ("validationcprodexist", ['msgOnTitle'],
    function (options) {
        options.rules['validationcprodexist'] = {
            other: options.params.other,
            msgOnTitle: options.params.msgOnTitle
        };
        if(options.params.msgOnTitle)
            options.messages['validationcprodexist'] = options.message;
        else
            options.messages['validationcprodexist'] = options.message;
    }
);

jQuery.validator.addMethod("validationcprodexist", function (value, element, params) {
    var retVal = true;

    $.getJSON('/Produto/Validation', { validationType: 'cProd', value: value }, function (json) {
        if(!json.result)
            retVal = false;
    });
    
    return retVal;
});