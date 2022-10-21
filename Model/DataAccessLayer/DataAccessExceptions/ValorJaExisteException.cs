using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.DataAccessExceptions
{
    public class ValorJaExisteException : DataAccessException
    {
        public object? Valor { get; }
        public string Nome { get; }

        public ValorJaExisteException()
        {

        }

        public ValorJaExisteException(string nome, object? valor)
            : this("O(a) " + nome + " " + valor == null ? "(vazio)" : valor.ToString() + " já existe na database")
        {
            Valor = valor;
            Nome = nome;
        }

        public ValorJaExisteException(string mensagem)
            : base(mensagem)
        {
        }

        public ValorJaExisteException(string mensagem, Exception excecaoInterna)
            : base(mensagem, excecaoInterna)
        {

        }

    }
}
