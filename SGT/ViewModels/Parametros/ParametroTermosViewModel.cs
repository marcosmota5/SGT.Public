using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class ParametroTermosViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private Termo _termoSelecionado = new();
        private Termo _termoAlterar = new();
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

        private ObservableCollection<Termo> _listaTermos = new();
        private ObservableCollection<Status> _listaStatus = new();
        private ObservableCollection<Cliente> _listaClientesAssociados = new();
        private ObservableCollection<Setor> _listaSetoresAssociados = new();
        private ObservableCollection<Cliente> _listaClientesDisponiveis = new();
        private ObservableCollection<Setor> _listaSetoresDisponiveis = new();
        private ObservableCollection<Cliente> _listaClientes = new();
        private ObservableCollection<Setor> _listaSetores = new();
        private ObservableCollection<TermoSuporte> _listaTermosSuporte = new();

        private ICommand _comandoSalvar;
        private ICommand _comandoAdicionar;
        private ICommand _comandoEditar;
        private ICommand _comandoDeletar;
        private ICommand _comandoCancelar;

        private ICommand _comandoAssociarTodosSetores;
        private ICommand _comandoAssociarSetores;
        private ICommand _comandoDesassociarSetores;
        private ICommand _comandoDesassociarTodosSetores;

        private ICommand _comandoAssociarTodosClientes;
        private ICommand _comandoAssociarClientes;
        private ICommand _comandoDesassociarClientes;
        private ICommand _comandoDesassociarTodosClientes;

        #endregion Campos

        #region Construtores

        public ParametroTermosViewModel(IDialogCoordinator dialogCoordinator)
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
                return "Termos";
            }
        }

        public string Icone
        {
            get
            {
                return "FileDocumentMultiple";
            }
        }

        public void LimparViewModel()
        {
            try
            {
                _termoSelecionado = null;
                _cts = null;
                _listaTermos = null;
                _listaStatus = null;
                _listaClientesAssociados = null;
                _listaSetoresAssociados = null;
                _listaClientesDisponiveis = null;
                _listaSetoresDisponiveis = null;
                _listaClientes = null;
                _listaSetores = null;
                _listaTermosSuporte = null;
                _comandoSalvar = null;
                _comandoAdicionar = null;
                _comandoEditar = null;
                _comandoDeletar = null;
                _comandoCancelar = null;

                _comandoAssociarTodosSetores = null;
                _comandoAssociarSetores = null;
                _comandoDesassociarSetores = null;
                _comandoDesassociarTodosSetores = null;

                _comandoAssociarTodosClientes = null;
                _comandoAssociarClientes = null;
                _comandoDesassociarClientes = null;
                _comandoDesassociarTodosClientes = null;
            }
            catch (Exception)
            {
            }
        }

        public bool ExistemCamposVazios { private get; set; }

        public List<object> ListaSetoresDisponiveisSelecionados { private get; set; }

        public List<object> ListaSetoresAssociadosSelecionados { private get; set; }

        public List<object> ListaClientesDisponiveisSelecionados { private get; set; }

        public List<object> ListaClientesAssociadosSelecionados { private get; set; }

        public Termo TermoSelecionado
        {
            get { return _termoSelecionado; }
            set
            {
                _termoSelecionado = value;
                OnPropertyChanged(nameof(TermoSelecionado));
                if (TermoSelecionado == null)
                {
                    TermoAlterar = null;
                }
                else
                {
                    TermoAlterar = (Termo)TermoSelecionado.Clone();
                    PermiteEditar = TermoAlterar != null;
                    PermiteDeletar = TermoAlterar != null;
                    // Define valores
                    try
                    {
                        TermoAlterar.Status = ListaStatus.First(stat => stat.Id == TermoSelecionado.Status.Id);
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        ListaClientesAssociados.Clear();
                        ListaClientesDisponiveis.Clear();
                        ListaSetoresAssociados.Clear();
                        ListaSetoresDisponiveis.Clear();

                        foreach (var item in _listaTermosSuporte.Where(d => d.Cliente?.Id != null && d.Termo?.Id == TermoAlterar?.Id).DistinctBy(d => d.Cliente?.Id))
                        {
                            ListaClientesAssociados.Add(item?.Cliente);
                        }
                        foreach (var item in _listaTermosSuporte.Where(d => d.Setor?.Id != null && d.Termo?.Id == TermoAlterar?.Id).DistinctBy(d => d.Setor?.Id))
                        {
                            ListaSetoresAssociados.Add(item?.Setor);
                        }
                        foreach (var item in _listaClientes.Where(item => !ListaClientesAssociados.Any(item2 => item2.Id == item.Id)).DistinctBy(c => c.Id))
                        {
                            ListaClientesDisponiveis.Add(item);
                        }
                        foreach (var item in _listaSetores.Where(item => !ListaSetoresAssociados.Any(item2 => item2.Id == item.Id)).DistinctBy(s => s.Id))
                        {
                            ListaSetoresDisponiveis.Add(item);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public Termo TermoAlterar
        {
            get { return _termoAlterar; }
            set
            {
                _termoAlterar = value;
                OnPropertyChanged(nameof(TermoAlterar));
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

        public ObservableCollection<Termo> ListaTermos
        {
            get { return _listaTermos; }
            set
            {
                if (value != _listaTermos)
                {
                    _listaTermos = value;
                    OnPropertyChanged(nameof(ListaTermos));
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

        public ObservableCollection<Cliente> ListaClientesAssociados
        {
            get { return _listaClientesAssociados; }
            set
            {
                if (value != _listaClientesAssociados)
                {
                    _listaClientesAssociados = value;
                    OnPropertyChanged(nameof(ListaClientesAssociados));
                }
            }
        }

        public ObservableCollection<Setor> ListaSetoresAssociados
        {
            get { return _listaSetoresAssociados; }
            set
            {
                if (value != _listaSetoresAssociados)
                {
                    _listaSetoresAssociados = value;
                    OnPropertyChanged(nameof(ListaSetoresAssociados));
                }
            }
        }

        public ObservableCollection<Cliente> ListaClientesDisponiveis
        {
            get { return _listaClientesDisponiveis; }
            set
            {
                if (value != _listaClientesDisponiveis)
                {
                    _listaClientesDisponiveis = value;
                    OnPropertyChanged(nameof(ListaClientesDisponiveis));
                }
            }
        }

        public ObservableCollection<Setor> ListaSetoresDisponiveis
        {
            get { return _listaSetoresDisponiveis; }
            set
            {
                if (value != _listaSetoresDisponiveis)
                {
                    _listaSetoresDisponiveis = value;
                    OnPropertyChanged(nameof(ListaSetoresDisponiveis));
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

        public ICommand ComandoAssociarTodosSetores
        {
            get
            {
                if (_comandoAssociarTodosSetores == null)
                {
                    _comandoAssociarTodosSetores = new RelayCommand(
                        param => AssociarTodosSetores(),
                        param => true
                    );
                }
                return _comandoAssociarTodosSetores;
            }
        }

        public ICommand ComandoAssociarSetores
        {
            get
            {
                if (_comandoAssociarSetores == null)
                {
                    _comandoAssociarSetores = new RelayCommand(
                        param => AssociarSetores(),
                        param => true
                    );
                }
                return _comandoAssociarSetores;
            }
        }

        public ICommand ComandoDesassociarSetores
        {
            get
            {
                if (_comandoDesassociarSetores == null)
                {
                    _comandoDesassociarSetores = new RelayCommand(
                        param => DesassociarSetores(),
                        param => true
                    );
                }
                return _comandoDesassociarSetores;
            }
        }

        public ICommand ComandoDesassociarTodosSetores
        {
            get
            {
                if (_comandoDesassociarTodosSetores == null)
                {
                    _comandoDesassociarTodosSetores = new RelayCommand(
                        param => DesassociarTodosSetores(),
                        param => true
                    );
                }
                return _comandoDesassociarTodosSetores;
            }
        }

        public ICommand ComandoAssociarTodosClientes
        {
            get
            {
                if (_comandoAssociarTodosClientes == null)
                {
                    _comandoAssociarTodosClientes = new RelayCommand(
                        param => AssociarTodosClientes(),
                        param => true
                    );
                }
                return _comandoAssociarTodosClientes;
            }
        }

        public ICommand ComandoAssociarClientes
        {
            get
            {
                if (_comandoAssociarClientes == null)
                {
                    _comandoAssociarClientes = new RelayCommand(
                        param => AssociarClientes(),
                        param => true
                    );
                }
                return _comandoAssociarClientes;
            }
        }

        public ICommand ComandoDesassociarClientes
        {
            get
            {
                if (_comandoDesassociarClientes == null)
                {
                    _comandoDesassociarClientes = new RelayCommand(
                        param => DesassociarClientes(),
                        param => true
                    );
                }
                return _comandoDesassociarClientes;
            }
        }

        public ICommand ComandoDesassociarTodosClientes
        {
            get
            {
                if (_comandoDesassociarTodosClientes == null)
                {
                    _comandoDesassociarTodosClientes = new RelayCommand(
                        param => DesassociarTodosClientes(),
                        param => true
                    );
                }
                return _comandoDesassociarTodosClientes;
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
                await Termo.PreencheListaTermosAsync(ListaTermos, true, null, CancellationToken.None, "ORDER BY term.nome ASC", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");
                await Cliente.PreencheListaClientesAsync(_listaClientes, true, null, CancellationToken.None, "ORDER BY clie.nome ASC", "");
                await Setor.PreencheListaSetoresAsync(_listaSetores, true, null, CancellationToken.None, "ORDER BY seto.nome ASC", "");
                await TermoSuporte.PreencheListaTermosSuporteAsync(_listaTermosSuporte, true, null, CancellationToken.None, "", "");

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
            TermoAlterar = new Termo();
            TermoAlterar.Status = null;

            ControlesHabilitados = true;
            ListaHabilitada = false;
            SalvarVisivel = true;
            CancelarVisivel = true;
            PermiteAdicionar = false;
            PermiteEditar = false;
            PermiteCancelar = true;
            PermiteSalvar = true;
            PermiteDeletar = false;

            ListaClientesAssociados.Clear();
            ListaSetoresAssociados.Clear();
            ListaClientesDisponiveis.Clear();
            ListaSetoresDisponiveis.Clear();

            foreach (var item in _listaClientes.Where(item => !ListaClientesAssociados.Any(item2 => item2.Id == item.Id)).DistinctBy(c => c.Id))
            {
                ListaClientesDisponiveis.Add(item);
            }
            foreach (var item in _listaSetores.Where(item => !ListaSetoresAssociados.Any(item2 => item2.Id == item.Id)).DistinctBy(s => s.Id))
            {
                ListaSetoresDisponiveis.Add(item);
            }
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
            PermiteEditar = TermoAlterar != null && TermoAlterar?.Id != null; ;
            PermiteCancelar = false;
            PermiteSalvar = false;
            PermiteDeletar = TermoAlterar != null && TermoAlterar?.Id != null; ;
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
            MensagemStatus = "Salvando dados do termo '" + TermoAlterar.Nome + "', aguarde...";
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
                await TermoAlterar.SalvarTermoDatabaseAsync(_cts.Token);

                TermoSuporte termoSuporte = new();
                termoSuporte.Termo = TermoAlterar;

                await termoSuporte.DeletarTermoSuporteDatabaseAsync(_cts.Token);

                foreach (var setor in ListaSetoresAssociados)
                {
                    TermoSuporte termoSuporteSetor = new();
                    termoSuporteSetor.Termo = TermoAlterar;
                    termoSuporteSetor.Setor = setor;
                    termoSuporteSetor.Cliente = null;
                    termoSuporteSetor.Status = TermoAlterar.Status;
                    await termoSuporteSetor.SalvarTermoSuporteDatabaseAsync(CancellationToken.None);
                }

                foreach (var cliente in ListaClientesAssociados)
                {
                    TermoSuporte termoSuporteCliente = new();
                    termoSuporteCliente.Termo = TermoAlterar;
                    termoSuporteCliente.Setor = null;
                    termoSuporteCliente.Cliente = cliente;
                    termoSuporteCliente.Status = TermoAlterar.Status;
                    await termoSuporteCliente.SalvarTermoSuporteDatabaseAsync(CancellationToken.None);
                }
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

            int? idTermoAlterado = TermoAlterar.Id;

            try
            {
                await Termo.PreencheListaTermosAsync(ListaTermos, true, null, CancellationToken.None, "ORDER BY term.nome ASC", "");
                await TermoSuporte.PreencheListaTermosSuporteAsync(_listaTermosSuporte, true, null, CancellationToken.None, "", "");
            }
            catch (Exception)
            {
            }

            try
            {
                TermoSelecionado = ListaTermos.First(forn => forn.Id == idTermoAlterado);
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
                "Atenção", "Tem certeza que deseja excluir o termo '" + TermoAlterar.Nome + "'? " +
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
            MensagemStatus = "Deletando termo '" + TermoAlterar.Nome + "', aguarde...";
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
                await TermoAlterar.DeletarTermoDatabaseAsync(_cts.Token);
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

            TermoSelecionado = null;
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
            MensagemStatus = "Termo excluído com sucesso!";

            try
            {
                await Termo.PreencheListaTermosAsync(ListaTermos, true, null, CancellationToken.None, "ORDER BY term.nome ASC", "");
                await TermoSuporte.PreencheListaTermosSuporteAsync(_listaTermosSuporte, true, null, CancellationToken.None, "", "");
            }
            catch (Exception)
            {
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
        }

        private void AssociarTodosSetores()
        {
            foreach (var item in ListaSetoresDisponiveis)
            {
                ListaSetoresAssociados.Add(item);
            }
            ListaSetoresDisponiveis.Clear();
        }

        private void AssociarSetores()
        {
            foreach (var item in ListaSetoresDisponiveisSelecionados)
            {
                ListaSetoresAssociados.Add((Setor)item);

                Setor setorARemover = ListaSetoresDisponiveis.First(d => d.Id == ((Setor)item).Id);

                ListaSetoresDisponiveis.Remove(setorARemover);
            }
        }

        private void DesassociarSetores()
        {
            foreach (var item in ListaSetoresAssociadosSelecionados)
            {
                ListaSetoresDisponiveis.Add((Setor)item);

                Setor setorARemover = ListaSetoresAssociados.First(d => d.Id == ((Setor)item).Id);

                ListaSetoresAssociados.Remove(setorARemover);
            }
        }

        private void DesassociarTodosSetores()
        {
            foreach (var item in ListaSetoresAssociados)
            {
                ListaSetoresDisponiveis.Add(item);
            }
            ListaSetoresAssociados.Clear();
        }

        private void AssociarTodosClientes()
        {
            foreach (var item in ListaClientesDisponiveis)
            {
                ListaClientesAssociados.Add(item);
            }
            ListaClientesDisponiveis.Clear();
        }

        private void AssociarClientes()
        {
            foreach (var item in ListaClientesDisponiveisSelecionados)
            {
                ListaClientesAssociados.Add((Cliente)item);

                Cliente ClienteARemover = ListaClientesDisponiveis.First(d => d.Id == ((Cliente)item).Id);

                ListaClientesDisponiveis.Remove(ClienteARemover);
            }
        }

        private void DesassociarClientes()
        {
            foreach (var item in ListaClientesAssociadosSelecionados)
            {
                ListaClientesDisponiveis.Add((Cliente)item);

                Cliente ClienteARemover = ListaClientesAssociados.First(d => d.Id == ((Cliente)item).Id);

                ListaClientesAssociados.Remove(ClienteARemover);
            }
        }

        private void DesassociarTodosClientes()
        {
            foreach (var item in ListaClientesAssociados)
            {
                ListaClientesDisponiveis.Add(item);
            }
            ListaClientesAssociados.Clear();
        }

        #endregion Métodos
    }
}