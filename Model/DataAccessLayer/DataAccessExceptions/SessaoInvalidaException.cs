using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.DataAccessExceptions
{
    public class SessaoInvalidaException : DataAccessException
    {
        public object? Valor { get; }
        public string Nome { get; }

        public SessaoInvalidaException()
        {
        }

        public SessaoInvalidaException(string nome, object? valor)
            : this("Sessão inválida")
        {
            Valor = valor;
            Nome = nome;
        }

        public SessaoInvalidaException(string mensagem)
            : base(mensagem)
        {
        }

        public SessaoInvalidaException(string mensagem, Exception excecaoInterna)
            : base(mensagem, excecaoInterna)
        {
        }
    }
}