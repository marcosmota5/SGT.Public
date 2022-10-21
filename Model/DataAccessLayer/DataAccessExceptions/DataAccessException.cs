using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.DataAccessExceptions
{
    public class DataAccessException : Exception
    {
        public DataAccessException()
        {

        }

        public DataAccessException(string mensagem)
            : base(mensagem)
        {

        }

        public DataAccessException(string mensagem, Exception excecaoInterna)
            : base(mensagem, excecaoInterna)
        {

        }
    }
}
