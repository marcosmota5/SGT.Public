using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class Versao : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private string? _nome;
        private DateTime? _dataLancamento;
        private bool? _ehCritica;
        private ObservableCollection<RegistroAlteracao> _listaRegistrosAlteracao;

        #endregion // Campos

        #region Propriedades

        public int? Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string? Nome
        {
            get { return _nome; }
            set
            {
                if (value != _nome)
                {
                    _nome = value;
                    OnPropertyChanged(nameof(Nome));
                }
            }
        }

        public DateTime? DataLancamento
        {
            get { return _dataLancamento; }
            set
            {
                if (value != _dataLancamento)
                {
                    _dataLancamento = value;
                    OnPropertyChanged(nameof(DataLancamento));
                }
            }
        }

        public bool? EhCritica
        {
            get { return _ehCritica; }
            set
            {
                if (value != _ehCritica)
                {
                    _ehCritica = value;
                    OnPropertyChanged(nameof(EhCritica));
                }
            }
        }

        public ObservableCollection<RegistroAlteracao> ListaRegistrosAlteracao
        {
            get { return _listaRegistrosAlteracao; }
            set
            {
                if (value != _listaRegistrosAlteracao)
                {
                    _listaRegistrosAlteracao = value;
                    OnPropertyChanged(nameof(ListaRegistrosAlteracao));
                }
            }
        }

        #endregion // Propriedades

        #region Construtores

        /// <summary>
        /// Construtor da versão com os parâmetros utilizados
        /// </summary>
        /// <param name="inicializaRegistrosAlteracao">Indica se a classe deve ser inicializada. Deve-se ter cuidado e levar em consideração loops infinitos</param>
        public Versao(bool inicializaRegistrosAlteracao = false)
        {
            if (inicializaRegistrosAlteracao)
            {
                ListaRegistrosAlteracao = new();
            }
        }

        #endregion // Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados da versão através do id
        /// </summary>
        /// <param name="id">Representa o id da versão que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetVersaoDatabaseAsync(int? id, CancellationToken ct, bool retornaRegistrosAlteracao = false)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

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
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT "
                                          + "id_versao AS IdVersao, "
                                          + "nome AS NomeVersao, "
                                          + "data_lancamento AS DataLancamento, "
                                          + "e_critica AS EhCritica "
                                          + "FROM tb_versoes "
                                          + "WHERE id_versao = @id";
                    command.Parameters.AddWithValue("@id", id);

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync(ct))
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync(ct))
                            {
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdVersao"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeVersao"]);
                                DataLancamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataLancamento"]);
                                EhCritica = FuncoesDeConversao.ConverteParaBool(reader["EhCritica"]);
                            }
                        }
                    }
                }
            }

            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Caso verdadeiro, a lista de registros de alteração será retornada
            if (retornaRegistrosAlteracao)
            {
                await RegistroAlteracao.PreencheListaRegistrosAlteracaoAsync(ListaRegistrosAlteracao, true, false, null,
                    ct, "WHERE vers.id_versao = @id_versao", "@id_versao", Id);
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados da versão através de condições
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetVersaoDatabaseAsync(CancellationToken ct, bool retornaRegistrosAlteracao, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

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
                    // Define o comando
                    string comando = "SELECT "
                      + "id_versao AS IdVersao, "
                      + "nome AS NomeVersao, "
                      + "data_lancamento AS DataLancamento, "
                      + "e_critica AS EhCritica "
                      + "FROM tb_versoes "
                      + condicoesExtras;

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
                        // Lança exceção de cancelamento caso ela tenha sido efetuada
                        ct.ThrowIfCancellationRequested();

                        command.Parameters.AddWithValue(nomesParametros[contadorParametros].Trim(), item);
                        contadorParametros++;
                    }

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync(ct))
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync(ct))
                            {
                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdVersao"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeVersao"]);
                                DataLancamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataLancamento"]);
                                EhCritica = FuncoesDeConversao.ConverteParaBool(reader["EhCritica"]);
                            }
                        }
                    }
                }
            }

            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Caso verdadeiro, a lista de registros de alteração será retornada
            if (retornaRegistrosAlteracao)
            {
                await RegistroAlteracao.PreencheListaRegistrosAlteracaoAsync(ListaRegistrosAlteracao, true, false, null,
                    ct, "WHERE vers.id_versao = @id_versao", "@id_versao", Id);
            }
        }

        /// <summary>
        /// Método assíncrono que retorna um valor booleano indicando se existe uma nova versão do sistema
        /// </summary>
        public static async Task<bool> ExisteNovaVersao(int? id, CancellationToken ct)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

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
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT COUNT(id_versao) AS Contagem FROM tb_versoes WHERE id_versao > @id";
                    command.Parameters.AddWithValue("@id", id);

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync(ct))
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync(ct))
                            {
                                // Verifica se há valores
                                if (FuncoesDeConversao.ConverteParaInt(reader["Contagem"]) > 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Método assíncrono que retorna um valor booleano indicando se existe uma nova versão crítica do sistema
        /// </summary>
        public static async Task<bool> ExisteNovaVersaoCritica(int? id, CancellationToken ct)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

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
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT COUNT(id_versao) AS Contagem FROM tb_versoes WHERE id_versao > @id AND e_critica = 1";
                    command.Parameters.AddWithValue("@id", id);

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync(ct))
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync(ct))
                            {
                                // Verifica se há valores
                                if (FuncoesDeConversao.ConverteParaInt(reader["Contagem"]) > 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de status com os argumentos utilizados
        /// </summary>
        /// <param name="listaVersoes">Representa a lista de versões que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaVersoesAsync(ObservableCollection<Versao> listaVersoes, bool limparLista, bool retornaRegistrosAlteracao, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaVersoes == null)
            {
                listaVersoes = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaVersoes.Clear();
            }

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

                // Define o comando
                string comando = "SELECT "
                  + "id_versao AS IdVersao, "
                  + "nome AS NomeVersao, "
                  + "data_lancamento AS DataLancamento, "
                  + "e_critica AS EhCritica "
                  + "FROM tb_versoes "
                  + condicoesExtras;

                // Cria e atribui a variável do total de linhas através da função específica para contagem de linhas
                int totalLinhas = await FuncoesDeDatabase.GetQuantidadeLinhasReaderAsync(db, comando, ct, nomesParametrosSeparadosPorVirgulas, valoresParametros);

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Utilização do comando
                using (var command = db.conexaoProreports.CreateCommand())
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
                                Versao item = new();

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdVersao"]);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeVersao"]);
                                item.DataLancamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataLancamento"]);
                                item.EhCritica = FuncoesDeConversao.ConverteParaBool(reader["EhCritica"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaVersoes.Add(item);

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


            int totalLinhasLista = listaVersoes.Count;
            int linhaAtualLista = 0;

            foreach (Versao item in listaVersoes)
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Caso verdadeiro, a lista de itens da proposta será retornada
                if (retornaRegistrosAlteracao)
                {
                    await RegistroAlteracao.PreencheListaRegistrosAlteracaoAsync(item.ListaRegistrosAlteracao, true, false, null,
                    ct, "WHERE vers.id_versao = @id_versao", "@id_versao", item.Id);
                }

                // Incrementa a linha atual
                linhaAtualLista++;

                // Reporta o progresso se o progresso não for nulo
                if (reportadorProgresso != null)
                {
                    reportadorProgresso.Report(linhaAtualLista / totalLinhasLista * 100);
                }
            }
        }

        #endregion // Métodos

        #region Interfaces

        /// <summary>
        /// Método para criar uma cópia da classe já que esse é um tipo de referência que não pode ser atribuído diretamente
        /// </summary>
        public object Clone()
        {
            Versao versaoCopia = new();
            versaoCopia.Id = Id;
            versaoCopia.Nome = Nome;
            versaoCopia.DataLancamento = DataLancamento;
            versaoCopia.EhCritica = EhCritica;

            return versaoCopia;
        }

        #endregion // Interfaces
    }
}