using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.DataAccessExceptions
{
    public class ValorInativoException : DataAccessException
    {
        public object? Valor { get; }
        public string Nome { get; }

        public ValorInativoException()
        {

        }

        public ValorInativoException(string nome, object? valor)
            : this("O(a) " + nome + " " + valor == null ? "(vazio)" : valor.ToString() + " está inativo")
        {
            Valor = valor;
            Nome = nome;
        }

        public ValorInativoException(string mensagem)
            : base(mensagem)
        {
        }

        public ValorInativoException(string mensagem, Exception excecaoInterna)
            : base(mensagem, excecaoInterna)
        {

        }

    }
}
