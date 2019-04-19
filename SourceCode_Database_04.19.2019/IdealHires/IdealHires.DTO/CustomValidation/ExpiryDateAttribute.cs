using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdealHires.DTO.CustomValidation
{
   
    public class ExpiryDateAttribute : ValidationAttribute, IClientValidatable
    {
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "expirydaterequired"
            };
        }

        public override bool IsValid(object value)
        {
            DateTime _dateJoin = Convert.ToDateTime(value);
            if (_dateJoin >= DateTime.Now)
            {
                return true;
            }
            return false;
        }
    }
}
