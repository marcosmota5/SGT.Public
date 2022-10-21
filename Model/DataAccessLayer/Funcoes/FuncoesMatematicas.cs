using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.Funcoes
{
    public static class FuncoesMatematicas
    {
        /// <summary>
        /// Efetua uma divisão entre decimais e retorna o valor, retornando 0 em caso de erro ou denominador 0
        /// </summary>
        /// <param name="numerador">Numerador a ser utilizado para a divisão</param>
        /// <param name="denoninador">Denominador a ser utilizado para a divisão</param>
        /// <returns>Retorna um valor decimal com o resultado da divisão ou 0 caso a divisão não seja possível</returns>
        public static decimal? DividirPorZeroDecimal(decimal? numerador, decimal? denoninador)
        {
            try
            {
                if (numerador == null || denoninador == null || denoninador == 0)
                {
                    return 0;
                }
                return numerador / denoninador;
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
            catch(Exception)
            {
                return null;
                throw;
            }
        }

    }
}
