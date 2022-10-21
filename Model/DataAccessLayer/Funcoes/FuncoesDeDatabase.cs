using Model.DataAccessLayer.Conexoes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.Funcoes
{
    public static class FuncoesDeDatabase
    {
        internal static async Task<int> GetQuantidadeLinhasReader(ConexaoMySQL db, string comando, CancellationToken ct)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Cria e atribui a variável do total de linhas
            int totalLinhas = 0;

            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Utilização do comando de contagem
            using (var commandCount = db.conexao.CreateCommand())
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Definição do tipo, texto e parâmetros do comando
                commandCount.CommandType = System.Data.CommandType.Text;
                commandCount.CommandText = "SELECT COUNT(*) AS total_linhas FROM (" + comando + ") AS count_table";

                // Utilização do reader para retornar os dados asíncronos
                using (var reader = await commandCount.ExecuteReaderAsync())
                {
                    // Verifica se o reader possui linhas
                    if (reader.HasRows)
                    {
                        // Enquanto o reader possuir linhas, define os valores
                        while (await reader.ReadAsync(ct))
                        {
                            // Lança exceção de cancelamento caso ela tenha sido efetuada
                            ct.ThrowIfCancellationRequested();

                            // Define o total de linhas
                            totalLinhas = Convert.ToInt32(FuncoesDeConversao.ConverteParaInt(reader["total_linhas"]));

                            // Lança exceção de cancelamento caso ela tenha sido efetuada
                            ct.ThrowIfCancellationRequested();
                        }
                    }
                }
            }

            return totalLinhas;
        }

        internal static async Task<int> GetQuantidadeLinhasReaderAsync(ConexaoMySQL db, string comando, CancellationToken ct, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Cria e atribui a variável do total de linhas
            int totalLinhas = 0;

            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Utilização do comando de contagem
            using (var commandCount = db.conexao.CreateCommand())
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Definição do tipo, texto e parâmetros do comando
                commandCount.CommandType = System.Data.CommandType.Text;
                commandCount.CommandText = "SELECT COUNT(*) AS total_linhas FROM (" + comando + ") AS count_table";

                // Cria uma array com os parâmetros passados utilizando vírgula como delimitador
                string[] nomesParametros = nomesParametrosSeparadosPorVirgulas.Split(",");

                // Cria um contador para retornar o nome do parametro corretamente
                int contadorParametros = 0;

                // Varre o array de parâmetros adicionando-os à consulta
                foreach (var item in valoresParametros)
                {
                    commandCount.Parameters.AddWithValue(nomesParametros[contadorParametros].Trim(), item);
                    contadorParametros++;
                }

                // Utilização do reader para retornar os dados asíncronos
                using (var reader = await commandCount.ExecuteReaderAsync())
                {
                    // Verifica se o reader possui linhas
                    if (reader.HasRows)
                    {
                        // Enquanto o reader possuir linhas, define os valores
                        while (await reader.ReadAsync(ct))
                        {
                            // Lança exceção de cancelamento caso ela tenha sido efetuada
                            ct.ThrowIfCancellationRequested();

                            // Define o total de linhas
                            totalLinhas = Convert.ToInt32(FuncoesDeConversao.ConverteParaInt(reader["total_linhas"]));

                            // Lança exceção de cancelamento caso ela tenha sido efetuada
                            ct.ThrowIfCancellationRequested();
                        }
                    }
                }
            }

            return totalLinhas;
        }

        internal static async Task<int> GetQuantidadeLinhasReaderAsync(ConexaoProreportsMySQL db, string comando, CancellationToken ct, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Cria e atribui a variável do total de linhas
            int totalLinhas = 0;

            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Utilização do comando de contagem
            using (var commandCount = db.conexaoProreports.CreateCommand())
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Definição do tipo, texto e parâmetros do comando
                commandCount.CommandType = System.Data.CommandType.Text;
                commandCount.CommandText = "SELECT COUNT(*) AS total_linhas FROM (" + comando + ") AS count_table";

                // Cria uma array com os parâmetros passados utilizando vírgula como delimitador
                string[] nomesParametros = nomesParametrosSeparadosPorVirgulas.Split(",");

                // Cria um contador para retornar o nome do parametro corretamente
                int contadorParametros = 0;

                // Varre o array de parâmetros adicionando-os à consulta
                foreach (var item in valoresParametros)
                {
                    commandCount.Parameters.AddWithValue(nomesParametros[contadorParametros].Trim(), item);
                    contadorParametros++;
                }

                // Utilização do reader para retornar os dados asíncronos
                using (var reader = await commandCount.ExecuteReaderAsync())
                {
                    // Verifica se o reader possui linhas
                    if (reader.HasRows)
                    {
                        // Enquanto o reader possuir linhas, define os valores
                        while (await reader.ReadAsync(ct))
                        {
                            // Lança exceção de cancelamento caso ela tenha sido efetuada
                            ct.ThrowIfCancellationRequested();

                            // Define o total de linhas
                            totalLinhas = Convert.ToInt32(FuncoesDeConversao.ConverteParaInt(reader["total_linhas"]));

                            // Lança exceção de cancelamento caso ela tenha sido efetuada
                            ct.ThrowIfCancellationRequested();
                        }
                    }
                }
            }

            return totalLinhas;
        }

        public static async Task<bool> GetPermiteDeletar(string nomeTabela, int? idProcurado, CancellationToken ct, string nomeTabelaIgnorar = "")
        {
            if (idProcurado == null)
            {
                return true;
            }

            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Utilização da conexão
            using (var db = new ConexaoMySQL())
            {
                // Tenta abrir a conexão e retorna um erro caso não consiga
                try
                {
                    // Abre a conexão
                    await db.AbreConexaoAsync();
                }
                catch (MySqlConnector.MySqlException)
                {
                    throw;
                }

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Define o comando
                string comando = "SELECT " +
                    "i.TABLE_NAME, " +
                    "k.COLUMN_NAME " +
                    "FROM information_schema.TABLE_CONSTRAINTS i " +
                    "LEFT JOIN information_schema.KEY_COLUMN_USAGE k ON i.CONSTRAINT_NAME = k.CONSTRAINT_NAME " +
                    "WHERE i.CONSTRAINT_TYPE = 'FOREIGN KEY' AND k.REFERENCED_TABLE_NAME = '" + nomeTabela + "'" + (nomeTabelaIgnorar != "" ? " AND i.TABLE_NAME <> '" + nomeTabelaIgnorar + "'" : "");

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                List<TabelaVerificar> listaTabelaVerificar = new();

                // Utilização do comando
                using (var command = db.conexao.CreateCommand())
                {
                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = comando;

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync(ct))
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync(ct))
                            {
                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona a tabela e coluna a serem verificadas
                                listaTabelaVerificar.Add(new TabelaVerificar(FuncoesDeConversao.ConverteParaString(reader["TABLE_NAME"]), FuncoesDeConversao.ConverteParaString(reader["COLUMN_NAME"])));

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();
                            }
                        }
                    }
                }

                // Se a lista estiver vazia, retorna verdadeiro
                if (listaTabelaVerificar.Count == 0)
                {
                    return true;
                }

                // Varre a lista retornada
                foreach (var item in listaTabelaVerificar)
                {
                    comando = "SELECT COUNT(*) AS Contagem FROM " + item.nome_tabela + " WHERE " + item.nome_coluna + " = " + idProcurado.ToString();

                    // Utilização do comando
                    using (var command = db.conexao.CreateCommand())
                    {
                        // Definição do tipo, texto e parâmetros do comando
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = comando;

                        // Utilização do reader para retornar os dados asíncronos
                        using (var reader = await command.ExecuteReaderAsync(ct))
                        {
                            // Verifica se o reader possui linhas
                            if (reader.HasRows)
                            {
                                // Enquanto o reader possuir linhas, define os valores
                                while (await reader.ReadAsync(ct))
                                {
                                    // Lança exceção de cancelamento caso ela tenha sido efetuada
                                    ct.ThrowIfCancellationRequested();

                                    // Verifica se há valores
                                    if (FuncoesDeConversao.ConverteParaInt(reader["Contagem"]) > 0)
                                    {
                                        return false;
                                    }

                                    // Lança exceção de cancelamento caso ela tenha sido efetuada
                                    ct.ThrowIfCancellationRequested();
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        public static async Task<bool> EhDesenvolvedor(string enderecoMac, CancellationToken ct)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            string enderecoMacFormatado = String.Join("", enderecoMac.Split(":"));
            string enderecoMacComPontuacao =
                enderecoMacFormatado.Substring(0, 2) + ":" +
                enderecoMacFormatado.Substring(2, 2) + ":" +
                enderecoMacFormatado.Substring(4, 2) + ":" +
                enderecoMacFormatado.Substring(6, 2) + ":" +
                enderecoMacFormatado.Substring(8, 2) + ":" +
                enderecoMacFormatado.Substring(10, 2);

            // Utilização da conexão
            using (var db = new ConexaoProreportsMySQL())
            {
                // Tenta abrir a conexão e retorna um erro caso não consiga
                try
                {
                    // Abre a conexão
                    await db.AbreConexaoAsync();
                }
                catch (MySqlConnector.MySqlException)
                {
                    throw;
                }

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Utilização do comando
                using (var command = db.conexaoProreports.CreateCommand())
                {
                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT COUNT(*) AS Contagem FROM tb_enderecos_mac_desenvolvedores WHERE endereco_mac_desenvolvedor = @endereco_mac_desenvolvedor";
                    command.Parameters.AddWithValue("@endereco_mac_desenvolvedor", enderecoMacComPontuacao);

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync(ct))
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync(ct))
                            {
                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Verifica se há valores
                                if (FuncoesDeConversao.ConverteParaInt(reader["Contagem"]) > 0)
                                {
                                    return true;
                                }

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static async Task<DataTable> GetDataTable(string nomeDataTable, string comandoSelecao, CancellationToken ct, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            var dt = new DataTable(nomeDataTable);

            // Utilização da conexão
            using (var db = new ConexaoMySQL())
            {
                // Tenta abrir a conexão e retorna um erro caso não consiga
                try
                {
                    // Abre a conexão
                    await db.AbreConexaoAsync();
                }
                catch (MySqlConnector.MySqlException)
                {
                    throw;
                }

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                using (var adapter = new MySqlConnector.MySqlDataAdapter(comandoSelecao, db.conexao))
                {
                    // Cria uma array com os parâmetros passados utilizando vírgula como delimitador
                    string[] nomesParametros = nomesParametrosSeparadosPorVirgulas.Split(",");

                    // Cria um contador para retornar o nome do parametro corretamente
                    int contadorParametros = 0;

                    // Varre o array de parâmetros adicionando-os à consulta
                    foreach (var item in valoresParametros)
                    {
                        adapter.SelectCommand.Parameters.AddWithValue(nomesParametros[contadorParametros].Trim(), item);
                        contadorParametros++;
                    }

                    adapter.Fill(dt);
                };
            }

            return dt;
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de int? com os argumentos utilizados
        /// </summary>
        /// <param name="lista">Representa a lista de int? que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="comando">Representa o comando completo</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaIntNullAsync(ObservableCollection<int?> lista, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string comando, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (lista == null)
            {
                lista = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                lista.Clear();
            }

            // Utilização da conexão
            using (var db = new ConexaoMySQL())
            {
                // Tenta abrir a conexão e retorna um erro caso não consiga
                try
                {
                    // Abre a conexão
                    await db.AbreConexaoAsync();
                }
                catch (MySqlConnector.MySqlException)
                {
                    throw;
                }

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Cria e atribui a variável do total de linhas através da função específica para contagem de linhas
                int totalLinhas = await FuncoesDeDatabase.GetQuantidadeLinhasReaderAsync(db, comando, ct, nomesParametrosSeparadosPorVirgulas, valoresParametros);

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Utilização do comando
                using (var command = db.conexao.CreateCommand())
                {
                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = comando;

                    // Cria uma array com os parâmetros passados utilizando vírgula como delimitador
                    string[] nomesParametros = nomesParametrosSeparadosPorVirgulas.Split(",");

                    // Cria um contador para retornar o nome do parametro corretamente
                    int contadorParametros = 0;

                    // Varre o array de parâmetros adicionando-os à consulta
                    foreach (var item in valoresParametros)
                    {
                        command.Parameters.AddWithValue(nomesParametros[contadorParametros].Trim(), item);
                        contadorParametros++;
                    }

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync(ct))
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            // Cria e atribui a variável de contagem de linhas
                            int linhaAtual = 0;

                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync(ct))
                            {
                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Cria um novo item e atribui os valores
                                int? item = null;

                                // Define as propriedades
                                item = FuncoesDeConversao.ConverteParaInt(reader[0]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                lista.Add(item);

                                // Incrementa a linha atual
                                linhaAtual++;

                                // Reporta o progresso se o progresso não for nulo
                                if (reportadorProgresso != null)
                                {
                                    reportadorProgresso.Report((double)linhaAtual / (double)totalLinhas * (double)100);
                                }

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();
                            }
                        }
                    }
                }
            }
        }

        internal class TabelaVerificar
        {
            public string nome_tabela;
            public string nome_coluna;

            public TabelaVerificar(string nome_tabela, string nome_coluna)
            {
                this.nome_tabela = nome_tabela;
                this.nome_coluna = nome_coluna;
            }
        }
    }
}