using System.Collections.ObjectModel;
using System.Diagnostics;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class Proposta : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private DateTime? _dataSolicitacao;
        private DateTime? _dataInsercao;
        private DateTime? _dataEnvioFaturamento;
        private DateTime? _dataFaturamento;
        private int? _notaFiscal;
        private decimal? _valorFaturamento;
        private string? _codigoProposta;
        private Filial? _filial;
        private Cliente? _cliente;
        private Contato? _contato;
        private Usuario? _usuarioInsercao;
        private Usuario? _usuarioEdicao;
        private Status? _status;
        private Prioridade? _prioridade;
        private Fabricante? _fabricante;
        private TipoEquipamento? _tipoEquipamento;
        private Modelo? _modelo;
        private Ano? _ano;
        private Serie? _serie;
        private decimal? _horimetro;
        private int? _ordemServico;
        private DateTime? _dataEnvio;
        private DateTime? _dataEdicao;
        private Proposta? _propostaOrigem;
        private Proposta? _ultimaProposta;
        private string? _textoPadrao;
        private string? _observacoes;
        private string? _prazoEntrega;
        private string? _condicaoPagamento;
        private string? _garantia;
        private string? _validade;
        private string? _comentarios;
        private int? _idUsuarioEmUso;
        private StatusAprovacao? _statusAprovacao;
        private JustificativaAprovacao? _justificativaAprovacao;
        private DateTime? _dataAprovacao;
        private ObservableCollection<ItemProposta> _listaItensProposta;

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

        public DateTime? DataSolicitacao
        {
            get { return _dataSolicitacao; }
            set
            {
                if (value != _dataSolicitacao)
                {
                    _dataSolicitacao = value;
                    OnPropertyChanged(nameof(DataSolicitacao));
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

        public DateTime? DataEnvioFaturamento
        {
            get { return _dataEnvioFaturamento; }
            set
            {
                if (value != _dataEnvioFaturamento)
                {
                    _dataEnvioFaturamento = value;
                    OnPropertyChanged(nameof(DataEnvioFaturamento));
                }
            }
        }

        public DateTime? DataFaturamento
        {
            get { return _dataFaturamento; }
            set
            {
                if (value != _dataFaturamento)
                {
                    _dataFaturamento = value;
                    OnPropertyChanged(nameof(DataFaturamento));
                }
            }
        }

        public int? NotaFiscal
        {
            get { return _notaFiscal; }
            set
            {
                if (value != _notaFiscal)
                {
                    _notaFiscal = value;
                    OnPropertyChanged(nameof(NotaFiscal));
                }
            }
        }

        public decimal? ValorFaturamento
        {
            get { return _valorFaturamento; }
            set
            {
                if (value != _valorFaturamento)
                {
                    _valorFaturamento = value;
                    OnPropertyChanged(nameof(ValorFaturamento));
                }
            }
        }

        public string? CodigoProposta
        {
            get { return _codigoProposta; }
            set
            {
                if (value != _codigoProposta)
                {
                    _codigoProposta = value;
                    OnPropertyChanged(nameof(CodigoProposta));
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

        public Contato? Contato
        {
            get { return _contato; }
            set
            {
                if (value != _contato)
                {
                    _contato = value;
                    OnPropertyChanged(nameof(Contato));
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

        public Prioridade? Prioridade
        {
            get { return _prioridade; }
            set
            {
                if (value != _prioridade)
                {
                    _prioridade = value;
                    OnPropertyChanged(nameof(Prioridade));
                }
            }
        }

        public Fabricante? Fabricante
        {
            get { return _fabricante; }
            set
            {
                if (value != _fabricante)
                {
                    _fabricante = value;
                    OnPropertyChanged(nameof(Fabricante));
                }
            }
        }

        public TipoEquipamento? TipoEquipamento
        {
            get { return _tipoEquipamento; }
            set
            {
                if (value != _tipoEquipamento)
                {
                    _tipoEquipamento = value;
                    OnPropertyChanged(nameof(TipoEquipamento));
                }
            }
        }

        public Modelo? Modelo
        {
            get { return _modelo; }
            set
            {
                if (value != _modelo)
                {
                    _modelo = value;
                    OnPropertyChanged(nameof(Modelo));
                }
            }
        }

        public Ano? Ano
        {
            get { return _ano; }
            set
            {
                if (value != _ano)
                {
                    _ano = value;
                    OnPropertyChanged(nameof(Ano));
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

        public int? OrdemServico
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

        public DateTime? DataEnvio
        {
            get { return _dataEnvio; }
            set
            {
                if (value != _dataEnvio)
                {
                    _dataEnvio = value;
                    OnPropertyChanged(nameof(DataEnvio));
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

        public Proposta? PropostaOrigem
        {
            get { return _propostaOrigem; }
            set
            {
                if (value != _propostaOrigem)
                {
                    _propostaOrigem = value;
                    OnPropertyChanged(nameof(PropostaOrigem));
                }
            }
        }

        public Proposta? UltimaProposta
        {
            get { return _ultimaProposta; }
            set
            {
                if (value != _ultimaProposta)
                {
                    _ultimaProposta = value;
                    OnPropertyChanged(nameof(UltimaProposta));
                }
            }
        }

        public string? TextoPadrao
        {
            get { return _textoPadrao; }
            set
            {
                if (value != _textoPadrao)
                {
                    _textoPadrao = value;
                    OnPropertyChanged(nameof(TextoPadrao));
                }
            }
        }

        public string? Observacoes
        {
            get { return _observacoes; }
            set
            {
                if (value != _observacoes)
                {
                    _observacoes = value;
                    OnPropertyChanged(nameof(Observacoes));
                }
            }
        }

        public string? PrazoEntrega
        {
            get { return _prazoEntrega; }
            set
            {
                if (value != _prazoEntrega)
                {
                    _prazoEntrega = value;
                    OnPropertyChanged(nameof(PrazoEntrega));
                }
            }
        }

        public string? CondicaoPagamento
        {
            get { return _condicaoPagamento; }
            set
            {
                if (value != _condicaoPagamento)
                {
                    _condicaoPagamento = value;
                    OnPropertyChanged(nameof(CondicaoPagamento));
                }
            }
        }

        public string? Garantia
        {
            get { return _garantia; }
            set
            {
                if (value != _garantia)
                {
                    _garantia = value;
                    OnPropertyChanged(nameof(Garantia));
                }
            }
        }

        public string? Validade
        {
            get { return _validade; }
            set
            {
                if (value != _validade)
                {
                    _validade = value;
                    OnPropertyChanged(nameof(Validade));
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

        public StatusAprovacao? StatusAprovacao
        {
            get { return _statusAprovacao; }
            set
            {
                if (value != _statusAprovacao)
                {
                    _statusAprovacao = value;
                    OnPropertyChanged(nameof(StatusAprovacao));
                }
            }
        }

        public JustificativaAprovacao? JustificativaAprovacao
        {
            get { return _justificativaAprovacao; }
            set
            {
                if (value != _justificativaAprovacao)
                {
                    _justificativaAprovacao = value;
                    OnPropertyChanged(nameof(JustificativaAprovacao));
                }
            }
        }

        public DateTime? DataAprovacao
        {
            get { return _dataAprovacao; }
            set
            {
                if (value != _dataAprovacao)
                {
                    _dataAprovacao = value;
                    OnPropertyChanged(nameof(DataAprovacao));
                }
            }
        }

        public ObservableCollection<ItemProposta> ListaItensProposta
        {
            get { return _listaItensProposta; }
            set
            {
                if (value != _listaItensProposta)
                {
                    _listaItensProposta = value;
                    OnPropertyChanged(nameof(ListaItensProposta));
                }
            }
        }

        #endregion Propriedades

        #region Construtores

        /// <summary>
        /// Construtor da proposta com os parâmetros utilizados
        /// </summary>
        /// <param name="inicializaItensProposta">Indica se a classe deve ser inicializada. Deve-se ter cuidado e levar em consideração loops infinitos</param>
        /// <param name="inicializaPropostaOrigem">Indica se a classe deve ser inicializada. Deve-se ter cuidado e levar em consideração loops infinitos</param>
        /// <param name="inicializaUltimaProposta">Indica se a classe deve ser inicializada. Deve-se ter cuidado e levar em consideração loops infinitos</param>
        public Proposta(bool inicializaItensProposta = false, bool inicializaPropostaOrigem = false, bool inicializaUltimaProposta = false)
        {
            Filial = new();
            Cliente = new();
            Contato = new();
            UsuarioInsercao = new();
            UsuarioEdicao = new();
            Status = new();
            Prioridade = new();
            Fabricante = new();
            TipoEquipamento = new();
            Modelo = new();
            Ano = new();
            Serie = new();
            StatusAprovacao = new();
            JustificativaAprovacao = new();

            if (inicializaPropostaOrigem)
            {
                PropostaOrigem = new();
            }

            if (inicializaUltimaProposta)
            {
                UltimaProposta = new();
            }

            if (inicializaItensProposta)
            {
                ListaItensProposta = new();
            }
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados da proposta com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id do setor que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="retornaItens">Representa a opção de preencher ou não a lista de itens dessa proposta</param>
        /// <param name="getPropostaOrigem">Representa a opção de retornar ou não a proposta de origem</param>
        /// <param name="getUltimaProposta">Representa a opção de retornar ou não a última proposta</param>
        public async Task GetPropostaDatabaseAsync(int? id, CancellationToken ct, bool retornaItens = false, bool getPropostaOrigem = false, bool getUltimaProposta = false)
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
                                          + "prop.id_proposta AS Id, "
                                          + "prop.data_solicitacao AS DataSolicitacao, "
                                          + "prop.data_insercao AS DataInsercao, "
                                          + "prop.data_envio_faturamento AS DataEnvioFaturamento, "
                                          + "prop.data_faturamento AS DataFaturamento, "
                                          + "prop.nota_fiscal AS NotaFiscal, "
                                          + "prop.valor_faturamento AS ValorFaturamento, "
                                          + "prop.codigo_proposta AS CodigoProposta, "
                                          + "prop.horimetro AS Horimetro, "
                                          + "prop.ordem_servico AS OrdemServico, "
                                          + "prop.data_envio AS DataEnvio, "
                                          + "prop.data_edicao AS DataEdicao, "
                                          + "prop.texto_padrao AS TextoPadrao, "
                                          + "prop.observacoes AS Observacoes, "
                                          + "prop.prazo_entrega AS PrazoEntrega, "
                                          + "prop.condicao_pagamento AS CondicaoPagamento, "
                                          + "prop.garantia AS Garantia, "
                                          + "prop.validade AS Validade, "
                                          + "prop.comentarios AS Comentarios, "
                                          + "prop.data_aprovacao AS DataAprovacao, "
                                          + "prop.id_filial AS idFilial, "
                                          + "prop.id_cliente AS idCliente, "
                                          + "prop.id_contato AS idContato, "
                                          + "prop.id_usuario_insercao AS idUsuarioInsercao, "
                                          + "prop.id_usuario_edicao AS idUsuarioEdicao, "
                                          + "prop.id_status AS idStatus, "
                                          + "prop.id_prioridade AS idPrioridade, "
                                          + "prop.id_fabricante AS idFabricante, "
                                          + "prop.id_tipo_equipamento AS idTipoEquipamento, "
                                          + "prop.id_modelo AS idModelo, "
                                          + "prop.id_ano AS idAno, "
                                          + "prop.id_serie AS idSerie, "
                                          + "prop.id_status_aprovacao AS idStatusAprovacao, "
                                          + "prop.id_justificativa_aprovacao AS idJustificativaAprovacao, "
                                          + "prop.id_proposta_origem AS idPropostaOrigem, "
                                          + "prop.id_ultima_proposta AS idUltimaProposta "
                                          + "FROM tb_propostas AS prop "
                                          + "WHERE prop.id_proposta = @id";

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
                                if (Contato == null)
                                {
                                    Contato = new();
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
                                if (Prioridade == null)
                                {
                                    Prioridade = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Fabricante == null)
                                {
                                    Fabricante = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoEquipamento == null)
                                {
                                    TipoEquipamento = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo == null)
                                {
                                    Modelo = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Ano == null)
                                {
                                    Ano = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Serie == null)
                                {
                                    Serie = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (StatusAprovacao == null)
                                {
                                    StatusAprovacao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (JustificativaAprovacao == null)
                                {
                                    JustificativaAprovacao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (getPropostaOrigem == true && PropostaOrigem == null)
                                {
                                    PropostaOrigem = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (getUltimaProposta == true && UltimaProposta == null)
                                {
                                    UltimaProposta = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaItens == true && ListaItensProposta == null)
                                {
                                    ListaItensProposta = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                DataSolicitacao = FuncoesDeConversao.ConverteParaDateTime(reader["DataSolicitacao"]);
                                DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                DataEnvioFaturamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataEnvioFaturamento"]);
                                DataFaturamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataFaturamento"]);
                                NotaFiscal = FuncoesDeConversao.ConverteParaInt(reader["NotaFiscal"]);
                                ValorFaturamento = FuncoesDeConversao.ConverteParaDecimal(reader["ValorFaturamento"]);
                                CodigoProposta = FuncoesDeConversao.ConverteParaString(reader["CodigoProposta"]);
                                Horimetro = FuncoesDeConversao.ConverteParaDecimal(reader["Horimetro"]);
                                OrdemServico = FuncoesDeConversao.ConverteParaInt(reader["OrdemServico"]);
                                DataEnvio = FuncoesDeConversao.ConverteParaDateTime(reader["DataEnvio"]);
                                DataEdicao = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicao"]);
                                TextoPadrao = FuncoesDeConversao.ConverteParaString(reader["TextoPadrao"]);
                                Observacoes = FuncoesDeConversao.ConverteParaString(reader["Observacoes"]);
                                PrazoEntrega = FuncoesDeConversao.ConverteParaString(reader["PrazoEntrega"]);
                                CondicaoPagamento = FuncoesDeConversao.ConverteParaString(reader["CondicaoPagamento"]);
                                Garantia = FuncoesDeConversao.ConverteParaString(reader["Garantia"]);
                                Validade = FuncoesDeConversao.ConverteParaString(reader["Validade"]);
                                Comentarios = FuncoesDeConversao.ConverteParaString(reader["Comentarios"]);
                                DataAprovacao = FuncoesDeConversao.ConverteParaDateTime(reader["DataAprovacao"]);

                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);
                                Filial.Id = FuncoesDeConversao.ConverteParaInt(reader["idFilial"]);
                                Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["idCliente"]);
                                Contato.Id = FuncoesDeConversao.ConverteParaInt(reader["idContato"]);
                                UsuarioInsercao.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuarioInsercao"]);
                                UsuarioEdicao.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuarioEdicao"]);
                                Prioridade.Id = FuncoesDeConversao.ConverteParaInt(reader["idPrioridade"]);
                                Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["idFabricante"]);
                                TipoEquipamento.Id = FuncoesDeConversao.ConverteParaInt(reader["idTipoEquipamento"]);
                                Modelo.Id = FuncoesDeConversao.ConverteParaInt(reader["idModelo"]);
                                Ano.Id = FuncoesDeConversao.ConverteParaInt(reader["idAno"]);
                                Serie.Id = FuncoesDeConversao.ConverteParaInt(reader["idSerie"]);
                                StatusAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatusAprovacao"]);
                                JustificativaAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idJustificativaAprovacao"]);

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (getPropostaOrigem == true)
                                {
                                    PropostaOrigem.Id = FuncoesDeConversao.ConverteParaInt(reader["idPropostaOrigem"]);
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (getUltimaProposta == true)
                                {
                                    UltimaProposta.Id = FuncoesDeConversao.ConverteParaInt(reader["idUltimaProposta"]);
                                }
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
            await Contato.GetContatoDatabaseAsync(Contato.Id, ct);
            await UsuarioInsercao.GetUsuarioDatabaseAsync(UsuarioInsercao.Id, ct);
            await UsuarioEdicao.GetUsuarioDatabaseAsync(UsuarioEdicao.Id, ct);
            await Status.GetStatusDatabaseAsync(Status.Id, ct);
            await Prioridade.GetPrioridadeDatabaseAsync(Prioridade.Id, ct);
            await Fabricante.GetFabricanteDatabaseAsync(Fabricante.Id, ct);
            await TipoEquipamento.GetTipoEquipamentoDatabaseAsync(TipoEquipamento.Id, ct);
            await Modelo.GetModeloDatabaseAsync(Modelo.Id, ct);
            await Ano.GetAnoDatabaseAsync(Ano.Id, ct);
            await Serie.GetSerieDatabaseAsync(Serie.Id, ct);
            await StatusAprovacao.GetStatusAprovacaoDatabaseAsync(StatusAprovacao.Id, ct);
            await JustificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(JustificativaAprovacao.Id, ct);

            // Caso verdadeiro, a classe PropostaOrigem será retornada. Classes subsequentes não serão retornadas para evitar estouro de carga
            if (getPropostaOrigem)
            {
                await PropostaOrigem.GetPropostaDatabaseAsync(PropostaOrigem.Id, ct);
            }

            // Caso verdadeiro, a classe UltimaProposta será retornada. Classes subsequentes não serão retornadas para evitar estouro de carga
            if (getUltimaProposta)
            {
                await UltimaProposta.GetPropostaDatabaseAsync(UltimaProposta.Id, ct);
            }

            // Caso verdadeiro, a lista de itens da proposta será retornada
            if (retornaItens)
            {
                await ItemProposta.PreencheListaItensPropostaAsync(ListaItensProposta, true, false, null,
                    ct, "WHERE itpr.id_proposta = @id_proposta ORDER BY itpr.id_item_proposta ASC", "@id_proposta", Id);

                foreach (var item in ListaItensProposta)
                {
                    await item.Status.GetStatusDatabaseAsync(item.Status.Id, ct);
                    await item.StatusAprovacao.GetStatusAprovacaoDatabaseAsync(item.StatusAprovacao.Id, ct);
                    await item.JustificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(item.JustificativaAprovacao.Id, ct);
                    await item.TipoItem.GetTipoItemDatabaseAsync(item.TipoItem.Id, ct);
                    await item.Fornecedor.GetFornecedorDatabaseAsync(item.Fornecedor.Id, ct);
                    await item.TipoSubstituicaoTributaria.GetTipoSubstituicaoTributariaDatabaseAsync(item.TipoSubstituicaoTributaria.Id, ct);
                    await item.Origem.GetOrigemDatabaseAsync(item.Origem.Id, ct);
                    await item.Conjunto.GetConjuntoDatabaseAsync(item.Conjunto.Id, ct);
                    await item.Especificacao.GetEspecificacaoDatabaseAsync(item.Especificacao.Id, ct);
                    await item.Usuario.GetUsuarioDatabaseAsync(item.Usuario.Id, ct);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados da proposta com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="retornaItens">Representa a opção de preencher ou não a lista de itens dessa proposta</param>
        /// <param name="getPropostaOrigem">Representa a opção de retornar ou não a proposta de origem</param>
        /// <param name="getUltimaProposta">Representa a opção de retornar ou não a última proposta</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetPropostaDatabaseAsync(CancellationToken ct, bool retornaItens, bool getPropostaOrigem, bool getUltimaProposta, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                      + "prop.id_proposta AS Id, "
                      + "prop.data_solicitacao AS DataSolicitacao, "
                      + "prop.data_insercao AS DataInsercao, "
                      + "prop.data_envio_faturamento AS DataEnvioFaturamento, "
                      + "prop.data_faturamento AS DataFaturamento, "
                      + "prop.nota_fiscal AS NotaFiscal, "
                      + "prop.valor_faturamento AS ValorFaturamento, "
                      + "prop.codigo_proposta AS CodigoProposta, "
                      + "prop.horimetro AS Horimetro, "
                      + "prop.ordem_servico AS OrdemServico, "
                      + "prop.data_envio AS DataEnvio, "
                      + "prop.data_edicao AS DataEdicao, "
                      + "prop.texto_padrao AS TextoPadrao, "
                      + "prop.observacoes AS Observacoes, "
                      + "prop.prazo_entrega AS PrazoEntrega, "
                      + "prop.condicao_pagamento AS CondicaoPagamento, "
                      + "prop.garantia AS Garantia, "
                      + "prop.validade AS Validade, "
                      + "prop.comentarios AS Comentarios, "
                      + "prop.data_aprovacao AS DataAprovacao, "
                      + "prop.id_filial AS idFilial, "
                      + "prop.id_cliente AS idCliente, "
                      + "prop.id_contato AS idContato, "
                      + "prop.id_usuario_insercao AS idUsuarioInsercao, "
                      + "prop.id_usuario_edicao AS idUsuarioEdicao, "
                      + "prop.id_status AS idStatus, "
                      + "prop.id_prioridade AS idPrioridade, "
                      + "prop.id_fabricante AS idFabricante, "
                      + "prop.id_tipo_equipamento AS idTipoEquipamento, "
                      + "prop.id_modelo AS idModelo, "
                      + "prop.id_ano AS idAno, "
                      + "prop.id_serie AS idSerie, "
                      + "prop.id_status_aprovacao AS idStatusAprovacao, "
                      + "prop.id_justificativa_aprovacao AS idJustificativaAprovacao, "
                      + "prop.id_proposta_origem AS idPropostaOrigem, "
                      + "prop.id_ultima_proposta AS idUltimaProposta "
                      + "FROM tb_propostas AS prop "
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
                                if (Contato == null)
                                {
                                    Contato = new();
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
                                if (Prioridade == null)
                                {
                                    Prioridade = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Fabricante == null)
                                {
                                    Fabricante = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoEquipamento == null)
                                {
                                    TipoEquipamento = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo == null)
                                {
                                    Modelo = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Ano == null)
                                {
                                    Ano = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (Serie == null)
                                {
                                    Serie = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (StatusAprovacao == null)
                                {
                                    StatusAprovacao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (JustificativaAprovacao == null)
                                {
                                    JustificativaAprovacao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (getPropostaOrigem == true && PropostaOrigem == null)
                                {
                                    PropostaOrigem = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (getUltimaProposta == true && UltimaProposta == null)
                                {
                                    UltimaProposta = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaItens == true && ListaItensProposta == null)
                                {
                                    ListaItensProposta = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                DataSolicitacao = FuncoesDeConversao.ConverteParaDateTime(reader["DataSolicitacao"]);
                                DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                DataEnvioFaturamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataEnvioFaturamento"]);
                                DataFaturamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataFaturamento"]);
                                NotaFiscal = FuncoesDeConversao.ConverteParaInt(reader["NotaFiscal"]);
                                ValorFaturamento = FuncoesDeConversao.ConverteParaDecimal(reader["ValorFaturamento"]);
                                CodigoProposta = FuncoesDeConversao.ConverteParaString(reader["CodigoProposta"]);
                                Horimetro = FuncoesDeConversao.ConverteParaDecimal(reader["Horimetro"]);
                                OrdemServico = FuncoesDeConversao.ConverteParaInt(reader["OrdemServico"]);
                                DataEnvio = FuncoesDeConversao.ConverteParaDateTime(reader["DataEnvio"]);
                                DataEdicao = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicao"]);
                                TextoPadrao = FuncoesDeConversao.ConverteParaString(reader["TextoPadrao"]);
                                Observacoes = FuncoesDeConversao.ConverteParaString(reader["Observacoes"]);
                                PrazoEntrega = FuncoesDeConversao.ConverteParaString(reader["PrazoEntrega"]);
                                CondicaoPagamento = FuncoesDeConversao.ConverteParaString(reader["CondicaoPagamento"]);
                                Garantia = FuncoesDeConversao.ConverteParaString(reader["Garantia"]);
                                Validade = FuncoesDeConversao.ConverteParaString(reader["Validade"]);
                                Comentarios = FuncoesDeConversao.ConverteParaString(reader["Comentarios"]);
                                DataAprovacao = FuncoesDeConversao.ConverteParaDateTime(reader["DataAprovacao"]);

                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);
                                Filial.Id = FuncoesDeConversao.ConverteParaInt(reader["idFilial"]);
                                Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["idCliente"]);
                                Contato.Id = FuncoesDeConversao.ConverteParaInt(reader["idContato"]);
                                UsuarioInsercao.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuarioInsercao"]);
                                UsuarioEdicao.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuarioEdicao"]);
                                Prioridade.Id = FuncoesDeConversao.ConverteParaInt(reader["idPrioridade"]);
                                Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["idFabricante"]);
                                TipoEquipamento.Id = FuncoesDeConversao.ConverteParaInt(reader["idTipoEquipamento"]);
                                Modelo.Id = FuncoesDeConversao.ConverteParaInt(reader["idModelo"]);
                                Ano.Id = FuncoesDeConversao.ConverteParaInt(reader["idAno"]);
                                Serie.Id = FuncoesDeConversao.ConverteParaInt(reader["idSerie"]);
                                StatusAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatusAprovacao"]);
                                JustificativaAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idJustificativaAprovacao"]);

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (getPropostaOrigem == true)
                                {
                                    PropostaOrigem.Id = FuncoesDeConversao.ConverteParaInt(reader["idPropostaOrigem"]);
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (getUltimaProposta == true)
                                {
                                    UltimaProposta.Id = FuncoesDeConversao.ConverteParaInt(reader["idUltimaProposta"]);
                                }
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
            await Contato.GetContatoDatabaseAsync(Contato.Id, ct);
            await UsuarioInsercao.GetUsuarioDatabaseAsync(UsuarioInsercao.Id, ct);
            await UsuarioEdicao.GetUsuarioDatabaseAsync(UsuarioEdicao.Id, ct);
            await Status.GetStatusDatabaseAsync(Status.Id, ct);
            await Prioridade.GetPrioridadeDatabaseAsync(Prioridade.Id, ct);
            await Fabricante.GetFabricanteDatabaseAsync(Fabricante.Id, ct);
            await TipoEquipamento.GetTipoEquipamentoDatabaseAsync(TipoEquipamento.Id, ct);
            await Modelo.GetModeloDatabaseAsync(Modelo.Id, ct);
            await Ano.GetAnoDatabaseAsync(Ano.Id, ct);
            await Serie.GetSerieDatabaseAsync(Serie.Id, ct);
            await StatusAprovacao.GetStatusAprovacaoDatabaseAsync(StatusAprovacao.Id, ct);
            await JustificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(JustificativaAprovacao.Id, ct);

            // Caso verdadeiro, a classe PropostaOrigem será retornada. Classes subsequentes não serão retornadas para evitar estouro de carga
            if (getPropostaOrigem)
            {
                await PropostaOrigem.GetPropostaDatabaseAsync(PropostaOrigem.Id, ct);
            }

            // Caso verdadeiro, a classe UltimaProposta será retornada. Classes subsequentes não serão retornadas para evitar estouro de carga
            if (getUltimaProposta)
            {
                await UltimaProposta.GetPropostaDatabaseAsync(UltimaProposta.Id, ct);
            }

            // Caso verdadeiro, a lista de itens da proposta será retornada
            if (retornaItens)
            {
                await ItemProposta.PreencheListaItensPropostaAsync(ListaItensProposta, true, false, null,
                    ct, "WHERE itpr.id_proposta = @id_proposta", "@id_proposta", Id);

                foreach (var item in ListaItensProposta)
                {
                    await item.Status.GetStatusDatabaseAsync(item.Status.Id, ct);
                    await item.StatusAprovacao.GetStatusAprovacaoDatabaseAsync(item.StatusAprovacao.Id, ct);
                    await item.JustificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(item.JustificativaAprovacao.Id, ct);
                    await item.TipoItem.GetTipoItemDatabaseAsync(item.TipoItem.Id, ct);
                    await item.Fornecedor.GetFornecedorDatabaseAsync(item.Fornecedor.Id, ct);
                    await item.TipoSubstituicaoTributaria.GetTipoSubstituicaoTributariaDatabaseAsync(item.TipoSubstituicaoTributaria.Id, ct);
                    await item.Origem.GetOrigemDatabaseAsync(item.Origem.Id, ct);
                    await item.Conjunto.GetConjuntoDatabaseAsync(item.Conjunto.Id, ct);
                    await item.Especificacao.GetEspecificacaoDatabaseAsync(item.Especificacao.Id, ct);
                    await item.Usuario.GetUsuarioDatabaseAsync(item.Usuario.Id, ct);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna a contagem de propostas iniciais
        /// </summary>
        /// <param name="dataInsercao">Data a ser considerada para a contagem</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <returns>Número inteiro representando a contagem de propostas no mesmo dia</returns>
        public async Task<int> GetContagemPropostasIniciaisAsync(DateTime dataInsercao, CancellationToken ct)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

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

                try
                {
                    // Lança exceção de cancelamento caso ela tenha sido efetuada
                    ct.ThrowIfCancellationRequested();

                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_contagem_propostas_iniciais", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_cliente", Cliente.Id);
                        command.Parameters.AddWithValue("p_data_insercao", dataInsercao);
                        command.Parameters.Add("p_contagem", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna o valor
                        return Convert.ToInt32(command.Parameters["p_contagem"].Value);
                    }
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna o código da proposta revisada
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <returns>String contendo o código da proposta revisada</returns>
        public async Task<string> GetCodigoPropostaRevisadaAsync(CancellationToken ct)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            string codigoPropostaSemRev = CodigoProposta.Split("_").First();
            string codigoPropostaComRev = codigoPropostaSemRev + "_REV";
            string codigoRetornado = codigoPropostaSemRev;

            try
            {
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
                                              + "codigo_proposta AS CodigoProposta "
                                              + "FROM tb_propostas "
                                              + "WHERE (id_cliente = @id_cliente AND codigo_proposta = @codigo_proposta_sem_rev) OR "
                                              + "(id_cliente = @id_cliente AND codigo_proposta LIKE @codigo_proposta_com_rev) "
                                              + "ORDER BY id_proposta DESC LIMIT 1";
                        command.Parameters.AddWithValue("@id_cliente", Cliente.Id);
                        command.Parameters.AddWithValue("@codigo_proposta_sem_rev", codigoPropostaSemRev);
                        command.Parameters.AddWithValue("@codigo_proposta_com_rev", codigoPropostaComRev + "%");

                        // Utilização do reader para retornar os dados asíncronos
                        using (var reader = await command.ExecuteReaderAsync(ct))
                        {
                            // Verifica se o reader possui linhas
                            if (reader.HasRows)
                            {
                                // Enquanto o reader possuir linhas, define os valores
                                while (await reader.ReadAsync(ct))
                                {
                                    codigoRetornado = FuncoesDeConversao.ConverteParaString(reader["CodigoProposta"]).ToString();
                                }
                            }
                        }
                    }
                }

                if (codigoRetornado.Contains("REV"))
                {
                    return codigoPropostaSemRev + "_REV" + Convert.ToString(Convert.ToInt32(codigoRetornado.Split("_REV").Last()) + 1);
                }
                else
                {
                    return codigoPropostaSemRev + "_REV1";
                }
            }
            catch (Exception)
            {
                throw new Exception("Erro ao retornar o código da proposta");
            }
        }

        /// <summary>
        /// Método assíncrono que atualiza o id da última proposta em caso de revisão
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public async Task SetUltimaPropostaAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_atualiza_ultima_proposta", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_proposta", PropostaOrigem.Id);
                    command.Parameters.AddWithValue("p_id_ultima_proposta", Id);
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    try
                    {
                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);
                    }
                    catch (Exception)
                    {
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("A proposta " + CodigoProposta + " não existe para o cliente " + Cliente.Nome);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva a proposta na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarPropostaDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_proposta", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_data_solicitacao", DataSolicitacao);
                        command.Parameters.AddWithValue("p_data_insercao", DateTime.Now);
                        command.Parameters.AddWithValue("p_data_envio_faturamento", DataEnvioFaturamento);
                        command.Parameters.AddWithValue("p_data_faturamento", DataFaturamento);
                        command.Parameters.AddWithValue("p_nota_fiscal", NotaFiscal);
                        command.Parameters.AddWithValue("p_valor_faturamento", ValorFaturamento);
                        command.Parameters.AddWithValue("p_id_filial", Filial == null ? null : Filial.Id);
                        command.Parameters.AddWithValue("p_id_cliente", Cliente == null ? null : Cliente.Id);
                        command.Parameters.AddWithValue("p_id_contato", Contato == null ? null : Contato.Id);
                        command.Parameters.AddWithValue("p_codigo_proposta", CodigoProposta);
                        command.Parameters.AddWithValue("p_id_usuario_insercao", UsuarioInsercao.Id);
                        command.Parameters.AddWithValue("p_id_status", Status == null ? null : Status.Id);
                        command.Parameters.AddWithValue("p_id_prioridade", Prioridade == null ? null : Prioridade.Id);
                        command.Parameters.AddWithValue("p_id_fabricante", Fabricante == null ? null : Fabricante.Id);
                        command.Parameters.AddWithValue("p_id_tipo_equipamento", TipoEquipamento == null ? null : TipoEquipamento.Id);
                        command.Parameters.AddWithValue("p_id_modelo", Modelo == null ? null : Modelo.Id);
                        command.Parameters.AddWithValue("p_id_ano", Ano == null ? null : Ano.Id);
                        command.Parameters.AddWithValue("p_id_serie", Serie == null ? null : Serie.Id);
                        command.Parameters.AddWithValue("p_id_status_aprovacao", StatusAprovacao == null ? null : StatusAprovacao.Id);
                        command.Parameters.AddWithValue("p_id_justificativa_aprovacao", JustificativaAprovacao == null ? null : JustificativaAprovacao.Id);
                        command.Parameters.AddWithValue("p_horimetro", Horimetro);
                        command.Parameters.AddWithValue("p_ordem_servico", OrdemServico);
                        command.Parameters.AddWithValue("p_data_envio", DataEnvio);
                        command.Parameters.AddWithValue("p_id_proposta_origem", PropostaOrigem == null ? null : PropostaOrigem.Id);
                        command.Parameters.AddWithValue("p_id_ultima_proposta", UltimaProposta == null ? null : UltimaProposta.Id);
                        command.Parameters.AddWithValue("p_texto_padrao", TextoPadrao);
                        command.Parameters.AddWithValue("p_observacoes", Observacoes);
                        command.Parameters.AddWithValue("p_prazo_entrega", PrazoEntrega);
                        command.Parameters.AddWithValue("p_condicao_pagamento", CondicaoPagamento);
                        command.Parameters.AddWithValue("p_garantia", Garantia);
                        command.Parameters.AddWithValue("p_validade", Validade);
                        command.Parameters.AddWithValue("p_comentarios", Comentarios);
                        command.Parameters.AddWithValue("p_data_aprovacao", DataAprovacao);
                        command.Parameters.AddWithValue("p_id_usuario_em_uso", IdUsuarioEmUso);
                        command.Parameters.Add("p_id_proposta", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException("A proposta " + CodigoProposta + " já existe para o cliente " + Cliente.Nome);
                        }
                        else
                        {
                            Id = FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_proposta"].Value);
                        }
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_proposta", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_proposta", Id);
                        command.Parameters.AddWithValue("p_data_solicitacao", DataSolicitacao);
                        command.Parameters.AddWithValue("p_data_edicao", DataEdicao);
                        command.Parameters.AddWithValue("p_data_envio_faturamento", DataEnvioFaturamento);
                        command.Parameters.AddWithValue("p_data_faturamento", DataFaturamento);
                        command.Parameters.AddWithValue("p_nota_fiscal", NotaFiscal);
                        command.Parameters.AddWithValue("p_valor_faturamento", ValorFaturamento);
                        command.Parameters.AddWithValue("p_id_filial", Filial == null ? null : Filial.Id);
                        command.Parameters.AddWithValue("p_id_cliente", Cliente == null ? null : Cliente.Id);
                        command.Parameters.AddWithValue("p_id_contato", Contato == null ? null : Contato.Id);
                        command.Parameters.AddWithValue("p_codigo_proposta", CodigoProposta);
                        command.Parameters.AddWithValue("p_id_usuario_edicao", UsuarioEdicao == null ? null : UsuarioEdicao.Id);
                        command.Parameters.AddWithValue("p_id_status", Status == null ? null : Status.Id);
                        command.Parameters.AddWithValue("p_id_prioridade", Prioridade == null ? null : Prioridade.Id);
                        command.Parameters.AddWithValue("p_id_fabricante", Fabricante == null ? null : Fabricante.Id);
                        command.Parameters.AddWithValue("p_id_tipo_equipamento", TipoEquipamento == null ? null : TipoEquipamento.Id);
                        command.Parameters.AddWithValue("p_id_modelo", Modelo == null ? null : Modelo.Id);
                        command.Parameters.AddWithValue("p_id_ano", Ano == null ? null : Ano.Id);
                        command.Parameters.AddWithValue("p_id_serie", Serie == null ? null : Serie.Id);
                        command.Parameters.AddWithValue("p_id_status_aprovacao", StatusAprovacao == null ? null : StatusAprovacao.Id);
                        command.Parameters.AddWithValue("p_id_justificativa_aprovacao", JustificativaAprovacao == null ? null : JustificativaAprovacao.Id);
                        command.Parameters.AddWithValue("p_horimetro", Horimetro);
                        command.Parameters.AddWithValue("p_ordem_servico", OrdemServico);
                        command.Parameters.AddWithValue("p_data_envio", DataEnvio);
                        command.Parameters.AddWithValue("p_id_proposta_origem", PropostaOrigem == null ? null : PropostaOrigem.Id);
                        command.Parameters.AddWithValue("p_id_ultima_proposta", UltimaProposta == null ? null : UltimaProposta.Id);
                        command.Parameters.AddWithValue("p_texto_padrao", TextoPadrao);
                        command.Parameters.AddWithValue("p_observacoes", Observacoes);
                        command.Parameters.AddWithValue("p_prazo_entrega", PrazoEntrega);
                        command.Parameters.AddWithValue("p_condicao_pagamento", CondicaoPagamento);
                        command.Parameters.AddWithValue("p_garantia", Garantia);
                        command.Parameters.AddWithValue("p_validade", Validade);
                        command.Parameters.AddWithValue("p_comentarios", Comentarios);
                        command.Parameters.AddWithValue("p_data_aprovacao", DataAprovacao);
                        command.Parameters.AddWithValue("p_id_usuario_em_uso", IdUsuarioEmUso);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException("A proposta " + CodigoProposta + " não existe para o cliente " + Cliente.Nome);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta a proposta na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="deletarItens">Valor booleano que indica se os itens da proposta também deverão ser excluídos. Verdadeiro por padrão</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarPropostaDatabaseAsync(CancellationToken ct, bool deletarItens = true)
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

                // Deleta os itens da proposta caso a opção seja verdadeira
                if (deletarItens)
                {
                    foreach (var item in ListaItensProposta)
                    {
                        await item.DeletarItemPropostaDatabaseAsync(ct);
                    }
                }

                // Utilização do comando
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_proposta", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_proposta", Id);
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
                            throw new ChaveEstrangeiraEmUsoException(nameof(Proposta).ToLower(), CodigoProposta);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("A proposta " + CodigoProposta + " não existe para o cliente " + Cliente.Nome);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de propostas com os argumentos utilizados
        /// </summary>
        /// <param name="listaPropostas">Representa a lista de propostas que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="retornaItens">Representa a opção de preencher ou não a lista de itens dessa proposta</param>
        /// <param name="getPropostaOrigem">Representa a opção de retornar ou não a proposta de origem</param>
        /// <param name="getUltimaProposta">Representa a opção de retornar ou não a última proposta</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaPropostasAsync(ObservableCollection<Proposta> listaPropostas, bool limparLista, bool retornaItens, bool getPropostaOrigem, bool getUltimaProposta, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaPropostas == null)
            {
                listaPropostas = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaPropostas.Clear();
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
                  + "prop.id_proposta AS Id, "
                  + "prop.data_solicitacao AS DataSolicitacao, "
                  + "prop.data_insercao AS DataInsercao, "
                  + "prop.data_envio_faturamento AS DataEnvioFaturamento, "
                  + "prop.data_faturamento AS DataFaturamento, "
                  + "prop.nota_fiscal AS NotaFiscal, "
                  + "prop.valor_faturamento AS ValorFaturamento, "
                  + "prop.codigo_proposta AS CodigoProposta, "
                  + "prop.horimetro AS Horimetro, "
                  + "prop.ordem_servico AS OrdemServico, "
                  + "prop.data_envio AS DataEnvio, "
                  + "prop.data_edicao AS DataEdicao, "
                  + "prop.texto_padrao AS TextoPadrao, "
                  + "prop.observacoes AS Observacoes, "
                  + "prop.prazo_entrega AS PrazoEntrega, "
                  + "prop.condicao_pagamento AS CondicaoPagamento, "
                  + "prop.garantia AS Garantia, "
                  + "prop.validade AS Validade, "
                  + "prop.comentarios AS Comentarios, "
                  + "prop.data_aprovacao AS DataAprovacao, "
                  + "prop.id_filial AS idFilial, "
                  + "prop.id_cliente AS idCliente, "
                  + "prop.id_contato AS idContato, "
                  + "prop.id_usuario_insercao AS idUsuarioInsercao, "
                  + "prop.id_usuario_edicao AS idUsuarioEdicao, "
                  + "prop.id_status AS idStatus, "
                  + "prop.id_prioridade AS idPrioridade, "
                  + "prop.id_fabricante AS idFabricante, "
                  + "prop.id_tipo_equipamento AS idTipoEquipamento, "
                  + "prop.id_modelo AS idModelo, "
                  + "prop.id_ano AS idAno, "
                  + "prop.id_serie AS idSerie, "
                  + "prop.id_status_aprovacao AS idStatusAprovacao, "
                  + "prop.id_justificativa_aprovacao AS idJustificativaAprovacao, "
                  + "prop.id_proposta_origem AS idPropostaOrigem, "
                  + "prop.id_ultima_proposta AS idUltimaProposta "
                  + "FROM tb_propostas AS prop "
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
                                Proposta item = new();

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
                                if (item.Contato == null)
                                {
                                    item.Contato = new();
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
                                if (item.Prioridade == null)
                                {
                                    item.Prioridade = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Fabricante == null)
                                {
                                    item.Fabricante = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.TipoEquipamento == null)
                                {
                                    item.TipoEquipamento = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo == null)
                                {
                                    item.Modelo = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Ano == null)
                                {
                                    item.Ano = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Serie == null)
                                {
                                    item.Serie = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.StatusAprovacao == null)
                                {
                                    item.StatusAprovacao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.JustificativaAprovacao == null)
                                {
                                    item.JustificativaAprovacao = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.PropostaOrigem == null)
                                {
                                    item.PropostaOrigem = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.UltimaProposta == null)
                                {
                                    item.UltimaProposta = new();
                                }

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.ListaItensProposta == null)
                                {
                                    item.ListaItensProposta = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                item.DataSolicitacao = FuncoesDeConversao.ConverteParaDateTime(reader["DataSolicitacao"]);
                                item.DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                item.DataEnvioFaturamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataEnvioFaturamento"]);
                                item.DataFaturamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataFaturamento"]);
                                item.NotaFiscal = FuncoesDeConversao.ConverteParaInt(reader["NotaFiscal"]);
                                item.ValorFaturamento = FuncoesDeConversao.ConverteParaDecimal(reader["ValorFaturamento"]);
                                item.CodigoProposta = FuncoesDeConversao.ConverteParaString(reader["CodigoProposta"]);
                                item.Horimetro = FuncoesDeConversao.ConverteParaDecimal(reader["Horimetro"]);
                                item.OrdemServico = FuncoesDeConversao.ConverteParaInt(reader["OrdemServico"]);
                                item.DataEnvio = FuncoesDeConversao.ConverteParaDateTime(reader["DataEnvio"]);
                                item.DataEdicao = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicao"]);
                                item.TextoPadrao = FuncoesDeConversao.ConverteParaString(reader["TextoPadrao"]);
                                item.Observacoes = FuncoesDeConversao.ConverteParaString(reader["Observacoes"]);
                                item.PrazoEntrega = FuncoesDeConversao.ConverteParaString(reader["PrazoEntrega"]);
                                item.CondicaoPagamento = FuncoesDeConversao.ConverteParaString(reader["CondicaoPagamento"]);
                                item.Garantia = FuncoesDeConversao.ConverteParaString(reader["Garantia"]);
                                item.Validade = FuncoesDeConversao.ConverteParaString(reader["Validade"]);
                                item.Comentarios = FuncoesDeConversao.ConverteParaString(reader["Comentarios"]);
                                item.DataAprovacao = FuncoesDeConversao.ConverteParaDateTime(reader["DataAprovacao"]);

                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);
                                item.Filial.Id = FuncoesDeConversao.ConverteParaInt(reader["idFilial"]);
                                item.Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["idCliente"]);
                                item.Contato.Id = FuncoesDeConversao.ConverteParaInt(reader["idContato"]);
                                item.UsuarioInsercao.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuarioInsercao"]);
                                item.UsuarioEdicao.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuarioEdicao"]);
                                item.Prioridade.Id = FuncoesDeConversao.ConverteParaInt(reader["idPrioridade"]);
                                item.Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["idFabricante"]);
                                item.TipoEquipamento.Id = FuncoesDeConversao.ConverteParaInt(reader["idTipoEquipamento"]);
                                item.Modelo.Id = FuncoesDeConversao.ConverteParaInt(reader["idModelo"]);
                                item.Ano.Id = FuncoesDeConversao.ConverteParaInt(reader["idAno"]);
                                item.Serie.Id = FuncoesDeConversao.ConverteParaInt(reader["idSerie"]);
                                item.StatusAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatusAprovacao"]);
                                item.JustificativaAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idJustificativaAprovacao"]);
                                item.PropostaOrigem.Id = FuncoesDeConversao.ConverteParaInt(reader["idPropostaOrigem"]);
                                item.UltimaProposta.Id = FuncoesDeConversao.ConverteParaInt(reader["idUltimaProposta"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaPropostas.Add(item);

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

            int totalLinhasLista = listaPropostas.Count;
            int linhaAtualLista = 0;

            foreach (Proposta item in listaPropostas)
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Define as classes
                await item.Filial.GetFilialDatabaseAsync(item.Filial.Id, ct);
                await item.Cliente.GetClienteDatabaseAsync(item.Cliente.Id, ct);
                await item.Contato.GetContatoDatabaseAsync(item.Contato.Id, ct);
                await item.UsuarioInsercao.GetUsuarioDatabaseAsync(item.UsuarioInsercao.Id, ct);
                await item.UsuarioEdicao.GetUsuarioDatabaseAsync(item.UsuarioEdicao.Id, ct);
                await item.Status.GetStatusDatabaseAsync(item.Status.Id, ct);
                await item.Prioridade.GetPrioridadeDatabaseAsync(item.Prioridade.Id, ct);
                await item.Fabricante.GetFabricanteDatabaseAsync(item.Fabricante.Id, ct);
                await item.TipoEquipamento.GetTipoEquipamentoDatabaseAsync(item.TipoEquipamento.Id, ct);
                await item.Modelo.GetModeloDatabaseAsync(item.Modelo.Id, ct);
                await item.Ano.GetAnoDatabaseAsync(item.Ano.Id, ct);
                await item.Serie.GetSerieDatabaseAsync(item.Serie.Id, ct);
                await item.StatusAprovacao.GetStatusAprovacaoDatabaseAsync(item.StatusAprovacao.Id, ct);
                await item.JustificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(item.JustificativaAprovacao.Id, ct);

                // Caso verdadeiro, a classe PropostaOrigem será retornada. Classes subsequentes não serão retornadas para evitar estouro de carga
                if (getPropostaOrigem)
                {
                    await item.PropostaOrigem.GetPropostaDatabaseAsync(item.PropostaOrigem.Id, ct);
                }

                // Caso verdadeiro, a classe UltimaProposta será retornada. Classes subsequentes não serão retornadas para evitar estouro de carga
                if (getUltimaProposta)
                {
                    await item.UltimaProposta.GetPropostaDatabaseAsync(item.UltimaProposta.Id, ct);
                }

                // Caso verdadeiro, a lista de itens da proposta será retornada
                if (retornaItens)
                {
                    await ItemProposta.PreencheListaItensPropostaAsync(item.ListaItensProposta, true, false, null,
                        ct, "WHERE itpr.id_proposta = @id_proposta", "@id_proposta", item.Id);
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
        /// Método assíncrono que salva os dados de envio da proposta
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public async Task SalvaDadosEnvioPropostaAsync(CancellationToken ct)
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

                using (var command = db.conexao.CreateCommand())
                {
                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "UPDATE tb_propostas SET " +
                        "texto_padrao = @texto_padrao, " +
                        "observacoes = @observacoes, " +
                        "prazo_entrega = @prazo_entrega, " +
                        "condicao_pagamento = @condicao_pagamento, " +
                        "garantia = @garantia, " +
                        "validade = @validade, " +
                        "data_envio = @data_envio WHERE id_proposta = @id_proposta";
                    command.Parameters.AddWithValue("@texto_padrao", TextoPadrao);
                    command.Parameters.AddWithValue("@observacoes", Observacoes);
                    command.Parameters.AddWithValue("@prazo_entrega", PrazoEntrega);
                    command.Parameters.AddWithValue("@condicao_pagamento", CondicaoPagamento);
                    command.Parameters.AddWithValue("@garantia", Garantia);
                    command.Parameters.AddWithValue("@validade", Validade);
                    command.Parameters.AddWithValue("@data_envio", DataEnvio);
                    command.Parameters.AddWithValue("@id_proposta", Id);

                    // Executa o comando
                    await command.ExecuteNonQueryAsync(ct);
                }
            }
        }

        public static async Task<bool> PropostaExiste(int? idProposta, CancellationToken ct)
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
                      + "COUNT(id_proposta) AS Contagem "
                      + "FROM tb_propostas WHERE id_proposta = @id_proposta";

                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = comando;
                    command.Parameters.AddWithValue("@id_proposta", idProposta);

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
                                return FuncoesDeConversao.ConverteParaInt(reader["Contagem"]) > 0;
                            }
                        }
                    }
                }
            }
            return false;
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
                using (MySqlConnector.MySqlCommand command = new("sp_atualiza_id_usuario_em_uso_proposta", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_proposta", Id);
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
                using (MySqlConnector.MySqlCommand command = new("sp_retorna_id_usuario_em_uso_proposta", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_proposta", Id);
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
            Proposta propostaCopia = new(true);

            propostaCopia.Id = Id;
            propostaCopia.DataSolicitacao = DataSolicitacao;
            propostaCopia.DataInsercao = DataInsercao;
            propostaCopia.DataEnvioFaturamento = DataEnvioFaturamento;
            propostaCopia.DataFaturamento = DataFaturamento;
            propostaCopia.NotaFiscal = NotaFiscal;
            propostaCopia.ValorFaturamento = ValorFaturamento;
            propostaCopia.CodigoProposta = CodigoProposta;
            propostaCopia.Filial = (Filial)Filial.Clone();
            propostaCopia.Cliente = (Cliente)Cliente.Clone();
            propostaCopia.Contato = (Contato)Contato.Clone();
            propostaCopia.UsuarioInsercao = (Usuario)UsuarioInsercao.Clone();
            propostaCopia.UsuarioEdicao = (Usuario)UsuarioEdicao.Clone();
            propostaCopia.Status = (Status)Status.Clone();
            propostaCopia.Prioridade = (Prioridade)Prioridade.Clone();
            propostaCopia.Fabricante = (Fabricante)Fabricante.Clone();
            propostaCopia.TipoEquipamento = (TipoEquipamento)TipoEquipamento.Clone();
            propostaCopia.Modelo = (Modelo)Modelo.Clone();
            propostaCopia.Ano = (Ano)Ano.Clone();
            propostaCopia.Serie = (Serie)Serie.Clone();
            propostaCopia.StatusAprovacao = (StatusAprovacao)StatusAprovacao.Clone();
            propostaCopia.JustificativaAprovacao = (JustificativaAprovacao)JustificativaAprovacao.Clone();
            propostaCopia.Horimetro = Horimetro;
            propostaCopia.OrdemServico = OrdemServico;
            propostaCopia.DataEnvio = DataEnvio;
            propostaCopia.DataEdicao = DataEdicao;

            if (PropostaOrigem == null)
            {
                propostaCopia.PropostaOrigem = new();
            }
            else
            {
                propostaCopia.PropostaOrigem = (Proposta)PropostaOrigem.Clone();
            }

            if (UltimaProposta == null)
            {
                propostaCopia.UltimaProposta = new();
            }
            else
            {
                propostaCopia.UltimaProposta = (Proposta)UltimaProposta.Clone();
            }

            propostaCopia.TextoPadrao = TextoPadrao;
            propostaCopia.Observacoes = Observacoes;
            propostaCopia.PrazoEntrega = PrazoEntrega;
            propostaCopia.CondicaoPagamento = CondicaoPagamento;
            propostaCopia.Garantia = Garantia;
            propostaCopia.Validade = Validade;
            propostaCopia.Comentarios = Comentarios;
            propostaCopia.DataAprovacao = DataAprovacao;

            if (ListaItensProposta != null)
            {
                foreach (var item in ListaItensProposta)
                {
                    propostaCopia.ListaItensProposta.Add(item);
                }
            }

            return propostaCopia;
        }

        #endregion Interfaces
    }
}