﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.CustomValidation
{
    public class ValidateAtLeastOneCheckedAttribute : ValidationAttribute
    {
        private string[] _checkboxNames;

        public ValidateAtLeastOneCheckedAttribute(params string[] checkboxNames)
        {
            _checkboxNames = checkboxNames;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Type type = value.GetType();
            IEnumerable<PropertyInfo> checkBoxeProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.PropertyType == typeof(bool));

            foreach (PropertyInfo checkBoxProperty in checkBoxeProperties)
            {
                var isChecked = (bool)checkBoxProperty.GetValue(value);
                if (isChecked)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(base.ErrorMessageString);
        }
    }
}
