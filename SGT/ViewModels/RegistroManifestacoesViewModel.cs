using ByteSizeLib;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;
using Model.Email;
using Ookii.Dialogs.Wpf;
using SGT.HelperClasses;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class RegistroManifestacoesViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private RegistroManifestacao _registroManifestacao;
        private StatusManifestacao _statusManifestacaoSelecionado;
        private ImagemRegistroManifestacao _imagemSelecionada;
        private LogRegistroManifestacao _logSelecionado;
        private ComentarioRegistroManifestacao _comentarioSelecionado;
        private CancellationTokenSource _cts;

        private bool _controlesHabilitados;
        private bool _progressoEhIndeterminavel = true;
        private bool _progressoVisivel;
        private double _valorProgresso = 0;
        private string _textoProgresso;
        private string _mensagemStatus;

        private bool ehDesenvolvedor;

        private bool _statusHabilitado;

        private bool _salvarVisivel;
        private bool _deletarVisivel;
        private bool _cancelarVisivel;

        private bool _permiteSalvar;
        private bool _permiteEditar;
        private bool _permiteCancelar;

        private bool _permiteDeletarImagem;
        private bool _permiteDeletarLog;
        private bool _permiteDeletarComentario;

        private bool _dadosFechamentoVisivel;

        private string _textoTamanhoTotalImagens;
        private string _textoTamanhoTotalLogs;
        private bool _tamanhoExcedido;
        private bool _tamanhoLogExcedido;

        private bool _carregamentoVisivel = true;

        private readonly IDialogCoordinator _dialogCoordinator;

        private ObservableCollection<PrioridadeManifestacao> _listaPrioridadesManifestacao = new();
        private ObservableCollection<TipoManifestacao> _listaTiposManifestacao = new();
        private ObservableCollection<StatusManifestacao> _listaStatusManifestacao = new();

        private ObservableCollection<ComentarioRegistroManifestacao> _listaComentarios = new();
        private ObservableCollection<ImagemRegistroManifestacao> _listaImagens = new();
        private ObservableCollection<LogRegistroManifestacao> _listaLogs = new();
        private ObservableCollection<HistoricoManifestacao> _listaHistorico = new();

        private ICommand _comandoSalvar;
        private ICommand _comandoEditar;
        private ICommand _comandoCancelar;

        private ICommand _comandoAdicionarImagem;
        private ICommand _comandoDeletarImagem;

        private ICommand _comandoVisualizarImagem;
        private ICommand _comandoSalvarImagem;

        private ICommand _comandoAdicionarLog;
        private ICommand _comandoDeletarLog;

        private ICommand _comandoVisualizarLog;
        private ICommand _comandoSalvarLog;

        private ICommand _comandoAdicionarComentario;
        private ICommand _comandoDeletarComentario;

        #endregion Campos

        #region Construtores

        public RegistroManifestacoesViewModel(IDialogCoordinator dialogCoordinator, RegistroManifestacao? registroManifestacao = null)
        {
            _dialogCoordinator = dialogCoordinator;

            ConstrutorAsync(registroManifestacao).Await();
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Requisição";
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
                _registroManifestacao = null;
                _statusManifestacaoSelecionado = null;
                _imagemSelecionada = null;
                _logSelecionado = null;
                _comentarioSelecionado = null;
                _cts = null;

                _listaPrioridadesManifestacao = null;
                _listaTiposManifestacao = null;
                _listaStatusManifestacao = null;

                _listaComentarios = null;
                _listaImagens = null;
                _listaLogs = null;
                _listaHistorico = null;

                _comandoSalvar = null;
                _comandoEditar = null;
                _comandoCancelar = null;

                _comandoAdicionarImagem = null;
                _comandoDeletarImagem = null;

                _comandoVisualizarImagem = null;
                _comandoSalvarImagem = null;

                _comandoAdicionarLog = null;
                _comandoDeletarLog = null;

                _comandoVisualizarLog = null;
                _comandoSalvarLog = null;

                _comandoAdicionarComentario = null;
                _comandoDeletarComentario = null;
            }
            catch (Exception)
            {
            }
        }

        public Window Janela { private get; set; }

        public bool ExistemCamposVazios { private get; set; }

        public RegistroManifestacao RegistroManifestacao
        {
            get { return _registroManifestacao; }
            set
            {
                _registroManifestacao = value;
                OnPropertyChanged(nameof(RegistroManifestacao));
            }
        }

        public StatusManifestacao StatusManifestacaoSelecionado
        {
            get { return _statusManifestacaoSelecionado; }
            set
            {
                _statusManifestacaoSelecionado = value;
                OnPropertyChanged(nameof(StatusManifestacaoSelecionado));
                RegistroManifestacao.StatusManifestacao = StatusManifestacaoSelecionado;
                switch (StatusManifestacaoSelecionado.Id)
                {
                    case 4:
                        DadosFechamentoVisivel = true;
                        break;

                    case 5:
                        DadosFechamentoVisivel = !String.IsNullOrEmpty(RegistroManifestacao.DescricaoFechamento);
                        break;

                    default:
                        break;
                }
            }
        }

        public ImagemRegistroManifestacao ImagemSelecionada
        {
            get { return _imagemSelecionada; }
            set
            {
                _imagemSelecionada = value;
                OnPropertyChanged(nameof(ImagemSelecionada));

                PermiteDeletarImagem = ImagemSelecionada != null;
            }
        }

        public LogRegistroManifestacao LogSelecionado
        {
            get { return _logSelecionado; }
            set
            {
                _logSelecionado = value;
                OnPropertyChanged(nameof(LogSelecionado));

                PermiteDeletarLog = LogSelecionado != null;
            }
        }

        public ComentarioRegistroManifestacao ComentarioSelecionado
        {
            get { return _comentarioSelecionado; }
            set
            {
                _comentarioSelecionado = value;
                OnPropertyChanged(nameof(ComentarioSelecionado));

                PermiteDeletarComentario = ComentarioSelecionado != null;
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

        public bool PermiteDeletarComentario
        {
            get { return _permiteDeletarComentario; }
            set
            {
                if (value != _permiteDeletarComentario)
                {
                    _permiteDeletarComentario = value;
                    OnPropertyChanged(nameof(PermiteDeletarComentario));
                }
            }
        }

        public bool PermiteDeletarImagem
        {
            get { return _permiteDeletarImagem; }
            set
            {
                if (value != _permiteDeletarImagem)
                {
                    _permiteDeletarImagem = value;
                    OnPropertyChanged(nameof(PermiteDeletarImagem));
                }
            }
        }

        public bool PermiteDeletarLog
        {
            get { return _permiteDeletarLog; }
            set
            {
                if (value != _permiteDeletarLog)
                {
                    _permiteDeletarLog = value;
                    OnPropertyChanged(nameof(PermiteDeletarLog));
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

        public string TextoTamanhoTotalImagens
        {
            get { return _textoTamanhoTotalImagens; }
            set
            {
                if (value != _textoTamanhoTotalImagens)
                {
                    _textoTamanhoTotalImagens = value;
                    OnPropertyChanged(nameof(TextoTamanhoTotalImagens));
                }
            }
        }

        public string TextoTamanhoTotalLogs
        {
            get { return _textoTamanhoTotalLogs; }
            set
            {
                if (value != _textoTamanhoTotalLogs)
                {
                    _textoTamanhoTotalLogs = value;
                    OnPropertyChanged(nameof(TextoTamanhoTotalLogs));
                }
            }
        }

        public bool TamanhoExcedido
        {
            get { return _tamanhoExcedido; }
            set
            {
                if (value != _tamanhoExcedido)
                {
                    _tamanhoExcedido = value;
                    OnPropertyChanged(nameof(TamanhoExcedido));
                }
            }
        }

        public bool TamanhoLogExcedido
        {
            get { return _tamanhoLogExcedido; }
            set
            {
                if (value != _tamanhoLogExcedido)
                {
                    _tamanhoLogExcedido = value;
                    OnPropertyChanged(nameof(TamanhoLogExcedido));
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

        public bool StatusHabilitado
        {
            get { return _statusHabilitado; }
            set
            {
                if (value != _statusHabilitado)
                {
                    _statusHabilitado = value;
                    OnPropertyChanged(nameof(StatusHabilitado));
                }
            }
        }

        public bool DadosFechamentoVisivel
        {
            get { return _dadosFechamentoVisivel; }
            set
            {
                if (value != _dadosFechamentoVisivel)
                {
                    _dadosFechamentoVisivel = value;
                    OnPropertyChanged(nameof(DadosFechamentoVisivel));
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

        public ObservableCollection<PrioridadeManifestacao> ListaPrioridadesManifestacao
        {
            get { return _listaPrioridadesManifestacao; }
            set
            {
                if (value != _listaPrioridadesManifestacao)
                {
                    _listaPrioridadesManifestacao = value;
                    OnPropertyChanged(nameof(ListaPrioridadesManifestacao));
                }
            }
        }

        public ObservableCollection<TipoManifestacao> ListaTiposManifestacao
        {
            get { return _listaTiposManifestacao; }
            set
            {
                if (value != _listaTiposManifestacao)
                {
                    _listaTiposManifestacao = value;
                    OnPropertyChanged(nameof(ListaTiposManifestacao));
                }
            }
        }

        public ObservableCollection<StatusManifestacao> ListaStatusManifestacao
        {
            get { return _listaStatusManifestacao; }
            set
            {
                if (value != _listaStatusManifestacao)
                {
                    _listaStatusManifestacao = value;
                    OnPropertyChanged(nameof(ListaStatusManifestacao));
                }
            }
        }

        public ObservableCollection<ComentarioRegistroManifestacao> ListaComentarios
        {
            get { return _listaComentarios; }
            set
            {
                if (value != _listaComentarios)
                {
                    _listaComentarios = value;
                    OnPropertyChanged(nameof(ListaComentarios));
                }
            }
        }

        public ObservableCollection<ImagemRegistroManifestacao> ListaImagens
        {
            get { return _listaImagens; }
            set
            {
                if (value != _listaImagens)
                {
                    _listaImagens = value;
                    OnPropertyChanged(nameof(ListaImagens));
                }
            }
        }

        public ObservableCollection<LogRegistroManifestacao> ListaLogs
        {
            get { return _listaLogs; }
            set
            {
                if (value != _listaLogs)
                {
                    _listaLogs = value;
                    OnPropertyChanged(nameof(ListaLogs));
                }
            }
        }

        public ObservableCollection<HistoricoManifestacao> ListaHistorico
        {
            get { return _listaHistorico; }
            set
            {
                if (value != _listaHistorico)
                {
                    _listaHistorico = value;
                    OnPropertyChanged(nameof(ListaHistorico));
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

        public ICommand ComandoAdicionarImagem
        {
            get
            {
                if (_comandoAdicionarImagem == null)
                {
                    _comandoAdicionarImagem = new RelayCommand(
                        param => AdicionarImagem(),
                        param => true
                    );
                }
                return _comandoAdicionarImagem;
            }
        }

        public ICommand ComandoVisualizarImagem
        {
            get
            {
                if (_comandoVisualizarImagem == null)
                {
                    _comandoVisualizarImagem = new RelayCommand(
                        param => VisualizarImagem(),
                        param => true
                    );
                }
                return _comandoVisualizarImagem;
            }
        }

        public ICommand ComandoSalvarImagem
        {
            get
            {
                if (_comandoSalvarImagem == null)
                {
                    _comandoSalvarImagem = new RelayCommand(
                        param => SalvarImagem(),
                        param => true
                    );
                }
                return _comandoSalvarImagem;
            }
        }

        public ICommand ComandoDeletarImagem
        {
            get
            {
                if (_comandoDeletarImagem == null)
                {
                    _comandoDeletarImagem = new RelayCommand(
                        param => DeletarImagem(),
                        param => true
                    );
                }
                return _comandoDeletarImagem;
            }
        }

        public ICommand ComandoAdicionarLog
        {
            get
            {
                if (_comandoAdicionarLog == null)
                {
                    _comandoAdicionarLog = new RelayCommand(
                        param => AdicionarLog(),
                        param => true
                    );
                }
                return _comandoAdicionarLog;
            }
        }

        public ICommand ComandoVisualizarLog
        {
            get
            {
                if (_comandoVisualizarLog == null)
                {
                    _comandoVisualizarLog = new RelayCommand(
                        param => VisualizarLog(),
                        param => true
                    );
                }
                return _comandoVisualizarLog;
            }
        }

        public ICommand ComandoSalvarLog
        {
            get
            {
                if (_comandoSalvarLog == null)
                {
                    _comandoSalvarLog = new RelayCommand(
                        param => SalvarLog(),
                        param => true
                    );
                }
                return _comandoSalvarLog;
            }
        }

        public ICommand ComandoDeletarLog
        {
            get
            {
                if (_comandoDeletarLog == null)
                {
                    _comandoDeletarLog = new RelayCommand(
                        param => DeletarLog(),
                        param => true
                    );
                }
                return _comandoDeletarLog;
            }
        }

        public ICommand ComandoAdicionarComentario
        {
            get
            {
                if (_comandoAdicionarComentario == null)
                {
                    _comandoAdicionarComentario = new RelayCommand(
                        param => AdicionarComentario().Await(),
                        param => true
                    );
                }
                return _comandoAdicionarComentario;
            }
        }

        public ICommand ComandoDeletarComentario
        {
            get
            {
                if (_comandoDeletarComentario == null)
                {
                    _comandoDeletarComentario = new RelayCommand(
                        param => DeletarComentario(),
                        param => true
                    );
                }
                return _comandoDeletarComentario;
            }
        }

        #endregion Propriedades/Comandos

        #region Métodos

        public async Task ConstrutorAsync(RegistroManifestacao? registroManifestacao = null)
        {
            // Redefine as permissões
            PermiteSalvar = false;
            PermiteEditar = false;
            PermiteCancelar = false;
            CancelarVisivel = false;
            SalvarVisivel = true;
            ControlesHabilitados = false;

            try
            {
                // Preenche as listas com as classes necessárias
                await PrioridadeManifestacao.PreencheListaPrioridadesManifestacaoAsync(ListaPrioridadesManifestacao, true, null, CancellationToken.None, "", "");
                await TipoManifestacao.PreencheListaTiposManifestacaoAsync(ListaTiposManifestacao, true, null, CancellationToken.None, "", "");
                await StatusManifestacao.PreencheListaStatusManifestacaoAsync(ListaStatusManifestacao, true, null, CancellationToken.None, "", "");

                var enderecoMacAtual =
                    (
                        from nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                        where nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up
                        select nic.GetPhysicalAddress().ToString()
                    ).FirstOrDefault();

                ehDesenvolvedor = await FuncoesDeDatabase.EhDesenvolvedor(enderecoMacAtual, CancellationToken.None);

                // Retorna o código da instância atual
                InstanciaLocal instanciaLocal = new();
                await instanciaLocal.GetInstanciaLocal(CancellationToken.None);

                // Se o registro da manifestação não foi informado, cria uma nova instância de registro da manifestação
                if (registroManifestacao == null)
                {
                    RegistroManifestacao = new();
                    RegistroManifestacao.DataAbertura = DateTime.Now;
                    RegistroManifestacao.NomePessoaAbertura = App.Usuario.Nome;
                    RegistroManifestacao.EmailPessoaAbertura = App.Usuario.Email;
                    RegistroManifestacao.CodigoInstancia = instanciaLocal.CodigoInstancia;
                    RegistroManifestacao.IdPessoaAbertura = App.Usuario.Id;

                    // Define valores
                    try
                    {
                        RegistroManifestacao.PrioridadeManifestacao = ListaPrioridadesManifestacao.First(prma => prma.Id == 1);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        RegistroManifestacao.TipoManifestacao = ListaTiposManifestacao.First(tima => tima.Id == 1);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        StatusManifestacaoSelecionado = ListaStatusManifestacao.First(stma => stma.Id == 1);
                    }
                    catch (Exception)
                    {
                    }

                    DadosFechamentoVisivel = false;
                    StatusHabilitado = false;
                    SalvarVisivel = true;
                    PermiteSalvar = true;
                    PermiteEditar = false;
                    PermiteCancelar = false;
                    CancelarVisivel = false;
                    ControlesHabilitados = true;
                }
                else
                {
                    RegistroManifestacao = registroManifestacao;

                    await CarregarRegistroManifestacao();
                }
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");

                MensagemStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                PermiteSalvar = false;
                PermiteEditar = false;
                PermiteCancelar = false;
                CancelarVisivel = false;
                SalvarVisivel = true;
                ControlesHabilitados = false;
            }

            CarregamentoVisivel = false;
        }

        private async Task CarregarRegistroManifestacao()
        {
            try
            {
                await RegistroManifestacao.GetRegistroManifestacaoDatabaseAsync(RegistroManifestacao.Id, CancellationToken.None);
                await ComentarioRegistroManifestacao.PreencheListaComentariosManifestacaoAsync(ListaComentarios, true, null,
                    CancellationToken.None, "WHERE corm.id_registro_manifestacao = @id_registro_manifestacao",
                    "@id_registro_manifestacao", RegistroManifestacao.Id);
                await ImagemRegistroManifestacao.PreencheListaImagensManifestacaoAsync(ListaImagens, true, null,
                    CancellationToken.None, "WHERE imrm.id_registro_manifestacao = @id_registro_manifestacao",
                    "@id_registro_manifestacao", RegistroManifestacao.Id);
                await LogRegistroManifestacao.PreencheListaLogsManifestacaoAsync(ListaLogs, true, null,
                        CancellationToken.None, "WHERE logrm.id_registro_manifestacao = @id_registro_manifestacao",
                        "@id_registro_manifestacao", RegistroManifestacao.Id);
                await HistoricoManifestacao.PreencheListaHistoricoManifestacaoAsync(ListaHistorico, true, null,
                    CancellationToken.None, "WHERE hima.id_registro_manifestacao = @id_registro_manifestacao",
                    "@id_registro_manifestacao", RegistroManifestacao.Id);

                // Retorna o código da instância atual
                InstanciaLocal instanciaLocal = new();
                await instanciaLocal.GetInstanciaLocal(CancellationToken.None);

                // Define valores
                try
                {
                    RegistroManifestacao.PrioridadeManifestacao = ListaPrioridadesManifestacao.First(prma => prma.Id == RegistroManifestacao.PrioridadeManifestacao.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    RegistroManifestacao.TipoManifestacao = ListaTiposManifestacao.First(tima => tima.Id == RegistroManifestacao.TipoManifestacao.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    StatusManifestacaoSelecionado = ListaStatusManifestacao.First(stma => stma.Id == RegistroManifestacao.StatusManifestacao.Id);
                }
                catch (Exception)
                {
                }

                StatusHabilitado = true;
                DadosFechamentoVisivel = RegistroManifestacao?.StatusManifestacao?.Id == 4;

                SalvarVisivel = true;
                PermiteSalvar = false;

                if (ehDesenvolvedor)
                {
                    PermiteEditar = true;
                }
                else
                {
                    PermiteEditar = RegistroManifestacao.CodigoInstancia == instanciaLocal.CodigoInstancia && RegistroManifestacao.IdPessoaAbertura == App.Usuario.Id;
                }

                PermiteCancelar = false;
                CancelarVisivel = false;
                ControlesHabilitados = false;
            }
            catch (Exception)
            {
            }
        }

        private void Editar()
        {
            ControlesHabilitados = true;
            SalvarVisivel = true;
            CancelarVisivel = true;
            PermiteEditar = false;
            PermiteCancelar = true;
            PermiteSalvar = true;
            StatusHabilitado = ehDesenvolvedor;
        }

        private void Cancelar()
        {
            if (_cts != null)
                _cts.Cancel();

            ControlesHabilitados = false;
            SalvarVisivel = true;
            CancelarVisivel = false;
            PermiteEditar = true;
            PermiteCancelar = false;
            PermiteSalvar = false;
        }

        private async Task SalvarAsync()
        {
            if (ExistemCamposVazios)
            {
                MensagemStatus = "Campos obrigatórios vazios/inválidos";
                return;
            }

            if (TamanhoExcedido)
            {
                MensagemStatus = "Tamanho máximo de imagens (3mb) excedido. Exclua alguma imagem e tente novamente";
                return;
            }

            if (TamanhoLogExcedido)
            {
                MensagemStatus = "Tamanho máximo de logs (3mb) excedido. Exclua alguma log e tente novamente";
                return;
            }

            bool permiteSalvarAnterior = PermiteSalvar;
            bool permiteEditarAnterior = PermiteEditar;

            _cts = new();

            ValorProgresso = 0;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = true;
            MensagemStatus = "Salvando requisição, aguarde...";
            CancelarVisivel = true;
            PermiteCancelar = true;
            PermiteSalvar = false;
            PermiteEditar = false;

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
                ControlesHabilitados = true;
                CancelarVisivel = false;
                PermiteCancelar = false;
                PermiteSalvar = permiteSalvarAnterior;
                PermiteEditar = permiteEditarAnterior;

                return;
            }

            bool ehNova = RegistroManifestacao.Id == null;

            try
            {
                await RegistroManifestacao.SalvarRegistroManifestacaoDatabaseAsync(_cts.Token, App.Usuario.Nome, App.Usuario.Email);
            }
            catch (Exception ex)
            {
                ValorProgresso = 0;
                ControlesHabilitados = true;
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = true;

                ControlesHabilitados = true;
                CancelarVisivel = false;
                PermiteCancelar = false;
                PermiteSalvar = permiteSalvarAnterior;
                PermiteEditar = permiteEditarAnterior;

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

                return;
            }

            // Se a manifestação não for nova, verifica se há itens removidos da lista e deleta da database
            if (!ehNova)
            {
                try
                {
                    ObservableCollection<ComentarioRegistroManifestacao> listaComentariosAnteriores = new();

                    await ComentarioRegistroManifestacao.PreencheListaComentariosManifestacaoAsync(listaComentariosAnteriores, true, null,
                        CancellationToken.None, "WHERE corm.id_registro_manifestacao = @id_registro_manifestacao", "@id_registro_manifestacao", RegistroManifestacao.Id);

                    foreach (var item in listaComentariosAnteriores)
                    {
                        if (!ListaComentarios.Where(it => it.Id == item.Id).Any())
                        {
                            await item.DeletaComentarioManifestacaoDatabaseAsync(CancellationToken.None, App.Usuario.Nome, App.Usuario.Email);
                        }
                    }

                    ObservableCollection<ImagemRegistroManifestacao> listaImagensAnteriores = new();

                    await ImagemRegistroManifestacao.PreencheListaImagensManifestacaoAsync(listaImagensAnteriores, true, null,
                        CancellationToken.None, "WHERE imrm.id_registro_manifestacao = @id_registro_manifestacao", "@id_registro_manifestacao", RegistroManifestacao.Id);

                    foreach (var item in listaImagensAnteriores)
                    {
                        if (!ListaImagens.Where(it => it.Id == item.Id).Any())
                        {
                            await item.DeletaImagemManifestacaoDatabaseAsync(CancellationToken.None, App.Usuario.Nome, App.Usuario.Email);
                        }
                    }

                    ObservableCollection<LogRegistroManifestacao> listaLogsAnteriores = new();

                    await LogRegistroManifestacao.PreencheListaLogsManifestacaoAsync(listaLogsAnteriores, true, null,
                        CancellationToken.None, "WHERE logrm.id_registro_manifestacao = @id_registro_manifestacao", "@id_registro_manifestacao", RegistroManifestacao.Id);

                    foreach (var item in listaLogsAnteriores)
                    {
                        if (!ListaLogs.Where(it => it.Id == item.Id).Any())
                        {
                            await item.DeletaLogManifestacaoDatabaseAsync(CancellationToken.None, App.Usuario.Nome, App.Usuario.Email);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            try
            {
                foreach (var item in ListaComentarios)
                {
                    if (item.Id == null)
                    {
                        item.IdRegistroManifestacao = RegistroManifestacao.Id;

                        await item.SalvarComentarioManifestacaoDatabaseAsync(CancellationToken.None);
                    }
                }
            }
            catch (Exception)
            {
            }

            try
            {
                foreach (var item in ListaImagens)
                {
                    if (item.Id == null)
                    {
                        item.IdRegistroManifestacao = RegistroManifestacao.Id;

                        await item.SalvarImagemManifestacaoDatabaseAsync(CancellationToken.None);
                    }
                }
            }
            catch (Exception)
            {
            }

            try
            {
                foreach (var item in ListaLogs)
                {
                    if (item.Id == null)
                    {
                        item.IdRegistroManifestacao = RegistroManifestacao.Id;

                        await item.SalvarLogManifestacaoDatabaseAsync(CancellationToken.None);
                    }
                }
            }
            catch (Exception)
            {
            }

            try
            {
                await Email.EnviarEmailManifestacao(ehNova, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(), App.Usuario.Nome, App.Usuario.Email, RegistroManifestacao);
            }
            catch (Exception)
            {
            }

            ValorProgresso = 0;
            ControlesHabilitados = true;
            ProgressoVisivel = false;
            ProgressoEhIndeterminavel = true;

            ControlesHabilitados = false;
            CancelarVisivel = false;
            PermiteCancelar = false;
            PermiteSalvar = false;
            PermiteEditar = true;

            MensagemStatus = "Requisição salva";

            await CarregarRegistroManifestacao();

            if (ehNova)
            {
                try
                {
                    Janela.Title = "Requisição REQN" + ((int)RegistroManifestacao.Id).ToString("00000000");
                }
                catch (Exception)
                {
                }
            }
        }

        private async Task AdicionarComentario()
        {
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Ok",
                NegativeButtonText = "Cancelar"
            };

            var comentarioAdicionado = await _dialogCoordinator.ShowInputAsync(this,
                "Novo comentário", "", mySettings);

            if (!String.IsNullOrEmpty(comentarioAdicionado))
            {
                ListaComentarios.Add(new ComentarioRegistroManifestacao()
                {
                    DataInsercao = DateTime.Now,
                    NomePessoaInsercao = App.Usuario.Nome,
                    EmailPessoaInsercao = App.Usuario.Email,
                    Comentario = comentarioAdicionado
                });
            }
        }

        private void DeletarComentario()
        {
            ListaComentarios.Remove(ComentarioSelecionado);
        }

        public static void OpenWithDefaultProgram(string path)
        {
            using System.Diagnostics.Process fileopener = new();

            fileopener.StartInfo.FileName = "explorer";
            fileopener.StartInfo.Arguments = "\"" + path + "\"";
            fileopener.Start();
        }

        private void AdicionarImagem()
        {
            string caminhoArquivo = "";

            VistaOpenFileDialog vistaOpenFileDialog = new VistaOpenFileDialog()
            {
                Filter = "Arquivos de imagem (*.jpg;*.bmp;*.png)|*.jpg;*.bmp;*.png",
                Title = "Selecione um arquivo de imagem",
                //DefaultExt = "jpg",
                //AddExtension = true,
                FilterIndex = 0
            };

            if ((bool)vistaOpenFileDialog.ShowDialog())
            {
                caminhoArquivo = vistaOpenFileDialog.FileName;
            }

            if (String.IsNullOrEmpty(caminhoArquivo))
            {
                return;
            }

            Image img = Image.FromFile(caminhoArquivo);
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }

            ListaImagens.Add(new ImagemRegistroManifestacao()
            {
                DataInsercao = DateTime.Now,
                NomePessoaInsercao = App.Usuario.Nome,
                EmailPessoaInsercao = App.Usuario.Email,
                Imagem = arr,
                TamanhoImagem = arr.Length
            });

            try
            {
                var byteSize = ByteSize.FromBytes(ListaImagens.Sum(d => d.Imagem.Length));

                TamanhoExcedido = byteSize.MegaBytes > 3d;

                TextoTamanhoTotalImagens = "Tamanho total: " + byteSize.ToString();
            }
            catch (Exception)
            {
                TamanhoExcedido = false;
                TextoTamanhoTotalImagens = "Tamanho indeterminado";
            }
        }

        private void DeletarImagem()
        {
            ListaImagens.Remove(ImagemSelecionada);

            try
            {
                var byteSize = ByteSize.FromBytes(ListaImagens.Sum(d => d.Imagem.Length));

                TamanhoExcedido = byteSize.MegaBytes > 3d;

                TextoTamanhoTotalImagens = "Tamanho total: " + byteSize.ToString();
            }
            catch (Exception)
            {
                TamanhoExcedido = false;
                TextoTamanhoTotalImagens = "Tamanho indeterminado";
            }
        }

        private void VisualizarImagem()
        {
            if (ImagemSelecionada?.Imagem != null)
            {
                string pastaSalvar = "";

                pastaSalvar = System.IO.Path.GetTempPath() + "Proreports\\SGT\\Imagens";

                try
                {
                    System.IO.Directory.CreateDirectory(pastaSalvar);

                    string caminhoArquivo = pastaSalvar + "\\imagem_manifestacao.png";

                    using (Image image = Image.FromStream(new MemoryStream(ImagemSelecionada.Imagem)))
                    {
                        image.Save(caminhoArquivo, System.Drawing.Imaging.ImageFormat.Png);  // Or Png
                    }

                    OpenWithDefaultProgram(caminhoArquivo);
                }
                catch (Exception)
                {
                    MensagemStatus = "Não foi possível abrir a imagem";
                }
            }
        }

        private void SalvarImagem()
        {
            if (ImagemSelecionada?.Imagem != null)
            {
                string nomeInicial = "";
                if (RegistroManifestacao?.Id == null)
                {
                    nomeInicial = "Imagem_Requisicao_Nova_Requisicao";
                }
                else
                {
                    nomeInicial = "Imagem_Requisicao_REQN" + ((int)RegistroManifestacao.Id).ToString("00000000");
                }

                string caminhoArquivo = "";

                VistaSaveFileDialog vistaSaveFileDialog = new VistaSaveFileDialog()
                {
                    Filter = "PNG (*.png)|*.png",
                    Title = "Informe o local e o nome do arquivo",
                    DefaultExt = "png",
                    FileName = nomeInicial
                };

                if ((bool)vistaSaveFileDialog.ShowDialog())
                {
                    caminhoArquivo = vistaSaveFileDialog.FileName;
                }

                if (String.IsNullOrEmpty(caminhoArquivo))
                {
                    return;
                }

                try
                {
                    using (Image image = Image.FromStream(new MemoryStream(ImagemSelecionada.Imagem)))
                    {
                        image.Save(caminhoArquivo, System.Drawing.Imaging.ImageFormat.Png);  // Or Png
                    }

                    //OpenWithDefaultProgram(caminhoArquivo);
                }
                catch (Exception)
                {
                    MensagemStatus = "Não foi possível salvar a imagem";
                }
            }
        }

        private void AdicionarLog()
        {
            string caminhoArquivo = "";

            VistaOpenFileDialog vistaOpenFileDialog = new VistaOpenFileDialog()
            {
                Filter = "Arquivos de texto (*.txt)|*.txt",
                Title = "Selecione um arquivo de log",
                //DefaultExt = "jpg",
                //AddExtension = true,
                FilterIndex = 0
            };

            if ((bool)vistaOpenFileDialog.ShowDialog())
            {
                caminhoArquivo = vistaOpenFileDialog.FileName;
            }

            if (String.IsNullOrEmpty(caminhoArquivo))
            {
                return;
            }

            string pastaSalvar = "";

            pastaSalvar = Path.GetTempPath() + "Proreports\\SGT\\Logs";

            Directory.CreateDirectory(pastaSalvar);

            File.Copy(caminhoArquivo, pastaSalvar + "\\arquivo_log_inserir.txt", true);

            byte[] arr;

            using (FileStream fs = new FileStream(pastaSalvar + "\\arquivo_log_inserir.txt", FileMode.Open, FileAccess.Read))
            {
                arr = File.ReadAllBytes(pastaSalvar + "\\arquivo_log_inserir.txt");

                fs.Read(arr, 0, Convert.ToInt32(fs.Length));

                fs.Close();
            }

            ListaLogs.Add(new LogRegistroManifestacao()
            {
                DataInsercao = DateTime.Now,
                NomePessoaInsercao = App.Usuario.Nome,
                EmailPessoaInsercao = App.Usuario.Email,
                ArquivoLog = arr,
                NomeArquivo = Path.GetFileName(caminhoArquivo)
            });

            try
            {
                var byteSize = ByteSize.FromBytes(ListaLogs.Sum(d => d.ArquivoLog.Length));

                TamanhoLogExcedido = byteSize.MegaBytes > 3d;

                TextoTamanhoTotalLogs = "Tamanho total: " + byteSize.ToString();
            }
            catch (Exception)
            {
                TamanhoLogExcedido = false;
                TextoTamanhoTotalLogs = "Tamanho indeterminado";
            }
        }

        private void DeletarLog()
        {
            ListaLogs.Remove(LogSelecionado);

            try
            {
                var byteSize = ByteSize.FromBytes(ListaLogs.Sum(d => d.ArquivoLog.Length));

                TamanhoLogExcedido = byteSize.MegaBytes > 3d;

                TextoTamanhoTotalLogs = "Tamanho total: " + byteSize.ToString();
            }
            catch (Exception)
            {
                TamanhoLogExcedido = false;
                TextoTamanhoTotalLogs = "Tamanho indeterminado";
            }
        }

        private void VisualizarLog()
        {
            if (LogSelecionado?.ArquivoLog != null)
            {
                string pastaSalvar = "";

                pastaSalvar = Path.GetTempPath() + "Proreports\\SGT\\Logs";

                try
                {
                    Directory.CreateDirectory(pastaSalvar);

                    string caminhoArquivo = pastaSalvar + "\\arquivo_log.txt";

                    File.WriteAllBytes(caminhoArquivo, LogSelecionado.ArquivoLog);

                    OpenWithDefaultProgram(caminhoArquivo);
                }
                catch (Exception)
                {
                    MensagemStatus = "Não foi possível abrir o log";
                }
            }
        }

        private void SalvarLog()
        {
            if (LogSelecionado?.ArquivoLog != null)
            {
                string nomeInicial = "";
                if (RegistroManifestacao?.Id == null)
                {
                    nomeInicial = "Log_Requisicao_Nova_Requisicao";
                }
                else
                {
                    nomeInicial = "Log_Requisicao_REQN" + ((int)RegistroManifestacao.Id).ToString("00000000");
                }

                string caminhoArquivo = "";

                VistaSaveFileDialog vistaSaveFileDialog = new VistaSaveFileDialog()
                {
                    Filter = "Arquivo de texto (*.txt)|*.txt",
                    Title = "Informe o local e o nome do arquivo",
                    DefaultExt = "txt",
                    FileName = nomeInicial
                };

                if ((bool)vistaSaveFileDialog.ShowDialog())
                {
                    caminhoArquivo = vistaSaveFileDialog.FileName;
                }

                if (String.IsNullOrEmpty(caminhoArquivo))
                {
                    return;
                }

                try
                {
                    File.WriteAllBytes(caminhoArquivo, LogSelecionado.ArquivoLog);

                    //OpenWithDefaultProgram(caminhoArquivo);
                }
                catch (Exception)
                {
                    MensagemStatus = "Não foi possível salvar o arquivo de log";
                }
            }
        }

        #endregion Métodos
    }
}