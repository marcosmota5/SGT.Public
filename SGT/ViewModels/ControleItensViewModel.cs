using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class ControleItensViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private ObservableCollection<TipoItem> _listaTiposItem = new();
        private ObservableCollection<Status> _listaStatus = new();
        private ObservableCollection<StatusAprovacao> _listaStatusAprovacao = new();
        private ObservableCollection<JustificativaAprovacao> _listaJustificativasAprovacao = new();
        private ObservableCollection<Fornecedor> _listaFornecedores = new();
        private ObservableCollection<Peca> _listaPecas = new();
        private ObservableCollection<TipoSubstituicaoTributaria> _listaTiposSubstituicaoTributaria = new();
        private ObservableCollection<Origem> _listaOrigens = new();
        private ObservableCollection<Conjunto> _listaConjuntos = new();
        private ObservableCollection<Especificacao> _listaEspecificacoes = new();
        private ItemProposta _itemProposta;
        private ItemProposta _itemPropostaInicial;
        private Peca _peca;
        private Proposta _proposta;
        private Fornecedor _fornecedor;
        private Conjunto _conjunto;
        private decimal? _quantidadeItem;
        private bool _ehNovoItem;
        private string _mensagemStatus;
        private bool _controlesHabilitados;
        private bool _carregamentoVisivel = true;

        private ICommand _comandoRetornaItens;
        private ICommand _comandoCarregaPeca;
        private ICommand _comandoSalvar;

        #endregion Campos

        #region Construtores

        /// <summary>
        /// Construtor do controle do item
        /// </summary>
        /// <param name="proposta">Proposta que terá o item inserido ou editado</param>
        /// <param name="itemProposta">Item da proposta que será inserido ou editado</param>
        /// <param name="ehNovoItem">Valor booleano informando se o item é novo ou existente</param>
        /// <param name="closeHandler">Ação para fechar a caixa de diálogo</param>
        public ControleItensViewModel(Proposta proposta, ItemProposta? itemProposta, bool ehNovoItem, Action<ControleItensViewModel> closeHandler)
        {
            // Define os itens internos como o informado no construtor
            _proposta = proposta;
            _ehNovoItem = ehNovoItem;

            // Verifica se é um novo item
            if (_ehNovoItem)
            {
                // Define o item da proposta
                ItemProposta = itemProposta;

                // Inicializa o fornecedor
                Fornecedor = new();

                // Inicializa a peça
                Peca = new();
            }
            else
            {
                // Salva o item inicial que foi passado
                _itemPropostaInicial = itemProposta;

                // Define o item da proposta a ser utilizado como uma cópia do item existente
                ItemProposta = (ItemProposta)itemProposta.Clone();
            }

            // Atribui o método de limpar listas e a ação de fechar a caixa de diálogo ao comando
            this.ComandoFechar = new SimpleCommand(o => true, o =>
            {
                LimparListas();
                closeHandler(this);
            });

            // Chama o méodo assíncrono para construir o controle de itens
            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Item da proposta";
            }
        }

        public string Icone
        {
            get
            {
                return "";
            }
        }

        public void LimparViewModel()
        {
            try
            {
                _listaTiposItem = null;
                _listaStatus = null;
                _listaStatusAprovacao = null;
                _listaJustificativasAprovacao = null;
                _listaFornecedores = null;
                _listaPecas = null;
                _listaTiposSubstituicaoTributaria = null;
                _listaOrigens = null;
                _listaConjuntos = null;
                _listaEspecificacoes = null;
                _peca = null;
                _fornecedor = null;
                _conjunto = null;

                _comandoRetornaItens = null;
                _comandoCarregaPeca = null;
                _comandoSalvar = null;
            }
            catch (Exception)
            {
            }
        }

        private CollectionView _listaEspecificacoesView;

        public CollectionView ListaEspecificacoesView
        {
            get { return _listaEspecificacoesView; }
            set
            {
                if (_listaEspecificacoesView != value)
                {
                    _listaEspecificacoesView = value;
                    OnPropertyChanged(nameof(ListaEspecificacoesView));
                }
            }
        }

        //public CollectionView ListaEspecificacoesView { get; private set; }

        public bool ExistemCamposVazios { private get; set; }

        public ICommand ComandoFechar { get; }

        public ICommand ComandoSalvar
        {
            get
            {
                if (_comandoSalvar == null)
                {
                    _comandoSalvar = new RelayCommand(
                        param => SalvarItemPropostaAsync().Await(),
                        param => true
                    );
                }

                return _comandoSalvar;
            }
        }

        public ICommand ComandoRetornaItens
        {
            get
            {
                if (_comandoRetornaItens == null)
                {
                    _comandoRetornaItens = new RelayCommand(
                        param => PreencherPecas().Await(),
                        param => true
                    );
                }
                return _comandoRetornaItens;
            }
        }

        public ICommand ComandoCarregaPeca
        {
            get
            {
                if (_comandoCarregaPeca == null)
                {
                    _comandoCarregaPeca = new RelayCommand(
                        param => CarregaPecaAsync().Await(),
                        param => true
                    );
                }
                return _comandoCarregaPeca;
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
                    ItemProposta.QuantidadeItem = value;
                    ItemProposta.ValorTotalInicialItem = ItemProposta.PrecoUnitarioInicialItem * QuantidadeItem;
                    OnPropertyChanged(nameof(QuantidadeItem));
                }
            }
        }

        public Fornecedor Fornecedor
        {
            get { return _fornecedor; }
            set
            {
                if (value != _fornecedor)
                {
                    _fornecedor = value;
                    ItemProposta.Fornecedor = value;
                    OnPropertyChanged(nameof(Fornecedor));

                    PreencherPecas().Await();
                }
            }
        }

        public Conjunto Conjunto
        {
            get { return _conjunto; }
            set
            {
                if (value != _conjunto)
                {
                    _conjunto = value;
                    ItemProposta.Conjunto = value;
                    OnPropertyChanged(nameof(Conjunto));
                    ListaEspecificacoesView.Filter = FiltraEspecificacoes;
                    CollectionViewSource.GetDefaultView(ListaEspecificacoes).Refresh();
                    OnPropertyChanged(nameof(ListaEspecificacoesView));
                }
            }
        }

        public Peca Peca
        {
            get { return _peca; }
            set
            {
                if (value != _peca)
                {
                    _peca = value;
                    ItemProposta.CodigoItem = value.CodigoItem;
                    OnPropertyChanged(nameof(Peca));

                    PreencherPecas().Await();
                }
            }
        }

        private async Task PreencherPecas()
        {
            if (ItemProposta?.TipoItem != null)
            {
                if (ItemProposta?.Fornecedor != null)
                {
                    await Peca.PreencheListaPecasAsync(ListaPecas, true, null, CancellationToken.None,
                            "WHERE itpr.id_tipo_item = @id_tipo_item AND itpr.id_fornecedor = @id_fornecedor GROUP BY itpr.codigo_item ORDER BY itpr.codigo_item ASC",
                             "id_tipo_item, id_fornecedor", ItemProposta.TipoItem.Id, ItemProposta.Fornecedor.Id);
                }
            }
        }

        public ObservableCollection<TipoItem> ListaTiposItem
        {
            get { return _listaTiposItem; }
            set
            {
                if (value != _listaTiposItem)
                {
                    _listaTiposItem = value;
                    OnPropertyChanged(nameof(ListaTiposItem));
                }
            }
        }

        public ObservableCollection<Status> ListaStatus
        {
            get { return _listaStatus; }
            set
            {
                if (value != _listaStatus)
                {
                    _listaStatus = value;
                    OnPropertyChanged(nameof(ListaStatus));
                }
            }
        }

        public ObservableCollection<StatusAprovacao> ListaStatusAprovacao
        {
            get { return _listaStatusAprovacao; }
            set
            {
                if (value != _listaStatusAprovacao)
                {
                    _listaStatusAprovacao = value;
                    OnPropertyChanged(nameof(ListaStatusAprovacao));
                }
            }
        }

        public ObservableCollection<JustificativaAprovacao> ListaJustificativasAprovacao
        {
            get { return _listaJustificativasAprovacao; }
            set
            {
                if (value != _listaJustificativasAprovacao)
                {
                    _listaJustificativasAprovacao = value;
                    OnPropertyChanged(nameof(ListaJustificativasAprovacao));
                }
            }
        }

        public ObservableCollection<Fornecedor> ListaFornecedores
        {
            get { return _listaFornecedores; }
            set
            {
                if (value != _listaFornecedores)
                {
                    _listaFornecedores = value;
                    OnPropertyChanged(nameof(ListaFornecedores));
                }
            }
        }

        public ObservableCollection<Peca> ListaPecas
        {
            get { return _listaPecas; }
            set
            {
                if (value != _listaPecas)
                {
                    _listaPecas = value;
                    OnPropertyChanged(nameof(ListaPecas));
                }
            }
        }

        public ObservableCollection<TipoSubstituicaoTributaria> ListaTiposSubstituicaoTributaria
        {
            get { return _listaTiposSubstituicaoTributaria; }
            set
            {
                if (value != _listaTiposSubstituicaoTributaria)
                {
                    _listaTiposSubstituicaoTributaria = value;
                    OnPropertyChanged(nameof(ListaTiposSubstituicaoTributaria));
                }
            }
        }

        public ObservableCollection<Origem> ListaOrigens
        {
            get { return _listaOrigens; }
            set
            {
                if (value != _listaOrigens)
                {
                    _listaOrigens = value;
                    OnPropertyChanged(nameof(ListaOrigens));
                }
            }
        }

        public ObservableCollection<Conjunto> ListaConjuntos
        {
            get { return _listaConjuntos; }
            set
            {
                if (value != _listaConjuntos)
                {
                    _listaConjuntos = value;
                    OnPropertyChanged(nameof(ListaConjuntos));
                }
            }
        }

        public ObservableCollection<Especificacao> ListaEspecificacoes
        {
            get { return _listaEspecificacoes; }
            set
            {
                if (value != _listaEspecificacoes)
                {
                    _listaEspecificacoes = value;
                    OnPropertyChanged(nameof(ListaEspecificacoes));
                }
            }
        }

        public ItemProposta ItemProposta
        {
            get { return _itemProposta; }
            set
            {
                if (value != _itemProposta)
                {
                    _itemProposta = value;
                    OnPropertyChanged(nameof(ItemProposta));
                }
            }
        }

        public string MensagemStatus
        {
            get { return _mensagemStatus; }
            set
            {
                if (value != _mensagemStatus)
                {
                    _mensagemStatus = value;
                    OnPropertyChanged(nameof(MensagemStatus));
                }
            }
        }

        public bool ControlesHabilitados
        {
            get { return _controlesHabilitados; }
            set
            {
                if (value != _controlesHabilitados)
                {
                    _controlesHabilitados = value;
                    OnPropertyChanged(nameof(ControlesHabilitados));
                }
            }
        }

        public bool CarregamentoVisivel
        {
            get { return _carregamentoVisivel; }
            set
            {
                if (_carregamentoVisivel != value)
                {
                    _carregamentoVisivel = value;
                    OnPropertyChanged(nameof(CarregamentoVisivel));
                }
            }
        }

        #endregion Propriedades/Comandos

        #region Métodos

        /// <summary>
        /// Método assíncrono que carrega a peça vinda do servidor
        /// </summary>
        private async Task CarregaPecaAsync()
        {
            try
            {
                if (ItemProposta?.Fornecedor != null)
                {
                    if (!String.IsNullOrEmpty(ItemProposta?.CodigoItem))
                    {
                        int? idAnterior = ItemProposta.Id;
                        await ItemProposta.GetItemPropostaDatabaseAsync(false, CancellationToken.None, true, "WHERE itpr.id_fornecedor = @id_fornecedor AND itpr.codigo_item = @codigo_item ORDER BY itpr.data_insercao DESC LIMIT 1",
                            "@id_fornecedor, @codigo_item", ItemProposta.Fornecedor.Id, ItemProposta.CodigoItem);
                        ItemProposta.Id = idAnterior;
                        try
                        {
                            ItemProposta.TipoSubstituicaoTributaria = ListaTiposSubstituicaoTributaria.First(tist => tist.Id == ItemProposta?.TipoSubstituicaoTributaria?.Id);
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            ItemProposta.Origem = ListaOrigens.First(orig => orig.Id == ItemProposta?.Origem?.Id);
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            Conjunto = ListaConjuntos.First(conj => conj.Id == ItemProposta?.Conjunto?.Id);
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            ItemProposta.Especificacao = ListaEspecificacoes.First(espe => espe.Id == ItemProposta?.Especificacao?.Id);
                        }
                        catch (Exception)
                        {
                        }

                        MensagemStatus = $"Dados carregados da peça presente na proposta {ItemProposta.Proposta.Cliente.Nome} - {ItemProposta.Proposta.CodigoProposta} gerada em {ItemProposta.Proposta.DataInsercao}";

                        ItemProposta.ValorTotalInicialItem = ItemProposta.PrecoUnitarioInicialItem * QuantidadeItem;

                        MessageBox.Show("É altamente recomendado que os preços e prazos sejam verificados com o fornecedor pois estes podem estar desatualizados", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

        /// <summary>
        /// Método para limpar as listas e salvar memória
        /// </summary>
        private void LimparListas()
        {
            ListaTiposItem = null;
            ListaStatus = null;
            ListaStatusAprovacao = null;
            ListaJustificativasAprovacao = null;
            ListaFornecedores = null;
            ListaTiposSubstituicaoTributaria = null;
            ListaOrigens = null;
            ListaConjuntos = null;
            ListaEspecificacoes = null;
        }

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        /// <summary>
        /// Método assíncrono que preenche e define os itens, deve ser chamado no construtor
        /// </summary>
        private async Task ConstrutorAsync()
        {
            try
            {
                // Preenche as listas com as classes necessárias
                await TipoItem.PreencheListaTiposItemAsync(ListaTiposItem, true, null, CancellationToken.None, "", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");
                await StatusAprovacao.PreencheListaStatusAprovacaoAsync(ListaStatusAprovacao, true, null, CancellationToken.None, "WHERE stap.id_status = 1", "");
                await JustificativaAprovacao.PreencheListaJustificativasAprovacaoAsync(ListaJustificativasAprovacao, true, null, CancellationToken.None, "WHERE juap.id_status = 1", "");
                await Fornecedor.PreencheListaFornecedoresAsync(ListaFornecedores, true, null, CancellationToken.None, "WHERE forn.id_status = 1", "");
                await TipoSubstituicaoTributaria.PreencheListaSubstituicaoTributariaItemAsync(ListaTiposSubstituicaoTributaria, true, null, CancellationToken.None, "", "");
                await Origem.PreencheListaOrigensAsync(ListaOrigens, true, null, CancellationToken.None, "", "");
                await Conjunto.PreencheListaConjuntosAsync(ListaConjuntos, true, null, CancellationToken.None, "WHERE conj.id_status = 1 ORDER BY conj.nome ASC", "");
                await Especificacao.PreencheListaEspecificacoesAsync(ListaEspecificacoes, true, null, CancellationToken.None, "WHERE espe.id_status = 1 ORDER BY espe.nome ASC", "");

                ListaEspecificacoesView = GetEspecificacaoCollectionView(ListaEspecificacoes);

                ListaEspecificacoesView.Filter = LimpaListaEspecificacoes;
                CollectionViewSource.GetDefaultView(ListaEspecificacoes).Refresh();

                // Define as classes da proposta como classes com a mesma referência das listas

                try
                {
                    ItemProposta.TipoItem = ListaTiposItem.First(tiip => tiip.Id == ItemProposta?.TipoItem?.Id);

                    switch (ItemProposta.TipoItem.Id)
                    {
                        case 2:

                            if (String.IsNullOrEmpty(ItemProposta.PrazoInicialItem))
                            {
                                ItemProposta.PrazoInicialItem = "A realizar";
                            }

                            if (String.IsNullOrEmpty(ItemProposta.PrazoFinalItem))
                            {
                                ItemProposta.PrazoFinalItem = "A realizar";
                            }

                            break;

                        case 3:

                            if (String.IsNullOrEmpty(ItemProposta.PrazoInicialItem))
                            {
                                ItemProposta.PrazoInicialItem = "A realizar";
                            }

                            if (String.IsNullOrEmpty(ItemProposta.PrazoFinalItem))
                            {
                                ItemProposta.PrazoFinalItem = "A realizar";
                            }

                            if (String.IsNullOrEmpty(ItemProposta.DescricaoItem))
                            {
                                ItemProposta.DescricaoItem = "DESLOCAMENTO";
                            }

                            await ItemProposta.GetUltimoValorDeslocamento(_proposta.Cliente.Id, CancellationToken.None);

                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                }

                try
                {
                    if (ItemProposta?.StatusAprovacao?.Id > 0 && !ListaStatusAprovacao.Any(x => x.Id == ItemProposta?.StatusAprovacao?.Id))
                    {
                        StatusAprovacao statusAprovacao = new();

                        await statusAprovacao.GetStatusAprovacaoDatabaseAsync(ItemProposta?.StatusAprovacao?.Id, CancellationToken.None);

                        ListaStatusAprovacao.Add(statusAprovacao);
                    }

                    if (ItemProposta?.JustificativaAprovacao?.Id > 0 && !ListaJustificativasAprovacao.Any(x => x.Id == ItemProposta?.JustificativaAprovacao?.Id))
                    {
                        JustificativaAprovacao justificativaAprovacao = new();

                        await justificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(ItemProposta?.JustificativaAprovacao?.Id, CancellationToken.None);

                        ListaJustificativasAprovacao.Add(justificativaAprovacao);
                    }

                    if (ItemProposta?.Fornecedor?.Id > 0 && !ListaFornecedores.Any(x => x.Id == ItemProposta?.Fornecedor?.Id))
                    {
                        Fornecedor fornecedor = new();

                        await fornecedor.GetFornecedorDatabaseAsync(ItemProposta?.Fornecedor?.Id, CancellationToken.None);

                        ListaFornecedores.Add(fornecedor);
                    }

                    if (ItemProposta?.Conjunto?.Id > 0 && !ListaConjuntos.Any(x => x.Id == ItemProposta?.Conjunto?.Id))
                    {
                        Conjunto conjunto = new();

                        await conjunto.GetConjuntoDatabaseAsync(ItemProposta?.Conjunto?.Id, CancellationToken.None);

                        ListaConjuntos.Add(conjunto);
                    }

                    if (ItemProposta?.Especificacao?.Id > 0 && !ListaEspecificacoes.Any(x => x.Id == ItemProposta?.Especificacao?.Id))
                    {
                        Especificacao especificacao = new();

                        await especificacao.GetEspecificacaoDatabaseAsync(ItemProposta?.Especificacao?.Id, CancellationToken.None);

                        ListaEspecificacoes.Add(especificacao);
                    }
                }
                catch (Exception)
                {
                }

                try
                {
                    ItemProposta.Status = ListaStatus.First(tiip => tiip.Id == ItemProposta?.Status?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    ItemProposta.StatusAprovacao = ListaStatusAprovacao.First(tiip => tiip.Id == ItemProposta?.StatusAprovacao?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    ItemProposta.JustificativaAprovacao = ListaJustificativasAprovacao.First(tiip => tiip.Id == ItemProposta?.JustificativaAprovacao?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    ItemProposta.Fornecedor = ListaFornecedores.First(forn => forn.Id == ItemProposta?.Fornecedor?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    ItemProposta.TipoSubstituicaoTributaria = ListaTiposSubstituicaoTributaria.First(tist => tist.Id == ItemProposta?.TipoSubstituicaoTributaria?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    ItemProposta.Origem = ListaOrigens.First(orig => orig.Id == ItemProposta?.Origem?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    Conjunto = ListaConjuntos.First(conj => conj.Id == ItemProposta?.Conjunto?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    ItemProposta.Especificacao = ListaEspecificacoes.First(espe => espe.Id == ItemProposta?.Especificacao?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
#pragma warning disable CS8601 // Possible null reference assignment.
                    Fornecedor = ItemProposta.Fornecedor;
#pragma warning restore CS8601 // Possible null reference assignment.
                    QuantidadeItem = ItemProposta.QuantidadeItem;
                }
                catch (Exception)
                {
                }
                ControlesHabilitados = true;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");

                MensagemStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                ControlesHabilitados = false;
            }
            CarregamentoVisivel = false;
        }

        /// <summary>
        /// Método assíncrono para salvar o item da proposta
        /// </summary>
        private async Task SalvarItemPropostaAsync()
        {
            // Verifica se existem campos vazios e, caso verdadeiro, encerra a execução do método
            if (ExistemCamposVazios)
            {
                return;
            }

            // Caso o item não seja uma peça, limpa os campos referentes à peça
            if (ItemProposta.TipoItem.Id != 1)
            {
                ItemProposta.CodigoItem = null;
                ItemProposta.QuantidadeItem = null;
                ItemProposta.PercentualIpiItem = null;
                ItemProposta.PercentualIcmsItem = null;
                ItemProposta.NcmItem = null;
                ItemProposta.MvaItem = null;
                ItemProposta.ValorStItem = null;
                ItemProposta.ValorTotalInicialItem = null;
                ItemProposta.FreteUnitarioItem = null;
                ItemProposta.DescontoInicialItem = null;
                ItemProposta.PrecoAposDescontoInicialItem = null;
                ItemProposta.PrecoComIpiEIcmsItem = null;
                ItemProposta.PercentualAliquotaExternaIcmsItem = null;
                ItemProposta.ValorIcmsCreditoItem = null;
                ItemProposta.PercentualMvaItem = null;
                ItemProposta.ValorMvaItem = null;
                ItemProposta.PrecoComMvaItem = null;
                ItemProposta.PercentualAliquotaInternaIcmsItem = null;
                ItemProposta.ValorIcmsDebitoItem = null;
                ItemProposta.SaldoIcmsItem = null;
                ItemProposta.PrecoUnitarioParaRevendedorItem = null;
                ItemProposta.ImpostosFederaisItem = null;
                ItemProposta.RateioDespesaFixaItem = null;
                ItemProposta.PercentualLucroNecessarioItem = null;
                ItemProposta.PrecoListaVendaItem = null;
                ItemProposta.QuantidadeEstoqueCodigoCompletoItem = null;
                ItemProposta.QuantidadeEstoqueCodigoAbreviadoItem = null;
                ItemProposta.InformacaoEstoqueCodigoCompletoItem = null;
                ItemProposta.InformacaoEstoqueCodigoAbreviadoItem = null;

                ItemProposta.Conjunto = null;
                ItemProposta.Especificacao = null;
                ItemProposta.Fornecedor = null;
                ItemProposta.TipoSubstituicaoTributaria = null;
                ItemProposta.Origem = null;
            }

#pragma warning disable CS8604 // Possible null reference argument.
            await ItemProposta.CalculaValoresItemPropostaAsync(_proposta.Cliente);
#pragma warning restore CS8604 // Possible null reference argument.

            // Verifica se é um novo item e, caso verdadeiro, adiciona-o a proposta. Caso contrário, apenas altera-o
            if (_ehNovoItem)
            {
                var codigoItemAnterior = ItemProposta.CodigoItem;
                var conjuntoAnterior = ItemProposta.Conjunto;
                var especificacaoAnterior = ItemProposta.Especificacao;
                ItemProposta.DataInsercao = DateTime.Now;
                ItemProposta.Usuario = App.Usuario == null ? null : (Usuario)App.Usuario.Clone();

                _proposta.ListaItensProposta.Add(ItemProposta);

                ComandoFechar.Execute(null);

                ItemProposta.CodigoItem = codigoItemAnterior;
                ItemProposta.Conjunto = conjuntoAnterior;
                ItemProposta.Especificacao = especificacaoAnterior;
            }
            else
            {
                _itemPropostaInicial.CodigoItem = ItemProposta.CodigoItem;
                _itemPropostaInicial.DescricaoItem = ItemProposta.DescricaoItem;
                _itemPropostaInicial.QuantidadeItem = ItemProposta.QuantidadeItem;
                _itemPropostaInicial.PrecoUnitarioInicialItem = ItemProposta.PrecoUnitarioInicialItem;
                _itemPropostaInicial.PercentualIpiItem = ItemProposta.PercentualIpiItem;
                _itemPropostaInicial.PercentualIcmsItem = ItemProposta.PercentualIcmsItem;
                _itemPropostaInicial.NcmItem = ItemProposta.NcmItem;
                _itemPropostaInicial.MvaItem = ItemProposta.MvaItem;
                _itemPropostaInicial.ValorStItem = ItemProposta.ValorStItem;
                _itemPropostaInicial.ValorTotalInicialItem = ItemProposta.ValorTotalInicialItem;
                _itemPropostaInicial.PrazoInicialItem = ItemProposta.PrazoInicialItem;
                _itemPropostaInicial.FreteUnitarioItem = ItemProposta.FreteUnitarioItem;
                _itemPropostaInicial.DescontoInicialItem = ItemProposta.DescontoInicialItem;
                _itemPropostaInicial.PrecoAposDescontoInicialItem = ItemProposta.PrecoAposDescontoInicialItem;
                _itemPropostaInicial.PrecoComIpiEIcmsItem = ItemProposta.PrecoComIpiEIcmsItem;
                _itemPropostaInicial.PercentualAliquotaExternaIcmsItem = ItemProposta.PercentualAliquotaExternaIcmsItem;
                _itemPropostaInicial.ValorIcmsCreditoItem = ItemProposta.ValorIcmsCreditoItem;
                _itemPropostaInicial.PercentualMvaItem = ItemProposta.PercentualMvaItem;
                _itemPropostaInicial.ValorMvaItem = ItemProposta.ValorMvaItem;
                _itemPropostaInicial.PrecoComMvaItem = ItemProposta.PrecoComMvaItem;
                _itemPropostaInicial.PercentualAliquotaInternaIcmsItem = ItemProposta.PercentualAliquotaInternaIcmsItem;
                _itemPropostaInicial.ValorIcmsDebitoItem = ItemProposta.ValorIcmsDebitoItem;
                _itemPropostaInicial.SaldoIcmsItem = ItemProposta.SaldoIcmsItem;
                _itemPropostaInicial.PrecoUnitarioParaRevendedorItem = ItemProposta.PrecoUnitarioParaRevendedorItem;
                _itemPropostaInicial.ImpostosFederaisItem = ItemProposta.ImpostosFederaisItem;
                _itemPropostaInicial.RateioDespesaFixaItem = ItemProposta.RateioDespesaFixaItem;
                _itemPropostaInicial.PercentualLucroNecessarioItem = ItemProposta.PercentualLucroNecessarioItem;
                _itemPropostaInicial.PrecoListaVendaItem = ItemProposta.PrecoListaVendaItem;
                _itemPropostaInicial.DescontoFinalItem = ItemProposta.DescontoFinalItem;
                _itemPropostaInicial.PrecoUnitarioFinalItem = ItemProposta.PrecoUnitarioFinalItem;
                _itemPropostaInicial.PrecoTotalFinalItem = ItemProposta.PrecoTotalFinalItem;
                _itemPropostaInicial.QuantidadeEstoqueCodigoCompletoItem = ItemProposta.QuantidadeEstoqueCodigoCompletoItem;
                _itemPropostaInicial.QuantidadeEstoqueCodigoAbreviadoItem = ItemProposta.QuantidadeEstoqueCodigoAbreviadoItem;
                _itemPropostaInicial.InformacaoEstoqueCodigoCompletoItem = ItemProposta.InformacaoEstoqueCodigoCompletoItem;
                _itemPropostaInicial.InformacaoEstoqueCodigoAbreviadoItem = ItemProposta.InformacaoEstoqueCodigoAbreviadoItem;
                _itemPropostaInicial.PrazoFinalItem = ItemProposta.PrazoFinalItem;
                _itemPropostaInicial.ComentariosItem = ItemProposta.ComentariosItem;
                _itemPropostaInicial.DataAprovacaoItem = ItemProposta.DataAprovacaoItem;

                if (_itemPropostaInicial.Id != null)
                {
                    _itemPropostaInicial.DataEdicaoItem = DateTime.Now;
                }

                _itemPropostaInicial.Usuario = App.Usuario == null ? null : (Usuario)App.Usuario.Clone();
                _itemPropostaInicial.Status = ItemProposta.Status == null ? null : (Status)ItemProposta.Status.Clone();
                _itemPropostaInicial.StatusAprovacao = ItemProposta.StatusAprovacao == null ? null : (StatusAprovacao)ItemProposta.StatusAprovacao.Clone();
                _itemPropostaInicial.JustificativaAprovacao = ItemProposta.JustificativaAprovacao == null ? null : (JustificativaAprovacao)ItemProposta.JustificativaAprovacao.Clone();
                _itemPropostaInicial.TipoItem = ItemProposta.TipoItem == null ? null : (TipoItem)ItemProposta.TipoItem.Clone();
                _itemPropostaInicial.Conjunto = ItemProposta.Conjunto == null ? null : (Conjunto)ItemProposta.Conjunto.Clone();
                _itemPropostaInicial.Especificacao = ItemProposta.Especificacao == null ? null : (Especificacao)ItemProposta.Especificacao.Clone();
                _itemPropostaInicial.Fornecedor = ItemProposta.Fornecedor == null ? null : (Fornecedor)ItemProposta.Fornecedor.Clone();
                _itemPropostaInicial.TipoSubstituicaoTributaria = ItemProposta.TipoSubstituicaoTributaria == null ? null : (TipoSubstituicaoTributaria)ItemProposta.TipoSubstituicaoTributaria.Clone();
                _itemPropostaInicial.Origem = ItemProposta.Origem == null ? null : (Origem)ItemProposta.Origem.Clone();

                // Executa o comando que fecha a caixa de diálogo
                ComandoFechar.Execute(null);

                OnPropertyChanged(nameof(_itemPropostaInicial));
            }
        }

        public bool FiltraEspecificacoes(object item)
        {
            if (Conjunto == null)
            {
                return false;
            }
            if (item is Especificacao especificacao)
            {
                return especificacao.Conjunto.Id == Conjunto.Id;
            }
            return true;
        }

        public bool LimpaListaEspecificacoes(object item)
        {
            return false;
        }

        #endregion Métodos

        #region Classes

        public CollectionView GetEspecificacaoCollectionView(ObservableCollection<Especificacao> especificacaoList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(especificacaoList);
        }

        #endregion Classes
    }
}