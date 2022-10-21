using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
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
using System.Windows;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class PesquisarRegistroManifestacoesViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private string _name;
        private string _textoLimiteDeResultados;
        private string _textoPesquisa;
        private readonly IDialogCoordinator _dialogCoordinator;
        private ObservableCollection<Parametro> _listaClassificarPor = new();
        private ObservableCollection<Parametro> _listaOrdem = new();
        private ObservableCollection<ResultadoPesquisaRegistroManifestacao> _listaResultadosPesquisaRegistroManifestacao = new();
        private ObservableCollection<ObjetoSelecionavel> _listaObjetoSelecionavelPrioridades = new();
        private ObservableCollection<ObjetoSelecionavel> _listaObjetoSelecionavelTipos = new();
        private ObservableCollection<ObjetoSelecionavel> _listaObjetoSelecionavelStatus = new();

        private Parametro _classificarPorSelecionado;
        private Parametro _ordemSelecionada;

        private ResultadoPesquisaRegistroManifestacao _resultadoPesquisaRegistroManifestacaoSelecionado;

        private int _filterSize = 230;
        private bool _filterVisible = true;
        private string _filterIcon = "ChevronLeftCircleOutline";

        private bool ehDesenvolvedor;

        private CancellationTokenSource _cts;
        private string _textoResultadosEncontrados;

        private double _valorProgresso;

        private bool _controlesHabilitados;
        private bool _carregamentoVisivel = true;

        private ICommand _comandoPesquisar;
        private ICommand _comandoAbrirRegistroManifestacao;
        private ICommand _comandoExportarPesquisa;

        private ICommand _comandoSelecionarTodosPrioridade;
        private ICommand _comandoLimparFiltroPrioridade;

        private ICommand _comandoSelecionarTodosTipo;
        private ICommand _comandoLimparFiltroTipo;

        private ICommand _comandoSelecionarTodosStatus;
        private ICommand _comandoLimparFiltroStatus;

        private ICommand _comandoAlterarTamanhoFiltro;

        #endregion Campos

        #region Construtores

        public PesquisarRegistroManifestacoesViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;

            TextoLimiteDeResultados = "*Limitado a " + App.ConfiguracoesGerais.LimiteResultadosPesquisa.ToString() + " resultado (s). Você pode alterar isso nas configurações.";

            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Pesquisar registro de manifestação";
            }
        }

        public string Icone
        {
            get
            {
                return "";
            }
        }

        public SfDataGrid DataGrid { get; set; }

        public void LimparViewModel()
        {
            try
            {
                _listaClassificarPor = null;
                _listaOrdem = null;
                _listaResultadosPesquisaRegistroManifestacao = null;
                _listaObjetoSelecionavelPrioridades = null;
                _listaObjetoSelecionavelTipos = null;
                _listaObjetoSelecionavelStatus = null;
                _classificarPorSelecionado = null;
                _ordemSelecionada = null;
                _resultadoPesquisaRegistroManifestacaoSelecionado = null;
                _cts = null;
                _comandoPesquisar = null;
                _comandoAbrirRegistroManifestacao = null;
                _comandoExportarPesquisa = null;
                _comandoSelecionarTodosPrioridade = null;
                _comandoLimparFiltroPrioridade = null;
                _comandoSelecionarTodosTipo = null;
                _comandoLimparFiltroTipo = null;
                _comandoSelecionarTodosStatus = null;
                _comandoLimparFiltroStatus = null;
            }
            catch (Exception)
            {
            }
        }

        public Window Janela { private get; set; }

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

        public ObservableCollection<ResultadoPesquisaRegistroManifestacao> ListaResultadosPesquisaRegistroManifestacao
        {
            get { return _listaResultadosPesquisaRegistroManifestacao; }
            set
            {
                if (value != _listaResultadosPesquisaRegistroManifestacao)
                {
                    _listaResultadosPesquisaRegistroManifestacao = value;
                    OnPropertyChanged(nameof(ListaResultadosPesquisaRegistroManifestacao));
                }
            }
        }

        public ObservableCollection<ObjetoSelecionavel> ListaObjetoSelecionavelPrioridades
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

        public ObservableCollection<ObjetoSelecionavel> ListaObjetoSelecionavelTipos
        {
            get { return _listaObjetoSelecionavelTipos; }
            set
            {
                if (value != _listaObjetoSelecionavelTipos)
                {
                    _listaObjetoSelecionavelTipos = value;
                    OnPropertyChanged(nameof(ListaObjetoSelecionavelTipos));
                }
            }
        }

        public ObservableCollection<ObjetoSelecionavel> ListaObjetoSelecionavelStatus
        {
            get { return _listaObjetoSelecionavelStatus; }
            set
            {
                if (value != _listaObjetoSelecionavelStatus)
                {
                    _listaObjetoSelecionavelStatus = value;
                    OnPropertyChanged(nameof(ListaObjetoSelecionavelStatus));
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

        public ResultadoPesquisaRegistroManifestacao ResultadoPesquisaRegistroManifestacaoSelecionado
        {
            get { return _resultadoPesquisaRegistroManifestacaoSelecionado; }
            set
            {
                if (value != _resultadoPesquisaRegistroManifestacaoSelecionado)
                {
                    _resultadoPesquisaRegistroManifestacaoSelecionado = value;
                    OnPropertyChanged(nameof(ResultadoPesquisaRegistroManifestacaoSelecionado));
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
                    OnPropertyChanged(nameof(ValorProgresso));
                    Messenger.Default.Send<double>(ValorProgresso, "ValorProgresso2");
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

        public ICommand ComandoPesquisar
        {
            get
            {
                if (_comandoPesquisar == null)
                {
                    _comandoPesquisar = new RelayCommand(
                        param => ExecutarPesquisa().Await(),
                        param => true
                    );
                }
                return _comandoPesquisar;
            }
        }

        public ICommand ComandoAbrirRegistroManifestacao
        {
            get
            {
                if (_comandoAbrirRegistroManifestacao == null)
                {
                    _comandoAbrirRegistroManifestacao = new RelayCommand(
                        param => AbrirRegistroManifestacao(),
                        param => true
                    );
                }
                return _comandoAbrirRegistroManifestacao;
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

        public ICommand ComandoSelecionarTodosPrioridade
        {
            get
            {
                if (_comandoSelecionarTodosPrioridade == null)
                {
                    _comandoSelecionarTodosPrioridade = new RelayCommand(
                        param => SelecaoTodosLista(ListaObjetoSelecionavelPrioridades, true),
                        param => true
                    );
                }
                return _comandoSelecionarTodosPrioridade;
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

        public ICommand ComandoLimparFiltroPrioridade
        {
            get
            {
                if (_comandoLimparFiltroPrioridade == null)
                {
                    _comandoLimparFiltroPrioridade = new RelayCommand(
                        param => SelecaoTodosLista(ListaObjetoSelecionavelPrioridades, false),
                        param => true
                    );
                }
                return _comandoLimparFiltroPrioridade;
            }
        }

        public ICommand ComandoSelecionarTodosTipo
        {
            get
            {
                if (_comandoSelecionarTodosTipo == null)
                {
                    _comandoSelecionarTodosTipo = new RelayCommand(
                        param => SelecaoTodosLista(ListaObjetoSelecionavelTipos, true),
                        param => true
                    );
                }
                return _comandoSelecionarTodosTipo;
            }
        }

        public ICommand ComandoLimparFiltroTipo
        {
            get
            {
                if (_comandoLimparFiltroTipo == null)
                {
                    _comandoLimparFiltroTipo = new RelayCommand(
                        param => SelecaoTodosLista(ListaObjetoSelecionavelTipos, false),
                        param => true
                    );
                }
                return _comandoLimparFiltroTipo;
            }
        }

        public ICommand ComandoSelecionarTodosStatus
        {
            get
            {
                if (_comandoSelecionarTodosStatus == null)
                {
                    _comandoSelecionarTodosStatus = new RelayCommand(
                        param => SelecaoTodosLista(ListaObjetoSelecionavelStatus, true),
                        param => true
                    );
                }
                return _comandoSelecionarTodosStatus;
            }
        }

        public ICommand ComandoLimparFiltroStatus
        {
            get
            {
                if (_comandoLimparFiltroStatus == null)
                {
                    _comandoLimparFiltroStatus = new RelayCommand(
                        param => SelecaoTodosLista(ListaObjetoSelecionavelStatus, false),
                        param => true
                    );
                }
                return _comandoLimparFiltroStatus;
            }
        }

        #endregion Propriedades/Comandos

        #region Métodos

        public async Task ConstrutorAsync()
        {
            try
            {
                // Preenche a lista de classificar por
                ListaClassificarPor.Add(new Parametro("Código da requisição", "CodigoManifestacao", "string", "", "CardText", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Prioridade", "Prioridade", "string", "", "CardText", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Tipo", "Tipo", "string", "", "CardText", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Status", "Status", "string", "", "CardText", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Data de abertura", "DataAbertura", "string", "", "Calendar", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Solicitante", "PessoaAbertura", "string", "", "CardText", "SteelBlue"));
                ListaClassificarPor.Add(new Parametro("Descrição", "DescricaoAbertura", "string", "", "CardText", "SteelBlue"));

                // Preenche a lista de ordens
                ListaOrdem.Add(new Parametro("Crescente", "ASC", "string", "", "SortAlphabeticalAscending", "SteelBlue"));
                ListaOrdem.Add(new Parametro("Decrescente", "DESC", "string", "", "SortAlphabeticalDescending", "SteelBlue"));

                // Preenche as listas com as classes necessárias
                ObservableCollection<PrioridadeManifestacao> listaPrioridadesManifestacao = new();
                ObservableCollection<TipoManifestacao> listaTiposManifestacao = new();
                ObservableCollection<StatusManifestacao> listaStatusManifestacao = new();

                await PrioridadeManifestacao.PreencheListaPrioridadesManifestacaoAsync(listaPrioridadesManifestacao, true, null, CancellationToken.None, "INNER JOIN tb_registro_manifestacoes AS rema ON prma.id_prioridade_manifestacao = rema.id_prioridade_manifestacao GROUP BY prma.id_prioridade_manifestacao", "");
                await TipoManifestacao.PreencheListaTiposManifestacaoAsync(listaTiposManifestacao, true, null, CancellationToken.None, "INNER JOIN tb_registro_manifestacoes AS rema ON tima.id_tipo_manifestacao = rema.id_tipo_manifestacao GROUP BY tima.id_tipo_manifestacao", "");
                await StatusManifestacao.PreencheListaStatusManifestacaoAsync(listaStatusManifestacao, true, null, CancellationToken.None, "INNER JOIN tb_registro_manifestacoes AS rema ON stma.id_status_manifestacao = rema.id_status_manifestacao GROUP BY stma.id_status_manifestacao", "");

                var enderecoMacAtual =
                (
                    from nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up
                    select nic.GetPhysicalAddress().ToString()
                ).FirstOrDefault();

                ehDesenvolvedor = await FuncoesDeDatabase.EhDesenvolvedor(enderecoMacAtual, CancellationToken.None);

                foreach (var item in listaPrioridadesManifestacao)
                {
                    ListaObjetoSelecionavelPrioridades.Add(new ObjetoSelecionavel { Objeto = item, Selecionado = false });
                }

                foreach (var item in listaTiposManifestacao)
                {
                    ListaObjetoSelecionavelTipos.Add(new ObjetoSelecionavel { Objeto = item, Selecionado = false });
                }

                foreach (var item in listaStatusManifestacao)
                {
                    ListaObjetoSelecionavelStatus.Add(new ObjetoSelecionavel { Objeto = item, Selecionado = item.Id != 4 && item.Id != 5 });
                }

                ClassificarPorSelecionado = ListaClassificarPor.Where(dat => dat.Valor == "DataAbertura").First();
                OrdemSelecionada = ListaOrdem.Last();
                ControlesHabilitados = true;
            }
            catch (Exception ex)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Entendi"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Erro", "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor", MessageDialogStyle.Affirmative, mySettings);

                Serilog.Log.Error(ex, "Erro ao carregar dados");

                ControlesHabilitados = false;
            }

            CarregamentoVisivel = false;
        }

        private async Task ExecutarPesquisa()
        {
            ValorProgresso = 0;
            _cts = new();

            Progress<double> progresso = new(dbl =>
            {
                ValorProgresso = dbl;
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
                List<string> condicao = new();
                List<string> listaNomeParametros = new();
                List<object> listaValores = new();

                // Retorna o código da instância atual
                InstanciaLocal instanciaLocal = new();
                await instanciaLocal.GetInstanciaLocal(CancellationToken.None);

                if (!ehDesenvolvedor)
                {
                    condicao.Add("(rema.codigo_instancia = @codigo_instancia OR rema.codigo_instancia IS NULL)");
                    listaNomeParametros.Add("@codigo_instancia");
                    listaValores.Add(instanciaLocal.CodigoInstancia == null ? "" : instanciaLocal.CodigoInstancia);
                }

                if (!String.IsNullOrEmpty(TextoPesquisa))
                {
                    condicao.Add("(rema.descricao_abertura LIKE @texto_pesquisa OR " +
                        "rema.descricao_fechamento LIKE @texto_pesquisa)");
                    listaNomeParametros.Add("@texto_pesquisa");
                    listaValores.Add("%" + TextoPesquisa + "%");
                }

                string textoParametrosPrioridades = "";
                foreach (var item in ListaObjetoSelecionavelPrioridades.Where(d => d.Selecionado))
                {
                    listaNomeParametros.Add("@id_prioridade" + ListaObjetoSelecionavelPrioridades.IndexOf(item));
                    listaValores.Add(((PrioridadeManifestacao)item.Objeto).Id);

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
                    condicao.Add("prma.id_prioridade_manifestacao IN (" + textoParametrosPrioridades + ")");
                }

                string textoParametrosTipos = "";
                foreach (var item in ListaObjetoSelecionavelTipos.Where(d => d.Selecionado))
                {
                    listaNomeParametros.Add("@id_filtro" + ListaObjetoSelecionavelTipos.IndexOf(item));
                    listaValores.Add(((TipoManifestacao)item.Objeto).Id);

                    if (String.IsNullOrEmpty(textoParametrosTipos))
                    {
                        textoParametrosTipos = "@id_filtro" + ListaObjetoSelecionavelTipos.IndexOf(item);
                    }
                    else
                    {
                        textoParametrosTipos = textoParametrosTipos + ", @id_filtro" + ListaObjetoSelecionavelTipos.IndexOf(item);
                    }
                }

                if (!String.IsNullOrEmpty(textoParametrosTipos))
                {
                    condicao.Add("tima.id_tipo_manifestacao IN (" + textoParametrosTipos + ")");
                }

                string textoParametrosStatus = "";
                foreach (var item in ListaObjetoSelecionavelStatus.Where(d => d.Selecionado))
                {
                    listaNomeParametros.Add("@id_status" + ListaObjetoSelecionavelStatus.IndexOf(item));
                    listaValores.Add(((StatusManifestacao)item.Objeto).Id);

                    if (String.IsNullOrEmpty(textoParametrosStatus))
                    {
                        textoParametrosStatus = "@id_status" + ListaObjetoSelecionavelStatus.IndexOf(item);
                    }
                    else
                    {
                        textoParametrosStatus = textoParametrosStatus + ", @id_status" + ListaObjetoSelecionavelStatus.IndexOf(item);
                    }
                }

                if (!String.IsNullOrEmpty(textoParametrosStatus))
                {
                    condicao.Add("stma.id_status_manifestacao IN (" + textoParametrosStatus + ")");
                }

                await ResultadoPesquisaRegistroManifestacao.PreencheListaResultadosPesquisaRegistroManifestacaoAsync(ListaResultadosPesquisaRegistroManifestacao,
                    true, progresso, _cts.Token,
                    condicao.Count > 0 ? "WHERE " + String.Join(" AND ", condicao.ToArray()) + " ORDER BY " + ClassificarPorSelecionado.Valor + " " + OrdemSelecionada.Valor + " LIMIT " + App.ConfiguracoesGerais.LimiteResultadosPesquisa.ToString() : "ORDER BY " + ClassificarPorSelecionado.Valor + " " + OrdemSelecionada.Valor + " LIMIT " + App.ConfiguracoesGerais.LimiteResultadosPesquisa.ToString(),
                    String.Join(", ", listaNomeParametros.ToArray()),
                    listaValores.ToArray());

                TextoLimiteDeResultados = "*Limitado a " + App.ConfiguracoesGerais.LimiteResultadosPesquisa.ToString() + " resultado (s). Você pode alterar isso nas configurações.";
                TextoResultadosEncontrados = "Resultado (s) encontrado (s): " + ListaResultadosPesquisaRegistroManifestacao.Count;
            }
            catch (Exception ex)
            {
                await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Entendi"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Erro", "Erro ao executar a pesquisa", MessageDialogStyle.Affirmative, mySettings);

                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao pesquisar dados");

                ControlesHabilitados = false;
            }

            try
            {
                await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            }
            catch (Exception)
            {
            }
        }

        private void AbrirRegistroManifestacao()
        {
            if (ResultadoPesquisaRegistroManifestacaoSelecionado != null)
            {
                RegistroManifestacao registroManifestacao = new();
                registroManifestacao.Id = ResultadoPesquisaRegistroManifestacaoSelecionado.IdRegistroManifestacao;

                var win = new MetroWindow();
                win.Height = 640;
                win.Width = 740;
                win.Content = new RegistroManifestacoesViewModel(DialogCoordinator.Instance, registroManifestacao);
                win.Title = "Requisição " + ResultadoPesquisaRegistroManifestacaoSelecionado.CodigoManifestacao;
                win.ShowDialogsOverTitleBar = false;
                win.Owner = Janela;
                win.Show();
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
            //    await ExcelClasses.ExportarPesquisaRegistroManifestacao(ListaResultadosPesquisaRegistroManifestacao, progresso, _cts.Token);
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
                FileName = "Pesquisa_Requisicoes_" + DateTime.Now.ToString("yyyyMMddhhmmss"),
                AddExtension = true
            };

            bool houveErro = false;

            try
            {
                var options = new ExcelExportingOptions();
                options.ExcelVersion = ExcelVersion.Excel2013;
                var excelEngine = DataGrid.ExportToExcel(DataGrid.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];
                workBook.Worksheets[0].Name = "pesquisa_requisicoes";
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

                Messenger.Default.Send<string>("Falha na eportação dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log", "MensagemStatus");
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

                    Messenger.Default.Send<string>("Falha na abertura do arquivo. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log", "MensagemStatus");
                }
            }
        }

        private void SelecaoTodosLista(ObservableCollection<ObjetoSelecionavel> lista, bool itemSelecionado)
        {
            foreach (var item in lista)
            {
                item.Selecionado = itemSelecionado;
            }
        }

        private void AlterarTamanhoFiltro()
        {
            if (!FilterVisible)
            {
                FilterVisible = true;
                FilterSize = 230;
                FilterIcon = "ChevronLeftCircleOutline";
            }
            else
            {
                FilterVisible = false;
                FilterSize = 25;
                FilterIcon = "ChevronRightCircleOutline";
            }
        }

        #endregion Métodos
    }
}