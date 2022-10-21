using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class Cliente : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private string? _nome;
        private bool? _considerarPercentuaisTabelaKion;
        private decimal? _percentualTabelaKion1;
        private decimal? _percentualTabelaKion2;
        private decimal? _percentualTabelaKion3;
        private bool? _considerarAcrescimoEspecifico;
        private decimal? _percentualAcrescimoEspecifico;
        private Status? _status;

        #endregion Campos

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

        public bool? ConsiderarPercentuaisTabelaKion
        {
            get { return _considerarPercentuaisTabelaKion; }
            set
            {
                if (value != _considerarPercentuaisTabelaKion)
                {
                    _considerarPercentuaisTabelaKion = value;
                    OnPropertyChanged(nameof(ConsiderarPercentuaisTabelaKion));
                }
            }
        }

        public decimal? PercentualTabelaKion1
        {
            get { return _percentualTabelaKion1; }
            set
            {
                if (value != _percentualTabelaKion1)
                {
                    _percentualTabelaKion1 = value;
                    OnPropertyChanged(nameof(PercentualTabelaKion1));
                }
            }
        }

        public decimal? PercentualTabelaKion2
        {
            get { return _percentualTabelaKion2; }
            set
            {
                if (value != _percentualTabelaKion2)
                {
                    _percentualTabelaKion2 = value;
                    OnPropertyChanged(nameof(PercentualTabelaKion2));
                }
            }
        }

        public decimal? PercentualTabelaKion3
        {
            get { return _percentualTabelaKion3; }
            set
            {
                if (value != _percentualTabelaKion3)
                {
                    _percentualTabelaKion3 = value;
                    OnPropertyChanged(nameof(PercentualTabelaKion3));
                }
            }
        }

        public bool? ConsiderarAcrescimoEspecifico
        {
            get { return _considerarAcrescimoEspecifico; }
            set
            {
                if (value != _considerarAcrescimoEspecifico)
                {
                    _considerarAcrescimoEspecifico = value;
                    OnPropertyChanged(nameof(ConsiderarAcrescimoEspecifico));
                }
            }
        }

        public decimal? PercentualAcrescimoEspecifico
        {
            get { return _percentualAcrescimoEspecifico; }
            set
            {
                if (value != _percentualAcrescimoEspecifico)
                {
                    _percentualAcrescimoEspecifico = value;
                    OnPropertyChanged(nameof(PercentualAcrescimoEspecifico));
                }
            }
        }

        public Status? Status
        {
            get { return _status; }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        #endregion Propriedades

        #region Construtores

        /// <summary>
        /// Construtor sem parâmetros que cria uma nova instância de todas as classes
        /// </summary>
        public Cliente()
        {
            Status = new();
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados do cliente com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id do cliente que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetClienteDatabaseAsync(int? id, CancellationToken ct)
        {
            // Utilização da conexão
            using (var db = new ConexaoMySQL())
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

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
                using (var command = db.conexao.CreateCommand())
                {
                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT "
                                          + "clie.id_cliente AS IdCliente, "
                                          + "clie.nome AS NomeCliente, "
                                          + "clie.considerar_percentuais_tabela_kion AS ConsiderarPercentuaisTabelaKion, "
                                          + "clie.percentual_tabela_kion_1 AS PercentualTabelaKion1, "
                                          + "clie.percentual_tabela_kion_2 AS PercentualTabelaKion2, "
                                          + "clie.percentual_tabela_kion_3 AS PercentualTabelaKion3, "
                                          + "clie.considerar_acrescimo_especifico AS ConsiderarAcrescimoEspecifico, "
                                          + "clie.percentual_acrescimo_especifico AS PercentualAcrescimoEspecifico, "
                                          + "stat.id_status AS IdStatusCliente, "
                                          + "stat.nome AS NomeStatusCliente "
                                          + "FROM tb_clientes AS clie "
                                          + "LEFT JOIN tb_status AS stat ON stat.id_status = clie.id_status "
                                          + "WHERE clie.id_cliente = @id";

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
                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Status == null)
                                {
                                    Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                ConsiderarPercentuaisTabelaKion = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarPercentuaisTabelaKion"]);
                                PercentualTabelaKion1 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion1"]);
                                PercentualTabelaKion2 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion2"]);
                                PercentualTabelaKion3 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion3"]);
                                ConsiderarAcrescimoEspecifico = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarAcrescimoEspecifico"]);
                                PercentualAcrescimoEspecifico = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoEspecifico"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados do cliente com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetClienteDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
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

                // Utilização do comando
                using (var command = db.conexao.CreateCommand())
                {
                    // Define o comando
                    string comando = "SELECT "
                      + "clie.id_cliente AS IdCliente, "
                      + "clie.nome AS NomeCliente, "
                      + "clie.considerar_percentuais_tabela_kion AS ConsiderarPercentuaisTabelaKion, "
                      + "clie.percentual_tabela_kion_1 AS PercentualTabelaKion1, "
                      + "clie.percentual_tabela_kion_2 AS PercentualTabelaKion2, "
                      + "clie.percentual_tabela_kion_3 AS PercentualTabelaKion3, "
                      + "clie.considerar_acrescimo_especifico AS ConsiderarAcrescimoEspecifico, "
                      + "clie.percentual_acrescimo_especifico AS PercentualAcrescimoEspecifico, "
                      + "stat.id_status AS IdStatusCliente, "
                      + "stat.nome AS NomeStatusCliente "
                      + "FROM tb_clientes AS clie "
                      + "LEFT JOIN tb_status AS stat ON stat.id_status = clie.id_status "
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
                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Status == null)
                                {
                                    Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                ConsiderarPercentuaisTabelaKion = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarPercentuaisTabelaKion"]);
                                PercentualTabelaKion1 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion1"]);
                                PercentualTabelaKion2 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion2"]);
                                PercentualTabelaKion3 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion3"]);
                                ConsiderarAcrescimoEspecifico = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarAcrescimoEspecifico"]);
                                PercentualAcrescimoEspecifico = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoEspecifico"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva o cliente na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarClienteDatabaseAsync(CancellationToken ct)
        {
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

                // Se o id for nulo, efetua uma inserção, caso contrário efetua uma edição
                if (Id == null)
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_cliente", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_considerar_percentuais_tabela_kion", ConsiderarPercentuaisTabelaKion);
                        command.Parameters.AddWithValue("p_percentual_tabela_kion_1", PercentualTabelaKion1);
                        command.Parameters.AddWithValue("p_percentual_tabela_kion_2", PercentualTabelaKion2);
                        command.Parameters.AddWithValue("p_percentual_tabela_kion_3", PercentualTabelaKion3);
                        command.Parameters.AddWithValue("p_considerar_acrescimo_especifico", ConsiderarAcrescimoEspecifico);
                        command.Parameters.AddWithValue("p_percentual_acrescimo_especifico", PercentualAcrescimoEspecifico);
                        command.Parameters.Add("p_id_cliente", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException(nameof(Cliente).ToLower(), Nome);
                        }

                        // Retorna o id da série
                        Id = FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_cliente"].Value);

                        // Cria e salva nova Planta
                        Planta planta = new();
                        planta.Nome = "Geral";
                        planta.Status = new() { Id = 1, Nome = "Ativo" };
                        planta.Cliente = this;
                        planta.Cidade = new();
                        await planta.Cidade.GetCidadeDatabaseAsync(296, CancellationToken.None);
                        await planta.SalvarPlantaDatabaseAsync(CancellationToken.None);

                        // Cria e salva nova área
                        Area area = new();
                        area.Nome = "Geral";
                        area.Status = new() { Id = 1, Nome = "Ativo" };
                        area.Planta = planta;
                        await area.SalvarAreaDatabaseAsync(CancellationToken.None);

                        // Cria e salva nova frota
                        Frota frota = new();
                        frota.Nome = "Geral";
                        frota.Status = new() { Id = 1, Nome = "Ativo" };
                        frota.Area = area;
                        await frota.SalvarFrotaDatabaseAsync(CancellationToken.None);
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_cliente", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_cliente", Id);
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_considerar_percentuais_tabela_kion", ConsiderarPercentuaisTabelaKion);
                        command.Parameters.AddWithValue("p_percentual_tabela_kion_1", PercentualTabelaKion1);
                        command.Parameters.AddWithValue("p_percentual_tabela_kion_2", PercentualTabelaKion2);
                        command.Parameters.AddWithValue("p_percentual_tabela_kion_3", PercentualTabelaKion3);
                        command.Parameters.AddWithValue("p_considerar_acrescimo_especifico", ConsiderarAcrescimoEspecifico);
                        command.Parameters.AddWithValue("p_percentual_acrescimo_especifico", PercentualAcrescimoEspecifico);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException(nameof(Cliente).ToLower(), Nome);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta o cliente na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarClienteDatabaseAsync(CancellationToken ct)
        {
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

                // Utilização do comando
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_cliente", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_cliente", Id);
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    try
                    {
                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);
                    }
                    catch (MySqlConnector.MySqlException ex)
                    {
                        // Se o número da exceção for referente à chave estrangeira em uso, lança a exceção referente a isso
                        if (ex.Number == 1451 || ex.Number == 1417)
                        {
                            throw new ChaveEstrangeiraEmUsoException(nameof(Cliente).ToLower(), Nome);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException(nameof(Cliente).ToLower(), Nome);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de clientes com os argumentos utilizados
        /// </summary>
        /// <param name="listaClientes">Representa a lista de clientes que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaClientesAsync(ObservableCollection<Cliente> listaClientes, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaClientes == null)
            {
                listaClientes = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaClientes.Clear();
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

                // Define o comando
                string comando = "SELECT "
                  + "clie.id_cliente AS IdCliente, "
                  + "clie.nome AS NomeCliente, "
                  + "clie.considerar_percentuais_tabela_kion AS ConsiderarPercentuaisTabelaKion, "
                  + "clie.percentual_tabela_kion_1 AS PercentualTabelaKion1, "
                  + "clie.percentual_tabela_kion_2 AS PercentualTabelaKion2, "
                  + "clie.percentual_tabela_kion_3 AS PercentualTabelaKion3, "
                  + "clie.considerar_acrescimo_especifico AS ConsiderarAcrescimoEspecifico, "
                  + "clie.percentual_acrescimo_especifico AS PercentualAcrescimoEspecifico, "
                  + "stat.id_status AS IdStatusCliente, "
                  + "stat.nome AS NomeStatusCliente "
                  + "FROM tb_clientes AS clie "
                  + "LEFT JOIN tb_status AS stat ON stat.id_status = clie.id_status "
                  + condicoesExtras;

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
                                Cliente item = new();

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                item.ConsiderarPercentuaisTabelaKion = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarPercentuaisTabelaKion"]);
                                item.PercentualTabelaKion1 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion1"]);
                                item.PercentualTabelaKion2 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion2"]);
                                item.PercentualTabelaKion3 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion3"]);
                                item.ConsiderarAcrescimoEspecifico = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarAcrescimoEspecifico"]);
                                item.PercentualAcrescimoEspecifico = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoEspecifico"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaClientes.Add(item);

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

        #endregion Métodos

        #region Interfaces

        /// <summary>
        /// Método para criar uma cópia da classe já que esse é um tipo de referência que não pode ser atribuído diretamente
        /// </summary>
        public object Clone()
        {
            Cliente clienteCopia = new();

            clienteCopia.Id = Id;
            clienteCopia.Nome = Nome;
            clienteCopia.ConsiderarPercentuaisTabelaKion = ConsiderarPercentuaisTabelaKion;
            clienteCopia.PercentualTabelaKion1 = PercentualTabelaKion1;
            clienteCopia.PercentualTabelaKion2 = PercentualTabelaKion2;
            clienteCopia.PercentualTabelaKion3 = PercentualTabelaKion3;
            clienteCopia.ConsiderarAcrescimoEspecifico = ConsiderarAcrescimoEspecifico;
            clienteCopia.PercentualAcrescimoEspecifico = PercentualAcrescimoEspecifico;
            clienteCopia.Status = Status == null ? new() : (Status)Status.Clone();

            return clienteCopia;
        }

        #endregion Interfaces
    }
}