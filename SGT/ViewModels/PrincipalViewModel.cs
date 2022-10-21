using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using SGT.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class PrincipalViewModel : ObservableObject, IPageViewModel
    {
        #region Constantes

        private const int limiteAbas = 5;

        #endregion Constantes

        #region Campos

        private ICommand _changePageCommand;
        private ICommand _comandoAbrirParametros;
        private ICommand _comandoAbrirLogAlteracoes;
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        private List<IPageViewModel> _parametrosViewModels;
        private int _indicePaginaSelecionada;

        private readonly IDialogCoordinator _dialogCoordinator;
        private static int tabs = 1;
        private ICommand _adicionaTabProposta;
        private ICommand _adicionaTabOrdemServico;
        private ICommand _removeTab;
        private ObservableCollection<IPageViewModel> _titles;
        private IPageViewModel _selectedTitle;
        private ICommand _comandoPesquisarProposta;
        private ICommand _comandoPesquisarOrdemServico;
        private ICommand _comandoExportarOrdemServico;
        private ICommand _comandoTeste;
        private Usuario _usuarioLogado;
        private string _nomeAbreviado;
        private bool _imagemPadraoVisivel;
        private string _tituloJanela;
        private bool _menuParametrosVisivel;
        private string _iconeParametro = "ChevronRightCircleOutline";

        private bool _cadastrarVisivel;
        private bool _gerenciarUsuariosVisivel;
        private bool _parametrosVisivel;
        private bool _ordemServicoVisivel;

        private bool _edicaoEhEnterprise;
        private bool _permiteSelecionar;

        private bool _clicouLembrarDepoisOS;

        private ICommand _comandoAbrirPerfil;
        private ICommand _comandoCadastrar;
        private ICommand _comandoGerenciar;
        private ICommand _comandoAlterarSenha;
        private ICommand _comandoNovaRequisicao;
        private ICommand _comandoPesquisarRequisicao;
        private ICommand _comandoAbrirSobre;
        private ICommand _comandoAlterarUsuario;

        #region ControlesMenu

        private bool _menuVisivel = true;
        private int _tamanhoMenu = 190;
        private int _tamanhoDadosUsuario = 80;
        private ICommand _comandoVisibilidadeMenu;

        public bool MenuVisivel
        {
            get { return _menuVisivel; }
            set
            {
                _menuVisivel = value;
                OnPropertyChanged(nameof(MenuVisivel));
            }
        }

        public int TamanhoMenu
        {
            get { return _tamanhoMenu; }
            set
            {
                _tamanhoMenu = value;
                OnPropertyChanged(nameof(TamanhoMenu));
            }
        }

        public int TamanhoDadosUsuario
        {
            get { return _tamanhoDadosUsuario; }
            set
            {
                _tamanhoDadosUsuario = value;
                OnPropertyChanged(nameof(TamanhoDadosUsuario));
            }
        }

        public ICommand ComandoVisibilidadeMenu
        {
            get
            {
                if (_comandoVisibilidadeMenu == null)
                {
                    _comandoVisibilidadeMenu = new RelayCommand(
                        param => AlteraVisibilidadeMenu(),
                        param => true
                    );
                }
                return _comandoVisibilidadeMenu;
            }
        }

        private void AlteraVisibilidadeMenu()
        {
            if (MenuVisivel)
            {
                MenuVisivel = false;
                TamanhoMenu = 38;
                TamanhoDadosUsuario = 0;
            }
            else
            {
                MenuVisivel = true;
                TamanhoMenu = 190;
                TamanhoDadosUsuario = 80;
            }
        }

        #endregion ControlesMenu

        private MetroWindow? testWindow;

        #endregion Campos

        public PrincipalViewModel(IDialogCoordinator dialogCoordinator)
        {
            EdicaoEhEnterprise = App.EdicaoEhEnterprise;

            _dialogCoordinator = dialogCoordinator;
            Titles = new();
            UsuarioLogado = App.Usuario;

            OrdemServicoVisivel = true;

            //NomeAbreviado = FuncoesDeTexto.NomeSobrenome(App.Usuario?.Nome);
            //PageViewModels.Add(new PropostaViewModel(DialogCoordinator.Instance));

            //// Define o controle inicial
            //CurrentPageViewModel = PageViewModels[0];

            Messenger.Default.Register<Proposta>(this, "PropostaAdicionar", delegate (Proposta proposta)
            {
                AddTabProposta(proposta);
            });

            Messenger.Default.Register<Proposta>(this, "PropostaCopiar", delegate (Proposta proposta)
            {
                AddTabProposta(proposta, true);
            });

            Messenger.Default.Register<OrdemServico>(this, "OrdemServicoCopiar", delegate (OrdemServico ordemServico)
            {
                AddTabOrdemServico(ordemServico, true);
            });

            Messenger.Default.Register<OrdemServico>(this, "OrdemServicoAdicionar", delegate (OrdemServico ordemServico)
            {
                AddTabOrdemServico(ordemServico);
            });

            Messenger.Default.Register<IPageViewModel>(this, "PrincipalPaginaRemover", delegate (IPageViewModel paginaPropostaRemover)
            {
                RemoveTabItem(paginaPropostaRemover);
            });

            Messenger.Default.Register<IPageViewModel>(this, "PrincipalPagina", delegate (IPageViewModel pagina)
            {
                AlteraNomePagina(pagina);
            });

            Messenger.Default.Register<IPageViewModel>(this, "SelecionaPrimeiraPagina", delegate (IPageViewModel pagina)
            {
                if (SelectedTitle == null)
                {
                    SelectedTitle = pagina;
                    PermiteSelecionar = true;
                }
            });

            Messenger.Default.Register<bool>(this, "ClicouLembrarDepoisOS", delegate (bool clicouLembrarDepoisOS)
            {
                if (clicouLembrarDepoisOS)
                {
                    IPageViewModel pageViewModel;
                    pageViewModel = new OrdemServicoViewModel(DialogCoordinator.Instance);

                    Titles.Add(pageViewModel);
                    tabs++;

                    AlteraNomePagina(Titles.Last());
                }
            });

            Messenger.Default.Register<bool>(this, "UsuarioAlterado", delegate (bool usuarioAlterado)
            {
                if (usuarioAlterado)
                {
                    UsuarioLogado = App.Usuario;
                }
            });

            _parametrosViewModels = new();

            // Add available pages
            ParametrosViewModels.Add(new ParametroClientesViewModel(DialogCoordinator.Instance));
            ParametrosViewModels.Add(new ParametroContatosViewModel(DialogCoordinator.Instance));
        }

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Principal";
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
        }

        private MetroWindow GetTestWindow()
        {
            if (this.testWindow != null)
            {
                this.testWindow.Close();
            }

            this.testWindow = new MetroWindow
            {
                Owner = App.Current.MainWindow,
                Title = _tituloJanela
            };

            this.testWindow.Closed += (o, args) => this.testWindow = null;
            this.testWindow.Closed += (o, args) => Messenger.Default.Send<bool>(false, "FundoPrincipalVisivel");
            this.testWindow.Closed += (o, args) => Messenger.Default.Send<string>("", "TextoFundoPrincipal");
            return this.testWindow;
        }

        //public Usuario UsuarioLogado { get; set; }

        //public string NomeAbreviado { get; set; }

        public Usuario UsuarioLogado
        {
            get { return _usuarioLogado; }
            set
            {
                _usuarioLogado = value;
                OnPropertyChanged(nameof(UsuarioLogado));
                NomeAbreviado = FuncoesDeTexto.NomeSobrenome(UsuarioLogado.Nome);
                ImagemPadraoVisivel = UsuarioLogado.Imagem == null;

                CadastrarVisivel = UsuarioLogado.Perfil.Id == 1;
                GerenciarUsuariosVisivel = UsuarioLogado.Perfil.Id == 1;
                ParametrosVisivel = UsuarioLogado.Perfil.Id == 1;
            }
        }

        public string NomeAbreviado
        {
            get { return _nomeAbreviado; }
            set
            {
                _nomeAbreviado = value;
                OnPropertyChanged(nameof(NomeAbreviado));
            }
        }

        public bool ImagemPadraoVisivel
        {
            get { return _imagemPadraoVisivel; }
            set
            {
                _imagemPadraoVisivel = value;
                OnPropertyChanged(nameof(ImagemPadraoVisivel));
            }
        }

        public bool MenuParametrosVisivel
        {
            get { return _menuParametrosVisivel; }
            set
            {
                _menuParametrosVisivel = value;
                OnPropertyChanged(nameof(MenuParametrosVisivel));
            }
        }

        public string IconeParametro
        {
            get { return _iconeParametro; }
            set
            {
                _iconeParametro = value;
                OnPropertyChanged(nameof(IconeParametro));
            }
        }

        public bool CadastrarVisivel
        {
            get { return _cadastrarVisivel; }
            set
            {
                _cadastrarVisivel = value;
                OnPropertyChanged(nameof(CadastrarVisivel));
            }
        }

        public bool GerenciarUsuariosVisivel
        {
            get { return _gerenciarUsuariosVisivel; }
            set
            {
                _gerenciarUsuariosVisivel = value;
                OnPropertyChanged(nameof(GerenciarUsuariosVisivel));
            }
        }

        public bool ParametrosVisivel
        {
            get { return _parametrosVisivel; }
            set
            {
                _parametrosVisivel = value;
                OnPropertyChanged(nameof(ParametrosVisivel));
            }
        }

        public bool OrdemServicoVisivel
        {
            get { return _ordemServicoVisivel; }
            set
            {
                _ordemServicoVisivel = value;
                OnPropertyChanged(nameof(OrdemServicoVisivel));
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

        public bool PermiteSelecionar
        {
            get { return _permiteSelecionar; }
            set
            {
                if (value != _permiteSelecionar)
                {
                    _permiteSelecionar = value;
                    OnPropertyChanged(nameof(PermiteSelecionar));
                }
            }
        }

        public bool ClicouLembrarDepoisOS
        {
            get { return _clicouLembrarDepoisOS; }
            set
            {
                if (value != _clicouLembrarDepoisOS)
                {
                    _clicouLembrarDepoisOS = value;
                    OnPropertyChanged(nameof(ClicouLembrarDepoisOS));
                }
            }
        }

        public ICommand ComandoAbrirParametros
        {
            get
            {
                if (_comandoAbrirParametros == null)
                {
                    _comandoAbrirParametros = new RelayCommand(
                        param => AbrirParametro().Await(),
                        param => true
                    );
                }
                return _comandoAbrirParametros;
            }
        }

        public ICommand ComandoAbrirLogAlteracoes
        {
            get
            {
                if (_comandoAbrirLogAlteracoes == null)
                {
                    _comandoAbrirLogAlteracoes = new RelayCommand(
                        param => AbrirLogAlteracoes(),
                        param => true
                    );
                }
                return _comandoAbrirLogAlteracoes;
            }
        }

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }

                return _changePageCommand;
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public List<IPageViewModel> ParametrosViewModels
        {
            get
            {
                if (_parametrosViewModels == null)
                    _parametrosViewModels = new List<IPageViewModel>();

                return _parametrosViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged(nameof(CurrentPageViewModel));
                }
            }
        }

        public ICommand ComandoInserirProposta
        {
            get
            {
                return _adicionaTabProposta ?? (_adicionaTabProposta = new RelayCommand(
                   x =>
                   {
                       AdicionaTabProposta(false).Await();
                   }));
            }
        }

        public ICommand ComandoPesquisarProposta
        {
            get
            {
                return _comandoPesquisarProposta ?? (_comandoPesquisarProposta = new RelayCommand(
                   x =>
                   {
                       AdicionaTabProposta(true).Await();
                   }));
            }
        }

        public ICommand ComandoInserirOrdemServico
        {
            get
            {
                return _adicionaTabOrdemServico ?? (_adicionaTabOrdemServico = new RelayCommand(
                   x =>
                   {
                       AdicionaTabOrdemServico(false).Await();
                   }));
            }
        }

        public ICommand ComandoPesquisarOrdemServico
        {
            get
            {
                return _comandoPesquisarOrdemServico ?? (_comandoPesquisarOrdemServico = new RelayCommand(
                   x =>
                   {
                       AdicionaTabOrdemServico(true).Await();
                   }));
            }
        }

        public ICommand ComandoExportarOrdemServico
        {
            get
            {
                return _comandoExportarOrdemServico ?? (_comandoExportarOrdemServico = new RelayCommand(
                   x =>
                   {
                       AbrirExportacaoOS();
                   }));
            }
        }

        public ICommand ComandoTeste
        {
            get
            {
                return _comandoTeste ?? (_comandoTeste = new RelayCommand(
                   x =>
                   {
                       AbrirParametro().Await();
                   }));
            }
        }

        public ICommand ComandoAbrirPerfil
        {
            get
            {
                if (_comandoAbrirPerfil == null)
                {
                    _comandoAbrirPerfil = new RelayCommand(
                        param => AbrirPerfil().Await(),
                        param => true
                    );
                }
                return _comandoAbrirPerfil;
            }
        }

        public ICommand ComandoCadastrar
        {
            get
            {
                if (_comandoCadastrar == null)
                {
                    _comandoCadastrar = new RelayCommand(
                        param => AbrirCadastrar().Await(),
                        param => true
                    );
                }
                return _comandoCadastrar;
            }
        }

        public ICommand ComandoGerenciar
        {
            get
            {
                if (_comandoGerenciar == null)
                {
                    _comandoGerenciar = new RelayCommand(
                        param => AbrirGerenciar().Await(),
                        param => true
                    );
                }
                return _comandoGerenciar;
            }
        }

        public ICommand ComandoAlterarSenha
        {
            get
            {
                if (_comandoAlterarSenha == null)
                {
                    _comandoAlterarSenha = new RelayCommand(
                        param => AlterarSenha().Await(),
                        param => true
                    );
                }
                return _comandoAlterarSenha;
            }
        }

        public ICommand ComandoNovaRequisicao
        {
            get
            {
                if (_comandoNovaRequisicao == null)
                {
                    _comandoNovaRequisicao = new RelayCommand(
                        param => NovaRequisicao().Await(),
                        param => true
                    );
                }
                return _comandoNovaRequisicao;
            }
        }

        public ICommand ComandoPesquisarRequisicao

        {
            get
            {
                if (_comandoPesquisarRequisicao == null)
                {
                    _comandoPesquisarRequisicao = new RelayCommand(
                        param => PesquisarRequisicao().Await(),
                        param => true
                    );
                }
                return _comandoPesquisarRequisicao;
            }
        }

        public ICommand ComandoAbrirSobre

        {
            get
            {
                if (_comandoAbrirSobre == null)
                {
                    _comandoAbrirSobre = new RelayCommand(
                        param => AbrirSobre(),
                        param => true
                    );
                }
                return _comandoAbrirSobre;
            }
        }

        public ICommand ComandoAlterarUsuario

        {
            get
            {
                if (_comandoAlterarUsuario == null)
                {
                    _comandoAlterarUsuario = new RelayCommand(
                        param => AbrirAlterarUsuario(),
                        param => true
                    );
                }
                return _comandoAlterarUsuario;
            }
        }

        //public ICommand RemoveTab
        //{
        //    get
        //    {
        //        return _removeTab ?? (_removeTab = new RelayCommand(
        //           x =>
        //           {
        //               RemoveTabItem();
        //           }));
        //    }
        //}

        public ICommand RemoveTab
        {
            get
            {
                return _removeTab ?? (_removeTab = new RelayCommand(
                   x =>
                   {
                       RemoveTabItem((IPageViewModel)x);
                   }));
            }
        }

        public ObservableCollection<IPageViewModel> Titles
        {
            get { return _titles; }
            set
            {
                _titles = value;
                OnPropertyChanged(nameof(Titles));
            }
        }

        public IPageViewModel SelectedTitle
        {
            get { return _selectedTitle; }
            set
            {
                _selectedTitle = value;
                OnPropertyChanged(nameof(SelectedTitle));
            }
        }

        public int IndicePaginaSelecionada
        {
            get { return _indicePaginaSelecionada; }
            set
            {
                _indicePaginaSelecionada = value;
                OnPropertyChanged(nameof(IndicePaginaSelecionada));
            }
        }

        #endregion Propriedades/Comandos

        #region Métodos

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

#pragma warning disable CS8601 // Possible null reference assignment.
            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        private void RemoveTabItem(IPageViewModel pageViewModel)
        {
            if (pageViewModel != null)
            {
                Titles.Remove(pageViewModel);
            }

            //pageViewModel.LimparViewModel();
        }

        private void AddTabProposta(Proposta proposta, bool ehCopia = false)
        {
            if (!EdicaoEhEnterprise && Titles.Count == limiteAbas)
            {
                _dialogCoordinator.ShowModalMessageExternal(this, "Limite atingido", "Você atingiu o limite de " + limiteAbas.ToString() + " abas. Por favor, feche alguma aba antes de abrir uma nova ou atualize para a versão Enterprise e tenha abas ilimitadas");
                return;
            }

            IPageViewModel pageViewModel = new PropostaViewModel(DialogCoordinator.Instance, proposta, ehCopia);
            PageViewModels.Add(pageViewModel);

            //var content = PageViewModels[tabs - 1];
            //var item = new Item { Content = content, Icon = "Handshake" };

            Titles.Add(pageViewModel);

            //tabs++;

            AlteraNomePagina(Titles.Last());
        }

        private async Task AdicionaTabProposta(bool pesquisar)
        {
            if (!EdicaoEhEnterprise && Titles.Count == limiteAbas)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Limite atingido", "Você atingiu o limite de " + limiteAbas.ToString() + " abas. Por favor, feche alguma aba antes de abrir uma nova ou atualize para a versão Enterprise e tenha abas ilimitadas");
                return;
            }

            await HelperClasses.ValidaSessao.ValidaSessaoUsuarioAsync(_dialogCoordinator, this);

            IPageViewModel pageViewModel;
            if (pesquisar)
            {
                pageViewModel = new PesquisarPropostaViewModel(DialogCoordinator.Instance);
            }
            else
            {
                pageViewModel = new PropostaViewModel(DialogCoordinator.Instance);
            }

            //var content = PageViewModels[tabs - 1];
            //var item = new Item { Header = "Carregando...", Content = content, Icon = "Handshake" };

            Titles.Add(pageViewModel);
            tabs++;

            AlteraNomePagina(Titles.Last());
        }

        private void AddTabOrdemServico(OrdemServico ordemServico, bool ehCopia = false)
        {
            if (!EdicaoEhEnterprise && Titles.Count == limiteAbas)
            {
                _dialogCoordinator.ShowModalMessageExternal(this, "Limite atingido", "Você atingiu o limite de " + limiteAbas.ToString() + " abas. Por favor, feche alguma aba antes de abrir uma nova ou atualize para a versão Enterprise e tenha abas ilimitadas");
                return;
            }

            IPageViewModel pageViewModel = new OrdemServicoViewModel(DialogCoordinator.Instance, ordemServico, ehCopia);
            PageViewModels.Add(pageViewModel);

            //var content = PageViewModels[tabs - 1];
            //var item = new Item { Content = content, Icon = "Handshake" };

            Titles.Add(pageViewModel);

            //tabs++;

            AlteraNomePagina(Titles.Last());
        }

        private async Task AdicionaTabOrdemServico(bool pesquisar)
        {
            if (!EdicaoEhEnterprise && Titles.Count == limiteAbas)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Limite atingido", "Você atingiu o limite de " + limiteAbas.ToString() + " abas. Por favor, feche alguma aba antes de abrir uma nova ou atualize para a versão Enterprise e tenha abas ilimitadas");
                return;
            }

            //if (pesquisar)
            //{
            //    PageViewModels.Add(new PesquisarPropostaViewModel(DialogCoordinator.Instance));
            //}
            //else
            //{
            //    PageViewModels.Add(new OrdemServicoViewModel(DialogCoordinator.Instance));
            //}

            //var content = PageViewModels[tabs - 1];
            //var item = new Item { Header = "Carregando...", Content = content, Icon = "CarWrench" };

            //Titles.Add(item);
            //tabs++;

            //AlteraNomePagina(PageViewModels.Last());

            await HelperClasses.ValidaSessao.ValidaSessaoUsuarioAsync(_dialogCoordinator, this);

            IPageViewModel pageViewModel;
            if (pesquisar)
            {
                pageViewModel = new PesquisarOrdemServicoViewModel(DialogCoordinator.Instance);
            }
            else
            {
                if (await OrdemServico.ExistemOrdensServicoIncompletas(CancellationToken.None))
                {
                    ClicouLembrarDepoisOS = false;

                    var customDialog2 = new CustomDialog();

                    var dataContext2 = new MensagemOrdensServicoIncompletasViewModel(instance =>
                    {
                        _dialogCoordinator.HideMetroDialogAsync(this, customDialog2);
                    });

                    customDialog2.Content = new MensagemOrdensServicoIncompletasView { DataContext = dataContext2 };

                    await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog2);

                    return;
                }
                else
                {
                    pageViewModel = new OrdemServicoViewModel(DialogCoordinator.Instance);
                }
            }

            //var content = PageViewModels[tabs - 1];
            //var item = new Item { Header = "Carregando...", Content = content, Icon = "Handshake" };

            Titles.Add(pageViewModel);
            tabs++;

            AlteraNomePagina(Titles.Last());
        }

        private void AlteraNomePagina(IPageViewModel pageViewModel)
        {
            //foreach (var it in Titles)
            //{
            //    if (it.Content == pageViewModel)
            //    {
            //        it.Header = pageViewModel.Name;
            //    }
            //}

            OnPropertyChanged(nameof(Titles));
        }

        private async Task AbrirPerfil()
        {
            await HelperClasses.ValidaSessao.ValidaSessaoUsuarioAsync(_dialogCoordinator, this);

            var pagina = new PerfilViewModel(DialogCoordinator.Instance, App.Usuario);

            _tituloJanela = pagina.Name;

            Messenger.Default.Send<bool>(true, "FundoPrincipalVisivel");
            Messenger.Default.Send<string>("Aguardando a operação na janela " + pagina.Name, "TextoFundoPrincipal");

            var w = this.GetTestWindow();
            w.ShowInTaskbar = true;
            w.Height = 320;
            w.Width = 600;
            w.Content = pagina;
            w.ResizeMode = System.Windows.ResizeMode.NoResize;
            w.ShowDialogsOverTitleBar = false;
            w.Show();
        }

        private async Task AbrirCadastrar()
        {
            await HelperClasses.ValidaSessao.ValidaSessaoUsuarioAsync(_dialogCoordinator, this);

            var pagina = new CadastrarViewModel();

            _tituloJanela = pagina.Name;

            Messenger.Default.Send<bool>(true, "FundoPrincipalVisivel");
            Messenger.Default.Send<string>("Aguardando a operação na janela " + pagina.Name, "TextoFundoPrincipal");

            var w = this.GetTestWindow();
            w.ShowInTaskbar = true;
            w.Height = 310;
            w.Width = 600;
            w.Content = pagina;
            w.ResizeMode = System.Windows.ResizeMode.NoResize;
            w.ShowDialogsOverTitleBar = false;
            w.Show();
        }

        private async Task AbrirGerenciar()
        {
            await HelperClasses.ValidaSessao.ValidaSessaoUsuarioAsync(_dialogCoordinator, this);

            var pagina = new GerenciarUsuariosViewModel(DialogCoordinator.Instance);

            _tituloJanela = pagina.Name;

            Messenger.Default.Send<bool>(true, "FundoPrincipalVisivel");
            Messenger.Default.Send<string>("Aguardando a operação na janela " + pagina.Name, "TextoFundoPrincipal");

            var w = this.GetTestWindow();
            w.ShowInTaskbar = true;
            w.Height = 580;
            w.Width = 660;
            w.Content = pagina;
            w.ShowDialogsOverTitleBar = false;
            w.Show();
        }

        private async Task AlterarSenha()
        {
            await HelperClasses.ValidaSessao.ValidaSessaoUsuarioAsync(_dialogCoordinator, this);

            var pagina = new AlterarSenhaComAtualViewModel(App.Usuario);

            _tituloJanela = pagina.Name;

            Messenger.Default.Send<bool>(true, "FundoPrincipalVisivel");
            Messenger.Default.Send<string>("Aguardando a operação na janela " + pagina.Name, "TextoFundoPrincipal");

            var w = this.GetTestWindow();
            w.ShowInTaskbar = true;
            w.Height = 300;
            w.Width = 270;
            w.Content = pagina;
            w.ShowDialogsOverTitleBar = false;
            w.Show();
        }

        private async Task AbrirParametro()
        {
            await HelperClasses.ValidaSessao.ValidaSessaoUsuarioAsync(_dialogCoordinator, this);

            var pagina = new ParametrosViewModel();

            _tituloJanela = pagina.Name;

            Messenger.Default.Send<bool>(true, "FundoPrincipalVisivel");
            Messenger.Default.Send<string>("Aguardando a operação na janela " + pagina.Name, "TextoFundoPrincipal");

            var w = this.GetTestWindow();
            w.ShowInTaskbar = true;
            w.Height = 620;
            w.Width = 860;
            w.Content = pagina;
            w.ShowDialogsOverTitleBar = false;
            w.Show();
        }

        private void AbrirExportacaoOS()
        {
            var pagina = new ExportarOrdemServicoViewModel(DialogCoordinator.Instance);

            _tituloJanela = pagina.Name;

            Messenger.Default.Send<bool>(true, "FundoPrincipalVisivel");
            Messenger.Default.Send<string>("Aguardando a operação na janela " + pagina.Name, "TextoFundoPrincipal");

            var w = this.GetTestWindow();
            w.ShowInTaskbar = true;
            w.Height = 520;
            w.Width = 820;
            w.Content = pagina;
            w.ShowDialogsOverTitleBar = false;
            w.Show();
        }

        private void AbrirLogAlteracoes()
        {
            var win = new MetroWindow();
            win.Height = 640;
            win.Width = 740;
            win.Content = new LogAlteracoesViewModel();
            win.Title = "SGT - Log de alterações";
            win.ShowDialogsOverTitleBar = false;
            win.Owner = App.Current.MainWindow;
            win.Show();
        }

        private async Task NovaRequisicao()
        {
            await HelperClasses.ValidaSessao.ValidaSessaoUsuarioAsync(_dialogCoordinator, this);

            var win = new MetroWindow();
            win.Height = 640;
            win.Width = 740;
            win.Content = new RegistroManifestacoesViewModel(DialogCoordinator.Instance);
            win.Title = "Nova requisição";
            win.ShowDialogsOverTitleBar = false;
            win.Owner = App.Current.MainWindow;
            win.Show();
        }

        private async Task PesquisarRequisicao()
        {
            await HelperClasses.ValidaSessao.ValidaSessaoUsuarioAsync(_dialogCoordinator, this);

            var win = new MetroWindow();
            win.Height = 620;
            win.Width = 980;
            win.Content = new PesquisarRegistroManifestacoesViewModel(DialogCoordinator.Instance);
            win.Title = "Pesquisar requisição";
            win.ShowDialogsOverTitleBar = false;
            win.Owner = App.Current.MainWindow;
            win.Show();
        }

        private void AbrirSobre()
        {
            var pagina = new SobreViewModel();

            _tituloJanela = pagina.Name;

            Messenger.Default.Send<bool>(true, "FundoPrincipalVisivel");
            Messenger.Default.Send<string>("Aguardando a operação na janela " + pagina.Name, "TextoFundoPrincipal");

            var w = this.GetTestWindow();
            w.ShowInTaskbar = true;
            w.Height = 440;
            w.Width = 500;
            w.ResizeMode = System.Windows.ResizeMode.NoResize;
            w.Content = pagina;
            w.ShowDialogsOverTitleBar = false;
            w.Show();
        }

        private void AbrirAlterarUsuario()
        {
            var pagina = new RenovarSessaoViewModel(true);

            _tituloJanela = pagina.Name;

            Messenger.Default.Send<bool>(true, "FundoPrincipalVisivel");
            Messenger.Default.Send<string>("Aguardando a operação na janela " + pagina.Name, "TextoFundoPrincipal");

            var w = this.GetTestWindow();
            pagina.ComandoFechar = new RelayCommand(
                        param => w.Close(),
                        param => true
                    );
            w.ShowInTaskbar = true;
            w.ResizeMode = System.Windows.ResizeMode.NoResize;
            w.Height = 335;
            w.IsCloseButtonEnabled = true;
            w.Width = 300;
            w.ResizeMode = System.Windows.ResizeMode.NoResize;
            w.Content = pagina;
            w.ShowDialogsOverTitleBar = false;
            w.Show();
        }

        #endregion Métodos
    }

    public class Item : ObservableObject
    {
        private string _header;
        public IPageViewModel _content;

        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                OnPropertyChanged(nameof(Header));
            }
        }

        public IPageViewModel Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public string Icon { get; set; }
    }
}