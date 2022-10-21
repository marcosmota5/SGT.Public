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
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class ParametroStatusAprovacaoViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private StatusAprovacao _statusAprovacaoSelecionado = new();
        private StatusAprovacao _statusAprovacaoAlterar = new();
        private CancellationTokenSource _cts;

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

        private ObservableCollection<StatusAprovacao> _listaStatusAprovacao = new();
        private ObservableCollection<Status> _listaStatus = new();

        private ICommand _comandoSalvar;
        private ICommand _comandoAdicionar;
        private ICommand _comandoEditar;
        private ICommand _comandoDeletar;
        private ICommand _comandoCancelar;

        #endregion Campos

        #region Construtores

        public ParametroStatusAprovacaoViewModel(IDialogCoordinator dialogCoordinator)
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
                return "Status da aprovação";
            }
        }

        public string Icone
        {
            get
            {
                return "ListStatus";
            }
        }

        public void LimparViewModel()
        {
            try
            {
                _statusAprovacaoSelecionado = null;
                _cts = null;
                _listaStatusAprovacao = null;
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

        public StatusAprovacao StatusAprovacaoSelecionado
        {
            get { return _statusAprovacaoSelecionado; }
            set
            {
                _statusAprovacaoSelecionado = value;
                OnPropertyChanged(nameof(StatusAprovacaoSelecionado));
                if (StatusAprovacaoSelecionado == null)
                {
                    StatusAprovacaoAlterar = null;
                }
                else
                {
                    StatusAprovacaoAlterar = (StatusAprovacao)StatusAprovacaoSelecionado.Clone();
                    PermiteEditar = StatusAprovacaoAlterar != null;
                    PermiteDeletar = StatusAprovacaoAlterar != null;
                    // Define valores
                    try
                    {
                        StatusAprovacaoAlterar.Status = ListaStatus.First(stat => stat.Id == StatusAprovacaoSelecionado.Status.Id);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public StatusAprovacao StatusAprovacaoAlterar
        {
            get { return _statusAprovacaoAlterar; }
            set
            {
                _statusAprovacaoAlterar = value;
                OnPropertyChanged(nameof(StatusAprovacaoAlterar));
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

        public ObservableCollection<StatusAprovacao> ListaStatusAprovacao
        {
            get { return _listaStatusAprovacao; }
            set
            {
                if (value != _listaStatusAprovacao)
                {
                    _listaStatusAprovacao = value;
                    OnPropertyChanged(nameof(ListaStatusAprovacao));
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

        #endregion Propriedades/Comandos

        #region Métodos

        public async Task ConstrutorAsync()
        {
            try
            {
                CarregamentoVisivel = true;

                // Preenche as listas com as classes necessárias
                await StatusAprovacao.PreencheListaStatusAprovacaoAsync(ListaStatusAprovacao, true, null, CancellationToken.None, "ORDER BY stap.nome ASC", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");

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
            StatusAprovacaoAlterar = new StatusAprovacao();
            StatusAprovacaoAlterar.Status = null;

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
            PermiteEditar = StatusAprovacaoAlterar != null && StatusAprovacaoAlterar?.Id != null; ;
            PermiteCancelar = false;
            PermiteSalvar = false;
            PermiteDeletar = StatusAprovacaoAlterar != null && StatusAprovacaoAlterar?.Id != null; ;
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
            MensagemStatus = "Salvando dados do status da aprovação '" + StatusAprovacaoAlterar.Nome + "', aguarde...";
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

            try
            {
                await StatusAprovacaoAlterar.SalvarStatusAprovacaoDatabaseAsync(_cts.Token);
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

            int? idStatusAprovacaoAlterado = StatusAprovacaoAlterar.Id;

            try
            {
                await StatusAprovacao.PreencheListaStatusAprovacaoAsync(ListaStatusAprovacao, true, null, CancellationToken.None, "ORDER BY stap.nome ASC", "");
            }
            catch (Exception)
            {
            }

            try
            {
                StatusAprovacaoSelecionado = ListaStatusAprovacao.First(stap => stap.Id == idStatusAprovacaoAlterado);
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
                "Atenção", "Tem certeza que deseja excluir o status da aprovação '" + StatusAprovacaoAlterar.Nome + "'? " +
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
            MensagemStatus = "Deletando status da aprovação '" + StatusAprovacaoAlterar.Nome + "', aguarde...";
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
                await StatusAprovacaoAlterar.DeletarStatusAprovacaoDatabaseAsync(_cts.Token);
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

            StatusAprovacaoSelecionado = null;
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
            MensagemStatus = "Status da aprovação excluído com sucesso!";

            try
            {
                await StatusAprovacao.PreencheListaStatusAprovacaoAsync(ListaStatusAprovacao, true, null, CancellationToken.None, "ORDER BY stap.nome ASC", "");
            }
            catch (Exception)
            {
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
        }

        #endregion Métodos
    }
}