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
using System.Windows.Data;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class ParametroContatosViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private Contato _contatoSelecionado = new();
        private Contato _contatoAlterar = new();
        private CancellationTokenSource _cts;

        private Pais? _pais;
        private Estado? _estado;
        private Cidade? _cidade;

        private bool _controlesHabilitados = false;
        private bool _listaHabilitada = true;
        private bool _progressoEhIndeterminavel = true;
        private bool _progressoVisivel = false;
        private double _valorProgresso = 0;
        private string _textoProgresso;
        private string _mensagemStatus;
        private string _formatoTelefone;

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

        private ObservableCollection<Contato> _listaContatos = new();
        private ObservableCollection<Cliente> _listaClientes = new();
        private ObservableCollection<Status> _listaStatus = new();
        private ObservableCollection<Pais> _listaPaises = new();
        private ObservableCollection<Estado> _listaEstados = new();
        private ObservableCollection<Cidade> _listaCidades = new();

        private ICommand _comandoSalvar;
        private ICommand _comandoAdicionar;
        private ICommand _comandoEditar;
        private ICommand _comandoDeletar;
        private ICommand _comandoCancelar;

        #endregion Campos

        #region Construtores

        public ParametroContatosViewModel(IDialogCoordinator dialogCoordinator)
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
                return "Contatos";
            }
        }

        public string Icone
        {
            get
            {
                return "CardAccountPhone";
            }
        }

        public void LimparViewModel()
        {
            try
            {
                _contatoSelecionado = null;
                _cts = null;
                _listaContatos = null;
                _listaClientes = null;
                _listaStatus = null;
                _listaPaises = null;
                _listaEstados = null;
                _listaCidades = null;
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

        public Contato ContatoSelecionado
        {
            get { return _contatoSelecionado; }
            set
            {
                _contatoSelecionado = value;
                OnPropertyChanged(nameof(ContatoSelecionado));
                if (ContatoSelecionado == null)
                {
                    ContatoAlterar = null;
                }
                else
                {
                    ContatoAlterar = (Contato)ContatoSelecionado.Clone();
                    PermiteEditar = ContatoAlterar != null;
                    PermiteDeletar = ContatoAlterar != null;

                    if (ContatoAlterar.Telefone != null)
                    {
                        FormatoTelefone = ContatoAlterar.Telefone.Length > 10 ? @"\(00\)\ 00000\-0000" : @"\(00\)\ 0000\-0000";
                    }

                    // Define valores
                    try
                    {
                        ContatoAlterar.Status = ListaStatus.First(stat => stat.Id == ContatoSelecionado.Status.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        ContatoAlterar.Cliente = ListaClientes.First(clie => clie.Id == ContatoSelecionado.Cliente.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        Pais = ListaPaises.First(pais => pais.Id == ContatoSelecionado.Cidade.Estado.Pais.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        Estado = ListaEstados.First(esta => esta.Id == ContatoSelecionado.Cidade.Estado.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        Cidade = ListaCidades.First(cida => cida.Id == ContatoSelecionado.Cidade.Id);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public Contato ContatoAlterar
        {
            get { return _contatoAlterar; }
            set
            {
                _contatoAlterar = value;
                OnPropertyChanged(nameof(ContatoAlterar));
                PermiteEditar = ContatoAlterar != null && ContatoAlterar?.Id != null;
                PermiteDeletar = ContatoAlterar != null && ContatoAlterar?.Id != null;
            }
        }

        public Pais? Pais
        {
            get { return _pais; }
            set
            {
                if (value != _pais)
                {
                    _pais = value;
                    OnPropertyChanged(nameof(Pais));
                    ListaEstadosView.Filter = FiltraEstados;
                    CollectionViewSource.GetDefaultView(ListaEstados).Refresh();
                }
            }
        }

        public Estado? Estado
        {
            get { return _estado; }
            set
            {
                if (value != _estado)
                {
                    _estado = value;
                    OnPropertyChanged(nameof(Estado));
                    ListaCidadesView.Filter = FiltraCidades;
                    CollectionViewSource.GetDefaultView(ListaCidades).Refresh();
                }
            }
        }

        public Cidade? Cidade
        {
            get { return _cidade; }
            set
            {
                if (value != _cidade)
                {
                    _cidade = value;
                    OnPropertyChanged(nameof(Cidade));
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

        public string FormatoTelefone
        {
            get { return _formatoTelefone; }
            set
            {
                _formatoTelefone = value;
                OnPropertyChanged(nameof(FormatoTelefone));
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

        public ObservableCollection<Contato> ListaContatos
        {
            get { return _listaContatos; }
            set
            {
                if (value != _listaContatos)
                {
                    _listaContatos = value;
                    OnPropertyChanged(nameof(ListaContatos));
                }
            }
        }

        public ObservableCollection<Cliente> ListaClientes
        {
            get { return _listaClientes; }
            set
            {
                if (value != _listaClientes)
                {
                    _listaClientes = value;
                    OnPropertyChanged(nameof(ListaClientes));
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

        public ObservableCollection<Pais> ListaPaises
        {
            get { return _listaPaises; }
            set
            {
                if (value != _listaPaises)
                {
                    _listaPaises = value;
                    OnPropertyChanged(nameof(ListaPaises));
                }
            }
        }

        public ObservableCollection<Estado> ListaEstados
        {
            get { return _listaEstados; }
            set
            {
                if (value != _listaEstados)
                {
                    _listaEstados = value;
                    OnPropertyChanged(nameof(ListaEstados));
                }
            }
        }

        public ObservableCollection<Cidade> ListaCidades
        {
            get { return _listaCidades; }
            set
            {
                if (value != _listaCidades)
                {
                    _listaCidades = value;
                    OnPropertyChanged(nameof(ListaCidades));
                }
            }
        }

        private CollectionView _listaEstadosView;
        private CollectionView _listaCidadesView;

        public CollectionView ListaEstadosView
        {
            get { return _listaEstadosView; }
            set
            {
                if (_listaEstadosView != value)
                {
                    _listaEstadosView = value;
                    OnPropertyChanged(nameof(ListaEstadosView));
                }
            }
        }

        public CollectionView ListaCidadesView
        {
            get { return _listaCidadesView; }
            set
            {
                if (_listaCidadesView != value)
                {
                    _listaCidadesView = value;
                    OnPropertyChanged(nameof(ListaCidadesView));
                }
            }
        }

        //public CollectionView ListaEstadosView { get; private set; }
        //public CollectionView ListaCidadesView { get; private set; }

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
                await Contato.PreencheListaContatosAsync(ListaContatos, true, null, CancellationToken.None, "ORDER BY cont.nome ASC", "");
                await Cliente.PreencheListaClientesAsync(ListaClientes, true, null, CancellationToken.None, "ORDER BY clie.nome ASC", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");

                await Pais.PreencheListaPaisesAsync(ListaPaises, true, null, CancellationToken.None, "", "");
                await Estado.PreencheListaEstadosAsync(ListaEstados, true, null, CancellationToken.None, "", "");
                await Cidade.PreencheListaCidadesAsync(ListaCidades, true, null, CancellationToken.None, "", "");

                ListaEstadosView = GetEstadoCollectionView(ListaEstados);
                ListaCidadesView = GetCidadeCollectionView(ListaCidades);

                ListaEstadosView.Filter = LimpaListaEstados;
                CollectionViewSource.GetDefaultView(ListaEstados).Refresh();

                ListaCidadesView.Filter = LimpaListaCidades;
                CollectionViewSource.GetDefaultView(ListaCidades).Refresh();

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
            ContatoAlterar = new Contato();
            ContatoAlterar.Cliente = null;
            ContatoAlterar.Status = null;
            Pais = null;
            Estado = null;
            Cidade = null;

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
            PermiteEditar = ContatoAlterar != null && ContatoAlterar?.Id != null; ;
            PermiteCancelar = false;
            PermiteSalvar = false;
            PermiteDeletar = ContatoAlterar != null && ContatoAlterar?.Id != null; ;
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
            MensagemStatus = "Salvando dados do contato '" + ContatoAlterar.Nome + "', aguarde...";
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

            ContatoAlterar.Cidade = Cidade;

            try
            {
                await ContatoAlterar.SalvarContatoDatabaseAsync(_cts.Token);
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

            int? idContatoteAlterado = ContatoAlterar.Id;

            try
            {
                await Contato.PreencheListaContatosAsync(ListaContatos, true, null, CancellationToken.None, "ORDER BY cont.nome ASC", "");
            }
            catch (Exception)
            {
            }

            try
            {
                ContatoSelecionado = ListaContatos.First(cont => cont.Id == idContatoteAlterado);
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
                "Atenção", "Tem certeza que deseja excluir o contato '" + ContatoAlterar.Nome + "'? " +
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
            MensagemStatus = "Deletando contato '" + ContatoAlterar.Nome + "', aguarde...";
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
                await ContatoAlterar.DeletarContatoDatabaseAsync(_cts.Token);
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

            ContatoSelecionado = null;
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
            MensagemStatus = "Contato excluído com sucesso!";

            try
            {
                await Contato.PreencheListaContatosAsync(ListaContatos, true, null, CancellationToken.None, "ORDER BY cont.nome ASC", "");
            }
            catch (Exception)
            {
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
        }

        public bool FiltraEstados(object item)
        {
            if (Pais == null)
            {
                return false;
            }
            if (item is Estado estado)
            {
                return estado.Pais.Id == Pais.Id;
            }
            return true;
        }

        public bool FiltraCidades(object item)
        {
            if (Estado == null)
            {
                return false;
            }
            if (item is Cidade cidade)
            {
                return cidade.Estado.Id == Estado.Id;
            }
            return true;
        }

        public bool LimpaListaEstados(object item)
        {
            return false;
        }

        public bool LimpaListaCidades(object item)
        {
            return false;
        }

        #endregion Métodos

        #region Classes

        public CollectionView GetEstadoCollectionView(ObservableCollection<Estado> estadoList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(estadoList);
        }

        public CollectionView GetCidadeCollectionView(ObservableCollection<Cidade> cidadeList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(cidadeList);
        }

        #endregion Classes
    }
}