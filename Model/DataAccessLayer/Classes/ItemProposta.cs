using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class ItemProposta : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private DateTime? _dataInsercao;
        private string? _codigoItem;
        private string? _descricaoItem;
        private decimal? _quantidadeItem;
        private decimal? _precoUnitarioInicialItem;
        private decimal? _percentualIpiItem;
        private decimal? _percentualIcmsItem;
        private string? _ncmItem;
        private decimal? _mvaItem;
        private decimal? _valorStItem;
        private decimal? _valorTotalInicialItem;
        private string? _prazoInicialItem;
        private decimal? _freteUnitarioItem;
        private decimal? _descontoInicialItem;
        private decimal? _precoAposDescontoInicialItem;
        private decimal? _precoComIpiEIcmsItem;
        private decimal? _percentualAliquotaExternaIcmsItem;
        private decimal? _valorIcmsCreditoItem;
        private decimal? _percentualMvaItem;
        private decimal? _valorMvaItem;
        private decimal? _precoComMvaItem;
        private decimal? _percentualAliquotaInternaIcmsItem;
        private decimal? _valorIcmsDebitoItem;
        private decimal? _saldoIcmsItem;
        private decimal? _precoUnitarioParaRevendedorItem;
        private decimal? _impostosFederaisItem;
        private decimal? _rateioDespesaFixaItem;
        private decimal? _percentualLucroNecessarioItem;
        private decimal? _precoListaVendaItem;
        private decimal? _descontoFinalItem;
        private decimal? _precoUnitarioFinalItem;
        private decimal? _precoTotalFinalItem;
        private decimal? _quantidadeEstoqueCodigoCompletoItem;
        private decimal? _quantidadeEstoqueCodigoAbreviadoItem;
        private string? _informacaoEstoqueCodigoCompletoItem;
        private string? _informacaoEstoqueCodigoAbreviadoItem;
        private string? _prazoFinalItem;
        private string? _comentariosItem;
        private DateTime? _dataAprovacaoItem;
        private DateTime? _dataEdicaoItem;
        private Usuario? _usuario;
        private Status? _status;
        private StatusAprovacao? _statusAprovacao;
        private JustificativaAprovacao? _justificativaAprovacao;
        private TipoItem? _tipoItem;
        private Fornecedor? _fornecedor;
        private TipoSubstituicaoTributaria? _tipoSubstituicaoTributaria;
        private Origem? _origem;
        private Conjunto? _conjunto;
        private Especificacao? _especificacao;
        private Proposta? _proposta;

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

        public decimal? PrecoUnitarioInicialItem
        {
            get { return _precoUnitarioInicialItem; }
            set
            {
                if (value != _precoUnitarioInicialItem)
                {
                    _precoUnitarioInicialItem = value;
                    OnPropertyChanged(nameof(PrecoUnitarioInicialItem));
                }
            }
        }

        public decimal? PercentualIpiItem
        {
            get { return _percentualIpiItem; }
            set
            {
                if (value != _percentualIpiItem)
                {
                    _percentualIpiItem = value;
                    OnPropertyChanged(nameof(PercentualIpiItem));
                }
            }
        }

        public decimal? PercentualIcmsItem
        {
            get { return _percentualIcmsItem; }
            set
            {
                if (value != _percentualIcmsItem)
                {
                    _percentualIcmsItem = value;
                    OnPropertyChanged(nameof(PercentualIcmsItem));
                }
            }
        }

        public string? NcmItem
        {
            get { return _ncmItem; }
            set
            {
                if (value != _ncmItem)
                {
                    _ncmItem = value;
                    OnPropertyChanged(nameof(NcmItem));
                }
            }
        }

        public decimal? MvaItem
        {
            get { return _mvaItem; }
            set
            {
                if (value != _mvaItem)
                {
                    _mvaItem = value;
                    OnPropertyChanged(nameof(MvaItem));
                }
            }
        }

        public decimal? ValorStItem
        {
            get { return _valorStItem; }
            set
            {
                if (value != _valorStItem)
                {
                    _valorStItem = value;
                    OnPropertyChanged(nameof(ValorStItem));
                }
            }
        }

        public decimal? ValorTotalInicialItem
        {
            get { return _valorTotalInicialItem; }
            set
            {
                if (value != _valorTotalInicialItem)
                {
                    _valorTotalInicialItem = value;
                    OnPropertyChanged(nameof(ValorTotalInicialItem));
                }
            }
        }

        public string? PrazoInicialItem
        {
            get { return _prazoInicialItem; }
            set
            {
                if (value != _prazoInicialItem)
                {
                    _prazoInicialItem = value;
                    OnPropertyChanged(nameof(PrazoInicialItem));
                }
            }
        }

        public decimal? FreteUnitarioItem
        {
            get { return _freteUnitarioItem; }
            set
            {
                if (value != _freteUnitarioItem)
                {
                    _freteUnitarioItem = value;
                    OnPropertyChanged(nameof(FreteUnitarioItem));
                }
            }
        }

        public decimal? DescontoInicialItem
        {
            get { return _descontoInicialItem; }
            set
            {
                if (value != _descontoInicialItem)
                {
                    _descontoInicialItem = value;
                    OnPropertyChanged(nameof(DescontoInicialItem));
                }
            }
        }

        public decimal? PrecoAposDescontoInicialItem
        {
            get { return _precoAposDescontoInicialItem; }
            set
            {
                if (value != _precoAposDescontoInicialItem)
                {
                    _precoAposDescontoInicialItem = value;
                    OnPropertyChanged(nameof(PrecoAposDescontoInicialItem));
                }
            }
        }

        public decimal? PrecoComIpiEIcmsItem
        {
            get { return _precoComIpiEIcmsItem; }
            set
            {
                if (value != _precoComIpiEIcmsItem)
                {
                    _precoComIpiEIcmsItem = value;
                    OnPropertyChanged(nameof(PrecoComIpiEIcmsItem));
                }
            }
        }

        public decimal? PercentualAliquotaExternaIcmsItem
        {
            get { return _percentualAliquotaExternaIcmsItem; }
            set
            {
                if (value != _percentualAliquotaExternaIcmsItem)
                {
                    _percentualAliquotaExternaIcmsItem = value;
                    OnPropertyChanged(nameof(PercentualAliquotaExternaIcmsItem));
                }
            }
        }

        public decimal? ValorIcmsCreditoItem
        {
            get { return _valorIcmsCreditoItem; }
            set
            {
                if (value != _valorIcmsCreditoItem)
                {
                    _valorIcmsCreditoItem = value;
                    OnPropertyChanged(nameof(ValorIcmsCreditoItem));
                }
            }
        }

        public decimal? PercentualMvaItem
        {
            get { return _percentualMvaItem; }
            set
            {
                if (value != _percentualMvaItem)
                {
                    _percentualMvaItem = value;
                    OnPropertyChanged(nameof(PercentualMvaItem));
                }
            }
        }

        public decimal? ValorMvaItem
        {
            get { return _valorMvaItem; }
            set
            {
                if (value != _valorMvaItem)
                {
                    _valorMvaItem = value;
                    OnPropertyChanged(nameof(ValorMvaItem));
                }
            }
        }

        public decimal? PrecoComMvaItem
        {
            get { return _precoComMvaItem; }
            set
            {
                if (value != _precoComMvaItem)
                {
                    _precoComMvaItem = value;
                    OnPropertyChanged(nameof(PrecoComMvaItem));
                }
            }
        }

        public decimal? PercentualAliquotaInternaIcmsItem
        {
            get { return _percentualAliquotaInternaIcmsItem; }
            set
            {
                if (value != _percentualAliquotaInternaIcmsItem)
                {
                    _percentualAliquotaInternaIcmsItem = value;
                    OnPropertyChanged(nameof(PercentualAliquotaInternaIcmsItem));
                }
            }
        }

        public decimal? ValorIcmsDebitoItem
        {
            get { return _valorIcmsDebitoItem; }
            set
            {
                if (value != _valorIcmsDebitoItem)
                {
                    _valorIcmsDebitoItem = value;
                    OnPropertyChanged(nameof(ValorIcmsDebitoItem));
                }
            }
        }

        public decimal? SaldoIcmsItem
        {
            get { return _saldoIcmsItem; }
            set
            {
                if (value != _saldoIcmsItem)
                {
                    _saldoIcmsItem = value;
                    OnPropertyChanged(nameof(SaldoIcmsItem));
                }
            }
        }

        public decimal? PrecoUnitarioParaRevendedorItem
        {
            get { return _precoUnitarioParaRevendedorItem; }
            set
            {
                if (value != _precoUnitarioParaRevendedorItem)
                {
                    _precoUnitarioParaRevendedorItem = value;
                    OnPropertyChanged(nameof(PrecoUnitarioParaRevendedorItem));
                }
            }
        }

        public decimal? ImpostosFederaisItem
        {
            get { return _impostosFederaisItem; }
            set
            {
                if (value != _impostosFederaisItem)
                {
                    _impostosFederaisItem = value;
                    OnPropertyChanged(nameof(ImpostosFederaisItem));
                }
            }
        }

        public decimal? RateioDespesaFixaItem
        {
            get { return _rateioDespesaFixaItem; }
            set
            {
                if (value != _rateioDespesaFixaItem)
                {
                    _rateioDespesaFixaItem = value;
                    OnPropertyChanged(nameof(RateioDespesaFixaItem));
                }
            }
        }

        public decimal? PercentualLucroNecessarioItem
        {
            get { return _percentualLucroNecessarioItem; }
            set
            {
                if (value != _percentualLucroNecessarioItem)
                {
                    _percentualLucroNecessarioItem = value;
                    OnPropertyChanged(nameof(PercentualLucroNecessarioItem));
                }
            }
        }

        public decimal? PrecoListaVendaItem
        {
            get { return _precoListaVendaItem; }
            set
            {
                if (value != _precoListaVendaItem)
                {
                    _precoListaVendaItem = value;
                    OnPropertyChanged(nameof(PrecoListaVendaItem));
                }
            }
        }

        public decimal? DescontoFinalItem
        {
            get { return _descontoFinalItem; }
            set
            {
                if (value != _descontoFinalItem)
                {
                    _descontoFinalItem = value;
                    OnPropertyChanged(nameof(DescontoFinalItem));
                }
            }
        }

        public decimal? PrecoUnitarioFinalItem
        {
            get { return _precoUnitarioFinalItem; }
            set
            {
                if (value != _precoUnitarioFinalItem)
                {
                    _precoUnitarioFinalItem = value;
                    OnPropertyChanged(nameof(PrecoUnitarioFinalItem));
                }
            }
        }

        public decimal? PrecoTotalFinalItem
        {
            get { return _precoTotalFinalItem; }
            set
            {
                if (value != _precoTotalFinalItem)
                {
                    _precoTotalFinalItem = value;
                    OnPropertyChanged(nameof(PrecoTotalFinalItem));
                }
            }
        }

        public decimal? QuantidadeEstoqueCodigoCompletoItem
        {
            get { return _quantidadeEstoqueCodigoCompletoItem; }
            set
            {
                if (value != _quantidadeEstoqueCodigoCompletoItem)
                {
                    _quantidadeEstoqueCodigoCompletoItem = value;
                    OnPropertyChanged(nameof(QuantidadeEstoqueCodigoCompletoItem));
                }
            }
        }

        public decimal? QuantidadeEstoqueCodigoAbreviadoItem
        {
            get { return _quantidadeEstoqueCodigoAbreviadoItem; }
            set
            {
                if (value != _quantidadeEstoqueCodigoAbreviadoItem)
                {
                    _quantidadeEstoqueCodigoAbreviadoItem = value;
                    OnPropertyChanged(nameof(QuantidadeEstoqueCodigoAbreviadoItem));
                }
            }
        }

        public string? InformacaoEstoqueCodigoCompletoItem
        {
            get { return _informacaoEstoqueCodigoCompletoItem; }
            set
            {
                if (value != _informacaoEstoqueCodigoCompletoItem)
                {
                    _informacaoEstoqueCodigoCompletoItem = value;
                    OnPropertyChanged(nameof(InformacaoEstoqueCodigoCompletoItem));
                }
            }
        }

        public string? InformacaoEstoqueCodigoAbreviadoItem
        {
            get { return _informacaoEstoqueCodigoAbreviadoItem; }
            set
            {
                if (value != _informacaoEstoqueCodigoAbreviadoItem)
                {
                    _informacaoEstoqueCodigoAbreviadoItem = value;
                    OnPropertyChanged(nameof(InformacaoEstoqueCodigoAbreviadoItem));
                }
            }
        }

        public string? PrazoFinalItem
        {
            get { return _prazoFinalItem; }
            set
            {
                if (value != _prazoFinalItem)
                {
                    _prazoFinalItem = value;
                    OnPropertyChanged(nameof(PrazoFinalItem));
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

        public DateTime? DataAprovacaoItem
        {
            get { return _dataAprovacaoItem; }
            set
            {
                if (value != _dataAprovacaoItem)
                {
                    _dataAprovacaoItem = value;
                    OnPropertyChanged(nameof(DataAprovacaoItem));
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

        public TipoItem? TipoItem
        {
            get { return _tipoItem; }
            set
            {
                if (value != _tipoItem)
                {
                    _tipoItem = value;
                    OnPropertyChanged(nameof(TipoItem));
                }
            }
        }

        public Fornecedor? Fornecedor
        {
            get { return _fornecedor; }
            set
            {
                if (value != _fornecedor)
                {
                    _fornecedor = value;
                    OnPropertyChanged(nameof(Fornecedor));
                }
            }
        }

        public TipoSubstituicaoTributaria? TipoSubstituicaoTributaria
        {
            get { return _tipoSubstituicaoTributaria; }
            set
            {
                if (value != _tipoSubstituicaoTributaria)
                {
                    _tipoSubstituicaoTributaria = value;
                    OnPropertyChanged(nameof(TipoSubstituicaoTributaria));
                }
            }
        }

        public Origem? Origem
        {
            get { return _origem; }
            set
            {
                if (value != _origem)
                {
                    _origem = value;
                    OnPropertyChanged(nameof(Origem));
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

        public Proposta? Proposta
        {
            get { return _proposta; }
            set
            {
                if (value != _proposta)
                {
                    _proposta = value;
                    OnPropertyChanged(nameof(Proposta));
                }
            }
        }

        #endregion Propriedades

        #region Construtores

        /// <summary>
        /// Construtor do item da proposta com os parâmetros utilizados
        /// </summary>
        /// <param name="inicializaProposta">Indica se a classe deve ser inicializada. Deve-se ter cuidado e levar em consideração loops infinitos</param>
        public ItemProposta(bool inicializaProposta = false, bool inicializarDemaisItens = false)
        {
            if (inicializaProposta)
            {
                Proposta = new();
            }

            if (inicializarDemaisItens)
            {
                Usuario = new();
                Status = new();
                StatusAprovacao = new();
                JustificativaAprovacao = new();
                TipoItem = new();
                Fornecedor = new();
                TipoSubstituicaoTributaria = new();
                Origem = new();
                Conjunto = new();
                Especificacao = new();
            }
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados do item da proposta com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id do item da proposta que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetItemPropostaDatabaseAsync(int? id, CancellationToken ct, bool retornaProposta = false)
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
                                          + "itpr.id_item_proposta AS Id, "
                                          + "itpr.data_insercao AS DataInsercao, "
                                          + "itpr.codigo_item AS CodigoItem, "
                                          + "itpr.descricao_item AS DescricaoItem, "
                                          + "itpr.quantidade_item AS QuantidadeItem, "
                                          + "itpr.preco_unitario_inicial_item AS PrecoUnitarioInicialItem, "
                                          + "itpr.percentual_ipi_item AS PercentualIpiItem, "
                                          + "itpr.percentual_icms_item AS PercentualIcmsItem, "
                                          + "itpr.ncm_item AS NcmItem, "
                                          + "itpr.mva_item AS MvaItem, "
                                          + "itpr.valor_st_item AS ValorStItem, "
                                          + "itpr.valor_total_inicial_item AS ValorTotalInicialItem, "
                                          + "itpr.prazo_inicial_item AS PrazoInicialItem, "
                                          + "itpr.frete_unitario_item AS FreteUnitarioItem, "
                                          + "itpr.desconto_inicial_item AS DescontoInicialItem, "
                                          + "itpr.preco_apos_desconto_inicial_item AS PrecoAposDescontoInicialItem, "
                                          + "itpr.preco_com_ipi_e_icms_item AS PrecoComIpiEIcmsItem, "
                                          + "itpr.percentual_aliquota_externa_icms_item AS PercentualAliquotaExternaIcmsItem, "
                                          + "itpr.valor_icms_credito_item AS ValorIcmsCreditoItem, "
                                          + "itpr.percentual_mva_item AS PercentualMvaItem, "
                                          + "itpr.valor_mva_item AS ValorMvaItem, "
                                          + "itpr.preco_com_mva_item AS PrecoComMvaItem, "
                                          + "itpr.percentual_aliquota_interna_icms_item AS PercentualAliquotaInternaIcmsItem, "
                                          + "itpr.valor_icms_debito_item AS ValorIcmsDebitoItem, "
                                          + "itpr.saldo_icms_item AS SaldoIcmsItem, "
                                          + "itpr.preco_unitario_para_revendedor_item AS PrecoUnitarioParaRevendedorItem, "
                                          + "itpr.impostos_federais_item AS ImpostosFederaisItem, "
                                          + "itpr.rateio_despesa_fixa_item AS RateioDespesaFixaItem, "
                                          + "itpr.percentual_lucro_necessario_item AS PercentualLucroNecessarioItem, "
                                          + "itpr.preco_lista_venda_item AS PrecoListaVendaItem, "
                                          + "itpr.desconto_final_item AS DescontoFinalItem, "
                                          + "itpr.preco_unitario_final_item AS PrecoUnitarioFinalItem, "
                                          + "itpr.preco_total_final_item AS PrecoTotalFinalItem, "
                                          + "itpr.quantidade_estoque_codigo_completo_item AS QuantidadeEstoqueCodigoCompletoItem, "
                                          + "itpr.quantidade_estoque_codigo_abreviado_item AS QuantidadeEstoqueCodigoAbreviadoItem, "
                                          + "itpr.informacao_estoque_codigo_completo_item AS InformacaoEstoqueCodigoCompletoItem, "
                                          + "itpr.informacao_estoque_codigo_abreviado_item AS InformacaoEstoqueCodigoAbreviadoItem, "
                                          + "itpr.prazo_final_item AS PrazoFinalItem, "
                                          + "itpr.comentarios_item AS ComentariosItem, "
                                          + "itpr.data_aprovacao_item AS DataAprovacaoItem, "
                                          + "itpr.data_edicao_item AS DataEdicaoItem, "
                                          + "itpr.id_usuario AS idUsuario, "
                                          + "itpr.id_status AS idStatus, "
                                          + "itpr.id_proposta AS idProposta, "
                                          + "itpr.id_status_aprovacao AS idStatusAprovacao, "
                                          + "itpr.id_justificativa_aprovacao AS idJustificativaAprovacao, "
                                          + "itpr.id_tipo_item AS idTipoItem, "
                                          + "itpr.id_conjunto AS idConjunto, "
                                          + "itpr.id_especificacao AS idEspecificacao, "
                                          + "itpr.id_fornecedor AS idFornecedor, "
                                          + "itpr.id_tipo_substituicao_tributaria_item AS idTipoSubstituicaoTributaria, "
                                          + "itpr.id_origem_item AS idOrigem "
                                          + "FROM tb_itens_propostas AS itpr "
                                          + "WHERE itpr.id_item_proposta = @id";

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
                                if (retornaProposta)
                                {
                                    if (Proposta == null)
                                    {
                                        Proposta = new();
                                    }
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (StatusAprovacao == null)
                                {
                                    StatusAprovacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (JustificativaAprovacao == null)
                                {
                                    JustificativaAprovacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoItem == null)
                                {
                                    TipoItem = new();
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
                                if (Fornecedor == null)
                                {
                                    Fornecedor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoSubstituicaoTributaria == null)
                                {
                                    TipoSubstituicaoTributaria = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Origem == null)
                                {
                                    Origem = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                CodigoItem = FuncoesDeConversao.ConverteParaString(reader["CodigoItem"]);
                                DescricaoItem = FuncoesDeConversao.ConverteParaString(reader["DescricaoItem"]);
                                QuantidadeItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeItem"]);
                                PrecoUnitarioInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioInicialItem"]);
                                PercentualIpiItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualIpiItem"]);
                                PercentualIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualIcmsItem"]);
                                NcmItem = FuncoesDeConversao.ConverteParaString(reader["NcmItem"]);
                                MvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["MvaItem"]);
                                ValorStItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorStItem"]);
                                ValorTotalInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorTotalInicialItem"]);
                                PrazoInicialItem = FuncoesDeConversao.ConverteParaString(reader["PrazoInicialItem"]);
                                FreteUnitarioItem = FuncoesDeConversao.ConverteParaDecimal(reader["FreteUnitarioItem"]);
                                DescontoInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["DescontoInicialItem"]);
                                PrecoAposDescontoInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoAposDescontoInicialItem"]);
                                PrecoComIpiEIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoComIpiEIcmsItem"]);
                                PercentualAliquotaExternaIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaExternaIcmsItem"]);
                                ValorIcmsCreditoItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorIcmsCreditoItem"]);
                                PercentualMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualMvaItem"]);
                                ValorMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorMvaItem"]);
                                PrecoComMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoComMvaItem"]);
                                PercentualAliquotaInternaIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaInternaIcmsItem"]);
                                ValorIcmsDebitoItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorIcmsDebitoItem"]);
                                SaldoIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["SaldoIcmsItem"]);
                                PrecoUnitarioParaRevendedorItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioParaRevendedorItem"]);
                                ImpostosFederaisItem = FuncoesDeConversao.ConverteParaDecimal(reader["ImpostosFederaisItem"]);
                                RateioDespesaFixaItem = FuncoesDeConversao.ConverteParaDecimal(reader["RateioDespesaFixaItem"]);
                                PercentualLucroNecessarioItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItem"]);
                                PrecoListaVendaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoListaVendaItem"]);
                                DescontoFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["DescontoFinalItem"]);
                                PrecoUnitarioFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioFinalItem"]);
                                PrecoTotalFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoTotalFinalItem"]);
                                QuantidadeEstoqueCodigoCompletoItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeEstoqueCodigoCompletoItem"]);
                                QuantidadeEstoqueCodigoAbreviadoItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeEstoqueCodigoAbreviadoItem"]);
                                InformacaoEstoqueCodigoCompletoItem = FuncoesDeConversao.ConverteParaString(reader["InformacaoEstoqueCodigoCompletoItem"]);
                                InformacaoEstoqueCodigoAbreviadoItem = FuncoesDeConversao.ConverteParaString(reader["InformacaoEstoqueCodigoAbreviadoItem"]);
                                PrazoFinalItem = FuncoesDeConversao.ConverteParaString(reader["PrazoFinalItem"]);
                                ComentariosItem = FuncoesDeConversao.ConverteParaString(reader["ComentariosItem"]);
                                DataAprovacaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataAprovacaoItem"]);
                                DataEdicaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicaoItem"]);

                                Usuario.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuario"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);
                                StatusAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatusAprovacao"]);
                                JustificativaAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idJustificativaAprovacao"]);
                                TipoItem.Id = FuncoesDeConversao.ConverteParaInt(reader["idTipoItem"]);
                                Conjunto.Id = FuncoesDeConversao.ConverteParaInt(reader["idConjunto"]);
                                Especificacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idEspecificacao"]);
                                Fornecedor.Id = FuncoesDeConversao.ConverteParaInt(reader["idFornecedor"]);
                                TipoSubstituicaoTributaria.Id = FuncoesDeConversao.ConverteParaInt(reader["idTipoSubstituicaoTributaria"]);
                                Origem.Id = FuncoesDeConversao.ConverteParaInt(reader["idOrigem"]);

                                if (retornaProposta)
                                {
                                    Proposta.Id = FuncoesDeConversao.ConverteParaInt(reader["idProposta"]);
                                }
                            }
                        }
                    }
                }
            }

            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            await Usuario.GetUsuarioDatabaseAsync(Usuario.Id, ct);
            await Status.GetStatusDatabaseAsync(Status.Id, ct);
            await StatusAprovacao.GetStatusAprovacaoDatabaseAsync(StatusAprovacao.Id, ct);
            await JustificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(JustificativaAprovacao.Id, ct);
            await TipoItem.GetTipoItemDatabaseAsync(TipoItem.Id, ct);
            await Conjunto.GetConjuntoDatabaseAsync(Conjunto.Id, ct);
            await Especificacao.GetEspecificacaoDatabaseAsync(Especificacao.Id, ct);
            await Fornecedor.GetFornecedorDatabaseAsync(Fornecedor.Id, ct);
            await TipoSubstituicaoTributaria.GetTipoSubstituicaoTributariaDatabaseAsync(TipoSubstituicaoTributaria.Id, ct);
            await Origem.GetOrigemDatabaseAsync(Origem.Id, ct);

            if (retornaProposta)
            {
                await Proposta.GetPropostaDatabaseAsync(Proposta.Id, ct);
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados do item da proposta com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetItemPropostaDatabaseAsync(CancellationToken ct, bool retornaProposta, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                      + "itpr.id_item_proposta AS Id, "
                      + "itpr.data_insercao AS DataInsercao, "
                      + "itpr.codigo_item AS CodigoItem, "
                      + "itpr.descricao_item AS DescricaoItem, "
                      + "itpr.quantidade_item AS QuantidadeItem, "
                      + "itpr.preco_unitario_inicial_item AS PrecoUnitarioInicialItem, "
                      + "itpr.percentual_ipi_item AS PercentualIpiItem, "
                      + "itpr.percentual_icms_item AS PercentualIcmsItem, "
                      + "itpr.ncm_item AS NcmItem, "
                      + "itpr.mva_item AS MvaItem, "
                      + "itpr.valor_st_item AS ValorStItem, "
                      + "itpr.valor_total_inicial_item AS ValorTotalInicialItem, "
                      + "itpr.prazo_inicial_item AS PrazoInicialItem, "
                      + "itpr.frete_unitario_item AS FreteUnitarioItem, "
                      + "itpr.desconto_inicial_item AS DescontoInicialItem, "
                      + "itpr.preco_apos_desconto_inicial_item AS PrecoAposDescontoInicialItem, "
                      + "itpr.preco_com_ipi_e_icms_item AS PrecoComIpiEIcmsItem, "
                      + "itpr.percentual_aliquota_externa_icms_item AS PercentualAliquotaExternaIcmsItem, "
                      + "itpr.valor_icms_credito_item AS ValorIcmsCreditoItem, "
                      + "itpr.percentual_mva_item AS PercentualMvaItem, "
                      + "itpr.valor_mva_item AS ValorMvaItem, "
                      + "itpr.preco_com_mva_item AS PrecoComMvaItem, "
                      + "itpr.percentual_aliquota_interna_icms_item AS PercentualAliquotaInternaIcmsItem, "
                      + "itpr.valor_icms_debito_item AS ValorIcmsDebitoItem, "
                      + "itpr.saldo_icms_item AS SaldoIcmsItem, "
                      + "itpr.preco_unitario_para_revendedor_item AS PrecoUnitarioParaRevendedorItem, "
                      + "itpr.impostos_federais_item AS ImpostosFederaisItem, "
                      + "itpr.rateio_despesa_fixa_item AS RateioDespesaFixaItem, "
                      + "itpr.percentual_lucro_necessario_item AS PercentualLucroNecessarioItem, "
                      + "itpr.preco_lista_venda_item AS PrecoListaVendaItem, "
                      + "itpr.desconto_final_item AS DescontoFinalItem, "
                      + "itpr.preco_unitario_final_item AS PrecoUnitarioFinalItem, "
                      + "itpr.preco_total_final_item AS PrecoTotalFinalItem, "
                      + "itpr.quantidade_estoque_codigo_completo_item AS QuantidadeEstoqueCodigoCompletoItem, "
                      + "itpr.quantidade_estoque_codigo_abreviado_item AS QuantidadeEstoqueCodigoAbreviadoItem, "
                      + "itpr.informacao_estoque_codigo_completo_item AS InformacaoEstoqueCodigoCompletoItem, "
                      + "itpr.informacao_estoque_codigo_abreviado_item AS InformacaoEstoqueCodigoAbreviadoItem, "
                      + "itpr.prazo_final_item AS PrazoFinalItem, "
                      + "itpr.comentarios_item AS ComentariosItem, "
                      + "itpr.data_aprovacao_item AS DataAprovacaoItem, "
                      + "itpr.data_edicao_item AS DataEdicaoItem, "
                      + "itpr.id_usuario AS idUsuario, "
                      + "itpr.id_status AS idStatus, "
                      + "itpr.id_proposta AS idProposta, "
                      + "itpr.id_status_aprovacao AS idStatusAprovacao, "
                      + "itpr.id_justificativa_aprovacao AS idJustificativaAprovacao, "
                      + "itpr.id_tipo_item AS idTipoItem, "
                      + "itpr.id_conjunto AS idConjunto, "
                      + "itpr.id_especificacao AS idEspecificacao, "
                      + "itpr.id_fornecedor AS idFornecedor, "
                      + "itpr.id_tipo_substituicao_tributaria_item AS idTipoSubstituicaoTributaria, "
                      + "itpr.id_origem_item AS idOrigem "
                      + "FROM tb_itens_propostas AS itpr "
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
                                if (retornaProposta)
                                {
                                    if (Proposta == null)
                                    {
                                        Proposta = new();
                                    }
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (StatusAprovacao == null)
                                {
                                    StatusAprovacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (JustificativaAprovacao == null)
                                {
                                    JustificativaAprovacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoItem == null)
                                {
                                    TipoItem = new();
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
                                if (Fornecedor == null)
                                {
                                    Fornecedor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoSubstituicaoTributaria == null)
                                {
                                    TipoSubstituicaoTributaria = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Origem == null)
                                {
                                    Origem = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                CodigoItem = FuncoesDeConversao.ConverteParaString(reader["CodigoItem"]);
                                DescricaoItem = FuncoesDeConversao.ConverteParaString(reader["DescricaoItem"]);
                                QuantidadeItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeItem"]);
                                PrecoUnitarioInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioInicialItem"]);
                                PercentualIpiItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualIpiItem"]);
                                PercentualIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualIcmsItem"]);
                                NcmItem = FuncoesDeConversao.ConverteParaString(reader["NcmItem"]);
                                MvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["MvaItem"]);
                                ValorStItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorStItem"]);
                                ValorTotalInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorTotalInicialItem"]);
                                PrazoInicialItem = FuncoesDeConversao.ConverteParaString(reader["PrazoInicialItem"]);
                                FreteUnitarioItem = FuncoesDeConversao.ConverteParaDecimal(reader["FreteUnitarioItem"]);
                                DescontoInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["DescontoInicialItem"]);
                                PrecoAposDescontoInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoAposDescontoInicialItem"]);
                                PrecoComIpiEIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoComIpiEIcmsItem"]);
                                PercentualAliquotaExternaIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaExternaIcmsItem"]);
                                ValorIcmsCreditoItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorIcmsCreditoItem"]);
                                PercentualMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualMvaItem"]);
                                ValorMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorMvaItem"]);
                                PrecoComMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoComMvaItem"]);
                                PercentualAliquotaInternaIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaInternaIcmsItem"]);
                                ValorIcmsDebitoItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorIcmsDebitoItem"]);
                                SaldoIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["SaldoIcmsItem"]);
                                PrecoUnitarioParaRevendedorItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioParaRevendedorItem"]);
                                ImpostosFederaisItem = FuncoesDeConversao.ConverteParaDecimal(reader["ImpostosFederaisItem"]);
                                RateioDespesaFixaItem = FuncoesDeConversao.ConverteParaDecimal(reader["RateioDespesaFixaItem"]);
                                PercentualLucroNecessarioItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItem"]);
                                PrecoListaVendaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoListaVendaItem"]);
                                DescontoFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["DescontoFinalItem"]);
                                PrecoUnitarioFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioFinalItem"]);
                                PrecoTotalFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoTotalFinalItem"]);
                                QuantidadeEstoqueCodigoCompletoItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeEstoqueCodigoCompletoItem"]);
                                QuantidadeEstoqueCodigoAbreviadoItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeEstoqueCodigoAbreviadoItem"]);
                                InformacaoEstoqueCodigoCompletoItem = FuncoesDeConversao.ConverteParaString(reader["InformacaoEstoqueCodigoCompletoItem"]);
                                InformacaoEstoqueCodigoAbreviadoItem = FuncoesDeConversao.ConverteParaString(reader["InformacaoEstoqueCodigoAbreviadoItem"]);
                                PrazoFinalItem = FuncoesDeConversao.ConverteParaString(reader["PrazoFinalItem"]);
                                ComentariosItem = FuncoesDeConversao.ConverteParaString(reader["ComentariosItem"]);
                                DataAprovacaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataAprovacaoItem"]);
                                DataEdicaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicaoItem"]);

                                Usuario.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuario"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);
                                StatusAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatusAprovacao"]);
                                JustificativaAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idJustificativaAprovacao"]);
                                TipoItem.Id = FuncoesDeConversao.ConverteParaInt(reader["idTipoItem"]);
                                Conjunto.Id = FuncoesDeConversao.ConverteParaInt(reader["idConjunto"]);
                                Especificacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idEspecificacao"]);
                                Fornecedor.Id = FuncoesDeConversao.ConverteParaInt(reader["idFornecedor"]);
                                TipoSubstituicaoTributaria.Id = FuncoesDeConversao.ConverteParaInt(reader["idTipoSubstituicaoTributaria"]);
                                Origem.Id = FuncoesDeConversao.ConverteParaInt(reader["idOrigem"]);

                                if (retornaProposta)
                                {
                                    Proposta.Id = FuncoesDeConversao.ConverteParaInt(reader["idProposta"]);
                                }
                            }
                        }
                    }
                }
            }

            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            await Usuario.GetUsuarioDatabaseAsync(Usuario.Id, ct);
            await Status.GetStatusDatabaseAsync(Status.Id, ct);
            await StatusAprovacao.GetStatusAprovacaoDatabaseAsync(StatusAprovacao.Id, ct);
            await JustificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(JustificativaAprovacao.Id, ct);
            await TipoItem.GetTipoItemDatabaseAsync(TipoItem.Id, ct);
            await Conjunto.GetConjuntoDatabaseAsync(Conjunto.Id, ct);
            await Especificacao.GetEspecificacaoDatabaseAsync(Especificacao.Id, ct);
            await Fornecedor.GetFornecedorDatabaseAsync(Fornecedor.Id, ct);
            await TipoSubstituicaoTributaria.GetTipoSubstituicaoTributariaDatabaseAsync(TipoSubstituicaoTributaria.Id, ct);
            await Origem.GetOrigemDatabaseAsync(Origem.Id, ct);

            if (retornaProposta)
            {
                await Proposta.GetPropostaDatabaseAsync(Proposta.Id, ct);
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados do item da proposta com os argumentos utilizados
        /// </summary>
        /// <param name="retornoCompleto">Representa a opção de retornar os dados completos</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetItemPropostaDatabaseAsync(bool retornoCompleto, CancellationToken ct, bool retornaProposta, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                      + "itpr.id_item_proposta AS Id, "
                      + "itpr.data_insercao AS DataInsercao, "
                      + "itpr.codigo_item AS CodigoItem, "
                      + "itpr.descricao_item AS DescricaoItem, "
                      + "itpr.quantidade_item AS QuantidadeItem, "
                      + "itpr.preco_unitario_inicial_item AS PrecoUnitarioInicialItem, "
                      + "itpr.percentual_ipi_item AS PercentualIpiItem, "
                      + "itpr.percentual_icms_item AS PercentualIcmsItem, "
                      + "itpr.ncm_item AS NcmItem, "
                      + "itpr.mva_item AS MvaItem, "
                      + "itpr.valor_st_item AS ValorStItem, "
                      + "itpr.valor_total_inicial_item AS ValorTotalInicialItem, "
                      + "itpr.prazo_inicial_item AS PrazoInicialItem, "
                      + "itpr.frete_unitario_item AS FreteUnitarioItem, "
                      + "itpr.desconto_inicial_item AS DescontoInicialItem, "
                      + "itpr.preco_apos_desconto_inicial_item AS PrecoAposDescontoInicialItem, "
                      + "itpr.preco_com_ipi_e_icms_item AS PrecoComIpiEIcmsItem, "
                      + "itpr.percentual_aliquota_externa_icms_item AS PercentualAliquotaExternaIcmsItem, "
                      + "itpr.valor_icms_credito_item AS ValorIcmsCreditoItem, "
                      + "itpr.percentual_mva_item AS PercentualMvaItem, "
                      + "itpr.valor_mva_item AS ValorMvaItem, "
                      + "itpr.preco_com_mva_item AS PrecoComMvaItem, "
                      + "itpr.percentual_aliquota_interna_icms_item AS PercentualAliquotaInternaIcmsItem, "
                      + "itpr.valor_icms_debito_item AS ValorIcmsDebitoItem, "
                      + "itpr.saldo_icms_item AS SaldoIcmsItem, "
                      + "itpr.preco_unitario_para_revendedor_item AS PrecoUnitarioParaRevendedorItem, "
                      + "itpr.impostos_federais_item AS ImpostosFederaisItem, "
                      + "itpr.rateio_despesa_fixa_item AS RateioDespesaFixaItem, "
                      + "itpr.percentual_lucro_necessario_item AS PercentualLucroNecessarioItem, "
                      + "itpr.preco_lista_venda_item AS PrecoListaVendaItem, "
                      + "itpr.desconto_final_item AS DescontoFinalItem, "
                      + "itpr.preco_unitario_final_item AS PrecoUnitarioFinalItem, "
                      + "itpr.preco_total_final_item AS PrecoTotalFinalItem, "
                      + "itpr.quantidade_estoque_codigo_completo_item AS QuantidadeEstoqueCodigoCompletoItem, "
                      + "itpr.quantidade_estoque_codigo_abreviado_item AS QuantidadeEstoqueCodigoAbreviadoItem, "
                      + "itpr.informacao_estoque_codigo_completo_item AS InformacaoEstoqueCodigoCompletoItem, "
                      + "itpr.informacao_estoque_codigo_abreviado_item AS InformacaoEstoqueCodigoAbreviadoItem, "
                      + "itpr.prazo_final_item AS PrazoFinalItem, "
                      + "itpr.comentarios_item AS ComentariosItem, "
                      + "itpr.data_aprovacao_item AS DataAprovacaoItem, "
                      + "itpr.data_edicao_item AS DataEdicaoItem, "
                      + "itpr.id_usuario AS idUsuario, "
                      + "itpr.id_status AS idStatus, "
                      + "itpr.id_proposta AS idProposta, "
                      + "itpr.id_status_aprovacao AS idStatusAprovacao, "
                      + "itpr.id_justificativa_aprovacao AS idJustificativaAprovacao, "
                      + "itpr.id_tipo_item AS idTipoItem, "
                      + "itpr.id_conjunto AS idConjunto, "
                      + "itpr.id_especificacao AS idEspecificacao, "
                      + "itpr.id_fornecedor AS idFornecedor, "
                      + "itpr.id_tipo_substituicao_tributaria_item AS idTipoSubstituicaoTributaria, "
                      + "itpr.id_origem_item AS idOrigem "
                      + "FROM tb_itens_propostas AS itpr "
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
                                if (retornaProposta)
                                {
                                    if (Proposta == null)
                                    {
                                        Proposta = new();
                                    }
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (StatusAprovacao == null)
                                {
                                    StatusAprovacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (JustificativaAprovacao == null)
                                {
                                    JustificativaAprovacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoItem == null)
                                {
                                    TipoItem = new();
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
                                if (Fornecedor == null)
                                {
                                    Fornecedor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoSubstituicaoTributaria == null)
                                {
                                    TipoSubstituicaoTributaria = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Origem == null)
                                {
                                    Origem = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                CodigoItem = FuncoesDeConversao.ConverteParaString(reader["CodigoItem"]);
                                DescricaoItem = FuncoesDeConversao.ConverteParaString(reader["DescricaoItem"]);
                                PrazoInicialItem = FuncoesDeConversao.ConverteParaString(reader["PrazoInicialItem"]);
                                PrecoUnitarioInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioInicialItem"]);
                                PercentualIpiItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualIpiItem"]);
                                PercentualIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualIcmsItem"]);
                                NcmItem = FuncoesDeConversao.ConverteParaString(reader["NcmItem"]);
                                MvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["MvaItem"]);
                                ValorStItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorStItem"]);

                                FreteUnitarioItem = FuncoesDeConversao.ConverteParaDecimal(reader["FreteUnitarioItem"]);

                                PrecoAposDescontoInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoAposDescontoInicialItem"]);
                                PrecoComIpiEIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoComIpiEIcmsItem"]);
                                PercentualAliquotaExternaIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaExternaIcmsItem"]);
                                ValorIcmsCreditoItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorIcmsCreditoItem"]);
                                PercentualMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualMvaItem"]);
                                ValorMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorMvaItem"]);
                                PrecoComMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoComMvaItem"]);
                                PercentualAliquotaInternaIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaInternaIcmsItem"]);
                                ValorIcmsDebitoItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorIcmsDebitoItem"]);
                                SaldoIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["SaldoIcmsItem"]);
                                PrecoUnitarioParaRevendedorItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioParaRevendedorItem"]);
                                ImpostosFederaisItem = FuncoesDeConversao.ConverteParaDecimal(reader["ImpostosFederaisItem"]);
                                RateioDespesaFixaItem = FuncoesDeConversao.ConverteParaDecimal(reader["RateioDespesaFixaItem"]);
                                PercentualLucroNecessarioItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItem"]);
                                PrecoListaVendaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoListaVendaItem"]);

                                PrecoUnitarioFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioFinalItem"]);
                                PrecoTotalFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoTotalFinalItem"]);

                                Conjunto.Id = FuncoesDeConversao.ConverteParaInt(reader["idConjunto"]);
                                Especificacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idEspecificacao"]);
                                Fornecedor.Id = FuncoesDeConversao.ConverteParaInt(reader["idFornecedor"]);
                                TipoSubstituicaoTributaria.Id = FuncoesDeConversao.ConverteParaInt(reader["idTipoSubstituicaoTributaria"]);
                                Origem.Id = FuncoesDeConversao.ConverteParaInt(reader["idOrigem"]);

                                if (retornoCompleto)
                                {
                                    
                                    QuantidadeItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeItem"]);
                                    ValorTotalInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorTotalInicialItem"]);
                                    QuantidadeEstoqueCodigoCompletoItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeEstoqueCodigoCompletoItem"]);
                                    QuantidadeEstoqueCodigoAbreviadoItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeEstoqueCodigoAbreviadoItem"]);
                                    InformacaoEstoqueCodigoCompletoItem = FuncoesDeConversao.ConverteParaString(reader["InformacaoEstoqueCodigoCompletoItem"]);
                                    InformacaoEstoqueCodigoAbreviadoItem = FuncoesDeConversao.ConverteParaString(reader["InformacaoEstoqueCodigoAbreviadoItem"]);
                                    PrazoFinalItem = FuncoesDeConversao.ConverteParaString(reader["PrazoFinalItem"]);
                                    ComentariosItem = FuncoesDeConversao.ConverteParaString(reader["ComentariosItem"]);
                                    DataAprovacaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataAprovacaoItem"]);
                                    DataEdicaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicaoItem"]);
                                    DescontoInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["DescontoInicialItem"]);
                                    DescontoFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["DescontoFinalItem"]);

                                    Usuario.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuario"]);
                                    TipoItem.Id = FuncoesDeConversao.ConverteParaInt(reader["idTipoItem"]);
                                    Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);
                                    StatusAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatusAprovacao"]);
                                    JustificativaAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idJustificativaAprovacao"]);
                                }

                                if (retornaProposta)
                                {
                                    Proposta.Id = FuncoesDeConversao.ConverteParaInt(reader["idProposta"]);
                                }
                            }
                        }
                    }
                }
            }

            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            await Conjunto.GetConjuntoDatabaseAsync(Conjunto.Id, ct);
            await Especificacao.GetEspecificacaoDatabaseAsync(Especificacao.Id, ct);
            await Fornecedor.GetFornecedorDatabaseAsync(Fornecedor.Id, ct);
            await TipoSubstituicaoTributaria.GetTipoSubstituicaoTributariaDatabaseAsync(TipoSubstituicaoTributaria.Id, ct);
            await Origem.GetOrigemDatabaseAsync(Origem.Id, ct);

            if (retornoCompleto)
            {
                await Usuario.GetUsuarioDatabaseAsync(Usuario.Id, ct);
                await Status.GetStatusDatabaseAsync(Status.Id, ct);
                await StatusAprovacao.GetStatusAprovacaoDatabaseAsync(StatusAprovacao.Id, ct);
                await JustificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(JustificativaAprovacao.Id, ct);
                await TipoItem.GetTipoItemDatabaseAsync(TipoItem.Id, ct);
            }

            if (retornaProposta)
            {
                await Proposta.GetPropostaDatabaseAsync(Proposta.Id, ct);
            }
        }

        /// <summary>
        /// Método assíncrono que salva o item da proposta na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarItemPropostaDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_item_proposta", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_data_insercao", DataInsercao);
                        command.Parameters.AddWithValue("p_id_usuario", Usuario == null ? null : Usuario.Id);
                        command.Parameters.AddWithValue("p_id_status", Status == null ? null : Status.Id);
                        command.Parameters.AddWithValue("p_id_proposta", Proposta == null ? null : Proposta.Id);
                        command.Parameters.AddWithValue("p_id_status_aprovacao", StatusAprovacao == null ? null : StatusAprovacao.Id);
                        command.Parameters.AddWithValue("p_id_justificativa_aprovacao", JustificativaAprovacao == null ? null : JustificativaAprovacao.Id);
                        command.Parameters.AddWithValue("p_id_tipo_item", TipoItem == null ? null : TipoItem.Id);
                        command.Parameters.AddWithValue("p_id_conjunto", Conjunto == null ? null : Conjunto.Id);
                        command.Parameters.AddWithValue("p_id_especificacao", Especificacao == null ? null : Especificacao.Id);
                        command.Parameters.AddWithValue("p_id_fornecedor", Fornecedor == null ? null : Fornecedor.Id);
                        command.Parameters.AddWithValue("p_codigo_item", CodigoItem);
                        command.Parameters.AddWithValue("p_descricao_item", DescricaoItem);
                        command.Parameters.AddWithValue("p_quantidade_item", QuantidadeItem);
                        command.Parameters.AddWithValue("p_preco_unitario_inicial_item", PrecoUnitarioInicialItem);
                        command.Parameters.AddWithValue("p_percentual_ipi_item", PercentualIpiItem);
                        command.Parameters.AddWithValue("p_percentual_icms_item", PercentualIcmsItem);
                        command.Parameters.AddWithValue("p_ncm_item", NcmItem);
                        command.Parameters.AddWithValue("p_mva_item", MvaItem);
                        command.Parameters.AddWithValue("p_valor_st_item", ValorStItem);
                        command.Parameters.AddWithValue("p_valor_total_inicial_item", ValorTotalInicialItem);
                        command.Parameters.AddWithValue("p_prazo_inicial_item", PrazoInicialItem);
                        command.Parameters.AddWithValue("p_frete_unitario_item", FreteUnitarioItem);
                        command.Parameters.AddWithValue("p_desconto_inicial_item", DescontoInicialItem);
                        command.Parameters.AddWithValue("p_id_tipo_substituicao_tributaria_item", TipoSubstituicaoTributaria == null ? null : TipoSubstituicaoTributaria.Id);
                        command.Parameters.AddWithValue("p_id_origem_item", Origem == null ? null : Origem.Id);
                        command.Parameters.AddWithValue("p_preco_apos_desconto_inicial_item", PrecoAposDescontoInicialItem);
                        command.Parameters.AddWithValue("p_preco_com_ipi_e_icms_item", PrecoComIpiEIcmsItem);
                        command.Parameters.AddWithValue("p_percentual_aliquota_externa_icms_item", PercentualAliquotaExternaIcmsItem);
                        command.Parameters.AddWithValue("p_valor_icms_credito_item", ValorIcmsCreditoItem);
                        command.Parameters.AddWithValue("p_percentual_mva_item", PercentualMvaItem);
                        command.Parameters.AddWithValue("p_valor_mva_item", ValorMvaItem);
                        command.Parameters.AddWithValue("p_preco_com_mva_item", PrecoComMvaItem);
                        command.Parameters.AddWithValue("p_percentual_aliquota_interna_icms_item", PercentualAliquotaInternaIcmsItem);
                        command.Parameters.AddWithValue("p_valor_icms_debito_item", ValorIcmsDebitoItem);
                        command.Parameters.AddWithValue("p_saldo_icms_item", SaldoIcmsItem);
                        command.Parameters.AddWithValue("p_preco_unitario_para_revendedor_item", PrecoUnitarioParaRevendedorItem);
                        command.Parameters.AddWithValue("p_impostos_federais_item", ImpostosFederaisItem);
                        command.Parameters.AddWithValue("p_rateio_despesa_fixa_item", RateioDespesaFixaItem);
                        command.Parameters.AddWithValue("p_percentual_lucro_necessario_item", PercentualLucroNecessarioItem);
                        command.Parameters.AddWithValue("p_preco_lista_venda_item", PrecoListaVendaItem);
                        command.Parameters.AddWithValue("p_desconto_final_item", DescontoFinalItem);
                        command.Parameters.AddWithValue("p_preco_unitario_final_item", PrecoUnitarioFinalItem);
                        command.Parameters.AddWithValue("p_preco_total_final_item", PrecoTotalFinalItem);
                        command.Parameters.AddWithValue("p_quantidade_estoque_codigo_completo_item", QuantidadeEstoqueCodigoCompletoItem);
                        command.Parameters.AddWithValue("p_quantidade_estoque_codigo_abreviado_item", QuantidadeEstoqueCodigoAbreviadoItem);
                        command.Parameters.AddWithValue("p_informacao_estoque_codigo_completo_item", InformacaoEstoqueCodigoCompletoItem);
                        command.Parameters.AddWithValue("p_informacao_estoque_codigo_abreviado_item", InformacaoEstoqueCodigoAbreviadoItem);
                        command.Parameters.AddWithValue("p_prazo_final_item", PrazoFinalItem);
                        command.Parameters.AddWithValue("p_comentarios_item", ComentariosItem);
                        command.Parameters.AddWithValue("p_data_aprovacao_item", DataAprovacaoItem);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_item_proposta", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_item_proposta", Id);
                        command.Parameters.AddWithValue("p_id_status", Status == null ? null : Status.Id);
                        command.Parameters.AddWithValue("p_id_proposta", Proposta == null ? null : Proposta.Id);
                        command.Parameters.AddWithValue("p_id_status_aprovacao", StatusAprovacao == null ? null : StatusAprovacao.Id);
                        command.Parameters.AddWithValue("p_id_justificativa_aprovacao", JustificativaAprovacao == null ? null : JustificativaAprovacao.Id);
                        command.Parameters.AddWithValue("p_id_tipo_item", TipoItem == null ? null : TipoItem.Id);
                        command.Parameters.AddWithValue("p_id_conjunto", Conjunto == null ? null : Conjunto.Id);
                        command.Parameters.AddWithValue("p_id_especificacao", Especificacao == null ? null : Especificacao.Id);
                        command.Parameters.AddWithValue("p_id_fornecedor", Fornecedor == null ? null : Fornecedor.Id);
                        command.Parameters.AddWithValue("p_codigo_item", CodigoItem);
                        command.Parameters.AddWithValue("p_descricao_item", DescricaoItem);
                        command.Parameters.AddWithValue("p_quantidade_item", QuantidadeItem);
                        command.Parameters.AddWithValue("p_preco_unitario_inicial_item", PrecoUnitarioInicialItem);
                        command.Parameters.AddWithValue("p_percentual_ipi_item", PercentualIpiItem);
                        command.Parameters.AddWithValue("p_percentual_icms_item", PercentualIcmsItem);
                        command.Parameters.AddWithValue("p_ncm_item", NcmItem);
                        command.Parameters.AddWithValue("p_mva_item", MvaItem);
                        command.Parameters.AddWithValue("p_valor_st_item", ValorStItem);
                        command.Parameters.AddWithValue("p_valor_total_inicial_item", ValorTotalInicialItem);
                        command.Parameters.AddWithValue("p_prazo_inicial_item", PrazoInicialItem);
                        command.Parameters.AddWithValue("p_frete_unitario_item", FreteUnitarioItem);
                        command.Parameters.AddWithValue("p_desconto_inicial_item", DescontoInicialItem);
                        command.Parameters.AddWithValue("p_id_tipo_substituicao_tributaria_item", TipoSubstituicaoTributaria == null ? null : TipoSubstituicaoTributaria.Id);
                        command.Parameters.AddWithValue("p_id_origem_item", Origem == null ? null : Origem.Id);
                        command.Parameters.AddWithValue("p_preco_apos_desconto_inicial_item", PrecoAposDescontoInicialItem);
                        command.Parameters.AddWithValue("p_preco_com_ipi_e_icms_item", PrecoComIpiEIcmsItem);
                        command.Parameters.AddWithValue("p_percentual_aliquota_externa_icms_item", PercentualAliquotaExternaIcmsItem);
                        command.Parameters.AddWithValue("p_valor_icms_credito_item", ValorIcmsCreditoItem);
                        command.Parameters.AddWithValue("p_percentual_mva_item", PercentualMvaItem);
                        command.Parameters.AddWithValue("p_valor_mva_item", ValorMvaItem);
                        command.Parameters.AddWithValue("p_preco_com_mva_item", PrecoComMvaItem);
                        command.Parameters.AddWithValue("p_percentual_aliquota_interna_icms_item", PercentualAliquotaInternaIcmsItem);
                        command.Parameters.AddWithValue("p_valor_icms_debito_item", ValorIcmsDebitoItem);
                        command.Parameters.AddWithValue("p_saldo_icms_item", SaldoIcmsItem);
                        command.Parameters.AddWithValue("p_preco_unitario_para_revendedor_item", PrecoUnitarioParaRevendedorItem);
                        command.Parameters.AddWithValue("p_impostos_federais_item", ImpostosFederaisItem);
                        command.Parameters.AddWithValue("p_rateio_despesa_fixa_item", RateioDespesaFixaItem);
                        command.Parameters.AddWithValue("p_percentual_lucro_necessario_item", PercentualLucroNecessarioItem);
                        command.Parameters.AddWithValue("p_preco_lista_venda_item", PrecoListaVendaItem);
                        command.Parameters.AddWithValue("p_desconto_final_item", DescontoFinalItem);
                        command.Parameters.AddWithValue("p_preco_unitario_final_item", PrecoUnitarioFinalItem);
                        command.Parameters.AddWithValue("p_preco_total_final_item", PrecoTotalFinalItem);
                        command.Parameters.AddWithValue("p_quantidade_estoque_codigo_completo_item", QuantidadeEstoqueCodigoCompletoItem);
                        command.Parameters.AddWithValue("p_quantidade_estoque_codigo_abreviado_item", QuantidadeEstoqueCodigoAbreviadoItem);
                        command.Parameters.AddWithValue("p_informacao_estoque_codigo_completo_item", InformacaoEstoqueCodigoCompletoItem);
                        command.Parameters.AddWithValue("p_informacao_estoque_codigo_abreviado_item", InformacaoEstoqueCodigoAbreviadoItem);
                        command.Parameters.AddWithValue("p_prazo_final_item", PrazoFinalItem);
                        command.Parameters.AddWithValue("p_comentarios_item", ComentariosItem);
                        command.Parameters.AddWithValue("p_data_aprovacao_item", DataAprovacaoItem);
                        command.Parameters.AddWithValue("p_data_edicao_item", DataEdicaoItem);
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
        /// Método assíncrono que deleta o item da proposta na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarItemPropostaDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_item_proposta", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_item_proposta", Id);
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
                            throw new ChaveEstrangeiraEmUsoException("item da proposta", CodigoItem + " - " + DescricaoItem);
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
        /// Método assíncrono que preenche uma lista de itens da proposta com os argumentos utilizados. ATENÇÃO: RETORNA APENAS OS ID'S DAS CLASSES
        /// </summary>
        /// <param name="listaItensProposta">Representa a lista de itens da proposta que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaItensPropostaAsync(ObservableCollection<ItemProposta> listaItensProposta, bool limparLista, bool retornaProposta, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaItensProposta == null)
            {
                listaItensProposta = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaItensProposta.Clear();
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
                  + "itpr.id_item_proposta AS Id, "
                  + "itpr.data_insercao AS DataInsercao, "
                  + "itpr.codigo_item AS CodigoItem, "
                  + "itpr.descricao_item AS DescricaoItem, "
                  + "itpr.quantidade_item AS QuantidadeItem, "
                  + "itpr.preco_unitario_inicial_item AS PrecoUnitarioInicialItem, "
                  + "itpr.percentual_ipi_item AS PercentualIpiItem, "
                  + "itpr.percentual_icms_item AS PercentualIcmsItem, "
                  + "itpr.ncm_item AS NcmItem, "
                  + "itpr.mva_item AS MvaItem, "
                  + "itpr.valor_st_item AS ValorStItem, "
                  + "itpr.valor_total_inicial_item AS ValorTotalInicialItem, "
                  + "itpr.prazo_inicial_item AS PrazoInicialItem, "
                  + "itpr.frete_unitario_item AS FreteUnitarioItem, "
                  + "itpr.desconto_inicial_item AS DescontoInicialItem, "
                  + "itpr.preco_apos_desconto_inicial_item AS PrecoAposDescontoInicialItem, "
                  + "itpr.preco_com_ipi_e_icms_item AS PrecoComIpiEIcmsItem, "
                  + "itpr.percentual_aliquota_externa_icms_item AS PercentualAliquotaExternaIcmsItem, "
                  + "itpr.valor_icms_credito_item AS ValorIcmsCreditoItem, "
                  + "itpr.percentual_mva_item AS PercentualMvaItem, "
                  + "itpr.valor_mva_item AS ValorMvaItem, "
                  + "itpr.preco_com_mva_item AS PrecoComMvaItem, "
                  + "itpr.percentual_aliquota_interna_icms_item AS PercentualAliquotaInternaIcmsItem, "
                  + "itpr.valor_icms_debito_item AS ValorIcmsDebitoItem, "
                  + "itpr.saldo_icms_item AS SaldoIcmsItem, "
                  + "itpr.preco_unitario_para_revendedor_item AS PrecoUnitarioParaRevendedorItem, "
                  + "itpr.impostos_federais_item AS ImpostosFederaisItem, "
                  + "itpr.rateio_despesa_fixa_item AS RateioDespesaFixaItem, "
                  + "itpr.percentual_lucro_necessario_item AS PercentualLucroNecessarioItem, "
                  + "itpr.preco_lista_venda_item AS PrecoListaVendaItem, "
                  + "itpr.desconto_final_item AS DescontoFinalItem, "
                  + "itpr.preco_unitario_final_item AS PrecoUnitarioFinalItem, "
                  + "itpr.preco_total_final_item AS PrecoTotalFinalItem, "
                  + "itpr.quantidade_estoque_codigo_completo_item AS QuantidadeEstoqueCodigoCompletoItem, "
                  + "itpr.quantidade_estoque_codigo_abreviado_item AS QuantidadeEstoqueCodigoAbreviadoItem, "
                  + "itpr.informacao_estoque_codigo_completo_item AS InformacaoEstoqueCodigoCompletoItem, "
                  + "itpr.informacao_estoque_codigo_abreviado_item AS InformacaoEstoqueCodigoAbreviadoItem, "
                  + "itpr.prazo_final_item AS PrazoFinalItem, "
                  + "itpr.comentarios_item AS ComentariosItem, "
                  + "itpr.data_aprovacao_item AS DataAprovacaoItem, "
                  + "itpr.data_edicao_item AS DataEdicaoItem, "
                  + "itpr.id_usuario AS idUsuario, "
                  + "itpr.id_status AS idStatus, "
                  + "itpr.id_proposta AS idProposta, "
                  + "itpr.id_status_aprovacao AS idStatusAprovacao, "
                  + "itpr.id_justificativa_aprovacao AS idJustificativaAprovacao, "
                  + "itpr.id_tipo_item AS idTipoItem, "
                  + "itpr.id_conjunto AS idConjunto, "
                  + "itpr.id_especificacao AS idEspecificacao, "
                  + "itpr.id_fornecedor AS idFornecedor, "
                  + "itpr.id_tipo_substituicao_tributaria_item AS idTipoSubstituicaoTributaria, "
                  + "itpr.id_origem_item AS idOrigem "
                  + "FROM tb_itens_propostas AS itpr "
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
                                ItemProposta item = new();

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
                                if (retornaProposta)
                                {
                                    if (item.Proposta == null)
                                    {
                                        item.Proposta = new();
                                    }
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.StatusAprovacao == null)
                                {
                                    item.StatusAprovacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.JustificativaAprovacao == null)
                                {
                                    item.JustificativaAprovacao = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.TipoItem == null)
                                {
                                    item.TipoItem = new();
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

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Fornecedor == null)
                                {
                                    item.Fornecedor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.TipoSubstituicaoTributaria == null)
                                {
                                    item.TipoSubstituicaoTributaria = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Origem == null)
                                {
                                    item.Origem = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                item.DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                item.CodigoItem = FuncoesDeConversao.ConverteParaString(reader["CodigoItem"]);
                                item.DescricaoItem = FuncoesDeConversao.ConverteParaString(reader["DescricaoItem"]);
                                item.QuantidadeItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeItem"]);
                                item.PrecoUnitarioInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioInicialItem"]);
                                item.PercentualIpiItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualIpiItem"]);
                                item.PercentualIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualIcmsItem"]);
                                item.NcmItem = FuncoesDeConversao.ConverteParaString(reader["NcmItem"]);
                                item.MvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["MvaItem"]);
                                item.ValorStItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorStItem"]);
                                item.ValorTotalInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorTotalInicialItem"]);
                                item.PrazoInicialItem = FuncoesDeConversao.ConverteParaString(reader["PrazoInicialItem"]);
                                item.FreteUnitarioItem = FuncoesDeConversao.ConverteParaDecimal(reader["FreteUnitarioItem"]);
                                item.DescontoInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["DescontoInicialItem"]);
                                item.PrecoAposDescontoInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoAposDescontoInicialItem"]);
                                item.PrecoComIpiEIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoComIpiEIcmsItem"]);
                                item.PercentualAliquotaExternaIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaExternaIcmsItem"]);
                                item.ValorIcmsCreditoItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorIcmsCreditoItem"]);
                                item.PercentualMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualMvaItem"]);
                                item.ValorMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorMvaItem"]);
                                item.PrecoComMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoComMvaItem"]);
                                item.PercentualAliquotaInternaIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaInternaIcmsItem"]);
                                item.ValorIcmsDebitoItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorIcmsDebitoItem"]);
                                item.SaldoIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["SaldoIcmsItem"]);
                                item.PrecoUnitarioParaRevendedorItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioParaRevendedorItem"]);
                                item.ImpostosFederaisItem = FuncoesDeConversao.ConverteParaDecimal(reader["ImpostosFederaisItem"]);
                                item.RateioDespesaFixaItem = FuncoesDeConversao.ConverteParaDecimal(reader["RateioDespesaFixaItem"]);
                                item.PercentualLucroNecessarioItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItem"]);
                                item.PrecoListaVendaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoListaVendaItem"]);
                                item.DescontoFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["DescontoFinalItem"]);
                                item.PrecoUnitarioFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioFinalItem"]);
                                item.PrecoTotalFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoTotalFinalItem"]);
                                item.QuantidadeEstoqueCodigoCompletoItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeEstoqueCodigoCompletoItem"]);
                                item.QuantidadeEstoqueCodigoAbreviadoItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeEstoqueCodigoAbreviadoItem"]);
                                item.InformacaoEstoqueCodigoCompletoItem = FuncoesDeConversao.ConverteParaString(reader["InformacaoEstoqueCodigoCompletoItem"]);
                                item.InformacaoEstoqueCodigoAbreviadoItem = FuncoesDeConversao.ConverteParaString(reader["InformacaoEstoqueCodigoAbreviadoItem"]);
                                item.PrazoFinalItem = FuncoesDeConversao.ConverteParaString(reader["PrazoFinalItem"]);
                                item.ComentariosItem = FuncoesDeConversao.ConverteParaString(reader["ComentariosItem"]);
                                item.DataAprovacaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataAprovacaoItem"]);
                                item.DataEdicaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicaoItem"]);

                                item.Usuario.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuario"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);
                                item.StatusAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatusAprovacao"]);
                                item.JustificativaAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idJustificativaAprovacao"]);
                                item.TipoItem.Id = FuncoesDeConversao.ConverteParaInt(reader["idTipoItem"]);
                                item.Conjunto.Id = FuncoesDeConversao.ConverteParaInt(reader["idConjunto"]);
                                item.Especificacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idEspecificacao"]);
                                item.Fornecedor.Id = FuncoesDeConversao.ConverteParaInt(reader["idFornecedor"]);
                                item.TipoSubstituicaoTributaria.Id = FuncoesDeConversao.ConverteParaInt(reader["idTipoSubstituicaoTributaria"]);
                                item.Origem.Id = FuncoesDeConversao.ConverteParaInt(reader["idOrigem"]);

                                if (retornaProposta)
                                {
                                    item.Proposta.Id = FuncoesDeConversao.ConverteParaInt(reader["idProposta"]);
                                }

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaItensProposta.Add(item);

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

        public async Task CalculaValoresItemPropostaAsync(Cliente cliente)
        {
            Taxa taxa = new();
            await taxa.GetTaxaDatabaseAsync(CancellationToken.None, "WHERE data_fim IS NULL AND data_inicio = (SELECT MAX(data_inicio) FROM tb_taxas WHERE data_fim IS NULL)", "");

            decimal taxaPercentualAliquotaExternaICMSItem;
            decimal taxaPercentualMVAItem;
            decimal taxaPercentualAliquotaInternaICMSItem;
            decimal taxaImpostosFederaisItem;
            decimal taxaRateioDespesaFixaItem;
            decimal taxaPercentualLucroNecessarioItem;
            decimal taxaPercentualAcrescimoItem;

            if (Origem?.Id == 1)
            {
                taxaPercentualAliquotaExternaICMSItem = Convert.ToDecimal(taxa.PercentualAliquotaExternaIcmsItemNacional);
                taxaPercentualMVAItem = Convert.ToDecimal(taxa.PercentualMvaItemNacional);
                taxaPercentualAliquotaInternaICMSItem = Convert.ToDecimal(taxa.PercentualAliquotaInternaIcmsItemNacional);
                taxaImpostosFederaisItem = Convert.ToDecimal(taxa.ImpostosFederaisItemNacional);
                taxaRateioDespesaFixaItem = Convert.ToDecimal(taxa.RateioDespesaFixaItemNacional);
                if (TipoSubstituicaoTributaria?.Id == 1)
                {
                    taxaPercentualLucroNecessarioItem = Convert.ToDecimal(taxa.PercentualLucroNecessarioItemRevendaStNacional);
                }
                else
                {
                    taxaPercentualLucroNecessarioItem = Convert.ToDecimal(taxa.PercentualLucroNecessarioItemOutrosNacional);
                }
                taxaPercentualAcrescimoItem = Convert.ToDecimal(taxa.PercentualAcrescimoItemNacional);
            }
            else
            {
                taxaPercentualAliquotaExternaICMSItem = Convert.ToDecimal(taxa.PercentualAliquotaExternaIcmsItemImportado);
                taxaPercentualMVAItem = Convert.ToDecimal(taxa.PercentualMvaItemImportado);
                taxaPercentualAliquotaInternaICMSItem = Convert.ToDecimal(taxa.PercentualAliquotaInternaIcmsItemImportado);
                taxaImpostosFederaisItem = Convert.ToDecimal(taxa.ImpostosFederaisItemImportado);
                taxaRateioDespesaFixaItem = Convert.ToDecimal(taxa.RateioDespesaFixaItemImportado);
                if (TipoSubstituicaoTributaria?.Id == 1)
                {
                    taxaPercentualLucroNecessarioItem = Convert.ToDecimal(taxa.PercentualLucroNecessarioItemRevendaStImportado);
                }
                else
                {
                    taxaPercentualLucroNecessarioItem = Convert.ToDecimal(taxa.PercentualLucroNecessarioItemOutrosImportado);
                }
                taxaPercentualAcrescimoItem = Convert.ToDecimal(taxa.PercentualAcrescimoItemImportado);
            }

            if (Fornecedor?.Id == 1)
            {
                PrecoAposDescontoInicialItem = Convert.ToDecimal(PrecoUnitarioInicialItem) * (Convert.ToDecimal(1.0) + taxaPercentualAcrescimoItem) * (Convert.ToDecimal(1.0) - Convert.ToDecimal(DescontoInicialItem));
            }
            else
            {
                PrecoAposDescontoInicialItem = Convert.ToDecimal(PrecoUnitarioInicialItem) * (Convert.ToDecimal(1.0) - Convert.ToDecimal(DescontoInicialItem));
            }

            PrecoComIpiEIcmsItem = Convert.ToDecimal(PrecoAposDescontoInicialItem) * (Convert.ToDecimal(1.0) + Convert.ToDecimal(PercentualIpiItem));
            PercentualAliquotaExternaIcmsItem = taxaPercentualAliquotaExternaICMSItem;
            PercentualAliquotaInternaIcmsItem = taxaPercentualAliquotaInternaICMSItem;

            ImpostosFederaisItem = taxaImpostosFederaisItem;
            RateioDespesaFixaItem = taxaRateioDespesaFixaItem;
            PercentualLucroNecessarioItem = taxaPercentualLucroNecessarioItem;

            if (TipoSubstituicaoTributaria?.Id == 1)
            {
                ValorIcmsCreditoItem = Convert.ToDecimal(PrecoAposDescontoInicialItem) * Convert.ToDecimal(PercentualAliquotaExternaIcmsItem);
                PercentualMvaItem = taxaPercentualMVAItem;
                ValorMvaItem = Convert.ToDecimal(PrecoComIpiEIcmsItem) * Convert.ToDecimal(PercentualMvaItem);
                PrecoComMvaItem = Convert.ToDecimal(PrecoComIpiEIcmsItem) + Convert.ToDecimal(ValorMvaItem);
                ValorIcmsDebitoItem = Convert.ToDecimal(PrecoComMvaItem) * Convert.ToDecimal(PercentualAliquotaInternaIcmsItem);
                SaldoIcmsItem = Convert.ToDecimal(ValorIcmsDebitoItem) - Convert.ToDecimal(ValorIcmsCreditoItem);
                PrecoUnitarioParaRevendedorItem = Convert.ToDecimal(PrecoComIpiEIcmsItem) + Convert.ToDecimal(SaldoIcmsItem);
                PrecoListaVendaItem = FuncoesMatematicas.DividirPorZeroDecimal(Convert.ToDecimal(PrecoUnitarioParaRevendedorItem),
                    Convert.ToDecimal(1.0) - Convert.ToDecimal(PercentualLucroNecessarioItem) - Convert.ToDecimal(ImpostosFederaisItem) - Convert.ToDecimal(RateioDespesaFixaItem));
            }
            else
            {
                ValorIcmsCreditoItem = 0;
                PercentualMvaItem = 0;
                ValorMvaItem = 0;
                PrecoComMvaItem = Convert.ToDecimal(PrecoComIpiEIcmsItem) + Convert.ToDecimal(ValorMvaItem);
                ValorIcmsDebitoItem = Convert.ToDecimal(PrecoComIpiEIcmsItem) * (Convert.ToDecimal(PercentualAliquotaInternaIcmsItem) - Convert.ToDecimal(PercentualAliquotaExternaIcmsItem));
                SaldoIcmsItem = Convert.ToDecimal(ValorIcmsDebitoItem);
                PrecoUnitarioParaRevendedorItem = Convert.ToDecimal(PrecoComIpiEIcmsItem);
                PrecoListaVendaItem = FuncoesMatematicas.DividirPorZeroDecimal(Convert.ToDecimal(PrecoUnitarioParaRevendedorItem) - Convert.ToDecimal(PrecoAposDescontoInicialItem) * Convert.ToDecimal(PercentualAliquotaInternaIcmsItem),
                    Convert.ToDecimal(1.0) - Convert.ToDecimal(PercentualLucroNecessarioItem) - Convert.ToDecimal(ImpostosFederaisItem) - Convert.ToDecimal(PercentualAliquotaInternaIcmsItem) - Convert.ToDecimal(RateioDespesaFixaItem));
            }

            if (TipoSubstituicaoTributaria?.Id == 3 || TipoItem?.Id != 1)
            {
                if ((bool)cliente.ConsiderarAcrescimoEspecifico)
                {
                    PrecoUnitarioFinalItem = Convert.ToDecimal(PrecoUnitarioInicialItem) * (Convert.ToDecimal(1.0) - Convert.ToDecimal(DescontoFinalItem)) * (Convert.ToDecimal(1.0) + Convert.ToDecimal(cliente.PercentualAcrescimoEspecifico));
                }
                else
                {
                    PrecoUnitarioFinalItem = Convert.ToDecimal(PrecoUnitarioInicialItem) * (Convert.ToDecimal(1.0) - Convert.ToDecimal(DescontoFinalItem));
                }
            }
            else
            {
                if (Fornecedor?.Id == 1 && (bool)cliente.ConsiderarPercentuaisTabelaKion)
                {
                    if ((bool)cliente.ConsiderarAcrescimoEspecifico)
                    {
                        PrecoUnitarioFinalItem = ((FuncoesMatematicas.DividirPorZeroDecimal(FuncoesMatematicas.DividirPorZeroDecimal((Convert.ToDecimal(PrecoUnitarioInicialItem) + (Convert.ToDecimal(PrecoUnitarioInicialItem) * taxaPercentualAcrescimoItem)) *
                            (Convert.ToDecimal(1.0) - Convert.ToDecimal(DescontoInicialItem)), Convert.ToDecimal(cliente.PercentualTabelaKion1)) * Convert.ToDecimal(cliente.PercentualTabelaKion2), Convert.ToDecimal(cliente.PercentualTabelaKion3)) +
                            (FuncoesMatematicas.DividirPorZeroDecimal(FuncoesMatematicas.DividirPorZeroDecimal((Convert.ToDecimal(PrecoUnitarioInicialItem) + (Convert.ToDecimal(PrecoUnitarioInicialItem) * taxaPercentualAcrescimoItem)) *
                            (Convert.ToDecimal(1.0) - Convert.ToDecimal(DescontoInicialItem)), Convert.ToDecimal(cliente.PercentualTabelaKion1)) * Convert.ToDecimal(cliente.PercentualTabelaKion2), Convert.ToDecimal(cliente.PercentualTabelaKion3)) *
                            Convert.ToDecimal(cliente.PercentualAcrescimoEspecifico))) * (Convert.ToDecimal(1.0) - Convert.ToDecimal(DescontoFinalItem))) + Convert.ToDecimal(FreteUnitarioItem);
                    }
                    else
                    {
                        PrecoUnitarioFinalItem = ((FuncoesMatematicas.DividirPorZeroDecimal(FuncoesMatematicas.DividirPorZeroDecimal((Convert.ToDecimal(PrecoUnitarioInicialItem) + (Convert.ToDecimal(PrecoUnitarioInicialItem) * taxaPercentualAcrescimoItem)) *
                            (Convert.ToDecimal(1.0) - Convert.ToDecimal(DescontoInicialItem)), Convert.ToDecimal(cliente.PercentualTabelaKion1)) * Convert.ToDecimal(cliente.PercentualTabelaKion2), Convert.ToDecimal(cliente.PercentualTabelaKion3)) +
                            (FuncoesMatematicas.DividirPorZeroDecimal(FuncoesMatematicas.DividirPorZeroDecimal((Convert.ToDecimal(PrecoUnitarioInicialItem) + (Convert.ToDecimal(PrecoUnitarioInicialItem) * taxaPercentualAcrescimoItem)) *
                            (Convert.ToDecimal(1.0) - Convert.ToDecimal(DescontoInicialItem)), Convert.ToDecimal(cliente.PercentualTabelaKion1)) * Convert.ToDecimal(cliente.PercentualTabelaKion2), Convert.ToDecimal(cliente.PercentualTabelaKion3)) * Convert.ToDecimal(0.0))) *
                            (Convert.ToDecimal(1.0) - Convert.ToDecimal(DescontoFinalItem))) + Convert.ToDecimal(FreteUnitarioItem);
                    }
                }
                else
                {
                    if ((bool)cliente.ConsiderarAcrescimoEspecifico)
                    {
                        PrecoUnitarioFinalItem = (Convert.ToDecimal(PrecoListaVendaItem) * (Convert.ToDecimal(1.0) - Convert.ToDecimal(DescontoFinalItem)) * (Convert.ToDecimal(1.0) + Convert.ToDecimal(cliente.PercentualAcrescimoEspecifico))) + Convert.ToDecimal(FreteUnitarioItem);
                    }
                    else
                    {
                        PrecoUnitarioFinalItem = Convert.ToDecimal(PrecoListaVendaItem) * (Convert.ToDecimal(1.0) - Convert.ToDecimal(DescontoFinalItem)) + Convert.ToDecimal(FreteUnitarioItem);
                    }
                }
            }

            PrecoTotalFinalItem = Convert.ToDecimal(PrecoUnitarioFinalItem) * Convert.ToDecimal(QuantidadeItem);

            if (TipoItem?.Id != 1)
            {
                PrecoAposDescontoInicialItem = null;
                PrecoComIpiEIcmsItem = null;
                PercentualAliquotaExternaIcmsItem = null;
                PercentualAliquotaInternaIcmsItem = null;
                ImpostosFederaisItem = null;
                RateioDespesaFixaItem = null;
                PercentualLucroNecessarioItem = null;
                ValorIcmsCreditoItem = null;
                PercentualMvaItem = null;
                ValorMvaItem = null;
                PrecoComMvaItem = null;
                ValorIcmsDebitoItem = null;
                SaldoIcmsItem = null;
                PrecoUnitarioParaRevendedorItem = null;
                PrecoListaVendaItem = null;
                PrecoUnitarioFinalItem = null;
                PrecoTotalFinalItem = null;
                PrecoUnitarioFinalItem = Convert.ToDecimal(PrecoUnitarioInicialItem) - (Convert.ToDecimal(PrecoUnitarioInicialItem) * Convert.ToDecimal(DescontoFinalItem));
                PrecoTotalFinalItem = Convert.ToDecimal(PrecoUnitarioInicialItem) - (Convert.ToDecimal(PrecoUnitarioInicialItem) * Convert.ToDecimal(DescontoFinalItem));
            }
        }

        /// <summary>
        /// Método assíncrono que retorna o último valor pago de deslocamento
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetUltimoValorDeslocamento(int? idCliente, CancellationToken ct)
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
                                          + "itpr.preco_unitario_inicial_item AS PrecoUnitarioInicialItem "
                                          + "FROM tb_itens_propostas AS itpr "
                                          + "LEFT JOIN tb_propostas AS prop ON itpr.id_proposta = prop.id_proposta "
                                          + "WHERE itpr.id_tipo_item = 3 AND prop.id_cliente = @id_cliente "
                                          + "ORDER BY itpr.data_insercao DESC "
                                          + "LIMIT 1";

                    command.Parameters.AddWithValue("@id_cliente", idCliente);

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
                                PrecoUnitarioInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioInicialItem"]);
                            }
                        }
                    }
                }
            }
        }

        #endregion Métodos

        #region PreencheListaComentado

        ///// <summary>
        ///// Método assíncrono que preenche uma lista de itens da proposta com os argumentos utilizados
        ///// </summary>
        ///// <param name="listaItensProposta">Representa a lista de itens da proposta que deseja preencher</param>
        ///// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        ///// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        ///// <param name="ct">Token de cancelamento</param>
        ///// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        ///// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        ///// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        //public static async Task PreencheListaItensPropostaAsync(ObservableCollection<ItemProposta> listaItensProposta, bool limparLista, bool retornaProposta, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        //{
        //    // Lança exceção de cancelamento caso ela tenha sido efetuada
        //    ct.ThrowIfCancellationRequested();

        //    // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
        //    if (listaItensProposta == null)
        //    {
        //        listaItensProposta = new();
        //    }

        //    // Limpa a lista caso verdadeiro
        //    if (limparLista)
        //    {
        //        listaItensProposta.Clear();
        //    }

        //    // Utilização da conexão
        //    using (var db = new ConexaoMySQL())
        //    {
        //        // Tenta abrir a conexão e retorna um erro caso não consiga
        //        try
        //        {
        //            // Abre a conexão
        //            await db.AbreConexaoAsync();

        //        }
        //        catch (MySqlConnector.MySqlException)
        //        {
        //            throw;
        //        }

        //        // Lança exceção de cancelamento caso ela tenha sido efetuada
        //        ct.ThrowIfCancellationRequested();

        //        // Define o comando
        //        string comando = "SELECT "
        //          + "itpr.id_item_proposta AS Id, "
        //          + "itpr.data_insercao AS DataInsercao, "
        //          + "itpr.codigo_item AS CodigoItem, "
        //          + "itpr.descricao_item AS DescricaoItem, "
        //          + "itpr.quantidade_item AS QuantidadeItem, "
        //          + "itpr.preco_unitario_inicial_item AS PrecoUnitarioInicialItem, "
        //          + "itpr.percentual_ipi_item AS PercentualIpiItem, "
        //          + "itpr.percentual_icms_item AS PercentualIcmsItem, "
        //          + "itpr.ncm_item AS NcmItem, "
        //          + "itpr.mva_item AS MvaItem, "
        //          + "itpr.valor_st_item AS ValorStItem, "
        //          + "itpr.valor_total_inicial_item AS ValorTotalInicialItem, "
        //          + "itpr.prazo_inicial_item AS PrazoInicialItem, "
        //          + "itpr.frete_unitario_item AS FreteUnitarioItem, "
        //          + "itpr.desconto_inicial_item AS DescontoInicialItem, "
        //          + "itpr.preco_apos_desconto_inicial_item AS PrecoAposDescontoInicialItem, "
        //          + "itpr.preco_com_ipi_e_icms_item AS PrecoComIpiEIcmsItem, "
        //          + "itpr.percentual_aliquota_externa_icms_item AS PercentualAliquotaExternaIcmsItem, "
        //          + "itpr.valor_icms_credito_item AS ValorIcmsCreditoItem, "
        //          + "itpr.percentual_mva_item AS PercentualMvaItem, "
        //          + "itpr.valor_mva_item AS ValorMvaItem, "
        //          + "itpr.preco_com_mva_item AS PrecoComMvaItem, "
        //          + "itpr.percentual_aliquota_interna_icms_item AS PercentualAliquotaInternaIcmsItem, "
        //          + "itpr.valor_icms_debito_item AS ValorIcmsDebitoItem, "
        //          + "itpr.saldo_icms_item AS SaldoIcmsItem, "
        //          + "itpr.preco_unitario_para_revendedor_item AS PrecoUnitarioParaRevendedorItem, "
        //          + "itpr.impostos_federais_item AS ImpostosFederaisItem, "
        //          + "itpr.rateio_despesa_fixa_item AS RateioDespesaFixaItem, "
        //          + "itpr.percentual_lucro_necessario_item AS PercentualLucroNecessarioItem, "
        //          + "itpr.preco_lista_venda_item AS PrecoListaVendaItem, "
        //          + "itpr.desconto_final_item AS DescontoFinalItem, "
        //          + "itpr.preco_unitario_final_item AS PrecoUnitarioFinalItem, "
        //          + "itpr.preco_total_final_item AS PrecoTotalFinalItem, "
        //          + "itpr.quantidade_estoque_codigo_completo_item AS QuantidadeEstoqueCodigoCompletoItem, "
        //          + "itpr.quantidade_estoque_codigo_abreviado_item AS QuantidadeEstoqueCodigoAbreviadoItem, "
        //          + "itpr.informacao_estoque_codigo_completo_item AS InformacaoEstoqueCodigoCompletoItem, "
        //          + "itpr.informacao_estoque_codigo_abreviado_item AS InformacaoEstoqueCodigoAbreviadoItem, "
        //          + "itpr.prazo_final_item AS PrazoFinalItem, "
        //          + "itpr.comentarios_item AS ComentariosItem, "
        //          + "itpr.data_aprovacao_item AS DataAprovacaoItem, "
        //          + "itpr.data_edicao_item AS DataEdicaoItem, "
        //          + "itpr.id_usuario AS idUsuario, "
        //          + "itpr.id_status AS idStatus, "
        //          + "itpr.id_proposta AS idProposta, "
        //          + "itpr.id_status_aprovacao AS idStatusAprovacao, "
        //          + "itpr.id_justificativa_aprovacao AS idJustificativaAprovacao, "
        //          + "itpr.id_tipo_item AS idTipoItem, "
        //          + "itpr.id_conjunto AS idConjunto, "
        //          + "itpr.id_especificacao AS idEspecificacao, "
        //          + "itpr.id_fornecedor AS idFornecedor, "
        //          + "itpr.id_tipo_substituicao_tributaria_item AS idTipoSubstituicaoTributaria, "
        //          + "itpr.id_origem_item AS idOrigem "
        //          + "FROM tb_itens_propostas AS itpr "
        //          + condicoesExtras;

        //        // Cria e atribui a variável do total de linhas através da função específica para contagem de linhas
        //        int totalLinhas = await FuncoesDeDatabase.GetQuantidadeLinhasReader(db, comando, ct, nomesParametrosSeparadosPorVirgulas, valoresParametros);

        //        // Lança exceção de cancelamento caso ela tenha sido efetuada
        //        ct.ThrowIfCancellationRequested();

        //        // Utilização do comando
        //        using (var command = db.conexao.CreateCommand())
        //        {
        //            // Definição do tipo, texto e parâmetros do comando
        //            command.CommandType = System.Data.CommandType.Text;
        //            command.CommandText = comando;

        //            // Cria uma array com os parâmetros passados utilizando vírgula como delimitador
        //            string[] nomesParametros = nomesParametrosSeparadosPorVirgulas.Split(",");

        //            // Cria um contador para retornar o nome do parametro corretamente
        //            int contadorParametros = 0;

        //            // Varre o array de parâmetros adicionando-os à consulta
        //            foreach (var item in valoresParametros)
        //            {
        //                command.Parameters.AddWithValue(nomesParametros[contadorParametros].Trim(), item);
        //                contadorParametros++;
        //            }

        //            // Utilização do reader para retornar os dados asíncronos
        //            using (var reader = await command.ExecuteReaderAsync(ct))
        //            {
        //                // Verifica se o reader possui linhas
        //                if (reader.HasRows)
        //                {
        //                    // Cria e atribui a variável de contagem de linhas
        //                    int linhaAtual = 0;

        //                    // Enquanto o reader possuir linhas, define os valores
        //                    while (await reader.ReadAsync(ct))
        //                    {
        //                        // Lança exceção de cancelamento caso ela tenha sido efetuada
        //                        ct.ThrowIfCancellationRequested();

        //                        // Cria um novo item e atribui os valores
        //                        ItemProposta item = new();

        //                        // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
        //                        if (item.Status == null)
        //                        {
        //                            item.Status = new();
        //                        }

        //                        // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
        //                        if (item.Usuario == null)
        //                        {
        //                            item.Usuario = new();
        //                        }

        //                        // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
        //                        if (retornaProposta)
        //                        {
        //                            if (item.Proposta == null)
        //                            {
        //                                item.Proposta = new();
        //                            }
        //                        }

        //                        // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
        //                        if (item.StatusAprovacao == null)
        //                        {
        //                            item.StatusAprovacao = new();
        //                        }

        //                        // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
        //                        if (item.JustificativaAprovacao == null)
        //                        {
        //                            item.JustificativaAprovacao = new();
        //                        }

        //                        // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
        //                        if (item.TipoItem == null)
        //                        {
        //                            item.TipoItem = new();
        //                        }

        //                        // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
        //                        if (item.Conjunto == null)
        //                        {
        //                            item.Conjunto = new();
        //                        }

        //                        // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
        //                        if (item.Especificacao == null)
        //                        {
        //                            item.Especificacao = new();
        //                        }

        //                        // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
        //                        if (item.Fornecedor == null)
        //                        {
        //                            item.Fornecedor = new();
        //                        }

        //                        // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
        //                        if (item.TipoSubstituicaoTributaria == null)
        //                        {
        //                            item.TipoSubstituicaoTributaria = new();
        //                        }

        //                        // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
        //                        if (item.Origem == null)
        //                        {
        //                            item.Origem = new();
        //                        }

        //                        // Define as propriedades
        //                        item.Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
        //                        item.DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
        //                        item.CodigoItem = FuncoesDeConversao.ConverteParaString(reader["CodigoItem"]);
        //                        item.DescricaoItem = FuncoesDeConversao.ConverteParaString(reader["DescricaoItem"]);
        //                        item.QuantidadeItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeItem"]);
        //                        item.PrecoUnitarioInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioInicialItem"]);
        //                        item.PercentualIpiItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualIpiItem"]);
        //                        item.PercentualIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualIcmsItem"]);
        //                        item.NcmItem = FuncoesDeConversao.ConverteParaString(reader["NcmItem"]);
        //                        item.MvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["MvaItem"]);
        //                        item.ValorStItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorStItem"]);
        //                        item.ValorTotalInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorTotalInicialItem"]);
        //                        item.PrazoInicialItem = FuncoesDeConversao.ConverteParaString(reader["PrazoInicialItem"]);
        //                        item.FreteUnitarioItem = FuncoesDeConversao.ConverteParaDecimal(reader["FreteUnitarioItem"]);
        //                        item.DescontoInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["DescontoInicialItem"]);
        //                        item.PrecoAposDescontoInicialItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoAposDescontoInicialItem"]);
        //                        item.PrecoComIpiEIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoComIpiEIcmsItem"]);
        //                        item.PercentualAliquotaExternaIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaExternaIcmsItem"]);
        //                        item.ValorIcmsCreditoItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorIcmsCreditoItem"]);
        //                        item.PercentualMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualMvaItem"]);
        //                        item.ValorMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorMvaItem"]);
        //                        item.PrecoComMvaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoComMvaItem"]);
        //                        item.PercentualAliquotaInternaIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaInternaIcmsItem"]);
        //                        item.ValorIcmsDebitoItem = FuncoesDeConversao.ConverteParaDecimal(reader["ValorIcmsDebitoItem"]);
        //                        item.SaldoIcmsItem = FuncoesDeConversao.ConverteParaDecimal(reader["SaldoIcmsItem"]);
        //                        item.PrecoUnitarioParaRevendedorItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioParaRevendedorItem"]);
        //                        item.ImpostosFederaisItem = FuncoesDeConversao.ConverteParaDecimal(reader["ImpostosFederaisItem"]);
        //                        item.RateioDespesaFixaItem = FuncoesDeConversao.ConverteParaDecimal(reader["RateioDespesaFixaItem"]);
        //                        item.PercentualLucroNecessarioItem = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItem"]);
        //                        item.PrecoListaVendaItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoListaVendaItem"]);
        //                        item.DescontoFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["DescontoFinalItem"]);
        //                        item.PrecoUnitarioFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoUnitarioFinalItem"]);
        //                        item.PrecoTotalFinalItem = FuncoesDeConversao.ConverteParaDecimal(reader["PrecoTotalFinalItem"]);
        //                        item.QuantidadeEstoqueCodigoCompletoItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeEstoqueCodigoCompletoItem"]);
        //                        item.QuantidadeEstoqueCodigoAbreviadoItem = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeEstoqueCodigoAbreviadoItem"]);
        //                        item.InformacaoEstoqueCodigoCompletoItem = FuncoesDeConversao.ConverteParaString(reader["InformacaoEstoqueCodigoCompletoItem"]);
        //                        item.InformacaoEstoqueCodigoAbreviadoItem = FuncoesDeConversao.ConverteParaString(reader["InformacaoEstoqueCodigoAbreviadoItem"]);
        //                        item.PrazoFinalItem = FuncoesDeConversao.ConverteParaString(reader["PrazoFinalItem"]);
        //                        item.ComentariosItem = FuncoesDeConversao.ConverteParaString(reader["ComentariosItem"]);
        //                        item.DataAprovacaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataAprovacaoItem"]);
        //                        item.DataEdicaoItem = FuncoesDeConversao.ConverteParaDateTime(reader["DataEdicaoItem"]);

        //                        item.Usuario.Id = FuncoesDeConversao.ConverteParaInt(reader["idUsuario"]);
        //                        item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatus"]);
        //                        item.StatusAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idStatusAprovacao"]);
        //                        item.JustificativaAprovacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idJustificativaAprovacao"]);
        //                        item.TipoItem.Id = FuncoesDeConversao.ConverteParaInt(reader["idTipoItem"]);
        //                        item.Conjunto.Id = FuncoesDeConversao.ConverteParaInt(reader["idConjunto"]);
        //                        item.Especificacao.Id = FuncoesDeConversao.ConverteParaInt(reader["idEspecificacao"]);
        //                        item.Fornecedor.Id = FuncoesDeConversao.ConverteParaInt(reader["idFornecedor"]);
        //                        item.TipoSubstituicaoTributaria.Id = FuncoesDeConversao.ConverteParaInt(reader["idTipoSubstituicaoTributaria"]);
        //                        item.Origem.Id = FuncoesDeConversao.ConverteParaInt(reader["idOrigem"]);

        //                        if (retornaProposta)
        //                        {
        //                            item.Proposta.Id = FuncoesDeConversao.ConverteParaInt(reader["idProposta"]);
        //                        }

        //                        // Lança exceção de cancelamento caso ela tenha sido efetuada
        //                        ct.ThrowIfCancellationRequested();

        //                        // Adiciona o item à coleção
        //                        listaItensProposta.Add(item);

        //                        // Incrementa a linha atual
        //                        linhaAtual++;

        //                        // Reporta o progresso se o progresso não for nulo
        //                        if (reportadorProgresso != null)
        //                        {
        //                            reportadorProgresso.Report((double)linhaAtual / (double)totalLinhas * (double)100);
        //                        }

        //                        // Lança exceção de cancelamento caso ela tenha sido efetuada
        //                        ct.ThrowIfCancellationRequested();
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    int totalLinhasLista = listaItensProposta.Count;
        //    int linhaAtualLista = 0;

        //    foreach (ItemProposta item in listaItensProposta)
        //    {
        //        // Lança exceção de cancelamento caso ela tenha sido efetuada
        //        ct.ThrowIfCancellationRequested();

        //        // Define as classes
        //        await item.Usuario.GetUsuarioDatabaseAsync(item.Usuario.Id, ct);
        //        await item.Status.GetStatusDatabaseAsync(item.Status.Id, ct);
        //        await item.StatusAprovacao.GetStatusAprovacaoDatabaseAsync(item.StatusAprovacao.Id, ct);
        //        await item.JustificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(item.JustificativaAprovacao.Id, ct);
        //        await item.TipoItem.GetTipoItemDatabaseAsync(item.TipoItem.Id, ct);
        //        await item.Conjunto.GetConjuntoDatabaseAsync(item.Conjunto.Id, ct);
        //        await item.Especificacao.GetEspecificacaoDatabaseAsync(item.Especificacao.Id, ct);
        //        await item.Fornecedor.GetFornecedorDatabaseAsync(item.Fornecedor.Id, ct);
        //        await item.TipoSubstituicaoTributaria.GetTipoSubstituicaoTributariaDatabaseAsync(item.TipoSubstituicaoTributaria.Id, ct);
        //        await item.Origem.GetOrigemDatabaseAsync(item.Origem.Id, ct);

        //        // Caso verdadeiro, a classe Proposta será retornada. Classes subsequentes não serão retornadas para evitar estouro de carga
        //        if (retornaProposta)
        //        {
        //            await item.Proposta.GetPropostaDatabaseAsync(item.Proposta.Id, ct);
        //        }

        //        // Incrementa a linha atual
        //        linhaAtualLista++;

        //        // Reporta o progresso se o progresso não for nulo
        //        if (reportadorProgresso != null)
        //        {
        //            reportadorProgresso.Report(linhaAtualLista / totalLinhasLista * 100);
        //        }
        //    }
        //}

        #endregion PreencheListaComentado

        #region Interfaces

        /// <summary>
        /// Método para criar uma cópia da classe já que esse é um tipo de referência que não pode ser atribuído diretamente
        /// </summary>
        public object Clone()
        {
            ItemProposta itemPropostaCopia = new();

            itemPropostaCopia.Id = Id;
            itemPropostaCopia.DataInsercao = DataInsercao;
            itemPropostaCopia.CodigoItem = CodigoItem;
            itemPropostaCopia.DescricaoItem = DescricaoItem;
            itemPropostaCopia.QuantidadeItem = QuantidadeItem;
            itemPropostaCopia.PrecoUnitarioInicialItem = PrecoUnitarioInicialItem;
            itemPropostaCopia.PercentualIpiItem = PercentualIpiItem;
            itemPropostaCopia.PercentualIcmsItem = PercentualIcmsItem;
            itemPropostaCopia.NcmItem = NcmItem;
            itemPropostaCopia.MvaItem = MvaItem;
            itemPropostaCopia.ValorStItem = ValorStItem;
            itemPropostaCopia.ValorTotalInicialItem = ValorTotalInicialItem;
            itemPropostaCopia.PrazoInicialItem = PrazoInicialItem;
            itemPropostaCopia.FreteUnitarioItem = FreteUnitarioItem;
            itemPropostaCopia.DescontoInicialItem = DescontoInicialItem;
            itemPropostaCopia.PrecoAposDescontoInicialItem = PrecoAposDescontoInicialItem;
            itemPropostaCopia.PrecoComIpiEIcmsItem = PrecoComIpiEIcmsItem;
            itemPropostaCopia.PercentualAliquotaExternaIcmsItem = PercentualAliquotaExternaIcmsItem;
            itemPropostaCopia.ValorIcmsCreditoItem = ValorIcmsCreditoItem;
            itemPropostaCopia.PercentualMvaItem = PercentualMvaItem;
            itemPropostaCopia.ValorMvaItem = ValorMvaItem;
            itemPropostaCopia.PrecoComMvaItem = PrecoComMvaItem;
            itemPropostaCopia.PercentualAliquotaInternaIcmsItem = PercentualAliquotaInternaIcmsItem;
            itemPropostaCopia.ValorIcmsDebitoItem = ValorIcmsDebitoItem;
            itemPropostaCopia.SaldoIcmsItem = SaldoIcmsItem;
            itemPropostaCopia.PrecoUnitarioParaRevendedorItem = PrecoUnitarioParaRevendedorItem;
            itemPropostaCopia.ImpostosFederaisItem = ImpostosFederaisItem;
            itemPropostaCopia.RateioDespesaFixaItem = RateioDespesaFixaItem;
            itemPropostaCopia.PercentualLucroNecessarioItem = PercentualLucroNecessarioItem;
            itemPropostaCopia.PrecoListaVendaItem = PrecoListaVendaItem;
            itemPropostaCopia.DescontoFinalItem = DescontoFinalItem;
            itemPropostaCopia.PrecoUnitarioFinalItem = PrecoUnitarioFinalItem;
            itemPropostaCopia.PrecoTotalFinalItem = PrecoTotalFinalItem;
            itemPropostaCopia.QuantidadeEstoqueCodigoCompletoItem = QuantidadeEstoqueCodigoCompletoItem;
            itemPropostaCopia.QuantidadeEstoqueCodigoAbreviadoItem = QuantidadeEstoqueCodigoAbreviadoItem;
            itemPropostaCopia.InformacaoEstoqueCodigoCompletoItem = InformacaoEstoqueCodigoCompletoItem;
            itemPropostaCopia.InformacaoEstoqueCodigoAbreviadoItem = InformacaoEstoqueCodigoAbreviadoItem;
            itemPropostaCopia.PrazoFinalItem = PrazoFinalItem;
            itemPropostaCopia.ComentariosItem = ComentariosItem;
            itemPropostaCopia.DataAprovacaoItem = DataAprovacaoItem;
            itemPropostaCopia.DataEdicaoItem = DataEdicaoItem;
            itemPropostaCopia.Usuario = Usuario == null ? new() : (Usuario)Usuario.Clone();
            itemPropostaCopia.TipoItem = TipoItem == null ? new() : (TipoItem)TipoItem.Clone();
            itemPropostaCopia.Fornecedor = Fornecedor == null ? new() : (Fornecedor)Fornecedor.Clone();
            itemPropostaCopia.TipoSubstituicaoTributaria = TipoSubstituicaoTributaria == null ? new() : (TipoSubstituicaoTributaria)TipoSubstituicaoTributaria.Clone();
            itemPropostaCopia.Origem = Origem == null ? new() : (Origem)Origem.Clone();
            itemPropostaCopia.Status = Status == null ? new() : (Status)Status.Clone();
            itemPropostaCopia.StatusAprovacao = StatusAprovacao == null ? new() : (StatusAprovacao)StatusAprovacao.Clone();
            itemPropostaCopia.JustificativaAprovacao = JustificativaAprovacao == null ? new() : (JustificativaAprovacao)JustificativaAprovacao.Clone();
            itemPropostaCopia.Conjunto = Conjunto == null ? new() : (Conjunto)Conjunto.Clone();
            itemPropostaCopia.Especificacao = Especificacao == null ? new() : (Especificacao)Especificacao.Clone();

            return itemPropostaCopia;
        }

        #endregion Interfaces
    }
}