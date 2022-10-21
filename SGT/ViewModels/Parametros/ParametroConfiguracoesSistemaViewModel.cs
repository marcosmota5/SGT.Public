using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.HelperClasses;
using Ookii.Dialogs.Wpf;
using SGT.HelperClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class ParametroConfiguracoesSistemaViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private ConfiguracaoSistema _configuracaoSistemaInicial = new();
        private ConfiguracaoSistema _configuracaoSistemaAlterar = new();
        private CancellationTokenSource _cts;

        private bool _controlesHabilitados;
        private bool _progressoEhIndeterminavel = true;
        private bool _progressoVisivel = false;
        private double _valorProgresso = 0;
        private string _textoProgresso;
        private string _mensagemStatus;

        private bool _cancelarVisivel;

        private bool _permiteSalvar = true;
        private bool _permiteCancelar;

        private bool _carregamentoVisivel = true;

        private readonly IDialogCoordinator _dialogCoordinator;

        private ICommand _comandoSalvar;
        private ICommand _comandoCancelar;
        private ICommand _comandoSelecionarCaminhoPastaEstoque;
        private ICommand _comandoSelecionarCaminhoPastaOrdensServico;

        #endregion Campos

        #region Construtores

        public ParametroConfiguracoesSistemaViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;

            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Configurações do sistema";
            }
        }

        public string Icone
        {
            get
            {
                return "CogOutline";
            }
        }

        public void LimparViewModel()
        {
            try
            {
                _configuracaoSistemaInicial = null;
                _cts = null;
                _comandoSalvar = null;
                _comandoCancelar = null;
                _comandoSelecionarCaminhoPastaEstoque = null;
                _comandoSelecionarCaminhoPastaOrdensServico = null;
            }
            catch (Exception)
            {
            }
        }

        public bool ExistemCamposVazios { private get; set; }

        public ConfiguracaoSistema ConfiguracaoSistemaInicial
        {
            get { return _configuracaoSistemaInicial; }
            set
            {
                _configuracaoSistemaInicial = value;
                OnPropertyChanged(nameof(ConfiguracaoSistemaInicial));
            }
        }

        public ConfiguracaoSistema ConfiguracaoSistemaAlterar
        {
            get { return _configuracaoSistemaAlterar; }
            set
            {
                _configuracaoSistemaAlterar = value;
                OnPropertyChanged(nameof(ConfiguracaoSistemaAlterar));
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

        public ICommand ComandoSelecionarCaminhoPastaEstoque
        {
            get
            {
                if (_comandoSelecionarCaminhoPastaEstoque == null)
                {
                    _comandoSelecionarCaminhoPastaEstoque = new RelayCommand(
                        param => SelecionarPastaEstoque(),
                        param => true
                    );
                }
                return _comandoSelecionarCaminhoPastaEstoque;
            }
        }

        public ICommand ComandoSelecionarCaminhoPastaOrdensServico
        {
            get
            {
                if (_comandoSelecionarCaminhoPastaOrdensServico == null)
                {
                    _comandoSelecionarCaminhoPastaOrdensServico = new RelayCommand(
                        param => SelecionarPastaOrdensServico(),
                        param => true
                    );
                }
                return _comandoSelecionarCaminhoPastaOrdensServico;
            }
        }

        #endregion Propriedades/Comandos

        #region Métodos

        public async Task ConstrutorAsync()
        {
            try
            {
                ConfiguracaoSistemaInicial = new();

                await ConfiguracaoSistemaInicial.GetConfiguracaoSistemaDatabaseAsync(1, CancellationToken.None);

                ConfiguracaoSistemaAlterar = (ConfiguracaoSistema)ConfiguracaoSistemaInicial.Clone();

                CarregamentoVisivel = true;

                // Redefine as permissões
                PermiteSalvar = true;
                PermiteCancelar = false;
                CancelarVisivel = false;
                ControlesHabilitados = true;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");

                MensagemStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                PermiteSalvar = false;
                PermiteCancelar = false;
                CancelarVisivel = false;
                ControlesHabilitados = false;
            }

            CarregamentoVisivel = false;
        }

        private void Cancelar()
        {
            if (_cts != null)
                _cts.Cancel();

            ControlesHabilitados = true;
            CancelarVisivel = false;
            PermiteCancelar = false;
            PermiteSalvar = true;
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
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = true;
            MensagemStatus = "Salvando configurações, aguarde...";
            CancelarVisivel = true;
            PermiteCancelar = true;
            PermiteSalvar = false;

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

            try
            {
                await ConfiguracaoSistemaAlterar.SalvarConfiguracaoSistemaDatabaseAsync(_cts.Token);
            }
            catch (Exception ex)
            {
                ValorProgresso = 0;
                ControlesHabilitados = true;
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = true;

                CancelarVisivel = false;
                PermiteCancelar = false;
                PermiteSalvar = true;

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

            try
            {
                await ConfiguracaoSistemaInicial.GetConfiguracaoSistemaDatabaseAsync(1, CancellationToken.None);
            }
            catch (Exception)
            {
            }

            ValorProgresso = 0;
            ControlesHabilitados = false;
            ProgressoVisivel = false;
            ProgressoEhIndeterminavel = false;
            CancelarVisivel = false;
            PermiteCancelar = false;
            PermiteSalvar = true;
            MensagemStatus = "Dados salvos com sucesso!";
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
        }

        private void SelecionarPastaEstoque()
        {
            VistaFolderBrowserDialog vistaFolderBrowserDialog = new()
            {
                Multiselect = false
            };

            if ((bool)vistaFolderBrowserDialog.ShowDialog())
            {
                ConfiguracaoSistemaAlterar.LocalArquivoEstoque = vistaFolderBrowserDialog.SelectedPath;
            }
        }

        private void SelecionarPastaOrdensServico()
        {
            VistaFolderBrowserDialog vistaFolderBrowserDialog = new()
            {
                Multiselect = false
            };

            if ((bool)vistaFolderBrowserDialog.ShowDialog())
            {
                ConfiguracaoSistemaAlterar.LocalOrdensServico = vistaFolderBrowserDialog.SelectedPath;
            }
        }

        #endregion Métodos
    }
}