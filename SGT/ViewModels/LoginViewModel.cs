using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SGT.ViewModels
{
    internal class LoginViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private ICommand _changePageCommand;
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        private int _paginaAtual;
        private Instancia _instancia;
        private string _versao;
        private bool _exibeMensagemErroLogin;
        private string _mensagemErroLogin;
        private bool _controlesHabilitados;
        private bool _carregamentoVisivel = true;
        private bool existeNovaVersaoCritica;
        private bool _naoEhEnterprise;

        #endregion Campos

        public LoginViewModel()
        {
            // Add available pages
            PageViewModels.Add(new EntrarViewModel());
            PageViewModels.Add(new EsqueciASenhaViewModel());
            PageViewModels.Add(new AlterarSenhaSemAtualViewModel());

            Messenger.Default.Register<int>(this, "PaginaAtualLogin", delegate (int paginaRecebida) { PaginaAtual = paginaRecebida; });
            Messenger.Default.Send<double?>(450, "AlturaJanela");
            Messenger.Default.Send<double?>(800, "LarguraJanela");

            // Set starting page
            CurrentPageViewModel = PageViewModels[0];

            ConstrutorAsync().Await();
        }

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Login";
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
                _changePageCommand = null;
                _instancia = null;
                _pageViewModels = null;
                _currentPageViewModel = null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Versao
        {
            get { return _versao; }
            set
            {
                if (value != _versao)
                {
                    _versao = value;
                    OnPropertyChanged(nameof(Versao));
                }
            }
        }

        public Instancia Instancia
        {
            get { return _instancia; }
            set
            {
                if (value != _instancia)
                {
                    _instancia = value;
                    OnPropertyChanged(nameof(Instancia));
                }
            }
        }

        public bool ExibeMensagemErroLogin
        {
            get { return _exibeMensagemErroLogin; }
            set
            {
                if (value != _exibeMensagemErroLogin)
                {
                    _exibeMensagemErroLogin = value;
                    OnPropertyChanged(nameof(ExibeMensagemErroLogin));
                }
            }
        }

        public string MensagemErroLogin
        {
            get { return _mensagemErroLogin; }
            set
            {
                if (value != _mensagemErroLogin)
                {
                    _mensagemErroLogin = value;
                    OnPropertyChanged(nameof(MensagemErroLogin));
                }
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

        public int PaginaAtual
        {
            get
            {
                return _paginaAtual;
            }
            set
            {
                if (value != _paginaAtual)
                {
                    _paginaAtual = value;
                    OnPropertyChanged(nameof(PaginaAtual));
                    CurrentPageViewModel = PageViewModels[PaginaAtual];
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

        public bool NaoEhEnterprise
        {
            get { return _naoEhEnterprise; }
            set
            {
                if (_naoEhEnterprise != value)
                {
                    _naoEhEnterprise = value;
                    OnPropertyChanged(nameof(NaoEhEnterprise));
                }
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

        private async Task ConstrutorAsync()
        {
            try
            {
                Instancia = new();
                InstanciaLocal instanciaLocal = new();

                Versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

                try
                {
                    await instanciaLocal.GetInstanciaLocal(CancellationToken.None);
                }
                catch (Exception ex)
                {
                    // Escreve no log a exceção e uma mensagem de erro
                    Serilog.Log.Error(ex, "Erro na verificação de instância local");

                    MensagemErroLogin = "Falha na verificação de instância local. Se o problema persistir, contate o desenvolvedor";
                    ExibeMensagemErroLogin = true;
                    CarregamentoVisivel = false;
                    return;
                }

                try
                {
                    await Instancia.GetInstanciaDatabaseAsync(instanciaLocal.CodigoInstancia, CancellationToken.None);
                    Serilog.Context.GlobalLogContext.PushProperty("EmpresaInstancia", Instancia.NomeInstancia);

                    App.EdicaoEhEnterprise = Instancia.NomeEdicao == "Enterprise";
                    NaoEhEnterprise = !App.EdicaoEhEnterprise;

                    try
                    {
                        await InstanciaLocal.AtualizaDataInstancia(CancellationToken.None);

                        //if (!App.EdicaoEhEnterprise)
                        //{
                        //    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        //    {
                        //        FileName = "http://www.proreports.com.br",
                        //        UseShellExecute = true
                        //    });
                        //}
                    }
                    catch (Exception)
                    {
                    }

                    if (Instancia.Id == null)
                    {
                        MensagemErroLogin = "Instância de login inválida. Contate o desenvolvedor";
                        ExibeMensagemErroLogin = true;
                        CarregamentoVisivel = false;
                        return;
                    }
                    else
                    {
                        if (Instancia.DataFim != null)
                        {
                            if (DateTime.Now > (DateTime)Instancia.DataFim)
                            {
                                MensagemErroLogin = "Instância expirada. Contate o desenvolvedor";
                                ExibeMensagemErroLogin = true;
                                CarregamentoVisivel = false;
                                return;
                            }
                        }
                    }

                    try
                    {
                        Versao versaoAtual = new();
                        await versaoAtual.GetVersaoDatabaseAsync(CancellationToken.None, false, "WHERE nome = @nome", "@nome", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

                        bool existeNovaVersao = await Model.DataAccessLayer.Classes.Versao.ExisteNovaVersao(versaoAtual.Id, CancellationToken.None);
                        existeNovaVersaoCritica = await Model.DataAccessLayer.Classes.Versao.ExisteNovaVersaoCritica(versaoAtual.Id, CancellationToken.None);

                        if (existeNovaVersao)
                        {
                            var win = new MahApps.Metro.Controls.MetroWindow();
                            win.Height = 640;
                            win.Width = 760;
                            win.Content = new NovaVersaoViewModel(DialogCoordinator.Instance, versaoAtual, existeNovaVersaoCritica);
                            win.Title = existeNovaVersaoCritica ? "Nova versão crítica disponível" : "Nova versão opcional disponível";
                            win.ShowDialogsOverTitleBar = false;
                            win.Owner = App.Current.MainWindow;
                            win.Closing += Win_Closing;
                            win.ShowDialog();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Escreve no log a exceção e uma mensagem de erro
                        Serilog.Log.Error(ex, "Erro na verificação de versão");

                        MensagemErroLogin = "Falha na verificação de novas versões. Caso o problema persista, contate o desenvolvedor e envie o log";
                        ExibeMensagemErroLogin = true;
                    }

                    MensagemErroLogin = "";
                    ExibeMensagemErroLogin = false;

                    // Set starting page
                    //CurrentPageViewModel = PageViewModels[0];
                }
                catch (Exception ex)
                {
                    DateTime dataAtualizacao = instanciaLocal.DataAtualizacao == null ? DateTime.Now : (DateTime)instanciaLocal.DataAtualizacao;
                    TimeSpan diasVerificacao = DateTime.Now - dataAtualizacao;

                    if (diasVerificacao.Days > 15)
                    {
                        // Escreve no log a exceção e uma mensagem de erro
                        Serilog.Log.Error(ex, "Erro na autenticação de instância (limite de 15 dias ultrapassado)");

                        MensagemErroLogin = "Falha na autenticação de instância (limite de 15 dias ultrapassado). Contate o desenvolvedor";
                        ExibeMensagemErroLogin = true;
                        CarregamentoVisivel = false;
                        return;
                    }
                }
                ControlesHabilitados = true;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro no carregamento de dados");

                MensagemErroLogin = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor";
                ExibeMensagemErroLogin = true;
            }

            CarregamentoVisivel = false;
        }

        private void Win_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (existeNovaVersaoCritica)
            {
                var resultado = MessageBox.Show("Como existe uma versão obrigatória, o sistema será encerrado. Deseja continuar?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                if (resultado != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }

        #endregion Métodos
    }
}