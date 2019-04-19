using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdealHires.DTO.CustomValidation
{
    public sealed class DateCompareAttribute : ValidationAttribute, IClientValidatable
    {
        private DateCompareOperator operatorname = DateCompareOperator.GreaterThanOrEqual;

        public string CompareToPropertyName { get; set; }
        public DateCompareOperator OperatorName { get { return operatorname; } set { operatorname = value; } }

        public DateCompareAttribute() : base() { }
        //Override IsValid
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string operstring = (OperatorName == DateCompareOperator.GreaterThan ?
            "greater than " : (OperatorName == DateCompareOperator.GreaterThanOrEqual ?
            "greater than or equal to " :
            (OperatorName == DateCompareOperator.LessThan ? "less than " :
            (OperatorName == DateCompareOperator.LessThanOrEqual ? "less than or equal to " : ""))));
            var basePropertyInfo = validationContext.ObjectType.GetProperty(CompareToPropertyName);

            var valOther = (IComparable)basePropertyInfo.GetValue(validationContext.ObjectInstance, null);

            var valThis = (IComparable)value;
            if (valThis != null)
            {
                if ((operatorname == DateCompareOperator.GreaterThan && valThis.CompareTo(valOther) <= 0) ||
                        (operatorname == DateCompareOperator.GreaterThanOrEqual && valThis.CompareTo(valOther) < 0) ||
                        (operatorname == DateCompareOperator.LessThan && valThis.CompareTo(valOther) >= 0) ||
                        (operatorname == DateCompareOperator.LessThanOrEqual && valThis.CompareTo(valOther) > 0))
                {
                    return new ValidationResult(base.ErrorMessage);
                }
                else
                {

                    return null;
                }
            }
            else
            {
                return ValidationResult.Success;
            }
        }


        #region IClientValidatable Members

        public IEnumerable<ModelClientValidationRule>
        GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
            ModelClientValidationRule compareRule = new ModelClientValidationRule();
            compareRule.ErrorMessage = errorMessage;
            compareRule.ValidationType = "datecompare";
            compareRule.ValidationParameters.Add("comparetopropertyname", CompareToPropertyName);
            compareRule.ValidationParameters.Add("operatorname", OperatorName.ToString());
            yield return compareRule;
        }

        #endregion
    }
}
