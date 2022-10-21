using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class ControleItemOrdemServicoViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private ObservableCollection<MotivoItemOrdemServico> _listaMotivos = new();
        private ObservableCollection<FornecimentoItemOrdemServico> _listaFornecimentos = new();
        private ObservableCollection<Status> _listaStatus = new();
        private ObservableCollection<Peca> _listaPecas = new();
        private ObservableCollection<Conjunto> _listaConjuntos = new();
        private ObservableCollection<Especificacao> _listaEspecificacoes = new();
        private ItemOrdemServico _itemOrdemServico;
        private ItemOrdemServico _itemOrdemServicoInicial;
        private Peca _peca;
        private OrdemServico _ordemServico;
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
        /// <param name="ordemServico">Ordem de serviço que terá o item inserido ou editado</param>
        /// <param name="itemOrdemServico">Item da proposta que será inserido ou editado</param>
        /// <param name="ehNovoItem">Valor booleano informando se o item é novo ou existente</param>
        /// <param name="closeHandler">Ação para fechar a caixa de diálogo</param>
        public ControleItemOrdemServicoViewModel(OrdemServico ordemServico, ItemOrdemServico? itemOrdemServico, bool ehNovoItem, Action<ControleItemOrdemServicoViewModel> closeHandler)
        {
            // Define os itens internos como o informado no construtor
            _ordemServico = ordemServico;
            _ehNovoItem = ehNovoItem;

            // Verifica se é um novo item
            if (_ehNovoItem)
            {
                // Define o item da proposta
                ItemOrdemServico = itemOrdemServico;

                // Inicializa a peça
                Peca = new();
            }
            else
            {
                // Salva o item inicial que foi passado
                _itemOrdemServicoInicial = itemOrdemServico;

                // Define o item da proposta a ser utilizado como uma cópia do item existente
                ItemOrdemServico = (ItemOrdemServico)itemOrdemServico.Clone();
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
                return "Item da ordem de serviço";
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
                _listaMotivos = null;
                _listaFornecimentos = null;
                _listaStatus = null;
                _listaPecas = null;
                _listaConjuntos = null;
                _listaEspecificacoes = null;
                _peca = null;
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
                        param => SalvarItemOrdemServicoAsync().Await(),
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
                    ItemOrdemServico.QuantidadeItem = value;
                    OnPropertyChanged(nameof(QuantidadeItem));
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
                    ItemOrdemServico.Conjunto = value;
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
                    ItemOrdemServico.CodigoItem = value.CodigoItem;
                    OnPropertyChanged(nameof(Peca));

                    PreencherPecas().Await();
                }
            }
        }

        private async Task PreencherPecas()
        {
            await Peca.PreencheListaPecasAsync(ListaPecas, true, null, CancellationToken.None,
                    "GROUP BY itpr.codigo_item ORDER BY itpr.codigo_item ASC", "");
        }

        public ObservableCollection<MotivoItemOrdemServico> ListaMotivos
        {
            get { return _listaMotivos; }
            set
            {
                if (value != _listaMotivos)
                {
                    _listaMotivos = value;
                    OnPropertyChanged(nameof(ListaMotivos));
                }
            }
        }

        public ObservableCollection<FornecimentoItemOrdemServico> ListaFornecimentos
        {
            get { return _listaFornecimentos; }
            set
            {
                if (value != _listaFornecimentos)
                {
                    _listaFornecimentos = value;
                    OnPropertyChanged(nameof(ListaFornecimentos));
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

        public ItemOrdemServico ItemOrdemServico
        {
            get { return _itemOrdemServico; }
            set
            {
                if (value != _itemOrdemServico)
                {
                    _itemOrdemServico = value;
                    OnPropertyChanged(nameof(ItemOrdemServico));
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
                if (!String.IsNullOrEmpty(ItemOrdemServico?.CodigoItem))
                {
                    ItemProposta itemProposta = new(true, true);
                    //int? idAnterior = ItemOrdemServico.Id;
                    await itemProposta.GetItemPropostaDatabaseAsync(false, CancellationToken.None, true, "WHERE itpr.codigo_item = @codigo_item ORDER BY itpr.data_insercao DESC LIMIT 1",
                             "@codigo_item", ItemOrdemServico?.CodigoItem);

                    ItemOrdemServico.CodigoItem = itemProposta.CodigoItem;
                    ItemOrdemServico.DescricaoItem = itemProposta.DescricaoItem;

                    try
                    {
                        Conjunto = ListaConjuntos.First(conj => conj.Id == itemProposta?.Conjunto?.Id);
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        ItemOrdemServico.Especificacao = ListaEspecificacoes.First(espe => espe.Id == itemProposta?.Especificacao?.Id);
                    }
                    catch (Exception)
                    {
                    }

                    MensagemStatus = $"Dados carregados da peça presente na proposta {itemProposta.Proposta.Cliente.Nome} - {itemProposta.Proposta.CodigoProposta} gerada em {itemProposta.Proposta.DataInsercao}";
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
            ListaMotivos = null;
            ListaFornecimentos = null;
            ListaStatus = null;
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
                await MotivoItemOrdemServico.PreencheListaMotivosItemOrdemServicoAsync(ListaMotivos, true, null, CancellationToken.None, "WHERE mios.id_status = 1", "");
                await FornecimentoItemOrdemServico.PreencheListaFornecimentosItemOrdemServicoAsync(ListaFornecimentos, true, null, CancellationToken.None, "WHERE fios.id_status = 1", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");
                await Conjunto.PreencheListaConjuntosAsync(ListaConjuntos, true, null, CancellationToken.None, "WHERE conj.id_status = 1 ORDER BY conj.nome ASC", "");
                await Especificacao.PreencheListaEspecificacoesAsync(ListaEspecificacoes, true, null, CancellationToken.None, "WHERE espe.id_status = 1 ORDER BY espe.nome ASC", "");

                ListaEspecificacoesView = GetEspecificacaoCollectionView(ListaEspecificacoes);

                ListaEspecificacoesView.Filter = LimpaListaEspecificacoes;
                CollectionViewSource.GetDefaultView(ListaEspecificacoes).Refresh();
                try
                {
                    if (ItemOrdemServico?.MotivoItemOrdemServico?.Id > 0 && !ListaMotivos.Any(x => x.Id == ItemOrdemServico?.MotivoItemOrdemServico?.Id))
                    {
                        MotivoItemOrdemServico motivoItemOrdemServico = new();

                        await motivoItemOrdemServico.GetMotivoItemOrdemServicoDatabaseAsync(ItemOrdemServico?.MotivoItemOrdemServico?.Id, CancellationToken.None);

                        ListaMotivos.Add(motivoItemOrdemServico);
                    }

                    if (ItemOrdemServico?.FornecimentoItemOrdemServico?.Id > 0 && !ListaFornecimentos.Any(x => x.Id == ItemOrdemServico?.FornecimentoItemOrdemServico?.Id))
                    {
                        FornecimentoItemOrdemServico fornecimentoItemOrdemServico = new();

                        await fornecimentoItemOrdemServico.GetFornecimentoItemOrdemServicoDatabaseAsync(ItemOrdemServico?.FornecimentoItemOrdemServico?.Id, CancellationToken.None);

                        ListaFornecimentos.Add(fornecimentoItemOrdemServico);
                    }

                    if (ItemOrdemServico?.Conjunto?.Id > 0 && !ListaConjuntos.Any(x => x.Id == ItemOrdemServico?.Conjunto?.Id))
                    {
                        Conjunto conjunto = new();

                        await conjunto.GetConjuntoDatabaseAsync(ItemOrdemServico?.Conjunto?.Id, CancellationToken.None);

                        ListaConjuntos.Add(conjunto);
                    }

                    if (ItemOrdemServico?.Especificacao?.Id > 0 && !ListaEspecificacoes.Any(x => x.Id == ItemOrdemServico?.Especificacao?.Id))
                    {
                        Especificacao especificacao = new();

                        await especificacao.GetEspecificacaoDatabaseAsync(ItemOrdemServico?.Especificacao?.Id, CancellationToken.None);

                        ListaEspecificacoes.Add(especificacao);
                    }
                }
                catch (Exception)
                {
                }

                // Define as classes da proposta como classes com a mesma referência das listas
                try
                {
                    ItemOrdemServico.MotivoItemOrdemServico = ListaMotivos.First(tiip => tiip.Id == ItemOrdemServico?.MotivoItemOrdemServico?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    ItemOrdemServico.FornecimentoItemOrdemServico = ListaFornecimentos.First(tiip => tiip.Id == ItemOrdemServico?.FornecimentoItemOrdemServico?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    ItemOrdemServico.Status = ListaStatus.First(tiip => tiip.Id == ItemOrdemServico?.Status?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    Conjunto = ListaConjuntos.First(conj => conj.Id == ItemOrdemServico?.Conjunto?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    ItemOrdemServico.Especificacao = ListaEspecificacoes.First(espe => espe.Id == ItemOrdemServico?.Especificacao?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    QuantidadeItem = ItemOrdemServico.QuantidadeItem;
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
        private async Task SalvarItemOrdemServicoAsync()
        {
            // Verifica se existem campos vazios e, caso verdadeiro, encerra a execução do método
            if (ExistemCamposVazios)
            {
                return;
            }

            // Verifica se é um novo item e, caso verdadeiro, adiciona-o a proposta. Caso contrário, apenas altera-o
            if (_ehNovoItem)
            {
                var codigoItemAnterior = ItemOrdemServico.CodigoItem;
                var conjuntoAnterior = ItemOrdemServico.Conjunto;
                var especificacaoAnterior = ItemOrdemServico.Especificacao;
                ItemOrdemServico.DataInsercao = DateTime.Now;
                ItemOrdemServico.Usuario = App.Usuario == null ? null : (Usuario)App.Usuario.Clone();

                _ordemServico.ListaItensOrdemServico.Add(ItemOrdemServico);

                ComandoFechar.Execute(null);

                ItemOrdemServico.CodigoItem = codigoItemAnterior;
                ItemOrdemServico.Conjunto = conjuntoAnterior;
                ItemOrdemServico.Especificacao = especificacaoAnterior;
            }
            else
            {
                _itemOrdemServicoInicial.CodigoItem = ItemOrdemServico.CodigoItem;
                _itemOrdemServicoInicial.DescricaoItem = ItemOrdemServico.DescricaoItem;
                _itemOrdemServicoInicial.QuantidadeItem = ItemOrdemServico.QuantidadeItem;
                _itemOrdemServicoInicial.ComentariosItem = ItemOrdemServico.ComentariosItem;

                if (_itemOrdemServicoInicial.Id != null)
                {
                    _itemOrdemServicoInicial.DataEdicaoItem = DateTime.Now;
                }

                _itemOrdemServicoInicial.Usuario = App.Usuario == null ? null : (Usuario)App.Usuario.Clone();
                _itemOrdemServicoInicial.MotivoItemOrdemServico = ItemOrdemServico.MotivoItemOrdemServico == null ? null : (MotivoItemOrdemServico)ItemOrdemServico.MotivoItemOrdemServico.Clone();
                _itemOrdemServicoInicial.FornecimentoItemOrdemServico = ItemOrdemServico.FornecimentoItemOrdemServico == null ? null : (FornecimentoItemOrdemServico)ItemOrdemServico.FornecimentoItemOrdemServico.Clone();
                _itemOrdemServicoInicial.Status = ItemOrdemServico.Status == null ? null : (Status)ItemOrdemServico.Status.Clone();
                _itemOrdemServicoInicial.Conjunto = ItemOrdemServico.Conjunto == null ? null : (Conjunto)ItemOrdemServico.Conjunto.Clone();
                _itemOrdemServicoInicial.Especificacao = ItemOrdemServico.Especificacao == null ? null : (Especificacao)ItemOrdemServico.Especificacao.Clone();

                // Executa o comando que fecha a caixa de diálogo
                ComandoFechar.Execute(null);

                OnPropertyChanged(nameof(_itemOrdemServicoInicial));
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