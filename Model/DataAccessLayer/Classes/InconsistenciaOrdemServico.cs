using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class InconsistenciaOrdemServico : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private DateTime? _dataInsercao;
        private string? _comentariosItem;
        private DateTime? _dataEdicaoItem;
        private Inconsistencia? _inconsistencia;
        private Usuario? _usuario;
        private Status? _status;
        private OrdemServico? _ordemServico;

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

        public DateTime? DataInsercao
        {
            get { return _dataInsercao; }
            set
            {
                if (value != _dataInsercao)
                {
                    _dataInsercao = value;
                    OnPropertyChanged(nameof(DataInsercao));
                }
            }
        }

        public string? ComentariosItem
        {
            get { return _comentariosItem; }
            set
            {
                if (value != _comentariosItem)
                {
                    _comentariosItem = value;
                    OnPropertyChanged(nameof(ComentariosItem));
                }
            }
        }

        public DateTime? DataEdicaoItem
        {
            get { return _dataEdicaoItem; }
            set
            {
                if (value != _dataEdicaoItem)
                {
                    _dataEdicaoItem = value;
                    OnPropertyChanged(nameof(DataEdicaoItem));
                }
            }
        }

        public Inconsistencia? Inconsistencia
        {
            get { return _inconsistencia; }
            set
            {
                if (value != _inconsistencia)
                {
                    _inconsistencia = value;
                    OnPropertyChanged(nameof(Inconsistencia));
                }
            }
        }

        public Usuario? Usuario
        {
            get { return _usuario; }
            set
            {
                if (value != _usuario)
                {
                    _usuario = value;
                    OnPropertyChanged(nameof(Usuario));
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

        public OrdemServico? OrdemServico
        {
            get { return _ordemServico; }
            set
            {
                if (value != _ordemServico)
                {
                    _ordemServico = value;
                    OnPropertyChanged(nameof(OrdemServico));
                }
            }
        }

        #endregion Propriedades

        #region Construtores

        /// <summary>
        /// Construtor da inconsistência da ordem de serviço com os parâmetros utilizados
        /// </summary>
        /// <param name="inicializaOrdemServico">Indica se a classe deve ser inicializada. Deve-se ter cuidado e levar em consideração loops infinitos</param>
        public InconsistenciaOrdemServico(bool inicializaOrdemServico = false, bool inicializarDemaisItens = false)
        {
            if (inicializaOrdemServico)
            {
                OrdemServico = new();
            }

            if (inicializarDemaisItens)
            {
                Usuario = new();
                Status = new();
                Inconsistencia = new();
            }
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados da inconsistencia da ordem de serviço com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id da inconsistencia da ordem de serviço que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetInconsistenciaOrdemServicoDatabaseAsync(int? id, CancellationToken ct, bool retornaOrdemServico = false)
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
                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT "
                                          + "inos.id_inconsistencia_ordem_servico AS Id, "
                                          + "inos.data_insercao AS DataInsercao, "
                                          + "inos.comentarios_item AS ComentariosItem, "
                                          + "inos.data_edicao_item AS DataEdicaoItem, "
                                          + "inos.id_inconsistencia AS idInconsistencia, "
                                          + "inos.id_usuario AS idUsuario, "
                                          + "inos.id_status AS idStatus, "
                                          + "inos.id_ordem_servico AS idOrdemServico "
                                          + "FROM tb_inconsistencias_ordem_servico AS inos "
                                          + "WHERE inos.id_inconsistencia_ordem_servico = @id";

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

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Usuario == null)
                                {
                                    Usuario = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Inconsistencia == null)
                                {
                                    Inconsistencia = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaOrdemServico)
                                {
                                    if (OrdemServico == null)
                                    {
                                        OrdemServico = new();
                                    }
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                ComentariosItem = FuncoesDeConversao.ConverteParaString(reader["ComentariosItem"]);
                                DataEdicaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicaoItem"]);
                                Usuario.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuario"]);
                                Inconsistencia.Id = FuncoesDeConversao.ConverteParaInt(reader["idInconsistencia"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);

                                if (retornaOrdemServico)
                                {
                                    OrdemServico.Id = FuncoesDeConversao.ConverteParaInt(reader["idOrdemServico"]);
                                }
                            }
                        }
                    }
                }
            }

            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            await Usuario.GetUsuarioDatabaseAsync(Usuario.Id, ct);
            await Inconsistencia.GetInconsistenciaDatabaseAsync(Inconsistencia.Id, ct);
            await Status.GetStatusDatabaseAsync(Status.Id, ct);

            if (retornaOrdemServico)
            {
                await OrdemServico.GetOrdemServicoDatabaseAsync(OrdemServico.Id, ct);
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados da inconsistencia da ordem de serviço com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetInconsistenciaOrdemServicoDatabaseAsync(CancellationToken ct, bool retornaOrdemServico, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                                    + "inos.id_inconsistencia_ordem_servico AS Id, "
                                    + "inos.data_insercao AS DataInsercao, "
                                    + "inos.comentarios_item AS ComentariosItem, "
                                    + "inos.data_edicao_item AS DataEdicaoItem, "
                                    + "inos.id_inconsistencia AS idInconsistencia, "
                                    + "inos.id_usuario AS idUsuario, "
                                    + "inos.id_status AS idStatus, "
                                    + "inos.id_ordem_servico AS idOrdemServico "
                                    + "FROM tb_inconsistencias_ordem_servico AS inos "
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

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Usuario == null)
                                {
                                    Usuario = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Inconsistencia == null)
                                {
                                    Inconsistencia = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaOrdemServico)
                                {
                                    if (OrdemServico == null)
                                    {
                                        OrdemServico = new();
                                    }
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                ComentariosItem = FuncoesDeConversao.ConverteParaString(reader["ComentariosItem"]);
                                DataEdicaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicaoItem"]);
                                Usuario.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuario"]);
                                Inconsistencia.Id = FuncoesDeConversao.ConverteParaInt(reader["idInconsistencia"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);

                                if (retornaOrdemServico)
                                {
                                    OrdemServico.Id = FuncoesDeConversao.ConverteParaInt(reader["idOrdemServico"]);
                                }
                            }
                        }
                    }
                }
            }

            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            await Usuario.GetUsuarioDatabaseAsync(Usuario.Id, ct);
            await Inconsistencia.GetInconsistenciaDatabaseAsync(Inconsistencia.Id, ct);
            await Status.GetStatusDatabaseAsync(Status.Id, ct);

            if (retornaOrdemServico)
            {
                await OrdemServico.GetOrdemServicoDatabaseAsync(OrdemServico.Id, ct);
            }
        }

        /// <summary>
        /// Método assíncrono que salva a inconsistencia da ordem de serviço na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarInconsistenciaOrdemServicoDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_inconsistencia_ordem_servico", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_data_insercao", DataInsercao);
                        command.Parameters.AddWithValue("p_comentarios_item", ComentariosItem);
                        command.Parameters.AddWithValue("p_id_inconsistencia", Inconsistencia == null ? null : Inconsistencia.Id);
                        command.Parameters.AddWithValue("p_id_usuario", Usuario == null ? null : Usuario.Id);
                        command.Parameters.AddWithValue("p_id_status", Status == null ? null : Status.Id);
                        command.Parameters.AddWithValue("p_id_ordem_servico", OrdemServico == null ? null : OrdemServico.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_inconsistencia_ordem_servico", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_inconsistencia_ordem_servico", Id);
                        command.Parameters.AddWithValue("p_data_edicao_item", DataEdicaoItem);
                        command.Parameters.AddWithValue("p_comentarios_item", ComentariosItem);
                        command.Parameters.AddWithValue("p_id_inconsistencia", Inconsistencia == null ? null : Inconsistencia.Id);
                        command.Parameters.AddWithValue("p_id_usuario", Usuario == null ? null : Usuario.Id);
                        command.Parameters.AddWithValue("p_id_status", Status == null ? null : Status.Id);
                        command.Parameters.AddWithValue("p_id_ordem_servico", OrdemServico == null ? null : OrdemServico.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException("O inconsistencia não existe");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta a inconsistencia da ordem de serviço na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarInconsistenciaOrdemServicoDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_inconsistencia_ordem_servico", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_inconsistencia_ordem_servico", Id);
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
                            throw new ChaveEstrangeiraEmUsoException("inconsistencia da ordem de serviço em uso");
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("O inconsistencia não existe");
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de inconsistencias da ordem de serviço com os argumentos utilizados. ATENÇÃO: RETORNA APENAS OS ID'S DAS CLASSES
        /// </summary>
        /// <param name="listaInconsistenciasOrdemServico">Representa a lista de inconsistencias da ordem de serviço que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaInconsistenciasOrdemServicoAsync(ObservableCollection<InconsistenciaOrdemServico> listaInconsistenciasOrdemServico, bool limparLista, bool retornaOrdemServico, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaInconsistenciasOrdemServico == null)
            {
                listaInconsistenciasOrdemServico = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaInconsistenciasOrdemServico.Clear();
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
                                + "inos.id_inconsistencia_ordem_servico AS Id, "
                                + "inos.data_insercao AS DataInsercao, "
                                + "inos.comentarios_item AS ComentariosItem, "
                                + "inos.data_edicao_item AS DataEdicaoItem, "
                                + "inos.id_inconsistencia AS idInconsistencia, "
                                + "inos.id_usuario AS idUsuario, "
                                + "inos.id_status AS idStatus, "
                                + "inos.id_ordem_servico AS idOrdemServico "
                                + "FROM tb_inconsistencias_ordem_servico AS inos "
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
                                InconsistenciaOrdemServico item = new();

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Usuario == null)
                                {
                                    item.Usuario = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaOrdemServico)
                                {
                                    if (item.OrdemServico == null)
                                    {
                                        item.OrdemServico = new();
                                    }
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Inconsistencia == null)
                                {
                                    item.Inconsistencia = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                item.DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                item.ComentariosItem = FuncoesDeConversao.ConverteParaString(reader["ComentariosItem"]);
                                item.DataEdicaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicaoItem"]);
                                item.Usuario.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuario"]);
                                item.Inconsistencia.Id = FuncoesDeConversao.ConverteParaInt(reader["idInconsistencia"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);

                                if (retornaOrdemServico)
                                {
                                    item.OrdemServico.Id = FuncoesDeConversao.ConverteParaInt(reader["idOrdemServico"]);
                                }

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaInconsistenciasOrdemServico.Add(item);

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
            InconsistenciaOrdemServico inconsistenciaOrdemServicoCopia = new();

            inconsistenciaOrdemServicoCopia.Id = Id;
            inconsistenciaOrdemServicoCopia.DataInsercao = DataInsercao;
            inconsistenciaOrdemServicoCopia.ComentariosItem = ComentariosItem;
            inconsistenciaOrdemServicoCopia.DataEdicaoItem = DataEdicaoItem;
            inconsistenciaOrdemServicoCopia.Usuario = Usuario == null ? new() : (Usuario)Usuario.Clone();
            inconsistenciaOrdemServicoCopia.Inconsistencia = Inconsistencia == null ? new() : (Inconsistencia)Inconsistencia.Clone();
            inconsistenciaOrdemServicoCopia.Status = Status == null ? new() : (Status)Status.Clone();

            return inconsistenciaOrdemServicoCopia;
        }

        #endregion Interfaces
    }
}