using Model.Email;
using System;
using System.Globalization;
using System.Windows.Controls;

namespace SGT.Regras
{
    public class RegraCampoInvalido : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (String.IsNullOrEmpty(value?.ToString()))
                return new ValidationResult(true, null);

            try
            {
                return new ValidationResult(Email.EmailEhValido(value.ToString()), null);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Campo inválido");
            }
            
        }
    }
}
