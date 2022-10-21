using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
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
    public class PesquisarPropostaViewModel : ObservableObject, IPageViewModel
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
        private readonly IDialogCoordinator _dialogCoordinator;
        private ObservableCollection<Parametro> _listaParametros = new();
        private ObservableCollection<Parametro> _listaOpcaoPesquisa = new();
        private ObservableCollection<Parametro> _listaOperadores = new();
        private ObservableCollection<Parametro> _listaClassificarPor = new();
        private ObservableCollection<Parametro> _listaOrdem = new();
        private ObservableCollection<FiltroUsuario> _listaFiltroUsuario = new();
        private ObservableCollection<ParametroFiltroPesquisa> _listaFiltros = new();
        private ObservableCollection<ResultadoPesquisaProposta> _listaResultadosPesquisaProposta = new();
        private ObservableCollection<TextoFiltro> _listaTextoPesquisa = new();
        private Parametro _parametroSelecionado;
        private Parametro _opcaoPesquisaSelecionada;
        private Parametro _operadorSelecionado;
        private Parametro _classificarPorSelecionado;
        private Parametro _ordemSelecionada;
        private FiltroUsuario _filtroUsuarioCarregado;
        private ParametroFiltroPesquisa _filtroSelecionado;
        private ResultadoPesquisaProposta _resultadoPesquisaPropostaSelecionado;
        private bool _consideraPropostasRevisadas;
        private CancellationTokenSource _cts;
        private string _textoResultadosEncontrados;
        private string _nomeNovoFiltroUsuario;

        private int _filterSize = 300;
        private bool _filterVisible = true;
        private string _filterIcon = "ChevronLeftCircleOutline";

        private bool _pesquisaDataVisivel;
        private bool _pesquisaTextoVisivel;
        private bool _pesquisaIntegerVisivel;
        private bool _pesquisaDecimalVisivel;

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

        private ICommand _comandoVisualizarProposta;

        // Dados da pesquisa básica
        private bool _pesquisaBasica = true;

        private string _textoPesquisado;

        private DateTime? _dataDeSolicitacao;
        private DateTime? _dataAteSolicitacao;
        private DateTime? _dataDeEnvio;
        private DateTime? _dataAteEnvio;
        private DateTime? _dataDeFaturamento;
        private DateTime? _dataAteFaturamento;
        private DateTime? _dataDeInsercao;
        private DateTime? _dataAteInsercao;

        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelSetores = new();
        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelUsuariosInsercao = new();
        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelClientes = new();
        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelStatusAprovacao = new();
        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelJustificativaAprovacao = new();
        private ObservableCollection<ItemFiltro> _listaObjetoSelecionavelPrioridades = new();

        private ICommand _comandoLimparFiltroSetor;
        private ICommand _comandoLimparFiltroUsuarioInsercao;
        private ICommand _comandoLimparFiltroCliente;
        private ICommand _comandoLimparFiltroStatusAprovacao;
        private ICommand _comandoLimparFiltroJustificativaAprovacao;
        private ICommand _comandoLimparFiltroPrioridade;

        private ICommand _comandoLimparFiltroDataSolicitacao;
        private ICommand _comandoVerificaDataDeSolicitacao;
        private ICommand _comandoVerificaDataAteSolicitacao;
        private ICommand _comandoLimparFiltroDataEnvio;
        private ICommand _comandoVerificaDataDeEnvio;
        private ICommand _comandoVerificaDataAteEnvio;
        private ICommand _comandoLimparFiltroDataFaturamento;
        private ICommand _comandoVerificaDataDeFaturamento;
        private ICommand _comandoVerificaDataAteFaturamento;
        private ICommand _comandoLimparFiltroDataInsercao;
        private ICommand _comandoVerificaDataDeInsercao;
        private ICommand _comandoVerificaDataAteInsercao;

        #endregion Campos

        #region Construtores

        public PesquisarPropostaViewModel(IDialogCoordinator dialogCoordinator)
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

            Name = "Pesquisar proposta";

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
                return "Handshake";
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
                _listaResultadosPesquisaProposta = null;
                _listaTextoPesquisa = null;
                _parametroSelecionado = null;
                _opcaoPesquisaSelecionada = null;
                _operadorSelecionado = null;
                _classificarPorSelecionado = null;
                _ordemSelecionada = null;
                _filtroUsuarioCarregado = null;
                _filtroSelecionado = null;
                _resultadoPesquisaPropostaSelecionado = null;
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
                _comandoVisualizarProposta = null;
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

        public ObservableCollection<ResultadoPesquisaProposta> ListaResultadosPesquisaProposta
        {
            get { return _listaResultadosPesquisaProposta; }
            set
            {
                if (value != _listaResultadosPesquisaProposta)
                {
                    _listaResultadosPesquisaProposta = value;
                    OnPropertyChanged(nameof(ListaResultadosPesquisaProposta));
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
                            PesquisaDecimalVisivel = false;
                            PesquisaIntegerVisivel = false;
                            PesquisaDataVisivel = true;
                            PesquisaTextoVisivel = false;
                            break;

                        case "string":
                            PesquisaDecimalVisivel = false;
                            PesquisaIntegerVisivel = false;
                            PesquisaDataVisivel = false;
                            PesquisaTextoVisivel = true;
                            break;

                        case "decimal":
                            PesquisaDecimalVisivel = true;
                            PesquisaIntegerVisivel = false;
                            PesquisaDataVisivel = false;
                            PesquisaTextoVisivel = false;
                            break;

                        case "integer":
                            PesquisaDecimalVisivel = false;
                            PesquisaIntegerVisivel = true;
                            PesquisaDataVisivel = false;
                            PesquisaTextoVisivel = false;
                            break;

                        default:
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

        public ResultadoPesquisaProposta ResultadoPesquisaPropostaSelecionado
        {
            get { return _resultadoPesquisaPropostaSelecionado; }
            set
            {
                if (value != _resultadoPesquisaPropostaSelecionado)
                {
                    _resultadoPesquisaPropostaSelecionado = value;
                    OnPropertyChanged(nameof(ResultadoPesquisaPropostaSelecionado));
                }
            }
        }

        public bool ConsideraPropostasRevisadas
        {
            get { return _consideraPropostasRevisadas; }
            set
            {
                if (value != _consideraPropostasRevisadas)
                {
                    _consideraPropostasRevisadas = value;
                    OnPropertyChanged(nameof(ConsideraPropostasRevisadas));
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
                        param => AbrirProposta().Await(),
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

        public ICommand ComandoVisualizarProposta
        {
            get
            {
                if (_comandoVisualizarProposta == null)
                {
                    _comandoVisualizarProposta = new RelayCommand(
                        param => VisualizarProposta().Await(),
                        param => true
                    );
                }
                return _comandoVisualizarProposta;
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

        public DateTime? DataDeSolicitacao
        {
            get { return _dataDeSolicitacao; }
            set
            {
                if (value != _dataDeSolicitacao)
                {
                    _dataDeSolicitacao = value;
                    OnPropertyChanged(nameof(DataDeSolicitacao));
                }
            }
        }

        public DateTime? DataAteSolicitacao
        {
            get { return _dataAteSolicitacao; }
            set
            {
                if (value != _dataAteSolicitacao)
                {
                    _dataAteSolicitacao = value;
                    OnPropertyChanged(nameof(DataAteSolicitacao));
                }
            }
        }

        public DateTime? DataDeEnvio
        {
            get { return _dataDeEnvio; }
            set
            {
                if (value != _dataDeEnvio)
                {
                    _dataDeEnvio = value;
                    OnPropertyChanged(nameof(DataDeEnvio));
                }
            }
        }

        public DateTime? DataAteEnvio
        {
            get { return _dataAteEnvio; }
            set
            {
                if (value != _dataAteEnvio)
                {
                    _dataAteEnvio = value;
                    OnPropertyChanged(nameof(DataAteEnvio));
                }
            }
        }

        public DateTime? DataDeFaturamento
        {
            get { return _dataDeFaturamento; }
            set
            {
                if (value != _dataDeFaturamento)
                {
                    _dataDeFaturamento = value;
                    OnPropertyChanged(nameof(DataDeFaturamento));
                }
            }
        }

        public DateTime? DataAteFaturamento
        {
            get { return _dataAteFaturamento; }
            set
            {
                if (value != _dataAteFaturamento)
                {
                    _dataAteFaturamento = value;
                    OnPropertyChanged(nameof(DataAteFaturamento));
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

        public ObservableCollection<ItemFiltro> ListaObjetoSelecionavelStatusAprovacao
        {
            get { return _listaObjetoSelecionavelStatusAprovacao; }
            set
            {
                if (value != _listaObjetoSelecionavelStatusAprovacao)
                {
                    _listaObjetoSelecionavelStatusAprovacao = value;
                    OnPropertyChanged(nameof(ListaObjetoSelecionavelStatusAprovacao));
                }
            }
        }

        public ObservableCollection<ItemFiltro> ListaObjetoSelecionavelJustificativaAprovacao
        {
            get { return _listaObjetoSelecionavelJustificativaAprovacao; }
            set
            {
                if (value != _listaObjetoSelecionavelJustificativaAprovacao)
                {
                    _listaObjetoSelecionavelJustificativaAprovacao = value;
                    OnPropertyChanged(nameof(ListaObjetoSelecionavelJustificativaAprovacao));
                }
            }
        }

        public ObservableCollection<ItemFiltro> ListaObjetoSelecionavelPrioridades
        {
            get { return _listaObjetoSelecionavelPrioridades; }
            set
            {
                if (value != _listaObjetoSelecionavelPrioridades)
                {
                    _listaObjetoSelecionavelPrioridades = value;
                    OnPropertyChanged(nameof(ListaObjetoSelecionavelPrioridades));
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

        public ICommand ComandoLimparFiltroStatusAprovacao
        {
            get
            {
                if (_comandoLimparFiltroStatusAprovacao == null)
                {
                    _comandoLimparFiltroStatusAprovacao = new RelayCommand(
                        param => LimparLista(ListaObjetoSelecionavelStatusAprovacao),
                        param => true
                    );
                }
                return _comandoLimparFiltroStatusAprovacao;
            }
        }

        public ICommand ComandoLimparFiltroJustificativaAprovacao
        {
            get
            {
                if (_comandoLimparFiltroJustificativaAprovacao == null)
                {
                    _comandoLimparFiltroJustificativaAprovacao = new RelayCommand(
                        param => LimparLista(ListaObjetoSelecionavelJustificativaAprovacao),
                        param => true
                    );
                }
                return _comandoLimparFiltroJustificativaAprovacao;
            }
        }

        public ICommand ComandoLimparFiltroPrioridade
        {
            get
            {
                if (_comandoLimparFiltroPrioridade == null)
                {
                    _comandoLimparFiltroPrioridade = new RelayCommand(
                        param => LimparLista(ListaObjetoSelecionavelPrioridades),
                        param => true
                    );
                }
                return _comandoLimparFiltroPrioridade;
            }
        }

        public ICommand ComandoLimparFiltroDataSolicitacao
        {
            get
            {
                if (_comandoLimparFiltroDataSolicitacao == null)
                {
                    _comandoLimparFiltroDataSolicitacao = new RelayCommand(
                        param => LimparFiltroDataSolicitacao(),
                        param => true
                    );
                }
                return _comandoLimparFiltroDataSolicitacao;
            }
        }

        public ICommand ComandoVerificaDataDeSolicitacao
        {
            get
            {
                if (_comandoVerificaDataDeSolicitacao == null)
                {
                    _comandoVerificaDataDeSolicitacao = new RelayCommand(
                        param => VerificaDataDeSolicitacao(),
                        param => true
                    );
                }
                return _comandoVerificaDataDeSolicitacao;
            }
        }

        public ICommand ComandoVerificaDataAteSolicitacao
        {
            get
            {
                if (_comandoVerificaDataAteSolicitacao == null)
                {
                    _comandoVerificaDataAteSolicitacao = new RelayCommand(
                        param => VerificaDataAteSolicitacao(),
                        param => true
                    );
                }
                return _comandoVerificaDataAteSolicitacao;
            }
        }

        public ICommand ComandoLimparFiltroDataEnvio
        {
            get
            {
                if (_comandoLimparFiltroDataEnvio == null)
                {
                    _comandoLimparFiltroDataEnvio = new RelayCommand(
                        param => LimparFiltroDataEnvio(),
                        param => true
                    );
                }
                return _comandoLimparFiltroDataEnvio;
            }
        }

        public ICommand ComandoVerificaDataDeEnvio
        {
            get
            {
                if (_comandoVerificaDataDeEnvio == null)
                {
                    _comandoVerificaDataDeEnvio = new RelayCommand(
                        param => VerificaDataDeEnvio(),
                        param => true
                    );
                }
                return _comandoVerificaDataDeEnvio;
            }
        }

        public ICommand ComandoVerificaDataAteEnvio
        {
            get
            {
                if (_comandoVerificaDataAteEnvio == null)
                {
                    _comandoVerificaDataAteEnvio = new RelayCommand(
                        param => VerificaDataAteEnvio(),
                        param => true
                    );
                }
                return _comandoVerificaDataAteEnvio;
            }
        }

        public ICommand ComandoLimparFiltroDataFaturamento
        {
            get
            {
                if (_comandoLimparFiltroDataFaturamento == null)
                {
                    _comandoLimparFiltroDataFaturamento = new RelayCommand(
                        param => LimparFiltroDataFaturamento(),
                        param => true
                    );
                }
                return _comandoLimparFiltroDataFaturamento;
            }
        }

        public ICommand ComandoVerificaDataDeFaturamento
        {
            get
            {
                if (_comandoVerificaDataDeFaturamento == null)
                {
                    _comandoVerificaDataDeFaturamento = new RelayCommand(
                        param => VerificaDataDeFaturamento(),
                        param => true
                    );
                }
                return _comandoVerificaDataDeFaturamento;
            }
        }

        public ICommand ComandoVerificaDataAteFaturamento
        {
            get
            {
                if (_comandoVerificaDataAteFaturamento == null)
                {
                    _comandoVerificaDataAteFaturamento = new RelayCommand(
                        param => VerificaDataAteFaturamento(),
                        param => true
                    );
                }
                return _comandoVerificaDataAteFaturamento;
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
                ListaParametros.Add(new Parametro("Data de solicitação", "prop.data_solicitacao", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data de inserção", "prop.data_insercao", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data de envio para o faturamento", "prop.data_envio_faturamento", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data de faturamento", "prop.data_faturamento", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Nota fiscal", "prop.nota_fiscal", "integer", "N0", "Numeric", "SteelBlue"));
                ListaParametros.Add(new Parametro("Valor do faturamento", "prop.valor_faturamento", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Filial", "fili.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Cliente", "clie.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Contato", "cont.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Código da proposta", "prop.codigo_proposta", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Setor do usuário de inserção", "seto_ins.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Usuário de inserção", "prop_usua_ins.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Setor do usuário de edição", "seto_edi.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Usuário de edição", "prop_usua_edi.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Status da proposta", "prop_stat.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data da aprovação da proposta", "prop.data_aprovacao", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Status da aprovação da proposta", "prop_stap.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Justificativa da aprovação da proposta", "prop_juap.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Prioridade da proposta", "prio.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Fabricante do equipamento", "fabr.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Tipo do equipamento", "tieq.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Modelo do equipamento", "mode.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Ano do equipamento", "ano.ano", "integer", "N0", "Numeric", "SteelBlue"));
                ListaParametros.Add(new Parametro("Série do equipamento", "seri.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Horímetro", "prop.horimetro", "decimal", "N2", "DecimalComma", "SteelBlue"));
                ListaParametros.Add(new Parametro("Ordem de serviço", "prop.ordem_servico", "integer", "N0", "Numeric", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data de envio da proposta", "prop.data_envio", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data de edição da proposta", "prop.data_edicao", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Texto inicial", "prop.texto_padrao", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Observações", "prop.observacoes", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Prazo de entrega", "prop.prazo_entrega", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Condição de pagamento", "prop.condicao_pagamento", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Garantia", "prop.garantia", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Validade", "prop.validade", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Comentários da proposta", "prop.comentarios", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Valor total da proposta", "prop.valor_total_proposta", "decimal", "C", "CurrencyBrl", "SteelBlue"));

                // Adiciona na list os itens referentes a tabela de itens da proposta
                ListaParametros.Add(new Parametro("Data de inserção do item", "itpr.data_insercao", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Setor do usuário de inserção do item", "itpr_seto_ins.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Usuário de inserção do item", "itpr_usua_ins.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Status do item", "itpr_stat.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Status da aprovação do item", "itpr_stap.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Justificativa da aprovação do item", "itpr_juap.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Conjunto do item", "conj.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Especificação do item", "espe.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Tipo do item", "tiit.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Fornecedor do item", "forn.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Código do item", "itpr.codigo_item", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Descrição do item", "itpr.descricao_item", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Quantidade do item", "itpr.quantidade_item", "decimal", "N0", "DecimalComma", "SteelBlue"));
                ListaParametros.Add(new Parametro("Preço unitário inicial do item", "itpr.preco_unitario_inicial_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Percentual de IPI do item", "itpr.percentual_ipi_item", "decimal", "P2", "Percent", "SteelBlue"));
                ListaParametros.Add(new Parametro("Percentual de ICMS do item", "itpr.percentual_icms_item", "decimal", "P2", "Percent", "SteelBlue"));
                ListaParametros.Add(new Parametro("NCM do item", "itpr.ncm_item", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("MVA do item", "itpr.mva_item", "decimal", "P2", "Percent", "SteelBlue"));
                ListaParametros.Add(new Parametro("Valor ST do item", "itpr.valor_st_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Valor total inicial do item", "itpr.valor_total_inicial_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Prazo inicial do item", "itpr.prazo_inicial_item", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Frete unitário do item", "itpr.frete_unitario_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Desconto inicial do item", "itpr.desconto_inicial_item", "decimal", "P2", "Percent", "SteelBlue"));
                ListaParametros.Add(new Parametro("Tipo de substituição tributária do item", "tstr.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Origem do item", "orig.nome", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Preço após desconto inicial do item", "itpr.preco_apos_desconto_inicial_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Preço com IPI e ICMS do item", "itpr.preco_com_ipi_e_icms_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Percentual de alíquota externa ICMS do item", "itpr.percentual_aliquota_externa_icms_item", "decimal", "P2", "Percent", "SteelBlue"));
                ListaParametros.Add(new Parametro("Valor ICMS crédito do item", "itpr.valor_icms_credito_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Percentual do MVA do item", "itpr.percentual_mva_item", "decimal", "P2", "Percent", "SteelBlue"));
                ListaParametros.Add(new Parametro("Valor do MVA do item", "itpr.valor_mva_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Preço com MVA do item", "itpr.preco_com_mva_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Percentual de alíquota interna ICMS do item", "itpr.percentual_aliquota_interna_icms_item", "decimal", "P2", "Percent", "SteelBlue"));
                ListaParametros.Add(new Parametro("Valor do ICMS débito do item", "itpr.valor_icms_debito_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Saldo do ICMS do item", "itpr.saldo_icms_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Preço unitário para revendedor do item", "itpr.preco_unitario_para_revendedor_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Impostos federais do item", "itpr.impostos_federais_item", "decimal", "P2", "Percent", "SteelBlue"));
                ListaParametros.Add(new Parametro("Rateio despesa fixa do item", "itpr.rateio_despesa_fixa_item", "decimal", "P2", "Percent", "SteelBlue"));
                ListaParametros.Add(new Parametro("Percentual de lucro necessário do item", "itpr.percentual_lucro_necessario_item", "decimal", "P2", "Percent", "SteelBlue"));
                ListaParametros.Add(new Parametro("Preço da lista de venda do item", "itpr.preco_lista_venda_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Desconto final do item", "itpr.desconto_final_item", "decimal", "P2", "Percent", "SteelBlue"));
                ListaParametros.Add(new Parametro("Preço unitário final do item", "itpr.preco_unitario_final_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Preço total final do item", "itpr.preco_total_final_item", "decimal", "C", "CurrencyBrl", "SteelBlue"));
                ListaParametros.Add(new Parametro("Quantidade em estoque (código completo) do item", "itpr.quantidade_estoque_codigo_completo_item", "decimal", "N0", "DecimalComma", "SteelBlue"));
                ListaParametros.Add(new Parametro("Quantidade em estoque (código abreviado) do item", "itpr.quantidade_estoque_codigo_abreviado_item", "decimal", "N0", "DecimalComma", "SteelBlue"));
                ListaParametros.Add(new Parametro("Informação do estoque (código completo) do item", "itpr.informacao_estoque_codigo_completo_item", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Informação em estoque (código abreviado) do item", "itpr.informacao_estoque_codigo_abreviado_item", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Prazo final do item", "itpr.prazo_final_item", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Comentários do item", "itpr.comentarios_item", "string", "", "CardText", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data de aprovação do item", "itpr.data_aprovacao_item", "date", "", "Calendar", "SteelBlue"));
                ListaParametros.Add(new Parametro("Data de edição do item", "itpr.data_edicao_item", "date", "", "Calendar", "SteelBlue"));

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
                ListaClassificarPor.Add(new Parametro("Código da proposta", "CodigoProposta", "string", "", "CardText", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Data da inserção", "DataInsercao", "string", "", "Calendar", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Data da solicitação", "DataSolicitacao", "string", "", "Calendar", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Cliente", "NomeCliente", "string", "", "CardText", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Contato", "NomeContato", "string", "", "CardText", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Data de envio", "DataEnvio", "string", "", "Calendar", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Quantidade de itens", "QuantidadeTotal", "string", "", "Numeric", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Valor de peças", "ValorPecas", "string", "", "CurrencyBrl", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Valor de serviços", "ValorServicos", "string", "", "CurrencyBrl", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Status da aprovação", "NomeStatusAprovacao", "string", "", "CardText", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Valor total", "ValorTotal", "string", "", "CurrencyBrl", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Série", "NomeSerie", "string", "", "CardText", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Valor faturado", "ValorFaturamento", "string", "", "CurrencyBrl", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Data de envio para fat.", "DataEnvioFaturamento", "string", "", "Calendar", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Data do faturamento", "DataFaturamento", "string", "", "Calendar", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Nota fiscal", "NotaFiscal", "string", "", "Numeric", "SteelBlue"));

                // Preenche a lista de ordens
                ListaOrdem.Add(new Parametro("Crescente", "ASC", "string", "", "SortAlphabeticalAscending", "SteelBlue"));
                ListaOrdem.Add(new Parametro("Decrescente", "DESC", "string", "", "SortAlphabeticalDescending", "SteelBlue"));

                // Define os itens selecionados como os primeiros das listas
                ParametroSelecionado = ListaParametros.First();

                try
                {
                    ParametroSelecionado = ListaParametros.First(tema => tema.Valor == "prop.data_solicitacao");
                }
                catch (Exception)
                {
                }

                OpcaoPesquisaSelecionada = ListaOpcaoPesquisa.First();
                OperadorSelecionado = ListaOperadores.First();
                ClassificarPorSelecionado = ListaClassificarPor.Where(dat => dat.Valor == "DataSolicitacao").First();
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

            if (ConsideraPropostasRevisadas)
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
                    sCondicao = " WHERE id_ultima_proposta IS NULL AND " + sCondicao;
                }
                else
                {
                    sCondicao = " WHERE id_ultima_proposta IS NULL ";
                }
            }

            try
            {
                //string strsqlcommand = "";

                //ObservableCollection<StatusAprovacao> listaStatusAprovacao = new();

                //await StatusAprovacao.PreencheListaStatusAprovacaoAsync(listaStatusAprovacao, true, null, ct, "", "");

                //int quantidadeIfs = 0;

                //foreach (var item in listaStatusAprovacao)
                //{
                //    if (String.IsNullOrEmpty(strsqlcommand))
                //    {
                //        strsqlcommand = "IF(SUM(IF(itpr.id_status_aprovacao=" + item.Id.ToString() + ",1,0))=COUNT(itpr.id_item_proposta)," +
                //            "(SELECT nome FROM tb_status_aprovacao WHERE id_status_aprovacao=" + item.Id.ToString() + ")";
                //    }
                //    else
                //    {
                //        strsqlcommand = strsqlcommand + ",IF(SUM(IF(itpr.id_status_aprovacao=" + item.Id.ToString() + ",1,0))=COUNT(itpr.id_item_proposta)," +
                //            "(SELECT nome FROM tb_status_aprovacao WHERE id_status_aprovacao=" + item.Id.ToString() + ")";
                //    }
                //    quantidadeIfs++;
                //}
                //strsqlcommand = strsqlcommand + ",IF(SUM(IF(itpr.id_status_aprovacao=1,1,0))>0,(SELECT nome FROM tb_status_aprovacao WHERE id_status_aprovacao=2),'Diversos'";

                //for (int i = 1; i < quantidadeIfs + 1; i++)
                //{
                //    strsqlcommand = strsqlcommand + ")";
                //}
                //strsqlcommand = strsqlcommand + ")";

                string textoLimite = "";

                if (App.Usuario.LimiteResultados != null)
                {
                    textoLimite = " LIMIT " + App.Usuario.LimiteResultados.ToString();
                }

                string comandoInicial = "SELECT prop.data_insercao AS DataInsercao, prop_usua_ins.nome AS NomeUsuario, itpr.id_item_proposta AS IdItemProposta, prop.id_proposta AS IdProposta, prop.codigo_proposta AS CodigoProposta, " +
                                            "prop.data_solicitacao AS DataSolicitacao, clie.nome AS NomeCliente, cont.nome AS NomeContato, prop.data_envio AS DataEnvio, " +
                                            "COUNT(itpr.quantidade_item) AS QuantidadeTotal, SUM(CASE WHEN itpr.id_tipo_item = 1 THEN itpr.preco_total_final_item END) AS ValorPecas, " +
                                            "SUM(CASE WHEN itpr.id_tipo_item <> 1 THEN itpr.preco_total_final_item END) AS ValorServicos, " +
                                            "prop.data_aprovacao AS DataAprovacao, " +
                                            "prop_stap.nome AS NomeStatusAprovacao, " +
                                            "prop_juap.nome AS NomeJustificativaAprovacao, " +
                                            "SUM(itpr.preco_total_final_item) AS ValorTotal, seri.nome AS NomeSerie, " +
                                            "prop.valor_faturamento AS ValorFaturamento, prop.data_envio_faturamento AS DataEnvioFaturamento, prop.data_faturamento AS DataFaturamento, " +
                                            "prop.nota_fiscal AS NotaFiscal, prop.comentarios AS ComentariosProposta " +
                                            "FROM tb_propostas AS prop " +
                                            "LEFT JOIN tb_itens_propostas AS itpr ON itpr.id_proposta = prop.id_proposta " +
                                            "LEFT JOIN tb_usuarios AS itpr_usua_ins ON itpr.id_usuario = itpr_usua_ins.id_usuario " +
                                            "LEFT JOIN tb_usuarios AS prop_usua_ins ON prop.id_usuario_insercao = prop_usua_ins.id_usuario " +
                                            "LEFT JOIN tb_usuarios AS prop_usua_edi ON prop.id_usuario_edicao = prop_usua_edi.id_usuario " +
                                            "LEFT JOIN tb_setores AS seto_ins ON prop_usua_ins.id_setor = seto_ins.id_setor " +
                                            "LEFT JOIN tb_setores AS seto_edi ON prop_usua_edi.id_setor = seto_edi.id_setor " +
                                            "LEFT JOIN tb_setores AS itpr_seto_ins ON itpr_usua_ins.id_setor = itpr_seto_ins.id_setor " +
                                            "LEFT JOIN tb_status AS itpr_stat ON itpr.id_status = itpr_stat.id_status " +
                                            "LEFT JOIN tb_status AS prop_stat ON prop.id_status = prop_stat.id_status " +
                                            "LEFT JOIN tb_status_aprovacao AS prop_stap ON prop.id_status_aprovacao = prop_stap.id_status_aprovacao " +
                                            "LEFT JOIN tb_justificativas_aprovacao AS prop_juap ON prop.id_justificativa_aprovacao = prop_juap.id_justificativa_aprovacao " +
                                            "LEFT JOIN tb_status_aprovacao AS itpr_stap ON itpr.id_status_aprovacao = itpr_stap.id_status_aprovacao " +
                                            "LEFT JOIN tb_justificativas_aprovacao AS itpr_juap ON itpr.id_justificativa_aprovacao = itpr_juap.id_justificativa_aprovacao " +
                                            "LEFT JOIN tb_conjuntos AS conj ON itpr.id_conjunto = conj.id_conjunto " +
                                            "LEFT JOIN tb_especificacoes AS espe ON itpr.id_especificacao = espe.id_especificacao " +
                                            "LEFT JOIN tb_tipos_item AS tiit ON itpr.id_tipo_item = tiit.id_tipo_item " +
                                            "LEFT JOIN tb_fornecedores AS forn ON itpr.id_fornecedor = forn.id_fornecedor " +
                                            "LEFT JOIN tb_tipos_substituicao_tributaria AS tstr ON itpr.id_tipo_substituicao_tributaria_item = tstr.id_tipo_substituicao_tributaria " +
                                            "LEFT JOIN tb_origens AS orig ON itpr.id_origem_item = orig.id_origem " +
                                            "LEFT JOIN tb_filiais AS fili ON prop.id_filial = fili.id_filial " +
                                            "LEFT JOIN tb_clientes AS clie ON prop.id_cliente = clie.id_cliente " +
                                            "LEFT JOIN tb_contatos AS cont ON prop.id_contato = cont.id_contato " +
                                            "LEFT JOIN tb_prioridades AS prio ON prop.id_prioridade = prio.id_prioridade " +
                                            "LEFT JOIN tb_fabricantes AS fabr ON prop.id_fabricante = fabr.id_fabricante " +
                                            "LEFT JOIN tb_tipos_equipamento AS tieq ON prop.id_tipo_equipamento = tieq.id_tipo_equipamento " +
                                            "LEFT JOIN tb_modelos AS mode ON prop.id_modelo = mode.id_modelo " +
                                            "LEFT JOIN tb_anos AS ano ON prop.id_ano = ano.id_ano " +
                                            "LEFT JOIN tb_series AS seri ON prop.id_serie = seri.id_serie " +
                                            sCondicao + " GROUP BY itpr.id_proposta ORDER BY " +
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
                    await ResultadoPesquisaProposta.PreencheListaResultadosPesquisaPropostaAsync(ListaResultadosPesquisaProposta, true, progresso, _cts.Token, comandoInicial, parametros, valoresParametros.ToArray());

                    if (App.Usuario.LimiteResultados == null)
                    {
                        TextoLimiteDeResultados = "*Sem limite de resultados. Caso a pesquisa esteja lenta, defina um limite na opção Perfil.";
                    }
                    else
                    {
                        TextoLimiteDeResultados = "*Limitado a " + App.Usuario.LimiteResultados.ToString() + " resultado (s). Você pode alterar isso na opção Perfil.";
                    }

                    TextoResultadosEncontrados = "Resultado (s) encontrado (s): " + ListaResultadosPesquisaProposta.Count;
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
                condicao.Add("(fili.nome LIKE @texto_pesquisa OR " +
                "clie.nome LIKE @texto_pesquisa OR " +
                "cont.nome LIKE @texto_pesquisa OR " +
                "prop.codigo_proposta LIKE @texto_pesquisa OR " +
                "fabr.nome LIKE @texto_pesquisa OR " +
                "tieq.nome LIKE @texto_pesquisa OR " +
                "mode.nome LIKE @texto_pesquisa OR " +
                "seri.nome LIKE @texto_pesquisa OR " +
                "tiit.nome LIKE @texto_pesquisa OR " +
                "forn.nome LIKE @texto_pesquisa OR " +
                "itpr.codigo_item LIKE @texto_pesquisa OR " +
                "itpr.descricao_item LIKE @texto_pesquisa)");
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
                condicao.Add("prop_usua_ins.id_setor IN (" + textoParametrosSetores + ")");
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
                condicao.Add("prop.id_usuario_insercao IN (" + textoParametrosUsuarios + ")");
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
                condicao.Add("prop.id_cliente IN (" + textoParametrosClientes + ")");
            }

            string textoParametrosStatusAprovacao = "";
            foreach (var item in ListaObjetoSelecionavelStatusAprovacao.Where(d => d.Selecionado))
            {
                listaNomeParametros.Add("@id_status_aprovacao" + ListaObjetoSelecionavelStatusAprovacao.IndexOf(item));
                listaValores.Add(item.Id);

                if (String.IsNullOrEmpty(textoParametrosStatusAprovacao))
                {
                    textoParametrosStatusAprovacao = "@id_status_aprovacao" + ListaObjetoSelecionavelStatusAprovacao.IndexOf(item);
                }
                else
                {
                    textoParametrosStatusAprovacao = textoParametrosStatusAprovacao + ", @id_status_aprovacao" + ListaObjetoSelecionavelStatusAprovacao.IndexOf(item);
                }
            }

            if (!String.IsNullOrEmpty(textoParametrosStatusAprovacao))
            {
                condicao.Add("prop.id_status_aprovacao IN (" + textoParametrosStatusAprovacao + ")");
            }

            string textoParametrosJustificativaAprovacao = "";
            foreach (var item in ListaObjetoSelecionavelJustificativaAprovacao.Where(d => d.Selecionado))
            {
                listaNomeParametros.Add("@id_justificativa_aprovacao" + ListaObjetoSelecionavelJustificativaAprovacao.IndexOf(item));
                listaValores.Add(item.Id);

                if (String.IsNullOrEmpty(textoParametrosJustificativaAprovacao))
                {
                    textoParametrosJustificativaAprovacao = "@id_justificativa_aprovacao" + ListaObjetoSelecionavelJustificativaAprovacao.IndexOf(item);
                }
                else
                {
                    textoParametrosJustificativaAprovacao = textoParametrosJustificativaAprovacao + ", @id_justificativa_aprovacao" + ListaObjetoSelecionavelJustificativaAprovacao.IndexOf(item);
                }
            }

            if (!String.IsNullOrEmpty(textoParametrosJustificativaAprovacao))
            {
                condicao.Add("prop.id_justificativa_aprovacao IN (" + textoParametrosJustificativaAprovacao + ")");
            }

            string textoParametrosPrioridades = "";
            foreach (var item in ListaObjetoSelecionavelPrioridades.Where(d => d.Selecionado))
            {
                listaNomeParametros.Add("@id_prioridade" + ListaObjetoSelecionavelPrioridades.IndexOf(item));
                listaValores.Add(item.Id);

                if (String.IsNullOrEmpty(textoParametrosPrioridades))
                {
                    textoParametrosPrioridades = "@id_prioridade" + ListaObjetoSelecionavelPrioridades.IndexOf(item);
                }
                else
                {
                    textoParametrosPrioridades = textoParametrosPrioridades + ", @id_prioridade" + ListaObjetoSelecionavelPrioridades.IndexOf(item);
                }
            }

            if (!String.IsNullOrEmpty(textoParametrosPrioridades))
            {
                condicao.Add("prop.id_prioridade IN (" + textoParametrosPrioridades + ")");
            }

            if (DataDeSolicitacao != null && DataAteSolicitacao != null)
            {
                listaNomeParametros.Add("@data_solicitacao_de");
                listaValores.Add(DataDeSolicitacao);
                listaNomeParametros.Add("@data_solicitacao_ate");
                listaValores.Add(DataAteSolicitacao);
                condicao.Add("prop.data_solicitacao >= @data_solicitacao_de AND prop.data_solicitacao <= @data_solicitacao_ate");
            }

            if (DataDeSolicitacao != null && DataAteSolicitacao == null)
            {
                listaNomeParametros.Add("@data_solicitacao_de");
                listaValores.Add(DataDeSolicitacao);
                condicao.Add("prop.data_solicitacao >= @data_solicitacao_de");
            }

            if (DataDeSolicitacao == null && DataAteSolicitacao != null)
            {
                listaNomeParametros.Add("@data_solicitacao_ate");
                listaValores.Add(DataAteSolicitacao);
                condicao.Add("prop.data_solicitacao <= @data_solicitacao_ate");
            }

            if (DataDeEnvio != null && DataAteEnvio != null)
            {
                listaNomeParametros.Add("@data_envio_de");
                listaValores.Add(DataDeEnvio);
                listaNomeParametros.Add("@data_envio_ate");
                listaValores.Add(DataAteEnvio);
                condicao.Add("prop.data_envio >= @data_envio_de AND prop.data_envio <= @data_envio_ate");
            }

            if (DataDeEnvio != null && DataAteEnvio == null)
            {
                listaNomeParametros.Add("@data_envio_de");
                listaValores.Add(DataDeEnvio);
                condicao.Add("prop.data_envio >= @data_envio_de");
            }

            if (DataDeEnvio == null && DataAteEnvio != null)
            {
                listaNomeParametros.Add("@data_envio_ate");
                listaValores.Add(DataAteEnvio);
                condicao.Add("prop.data_envio <= @data_envio_ate");
            }

            if (DataDeFaturamento != null && DataAteFaturamento != null)
            {
                listaNomeParametros.Add("@data_faturamento_de");
                listaValores.Add(DataDeFaturamento);
                listaNomeParametros.Add("@data_faturamento_ate");
                listaValores.Add(DataAteFaturamento);
                condicao.Add("prop.data_faturamento >= @data_faturamento_de AND prop.data_faturamento <= @data_faturamento_ate");
            }

            if (DataDeFaturamento != null && DataAteFaturamento == null)
            {
                listaNomeParametros.Add("@data_faturamento_de");
                listaValores.Add(DataDeFaturamento);
                condicao.Add("prop.data_faturamento >= @data_faturamento_de");
            }

            if (DataDeFaturamento == null && DataAteFaturamento != null)
            {
                listaNomeParametros.Add("@data_faturamento_ate");
                listaValores.Add(DataAteFaturamento);
                condicao.Add("prop.data_faturamento <= @data_faturamento_ate");
            }

            if (DataDeInsercao != null && DataAteInsercao != null)
            {
                listaNomeParametros.Add("@data_insercao_de");
                listaValores.Add(DataDeInsercao);
                listaNomeParametros.Add("@data_insercao_ate");
                listaValores.Add(DataAteInsercao);
                condicao.Add("prop.data_insercao >= @data_insercao_de AND prop.data_insercao <= @data_insercao_ate");
            }

            if (DataDeInsercao != null && DataAteInsercao == null)
            {
                listaNomeParametros.Add("@data_insercao_de");
                listaValores.Add(DataDeInsercao);
                condicao.Add("prop.data_insercao >= @data_insercao_de");
            }

            if (DataDeInsercao == null && DataAteInsercao != null)
            {
                listaNomeParametros.Add("@data_insercao_ate");
                listaValores.Add(DataAteInsercao);
                condicao.Add("prop.data_insercao <= @data_insercao_ate");
            }

            if (ConsideraPropostasRevisadas)
            {
                sCondicao = condicao.Count > 0 ? "WHERE " + String.Join(" AND ", condicao.ToArray()) : "";
            }
            else
            {
                if (condicao.Count > 0)
                {
                    sCondicao = " WHERE id_ultima_proposta IS NULL AND " + String.Join(" AND ", condicao.ToArray());
                }
                else
                {
                    sCondicao = " WHERE id_ultima_proposta IS NULL ";
                }
            }

            try
            {
                //string strsqlcommand = "";

                //ObservableCollection<StatusAprovacao> listaStatusAprovacao = new();

                //await StatusAprovacao.PreencheListaStatusAprovacaoAsync(listaStatusAprovacao, true, null, ct, "", "");

                //int quantidadeIfs = 0;

                //foreach (var item in listaStatusAprovacao)
                //{
                //    if (String.IsNullOrEmpty(strsqlcommand))
                //    {
                //        strsqlcommand = "IF(SUM(IF(itpr.id_status_aprovacao=" + item.Id.ToString() + ",1,0))=COUNT(itpr.id_item_proposta)," +
                //            "(SELECT nome FROM tb_status_aprovacao WHERE id_status_aprovacao=" + item.Id.ToString() + ")";
                //    }
                //    else
                //    {
                //        strsqlcommand = strsqlcommand + ",IF(SUM(IF(itpr.id_status_aprovacao=" + item.Id.ToString() + ",1,0))=COUNT(itpr.id_item_proposta)," +
                //            "(SELECT nome FROM tb_status_aprovacao WHERE id_status_aprovacao=" + item.Id.ToString() + ")";
                //    }
                //    quantidadeIfs++;
                //}
                //strsqlcommand = strsqlcommand + ",IF(SUM(IF(itpr.id_status_aprovacao=1,1,0))>0,(SELECT nome FROM tb_status_aprovacao WHERE id_status_aprovacao=2),'Diversos'";

                //for (int i = 1; i < quantidadeIfs + 1; i++)
                //{
                //    strsqlcommand = strsqlcommand + ")";
                //}
                //strsqlcommand = strsqlcommand + ")";

                string textoLimite = "";

                if (App.Usuario.LimiteResultados != null)
                {
                    textoLimite = " LIMIT " + App.Usuario.LimiteResultados.ToString();
                }

                string comandoInicial = "SELECT prop.data_insercao AS DataInsercao, prop_usua_ins.nome AS NomeUsuario, itpr.id_item_proposta AS IdItemProposta, prop.id_proposta AS IdProposta, prop.codigo_proposta AS CodigoProposta, " +
                                            "prop.data_solicitacao AS DataSolicitacao, clie.nome AS NomeCliente, cont.nome AS NomeContato, prop.data_envio AS DataEnvio, " +
                                            "COUNT(itpr.quantidade_item) AS QuantidadeTotal, SUM(CASE WHEN itpr.id_tipo_item = 1 THEN itpr.preco_total_final_item END) AS ValorPecas, " +
                                            "SUM(CASE WHEN itpr.id_tipo_item <> 1 THEN itpr.preco_total_final_item END) AS ValorServicos, " +
                                            "prop.data_aprovacao AS DataAprovacao, " +
                                            "prop_stap.nome AS NomeStatusAprovacao, " +
                                            "prop_juap.nome AS NomeJustificativaAprovacao, " +
                                            "SUM(itpr.preco_total_final_item) AS ValorTotal, seri.nome AS NomeSerie, " +
                                            "prop.valor_faturamento AS ValorFaturamento, prop.data_envio_faturamento AS DataEnvioFaturamento, prop.data_faturamento AS DataFaturamento, " +
                                            "prop.nota_fiscal AS NotaFiscal, prop.comentarios AS ComentariosProposta " +
                                            "FROM tb_propostas AS prop " +
                                            "LEFT JOIN tb_itens_propostas AS itpr ON itpr.id_proposta = prop.id_proposta " +
                                            "LEFT JOIN tb_usuarios AS itpr_usua_ins ON itpr.id_usuario = itpr_usua_ins.id_usuario " +
                                            "LEFT JOIN tb_usuarios AS prop_usua_ins ON prop.id_usuario_insercao = prop_usua_ins.id_usuario " +
                                            "LEFT JOIN tb_usuarios AS prop_usua_edi ON prop.id_usuario_edicao = prop_usua_edi.id_usuario " +
                                            "LEFT JOIN tb_setores AS seto_ins ON prop_usua_ins.id_setor = seto_ins.id_setor " +
                                            "LEFT JOIN tb_setores AS seto_edi ON prop_usua_edi.id_setor = seto_edi.id_setor " +
                                            "LEFT JOIN tb_setores AS itpr_seto_ins ON itpr_usua_ins.id_setor = itpr_seto_ins.id_setor " +
                                            "LEFT JOIN tb_status AS itpr_stat ON itpr.id_status = itpr_stat.id_status " +
                                            "LEFT JOIN tb_status AS prop_stat ON prop.id_status = prop_stat.id_status " +
                                            "LEFT JOIN tb_status_aprovacao AS prop_stap ON prop.id_status_aprovacao = prop_stap.id_status_aprovacao " +
                                            "LEFT JOIN tb_justificativas_aprovacao AS prop_juap ON prop.id_justificativa_aprovacao = prop_juap.id_justificativa_aprovacao " +
                                            "LEFT JOIN tb_status_aprovacao AS itpr_stap ON itpr.id_status_aprovacao = itpr_stap.id_status_aprovacao " +
                                            "LEFT JOIN tb_justificativas_aprovacao AS itpr_juap ON itpr.id_justificativa_aprovacao = itpr_juap.id_justificativa_aprovacao " +
                                            "LEFT JOIN tb_conjuntos AS conj ON itpr.id_conjunto = conj.id_conjunto " +
                                            "LEFT JOIN tb_especificacoes AS espe ON itpr.id_especificacao = espe.id_especificacao " +
                                            "LEFT JOIN tb_tipos_item AS tiit ON itpr.id_tipo_item = tiit.id_tipo_item " +
                                            "LEFT JOIN tb_fornecedores AS forn ON itpr.id_fornecedor = forn.id_fornecedor " +
                                            "LEFT JOIN tb_tipos_substituicao_tributaria AS tstr ON itpr.id_tipo_substituicao_tributaria_item = tstr.id_tipo_substituicao_tributaria " +
                                            "LEFT JOIN tb_origens AS orig ON itpr.id_origem_item = orig.id_origem " +
                                            "LEFT JOIN tb_filiais AS fili ON prop.id_filial = fili.id_filial " +
                                            "LEFT JOIN tb_clientes AS clie ON prop.id_cliente = clie.id_cliente " +
                                            "LEFT JOIN tb_contatos AS cont ON prop.id_contato = cont.id_contato " +
                                            "LEFT JOIN tb_prioridades AS prio ON prop.id_prioridade = prio.id_prioridade " +
                                            "LEFT JOIN tb_fabricantes AS fabr ON prop.id_fabricante = fabr.id_fabricante " +
                                            "LEFT JOIN tb_tipos_equipamento AS tieq ON prop.id_tipo_equipamento = tieq.id_tipo_equipamento " +
                                            "LEFT JOIN tb_modelos AS mode ON prop.id_modelo = mode.id_modelo " +
                                            "LEFT JOIN tb_anos AS ano ON prop.id_ano = ano.id_ano " +
                                            "LEFT JOIN tb_series AS seri ON prop.id_serie = seri.id_serie " +
                                            sCondicao + " GROUP BY itpr.id_proposta ORDER BY " +
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
                    await ResultadoPesquisaProposta.PreencheListaResultadosPesquisaPropostaAsync(ListaResultadosPesquisaProposta, true, progresso, _cts.Token, comandoInicial, String.Join(", ", listaNomeParametros.ToArray()), listaValores.ToArray());

                    if (App.Usuario.LimiteResultados == null)
                    {
                        TextoLimiteDeResultados = "*Sem limite de resultados. Caso a pesquisa esteja lenta, defina um limite na opção Perfil.";
                    }
                    else
                    {
                        TextoLimiteDeResultados = "*Limitado a " + App.Usuario.LimiteResultados.ToString() + " resultado (s). Você pode alterar isso na opção Perfil.";
                    }

                    TextoResultadosEncontrados = "Resultado (s) encontrado (s): " + ListaResultadosPesquisaProposta.Count;
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

        private async Task AbrirProposta()
        {
            if (ResultadoPesquisaPropostaSelecionado != null)
            {
                try
                {
                    if (!await Proposta.PropostaExiste(ResultadoPesquisaPropostaSelecionado.IdProposta, CancellationToken.None))
                    {
                        var mySettings = new MetroDialogSettings
                        {
                            AffirmativeButtonText = "Ok"
                        };

                        var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                                "Proposta inexistente", "A proposta selecionada foi excluída da database. Não será possível abri-la", MessageDialogStyle.Affirmative, mySettings);

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
                            "Falha na verificação", "Não foi possível verificar a existência da proposta. Não será possível abri-la", MessageDialogStyle.Affirmative, mySettings);

                    return;
                }

                Proposta proposta = new();

                proposta.Id = ResultadoPesquisaPropostaSelecionado.IdProposta;

                Messenger.Default.Send<Proposta>(proposta, "PropostaAdicionar");
            }
        }

        private async Task PreecheListaTextoRetornado()
        {
            try
            {
                // Define o comando
                string comando = "SELECT " + ParametroSelecionado.Valor + " AS Coluna FROM tb_itens_propostas AS itpr "
                + "LEFT JOIN tb_propostas AS prop ON itpr.id_proposta = prop.id_proposta "
                + "LEFT JOIN tb_usuarios AS itpr_usua_ins ON itpr.id_usuario = itpr_usua_ins.id_usuario "
                + "LEFT JOIN tb_usuarios AS prop_usua_ins ON prop.id_usuario_insercao = prop_usua_ins.id_usuario "
                + "LEFT JOIN tb_usuarios AS prop_usua_edi ON prop.id_usuario_edicao = prop_usua_edi.id_usuario "
                + "LEFT JOIN tb_setores AS seto_ins ON prop_usua_ins.id_setor = seto_ins.id_setor "
                + "LEFT JOIN tb_setores AS seto_edi ON prop_usua_edi.id_setor = seto_edi.id_setor "
                + "LEFT JOIN tb_setores AS itpr_seto_ins ON itpr_usua_ins.id_setor = itpr_seto_ins.id_setor "
                + "LEFT JOIN tb_status AS itpr_stat ON itpr.id_status = itpr_stat.id_status "
                + "LEFT JOIN tb_status AS prop_stat ON prop.id_status = prop_stat.id_status "
                + "LEFT JOIN tb_status_aprovacao AS prop_stap ON prop.id_status_aprovacao = prop_stap.id_status_aprovacao "
                + "LEFT JOIN tb_justificativas_aprovacao AS prop_juap ON prop.id_justificativa_aprovacao = prop_juap.id_justificativa_aprovacao "
                + "LEFT JOIN tb_status_aprovacao AS itpr_stap ON itpr.id_status_aprovacao = itpr_stap.id_status_aprovacao "
                + "LEFT JOIN tb_justificativas_aprovacao AS itpr_juap ON itpr.id_justificativa_aprovacao = itpr_juap.id_justificativa_aprovacao "
                + "LEFT JOIN tb_conjuntos AS conj ON itpr.id_conjunto = conj.id_conjunto "
                + "LEFT JOIN tb_especificacoes AS espe ON itpr.id_especificacao = espe.id_especificacao "
                + "LEFT JOIN tb_tipos_item AS tiit ON itpr.id_tipo_item = tiit.id_tipo_item "
                + "LEFT JOIN tb_fornecedores AS forn ON itpr.id_fornecedor = forn.id_fornecedor "
                + "LEFT JOIN tb_tipos_substituicao_tributaria AS tstr ON itpr.id_tipo_substituicao_tributaria_item = tstr.id_tipo_substituicao_tributaria "
                + "LEFT JOIN tb_origens AS orig ON itpr.id_origem_item = orig.id_origem "
                + "LEFT JOIN tb_filiais AS fili ON prop.id_filial = fili.id_filial "
                + "LEFT JOIN tb_clientes AS clie ON prop.id_cliente = clie.id_cliente "
                + "LEFT JOIN tb_contatos AS cont ON prop.id_contato = cont.id_contato "
                + "LEFT JOIN tb_prioridades AS prio ON prop.id_prioridade = prio.id_prioridade "
                + "LEFT JOIN tb_fabricantes AS fabr ON prop.id_fabricante = fabr.id_fabricante "
                + "LEFT JOIN tb_tipos_equipamento AS tieq ON prop.id_tipo_equipamento = tieq.id_tipo_equipamento "
                + "LEFT JOIN tb_modelos AS mode ON prop.id_modelo = mode.id_modelo "
                + "LEFT JOIN tb_anos AS ano ON prop.id_ano = ano.id_ano "
                + "LEFT JOIN tb_series AS seri ON prop.id_serie = seri.id_serie "
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
                FileName = "Pesquisa_Propostas_" + DateTime.Now.ToString("yyyyMMddhhmmss"),
                AddExtension = true
            };

            bool houveErro = false;

            try
            {
                var options = new ExcelExportingOptions();
                options.ExcelVersion = ExcelVersion.Excel2013;
                var excelEngine = DataGrid.ExportToExcel(DataGrid.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];
                workBook.Worksheets[0].Name = "pesquisa_propostas";
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

        private async Task VisualizarProposta()
        {
            if (ResultadoPesquisaPropostaSelecionado != null)
            {
                try
                {
                    if (!await Proposta.PropostaExiste(ResultadoPesquisaPropostaSelecionado.IdProposta, CancellationToken.None))
                    {
                        var mySettings = new MetroDialogSettings
                        {
                            AffirmativeButtonText = "Ok"
                        };

                        var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                                "Proposta inexistente", "A proposta selecionada foi excluída da database. Não será possível abri-la", MessageDialogStyle.Affirmative, mySettings);

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
                            "Falha na verificação", "Não foi possível verificar a existência da proposta. Não será possível abri-la", MessageDialogStyle.Affirmative, mySettings);

                    return;
                }

                Proposta proposta = new();

                await proposta.GetPropostaDatabaseAsync(ResultadoPesquisaPropostaSelecionado.IdProposta, CancellationToken.None, true, true, true);

                var visualizarPropostaViewModel = new VisualizarPropostaViewModel(DialogCoordinator.Instance, proposta);

                var win = new MahApps.Metro.Controls.MetroWindow();
                win.Height = 802;
                win.Width = 956;
                win.Content = visualizarPropostaViewModel;
                win.Title = "Visualização da proposta " + proposta.Cliente?.Nome + " - " + proposta.CodigoProposta;
                win.ShowDialogsOverTitleBar = false;
                win.Owner = App.Current.MainWindow;
                win.Show();
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
                    "WHERE id_usuario = @id_usuario AND id_modulo = @id_modulo", "@id_usuario,@id_modulo", App.Usuario.Id, 1);

                // Preenche as listas com as classes necessárias
                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelSetores, "SELECT seto.id_setor AS Id, seto.nome AS Nome FROM tb_setores AS seto LEFT JOIN tb_usuarios AS usua ON usua.id_setor = seto.id_setor INNER JOIN tb_propostas AS prop ON prop.id_usuario_insercao = usua.id_usuario GROUP BY seto.id_setor ORDER BY seto.nome");
                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelUsuariosInsercao, "SELECT usua.id_usuario AS Id, usua.nome AS Nome FROM tb_usuarios AS usua INNER JOIN tb_propostas AS prop ON prop.id_usuario_insercao = usua.id_usuario GROUP BY usua.id_usuario ORDER BY usua.nome");
                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelClientes, "SELECT clie.id_cliente AS Id, clie.nome AS Nome FROM tb_clientes AS clie INNER JOIN tb_propostas AS prop ON prop.id_cliente = clie.id_cliente GROUP BY clie.id_cliente ORDER BY clie.nome");
                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelStatusAprovacao, "SELECT stap.id_status_aprovacao AS Id, stap.nome AS Nome FROM tb_status_aprovacao AS stap INNER JOIN tb_propostas AS prop ON prop.id_status_aprovacao = stap.id_status_aprovacao GROUP BY stap.id_status_aprovacao ORDER BY stap.nome");
                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelJustificativaAprovacao, "SELECT juap.id_justificativa_aprovacao AS Id, juap.nome AS Nome FROM tb_justificativas_aprovacao AS juap INNER JOIN tb_propostas AS prop ON prop.id_justificativa_aprovacao = juap.id_justificativa_aprovacao GROUP BY juap.id_justificativa_aprovacao ORDER BY juap.nome");
                await ItemFiltro.PreencheListaItemFiltroAsync(ListaObjetoSelecionavelPrioridades, "SELECT prio.id_prioridade AS Id, prio.nome AS Nome FROM tb_prioridades AS prio INNER JOIN tb_propostas AS prop ON prop.id_prioridade = prio.id_prioridade GROUP BY prio.id_prioridade ORDER BY prio.nome");
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
                FiltroUsuarioCarregado.IdModulo = 1;

                FiltroUsuarioCarregado.ListaParametroFiltroPesquisaProposta.Clear();

                foreach (var item in ListaFiltros)
                {
                    FiltroUsuarioCarregado.ListaParametroFiltroPesquisaProposta.Add(item);
                }

                await FiltroUsuarioCarregado.SalvarFiltroUsuarioDatabaseAsync(CancellationToken.None);

                // Preenche as listas com as classes necessárias
                await FiltroUsuario.PreencheListaFiltroUsuarioAsync(ListaFiltroUsuario, true, null, CancellationToken.None,
                    "WHERE id_usuario = @id_usuario AND id_modulo = @id_modulo", "@id_usuario,@id_modulo", App.Usuario.Id, 1);

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
            ExecutarPesquisaAvancada(CancellationToken.None).Await();
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

        private void LimparFiltroDataSolicitacao()
        {
            DataDeSolicitacao = null;
            DataAteSolicitacao = null;
        }

        private void VerificaDataDeSolicitacao()
        {
            if (DataDeSolicitacao != null && DataAteSolicitacao != null)
            {
                if (DataDeSolicitacao > DataAteSolicitacao)
                {
                    DataDeSolicitacao = null;
                }
            }
        }

        private void VerificaDataAteSolicitacao()
        {
            if (DataDeSolicitacao != null && DataAteSolicitacao != null)
            {
                if (DataAteSolicitacao < DataDeSolicitacao)
                {
                    DataAteSolicitacao = null;
                }
            }
        }

        private void LimparFiltroDataEnvio()
        {
            DataDeEnvio = null;
            DataAteEnvio = null;
        }

        private void VerificaDataDeEnvio()
        {
            if (DataDeEnvio != null && DataAteEnvio != null)
            {
                if (DataDeEnvio > DataAteEnvio)
                {
                    DataDeEnvio = null;
                }
            }
        }

        private void VerificaDataAteEnvio()
        {
            if (DataDeEnvio != null && DataAteEnvio != null)
            {
                if (DataAteEnvio < DataDeEnvio)
                {
                    DataAteEnvio = null;
                }
            }
        }

        private void LimparFiltroDataFaturamento()
        {
            DataDeFaturamento = null;
            DataAteFaturamento = null;
        }

        private void VerificaDataDeFaturamento()
        {
            if (DataDeFaturamento != null && DataAteFaturamento != null)
            {
                if (DataDeFaturamento > DataAteFaturamento)
                {
                    DataDeFaturamento = null;
                }
            }
        }

        private void VerificaDataAteFaturamento()
        {
            if (DataDeFaturamento != null && DataAteFaturamento != null)
            {
                if (DataAteFaturamento < DataDeFaturamento)
                {
                    DataAteFaturamento = null;
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