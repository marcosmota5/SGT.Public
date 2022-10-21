﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;

namespace SGT.Regras
{
    public class RegraComboBoxVazio : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Campo obrigatório");

            try
            {
                if ((int)value == -1)
                    return new ValidationResult(false, "Campo obrigatório");
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Campo obrigatório");
            }

            return new ValidationResult(true, null);
        }
    }
}