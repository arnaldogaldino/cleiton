using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ProjectMaster.Bussiness.DataModels;

namespace ProjectMaster.Bussiness.Validations
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

        public override bool IsValid(object value)
        {
            if (new Produto().PegarProduto(value.ToString()) != null)
                return false;

            return base.IsValid(value);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (new Produto().PegarProduto(value.ToString()) != null)
                return ValidationResult.Success;

            return new ValidationResult(validationContext.DisplayName + " já existe no cadastro.");
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelClientValidationSelectOneRule("validationcprodexist", FormatErrorMessage(metadata.DisplayName), MsgOnTitle) };
        }
   
    }
}
