using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
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
    public class ParametroSeriesViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private Serie _serieSelecionada = new();
        private Serie _serieAlterar = new();
        private CancellationTokenSource _cts;

        private Cliente _cliente;
        private Planta _planta;
        private Area _area;
        private Fabricante? _fabricante;
        private TipoEquipamento? _tipoEquipamento;
        private Modelo? _modelo;

        private bool _controlesHabilitados;
        private bool _listaHabilitada = true;
        private bool _progressoEhIndeterminavel = true;
        private bool _progressoVisivel = false;
        private double _valorProgresso = 0;
        private string _textoProgresso;
        private string _mensagemStatus;

        private bool _salvarVisivel = false;
        private bool _deletarVisivel;
        private bool _cancelarVisivel;

        private bool _permiteSalvar;
        private bool _permiteAdicionar;
        private bool _permiteEditar;
        private bool _permiteDeletar;
        private bool _permiteCancelar;

        private bool _carregamentoVisivel = true;

        private readonly IDialogCoordinator _dialogCoordinator;

        private ObservableCollection<Serie> _listaSeries = new();
        private ObservableCollection<Cliente> _listaClientes = new();
        private ObservableCollection<Planta> _listaPlantas = new();
        private ObservableCollection<Area> _listaAreas = new();
        private ObservableCollection<Frota> _listaFrotas = new();
        private ObservableCollection<Modelo> _listaModelos = new();
        private ObservableCollection<Fabricante> _listaFabricantes = new();
        private ObservableCollection<TipoEquipamento> _listaTiposEquipamentos = new();
        private ObservableCollection<Status> _listaStatus = new();
        private ObservableCollection<Ano> _listaAnos = new();

        private ICommand _comandoSalvar;
        private ICommand _comandoAdicionar;
        private ICommand _comandoEditar;
        private ICommand _comandoDeletar;
        private ICommand _comandoCancelar;
        private ICommand _comandoGetSerie;

        #endregion Campos

        #region Construtores

        public ParametroSeriesViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;

            DeletarVisivel = App.Usuario.Perfil.Id == 1;

            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Série";
            }
        }

        public string Icone
        {
            get
            {
                return "FormatListNumbered";
            }
        }

        public void LimparViewModel()
        {
            try
            {
                _serieSelecionada = null;
                _fabricante = null;
                _tipoEquipamento = null;
                _modelo = null;
                _cts = null;
                _listaSeries = null;
                _listaClientes = null;
                _listaModelos = null;
                _listaAnos = null;
                _listaFabricantes = null;
                _listaTiposEquipamentos = null;
                _listaStatus = null;
                _comandoSalvar = null;
                _comandoAdicionar = null;
                _comandoEditar = null;
                _comandoDeletar = null;
                _comandoCancelar = null;
            }
            catch (Exception)
            {
            }
        }

        public bool ExistemCamposVazios { private get; set; }

        public Serie SerieSelecionada
        {
            get { return _serieSelecionada; }
            set
            {
                _serieSelecionada = value;
                OnPropertyChanged(nameof(SerieSelecionada));
                if (SerieSelecionada == null)
                {
                    SerieAlterar = null;
                }
                else
                {
                    SerieAlterar = (Serie)SerieSelecionada.Clone();
                    PermiteEditar = SerieAlterar != null;
                    PermiteDeletar = SerieAlterar != null;
                    // Define valores
                    try
                    {
                        Cliente = ListaClientes.First(clie => clie.Id == SerieSelecionada.Cliente.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        Planta = ListaPlantas.First(plan => plan.Id == SerieSelecionada?.Frota?.Area?.Planta?.Id);
                        //Cliente = Proposta.Cliente;
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        Area = ListaAreas.First(area => area.Id == SerieSelecionada?.Frota?.Area?.Id);
                        //Cliente = Proposta.Cliente;
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        SerieAlterar.Frota = ListaFrotas.First(frot => frot.Id == SerieSelecionada?.Frota?.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        Fabricante = ListaFabricantes.First(fabr => fabr.Id == SerieSelecionada.Modelo.Fabricante.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        TipoEquipamento = ListaTiposEquipamentos.First(tieq => tieq.Id == SerieSelecionada.Modelo.TipoEquipamento.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        Modelo = ListaModelos.First(mode => mode.Id == SerieSelecionada.Modelo.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        SerieAlterar.Status = ListaStatus.First(stat => stat.Id == SerieSelecionada.Status.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        SerieAlterar.Ano = ListaAnos.First(anno => anno.Id == SerieSelecionada.Ano.Id);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public Serie SerieAlterar
        {
            get { return _serieAlterar; }
            set
            {
                _serieAlterar = value;
                OnPropertyChanged(nameof(SerieAlterar));
            }
        }

        public bool ClienteSelecionado => Convert.ToBoolean(Cliente != null);

        public Cliente Cliente
        {
            get { return _cliente; }
            set
            {
                if (value != _cliente)
                {
                    _cliente = value;
                    SerieAlterar.Cliente = value;
                    OnPropertyChanged(nameof(Cliente));

                    // Notifica o estado de seleção do cliente
                    OnPropertyChanged(nameof(ClienteSelecionado));

                    ListaPlantasView.Filter = FiltraPlantas;
                    CollectionViewSource.GetDefaultView(ListaPlantas).Refresh();

                    if (ListaPlantasView.Count == 1)
                    {
                        ListaPlantasView.MoveCurrentToFirst();
                        Planta = (Planta)ListaPlantasView.CurrentItem;
                    }
                }
            }
        }

        public Planta Planta
        {
            get { return _planta; }
            set
            {
                if (value != _planta)
                {
                    _planta = value;
                    OnPropertyChanged(nameof(Planta));

                    ListaAreasView.Filter = FiltraAreas;
                    CollectionViewSource.GetDefaultView(ListaAreas).Refresh();

                    if (ListaAreasView.Count == 1)
                    {
                        ListaAreasView.MoveCurrentToFirst();
                        Area = (Area)ListaAreasView.CurrentItem;
                    }
                }
            }
        }

        public Area Area
        {
            get { return _area; }
            set
            {
                if (value != _area)
                {
                    _area = value;
                    OnPropertyChanged(nameof(Area));

                    ListaFrotasView.Filter = FiltraFrotas;
                    CollectionViewSource.GetDefaultView(ListaFrotas).Refresh();

                    if (ListaFrotasView.Count == 1)
                    {
                        ListaFrotasView.MoveCurrentToFirst();
                        SerieAlterar.Frota = (Frota)ListaFrotasView.CurrentItem;
                    }
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

        public bool ListaHabilitada
        {
            get { return _listaHabilitada; }
            set
            {
                if (value != _listaHabilitada)
                {
                    _listaHabilitada = value;
                    OnPropertyChanged(nameof(ListaHabilitada));
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
                    OnPropertyChanged(nameof(ProgressoEhIndeterminavel));
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

        public bool PermiteAdicionar
        {
            get { return _permiteAdicionar; }
            set
            {
                if (value != _permiteAdicionar)
                {
                    _permiteAdicionar = value;
                    OnPropertyChanged(nameof(PermiteAdicionar));
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

        public double ValorProgresso
        {
            get { return _valorProgresso; }
            set
            {
                if (value != _valorProgresso)
                {
                    _valorProgresso = value;
                    if (!ProgressoEhIndeterminavel)
                    {
                        TextoProgresso = (value / 100).ToString("P1");
                    }
                    else
                    {
                        TextoProgresso = "";
                    }
                    OnPropertyChanged(nameof(ValorProgresso));
                }
            }
        }

        public string TextoProgresso
        {
            get { return _textoProgresso; }
            set
            {
                if (value != _textoProgresso)
                {
                    _textoProgresso = value;
                    OnPropertyChanged(nameof(TextoProgresso));
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

        public bool SalvarVisivel
        {
            get { return _salvarVisivel; }
            set
            {
                if (value != _salvarVisivel)
                {
                    _salvarVisivel = value;
                    OnPropertyChanged(nameof(SalvarVisivel));
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

        public ObservableCollection<Planta> ListaPlantas
        {
            get { return _listaPlantas; }
            set
            {
                if (value != _listaPlantas)
                {
                    _listaPlantas = value;
                    OnPropertyChanged(nameof(ListaPlantas));
                }
            }
        }

        public ObservableCollection<Area> ListaAreas
        {
            get { return _listaAreas; }
            set
            {
                if (value != _listaAreas)
                {
                    _listaAreas = value;
                    OnPropertyChanged(nameof(ListaAreas));
                }
            }
        }

        public ObservableCollection<Frota> ListaFrotas
        {
            get { return _listaFrotas; }
            set
            {
                if (value != _listaFrotas)
                {
                    _listaFrotas = value;
                    OnPropertyChanged(nameof(ListaFrotas));
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

        public ObservableCollection<TipoEquipamento> ListaTiposEquipamentos
        {
            get { return _listaTiposEquipamentos; }
            set
            {
                if (value != _listaTiposEquipamentos)
                {
                    _listaTiposEquipamentos = value;
                    OnPropertyChanged(nameof(ListaTiposEquipamentos));
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

        public ICommand ComandoSalvar
        {
            get
            {
                if (_comandoSalvar == null)
                {
                    _comandoSalvar = new RelayCommand(
                        param => SalvarAsync().Await(),
                        param => true
                    );
                }
                return _comandoSalvar;
            }
        }

        public ICommand ComandoAdicionar
        {
            get
            {
                if (_comandoAdicionar == null)
                {
                    _comandoAdicionar = new RelayCommand(
                        param => Adicionar(),
                        param => true
                    );
                }
                return _comandoAdicionar;
            }
        }

        public ICommand ComandoEditar
        {
            get
            {
                if (_comandoEditar == null)
                {
                    _comandoEditar = new RelayCommand(
                        param => Editar(),
                        param => true
                    );
                }
                return _comandoEditar;
            }
        }

        public ICommand ComandoDeletar
        {
            get
            {
                if (_comandoDeletar == null)
                {
                    _comandoDeletar = new RelayCommand(
                        param => Deletar().Await(),
                        param => true
                    );
                }
                return _comandoDeletar;
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

        #endregion Propriedades/Comandos

        #region Métodos

        public async Task ConstrutorAsync()
        {
            try
            {
                CarregamentoVisivel = true;

                // Preenche as listas com as classes necessárias
                await Serie.PreencheListaSeriesAsync(ListaSeries, true, null, CancellationToken.None, "ORDER BY seri.nome ASC", "");
                await Cliente.PreencheListaClientesAsync(ListaClientes, true, null, CancellationToken.None, "ORDER BY clie.nome ASC", "");
                await Planta.PreencheListaPlantasAsync(ListaPlantas, true, null, CancellationToken.None, "ORDER BY plan.nome ASC", "");
                await Area.PreencheListaAreasAsync(ListaAreas, true, null, CancellationToken.None, "ORDER BY area.nome ASC", "");
                await Frota.PreencheListaFrotasAsync(ListaFrotas, true, null, CancellationToken.None, "ORDER BY frot.nome ASC", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");
                await Modelo.PreencheListaModelosAsync(ListaModelos, true, null, CancellationToken.None, "ORDER BY mode.nome ASC", "");
                await Fabricante.PreencheListaFabricantesAsync(ListaFabricantes, true, null, CancellationToken.None, "ORDER BY fabr.nome ASC", "");
                await TipoEquipamento.PreencheListaTiposEquipamentoAsync(ListaTiposEquipamentos, true, null, CancellationToken.None, "ORDER BY tieq.nome ASC", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");
                await Ano.PreencheListaAnosAsync(ListaAnos, true, null, CancellationToken.None, "ORDER BY t_ano.ano ASC", "");

                ListaPlantasView = GetPlantaCollectionView(ListaPlantas);
                ListaAreasView = GetAreaCollectionView(ListaAreas);
                ListaFrotasView = GetFrotaCollectionView(ListaFrotas);
                ListaModelosView = GetModeloCollectionView(ListaModelos);

                ListaPlantasView.Filter = LimpaListaPlantas;
                CollectionViewSource.GetDefaultView(ListaPlantas).Refresh();

                ListaAreasView.Filter = LimpaListaAreas;
                CollectionViewSource.GetDefaultView(ListaAreas).Refresh();

                ListaFrotasView.Filter = LimpaListaFrotas;
                CollectionViewSource.GetDefaultView(ListaFrotas).Refresh();

                ListaModelosView.Filter = LimpaListaModelos;
                CollectionViewSource.GetDefaultView(ListaModelos).Refresh();

                // Redefine as permissões
                PermiteSalvar = false;
                PermiteAdicionar = true;
                PermiteEditar = false;
                PermiteDeletar = false;
                PermiteCancelar = false;
                CancelarVisivel = false;
                SalvarVisivel = false;
                ListaHabilitada = true;
                ControlesHabilitados = false;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");

                MensagemStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                PermiteSalvar = false;
                PermiteAdicionar = false;
                PermiteEditar = false;
                PermiteDeletar = false;
                PermiteCancelar = false;
                CancelarVisivel = false;
                SalvarVisivel = false;
                ListaHabilitada = false;
                ControlesHabilitados = false;
            }

            CarregamentoVisivel = false;
        }

        private void Adicionar()
        {
            SerieAlterar = new Serie();
            Cliente = null;
            Planta = null;
            Area = null;
            SerieAlterar.Frota = null;
            SerieAlterar.Status = null;
            SerieAlterar.Ano = null;
            Fabricante = null;
            TipoEquipamento = null;
            Modelo = null;

            ControlesHabilitados = true;
            ListaHabilitada = false;
            SalvarVisivel = true;
            CancelarVisivel = true;
            PermiteAdicionar = false;
            PermiteEditar = false;
            PermiteCancelar = true;
            PermiteSalvar = true;
            PermiteDeletar = false;
        }

        private void Editar()
        {
            ControlesHabilitados = true;
            ListaHabilitada = false;
            SalvarVisivel = true;
            CancelarVisivel = true;
            PermiteAdicionar = false;
            PermiteEditar = false;
            PermiteCancelar = true;
            PermiteSalvar = true;
            PermiteDeletar = false;
        }

        private void Cancelar()
        {
            if (_cts != null)
                _cts.Cancel();

            ControlesHabilitados = false;
            ListaHabilitada = true;
            SalvarVisivel = false;
            CancelarVisivel = false;
            PermiteAdicionar = true;
            PermiteEditar = SerieAlterar != null && SerieAlterar?.Id != null; ;
            PermiteCancelar = false;
            PermiteSalvar = false;
            PermiteDeletar = SerieAlterar != null && SerieAlterar?.Id != null; ;
        }

        private async Task SalvarAsync()
        {
            if (ExistemCamposVazios)
            {
                MensagemStatus = "Campos obrigatórios vazios/inválidos";
                return;
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(false, "SelecaoParametrosHabilitado");

            _cts = new();

            ValorProgresso = 0;
            ListaHabilitada = false;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = true;
            MensagemStatus = "Salvando dados da série '" + SerieAlterar.Nome + "', aguarde...";
            CancelarVisivel = true;
            PermiteCancelar = true;
            PermiteSalvar = false;
            PermiteAdicionar = false;
            PermiteEditar = false;
            PermiteDeletar = false;

            try
            {
                await Task.Delay(1000, _cts.Token);
            }
            catch (Exception)
            {
                ValorProgresso = 0;
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = true;
                MensagemStatus = "Operação cancelada";
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
                return;
            }

            SerieAlterar.Modelo = Modelo;

            try
            {
                await SerieAlterar.SalvarSerieDatabaseAsync(_cts.Token);
            }
            catch (Exception ex)
            {
                ValorProgresso = 0;
                ControlesHabilitados = true;
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = true;

                ListaHabilitada = false;
                SalvarVisivel = true;
                CancelarVisivel = true;
                PermiteAdicionar = false;
                PermiteEditar = false;
                PermiteCancelar = true;
                PermiteSalvar = true;
                PermiteDeletar = false;

                if (ex is OperationCanceledException)
                {
                    MensagemStatus = "Operação cancelada";
                }
                else
                {
                    if (ex is ValorJaExisteException || ex is ValorNaoExisteException)
                    {
                        MensagemStatus = ex.Message;
                    }
                    else
                    {
                        Serilog.Log.Error(ex, "Erro ao salvar dados");
                        MensagemStatus = "Falha ao salvar os dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                    }
                }

                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
                return;
            }

            int? idSerieAlterado = SerieAlterar.Id;

            try
            {
                await Serie.PreencheListaSeriesAsync(ListaSeries, true, null, CancellationToken.None, "ORDER BY seri.nome ASC", "");
            }
            catch (Exception)
            {
            }

            try
            {
                SerieSelecionada = ListaSeries.First(fami => fami.Id == idSerieAlterado);
            }
            catch (Exception)
            {
            }

            ValorProgresso = 0;
            ListaHabilitada = true;
            ControlesHabilitados = false;
            ProgressoVisivel = false;
            ProgressoEhIndeterminavel = false;
            SalvarVisivel = false;
            CancelarVisivel = false;
            PermiteCancelar = false;
            PermiteSalvar = false;
            PermiteAdicionar = true;
            PermiteEditar = true;
            PermiteDeletar = true;
            MensagemStatus = "Dados salvos com sucesso!";
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
        }

        private async Task Deletar()
        {
            // Questiona ao usuário se deseja mesmo excluir a proposta
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Sim",
                NegativeButtonText = "Não"
            };

            var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                "Atenção", "Tem certeza que deseja excluir a série '" + SerieAlterar.Nome + "'? " +
                "O processo não poderá ser desfeito.",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem != MessageDialogResult.Affirmative)
            {
                return;
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(false, "SelecaoParametrosHabilitado");

            _cts = new();

            ValorProgresso = 0;
            ListaHabilitada = false;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = true;
            MensagemStatus = "Deletando série '" + SerieAlterar.Nome + "', aguarde...";
            CancelarVisivel = true;
            PermiteCancelar = true;
            PermiteSalvar = false;
            PermiteAdicionar = false;
            PermiteEditar = false;
            PermiteDeletar = false;

            try
            {
                await Task.Delay(3000, _cts.Token);
            }
            catch (Exception)
            {
                ValorProgresso = 0;
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = true;
                MensagemStatus = "Operação cancelada";
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
                return;
            }

            try
            {
                await SerieAlterar.DeletarSerieDatabaseAsync(_cts.Token);
            }
            catch (Exception ex)
            {
                ValorProgresso = 0;
                ListaHabilitada = true;
                ControlesHabilitados = false;
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = true;

                CancelarVisivel = false;
                PermiteCancelar = false;
                PermiteSalvar = false;
                PermiteAdicionar = true;
                PermiteEditar = true;
                PermiteDeletar = true;

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
                        MensagemStatus = "Falha ao excluir os dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                    }
                }

                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
                return;
            }

            SerieSelecionada = null;
            ValorProgresso = 0;
            ListaHabilitada = true;
            ControlesHabilitados = false;
            ProgressoVisivel = false;
            ProgressoEhIndeterminavel = false;
            SalvarVisivel = false;
            CancelarVisivel = false;
            PermiteCancelar = false;
            PermiteSalvar = false;
            PermiteAdicionar = true;
            PermiteEditar = false;
            PermiteDeletar = false;
            MensagemStatus = "Série excluída com sucesso!";

            try
            {
                await Serie.PreencheListaSeriesAsync(ListaSeries, true, null, CancellationToken.None, "ORDER BY seri.nome ASC", "");
            }
            catch (Exception)
            {
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
        }

        /// <summary>
        /// Método para retornar os dados do contato, caso ele exista
        /// </summary>
        private async Task GetSerieAsync()
        {
            bool familiaExiste = false;

            if (!String.IsNullOrEmpty(SerieAlterar.Nome))
            {
                try
                {
                    Familia familia = new();
                    await familia.GetFamiliaDatabaseAsync(SerieAlterar.Nome, CancellationToken.None);

                    if (familia.Id != null)
                    {
                        Fabricante = ListaFabricantes.First(fabr => fabr.Id == familia.Modelo?.Fabricante?.Id);
                        TipoEquipamento = ListaTiposEquipamentos.First(tieq => tieq.Id == familia.Modelo?.TipoEquipamento?.Id);
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
                string? caracteres = SerieAlterar.Nome.Substring((int)(posicaoInicioCaracters == null ? 0 : posicaoInicioCaracters) - 1, 1);

                ObservableCollection<Ano> listaAnosTemporaria = new();

                await Ano.PreencheListaAnosAsync(listaAnosTemporaria, true, null, CancellationToken.None,
                "WHERE t_ano.id_status = 1 AND fabr.id_fabricante = @id_fabricante AND ano <= @ano AND posicao_inicio_caracteres = @posicao_inicio_caracteres AND caracteres = @caracteres",
                "@id_fabricante, @ano, @posicao_inicio_caracteres, @caracteres", Fabricante.Id, DateTime.Now.Year + 1, posicaoInicioCaracters, caracteres);

                // Se a lista retornada for maior que 0 significa que existem anos com os critérios informados, portanto preenche a lista de anos através dessa lista, caso contrário preenche a lista completa
                if (listaAnosTemporaria.Count > 0 && familiaExiste)
                {
                    ListaAnos = listaAnosTemporaria;
                    SerieAlterar.Ano = ListaAnos.Last();
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

        public bool LimpaListaModelos(object item)
        {
            return false;
        }

        #endregion Métodos

        private CollectionView _listaPlantasView;
        private CollectionView _listaAreasView;
        private CollectionView _listaFrotasView;
        private CollectionView _listaModelosView;

        public CollectionView ListaPlantasView
        {
            get { return _listaPlantasView; }
            set
            {
                if (_listaPlantasView != value)
                {
                    _listaPlantasView = value;
                    OnPropertyChanged(nameof(ListaPlantasView));
                }
            }
        }

        public CollectionView ListaAreasView
        {
            get { return _listaAreasView; }
            set
            {
                if (_listaAreasView != value)
                {
                    _listaAreasView = value;
                    OnPropertyChanged(nameof(ListaAreasView));
                }
            }
        }

        public CollectionView ListaFrotasView
        {
            get { return _listaFrotasView; }
            set
            {
                if (_listaFrotasView != value)
                {
                    _listaFrotasView = value;
                    OnPropertyChanged(nameof(ListaFrotasView));
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

        //public CollectionView ListaModelosView { get; private set; }
        //public CollectionView ListaPlantasView { get; private set; }
        //public CollectionView ListaAreasView { get; private set; }
        //public CollectionView ListaFrotasView { get; private set; }

        public CollectionView GetPlantaCollectionView(ObservableCollection<Planta> plantaList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(plantaList);
        }

        public CollectionView GetAreaCollectionView(ObservableCollection<Area> areaList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(areaList);
        }

        public CollectionView GetFrotaCollectionView(ObservableCollection<Frota> frotaList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(frotaList);
        }

        public bool FiltraPlantas(object item)
        {
            if (Cliente == null)
            {
                return false;
            }
            if (item is Planta planta)
            {
                return planta.Cliente.Id == Cliente.Id;
            }
            return true;
        }

        public bool FiltraAreas(object item)
        {
            if (Planta == null)
            {
                return false;
            }
            if (item is Area area)
            {
                return area.Planta.Id == Planta.Id;
            }
            return true;
        }

        public bool FiltraFrotas(object item)
        {
            if (Area == null)
            {
                return false;
            }
            if (item is Frota frota)
            {
                return frota.Area.Id == Area.Id;
            }
            return true;
        }

        public bool LimpaListaPlantas(object item)
        {
            return false;
        }

        public bool LimpaListaAreas(object item)
        {
            return false;
        }

        public bool LimpaListaFrotas(object item)
        {
            return false;
        }

        #region Classes

        public CollectionView GetModeloCollectionView(ObservableCollection<Modelo> modeloList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(modeloList);
        }

        #endregion Classes
    }
}