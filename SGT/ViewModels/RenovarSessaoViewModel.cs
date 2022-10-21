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
using System.Windows.Threading;

namespace SGT.ViewModels
{
    public class RenovarSessaoViewModel : ObservableObject, IPageViewModel
    {
        #region Constantes

        private DispatcherTimer _timer;
        private TimeSpan _time;

        #endregion Constantes

        #region Campos

        private ICommand _changePageCommand;
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        private int _paginaAtual;
        private string _versao;
        private bool _exibeMensagemErroLogin;
        private string _mensagemErroLogin;
        private bool _controlesHabilitados;
        private bool _carregamentoVisivel = true;
        private bool existeNovaVersaoCritica;
        private bool _permiteFechar;

        private double _valorProgresso;
        private bool _progressoEhIndeterminavel;
        private bool _progressoVisivel = false;
        private string _mensagemStatus;
        private double _alturaGrid = 25;
        private int _tempoEspera = 30;

        #endregion Campos

        public RenovarSessaoViewModel(bool permiteFechar)
        {
            PermiteFechar = permiteFechar;

            Messenger.Default.Send<bool>(true, "LoginAposUso");

            // Add available pages
            PageViewModels.Add(new EntrarViewModel());
            PageViewModels.Add(new EsqueciASenhaViewModel());
            PageViewModels.Add(new AlterarSenhaSemAtualViewModel());

            Messenger.Default.Register<int>(this, "PaginaAtualLogin", delegate (int paginaRecebida) { PaginaAtual = paginaRecebida; });
            Messenger.Default.Register<bool>(this, "SessaoRenovada", delegate (bool sessaoRenovada)
            {
                if (sessaoRenovada)
                {
                    ComandoFechar.Execute(null);
                }
            });

            Messenger.Default.Register<int>(this, "TempoDuracaoMensagem", delegate (int tempoDuracaoMensagem) { TempoEspera = tempoDuracaoMensagem; });
            Messenger.Default.Register<string>(this, "MensagemStatus", delegate (string mensagemStatusRecebida) { MensagemStatus = mensagemStatusRecebida; });
            Messenger.Default.Register<double>(this, "ValorProgresso", delegate (double valorProgressoRecebido) { ValorProgresso = valorProgressoRecebido; });
            Messenger.Default.Register<bool>(this, "ProgressoEhIndeterminavel", delegate (bool progressoEhIndeterminavelRecebido) { ProgressoEhIndeterminavel = progressoEhIndeterminavelRecebido; });

            // Set starting page
            CurrentPageViewModel = PageViewModels[0];

            ConstrutorAsync().Await();
        }

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Renovar sessão";
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
                _pageViewModels = null;
                _currentPageViewModel = null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICommand ComandoFechar { get; set; }

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

        public int TempoEspera
        {
            get
            {
                return _tempoEspera;
            }
            set
            {
                if (value != _tempoEspera)
                {
                    _tempoEspera = value;
                    OnPropertyChanged(nameof(TempoEspera));
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

                    if (!String.IsNullOrEmpty(MensagemStatus))
                    {
                        // Definição do tempo que deve ser aguardado de acordo com a constante
                        _time = TimeSpan.FromSeconds(TempoEspera);

                        // Definição do timer
                        _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                        {
                            if (_time == TimeSpan.Zero)
                            {
                                MensagemStatus = "";
                                _timer.Stop();
                            }
                            _time = _time.Add(TimeSpan.FromSeconds(-1));
                        }, Application.Current.Dispatcher);

                        // Inicia o tempo
                        _timer.Start();
                    }
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

        public bool ProgressoVisivel
        {
            get { return _progressoVisivel; }
            set
            {
                if (value != _progressoVisivel)
                {
                    _progressoVisivel = value;
                    if (_progressoVisivel)
                    {
                        AlturaGrid = 35;
                    }
                    else
                    {
                        AlturaGrid = 25;
                    }
                    OnPropertyChanged(nameof(ProgressoVisivel));
                }
            }
        }

        public double AlturaGrid
        {
            get
            {
                return _alturaGrid;
            }
            set
            {
                if (value != _alturaGrid)
                {
                    _alturaGrid = value;
                    OnPropertyChanged(nameof(AlturaGrid));
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

        public bool PermiteFechar
        {
            get { return _permiteFechar; }
            set
            {
                if (_permiteFechar != value)
                {
                    _permiteFechar = value;
                    OnPropertyChanged(nameof(PermiteFechar));
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
                try
                {
                    Versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

                    Versao versaoAtual = new();
                    await versaoAtual.GetVersaoDatabaseAsync(CancellationToken.None, false, "WHERE nome = @nome", "@nome", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

                    bool existeNovaVersao = await Model.DataAccessLayer.Classes.Versao.ExisteNovaVersao(versaoAtual.Id, CancellationToken.None);
                    existeNovaVersaoCritica = await Model.DataAccessLayer.Classes.Versao.ExisteNovaVersaoCritica(versaoAtual.Id, CancellationToken.None);

                    if (existeNovaVersaoCritica)
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