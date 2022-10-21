using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class RegistroManifestacao : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private string? _descricaoAbertura;
        private DateTime? _dataAbertura;
        private string? _nomePessoaAbertura;
        private string? _emailPessoaAbertura;
        private string? _descricaoFechamento;
        private DateTime? _dataFechamento;
        private string? _nomePessoaFechamento;
        private string? _emailPessoaFechamento;
        private string? _codigoInstancia;
        private int? _idPessoaAbertura;
        private TipoManifestacao? _tipoManifestacao;
        private PrioridadeManifestacao? _prioridadeManifestacao;
        private StatusManifestacao? _statusManifestacao;

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

        public string? DescricaoAbertura
        {
            get { return _descricaoAbertura; }
            set
            {
                if (value != _descricaoAbertura)
                {
                    _descricaoAbertura = value;
                    OnPropertyChanged(nameof(DescricaoAbertura));
                }
            }
        }

        public DateTime? DataAbertura
        {
            get { return _dataAbertura; }
            set
            {
                if (value != _dataAbertura)
                {
                    _dataAbertura = value;
                    OnPropertyChanged(nameof(DataAbertura));
                }
            }
        }

        public string? NomePessoaAbertura
        {
            get { return _nomePessoaAbertura; }
            set
            {
                if (value != _nomePessoaAbertura)
                {
                    _nomePessoaAbertura = value;
                    OnPropertyChanged(nameof(NomePessoaAbertura));
                }
            }
        }

        public string? EmailPessoaAbertura
        {
            get { return _emailPessoaAbertura; }
            set
            {
                if (value != _emailPessoaAbertura)
                {
                    _emailPessoaAbertura = value;
                    OnPropertyChanged(nameof(EmailPessoaAbertura));
                }
            }
        }

        public string? DescricaoFechamento
        {
            get { return _descricaoFechamento; }
            set
            {
                if (value != _descricaoFechamento)
                {
                    _descricaoFechamento = value;
                    OnPropertyChanged(nameof(DescricaoFechamento));
                }
            }
        }

        public DateTime? DataFechamento
        {
            get { return _dataFechamento; }
            set
            {
                if (value != _dataFechamento)
                {
                    _dataFechamento = value;
                    OnPropertyChanged(nameof(DataFechamento));
                }
            }
        }

        public string? NomePessoaFechamento
        {
            get { return _nomePessoaFechamento; }
            set
            {
                if (value != _nomePessoaFechamento)
                {
                    _nomePessoaFechamento = value;
                    OnPropertyChanged(nameof(NomePessoaFechamento));
                }
            }
        }

        public string? EmailPessoaFechamento
        {
            get { return _emailPessoaFechamento; }
            set
            {
                if (value != _emailPessoaFechamento)
                {
                    _emailPessoaFechamento = value;
                    OnPropertyChanged(nameof(EmailPessoaFechamento));
                }
            }
        }

        public string? CodigoInstancia
        {
            get { return _codigoInstancia; }
            set
            {
                if (value != _codigoInstancia)
                {
                    _codigoInstancia = value;
                    OnPropertyChanged(nameof(CodigoInstancia));
                }
            }
        }

        public int? IdPessoaAbertura
        {
            get { return _idPessoaAbertura; }
            set
            {
                if (value != _idPessoaAbertura)
                {
                    _idPessoaAbertura = value;
                    OnPropertyChanged(nameof(IdPessoaAbertura));
                }
            }
        }

        public TipoManifestacao? TipoManifestacao
        {
            get { return _tipoManifestacao; }
            set
            {
                if (value != _tipoManifestacao)
                {
                    _tipoManifestacao = value;
                    OnPropertyChanged(nameof(TipoManifestacao));
                }
            }
        }

        public PrioridadeManifestacao? PrioridadeManifestacao
        {
            get { return _prioridadeManifestacao; }
            set
            {
                if (value != _prioridadeManifestacao)
                {
                    _prioridadeManifestacao = value;
                    OnPropertyChanged(nameof(PrioridadeManifestacao));
                }
            }
        }

        public StatusManifestacao? StatusManifestacao
        {
            get { return _statusManifestacao; }
            set
            {
                if (value != _statusManifestacao)
                {
                    _statusManifestacao = value;
                    OnPropertyChanged(nameof(StatusManifestacao));
                }
            }
        }

        #endregion // Propriedades

        #region Construtores

        /// <summary>
        /// Construtor sem parâmetros que cria uma nova instância de todas as classes
        /// </summary>
        public RegistroManifestacao()
        {
            TipoManifestacao = new();
            PrioridadeManifestacao = new();
            StatusManifestacao = new();
        }

        #endregion // Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados do registro da manifestação através do id
        /// </summary>
        /// <param name="id">Representa o id do registro da manifestação que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetRegistroManifestacaoDatabaseAsync(int? id, CancellationToken ct)
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
                                         + "rema.id_registro_manifestacao AS IdRegistroManifestacao, "
                                         + "rema.descricao_abertura AS DescricaoAbertura, "
                                         + "rema.data_abertura AS DataAbertura, "
                                         + "rema.nome_pessoa_abertura AS NomePessoaAbertura, "
                                         + "rema.email_pessoa_abertura AS EmailPessoaAbertura, "
                                         + "rema.descricao_fechamento AS DescricaoFechamento, "
                                         + "rema.data_fechamento AS DataFechamento, "
                                         + "rema.nome_pessoa_fechamento AS NomePessoaFechamento, "
                                         + "rema.email_pessoa_fechamento AS EmailPessoaFechamento, "
                                         + "rema.codigo_instancia AS CodigoInstancia, "
                                         + "rema.id_usuario_abertura AS IdPessoaAbertura, "
                                         + "tima.id_tipo_manifestacao AS IdTipoManifestacao, "
                                         + "tima.nome AS NomeTipoManifestacao, "
                                         + "prma.id_prioridade_manifestacao AS IdPrioridadeManifestacao, "
                                         + "prma.nome AS NomePrioridadeManifestacao, "
                                         + "stma.id_status_manifestacao AS IdStatusManifestacao, "
                                         + "stma.nome AS NomeStatusManifestacao "
                                         + "FROM tb_registro_manifestacoes AS rema "
                                         + "LEFT JOIN tb_tipos_manifestacoes AS tima ON tima.id_tipo_manifestacao = rema.id_tipo_manifestacao "
                                         + "LEFT JOIN tb_prioridades_manifestacoes AS prma ON prma.id_prioridade_manifestacao = rema.id_prioridade_manifestacao "
                                         + "LEFT JOIN tb_status_manifestacoes AS stma ON stma.id_status_manifestacao = rema.id_status_manifestacao "
                                         + "WHERE rema.id_registro_manifestacao = @id";
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
                                if (TipoManifestacao == null)
                                {
                                    TipoManifestacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (PrioridadeManifestacao == null)
                                {
                                    PrioridadeManifestacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (StatusManifestacao == null)
                                {
                                    StatusManifestacao = new();
                                }

                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdRegistroManifestacao"]);
                                DescricaoAbertura = FuncoesDeConversao.ConverteParaString(reader["DescricaoAbertura"]);
                                DataAbertura = FuncoesDeConversao.ConverteParaDateTime(reader["DataAbertura"]);
                                NomePessoaAbertura = FuncoesDeConversao.ConverteParaString(reader["NomePessoaAbertura"]);
                                EmailPessoaAbertura = FuncoesDeConversao.ConverteParaString(reader["EmailPessoaAbertura"]);
                                DescricaoFechamento = FuncoesDeConversao.ConverteParaString(reader["DescricaoFechamento"]);
                                DataFechamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataFechamento"]);
                                NomePessoaFechamento = FuncoesDeConversao.ConverteParaString(reader["NomePessoaFechamento"]);
                                EmailPessoaFechamento = FuncoesDeConversao.ConverteParaString(reader["EmailPessoaFechamento"]);
                                CodigoInstancia = FuncoesDeConversao.ConverteParaString(reader["CodigoInstancia"]);
                                IdPessoaAbertura = FuncoesDeConversao.ConverteParaInt(reader["IdPessoaAbertura"]);
                                TipoManifestacao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoManifestacao"]);
                                TipoManifestacao.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTipoManifestacao"]);
                                PrioridadeManifestacao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPrioridadeManifestacao"]);
                                PrioridadeManifestacao.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePrioridadeManifestacao"]);
                                StatusManifestacao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusManifestacao"]);
                                StatusManifestacao.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusManifestacao"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados do registro de manifestação através de condições
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetRegistroManifestacaoDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                                    + "rema.id_registro_manifestacao AS IdRegistroManifestacao, "
                                    + "rema.descricao_abertura AS DescricaoAbertura, "
                                    + "rema.data_abertura AS DataAbertura, "
                                    + "rema.nome_pessoa_abertura AS NomePessoaAbertura, "
                                    + "rema.email_pessoa_abertura AS EmailPessoaAbertura, "
                                    + "rema.descricao_fechamento AS DescricaoFechamento, "
                                    + "rema.data_fechamento AS DataFechamento, "
                                    + "rema.nome_pessoa_fechamento AS NomePessoaFechamento, "
                                    + "rema.email_pessoa_fechamento AS EmailPessoaFechamento, "
                                    + "rema.codigo_instancia AS CodigoInstancia, "
                                    + "rema.id_usuario_abertura AS IdPessoaAbertura, "
                                    + "tima.id_tipo_manifestacao AS IdTipoManifestacao, "
                                    + "tima.nome AS NomeTipoManifestacao, "
                                    + "prma.id_prioridade_manifestacao AS IdPrioridadeManifestacao, "
                                    + "prma.nome AS NomePrioridadeManifestacao, "
                                    + "stma.id_status_manifestacao AS IdStatusManifestacao, "
                                    + "stma.nome AS NomeStatusManifestacao "
                                    + "FROM tb_registro_manifestacoes AS rema "
                                    + "LEFT JOIN tb_tipos_manifestacoes AS tima ON tima.id_tipo_manifestacao = rema.id_tipo_manifestacao "
                                    + "LEFT JOIN tb_prioridades_manifestacoes AS prma ON prma.id_prioridade_manifestacao = rema.id_prioridade_manifestacao "
                                    + "LEFT JOIN tb_status_manifestacoes AS stma ON stma.id_status_manifestacao = rema.id_status_manifestacao "
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
                                if (TipoManifestacao == null)
                                {
                                    TipoManifestacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (PrioridadeManifestacao == null)
                                {
                                    PrioridadeManifestacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (StatusManifestacao == null)
                                {
                                    StatusManifestacao = new();
                                }

                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdRegistroManifestacao"]);
                                DescricaoAbertura = FuncoesDeConversao.ConverteParaString(reader["DescricaoAbertura"]);
                                DataAbertura = FuncoesDeConversao.ConverteParaDateTime(reader["DataAbertura"]);
                                NomePessoaAbertura = FuncoesDeConversao.ConverteParaString(reader["NomePessoaAbertura"]);
                                EmailPessoaAbertura = FuncoesDeConversao.ConverteParaString(reader["EmailPessoaAbertura"]);
                                DescricaoFechamento = FuncoesDeConversao.ConverteParaString(reader["DescricaoFechamento"]);
                                DataFechamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataFechamento"]);
                                NomePessoaFechamento = FuncoesDeConversao.ConverteParaString(reader["NomePessoaFechamento"]);
                                EmailPessoaFechamento = FuncoesDeConversao.ConverteParaString(reader["EmailPessoaFechamento"]);
                                CodigoInstancia = FuncoesDeConversao.ConverteParaString(reader["CodigoInstancia"]);
                                IdPessoaAbertura = FuncoesDeConversao.ConverteParaInt(reader["IdPessoaAbertura"]);
                                TipoManifestacao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoManifestacao"]);
                                TipoManifestacao.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTipoManifestacao"]);
                                PrioridadeManifestacao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPrioridadeManifestacao"]);
                                PrioridadeManifestacao.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePrioridadeManifestacao"]);
                                StatusManifestacao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusManifestacao"]);
                                StatusManifestacao.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusManifestacao"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva o registro da manifestação na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarRegistroManifestacaoDatabaseAsync(CancellationToken ct, string nomePessoaAlteracao, string emailPessoaAlteracao)
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

                // Retorna o código da instância atual
                InstanciaLocal instanciaLocal = new();

                try
                {
                    await instanciaLocal.GetInstanciaLocal(CancellationToken.None);
                }
                catch (Exception)
                {
                    throw new DataAccessException("Falha na verificação de instância local. Se o problema persistir, contate o desenvolvedor");
                }

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Se o id for nulo, efetua uma inserção, caso contrário efetua uma edição
                if (Id == null)
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_manifestacao", db.conexaoProreports))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_descricao_abertura", DescricaoAbertura);
                        command.Parameters.AddWithValue("p_nome_pessoa_abertura", NomePessoaAbertura);
                        command.Parameters.AddWithValue("p_email_pessoa_abertura", EmailPessoaAbertura);
                        command.Parameters.AddWithValue("p_id_tipo_manifestacao", TipoManifestacao.Id);
                        command.Parameters.AddWithValue("p_id_prioridade_manifestacao", PrioridadeManifestacao.Id);
                        command.Parameters.AddWithValue("p_id_status_manifestacao", StatusManifestacao.Id);
                        command.Parameters.AddWithValue("p_codigo_instancia", instanciaLocal.CodigoInstancia);
                        command.Parameters.AddWithValue("p_id_usuario_abertura", IdPessoaAbertura);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add("p_id_registro_manifestacao", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException("O registro da manifestação " + DescricaoAbertura + " já existe");
                        }

                        // Retorna o id do contato
                        Id = FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_registro_manifestacao"].Value);
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_manifestacao", db.conexaoProreports))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_registro_manifestacao", Id);
                        command.Parameters.AddWithValue("p_nome_pessoa_alteracao", nomePessoaAlteracao);
                        command.Parameters.AddWithValue("p_email_pessoa_alteracao", emailPessoaAlteracao);
                        command.Parameters.AddWithValue("p_descricao_fechamento", DescricaoFechamento);
                        command.Parameters.AddWithValue("p_id_tipo_manifestacao", TipoManifestacao.Id);
                        command.Parameters.AddWithValue("p_id_prioridade_manifestacao", PrioridadeManifestacao.Id);
                        command.Parameters.AddWithValue("p_id_status_manifestacao", StatusManifestacao.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException(nameof(RegistroManifestacao).ToLower(), DescricaoAbertura);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de registros de manifestação com os argumentos utilizados
        /// </summary>
        /// <param name="listaRegistrosManifestacao">Representa a lista de registros de manifestação que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaRegistrosManifestacaoAsync(ObservableCollection<RegistroManifestacao> listaRegistrosManifestacao, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaRegistrosManifestacao == null)
            {
                listaRegistrosManifestacao = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaRegistrosManifestacao.Clear();
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
                                   + "rema.id_registro_manifestacao AS IdRegistroManifestacao, "
                                   + "rema.descricao_abertura AS DescricaoAbertura, "
                                   + "rema.data_abertura AS DataAbertura, "
                                   + "rema.nome_pessoa_abertura AS NomePessoaAbertura, "
                                   + "rema.email_pessoa_abertura AS EmailPessoaAbertura, "
                                   + "rema.descricao_fechamento AS DescricaoFechamento, "
                                   + "rema.data_fechamento AS DataFechamento, "
                                   + "rema.nome_pessoa_fechamento AS NomePessoaFechamento, "
                                   + "rema.email_pessoa_fechamento AS EmailPessoaFechamento, "
                                   + "rema.codigo_instancia AS CodigoInstancia, "
                                   + "rema.id_usuario_abertura AS IdPessoaAbertura, "
                                   + "tima.id_tipo_manifestacao AS IdTipoManifestacao, "
                                   + "tima.nome AS NomeTipoManifestacao, "
                                   + "prma.id_prioridade_manifestacao AS IdPrioridadeManifestacao, "
                                   + "prma.nome AS NomePrioridadeManifestacao, "
                                   + "stma.id_status_manifestacao AS IdStatusManifestacao, "
                                   + "stma.nome AS NomeStatusManifestacao "
                                   + "FROM tb_registro_manifestacoes AS rema "
                                   + "LEFT JOIN tb_tipos_manifestacoes AS tima ON tima.id_tipo_manifestacao = rema.id_tipo_manifestacao "
                                   + "LEFT JOIN tb_prioridades_manifestacoes AS prma ON prma.id_prioridade_manifestacao = rema.id_prioridade_manifestacao "
                                   + "LEFT JOIN tb_status_manifestacoes AS stma ON stma.id_status_manifestacao = rema.id_status_manifestacao "
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
                                RegistroManifestacao item = new();

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.TipoManifestacao == null)
                                {
                                    item.TipoManifestacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.PrioridadeManifestacao == null)
                                {
                                    item.PrioridadeManifestacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.StatusManifestacao == null)
                                {
                                    item.StatusManifestacao = new();
                                }

                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdRegistroManifestacao"]);
                                item.DescricaoAbertura = FuncoesDeConversao.ConverteParaString(reader["DescricaoAbertura"]);
                                item.DataAbertura = FuncoesDeConversao.ConverteParaDateTime(reader["DataAbertura"]);
                                item.NomePessoaAbertura = FuncoesDeConversao.ConverteParaString(reader["NomePessoaAbertura"]);
                                item.EmailPessoaAbertura = FuncoesDeConversao.ConverteParaString(reader["EmailPessoaAbertura"]);
                                item.DescricaoFechamento = FuncoesDeConversao.ConverteParaString(reader["DescricaoFechamento"]);
                                item.DataFechamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataFechamento"]);
                                item.NomePessoaFechamento = FuncoesDeConversao.ConverteParaString(reader["NomePessoaFechamento"]);
                                item.EmailPessoaFechamento = FuncoesDeConversao.ConverteParaString(reader["EmailPessoaFechamento"]);
                                item.CodigoInstancia = FuncoesDeConversao.ConverteParaString(reader["CodigoInstancia"]);
                                item.IdPessoaAbertura = FuncoesDeConversao.ConverteParaInt(reader["IdPessoaAbertura"]);
                                item.TipoManifestacao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoManifestacao"]);
                                item.TipoManifestacao.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTipoManifestacao"]);
                                item.PrioridadeManifestacao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPrioridadeManifestacao"]);
                                item.PrioridadeManifestacao.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePrioridadeManifestacao"]);
                                item.StatusManifestacao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusManifestacao"]);
                                item.StatusManifestacao.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusManifestacao"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaRegistrosManifestacao.Add(item);

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

        #endregion // Métodos

        #region Interfaces

        /// <summary>
        /// Método para criar uma cópia da classe já que esse é um tipo de referência que não pode ser atribuído diretamente
        /// </summary>
        public object Clone()
        {
            RegistroManifestacao registroManifestacaoCopia = new();
            registroManifestacaoCopia.Id = Id;
            registroManifestacaoCopia.DescricaoAbertura = DescricaoAbertura;
            registroManifestacaoCopia.DataAbertura = DataAbertura;
            registroManifestacaoCopia.NomePessoaAbertura = NomePessoaAbertura;
            registroManifestacaoCopia.EmailPessoaAbertura = EmailPessoaAbertura;
            registroManifestacaoCopia.DescricaoFechamento = DescricaoFechamento;
            registroManifestacaoCopia.DataFechamento = DataFechamento;
            registroManifestacaoCopia.NomePessoaFechamento = NomePessoaFechamento;
            registroManifestacaoCopia.EmailPessoaFechamento = EmailPessoaFechamento;
            registroManifestacaoCopia.CodigoInstancia = CodigoInstancia;
            registroManifestacaoCopia.IdPessoaAbertura = IdPessoaAbertura;
            registroManifestacaoCopia.TipoManifestacao = TipoManifestacao == null ? new() : (TipoManifestacao)TipoManifestacao.Clone();
            registroManifestacaoCopia.PrioridadeManifestacao = PrioridadeManifestacao == null ? new() : (PrioridadeManifestacao)PrioridadeManifestacao.Clone();
            registroManifestacaoCopia.StatusManifestacao = StatusManifestacao == null ? new() : (StatusManifestacao)StatusManifestacao.Clone();

            return registroManifestacaoCopia;
        }

        #endregion // Interfaces
    }
}
