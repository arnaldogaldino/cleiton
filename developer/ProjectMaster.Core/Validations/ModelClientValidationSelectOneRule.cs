using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ProjectMaster.Core.Validations
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
