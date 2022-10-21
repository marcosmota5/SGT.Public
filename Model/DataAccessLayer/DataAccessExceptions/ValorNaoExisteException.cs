using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.DataAccessExceptions
{
    public class ValorNaoExisteException : DataAccessException
    {
        public object? Valor { get; }
        public string Nome { get; }

        public ValorNaoExisteException()
        {

        }

        public ValorNaoExisteException(string nome, object? valor)
            : this("O(a) " + nome + " " + valor == null ? "(vazio)" : valor.ToString() + " não existe na database")
        {
            Valor = valor;
            Nome = nome;
        }

        public ValorNaoExisteException(string mensagem)
            : base(mensagem)
        {
        }

        public ValorNaoExisteException(string mensagem, Exception excecaoInterna)
            : base(mensagem, excecaoInterna)
        {

        }

    }
}
