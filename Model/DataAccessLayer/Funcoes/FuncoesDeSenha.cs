using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.Funcoes
{
    public static class FuncoesDeSenha
    {
        const int _tamanhoMinimo = 8;
        const int _quantidadeMaiusculas = 1;
        const int _quantidadeMinusculas = 1;
        const int _quantidadeNumeros = 1;
        const int _quantidadeCaractereEspecial = 1;


        public static bool SenhaEhValida(string senha)
        {

            if (!SenhaPossuiTamanhoMinimo(senha)) return false;
            if (!SenhaPossuiMaiusculas(senha)) return false;
            if (!SenhaPossuiMinusculas(senha)) return false;
            if (!SenhaPossuiNumeros(senha)) return false;
            if (!SenhaPossuiCaraceteresEspeciais(senha)) return false;

            return true;
        }

        public static bool SenhaPossuiTamanhoMinimo(string senha)
        {
            if (senha.Length < _tamanhoMinimo) return false;

            return true;
        }

        public static bool SenhaPossuiMaiusculas(string senha)
        {
            // Replace [A-Z] with \p{Lu}, to allow for Unicode uppercase letters.
            var maiuscula = new System.Text.RegularExpressions.Regex("[A-Z]");

            if (maiuscula.Matches(senha).Count < _quantidadeMaiusculas) return false;

            return true;
        }

        public static bool SenhaPossuiMinusculas(string senha)
        {
            // Replace [A-Z] with \p{Lu}, to allow for Unicode uppercase letters.
            var minuscula = new System.Text.RegularExpressions.Regex("[a-z]");

            if (minuscula.Matches(senha).Count < _quantidadeMinusculas) return false;

            return true;
        }

        public static bool SenhaPossuiNumeros(string senha)
        {
            // Replace [A-Z] with \p{Lu}, to allow for Unicode uppercase letters.
            var numero = new System.Text.RegularExpressions.Regex("[0-9]");
            // Special is "none of the above".
            if (numero.Matches(senha).Count < _quantidadeNumeros) return false;

            return true;
        }

        public static bool SenhaPossuiCaraceteresEspeciais(string senha)
        {
            var caractereEspecial = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]");

            if (caractereEspecial.Matches(senha).Count < _quantidadeCaractereEspecial) return false;

            return true;
        }

    }
}
