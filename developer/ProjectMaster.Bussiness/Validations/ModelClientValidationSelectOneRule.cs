using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.ComponentModel.DataAnnotations;

namespace ProjectMaster.Bussiness.Validations
{
    public class ModelClientValidationSelectOneRule : ModelClientValidationRule
    {
        public ModelClientValidationSelectOneRule(string validationType, string errorMessage, bool msgOnTitle)
        {
            ErrorMessage = errorMessage;
            ValidationType = validationType;
            ValidationParameters.Add("msgontitle", msgOnTitle);
        }
    }
}
