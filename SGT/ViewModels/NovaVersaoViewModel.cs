using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using SGT.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HttpClientProgress;
using System.Diagnostics;

namespace SGT.ViewModels
{
    public class NovaVersaoViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private bool _carregamentoVisivel = true;
        private bool _visibilidadeImagemInformacao;
        private bool _visibilidadeImagemAtencao;
        private string _textoAtualizacao;
        private readonly IDialogCoordinator _dialogCoordinator;
        private Versao _versaoAtual;
        private IPageViewModel _logAlteracoesViewModel;
        private Versao _novaVersao = new();
        private CancellationTokenSource _cts;
        private bool _atualizacaoObrigatoria;
        private double _valorProgresso;

        private ICommand _comandoCancelar;
        private ICommand _comandoBaixarInstalar;

        #endregion Campos

        #region Construtores

        public NovaVersaoViewModel(IDialogCoordinator dialogCoordinator, Versao versaoAtual, bool atualizacaoObrigatoria)
        {
            _dialogCoordinator = dialogCoordinator;

            _atualizacaoObrigatoria = atualizacaoObrigatoria;
            VersaoAtual = versaoAtual;

            if (atualizacaoObrigatoria)
            {
                VisibilidadeImagemInformacao = false;
                VisibilidadeImagemAtencao = true;
                TextoAtualizacao = "Existe uma nova versão obrigatória do sistema, não será possível utilizar a versão atual. Deseja efetuar o download e instalar agora?";
            }
            else
            {
                VisibilidadeImagemInformacao = true;
                VisibilidadeImagemAtencao = false;
                TextoAtualizacao = "Existe uma nova versão opcional do sistema. Deseja efetuar o download e instalar agora?";
            }

            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Propriedades / Comandos

        public string Name
        {
            get
            {
                return "Nova versão";
            }
        }

        public string Icone
        {
            get
            {
                return "";
            }
        }

        public Action CloseAction { get; set; }

        public void LimparViewModel()
        {
            try
            {
                LogAlteracoesViewModel = null;
            }
            catch (Exception)
            {
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

        public bool VisibilidadeImagemInformacao
        {
            get { return _visibilidadeImagemInformacao; }
            set
            {
                if (_visibilidadeImagemInformacao != value)
                {
                    _visibilidadeImagemInformacao = value;
                    OnPropertyChanged(nameof(VisibilidadeImagemInformacao));
                }
            }
        }

        public bool VisibilidadeImagemAtencao
        {
            get { return _visibilidadeImagemAtencao; }
            set
            {
                if (_visibilidadeImagemAtencao != value)
                {
                    _visibilidadeImagemAtencao = value;
                    OnPropertyChanged(nameof(VisibilidadeImagemAtencao));
                }
            }
        }

        public string TextoAtualizacao
        {
            get { return _textoAtualizacao; }
            set
            {
                if (_textoAtualizacao != value)
                {
                    _textoAtualizacao = value;
                    OnPropertyChanged(nameof(TextoAtualizacao));
                }
            }
        }

        public Versao VersaoAtual
        {
            get { return _versaoAtual; }
            set
            {
                if (_versaoAtual != value)
                {
                    _versaoAtual = value;
                    OnPropertyChanged(nameof(VersaoAtual));
                }
            }
        }

        public IPageViewModel LogAlteracoesViewModel
        {
            get
            {
                return _logAlteracoesViewModel;
            }
            set
            {
                if (_logAlteracoesViewModel != value)
                {
                    _logAlteracoesViewModel = value;
                    OnPropertyChanged(nameof(LogAlteracoesViewModel));
                }
            }
        }

        public Versao NovaVersao
        {
            get { return _novaVersao; }
            set
            {
                if (_novaVersao != value)
                {
                    _novaVersao = value;
                    OnPropertyChanged(nameof(NovaVersao));
                }
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

        public ICommand ComandoBaixarInstalar
        {
            get
            {
                if (_comandoBaixarInstalar == null)
                {
                    _comandoBaixarInstalar = new RelayCommand(
                        param => BaixarInstalar().Await(),
                        param => true
                    );
                }
                return _comandoBaixarInstalar;
            }
        }

        #endregion Propriedades / Comandos

        #region Métodos

        public async Task ConstrutorAsync()
        {
            try
            {
                await NovaVersao.GetVersaoDatabaseAsync(CancellationToken.None, false, "WHERE id_versao > @id_versao ORDER BY id_versao DESC LIMIT 1", "@id_versao", VersaoAtual.Id);

                LogAlteracoesViewModel = new LogAlteracoesViewModel(VersaoAtual);
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");
            }
            CarregamentoVisivel = false;
        }

        private void Cancelar()
        {
            if (_atualizacaoObrigatoria)
            {
                var resultado = MessageBox.Show("Como existe uma versão obrigatória, o sistema será encerrado. Deseja continuar?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                if (resultado == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
            }
            else
            {
                CloseAction();
            }
        }

        private async Task BaixarInstalar()
        {
            ValorProgresso = 0;
            _cts = new();

            Progress<double> progresso = new(dbl =>
            {
                ValorProgresso = dbl;
            });

            var customDialog = new CustomDialog();

            var dataContext = new CustomProgressViewModel("Efetuando download", "Aguarde...", false, true, _cts, instance =>
            {
                _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            });

            customDialog.Content = new CustomProgressView { DataContext = dataContext };

            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);

            try
            {
                string docUrl = "https://www.proreports.com.br/proreports_sgt/SGT_Setup.exe";
                string username = "proreports_sgt";
                string password = "=txn4j-kNI]d";
                string filePath = Path.GetTempPath() + "Proreports\\SGT\\Instaladores\\SGT_Setup.exe";

                Directory.CreateDirectory(Path.GetTempPath() + "Proreports\\SGT\\Instaladores");
                
                using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(username, password) })
                {
                    var httpClient = new HttpClient(handler);

                    // Use the provided extension method
                    using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await httpClient.DownloadDataAsync(docUrl, file, progresso, _cts.Token);
                    }
                }

                dataContext.Titulo = "Download concluído";
                dataContext.Mensagem = "Iniciando instalação...";

                await Task.Delay(1000);

                Process.Start(filePath);
                Application.Current.Shutdown();

            }
            catch (OperationCanceledException)
            {
                Messenger.Default.Send<string>("Operação cancelada", "MensagemStatus");
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao baixar a nova versão");

                await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Erro ao baixar/instalar a nova versão", "Não foi possível efetuar o download da nova versão. Caso o problema persista, contante o desenvolvedor e envie o arquivo de log", MessageDialogStyle.Affirmative, mySettings);
            }

            try
            {
                await _dialogCoordinator.HideMetroDialogAsync(this, customDialog);
            }
            catch (Exception)
            {
            }
        }

        #endregion Métodos
    }
}
