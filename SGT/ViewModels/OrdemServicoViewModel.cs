using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.HelperClasses;
using Ookii.Dialogs.Wpf;
using SGT.HelperClasses;
using SGT.Views;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class OrdemServicoViewModel : ObservableObject, IPageViewModel, INotifyDataErrorInfo
    {
        #region Campos

        private string _name;
        private readonly IDialogCoordinator _dialogCoordinator;
        private ObservableCollection<Cliente> _listaClientes = new();
        private ObservableCollection<Planta> _listaPlantas = new();
        private ObservableCollection<Area> _listaAreas = new();
        private ObservableCollection<Frota> _listaFrotas = new();

        private ObservableCollection<Serie> _listaSeries = new();
        private ObservableCollection<Fabricante> _listaFabricantes = new();
        private ObservableCollection<Categoria> _listaCategorias = new();
        private ObservableCollection<TipoEquipamento> _listaTiposEquipamento = new();
        private ObservableCollection<Classe> _listaClasses = new();
        private ObservableCollection<Modelo> _listaModelos = new();
        private ObservableCollection<Ano> _listaAnos = new();
        private ObservableCollection<Status> _listaStatus = new();
        private ObservableCollection<Conjunto> _listaConjunto = new();
        private ObservableCollection<Especificacao> _listaEspecificacao = new();

        private ObservableCollection<TipoOrdemServico> _listaTiposOrdemServico = new();
        private ObservableCollection<StatusEquipamentoAposManutencao> _listaStatusEquipamentoAposManutencao = new();
        private ObservableCollection<TipoManutencao> _listaTipoManutencao = new();
        private ObservableCollection<UsoIndevido> _listaUsoIndevido = new();
        private ObservableCollection<ExecutanteServico> _listaExecutanteServico = new();

        private ConfiguracaoSistema _configuracaoSistema;

        private bool ehInsercao;

        private OrdemServico _ordemServico;
        private ItemOrdemServico _itemOrdemServicoSelecionado;
        private ObservableCollection<ItemOrdemServico> _listaItensOrdemServicoSelecionados = new();
        private EventoOrdemServico _eventoOrdemServicoSelecionado;
        private ObservableCollection<EventoOrdemServico> _listaEventosOrdemServicoSelecionados = new();
        private InconsistenciaOrdemServico _inconsistenciaOrdemServicoSelecionada;
        private ObservableCollection<InconsistenciaOrdemServico> _listaInconsistenciasOrdemServicoSelecionados = new();
        private Cliente _cliente;
        private Planta _planta;
        private Area _area;
        private Fabricante? _fabricante;
        private Categoria? _categoria;
        private TipoEquipamento? _tipoEquipamento;
        private Classe? _classe;
        private Modelo? _modelo;
        private TipoManutencao? _tipoManutencao;
        private Conjunto _conjuntoUnico;
        private Conjunto _conjuntoTodos;
        private Especificacao _especificacaoUnico;
        private Especificacao _especificacaoTodos;

        private CancellationTokenSource _cts;
        private double _valorProgresso;
        private bool _progressoEhIndeterminavel;
        private bool _progressoVisivel = false;
        private string _mensagemStatus;

        private bool _usoIndevidoVisivel;
        private bool _horasPreventivaVisivel;
        private bool _outroVisivel;

        private bool _deletarVisivel;
        private bool _cancelarEdicaoVisivel;
        private bool _cancelarVisivel;
        private bool _edicaoHabilitada;
        private bool _controlesHabilitados;
        private bool _carregamentoVisivel = true;

        private bool _permiteSalvar;
        private bool _permiteEditar;
        private bool _permiteDeletar;
        private bool _permiteVisualizar;
        private bool _permiteCancelarEdicao;
        private bool _permiteCancelar;

        private int? _ordemServicoAtual;
        private int? _ordemServicoPrimaria;

        private bool _arquivoOrdemServicoAtualInexistente;
        private bool _arquivoOrdemServicoPrimariaInexistente;

        private bool _ehCarregamento;
        private int _ehConstrutor = 0;

        private bool removeuItemExistente = false;
        private bool removeuEventoExistente = false;
        private bool removeuInconsistenciaExistente = false;

        private string _textoSalvar = "Salvar e avançar";
        private string _iconeSalvar = "ChevronRightCircle";

        private bool _ehCopia;
        private bool _edicaoEhEnterprise;
        private bool _permiteCopiar;

        private ICommand _comandoCopiarOrdemServicoNovaOrdemServico;
        private ICommand _comandoCopiarOrdemServicoNovaProposta;

        private ICommand _comandoVisualizarOrdemServicoAtual;
        private ICommand _comandoVisualizarOrdemServicoPrimaria;

        private ICommand _comandoVerificaDataSaida;
        private ICommand _comandoVerificaDataChegada;
        private ICommand _comandoVerificaDataRetorno;

        private ICommand _comandoVerificaOrdemServicoAtual;
        private ICommand _comandoVerificaOrdemServicoPrimaria;

        private ICommand _comandoGetFrota;
        private ICommand _comandoGetSerie;

        private ICommand _comandoAdicionarItem;
        private ICommand _comandoEditarItem;
        private ICommand _comandoRemoverItem;
        private ICommand _comandoExportarFormatoFluig;
        private ICommand _comandoExportarTabelaCompleta;

        private ICommand _comandoAdicionarEvento;
        private ICommand _comandoEditarEvento;
        private ICommand _comandoRemoverEvento;
        private ICommand _comandoExportarEvento;

        private ICommand _comandoAdicionarInconsistencia;
        private ICommand _comandoEditarInconsistencia;
        private ICommand _comandoRemoverInconsistencia;
        private ICommand _comandoExportarInconsistencia;

        private ICommand _comandoAlteraConjuntoUnico;
        private ICommand _comandoAlteraConjuntoTodos;
        private ICommand _comandoAlteraEspecificacaoUnico;
        private ICommand _comandoAlteraEspecificacaoTodos;

        private ICommand _comandoSalvar;
        private ICommand _comandoEditar;
        private ICommand _comandoDeletar;
        private ICommand _comandoCancelarEdicao;
        private ICommand _comandoCancelar;
        private ICommand _comandoVisualizar;

        #endregion Campos

        public OrdemServicoViewModel(IDialogCoordinator dialogCoordinator, OrdemServico? ordemServico = null, bool ehCopia = false)
        {
            Name = "Carregando...";

            _dialogCoordinator = dialogCoordinator;

            _ehCopia = ehCopia;

            EdicaoEhEnterprise = App.EdicaoEhEnterprise;

            // Executa o método para preencher as listas
            ConstrutorAsync(ordemServico).Await();
        }

        public void LimparViewModel()
        {
            try
            {
                if (EhMovimentacao)
                {
                    EhMovimentacao = false;
                    return;
                }

                if (OrdemServico.Id != null && OrdemServico.IdUsuarioEmUso != null)
                {
                    OrdemServico.IdUsuarioEmUso = null;

                    OrdemServico.AtualizaIdUsuarioEmUsoAsync(CancellationToken.None).Await();
                }

                _listaClientes = null;
                _listaPlantas = null;
                _listaAreas = null;
                _listaFrotas = null;
                _listaSeries = null;
                _listaFabricantes = null;
                _listaCategorias = null;
                _listaTiposEquipamento = null;
                _listaClasses = null;
                _listaModelos = null;
                _listaAnos = null;
                _listaStatus = null;
                _listaTiposOrdemServico = null;
                _listaStatusEquipamentoAposManutencao = null;
                _listaTipoManutencao = null;
                _listaUsoIndevido = null;
                _listaExecutanteServico = null;
                _configuracaoSistema = null;
                _ordemServico = null;
                _itemOrdemServicoSelecionado = null;
                _eventoOrdemServicoSelecionado = null;
                _inconsistenciaOrdemServicoSelecionada = null;
                _cliente = null;
                _planta = null;
                _area = null;
                _fabricante = null;
                _categoria = null;
                _tipoEquipamento = null;
                _classe = null;
                _modelo = null;
                _tipoManutencao = null;

                _comandoCopiarOrdemServicoNovaOrdemServico = null;
                _comandoCopiarOrdemServicoNovaProposta = null;

                _comandoVisualizarOrdemServicoAtual = null;
                _comandoVisualizarOrdemServicoPrimaria = null;

                _comandoVerificaDataSaida = null;
                _comandoVerificaDataChegada = null;
                _comandoVerificaDataRetorno = null;

                _comandoVerificaOrdemServicoAtual = null;
                _comandoVerificaOrdemServicoPrimaria = null;

                _comandoGetFrota = null;
                _comandoGetSerie = null;

                _comandoAdicionarItem = null;
                _comandoEditarItem = null;
                _comandoRemoverItem = null;
                _comandoExportarFormatoFluig = null;
                _comandoExportarTabelaCompleta = null;

                _comandoAdicionarEvento = null;
                _comandoEditarEvento = null;
                _comandoRemoverEvento = null;
                _comandoExportarEvento = null;

                _comandoAdicionarInconsistencia = null;
                _comandoEditarInconsistencia = null;
                _comandoRemoverInconsistencia = null;
                _comandoExportarInconsistencia = null;

                _comandoSalvar = null;
                _comandoEditar = null;
                _comandoDeletar = null;
                _comandoCancelarEdicao = null;
                _comandoCancelar = null;
                _comandoVisualizar = null;
            }
            catch (Exception)
            {
            }
        }

        #region Propriedades/Comandos

        public SfDataGrid DataGrid { get; set; }
        public SfDataGrid DataGridEventos { get; set; }
        public SfDataGrid DataGridInconsistencias { get; set; }

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

        public bool ExistemCamposVazios { private get; set; }
        public bool EhMovimentacao { private get; set; }

        public OrdemServico OrdemServico
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

        public ItemOrdemServico ItemOrdemServicoSelecionado
        {
            get { return _itemOrdemServicoSelecionado; }
            set
            {
                if (value != _itemOrdemServicoSelecionado)
                {
                    _itemOrdemServicoSelecionado = value;
                    OnPropertyChanged(nameof(ItemOrdemServicoSelecionado));
                }
            }
        }

        public ObservableCollection<ItemOrdemServico> ListaItensOrdemServicoSelecionados
        {
            get { return _listaItensOrdemServicoSelecionados; }
            set
            {
                if (value != _listaItensOrdemServicoSelecionados)
                {
                    _listaItensOrdemServicoSelecionados = value;
                    OnPropertyChanged(nameof(ListaItensOrdemServicoSelecionados));
                }
            }
        }

        public EventoOrdemServico EventoOrdemServicoSelecionado
        {
            get { return _eventoOrdemServicoSelecionado; }
            set
            {
                if (value != _eventoOrdemServicoSelecionado)
                {
                    _eventoOrdemServicoSelecionado = value;
                    OnPropertyChanged(nameof(EventoOrdemServicoSelecionado));
                }
            }
        }

        public ObservableCollection<EventoOrdemServico> ListaEventosOrdemServicoSelecionados
        {
            get { return _listaEventosOrdemServicoSelecionados; }
            set
            {
                if (value != _listaEventosOrdemServicoSelecionados)
                {
                    _listaEventosOrdemServicoSelecionados = value;
                    OnPropertyChanged(nameof(ListaEventosOrdemServicoSelecionados));
                }
            }
        }

        public InconsistenciaOrdemServico InconsistenciaOrdemServicoSelecionada
        {
            get { return _inconsistenciaOrdemServicoSelecionada; }
            set
            {
                if (value != _inconsistenciaOrdemServicoSelecionada)
                {
                    _inconsistenciaOrdemServicoSelecionada = value;
                    OnPropertyChanged(nameof(InconsistenciaOrdemServicoSelecionada));
                }
            }
        }

        public ObservableCollection<InconsistenciaOrdemServico> ListaInconsistenciasOrdemServicoSelecionados
        {
            get { return _listaInconsistenciasOrdemServicoSelecionados; }
            set
            {
                if (value != _listaInconsistenciasOrdemServicoSelecionados)
                {
                    _listaInconsistenciasOrdemServicoSelecionados = value;
                    OnPropertyChanged(nameof(ListaInconsistenciasOrdemServicoSelecionados));
                }
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
                    OrdemServico.Cliente = value;
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
                        OrdemServico.Frota = (Frota)ListaFrotasView.CurrentItem;
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

        public TipoManutencao? TipoManutencao
        {
            get { return _tipoManutencao; }
            set
            {
                if (value != _tipoManutencao)
                {
                    _tipoManutencao = value;
                    OrdemServico.TipoManutencao = TipoManutencao;
                    OnPropertyChanged(nameof(TipoManutencao));

                    if (TipoManutencao == null)
                    {
                        UsoIndevidoVisivel = false;
                        HorasPreventivaVisivel = false;
                        OutroVisivel = false;
                    }
                    else
                    {
                        switch (TipoManutencao.Id)
                        {
                            case 1:
                                UsoIndevidoVisivel = false;
                                HorasPreventivaVisivel = true;
                                OutroVisivel = false;
                                break;

                            case 5:
                                UsoIndevidoVisivel = true;
                                HorasPreventivaVisivel = false;
                                OutroVisivel = false;
                                break;

                            case 12:
                                UsoIndevidoVisivel = false;
                                HorasPreventivaVisivel = false;
                                OutroVisivel = true;
                                break;

                            default:
                                UsoIndevidoVisivel = false;
                                HorasPreventivaVisivel = false;
                                OutroVisivel = false;
                                break;
                        }
                    }
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

        public bool UsoIndevidoVisivel
        {
            get { return _usoIndevidoVisivel; }
            set
            {
                if (value != _usoIndevidoVisivel)
                {
                    _usoIndevidoVisivel = value;
                    OnPropertyChanged(nameof(UsoIndevidoVisivel));
                }
            }
        }

        public bool HorasPreventivaVisivel
        {
            get { return _horasPreventivaVisivel; }
            set
            {
                if (value != _horasPreventivaVisivel)
                {
                    _horasPreventivaVisivel = value;
                    OnPropertyChanged(nameof(HorasPreventivaVisivel));
                }
            }
        }

        public bool OutroVisivel
        {
            get { return _outroVisivel; }
            set
            {
                if (value != _outroVisivel)
                {
                    _outroVisivel = value;
                    OnPropertyChanged(nameof(OutroVisivel));
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

        public int? OrdemServicoAtual
        {
            get { return _ordemServicoAtual; }
            set
            {
                if (value != _ordemServicoAtual)
                {
                    _ordemServicoAtual = value;
                    OnPropertyChanged(nameof(OrdemServicoAtual));

                    ArquivoOrdemServicoAtualInexistente = false;

                    if (OrdemServicoAtual != null)
                    {
                        ArquivoOrdemServicoAtualInexistente = !ArquivoOrdemServicoExiste(OrdemServicoAtual);
                        if (OrdemServico.Id == null)
                        {
                            VerificaExistenciaOrdemServicoAtual().Await();
                        }
                        else
                        {
                            if (OrdemServicoAtual != OrdemServico.OrdemServicoAtual)
                            {
                                VerificaExistenciaOrdemServicoAtual().Await();
                            }
                        }
                    }
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

                    ArquivoOrdemServicoPrimariaInexistente = false;

                    if (OrdemServicoPrimaria != null)
                    {
                        ArquivoOrdemServicoPrimariaInexistente = !ArquivoOrdemServicoExiste(OrdemServicoPrimaria);
                        if (OrdemServico.Id == null)
                        {
                            VerificaExistenciaOrdemServicoPrimaria().Await();
                        }
                        else
                        {
                            if (OrdemServicoPrimaria != OrdemServico.OrdemServicoPrimaria)
                            {
                                VerificaExistenciaOrdemServicoPrimaria().Await();
                            }
                        }
                    }
                }
            }
        }

        public bool ArquivoOrdemServicoAtualInexistente
        {
            get { return _arquivoOrdemServicoAtualInexistente; }
            set
            {
                if (value != _arquivoOrdemServicoAtualInexistente)
                {
                    _arquivoOrdemServicoAtualInexistente = value;
                    OnPropertyChanged(nameof(ArquivoOrdemServicoAtualInexistente));
                }
            }
        }

        public bool ArquivoOrdemServicoPrimariaInexistente
        {
            get { return _arquivoOrdemServicoPrimariaInexistente; }
            set
            {
                if (value != _arquivoOrdemServicoPrimariaInexistente)
                {
                    _arquivoOrdemServicoPrimariaInexistente = value;
                    OnPropertyChanged(nameof(ArquivoOrdemServicoPrimariaInexistente));
                }
            }
        }

        public bool EhCarregamento
        {
            get { return _ehCarregamento; }
            set
            {
                if (value != _ehCarregamento)
                {
                    _ehCarregamento = value;
                    OnPropertyChanged(nameof(EhCarregamento));
                }
            }
        }

        public int EhConstrutor
        {
            get { return _ehConstrutor; }
            set
            {
                if (value != _ehConstrutor)
                {
                    _ehConstrutor = value;
                    OnPropertyChanged(nameof(EhConstrutor));
                }
            }
        }

        public string TextoSalvar
        {
            get { return _textoSalvar; }
            set
            {
                if (value != _textoSalvar)
                {
                    _textoSalvar = value;
                    OnPropertyChanged(nameof(TextoSalvar));
                }
            }
        }

        public string IconeSalvar
        {
            get { return _iconeSalvar; }
            set
            {
                if (value != _iconeSalvar)
                {
                    _iconeSalvar = value;
                    OnPropertyChanged(nameof(IconeSalvar));
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

        public ObservableCollection<TipoOrdemServico> ListaTiposOrdemServico
        {
            get { return _listaTiposOrdemServico; }
            set
            {
                if (value != _listaTiposOrdemServico)
                {
                    _listaTiposOrdemServico = value;
                    OnPropertyChanged(nameof(ListaTiposOrdemServico));
                }
            }
        }

        public ObservableCollection<StatusEquipamentoAposManutencao> ListaStatusEquipamentoAposManutencao
        {
            get { return _listaStatusEquipamentoAposManutencao; }
            set
            {
                if (value != _listaStatusEquipamentoAposManutencao)
                {
                    _listaStatusEquipamentoAposManutencao = value;
                    OnPropertyChanged(nameof(ListaStatusEquipamentoAposManutencao));
                }
            }
        }

        public ObservableCollection<TipoManutencao> ListaTipoManutencao
        {
            get { return _listaTipoManutencao; }
            set
            {
                if (value != _listaTipoManutencao)
                {
                    _listaTipoManutencao = value;
                    OnPropertyChanged(nameof(ListaTipoManutencao));
                }
            }
        }

        public ObservableCollection<UsoIndevido> ListaUsoIndevido
        {
            get { return _listaUsoIndevido; }
            set
            {
                if (value != _listaUsoIndevido)
                {
                    _listaUsoIndevido = value;
                    OnPropertyChanged(nameof(ListaUsoIndevido));
                }
            }
        }

        public ObservableCollection<ExecutanteServico> ListaExecutanteServico
        {
            get { return _listaExecutanteServico; }
            set
            {
                if (value != _listaExecutanteServico)
                {
                    _listaExecutanteServico = value;
                    OnPropertyChanged(nameof(ListaExecutanteServico));
                }
            }
        }

        private CollectionView _listaPlantasView;
        private CollectionView _listaAreasView;
        private CollectionView _listaFrotasView;
        private CollectionView _listaSeriesView;
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

        //public CollectionView ListaPlantasView { get; private set; }
        //public CollectionView ListaAreasView { get; private set; }
        //public CollectionView ListaFrotasView { get; private set; }
        //public CollectionView ListaSeriesView { get; private set; }
        //public CollectionView ListaModelosView { get; private set; }

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

        public CollectionView GetSerieCollectionView(ObservableCollection<Serie> serieList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(serieList);
        }

        public CollectionView GetModeloCollectionView(ObservableCollection<Modelo> modeloList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(modeloList);
        }

        public ICommand ComandoCopiarOrdemServicoNovaOrdemServico
        {
            get
            {
                if (_comandoCopiarOrdemServicoNovaOrdemServico == null)
                {
                    _comandoCopiarOrdemServicoNovaOrdemServico = new RelayCommand(
                        param => CopiarOrdemServico(CopiarOrdemServicoViewModel.TipoCopia.ParaOrdemServico).Await(),
                        param => true
                    );
                }
                return _comandoCopiarOrdemServicoNovaOrdemServico;
            }
        }

        public ICommand ComandoCopiarOrdemServicoNovaProposta
        {
            get
            {
                if (_comandoCopiarOrdemServicoNovaProposta == null)
                {
                    _comandoCopiarOrdemServicoNovaProposta = new RelayCommand(
                        param => CopiarOrdemServico(CopiarOrdemServicoViewModel.TipoCopia.ParaProposta).Await(),
                        param => true
                    );
                }
                return _comandoCopiarOrdemServicoNovaProposta;
            }
        }

        public ICommand ComandoVisualizarOrdemServicoAtual
        {
            get
            {
                if (_comandoVisualizarOrdemServicoAtual == null)
                {
                    _comandoVisualizarOrdemServicoAtual = new RelayCommand(
                        param => AbrirOrdemServico(OrdemServicoAtual).Await(),
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
                        param => AbrirOrdemServico(OrdemServicoPrimaria).Await(),
                        param => true
                    );
                }
                return _comandoVisualizarOrdemServicoPrimaria;
            }
        }

        public ICommand ComandoVerificaDataSaida
        {
            get
            {
                if (_comandoVerificaDataSaida == null)
                {
                    _comandoVerificaDataSaida = new RelayCommand(
                        param => VerificaDataSaida(),
                        param => true
                    );
                }
                return _comandoVerificaDataSaida;
            }
        }

        public ICommand ComandoVerificaDataChegada
        {
            get
            {
                if (_comandoVerificaDataChegada == null)
                {
                    _comandoVerificaDataChegada = new RelayCommand(
                        param => VerificaDataChegada(),
                        param => true
                    );
                }
                return _comandoVerificaDataChegada;
            }
        }

        public ICommand ComandoVerificaDataRetorno
        {
            get
            {
                if (_comandoVerificaDataRetorno == null)
                {
                    _comandoVerificaDataRetorno = new RelayCommand(
                        param => VerificaDataRetorno(),
                        param => true
                    );
                }
                return _comandoVerificaDataRetorno;
            }
        }

        public ICommand ComandoVerificaOrdemServicoAtual
        {
            get
            {
                if (_comandoVerificaOrdemServicoAtual == null)
                {
                    _comandoVerificaOrdemServicoAtual = new RelayCommand(
                        param => VerificaExistenciaOrdemServicoAtual().Await(),
                        param => true
                    );
                }
                return _comandoVerificaOrdemServicoAtual;
            }
        }

        public ICommand ComandoVerificaOrdemServicoPrimaria
        {
            get
            {
                if (_comandoVerificaOrdemServicoPrimaria == null)
                {
                    _comandoVerificaOrdemServicoPrimaria = new RelayCommand(
                        param => VerificaExistenciaOrdemServicoPrimaria().Await(),
                        param => true
                    );
                }
                return _comandoVerificaOrdemServicoPrimaria;
            }
        }

        public ICommand ComandoGetFrota
        {
            get
            {
                if (_comandoGetFrota == null)
                {
                    _comandoGetFrota = new RelayCommand(
                        param =>
                        {
                            ListaSeriesView.Filter = FiltraSeries;
                            CollectionViewSource.GetDefaultView(ListaSeries).Refresh();
                        },
                        param => true
                    );
                }
                return _comandoGetFrota;
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

        public ICommand ComandoAdicionarItem
        {
            get
            {
                if (_comandoAdicionarItem == null)
                {
                    _comandoAdicionarItem = new RelayCommand(
                        param => ControleItem().Await(),
                        param => true
                    );
                }
                return _comandoAdicionarItem;
            }
        }

        public ICommand ComandoEditarItem
        {
            get
            {
                if (_comandoEditarItem == null)
                {
                    _comandoEditarItem = new RelayCommand(
                        param => ControleItem(ItemOrdemServicoSelecionado).Await(),
                        param => ItemOrdemServicoSelecionado != null
                    );
                }
                return _comandoEditarItem;
            }
        }

        public ICommand ComandoRemoverItem
        {
            get
            {
                if (_comandoRemoverItem == null)
                {
                    _comandoRemoverItem = new RelayCommand(
                        param => RemoverItens(ListaItensOrdemServicoSelecionados).Await(),
                        param => ItemOrdemServicoSelecionado != null
                    );
                }
                return _comandoRemoverItem;
            }
        }

        public ICommand ComandoExportarFormatoFluig
        {
            get
            {
                if (_comandoExportarFormatoFluig == null)
                {
                    _comandoExportarFormatoFluig = new RelayCommand(
                        param => ExportarItens(ExcelClasses.TipoExportacaoItensProposta.FormatoFluig, DataGrid, "Itens_Ordem_Servico").Await(),
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
                        param => ExportarItens(ExcelClasses.TipoExportacaoItensProposta.TabelaCompleta, DataGrid, "Itens_Ordem_Servico").Await(),
                        param => true
                    );
                }
                return _comandoExportarTabelaCompleta;
            }
        }

        public ICommand ComandoAdicionarEvento
        {
            get
            {
                if (_comandoAdicionarEvento == null)
                {
                    _comandoAdicionarEvento = new RelayCommand(
                        param => ControleEvento().Await(),
                        param => true
                    );
                }
                return _comandoAdicionarEvento;
            }
        }

        public ICommand ComandoEditarEvento
        {
            get
            {
                if (_comandoEditarEvento == null)
                {
                    _comandoEditarEvento = new RelayCommand(
                        param => ControleEvento(EventoOrdemServicoSelecionado).Await(),
                        param => EventoOrdemServicoSelecionado != null
                    );
                }
                return _comandoEditarEvento;
            }
        }

        public ICommand ComandoRemoverEvento
        {
            get
            {
                if (_comandoRemoverEvento == null)
                {
                    _comandoRemoverEvento = new RelayCommand(
                        param => RemoverEventos(ListaEventosOrdemServicoSelecionados).Await(),
                        param => EventoOrdemServicoSelecionado != null
                    );
                }
                return _comandoRemoverEvento;
            }
        }

        public ICommand ComandoExportarEvento
        {
            get
            {
                if (_comandoExportarEvento == null)
                {
                    _comandoExportarEvento = new RelayCommand(
                        param => ExportarItens(ExcelClasses.TipoExportacaoItensProposta.TabelaCompleta, DataGridEventos, "Eventos_Ordem_Servico").Await(),
                        param => EventoOrdemServicoSelecionado != null
                    );
                }
                return _comandoExportarEvento;
            }
        }

        public ICommand ComandoAdicionarInconsistencia
        {
            get
            {
                if (_comandoAdicionarInconsistencia == null)
                {
                    _comandoAdicionarInconsistencia = new RelayCommand(
                        param => ControleInconsistencia().Await(),
                        param => true
                    );
                }
                return _comandoAdicionarInconsistencia;
            }
        }

        public ICommand ComandoEditarInconsistencia
        {
            get
            {
                if (_comandoEditarInconsistencia == null)
                {
                    _comandoEditarInconsistencia = new RelayCommand(
                        param => ControleInconsistencia(InconsistenciaOrdemServicoSelecionada).Await(),
                        param => InconsistenciaOrdemServicoSelecionada != null
                    );
                }
                return _comandoEditarInconsistencia;
            }
        }

        public ICommand ComandoRemoverInconsistencia
        {
            get
            {
                if (_comandoRemoverInconsistencia == null)
                {
                    _comandoRemoverInconsistencia = new RelayCommand(
                        param => RemoverInconsistencias(ListaInconsistenciasOrdemServicoSelecionados).Await(),
                        param => InconsistenciaOrdemServicoSelecionada != null
                    );
                }
                return _comandoRemoverInconsistencia;
            }
        }

        public ICommand ComandoExportarInconsistencia
        {
            get
            {
                if (_comandoExportarInconsistencia == null)
                {
                    _comandoExportarInconsistencia = new RelayCommand(
                        param => ExportarItens(ExcelClasses.TipoExportacaoItensProposta.TabelaCompleta, DataGridInconsistencias, "Inconsistencias_Ordem_Servico").Await(),
                        param => InconsistenciaOrdemServicoSelecionada != null
                    );
                }
                return _comandoExportarInconsistencia;
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
                        param => ItemOrdemServicoSelecionado != null
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
                        param => ItemOrdemServicoSelecionado != null
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
                        param => ItemOrdemServicoSelecionado != null
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
                        param => ItemOrdemServicoSelecionado != null
                    );
                }
                return _comandoAlteraEspecificacaoTodos;
            }
        }

        public ICommand ComandoSalvar
        {
            get
            {
                if (_comandoSalvar == null)
                {
                    _comandoSalvar = new RelayCommand(
                        param => Salvar().Await(),
                        param => true
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
                        param => Editar().Await(),
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

        public ICommand ComandoVisualizar
        {
            get
            {
                if (_comandoVisualizar == null)
                {
                    _comandoVisualizar = new RelayCommand(
                        param => Visualizar(),
                        param => true
                    );
                }
                return _comandoVisualizar;
            }
        }

        public ICommand ComandoCancelarEdicao
        {
            get
            {
                if (_comandoCancelarEdicao == null)
                {
                    _comandoCancelarEdicao = new RelayCommand(
                        param => CancelarEdicao().Await(),
                        param => true
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

        #endregion Propriedades/Comandos

        #region Métodos

        /// <summary>
        /// Método assíncrono que serve como construtor da proposta já que construtores não podem ser assíncronos
        /// </summary>
        private async Task ConstrutorAsync(OrdemServico? ordemServico = null)
        {
            try
            {
                EhConstrutor = 1;

                // Preenche as listas com as classes necessárias
                await Cliente.PreencheListaClientesAsync(ListaClientes, true, null, CancellationToken.None, "WHERE clie.id_status = 1 ORDER BY clie.nome ASC", "");
                await Planta.PreencheListaPlantasAsync(ListaPlantas, true, null, CancellationToken.None, "WHERE plan.id_status = 1 ORDER BY plan.nome ASC", "");
                await Area.PreencheListaAreasAsync(ListaAreas, true, null, CancellationToken.None, "WHERE area.id_status = 1 ORDER BY area.nome ASC", "");
                await Frota.PreencheListaFrotasAsync(ListaFrotas, true, null, CancellationToken.None, "WHERE frot.id_status = 1 ORDER BY frot.nome ASC", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");
                await Serie.PreencheListaSeriesAsync(ListaSeries, true, null, CancellationToken.None, "WHERE seri.id_status = 1", "");
                await Fabricante.PreencheListaFabricantesAsync(ListaFabricantes, true, null, CancellationToken.None, "WHERE fabr.id_status = 1 ORDER BY fabr.nome ASC", "");
                await Categoria.PreencheListaCategoriasAsync(ListaCategorias, true, null, CancellationToken.None, "WHERE cate.id_status = 1 ORDER BY cate.nome ASC", "");
                await TipoEquipamento.PreencheListaTiposEquipamentoAsync(ListaTiposEquipamento, true, null, CancellationToken.None, "WHERE tieq.id_status = 1 ORDER BY tieq.nome ASC", "");
                await Classe.PreencheListaClassesAsync(ListaClasses, true, null, CancellationToken.None, "WHERE clas.id_status = 1 ORDER BY clas.nome ASC", "");
                await Modelo.PreencheListaModelosAsync(ListaModelos, true, null, CancellationToken.None, "WHERE mode.id_status = 1 ORDER BY mode.nome ASC", "");
                await Ano.PreencheListaAnosAsync(ListaAnos, true, null, CancellationToken.None, "WHERE ano <= @ano", "@ano", DateTime.Now.Year + 1);

                await TipoOrdemServico.PreencheListaTiposOrdemServicoAsync(ListaTiposOrdemServico, true, null, CancellationToken.None, "WHERE tios.id_status = 1 ORDER BY tios.nome ASC", "");
                await StatusEquipamentoAposManutencao.PreencheListaStatusEquipamentoAposManutencaoAsync(ListaStatusEquipamentoAposManutencao, true, null, CancellationToken.None, "WHERE seam.id_status = 1 ORDER BY seam.nome ASC", "");
                await TipoManutencao.PreencheListaTiposManutencaoAsync(ListaTipoManutencao, true, null, CancellationToken.None, "WHERE tima.id_status = 1 ORDER BY tima.id_tipo_manutencao ASC", "");
                await UsoIndevido.PreencheListaUsosIndevidosAsync(ListaUsoIndevido, true, null, CancellationToken.None, "WHERE usoi.id_status = 1 ORDER BY usoi.nome ASC", "");
                await ExecutanteServico.PreencheListaExecutantesServicoAsync(ListaExecutanteServico, true, null, CancellationToken.None, "WHERE exse.id_status = 1 ORDER BY exse.nome ASC", "");

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

                _configuracaoSistema = new();

                await _configuracaoSistema.GetConfiguracaoSistemaDatabaseAsync(1, CancellationToken.None);

                ListaPlantasView = GetPlantaCollectionView(ListaPlantas);
                ListaAreasView = GetAreaCollectionView(ListaAreas);
                ListaFrotasView = GetFrotaCollectionView(ListaFrotas);
                ListaSeriesView = GetSerieCollectionView(ListaSeries);
                ListaModelosView = GetModeloCollectionView(ListaModelos);

                ListaPlantasView.Filter = LimpaListaPlantas;
                CollectionViewSource.GetDefaultView(ListaPlantas).Refresh();

                ListaAreasView.Filter = LimpaListaAreas;
                CollectionViewSource.GetDefaultView(ListaAreas).Refresh();

                ListaFrotasView.Filter = LimpaListaFrotas;
                CollectionViewSource.GetDefaultView(ListaFrotas).Refresh();

                ListaSeriesView.Filter = LimpaListaSeries;
                CollectionViewSource.GetDefaultView(ListaSeries).Refresh();

                ListaModelosView.Filter = LimpaListaModelos;
                CollectionViewSource.GetDefaultView(ListaModelos).Refresh();

                // Limita os anos trazendo apenas valores únicos
                ListaAnos.DistinctBy(test => test.AnoValor);

                // Se a ordem de serviço não foi informada, cria uma nova instância de ordem de serviço
                if (ordemServico == null)
                {
                    OrdemServico = new(true, true, true);

                    OrdemServico.IdUsuarioEmUso = App.Usuario.Id;

                    OrdemServico.Status = ListaStatus.First(stat => stat.Id == 1);

                    OrdemServico.Filial = App.Usuario.Filial;
                    OrdemServico.UsuarioInsercao = (Usuario)App.Usuario.Clone();
                    OrdemServico.EtapasConcluidas = 1;

                    Name = "Nova OS";
                    CancelarEdicaoVisivel = false;
                    CancelarVisivel = false;

                    PermiteSalvar = true;
                    PermiteVisualizar = false;
                    PermiteEditar = false;
                    PermiteCopiar = false;
                    PermiteDeletar = false;

                    ControlesHabilitados = true;
                    ehInsercao = true;
                }
                else
                {
                    OrdemServico = ordemServico;
                    ehInsercao = false;

                    await CarregarOrdemServico();
                }
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");

                Messenger.Default.Send<string>("Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log", "MensagemStatus");

                ControlesHabilitados = false;
            }
            CarregamentoVisivel = false;
            Messenger.Default.Send<IPageViewModel>(this, "SelecionaPrimeiraPagina");
        }

        private async Task CarregarOrdemServico()
        {
            if (OrdemServico.Id == null && !_ehCopia)
            {
                return;
            }

            ControlesHabilitados = false;

            DeletarVisivel = App.Usuario.Perfil.Id == 1;

            CancelarVisivel = false;
            PermiteCancelar = false;

            CancelarEdicaoVisivel = false;
            PermiteCancelarEdicao = false;
            PermiteSalvar = false;
            PermiteEditar = true;
            PermiteCopiar = true;
            PermiteDeletar = true;
            PermiteVisualizar = true;
            EhCarregamento = true;

            if (!_ehCopia)
            {
                try
                {
                    // Retorna a ordem de serviço de acordo com a database
                    await OrdemServico.GetOrdemServicoDatabaseAsync(OrdemServico.Id, CancellationToken.None, true, true, true);
                }
                catch (Exception ex)
                {
                    // Escreve no log a exceção e uma mensagem de erro
                    Serilog.Log.Error(ex, "Erro ao carregar dados");

                    Messenger.Default.Send<string>("Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log", "MensagemStatus");
                }
            }

            try
            {
                if (OrdemServico?.Cliente?.Id > 0 && !ListaClientes.Any(x => x.Id == OrdemServico?.Cliente?.Id))
                {
                    Cliente cliente = new();

                    await cliente.GetClienteDatabaseAsync(OrdemServico?.Cliente?.Id, CancellationToken.None);

                    ListaClientes.Add(cliente);
                }

                if (OrdemServico?.Frota?.Area?.Planta?.Id > 0 && !ListaPlantas.Any(x => x.Id == OrdemServico?.Frota?.Area?.Planta?.Id))
                {
                    Planta planta = new();

                    await planta.GetPlantaDatabaseAsync(OrdemServico?.Frota?.Area?.Planta?.Id, CancellationToken.None);

                    ListaPlantas.Add(planta);
                }

                if (OrdemServico?.Frota?.Area?.Id > 0 && !ListaAreas.Any(x => x.Id == OrdemServico?.Frota?.Area?.Id))
                {
                    Area area = new();

                    await area.GetAreaDatabaseAsync(OrdemServico?.Frota?.Area?.Id, CancellationToken.None);

                    ListaAreas.Add(area);
                }

                if (OrdemServico?.Frota?.Id > 0 && !ListaFrotas.Any(x => x.Id == OrdemServico?.Frota?.Id))
                {
                    Frota frota = new();

                    await frota.GetFrotaDatabaseAsync(OrdemServico?.Frota?.Id, CancellationToken.None);

                    ListaFrotas.Add(frota);
                }

                if (OrdemServico?.Serie?.Id > 0 && !ListaSeries.Any(x => x.Id == OrdemServico?.Serie?.Id))
                {
                    Serie serie = new();

                    await serie.GetSerieDatabaseAsync(OrdemServico?.Serie?.Id, CancellationToken.None);

                    ListaSeries.Add(serie);
                }

                if (OrdemServico?.Serie?.Modelo?.Fabricante?.Id > 0 && !ListaFabricantes.Any(x => x.Id == OrdemServico?.Serie?.Modelo?.Fabricante?.Id))
                {
                    Fabricante fabricante = new();

                    await fabricante.GetFabricanteDatabaseAsync(OrdemServico?.Serie?.Modelo?.Fabricante?.Id, CancellationToken.None);

                    ListaFabricantes.Add(fabricante);
                }

                if (OrdemServico?.Serie?.Modelo?.Categoria?.Id > 0 && !ListaCategorias.Any(x => x.Id == OrdemServico?.Serie?.Modelo?.Categoria?.Id))
                {
                    Categoria categoria = new();

                    await categoria.GetCategoriaDatabaseAsync(OrdemServico?.Serie?.Modelo?.Categoria?.Id, CancellationToken.None);

                    ListaCategorias.Add(categoria);
                }

                if (OrdemServico?.Serie?.Modelo?.TipoEquipamento?.Id > 0 && !ListaTiposEquipamento.Any(x => x.Id == OrdemServico?.Serie?.Modelo?.TipoEquipamento?.Id))
                {
                    TipoEquipamento tipoEquipamento = new();

                    await tipoEquipamento.GetTipoEquipamentoDatabaseAsync(OrdemServico?.Serie?.Modelo?.TipoEquipamento?.Id, CancellationToken.None);

                    ListaTiposEquipamento.Add(tipoEquipamento);
                }

                if (OrdemServico?.Serie?.Modelo?.Classe?.Id > 0 && !ListaClasses.Any(x => x.Id == OrdemServico?.Serie?.Modelo?.Classe?.Id))
                {
                    Classe classe = new();

                    await classe.GetClasseDatabaseAsync(OrdemServico?.Serie?.Modelo?.Classe?.Id, CancellationToken.None);

                    ListaClasses.Add(classe);
                }

                if (OrdemServico?.Serie?.Modelo?.Id > 0 && !ListaModelos.Any(x => x.Id == OrdemServico?.Serie?.Modelo?.Id))
                {
                    Modelo modelo = new();

                    await modelo.GetModeloDatabaseAsync(OrdemServico?.Serie?.Modelo?.Id, CancellationToken.None);

                    ListaModelos.Add(modelo);
                }

                if (OrdemServico?.TipoOrdemServico?.Id > 0 && !ListaTiposOrdemServico.Any(x => x.Id == OrdemServico?.TipoOrdemServico?.Id))
                {
                    TipoOrdemServico tipoOrdemServico = new();

                    await tipoOrdemServico.GetTipoOrdemServicoDatabaseAsync(OrdemServico?.TipoOrdemServico?.Id, CancellationToken.None);

                    ListaTiposOrdemServico.Add(tipoOrdemServico);
                }

                if (OrdemServico?.StatusEquipamentoAposManutencao?.Id > 0 && !ListaStatusEquipamentoAposManutencao.Any(x => x.Id == OrdemServico?.StatusEquipamentoAposManutencao?.Id))
                {
                    StatusEquipamentoAposManutencao statusEquipamentoAposManutencao = new();

                    await statusEquipamentoAposManutencao.GetStatusEquipamentoAposManutencaoDatabaseAsync(OrdemServico?.StatusEquipamentoAposManutencao?.Id, CancellationToken.None);

                    ListaStatusEquipamentoAposManutencao.Add(statusEquipamentoAposManutencao);
                }

                if (OrdemServico?.TipoManutencao?.Id > 0 && !ListaTipoManutencao.Any(x => x.Id == OrdemServico?.TipoManutencao?.Id))
                {
                    TipoManutencao tipoManutencao = new();

                    await tipoManutencao.GetTipoManutencaoDatabaseAsync(OrdemServico?.TipoManutencao?.Id, CancellationToken.None);

                    ListaTipoManutencao.Add(tipoManutencao);
                }

                if (OrdemServico?.UsoIndevido?.Id > 0 && !ListaUsoIndevido.Any(x => x.Id == OrdemServico?.UsoIndevido?.Id))
                {
                    UsoIndevido usoIndevido = new();

                    await usoIndevido.GetUsoIndevidoDatabaseAsync(OrdemServico?.UsoIndevido?.Id, CancellationToken.None);

                    ListaUsoIndevido.Add(usoIndevido);
                }

                if (OrdemServico?.ExecutanteServico?.Id > 0 && !ListaExecutanteServico.Any(x => x.Id == OrdemServico?.ExecutanteServico?.Id))
                {
                    ExecutanteServico executanteServico = new();

                    await executanteServico.GetExecutanteServicoDatabaseAsync(OrdemServico?.ExecutanteServico?.Id, CancellationToken.None);

                    ListaExecutanteServico.Add(executanteServico);
                }
            }
            catch (Exception)
            {
            }

            EhCarregamento = true;
            OrdemServicoAtual = OrdemServico.OrdemServicoAtual;
            OrdemServicoPrimaria = OrdemServico.OrdemServicoPrimaria;

            if (OrdemServico.EtapasConcluidas >= 3)
            {
                TextoSalvar = "Salvar e finalizar";
                IconeSalvar = "CheckCircle";
            }

            try
            {
                OrdemServico.TipoOrdemServico = ListaTiposOrdemServico.First(stat => stat.Id == OrdemServico?.TipoOrdemServico?.Id);
            }
            catch (Exception)
            {
            }
            try
            {
                OrdemServico.Status = ListaStatus.First(stat => stat.Id == OrdemServico?.Status?.Id);
            }
            catch (Exception)
            {
            }
            try
            {
                Cliente = ListaClientes.First(clie => clie.Id == OrdemServico?.Cliente?.Id);
                //Cliente = Proposta.Cliente;
            }
            catch (Exception)
            {
            }
            try
            {
                Planta = ListaPlantas.First(plan => plan.Id == OrdemServico?.Frota?.Area?.Planta?.Id);
                //Cliente = Proposta.Cliente;
            }
            catch (Exception)
            {
            }
            try
            {
                Area = ListaAreas.First(area => area.Id == OrdemServico?.Frota?.Area?.Id);
                //Cliente = Proposta.Cliente;
            }
            catch (Exception)
            {
            }

            try
            {
                OrdemServico.Serie.Modelo.Fabricante = ListaFabricantes.First(fabr => fabr.Id == OrdemServico?.Serie?.Modelo?.Fabricante?.Id);
                Fabricante = OrdemServico.Serie.Modelo.Fabricante;
                //await PreencheTiposEquipamentoAsync();
            }
            catch (Exception)
            {
            }
            try
            {
                OrdemServico.Serie.Modelo.TipoEquipamento = ListaTiposEquipamento.First(tieq => tieq.Id == OrdemServico?.Serie?.Modelo?.TipoEquipamento?.Id);
                TipoEquipamento = OrdemServico.Serie.Modelo.TipoEquipamento;
                //await PreencheModelosAsync();
            }
            catch (Exception)
            {
            }
            try
            {
                OrdemServico.Serie.Modelo = ListaModelos.First(tieq => tieq.Id == OrdemServico?.Serie?.Modelo?.Id);
                Modelo = OrdemServico.Serie.Modelo;
                //await PreencheModelosAsync();
            }
            catch (Exception)
            {
            }
            try
            {
                OrdemServico.Serie.Ano = ListaAnos.First(anno => anno.Id == OrdemServico?.Serie?.Ano?.Id);
            }
            catch (Exception)
            {
            }
            try
            {
                OrdemServico.Serie.Modelo.Categoria = ListaCategorias.First(cate => cate.Id == OrdemServico?.Serie?.Modelo?.Categoria.Id);
                Categoria = OrdemServico.Serie.Modelo.Categoria;
            }
            catch (Exception)
            {
            }
            try
            {
                OrdemServico.Serie.Modelo.Classe = ListaClasses.First(clas => clas.Id == OrdemServico?.Serie?.Modelo?.Classe.Id);
                Classe = OrdemServico.Serie.Modelo.Classe;
            }
            catch (Exception)
            {
            }
            try
            {
                OrdemServico.StatusEquipamentoAposManutencao = ListaStatusEquipamentoAposManutencao.First(stat => stat.Id == OrdemServico?.StatusEquipamentoAposManutencao?.Id);
            }
            catch (Exception)
            {
            }
            try
            {
                TipoManutencao = ListaTipoManutencao.First(clie => clie.Id == OrdemServico?.TipoManutencao?.Id);
                //Cliente = Proposta.Cliente;
            }
            catch (Exception)
            {
            }
            try
            {
                OrdemServico.UsoIndevido = ListaUsoIndevido.First(clie => clie.Id == OrdemServico?.UsoIndevido?.Id);
                //Cliente = Proposta.Cliente;
            }
            catch (Exception)
            {
            }
            try
            {
                OrdemServico.ExecutanteServico = ListaExecutanteServico.First(clie => clie.Id == OrdemServico?.ExecutanteServico?.Id);
                //Cliente = Proposta.Cliente;
            }
            catch (Exception)
            {
            }

            try
            {
                if (_ehCopia)
                {
                    Name = "Nova OS";
                    CancelarEdicaoVisivel = false;
                    CancelarVisivel = false;

                    PermiteSalvar = true;
                    PermiteVisualizar = false;
                    PermiteEditar = false;
                    PermiteCopiar = false;
                    PermiteDeletar = false;

                    ControlesHabilitados = true;
                    ehInsercao = true;
                }
                else
                {
                    Name = OrdemServico.Cliente.Nome + " - " + OrdemServico.OrdemServicoAtual;
                }
            }
            catch (Exception)
            {
                Name = "OS não identificada";
            }
        }

        /// <summary>
        /// Método para retornar os dados do contato, caso ele exista
        /// </summary>
        private async Task GetSerieAsync()
        {
            if (OrdemServico.Serie.Id != null)
            {
                try
                {
                    await OrdemServico.Serie.GetSerieDatabaseAsync(OrdemServico.Serie.Id, CancellationToken.None);

                    if (OrdemServico?.Serie?.Modelo?.Fabricante?.Id > 0 && !ListaFabricantes.Any(x => x.Id == OrdemServico?.Serie?.Modelo?.Fabricante?.Id))
                    {
                        Fabricante fabricante = new();

                        await fabricante.GetFabricanteDatabaseAsync(OrdemServico?.Serie?.Modelo?.Fabricante?.Id, CancellationToken.None);

                        ListaFabricantes.Add(fabricante);
                    }

                    if (OrdemServico?.Serie?.Modelo?.Categoria?.Id > 0 && !ListaCategorias.Any(x => x.Id == OrdemServico?.Serie?.Modelo?.Categoria?.Id))
                    {
                        Categoria categoria = new();

                        await categoria.GetCategoriaDatabaseAsync(OrdemServico?.Serie?.Modelo?.Categoria?.Id, CancellationToken.None);

                        ListaCategorias.Add(categoria);
                    }

                    if (OrdemServico?.Serie?.Modelo?.TipoEquipamento?.Id > 0 && !ListaTiposEquipamento.Any(x => x.Id == OrdemServico?.Serie?.Modelo?.TipoEquipamento?.Id))
                    {
                        TipoEquipamento tipoEquipamento = new();

                        await tipoEquipamento.GetTipoEquipamentoDatabaseAsync(OrdemServico?.Serie?.Modelo?.TipoEquipamento?.Id, CancellationToken.None);

                        ListaTiposEquipamento.Add(tipoEquipamento);
                    }

                    if (OrdemServico?.Serie?.Modelo?.Classe?.Id > 0 && !ListaClasses.Any(x => x.Id == OrdemServico?.Serie?.Modelo?.Classe?.Id))
                    {
                        Classe classe = new();

                        await classe.GetClasseDatabaseAsync(OrdemServico?.Serie?.Modelo?.Classe?.Id, CancellationToken.None);

                        ListaClasses.Add(classe);
                    }

                    if (OrdemServico?.Serie?.Modelo?.Id > 0 && !ListaModelos.Any(x => x.Id == OrdemServico?.Serie?.Modelo?.Id))
                    {
                        Modelo modelo = new();

                        await modelo.GetModeloDatabaseAsync(OrdemServico?.Serie?.Modelo?.Id, CancellationToken.None);

                        ListaModelos.Add(modelo);
                    }

                    Fabricante = ListaFabricantes.First(fabr => fabr.Id == OrdemServico?.Serie?.Modelo?.Fabricante?.Id);
                    Categoria = ListaCategorias.First(cate => cate.Id == OrdemServico?.Serie?.Modelo?.Categoria?.Id);
                    TipoEquipamento = ListaTiposEquipamento.First(tieq => tieq.Id == OrdemServico?.Serie?.Modelo?.TipoEquipamento?.Id);
                    Classe = ListaClasses.First(clas => clas.Id == OrdemServico?.Serie?.Modelo?.Classe?.Id);
                    Modelo = ListaModelos.First(mode => mode.Id == OrdemServico?.Serie?.Modelo?.Id);

                    await Ano.PreencheListaAnosAsync(ListaAnos, true, null, CancellationToken.None, "WHERE ano <= @ano", "@ano", DateTime.Now.Year + 1);
                    ListaAnos.DistinctBy(test => test.AnoValor);

                    try
                    {
                        OrdemServico.Serie.Ano = ListaAnos.First(anno => anno.Id == OrdemServico?.Serie?.Ano?.Id);
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

                if (!String.IsNullOrEmpty(OrdemServico.Serie.Nome))
                {
                    try
                    {
                        Familia familia = new();
                        await familia.GetFamiliaDatabaseAsync(OrdemServico.Serie.Nome, CancellationToken.None);

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
                    string? caracteres = OrdemServico.Serie.Nome.Substring((int)(posicaoInicioCaracters == null ? 0 : posicaoInicioCaracters) - 1, 1);

                    ObservableCollection<Ano> listaAnosTemporaria = new();

                    await Ano.PreencheListaAnosAsync(listaAnosTemporaria, true, null, CancellationToken.None,
                    "WHERE t_ano.id_status = 1 AND fabr.id_fabricante = @id_fabricante AND ano <= @ano AND posicao_inicio_caracteres = @posicao_inicio_caracteres AND caracteres = @caracteres",
                    "@id_fabricante, @ano, @posicao_inicio_caracteres, @caracteres", Fabricante.Id, DateTime.Now.Year + 1, posicaoInicioCaracters, caracteres);

                    // Se a lista retornada for maior que 0 significa que existem anos com os critérios informados, portanto preenche a lista de anos através dessa lista, caso contrário preenche a lista completa
                    if (listaAnosTemporaria.Count > 0 && familiaExiste)
                    {
                        ListaAnos = listaAnosTemporaria;
                        OrdemServico.Serie.Ano = ListaAnos.Last();
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

        private async Task ControleItem(ItemOrdemServico itemOrdemServico = null)
        {
            try
            {
                bool ehNovoItem = false;

                if (itemOrdemServico == null)
                {
                    ehNovoItem = true;

                    Status statusItem = new();

                    await statusItem.GetStatusDatabaseAsync(1, CancellationToken.None);

                    itemOrdemServico = new ItemOrdemServico { Status = statusItem };
                }

                var customDialog = new CustomDialog();

                var dataContext = new ControleItemOrdemServicoViewModel(OrdemServico, itemOrdemServico, ehNovoItem, instance =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                });

                customDialog.Content = new ControleItemOrdemServicoView { DataContext = dataContext };

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

        private async Task RemoverItem(ItemOrdemServico itemOrdemServico)
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
                OrdemServico.ListaItensOrdemServico.Remove(itemOrdemServico);
            }

            if (itemOrdemServico.Id != null)
            {
                removeuItemExistente = true;
            }
        }

        private async Task RemoverItens(ObservableCollection<ItemOrdemServico> itensOrdemServicoRemover)
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
                foreach (var itemOrdemServico in itensOrdemServicoRemover.ToList())
                {
                    OrdemServico.ListaItensOrdemServico.Remove(itemOrdemServico);

                    if (itemOrdemServico.Id != null)
                    {
                        removeuItemExistente = true;
                    }
                }
            }
        }

        private async Task ControleEvento(EventoOrdemServico eventoOrdemServico = null)
        {
            try
            {
                bool ehNovoItem = false;

                if (eventoOrdemServico == null)
                {
                    ehNovoItem = true;

                    Status statusItem = new();

                    await statusItem.GetStatusDatabaseAsync(1, CancellationToken.None);

                    eventoOrdemServico = new EventoOrdemServico { Status = statusItem };
                }

                var customDialog = new CustomDialog();

                var dataContext = new ControleEventoOrdemServicoViewModel(OrdemServico, eventoOrdemServico, ehNovoItem, instance =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                });

                customDialog.Content = new ControleEventoOrdemServicoView { DataContext = dataContext };

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

        private async Task RemoverEvento(EventoOrdemServico eventoOrdemServico)
        {
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Sim",
                NegativeButtonText = "Não"
            };

            var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                "Atenção", "Tem certeza que deseja remover o evento? O processo não poderá ser desfeito",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem == MessageDialogResult.Affirmative)
            {
                OrdemServico.ListaEventosOrdemServico.Remove(eventoOrdemServico);
            }

            if (eventoOrdemServico.Id != null)
            {
                removeuEventoExistente = true;
            }
        }

        private async Task RemoverEventos(ObservableCollection<EventoOrdemServico> eventosOrdemServicoRemover)
        {
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Sim",
                NegativeButtonText = "Não"
            };

            var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                "Atenção", "Tem certeza que deseja remover os eventos selecionados? O processo não poderá ser desfeito",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem == MessageDialogResult.Affirmative)
            {
                foreach (var eventoOrdemServico in eventosOrdemServicoRemover.ToList())
                {
                    OrdemServico.ListaEventosOrdemServico.Remove(eventoOrdemServico);

                    if (eventoOrdemServico.Id != null)
                    {
                        removeuEventoExistente = true;
                    }
                }
            }
        }

        private async Task ControleInconsistencia(InconsistenciaOrdemServico inconsistenciaOrdemServico = null)
        {
            try
            {
                bool ehNovoItem = false;

                if (inconsistenciaOrdemServico == null)
                {
                    ehNovoItem = true;

                    Status statusItem = new();

                    await statusItem.GetStatusDatabaseAsync(1, CancellationToken.None);

                    inconsistenciaOrdemServico = new InconsistenciaOrdemServico { Status = statusItem };
                }

                var customDialog = new CustomDialog();

                var dataContext = new ControleInconsistenciaOrdemServicoViewModel(OrdemServico, inconsistenciaOrdemServico, ehNovoItem, instance =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                });

                customDialog.Content = new ControleInconsistenciaOrdemServicoView { DataContext = dataContext };

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

        private async Task RemoverInconsistencia(InconsistenciaOrdemServico inconsistenciaOrdemServico)
        {
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Sim",
                NegativeButtonText = "Não"
            };

            var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                "Atenção", "Tem certeza que deseja remover a inconsistencia? O processo não poderá ser desfeito",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem == MessageDialogResult.Affirmative)
            {
                OrdemServico.ListaInconsistenciasOrdemServico.Remove(inconsistenciaOrdemServico);
            }

            if (inconsistenciaOrdemServico.Id != null)
            {
                removeuItemExistente = true;
            }
        }

        private async Task RemoverInconsistencias(ObservableCollection<InconsistenciaOrdemServico> inconsistenciasOrdemServicoRemover)
        {
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Sim",
                NegativeButtonText = "Não"
            };

            var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                "Atenção", "Tem certeza que deseja remover as inconsistências selecionadas? O processo não poderá ser desfeito",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem == MessageDialogResult.Affirmative)
            {
                foreach (var inconsistenciaOrdemServico in inconsistenciasOrdemServicoRemover.ToList())
                {
                    OrdemServico.ListaInconsistenciasOrdemServico.Remove(inconsistenciaOrdemServico);

                    if (inconsistenciaOrdemServico.Id != null)
                    {
                        removeuInconsistenciaExistente = true;
                    }
                }
            }
        }

        private async Task ExportarItens(ExcelClasses.TipoExportacaoItensProposta tipoExportacaoItens, SfDataGrid dataGrid, string nomeInicial)
        {
            VistaSaveFileDialog sfd = new VistaSaveFileDialog();
            var options = new ExcelExportingOptions();
            string nomeWorksheet = "";

            switch (tipoExportacaoItens)
            {
                case ExcelClasses.TipoExportacaoItensProposta.FormatoFluig:
                    sfd.Filter = "Arquivo do Excel (*.xlsx)|*.xlsx";
                    sfd.Title = "Informe o local e o nome do arquivo";
                    sfd.FileName = "importacao";
                    sfd.AddExtension = true;
                    nomeWorksheet = "importacao1";
                    foreach (var item in dataGrid.Columns)
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
                    sfd.FileName = nomeInicial + "_" + OrdemServico.OrdemServicoAtual + "_" + OrdemServico.Cliente.Nome;
                    sfd.AddExtension = true;
                    nomeWorksheet = nomeInicial;
                    foreach (var item in dataGrid.Columns)
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
                    sfd.FileName = nomeInicial + "_" + OrdemServico.OrdemServicoAtual + "_" + OrdemServico.Cliente.Nome;
                    sfd.AddExtension = true;
                    nomeWorksheet = nomeInicial;
                    foreach (var item in dataGrid.Columns)
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
                var excelEngine = dataGrid.ExportToExcel(dataGrid.View, options);
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

                if (tipoExportacaoItens == ExcelClasses.TipoExportacaoItensProposta.FormatoFluig)
                {
                    if (workBook.Worksheets[0].Range["A1"].Value != dataGrid.Columns["CodigoItem"].HeaderText)
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
                }
                else
                {
                    int contador = 0;
                    foreach (var item in dataGrid.Columns)
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
                        "Exportação concluída", "Deseja abrir o arquivo?",
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

        private async Task Salvar()
        {
            // Retorna o id do usuário em uso
            await OrdemServico.GetIdUsuarioEmUsoAsync(CancellationToken.None);

            // Verifica se o id do usuário em uso não é nulo e, caso verdadeiro, impede a edição
            if (OrdemServico.IdUsuarioEmUso != null && OrdemServico.IdUsuarioEmUso != App.Usuario.Id)
            {
                // Define o usuário
                Usuario usuarioEmUso = new();
                await usuarioEmUso.GetUsuarioDatabaseAsync(OrdemServico.IdUsuarioEmUso, CancellationToken.None);

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Ordem de serviço em uso", "A ordem de serviço em questão está sendo editada por " + usuarioEmUso.Nome + ", portanto não será possível salva-la. Aguarde o usuário finalizar a edição da ordem de serviço para poder salva-la.", MessageDialogStyle.Affirmative, mySettings);

                usuarioEmUso = null;

                return;
            }

            // Verifica se existem campos vazios e, caso verdadeiro, encerra a execução do método
            if (ExistemCamposVazios)
            {
                return;
            }

            // Verifica se existem itens na proposta e, caso contrário, retorna mensagem
            if (OrdemServico.ListaEventosOrdemServico.Count == 0 && OrdemServico.EtapasConcluidas == 4)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Ordem de serviço vazia", "Adicione ao menos um evento à ordem de serviço para poder salvá-la", MessageDialogStyle.Affirmative, mySettings);

                return;
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
                    "Atenção", "Você removeu itens dessa ordem de serviço que já constavam na database. Eles serão excluídos permanentemente e não poderão ser recuperados. Tem certeza que deseja continuar?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);

                if (respostaMensagem != MessageDialogResult.Affirmative)
                {
                    return;
                }
            }

            // Verifica se eventos existentes na database foram excluidos e, caso positivo, notifica o usuário e pergunta se deseja continuar
            if (removeuEventoExistente)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Sim",
                    NegativeButtonText = "Não"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                    "Atenção", "Você removeu eventos dessa ordem de serviço que já constavam na database. Eles serão excluídos permanentemente e não poderão ser recuperados. Tem certeza que deseja continuar?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);

                if (respostaMensagem != MessageDialogResult.Affirmative)
                {
                    return;
                }
            }

            // Verifica se inconsistencias existentes na database foram excluidos e, caso positivo, notifica o usuário e pergunta se deseja continuar
            if (removeuInconsistenciaExistente)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Sim",
                    NegativeButtonText = "Não"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                    "Atenção", "Você removeu inconsistencias dessa ordem de serviço que já constavam na database. Elas serão excluídas permanentemente e não poderão ser recuperadas. Tem certeza que deseja continuar?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);

                if (respostaMensagem != MessageDialogResult.Affirmative)
                {
                    return;
                }
            }

            // Valida a sessão e obriga o login
            await HelperClasses.ValidaSessao.ValidaSessaoUsuarioAsync(_dialogCoordinator, this);

            // Define novamente os dados do usuário atual
            if (ehInsercao)
            {
                OrdemServico.UsuarioInsercao = (Usuario)App.Usuario.Clone();
                OrdemServico.DataInsercao = DateTime.Now;
            }
            else
            {
                OrdemServico.UsuarioEdicao = (Usuario)App.Usuario.Clone();
                OrdemServico.DataEdicao = DateTime.Now;
            }

            string messagemErroSalvarItens = "";
            bool ehNovaOrdemServico = false;

            ProgressoEhIndeterminavel = true;
            ValorProgresso = (double)0;
            MensagemStatus = "Salvando a ordem de serviço. Aguarde...";

            bool permiteSalvarAnterior = PermiteSalvar;
            bool permiteEditarAnterior = PermiteEditar;
            bool permiteCopiarAnterior = PermiteCopiar;
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
            PermiteDeletar = false;
            PermiteVisualizar = false;
            PermiteCancelarEdicao = false;

            Random numeroAleatorio = new();
            int milisegundosParaAguardar = 0;

            // O trecho de código abaixo é obrigatório para evitar que, caso dois usuários cliquem em salvar uma nova ordem de serviço exatamente ao mesmo tempo exista uma distinção
            if (OrdemServico.Id == null)
            {
                // Determina que é uma nova ordem de serviço
                ehNovaOrdemServico = true;

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
                    PermiteDeletar = permiteDeletarAnterior;
                    PermiteVisualizar = permiteVisualizarAnterior;
                    PermiteCancelarEdicao = permiteCancelarEdicaoAnterior;
                    return;
                }
            }
            else
            {
                await Task.Delay(1000, _cts.Token);
            }

            if (ehInsercao)
            {
                OrdemServico.UsuarioInsercao = (Usuario)App.Usuario.Clone();
            }
            else
            {
                OrdemServico.UsuarioEdicao = (Usuario)App.Usuario.Clone();
            }

            OrdemServico.OrdemServicoAtual = OrdemServicoAtual;
            OrdemServico.OrdemServicoPrimaria = OrdemServicoPrimaria;

            try
            {
                if (!String.IsNullOrEmpty(OrdemServico.Frota.Nome))
                {
                    OrdemServico.Frota.Status = new Status { Id = 1, Nome = "Ativo" };
                    OrdemServico.Frota.Area = Area;
                    OrdemServico.Frota.Area.Planta = Planta;
                    OrdemServico.Frota.Area.Planta.Cliente = Cliente;

                    await OrdemServico.Frota.SalvarFrotaDatabaseAsync(CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao salvar frota");
            }

            try
            {
                if (!String.IsNullOrEmpty(OrdemServico.Serie.Nome))
                {
                    OrdemServico.Serie.Status = new Status { Id = 1, Nome = "Ativo" };
                    OrdemServico.Serie.Frota = OrdemServico.Frota;
                    OrdemServico.Serie.Cliente = OrdemServico.Cliente;
                    OrdemServico.Serie.Modelo = Modelo;

                    await OrdemServico.Serie.SalvarSerieDatabaseAsync(CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao salvar série");
            }

            try
            {
                await OrdemServico.SalvarOrdemServicoDatabaseAsync(_cts.Token);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao salvar dados");

                ProgressoEhIndeterminavel = false;
                ValorProgresso = (double)0;
                MensagemStatus = "Erro ao salvar a ordem de serviço. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";

                ControlesHabilitados = true;
                CancelarVisivel = false;
                PermiteCancelar = false;
                PermiteSalvar = permiteSalvarAnterior;
                PermiteEditar = permiteEditarAnterior;
                PermiteCopiar = permiteCopiarAnterior;
                PermiteDeletar = permiteDeletarAnterior;
                PermiteVisualizar = permiteVisualizarAnterior;
                PermiteCancelarEdicao = permiteCancelarEdicaoAnterior;
                return;
            }

            MensagemStatus = "Salvando os itens da ordem de serviço. Aguarde...";

            int contadorItens = 0;
            int totalItens = OrdemServico.ListaItensOrdemServico.Count;

            // Se não for uma nova ordem de serviço, varre os itens anteriores da ordem de serviço e verifica se esses itens ainda existem, caso contrário, exclui os itens da database
            if (!ehNovaOrdemServico)
            {
                try
                {
                    ObservableCollection<ItemOrdemServico> listaItensAnterioresOrdemServico = new();

                    await ItemOrdemServico.PreencheListaItensOrdemServicoAsync(listaItensAnterioresOrdemServico, true, false, null,
                        CancellationToken.None, "WHERE itos.id_ordem_servico = @id_ordem_servico", "@id_ordem_servico", OrdemServico.Id);

                    foreach (var item in listaItensAnterioresOrdemServico)
                    {
                        if (!OrdemServico.ListaItensOrdemServico.Where(it => it.Id == item.Id).Any())
                        {
                            await item.DeletarItemOrdemServicoDatabaseAsync(CancellationToken.None);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            try
            {
                foreach (var item in OrdemServico.ListaItensOrdemServico)
                {
                    item.OrdemServico = OrdemServico;

                    await item.SalvarItemOrdemServicoDatabaseAsync(CancellationToken.None);

                    contadorItens++;

                    Messenger.Default.Send<double>((double)contadorItens / (double)totalItens * (double)100, "ValorProgresso");
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao salvar dados");

                messagemErroSalvarItens = ". Porém, houve erro ao salvar um ou mais itens. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
            }

            contadorItens = 0;
            totalItens = OrdemServico.ListaEventosOrdemServico.Count;

            // Se não for uma nova ordem de serviço, varre os eventos anteriores da ordem de serviço e verifica se esses itens ainda existem, caso contrário, exclui os eventos da database
            if (!ehNovaOrdemServico)
            {
                try
                {
                    ObservableCollection<EventoOrdemServico> listaEventosAnterioresOrdemServico = new();

                    await EventoOrdemServico.PreencheListaEventosOrdemServicoAsync(listaEventosAnterioresOrdemServico, true, false, null,
                        CancellationToken.None, "WHERE evos.id_ordem_servico = @id_ordem_servico", "@id_ordem_servico", OrdemServico.Id);

                    foreach (var item in listaEventosAnterioresOrdemServico)
                    {
                        if (!OrdemServico.ListaEventosOrdemServico.Where(it => it.Id == item.Id).Any())
                        {
                            await item.DeletarEventoOrdemServicoDatabaseAsync(CancellationToken.None);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            try
            {
                foreach (var item in OrdemServico.ListaEventosOrdemServico)
                {
                    item.OrdemServico = OrdemServico;

                    await item.SalvarEventoOrdemServicoDatabaseAsync(CancellationToken.None);

                    contadorItens++;

                    Messenger.Default.Send<double>((double)contadorItens / (double)totalItens * (double)100, "ValorProgresso");
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao salvar dados");

                if (String.IsNullOrEmpty(messagemErroSalvarItens))
                {
                    messagemErroSalvarItens = ". Porém, houve erro ao salvar um ou mais itens e eventos. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                }
                else
                {
                    messagemErroSalvarItens = ". Porém, houve erro ao salvar um ou mais eventos. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                }
            }

            // Se não for uma nova ordem de serviço, varre as inconsistencias anteriores da ordem de serviço e verifica se esses itens ainda existem, caso contrário, exclui os inconsistencias da database
            if (!ehNovaOrdemServico)
            {
                try
                {
                    ObservableCollection<InconsistenciaOrdemServico> listaInconsistenciasAnterioresOrdemServico = new();

                    await InconsistenciaOrdemServico.PreencheListaInconsistenciasOrdemServicoAsync(listaInconsistenciasAnterioresOrdemServico, true, false, null,
                        CancellationToken.None, "WHERE inos.id_ordem_servico = @id_ordem_servico", "@id_ordem_servico", OrdemServico.Id);

                    foreach (var item in listaInconsistenciasAnterioresOrdemServico)
                    {
                        if (!OrdemServico.ListaInconsistenciasOrdemServico.Where(it => it.Id == item.Id).Any())
                        {
                            await item.DeletarInconsistenciaOrdemServicoDatabaseAsync(CancellationToken.None);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            try
            {
                foreach (var item in OrdemServico.ListaInconsistenciasOrdemServico)
                {
                    item.OrdemServico = OrdemServico;

                    await item.SalvarInconsistenciaOrdemServicoDatabaseAsync(CancellationToken.None);

                    contadorItens++;

                    Messenger.Default.Send<double>((double)contadorItens / (double)totalItens * (double)100, "ValorProgresso");
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao salvar dados");

                if (String.IsNullOrEmpty(messagemErroSalvarItens))
                {
                    messagemErroSalvarItens = ". Porém, houve erro ao salvar um ou mais itens, eventos ou inconsistencias. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                }
                else
                {
                    messagemErroSalvarItens = ". Porém, houve erro ao salvar uma ou mais inconsistencias. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                }
            }

            _ehCopia = false;
            ProgressoEhIndeterminavel = false;
            ValorProgresso = (double)0;
            MensagemStatus = "Ordem de serviço salva" + messagemErroSalvarItens;

            Name = OrdemServico.Cliente.Nome + " - " + OrdemServico.OrdemServicoAtual;

            if (OrdemServico.EtapasConcluidas == 3)
            {
                TextoSalvar = "Salvar e finalizar";
                IconeSalvar = "CheckCircle";
            }

            if (OrdemServico.EtapasConcluidas == 4)
            {
                OrdemServico.IdUsuarioEmUso = null;

                await OrdemServico.AtualizaIdUsuarioEmUsoAsync(CancellationToken.None);

                await CarregarOrdemServico();
                return;
            }

            EhConstrutor = 0;
            OrdemServico.EtapasConcluidas++;
            ControlesHabilitados = true;
            PermiteSalvar = true;
            CancelarVisivel = false;
            PermiteCancelar = false;
            PermiteCancelarEdicao = permiteCancelarEdicaoAnterior;
        }

        private async Task Editar()
        {
            // Retorna o id do usuário em uso
            await OrdemServico.GetIdUsuarioEmUsoAsync(CancellationToken.None);

            // Verifica se o id do usuário em uso não é nulo e, caso verdadeiro, impede a edição
            if (OrdemServico.IdUsuarioEmUso != null && OrdemServico.IdUsuarioEmUso != App.Usuario.Id)
            {
                // Define o usuário
                Usuario usuarioEmUso = new();
                await usuarioEmUso.GetUsuarioDatabaseAsync(OrdemServico.IdUsuarioEmUso, CancellationToken.None);

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Ordem de serviço em uso", "A ordem de serviço em questão está sendo editada por " + usuarioEmUso.Nome + ", portanto não será possível edita-la. Aguarde o usuário finalizar a edição da ordem de serviço para poder edita-la.", MessageDialogStyle.Affirmative, mySettings);

                usuarioEmUso = null;

                return;
            }

            ControlesHabilitados = true;
            CancelarVisivel = false;
            PermiteCancelar = false;

            CancelarEdicaoVisivel = true;
            PermiteCancelarEdicao = true;

            PermiteSalvar = true;
            PermiteEditar = false;
            PermiteCopiar = false;
            PermiteDeletar = false;
            PermiteVisualizar = false;

            OrdemServico.UsuarioEdicao = (Usuario)App.Usuario.Clone();
            OrdemServico.DataEdicao = DateTime.Now;
            OrdemServico.IdUsuarioEmUso = App.Usuario.Id;

            await OrdemServico.AtualizaIdUsuarioEmUsoAsync(CancellationToken.None);
        }

        private async Task Deletar()
        {
            // Retorna o id do usuário em uso
            await OrdemServico.GetIdUsuarioEmUsoAsync(CancellationToken.None);

            // Verifica se o id do usuário em uso não é nulo e, caso verdadeiro, impede a edição
            if (OrdemServico.IdUsuarioEmUso != null && OrdemServico.IdUsuarioEmUso != App.Usuario.Id)
            {
                // Define o usuário
                Usuario usuarioEmUso = new();
                await usuarioEmUso.GetUsuarioDatabaseAsync(OrdemServico.IdUsuarioEmUso, CancellationToken.None);

                var mySettingsUso = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagemUso = await _dialogCoordinator.ShowMessageAsync(this,
                        "Ordem de serviço em uso", "A ordem de serviço em questão está sendo editada por " + usuarioEmUso.Nome + ", portanto não será possível deleta-la. Aguarde o usuário finalizar a edição da ordem de serviço para poder deleta-la.", MessageDialogStyle.Affirmative, mySettingsUso);

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
                "Atenção", "Tem certeza que deseja excluir a ordem de serviço '" + OrdemServico.Cliente.Nome + " - " + OrdemServico.OrdemServicoAtual + "' e seus respectivos itens/eventos? " +
                "O processo não poderá ser desfeito.",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem != MessageDialogResult.Affirmative)
            {
                return;
            }

            ProgressoEhIndeterminavel = true;
            ValorProgresso = (double)0;
            MensagemStatus = "Deletando a ordem de serviço. Aguarde...";

            bool controlesHabilitadosAnterior = ControlesHabilitados;
            bool permiteSalvarAnterior = PermiteSalvar;
            bool permiteEditarAnterior = PermiteEditar;
            bool permiteCopiarAnterior = PermiteCopiar;
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

                CancelarVisivel = false;
                PermiteCancelar = false;

                ControlesHabilitados = controlesHabilitadosAnterior;
                PermiteSalvar = permiteSalvarAnterior;
                PermiteEditar = permiteEditarAnterior;
                PermiteCopiar = permiteCopiarAnterior;
                PermiteDeletar = permiteDeletarAnterior;
                PermiteVisualizar = permiteVisualizarAnterior;
                PermiteCancelarEdicao = permiteCancelarEdicaoAnterior;
                return;
            }

            try
            {
                await OrdemServico.DeletarOrdemServicoDatabaseAsync(_cts.Token, true, true);
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
                        MensagemStatus = "Erro ao excluir a ordem de serviço. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                    }
                }

                ProgressoEhIndeterminavel = false;
                ValorProgresso = (double)0;

                CancelarVisivel = false;
                PermiteCancelar = false;
                PermiteSalvar = permiteSalvarAnterior;
                ControlesHabilitados = controlesHabilitadosAnterior;
                PermiteEditar = permiteEditarAnterior;
                PermiteCopiar = permiteCopiarAnterior;
                PermiteDeletar = permiteDeletarAnterior;
                PermiteVisualizar = permiteVisualizarAnterior;
                PermiteCancelarEdicao = permiteCancelarEdicaoAnterior;
                return;
            }

            ProgressoEhIndeterminavel = false;
            ValorProgresso = (double)0;
            MensagemStatus = "Ordem de serviço deletada";

            var mySettings2 = new MetroDialogSettings
            {
                AffirmativeButtonText = "OK"
            };

            var respostaMensagem2 = await _dialogCoordinator.ShowMessageAsync(this,
                    "Ordem de serviço deletada", "A Ordem de serviço foi deletada com sucesso. Esta página será fechada ao clicar em 'OK'", MessageDialogStyle.Affirmative, mySettings2);

            Messenger.Default.Send<IPageViewModel>(this, "PrincipalPaginaRemover");
        }

        private void Visualizar()
        {
            //ProgressoEhIndeterminavel = true;
            //ValorProgresso = (double)0;
            //MensagemStatus = "Abrindo visualização da proposta";

            //var win = new MetroWindow();
            //win.Height = 802;
            //win.Width = 956;
            //win.Content = new VisualizarPropostaViewModel(DialogCoordinator.Instance, Proposta);
            //win.Title = "Visualização da proposta " + Proposta.Cliente?.Nome + " - " + Proposta.CodigoProposta;
            //win.ShowDialogsOverTitleBar = false;
            //win.Owner = App.Current.MainWindow;
            //win.Show();

            //ProgressoEhIndeterminavel = true;
            //ValorProgresso = (double)0;
            //MensagemStatus = "";
        }

        private async Task CancelarEdicao()
        {
            OrdemServico.IdUsuarioEmUso = null;

            await OrdemServico.AtualizaIdUsuarioEmUsoAsync(CancellationToken.None);

            await CarregarOrdemServico();
        }

        private void Cancelar()
        {
            if (_cts != null)
                _cts.Cancel();
        }

        private async Task VerificaExistenciaOrdemServicoAtual()
        {
            if (await OrdemServico.OrdemServicoAtualExiste(CancellationToken.None, OrdemServicoAtual))
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Ordem de serviço duplicada", "A ordem de serviço informada já existe na database. Não é permitida a inserção de ordem de serviços duplicadas", MessageDialogStyle.Affirmative, mySettings);

                OrdemServicoAtual = OrdemServico.OrdemServicoAtual;

                return;
            }
        }

        private async Task VerificaExistenciaOrdemServicoPrimaria()
        {
            if (!await OrdemServico.OrdemServicoPrimariaExiste(CancellationToken.None, OrdemServicoPrimaria))
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Ordem de serviço não encontrada", "A ordem de serviço primária informada não existe na database. Não é permitido inserir ordens de serviço primárias inexistentes", MessageDialogStyle.Affirmative, mySettings);

                OrdemServicoPrimaria = OrdemServico.OrdemServicoPrimaria;

                return;
            }
        }

        private bool ArquivoOrdemServicoExiste(int? ordemServico)
        {
            if (ordemServico == null)
            {
                return false;
            }

            return File.Exists(_configuracaoSistema.LocalOrdensServico.ToString() + "\\" + ordemServico.ToString() + ".pdf");
        }

        private async Task CopiarOrdemServico(CopiarOrdemServicoViewModel.TipoCopia tipoCopia)
        {
            try
            {
                var customDialog = new CustomDialog();

                var dataContext = new CopiarOrdemServicoViewModel(OrdemServico, tipoCopia, instance =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                });

                customDialog.Content = new CopiarOrdemServicoView { DataContext = dataContext };

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

        public bool FiltraSeries(object item)
        {
            if (OrdemServico?.Frota == null)
            {
                return false;
            }
            if (item is Serie serie)
            {
                return serie.Frota.Id == OrdemServico.Frota.Id;
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

        public bool LimpaListaSeries(object item)
        {
            return false;
        }

        public bool LimpaListaModelos(object item)
        {
            return false;
        }

        private void VerificaDataSaida()
        {
            if (OrdemServico.DataSaida != null && OrdemServico.DataChegada != null)
            {
                if (OrdemServico.DataSaida > OrdemServico.DataChegada)
                {
                    OrdemServico.DataSaida = null;
                }
            }
        }

        private void VerificaDataChegada()
        {
            if (OrdemServico.DataSaida != null && OrdemServico.DataChegada != null)
            {
                if (OrdemServico.DataChegada < OrdemServico.DataSaida)
                {
                    OrdemServico.DataChegada = null;
                }
            }

            if (OrdemServico.DataSaida != null && OrdemServico.DataRetorno != null)
            {
                if (OrdemServico.DataChegada > OrdemServico.DataRetorno)
                {
                    OrdemServico.DataChegada = null;
                }
            }
        }

        private void VerificaDataRetorno()
        {
            if (OrdemServico.DataChegada != null && OrdemServico.DataRetorno != null)
            {
                if (OrdemServico.DataRetorno < OrdemServico.DataChegada)
                {
                    OrdemServico.DataRetorno = null;
                }
            }
        }

        private async Task AbrirOrdemServico(int? ordemServico)
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

        private void AlteraConjuntoUnico(Conjunto conjunto)
        {
            //ItemPropostaSelecionado.Especificacao = null;

            //if (conjunto.Id == 0)
            //    ItemPropostaSelecionado.Conjunto = null;
            //else
            //    ItemPropostaSelecionado.Conjunto = conjunto;
            foreach (var item in OrdemServico.ListaItensOrdemServico)
            {
                if (ListaItensOrdemServicoSelecionados.Contains(item))
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
            foreach (var item in OrdemServico.ListaItensOrdemServico)
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
            foreach (var item in OrdemServico.ListaItensOrdemServico)
            {
                if (ListaItensOrdemServicoSelecionados.Contains(item))
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
            foreach (var item in OrdemServico.ListaItensOrdemServico)
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

        #endregion Métodos

        #region Interfaces

        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public bool HasErrors => _errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
                _errorsByPropertyName[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        #endregion Interfaces

        #region Validations

        private void ExecuteAllValidations()
        {
            ValidateLoginOrEmail();
            ValidateUserPassword();
        }

        private void ValidateLoginOrEmail()
        {
            //ClearErrors(nameof(LoginOrEmail));
            //if (!IntegratedAuthentication)
            //{
            //    if (string.IsNullOrWhiteSpace(LoginOrEmail) || string.IsNullOrEmpty(LoginOrEmail))
            //        AddError(nameof(LoginOrEmail), "Campo obrigatório");
            //}
        }

        private void ValidateUserPassword()
        {
            //ClearErrors(nameof(UserPassword));
            //if (!IntegratedAuthentication)
            //{
            //    if (string.IsNullOrWhiteSpace(UserPassword) || string.IsNullOrEmpty(UserPassword))
            //        AddError(nameof(UserPassword), "Campo obrigatório");
            //}
        }

        #endregion Validations
    }
}