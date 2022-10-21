using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class ItemOrdemServico : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private DateTime? _dataInsercao;
        private string? _codigoItem;
        private string? _descricaoItem;
        private decimal? _quantidadeItem;
        private string? _comentariosItem;
        private DateTime? _dataEdicaoItem;
        private Usuario? _usuario;
        private MotivoItemOrdemServico? _motivoItemOrdemServico;
        private FornecimentoItemOrdemServico? _fornecimentoItemOrdemServico;
        private Status? _status;
        private Conjunto? _conjunto;
        private Especificacao? _especificacao;
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

        public string? CodigoItem
        {
            get { return _codigoItem; }
            set
            {
                if (value != _codigoItem)
                {
                    _codigoItem = value;
                    OnPropertyChanged(nameof(CodigoItem));
                }
            }
        }

        public string? DescricaoItem
        {
            get { return _descricaoItem; }
            set
            {
                if (value != _descricaoItem)
                {
                    _descricaoItem = value;
                    OnPropertyChanged(nameof(DescricaoItem));
                }
            }
        }

        public decimal? QuantidadeItem
        {
            get { return _quantidadeItem; }
            set
            {
                if (value != _quantidadeItem)
                {
                    _quantidadeItem = value;
                    OnPropertyChanged(nameof(QuantidadeItem));
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

        public MotivoItemOrdemServico? MotivoItemOrdemServico
        {
            get { return _motivoItemOrdemServico; }
            set
            {
                if (value != _motivoItemOrdemServico)
                {
                    _motivoItemOrdemServico = value;
                    OnPropertyChanged(nameof(MotivoItemOrdemServico));
                }
            }
        }

        public FornecimentoItemOrdemServico? FornecimentoItemOrdemServico
        {
            get { return _fornecimentoItemOrdemServico; }
            set
            {
                if (value != _fornecimentoItemOrdemServico)
                {
                    _fornecimentoItemOrdemServico = value;
                    OnPropertyChanged(nameof(FornecimentoItemOrdemServico));
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

        public Conjunto? Conjunto
        {
            get { return _conjunto; }
            set
            {
                if (value != _conjunto)
                {
                    _conjunto = value;
                    OnPropertyChanged(nameof(Conjunto));
                }
            }
        }

        public Especificacao? Especificacao
        {
            get { return _especificacao; }
            set
            {
                if (value != _especificacao)
                {
                    _especificacao = value;
                    OnPropertyChanged(nameof(Especificacao));
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
        /// Construtor do item da ordem de serviço com os parâmetros utilizados
        /// </summary>
        /// <param name="inicializaOrdemServico">Indica se a classe deve ser inicializada. Deve-se ter cuidado e levar em consideração loops infinitos</param>
        public ItemOrdemServico(bool inicializaOrdemServico = false, bool inicializarDemaisItens = false)
        {
            if (inicializaOrdemServico)
            {
                OrdemServico = new();
            }

            if (inicializarDemaisItens)
            {
                Usuario = new();
                Status = new();
                MotivoItemOrdemServico = new();
                FornecimentoItemOrdemServico = new();
                Conjunto = new();
                Especificacao = new();
            }
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados do item da ordem de serviço com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id do item da ordem de serviço que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetItemOrdemServicoDatabaseAsync(int? id, CancellationToken ct, bool retornaOrdemServico = false)
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
                                          + "itos.id_item_ordem_servico AS Id, "
                                          + "itos.data_insercao AS DataInsercao, "
                                          + "itos.codigo_item AS CodigoItem, "
                                          + "itos.descricao_item AS DescricaoItem, "
                                          + "itos.quantidade_item AS QuantidadeItem, "
                                          + "itos.comentarios_item AS ComentariosItem, "
                                          + "itos.data_edicao_item AS DataEdicaoItem, "
                                          + "itos.id_usuario AS idUsuario, "
                                          + "itos.id_motivo_item_ordem_servico AS idMotivoItemOrdemServico, "
                                          + "itos.id_fornecimento_item_ordem_servico AS idFornecimentoItemOrdemServico, "
                                          + "itos.id_status AS idStatus, "
                                          + "itos.id_conjunto AS idConjunto, "
                                          + "itos.id_especificacao AS idEspecificacao, "
                                          + "itos.id_ordem_servico AS idOrdemServico "
                                          + "FROM tb_itens_ordem_servico AS itos "
                                          + "WHERE itos.id_item_ordem_servico = @id";

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
                                if (MotivoItemOrdemServico == null)
                                {
                                    MotivoItemOrdemServico = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (FornecimentoItemOrdemServico == null)
                                {
                                    FornecimentoItemOrdemServico = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Conjunto == null)
                                {
                                    Conjunto = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Especificacao == null)
                                {
                                    Especificacao = new();
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
                                CodigoItem = FuncoesDeConversao.ConverteParaString(reader["CodigoItem"]);
                                DescricaoItem = FuncoesDeConversao.ConverteParaString(reader["DescricaoItem"]);
                                QuantidadeItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeItem"]);
                                ComentariosItem = FuncoesDeConversao.ConverteParaString(reader["ComentariosItem"]);
                                DataEdicaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicaoItem"]);
                                Usuario.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuario"]);
                                MotivoItemOrdemServico.Id = FuncoesDeConversao.ConverteParaInt(reader["idMotivoItemOrdemServico"]);
                                FornecimentoItemOrdemServico.Id = FuncoesDeConversao.ConverteParaInt(reader["idFornecimentoItemOrdemServico"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);
                                Conjunto.Id = FuncoesDeConversao.ConverteParaInt(reader["idConjunto"]);
                                Especificacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idEspecificacao"]);

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
            await MotivoItemOrdemServico.GetMotivoItemOrdemServicoDatabaseAsync(MotivoItemOrdemServico.Id, ct);
            await FornecimentoItemOrdemServico.GetFornecimentoItemOrdemServicoDatabaseAsync(FornecimentoItemOrdemServico.Id, ct);
            await Status.GetStatusDatabaseAsync(Status.Id, ct);
            await Conjunto.GetConjuntoDatabaseAsync(Conjunto.Id, ct);
            await Especificacao.GetEspecificacaoDatabaseAsync(Especificacao.Id, ct);

            if (retornaOrdemServico)
            {
                await OrdemServico.GetOrdemServicoDatabaseAsync(OrdemServico.Id, ct);
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados do item da ordem de serviço com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetItemOrdemServicoDatabaseAsync(CancellationToken ct, bool retornaOrdemServico, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                                    + "itos.id_item_ordem_servico AS Id, "
                                    + "itos.data_insercao AS DataInsercao, "
                                    + "itos.codigo_item AS CodigoItem, "
                                    + "itos.descricao_item AS DescricaoItem, "
                                    + "itos.quantidade_item AS QuantidadeItem, "
                                    + "itos.comentarios_item AS ComentariosItem, "
                                    + "itos.data_edicao_item AS DataEdicaoItem, "
                                    + "itos.id_usuario AS idUsuario, "
                                    + "itos.id_motivo_item_ordem_servico AS idMotivoItemOrdemServico, "
                                    + "itos.id_fornecimento_item_ordem_servico AS idFornecimentoItemOrdemServico, "
                                    + "itos.id_status AS idStatus, "
                                    + "itos.id_conjunto AS idConjunto, "
                                    + "itos.id_especificacao AS idEspecificacao, "
                                    + "itos.id_ordem_servico AS idOrdemServico "
                                    + "FROM tb_itens_ordem_servico AS itos "
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
                                if (MotivoItemOrdemServico == null)
                                {
                                    MotivoItemOrdemServico = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (FornecimentoItemOrdemServico == null)
                                {
                                    FornecimentoItemOrdemServico = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Conjunto == null)
                                {
                                    Conjunto = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Especificacao == null)
                                {
                                    Especificacao = new();
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
                                CodigoItem = FuncoesDeConversao.ConverteParaString(reader["CodigoItem"]);
                                DescricaoItem = FuncoesDeConversao.ConverteParaString(reader["DescricaoItem"]);
                                QuantidadeItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeItem"]);
                                ComentariosItem = FuncoesDeConversao.ConverteParaString(reader["ComentariosItem"]);
                                DataEdicaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicaoItem"]);
                                Usuario.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuario"]);
                                MotivoItemOrdemServico.Id = FuncoesDeConversao.ConverteParaInt(reader["idMotivoItemOrdemServico"]);
                                FornecimentoItemOrdemServico.Id = FuncoesDeConversao.ConverteParaInt(reader["idFornecimentoItemOrdemServico"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);
                                Conjunto.Id = FuncoesDeConversao.ConverteParaInt(reader["idConjunto"]);
                                Especificacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idEspecificacao"]);

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
            await MotivoItemOrdemServico.GetMotivoItemOrdemServicoDatabaseAsync(MotivoItemOrdemServico.Id, ct);
            await FornecimentoItemOrdemServico.GetFornecimentoItemOrdemServicoDatabaseAsync(FornecimentoItemOrdemServico.Id, ct);
            await Status.GetStatusDatabaseAsync(Status.Id, ct);
            await Conjunto.GetConjuntoDatabaseAsync(Conjunto.Id, ct);
            await Especificacao.GetEspecificacaoDatabaseAsync(Especificacao.Id, ct);

            if (retornaOrdemServico)
            {
                await OrdemServico.GetOrdemServicoDatabaseAsync(OrdemServico.Id, ct);
            }
        }

        /// <summary>
        /// Método assíncrono que salva o item da ordem de serviço na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarItemOrdemServicoDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_item_ordem_servico", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_data_insercao", DataInsercao);
                        command.Parameters.AddWithValue("p_codigo_item", CodigoItem);
                        command.Parameters.AddWithValue("p_descricao_item", DescricaoItem);
                        command.Parameters.AddWithValue("p_quantidade_item", QuantidadeItem);
                        command.Parameters.AddWithValue("p_comentarios_item", ComentariosItem);
                        command.Parameters.AddWithValue("p_id_usuario", Usuario == null ? null : Usuario.Id);
                        command.Parameters.AddWithValue("p_id_motivo_item_ordem_servico", MotivoItemOrdemServico == null ? null : MotivoItemOrdemServico.Id);
                        command.Parameters.AddWithValue("p_id_fornecimento_item_ordem_servico", FornecimentoItemOrdemServico == null ? null : FornecimentoItemOrdemServico.Id);
                        command.Parameters.AddWithValue("p_id_status", Status == null ? null : Status.Id);
                        command.Parameters.AddWithValue("p_id_conjunto", Conjunto == null ? null : Conjunto.Id);
                        command.Parameters.AddWithValue("p_id_especificacao", Especificacao == null ? null : Especificacao.Id);
                        command.Parameters.AddWithValue("p_id_ordem_servico", OrdemServico == null ? null : OrdemServico.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add("p_id_item_ordem_servico", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna o id da série
                        Id = FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_item_ordem_servico"].Value);
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_item_ordem_servico", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_item_ordem_servico", Id);
                        command.Parameters.AddWithValue("p_codigo_item", CodigoItem);
                        command.Parameters.AddWithValue("p_descricao_item", DescricaoItem);
                        command.Parameters.AddWithValue("p_quantidade_item", QuantidadeItem);
                        command.Parameters.AddWithValue("p_comentarios_item", ComentariosItem);
                        command.Parameters.AddWithValue("p_data_edicao_item", DataEdicaoItem);
                        command.Parameters.AddWithValue("p_id_usuario", Usuario == null ? null : Usuario.Id);
                        command.Parameters.AddWithValue("p_id_motivo_item_ordem_servico", MotivoItemOrdemServico == null ? null : MotivoItemOrdemServico.Id);
                        command.Parameters.AddWithValue("p_id_fornecimento_item_ordem_servico", FornecimentoItemOrdemServico == null ? null : FornecimentoItemOrdemServico.Id);
                        command.Parameters.AddWithValue("p_id_status", Status == null ? null : Status.Id);
                        command.Parameters.AddWithValue("p_id_conjunto", Conjunto == null ? null : Conjunto.Id);
                        command.Parameters.AddWithValue("p_id_especificacao", Especificacao == null ? null : Especificacao.Id);
                        command.Parameters.AddWithValue("p_id_ordem_servico", OrdemServico == null ? null : OrdemServico.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException("O item " + CodigoItem + " - " + DescricaoItem + " não existe");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta o item da ordem de serviço na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarItemOrdemServicoDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_item_ordem_servico", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_item_ordem_servico", Id);
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
                            throw new ChaveEstrangeiraEmUsoException("item da ordem de serviço", CodigoItem + " - " + DescricaoItem);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("O item " + CodigoItem + " - " + DescricaoItem + " não existe");
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de itens da ordem de serviço com os argumentos utilizados. ATENÇÃO: RETORNA APENAS OS ID'S DAS CLASSES
        /// </summary>
        /// <param name="listaItensOrdemServico">Representa a lista de itens da ordem de serviço que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaItensOrdemServicoAsync(ObservableCollection<ItemOrdemServico> listaItensOrdemServico, bool limparLista, bool retornaOrdemServico, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaItensOrdemServico == null)
            {
                listaItensOrdemServico = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaItensOrdemServico.Clear();
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
                                + "itos.id_item_ordem_servico AS Id, "
                                + "itos.data_insercao AS DataInsercao, "
                                + "itos.codigo_item AS CodigoItem, "
                                + "itos.descricao_item AS DescricaoItem, "
                                + "itos.quantidade_item AS QuantidadeItem, "
                                + "itos.comentarios_item AS ComentariosItem, "
                                + "itos.data_edicao_item AS DataEdicaoItem, "
                                + "itos.id_usuario AS idUsuario, "
                                + "itos.id_motivo_item_ordem_servico AS idMotivoItemOrdemServico, "
                                + "itos.id_fornecimento_item_ordem_servico AS idFornecimentoItemOrdemServico, "
                                + "itos.id_status AS idStatus, "
                                + "itos.id_conjunto AS idConjunto, "
                                + "itos.id_especificacao AS idEspecificacao, "
                                + "itos.id_ordem_servico AS idOrdemServico "
                                + "FROM tb_itens_ordem_servico AS itos "
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
                                ItemOrdemServico item = new();

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
                                if (item.MotivoItemOrdemServico == null)
                                {
                                    item.MotivoItemOrdemServico = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.FornecimentoItemOrdemServico == null)
                                {
                                    item.FornecimentoItemOrdemServico = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Conjunto == null)
                                {
                                    item.Conjunto = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Especificacao == null)
                                {
                                    item.Especificacao = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                item.DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                item.CodigoItem = FuncoesDeConversao.ConverteParaString(reader["CodigoItem"]);
                                item.DescricaoItem = FuncoesDeConversao.ConverteParaString(reader["DescricaoItem"]);
                                item.QuantidadeItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeItem"]);
                                item.ComentariosItem = FuncoesDeConversao.ConverteParaString(reader["ComentariosItem"]);
                                item.DataEdicaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicaoItem"]);
                                item.Usuario.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuario"]);
                                item.MotivoItemOrdemServico.Id = FuncoesDeConversao.ConverteParaInt(reader["idMotivoItemOrdemServico"]);
                                item.FornecimentoItemOrdemServico.Id = FuncoesDeConversao.ConverteParaInt(reader["idFornecimentoItemOrdemServico"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);
                                item.Conjunto.Id = FuncoesDeConversao.ConverteParaInt(reader["idConjunto"]);
                                item.Especificacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idEspecificacao"]);

                                if (retornaOrdemServico)
                                {
                                    item.OrdemServico.Id = FuncoesDeConversao.ConverteParaInt(reader["idOrdemServico"]);
                                }

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaItensOrdemServico.Add(item);

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
            ItemOrdemServico itemOrdemServicoCopia = new();

            itemOrdemServicoCopia.Id = Id;
            itemOrdemServicoCopia.DataInsercao = DataInsercao;
            itemOrdemServicoCopia.CodigoItem = CodigoItem;
            itemOrdemServicoCopia.DescricaoItem = DescricaoItem;
            itemOrdemServicoCopia.QuantidadeItem = QuantidadeItem;
            itemOrdemServicoCopia.ComentariosItem = ComentariosItem;
            itemOrdemServicoCopia.DataEdicaoItem = DataEdicaoItem;
            itemOrdemServicoCopia.Usuario = Usuario == null ? new() : (Usuario)Usuario.Clone();
            itemOrdemServicoCopia.MotivoItemOrdemServico = MotivoItemOrdemServico == null ? new() : (MotivoItemOrdemServico)MotivoItemOrdemServico.Clone();
            itemOrdemServicoCopia.FornecimentoItemOrdemServico = FornecimentoItemOrdemServico == null ? new() : (FornecimentoItemOrdemServico)FornecimentoItemOrdemServico.Clone();
            itemOrdemServicoCopia.Status = Status == null ? new() : (Status)Status.Clone();
            itemOrdemServicoCopia.Conjunto = Conjunto == null ? new() : (Conjunto)Conjunto.Clone();
            itemOrdemServicoCopia.Especificacao = Especificacao == null ? new() : (Especificacao)Especificacao.Clone();

            return itemOrdemServicoCopia;
        }

        #endregion Interfaces
    }
}