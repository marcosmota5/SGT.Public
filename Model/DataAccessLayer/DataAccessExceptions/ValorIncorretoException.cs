using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.DataAccessExceptions
{
    public class ValorIncorretoException : DataAccessException
    {
        public object? Valor { get; }
        public string Nome { get; }

        public ValorIncorretoException()
        {

        }

        public ValorIncorretoException(string nome, object? valor)
            : this("O(a) " + nome + " " + valor == null ? "(vazio)" : valor.ToString() + " está incorreto")
        {
            Valor = valor;
            Nome = nome;
        }

        public ValorIncorretoException(string mensagem)
            : base(mensagem)
        {
        }

        public ValorIncorretoException(string mensagem, Exception excecaoInterna)
            : base(mensagem, excecaoInterna)
        {

        }

    }
}
