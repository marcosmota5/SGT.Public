using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;
using Ookii.Dialogs.Wpf;
using SGT.HelperClasses;
using SGT.Views;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class PropostaViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private string _name;
        private ObservableCollection<Cliente> _listaClientes = new();
        private ObservableCollection<Contato> _listaContatos = new();
        private ObservableCollection<Pais> _listaPaises = new();
        private ObservableCollection<Estado> _listaEstados = new();
        private ObservableCollection<Cidade> _listaCidades = new();
        private ObservableCollection<Serie> _listaSeries = new();
        private ObservableCollection<Fabricante> _listaFabricantes = new();
        private ObservableCollection<Categoria> _listaCategorias = new();
        private ObservableCollection<TipoEquipamento> _listaTiposEquipamento = new();
        private ObservableCollection<Classe> _listaClasses = new();
        private ObservableCollection<Modelo> _listaModelos = new();
        private ObservableCollection<Ano> _listaAnos = new();
        private ObservableCollection<Prioridade> _listaPrioridades = new();
        private ObservableCollection<Status> _listaStatus = new();
        private ObservableCollection<int?> _listaNumeroOrdensServico = new();
        private ObservableCollection<StatusAprovacao> _listaStatusAprovacao = new();
        private ObservableCollection<JustificativaAprovacao> _listaJustificativasAprovacao = new();
        private ObservableCollection<Conjunto> _listaConjunto = new();
        private ObservableCollection<Especificacao> _listaEspecificacao = new();
        private ObservableCollection<CopiarDeTermo> _listaCopiarDe = new();
        private ObservableCollection<Termo> _listaTermosClienteAtual = new();
        private ObservableCollection<Termo> _listaTermosClienteTodos = new();
        private ObservableCollection<Termo> _listaTermos = new();
        private ObservableCollection<Proposta> _listaPropostas = new();
        private ObservableCollection<Setor> _listaSetores = new();
        private ObservableCollection<Termo> _listaTermosSetor = new();
        private Proposta _proposta;
        private ItemProposta _itemPropostaSelecionado;
        private ObservableCollection<ItemProposta> _listaItensPropostaSelecionados = new();
        private Cliente _cliente;
        private CancellationTokenSource _cts;
        private double _valorProgresso;
        private bool _progressoEhIndeterminavel;
        private bool _progressoVisivel = false;
        private string _mensagemStatus;
        private bool ehEdicaoLimitada = false;
        private bool ehRevisao = false;
        private int? idPropostaOriginal;
        private string _textoColunaTodas = "Exibir todas";
        private string _formatoTelefone;

        private bool _ehCopia;
        private bool _edicaoEhEnterprise;
        private bool _permiteCopiar;

        private ICommand _comandoSalvar;
        private ICommand _comandoEditar;
        private ICommand _comandoRevisar;
        private ICommand _comandoDeletar;
        private ICommand _comandoCancelarEdicao;
        private ICommand _comandoCancelar;
        private ICommand _comandoGetContato;
        private ICommand _comandoGetSerie;
        private ICommand _comandoPreencheEstados;
        private ICommand _comandoPreencheCidades;
        private ICommand _comandoAdicionarPeca;
        private ICommand _comandoAdicionarServico;
        private ICommand _comandoAdicionarDeslocamento;
        private ICommand _comandoEditarItens;
        private ICommand _comandoRemoverItens;
        private ICommand _comandoImportarItensKion;
        private ICommand _comandoImportarItensTVH;
        private ICommand _comandoExportarFormatoFluig;
        private ICommand _comandoExportarTabelaCompleta;
        private ICommand _comandoAtualizarPrazoItensCodigoCompleto;
        private ICommand _comandoAtualizarPrazoItensCodigoAbreviado;
        private ICommand _comandoAtualizarPrazoItensDecidirPorPeca;

        private ICommand _comandoFreteItensValorIgual;
        private ICommand _comandoFreteItensValorDivididoContagem;
        private ICommand _comandoFreteItensValorDivididoQuantidade;
        private ICommand _comandoFreteItensValorDivididoPrecoUnitarioInicial;
        private ICommand _comandoFreteItensValorDivididoPrecoUnitarioFinal;
        private ICommand _comandoFreteItensValorDivididoPrecoTotalInicial;
        private ICommand _comandoFreteItensValorDivididoPrecoTotalFinal;

        private ICommand _comandoAplicarDescontoCusto;
        private ICommand _comandoAplicarDescontoFinalPecas;
        private ICommand _comandoAplicarDescontoFinalServicos;
        private ICommand _comandoAplicarDescontoFinalDeslocamentos;
        private ICommand _comandoAplicarDescontoFinalTodosOsItens;
        private ICommand _comandoConfirmarCopia;

        private ICommand _comandoAlteraStatusAprovacaoUnico;
        private ICommand _comandoAlteraStatusAprovacaoTodos;
        private ICommand _comandoAlteraJustificativaAprovacaoUnico;
        private ICommand _comandoAlteraJustificativaAprovacaoTodos;
        private ICommand _comandoAlteraConjuntoUnico;
        private ICommand _comandoAlteraConjuntoTodos;
        private ICommand _comandoAlteraEspecificacaoUnico;
        private ICommand _comandoAlteraEspecificacaoTodos;

        private ICommand _comandoCopiarPropostaNovaProposta;
        private ICommand _comandoCopiarPropostaNovaOrdemServico;

        private ICommand _comandoVisibilidadeColunaTodos;

        private ICommand _comandoVisualizarProposta;

        private ICommand _comandoAbrirOrdemServico;

        private ICommand _comandoAlteraStatusAprovacaoProposta;
        private ICommand _comandoAlteraJustificativaAprovacaoProposta;

        private CopiarDeTermo? copiarDe;
        private Cliente? _clienteTodos;
        private Cliente? _clienteProposta;
        private Proposta? _propostaTermo;
        private Setor? _setorTermo;
        private Termo? _termoClienteAtual;
        private Termo? _termoClienteTodos;
        private Termo? _termoTodos;
        private Termo? _termoSetor;

        private int _idStatusAprovacaoUnico;
        private int _idStatusAprovacaoTodos;
        private int _idJustificativaAprovacaoUnico;
        private int _idJustificativaAprovacaoTodos;
        private Conjunto _conjuntoUnico;
        private Conjunto _conjuntoTodos;
        private Especificacao _especificacaoUnico;
        private Especificacao _especificacaoTodos;

        private Pais? _pais;
        private Estado? _estado;
        private Cidade? _cidade;
        private Fabricante? _fabricante;
        private Categoria? _categoria;
        private TipoEquipamento? _tipoEquipamento;
        private Classe? _classe;
        private Modelo? _modelo;
        private readonly IDialogCoordinator _dialogCoordinator;
        private decimal? _valorFrete;
        private decimal? _descontoCusto;
        private decimal? _descontoFinalPecas;
        private decimal? _descontoFinalServicos;
        private decimal? _descontoFinalDeslocamentos;
        private decimal? _descontoFinalTodosOsItens;

        private bool _deletarVisivel;
        private bool _cancelarEdicaoVisivel;
        private bool _contextMenuVisivel;
        private bool _cancelarVisivel;

        private bool _visibilidadeColunaTodas;
        private bool _visibilidadeColunaOutrosDadosIniciais;
        private bool _visibilidadeColunaCalculos;
        private bool _visibilidadeColunaQuantidadeEstoque;

        private bool _edicaoHabilitada;

        private bool _edicaoSolicitante;
        private bool _edicaoFaturamento;
        private bool _edicaoProposta;
        private bool _edicaoEquipamento;
        private bool _edicaoComentarios;

        private bool _controlesHabilitados;
        private bool _carregamentoVisivel = true;

        private bool _permiteSalvar;
        private bool _permiteEditar;
        private bool _permiteRevisar;
        private bool _permiteDeletar;
        private bool _permiteVisualizar;
        private bool _permiteCancelarEdicao;
        private bool _permiteCancelar;

        private bool atualizouPrazo = false;
        private bool removeuItemExistente = false;
        private bool ehCarregamento = false;

        #endregion Campos

        #region Construtores

        /// <summary>
        /// Construtor sem parâmetros para criar uma nova proposta
        /// </summary>
        public PropostaViewModel(IDialogCoordinator dialogCoordinator, Proposta? proposta = null, bool ehCopia = false)
        {
            Name = "Carregando...";

            _dialogCoordinator = dialogCoordinator;

            _ehCopia = ehCopia;

            EdicaoEhEnterprise = App.EdicaoEhEnterprise;

            // Executa o método para preencher as listas
            ConstrutorAsync(proposta).Await();
        }

        /// <summary>
        /// Construtor que carrega uma proposta com os parâmetros utilizados
        /// </summary>
        /// <param name="proposta">Instância de proposta a ser carregada</param>
        public PropostaViewModel(Proposta proposta)
        {
            EdicaoEhEnterprise = App.EdicaoEhEnterprise;

            // Executa o método para preencher as listas
            ConstrutorAsync(proposta).Await();
        }

        #endregion Construtores

        #region Propriedades/Comandos

        //public string Name
        //{
        //    get
        //    {
        //        return "Nova proposta";
        //    }
        //}

        public SfDataGrid DataGrid { get; set; }

        public void LimparViewModel()
        {
            try
            {
                if (EhMovimentacao)
                {
                    EhMovimentacao = false;
                    return;
                }

                if (Proposta?.Id != null && Proposta?.IdUsuarioEmUso != null)
                {
                    Proposta.IdUsuarioEmUso = null;

                    Proposta.AtualizaIdUsuarioEmUsoAsync(CancellationToken.None).Await();
                }

                _listaClientes = null;
                _listaContatos = null;
                _listaPaises = null;
                _listaEstados = null;
                _listaCidades = null;
                _listaSeries = null;
                _listaFabricantes = null;
                _listaCategorias = null;
                _listaTiposEquipamento = null;
                _listaClasses = null;
                _listaModelos = null;
                _listaAnos = null;
                _listaPrioridades = null;
                _listaStatus = null;
                _listaStatusAprovacao = null;
                _listaJustificativasAprovacao = null;
                _listaCopiarDe = null;
                _listaTermosClienteAtual = null;
                _listaTermosClienteTodos = null;
                _listaTermos = null;
                _listaPropostas = null;
                _listaSetores = null;
                _listaTermosSetor = null;

                _comandoSalvar = null;
                _comandoEditar = null;
                _comandoRevisar = null;
                _comandoDeletar = null;
                _comandoVisualizarProposta = null;
                _comandoCancelarEdicao = null;
                _comandoGetContato = null;
                _comandoGetSerie = null;
                _comandoPreencheEstados = null;
                _comandoPreencheCidades = null;
                _comandoAdicionarPeca = null;
                _comandoAdicionarServico = null;
                _comandoAdicionarDeslocamento = null;
                _comandoEditarItens = null;
                _comandoRemoverItens = null;
                _comandoImportarItensKion = null;
                _comandoImportarItensTVH = null;
                _comandoExportarFormatoFluig = null;
                _comandoExportarTabelaCompleta = null;
                _comandoAtualizarPrazoItensCodigoCompleto = null;
                _comandoAtualizarPrazoItensCodigoAbreviado = null;
                _comandoAtualizarPrazoItensDecidirPorPeca = null;

                _comandoFreteItensValorIgual = null;
                _comandoFreteItensValorDivididoContagem = null;
                _comandoFreteItensValorDivididoQuantidade = null;
                _comandoFreteItensValorDivididoPrecoUnitarioInicial = null;
                _comandoFreteItensValorDivididoPrecoUnitarioFinal = null;
                _comandoFreteItensValorDivididoPrecoTotalInicial = null;
                _comandoFreteItensValorDivididoPrecoTotalFinal = null;

                _comandoAplicarDescontoCusto = null;
                _comandoAplicarDescontoFinalPecas = null;
                _comandoAplicarDescontoFinalServicos = null;
                _comandoAplicarDescontoFinalDeslocamentos = null;
                _comandoAplicarDescontoFinalTodosOsItens = null;
                _comandoConfirmarCopia = null;

                _comandoAlteraStatusAprovacaoUnico = null;
                _comandoAlteraStatusAprovacaoTodos = null;
                _comandoAlteraJustificativaAprovacaoUnico = null;
                _comandoAlteraJustificativaAprovacaoTodos = null;
                _comandoCopiarPropostaNovaProposta = null;

                _comandoVisibilidadeColunaTodos = null;
                _comandoCopiarPropostaNovaProposta = null;
                _comandoCopiarPropostaNovaOrdemServico = null;

                copiarDe = null;
                _clienteTodos = null;
                _clienteProposta = null;
                _propostaTermo = null;
                _setorTermo = null;
                _termoClienteAtual = null;
                _termoClienteTodos = null;
                _termoTodos = null;
                _termoSetor = null;

                _pais = null;
                _estado = null;
                _cidade = null;
                _fabricante = null;
                _categoria = null;
                _tipoEquipamento = null;
                _classe = null;
                _modelo = null;

                ListaContatosView = null;
                ListaEstadosView = null;
                ListaCidadesView = null;
                ListaSeriesView = null;
                ListaModelosView = null;

                _proposta = null;
                _itemPropostaSelecionado = null;
                _cliente = null;
                _cts = null;
            }
            catch (Exception)
            {
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                    Messenger.Default.Send<IPageViewModel>(this, "PrincipalPagina");
                }
            }
        }

        public string Icone
        {
            get
            {
                return "Handshake";
            }
        }

        public bool ExistemCamposVazios { private get; set; }
        public bool EhMovimentacao { private get; set; }

        public int IdStatusAprovacaoUnico
        {
            get { return _idStatusAprovacaoUnico; }
            set
            {
                if (value != _idStatusAprovacaoUnico)
                {
                    _idStatusAprovacaoUnico = value;
                    OnPropertyChanged(nameof(IdStatusAprovacaoUnico));
                }
            }
        }

        public int IdStatusAprovacaoTodos
        {
            get { return _idStatusAprovacaoTodos; }
            set
            {
                if (value != _idStatusAprovacaoTodos)
                {
                    _idStatusAprovacaoTodos = value;
                    OnPropertyChanged(nameof(IdStatusAprovacaoTodos));
                }
            }
        }

        public int IdJustificativaAprovacaoUnico
        {
            get { return _idJustificativaAprovacaoUnico; }
            set
            {
                if (value != _idJustificativaAprovacaoUnico)
                {
                    _idJustificativaAprovacaoUnico = value;
                    OnPropertyChanged(nameof(IdJustificativaAprovacaoUnico));
                }
            }
        }

        public int IdJustificativaAprovacaoTodos
        {
            get { return _idJustificativaAprovacaoTodos; }
            set
            {
                if (value != _idJustificativaAprovacaoTodos)
                {
                    _idJustificativaAprovacaoTodos = value;
                    OnPropertyChanged(nameof(IdJustificativaAprovacaoTodos));
                }
            }
        }

        public Conjunto ConjuntoUnico
        {
            get { return _conjuntoUnico; }
            set
            {
                if (value != _conjuntoUnico)
                {
                    _conjuntoUnico = value;
                    OnPropertyChanged(nameof(ConjuntoUnico));
                }
            }
        }

        public Conjunto ConjuntoTodos
        {
            get { return _conjuntoTodos; }
            set
            {
                if (value != _conjuntoTodos)
                {
                    _conjuntoTodos = value;
                    OnPropertyChanged(nameof(ConjuntoTodos));
                }
            }
        }

        public Especificacao EspecificacaoUnico
        {
            get { return _especificacaoUnico; }
            set
            {
                if (value != _especificacaoUnico)
                {
                    _especificacaoUnico = value;
                    OnPropertyChanged(nameof(EspecificacaoUnico));
                }
            }
        }

        public Especificacao EspecificacaoTodos
        {
            get { return _especificacaoTodos; }
            set
            {
                if (value != _especificacaoTodos)
                {
                    _especificacaoTodos = value;
                    OnPropertyChanged(nameof(EspecificacaoTodos));
                }
            }
        }

        public string FormatoTelefone
        {
            get { return _formatoTelefone; }
            set
            {
                _formatoTelefone = value;
                OnPropertyChanged(nameof(FormatoTelefone));
            }
        }

        public bool EdicaoEhEnterprise
        {
            get { return _edicaoEhEnterprise; }
            set
            {
                if (value != _edicaoEhEnterprise)
                {
                    _edicaoEhEnterprise = value;
                    OnPropertyChanged(nameof(EdicaoEhEnterprise));
                }
            }
        }

        public bool PermiteCopiar
        {
            get { return _permiteCopiar; }
            set
            {
                if (value != _permiteCopiar)
                {
                    _permiteCopiar = value;
                    OnPropertyChanged(nameof(PermiteCopiar));
                }
            }
        }

        public CopiarDeTermo? CopiarDe
        {
            get { return copiarDe; }
            set
            {
                if (value != copiarDe)
                {
                    copiarDe = value;
                    OnPropertyChanged(nameof(CopiarDe));
                }
            }
        }

        public Cliente? ClienteTodos
        {
            get { return _clienteTodos; }
            set
            {
                if (value != _clienteTodos)
                {
                    _clienteTodos = value;
                    OnPropertyChanged(nameof(ClienteTodos));

                    // Preenche a lista de termos do cliente selecionado
                    PreencheTermosCliente(ClienteTodos, ListaTermosClienteTodos).Await();
                }
            }
        }

        public Cliente? ClienteProposta
        {
            get { return _clienteProposta; }
            set
            {
                if (value != _clienteProposta)
                {
                    _clienteProposta = value;
                    OnPropertyChanged(nameof(ClienteProposta));

                    // Preenche a lista de propostas do cliente
                    PreenchePropostasParaTermo().Await();
                }
            }
        }

        public Proposta? PropostaTermo
        {
            get { return _propostaTermo; }
            set
            {
                if (value != _propostaTermo)
                {
                    _propostaTermo = value;
                    OnPropertyChanged(nameof(PropostaTermo));
                }
            }
        }

        public Setor? SetorTermo
        {
            get { return _setorTermo; }
            set
            {
                if (value != _setorTermo)
                {
                    _setorTermo = value;
                    OnPropertyChanged(nameof(SetorTermo));

                    // Preenche a lista de termos do cliente selecionado
                    PreencheTermosSetor().Await();
                }
            }
        }

        public Termo? TermoClienteAtual
        {
            get { return _termoClienteAtual; }
            set
            {
                if (value != _termoClienteAtual)
                {
                    _termoClienteAtual = value;
                    OnPropertyChanged(nameof(TermoClienteAtual));
                }
            }
        }

        public Termo? TermoClienteTodos
        {
            get { return _termoClienteTodos; }
            set
            {
                if (value != _termoClienteTodos)
                {
                    _termoClienteTodos = value;
                    OnPropertyChanged(nameof(TermoClienteTodos));
                }
            }
        }

        public Termo? TermoTodos
        {
            get { return _termoTodos; }
            set
            {
                if (value != _termoTodos)
                {
                    _termoTodos = value;
                    OnPropertyChanged(nameof(TermoTodos));
                }
            }
        }

        public Termo? TermoSetor
        {
            get { return _termoSetor; }
            set
            {
                if (value != _termoSetor)
                {
                    _termoSetor = value;
                    OnPropertyChanged(nameof(TermoSetor));
                }
            }
        }

        public Pais? Pais
        {
            get { return _pais; }
            set
            {
                if (value != _pais)
                {
                    _pais = value;
                    OnPropertyChanged(nameof(Pais));
                    ListaEstadosView.Filter = FiltraEstados;
                    CollectionViewSource.GetDefaultView(ListaEstados).Refresh();
                }
            }
        }

        public Estado? Estado
        {
            get { return _estado; }
            set
            {
                if (value != _estado)
                {
                    _estado = value;
                    OnPropertyChanged(nameof(Estado));
                    ListaCidadesView.Filter = FiltraCidades;
                    CollectionViewSource.GetDefaultView(ListaCidades).Refresh();
                }
            }
        }

        public Cidade? Cidade
        {
            get { return _cidade; }
            set
            {
                if (value != _cidade)
                {
                    _cidade = value;
                    OnPropertyChanged(nameof(Cidade));
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
                    ListaModelosView.Filter = FiltraModelos;
                    CollectionViewSource.GetDefaultView(ListaModelos).Refresh();
                }
            }
        }

        public Categoria? Categoria
        {
            get { return _categoria; }
            set
            {
                if (value != _categoria)
                {
                    _categoria = value;
                    OnPropertyChanged(nameof(Categoria));
                    ListaModelosView.Filter = FiltraModelos;
                    CollectionViewSource.GetDefaultView(ListaModelos).Refresh();
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
                    ListaModelosView.Filter = FiltraModelos;
                    CollectionViewSource.GetDefaultView(ListaModelos).Refresh();
                }
            }
        }

        public Classe? Classe
        {
            get { return _classe; }
            set
            {
                if (value != _classe)
                {
                    _classe = value;
                    OnPropertyChanged(nameof(Classe));
                    ListaModelosView.Filter = FiltraModelos;
                    CollectionViewSource.GetDefaultView(ListaModelos).Refresh();
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

        public bool PermiteSalvar
        {
            get { return _permiteSalvar; }
            set
            {
                if (value != _permiteSalvar)
                {
                    _permiteSalvar = value;
                    OnPropertyChanged(nameof(PermiteSalvar));
                }
            }
        }

        public bool PermiteEditar
        {
            get { return _permiteEditar; }
            set
            {
                if (value != _permiteEditar)
                {
                    _permiteEditar = value;
                    OnPropertyChanged(nameof(PermiteEditar));
                }
            }
        }

        public bool PermiteRevisar
        {
            get { return _permiteRevisar; }
            set
            {
                if (value != _permiteRevisar)
                {
                    _permiteRevisar = value;
                    OnPropertyChanged(nameof(PermiteRevisar));
                }
            }
        }

        public bool PermiteDeletar
        {
            get { return _permiteDeletar; }
            set
            {
                if (value != _permiteDeletar)
                {
                    _permiteDeletar = value;
                    OnPropertyChanged(nameof(PermiteDeletar));
                }
            }
        }

        public bool PermiteVisualizar
        {
            get { return _permiteVisualizar; }
            set
            {
                if (value != _permiteVisualizar)
                {
                    _permiteVisualizar = value;
                    OnPropertyChanged(nameof(PermiteVisualizar));
                }
            }
        }

        public bool PermiteCancelarEdicao
        {
            get { return _permiteCancelarEdicao; }
            set
            {
                if (value != _permiteCancelarEdicao)
                {
                    _permiteCancelarEdicao = value;
                    OnPropertyChanged(nameof(PermiteCancelarEdicao));
                }
            }
        }

        public bool PermiteCancelar
        {
            get { return _permiteCancelar; }
            set
            {
                if (value != _permiteCancelar)
                {
                    _permiteCancelar = value;
                    OnPropertyChanged(nameof(PermiteCancelar));
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

        private CollectionView _listaContatosView;
        private CollectionView _listaEstadosView;
        private CollectionView _listaCidadesView;
        private CollectionView _listaSeriesView;
        private CollectionView _listaModelosView;

        public CollectionView ListaContatosView
        {
            get { return _listaContatosView; }
            set
            {
                if (_listaContatosView != value)
                {
                    _listaContatosView = value;
                    OnPropertyChanged(nameof(ListaContatosView));
                }
            }
        }

        public CollectionView ListaEstadosView
        {
            get { return _listaEstadosView; }
            set
            {
                if (_listaEstadosView != value)
                {
                    _listaEstadosView = value;
                    OnPropertyChanged(nameof(ListaEstadosView));
                }
            }
        }

        public CollectionView ListaCidadesView
        {
            get { return _listaCidadesView; }
            set
            {
                if (_listaCidadesView != value)
                {
                    _listaCidadesView = value;
                    OnPropertyChanged(nameof(ListaCidadesView));
                }
            }
        }

        public CollectionView ListaSeriesView
        {
            get { return _listaSeriesView; }
            set
            {
                if (_listaSeriesView != value)
                {
                    _listaSeriesView = value;
                    OnPropertyChanged(nameof(ListaSeriesView));
                }
            }
        }

        public CollectionView ListaModelosView
        {
            get { return _listaModelosView; }
            set
            {
                if (_listaModelosView != value)
                {
                    _listaModelosView = value;
                    OnPropertyChanged(nameof(ListaModelosView));
                }
            }
        }

        //public CollectionView ListaContatosView { get; private set; }
        //public CollectionView ListaEstadosView { get; private set; }
        //public CollectionView ListaCidadesView { get; private set; }
        //public CollectionView ListaSeriesView { get; private set; }
        //public CollectionView ListaModelosView { get; private set; }

        public ICommand ComandoSalvar
        {
            get
            {
                if (_comandoSalvar == null)
                {
                    _comandoSalvar = new RelayCommand(
                        param => SalvarProposta().Await(),
                        param => PermiteSalvar
                    );
                }
                return _comandoSalvar;
            }
        }

        public ICommand ComandoEditar
        {
            get
            {
                if (_comandoEditar == null)
                {
                    _comandoEditar = new RelayCommand(
                        param => EditarProposta().Await(),
                        param => PermiteEditar
                    );
                }
                return _comandoEditar;
            }
        }

        public ICommand ComandoRevisar
        {
            get
            {
                if (_comandoRevisar == null)
                {
                    _comandoRevisar = new RelayCommand(
                        param => RevisarProposta().Await(),
                        param => PermiteRevisar
                    );
                }
                return _comandoRevisar;
            }
        }

        public ICommand ComandoDeletar
        {
            get
            {
                if (_comandoDeletar == null)
                {
                    _comandoDeletar = new RelayCommand(
                        param => DeletarProposta().Await(),
                        param => PermiteDeletar
                    );
                }
                return _comandoDeletar;
            }
        }

        public ICommand ComandoVisualizarProposta
        {
            get
            {
                if (_comandoVisualizarProposta == null)
                {
                    _comandoVisualizarProposta = new RelayCommand(
                        param => VisualizarProposta(),
                        param => PermiteVisualizar
                    );
                }
                return _comandoVisualizarProposta;
            }
        }

        public ICommand ComandoAbrirOrdemServico
        {
            get
            {
                if (_comandoAbrirOrdemServico == null)
                {
                    _comandoAbrirOrdemServico = new RelayCommand(
                        param => AbrirOrdemServico().Await(),
                        param => true
                    );
                }
                return _comandoAbrirOrdemServico;
            }
        }

        public ICommand ComandoAlteraStatusAprovacaoProposta
        {
            get
            {
                if (_comandoAlteraStatusAprovacaoProposta == null)
                {
                    _comandoAlteraStatusAprovacaoProposta = new RelayCommand(
                        param => AlteraStatusAprovacaoProposta().Await(),
                        param => true
                    );
                }
                return _comandoAlteraStatusAprovacaoProposta;
            }
        }

        public ICommand ComandoAlteraJustificativaAprovacaoProposta
        {
            get
            {
                if (_comandoAlteraJustificativaAprovacaoProposta == null)
                {
                    _comandoAlteraJustificativaAprovacaoProposta = new RelayCommand(
                        param => AlteraJustificativaAprovacaoProposta().Await(),
                        param => true
                    );
                }
                return _comandoAlteraJustificativaAprovacaoProposta;
            }
        }

        public ICommand ComandoCancelarEdicao
        {
            get
            {
                if (_comandoCancelarEdicao == null)
                {
                    _comandoCancelarEdicao = new RelayCommand(
                        param => CancelarEdicaoOuRevisao().Await(),
                        param => PermiteCancelarEdicao
                    );
                }
                return _comandoCancelarEdicao;
            }
        }

        public ICommand ComandoCancelar
        {
            get
            {
                if (_comandoCancelar == null)
                {
                    _comandoCancelar = new RelayCommand(
                        param => Cancelar(),
                        param => true
                    );
                }
                return _comandoCancelar;
            }
        }

        public ICommand ComandoGetContato
        {
            get
            {
                if (_comandoGetContato == null)
                {
                    _comandoGetContato = new RelayCommand(
                        param => GetContatoAsync().Await(),
                        param => true
                    );
                }
                return _comandoGetContato;
            }
        }

        public ICommand ComandoGetSerie
        {
            get
            {
                if (_comandoGetSerie == null)
                {
                    _comandoGetSerie = new RelayCommand(
                        param => GetSerieAsync().Await(),
                        param => true
                    );
                }
                return _comandoGetSerie;
            }
        }

        public ICommand ComandoPreencheEstados
        {
            get
            {
                if (_comandoPreencheEstados == null)
                {
                    _comandoPreencheEstados = new RelayCommand(
                        param => PreencheEstadosAsync().Await(),
                        param => true
                    );
                }
                return _comandoPreencheEstados;
            }
        }

        public ICommand ComandoPreencheCidades
        {
            get
            {
                if (_comandoPreencheCidades == null)
                {
                    _comandoPreencheCidades = new RelayCommand(
                        param => PreencheCidadesAsync().Await(),
                        param => true
                    );
                }
                return _comandoPreencheCidades;
            }
        }

        public ICommand ComandoAdicionarPeca
        {
            get
            {
                if (_comandoAdicionarPeca == null)
                {
                    _comandoAdicionarPeca = new RelayCommand(
                        param => ControleItem(TipoItemAdicionar.Peca).Await(),
                        param => true
                    );
                }
                return _comandoAdicionarPeca;
            }
        }

        public ICommand ComandoAdicionarServico
        {
            get
            {
                if (_comandoAdicionarServico == null)
                {
                    _comandoAdicionarServico = new RelayCommand(
                        param => ControleItem(TipoItemAdicionar.Servico).Await(),
                        param => true
                    );
                }
                return _comandoAdicionarServico;
            }
        }

        public ICommand ComandoAdicionarDeslocamento
        {
            get
            {
                if (_comandoAdicionarDeslocamento == null)
                {
                    _comandoAdicionarDeslocamento = new RelayCommand(
                        param => ControleItem(TipoItemAdicionar.Deslocamento).Await(),
                        param => true
                    );
                }
                return _comandoAdicionarDeslocamento;
            }
        }

        public ICommand ComandoImportarItensKion
        {
            get
            {
                if (_comandoImportarItensKion == null)
                {
                    _comandoImportarItensKion = new RelayCommand(
                        param => ImportarItens(FornecedorImportar.Kion).Await(),
                        param => true
                    );
                }
                return _comandoImportarItensKion;
            }
        }

        public ICommand ComandoImportarItensTVH
        {
            get
            {
                if (_comandoImportarItensTVH == null)
                {
                    _comandoImportarItensTVH = new RelayCommand(
                        param => ImportarItens(FornecedorImportar.TVH).Await(),
                        param => true
                    );
                }
                return _comandoImportarItensTVH;
            }
        }

        public ICommand ComandoExportarFormatoFluig
        {
            get
            {
                if (_comandoExportarFormatoFluig == null)
                {
                    _comandoExportarFormatoFluig = new RelayCommand(
                        param => ExportarItens(ExcelClasses.TipoExportacaoItensProposta.FormatoFluig).Await(),
                        param => true
                    );
                }
                return _comandoExportarFormatoFluig;
            }
        }

        public ICommand ComandoExportarTabelaCompleta
        {
            get
            {
                if (_comandoExportarTabelaCompleta == null)
                {
                    _comandoExportarTabelaCompleta = new RelayCommand(
                        param => ExportarItens(ExcelClasses.TipoExportacaoItensProposta.TabelaCompleta).Await(),
                        param => true
                    );
                }
                return _comandoExportarTabelaCompleta;
            }
        }

        public ICommand ComandoAtualizarPrazoItensCodigoCompleto
        {
            get
            {
                if (_comandoAtualizarPrazoItensCodigoCompleto == null)
                {
                    _comandoAtualizarPrazoItensCodigoCompleto = new RelayCommand(
                        param => AtualizarPrazo(ExcelClasses.TipoInformacaoEstoque.CodigoCompleto).Await(),
                        param => true
                    );
                }
                return _comandoAtualizarPrazoItensCodigoCompleto;
            }
        }

        public ICommand ComandoAtualizarPrazoItensCodigoAbreviado
        {
            get
            {
                if (_comandoAtualizarPrazoItensCodigoAbreviado == null)
                {
                    _comandoAtualizarPrazoItensCodigoAbreviado = new RelayCommand(
                        param => AtualizarPrazo(ExcelClasses.TipoInformacaoEstoque.CodigoAbreviado).Await(),
                        param => true
                    );
                }
                return _comandoAtualizarPrazoItensCodigoAbreviado;
            }
        }

        public ICommand ComandoAtualizarPrazoItensDecidirPorPeca
        {
            get
            {
                if (_comandoAtualizarPrazoItensDecidirPorPeca == null)
                {
                    _comandoAtualizarPrazoItensDecidirPorPeca = new RelayCommand(
                        param => AtualizarPrazo(ExcelClasses.TipoInformacaoEstoque.PorPeca).Await(),
                        param => true
                    );
                }
                return _comandoAtualizarPrazoItensDecidirPorPeca;
            }
        }

        public ICommand ComandoFreteItensValorIgual
        {
            get
            {
                if (_comandoFreteItensValorIgual == null)
                {
                    _comandoFreteItensValorIgual = new RelayCommand(
                        param => DefinirFrete(TipoDefinicaoFrete.IgualParaTodos).Await(),
                        param => true
                    );
                }
                return _comandoFreteItensValorIgual;
            }
        }

        public ICommand ComandoFreteItensValorDivididoContagem
        {
            get
            {
                if (_comandoFreteItensValorDivididoContagem == null)
                {
                    _comandoFreteItensValorDivididoContagem = new RelayCommand(
                        param => DefinirFrete(TipoDefinicaoFrete.Contagem).Await(),
                        param => true
                    );
                }
                return _comandoFreteItensValorDivididoContagem;
            }
        }

        public ICommand ComandoFreteItensValorDivididoQuantidade
        {
            get
            {
                if (_comandoFreteItensValorDivididoQuantidade == null)
                {
                    _comandoFreteItensValorDivididoQuantidade = new RelayCommand(
                        param => DefinirFrete(TipoDefinicaoFrete.PorQuantidade).Await(),
                        param => true
                    );
                }
                return _comandoFreteItensValorDivididoQuantidade;
            }
        }

        public ICommand ComandoFreteItensValorDivididoPrecoUnitarioInicial
        {
            get
            {
                if (_comandoFreteItensValorDivididoPrecoUnitarioInicial == null)
                {
                    _comandoFreteItensValorDivididoPrecoUnitarioInicial = new RelayCommand(
                        param => DefinirFrete(TipoDefinicaoFrete.PorPrecoUnitarioInicial).Await(),
                        param => true
                    );
                }
                return _comandoFreteItensValorDivididoPrecoUnitarioInicial;
            }
        }

        public ICommand ComandoFreteItensValorDivididoPrecoUnitarioFinal
        {
            get
            {
                if (_comandoFreteItensValorDivididoPrecoUnitarioFinal == null)
                {
                    _comandoFreteItensValorDivididoPrecoUnitarioFinal = new RelayCommand(
                        param => DefinirFrete(TipoDefinicaoFrete.PorPrecoUnitarioFinal).Await(),
                        param => true
                    );
                }
                return _comandoFreteItensValorDivididoPrecoUnitarioFinal;
            }
        }

        public ICommand ComandoFreteItensValorDivididoPrecoTotalInicial
        {
            get
            {
                if (_comandoFreteItensValorDivididoPrecoTotalInicial == null)
                {
                    _comandoFreteItensValorDivididoPrecoTotalInicial = new RelayCommand(
                        param => DefinirFrete(TipoDefinicaoFrete.PorPrecoTotalInicial).Await(),
                        param => true
                    );
                }
                return _comandoFreteItensValorDivididoPrecoTotalInicial;
            }
        }

        public ICommand ComandoFreteItensValorDivididoPrecoTotalFinal
        {
            get
            {
                if (_comandoFreteItensValorDivididoPrecoTotalFinal == null)
                {
                    _comandoFreteItensValorDivididoPrecoTotalFinal = new RelayCommand(
                        param => DefinirFrete(TipoDefinicaoFrete.PorPrecoTotalFinal).Await(),
                        param => true
                    );
                }
                return _comandoFreteItensValorDivididoPrecoTotalFinal;
            }
        }

        public ICommand ComandoAplicarDescontoCusto
        {
            get
            {
                if (_comandoAplicarDescontoCusto == null)
                {
                    _comandoAplicarDescontoCusto = new RelayCommand(
                        param => DefinirDesconto(TipoDesconto.DescontoCusto).Await(),
                        param => true
                    );
                }
                return _comandoAplicarDescontoCusto;
            }
        }

        public ICommand ComandoAplicarDescontoFinalPecas
        {
            get
            {
                if (_comandoAplicarDescontoFinalPecas == null)
                {
                    _comandoAplicarDescontoFinalPecas = new RelayCommand(
                        param => DefinirDesconto(TipoDesconto.DescontoFinalApenasPecas).Await(),
                        param => true
                    );
                }
                return _comandoAplicarDescontoFinalPecas;
            }
        }

        public ICommand ComandoAplicarDescontoFinalServicos
        {
            get
            {
                if (_comandoAplicarDescontoFinalServicos == null)
                {
                    _comandoAplicarDescontoFinalServicos = new RelayCommand(
                        param => DefinirDesconto(TipoDesconto.DescontoFinalApenasServicos).Await(),
                        param => true
                    );
                }
                return _comandoAplicarDescontoFinalServicos;
            }
        }

        public ICommand ComandoAplicarDescontoFinalDeslocamentos
        {
            get
            {
                if (_comandoAplicarDescontoFinalDeslocamentos == null)
                {
                    _comandoAplicarDescontoFinalDeslocamentos = new RelayCommand(
                        param => DefinirDesconto(TipoDesconto.DescontoFinalApenasDeslocamentos).Await(),
                        param => true
                    );
                }
                return _comandoAplicarDescontoFinalDeslocamentos;
            }
        }

        public ICommand ComandoAplicarDescontoFinalTodosOsItens
        {
            get
            {
                if (_comandoAplicarDescontoFinalTodosOsItens == null)
                {
                    _comandoAplicarDescontoFinalTodosOsItens = new RelayCommand(
                        param => DefinirDesconto(TipoDesconto.DescontoFinalTodosOsItens).Await(),
                        param => true
                    );
                }
                return _comandoAplicarDescontoFinalTodosOsItens;
            }
        }

        public ICommand ComandoConfirmarCopia
        {
            get
            {
                if (_comandoConfirmarCopia == null)
                {
                    _comandoConfirmarCopia = new RelayCommand(
                        param => ConfirmarCopiaTermo().Await(),
                        param => true
                    );
                }
                return _comandoConfirmarCopia;
            }
        }

        public ICommand ComandoEditarItens
        {
            get
            {
                if (_comandoEditarItens == null)
                {
                    _comandoEditarItens = new RelayCommand(
                        param => ControleItem(itemProposta: ItemPropostaSelecionado).Await(),
                        param => ItemPropostaSelecionado != null
                    );
                }
                return _comandoEditarItens;
            }
        }

        public ICommand ComandoAlteraStatusAprovacaoUnico
        {
            get
            {
                if (_comandoAlteraStatusAprovacaoUnico == null)
                {
                    _comandoAlteraStatusAprovacaoUnico = new RelayCommand(
                        param => AlteraStatusAprovacaoUnico(IdStatusAprovacaoUnico).Await(),
                        param => ItemPropostaSelecionado != null
                    );
                }
                return _comandoAlteraStatusAprovacaoUnico;
            }
        }

        public ICommand ComandoAlteraStatusAprovacaoTodos
        {
            get
            {
                if (_comandoAlteraStatusAprovacaoTodos == null)
                {
                    _comandoAlteraStatusAprovacaoTodos = new RelayCommand(
                        param => AlteraStatusAprovacaoTodos(IdStatusAprovacaoTodos).Await(),
                        param => ItemPropostaSelecionado != null
                    );
                }
                return _comandoAlteraStatusAprovacaoTodos;
            }
        }

        public ICommand ComandoAlteraJustificativaAprovacaoUnico
        {
            get
            {
                if (_comandoAlteraJustificativaAprovacaoUnico == null)
                {
                    _comandoAlteraJustificativaAprovacaoUnico = new RelayCommand(
                        param => AlteraJustificativaAprovacaoUnico(IdJustificativaAprovacaoUnico).Await(),
                        param => ItemPropostaSelecionado != null
                    );
                }
                return _comandoAlteraJustificativaAprovacaoUnico;
            }
        }

        public ICommand ComandoAlteraJustificativaAprovacaoTodos
        {
            get
            {
                if (_comandoAlteraJustificativaAprovacaoTodos == null)
                {
                    _comandoAlteraJustificativaAprovacaoTodos = new RelayCommand(
                        param => AlteraJustificativaAprovacaoTodos(IdJustificativaAprovacaoTodos).Await(),
                        param => ItemPropostaSelecionado != null
                    );
                }
                return _comandoAlteraJustificativaAprovacaoTodos;
            }
        }

        public ICommand ComandoAlteraConjuntoUnico
        {
            get
            {
                if (_comandoAlteraConjuntoUnico == null)
                {
                    _comandoAlteraConjuntoUnico = new RelayCommand(
                        param => AlteraConjuntoUnico(ConjuntoUnico),
                        param => ItemPropostaSelecionado != null
                    );
                }
                return _comandoAlteraConjuntoUnico;
            }
        }

        public ICommand ComandoAlteraConjuntoTodos
        {
            get
            {
                if (_comandoAlteraConjuntoTodos == null)
                {
                    _comandoAlteraConjuntoTodos = new RelayCommand(
                        param => AlteraConjuntoTodos(ConjuntoTodos),
                        param => ItemPropostaSelecionado != null
                    );
                }
                return _comandoAlteraConjuntoTodos;
            }
        }

        public ICommand ComandoAlteraEspecificacaoUnico
        {
            get
            {
                if (_comandoAlteraEspecificacaoUnico == null)
                {
                    _comandoAlteraEspecificacaoUnico = new RelayCommand(
                        param => AlteraEspecificacaoUnico(EspecificacaoUnico),
                        param => ItemPropostaSelecionado != null
                    );
                }
                return _comandoAlteraEspecificacaoUnico;
            }
        }

        public ICommand ComandoAlteraEspecificacaoTodos
        {
            get
            {
                if (_comandoAlteraEspecificacaoTodos == null)
                {
                    _comandoAlteraEspecificacaoTodos = new RelayCommand(
                        param => AlteraEspecificacaoTodos(EspecificacaoTodos),
                        param => ItemPropostaSelecionado != null
                    );
                }
                return _comandoAlteraEspecificacaoTodos;
            }
        }

        public ICommand ComandoCopiarPropostaNovaProposta
        {
            get
            {
                if (_comandoCopiarPropostaNovaProposta == null)
                {
                    _comandoCopiarPropostaNovaProposta = new RelayCommand(
                        param => CopiarProposta(CopiarPropostaViewModel.TipoCopia.ParaProposta).Await(),
                        param => true
                    );
                }
                return _comandoCopiarPropostaNovaProposta;
            }
        }

        public ICommand ComandoCopiarPropostaNovaOrdemServico
        {
            get
            {
                if (_comandoCopiarPropostaNovaOrdemServico == null)
                {
                    _comandoCopiarPropostaNovaOrdemServico = new RelayCommand(
                        param => CopiarProposta(CopiarPropostaViewModel.TipoCopia.ParaOrdemServico).Await(),
                        param => true
                    );
                }
                return _comandoCopiarPropostaNovaOrdemServico;
            }
        }

        public ICommand ComandoVisibilidadeColunaTodas
        {
            get
            {
                if (_comandoVisibilidadeColunaTodos == null)
                {
                    _comandoVisibilidadeColunaTodos = new RelayCommand(
                        param =>
                        {
                            if (VisibilidadeColunaTodas)
                            {
                                TextoColunaTodas = "Exibir todas";
                                VisibilidadeColunaTodas = false;
                                VisibilidadeColunaOutrosDadosIniciais = false;
                                VisibilidadeColunaCalculos = false;
                                VisibilidadeColunaQuantidadeEstoque = false;
                            }
                            else
                            {
                                TextoColunaTodas = "Ocultar todas";
                                VisibilidadeColunaTodas = true;
                                VisibilidadeColunaOutrosDadosIniciais = true;
                                VisibilidadeColunaCalculos = true;
                                VisibilidadeColunaQuantidadeEstoque = true;
                            }
                        },
                        param => true
                    );
                }
                return _comandoVisibilidadeColunaTodos;
            }
        }

        public ICommand ComandoRemoverItens
        {
            get
            {
                if (_comandoRemoverItens == null)
                {
                    _comandoRemoverItens = new RelayCommand(
                        param => RemoverItens(ListaItensPropostaSelecionados).Await(),
                        param => ItemPropostaSelecionado != null
                    );
                }
                return _comandoRemoverItens;
            }
        }

        public Cliente Cliente
        {
            get { return _cliente; }
            set
            {
                if (value != _cliente)
                {
                    _cliente = value;
                    Proposta.Cliente = value;
                    OnPropertyChanged(nameof(Cliente));

                    // Notifica o estado de seleção do cliente
                    OnPropertyChanged(nameof(ClienteSelecionado));

                    // Preenche a lista de contatos
                    PreencheInformacoesClienteAsync().Await();

                    // Preenche a lista de termos do cliente atual
                    PreencheTermosCliente(Cliente, ListaTermosClienteAtual).Await();

                    ListaContatosView.Filter = FiltraContatos;
                    CollectionViewSource.GetDefaultView(ListaContatos).Refresh();

                    ListaSeriesView.Filter = FiltraSeries;
                    CollectionViewSource.GetDefaultView(ListaSeries).Refresh();
                }
            }
        }

        public Proposta Proposta
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

        public ItemProposta ItemPropostaSelecionado
        {
            get { return _itemPropostaSelecionado; }
            set
            {
                if (value != _itemPropostaSelecionado)
                {
                    _itemPropostaSelecionado = value;
                    OnPropertyChanged(nameof(ItemPropostaSelecionado));
                }
            }
        }

        public ObservableCollection<ItemProposta> ListaItensPropostaSelecionados
        {
            get { return _listaItensPropostaSelecionados; }
            set
            {
                if (value != _listaItensPropostaSelecionados)
                {
                    _listaItensPropostaSelecionados = value;
                    OnPropertyChanged(nameof(ListaItensPropostaSelecionados));
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

        public bool ProgressoVisivel
        {
            get { return _progressoVisivel; }
            set
            {
                if (value != _progressoVisivel)
                {
                    _progressoVisivel = value;
                    OnPropertyChanged(nameof(ProgressoVisivel));
                }
            }
        }

        public double ValorProgresso
        {
            get
            {
                return _valorProgresso;
            }
            set
            {
                if (value != _valorProgresso)
                {
                    _valorProgresso = value;
                    if (ProgressoEhIndeterminavel)
                    {
                        ProgressoVisivel = true;
                    }
                    else
                    {
                        ProgressoVisivel = ValorProgresso > 0 && ValorProgresso < 100;
                    }
                    OnPropertyChanged(nameof(ValorProgresso));
                }
            }
        }

        public bool ProgressoEhIndeterminavel
        {
            get { return _progressoEhIndeterminavel; }
            set
            {
                if (value != _progressoEhIndeterminavel)
                {
                    _progressoEhIndeterminavel = value;
                    if (ProgressoEhIndeterminavel)
                    {
                        ProgressoVisivel = true;
                    }
                    else
                    {
                        ProgressoVisivel = ValorProgresso > 0 && ValorProgresso < 100;
                    }
                    OnPropertyChanged(nameof(ProgressoEhIndeterminavel));
                }
            }
        }

        public string TextoColunaTodas
        {
            get { return _textoColunaTodas; }
            set
            {
                if (value != _textoColunaTodas)
                {
                    _textoColunaTodas = value;
                    OnPropertyChanged(nameof(TextoColunaTodas));
                }
            }
        }

        public string? TextoEnvio => Proposta?.DataEnvio == null ? "Não enviada" : "Enviada em " + Proposta.DataEnvio?.ToString("dd/MM/yyyy HH:mm");
        public string? TextoOrigem => Proposta?.PropostaOrigem?.Id > 0 ? "Revisão da proposta " + Proposta.PropostaOrigem.CodigoProposta : null;
        public string? TextoRevisao => Proposta?.UltimaProposta?.Id > 0 ? "Revisada em " + Proposta.UltimaProposta.DataInsercao?.ToString("dd/MM/yyyy HH:mm") + ". Código: " + Proposta.UltimaProposta.CodigoProposta : null;
        public bool ClienteSelecionado => Convert.ToBoolean(Cliente != null);
        public bool ContemPecasKion => Convert.ToBoolean(Proposta?.ListaItensProposta?.Where(forn => forn.Fornecedor?.Id == 1).Any());
        public bool ContemPecas => Convert.ToBoolean(Proposta?.ListaItensProposta?.Where(tip => tip.TipoItem?.Id == 1).Any());
        public bool ContemServicos => Convert.ToBoolean(Proposta?.ListaItensProposta?.Where(tip => tip.TipoItem?.Id == 2).Any());
        public bool ContemDeslocamentos => Convert.ToBoolean(Proposta?.ListaItensProposta?.Where(tip => tip.TipoItem?.Id == 3).Any());

        public string? TextoTotalPecas => Convert.ToBoolean(Proposta?.ListaItensProposta?.Where(tiit => tiit.TipoItem.Id == 1).Any()) ?
            "Total de peças: " + Convert.ToDecimal(Proposta?.ListaItensProposta?.Where(tiit => tiit.TipoItem.Id == 1).Sum(prec => prec.PrecoTotalFinalItem)).ToString("C2") :
            null;

        public string? TextoTotalServicos => Convert.ToBoolean(Proposta?.ListaItensProposta?.Where(tiit => tiit.TipoItem.Id == 2).Any()) ?
            "Total de serviços: " + Convert.ToDecimal(Proposta?.ListaItensProposta?.Where(tiit => tiit.TipoItem.Id == 2).Sum(prec => prec.PrecoTotalFinalItem)).ToString("C2") :
            null;

        public string? TextoTotalDeslocamentos => Convert.ToBoolean(Proposta?.ListaItensProposta?.Where(tiit => tiit.TipoItem.Id == 3).Any()) ?
             "Total de deslocamentos: " + Convert.ToDecimal(Proposta?.ListaItensProposta?.Where(tiit => tiit.TipoItem.Id == 3).Sum(prec => prec.PrecoTotalFinalItem)).ToString("C2") :
             null;

        public string? TextoTotalGeral => Convert.ToBoolean(Proposta?.ListaItensProposta?.Count > 0) ?
            "Total geral: " + Convert.ToDecimal(Proposta?.ListaItensProposta?.Sum(prec => prec.PrecoTotalFinalItem)).ToString("C2") :
            null;

        public decimal? ValorFrete
        {
            get
            {
                return _valorFrete;
            }
            set
            {
                if (value != _valorFrete)
                {
                    _valorFrete = value;
                    OnPropertyChanged(nameof(ValorFrete));
                }
            }
        }

        public decimal? DescontoCusto
        {
            get
            {
                return _descontoCusto;
            }
            set
            {
                if (value != _descontoCusto)
                {
                    _descontoCusto = value;
                    OnPropertyChanged(nameof(DescontoCusto));
                }
            }
        }

        public decimal? DescontoFinalPecas
        {
            get
            {
                return _descontoFinalPecas;
            }
            set
            {
                if (value != _descontoFinalPecas)
                {
                    _descontoFinalPecas = value;
                    OnPropertyChanged(nameof(DescontoFinalPecas));
                }
            }
        }

        public decimal? DescontoFinalServicos
        {
            get
            {
                return _descontoFinalServicos;
            }
            set
            {
                if (value != _descontoFinalServicos)
                {
                    _descontoFinalServicos = value;
                    OnPropertyChanged(nameof(DescontoFinalServicos));
                }
            }
        }

        public decimal? DescontoFinalDeslocamentos
        {
            get
            {
                return _descontoFinalDeslocamentos;
            }
            set
            {
                if (value != _descontoFinalDeslocamentos)
                {
                    _descontoFinalDeslocamentos = value;
                    OnPropertyChanged(nameof(DescontoFinalDeslocamentos));
                }
            }
        }

        public decimal? DescontoFinalTodosOsItens
        {
            get
            {
                return _descontoFinalTodosOsItens;
            }
            set
            {
                if (value != _descontoFinalTodosOsItens)
                {
                    _descontoFinalTodosOsItens = value;
                    OnPropertyChanged(nameof(DescontoFinalTodosOsItens));
                }
            }
        }

        public CopiarDeTermo CopiarTermo { get; set; }

        public ObservableCollection<Cliente> ListaClientes
        {
            get { return _listaClientes; }
            set
            {
                if (value != _listaClientes)
                {
                    _listaClientes = value;
                    OnPropertyChanged(nameof(ListaClientes));
                }
            }
        }

        public ObservableCollection<Contato> ListaContatos
        {
            get { return _listaContatos; }
            set
            {
                if (value != _listaContatos)
                {
                    _listaContatos = value;
                    OnPropertyChanged(nameof(ListaContatos));
                }
            }
        }

        public ObservableCollection<Pais> ListaPaises
        {
            get { return _listaPaises; }
            set
            {
                if (value != _listaPaises)
                {
                    _listaPaises = value;
                    OnPropertyChanged(nameof(ListaPaises));
                }
            }
        }

        public ObservableCollection<Estado> ListaEstados
        {
            get { return _listaEstados; }
            set
            {
                if (value != _listaEstados)
                {
                    _listaEstados = value;
                    OnPropertyChanged(nameof(ListaEstados));
                }
            }
        }

        public ObservableCollection<Cidade> ListaCidades
        {
            get { return _listaCidades; }
            set
            {
                if (value != _listaCidades)
                {
                    _listaCidades = value;
                    OnPropertyChanged(nameof(ListaCidades));
                }
            }
        }

        public ObservableCollection<Serie> ListaSeries
        {
            get { return _listaSeries; }
            set
            {
                if (value != _listaSeries)
                {
                    _listaSeries = value;
                    OnPropertyChanged(nameof(ListaSeries));
                }
            }
        }

        public ObservableCollection<Fabricante> ListaFabricantes
        {
            get { return _listaFabricantes; }
            set
            {
                if (value != _listaFabricantes)
                {
                    _listaFabricantes = value;
                    OnPropertyChanged(nameof(ListaFabricantes));
                }
            }
        }

        public ObservableCollection<Categoria> ListaCategorias
        {
            get { return _listaCategorias; }
            set
            {
                if (value != _listaCategorias)
                {
                    _listaCategorias = value;
                    OnPropertyChanged(nameof(ListaCategorias));
                }
            }
        }

        public ObservableCollection<TipoEquipamento> ListaTiposEquipamento
        {
            get { return _listaTiposEquipamento; }
            set
            {
                if (value != _listaTiposEquipamento)
                {
                    _listaTiposEquipamento = value;
                    OnPropertyChanged(nameof(ListaTiposEquipamento));
                }
            }
        }

        public ObservableCollection<Classe> ListaClasses
        {
            get { return _listaClasses; }
            set
            {
                if (value != _listaClasses)
                {
                    _listaClasses = value;
                    OnPropertyChanged(nameof(ListaClasses));
                }
            }
        }

        public ObservableCollection<Modelo> ListaModelos
        {
            get { return _listaModelos; }
            set
            {
                if (value != _listaModelos)
                {
                    _listaModelos = value;
                    OnPropertyChanged(nameof(ListaModelos));
                }
            }
        }

        public ObservableCollection<Ano> ListaAnos
        {
            get { return _listaAnos; }
            set
            {
                if (value != _listaAnos)
                {
                    _listaAnos = value;
                    OnPropertyChanged(nameof(ListaAnos));
                }
            }
        }

        public ObservableCollection<Prioridade> ListaPrioridades
        {
            get { return _listaPrioridades; }
            set
            {
                if (value != _listaPrioridades)
                {
                    _listaPrioridades = value;
                    OnPropertyChanged(nameof(ListaPrioridades));
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

        public ObservableCollection<int?> ListaNumeroOrdensServico
        {
            get { return _listaNumeroOrdensServico; }
            set
            {
                if (value != _listaNumeroOrdensServico)
                {
                    _listaNumeroOrdensServico = value;
                    OnPropertyChanged(nameof(ListaNumeroOrdensServico));
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

        public ObservableCollection<Conjunto> ListaConjunto
        {
            get { return _listaConjunto; }
            set
            {
                if (value != _listaConjunto)
                {
                    _listaConjunto = value;
                    OnPropertyChanged(nameof(ListaConjunto));
                }
            }
        }

        public ObservableCollection<Especificacao> ListaEspecificacao
        {
            get { return _listaEspecificacao; }
            set
            {
                if (value != _listaEspecificacao)
                {
                    _listaEspecificacao = value;
                    OnPropertyChanged(nameof(ListaEspecificacao));
                }
            }
        }

        public ObservableCollection<CopiarDeTermo> ListaCopiarDe
        {
            get { return _listaCopiarDe; }
            set
            {
                if (value != _listaCopiarDe)
                {
                    _listaCopiarDe = value;
                    OnPropertyChanged(nameof(ListaCopiarDe));
                }
            }
        }

        public ObservableCollection<Termo> ListaTermosClienteAtual
        {
            get { return _listaTermosClienteAtual; }
            set
            {
                if (value != _listaTermosClienteAtual)
                {
                    _listaTermosClienteAtual = value;
                    OnPropertyChanged(nameof(ListaTermosClienteAtual));
                }
            }
        }

        public ObservableCollection<Termo> ListaTermosClienteTodos
        {
            get { return _listaTermosClienteTodos; }
            set
            {
                if (value != _listaTermosClienteTodos)
                {
                    _listaTermosClienteTodos = value;
                    OnPropertyChanged(nameof(ListaTermosClienteTodos));
                }
            }
        }

        public ObservableCollection<Termo> ListaTermos
        {
            get { return _listaTermos; }
            set
            {
                if (value != _listaTermos)
                {
                    _listaTermos = value;
                    OnPropertyChanged(nameof(ListaTermos));
                }
            }
        }

        public ObservableCollection<Proposta> ListaPropostas
        {
            get { return _listaPropostas; }
            set
            {
                if (value != _listaPropostas)
                {
                    _listaPropostas = value;
                    OnPropertyChanged(nameof(ListaPropostas));
                }
            }
        }

        public ObservableCollection<Setor> ListaSetores
        {
            get { return _listaSetores; }
            set
            {
                if (value != _listaSetores)
                {
                    _listaSetores = value;
                    OnPropertyChanged(nameof(ListaSetores));
                }
            }
        }

        public ObservableCollection<Termo> ListaTermosSetor
        {
            get { return _listaTermosSetor; }
            set
            {
                if (value != _listaTermosSetor)
                {
                    _listaTermosSetor = value;
                    OnPropertyChanged(nameof(ListaTermosSetor));
                }
            }
        }

        public bool DeletarVisivel
        {
            get { return _deletarVisivel; }
            set
            {
                if (value != _deletarVisivel)
                {
                    _deletarVisivel = value;
                    OnPropertyChanged(nameof(DeletarVisivel));
                }
            }
        }

        public bool CancelarEdicaoVisivel
        {
            get { return _cancelarEdicaoVisivel; }
            set
            {
                if (value != _cancelarEdicaoVisivel)
                {
                    _cancelarEdicaoVisivel = value;
                    OnPropertyChanged(nameof(CancelarEdicaoVisivel));
                }
            }
        }

        public bool CancelarVisivel
        {
            get { return _cancelarVisivel; }
            set
            {
                if (value != _cancelarVisivel)
                {
                    _cancelarVisivel = value;
                    OnPropertyChanged(nameof(CancelarVisivel));
                }
            }
        }

        public bool EdicaoHabilitada
        {
            get { return _edicaoHabilitada; }
            set
            {
                if (value != _edicaoHabilitada)
                {
                    _edicaoHabilitada = value;
                    OnPropertyChanged(nameof(EdicaoHabilitada));
                }
            }
        }

        public bool EdicaoSolicitante
        {
            get { return _edicaoSolicitante; }
            set
            {
                if (value != _edicaoSolicitante)
                {
                    _edicaoSolicitante = value;
                    OnPropertyChanged(nameof(EdicaoSolicitante));
                }
            }
        }

        public bool EdicaoFaturamento
        {
            get { return _edicaoFaturamento; }
            set
            {
                if (value != _edicaoFaturamento)
                {
                    _edicaoFaturamento = value;
                    OnPropertyChanged(nameof(EdicaoFaturamento));
                }
            }
        }

        public bool EdicaoProposta
        {
            get { return _edicaoProposta; }
            set
            {
                if (value != _edicaoProposta)
                {
                    _edicaoProposta = value;
                    OnPropertyChanged(nameof(EdicaoProposta));
                }
            }
        }

        public bool EdicaoEquipamento
        {
            get { return _edicaoEquipamento; }
            set
            {
                if (value != _edicaoEquipamento)
                {
                    _edicaoEquipamento = value;
                    OnPropertyChanged(nameof(EdicaoEquipamento));
                }
            }
        }

        public bool EdicaoComentarios
        {
            get { return _edicaoComentarios; }
            set
            {
                if (value != _edicaoComentarios)
                {
                    _edicaoComentarios = value;
                    OnPropertyChanged(nameof(EdicaoComentarios));
                }
            }
        }

        public bool ContextMenuVisivel
        {
            get { return _contextMenuVisivel; }
            set
            {
                if (value != _contextMenuVisivel)
                {
                    _contextMenuVisivel = value;
                    OnPropertyChanged(nameof(ContextMenuVisivel));
                }
            }
        }

        public bool VisibilidadeColunaTodas
        {
            get { return _visibilidadeColunaTodas; }
            set
            {
                if (value != _visibilidadeColunaTodas)
                {
                    _visibilidadeColunaTodas = value;
                    OnPropertyChanged(nameof(VisibilidadeColunaTodas));
                }
            }
        }

        public bool VisibilidadeColunaOutrosDadosIniciais
        {
            get { return _visibilidadeColunaOutrosDadosIniciais; }
            set
            {
                if (value != _visibilidadeColunaOutrosDadosIniciais)
                {
                    _visibilidadeColunaOutrosDadosIniciais = value;
                    OnPropertyChanged(nameof(VisibilidadeColunaOutrosDadosIniciais));
                }
            }
        }

        public bool VisibilidadeColunaCalculos
        {
            get { return _visibilidadeColunaCalculos; }
            set
            {
                if (value != _visibilidadeColunaCalculos)
                {
                    _visibilidadeColunaCalculos = value;
                    OnPropertyChanged(nameof(VisibilidadeColunaCalculos));
                }
            }
        }

        public bool VisibilidadeColunaQuantidadeEstoque
        {
            get { return _visibilidadeColunaQuantidadeEstoque; }
            set
            {
                if (value != _visibilidadeColunaQuantidadeEstoque)
                {
                    _visibilidadeColunaQuantidadeEstoque = value;
                    OnPropertyChanged(nameof(VisibilidadeColunaQuantidadeEstoque));
                }
            }
        }

        #endregion Propriedades/Comandos

        #region Enums

        private enum TipoItemAdicionar
        {
            Peca = 1,
            Servico = 2,
            Deslocamento = 3
        }

        private enum FornecedorImportar
        {
            Kion = 1,
            TVH = 2
        }

        /// <summary>
        /// Enum para o tipo de exibição
        /// </summary>
        private enum ColunaItens
        {
            OutrosDadosIniciais,
            Calculos,
            QuantidadeEstoque
        }

        #endregion Enums

        #region Métodos

        /// <summary>
        /// Método assíncrono que serve como construtor da proposta já que construtores não podem ser assíncronos
        /// </summary>
        private async Task ConstrutorAsync(Proposta? proposta = null)
        {
            try
            {
                // Preenche as listas com as classes necessárias
                await Cliente.PreencheListaClientesAsync(ListaClientes, true, null, CancellationToken.None, "WHERE clie.id_status = 1 ORDER BY clie.nome ASC", "");
                await Prioridade.PreencheListaPrioridadesAsync(ListaPrioridades, true, null, CancellationToken.None, "", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");
                await StatusAprovacao.PreencheListaStatusAprovacaoAsync(ListaStatusAprovacao, true, null, CancellationToken.None, "WHERE stap.id_status = 1", "");
                await JustificativaAprovacao.PreencheListaJustificativasAprovacaoAsync(ListaJustificativasAprovacao, true, null, CancellationToken.None, "WHERE juap.id_status = 1", "");
                await Termo.PreencheListaTermosAsync(ListaTermos, true, null, CancellationToken.None, "WHERE stat.id_status = 1", "");
                await Setor.PreencheListaSetoresAsync(ListaSetores, true, null, CancellationToken.None, "WHERE stat.id_status = 1", "");
                await Contato.PreencheListaContatosAsync(ListaContatos, true, null, CancellationToken.None, "WHERE cont.id_status = 1", "");
                await Pais.PreencheListaPaisesAsync(ListaPaises, true, null, CancellationToken.None, "WHERE pais.id_status = 1", "");
                await Estado.PreencheListaEstadosAsync(ListaEstados, true, null, CancellationToken.None, "WHERE esta.id_status = 1", "");
                await Cidade.PreencheListaCidadesAsync(ListaCidades, true, null, CancellationToken.None, "WHERE cida.id_status = 1", "");
                await Serie.PreencheListaSeriesAsync(ListaSeries, true, null, CancellationToken.None, "WHERE seri.id_status = 1", "");
                await Fabricante.PreencheListaFabricantesAsync(ListaFabricantes, true, null, CancellationToken.None, "WHERE fabr.id_status = 1 ORDER BY fabr.nome ASC", "");
                await Categoria.PreencheListaCategoriasAsync(ListaCategorias, true, null, CancellationToken.None, "WHERE cate.id_status = 1 ORDER BY cate.nome ASC", "");
                await TipoEquipamento.PreencheListaTiposEquipamentoAsync(ListaTiposEquipamento, true, null, CancellationToken.None, "WHERE tieq.id_status = 1 ORDER BY tieq.nome ASC", "");
                await Classe.PreencheListaClassesAsync(ListaClasses, true, null, CancellationToken.None, "WHERE clas.id_status = 1 ORDER BY clas.nome ASC", "");
                await Modelo.PreencheListaModelosAsync(ListaModelos, true, null, CancellationToken.None, "WHERE mode.id_status = 1 ORDER BY mode.nome ASC", "");
                await Ano.PreencheListaAnosAsync(ListaAnos, true, null, CancellationToken.None, "WHERE ano <= @ano", "@ano", DateTime.Now.Year + 1);
                await FuncoesDeDatabase.PreencheListaIntNullAsync(ListaNumeroOrdensServico, true, null, CancellationToken.None, "SELECT DISTINCT ordem_servico_atual FROM tb_ordens_servico WHERE id_status = 1 ORDER BY ordem_servico_atual ASC", "");

                ListaConjunto.Add(new Conjunto() { Id = 0, Nome = "NENHUM CONJUNTO", Status = new Status() { Id = 1, Nome = "Ativo" } });
                //ListaEspecificacao.Add(new Especificacao() { Id = 0, Nome = "LIMPAR", Status = new Status() { Id = 1, Nome = "Ativo" } });

                await Conjunto.PreencheListaConjuntosAsync(ListaConjunto, false, null, CancellationToken.None, "WHERE conj.id_status = 1 ORDER BY conj.nome ASC", "");
                await Especificacao.PreencheListaEspecificacoesAsync(ListaEspecificacao, false, null, CancellationToken.None, "WHERE espe.id_status = 1 ORDER BY espe.nome ASC", "");

                foreach (var conjunto in ListaConjunto)
                {
                    conjunto.ListaEspecificacoes = new();

                    if (ListaEspecificacao.Where(e => e.Conjunto.Id == conjunto.Id).Any())
                    {
                        foreach (var especificacao in ListaEspecificacao.Where(e => e.Conjunto.Id == conjunto.Id).ToList())
                        {
                            if (!conjunto.ListaEspecificacoes.Where(d => d.Id == 0).Any())
                            {
                                conjunto.ListaEspecificacoes.Add(new Especificacao() { Id = 0, Nome = "NENHUMA ESPECIFICAÇÃO", Status = new Status() { Id = 1, Nome = "Ativo" } });
                            }
                            conjunto.ListaEspecificacoes.Add(especificacao);
                        }
                    }
                }

                ListaContatosView = GetContatoCollectionView(ListaContatos);
                ListaEstadosView = GetEstadoCollectionView(ListaEstados);
                ListaCidadesView = GetCidadeCollectionView(ListaCidades);
                ListaSeriesView = GetSerieCollectionView(ListaSeries);
                ListaModelosView = GetModeloCollectionView(ListaModelos);

                ListaContatosView.Filter = LimpaListaContatos;
                CollectionViewSource.GetDefaultView(ListaContatos).Refresh();

                ListaEstadosView.Filter = LimpaListaEstados;
                CollectionViewSource.GetDefaultView(ListaEstados).Refresh();

                ListaCidadesView.Filter = LimpaListaCidades;
                CollectionViewSource.GetDefaultView(ListaCidades).Refresh();

                ListaSeriesView.Filter = LimpaListaSeries;
                CollectionViewSource.GetDefaultView(ListaSeries).Refresh();

                ListaModelosView.Filter = LimpaListaModelos;
                CollectionViewSource.GetDefaultView(ListaModelos).Refresh();

                // Limita os anos trazendo apenas valores únicos
                ListaAnos.DistinctBy(test => test.AnoValor);

                // Adiciona os tipos de cópia dos termos
                ListaCopiarDe.Add(new CopiarDeTermo(1, "Cliente (atual)"));
                ListaCopiarDe.Add(new CopiarDeTermo(2, "Cliente (todos)"));
                ListaCopiarDe.Add(new CopiarDeTermo(3, "Cliente e proposta"));
                ListaCopiarDe.Add(new CopiarDeTermo(4, "Setor"));
                ListaCopiarDe.Add(new CopiarDeTermo(5, "Termo padrão"));

                // Se a proposta não foi informada, cria uma nova instância de proposta
                if (proposta == null)
                {
                    Proposta = new(true);

                    Proposta.IdUsuarioEmUso = App.Usuario.Id;

                    Proposta.DataSolicitacao = DateTime.Now;
                    Proposta.Status = ListaStatus.First(stat => stat.Id == 1);
                    Proposta.Prioridade = ListaPrioridades.First(prio => prio.Id == 1);

                    try
                    {
                        Proposta.StatusAprovacao = ListaStatusAprovacao.First(stap => stap.Id == 3);
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        Proposta.JustificativaAprovacao = ListaJustificativasAprovacao.First(juap => juap.Id == 1);
                    }
                    catch (Exception)
                    {
                    }

                    Proposta.Filial = App.Usuario.Filial;
                    Proposta.UsuarioInsercao = (Usuario)App.Usuario.Clone();

                    Name = "Nova proposta";
                    DeletarVisivel = false;
                    EdicaoHabilitada = true;
                    EdicaoSolicitante = true;
                    EdicaoFaturamento = true;
                    EdicaoProposta = true;
                    EdicaoEquipamento = true;
                    EdicaoComentarios = true;
                    ContextMenuVisivel = true;

                    PermiteSalvar = true;
                    PermiteVisualizar = false;
                    PermiteEditar = false;
                    PermiteCopiar = false;
                    PermiteRevisar = false;
                    PermiteDeletar = false;
                }
                else
                {
                    Proposta = proposta;

                    await CarregarProposta();
                }

                Proposta.ListaItensProposta.CollectionChanged += (s, e) => OnPropertyChanged(nameof(ContemPecasKion));
                Proposta.ListaItensProposta.CollectionChanged += (s, e) => OnPropertyChanged(nameof(ContemPecas));
                Proposta.ListaItensProposta.CollectionChanged += (s, e) => OnPropertyChanged(nameof(ContemServicos));
                Proposta.ListaItensProposta.CollectionChanged += (s, e) => OnPropertyChanged(nameof(ContemDeslocamentos));
                Proposta.ListaItensProposta.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TextoTotalPecas));
                Proposta.ListaItensProposta.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TextoTotalServicos));
                Proposta.ListaItensProposta.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TextoTotalDeslocamentos));
                Proposta.ListaItensProposta.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TextoTotalGeral));

                Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(TextoEnvio));
                Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(TextoOrigem));
                Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(TextoRevisao));

                if (Proposta?.Contato?.Telefone != null)
                {
                    FormatoTelefone = Proposta?.Contato?.Telefone.Length > 10 ? @"\(00\)\ 00000\-0000" : @"\(00\)\ 0000\-0000";
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
            Messenger.Default.Send<IPageViewModel>(this, "SelecionaPrimeiraPagina");
        }

        private async Task EditarProposta()
        {
            // Retorna o id do usuário em uso
            await Proposta.GetIdUsuarioEmUsoAsync(CancellationToken.None);

            // Verifica se o id do usuário em uso não é nulo e, caso verdadeiro, impede a edição
            if (Proposta.IdUsuarioEmUso != null && Proposta.IdUsuarioEmUso != App.Usuario.Id)
            {
                // Define o usuário
                Usuario usuarioEmUso = new();
                await usuarioEmUso.GetUsuarioDatabaseAsync(Proposta.IdUsuarioEmUso, CancellationToken.None);

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Proposta em uso", "A proposta em questão está sendo editada por " + usuarioEmUso.Nome + ", portanto não será possível edita-la. Aguarde o usuário finalizar a edição da proposta para poder edita-la.", MessageDialogStyle.Affirmative, mySettings);

                usuarioEmUso = null;

                return;
            }

            // Verifica se existem itens na proposta e, caso contrário, retorna mensagem
            if (Proposta.DataEnvio != null)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Entendi"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Informação", "Essa proposta já foi enviada, portanto só será possível alterar os dados de faturamento, " +
                        "comentários e aprovação dos itens. Caso queira alterar outras informações, revise a proposta através da opção 'Revisar'", MessageDialogStyle.Affirmative, mySettings);

                EdicaoHabilitada = false;

                EdicaoSolicitante = false;
                EdicaoFaturamento = true;
                EdicaoProposta = false;
                EdicaoEquipamento = false;
                EdicaoComentarios = true;
                ContextMenuVisivel = true;

                CancelarEdicaoVisivel = true;
                PermiteSalvar = true;
                PermiteEditar = false;
                PermiteCopiar = true;
                PermiteRevisar = false;
                PermiteDeletar = false;
                PermiteVisualizar = false;
                PermiteCancelarEdicao = true;

                ehRevisao = false;
            }
            else
            {
                EdicaoHabilitada = true;

                EdicaoSolicitante = true;
                EdicaoFaturamento = true;
                EdicaoProposta = true;
                EdicaoEquipamento = true;
                EdicaoComentarios = true;
                ContextMenuVisivel = true;

                CancelarEdicaoVisivel = true;
                PermiteSalvar = true;
                PermiteEditar = false;
                PermiteCopiar = true;
                PermiteRevisar = false;
                PermiteDeletar = false;
                PermiteVisualizar = false;
                PermiteCancelarEdicao = true;

                ehRevisao = false;
            }
            ehEdicaoLimitada = true;

            Proposta.UsuarioEdicao = (Usuario)App.Usuario.Clone();
            Proposta.DataEdicao = DateTime.Now;
            Proposta.IdUsuarioEmUso = App.Usuario.Id;

            await Proposta.AtualizaIdUsuarioEmUsoAsync(CancellationToken.None);
        }

        private async Task RevisarProposta()
        {
            // Verifica se existem itens na proposta e, caso contrário, retorna mensagem
            if (Proposta.DataEnvio != null)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Entendi"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Informação", "Lembre-se de atualizar o prazo dos itens após a revisão, pois pode ter acontecido mudança no prazo. " +
                        "É extremamente recomendado a verificação dos preços pois estes podem ter sofrido alteração desde a última proposta", MessageDialogStyle.Affirmative, mySettings);
            }

            EdicaoHabilitada = true;

            EdicaoSolicitante = true;
            EdicaoFaturamento = true;
            EdicaoProposta = true;
            EdicaoEquipamento = true;
            EdicaoComentarios = true;
            ContextMenuVisivel = true;

            CancelarEdicaoVisivel = true;
            PermiteSalvar = true;
            PermiteEditar = false;
            PermiteCopiar = false;
            PermiteRevisar = false;
            PermiteDeletar = false;
            PermiteVisualizar = false;
            PermiteCancelarEdicao = true;

            ehEdicaoLimitada = false;
            ehRevisao = true;
            idPropostaOriginal = Proposta.Id;

            await Proposta.PropostaOrigem.GetPropostaDatabaseAsync(Proposta.Id, CancellationToken.None);

            Proposta.Id = null;
            Proposta.UltimaProposta = null;
            Proposta.DataEnvio = null;
            Proposta.DataEnvioFaturamento = null;
            Proposta.DataFaturamento = null;
            Proposta.NotaFiscal = null;
            Proposta.ValorFaturamento = null;
            atualizouPrazo = false;

            foreach (var item in Proposta.ListaItensProposta)
            {
                item.Id = null;
                item.DataInsercao = DateTime.Now;
                item.DataEdicaoItem = null;
                item.Usuario = (Usuario)App.Usuario.Clone();
                item.QuantidadeEstoqueCodigoAbreviadoItem = null;
                item.QuantidadeEstoqueCodigoCompletoItem = null;
                item.InformacaoEstoqueCodigoAbreviadoItem = null;
                item.InformacaoEstoqueCodigoCompletoItem = null;

                if (item.TipoItem.Id == 1)
                {
                    item.PrazoFinalItem = null;
                }

                await item.StatusAprovacao.GetStatusAprovacaoDatabaseAsync(3, CancellationToken.None);
                await item.JustificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(1, CancellationToken.None);

                await item.CalculaValoresItemPropostaAsync(Proposta.Cliente);
            }

            Proposta.CodigoProposta = await Proposta.GetCodigoPropostaRevisadaAsync(CancellationToken.None);

            OnPropertyChanged(nameof(Proposta));
        }

        /// <summary>
        /// Método para preencher as informações provenientes do cliente selecionado
        /// </summary>
        private async Task PreencheInformacoesClienteAsync()
        {
            if (Proposta.Cliente != null)
            {
                if (Proposta.Id == null && !ehRevisao)
                {
                    Proposta.CodigoProposta = DateTime.Now.ToString("ddMMyyyy") + ExcelClasses.ConverterColunaNumericaParaLetra(await Proposta.GetContagemPropostasIniciaisAsync(DateTime.Now, CancellationToken.None) + 1);
                }

                //await Serie.PreencheListaSeriesAsync(ListaSeries, true, null, CancellationToken.None, "WHERE clie.id_cliente = @id_cliente", "@id_cliente", Proposta.Cliente.Id);
                //await Contato.PreencheListaContatosAsync(ListaContatos, true, null, CancellationToken.None, "WHERE clie.id_cliente = @id_cliente", "@id_cliente", Proposta.Cliente.Id);

                if (Pais == null)
                {
                    Pais = ListaPaises.First(pais => pais.Id == App.Usuario.Filial.Cidade.Estado.Pais.Id);
                }

                if (Estado == null)
                {
                    Estado = ListaEstados.First(estado => estado.Id == App.Usuario.Filial.Cidade.Estado.Id);
                }

                //if (Cidade == null)
                //{
                //    Cidade = ListaCidades.First(cidade => cidade.Id == App.Usuario.Filial.Cidade.Id);
                //}

                foreach (ItemProposta item in Proposta.ListaItensProposta)
                {
                    await item.CalculaValoresItemPropostaAsync(Cliente);
                }

                OnPropertyChanged(nameof(ContemPecasKion));
                OnPropertyChanged(nameof(ContemPecas));
                OnPropertyChanged(nameof(ContemServicos));
                OnPropertyChanged(nameof(ContemDeslocamentos));
                OnPropertyChanged(nameof(TextoTotalPecas));
                OnPropertyChanged(nameof(TextoTotalServicos));
                OnPropertyChanged(nameof(TextoTotalDeslocamentos));
                OnPropertyChanged(nameof(TextoTotalGeral));
            }
        }

        private async Task PreenchePaisesAsync()
        {
            await Pais.PreencheListaPaisesAsync(ListaPaises, true, null, CancellationToken.None, "", "");
        }

        /// <summary>
        /// Método para preencher a lista de estados de acordo com o país selecionado
        /// </summary>
        private async Task PreencheEstadosAsync()
        {
            // Verifica se há algum país selecionado e preenche a lista caso verdadeiro
            if (Pais != null)
            {
                await Estado.PreencheListaEstadosAsync(ListaEstados, true, null, CancellationToken.None, "WHERE pais.id_pais = @id_pais", "@id_pais", Pais.Id);
                Estado = null;
                ListaCidades.Clear();
                Cidade = null;
            }
        }

        /// <summary>
        /// Método para preencher a lista de cidades de acordo com o estado selecionado
        /// </summary>
        private async Task PreencheCidadesAsync()
        {
            // Verifica se há algum estado selecionado e preenche a lista caso verdadeiro
            if (Estado != null)
            {
                await Cidade.PreencheListaCidadesAsync(ListaCidades, true, null, CancellationToken.None, "WHERE esta.id_estado = @id_estado", "@id_estado", Estado.Id);
                Cidade = null;
            }
        }

        /// <summary>
        /// Método para preencher a lista de tipos de equipamento de acordo com o fabricante selecionado
        /// </summary>
        private async Task PreencheTiposEquipamentoAsync()
        {
            // Verifica se há algum país selecionado e preenche a lista caso verdadeiro
            if (Fabricante != null)
            {
                await TipoEquipamento.PreencheListaTiposEquipamentoAsync(ListaTiposEquipamento, true, null, CancellationToken.None,
                    "INNER JOIN tb_modelos AS mode ON mode.id_tipo_equipamento = tieq.id_tipo_equipamento WHERE mode.id_fabricante = @id_fabricante GROUP BY tieq.id_tipo_equipamento",
                    "@id_fabricante", Fabricante.Id);
                TipoEquipamento = null;
                ListaModelos.Clear();
                Modelo = null;
            }
        }

        /// <summary>
        /// Método para preencher a lista de modelos de acordo com o tipo de equipamento e fabricante selecionado
        /// </summary>
        private async Task PreencheModelosAsync()
        {
            // Verifica se há algum estado selecionado e preenche a lista caso verdadeiro
            if (Fabricante != null && TipoEquipamento != null)
            {
                await Modelo.PreencheListaModelosAsync(ListaModelos, true, null, CancellationToken.None, "WHERE fabr.id_fabricante = @id_fabricante AND tieq.id_tipo_equipamento = @id_tipo_equipamento"
                    , "@id_fabricante, @id_tipo_equipamento", Fabricante.Id, TipoEquipamento.Id);
                Modelo = null;
            }
        }

        /// <summary>
        /// Método para retornar os dados do contato, caso ele exista
        /// </summary>
        private async Task GetContatoAsync()
        {
            if (Proposta?.Contato?.Id != null)
            {
                await Proposta.Contato.GetContatoDatabaseAsync(Proposta.Contato.Id, CancellationToken.None);

                if (Proposta?.Contato?.Telefone != null)
                {
                    FormatoTelefone = Proposta?.Contato?.Telefone.Length > 10 ? @"\(00\)\ 00000\-0000" : @"\(00\)\ 0000\-0000";
                }

                if (Proposta.Contato.Cidade != null)
                {
                    if (Proposta.Contato.Cidade.Estado != null)
                    {
                        if (Proposta.Contato.Cidade.Estado.Pais != null)
                        {
                            if (Proposta?.Contato?.Cidade?.Estado?.Pais?.Id > 0 && !ListaPaises.Any(x => x.Id == Proposta?.Contato?.Cidade?.Estado?.Pais?.Id))
                            {
                                Pais pais = new();

                                await pais.GetPaisDatabaseAsync(Proposta?.Contato?.Cidade?.Estado?.Pais?.Id, CancellationToken.None);

                                ListaPaises.Add(pais);
                            }

                            if (Proposta?.Contato?.Cidade?.Estado?.Id > 0 && !ListaEstados.Any(x => x.Id == Proposta?.Contato?.Cidade?.Estado?.Id))
                            {
                                Estado estado = new();

                                await estado.GetEstadoDatabaseAsync(Proposta?.Contato?.Cidade?.Estado?.Id, CancellationToken.None);

                                ListaEstados.Add(estado);
                            }

                            if (Proposta?.Contato?.Cidade?.Id > 0 && !ListaCidades.Any(x => x.Id == Proposta?.Contato?.Cidade?.Id))
                            {
                                Cidade cidade = new();

                                await cidade.GetCidadeDatabaseAsync(Proposta?.Contato?.Cidade?.Id, CancellationToken.None);

                                ListaCidades.Add(cidade);
                            }

                            Pais = ListaPaises.First(pais => pais.Id == Proposta.Contato.Cidade.Estado.Pais.Id);
                            Estado = ListaEstados.First(estado => estado.Id == Proposta.Contato.Cidade.Estado.Id);
                            Cidade = ListaCidades.First(cidade => cidade.Id == Proposta.Contato.Cidade.Id);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método para retornar os dados do contato, caso ele exista
        /// </summary>
        private async Task GetSerieAsync()
        {
            if (Proposta.Serie.Id != null)
            {
                try
                {
                    await Proposta.Serie.GetSerieDatabaseAsync(Proposta.Serie.Id, CancellationToken.None);

                    if (Proposta?.Serie?.Modelo?.Fabricante?.Id > 0 && !ListaFabricantes.Any(x => x.Id == Proposta?.Serie?.Modelo?.Fabricante?.Id))
                    {
                        Fabricante fabricante = new();

                        await fabricante.GetFabricanteDatabaseAsync(Proposta?.Serie?.Modelo?.Fabricante?.Id, CancellationToken.None);

                        ListaFabricantes.Add(fabricante);
                    }

                    if (Proposta?.Serie?.Modelo?.Categoria?.Id > 0 && !ListaCategorias.Any(x => x.Id == Proposta?.Serie?.Modelo?.Categoria?.Id))
                    {
                        Categoria categoria = new();

                        await categoria.GetCategoriaDatabaseAsync(Proposta?.Serie?.Modelo?.Categoria?.Id, CancellationToken.None);

                        ListaCategorias.Add(categoria);
                    }

                    if (Proposta?.Serie?.Modelo?.TipoEquipamento?.Id > 0 && !ListaTiposEquipamento.Any(x => x.Id == Proposta?.Serie?.Modelo?.TipoEquipamento?.Id))
                    {
                        TipoEquipamento tipoEquipamento = new();

                        await tipoEquipamento.GetTipoEquipamentoDatabaseAsync(Proposta?.Serie?.Modelo?.TipoEquipamento?.Id, CancellationToken.None);

                        ListaTiposEquipamento.Add(tipoEquipamento);
                    }

                    if (Proposta?.Serie?.Modelo?.Classe?.Id > 0 && !ListaClasses.Any(x => x.Id == Proposta?.Serie?.Modelo?.Classe?.Id))
                    {
                        Classe classe = new();

                        await classe.GetClasseDatabaseAsync(Proposta?.Serie?.Modelo?.Classe?.Id, CancellationToken.None);

                        ListaClasses.Add(classe);
                    }

                    if (Proposta?.Serie?.Modelo?.Id > 0 && !ListaModelos.Any(x => x.Id == Proposta?.Serie?.Modelo?.Id))
                    {
                        Modelo modelo = new();

                        await modelo.GetModeloDatabaseAsync(Proposta?.Serie?.Modelo?.Id, CancellationToken.None);

                        ListaModelos.Add(modelo);
                    }

                    Fabricante = ListaFabricantes.First(fabr => fabr.Id == Proposta?.Serie?.Modelo?.Fabricante?.Id);
                    Categoria = ListaCategorias.First(cate => cate.Id == Proposta?.Serie?.Modelo?.Categoria?.Id);
                    TipoEquipamento = ListaTiposEquipamento.First(tieq => tieq.Id == Proposta?.Serie?.Modelo?.TipoEquipamento?.Id);
                    Classe = ListaClasses.First(clas => clas.Id == Proposta?.Serie?.Modelo?.Classe?.Id);
                    Modelo = ListaModelos.First(mode => mode.Id == Proposta?.Serie?.Modelo?.Id);

                    await Ano.PreencheListaAnosAsync(ListaAnos, true, null, CancellationToken.None, "WHERE ano <= @ano", "@ano", DateTime.Now.Year + 1);
                    ListaAnos.DistinctBy(test => test.AnoValor);

                    try
                    {
                        Proposta.Ano = ListaAnos.First(anno => anno.Id == Proposta?.Serie?.Ano?.Id);
                    }
                    catch (Exception)
                    {
                    }
                }
                catch (Exception)
                {
                }
            }
            else
            {
                bool familiaExiste = false;

                if (!String.IsNullOrEmpty(Proposta.Serie.Nome))
                {
                    try
                    {
                        Familia familia = new();
                        await familia.GetFamiliaDatabaseAsync(Proposta.Serie.Nome, CancellationToken.None);

                        if (familia.Id != null)
                        {
                            Fabricante = ListaFabricantes.First(fabr => fabr.Id == familia.Modelo?.Fabricante?.Id);
                            Categoria = ListaCategorias.First(cate => cate.Id == familia.Modelo?.Categoria?.Id);
                            TipoEquipamento = ListaTiposEquipamento.First(tieq => tieq.Id == familia.Modelo?.TipoEquipamento?.Id);
                            Classe = ListaClasses.First(clas => clas.Id == familia.Modelo?.Classe?.Id);
                            Modelo = ListaModelos.First(mode => mode.Id == familia.Modelo?.Id);

                            familiaExiste = true;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                try
                {
                    int? posicaoInicioCaracters = ListaAnos.First(anno => anno.Fabricante.Id == Fabricante?.Id).PosicaoInicioCaracteres;
                    string? caracteres = Proposta.Serie.Nome.Substring((int)(posicaoInicioCaracters == null ? 0 : posicaoInicioCaracters) - 1, 1);

                    ObservableCollection<Ano> listaAnosTemporaria = new();

                    await Ano.PreencheListaAnosAsync(listaAnosTemporaria, true, null, CancellationToken.None,
                    "WHERE t_ano.id_status = 1 AND fabr.id_fabricante = @id_fabricante AND ano <= @ano AND posicao_inicio_caracteres = @posicao_inicio_caracteres AND caracteres = @caracteres",
                    "@id_fabricante, @ano, @posicao_inicio_caracteres, @caracteres", Fabricante.Id, DateTime.Now.Year + 1, posicaoInicioCaracters, caracteres);

                    // Se a lista retornada for maior que 0 significa que existem anos com os critérios informados, portanto preenche a lista de anos através dessa lista, caso contrário preenche a lista completa
                    if (listaAnosTemporaria.Count > 0 && familiaExiste)
                    {
                        ListaAnos = listaAnosTemporaria;
                        Proposta.Ano = ListaAnos.Last();
                    }
                    else
                    {
                        await Ano.PreencheListaAnosAsync(ListaAnos, true, null, CancellationToken.None, "WHERE ano <= @ano", "@ano", DateTime.Now.Year + 1);
                        ListaAnos.DistinctBy(test => test.AnoValor);
                    }
                }
                catch (Exception)
                {
                    await Ano.PreencheListaAnosAsync(ListaAnos, true, null, CancellationToken.None, "WHERE ano <= @ano", "@ano", DateTime.Now.Year + 1);
                    ListaAnos.DistinctBy(test => test.AnoValor);
                }
            }
        }

        /// <summary>
        /// Método assíncrono para salvar a proposta
        /// </summary>
        private async Task SalvarProposta()
        {
            // Retorna o id do usuário em uso
            await Proposta.GetIdUsuarioEmUsoAsync(CancellationToken.None);

            // Verifica se o id do usuário em uso não é nulo e, caso verdadeiro, impede a edição
            if (Proposta.IdUsuarioEmUso != null && Proposta.IdUsuarioEmUso != App.Usuario.Id)
            {
                // Define o usuário
                Usuario usuarioEmUso = new();
                await usuarioEmUso.GetUsuarioDatabaseAsync(Proposta.IdUsuarioEmUso, CancellationToken.None);

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Proposta em uso", "A proposta em questão está sendo editada por " + usuarioEmUso.Nome + ", portanto não será possível salva-la. Aguarde o usuário finalizar a edição da proposta para poder salva-la.", MessageDialogStyle.Affirmative, mySettings);

                usuarioEmUso = null;

                return;
            }

            // Verifica se existem campos vazios e, caso verdadeiro, encerra a execução do método
            if (ExistemCamposVazios)
            {
                return;
            }

            // Verifica se existem itens na proposta e, caso contrário, retorna mensagem
            if (Proposta.ListaItensProposta.Count == 0)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Proposta vazia", "Adicione ao menos um item à proposta para poder salvá-la", MessageDialogStyle.Affirmative, mySettings);

                return;
            }

            // Verifica se não houve atualização de prazo e, caso positivo, notifica o usuário e pergunta se deseja continuar
            if (!atualizouPrazo && !ehEdicaoLimitada)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Sim",
                    NegativeButtonText = "Não"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                    "Atenção", "Você não atualizou o prazo, caso não faça isso poderá informar um prazo errado para o cliente. Tem certeza que deseja continuar?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);

                if (respostaMensagem != MessageDialogResult.Affirmative)
                {
                    return;
                }
            }

            // Verifica se existem campos de termos vazios e notifica o usuário
            if ((String.IsNullOrEmpty(Proposta.TextoPadrao) || String.IsNullOrEmpty(Proposta.Observacoes) || String.IsNullOrEmpty(Proposta.PrazoEntrega) ||
                String.IsNullOrEmpty(Proposta.CondicaoPagamento) || String.IsNullOrEmpty(Proposta.Garantia) || String.IsNullOrEmpty(Proposta.Validade)) && !ehEdicaoLimitada)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Sim",
                    NegativeButtonText = "Não"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                    "Atenção", "Existem campos dos termos vazios. Tem certeza que deseja continar?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);

                if (respostaMensagem != MessageDialogResult.Affirmative)
                {
                    return;
                }
            }

            // Verifica se itens existentes na database foram excluidos e, caso positivo, notifica o usuário e pergunta se deseja continuar
            if (removeuItemExistente)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Sim",
                    NegativeButtonText = "Não"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                    "Atenção", "Você removeu itens dessa proposta que já constavam na database. Eles serão excluídos permanentemente e não poderão ser recuperados. Tem certeza que deseja continuar?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);

                if (respostaMensagem != MessageDialogResult.Affirmative)
                {
                    return;
                }
            }

            // Valida a sessão e obriga o login
            await HelperClasses.ValidaSessao.ValidaSessaoUsuarioAsync(_dialogCoordinator, this);

            // Define novamente os dados do usuário atual
            if (Proposta.Id == null)
            {
                Proposta.UsuarioInsercao = (Usuario)App.Usuario.Clone();
                Proposta.DataInsercao = DateTime.Now;
            }
            else
            {
                Proposta.UsuarioEdicao = (Usuario)App.Usuario.Clone();
                Proposta.DataEdicao = DateTime.Now;
            }

            string messagemErroSalvarItens = "";
            bool ehNovaProposta = false;

            ProgressoEhIndeterminavel = true;
            ValorProgresso = (double)0;
            MensagemStatus = "Salvando a proposta. Aguarde...";

            bool permiteSalvarAnterior = PermiteSalvar;
            bool permiteEditarAnterior = PermiteEditar;
            bool permiteCopiarAnterior = PermiteCopiar;
            bool permiteRevisarAnterior = PermiteRevisar;
            bool permiteDeletarAnterior = PermiteDeletar;
            bool permiteVisualizarAnterior = PermiteVisualizar;
            bool permiteCancelarEdicaoAnterior = PermiteCancelarEdicao;

            _cts = new();
            ControlesHabilitados = false;
            CancelarVisivel = true;
            PermiteCancelar = true;
            PermiteSalvar = false;
            PermiteEditar = false;
            PermiteCopiar = false;
            PermiteRevisar = false;
            PermiteDeletar = false;
            PermiteVisualizar = false;
            PermiteCancelarEdicao = false;

            Random numeroAleatorio = new();
            int milisegundosParaAguardar = 0;
            string codigoPropostaAoClicar = Proposta.CodigoProposta.ToString();

            // O trecho de código abaixo é obrigatório para evitar que, caso dois usuários cliquem em salvar uma nova proposta exatamente ao mesmo tempo o sistema gere o código da proposta corretamente
            if (Proposta.Id == null)
            {
                // Determina que é uma nova proposta
                ehNovaProposta = true;

                // Gera um número aleatório entre 1000 e 3000, ou seja, entre 1 e 3 segundos
                milisegundosParaAguardar = numeroAleatorio.Next(1000, 3000);

                // Faz o sistema aguardar o número de tempo aleatório gerado acima
                try
                {
                    await Task.Delay(milisegundosParaAguardar, _cts.Token);
                }
                catch (Exception)
                {
                    ProgressoEhIndeterminavel = false;
                    ValorProgresso = (double)0;
                    MensagemStatus = "Operação cancelada";

                    ControlesHabilitados = true;
                    CancelarVisivel = false;
                    PermiteCancelar = false;
                    PermiteSalvar = permiteSalvarAnterior;
                    PermiteEditar = permiteEditarAnterior;
                    PermiteCopiar = permiteCopiarAnterior;
                    PermiteRevisar = permiteRevisarAnterior;
                    PermiteDeletar = permiteDeletarAnterior;
                    PermiteVisualizar = permiteVisualizarAnterior;
                    PermiteCancelarEdicao = permiteCancelarEdicaoAnterior;
                    return;
                }

                if (ehRevisao)
                {
                    Proposta.CodigoProposta = await Proposta.GetCodigoPropostaRevisadaAsync(CancellationToken.None);
                }
                else
                {
                    Proposta.CodigoProposta = DateTime.Now.ToString("ddMMyyyy") + ExcelClasses.ConverterColunaNumericaParaLetra(await Proposta.GetContagemPropostasIniciaisAsync(DateTime.Now, CancellationToken.None) + 1);
                }
            }
            else
            {
                await Task.Delay(1000, _cts.Token);
            }

            try
            {
                Proposta.Fabricante = Fabricante;
                Proposta.TipoEquipamento = TipoEquipamento;
                Proposta.Modelo = Modelo;
            }
            catch (Exception)
            {
            }

            try
            {
                if (!String.IsNullOrEmpty(Proposta.Contato.Nome))
                {
                    Proposta.Contato.Status = new Status { Id = 1, Nome = "Ativo" };
                    Proposta.Contato.Cliente = Proposta.Cliente;
                    Proposta.Contato.Cidade = Cidade;

                    await Proposta.Contato.SalvarContatoDatabaseAsync(CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao salvar contato");
            }

            try
            {
                if (!String.IsNullOrEmpty(Proposta.Serie.Nome))
                {
                    Proposta.Serie.Status = new Status { Id = 1, Nome = "Ativo" };
                    Proposta.Serie.Cliente = Proposta.Cliente;
                    Proposta.Serie.Modelo = Proposta.Modelo;
                    Proposta.Serie.Ano = Proposta.Ano;

                    await Proposta.Serie.SalvarSerieDatabaseAsync(CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao salvar série");
            }

            try
            {
                await Proposta.SalvarPropostaDatabaseAsync(_cts.Token);

                if (ehRevisao)
                {
                    await Proposta.SetUltimaPropostaAsync(CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao salvar dados");

                ProgressoEhIndeterminavel = false;
                ValorProgresso = (double)0;
                MensagemStatus = "Erro ao salvar a proposta. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";

                ControlesHabilitados = true;
                CancelarVisivel = false;
                PermiteCancelar = false;
                PermiteSalvar = permiteSalvarAnterior;
                PermiteEditar = permiteEditarAnterior;
                PermiteCopiar = permiteCopiarAnterior;
                PermiteRevisar = permiteRevisarAnterior;
                PermiteDeletar = permiteDeletarAnterior;
                PermiteVisualizar = permiteVisualizarAnterior;
                PermiteCancelarEdicao = permiteCancelarEdicaoAnterior;
                return;
            }

            MensagemStatus = "Salvando os itens da proposta. Aguarde...";

            int contadorItens = 0;
            int totalItens = Proposta.ListaItensProposta.Count;

            // Se não for uma nova proposta, varre os itens anteriores da proposta e verifica se esses itens ainda existem, caso contrário, exclui os itens da database
            if (!ehNovaProposta)
            {
                try
                {
                    ObservableCollection<ItemProposta> listaItensAnterioresProposta = new();

                    await ItemProposta.PreencheListaItensPropostaAsync(listaItensAnterioresProposta, true, false, null,
                        CancellationToken.None, "WHERE id_proposta = @id_proposta", "@id_proposta", Proposta.Id);

                    foreach (var item in listaItensAnterioresProposta)
                    {
                        if (!Proposta.ListaItensProposta.Where(it => it.Id == item.Id).Any())
                        {
                            await item.DeletarItemPropostaDatabaseAsync(CancellationToken.None);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            try
            {
                foreach (var item in Proposta.ListaItensProposta)
                {
                    item.Proposta = Proposta;

                    await item.SalvarItemPropostaDatabaseAsync(CancellationToken.None);

                    contadorItens++;

                    Messenger.Default.Send<double>((double)contadorItens / (double)totalItens * (double)100, "ValorProgresso");
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao salvar dados");

                messagemErroSalvarItens = ". Porém, houve erro ao salvar um ou mais itens. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
            }

            _ehCopia = false;
            ProgressoEhIndeterminavel = false;
            ValorProgresso = (double)0;
            MensagemStatus = "Proposta salva" + messagemErroSalvarItens;

            Proposta.IdUsuarioEmUso = null;

            await Proposta.AtualizaIdUsuarioEmUsoAsync(CancellationToken.None);

            await CarregarProposta();

            ControlesHabilitados = true;
            CancelarVisivel = false;
            PermiteCancelar = false;
            EdicaoHabilitada = false;
        }

        private async Task DeletarProposta()
        {
            // Retorna o id do usuário em uso
            await Proposta.GetIdUsuarioEmUsoAsync(CancellationToken.None);

            // Verifica se o id do usuário em uso não é nulo e, caso verdadeiro, impede a edição
            if (Proposta.IdUsuarioEmUso != null && Proposta.IdUsuarioEmUso != App.Usuario.Id)
            {
                // Define o usuário
                Usuario usuarioEmUso = new();
                await usuarioEmUso.GetUsuarioDatabaseAsync(Proposta.IdUsuarioEmUso, CancellationToken.None);

                var mySettingsUso = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagemUso = await _dialogCoordinator.ShowMessageAsync(this,
                        "Proposta em uso", "A proposta em questão está sendo editada por " + usuarioEmUso.Nome + ", portanto não será possível deleta-la. Aguarde o usuário finalizar a edição da proposta para poder deleta-la.", MessageDialogStyle.Affirmative, mySettingsUso);

                usuarioEmUso = null;

                return;
            }

            // Questiona ao usuário se deseja mesmo excluir a proposta
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Sim",
                NegativeButtonText = "Não"
            };

            var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                "Atenção", "Tem certeza que deseja excluir a proposta '" + Proposta.Cliente.Nome + " - " + Proposta.CodigoProposta + "' e seus respectivos itens? " +
                "O processo não poderá ser desfeito.",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem != MessageDialogResult.Affirmative)
            {
                return;
            }

            ProgressoEhIndeterminavel = true;
            ValorProgresso = (double)0;
            MensagemStatus = "Deletando a proposta. Aguarde...";

            bool permiteSalvarAnterior = PermiteSalvar;
            bool permiteEditarAnterior = PermiteEditar;
            bool permiteCopiarAnterior = PermiteCopiar;
            bool permiteRevisarAnterior = PermiteRevisar;
            bool permiteDeletarAnterior = PermiteDeletar;
            bool permiteVisualizarAnterior = PermiteVisualizar;
            bool permiteCancelarEdicaoAnterior = PermiteCancelarEdicao;

            _cts = new();
            ControlesHabilitados = false;
            CancelarVisivel = true;
            PermiteCancelar = true;
            PermiteSalvar = false;
            PermiteEditar = false;
            PermiteCopiar = false;
            PermiteRevisar = false;
            PermiteDeletar = false;
            PermiteVisualizar = false;
            PermiteCancelarEdicao = false;

            try
            {
                await Task.Delay(3000, _cts.Token);
            }
            catch (Exception)
            {
                ProgressoEhIndeterminavel = false;
                ValorProgresso = (double)0;
                MensagemStatus = "Operação cancelada";

                ControlesHabilitados = true;
                CancelarVisivel = false;
                PermiteCancelar = false;
                PermiteSalvar = permiteSalvarAnterior;
                PermiteEditar = permiteEditarAnterior;
                PermiteCopiar = permiteCopiarAnterior;
                PermiteRevisar = permiteRevisarAnterior;
                PermiteDeletar = permiteDeletarAnterior;
                PermiteVisualizar = permiteVisualizarAnterior;
                PermiteCancelarEdicao = permiteCancelarEdicaoAnterior;
                return;
            }

            try
            {
                await Proposta.DeletarPropostaDatabaseAsync(_cts.Token, true);
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    MensagemStatus = "Operação cancelada";
                }
                else
                {
                    if (ex is ValorNaoExisteException || ex is ChaveEstrangeiraEmUsoException)
                    {
                        MensagemStatus = ex.Message;
                    }
                    else
                    {
                        Serilog.Log.Error(ex, "Erro ao excluir dados");
                        MensagemStatus = "Erro ao excluir a proposta. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                    }
                }

                ProgressoEhIndeterminavel = false;
                ValorProgresso = (double)0;

                ControlesHabilitados = true;
                CancelarVisivel = false;
                PermiteCancelar = false;
                PermiteSalvar = permiteSalvarAnterior;
                PermiteEditar = permiteEditarAnterior;
                PermiteCopiar = permiteCopiarAnterior;
                PermiteRevisar = permiteRevisarAnterior;
                PermiteDeletar = permiteDeletarAnterior;
                PermiteVisualizar = permiteVisualizarAnterior;
                PermiteCancelarEdicao = permiteCancelarEdicaoAnterior;
                return;
            }

            ProgressoEhIndeterminavel = false;
            ValorProgresso = (double)0;
            MensagemStatus = "Proposta deletada";

            var mySettings2 = new MetroDialogSettings
            {
                AffirmativeButtonText = "OK"
            };

            var respostaMensagem2 = await _dialogCoordinator.ShowMessageAsync(this,
                    "Proposta deletada", "A proposta foi deletada com sucesso. Esta página será fechada ao clicar em 'OK'", MessageDialogStyle.Affirmative, mySettings2);

            Messenger.Default.Send<IPageViewModel>(this, "PrincipalPaginaRemover");
        }

        private async Task CancelarEdicaoOuRevisao()
        {
            if (ehRevisao)
            {
                Proposta.Id = idPropostaOriginal;
            }
            else
            {
                Proposta.IdUsuarioEmUso = null;

                await Proposta.AtualizaIdUsuarioEmUsoAsync(CancellationToken.None);
            }

            await CarregarProposta();
        }

        private void Cancelar()
        {
            if (_cts != null)
                _cts.Cancel();
        }

        /// <summary>
        /// Método assíncrono para carregar a proposta
        /// </summary>
        private async Task CarregarProposta()
        {
            // Se o id da proposta for nulo significa que é uma nova proposta, portanto encerra a execução
            if (Proposta.Id == null && !_ehCopia)
            {
                return;
            }

            DeletarVisivel = App.Usuario.Perfil.Id == 1;

            EdicaoHabilitada = false;
            EdicaoSolicitante = false;
            EdicaoFaturamento = false;
            EdicaoProposta = false;
            EdicaoEquipamento = false;
            EdicaoComentarios = false;
            ContextMenuVisivel = false;

            CancelarEdicaoVisivel = false;
            PermiteSalvar = false;
            PermiteEditar = true;
            PermiteCopiar = true;
            PermiteRevisar = true;
            PermiteDeletar = true;
            PermiteVisualizar = true;
            PermiteCancelarEdicao = false;

            if (!_ehCopia)
            {
                try
                {
                    // Retorna a proposta de acordo com a database
                    await Proposta.GetPropostaDatabaseAsync(Proposta.Id, CancellationToken.None, true, true, true);
                }
                catch (Exception ex)
                {
                    // Escreve no log a exceção e uma mensagem de erro
                    Serilog.Log.Error(ex, "Erro ao carregar dados");

                    ProgressoEhIndeterminavel = false;
                    ValorProgresso = (double)0;
                    MensagemStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                }
            }

            try
            {
                if (Proposta?.Cliente?.Id > 0 && !ListaClientes.Any(x => x.Id == Proposta?.Cliente?.Id))
                {
                    Cliente cliente = new();

                    await cliente.GetClienteDatabaseAsync(Proposta?.Cliente?.Id, CancellationToken.None);

                    ListaClientes.Add(cliente);
                }

                if (Proposta?.Prioridade?.Id > 0 && !ListaPrioridades.Any(x => x.Id == Proposta?.Prioridade?.Id))
                {
                    Prioridade prioridade = new();

                    await prioridade.GetPrioridadeDatabaseAsync(Proposta?.Prioridade?.Id, CancellationToken.None);

                    ListaPrioridades.Add(prioridade);
                }

                if (Proposta?.StatusAprovacao?.Id > 0 && !ListaStatusAprovacao.Any(x => x.Id == Proposta?.StatusAprovacao?.Id))
                {
                    StatusAprovacao statusAprovacao = new();

                    await statusAprovacao.GetStatusAprovacaoDatabaseAsync(Proposta?.StatusAprovacao?.Id, CancellationToken.None);

                    ListaStatusAprovacao.Add(statusAprovacao);
                }

                if (Proposta?.JustificativaAprovacao?.Id > 0 && !ListaJustificativasAprovacao.Any(x => x.Id == Proposta?.JustificativaAprovacao?.Id))
                {
                    JustificativaAprovacao justificativaAprovacao = new();

                    await justificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(Proposta?.JustificativaAprovacao?.Id, CancellationToken.None);

                    ListaJustificativasAprovacao.Add(justificativaAprovacao);
                }

                if (Proposta?.Contato?.Id > 0 && !ListaContatos.Any(x => x.Id == Proposta?.Contato?.Id))
                {
                    Contato contato = new();

                    await contato.GetContatoDatabaseAsync(Proposta?.Contato?.Id, CancellationToken.None);

                    ListaContatos.Add(contato);
                }

                if (Proposta?.Contato?.Cidade?.Estado?.Pais?.Id > 0 && !ListaPaises.Any(x => x.Id == Proposta?.Contato?.Cidade?.Estado?.Pais?.Id))
                {
                    Pais pais = new();

                    await pais.GetPaisDatabaseAsync(Proposta?.Contato?.Cidade?.Estado?.Pais?.Id, CancellationToken.None);

                    ListaPaises.Add(pais);
                }

                if (Proposta?.Contato?.Cidade?.Estado?.Id > 0 && !ListaEstados.Any(x => x.Id == Proposta?.Contato?.Cidade?.Estado?.Id))
                {
                    Estado estado = new();

                    await estado.GetEstadoDatabaseAsync(Proposta?.Contato?.Cidade?.Estado?.Id, CancellationToken.None);

                    ListaEstados.Add(estado);
                }

                if (Proposta?.Contato?.Cidade?.Id > 0 && !ListaCidades.Any(x => x.Id == Proposta?.Contato?.Cidade?.Id))
                {
                    Cidade cidade = new();

                    await cidade.GetCidadeDatabaseAsync(Proposta?.Contato?.Cidade?.Id, CancellationToken.None);

                    ListaCidades.Add(cidade);
                }

                if (Proposta?.Serie?.Id > 0 && !ListaSeries.Any(x => x.Id == Proposta?.Serie?.Id))
                {
                    Serie serie = new();

                    await serie.GetSerieDatabaseAsync(Proposta?.Serie?.Id, CancellationToken.None);

                    ListaSeries.Add(serie);
                }

                if (Proposta?.Fabricante?.Id > 0 && !ListaFabricantes.Any(x => x.Id == Proposta?.Fabricante?.Id))
                {
                    Fabricante fabricante = new();

                    await fabricante.GetFabricanteDatabaseAsync(Proposta?.Fabricante?.Id, CancellationToken.None);

                    ListaFabricantes.Add(fabricante);
                }

                if (Proposta?.Modelo?.Categoria?.Id > 0 && !ListaCategorias.Any(x => x.Id == Proposta?.Modelo?.Categoria?.Id))
                {
                    Categoria categoria = new();

                    await categoria.GetCategoriaDatabaseAsync(Proposta?.Modelo?.Categoria?.Id, CancellationToken.None);

                    ListaCategorias.Add(categoria);
                }

                if (Proposta?.TipoEquipamento?.Id > 0 && !ListaTiposEquipamento.Any(x => x.Id == Proposta?.TipoEquipamento?.Id))
                {
                    TipoEquipamento tipoEquipamento = new();

                    await tipoEquipamento.GetTipoEquipamentoDatabaseAsync(Proposta?.TipoEquipamento?.Id, CancellationToken.None);

                    ListaTiposEquipamento.Add(tipoEquipamento);
                }

                if (Proposta?.Modelo?.Classe?.Id > 0 && !ListaClasses.Any(x => x.Id == Proposta?.Modelo?.Classe?.Id))
                {
                    Classe classe = new();

                    await classe.GetClasseDatabaseAsync(Proposta?.Modelo?.Classe?.Id, CancellationToken.None);

                    ListaClasses.Add(classe);
                }

                if (Proposta?.Modelo?.Id > 0 && !ListaModelos.Any(x => x.Id == Proposta?.Modelo?.Id))
                {
                    Modelo modelo = new();

                    await modelo.GetModeloDatabaseAsync(Proposta?.Modelo?.Id, CancellationToken.None);

                    ListaModelos.Add(modelo);
                }
            }
            catch (Exception)
            {
            }

            ehCarregamento = true;

            // Define as classes da proposta como classes com a mesma referência das listas
            try
            {
                Cliente = ListaClientes.First(clie => clie.Id == Proposta?.Cliente?.Id);
                //Cliente = Proposta.Cliente;
            }
            catch (Exception)
            {
            }
            try
            {
                Proposta.Fabricante = ListaFabricantes.First(fabr => fabr.Id == Proposta?.Fabricante?.Id);
                Fabricante = Proposta.Fabricante;
                //await PreencheTiposEquipamentoAsync();
            }
            catch (Exception)
            {
            }
            try
            {
                Proposta.TipoEquipamento = ListaTiposEquipamento.First(tieq => tieq.Id == Proposta?.TipoEquipamento?.Id);
                TipoEquipamento = Proposta.TipoEquipamento;
                //await PreencheModelosAsync();
            }
            catch (Exception)
            {
            }
            try
            {
                Proposta.Modelo = ListaModelos.First(mode => mode.Id == Proposta?.Modelo?.Id);
                Modelo = Proposta.Modelo;
            }
            catch (Exception)
            {
            }
            try
            {
                Proposta.Modelo.Categoria = ListaCategorias.First(cate => cate.Id == Proposta?.Modelo?.Categoria.Id);
                Categoria = Proposta.Modelo.Categoria;
            }
            catch (Exception)
            {
            }
            try
            {
                Proposta.Modelo.Classe = ListaClasses.First(clas => clas.Id == Proposta?.Modelo?.Classe.Id);
                Classe = Proposta.Modelo.Classe;
            }
            catch (Exception)
            {
            }
            try
            {
                Proposta.Ano = ListaAnos.First(anno => anno.Id == Proposta?.Ano?.Id);
            }
            catch (Exception)
            {
            }
            try
            {
                Proposta.Prioridade = ListaPrioridades.First(prio => prio.Id == Proposta?.Prioridade?.Id);
            }
            catch (Exception)
            {
            }
            try
            {
                Proposta.Status = ListaStatus.First(stat => stat.Id == Proposta?.Status?.Id);
            }
            catch (Exception)
            {
            }
            try
            {
                Proposta.StatusAprovacao = ListaStatusAprovacao.First(stap => stap.Id == Proposta?.StatusAprovacao?.Id);
            }
            catch (Exception)
            {
            }
            try
            {
                Proposta.JustificativaAprovacao = ListaJustificativasAprovacao.First(juap => juap.Id == Proposta?.JustificativaAprovacao?.Id);
            }
            catch (Exception)
            {
            }
            try
            {
                await GetContatoAsync();
            }
            catch (Exception)
            {
            }
            try
            {
                await GetSerieAsync();
            }
            catch (Exception)
            {
            }
            try
            {
                if (_ehCopia)
                {
                    Name = "Nova proposta";
                    DeletarVisivel = false;
                    EdicaoHabilitada = true;
                    EdicaoSolicitante = true;
                    EdicaoFaturamento = true;
                    EdicaoProposta = true;
                    EdicaoEquipamento = true;
                    EdicaoComentarios = true;
                    ContextMenuVisivel = true;

                    PermiteSalvar = true;
                    PermiteVisualizar = false;
                    PermiteEditar = false;
                    PermiteCopiar = false;
                    PermiteRevisar = false;
                    PermiteDeletar = false;
                }
                else
                {
                    Name = Proposta.Cliente.Nome + " - " + Proposta.CodigoProposta;
                }
            }
            catch (Exception)
            {
                Name = "Proposta não identificada";
            }
            ehCarregamento = false;
        }

        private async Task ControleItem(TipoItemAdicionar tipoItemAdicionar = TipoItemAdicionar.Peca, ItemProposta itemProposta = null)
        {
            try
            {
                bool ehNovoItem = false;

                if (itemProposta == null)
                {
                    ehNovoItem = true;

                    TipoItem tipoItem = new();
                    Status statusItem = new();
                    StatusAprovacao statusAprovacaoItem = new();
                    JustificativaAprovacao justificativaAprovacaoItem = new();

                    await tipoItem.GetTipoItemDatabaseAsync((int)tipoItemAdicionar, CancellationToken.None);
                    await statusItem.GetStatusDatabaseAsync(1, CancellationToken.None);
                    await statusAprovacaoItem.GetStatusAprovacaoDatabaseAsync(3, CancellationToken.None);
                    await justificativaAprovacaoItem.GetJustificativaAprovacaoDatabaseAsync(1, CancellationToken.None);

                    itemProposta = new ItemProposta { TipoItem = tipoItem, Status = statusItem, StatusAprovacao = statusAprovacaoItem, JustificativaAprovacao = justificativaAprovacaoItem };
                }

                var customDialog = new CustomDialog();

                var dataContext = new ControleItensViewModel(Proposta, itemProposta, ehNovoItem, instance =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);

                    OnPropertyChanged(nameof(ContemPecasKion));
                    OnPropertyChanged(nameof(ContemPecas));
                    OnPropertyChanged(nameof(ContemServicos));
                    OnPropertyChanged(nameof(ContemDeslocamentos));
                    OnPropertyChanged(nameof(TextoTotalPecas));
                    OnPropertyChanged(nameof(TextoTotalServicos));
                    OnPropertyChanged(nameof(TextoTotalDeslocamentos));
                    OnPropertyChanged(nameof(TextoTotalGeral));
                });

                customDialog.Content = new ControleItensView { DataContext = dataContext };

                await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
            }
            catch (Exception ex)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Entendi"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Erro", "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor", MessageDialogStyle.Affirmative, mySettings);

                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");
            }
        }

        private async Task ImportarItens(FornecedorImportar fornecedorImportar)
        {
            Fornecedor fornecedor = new();

            await fornecedor.GetFornecedorDatabaseAsync((int)fornecedorImportar, CancellationToken.None);

            var customDialog = new CustomDialog();

            var dataContext = new ImportarPecasViewModel(Proposta, fornecedor, instance =>
            {
                _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            });

            customDialog.Content = new ImportarPecasView { DataContext = dataContext };

            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        private async Task RemoverItem(ItemProposta itemProposta)
        {
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Sim",
                NegativeButtonText = "Não"
            };

            var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                "Atenção", "Tem certeza que deseja remover o item? O processo não poderá ser desfeito",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem == MessageDialogResult.Affirmative)
            {
                Proposta.ListaItensProposta.Remove(itemProposta);

                if (itemProposta.Id != null)
                {
                    removeuItemExistente = true;
                }
            }
        }

        private async Task RemoverItens(ObservableCollection<ItemProposta> itensPropostaRemover)
        {
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Sim",
                NegativeButtonText = "Não"
            };

            var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                "Atenção", "Tem certeza que deseja remover os itens selecionados? O processo não poderá ser desfeito",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem == MessageDialogResult.Affirmative)
            {
                foreach (var itemProposta in itensPropostaRemover.ToList())
                {
                    Proposta.ListaItensProposta.Remove(itemProposta);

                    if (itemProposta.Id != null)
                    {
                        removeuItemExistente = true;
                    }
                }
            }
        }

        private async Task ExportarItens(ExcelClasses.TipoExportacaoItensProposta tipoExportacaoItensProposta)
        {
            VistaSaveFileDialog sfd = new VistaSaveFileDialog();
            var options = new ExcelExportingOptions();
            string nomeWorksheet = "";

            switch (tipoExportacaoItensProposta)
            {
                case ExcelClasses.TipoExportacaoItensProposta.FormatoFluig:
                    sfd.Filter = "Arquivo do Excel (*.xlsx)|*.xlsx";
                    sfd.Title = "Informe o local e o nome do arquivo";
                    sfd.FileName = "importacao";
                    sfd.AddExtension = true;
                    nomeWorksheet = "importacao1";
                    foreach (var item in DataGrid.Columns)
                    {
                        if (item.MappingName != "CodigoItem" && item.MappingName != "QuantidadeItem")
                        {
                            options.ExcludeColumns.Add(item.MappingName);
                        }
                    }
                    break;

                case ExcelClasses.TipoExportacaoItensProposta.TabelaCompleta:
                    sfd.Filter = "Arquivo do Excel (*.xlsx)|*.xlsx";
                    sfd.Title = "Informe o local e o nome do arquivo";
                    sfd.FileName = "Itens_Proposta_" + Proposta.CodigoProposta + "_" + Proposta.Cliente.Nome;
                    sfd.AddExtension = true;
                    nomeWorksheet = "Itens_Proposta";
                    foreach (var item in DataGrid.Columns)
                    {
                        if (item.IsHidden)
                        {
                            options.ExcludeColumns.Add(item.MappingName);
                        }
                    }
                    break;

                default:
                    sfd.Filter = "Arquivo do Excel (*.xlsx)|*.xlsx";
                    sfd.Title = "Informe o local e o nome do arquivo";
                    sfd.FileName = "Itens_Proposta_" + Proposta.CodigoProposta + "_" + Proposta.Cliente.Nome;
                    sfd.AddExtension = true;
                    nomeWorksheet = "Itens_Proposta";
                    foreach (var item in DataGrid.Columns)
                    {
                        if (item.IsHidden)
                        {
                            options.ExcludeColumns.Add(item.MappingName);
                        }
                    }
                    break;
            }

            bool houveErro = false;

            try
            {
                options.ExcelVersion = ExcelVersion.Excel2013;

                var excelEngine = DataGrid.ExportToExcel(DataGrid.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];

                workBook.Worksheets[0].Name = nomeWorksheet;
                try
                {
                    workBook.Worksheets[1].Remove();
                    workBook.Worksheets[2].Remove();
                }
                catch (Exception)
                {
                }

                if (tipoExportacaoItensProposta == ExcelClasses.TipoExportacaoItensProposta.FormatoFluig)
                {
                    if (workBook.Worksheets[0].Range["A1"].Value != DataGrid.Columns["CodigoItem"].HeaderText)
                    {
                        foreach (var item in workBook.Worksheets[0].Columns[0])
                        {
                            var copia1 = workBook.Worksheets[0].Range["A" + item.Row].Value;
                            var copia2 = workBook.Worksheets[0].Range["B" + item.Row].Value;

                            workBook.Worksheets[0].Range["A" + item.Row].Value = copia2;
                            workBook.Worksheets[0].Range["B" + item.Row].Value = copia1;
                        }
                    }
                    workBook.Worksheets[0].Range["A1"].Value = "item";
                    workBook.Worksheets[0].Range["B1"].Value = "quantidade";

                    foreach (var item in workBook.Worksheets[0].Columns[1])
                    {
                        if (item.Row > 1)
                        {
                            try
                            {
                                if (!String.IsNullOrEmpty(workBook.Worksheets[0].Range["B" + item.Row].Value))
                                {
                                    workBook.Worksheets[0].Range["B" + item.Row].Value = Decimal.Round(Convert.ToDecimal(workBook.Worksheets[0].Range["B" + item.Row].Value)).ToString();
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    workBook.Worksheets[0].Columns[1].NumberFormat = "0";
                }
                else
                {
                    int contador = 0;
                    foreach (var item in DataGrid.Columns)
                    {
                        if (item.IsHidden == false)
                        {
                            if (item.CellType == "Currency")
                            {
                                workBook.Worksheets[0].Columns[contador].NumberFormat = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol + " #,##0.00";
                            }
                            contador++;
                        }
                    }
                }

                if (sfd.ShowDialog() == true)
                {
                    if (!sfd.FileName.EndsWith(".xlsx"))
                    {
                        sfd.FileName = sfd.FileName + ".xlsx";
                    }

                    workBook.SaveAs(sfd.FileName);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                houveErro = true;

                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao exportar dados");

                ProgressoEhIndeterminavel = false;
                ValorProgresso = (double)0;
                MensagemStatus = "Falha na eportação dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
            }

            if (!houveErro)
            {
                try
                {
                    var mySettings = new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Sim",
                        NegativeButtonText = "Não"
                    };

                    var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Itens exportados", "Deseja abrir o arquivo?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);

                    if (respostaMensagem == MessageDialogResult.Affirmative)
                    {
                        ProcessStartInfo psInfo = new ProcessStartInfo
                        {
                            FileName = sfd.FileName,
                            UseShellExecute = true
                        };

                        Process.Start(psInfo);
                    }
                }
                catch (Exception ex)
                {
                    // Escreve no log a exceção e uma mensagem de erro
                    Serilog.Log.Error(ex, "Erro ao abrir o arquivo");

                    ProgressoEhIndeterminavel = false;
                    ValorProgresso = (double)0;
                    MensagemStatus = "Falha na abertura do arquivo. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                }
            }

            //ValorProgresso = 0;
            //_cts = new();

            //Progress<double> progresso = new(dbl =>
            //{
            //    ValorProgresso = dbl;
            //});

            //var customDialog = new CustomDialog();

            //var dataContext = new CustomProgressViewModel("Exportando itens", "Aguarde...", false, true, _cts, instance =>
            //{
            //    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            //});

            //customDialog.Content = new CustomProgressView { DataContext = dataContext };

            //await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);

            //try
            //{
            //    await ExcelClasses.ExportarItensProposta(tipoExportacaoItensProposta, Proposta, Proposta.ListaItensProposta, progresso, _cts.Token);
            //}
            //catch (Exception)
            //{
            //    await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            //}

            //try
            //{
            //    await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            //}
            //catch (Exception)
            //{
            //}
        }

        private async Task AtualizarPrazo(ExcelClasses.TipoInformacaoEstoque tipoInformacaoEstoque)
        {
            ObservableCollection<ValidacaoItemPropostaEstoque> listaValidacaoItemPropostaEstoque = new();

            ConfiguracaoSistema configuracaoSistema = new();

            try
            {
                await configuracaoSistema.GetConfiguracaoSistemaDatabaseAsync(1, CancellationToken.None);
            }
            catch (Exception ex)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao encontrar carregar configurações");

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Erro ao carregar configurações", "Erro ao carregar as configurações do sistema.\n\nSe o problema persistir, contate o desenvolvedor e envie o arquivo de log", MessageDialogStyle.Affirmative, mySettings);

                return;
            }

            string arquivoEstoque = "";

            try
            {
                arquivoEstoque = RetornaUltimoArquivoEstoque(configuracaoSistema.LocalArquivoEstoque.ToString(), "*" + configuracaoSistema.NomeArquivoEstoque.ToString() + "*");
            }
            catch (Exception ex)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao encontrar arquivo de estoque");

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Nenhum arquivo de estoque encontrado", "Entre em contato com o administrador master para verificar as configurações de estoque.\n\nSe o problema persistir, contate o desenvolvedor e envie o arquivo de log", MessageDialogStyle.Affirmative, mySettings);

                return;
            }

            _cts = new();

            Progress<double> progresso = new(dbl =>
            {
                Messenger.Default.Send<double>(dbl, "ValorProgresso2");
            });

            var customDialog = new CustomDialog();

            var dataContext = new CustomProgressViewModel("Atualizando prazo", "Aguarde...", false, true, _cts, instance =>
            {
                _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            });

            customDialog.Content = new CustomProgressView { DataContext = dataContext };

            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);

            try
            {
                ExcelClasses excelClasses = new();
                listaValidacaoItemPropostaEstoque = await excelClasses.AtualizarEstoque(tipoInformacaoEstoque,
                    Proposta.ListaItensProposta, arquivoEstoque, progresso, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                ProgressoEhIndeterminavel = false;
                ValorProgresso = (double)0;
                MensagemStatus = "Operação cancelada";
            }
            catch (Exception ex)
            {
                await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao atualizar prazo");

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Erro na atualização de prazo", "Entre em contato com o administrador master para verificar as configurações de estoque.\n\nSe o problema persistir, contate o desenvolvedor e envie o arquivo de log", MessageDialogStyle.Affirmative, mySettings);
            }

            try
            {
                await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            }
            catch (Exception)
            {
            }

            if (listaValidacaoItemPropostaEstoque.Count > 0)
            {
                var customDialog2 = new CustomDialog();

                var dataContext2 = new ConfirmacaoItensEstoqueViewModel(listaValidacaoItemPropostaEstoque, instance =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog2);
                });

                customDialog2.Content = new ConfirmacaoItensEstoqueView { DataContext = dataContext2 };

                await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog2);
            }

            atualizouPrazo = true;
        }

        public string RetornaUltimoArquivoEstoque(string caminhoEstoque, string nomeDoArquivoAProcurar)
        {
            try
            {
                DirectoryInfo directoryInfo = new(caminhoEstoque);
                List<FileInfo> files = directoryInfo.GetFiles(nomeDoArquivoAProcurar).ToList().Where(file => file.Name.ToLower().EndsWith("xlsx")).ToList();
                DateTime ultimaDataAlteracao = DateTime.MinValue;
                FileInfo ultimoArquivo = null;

                foreach (FileInfo file in files)
                {
                    if (file.LastWriteTime > ultimaDataAlteracao && !file.Name.StartsWith("~$"))
                    {
                        ultimaDataAlteracao = file.LastWriteTime;
                        ultimoArquivo = file;
                    }
                }

                if (ultimoArquivo is null)
                {
                    throw new FileNotFoundException();
                }

                return ultimoArquivo.FullName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private enum TipoDefinicaoFrete
        {
            IgualParaTodos,
            Contagem,
            PorQuantidade,
            PorPrecoUnitarioInicial,
            PorPrecoUnitarioFinal,
            PorPrecoTotalInicial,
            PorPrecoTotalFinal
        }

        private async Task DefinirFrete(TipoDefinicaoFrete tipoDefinicaoFrete)
        {
            int contagemItens = Proposta.ListaItensProposta.Count(it => it.TipoItem.Id == 1);
            decimal? somaQuantidade = Proposta.ListaItensProposta.Where(it => it.TipoItem.Id == 1).Sum(it => it.QuantidadeItem);
            decimal? somaPrecoUnitarioInicial = Proposta.ListaItensProposta.Where(it => it.TipoItem.Id == 1).Sum(it => it.PrecoUnitarioInicialItem);
            decimal? somaPrecoUnitarioFinal = Proposta.ListaItensProposta.Where(it => it.TipoItem.Id == 1).Sum(it => it.PrecoUnitarioFinalItem);
            decimal? somaPrecoTotalInicial = Proposta.ListaItensProposta.Where(it => it.TipoItem.Id == 1).Sum(it => it.ValorTotalInicialItem);
            decimal? somaPrecoTotalFinal = Proposta.ListaItensProposta.Where(it => it.TipoItem.Id == 1).Sum(it => it.PrecoTotalFinalItem);

            foreach (var itemProposta in Proposta.ListaItensProposta)
            {
                if (itemProposta.TipoItem.Id == 1)
                {
                    switch (tipoDefinicaoFrete)
                    {
                        case TipoDefinicaoFrete.IgualParaTodos:
                            itemProposta.FreteUnitarioItem = ValorFrete;
                            break;

                        case TipoDefinicaoFrete.Contagem:
                            itemProposta.FreteUnitarioItem = ValorFrete * FuncoesMatematicas.DividirPorZeroDecimal(1, contagemItens);
                            break;

                        case TipoDefinicaoFrete.PorQuantidade:
                            itemProposta.FreteUnitarioItem = ValorFrete * FuncoesMatematicas.DividirPorZeroDecimal(itemProposta.QuantidadeItem, somaQuantidade);
                            break;

                        case TipoDefinicaoFrete.PorPrecoUnitarioInicial:
                            itemProposta.FreteUnitarioItem = ValorFrete * FuncoesMatematicas.DividirPorZeroDecimal(itemProposta.PrecoUnitarioInicialItem, somaPrecoUnitarioInicial);
                            break;

                        case TipoDefinicaoFrete.PorPrecoUnitarioFinal:
                            itemProposta.FreteUnitarioItem = ValorFrete * FuncoesMatematicas.DividirPorZeroDecimal(itemProposta.PrecoUnitarioFinalItem, somaPrecoUnitarioFinal);
                            break;

                        case TipoDefinicaoFrete.PorPrecoTotalInicial:
                            itemProposta.FreteUnitarioItem = ValorFrete * FuncoesMatematicas.DividirPorZeroDecimal(itemProposta.ValorTotalInicialItem, somaPrecoTotalInicial);
                            break;

                        case TipoDefinicaoFrete.PorPrecoTotalFinal:
                            itemProposta.FreteUnitarioItem = ValorFrete * FuncoesMatematicas.DividirPorZeroDecimal(itemProposta.PrecoTotalFinalItem, somaPrecoTotalFinal);
                            break;

                        default:
                            break;
                    }
                }
                await itemProposta.CalculaValoresItemPropostaAsync(Proposta.Cliente);
            }

            OnPropertyChanged(nameof(TextoTotalPecas));
            OnPropertyChanged(nameof(TextoTotalServicos));
            OnPropertyChanged(nameof(TextoTotalDeslocamentos));
            OnPropertyChanged(nameof(TextoTotalGeral));
        }

        private enum TipoDesconto
        {
            DescontoCusto,
            DescontoFinalApenasPecas,
            DescontoFinalApenasServicos,
            DescontoFinalApenasDeslocamentos,
            DescontoFinalTodosOsItens
        }

        private async Task DefinirDesconto(TipoDesconto tipoDesconto)
        {
            foreach (var itemProposta in Proposta.ListaItensProposta)
            {
                switch (tipoDesconto)
                {
                    case TipoDesconto.DescontoCusto:
                        if (itemProposta.TipoItem.Id == 1)
                        {
                            itemProposta.DescontoInicialItem = DescontoCusto;
                        }
                        break;

                    case TipoDesconto.DescontoFinalApenasPecas:
                        if (itemProposta.TipoItem.Id == 1)
                        {
                            itemProposta.DescontoFinalItem = DescontoFinalPecas;
                        }
                        break;

                    case TipoDesconto.DescontoFinalApenasServicos:
                        if (itemProposta.TipoItem.Id == 2)
                        {
                            itemProposta.DescontoFinalItem = DescontoFinalServicos;
                        }
                        break;

                    case TipoDesconto.DescontoFinalApenasDeslocamentos:
                        if (itemProposta.TipoItem.Id == 3)
                        {
                            itemProposta.DescontoFinalItem = DescontoFinalDeslocamentos;
                        }
                        break;

                    case TipoDesconto.DescontoFinalTodosOsItens:
                        itemProposta.DescontoFinalItem = DescontoFinalTodosOsItens;
                        break;

                    default:
                        break;
                }
                await itemProposta.CalculaValoresItemPropostaAsync(Proposta.Cliente);
            }
            OnPropertyChanged(nameof(TextoTotalPecas));
            OnPropertyChanged(nameof(TextoTotalServicos));
            OnPropertyChanged(nameof(TextoTotalDeslocamentos));
            OnPropertyChanged(nameof(TextoTotalGeral));
        }

        /// <summary>
        /// Método para confirmar a cópia do termo
        /// </summary>
        private async Task ConfirmarCopiaTermo()
        {
            try
            {
                if (CopiarDe == null)
                {
                    return;
                }

                if (!String.IsNullOrEmpty(Proposta.TextoPadrao) || !String.IsNullOrEmpty(Proposta.Observacoes) || !String.IsNullOrEmpty(Proposta.PrazoEntrega) ||
                    !String.IsNullOrEmpty(Proposta.CondicaoPagamento) || !String.IsNullOrEmpty(Proposta.Garantia) || !String.IsNullOrEmpty(Proposta.Validade))
                {
                    var mySettings = new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Sim",
                        NegativeButtonText = "Não"
                    };

                    var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Atenção", "Os termos atuais serão substituídos. Deseja continar? O processo não poderá ser desfeito",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);

                    if (respostaMensagem != MessageDialogResult.Affirmative)
                    {
                        return;
                    }
                }

                switch (CopiarDe.Id)
                {
                    case 1:
                        if (TermoClienteAtual == null)
                        {
                            return;
                        }
                        Proposta.TextoPadrao = TermoClienteAtual.TextoPadrao;
                        Proposta.Observacoes = TermoClienteAtual.Observacoes;
                        Proposta.PrazoEntrega = TermoClienteAtual.PrazoEntrega;
                        Proposta.CondicaoPagamento = TermoClienteAtual.CondicaoPagamento;
                        Proposta.Garantia = TermoClienteAtual.Garantia;
                        Proposta.Validade = TermoClienteAtual.Validade;
                        break;

                    case 2:
                        if (TermoClienteTodos == null)
                        {
                            return;
                        }
                        Proposta.TextoPadrao = TermoClienteTodos.TextoPadrao;
                        Proposta.Observacoes = TermoClienteTodos.Observacoes;
                        Proposta.PrazoEntrega = TermoClienteTodos.PrazoEntrega;
                        Proposta.CondicaoPagamento = TermoClienteTodos.CondicaoPagamento;
                        Proposta.Garantia = TermoClienteTodos.Garantia;
                        Proposta.Validade = TermoClienteTodos.Validade;
                        break;

                    case 3:
                        if (PropostaTermo == null)
                        {
                            return;
                        }
                        Proposta.TextoPadrao = PropostaTermo.TextoPadrao;
                        Proposta.Observacoes = PropostaTermo.Observacoes;
                        Proposta.PrazoEntrega = PropostaTermo.PrazoEntrega;
                        Proposta.CondicaoPagamento = PropostaTermo.CondicaoPagamento;
                        Proposta.Garantia = PropostaTermo.Garantia;
                        Proposta.Validade = PropostaTermo.Validade;
                        break;

                    case 4:
                        if (TermoSetor == null)
                        {
                            return;
                        }
                        Proposta.TextoPadrao = TermoSetor.TextoPadrao;
                        Proposta.Observacoes = TermoSetor.Observacoes;
                        Proposta.PrazoEntrega = TermoSetor.PrazoEntrega;
                        Proposta.CondicaoPagamento = TermoSetor.CondicaoPagamento;
                        Proposta.Garantia = TermoSetor.Garantia;
                        Proposta.Validade = TermoSetor.Validade;
                        break;

                    case 5:
                        if (TermoTodos == null)
                        {
                            return;
                        }
                        Proposta.TextoPadrao = TermoTodos.TextoPadrao;
                        Proposta.Observacoes = TermoTodos.Observacoes;
                        Proposta.PrazoEntrega = TermoTodos.PrazoEntrega;
                        Proposta.CondicaoPagamento = TermoTodos.CondicaoPagamento;
                        Proposta.Garantia = TermoTodos.Garantia;
                        Proposta.Validade = TermoTodos.Validade;
                        break;

                    default:
                        if (TermoTodos == null)
                        {
                            return;
                        }
                        Proposta.TextoPadrao = TermoTodos.TextoPadrao;
                        Proposta.Observacoes = TermoTodos.Observacoes;
                        Proposta.PrazoEntrega = TermoTodos.PrazoEntrega;
                        Proposta.CondicaoPagamento = TermoTodos.CondicaoPagamento;
                        Proposta.Garantia = TermoTodos.Garantia;
                        Proposta.Validade = TermoTodos.Validade;
                        break;
                }
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");

                ProgressoEhIndeterminavel = false;
                ValorProgresso = (double)0;
                MensagemStatus = "Erro ao copiar termo";
            }
        }

        /// <summary>
        /// Método para preencher a lista de termos de acordo com o cliente selecionado
        /// </summary>
        /// <param name="clienteRetorno">Cliente para retornar os termos</param>
        /// <param name="listaTermos">Lista de termos a ser preenchida</param>
        private async Task PreencheTermosCliente(Cliente clienteRetorno, ObservableCollection<Termo> listaTermos)
        {
            // Verifica se há algum cliente selecionado e preenche a lista caso verdadeiro
            if (clienteRetorno != null)
            {
                // Cria a string de seleção
                string stringSelecao = "SELECT "
                               + "term.id_termo AS IdTermo, "
                               + "term.nome AS NomeTermo, "
                               + "term.texto_padrao AS TextoPadrao, "
                               + "term.observacoes AS Observacoes, "
                               + "term.prazo_entrega AS PrazoEntrega, "
                               + "term.condicao_pagamento AS CondicaoPagamento, "
                               + "term.garantia AS Garantia, "
                               + "term.validade AS Validade, "
                               + "term.nome AS NomeAdicional, "
                               + "stat.id_status AS IdStatusTermo, "
                               + "stat.nome AS NomeStatusTermo "
                               + "FROM tb_termos_suporte AS tesu "
                               + "LEFT JOIN tb_termos AS term ON tesu.id_termo = term.id_termo "
                               + "LEFT JOIN tb_status AS stat ON stat.id_status = term.id_status "
                               + "WHERE tesu.id_cliente = @id_cliente";

                // Preenche a lista
                await Termo.PreencheListaTermosArgumentosGenericosAsync(listaTermos, true, null, CancellationToken.None,
                    stringSelecao, "@id_cliente", clienteRetorno.Id);
            }
        }

        /// <summary>
        /// Método para preencher a lista de termos de acordo com a proposta selecionada
        /// </summary>
        private async Task PreenchePropostasParaTermo()
        {
            // Verifica se há algum cliente selecionado e preenche a lista caso verdadeiro
            if (ClienteProposta != null)
            {
                // Preenche a lista
                try
                {
                    await Proposta.PreencheListaPropostasAsync(ListaPropostas, true, false, false, false, null, CancellationToken.None,
                            "WHERE prop.id_cliente = @id_cliente", "@id_cliente", ClienteProposta.Id);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Método para preencher a lista de termos pelo setor
        /// </summary>
        private async Task PreencheTermosSetor()
        {
            // Verifica se há algum setor selecionado e preenche a lista caso verdadeiro
            if (SetorTermo != null)
            {
                // Cria a string de seleção
                string stringSelecao = "SELECT "
                               + "term.id_termo AS IdTermo, "
                               + "term.nome AS NomeTermo, "
                               + "term.texto_padrao AS TextoPadrao, "
                               + "term.observacoes AS Observacoes, "
                               + "term.prazo_entrega AS PrazoEntrega, "
                               + "term.condicao_pagamento AS CondicaoPagamento, "
                               + "term.garantia AS Garantia, "
                               + "term.validade AS Validade, "
                               + "term.nome AS NomeAdicional, "
                               + "stat.id_status AS IdStatusTermo, "
                               + "stat.nome AS NomeStatusTermo "
                               + "FROM tb_termos_suporte AS tesu "
                               + "LEFT JOIN tb_termos AS term ON tesu.id_termo = term.id_termo "
                               + "LEFT JOIN tb_status AS stat ON stat.id_status = term.id_status "
                               + "WHERE tesu.id_setor = @id_setor GROUP BY term.id_termo";

                // Preenche a lista
                try
                {
                    await Termo.PreencheListaTermosArgumentosGenericosAsync(ListaTermosSetor, true, null, CancellationToken.None,
                    stringSelecao, "@id_setor", SetorTermo.Id);
                }
                catch (Exception)
                {
                }
            }
        }

        private async Task AlteraStatusAprovacaoUnico(int idStatusAprovacao)
        {
            StatusAprovacao statusAprovacao = new();
            await statusAprovacao.GetStatusAprovacaoDatabaseAsync(idStatusAprovacao, CancellationToken.None);

            //ItemPropostaSelecionado.StatusAprovacao = statusAprovacao;

            foreach (var item in Proposta.ListaItensProposta)
            {
                if (ListaItensPropostaSelecionados.Contains(item))
                    item.StatusAprovacao = statusAprovacao;
            }
        }

        private async Task AlteraStatusAprovacaoTodos(int idStatusAprovacao)
        {
            StatusAprovacao statusAprovacao = new();
            await statusAprovacao.GetStatusAprovacaoDatabaseAsync(idStatusAprovacao, CancellationToken.None);

            foreach (var item in Proposta.ListaItensProposta)
            {
                item.StatusAprovacao = statusAprovacao;
            }
        }

        private async Task AlteraJustificativaAprovacaoUnico(int idJustificativaAprovacao)
        {
            JustificativaAprovacao justificativaAprovacao = new();
            await justificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(idJustificativaAprovacao, CancellationToken.None);

            foreach (var item in Proposta.ListaItensProposta)
            {
                if (ListaItensPropostaSelecionados.Contains(item))
                    item.JustificativaAprovacao = justificativaAprovacao;
            }

            // ItemPropostaSelecionado.JustificativaAprovacao = justificativaAprovacao;
        }

        private async Task AlteraJustificativaAprovacaoTodos(int idJustificativaAprovacao)
        {
            JustificativaAprovacao justificativaAprovacao = new();
            await justificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(idJustificativaAprovacao, CancellationToken.None);

            foreach (var item in Proposta.ListaItensProposta)
            {
                item.JustificativaAprovacao = justificativaAprovacao;
            }
        }

        private void AlteraConjuntoUnico(Conjunto conjunto)
        {
            //ItemPropostaSelecionado.Especificacao = null;

            //if (conjunto.Id == 0)
            //    ItemPropostaSelecionado.Conjunto = null;
            //else
            //    ItemPropostaSelecionado.Conjunto = conjunto;
            foreach (var item in Proposta.ListaItensProposta.Where(t => t.TipoItem.Id == 1))
            {
                if (ListaItensPropostaSelecionados.Contains(item))
                {
                    item.Especificacao = null;

                    if (conjunto.Id == 0)
                        item.Conjunto = null;
                    else
                        item.Conjunto = conjunto;
                }
            }
        }

        private void AlteraConjuntoTodos(Conjunto conjunto)
        {
            foreach (var item in Proposta.ListaItensProposta.Where(t => t.TipoItem.Id == 1))
            {
                item.Especificacao = null;

                if (conjunto.Id == 0)
                    item.Conjunto = null;
                else
                    item.Conjunto = conjunto;
            }
        }

        private void AlteraEspecificacaoUnico(Especificacao especificacao)
        {
            foreach (var item in Proposta.ListaItensProposta.Where(t => t.TipoItem.Id == 1))
            {
                if (ListaItensPropostaSelecionados.Contains(item))
                {
                    if (especificacao.Id == 0)
                        item.Especificacao = null;
                    else
                    {
                        item.Conjunto = especificacao.Conjunto;
                        item.Especificacao = especificacao;
                    }
                }
            }

            //if (especificacao.Id == 0)
            //    ItemPropostaSelecionado.Especificacao = null;
            //else
            //{
            //    ItemPropostaSelecionado.Conjunto = especificacao.Conjunto;
            //    ItemPropostaSelecionado.Especificacao = especificacao;
            //}
        }

        private void AlteraEspecificacaoTodos(Especificacao especificacao)
        {
            foreach (var item in Proposta.ListaItensProposta.Where(t => t.TipoItem.Id == 1))
            {
                if (especificacao.Id == 0)
                    item.Especificacao = null;
                else
                {
                    item.Conjunto = especificacao.Conjunto;
                    item.Especificacao = especificacao;
                }
            }
        }

        private void VisualizarProposta()
        {
            //ProgressoEhIndeterminavel = true;
            //ValorProgresso = (double)0;
            //MensagemStatus = "Abrindo visualização da proposta";

            var win = new MetroWindow();
            win.Height = 802;
            win.Width = 956;
            win.Content = new VisualizarPropostaViewModel(DialogCoordinator.Instance, Proposta);
            win.Title = "Visualização da proposta " + Proposta.Cliente?.Nome + " - " + Proposta.CodigoProposta;
            win.ShowDialogsOverTitleBar = false;
            win.Owner = App.Current.MainWindow;
            win.Show();

            //ProgressoEhIndeterminavel = true;
            //ValorProgresso = (double)0;
            //MensagemStatus = "";
        }

        private async Task AbrirOrdemServico()
        {
            if (Proposta.OrdemServico == null)
            {
                return;
            }

            try
            {
                OrdemServico ordemServico = new();

                ordemServico.Id = await OrdemServico.GetIdOrdemServicoPeloNumero(CancellationToken.None, Proposta.OrdemServico);

                Messenger.Default.Send<OrdemServico>(ordemServico, "OrdemServicoAdicionar");
            }
            catch (Exception)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Falha na abertura", "Não foi possível abrir a ordem de serviço", MessageDialogStyle.Affirmative, mySettings);

                return;
            }
        }

        private async Task CopiarProposta(CopiarPropostaViewModel.TipoCopia tipoCopia)
        {
            try
            {
                var customDialog = new CustomDialog();

                var dataContext = new CopiarPropostaViewModel(Proposta, tipoCopia, instance =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                });

                customDialog.Content = new CopiarPropostaView { DataContext = dataContext };

                await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
            }
            catch (Exception ex)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Entendi"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Erro", "Falha ao abrir opções de cópia. Caso o problema persista, contate o desenvolvedor", MessageDialogStyle.Affirmative, mySettings);

                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao abrir copia");
            }
        }

        private async Task AlteraStatusAprovacaoProposta()
        {
            if (_dialogCoordinator == null || Proposta?.ListaItensProposta?.Count == 0 || ehCarregamento)
                return;

            try
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Sim",
                    NegativeButtonText = "Não"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                    "Atenção", "Deseja alterar também o status da aprovação de todos os itens existentes na proposta?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);

                if (respostaMensagem == MessageDialogResult.Affirmative)
                {
                    foreach (var itemProposta in Proposta.ListaItensProposta)
                    {
                        itemProposta.StatusAprovacao = Proposta.StatusAprovacao;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private async Task AlteraJustificativaAprovacaoProposta()
        {
            if (_dialogCoordinator == null || Proposta?.ListaItensProposta?.Count == 0 || ehCarregamento)
                return;

            try
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Sim",
                    NegativeButtonText = "Não"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                    "Atenção", "Deseja alterar também a justificativa da aprovação de todos os itens existentes na proposta?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);

                if (respostaMensagem == MessageDialogResult.Affirmative)
                {
                    foreach (var itemProposta in Proposta.ListaItensProposta)
                    {
                        itemProposta.JustificativaAprovacao = Proposta.JustificativaAprovacao;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public bool FiltraContatos(object item)
        {
            if (Cliente == null)
            {
                return false;
            }
            if (item is Contato contato)
            {
                return contato.Cliente.Id == Cliente.Id;
            }
            return true;
        }

        public bool FiltraEstados(object item)
        {
            if (Pais == null)
            {
                return false;
            }
            if (item is Estado estado)
            {
                return estado.Pais.Id == Pais.Id;
            }
            return true;
        }

        public bool FiltraCidades(object item)
        {
            if (Estado == null)
            {
                return false;
            }
            if (item is Cidade cidade)
            {
                return cidade.Estado.Id == Estado.Id;
            }
            return true;
        }

        public bool FiltraSeries(object item)
        {
            if (Cliente == null)
            {
                return false;
            }
            if (item is Serie serie)
            {
                return serie.Cliente.Id == Cliente.Id;
            }
            return true;
        }

        public bool FiltraModelos(object item)
        {
            if (Fabricante == null || TipoEquipamento == null)
            {
                return false;
            }
            if (item is Modelo modelo)
            {
                return modelo.Fabricante.Id == Fabricante.Id && modelo.TipoEquipamento.Id == TipoEquipamento.Id;
            }
            return true;
        }

        public bool LimpaListaContatos(object item)
        {
            return false;
        }

        public bool LimpaListaEstados(object item)
        {
            return false;
        }

        public bool LimpaListaCidades(object item)
        {
            return false;
        }

        public bool LimpaListaSeries(object item)
        {
            return false;
        }

        public bool LimpaListaModelos(object item)
        {
            return false;
        }

        #endregion Métodos

        #region Classes

        public class CopiarDeTermo : ObservableObject
        {
            private int _id;
            private string _nome;

            public CopiarDeTermo(int id, string nome)
            {
                Id = id;
                Nome = nome;
            }

            public int Id
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

            public string Nome
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
        }

        public CollectionView GetContatoCollectionView(ObservableCollection<Contato> contatoList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(contatoList);
        }

        public CollectionView GetEstadoCollectionView(ObservableCollection<Estado> estadoList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(estadoList);
        }

        public CollectionView GetCidadeCollectionView(ObservableCollection<Cidade> cidadeList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(cidadeList);
        }

        public CollectionView GetSerieCollectionView(ObservableCollection<Serie> serieList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(serieList);
        }

        public CollectionView GetModeloCollectionView(ObservableCollection<Modelo> modeloList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(modeloList);
        }

        #endregion Classes
    }
}