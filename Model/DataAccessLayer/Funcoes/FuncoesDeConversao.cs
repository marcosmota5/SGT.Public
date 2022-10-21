namespace Model.DataAccessLayer.Funcoes
{
    public static class FuncoesDeConversao
    {
        /// <summary>
        /// Converte um valor para uma string considerando o valor DBNull de databases
        /// </summary>
        /// <param name="valorOrigem">Representa o valor a ser convertido</param>
        /// <returns>Retorna uma string ou um valor nulo</returns>
        public static string? ConverteParaString(object? valorOrigem)
        {
            return Convert.IsDBNull(valorOrigem) ? null : Convert.ToString(valorOrigem);
        }

        /// <summary>
        /// Converte um valor para um int considerando o valor DBNull de databases
        /// </summary>
        /// <param name="valorOrigem">Representa o valor a ser convertido</param>
        /// <returns>Retorna um int ou um valor nulo</returns>
        public static int? ConverteParaInt(object? valorOrigem)
        {
            return Convert.IsDBNull(valorOrigem) ? null : Convert.ToInt32(valorOrigem);
        }

        /// <summary>
        /// Converte um valor para um decimal considerando o valor DBNull de databases
        /// </summary>
        /// <param name="valorOrigem">Representa o valor a ser convertido</param>
        /// <returns>Retorna um decimal ou um valor nulo</returns>
        public static decimal? ConverteParaDecimal(object? valorOrigem)
        {
            return Convert.IsDBNull(valorOrigem) ? null : Convert.ToDecimal(valorOrigem);
        }

        /// <summary>
        /// Converte um valor para um double considerando o valor DBNull de databases
        /// </summary>
        /// <param name="valorOrigem">Representa o valor a ser convertido</param>
        /// <returns>Retorna um double ou um valor nulo</returns>
        public static double? ConverteParaDouble(object? valorOrigem)
        {
            return Convert.IsDBNull(valorOrigem) ? null : Convert.ToDouble(valorOrigem);
        }

        /// <summary>
        /// Converte um valor para um datetime considerando o valor DBNull de databases
        /// </summary>
        /// <param name="valorOrigem">Representa o valor a ser convertido</param>
        /// <returns>Retorna um datetime ou um valor nulo</returns>
        public static DateTime? ConverteParaDateTime(object? valorOrigem)
        {
            return Convert.IsDBNull(valorOrigem) ? null : Convert.ToDateTime(valorOrigem);
        }

        /// <summary>
        /// Converte um valor para um bool considerando o valor DBNull de databases
        /// </summary>
        /// <param name="valorOrigem">Representa o valor a ser convertido</param>
        /// <returns>Retorna um bool ou um valor nulo</returns>
        public static bool? ConverteParaBool(object? valorOrigem)
        {
            return Convert.IsDBNull(valorOrigem) ? null : Convert.ToBoolean(valorOrigem);
        }

        /// <summary>
        /// Converte um valor para um array de bytes considerando o valor DBNull de databases
        /// </summary>
        /// <param name="valorOrigem">Representa o valor a ser convertido</param>
        /// <returns>Retorna um array de bytes ou um valor nulo</returns>
        public static byte[]? ConverteParaArrayDeBytes(object? valorOrigem)
        {
            if (Convert.IsDBNull(valorOrigem))
            {
                return null;
            }

            if (valorOrigem == null)
            {
                return null;
            }

            return (byte[])valorOrigem;
        }

        /// <summary>
        /// Converte um valor para string evitando erros de referência nula
        /// </summary>
        /// <param name="valorAConverter">Valor a converter</param>
        /// <returns>String convertida ou vazia</returns>
        public static string ConverteValorParaStringFailSafe(object valorAConverter)
        {
            try
            {
                return valorAConverter == null ? "" : Convert.ToString(valorAConverter);
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
