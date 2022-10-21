using GalaSoft.MvvmLight.Messaging;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using SGT.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SGT
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Constantes

        private const int TEMPO_ESPERA_SEGUNDOS = 15;
        private DispatcherTimer _timer;
        private TimeSpan _time;

        #endregion Constantes

        #region Campos

        private ICommand _changePageCommand;

        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        private double _valorProgresso;
        private bool _progressoEhIndeterminavel;
        private bool _progressoVisivel = false;
        private string _mensagemStatus;
        private int _paginaAtual;
        private double? _alturaJanela = 450;
        private double? _larguraJanela = 800;
        private double _alturaGrid = 25;
        private int _tempoEspera = 30;

        private bool _fundoVisivel;
        private string _textoFundo;

        #endregion Campos

        public MainWindowViewModel()
        {
            // Adiciona os controles disponíveis
            PageViewModels.Add(new LoginViewModel());
            //PageViewModels.Add(new PrincipalViewModel());

            Messenger.Default.Register<int>(this, "PaginaAtualGeral", delegate (int paginaRecebida) { PaginaAtual = paginaRecebida; });

            // Define o controle inicial
            CurrentPageViewModel = PageViewModels[0];

            Messenger.Default.Register<IPageViewModel>(this, "PaginaAdicionar", delegate (IPageViewModel paginaAdicionar) { PageViewModels.Add(paginaAdicionar); });

            Messenger.Default.Register<int>(this, "TempoDuracaoMensagem", delegate (int tempoDuracaoMensagem) { TempoEspera = tempoDuracaoMensagem; });
            Messenger.Default.Register<string>(this, "MensagemStatus", delegate (string mensagemStatusRecebida) { MensagemStatus = mensagemStatusRecebida; });
            Messenger.Default.Register<double>(this, "ValorProgresso", delegate (double valorProgressoRecebido) { ValorProgresso = valorProgressoRecebido; });
            Messenger.Default.Register<bool>(this, "ProgressoEhIndeterminavel", delegate (bool progressoEhIndeterminavelRecebido) { ProgressoEhIndeterminavel = progressoEhIndeterminavelRecebido; });

            Messenger.Default.Register<bool>(this, "FundoPrincipalVisivel", delegate (bool fundoPrincipalVisivel) { FundoVisivel = fundoPrincipalVisivel; });
            Messenger.Default.Register<string>(this, "TextoFundoPrincipal", delegate (string textoFundoPrincipal) { TextoFundo = textoFundoPrincipal; });

            Messenger.Default.Register<double?>(this, "AlturaJanela", delegate (double? alturaJanela) { AlturaJanela = alturaJanela; });
            Messenger.Default.Register<double?>(this, "LarguraJanela", delegate (double? larguraJanela) { LarguraJanela = larguraJanela; });
        }

        #region Propriedades/Comandos

        public double? AlturaJanela
        {
            get
            {
                return _alturaJanela;
            }
            set
            {
                if (value != _alturaJanela)
                {
                    _alturaJanela = value;
                    OnPropertyChanged(nameof(AlturaJanela));
                }
            }
        }

        public double? LarguraJanela
        {
            get
            {
                return _larguraJanela;
            }
            set
            {
                if (value != _larguraJanela)
                {
                    _larguraJanela = value;
                    OnPropertyChanged(nameof(LarguraJanela));
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

        public bool FundoVisivel
        {
            get { return _fundoVisivel; }
            set
            {
                _fundoVisivel = value;
                OnPropertyChanged(nameof(FundoVisivel));
            }
        }

        public string TextoFundo
        {
            get { return _textoFundo; }
            set
            {
                _textoFundo = value;
                OnPropertyChanged(nameof(TextoFundo));
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

        #endregion Métodos
    }
}