using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.DataAccessExceptions
{
    public class ChaveEstrangeiraEmUsoException : DataAccessException
    {
        public object? Valor { get; }
        public string Nome { get; }

        public ChaveEstrangeiraEmUsoException()
        {

        }

        public ChaveEstrangeiraEmUsoException(string nome, object? valor)
            : this("O(a) " + nome + " " + valor == null ? "(vazio)" : valor.ToString() + " está sendo usado(a) em outra tabela. Altere o status para inativo ou remova-o(a) de todas as tabelas e tente novamente")
        {
            Valor = valor;
            Nome = nome;
        }

        public ChaveEstrangeiraEmUsoException(string mensagem)
            : base(mensagem)
        {
        }

        public ChaveEstrangeiraEmUsoException(string mensagem, Exception excecaoInterna)
            : base(mensagem, excecaoInterna)
        {

        }
    }
}
