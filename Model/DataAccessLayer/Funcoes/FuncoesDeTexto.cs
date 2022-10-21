using System.Text.RegularExpressions;

namespace Model.DataAccessLayer.Funcoes
{
    public static class FuncoesDeTexto
    {
        /// <summary>
        /// Remove todos os caracteres de um texto que não sejam números
        /// </summary>
        /// <param name="textoOrigem">Texto a ter os caraceteres removidos</param>
        /// <param name="retornoCasoNuloOuVazio">Retorno auxiliar caso o texto seja nulo</param>
        /// <returns>Retorna uma string com apenas números</returns>
        public static string? MantemApenasNumeros(string? textoOrigem, string? retornoCasoNuloOuVazio = null)
        {
            if (String.IsNullOrEmpty(textoOrigem))
            {
                return retornoCasoNuloOuVazio;
            }

            if (String.IsNullOrEmpty(Regex.Replace(textoOrigem, @"[^\d]", "")))
            {
                return retornoCasoNuloOuVazio;
            }
            else
            {
                return Regex.Replace(textoOrigem, @"[^\d]", "");
            }
        }

        public static string RemoveCodigosDaDescricao(string descricao)
        {
            string textoTemporario = descricao;
            var textosARemover = new List<string> 
            {
                "(CÓD",
                "( CÓD",
                "(COD",
                "( COD",
                "(CÃ?",
                "( CÃ?" 
            };

            foreach (var textoARemover in textosARemover)
            {
                if (textoTemporario.Contains(textoARemover))
                {
                    var strTmp = textoTemporario.Substring(0, textoTemporario.IndexOf(textoARemover));

                    if (strTmp.EndsWith(")"))
                    {
                        textoTemporario = strTmp.Substring(0, strTmp.Length - 1);
                    }
                    else
                    {
                        textoTemporario = strTmp.Substring(0, strTmp.Length); ;
                    }
                }
            }

            return textoTemporario.Trim();
        }

        public static string PrimeiroNome(string nomeCompleto)
        {
            var novoNome = nomeCompleto.Split(" ");

            return novoNome.First();
        }

        public static string NomeSobrenome(string nomeCompleto)
        {
            var novoNome = nomeCompleto.Split(" ");

            if (novoNome.First() == novoNome.Last())
            {
                return novoNome.First();
            }

            return novoNome.First() + " " + novoNome.Last();
        }

        public enum TipoFormacatao
        {
            CNPJ,
            CEP,
            Telefone,
            CPF
        }

        /// <summary>
        /// Formatar uma string de acordo com o parâmetro utilizado
        /// </summary>
        /// <param name="texto">Texto que deseja formatar</param>
        /// <param name="tipoFormacatao">Tipo da formatação</param>
        /// <returns>String com o texto formatado</returns>
        public static string FormatarTexto(string texto, TipoFormacatao tipoFormacatao)
        {
            switch (tipoFormacatao)
            {
                case TipoFormacatao.CNPJ:
                    return Convert.ToUInt64(texto).ToString(@"00\.000\.000\/0000\-00");
                case TipoFormacatao.CEP:
                    return Convert.ToUInt64(texto).ToString(@"00\.000\-000");
                case TipoFormacatao.Telefone:
                    return Convert.ToUInt64(texto).ToString(@"(00) 0000-0000");
                case TipoFormacatao.CPF:
                    return Convert.ToUInt64(texto).ToString(@"000\.000\.000\-00");
                default:
                    return "";
            }
        }

    }
}
