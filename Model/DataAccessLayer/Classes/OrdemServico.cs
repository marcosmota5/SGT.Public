using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;
using System.Collections.ObjectModel;

namespace Model.DataAccessLayer.Classes
{
    public class OrdemServico : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private int? _ordemServicoAtual;
        private int? _ordemServicoPrimaria;
        private int? _numeroChamado;
        private DateTime? _dataChamado;
        private DateTime? _dataInsercao;
        private DateTime? _dataAtendimento;
        private string? _mastro;
        private string? _codigoFalha;
        private int? _etapasConcluidas;
        private bool? _equipamentoOperacional;
        private string? _equipamentoOperacionalTexto;
        private decimal? _horimetro;
        private decimal? _horasPreventiva;
        private string? _outroTipoManutencao;
        private string? _motivoAtendimento;
        private string? _entrevistaInicial;
        private string? _intervencao;
        private DateTime? _dataSaida;
        private DateTime? _dataChegada;
        private DateTime? _dataRetorno;
        private DateTime? _dataEdicao;
        private string? _comentarios;
        private int? _idUsuarioEmUso;
        private Filial? _filial;
        private Cliente? _cliente;
        private Frota? _frota;
        private Usuario? _usuarioInsercao;
        private Usuario? _usuarioEdicao;
        private Status? _status;
        private Serie? _serie;
        private TipoOrdemServico? _tipoOrdemServico;
        private StatusEquipamentoAposManutencao? _statusEquipamentoAposManutencao;
        private UsoIndevido? _usoIndevido;
        private TipoManutencao? _tipoManutencao;
        private ExecutanteServico? _executanteServico;
        private ObservableCollection<ItemOrdemServico> _listaItensOrdemServico;
        private ObservableCollection<EventoOrdemServico> _listaEventosOrdemServico;
        private ObservableCollection<InconsistenciaOrdemServico> _listaInconsistenciasOrdemServico;

        #endregion Campos

        #region Propriedades

        public string PropriedadeAlterada { get; set; }

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

        public int? OrdemServicoAtual
        {
            get { return _ordemServicoAtual; }
            set
            {
                if (value != _ordemServicoAtual)
                {
                    _ordemServicoAtual = value;
                    OnPropertyChanged(nameof(OrdemServicoAtual));
                }
            }
        }

        public int? OrdemServicoPrimaria
        {
            get { return _ordemServicoPrimaria; }
            set
            {
                if (value != _ordemServicoPrimaria)
                {
                    _ordemServicoPrimaria = value;
                    OnPropertyChanged(nameof(OrdemServicoPrimaria));
                }
            }
        }

        public int? NumeroChamado
        {
            get { return _numeroChamado; }
            set
            {
                if (value != _numeroChamado)
                {
                    _numeroChamado = value;
                    OnPropertyChanged(nameof(NumeroChamado));
                }
            }
        }

        public DateTime? DataChamado
        {
            get { return _dataChamado; }
            set
            {
                if (value != _dataChamado)
                {
                    _dataChamado = value;
                    OnPropertyChanged(nameof(DataChamado));
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

        public DateTime? DataAtendimento
        {
            get { return _dataAtendimento; }
            set
            {
                if (value != _dataAtendimento)
                {
                    _dataAtendimento = value;
                    OnPropertyChanged(nameof(DataAtendimento));
                }
            }
        }

        public string? Mastro
        {
            get { return _mastro; }
            set
            {
                if (value != _mastro)
                {
                    _mastro = value;
                    OnPropertyChanged(nameof(Mastro));
                }
            }
        }

        public string? CodigoFalha
        {
            get { return _codigoFalha; }
            set
            {
                if (value != _codigoFalha)
                {
                    _codigoFalha = value;
                    OnPropertyChanged(nameof(CodigoFalha));
                }
            }
        }

        public int? EtapasConcluidas
        {
            get { return _etapasConcluidas; }
            set
            {
                if (value != _etapasConcluidas)
                {
                    _etapasConcluidas = value;
                    OnPropertyChanged(nameof(EtapasConcluidas));
                }
            }
        }

        public bool? EquipamentoOperacional
        {
            get { return _equipamentoOperacional; }
            set
            {
                if (value != _equipamentoOperacional)
                {
                    _equipamentoOperacional = value;
                    OnPropertyChanged(nameof(EquipamentoOperacional));

                    if (EquipamentoOperacional == null)
                        EquipamentoOperacionalTexto = null;
                    if (EquipamentoOperacional == true)
                        EquipamentoOperacionalTexto = "Sim";
                    if (EquipamentoOperacional == false)
                        EquipamentoOperacionalTexto = "Não";
                }
            }
        }

        public string? EquipamentoOperacionalTexto
        {
            get { return _equipamentoOperacionalTexto; }
            set
            {
                if (value != _equipamentoOperacionalTexto)
                {
                    _equipamentoOperacionalTexto = value;
                    OnPropertyChanged(nameof(EquipamentoOperacionalTexto));
                }
            }
        }

        public decimal? Horimetro
        {
            get { return _horimetro; }
            set
            {
                if (value != _horimetro)
                {
                    _horimetro = value;
                    OnPropertyChanged(nameof(Horimetro));
                }
            }
        }

        public decimal? HorasPreventiva
        {
            get { return _horasPreventiva; }
            set
            {
                if (value != _horasPreventiva)
                {
                    _horasPreventiva = value;
                    OnPropertyChanged(nameof(HorasPreventiva));
                }
            }
        }

        public string? OutroTipoManutencao
        {
            get { return _outroTipoManutencao; }
            set
            {
                if (value != _outroTipoManutencao)
                {
                    _outroTipoManutencao = value;
                    OnPropertyChanged(nameof(OutroTipoManutencao));
                }
            }
        }

        public string? MotivoAtendimento
        {
            get { return _motivoAtendimento; }
            set
            {
                if (value != _motivoAtendimento)
                {
                    _motivoAtendimento = value;
                    OnPropertyChanged(nameof(MotivoAtendimento));
                }
            }
        }

        public string? EntrevistaInicial
        {
            get { return _entrevistaInicial; }
            set
            {
                if (value != _entrevistaInicial)
                {
                    _entrevistaInicial = value;
                    OnPropertyChanged(nameof(EntrevistaInicial));
                }
            }
        }

        public string? Intervencao
        {
            get { return _intervencao; }
            set
            {
                if (value != _intervencao)
                {
                    _intervencao = value;
                    OnPropertyChanged(nameof(Intervencao));
                }
            }
        }

        public DateTime? DataSaida
        {
            get { return _dataSaida; }
            set
            {
                if (value != _dataSaida)
                {
                    _dataSaida = value;
                    OnPropertyChanged(nameof(DataSaida));
                }
            }
        }

        public DateTime? DataChegada
        {
            get { return _dataChegada; }
            set
            {
                if (value != _dataChegada)
                {
                    _dataChegada = value;
                    OnPropertyChanged(nameof(DataChegada));
                }
            }
        }

        public DateTime? DataRetorno
        {
            get { return _dataRetorno; }
            set
            {
                if (value != _dataRetorno)
                {
                    _dataRetorno = value;
                    OnPropertyChanged(nameof(DataRetorno));
                }
            }
        }

        public DateTime? DataEdicao
        {
            get { return _dataEdicao; }
            set
            {
                if (value != _dataEdicao)
                {
                    _dataEdicao = value;
                    OnPropertyChanged(nameof(DataEdicao));
                }
            }
        }

        public string? Comentarios
        {
            get { return _comentarios; }
            set
            {
                if (value != _comentarios)
                {
                    _comentarios = value;
                    OnPropertyChanged(nameof(Comentarios));
                }
            }
        }

        public int? IdUsuarioEmUso
        {
            get { return _idUsuarioEmUso; }
            set
            {
                if (value != _idUsuarioEmUso)
                {
                    _idUsuarioEmUso = value;
                    OnPropertyChanged(nameof(IdUsuarioEmUso));
                }
            }
        }

        public Filial? Filial
        {
            get { return _filial; }
            set
            {
                if (value != _filial)
                {
                    _filial = value;
                    OnPropertyChanged(nameof(Filial));
                }
            }
        }

        public Cliente? Cliente
        {
            get { return _cliente; }
            set
            {
                if (value != _cliente)
                {
                    _cliente = value;
                    OnPropertyChanged(nameof(Cliente));
                }
            }
        }

        public Frota? Frota
        {
            get { return _frota; }
            set
            {
                if (value != _frota)
                {
                    _frota = value;
                    OnPropertyChanged(nameof(Frota));
                }
            }
        }

        public Usuario? UsuarioInsercao
        {
            get { return _usuarioInsercao; }
            set
            {
                if (value != _usuarioInsercao)
                {
                    _usuarioInsercao = value;
                    OnPropertyChanged(nameof(UsuarioInsercao));
                }
            }
        }

        public Usuario? UsuarioEdicao
        {
            get { return _usuarioEdicao; }
            set
            {
                if (value != _usuarioEdicao)
                {
                    _usuarioEdicao = value;
                    OnPropertyChanged(nameof(UsuarioEdicao));
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

        public Serie? Serie
        {
            get { return _serie; }
            set
            {
                if (value != _serie)
                {
                    _serie = value;
                    OnPropertyChanged(nameof(Serie));
                }
            }
        }

        public TipoOrdemServico? TipoOrdemServico
        {
            get { return _tipoOrdemServico; }
            set
            {
                if (value != _tipoOrdemServico)
                {
                    _tipoOrdemServico = value;
                    OnPropertyChanged(nameof(TipoOrdemServico));
                }
            }
        }

        public StatusEquipamentoAposManutencao? StatusEquipamentoAposManutencao
        {
            get { return _statusEquipamentoAposManutencao; }
            set
            {
                if (value != _statusEquipamentoAposManutencao)
                {
                    _statusEquipamentoAposManutencao = value;
                    OnPropertyChanged(nameof(StatusEquipamentoAposManutencao));
                }
            }
        }

        public UsoIndevido? UsoIndevido
        {
            get { return _usoIndevido; }
            set
            {
                if (value != _usoIndevido)
                {
                    _usoIndevido = value;
                    OnPropertyChanged(nameof(UsoIndevido));
                }
            }
        }

        public TipoManutencao? TipoManutencao
        {
            get { return _tipoManutencao; }
            set
            {
                if (value != _tipoManutencao)
                {
                    _tipoManutencao = value;
                    OnPropertyChanged(nameof(TipoManutencao));
                }
            }
        }

        public ExecutanteServico? ExecutanteServico
        {
            get { return _executanteServico; }
            set
            {
                if (value != _executanteServico)
                {
                    _executanteServico = value;
                    OnPropertyChanged(nameof(ExecutanteServico));
                }
            }
        }

        public ObservableCollection<ItemOrdemServico> ListaItensOrdemServico
        {
            get { return _listaItensOrdemServico; }
            set
            {
                if (value != _listaItensOrdemServico)
                {
                    _listaItensOrdemServico = value;
                    OnPropertyChanged(nameof(ListaItensOrdemServico));
                }
            }
        }

        public ObservableCollection<EventoOrdemServico> ListaEventosOrdemServico
        {
            get { return _listaEventosOrdemServico; }
            set
            {
                if (value != _listaEventosOrdemServico)
                {
                    _listaEventosOrdemServico = value;
                    OnPropertyChanged(nameof(ListaEventosOrdemServico));
                }
            }
        }

        public ObservableCollection<InconsistenciaOrdemServico> ListaInconsistenciasOrdemServico
        {
            get { return _listaInconsistenciasOrdemServico; }
            set
            {
                if (value != _listaInconsistenciasOrdemServico)
                {
                    _listaInconsistenciasOrdemServico = value;
                    OnPropertyChanged(nameof(ListaInconsistenciasOrdemServico));
                }
            }
        }

        #endregion Propriedades

        #region Construtores

        /// <summary>
        /// Construtor da ordem de serviço com os parâmetros utilizados
        /// </summary>
        /// <param name="inicializaItensOrdemServico">Indica se a classe deve ser inicializada. Deve-se ter cuidado e levar em consideração loops infinitos</param>
        /// <param name="inicializaEventosOrdemServico">Indica se a classe deve ser inicializada. Deve-se ter cuidado e levar em consideração loops infinitos</param>
        /// <param name="inicializaInconsistenciasOrdemServico">Indica se a classe deve ser inicializada. Deve-se ter cuidado e levar em consideração loops infinitos</param>
        public OrdemServico(bool inicializaItensOrdemServico = false, bool inicializaEventosOrdemServico = false, bool inicializaInconsistenciasOrdemServico = false)
        {
            Filial = new();
            Cliente = new();
            Frota = new();
            UsuarioInsercao = new();
            UsuarioEdicao = new();
            Status = new();
            Serie = new();
            TipoOrdemServico = new();
            StatusEquipamentoAposManutencao = new();
            UsoIndevido = new();
            TipoManutencao = new();
            ExecutanteServico = new();

            if (inicializaItensOrdemServico)
            {
                ListaItensOrdemServico = new();
            }

            if (inicializaEventosOrdemServico)
            {
                ListaEventosOrdemServico = new();
            }

            if (inicializaInconsistenciasOrdemServico)
            {
                ListaInconsistenciasOrdemServico = new();
            }
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados da ordem de serviço com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id do setor que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="retornaItens">Representa a opção de preencher ou não a lista de itens dessa ordem de serviço</param>
        /// <param name="retornaEventos">Representa a opção de preencher ou não a lista de eventos dessa ordem de serviço</param>
        public async Task GetOrdemServicoDatabaseAsync(int? id, CancellationToken ct, bool retornaItens = false, bool retornaEventos = false, bool retornaInconsistencias = false)
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
                                        + "orse.id_ordem_servico AS Id, "
                                        + "orse.ordem_servico_atual AS OrdemServicoAtual, "
                                        + "orse.ordem_servico_primaria AS OrdemServicoPrimaria, "
                                        + "orse.numero_chamado AS NumeroChamado, "
                                        + "orse.data_chamado AS DataChamado, "
                                        + "orse.data_atendimento AS DataAtendimento, "
                                        + "orse.data_insercao AS DataInsercao, "
                                        + "orse.mastro AS Mastro, "
                                        + "orse.codigo_falha AS CodigoFalha, "
                                        + "orse.etapas_concluidas AS EtapasConcluidas, "
                                        + "orse.equipamento_operacional AS EquipamentoOperacional, "
                                        + "orse.horimetro AS Horimetro, "
                                        + "orse.horas_preventiva AS HorasPreventiva, "
                                        + "orse.outro_tipo_manutencao AS OutroTipoManutencao, "
                                        + "orse.motivo_atendimento AS MotivoAtendimento, "
                                        + "orse.entrevista_inicial AS EntrevistaInicial, "
                                        + "orse.intervencao AS Intervencao, "
                                        + "orse.data_saida AS DataSaida, "
                                        + "orse.data_chegada AS DataChegada, "
                                        + "orse.data_retorno AS DataRetorno, "
                                        + "orse.data_edicao AS DataEdicao, "
                                        + "orse.comentarios AS Comentarios, "
                                        + "orse.id_usuario_em_uso AS IdUsuarioEmUso, "
                                        + "orse.id_filial AS IdFilial, "
                                        + "orse.id_cliente AS IdCliente, "
                                        + "orse.id_frota AS IdFrota, "
                                        + "orse.id_usuario_insercao AS IdUsuarioInsercao, "
                                        + "orse.id_usuario_edicao AS IdUsuarioEdicao, "
                                        + "orse.id_status AS IdStatus, "
                                        + "orse.id_serie AS IdSerie, "
                                        + "orse.id_tipo_ordem_servico AS IdTipoOrdemServico, "
                                        + "orse.id_status_equipamento_apos_manutencao AS IdStatusEquipamentoAposManutencao, "
                                        + "orse.id_uso_indevido AS IdUsoIndevido, "
                                        + "orse.id_tipo_manutencao AS IdTipoManutencao, "
                                        + "orse.id_executante_servico AS IdExecutanteServico "
                                        + "FROM tb_ordens_servico AS orse "
                                        + "WHERE orse.id_ordem_servico = @id";

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
                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Status == null)
                                {
                                    Status = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial == null)
                                {
                                    Filial = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Cliente == null)
                                {
                                    Cliente = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota == null)
                                {
                                    Frota = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (UsuarioInsercao == null)
                                {
                                    UsuarioInsercao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (UsuarioEdicao == null)
                                {
                                    UsuarioEdicao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Serie == null)
                                {
                                    Serie = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoOrdemServico == null)
                                {
                                    TipoOrdemServico = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (StatusEquipamentoAposManutencao == null)
                                {
                                    StatusEquipamentoAposManutencao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (UsoIndevido == null)
                                {
                                    UsoIndevido = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoManutencao == null)
                                {
                                    TipoManutencao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (ExecutanteServico == null)
                                {
                                    ExecutanteServico = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaItens == true && ListaItensOrdemServico == null)
                                {
                                    ListaItensOrdemServico = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaEventos == true && ListaEventosOrdemServico == null)
                                {
                                    ListaEventosOrdemServico = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaInconsistencias == true && ListaInconsistenciasOrdemServico == null)
                                {
                                    ListaInconsistenciasOrdemServico = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                OrdemServicoAtual = FuncoesDeConversao.ConverteParaInt(reader["OrdemServicoAtual"]);
                                OrdemServicoPrimaria = FuncoesDeConversao.ConverteParaInt(reader["OrdemServicoPrimaria"]);
                                NumeroChamado = FuncoesDeConversao.ConverteParaInt(reader["NumeroChamado"]);
                                DataChamado = FuncoesDeConversao.ConverteParaDateTime(reader["DataChamado"]);
                                DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                DataAtendimento = FuncoesDeConversao.ConverteParaDateTime(reader["DataAtendimento"]);
                                Mastro = FuncoesDeConversao.ConverteParaString(reader["Mastro"]);
                                CodigoFalha = FuncoesDeConversao.ConverteParaString(reader["CodigoFalha"]);
                                EtapasConcluidas = FuncoesDeConversao.ConverteParaInt(reader["EtapasConcluidas"]);
                                EquipamentoOperacional = FuncoesDeConversao.ConverteParaBool(reader["EquipamentoOperacional"]);
                                Horimetro = FuncoesDeConversao.ConverteParaDecimal(reader["Horimetro"]);

                                HorasPreventiva = FuncoesDeConversao.ConverteParaDecimal(reader["HorasPreventiva"]);
                                OutroTipoManutencao = FuncoesDeConversao.ConverteParaString(reader["OutroTipoManutencao"]);

                                MotivoAtendimento = FuncoesDeConversao.ConverteParaString(reader["MotivoAtendimento"]);
                                EntrevistaInicial = FuncoesDeConversao.ConverteParaString(reader["EntrevistaInicial"]);
                                Intervencao = FuncoesDeConversao.ConverteParaString(reader["Intervencao"]);
                                DataSaida = FuncoesDeConversao.ConverteParaDateTime(reader["DataSaida"]);
                                DataChegada = FuncoesDeConversao.ConverteParaDateTime(reader["DataChegada"]);
                                DataRetorno = FuncoesDeConversao.ConverteParaDateTime(reader["DataRetorno"]);
                                DataEdicao = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicao"]);
                                Comentarios = FuncoesDeConversao.ConverteParaString(reader["Comentarios"]);
                                IdUsuarioEmUso = FuncoesDeConversao.ConverteParaInt(reader["IdUsuarioEmUso"]);

                                Filial.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFilial"]);
                                Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Frota.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFrota"]);
                                UsuarioInsercao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdUsuarioInsercao"]);
                                UsuarioEdicao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdUsuarioEdicao"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatus"]);
                                Serie.Id = FuncoesDeConversao.ConverteParaInt(reader["IdSerie"]);
                                TipoOrdemServico.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoOrdemServico"]);
                                StatusEquipamentoAposManutencao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEquipamentoAposManutencao"]);
                                UsoIndevido.Id = FuncoesDeConversao.ConverteParaInt(reader["IdUsoIndevido"]);
                                TipoManutencao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoManutencao"]);
                                ExecutanteServico.Id = FuncoesDeConversao.ConverteParaInt(reader["IdExecutanteServico"]);
                            }
                        }
                    }
                }
            }

            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Define as classes
            await Filial.GetFilialDatabaseAsync(Filial.Id, ct);
            await Cliente.GetClienteDatabaseAsync(Cliente.Id, ct);
            await Frota.GetFrotaDatabaseAsync(Frota.Id, ct);
            await UsuarioInsercao.GetUsuarioDatabaseAsync(UsuarioInsercao.Id, ct);
            await UsuarioEdicao.GetUsuarioDatabaseAsync(UsuarioEdicao.Id, ct);
            await Status.GetStatusDatabaseAsync(Status.Id, ct);
            await Serie.GetSerieDatabaseAsync(Serie.Id, ct);
            await TipoOrdemServico.GetTipoOrdemServicoDatabaseAsync(TipoOrdemServico.Id, ct);
            await StatusEquipamentoAposManutencao.GetStatusEquipamentoAposManutencaoDatabaseAsync(StatusEquipamentoAposManutencao.Id, ct);
            await UsoIndevido.GetUsoIndevidoDatabaseAsync(UsoIndevido.Id, ct);
            await TipoManutencao.GetTipoManutencaoDatabaseAsync(TipoManutencao.Id, ct);
            await ExecutanteServico.GetExecutanteServicoDatabaseAsync(ExecutanteServico.Id, ct);

            // Caso verdadeiro, a lista de itens da ordem de serviço será retornada
            if (retornaItens)
            {
                await ItemOrdemServico.PreencheListaItensOrdemServicoAsync(ListaItensOrdemServico, true, false, null,
                    ct, "WHERE itos.id_ordem_servico = @id_ordem_servico ORDER BY itos.id_ordem_servico ASC", "@id_ordem_servico", Id);

                foreach (var item in ListaItensOrdemServico)
                {
                    await item.Status.GetStatusDatabaseAsync(item.Status.Id, ct);

                    await item.Conjunto.GetConjuntoDatabaseAsync(item.Conjunto.Id, ct);
                    await item.Especificacao.GetEspecificacaoDatabaseAsync(item.Especificacao.Id, ct);
                    await item.Usuario.GetUsuarioDatabaseAsync(item.Usuario.Id, ct);
                    await item.MotivoItemOrdemServico.GetMotivoItemOrdemServicoDatabaseAsync(item.MotivoItemOrdemServico.Id, ct);
                    await item.FornecimentoItemOrdemServico.GetFornecimentoItemOrdemServicoDatabaseAsync(item.FornecimentoItemOrdemServico.Id, ct);
                }
            }

            // Caso verdadeiro, a lista de eventos da ordem de serviço será retornada
            if (retornaEventos)
            {
                await EventoOrdemServico.PreencheListaEventosOrdemServicoAsync(ListaEventosOrdemServico, true, false, null,
                    ct, "WHERE evos.id_ordem_servico = @id_ordem_servico ORDER BY evos.id_ordem_servico ASC", "@id_ordem_servico", Id);

                foreach (var item in ListaEventosOrdemServico)
                {
                    await item.Status.GetStatusDatabaseAsync(item.Status.Id, ct);
                    await item.Usuario.GetUsuarioDatabaseAsync(item.Usuario.Id, ct);
                    await item.Evento.GetEventoDatabaseAsync(item.Evento.Id, ct);
                }
            }

            // Caso verdadeiro, a lista de inconsistencias da ordem de serviço será retornada
            if (retornaInconsistencias)
            {
                await InconsistenciaOrdemServico.PreencheListaInconsistenciasOrdemServicoAsync(ListaInconsistenciasOrdemServico, true, false, null,
                    ct, "WHERE inos.id_ordem_servico = @id_ordem_servico ORDER BY inos.id_ordem_servico ASC", "@id_ordem_servico", Id);

                foreach (var item in ListaInconsistenciasOrdemServico)
                {
                    await item.Status.GetStatusDatabaseAsync(item.Status.Id, ct);
                    await item.Usuario.GetUsuarioDatabaseAsync(item.Usuario.Id, ct);
                    await item.Inconsistencia.GetInconsistenciaDatabaseAsync(item.Inconsistencia.Id, ct);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados da ordem de serviço com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="retornaItens">Representa a opção de preencher ou não a lista de itens dessa ordem de serviço</param>
        /// <param name="retornaEventos">Representa a opção de preencher ou não a lista de eventos dessa ordem de serviço</param>
        /// <param name="retornaInconsistencias">Representa a opção de preencher ou não a lista de inconsistencias dessa ordem de serviço</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetOrdemServicoDatabaseAsync(CancellationToken ct, bool retornaItens, bool retornaEventos, bool retornaInconsistencias, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                                    + "orse.id_ordem_servico AS Id, "
                                    + "orse.ordem_servico_atual AS OrdemServicoAtual, "
                                    + "orse.ordem_servico_primaria AS OrdemServicoPrimaria, "
                                    + "orse.numero_chamado AS NumeroChamado, "
                                    + "orse.data_chamado AS DataChamado, "
                                    + "orse.data_insercao AS DataInsercao, "
                                    + "orse.data_atendimento AS DataAtendimento, "
                                    + "orse.mastro AS Mastro, "
                                    + "orse.codigo_falha AS CodigoFalha, "
                                    + "orse.etapas_concluidas AS EtapasConcluidas, "
                                    + "orse.equipamento_operacional AS EquipamentoOperacional, "
                                    + "orse.horimetro AS Horimetro, "
                                    + "orse.horas_preventiva AS HorasPreventiva, "
                                    + "orse.outro_tipo_manutencao AS OutroTipoManutencao, "
                                    + "orse.motivo_atendimento AS MotivoAtendimento, "
                                    + "orse.entrevista_inicial AS EntrevistaInicial, "
                                    + "orse.intervencao AS Intervencao, "
                                    + "orse.data_saida AS DataSaida, "
                                    + "orse.data_chegada AS DataChegada, "
                                    + "orse.data_retorno AS DataRetorno, "
                                    + "orse.data_edicao AS DataEdicao, "
                                    + "orse.comentarios AS Comentarios, "
                                    + "orse.id_usuario_em_uso AS IdUsuarioEmUso, "
                                    + "orse.id_filial AS IdFilial, "
                                    + "orse.id_cliente AS IdCliente, "
                                    + "orse.id_frota AS IdFrota, "
                                    + "orse.id_usuario_insercao AS IdUsuarioInsercao, "
                                    + "orse.id_usuario_edicao AS IdUsuarioEdicao, "
                                    + "orse.id_status AS IdStatus, "
                                    + "orse.id_serie AS IdSerie, "
                                    + "orse.id_tipo_ordem_servico AS IdTipoOrdemServico, "
                                    + "orse.id_status_equipamento_apos_manutencao AS IdStatusEquipamentoAposManutencao, "
                                    + "orse.id_uso_indevido AS IdUsoIndevido, "
                                    + "orse.id_tipo_manutencao AS IdTipoManutencao, "
                                    + "orse.id_executante_servico AS IdExecutanteServico "
                                    + "FROM tb_ordens_servico AS orse "
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
                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Status == null)
                                {
                                    Status = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial == null)
                                {
                                    Filial = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Cliente == null)
                                {
                                    Cliente = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota == null)
                                {
                                    Frota = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (UsuarioInsercao == null)
                                {
                                    UsuarioInsercao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (UsuarioEdicao == null)
                                {
                                    UsuarioEdicao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Serie == null)
                                {
                                    Serie = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoOrdemServico == null)
                                {
                                    TipoOrdemServico = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (StatusEquipamentoAposManutencao == null)
                                {
                                    StatusEquipamentoAposManutencao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (UsoIndevido == null)
                                {
                                    UsoIndevido = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoManutencao == null)
                                {
                                    TipoManutencao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (ExecutanteServico == null)
                                {
                                    ExecutanteServico = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaItens == true && ListaItensOrdemServico == null)
                                {
                                    ListaItensOrdemServico = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaEventos == true && ListaEventosOrdemServico == null)
                                {
                                    ListaEventosOrdemServico = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaInconsistencias == true && ListaInconsistenciasOrdemServico == null)
                                {
                                    ListaInconsistenciasOrdemServico = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                OrdemServicoAtual = FuncoesDeConversao.ConverteParaInt(reader["OrdemServicoAtual"]);
                                OrdemServicoPrimaria = FuncoesDeConversao.ConverteParaInt(reader["OrdemServicoPrimaria"]);
                                NumeroChamado = FuncoesDeConversao.ConverteParaInt(reader["NumeroChamado"]);
                                DataChamado = FuncoesDeConversao.ConverteParaDateTime(reader["DataChamado"]);
                                DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                DataAtendimento = FuncoesDeConversao.ConverteParaDateTime(reader["DataAtendimento"]);
                                Mastro = FuncoesDeConversao.ConverteParaString(reader["Mastro"]);
                                CodigoFalha = FuncoesDeConversao.ConverteParaString(reader["CodigoFalha"]);
                                EtapasConcluidas = FuncoesDeConversao.ConverteParaInt(reader["EtapasConcluidas"]);
                                EquipamentoOperacional = FuncoesDeConversao.ConverteParaBool(reader["EquipamentoOperacional"]);
                                Horimetro = FuncoesDeConversao.ConverteParaDecimal(reader["Horimetro"]);
                                HorasPreventiva = FuncoesDeConversao.ConverteParaDecimal(reader["HorasPreventiva"]);
                                OutroTipoManutencao = FuncoesDeConversao.ConverteParaString(reader["OutroTipoManutencao"]);
                                MotivoAtendimento = FuncoesDeConversao.ConverteParaString(reader["MotivoAtendimento"]);
                                EntrevistaInicial = FuncoesDeConversao.ConverteParaString(reader["EntrevistaInicial"]);
                                Intervencao = FuncoesDeConversao.ConverteParaString(reader["Intervencao"]);
                                DataSaida = FuncoesDeConversao.ConverteParaDateTime(reader["DataSaida"]);
                                DataChegada = FuncoesDeConversao.ConverteParaDateTime(reader["DataChegada"]);
                                DataRetorno = FuncoesDeConversao.ConverteParaDateTime(reader["DataRetorno"]);
                                DataEdicao = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicao"]);
                                Comentarios = FuncoesDeConversao.ConverteParaString(reader["Comentarios"]);
                                IdUsuarioEmUso = FuncoesDeConversao.ConverteParaInt(reader["IdUsuarioEmUso"]);

                                Filial.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFilial"]);
                                Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Frota.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFrota"]);
                                UsuarioInsercao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdUsuarioInsercao"]);
                                UsuarioEdicao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdUsuarioEdicao"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatus"]);
                                Serie.Id = FuncoesDeConversao.ConverteParaInt(reader["IdSerie"]);
                                TipoOrdemServico.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoOrdemServico"]);
                                StatusEquipamentoAposManutencao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEquipamentoAposManutencao"]);
                                UsoIndevido.Id = FuncoesDeConversao.ConverteParaInt(reader["IdUsoIndevido"]);
                                TipoManutencao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoManutencao"]);
                                ExecutanteServico.Id = FuncoesDeConversao.ConverteParaInt(reader["IdExecutanteServico"]);
                            }
                        }
                    }
                }
            }

            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Define as classes
            await Filial.GetFilialDatabaseAsync(Filial.Id, ct);
            await Cliente.GetClienteDatabaseAsync(Cliente.Id, ct);
            await Frota.GetFrotaDatabaseAsync(Frota.Id, ct);
            await UsuarioInsercao.GetUsuarioDatabaseAsync(UsuarioInsercao.Id, ct);
            await UsuarioEdicao.GetUsuarioDatabaseAsync(UsuarioEdicao.Id, ct);
            await Status.GetStatusDatabaseAsync(Status.Id, ct);
            await Serie.GetSerieDatabaseAsync(Serie.Id, ct);
            await TipoOrdemServico.GetTipoOrdemServicoDatabaseAsync(TipoOrdemServico.Id, ct);
            await StatusEquipamentoAposManutencao.GetStatusEquipamentoAposManutencaoDatabaseAsync(StatusEquipamentoAposManutencao.Id, ct);
            await UsoIndevido.GetUsoIndevidoDatabaseAsync(UsoIndevido.Id, ct);
            await TipoManutencao.GetTipoManutencaoDatabaseAsync(TipoManutencao.Id, ct);
            await ExecutanteServico.GetExecutanteServicoDatabaseAsync(ExecutanteServico.Id, ct);

            // Caso verdadeiro, a lista de itens da ordem de serviço será retornada
            if (retornaItens)
            {
                await ItemOrdemServico.PreencheListaItensOrdemServicoAsync(ListaItensOrdemServico, true, false, null,
                    ct, "WHERE itos.id_ordem_servico = @id_ordem_servico ORDER BY itos.id_ordem_servico ASC", "@id_ordem_servico", Id);

                foreach (var item in ListaItensOrdemServico)
                {
                    await item.Status.GetStatusDatabaseAsync(item.Status.Id, ct);

                    await item.Conjunto.GetConjuntoDatabaseAsync(item.Conjunto.Id, ct);
                    await item.Especificacao.GetEspecificacaoDatabaseAsync(item.Especificacao.Id, ct);
                    await item.Usuario.GetUsuarioDatabaseAsync(item.Usuario.Id, ct);
                    await item.MotivoItemOrdemServico.GetMotivoItemOrdemServicoDatabaseAsync(item.MotivoItemOrdemServico.Id, ct);
                    await item.FornecimentoItemOrdemServico.GetFornecimentoItemOrdemServicoDatabaseAsync(item.FornecimentoItemOrdemServico.Id, ct);
                }
            }

            // Caso verdadeiro, a lista de eventos da ordem de serviço será retornada
            if (retornaEventos)
            {
                await EventoOrdemServico.PreencheListaEventosOrdemServicoAsync(ListaEventosOrdemServico, true, false, null,
                    ct, "WHERE evos.id_ordem_servico = @id_ordem_servico ORDER BY evos.id_ordem_servico ASC", "@id_ordem_servico", Id);

                foreach (var item in ListaEventosOrdemServico)
                {
                    await item.Status.GetStatusDatabaseAsync(item.Status.Id, ct);
                    await item.Usuario.GetUsuarioDatabaseAsync(item.Usuario.Id, ct);
                    await item.Evento.GetEventoDatabaseAsync(item.Evento.Id, ct);
                }
            }

            // Caso verdadeiro, a lista de inconsistencias da ordem de serviço será retornada
            if (retornaInconsistencias)
            {
                await InconsistenciaOrdemServico.PreencheListaInconsistenciasOrdemServicoAsync(ListaInconsistenciasOrdemServico, true, false, null,
                    ct, "WHERE inos.id_ordem_servico = @id_ordem_servico ORDER BY inos.id_ordem_servico ASC", "@id_ordem_servico", Id);

                foreach (var item in ListaInconsistenciasOrdemServico)
                {
                    await item.Status.GetStatusDatabaseAsync(item.Status.Id, ct);
                    await item.Usuario.GetUsuarioDatabaseAsync(item.Usuario.Id, ct);
                    await item.Inconsistencia.GetInconsistenciaDatabaseAsync(item.Inconsistencia.Id, ct);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva a ordem de serviço na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarOrdemServicoDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_ordem_servico", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_ordem_servico_atual", OrdemServicoAtual);
                        command.Parameters.AddWithValue("p_ordem_servico_primaria", OrdemServicoPrimaria);
                        command.Parameters.AddWithValue("p_numero_chamado", NumeroChamado);
                        command.Parameters.AddWithValue("p_data_chamado", DataChamado);
                        command.Parameters.AddWithValue("p_data_insercao", DateTime.Now);
                        command.Parameters.AddWithValue("p_data_atendimento", DataAtendimento);
                        command.Parameters.AddWithValue("p_mastro", Mastro);
                        command.Parameters.AddWithValue("p_codigo_falha", CodigoFalha);
                        command.Parameters.AddWithValue("p_etapas_concluidas", EtapasConcluidas);
                        command.Parameters.AddWithValue("p_equipamento_operacional", EquipamentoOperacional);
                        command.Parameters.AddWithValue("p_horimetro", Horimetro);
                        command.Parameters.AddWithValue("p_horas_preventiva", HorasPreventiva);
                        command.Parameters.AddWithValue("p_outro_tipo_manutencao", OutroTipoManutencao);
                        command.Parameters.AddWithValue("p_motivo_atendimento", MotivoAtendimento);
                        command.Parameters.AddWithValue("p_entrevista_inicial", EntrevistaInicial);
                        command.Parameters.AddWithValue("p_intervencao", Intervencao);
                        command.Parameters.AddWithValue("p_data_saida", DataSaida);
                        command.Parameters.AddWithValue("p_data_chegada", DataChegada);
                        command.Parameters.AddWithValue("p_data_retorno", DataRetorno);
                        command.Parameters.AddWithValue("p_comentarios", Comentarios);
                        command.Parameters.AddWithValue("p_id_usuario_em_uso", IdUsuarioEmUso);
                        command.Parameters.AddWithValue("p_id_filial", Filial == null ? null : Filial.Id);
                        command.Parameters.AddWithValue("p_id_cliente", Cliente == null ? null : Cliente.Id);
                        command.Parameters.AddWithValue("p_id_frota", Frota == null ? null : Frota.Id);
                        command.Parameters.AddWithValue("p_id_usuario_insercao", UsuarioInsercao.Id);
                        command.Parameters.AddWithValue("p_id_status", Status == null ? null : Status.Id);
                        command.Parameters.AddWithValue("p_id_serie", Serie == null ? null : Serie.Id);
                        command.Parameters.AddWithValue("p_id_tipo_ordem_servico", TipoOrdemServico == null ? null : TipoOrdemServico.Id);
                        command.Parameters.AddWithValue("p_id_status_equipamento_apos_manutencao", StatusEquipamentoAposManutencao == null ? null : StatusEquipamentoAposManutencao.Id);
                        command.Parameters.AddWithValue("p_id_uso_indevido", UsoIndevido == null ? null : UsoIndevido.Id);
                        command.Parameters.AddWithValue("p_id_tipo_manutencao", TipoManutencao == null ? null : TipoManutencao.Id);
                        command.Parameters.AddWithValue("p_id_executante_servico", ExecutanteServico == null ? null : ExecutanteServico.Id);
                        command.Parameters.Add("p_id_ordem_servico", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException("A ordem de serviço " + OrdemServicoAtual + " já existe");
                        }
                        else
                        {
                            Id = FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_ordem_servico"].Value);
                        }
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_ordem_servico", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_ordem_servico", Id);
                        command.Parameters.AddWithValue("p_ordem_servico_atual", OrdemServicoAtual);
                        command.Parameters.AddWithValue("p_ordem_servico_primaria", OrdemServicoPrimaria);
                        command.Parameters.AddWithValue("p_numero_chamado", NumeroChamado);
                        command.Parameters.AddWithValue("p_data_chamado", DataChamado);
                        command.Parameters.AddWithValue("p_data_edicao", DataEdicao);
                        command.Parameters.AddWithValue("p_data_atendimento", DataAtendimento);
                        command.Parameters.AddWithValue("p_mastro", Mastro);
                        command.Parameters.AddWithValue("p_codigo_falha", CodigoFalha);
                        command.Parameters.AddWithValue("p_etapas_concluidas", EtapasConcluidas);
                        command.Parameters.AddWithValue("p_equipamento_operacional", EquipamentoOperacional);
                        command.Parameters.AddWithValue("p_horimetro", Horimetro);
                        command.Parameters.AddWithValue("p_horas_preventiva", HorasPreventiva);
                        command.Parameters.AddWithValue("p_outro_tipo_manutencao", OutroTipoManutencao);
                        command.Parameters.AddWithValue("p_motivo_atendimento", MotivoAtendimento);
                        command.Parameters.AddWithValue("p_entrevista_inicial", EntrevistaInicial);
                        command.Parameters.AddWithValue("p_intervencao", Intervencao);
                        command.Parameters.AddWithValue("p_data_saida", DataSaida);
                        command.Parameters.AddWithValue("p_data_chegada", DataChegada);
                        command.Parameters.AddWithValue("p_data_retorno", DataRetorno);
                        command.Parameters.AddWithValue("p_comentarios", Comentarios);
                        command.Parameters.AddWithValue("p_id_usuario_em_uso", IdUsuarioEmUso);
                        command.Parameters.AddWithValue("p_id_filial", Filial == null ? null : Filial.Id);
                        command.Parameters.AddWithValue("p_id_cliente", Cliente == null ? null : Cliente.Id);
                        command.Parameters.AddWithValue("p_id_frota", Frota == null ? null : Frota.Id);
                        command.Parameters.AddWithValue("p_id_usuario_edicao", UsuarioEdicao.Id);
                        command.Parameters.AddWithValue("p_id_status", Status == null ? null : Status.Id);
                        command.Parameters.AddWithValue("p_id_serie", Serie == null ? null : Serie.Id);
                        command.Parameters.AddWithValue("p_id_tipo_ordem_servico", TipoOrdemServico == null ? null : TipoOrdemServico.Id);
                        command.Parameters.AddWithValue("p_id_status_equipamento_apos_manutencao", StatusEquipamentoAposManutencao == null ? null : StatusEquipamentoAposManutencao.Id);
                        command.Parameters.AddWithValue("p_id_uso_indevido", UsoIndevido == null ? null : UsoIndevido.Id);
                        command.Parameters.AddWithValue("p_id_tipo_manutencao", TipoManutencao == null ? null : TipoManutencao.Id);
                        command.Parameters.AddWithValue("p_id_executante_servico", ExecutanteServico == null ? null : ExecutanteServico.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException("A ordem de serviço " + OrdemServicoAtual + " não existe");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta a ordem de serviço na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="deletarItens">Valor booleano que indica se os itens da ordem de serviço também deverão ser excluídos. Verdadeiro por padrão</param>
        /// <param name="deletarEventos">Valor booleano que indica se os eventos da ordem de serviço também deverão ser excluídos. Verdadeiro por padrão</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarOrdemServicoDatabaseAsync(CancellationToken ct, bool deletarItens = true, bool deletarEventos = true)
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

                // Deleta os itens da ordem de serviço caso a opção seja verdadeira
                if (deletarItens)
                {
                    foreach (var item in ListaItensOrdemServico)
                    {
                        await item.DeletarItemOrdemServicoDatabaseAsync(ct);
                    }
                }

                // Deleta os eventos da ordem de serviço caso a opção seja verdadeira
                if (deletarEventos)
                {
                    foreach (var item in ListaEventosOrdemServico)
                    {
                        await item.DeletarEventoOrdemServicoDatabaseAsync(ct);
                    }
                }

                // Utilização do comando
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_ordem_servico", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_ordem_servico", Id);
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
                            throw new ChaveEstrangeiraEmUsoException("Ordem de serviço", OrdemServicoAtual);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("A ordem de serviço " + OrdemServicoAtual + " não existe");
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de ordens de serviço com os argumentos utilizados
        /// </summary>
        /// <param name="listaOrdensServico">Representa a lista de ordens de serviço que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="retornaItens">Representa a opção de preencher ou não a lista de itens dessa ordem de serviço</param>
        /// <param name="retornaEventos">Representa a opção de preencher ou não a lista de eventos dessa ordem de serviço</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaOrdensServicoAsync(ObservableCollection<OrdemServico> listaOrdensServico, bool limparLista, bool retornaItens, bool retornaEventos, bool retornaInconsistencias,
            IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaOrdensServico == null)
            {
                listaOrdensServico = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaOrdensServico.Clear();
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
                                    + "orse.id_ordem_servico AS Id, "
                                    + "orse.ordem_servico_atual AS OrdemServicoAtual, "
                                    + "orse.ordem_servico_primaria AS OrdemServicoPrimaria, "
                                    + "orse.numero_chamado AS NumeroChamado, "
                                    + "orse.data_chamado AS DataChamado, "
                                    + "orse.data_insercao AS DataInsercao, "
                                    + "orse.data_atendimento AS DataAtendimento, "
                                    + "orse.mastro AS Mastro, "
                                    + "orse.codigo_falha AS CodigoFalha, "
                                    + "orse.etapas_concluidas AS EtapasConcluidas, "
                                    + "orse.equipamento_operacional AS EquipamentoOperacional, "
                                    + "orse.horimetro AS Horimetro, "
                                    + "orse.horas_preventiva AS HorasPreventiva, "
                                    + "orse.outro_tipo_manutencao AS OutroTipoManutencao, "
                                    + "orse.motivo_atendimento AS MotivoAtendimento, "
                                    + "orse.entrevista_inicial AS EntrevistaInicial, "
                                    + "orse.intervencao AS Intervencao, "
                                    + "orse.data_saida AS DataSaida, "
                                    + "orse.data_chegada AS DataChegada, "
                                    + "orse.data_retorno AS DataRetorno, "
                                    + "orse.data_edicao AS DataEdicao, "
                                    + "orse.comentarios AS Comentarios, "
                                    + "orse.id_usuario_em_uso AS IdUsuarioEmUso, "
                                    + "orse.id_filial AS IdFilial, "
                                    + "orse.id_cliente AS IdCliente, "
                                    + "orse.id_frota AS IdFrota, "
                                    + "orse.id_usuario_insercao AS IdUsuarioInsercao, "
                                    + "orse.id_usuario_edicao AS IdUsuarioEdicao, "
                                    + "orse.id_status AS IdStatus, "
                                    + "orse.id_serie AS IdSerie, "
                                    + "orse.id_tipo_ordem_servico AS IdTipoOrdemServico, "
                                    + "orse.id_status_equipamento_apos_manutencao AS IdStatusEquipamentoAposManutencao, "
                                    + "orse.id_uso_indevido AS IdUsoIndevido, "
                                    + "orse.id_tipo_manutencao AS IdTipoManutencao, "
                                    + "orse.id_executante_servico AS IdExecutanteServico "
                                    + "FROM tb_ordens_servico AS orse "
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
                                OrdemServico item = new();

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Filial == null)
                                {
                                    item.Filial = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Cliente == null)
                                {
                                    item.Cliente = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota == null)
                                {
                                    item.Frota = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.UsuarioInsercao == null)
                                {
                                    item.UsuarioInsercao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.UsuarioEdicao == null)
                                {
                                    item.UsuarioEdicao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Serie == null)
                                {
                                    item.Serie = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.TipoOrdemServico == null)
                                {
                                    item.TipoOrdemServico = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.StatusEquipamentoAposManutencao == null)
                                {
                                    item.StatusEquipamentoAposManutencao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.UsoIndevido == null)
                                {
                                    item.UsoIndevido = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.TipoManutencao == null)
                                {
                                    item.TipoManutencao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.ExecutanteServico == null)
                                {
                                    item.ExecutanteServico = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaItens == true && item.ListaItensOrdemServico == null)
                                {
                                    item.ListaItensOrdemServico = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaEventos == true && item.ListaEventosOrdemServico == null)
                                {
                                    item.ListaEventosOrdemServico = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaInconsistencias == true && item.ListaInconsistenciasOrdemServico == null)
                                {
                                    item.ListaInconsistenciasOrdemServico = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                item.OrdemServicoAtual = FuncoesDeConversao.ConverteParaInt(reader["OrdemServicoAtual"]);
                                item.OrdemServicoPrimaria = FuncoesDeConversao.ConverteParaInt(reader["OrdemServicoPrimaria"]);
                                item.NumeroChamado = FuncoesDeConversao.ConverteParaInt(reader["NumeroChamado"]);
                                item.DataChamado = FuncoesDeConversao.ConverteParaDateTime(reader["DataChamado"]);
                                item.DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                item.DataAtendimento = FuncoesDeConversao.ConverteParaDateTime(reader["DataAtendimento"]);
                                item.Mastro = FuncoesDeConversao.ConverteParaString(reader["Mastro"]);
                                item.CodigoFalha = FuncoesDeConversao.ConverteParaString(reader["CodigoFalha"]);
                                item.EtapasConcluidas = FuncoesDeConversao.ConverteParaInt(reader["EtapasConcluidas"]);
                                item.EquipamentoOperacional = FuncoesDeConversao.ConverteParaBool(reader["EquipamentoOperacional"]);
                                item.Horimetro = FuncoesDeConversao.ConverteParaDecimal(reader["Horimetro"]);
                                item.HorasPreventiva = FuncoesDeConversao.ConverteParaDecimal(reader["HorasPreventiva"]);
                                item.OutroTipoManutencao = FuncoesDeConversao.ConverteParaString(reader["OutroTipoManutencao"]);
                                item.MotivoAtendimento = FuncoesDeConversao.ConverteParaString(reader["MotivoAtendimento"]);
                                item.EntrevistaInicial = FuncoesDeConversao.ConverteParaString(reader["EntrevistaInicial"]);
                                item.Intervencao = FuncoesDeConversao.ConverteParaString(reader["Intervencao"]);
                                item.DataSaida = FuncoesDeConversao.ConverteParaDateTime(reader["DataSaida"]);
                                item.DataChegada = FuncoesDeConversao.ConverteParaDateTime(reader["DataChegada"]);
                                item.DataRetorno = FuncoesDeConversao.ConverteParaDateTime(reader["DataRetorno"]);
                                item.DataEdicao = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicao"]);
                                item.Comentarios = FuncoesDeConversao.ConverteParaString(reader["Comentarios"]);
                                item.IdUsuarioEmUso = FuncoesDeConversao.ConverteParaInt(reader["IdUsuarioEmUso"]);

                                item.Filial.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFilial"]);
                                item.Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                item.Frota.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFrota"]);
                                item.UsuarioInsercao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdUsuarioInsercao"]);
                                item.UsuarioEdicao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdUsuarioEdicao"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatus"]);
                                item.Serie.Id = FuncoesDeConversao.ConverteParaInt(reader["IdSerie"]);
                                item.TipoOrdemServico.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoOrdemServico"]);
                                item.StatusEquipamentoAposManutencao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEquipamentoAposManutencao"]);
                                item.UsoIndevido.Id = FuncoesDeConversao.ConverteParaInt(reader["IdUsoIndevido"]);
                                item.TipoManutencao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoManutencao"]);
                                item.ExecutanteServico.Id = FuncoesDeConversao.ConverteParaInt(reader["IdExecutanteServico"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaOrdensServico.Add(item);

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

            int totalLinhasLista = listaOrdensServico.Count;
            int linhaAtualLista = 0;

            foreach (OrdemServico item in listaOrdensServico)
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Define as classes
                await item.Filial.GetFilialDatabaseAsync(item.Filial.Id, ct);
                await item.Cliente.GetClienteDatabaseAsync(item.Cliente.Id, ct);
                await item.Frota.GetFrotaDatabaseAsync(item.Frota.Id, ct);
                await item.UsuarioInsercao.GetUsuarioDatabaseAsync(item.UsuarioInsercao.Id, ct);
                await item.UsuarioEdicao.GetUsuarioDatabaseAsync(item.UsuarioEdicao.Id, ct);
                await item.Status.GetStatusDatabaseAsync(item.Status.Id, ct);
                await item.Serie.GetSerieDatabaseAsync(item.Serie.Id, ct);
                await item.TipoOrdemServico.GetTipoOrdemServicoDatabaseAsync(item.TipoOrdemServico.Id, ct);
                await item.StatusEquipamentoAposManutencao.GetStatusEquipamentoAposManutencaoDatabaseAsync(item.StatusEquipamentoAposManutencao.Id, ct);
                await item.UsoIndevido.GetUsoIndevidoDatabaseAsync(item.UsoIndevido.Id, ct);
                await item.TipoManutencao.GetTipoManutencaoDatabaseAsync(item.TipoManutencao.Id, ct);
                await item.ExecutanteServico.GetExecutanteServicoDatabaseAsync(item.ExecutanteServico.Id, ct);

                // Caso verdadeiro, a lista de itens da ordem de serviço será retornada
                if (retornaItens)
                {
                    await ItemOrdemServico.PreencheListaItensOrdemServicoAsync(item.ListaItensOrdemServico, true, false, null,
                        ct, "WHERE itos.id_ordem_servico = @id_ordem_servico ORDER BY itos.id_ordem_servico ASC", "@id_ordem_servico", item.Id);
                }

                // Caso verdadeiro, a lista de eventos da ordem de serviço será retornada
                if (retornaEventos)
                {
                    await EventoOrdemServico.PreencheListaEventosOrdemServicoAsync(item.ListaEventosOrdemServico, true, false, null,
                        ct, "WHERE evos.id_ordem_servico = @id_ordem_servico ORDER BY evos.id_ordem_servico ASC", "@id_ordem_servico", item.Id);
                }

                // Caso verdadeiro, a lista de eventos da ordem de serviço será retornada
                if (retornaInconsistencias)
                {
                    await InconsistenciaOrdemServico.PreencheListaInconsistenciasOrdemServicoAsync(item.ListaInconsistenciasOrdemServico, true, false, null,
                        ct, "WHERE inos.id_ordem_servico = @id_ordem_servico ORDER BY inos.id_ordem_servico ASC", "@id_ordem_servico", item.Id);
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

        /// <summary>
        /// Método assíncrono que verifica se uma ordem de serviço já existe na database através do id
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public static async Task<bool> OrdemServicoExiste(CancellationToken ct, int? idOrdemServico)
        {
            // Se a ordem de serviço for nula retorna falso
            if (idOrdemServico == null)
            {
                return false;
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

                // Utilização do comando
                using (MySqlConnector.MySqlCommand command = new("sp_ordem_servico_existe", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_ordem_servico", idOrdemServico);
                    command.Parameters.Add("p_existe", MySqlConnector.MySqlDbType.Bit, 255).Direction = System.Data.ParameterDirection.Output;

                    try
                    {
                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);
                    }
                    catch (Exception)
                    {
                    }

                    // Retorna o resultado da procedure
                    return Convert.ToBoolean(command.Parameters["p_existe"].Value);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que verifica se uma ordem de serviço já existe na database
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public static async Task<bool> OrdemServicoAtualExiste(CancellationToken ct, int? ordemServicoAtual)
        {
            // Se a ordem de serviço for nula retorna falso
            if (ordemServicoAtual == null)
            {
                return false;
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

                // Utilização do comando
                using (MySqlConnector.MySqlCommand command = new("sp_ordem_servico_atual_existe", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_ordem_servico_atual", ordemServicoAtual);
                    command.Parameters.Add("p_existe", MySqlConnector.MySqlDbType.Bit, 255).Direction = System.Data.ParameterDirection.Output;

                    try
                    {
                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);
                    }
                    catch (Exception)
                    {
                    }

                    // Retorna o resultado da procedure
                    return Convert.ToBoolean(command.Parameters["p_existe"].Value);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que verifica se uma ordem de serviço já existe na database
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public static async Task<bool> OrdemServicoPrimariaExiste(CancellationToken ct, int? ordemServicoPrimaria)
        {
            // Se a ordem de serviço for nula retorna falso
            if (ordemServicoPrimaria == null)
            {
                return false;
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

                // Utilização do comando
                using (MySqlConnector.MySqlCommand command = new("sp_ordem_servico_primaria_existe", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_ordem_servico_primaria", ordemServicoPrimaria);
                    command.Parameters.Add("p_existe", MySqlConnector.MySqlDbType.Bit, 255).Direction = System.Data.ParameterDirection.Output;

                    try
                    {
                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);
                    }
                    catch (Exception)
                    {
                    }

                    // Retorna o resultado da procedure
                    return Convert.ToBoolean(command.Parameters["p_existe"].Value);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna o id da ordem de serviço através do número da ordem de serviço
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public static async Task<int?> GetIdOrdemServicoPeloNumero(CancellationToken ct, int? ordemServico)
        {
            // Se a ordem de serviço for nula retorna nulo
            if (ordemServico == null)
            {
                return null;
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

                // Utilização do comando
                using (MySqlConnector.MySqlCommand command = new("sp_retorna_id_ordem_servico_pelo_numero", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_ordem_servico", ordemServico);
                    command.Parameters.Add("p_id", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                    try
                    {
                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);
                    }
                    catch (Exception)
                    {
                    }

                    // Retorna o resultado da procedure
                    return FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id"].Value);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que verifica se existem ordens de serviço incompleta
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public static async Task<bool> ExistemOrdensServicoIncompletas(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_existe_ordem_servico_incompleta", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("p_existe", MySqlConnector.MySqlDbType.Bit, 255).Direction = System.Data.ParameterDirection.Output;

                    try
                    {
                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);
                    }
                    catch (Exception)
                    {
                    }

                    // Retorna o resultado da procedure
                    return Convert.ToBoolean(command.Parameters["p_existe"].Value);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que atualiza o id do usuario em uso na database
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task AtualizaIdUsuarioEmUsoAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_atualiza_id_usuario_em_uso_ordem_servico", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_ordem_servico", Id);
                    command.Parameters.AddWithValue("p_id_usuario_em_uso", IdUsuarioEmUso);

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna o id do usuário em uso
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task GetIdUsuarioEmUsoAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_retorna_id_usuario_em_uso_ordem_servico", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_ordem_servico", Id);
                    command.Parameters.Add("p_id_usuario_em_uso", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);

                    // Retorna o id da série
                    IdUsuarioEmUso = FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_usuario_em_uso"].Value);
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
            OrdemServico ordemServicoCopia = new(true, true, true);

            ordemServicoCopia.Id = Id;
            ordemServicoCopia.OrdemServicoAtual = OrdemServicoAtual;
            ordemServicoCopia.OrdemServicoPrimaria = OrdemServicoPrimaria;
            ordemServicoCopia.NumeroChamado = NumeroChamado;
            ordemServicoCopia.DataChamado = DataChamado;
            ordemServicoCopia.DataInsercao = DataInsercao;
            ordemServicoCopia.DataAtendimento = DataAtendimento;
            ordemServicoCopia.Mastro = Mastro;
            ordemServicoCopia.CodigoFalha = CodigoFalha;
            ordemServicoCopia.EtapasConcluidas = EtapasConcluidas;
            ordemServicoCopia.Filial = (Filial)Filial.Clone();
            ordemServicoCopia.Cliente = (Cliente)Cliente.Clone();
            ordemServicoCopia.Frota = (Frota)Frota.Clone();
            ordemServicoCopia.UsuarioInsercao = (Usuario)UsuarioInsercao.Clone();
            ordemServicoCopia.UsuarioEdicao = (Usuario)UsuarioEdicao.Clone();
            ordemServicoCopia.Status = (Status)Status.Clone();
            ordemServicoCopia.Serie = (Serie)Serie.Clone();

            ordemServicoCopia.TipoOrdemServico = (TipoOrdemServico)TipoOrdemServico.Clone();
            ordemServicoCopia.StatusEquipamentoAposManutencao = (StatusEquipamentoAposManutencao)StatusEquipamentoAposManutencao.Clone();
            ordemServicoCopia.UsoIndevido = (UsoIndevido)UsoIndevido.Clone();
            ordemServicoCopia.TipoManutencao = (TipoManutencao)TipoManutencao.Clone();
            ordemServicoCopia.ExecutanteServico = (ExecutanteServico)ExecutanteServico.Clone();

            ordemServicoCopia.EquipamentoOperacional = EquipamentoOperacional;
            ordemServicoCopia.Horimetro = Horimetro;

            ordemServicoCopia.HorasPreventiva = HorasPreventiva;
            ordemServicoCopia.OutroTipoManutencao = OutroTipoManutencao;

            ordemServicoCopia.MotivoAtendimento = MotivoAtendimento;
            ordemServicoCopia.EntrevistaInicial = EntrevistaInicial;
            ordemServicoCopia.Intervencao = Intervencao;

            ordemServicoCopia.DataSaida = DataSaida;
            ordemServicoCopia.DataChegada = DataChegada;
            ordemServicoCopia.DataRetorno = DataRetorno;

            ordemServicoCopia.DataEdicao = DataEdicao;

            ordemServicoCopia.Comentarios = Comentarios;
            ordemServicoCopia.IdUsuarioEmUso = IdUsuarioEmUso;

            if (ListaItensOrdemServico != null)
            {
                foreach (var item in ListaItensOrdemServico)
                {
                    ordemServicoCopia.ListaItensOrdemServico.Add(item);
                }
            }

            if (ListaEventosOrdemServico != null)
            {
                foreach (var item in ListaEventosOrdemServico)
                {
                    ordemServicoCopia.ListaEventosOrdemServico.Add(item);
                }
            }

            if (ListaInconsistenciasOrdemServico != null)
            {
                foreach (var item in ListaInconsistenciasOrdemServico)
                {
                    ordemServicoCopia.ListaInconsistenciasOrdemServico.Add(item);
                }
            }

            return ordemServicoCopia;
        }

        #endregion Interfaces
    }
}