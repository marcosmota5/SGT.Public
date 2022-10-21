using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;

namespace SGT.Regras
{
    public class RegraCampoVazio : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var texto = "";

            if (value == null)
                return new ValidationResult(false, "Campo obrigatório");

            try
            {
                if (value.ToString().Length > 0)
                    texto = value.ToString();

                if (String.IsNullOrEmpty(texto))
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