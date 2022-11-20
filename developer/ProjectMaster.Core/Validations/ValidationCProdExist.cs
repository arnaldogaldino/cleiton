using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ProjectMaster.Core.Validations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class ValidationCProdExist : ValidationAttribute, IClientValidatable
    {
        private bool MsgOnTitle { get; set; }

        public ValidationCProdExist(string msg, bool msgOnTitle)
        {
            MsgOnTitle = msgOnTitle;
            ErrorMessage = msg;
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return new ValidationResult(validationContext.DisplayName + " já existe no cadastro.");
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelClientValidationSelectOneRule("validationcprodexist", FormatErrorMessage(metadata.DisplayName), MsgOnTitle) };
        }
    }
}
