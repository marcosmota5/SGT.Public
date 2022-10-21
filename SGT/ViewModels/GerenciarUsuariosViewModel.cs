using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using SGT.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class GerenciarUsuariosViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private Usuario _usuarioSelecionado = new();
        private Usuario _usuarioAlterar = new();
        private bool _sexoEhMasculino;
        private bool _sexoEhFeminino;
        private string _formatoTelefone;
        private CancellationTokenSource _cts;

        private bool _controlesHabilitados;
        private bool _listaHabilitada = true;
        private bool _progressoEhIndeterminavel = true;
        private bool _progressoVisivel = false;
        private double _valorProgresso = 0;
        private string _textoProgresso;
        private string _mensagemStatus;

        private bool _deletarVisivel;
        private bool _cancelarVisivel;

        private bool _permiteSalvar;
        private bool _permiteEditar;
        private bool _permiteDeletar;
        private bool _permiteCancelar;

        private bool _carregamentoVisivel = true;

        private readonly IDialogCoordinator _dialogCoordinator;

        private ObservableCollection<Usuario> _listaUsuarios = new();
        private ObservableCollection<Filial> _listaFiliais = new();
        private ObservableCollection<Setor> _listaSetores = new();
        private ObservableCollection<Perfil> _listaPerfis = new();
        private ObservableCollection<Status> _listaStatus = new();

        private ICommand _comandoSalvar;
        private ICommand _comandoEditar;
        private ICommand _comandoDeletar;
        private ICommand _comandoCancelar;
        private ICommand _comandoDeslogarUsuario;
        private ICommand _comandoResetarSenha;

        #endregion Campos

        #region Construtores

        public GerenciarUsuariosViewModel(IDialogCoordinator dialogCoordinator)
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
                return "Gerenciar usuários";
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
                _cts = null;

                _listaUsuarios = null;
                _listaFiliais = null;
                _listaSetores = null;
                _listaPerfis = null;
                _listaStatus = null;

                _comandoSalvar = null;
                _comandoEditar = null;
                _comandoDeletar = null;
                _comandoCancelar = null;
                _comandoDeslogarUsuario = null;
                _comandoResetarSenha = null;
            }
            catch (Exception)
            {
            }
        }

        public bool ExistemCamposVazios { private get; set; }

        public Usuario UsuarioSelecionado
        {
            get { return _usuarioSelecionado; }
            set
            {
                _usuarioSelecionado = value;
                OnPropertyChanged(nameof(UsuarioSelecionado));
                if (UsuarioSelecionado == null)
                {
                    UsuarioAlterar = null;
                }
                else
                {
                    UsuarioAlterar = (Usuario)UsuarioSelecionado.Clone();
                    SexoEhMasculino = UsuarioAlterar.Sexo == "M";
                    SexoEhFeminino = UsuarioAlterar.Sexo == "F";
                    PermiteEditar = UsuarioAlterar != null;
                    PermiteDeletar = UsuarioAlterar != null;

                    if (UsuarioAlterar.Telefone != null)
                    {
                        FormatoTelefone = UsuarioAlterar.Telefone.Length > 10 ? @"\(00\)\ 00000\-0000" : @"\(00\)\ 0000\-0000";
                    }

                    // Define valores
                    try
                    {
                        UsuarioAlterar.Filial = ListaFiliais.First(fili => fili.Id == UsuarioSelecionado.Filial.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        UsuarioAlterar.Setor = ListaSetores.First(seto => seto.Id == UsuarioSelecionado.Setor.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        UsuarioAlterar.Perfil = ListaPerfis.First(perf => perf.Id == UsuarioSelecionado.Perfil.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        UsuarioAlterar.Status = ListaStatus.First(stat => stat.Id == UsuarioSelecionado.Status.Id);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public Usuario UsuarioAlterar
        {
            get { return _usuarioAlterar; }
            set
            {
                _usuarioAlterar = value;
                OnPropertyChanged(nameof(UsuarioAlterar));
            }
        }

        public bool SexoEhMasculino
        {
            get { return _sexoEhMasculino; }
            set
            {
                _sexoEhMasculino = value;
                OnPropertyChanged(nameof(SexoEhMasculino));
                UsuarioAlterar.Sexo = SexoEhMasculino ? "M" : "F";
            }
        }

        public bool SexoEhFeminino
        {
            get { return _sexoEhFeminino; }
            set
            {
                _sexoEhFeminino = value;
                OnPropertyChanged(nameof(SexoEhFeminino));
                UsuarioAlterar.Sexo = SexoEhFeminino ? "F" : "M";
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

        public ObservableCollection<Usuario> ListaUsuarios
        {
            get { return _listaUsuarios; }
            set
            {
                if (value != _listaUsuarios)
                {
                    _listaUsuarios = value;
                    OnPropertyChanged(nameof(ListaUsuarios));
                }
            }
        }

        public ObservableCollection<Filial> ListaFiliais
        {
            get { return _listaFiliais; }
            set
            {
                if (value != _listaFiliais)
                {
                    _listaFiliais = value;
                    OnPropertyChanged(nameof(ListaFiliais));
                }
            }
        }

        public ObservableCollection<Setor> ListaSetores
        {
            get { return _listaSetores; }
            set
            {
                if (value != _listaSetores)
                {
                    _listaSetores = value;
                    OnPropertyChanged(nameof(ListaSetores));
                }
            }
        }

        public ObservableCollection<Perfil> ListaPerfis
        {
            get { return _listaPerfis; }
            set
            {
                if (value != _listaPerfis)
                {
                    _listaPerfis = value;
                    OnPropertyChanged(nameof(ListaSetores));
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

        public ICommand ComandoDeslogarUsuario
        {
            get
            {
                if (_comandoDeslogarUsuario == null)
                {
                    _comandoDeslogarUsuario = new RelayCommand(
                        param => DeslogarUsuario().Await(),
                        param => true
                    );
                }
                return _comandoDeslogarUsuario;
            }
        }

        public ICommand ComandoResetarSenha
        {
            get
            {
                if (_comandoResetarSenha == null)
                {
                    _comandoResetarSenha = new RelayCommand(
                        param => ResetarSenha().Await(),
                        param => true
                    );
                }
                return _comandoResetarSenha;
            }
        }

        #endregion Propriedades/Comandos

        #region Métodos

        private async Task ConstrutorAsync()
        {
            try
            {
                // Preenche as listas com as classes necessárias
                await Usuario.PreencheListaUsuariosAsync(ListaUsuarios, true, null, CancellationToken.None, "WHERE usua.id_usuario <> 1 AND usua.id_usuario <> @id_usuario_atual ORDER BY usua.nome ASC", "@id_usuario_atual", App.Usuario.Id);
                await Filial.PreencheListaFiliaisAsync(ListaFiliais, true, null, CancellationToken.None, "ORDER BY fili.nome ASC", "");
                await Setor.PreencheListaSetoresAsync(ListaSetores, true, null, CancellationToken.None, "ORDER BY seto.nome ASC", "");
                await Perfil.PreencheListaPerfisAsync(ListaPerfis, true, null, CancellationToken.None, "", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");

                ControlesHabilitados = false;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");

                MensagemStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                ControlesHabilitados = false;
            }

            CarregamentoVisivel = false;
        }

        private void Editar()
        {
            ControlesHabilitados = true;
            ListaHabilitada = false;
            CancelarVisivel = true;
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
            CancelarVisivel = false;
            PermiteEditar = true;
            PermiteCancelar = false;
            PermiteSalvar = false;
            PermiteDeletar = true;
        }

        private async Task SalvarAsync()
        {
            if (ExistemCamposVazios)
            {
                MensagemStatus = "Campos obrigatórios vazios/inválidos";
                return;
            }

            Instancia instancia = new();
            InstanciaLocal instanciaLocal = new();
            int quantidadeUsuariosAtivos = 0;

            try
            {
                quantidadeUsuariosAtivos = await Usuario.GetQuantidadeUsuariosAtivos(CancellationToken.None);
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro na verificação de usuários");

                MensagemStatus = "Falha na verificação de usuários. Se o problema persistir, contate o desenvolvedor e envie o log";
                return;
            }

            try
            {
                await instanciaLocal.GetInstanciaLocal(CancellationToken.None);
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro na verificação de instância");

                MensagemStatus = "Falha na verificação de instância local. Se o problema persistir, contate o desenvolvedor e envie o log";
                return;
            }

            try
            {
                await instancia.GetInstanciaDatabaseAsync(instanciaLocal.CodigoInstancia, CancellationToken.None);

                try
                {
                    await InstanciaLocal.AtualizaDataInstancia(CancellationToken.None);
                }
                catch (Exception)
                {
                }

                if (instancia.Id == null)
                {
                    MensagemStatus = "Instância de login inválida. Contate o desenvolvedor";
                    return;
                }
                else
                {
                    if (instancia.DataFim != null)
                    {
                        if (DateTime.Now > (DateTime)instancia.DataFim)
                        {
                            MensagemStatus = "Instância expirada. Contate o desenvolvedor";
                            return;
                        }
                        else
                        {
                            if (instancia.QuantidadeMaximaUsuariosAtivos != null)
                            {
                                if (quantidadeUsuariosAtivos >= (int)instancia.QuantidadeMaximaUsuariosAtivos)
                                {
                                    MensagemStatus = "Quantidade máxima de usuários (" + instancia.QuantidadeMaximaUsuariosAtivos.ToString() + ") atingida. Contate o desenvolvedor";
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (instancia.QuantidadeMaximaUsuariosAtivos != null)
                        {
                            if (quantidadeUsuariosAtivos >= (int)instancia.QuantidadeMaximaUsuariosAtivos)
                            {
                                MensagemStatus = "Quantidade máxima de usuários (" + instancia.QuantidadeMaximaUsuariosAtivos.ToString() + ") atingida. Contate o desenvolvedor";
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DateTime dataAtualizacao = instanciaLocal.DataAtualizacao == null ? DateTime.Now : (DateTime)instanciaLocal.DataAtualizacao;
                TimeSpan diasVerificacao = DateTime.Now - dataAtualizacao;

                if (diasVerificacao.Days > 15)
                {
                    // Escreve no log a exceção e uma mensagem de erro
                    Serilog.Log.Error(ex, "Erro na autenticação de instância (limite de 15 dias ultrapassado)");

                    MensagemStatus = "Falha na autenticação de instância (limite de 15 dias ultrapassado). Contate o desenvolvedor";
                    return;
                }
            }

            _cts = new();

            ValorProgresso = 0;
            ListaHabilitada = false;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = true;
            MensagemStatus = "Salvando dados do usuário '" + UsuarioAlterar.Login + "', aguarde...";
            CancelarVisivel = true;
            PermiteCancelar = true;
            PermiteSalvar = false;
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
                return;
            }

            try
            {
                await UsuarioAlterar.SalvarUsuarioDatabaseAsync(CancellationToken.None);
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
                PermiteEditar = true;
                PermiteDeletar = true;

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

                return;
            }

            int? idUsuarioAlterado = UsuarioAlterar.Id;

            try
            {
                await Usuario.PreencheListaUsuariosAsync(ListaUsuarios, true, null, CancellationToken.None, "WHERE usua.id_usuario <> 1 AND usua.id_usuario <> @id_usuario_atual", "@id_usuario_atual", App.Usuario.Id);
            }
            catch (Exception)
            {
            }

            try
            {
                UsuarioSelecionado = ListaUsuarios.First(usua => usua.Id == idUsuarioAlterado);
            }
            catch (Exception)
            {
            }

            ValorProgresso = 0;
            ListaHabilitada = true;
            ControlesHabilitados = false;
            ProgressoVisivel = false;
            ProgressoEhIndeterminavel = false;
            CancelarVisivel = false;
            PermiteCancelar = false;
            PermiteSalvar = false;
            PermiteEditar = true;
            PermiteDeletar = true;
            MensagemStatus = "Dados salvos com sucesso!";
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
                "Atenção", "Tem certeza que deseja excluir o usuário '" + UsuarioAlterar.Login + "'? " +
                "O processo não poderá ser desfeito.",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem != MessageDialogResult.Affirmative)
            {
                return;
            }

            _cts = new();

            ValorProgresso = 0;
            ListaHabilitada = false;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = true;
            MensagemStatus = "Deletando usuário '" + UsuarioAlterar.Login + "', aguarde...";
            CancelarVisivel = true;
            PermiteCancelar = true;
            PermiteSalvar = false;
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
                return;
            }

            try
            {
                await UsuarioAlterar.DeletarUsuarioDatabaseAsync(CancellationToken.None);
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

                return;
            }

            UsuarioSelecionado = null;
            ValorProgresso = 0;
            ListaHabilitada = true;
            ControlesHabilitados = false;
            ProgressoVisivel = false;
            ProgressoEhIndeterminavel = false;
            CancelarVisivel = false;
            PermiteCancelar = false;
            PermiteSalvar = false;
            PermiteEditar = false;
            PermiteDeletar = false;
            MensagemStatus = "Usuário excluído com sucesso!";

            try
            {
                await Usuario.PreencheListaUsuariosAsync(ListaUsuarios, true, null, CancellationToken.None, "WHERE usua.id_usuario <> 1 AND usua.id_usuario <> @id_usuario_atual", "@id_usuario_atual", App.Usuario.Id);
            }
            catch (Exception)
            {
            }
        }

        private async Task DeslogarUsuario()
        {
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Sim",
                NegativeButtonText = "Não"
            };

            var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                "Atenção", "Tem certeza que deseja deslogar o usuário '" + UsuarioAlterar.Login + "'? " +
                "O processo não poderá ser desfeito.",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem != MessageDialogResult.Affirmative)
            {
                return;
            }

            try
            {
                await UsuarioAlterar.LimpaIdUsuarioEmUsoAsync(CancellationToken.None);

                // Define os dados da sessão atual do usuário
                UsuarioAlterar.DataSessao = null;
                UsuarioAlterar.NomeComputadorSessao = null;
                UsuarioAlterar.UsuarioSessao = null;

                // Atualiza os dados da sessão atual no servidor
                await UsuarioAlterar.DefineSessaoUsuarioAsync(CancellationToken.None);

                var mensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Procedimento concluído", "Usuário desconectado com sucesso", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "Ok" });
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao deslogar usuário");
                MensagemStatus = "Erro ao deslogar o usuário. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
            }
        }

        private async Task ResetarSenha()
        {
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Sim",
                NegativeButtonText = "Não"
            };

            var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                "Atenção", "Tem certeza que deseja resetar a senha do usuário '" + UsuarioAlterar.Login + "'? " +
                "O processo não poderá ser desfeito.",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem != MessageDialogResult.Affirmative)
            {
                return;
            }

            try
            {
                string novaSenha = Usuario.GeraSenhaAleatoria();

                await Usuario.AlterarSenhaUsuarioDatabaseAsync(UsuarioAlterar.Id, novaSenha, CancellationToken.None, DateTime.MinValue);

                mySettings.AffirmativeButtonText = "OK";

                var customDialog2 = new CustomDialog();

                var dataContext2 = new MensagemComTextBoxViewModel("Senha resetada", "A senha do usuário " + UsuarioAlterar.Login + " foi resetada com sucesso. Informe a senha abaixo ao usuário, a qual deverá ser alterada no primeiro login", "Nova senha (temporária)", novaSenha, instance =>
                {
                    _dialogCoordinator.HideMetroDialogAsync(this, customDialog2);
                });

                customDialog2.Content = new MensagemComTextBoxView { DataContext = dataContext2 };

                await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog2);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao resetar senha");
                MensagemStatus = "Erro ao resetar senha. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
            }
        }

        #endregion Métodos
    }
}