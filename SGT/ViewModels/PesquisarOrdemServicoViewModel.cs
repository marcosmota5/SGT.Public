using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class PesquisarOrdemServicoViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private string _name;
        private string _textoLimiteDeResultados;
        private DateTime? _dataDe;
        private DateTime? _dataAte;
        private int? _integerDe;
        private int? _integerAte;
        private decimal? _decimalDe;
        private decimal? _decimalAte;
        private string _textoPesquisa;
        private bool _booleanValue;
        private readonly IDialogCoordinator _dialogCoordinator;
        private ObservableCollection<Parametro> _listaParametros = new();
        private ObservableCollection<Parametro> _listaOpcaoPesquisa = new();
        private ObservableCollection<Parametro> _listaOperadores = new();
        private ObservableCollection<Parametro> _listaClassificarPor = new();
        private ObservableCollection<Parametro> _listaOrdem = new();
        private ObservableCollection<FiltroUsuario> _listaFiltroUsuario = new();
        private ObservableCollection<ParametroFiltroPesquisa> _listaFiltros = new();
        private ObservableCollection<ResultadoPesquisaOrdemServico> _listaResultadosPesquisaOrdemServico = new();
        private ObservableCollection<TextoFiltro> _listaTextoPesquisa = new();
        private Parametro _parametroSelecionado;
        private Parametro _opcaoPesquisaSelecionada;
        private Parametro _operadorSelecionado;
        private Parametro _classificarPorSelecionado;
        private Parametro _ordemSelecionada;
        private FiltroUsuario _filtroUsuarioCarregado;
        private ParametroFiltroPesquisa _filtroSelecionado;
        private ResultadoPesquisaOrdemServico _resultadoPesquisaOrdemServicoSelecionado;
        private CancellationTokenSource _cts;
        private string _textoResultadosEncontrados;
        private string _nomeNovoFiltroUsuario;
        private bool _consideraApenasAtivos = true;

        private int _filterSize = 300;
        private bool _filterVisible = true;
        private string _filterIcon = "ChevronLeftCircleOutline";

        private ConfiguracaoSistema _configuracaoSistema;

        private bool _pesquisaDataVisivel;
        private bool _pesquisaTextoVisivel;
        private bool _pesquisaIntegerVisivel;
        private bool _pesquisaDecimalVisivel;
        private bool _pesquisaBooleanVisivel;

        private bool _visibilidadeColunas;
        private double _valorProgresso;
        private bool _progressoEhIndeterminavel;
        private bool _progressoVisivel = false;
        private string _mensagemStatus;

        private bool _edicaoEhEnterprise;
        private bool _edicaoNaoEhEnterprise;

        private ICommand _comandoAdicionarFiltro;
        private ICommand _comandoSubirOrdemFiltro;
        private ICommand _comandoDescerOrdemFiltro;
        private ICommand _comandoRemoverFiltro;
        private ICommand _comandoVerificaDataDe;
        private ICommand _comandoVerificaDataAte;
        private ICommand _comandoPesquisar;
        private ICommand _comandoAbrirProposta;
        private ICommand _comandoExportarPesquisa;
        private ICommand _comandoSalvarFiltros;
        private ICommand _comandoCarregarFiltro;
        private ICommand _comandoDeletarFiltro;
        private ICommand _comandoAlterarTamanhoFiltro;

        private ICommand _comandoVisualizarOrdemServicoAtual;
        private ICommand _comandoVisualizarOrdemServicoPrimaria;

        // Dados da pesquisa básica
        private bool _pesquisaBasica = true;

        private string _textoPesquisado;

        private DateTime? _dataDeChamado;
        private DateTime? _dataAteChamado;
        private DateTime? _dataDeAtendimento;
        private DateTime? _dataAteAtendimento;
        private DateTime? _dataDeInsercao;
        private DateTime? _dataAteInsercao;

        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelSetores = new();
        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelUsuariosInsercao = new();
        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelClientes = new();
        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelTipoOrdemServico = new();
        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelEquipamentoAposManutencao = new();
        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelTipoManutencao = new();
        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelExecutanteServico = new();
        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelPassosExecutados = new();

        private ICommand _comandoLimparFiltroSetor;
        private ICommand _comandoLimparFiltroUsuarioInsercao;
        private ICommand _comandoLimparFiltroCliente;
        private ICommand _comandoLimparFiltroTipoOrdemServico;
        private ICommand _comandoLimparFiltroEquipamentoAposManutencao;
        private ICommand _comandoLimparFiltroTipoManutencao;
        private ICommand _comandoLimparFiltroExecutanteServico;
        private ICommand _comandoLimparFiltroPassosExecutados;

        private ICommand _comandoLimparFiltroDataChamado;
        private ICommand _comandoVerificaDataDeChamado;
        private ICommand _comandoVerificaDataAteChamado;
        private ICommand _comandoLimparFiltroDataAtendimento;
        private ICommand _comandoVerificaDataDeAtendimento;
        private ICommand _comandoVerificaDataAteAtendimento;
        private ICommand _comandoLimparFiltroDataInsercao;
        private ICommand _comandoVerificaDataDeInsercao;
        private ICommand _comandoVerificaDataAteInsercao;

        #endregion Campos

        #region Construtores

        public PesquisarOrdemServicoViewModel(IDialogCoordinator dialogCoordinator)
        {
            EdicaoEhEnterprise = App.EdicaoEhEnterprise;
            EdicaoNaoEhEnterprise = !App.EdicaoEhEnterprise;

            Name = "Carregando...";

            _dialogCoordinator = dialogCoordinator;

            if (App.Usuario.LimiteResultados == null)
            {
                TextoLimiteDeResultados = "*Sem limite de resultados. Caso a pesquisa esteja lenta, defina um limite na opção Perfil.";
            }
            else
            {
                TextoLimiteDeResultados = "*Limitado a " + App.Usuario.LimiteResultados.ToString() + " resultado (s). Você pode alterar isso na opção Perfil.";
            }

            Name = "Pesquisar OS";

            PreencheListas();

            // Chama o méodo assíncrono para construir o controle de itens
            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Enums

        private enum OrdemFiltro
        {
            Subir,
            Descer
        }

        #endregion Enums

        #region Propriedades/Comandos

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
                return "CarWrench";
            }
        }

        public SfDataGrid DataGrid { get; set; }

        public void LimparViewModel()
        {
            try
            {
                _listaParametros = null;
                _listaOpcaoPesquisa = null;
                _listaOperadores = null;
                _listaClassificarPor = null;
                _listaOrdem = null;
                _listaFiltroUsuario = null;
                _listaFiltros = null;
                _listaResultadosPesquisaOrdemServico = null;
                _listaTextoPesquisa = null;
                _parametroSelecionado = null;
                _opcaoPesquisaSelecionada = null;
                _operadorSelecionado = null;
                _classificarPorSelecionado = null;
                _ordemSelecionada = null;
                _filtroUsuarioCarregado = null;
                _filtroSelecionado = null;
                _resultadoPesquisaOrdemServicoSelecionado = null;
                _cts = null;
                _comandoAdicionarFiltro = null;
                _comandoSubirOrdemFiltro = null;
                _comandoDescerOrdemFiltro = null;
                _comandoRemoverFiltro = null;
                _comandoVerificaDataDe = null;
                _comandoVerificaDataAte = null;
                _comandoPesquisar = null;
                _comandoAbrirProposta = null;
                _comandoExportarPesquisa = null;
                _comandoVisualizarOrdemServicoAtual = null;
                _comandoVisualizarOrdemServicoPrimaria = null;
                _comandoSalvarFiltros = null;
                _comandoCarregarFiltro = null;
                _comandoDeletarFiltro = null;
            }
            catch (Exception)
            {
            }
        }

        public string TextoLimiteDeResultados
        {
            get { return _textoLimiteDeResultados; }
            set
            {
                if (value != _textoLimiteDeResultados)
                {
                    _textoLimiteDeResultados = value;
                    OnPropertyChanged(nameof(TextoLimiteDeResultados));
                }
            }
        }

        public DateTime? DataDe
        {
            get { return _dataDe; }
            set
            {
                if (value != _dataDe)
                {
                    _dataDe = value;
                    OnPropertyChanged(nameof(DataDe));
                }
            }
        }

        public DateTime? DataAte
        {
            get { return _dataAte; }
            set
            {
                if (value != _dataAte)
                {
                    _dataAte = value;
                    OnPropertyChanged(nameof(DataAte));
                }
            }
        }

        public int? IntegerDe
        {
            get { return _integerDe; }
            set
            {
                if (value != _integerDe)
                {
                    _integerDe = value;
                    OnPropertyChanged(nameof(IntegerDe));
                }
            }
        }

        public int? IntegerAte
        {
            get { return _integerAte; }
            set
            {
                if (value != _integerAte)
                {
                    _integerAte = value;
                    OnPropertyChanged(nameof(IntegerAte));
                }
            }
        }

        public decimal? DecimalDe
        {
            get { return _decimalDe; }
            set
            {
                if (value != _decimalDe)
                {
                    _decimalDe = value;
                    OnPropertyChanged(nameof(DecimalDe));
                }
            }
        }

        public decimal? DecimalAte
        {
            get { return _decimalAte; }
            set
            {
                if (value != _decimalAte)
                {
                    _decimalAte = value;
                    OnPropertyChanged(nameof(DecimalAte));
                }
            }
        }

        public string TextoPesquisa
        {
            get { return _textoPesquisa; }
            set
            {
                if (value != _textoPesquisa)
                {
                    _textoPesquisa = value;
                    OnPropertyChanged(nameof(TextoPesquisa));
                }
            }
        }

        public bool BooleanValue
        {
            get { return _booleanValue; }
            set
            {
                if (value != _booleanValue)
                {
                    _booleanValue = value;
                    OnPropertyChanged(nameof(BooleanValue));
                }
            }
        }

        public ObservableCollection<Parametro> ListaParametros
        {
            get { return _listaParametros; }
            set
            {
                if (value != _listaParametros)
                {
                    _listaParametros = value;
                    OnPropertyChanged(nameof(ListaParametros));
                }
            }
        }

        public ObservableCollection<Parametro> ListaOpcaoPesquisa
        {
            get { return _listaOpcaoPesquisa; }
            set
            {
                if (value != _listaOpcaoPesquisa)
                {
                    _listaOpcaoPesquisa = value;
                    OnPropertyChanged(nameof(ListaOpcaoPesquisa));
                }
            }
        }

        public ObservableCollection<Parametro> ListaOperadores
        {
            get { return _listaOperadores; }
            set
            {
                if (value != _listaOperadores)
                {
                    _listaOperadores = value;
                    OnPropertyChanged(nameof(ListaOperadores));
                }
            }
        }

        public ObservableCollection<Parametro> ListaClassificarPor
        {
            get { return _listaClassificarPor; }
            set
            {
                if (value != _listaClassificarPor)
                {
                    _listaClassificarPor = value;
                    OnPropertyChanged(nameof(ListaClassificarPor));
                }
            }
        }

        public ObservableCollection<Parametro> ListaOrdem
        {
            get { return _listaOrdem; }
            set
            {
                if (value != _listaOrdem)
                {
                    _listaOrdem = value;
                    OnPropertyChanged(nameof(ListaOrdem));
                }
            }
        }

        public ObservableCollection<FiltroUsuario> ListaFiltroUsuario
        {
            get { return _listaFiltroUsuario; }
            set
            {
                if (value != _listaFiltroUsuario)
                {
                    _listaFiltroUsuario = value;
                    OnPropertyChanged(nameof(ListaFiltroUsuario));
                }
            }
        }

        public ObservableCollection<ParametroFiltroPesquisa> ListaFiltros
        {
            get { return _listaFiltros; }
            set
            {
                if (value != _listaFiltros)
                {
                    _listaFiltros = value;
                    OnPropertyChanged(nameof(ListaFiltros));
                }
            }
        }

        public ObservableCollection<ResultadoPesquisaOrdemServico> ListaResultadosPesquisaOrdemServico
        {
            get { return _listaResultadosPesquisaOrdemServico; }
            set
            {
                if (value != _listaResultadosPesquisaOrdemServico)
                {
                    _listaResultadosPesquisaOrdemServico = value;
                    OnPropertyChanged(nameof(ListaResultadosPesquisaOrdemServico));
                }
            }
        }

        public ObservableCollection<TextoFiltro> ListaTextoPesquisa
        {
            get { return _listaTextoPesquisa; }
            set
            {
                if (value != _listaTextoPesquisa)
                {
                    _listaTextoPesquisa = value;
                    OnPropertyChanged(nameof(ListaTextoPesquisa));
                }
            }
        }

        public Parametro ParametroSelecionado
        {
            get { return _parametroSelecionado; }
            set
            {
                if (value != _parametroSelecionado)
                {
                    _parametroSelecionado = value;

                    switch (ParametroSelecionado.Tipo)
                    {
                        case "date":
                            PesquisaBooleanVisivel = false;
                            PesquisaDecimalVisivel = false;
                            PesquisaIntegerVisivel = false;
                            PesquisaDataVisivel = true;
                            PesquisaTextoVisivel = false;
                            break;

                        case "string":
                            PesquisaBooleanVisivel = false;
                            PesquisaDecimalVisivel = false;
                            PesquisaIntegerVisivel = false;
                            PesquisaDataVisivel = false;
                            PesquisaTextoVisivel = true;
                            break;

                        case "decimal":
                            PesquisaBooleanVisivel = false;
                            PesquisaDecimalVisivel = true;
                            PesquisaIntegerVisivel = false;
                            PesquisaDataVisivel = false;
                            PesquisaTextoVisivel = false;
                            break;

                        case "integer":
                            PesquisaBooleanVisivel = false;
                            PesquisaDecimalVisivel = false;
                            PesquisaIntegerVisivel = true;
                            PesquisaDataVisivel = false;
                            PesquisaTextoVisivel = false;
                            break;

                        case "boolean":
                            PesquisaBooleanVisivel = true;
                            PesquisaDecimalVisivel = false;
                            PesquisaIntegerVisivel = false;
                            PesquisaDataVisivel = false;
                            PesquisaTextoVisivel = false;
                            break;

                        default:
                            PesquisaBooleanVisivel = false;
                            PesquisaDecimalVisivel = false;
                            PesquisaIntegerVisivel = false;
                            PesquisaDataVisivel = false;
                            PesquisaTextoVisivel = true;
                            break;
                    }

                    DataDe = null;
                    DataAte = null;
                    IntegerDe = null;
                    IntegerAte = null;
                    DecimalDe = null;
                    DecimalAte = null;
                    TextoPesquisa = "";
                    BooleanValue = false;

                    OnPropertyChanged(nameof(ParametroSelecionado));

                    PreecheListaTextoRetornado().Await();
                }
            }
        }

        public Parametro OpcaoPesquisaSelecionada
        {
            get { return _opcaoPesquisaSelecionada; }
            set
            {
                if (value != _opcaoPesquisaSelecionada)
                {
                    _opcaoPesquisaSelecionada = value;
                    OnPropertyChanged(nameof(OpcaoPesquisaSelecionada));
                }
            }
        }

        public Parametro OperadorSelecionado
        {
            get { return _operadorSelecionado; }
            set
            {
                if (value != _operadorSelecionado)
                {
                    _operadorSelecionado = value;
                    OnPropertyChanged(nameof(OperadorSelecionado));
                }
            }
        }

        public Parametro ClassificarPorSelecionado
        {
            get { return _classificarPorSelecionado; }
            set
            {
                if (value != _classificarPorSelecionado)
                {
                    _classificarPorSelecionado = value;
                    OnPropertyChanged(nameof(ClassificarPorSelecionado));
                }
            }
        }

        public Parametro OrdemSelecionada
        {
            get { return _ordemSelecionada; }
            set
            {
                if (value != _ordemSelecionada)
                {
                    _ordemSelecionada = value;
                    OnPropertyChanged(nameof(OrdemSelecionada));
                }
            }
        }

        public FiltroUsuario FiltroUsuarioCarregado
        {
            get { return _filtroUsuarioCarregado; }
            set
            {
                if (value != _filtroUsuarioCarregado)
                {
                    _filtroUsuarioCarregado = value;
                    OnPropertyChanged(nameof(FiltroUsuarioCarregado));
                }
            }
        }

        public string NomeNovoFiltroUsuario
        {
            get { return _nomeNovoFiltroUsuario; }
            set
            {
                if (value != _nomeNovoFiltroUsuario)
                {
                    _nomeNovoFiltroUsuario = value;
                    OnPropertyChanged(nameof(NomeNovoFiltroUsuario));
                }
            }
        }

        public ParametroFiltroPesquisa FiltroSelecionado
        {
            get { return _filtroSelecionado; }
            set
            {
                if (value != _filtroSelecionado)
                {
                    _filtroSelecionado = value;
                    OnPropertyChanged(nameof(FiltroSelecionado));
                }
            }
        }

        public ResultadoPesquisaOrdemServico ResultadoPesquisaOrdemServicoSelecionado
        {
            get { return _resultadoPesquisaOrdemServicoSelecionado; }
            set
            {
                if (value != _resultadoPesquisaOrdemServicoSelecionado)
                {
                    _resultadoPesquisaOrdemServicoSelecionado = value;
                    OnPropertyChanged(nameof(ResultadoPesquisaOrdemServicoSelecionado));
                }
            }
        }

        public bool ConsideraApenasAtivos
        {
            get { return _consideraApenasAtivos; }
            set
            {
                if (value != _consideraApenasAtivos)
                {
                    _consideraApenasAtivos = value;
                    OnPropertyChanged(nameof(ConsideraApenasAtivos));
                }
            }
        }

        public string TextoResultadosEncontrados
        {
            get { return _textoResultadosEncontrados; }
            set
            {
                if (value != _textoResultadosEncontrados)
                {
                    _textoResultadosEncontrados = value;
                    OnPropertyChanged(nameof(TextoResultadosEncontrados));
                }
            }
        }

        public int FilterSize
        {
            get { return _filterSize; }
            set
            {
                if (value != _filterSize)
                {
                    _filterSize = value;
                    OnPropertyChanged(nameof(FilterSize));
                }
            }
        }

        public bool FilterVisible
        {
            get { return _filterVisible; }
            set
            {
                if (value != _filterVisible)
                {
                    _filterVisible = value;
                    OnPropertyChanged(nameof(FilterVisible));
                }
            }
        }

        public string FilterIcon
        {
            get { return _filterIcon; }
            set
            {
                if (value != _filterIcon)
                {
                    _filterIcon = value;
                    OnPropertyChanged(nameof(FilterIcon));
                }
            }
        }

        public bool PesquisaDataVisivel
        {
            get { return _pesquisaDataVisivel; }
            set
            {
                if (value != _pesquisaDataVisivel)
                {
                    _pesquisaDataVisivel = value;
                    OnPropertyChanged(nameof(PesquisaDataVisivel));
                }
            }
        }

        public bool PesquisaTextoVisivel
        {
            get { return _pesquisaTextoVisivel; }
            set
            {
                if (value != _pesquisaTextoVisivel)
                {
                    _pesquisaTextoVisivel = value;
                    OnPropertyChanged(nameof(PesquisaTextoVisivel));
                }
            }
        }

        public bool PesquisaDecimalVisivel
        {
            get { return _pesquisaDecimalVisivel; }
            set
            {
                if (value != _pesquisaDecimalVisivel)
                {
                    _pesquisaDecimalVisivel = value;
                    OnPropertyChanged(nameof(PesquisaDecimalVisivel));
                }
            }
        }

        public bool PesquisaBooleanVisivel
        {
            get { return _pesquisaBooleanVisivel; }
            set
            {
                if (value != _pesquisaBooleanVisivel)
                {
                    _pesquisaBooleanVisivel = value;
                    OnPropertyChanged(nameof(PesquisaBooleanVisivel));
                }
            }
        }

        public bool VisibilidadeColunas
        {
            get { return _visibilidadeColunas; }
            set
            {
                if (value != _visibilidadeColunas)
                {
                    _visibilidadeColunas = value;
                    OnPropertyChanged(nameof(VisibilidadeColunas));
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

        public bool EdicaoNaoEhEnterprise
        {
            get { return _edicaoNaoEhEnterprise; }
            set
            {
                if (value != _edicaoNaoEhEnterprise)
                {
                    _edicaoNaoEhEnterprise = value;
                    OnPropertyChanged(nameof(EdicaoNaoEhEnterprise));
                }
            }
        }

        public bool PesquisaIntegerVisivel
        {
            get { return _pesquisaIntegerVisivel; }
            set
            {
                if (value != _pesquisaIntegerVisivel)
                {
                    _pesquisaIntegerVisivel = value;
                    OnPropertyChanged(nameof(PesquisaIntegerVisivel));
                }
            }
        }

        public ICommand ComandoAdicionarFiltro
        {
            get
            {
                if (_comandoAdicionarFiltro == null)
                {
                    _comandoAdicionarFiltro = new RelayCommand(
                        param => AdicionarFiltro(ParametroSelecionado, OperadorSelecionado, OpcaoPesquisaSelecionada),
                        param => true
                    );
                }
                return _comandoAdicionarFiltro;
            }
        }

        public ICommand ComandoSubirOrdemFiltro
        {
            get
            {
                if (_comandoSubirOrdemFiltro == null)
                {
                    _comandoSubirOrdemFiltro = new RelayCommand(
                        param => MoverFiltro(FiltroSelecionado, OrdemFiltro.Descer),
                        param => FiltroSelecionado != null
                    );
                }
                return _comandoSubirOrdemFiltro;
            }
        }

        public ICommand ComandoDescerOrdemFiltro
        {
            get
            {
                if (_comandoDescerOrdemFiltro == null)
                {
                    _comandoDescerOrdemFiltro = new RelayCommand(
                        param => MoverFiltro(FiltroSelecionado, OrdemFiltro.Subir),
                        param => FiltroSelecionado != null
                    );
                }
                return _comandoDescerOrdemFiltro;
            }
        }

        public ICommand ComandoRemoverFiltro
        {
            get
            {
                if (_comandoRemoverFiltro == null)
                {
                    _comandoRemoverFiltro = new RelayCommand(
                        param => RemoverFiltro(FiltroSelecionado),
                        param => FiltroSelecionado != null
                    );
                }
                return _comandoRemoverFiltro;
            }
        }

        public ICommand ComandoVerificaDataDe
        {
            get
            {
                if (_comandoVerificaDataDe == null)
                {
                    _comandoVerificaDataDe = new RelayCommand(
                        param => VerificaDataDe(),
                        param => true
                    );
                }
                return _comandoVerificaDataDe;
            }
        }

        public ICommand ComandoVerificaDataAte
        {
            get
            {
                if (_comandoVerificaDataAte == null)
                {
                    _comandoVerificaDataAte = new RelayCommand(
                        param => VerificaDataAte(),
                        param => true
                    );
                }
                return _comandoVerificaDataAte;
            }
        }

        public ICommand ComandoPesquisar
        {
            get
            {
                if (_comandoPesquisar == null)
                {
                    _comandoPesquisar = new RelayCommand(
                        param => ExecutarPesquisa(CancellationToken.None).Await(),
                        param => true
                    );
                }
                return _comandoPesquisar;
            }
        }

        public ICommand ComandoAbrirProposta
        {
            get
            {
                if (_comandoAbrirProposta == null)
                {
                    _comandoAbrirProposta = new RelayCommand(
                        param => AbrirOrdemServico().Await(),
                        param => true
                    );
                }
                return _comandoAbrirProposta;
            }
        }

        public ICommand ComandoExportarPesquisa
        {
            get
            {
                if (_comandoExportarPesquisa == null)
                {
                    _comandoExportarPesquisa = new RelayCommand(
                        param => ExportarPesquisa().Await(),
                        param => true
                    );
                }
                return _comandoExportarPesquisa;
            }
        }

        public ICommand ComandoAlterarTamanhoFiltro
        {
            get
            {
                if (_comandoAlterarTamanhoFiltro == null)
                {
                    _comandoAlterarTamanhoFiltro = new RelayCommand(
                        param => AlterarTamanhoFiltro(),
                        param => true
                    );
                }
                return _comandoAlterarTamanhoFiltro;
            }
        }

        public ICommand ComandoVisualizarOrdemServicoAtual
        {
            get
            {
                if (_comandoVisualizarOrdemServicoAtual == null)
                {
                    _comandoVisualizarOrdemServicoAtual = new RelayCommand(
                        param => VisualizarOrdemServico(ResultadoPesquisaOrdemServicoSelecionado.NumeroOrdemServicoAtual).Await(),
                        param => true
                    );
                }
                return _comandoVisualizarOrdemServicoAtual;
            }
        }

        public ICommand ComandoVisualizarOrdemServicoPrimaria
        {
            get
            {
                if (_comandoVisualizarOrdemServicoPrimaria == null)
                {
                    _comandoVisualizarOrdemServicoPrimaria = new RelayCommand(
                        param => VisualizarOrdemServico(ResultadoPesquisaOrdemServicoSelecionado.NumeroOrdemServicoPrimaria).Await(),
                        param => true
                    );
                }
                return _comandoVisualizarOrdemServicoPrimaria;
            }
        }

        public ICommand ComandoSalvarFiltros
        {
            get
            {
                if (_comandoSalvarFiltros == null)
                {
                    _comandoSalvarFiltros = new RelayCommand(
                        param => SalvarFiltros().Await(),
                        param => true
                    );
                }
                return _comandoSalvarFiltros;
            }
        }

        public ICommand ComandoCarregarFiltro
        {
            get
            {
                if (_comandoCarregarFiltro == null)
                {
                    _comandoCarregarFiltro = new RelayCommand(CarregaFiltro);
                }
                return _comandoCarregarFiltro;
            }
        }

        public ICommand ComandoDeletarFiltro
        {
            get
            {
                if (_comandoDeletarFiltro == null)
                {
                    _comandoDeletarFiltro = new RelayCommand(DeletaFiltro);
                }
                return _comandoDeletarFiltro;
            }
        }

        // Dados da pesquisa básica
        public bool PesquisaBasica
        {
            get { return _pesquisaBasica; }
            set
            {
                if (value != _pesquisaBasica)
                {
                    _pesquisaBasica = value;
                    OnPropertyChanged(nameof(PesquisaBasica));
                }
            }
        }

        public string TextoPesquisado
        {
            get { return _textoPesquisado; }
            set
            {
                if (value != _textoPesquisado)
                {
                    _textoPesquisado = value;
                    OnPropertyChanged(nameof(TextoPesquisado));
                }
            }
        }

        public DateTime? DataDeChamado
        {
            get { return _dataDeChamado; }
            set
            {
                if (value != _dataDeChamado)
                {
                    _dataDeChamado = value;
                    OnPropertyChanged(nameof(DataDeChamado));
                }
            }
        }

        public DateTime? DataAteChamado
        {
            get { return _dataAteChamado; }
            set
            {
                if (value != _dataAteChamado)
                {
                    _dataAteChamado = value;
                    OnPropertyChanged(nameof(DataAteChamado));
                }
            }
        }

        public DateTime? DataDeAtendimento
        {
            get { return _dataDeAtendimento; }
            set
            {
                if (value != _dataDeAtendimento)
                {
                    _dataDeAtendimento = value;
                    OnPropertyChanged(nameof(DataDeAtendimento));
                }
            }
        }

        public DateTime? DataAteAtendimento
        {
            get { return _dataAteAtendimento; }
            set
            {
                if (value != _dataAteAtendimento)
                {
                    _dataAteAtendimento = value;
                    OnPropertyChanged(nameof(DataAteAtendimento));
                }
            }
        }

        public DateTime? DataDeInsercao
        {
            get { return _dataDeInsercao; }
            set
            {
                if (value != _dataDeInsercao)
                {
                    _dataDeInsercao = value;
                    OnPropertyChanged(nameof(DataDeInsercao));
                }
            }
        }

        public DateTime? DataAteInsercao
        {
            get { return _dataAteInsercao; }
            set
            {
                if (value != _dataAteInsercao)
                {
                    _dataAteInsercao = value;
                    OnPropertyChanged(nameof(DataAteInsercao));
                }
            }
        }

        public ObservableCollection<ItemFiltro> ListaObjetoSelecionavelSetores
        {
            get { return _listaObjetoSelecionavelSetores; }
            set
            {
                if (value != _listaObjetoSelecionavelSetores)
                {
                    _listaObjetoSelecionavelSetores = value;
                    OnPropertyChanged(nameof(ListaObjetoSelecionavelSetores));
                }
            }
        }

        public ObservableCollection<ItemFiltro> ListaObjetoSelecionavelUsuariosInsercao
        {
            get { return _listaObjetoSelecionavelUsuariosInsercao; }
            set
            {
                if (value != _listaObjetoSelecionavelUsuariosInsercao)
                {
                    _listaObjetoSelecionavelUsuariosInsercao = value;
                    OnPropertyChanged(nameof(ListaObjetoSelecionavelUsuariosInsercao));
                }
            }
        }

        public ObservableCollection<ItemFiltro> ListaObjetoSelecionavelClientes
        {
            get { return _listaObjetoSelecionavelClientes; }
            set
            {
                if (value != _listaObjetoSelecionavelClientes)
                {
                    _listaObjetoSelecionavelClientes = value;
                    OnPropertyChanged(nameof(ListaObjetoSelecionavelClientes));
                }
            }
        }

        public ObservableCollection<ItemFiltro> ListaObjetoSelecionavelTipoOrdemServico
        {
            get { return _listaObjetoSelecionavelTipoOrdemServico; }
            set
            {
                if (value != _listaObjetoSelecionavelTipoOrdemServico)
                {
                    _listaObjetoSelecionavelTipoOrdemServico = value;
                    OnPropertyChanged(nameof(ListaObjetoSelecionavelTipoOrdemServico));
                }
            }
        }

        public ObservableCollection<ItemFiltro> ListaObjetoSelecionavelEquipamentoAposManutencao
        {
            get { return _listaObjetoSelecionavelEquipamentoAposManutencao; }
            set
            {
                if (value != _listaObjetoSelecionavelEquipamentoAposManutencao)
                {
                    _listaObjetoSelecionavelEquipamentoAposManutencao = value;
                    OnPropertyChanged(nameof(ListaObjetoSelecionavelEquipamentoAposManutencao));
                }
            }
        }

        public ObservableCollection<ItemFiltro> ListaObjetoSelecionavelTipoManutencao
        {
            get { return _listaObjetoSelecionavelTipoManutencao; }
            set
            {
                if (value != _listaObjetoSelecionavelTipoManutencao)
                {
                    _listaObjetoSelecionavelTipoManutencao = value;
                    OnPropertyChanged(nameof(ListaObjetoSelecionavelTipoManutencao));
                }
            }
        }

        public ObservableCollection<ItemFiltro> ListaObjetoSelecionavelExecutanteServico
        {
            get { return _listaObjetoSelecionavelExecutanteServico; }
            set
            {
                if (value != _listaObjetoSelecionavelExecutanteServico)
                {
                    _listaObjetoSelecionavelExecutanteServico = value;
                    OnPropertyChanged(nameof(ListaObjetoSelecionavelExecutanteServico));
                }
            }
        }

        public ObservableCollection<ItemFiltro> ListaObjetoSelecionavelPassosExecutados
        {
            get { return _listaObjetoSelecionavelPassosExecutados; }
            set
            {
                if (value != _listaObjetoSelecionavelPassosExecutados)
                {
                    _listaObjetoSelecionavelPassosExecutados = value;
                    OnPropertyChanged(nameof(ListaObjetoSelecionavelPassosExecutados));
                }
            }
        }

        public ICommand ComandoLimparFiltroSetor
        {
            get
            {
                if (_comandoLimparFiltroSetor == null)
                {
                    _comandoLimparFiltroSetor = new RelayCommand(
                        param => LimparLista(ListaObjetoSelecionavelSetores),
                        param => true
                    );
                }
                return _comandoLimparFiltroSetor;
            }
        }

        public ICommand ComandoLimparFiltroUsuarioInsercao
        {
            get
            {
                if (_comandoLimparFiltroUsuarioInsercao == null)
                {
                    _comandoLimparFiltroUsuarioInsercao = new RelayCommand(
                        param => LimparLista(ListaObjetoSelecionavelUsuariosInsercao),
                        param => true
                    );
                }
                return _comandoLimparFiltroUsuarioInsercao;
            }
        }

        public ICommand ComandoLimparFiltroCliente
        {
            get
            {
                if (_comandoLimparFiltroCliente == null)
                {
                    _comandoLimparFiltroCliente = new RelayCommand(
                        param => LimparLista(ListaObjetoSelecionavelClientes),
                        param => true
                    );
                }
                return _comandoLimparFiltroCliente;
            }
        }

        public ICommand ComandoLimparFiltroTipoOrdemServico
        {
            get
            {
                if (_comandoLimparFiltroTipoOrdemServico == null)
                {
                    _comandoLimparFiltroTipoOrdemServico = new RelayCommand(
                        param => LimparLista(ListaObjetoSelecionavelTipoOrdemServico),
                        param => true
                    );
                }
                return _comandoLimparFiltroTipoOrdemServico;
            }
        }

        public ICommand ComandoLimparFiltroEquipamentoAposManutencao
        {
            get
            {
                if (_comandoLimparFiltroEquipamentoAposManutencao == null)
                {
                    _comandoLimparFiltroEquipamentoAposManutencao = new RelayCommand(
                        param => LimparLista(ListaObjetoSelecionavelEquipamentoAposManutencao),
                        param => true
                    );
                }
                return _comandoLimparFiltroEquipamentoAposManutencao;
            }
        }

        public ICommand ComandoLimparFiltroTipoManutencao
        {
            get
            {
                if (_comandoLimparFiltroTipoManutencao == null)
                {
                    _comandoLimparFiltroTipoManutencao = new RelayCommand(
                        param => LimparLista(ListaObjetoSelecionavelTipoManutencao),
                        param => true
                    );
                }
                return _comandoLimparFiltroTipoManutencao;
            }
        }

        public ICommand ComandoLimparFiltroExecutanteServico
        {
            get
            {
                if (_comandoLimparFiltroExecutanteServico == null)
                {
                    _comandoLimparFiltroExecutanteServico = new RelayCommand(
                        param => LimparLista(ListaObjetoSelecionavelExecutanteServico),
                        param => true
                    );
                }
                return _comandoLimparFiltroExecutanteServico;
            }
        }

        public ICommand ComandoLimparFiltroPassosExecutados
        {
            get
            {
                if (_comandoLimparFiltroPassosExecutados == null)
                {
                    _comandoLimparFiltroPassosExecutados = new RelayCommand(
                        param => LimparLista(ListaObjetoSelecionavelPassosExecutados),
                        param => true
                    );
                }
                return _comandoLimparFiltroPassosExecutados;
            }
        }

        public ICommand ComandoLimparFiltroDataChamado
        {
            get
            {
                if (_comandoLimparFiltroDataChamado == null)
                {
                    _comandoLimparFiltroDataChamado = new RelayCommand(
                        param => LimparFiltroDataChamado(),
                        param => true
                    );
                }
                return _comandoLimparFiltroDataChamado;
            }
        }

        public ICommand ComandoVerificaDataDeChamado
        {
            get
            {
                if (_comandoVerificaDataDeChamado == null)
                {
                    _comandoVerificaDataDeChamado = new RelayCommand(
                        param => VerificaDataDeChamado(),
                        param => true
                    );
                }
                return _comandoVerificaDataDeChamado;
            }
        }

        public ICommand ComandoVerificaDataAteChamado
        {
            get
            {
                if (_comandoVerificaDataAteChamado == null)
                {
                    _comandoVerificaDataAteChamado = new RelayCommand(
                        param => VerificaDataAteChamado(),
                        param => true
                    );
                }
                return _comandoVerificaDataAteChamado;
            }
        }

        public ICommand ComandoLimparFiltroDataAtendimento
        {
            get
            {
                if (_comandoLimparFiltroDataAtendimento == null)
                {
                    _comandoLimparFiltroDataAtendimento = new RelayCommand(
                        param => LimparFiltroDataAtendimento(),
                        param => true
                    );
                }
                return _comandoLimparFiltroDataAtendimento;
            }
        }

        public ICommand ComandoVerificaDataDeAtendimento
        {
            get
            {
                if (_comandoVerificaDataDeAtendimento == null)
                {
                    _comandoVerificaDataDeAtendimento = new RelayCommand(
                        param => VerificaDataDeAtendimento(),
                        param => true
                    );
                }
                return _comandoVerificaDataDeAtendimento;
            }
        }

        public ICommand ComandoVerificaDataAteAtendimento
        {
            get
            {
                if (_comandoVerificaDataAteAtendimento == null)
                {
                    _comandoVerificaDataAteAtendimento = new RelayCommand(
                        param => VerificaDataAteAtendimento(),
                        param => true
                    );
                }
                return _comandoVerificaDataAteAtendimento;
            }
        }

        public ICommand ComandoLimparFiltroDataInsercao
        {
            get
            {
                if (_comandoLimparFiltroDataInsercao == null)
                {
                    _comandoLimparFiltroDataInsercao = new RelayCommand(
                        param => LimparFiltroDataInsercao(),
                        param => true
                    );
                }
                return _comandoLimparFiltroDataInsercao;
            }
        }

        public ICommand ComandoVerificaDataDeInsercao
        {
            get
            {
                if (_comandoVerificaDataDeInsercao == null)
                {
                    _comandoVerificaDataDeInsercao = new RelayCommand(
                        param => VerificaDataDeInsercao(),
                        param => true
                    );
                }
                return _comandoVerificaDataDeInsercao;
            }
        }

        public ICommand ComandoVerificaDataAteInsercao
        {
            get
            {
                if (_comandoVerificaDataAteInsercao == null)
                {
                    _comandoVerificaDataAteInsercao = new RelayCommand(
                        param => VerificaDataAteInsercao(),
                        param => true
                    );
                }
                return _comandoVerificaDataAteInsercao;
            }
        }

        #endregion Propriedades/Comandos

        #region Métodos

        private void PreencheListas()
        {
            try
            {
                // Adiciona na lista os itens referentes a tabela de propostas
                ListaParametros.Add(new Parametro("Ordem de serviço", "orse.ordem_servico_atual", "integer", "N0", "Numeric", "SteelBlue"));
                ListaParametros.Add(new Parametro("Ordem de serviço primária", "orse.ordem_servico_primaria", "integer", "N0", "Numeric", "SteelBlue"));
                ListaParametros.Add(new Parametro("Número do chamado", "orse.numero_chamado", "integer", "N0", "Numeric", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data do chamado", "orse.data_chamado", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data do atendimento", "orse.data_atendimento", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data de inserção", "orse.data_insercao", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Mastro", "orse.mastro", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Código da falha", "orse.codigo_falha", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Etapas concluídas", "orse.etapas_concluidas", "integer", "N0", "Numeric", "SteelBlue"));
                ListaParametros.Add(new Parametro("Equipamento operacional", "orse.equipamento_operacional", "boolean", "", "CheckUnderlineCircleOutline", "SteelBlue"));
                ListaParametros.Add(new Parametro("Horímetro", "orse.horimetro", "decimal", "N2", "DecimalComma", "SteelBlue"));
                ListaParametros.Add(new Parametro("Horas de preventiva", "orse.horas_preventiva", "decimal", "N2", "DecimalComma", "SteelBlue"));
                ListaParametros.Add(new Parametro("Comentário do tipo da manutenção", "orse.outro_tipo_manutencao", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Motivo do atendimento", "orse.motivo_atendimento", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Entrevista inicial", "orse.entrevista_inicial", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Intervenção", "orse.intervencao", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data da saída", "orse.data_saida", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data da chegada", "orse.data_chegada", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data do retorno", "orse.data_retorno", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Comentários", "orse.comentarios", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data de edição", "orse.data_edicao", "date", "", "Calendar", "SteelBlue"));

                ListaParametros.Add(new Parametro("Filial", "fili.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Cliente", "clie.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Planta", "plan.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Área", "area.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Frota", "frot.nome", "string", "", "CardText", "SteelBlue"));

                ListaParametros.Add(new Parametro("Setor do usuário de inserção", "seto_ins.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Usuário de inserção", "orse_usua_ins.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Setor do usuário de edição", "seto_edi.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Usuário de edição", "orse_usua_edi.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Status da ordem de serviço", "orse_stat.nome", "string", "", "CardText", "SteelBlue"));

                ListaParametros.Add(new Parametro("Fabricante do equipamento", "fabr.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Categoria do equipamento", "cate.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Classe do equipamento", "clas.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Tipo do equipamento", "tieq.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Modelo do equipamento", "mode.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Ano do equipamento", "ano.ano", "integer", "N0", "Numeric", "SteelBlue"));
                ListaParametros.Add(new Parametro("Série do equipamento", "seri.nome", "string", "", "CardText", "SteelBlue"));

                ListaParametros.Add(new Parametro("Tipo da ordem de serviço", "tios.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Status do equipamento após a manutenção", "staq.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Uso indevido", "usind.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Tipo da manutenção", "tima.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Executante do serviço", "exse.nome", "string", "", "CardText", "SteelBlue"));

                ListaParametros = new ObservableCollection<Parametro>(ListaParametros.OrderBy(x => x.Nome));

                // Preenche a lista de opções de pesquisa
                ListaOpcaoPesquisa.Add(new Parametro("É igual a", "igual_a", "string", "", "Equal", "SteelBlue"));
                ListaOpcaoPesquisa.Add(new Parametro("É diferente de", "diferente_de", "string", "", "NotEqual", "IndianRed"));
                ListaOpcaoPesquisa.Add(new Parametro("Começa com", "comeca_com", "string", "", "ContainStart", "SteelBlue"));
                ListaOpcaoPesquisa.Add(new Parametro("Não começa com", "nao_comeca_com", "string", "", "ContainStart", "IndianRed"));
                ListaOpcaoPesquisa.Add(new Parametro("Termina com", "termina_com", "string", "", "ContainEnd", "SteelBlue"));
                ListaOpcaoPesquisa.Add(new Parametro("Não termina com", "nao_termina_com", "string", "", "ContainEnd", "IndianRed"));
                ListaOpcaoPesquisa.Add(new Parametro("Contém", "contem", "string", "", "Contain", "SteelBlue"));
                ListaOpcaoPesquisa.Add(new Parametro("Não contém", "nao_contem", "string", "", "Contain", "IndianRed"));

                // Preenche a lista de operadores
                ListaOperadores.Add(new Parametro("E", "AND", "string", "", "", "SteelBlue"));
                ListaOperadores.Add(new Parametro("Ou", "OR", "string", "", "", "SteelBlue"));

                // Preenche a lista de classificar por
                ListaClassificarPor.Add(new Parametro("Ordem de serviço", "NumeroOrdemServicoAtual", "string", "", "Numeric", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Ordem de serviço primária", "NumeroOrdemServicoPrimaria", "string", "", "Numeric", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Número do chamado", "NumeroChamado", "string", "", "Numeric", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Data da inserção", "DataInsercao", "string", "", "Calendar", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Data do chamado", "DataChamado", "string", "", "Calendar", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Data do atendimento", "DataAtendimento", "string", "", "Calendar", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Etapas concluídas", "EtapasConcluidas", "string", "", "Numeric", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Horímetro", "Horimetro", "string", "", "Numeric", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Data da saída", "DataSaida", "string", "", "Calendar", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Data da chegada", "DataChegada", "string", "", "Calendar", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Data do retorno", "DataRetorno", "string", "", "Calendar", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Cliente", "NomeCliente", "string", "", "CardText", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Executante do serviço", "NomeExecutanteServico", "string", "", "CardText", "SteelBlue"));

                // Preenche a lista de ordens
                ListaOrdem.Add(new Parametro("Crescente", "ASC", "string", "", "SortAlphabeticalAscending", "SteelBlue"));
                ListaOrdem.Add(new Parametro("Decrescente", "DESC", "string", "", "SortAlphabeticalDescending", "SteelBlue"));

                // Define os itens selecionados como os primeiros das listas
                ParametroSelecionado = ListaParametros.First();

                try
                {
                    ParametroSelecionado = ListaParametros.First(tema => tema.Valor == "orse.data_chamado");
                }
                catch (Exception)
                {
                }

                OpcaoPesquisaSelecionada = ListaOpcaoPesquisa.First();
                OperadorSelecionado = ListaOperadores.First();
                ClassificarPorSelecionado = ListaClassificarPor.Where(dat => dat.Valor == "DataChamado").First();
                OrdemSelecionada = ListaOrdem.Last();
            }
            catch (Exception)
            {
            }
        }

        private void AdicionarFiltro(Parametro parametro, Parametro operador, Parametro opcaoPesquisa)
        {
            switch (parametro.Tipo)
            {
                case "date":

                    if (DataDe != null && DataAte != null)
                    {
                        ListaFiltros.Add(new ParametroFiltroPesquisa
                        {
                            TituloColuna = parametro.Nome,
                            TextoFiltro = "De: " + Convert.ToDateTime(DataDe).ToString("d") + "\n" + "Até: " + Convert.ToDateTime(DataAte).ToString("d"),
                            Coluna = "DATE(" + parametro.Valor + ")",
                            Tipo = parametro.Tipo,
                            Operador1 = ">=",
                            Parametro1 = "@" + parametro.Valor.Split(".").Last() + "_de",
                            Valor1 = Convert.ToDateTime(DataDe),
                            Operador2 = "<=",
                            Parametro2 = "@" + parametro.Valor.Split(".").Last() + "_ate",
                            Valor2 = Convert.ToDateTime(DataAte),
                            TituloOperador = operador.Nome,
                            Operador = operador.Valor
                        });
                    }

                    if (DataDe == null && DataAte == null)
                    {
                        ListaFiltros.Add(new ParametroFiltroPesquisa
                        {
                            TituloColuna = parametro.Nome,
                            TextoFiltro = "Está vazio (a)",
                            Coluna = parametro.Valor,
                            Tipo = "null",
                            Operador1 = "IS NULL",
                            Parametro1 = "",
                            Valor1 = "",
                            Operador2 = "",
                            Parametro2 = "",
                            Valor2 = "",
                            TituloOperador = operador.Nome,
                            Operador = operador.Valor
                        });
                    }

                    if (DataDe != null && DataAte == null)
                    {
                        ListaFiltros.Add(new ParametroFiltroPesquisa
                        {
                            TituloColuna = parametro.Nome,
                            TextoFiltro = "Após: " + Convert.ToDateTime(DataDe).ToString("d"),
                            Coluna = "DATE(" + parametro.Valor + ")",
                            Tipo = parametro.Tipo,
                            Operador1 = ">=",
                            Parametro1 = "@" + parametro.Valor.Split(".").Last() + "_de",
                            Valor1 = Convert.ToDateTime(DataDe),
                            Operador2 = "",
                            Parametro2 = "",
                            Valor2 = "",
                            TituloOperador = operador.Nome,
                            Operador = operador.Valor
                        });
                    }

                    if (DataDe == null && DataAte != null)
                    {
                        ListaFiltros.Add(new ParametroFiltroPesquisa
                        {
                            TituloColuna = parametro.Nome,
                            TextoFiltro = "Antes de: " + Convert.ToDateTime(DataAte).ToString("d"),
                            Coluna = "DATE(" + parametro.Valor + ")",
                            Tipo = parametro.Tipo,
                            Operador1 = "<=",
                            Parametro1 = "@" + parametro.Valor.Split(".").Last() + "_ate",
                            Valor1 = Convert.ToDateTime(DataAte),
                            Operador2 = "",
                            Parametro2 = "",
                            Valor2 = "",
                            TituloOperador = operador.Nome,
                            Operador = operador.Valor
                        });
                    }

                    break;

                case "integer":

                    if (IntegerDe != null && IntegerAte != null)
                    {
                        ListaFiltros.Add(new ParametroFiltroPesquisa
                        {
                            TituloColuna = parametro.Nome,
                            TextoFiltro = "Maior ou igual a: " + Convert.ToInt32(IntegerDe).ToString(parametro.Formato) + "\n" + "Menor ou igual a: " + Convert.ToInt32(IntegerAte).ToString(parametro.Formato),
                            Coluna = parametro.Valor,
                            Tipo = parametro.Tipo,
                            Operador1 = ">=",
                            Parametro1 = "@" + parametro.Valor.Split(".").Last() + "_de",
                            Valor1 = Convert.ToInt32(IntegerDe),
                            Operador2 = "<=",
                            Parametro2 = "@" + parametro.Valor.Split(".").Last() + "_ate",
                            Valor2 = Convert.ToInt32(IntegerAte),
                            TituloOperador = operador.Nome,
                            Operador = operador.Valor
                        });
                    }

                    if (IntegerDe == null && IntegerAte == null)
                    {
                        ListaFiltros.Add(new ParametroFiltroPesquisa
                        {
                            TituloColuna = parametro.Nome,
                            TextoFiltro = "Está vazio (a)",
                            Coluna = parametro.Valor,
                            Tipo = "null",
                            Operador1 = "IS NULL",
                            Parametro1 = "",
                            Valor1 = "",
                            Operador2 = "",
                            Parametro2 = "",
                            Valor2 = "",
                            TituloOperador = operador.Nome,
                            Operador = operador.Valor
                        });
                    }

                    if (IntegerDe != null && IntegerAte == null)
                    {
                        ListaFiltros.Add(new ParametroFiltroPesquisa
                        {
                            TituloColuna = parametro.Nome,
                            TextoFiltro = "Maior ou igual a: " + Convert.ToInt32(IntegerDe).ToString(parametro.Formato),
                            Coluna = parametro.Valor,
                            Tipo = parametro.Tipo,
                            Operador1 = ">=",
                            Parametro1 = "@" + parametro.Valor.Split(".").Last() + "_de",
                            Valor1 = Convert.ToInt32(IntegerDe),
                            Operador2 = "",
                            Parametro2 = "",
                            Valor2 = "",
                            TituloOperador = operador.Nome,
                            Operador = operador.Valor
                        });
                    }

                    if (IntegerDe == null && IntegerAte != null)
                    {
                        ListaFiltros.Add(new ParametroFiltroPesquisa
                        {
                            TituloColuna = parametro.Nome,
                            TextoFiltro = "Menor ou igual a: " + Convert.ToInt32(IntegerAte).ToString(parametro.Formato),
                            Coluna = parametro.Valor,
                            Tipo = parametro.Tipo,
                            Operador1 = "<=",
                            Parametro1 = "@" + parametro.Valor.Split(".").Last() + "_ate",
                            Valor1 = Convert.ToInt32(IntegerAte),
                            Operador2 = "",
                            Parametro2 = "",
                            Valor2 = "",
                            TituloOperador = operador.Nome,
                            Operador = operador.Valor
                        });
                    }

                    break;

                case "decimal":

                    if (DecimalDe != null && DecimalAte != null)
                    {
                        ListaFiltros.Add(new ParametroFiltroPesquisa
                        {
                            TituloColuna = parametro.Nome,
                            TextoFiltro = "Maior ou igual a: " + Convert.ToDecimal(DecimalDe).ToString(parametro.Formato) + "\n" + "Menor ou igual a: " + Convert.ToDecimal(DecimalAte).ToString(parametro.Formato),
                            Coluna = parametro.Valor,
                            Tipo = parametro.Tipo,
                            Operador1 = ">=",
                            Parametro1 = "@" + parametro.Valor.Split(".").Last() + "_de",
                            Valor1 = Convert.ToDecimal(DecimalDe),
                            Operador2 = "<=",
                            Parametro2 = "@" + parametro.Valor.Split(".").Last() + "_ate",
                            Valor2 = Convert.ToDecimal(DecimalAte),
                            TituloOperador = operador.Nome,
                            Operador = operador.Valor
                        });
                    }

                    if (DecimalDe == null && DecimalAte == null)
                    {
                        ListaFiltros.Add(new ParametroFiltroPesquisa
                        {
                            TituloColuna = parametro.Nome,
                            TextoFiltro = "Está vazio (a)",
                            Coluna = parametro.Valor,
                            Tipo = "null",
                            Operador1 = "IS NULL",
                            Parametro1 = "",
                            Valor1 = "",
                            Operador2 = "",
                            Parametro2 = "",
                            Valor2 = "",
                            TituloOperador = operador.Nome,
                            Operador = operador.Valor
                        });
                    }

                    if (DecimalDe != null && DecimalAte == null)
                    {
                        ListaFiltros.Add(new ParametroFiltroPesquisa
                        {
                            TituloColuna = parametro.Nome,
                            TextoFiltro = "Maior ou igual a: " + Convert.ToDecimal(DecimalDe).ToString(parametro.Formato),
                            Coluna = parametro.Valor,
                            Tipo = parametro.Tipo,
                            Operador1 = ">=",
                            Parametro1 = "@" + parametro.Valor.Split(".").Last() + "_de",
                            Valor1 = Convert.ToDecimal(DecimalDe),
                            Operador2 = "",
                            Parametro2 = "",
                            Valor2 = "",
                            TituloOperador = operador.Nome,
                            Operador = operador.Valor
                        });
                    }

                    if (DecimalDe == null && DecimalAte != null)
                    {
                        ListaFiltros.Add(new ParametroFiltroPesquisa
                        {
                            TituloColuna = parametro.Nome,
                            TextoFiltro = "Menor ou igual a: " + Convert.ToDecimal(DecimalAte).ToString(parametro.Formato),
                            Coluna = parametro.Valor,
                            Tipo = parametro.Tipo,
                            Operador1 = "<=",
                            Parametro1 = "@" + parametro.Valor.Split(".").Last() + "_ate",
                            Valor1 = Convert.ToDecimal(DecimalAte),
                            Operador2 = "",
                            Parametro2 = "",
                            Valor2 = "",
                            TituloOperador = operador.Nome,
                            Operador = operador.Valor
                        });
                    }

                    break;

                case "string":

                    ParametroFiltroPesquisa filtro = new();

                    if (String.IsNullOrEmpty(TextoPesquisa))
                    {
                        switch (opcaoPesquisa.Valor)
                        {
                            case "igual_a":
                                filtro.TextoFiltro = "Está vazio (a)";
                                filtro.Operador1 = "IS NULL";
                                break;

                            case "diferente_de":
                                filtro.TextoFiltro = "Não está vazio (a)";
                                filtro.Operador1 = "IS NOT NULL";
                                break;

                            case "comeca_com":
                                filtro.TextoFiltro = "Está vazio (a)";
                                filtro.Operador1 = "IS NULL";
                                break;

                            case "nao_comeca_com":
                                filtro.TextoFiltro = "Não está vazio (a)";
                                filtro.Operador1 = "IS NOT NULL";
                                break;

                            case "termina_com":
                                filtro.TextoFiltro = "Está vazio (a)";
                                filtro.Operador1 = "IS NULL";
                                break;

                            case "nao_termina_com":
                                filtro.TextoFiltro = "Não está vazio (a)";
                                filtro.Operador1 = "IS NOT NULL";
                                break;

                            case "contem":
                                filtro.TextoFiltro = "Está vazio (a)";
                                filtro.Operador1 = "IS NULL";
                                break;

                            case "nao_contem":
                                filtro.TextoFiltro = "Não está vazio (a)";
                                filtro.Operador1 = "IS NOT NULL";
                                break;

                            default:
                                break;
                        }

                        filtro.TituloColuna = parametro.Nome;
                        filtro.Coluna = parametro.Valor;
                        filtro.Tipo = "null";
                        filtro.Parametro1 = "";
                        filtro.Valor1 = "";
                        filtro.Operador2 = "";
                        filtro.Parametro2 = "";
                        filtro.Valor2 = "";
                        filtro.TituloOperador = operador.Nome;
                        filtro.Operador = operador.Valor;
                    }
                    else
                    {
                        filtro.TituloColuna = parametro.Nome;
                        filtro.TextoFiltro = opcaoPesquisa.Nome + ": " + TextoPesquisa;
                        filtro.Coluna = parametro.Valor;
                        filtro.Tipo = parametro.Tipo;
                        filtro.Parametro1 = "@" + parametro.Valor.Split(".").Last();
                        filtro.Operador2 = "";
                        filtro.Parametro2 = "";
                        filtro.Valor2 = "";
                        filtro.TituloOperador = operador.Nome;
                        filtro.Operador = operador.Valor;

                        switch (opcaoPesquisa.Valor)
                        {
                            case "igual_a":
                                filtro.Operador1 = "=";
                                filtro.Valor1 = TextoPesquisa;
                                break;

                            case "diferente_de":
                                filtro.Coluna = "NOT " + parametro.Valor;
                                filtro.Operador1 = "=";
                                filtro.Valor1 = TextoPesquisa;
                                break;

                            case "comeca_com":
                                filtro.Operador1 = "LIKE";
                                filtro.Valor1 = TextoPesquisa + "%";
                                break;

                            case "nao_comeca_com":
                                filtro.Operador1 = "NOT LIKE";
                                filtro.Valor1 = TextoPesquisa;
                                break;

                            case "termina_com":
                                filtro.Operador1 = "LIKE";
                                filtro.Valor1 = "%" + TextoPesquisa;
                                break;

                            case "nao_termina_com":
                                filtro.Operador1 = "NOT LIKE";
                                filtro.Valor1 = "%" + TextoPesquisa;
                                break;

                            case "contem":
                                filtro.Operador1 = "LIKE";
                                filtro.Valor1 = "%" + TextoPesquisa + "%";
                                break;

                            case "nao_contem":
                                filtro.Operador1 = "NOT LIKE";
                                filtro.Valor1 = "%" + TextoPesquisa + "%";
                                break;

                            default:
                                break;
                        }
                    }
                    ListaFiltros.Add(filtro);
                    break;

                case "boolean":

                    ListaFiltros.Add(new ParametroFiltroPesquisa
                    {
                        TituloColuna = parametro.Nome,
                        TextoFiltro = opcaoPesquisa.Nome + ": " + (BooleanValue == true ? "Sim" : "Não"),
                        Coluna = parametro.Valor,
                        Tipo = parametro.Tipo,
                        Operador1 = "=",
                        Parametro1 = "@" + parametro.Valor.Split(".").Last(),
                        Valor1 = BooleanValue,
                        Operador2 = "",
                        Parametro2 = "",
                        Valor2 = "",
                        TituloOperador = operador.Nome,
                        Operador = operador.Valor
                    });

                    break;

                default:
                    break;
            }

            LimparUltimoOperador();
        }

        private void RemoverFiltro(ParametroFiltroPesquisa filtro)
        {
            ListaFiltros.Remove(filtro);

            LimparUltimoOperador();
        }

        private void MoverFiltro(ParametroFiltroPesquisa filtro, OrdemFiltro ordemFiltro)
        {
            int indexFiltro = ListaFiltros.IndexOf(filtro);

            if (ordemFiltro == OrdemFiltro.Descer)
            {
                if (indexFiltro == ListaFiltros.IndexOf(ListaFiltros.First()))
                {
                    return;
                }
            }
            else
            {
                if (indexFiltro == ListaFiltros.IndexOf(ListaFiltros.Last()))
                {
                    return;
                }
            }

            ListaFiltros.Move(indexFiltro, ordemFiltro == OrdemFiltro.Subir ? indexFiltro + 1 : indexFiltro - 1);

            LimparUltimoOperador();
        }

        private void LimparUltimoOperador()
        {
            try
            {
                foreach (var item in ListaFiltros)
                {
                    item.TituloOperador = item.Operador == "AND" ? "E" : "Ou";
                }

                ListaFiltros.Last().TituloOperador = "";
            }
            catch (Exception)
            {
            }
        }

        private void VerificaDataDe()
        {
            if (DataDe != null && DataAte != null)
            {
                if (DataDe > DataAte)
                {
                    DataDe = null;
                }
            }
        }

        private void VerificaDataAte()
        {
            if (DataDe != null && DataAte != null)
            {
                if (DataAte < DataDe)
                {
                    DataAte = null;
                }
            }
        }

        private async Task ExecutarPesquisa(CancellationToken ct)
        {
            if (PesquisaBasica)
            {
                await ExecutarPesquisaBasica(ct);
            }
            else
            {
                await ExecutarPesquisaAvancada(ct);
            }
        }

        private async Task ExecutarPesquisaAvancada(CancellationToken ct)
        {
            string sCondicao = "";
            string textoOperador = "";
            int contagemParametro = 1;
            string textoContagemParametro = "";

            foreach (var filtro in ListaFiltros)
            {
                textoOperador = " " + filtro.Operador + " ";
                textoContagemParametro = filtro.Tipo == "null" ? "" : contagemParametro.ToString();
                if (String.IsNullOrEmpty(sCondicao))
                {
                    sCondicao = filtro.Coluna + " " + filtro.Operador1 + " " + filtro.Parametro1 + textoContagemParametro;
                    if (!String.IsNullOrEmpty(filtro.Operador2))
                    {
                        sCondicao = sCondicao + " AND " + filtro.Coluna + " " + filtro.Operador2 + " " + filtro.Parametro2 + textoContagemParametro;
                    }
                }
                else
                {
                    sCondicao = sCondicao + textoOperador + filtro.Coluna + " " + filtro.Operador1 + " " + filtro.Parametro1 + textoContagemParametro;
                    if (!String.IsNullOrEmpty(filtro.Operador2))
                    {
                        sCondicao = sCondicao + " AND " + filtro.Coluna + " " + filtro.Operador2 + " " + filtro.Parametro2 + textoContagemParametro;
                    }
                }

                contagemParametro++;
            }

            if (!ConsideraApenasAtivos)
            {
                if (!String.IsNullOrEmpty(sCondicao))
                {
                    sCondicao = " WHERE " + sCondicao;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(sCondicao))
                {
                    sCondicao = " WHERE orse.id_status = 1 AND " + sCondicao;
                }
                else
                {
                    sCondicao = " WHERE orse.id_status = 1 ";
                }
            }

            try
            {
                string textoLimite = "";

                if (App.Usuario.LimiteResultados != null)
                {
                    textoLimite = " LIMIT " + App.Usuario.LimiteResultados.ToString();
                }

                string comandoInicial = "SELECT " +
                                        "orse.data_insercao AS DataInsercao, " +
                                        "orse_usua_ins.nome AS NomeUsuario, " +
                                        "orse.id_ordem_servico AS IdOrdemServico, " +
                                        "orse.ordem_servico_atual AS NumeroOrdemServicoAtual, " +
                                        "orse.ordem_servico_primaria AS NumeroOrdemServicoPrimaria, " +
                                        "orse.numero_chamado AS NumeroChamado, " +
                                        "orse.data_chamado AS DataChamado, " +
                                        "orse.data_atendimento AS DataAtendimento, " +
                                        "orse.mastro AS Mastro, " +
                                        "orse.codigo_falha AS CodigoFalha, " +
                                        "orse.etapas_concluidas AS EtapasConcluidas, " +
                                        "orse.horimetro AS Horimetro, " +
                                        "orse.data_saida AS DataSaida, " +
                                        "orse.data_chegada AS DataChegada, " +
                                        "orse.data_retorno AS DataRetorno, " +

                                        "tios.nome AS NomeTipoOS, " +
                                        "clie.nome AS NomeCliente, " +
                                        "exse.nome AS NomeExecutanteServico, " +
                                        "seri.nome AS NomeSerie, " +
                                        "fabr.nome AS NomeFabricante, " +
                                        "tieq.nome AS NomeTipoEquipamento, " +
                                        "mode.nome AS NomeModelo, " +
                                        "tima.nome AS NomeTipoManutencao, " +
                                        "staq.nome AS NomeEquipamentoAposManutencao, " +
                                        "orse.equipamento_operacional AS EquipamentoOperacional, " +
                                        "(SELECT COUNT(itos.id_item_ordem_servico) FROM tb_itens_ordem_servico AS itos WHERE itos.id_ordem_servico = orse.id_ordem_servico) AS QuantidadeItens, " +
                                        "(SELECT COUNT(evos.id_evento_ordem_servico) FROM tb_eventos_ordem_servico AS evos WHERE evos.id_ordem_servico = orse.id_ordem_servico) AS QuantidadeEventos " +

                                        "FROM tb_ordens_servico AS orse " +

                                        "LEFT JOIN tb_tipos_ordem_servico AS tios ON orse.id_tipo_ordem_servico = tios.id_tipo_ordem_servico " +
                                        "LEFT JOIN tb_status_equipamento_apos_manutencao AS staq ON orse.id_status_equipamento_apos_manutencao = staq.id_status_equipamento_apos_manutencao " +
                                        "LEFT JOIN tb_usos_indevidos AS usind ON orse.id_uso_indevido = usind.id_uso_indevido " +
                                        "LEFT JOIN tb_tipos_manutencao AS tima ON orse.id_tipo_manutencao = tima.id_tipo_manutencao " +
                                        "LEFT JOIN tb_executantes_servico AS exse ON orse.id_executante_servico = exse.id_executante_servico " +

                                        "LEFT JOIN tb_usuarios AS orse_usua_ins ON orse.id_usuario_insercao = orse_usua_ins.id_usuario " +
                                        "LEFT JOIN tb_usuarios AS orse_usua_edi ON orse.id_usuario_edicao = orse_usua_edi.id_usuario " +
                                        "LEFT JOIN tb_setores AS seto_ins ON orse_usua_ins.id_setor = seto_ins.id_setor " +
                                        "LEFT JOIN tb_setores AS seto_edi ON orse_usua_edi.id_setor = seto_edi.id_setor " +
                                        "LEFT JOIN tb_status AS orse_stat ON orse.id_status = orse_stat.id_status " +

                                        "LEFT JOIN tb_filiais AS fili ON orse.id_filial = fili.id_filial " +
                                        "LEFT JOIN tb_clientes AS clie ON orse.id_cliente = clie.id_cliente " +
                                        "LEFT JOIN tb_frotas AS frot ON orse.id_frota = frot.id_frota " +
                                        "LEFT JOIN tb_areas AS area ON frot.id_area = area.id_area " +
                                        "LEFT JOIN tb_plantas AS plan ON area.id_planta = plan.id_planta " +

                                        "LEFT JOIN tb_series AS seri ON orse.id_serie = seri.id_serie " +
                                        "LEFT JOIN tb_fabricantes AS fabr ON seri.id_fabricante = fabr.id_fabricante " +
                                        "LEFT JOIN tb_tipos_equipamento AS tieq ON seri.id_tipo_equipamento = tieq.id_tipo_equipamento " +
                                        "LEFT JOIN tb_modelos AS mode ON seri.id_modelo = mode.id_modelo " +
                                        "LEFT JOIN tb_categorias AS cate ON mode.id_categoria = cate.id_categoria " +
                                        "LEFT JOIN tb_classes AS clas ON mode.id_classe = clas.id_classe " +
                                        "LEFT JOIN tb_anos AS ano ON seri.id_ano = ano.id_ano " +

                                        sCondicao + " ORDER BY " +
                                        ClassificarPorSelecionado.Valor + " " + OrdemSelecionada.Valor + textoLimite;

                string parametros = "";
                List<object?> valoresParametros = new();

                if (!String.IsNullOrEmpty(sCondicao))
                {
                    contagemParametro = 1;
                    foreach (var filtro in ListaFiltros)
                    {
                        if (filtro.Tipo != "null")
                        {
                            textoContagemParametro = filtro.Tipo == "null" ? "" : contagemParametro.ToString();
                            if (String.IsNullOrEmpty(parametros))
                            {
                                parametros = filtro.Parametro1.Replace("%", "").Replace("+", "").Replace(" ", "").Replace("'", "") + textoContagemParametro;
                            }
                            else
                            {
                                parametros = parametros + ", " + filtro.Parametro1.Replace("%", "").Replace("+", "").Replace(" ", "").Replace("'", "") + textoContagemParametro;
                            }
                            valoresParametros.Add(filtro.Valor1);

                            if (!String.IsNullOrEmpty(filtro.Parametro2))
                            {
                                if (String.IsNullOrEmpty(parametros))
                                {
                                    parametros = filtro.Parametro2.Replace("%", "").Replace("+", "").Replace(" ", "").Replace("'", "") + textoContagemParametro;
                                }
                                else
                                {
                                    parametros = parametros + ", " + filtro.Parametro2.Replace("%", "").Replace("+", "").Replace(" ", "").Replace("'", "") + textoContagemParametro;
                                }
                                valoresParametros.Add(filtro.Valor2);
                            }

                            contagemParametro++;
                        }
                    }
                }

                //System.Diagnostics.Trace.WriteLine(comandoInicial);

                //await ResultadoPesquisaProposta.PreencheListaResultadosPesquisaPropostaAsync(ListaResultadosPesquisaProposta, true, null, ct, comandoInicial, parametros, valoresParametros.ToArray());

                //TextoResultadosEncontrados = "Resultado (s) encontrado (s): " + ListaResultadosPesquisaProposta.Count;

                _cts = new();

                Progress<double> progresso = new(dbl =>
                {
                    Messenger.Default.Send<double>(dbl, "ValorProgresso2");
                });

                var customDialog = new CustomDialog();

                var dataContext = new CustomProgressViewModel("Executando pesquisa", "Aguarde...", false, true, _cts, instance =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                });

                customDialog.Content = new CustomProgressView { DataContext = dataContext };

                await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);

                try
                {
                    await ResultadoPesquisaOrdemServico.PreencheListaResultadosPesquisaOrdemServicoAsync(ListaResultadosPesquisaOrdemServico, true, progresso, _cts.Token, comandoInicial, parametros, valoresParametros.ToArray());

                    if (App.Usuario.LimiteResultados == null)
                    {
                        TextoLimiteDeResultados = "*Sem limite de resultados. Caso a pesquisa esteja lenta, defina um limite na opção Perfil.";
                    }
                    else
                    {
                        TextoLimiteDeResultados = "*Limitado a " + App.Usuario.LimiteResultados.ToString() + " resultado (s). Você pode alterar isso na opção Perfil.";
                    }

                    TextoResultadosEncontrados = "Resultado (s) encontrado (s): " + ListaResultadosPesquisaOrdemServico.Count;
                }
                catch (Exception)
                {
                    await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                }

                try
                {
                    await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                }
                catch (Exception)
                {
                }
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    MensagemStatus = "Pesquisa cancelada";
                }
                else
                {
                    Serilog.Log.Error(ex, "Erro ao pesquisar dados");
                }
            }
        }

        private async Task ExecutarPesquisaBasica(CancellationToken ct)
        {
            string sCondicao = "";

            List<string> condicao = new();
            List<string> listaNomeParametros = new();
            List<object> listaValores = new();

            if (!String.IsNullOrEmpty(TextoPesquisado))
            {
                condicao.Add("(orse.ordem_servico_atual LIKE @texto_pesquisa OR " +
                "orse.ordem_servico_primaria LIKE @texto_pesquisa OR " +
                "orse.numero_chamado LIKE @texto_pesquisa OR " +
                "clie.nome LIKE @texto_pesquisa OR " +
                "exse.nome LIKE @texto_pesquisa OR " +
                "fili.nome LIKE @texto_pesquisa OR " +
                "fabr.nome LIKE @texto_pesquisa OR " +
                "tieq.nome LIKE @texto_pesquisa OR " +
                "mode.nome LIKE @texto_pesquisa OR " +
                "seri.nome LIKE @texto_pesquisa)");
                listaNomeParametros.Add("@texto_pesquisa");
                listaValores.Add("%" + TextoPesquisado + "%");
            }

            string textoParametrosSetores = "";
            foreach (var item in ListaObjetoSelecionavelSetores.Where(d => d.Selecionado))
            {
                listaNomeParametros.Add("@id_setor" + ListaObjetoSelecionavelSetores.IndexOf(item));
                listaValores.Add(item.Id);

                if (String.IsNullOrEmpty(textoParametrosSetores))
                {
                    textoParametrosSetores = "@id_setor" + ListaObjetoSelecionavelSetores.IndexOf(item);
                }
                else
                {
                    textoParametrosSetores = textoParametrosSetores + ", @id_setor" + ListaObjetoSelecionavelSetores.IndexOf(item);
                }
            }

            if (!String.IsNullOrEmpty(textoParametrosSetores))
            {
                condicao.Add("orse_usua_ins.id_setor IN (" + textoParametrosSetores + ")");
            }

            string textoParametrosUsuarios = "";
            foreach (var item in ListaObjetoSelecionavelUsuariosInsercao.Where(d => d.Selecionado))
            {
                listaNomeParametros.Add("@id_usuario" + ListaObjetoSelecionavelUsuariosInsercao.IndexOf(item));
                listaValores.Add(item.Id);

                if (String.IsNullOrEmpty(textoParametrosUsuarios))
                {
                    textoParametrosUsuarios = "@id_usuario" + ListaObjetoSelecionavelUsuariosInsercao.IndexOf(item);
                }
                else
                {
                    textoParametrosUsuarios = textoParametrosUsuarios + ", @id_usuario" + ListaObjetoSelecionavelUsuariosInsercao.IndexOf(item);
                }
            }

            if (!String.IsNullOrEmpty(textoParametrosUsuarios))
            {
                condicao.Add("orse.id_usuario_insercao IN (" + textoParametrosUsuarios + ")");
            }

            string textoParametrosClientes = "";
            foreach (var item in ListaObjetoSelecionavelClientes.Where(d => d.Selecionado))
            {
                listaNomeParametros.Add("@id_cliente" + ListaObjetoSelecionavelClientes.IndexOf(item));
                listaValores.Add(item.Id);

                if (String.IsNullOrEmpty(textoParametrosClientes))
                {
                    textoParametrosClientes = "@id_cliente" + ListaObjetoSelecionavelClientes.IndexOf(item);
                }
                else
                {
                    textoParametrosClientes = textoParametrosClientes + ", @id_cliente" + ListaObjetoSelecionavelClientes.IndexOf(item);
                }
            }

            if (!String.IsNullOrEmpty(textoParametrosClientes))
            {
                condicao.Add("orse.id_cliente IN (" + textoParametrosClientes + ")");
            }

            string textoParametrosTipoOrdemServico = "";
            foreach (var item in ListaObjetoSelecionavelTipoOrdemServico.Where(d => d.Selecionado))
            {
                listaNomeParametros.Add("@id_tipo_ordem_servico " + ListaObjetoSelecionavelTipoOrdemServico.IndexOf(item));
                listaValores.Add(item.Id);

                if (String.IsNullOrEmpty(textoParametrosTipoOrdemServico))
                {
                    textoParametrosTipoOrdemServico = "@id_tipo_ordem_servico " + ListaObjetoSelecionavelTipoOrdemServico.IndexOf(item);
                }
                else
                {
                    textoParametrosTipoOrdemServico = textoParametrosTipoOrdemServico + ", @id_tipo_ordem_servico " + ListaObjetoSelecionavelTipoOrdemServico.IndexOf(item);
                }
            }

            if (!String.IsNullOrEmpty(textoParametrosTipoOrdemServico))
            {
                condicao.Add("orse.id_tipo_ordem_servico  IN (" + textoParametrosTipoOrdemServico + ")");
            }

            string textoParametrosEquipamentoAposManutencao = "";
            foreach (var item in ListaObjetoSelecionavelEquipamentoAposManutencao.Where(d => d.Selecionado))
            {
                listaNomeParametros.Add("@id_status_equipamento_apos_manutencao" + ListaObjetoSelecionavelEquipamentoAposManutencao.IndexOf(item));
                listaValores.Add(item.Id);

                if (String.IsNullOrEmpty(textoParametrosEquipamentoAposManutencao))
                {
                    textoParametrosEquipamentoAposManutencao = "@id_status_equipamento_apos_manutencao" + ListaObjetoSelecionavelEquipamentoAposManutencao.IndexOf(item);
                }
                else
                {
                    textoParametrosEquipamentoAposManutencao = textoParametrosEquipamentoAposManutencao + ", @id_status_equipamento_apos_manutencao" + ListaObjetoSelecionavelEquipamentoAposManutencao.IndexOf(item);
                }
            }

            if (!String.IsNullOrEmpty(textoParametrosEquipamentoAposManutencao))
            {
                condicao.Add("orse.id_status_equipamento_apos_manutencao IN (" + textoParametrosEquipamentoAposManutencao + ")");
            }

            string textoParametrosTipoManutencao = "";
            foreach (var item in ListaObjetoSelecionavelTipoManutencao.Where(d => d.Selecionado))
            {
                listaNomeParametros.Add("@id_tipo_manutencao" + ListaObjetoSelecionavelTipoManutencao.IndexOf(item));
                listaValores.Add(item.Id);

                if (String.IsNullOrEmpty(textoParametrosTipoManutencao))
                {
                    textoParametrosTipoManutencao = "@id_tipo_manutencao" + ListaObjetoSelecionavelTipoManutencao.IndexOf(item);
                }
                else
                {
                    textoParametrosTipoManutencao = textoParametrosTipoManutencao + ", @id_tipo_manutencao" + ListaObjetoSelecionavelTipoManutencao.IndexOf(item);
                }
            }

            if (!String.IsNullOrEmpty(textoParametrosTipoManutencao))
            {
                condicao.Add("orse.id_tipo_manutencao IN (" + textoParametrosTipoManutencao + ")");
            }

            string textoParametrosExecutanteServico = "";
            foreach (var item in ListaObjetoSelecionavelExecutanteServico.Where(d => d.Selecionado))
            {
                listaNomeParametros.Add("@id_executante_servico" + ListaObjetoSelecionavelExecutanteServico.IndexOf(item));
                listaValores.Add(item.Id);

                if (String.IsNullOrEmpty(textoParametrosExecutanteServico))
                {
                    textoParametrosExecutanteServico = "@id_executante_servico" + ListaObjetoSelecionavelExecutanteServico.IndexOf(item);
                }
                else
                {
                    textoParametrosExecutanteServico = textoParametrosExecutanteServico + ", @id_executante_servico" + ListaObjetoSelecionavelExecutanteServico.IndexOf(item);
                }
            }

            if (!String.IsNullOrEmpty(textoParametrosExecutanteServico))
            {
                condicao.Add("orse.id_executante_servico IN (" + textoParametrosExecutanteServico + ")");
            }

            string textoParametrosPassosExecutados = "";
            foreach (var item in ListaObjetoSelecionavelPassosExecutados.Where(d => d.Selecionado))
            {
                listaNomeParametros.Add("@etapas_concluidas" + ListaObjetoSelecionavelPassosExecutados.IndexOf(item));
                listaValores.Add(item.Id);

                if (String.IsNullOrEmpty(textoParametrosPassosExecutados))
                {
                    textoParametrosPassosExecutados = "@etapas_concluidas" + ListaObjetoSelecionavelPassosExecutados.IndexOf(item);
                }
                else
                {
                    textoParametrosPassosExecutados = textoParametrosPassosExecutados + ", @etapas_concluidas" + ListaObjetoSelecionavelPassosExecutados.IndexOf(item);
                }
            }

            if (!String.IsNullOrEmpty(textoParametrosPassosExecutados))
            {
                condicao.Add("orse.etapas_concluidas IN (" + textoParametrosPassosExecutados + ")");
            }

            if (DataDeChamado != null && DataAteChamado != null)
            {
                listaNomeParametros.Add("@data_chamado_de");
                listaValores.Add(DataDeChamado);
                listaNomeParametros.Add("@data_chamado_ate");
                listaValores.Add(DataAteChamado);
                condicao.Add("orse.data_chamado >= @data_chamado_de AND orse.data_chamado <= @data_chamado_ate");
            }

            if (DataDeChamado != null && DataAteChamado == null)
            {
                listaNomeParametros.Add("@data_chamado_de");
                listaValores.Add(DataDeChamado);
                condicao.Add("orse.data_chamado >= @data_chamado_de");
            }

            if (DataDeChamado == null && DataAteChamado != null)
            {
                listaNomeParametros.Add("@data_chamado_ate");
                listaValores.Add(DataAteChamado);
                condicao.Add("orse.data_chamado <= @data_chamado_ate");
            }

            if (DataDeAtendimento != null && DataAteAtendimento != null)
            {
                listaNomeParametros.Add("@data_atendimento_de");
                listaValores.Add(DataDeAtendimento);
                listaNomeParametros.Add("@data_atendimento_ate");
                listaValores.Add(DataAteAtendimento);
                condicao.Add("orse.data_atendimento >= @data_atendimento_de AND orse.data_atendimento <= @data_atendimento_ate");
            }

            if (DataDeAtendimento != null && DataAteAtendimento == null)
            {
                listaNomeParametros.Add("@data_atendimento_de");
                listaValores.Add(DataDeAtendimento);
                condicao.Add("orse.data_atendimento >= @data_atendimento_de");
            }

            if (DataDeAtendimento == null && DataAteAtendimento != null)
            {
                listaNomeParametros.Add("@data_atendimento_ate");
                listaValores.Add(DataAteAtendimento);
                condicao.Add("orse.data_atendimento <= @data_atendimento_ate");
            }

            if (DataDeInsercao != null && DataAteInsercao != null)
            {
                listaNomeParametros.Add("@data_insercao_de");
                listaValores.Add(DataDeInsercao);
                listaNomeParametros.Add("@data_insercao_ate");
                listaValores.Add(DataAteInsercao);
                condicao.Add("orse.data_insercao >= @data_insercao_de AND orse.data_insercao <= @data_insercao_ate");
            }

            if (DataDeInsercao != null && DataAteInsercao == null)
            {
                listaNomeParametros.Add("@data_insercao_de");
                listaValores.Add(DataDeInsercao);
                condicao.Add("orse.data_insercao >= @data_insercao_de");
            }

            if (DataDeInsercao == null && DataAteInsercao != null)
            {
                listaNomeParametros.Add("@data_insercao_ate");
                listaValores.Add(DataAteInsercao);
                condicao.Add("orse.data_insercao <= @data_insercao_ate");
            }

            if (!ConsideraApenasAtivos)
            {
                sCondicao = condicao.Count > 0 ? "WHERE " + String.Join(" AND ", condicao.ToArray()) : "";
            }
            else
            {
                if (condicao.Count > 0)
                {
                    sCondicao = " WHERE orse.id_status = 1 AND " + String.Join(" AND ", condicao.ToArray());
                }
                else
                {
                    sCondicao = " WHERE orse.id_status = 1 ";
                }
            }

            try
            {
                string textoLimite = "";

                if (App.Usuario.LimiteResultados != null)
                {
                    textoLimite = " LIMIT " + App.Usuario.LimiteResultados.ToString();
                }

                string comandoInicial = "SELECT " +
                                        "orse.data_insercao AS DataInsercao, " +
                                        "orse_usua_ins.nome AS NomeUsuario, " +
                                        "orse.id_ordem_servico AS IdOrdemServico, " +
                                        "orse.ordem_servico_atual AS NumeroOrdemServicoAtual, " +
                                        "orse.ordem_servico_primaria AS NumeroOrdemServicoPrimaria, " +
                                        "orse.numero_chamado AS NumeroChamado, " +
                                        "orse.data_chamado AS DataChamado, " +
                                        "orse.data_atendimento AS DataAtendimento, " +
                                        "orse.mastro AS Mastro, " +
                                        "orse.codigo_falha AS CodigoFalha, " +
                                        "orse.etapas_concluidas AS EtapasConcluidas, " +
                                        "orse.horimetro AS Horimetro, " +
                                        "orse.data_saida AS DataSaida, " +
                                        "orse.data_chegada AS DataChegada, " +
                                        "orse.data_retorno AS DataRetorno, " +

                                        "tios.nome AS NomeTipoOS, " +
                                        "clie.nome AS NomeCliente, " +
                                        "exse.nome AS NomeExecutanteServico, " +
                                        "seri.nome AS NomeSerie, " +
                                        "fabr.nome AS NomeFabricante, " +
                                        "tieq.nome AS NomeTipoEquipamento, " +
                                        "mode.nome AS NomeModelo, " +
                                        "tima.nome AS NomeTipoManutencao, " +
                                        "staq.nome AS NomeEquipamentoAposManutencao, " +
                                        "orse.equipamento_operacional AS EquipamentoOperacional, " +
                                        "(SELECT COUNT(itos.id_item_ordem_servico) FROM tb_itens_ordem_servico AS itos WHERE itos.id_ordem_servico = orse.id_ordem_servico) AS QuantidadeItens, " +
                                        "(SELECT COUNT(evos.id_evento_ordem_servico) FROM tb_eventos_ordem_servico AS evos WHERE evos.id_ordem_servico = orse.id_ordem_servico) AS QuantidadeEventos " +

                                        "FROM tb_ordens_servico AS orse " +

                                        "LEFT JOIN tb_tipos_ordem_servico AS tios ON orse.id_tipo_ordem_servico = tios.id_tipo_ordem_servico " +
                                        "LEFT JOIN tb_status_equipamento_apos_manutencao AS staq ON orse.id_status_equipamento_apos_manutencao = staq.id_status_equipamento_apos_manutencao " +
                                        "LEFT JOIN tb_usos_indevidos AS usind ON orse.id_uso_indevido = usind.id_uso_indevido " +
                                        "LEFT JOIN tb_tipos_manutencao AS tima ON orse.id_tipo_manutencao = tima.id_tipo_manutencao " +
                                        "LEFT JOIN tb_executantes_servico AS exse ON orse.id_executante_servico = exse.id_executante_servico " +

                                        "LEFT JOIN tb_usuarios AS orse_usua_ins ON orse.id_usuario_insercao = orse_usua_ins.id_usuario " +
                                        "LEFT JOIN tb_usuarios AS orse_usua_edi ON orse.id_usuario_edicao = orse_usua_edi.id_usuario " +
                                        "LEFT JOIN tb_setores AS seto_ins ON orse_usua_ins.id_setor = seto_ins.id_setor " +
                                        "LEFT JOIN tb_setores AS seto_edi ON orse_usua_edi.id_setor = seto_edi.id_setor " +
                                        "LEFT JOIN tb_status AS orse_stat ON orse.id_status = orse_stat.id_status " +

                                        "LEFT JOIN tb_filiais AS fili ON orse.id_filial = fili.id_filial " +
                                        "LEFT JOIN tb_clientes AS clie ON orse.id_cliente = clie.id_cliente " +
                                        "LEFT JOIN tb_frotas AS frot ON orse.id_frota = frot.id_frota " +
                                        "LEFT JOIN tb_areas AS area ON frot.id_area = area.id_area " +
                                        "LEFT JOIN tb_plantas AS plan ON area.id_planta = plan.id_planta " +

                                        "LEFT JOIN tb_series AS seri ON orse.id_serie = seri.id_serie " +
                                        "LEFT JOIN tb_fabricantes AS fabr ON seri.id_fabricante = fabr.id_fabricante " +
                                        "LEFT JOIN tb_tipos_equipamento AS tieq ON seri.id_tipo_equipamento = tieq.id_tipo_equipamento " +
                                        "LEFT JOIN tb_modelos AS mode ON seri.id_modelo = mode.id_modelo " +
                                        "LEFT JOIN tb_categorias AS cate ON mode.id_categoria = cate.id_categoria " +
                                        "LEFT JOIN tb_classes AS clas ON mode.id_classe = clas.id_classe " +
                                        "LEFT JOIN tb_anos AS ano ON seri.id_ano = ano.id_ano " +

                                        sCondicao + " ORDER BY " +
                                        ClassificarPorSelecionado.Valor + " " + OrdemSelecionada.Valor + textoLimite;

                _cts = new();

                Progress<double> progresso = new(dbl =>
                {
                    Messenger.Default.Send<double>(dbl, "ValorProgresso2");
                });

                var customDialog = new CustomDialog();

                var dataContext = new CustomProgressViewModel("Executando pesquisa", "Aguarde...", false, true, _cts, instance =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                });

                customDialog.Content = new CustomProgressView { DataContext = dataContext };

                await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);

                try
                {
                    await ResultadoPesquisaOrdemServico.PreencheListaResultadosPesquisaOrdemServicoAsync(ListaResultadosPesquisaOrdemServico, true, progresso, _cts.Token, comandoInicial, String.Join(", ", listaNomeParametros.ToArray()), listaValores.ToArray());

                    if (App.Usuario.LimiteResultados == null)
                    {
                        TextoLimiteDeResultados = "*Sem limite de resultados. Caso a pesquisa esteja lenta, defina um limite na opção Perfil.";
                    }
                    else
                    {
                        TextoLimiteDeResultados = "*Limitado a " + App.Usuario.LimiteResultados.ToString() + " resultado (s). Você pode alterar isso na opção Perfil.";
                    }

                    TextoResultadosEncontrados = "Resultado (s) encontrado (s): " + ListaResultadosPesquisaOrdemServico.Count;
                }
                catch (Exception)
                {
                    await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                }

                try
                {
                    await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                }
                catch (Exception)
                {
                }
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    MensagemStatus = "Pesquisa cancelada";
                }
                else
                {
                    Serilog.Log.Error(ex, "Erro ao pesquisar dados");
                }
            }
        }

        private async Task AbrirOrdemServico()
        {
            if (ResultadoPesquisaOrdemServicoSelecionado != null)
            {
                try
                {
                    if (!await OrdemServico.OrdemServicoExiste(CancellationToken.None, ResultadoPesquisaOrdemServicoSelecionado.IdOrdemServico))
                    {
                        var mySettings = new MetroDialogSettings
                        {
                            AffirmativeButtonText = "Ok"
                        };

                        var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                                "Ordem de serviço inexistente", "A ordem de serviço selecionada foi excluída da database. Não será possível abri-la", MessageDialogStyle.Affirmative, mySettings);

                        return;
                    }
                }
                catch (Exception)
                {
                    var mySettings = new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Ok"
                    };

                    var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                            "Falha na verificação", "Não foi possível verificar a existência da ordem de serviço. Não será possível abri-la", MessageDialogStyle.Affirmative, mySettings);

                    return;
                }

                OrdemServico ordemServico = new();

                ordemServico.Id = ResultadoPesquisaOrdemServicoSelecionado.IdOrdemServico;

                Messenger.Default.Send<OrdemServico>(ordemServico, "OrdemServicoAdicionar");
            }
        }

        private async Task PreecheListaTextoRetornado()
        {
            try
            {
                // Define o comando
                string comando = "SELECT " + ParametroSelecionado.Valor + " AS Coluna " +
                "FROM tb_ordens_servico AS orse " +
                "LEFT JOIN tb_tipos_ordem_servico AS tios ON orse.id_tipo_ordem_servico = tios.id_tipo_ordem_servico " +
                "LEFT JOIN tb_status_equipamento_apos_manutencao AS staq ON orse.id_status_equipamento_apos_manutencao = staq.id_status_equipamento_apos_manutencao " +
                "LEFT JOIN tb_usos_indevidos AS usind ON orse.id_uso_indevido = usind.id_uso_indevido " +
                "LEFT JOIN tb_tipos_manutencao AS tima ON orse.id_tipo_manutencao = tima.id_tipo_manutencao " +
                "LEFT JOIN tb_executantes_servico AS exse ON orse.id_executante_servico = exse.id_executante_servico " +
                "LEFT JOIN tb_usuarios AS orse_usua_ins ON orse.id_usuario_insercao = orse_usua_ins.id_usuario " +
                "LEFT JOIN tb_usuarios AS orse_usua_edi ON orse.id_usuario_edicao = orse_usua_edi.id_usuario " +
                "LEFT JOIN tb_setores AS seto_ins ON orse_usua_ins.id_setor = seto_ins.id_setor " +
                "LEFT JOIN tb_setores AS seto_edi ON orse_usua_edi.id_setor = seto_edi.id_setor " +
                "LEFT JOIN tb_status AS orse_stat ON orse.id_status = orse_stat.id_status " +
                "LEFT JOIN tb_filiais AS fili ON orse.id_filial = fili.id_filial " +
                "LEFT JOIN tb_clientes AS clie ON orse.id_cliente = clie.id_cliente " +
                "LEFT JOIN tb_frotas AS frot ON orse.id_frota = frot.id_frota " +
                "LEFT JOIN tb_areas AS area ON frot.id_area = area.id_area " +
                "LEFT JOIN tb_plantas AS plan ON area.id_planta = plan.id_planta " +
                "LEFT JOIN tb_series AS seri ON orse.id_serie = seri.id_serie " +
                "LEFT JOIN tb_fabricantes AS fabr ON seri.id_fabricante = fabr.id_fabricante " +
                "LEFT JOIN tb_tipos_equipamento AS tieq ON seri.id_tipo_equipamento = tieq.id_tipo_equipamento " +
                "LEFT JOIN tb_modelos AS mode ON seri.id_modelo = mode.id_modelo " +
                "LEFT JOIN tb_categorias AS cate ON mode.id_categoria = cate.id_categoria " +
                "LEFT JOIN tb_classes AS clas ON mode.id_classe = clas.id_classe " +
                "LEFT JOIN tb_anos AS ano ON seri.id_ano = ano.id_ano "
                + "GROUP BY " + ParametroSelecionado.Valor + " ORDER BY " + ParametroSelecionado.Valor;

                await TextoFiltro.PreencheListaTextoRetornadoAsync(ListaTextoPesquisa, true, null, CancellationToken.None, comando, App.ConfiguracoesGerais.LimiteResultadosPesquisa, "");
            }
            catch (Exception)
            {
            }
        }

        private async Task ExportarPesquisa()
        {
            //ValorProgresso = 0;
            //_cts = new();

            //Progress<double> progresso = new(dbl =>
            //{
            //    ValorProgresso = dbl;
            //});

            //var customDialog = new CustomDialog();

            //var dataContext = new CustomProgressViewModel("Exportando pesquisa", "Aguarde...", false, true, _cts, instance =>
            //{
            //    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            //});

            //customDialog.Content = new CustomProgressView { DataContext = dataContext };

            //await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);

            //try
            //{
            //    await ExcelClasses.ExportarPesquisaProposta(ListaResultadosPesquisaProposta, progresso, _cts.Token);
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

            VistaSaveFileDialog sfd = new VistaSaveFileDialog()
            {
                Filter = "Arquivo do Excel (*.xlsx)|*.xlsx",
                Title = "Informe o local e o nome do arquivo",
                FileName = "Pesquisa_Ordens_Servico_" + DateTime.Now.ToString("yyyyMMddhhmmss"),
                AddExtension = true
            };

            bool houveErro = false;

            try
            {
                var options = new ExcelExportingOptions();
                options.ExcelVersion = ExcelVersion.Excel2013;
                var excelEngine = DataGrid.ExportToExcel(DataGrid.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];
                workBook.Worksheets[0].Name = "pesquisa_ordem_servico";
                try
                {
                    workBook.Worksheets[1].Remove();
                    workBook.Worksheets[2].Remove();
                }
                catch (Exception)
                {
                }
                if (sfd.ShowDialog() == true)
                {
                    if (!sfd.FileName.EndsWith(".xlsx"))
                    {
                        sfd.FileName = sfd.FileName + ".xlsx";
                    }

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
                        "Pesquisa exportada", "Deseja abrir o arquivo?",
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

                    MensagemStatus = "Falha na abertura do arquivo. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                }
            }
        }

        private async Task VisualizarOrdemServico(int? ordemServico)
        {
            if (ordemServico == null)
            {
                return;
            }

            try
            {
                ProcessStartInfo psInfo = new ProcessStartInfo
                {
                    FileName = _configuracaoSistema.LocalOrdensServico.ToString() + "\\" + ordemServico.ToString() + ".pdf",
                    UseShellExecute = true
                };

                Process.Start(psInfo);
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao abrir o arquivo");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Ordem de serviço não encontrada", "Falha na abertura do arquivo. Verifique se o arquivo existe ou verifique a configuração de local de ordens de serviço (administrador master). Caso o problema persista, contate o desenvolvedor e envie o arquivo de log", MessageDialogStyle.Affirmative, mySettings);
            }
        }

        /// <summary>
        /// Método assíncrono que preenche e define os itens, deve ser chamado no construtor
        /// </summary>
        private async Task ConstrutorAsync()
        {
            try
            {
                // Preenche as listas com as classes necessárias
                await FiltroUsuario.PreencheListaFiltroUsuarioAsync(ListaFiltroUsuario, true, null, CancellationToken.None,
                    "WHERE id_usuario = @id_usuario AND id_modulo = @id_modulo", "@id_usuario,@id_modulo", App.Usuario.Id, 2);

                _configuracaoSistema = new();

                await _configuracaoSistema.GetConfiguracaoSistemaDatabaseAsync(1, CancellationToken.None);

                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelSetores, "SELECT seto.id_setor AS Id, seto.nome AS Nome FROM tb_setores AS seto LEFT JOIN tb_usuarios AS usua ON usua.id_setor = seto.id_setor INNER JOIN tb_ordens_servico AS orse ON orse.id_usuario_insercao = usua.id_usuario GROUP BY seto.id_setor ORDER BY seto.nome");
                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelUsuariosInsercao, "SELECT usua.id_usuario AS Id, usua.nome AS Nome FROM tb_usuarios AS usua INNER JOIN tb_ordens_servico AS orse ON orse.id_usuario_insercao = usua.id_usuario GROUP BY usua.id_usuario ORDER BY usua.nome");
                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelClientes, "SELECT clie.id_cliente AS Id, clie.nome AS Nome FROM tb_clientes AS clie INNER JOIN tb_ordens_servico AS orse ON orse.id_cliente = clie.id_cliente GROUP BY clie.id_cliente ORDER BY clie.nome");
                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelTipoOrdemServico, "SELECT tios.id_tipo_ordem_servico AS Id, tios.nome AS Nome FROM tb_tipos_ordem_servico AS tios INNER JOIN tb_ordens_servico AS orse ON orse.id_tipo_ordem_servico = tios.id_tipo_ordem_servico GROUP BY tios.id_tipo_ordem_servico ORDER BY tios.nome");
                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelEquipamentoAposManutencao, "SELECT seam.id_status_equipamento_apos_manutencao AS Id, seam.nome AS Nome FROM tb_status_equipamento_apos_manutencao AS seam INNER JOIN tb_ordens_servico AS orse ON orse.id_status_equipamento_apos_manutencao = seam.id_status_equipamento_apos_manutencao GROUP BY seam.id_status_equipamento_apos_manutencao ORDER BY seam.nome");
                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelTipoManutencao, "SELECT tima.id_tipo_manutencao AS Id, tima.nome AS Nome FROM tb_tipos_manutencao AS tima INNER JOIN tb_ordens_servico AS orse ON orse.id_tipo_manutencao = tima.id_tipo_manutencao GROUP BY tima.id_tipo_manutencao ORDER BY tima.nome");
                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelExecutanteServico, "SELECT exse.id_executante_servico AS Id, exse.nome AS Nome FROM tb_executantes_servico AS exse INNER JOIN tb_ordens_servico AS orse ON orse.id_executante_servico = exse.id_executante_servico GROUP BY exse.id_executante_servico ORDER BY exse.nome");
                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelPassosExecutados, "SELECT etapas_concluidas AS Id, etapas_concluidas AS Nome FROM tb_ordens_servico GROUP BY etapas_concluidas ORDER BY etapas_concluidas ASC");
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");
            }
            Messenger.Default.Send<IPageViewModel>(this, "SelecionaPrimeiraPagina");
        }

        private async Task SalvarFiltros()
        {
            try
            {
                FiltroUsuarioCarregado = new();

                //await FiltroUsuarioCarregado.DeletarFiltroUsuarioDatabaseAsync(CancellationToken.None);

                FiltroUsuarioCarregado.Nome = NomeNovoFiltroUsuario;
                FiltroUsuarioCarregado.Id = 0;
                FiltroUsuarioCarregado.IdUsuario = App.Usuario.Id.GetValueOrDefault(1);
                FiltroUsuarioCarregado.IdModulo = 2;

                FiltroUsuarioCarregado.ListaParametroFiltroPesquisaProposta.Clear();

                foreach (var item in ListaFiltros)
                {
                    FiltroUsuarioCarregado.ListaParametroFiltroPesquisaProposta.Add(item);
                }

                await FiltroUsuarioCarregado.SalvarFiltroUsuarioDatabaseAsync(CancellationToken.None);

                // Preenche as listas com as classes necessárias
                await FiltroUsuario.PreencheListaFiltroUsuarioAsync(ListaFiltroUsuario, true, null, CancellationToken.None,
                    "WHERE id_usuario = @id_usuario AND id_modulo = @id_modulo", "@id_usuario,@id_modulo", App.Usuario.Id, 2);

                NomeNovoFiltroUsuario = String.Empty;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao salvar filtros");
            }
        }

        private void CarregaFiltro(object filtroUsuario)
        {
            ListaFiltros.Clear();
            foreach (var item in ((FiltroUsuario)filtroUsuario).ListaParametroFiltroPesquisaProposta)
            {
                ListaFiltros.Add(item);
            }
            ExecutarPesquisa(CancellationToken.None).Await();
        }

        private async void DeletaFiltro(object filtroUsuario)
        {
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Sim",
                NegativeButtonText = "Não"
            };

            var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                "Confirmação", "Tem certeza que deseja excluir o filtro? O processo não poderá ser desfeito",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem == MessageDialogResult.Affirmative)
            {
                ((FiltroUsuario)filtroUsuario).DeletarFiltroUsuarioDatabaseAsync(CancellationToken.None).Await();
                ListaFiltroUsuario.Remove((FiltroUsuario)filtroUsuario);
            }
        }

        private void AlterarTamanhoFiltro()
        {
            if (!FilterVisible)
            {
                FilterVisible = true;
                FilterSize = 300;
                FilterIcon = "ChevronLeftCircleOutline";
            }
            else
            {
                FilterVisible = false;
                FilterSize = 25;
                FilterIcon = "ChevronRightCircleOutline";
            }
        }

        private void LimparLista(ObservableCollection<ItemFiltro> lista)
        {
            foreach (var item in lista)
            {
                item.Selecionado = false;
            }
        }

        private void LimparFiltroDataChamado()
        {
            DataDeChamado = null;
            DataAteChamado = null;
        }

        private void VerificaDataDeChamado()
        {
            if (DataDeChamado != null && DataAteChamado != null)
            {
                if (DataDeChamado > DataAteChamado)
                {
                    DataDeChamado = null;
                }
            }
        }

        private void VerificaDataAteChamado()
        {
            if (DataDeChamado != null && DataAteChamado != null)
            {
                if (DataAteChamado < DataDeChamado)
                {
                    DataAteChamado = null;
                }
            }
        }

        private void LimparFiltroDataAtendimento()
        {
            DataDeAtendimento = null;
            DataAteAtendimento = null;
        }

        private void VerificaDataDeAtendimento()
        {
            if (DataDeAtendimento != null && DataAteAtendimento != null)
            {
                if (DataDeAtendimento > DataAteAtendimento)
                {
                    DataDeAtendimento = null;
                }
            }
        }

        private void VerificaDataAteAtendimento()
        {
            if (DataDeAtendimento != null && DataAteAtendimento != null)
            {
                if (DataAteAtendimento < DataDeAtendimento)
                {
                    DataAteAtendimento = null;
                }
            }
        }

        private void LimparFiltroDataInsercao()
        {
            DataDeInsercao = null;
            DataAteInsercao = null;
        }

        private void VerificaDataDeInsercao()
        {
            if (DataDeInsercao != null && DataAteInsercao != null)
            {
                if (DataDeInsercao > DataAteInsercao)
                {
                    DataDeInsercao = null;
                }
            }
        }

        private void VerificaDataAteInsercao()
        {
            if (DataDeInsercao != null && DataAteInsercao != null)
            {
                if (DataAteInsercao < DataDeInsercao)
                {
                    DataAteInsercao = null;
                }
            }
        }

        #endregion Métodos
    }
}