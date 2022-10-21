using ControlzEx.Theming;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class EntrarViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private bool _lembrarUsuario = true;
        private string _loginOuEmail;
        private string _mensagemStatus;
        private double _valorProgresso;
        private int _paginaAtual;
        private bool _progressoEhIndeterminavel;
        private bool _controleHabilitado = true;
        private bool _cancelarVisivel;

        private CancellationTokenSource _cts;
        //private readonly IProgress<double> _progresso;

        private ICommand _comandoEntrar;
        private ICommand _comandoEsqueciASenha;
        private ICommand _comandoCancelar;

        #endregion Campos

        #region Construtores

        public EntrarViewModel()
        {
            LoginOuEmail = App.ConfiguracoesGerais.LoginUltimoUsuarioLogado == null ? "" : App.ConfiguracoesGerais.LoginUltimoUsuarioLogado;
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Entrar";
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
                _comandoEntrar = null;
                _comandoEsqueciASenha = null;
                _comandoCancelar = null;
            }
            catch (Exception)
            {
            }
        }

        public string Senha { private get; set; }

        public bool LembrarUsuario
        {
            get { return _lembrarUsuario; }
            set
            {
                if (_lembrarUsuario != value)
                {
                    _lembrarUsuario = value;
                    OnPropertyChanged(nameof(LembrarUsuario));
                }
            }
        }

        public string LoginOuEmail
        {
            get { return _loginOuEmail; }
            set
            {
                if (_loginOuEmail != value)
                {
                    _loginOuEmail = value;
                    OnPropertyChanged(nameof(LoginOuEmail));
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
                    Messenger.Default.Send<string>(MensagemStatus, "MensagemStatus");
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
                    OnPropertyChanged(nameof(ValorProgresso));
                    Messenger.Default.Send<double>(ValorProgresso, "ValorProgresso");
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
                    Messenger.Default.Send<bool>(ProgressoEhIndeterminavel, "ProgressoEhIndeterminavel");
                }
            }
        }

        public bool ControleHabilitado
        {
            get { return _controleHabilitado; }
            set
            {
                if (value != _controleHabilitado)
                {
                    _controleHabilitado = value;
                    OnPropertyChanged(nameof(ControleHabilitado));
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

        public ICommand ComandoEntrar
        {
            get
            {
                if (_comandoEntrar == null)
                {
                    _comandoEntrar = new RelayCommand(
                        param => EfetuarLogin().Await(),
                        param => true
                    );
                }
                return _comandoEntrar;
            }
        }

        public ICommand ComandoEsqueciASenha
        {
            get
            {
                if (_comandoEsqueciASenha == null)
                {
                    _comandoEsqueciASenha = new RelayCommand(
                        param => NavegaEsqueciASenha(),
                        param => true
                    );
                }
                return _comandoEsqueciASenha;
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

        private void NavegaEsqueciASenha()
        {
            Messenger.Default.Send<int>(1, "PaginaAtualLogin");
        }

        private async Task EfetuarLogin()
        {
            // Tenta efetuar o login e retorna mensagem de erro caso não consiga
            try
            {
                // Inicializa o token de cancelamento
                _cts = new();

                // Desabilita todos os controles
                ControleHabilitado = false;

                // Torna o cancelar visível
                CancelarVisivel = true;

                // Determia o progresso como indeterminável
                ProgressoEhIndeterminavel = true;

                // Altera a mensagem e lança um delay para que ela tenha tempo de ser exibida
                MensagemStatus = "Obtendo dados do usuário. Aguarde...";
                await Task.Delay(500, _cts.Token);

                // Instancia o usuário
                Usuario usuario = new();

                // Tenta efetuar o login
                await usuario.EfetuarLoginUsuarioDatabaseAsync(LoginOuEmail, Senha, _cts.Token);

                // Verifica se a filial está ativa e, caso contrário, lança uma exceção
                if (usuario.Filial.Status.Id != 1)
                {
                    throw new ValorInativoException("Filial inativa. Contate o administrador");
                }

                // Verifica se a empresa está ativa e, caso contrário, lança uma exceção
                if (usuario.Filial.Empresa.Status.Id != 1)
                {
                    throw new ValorInativoException("Empresa inativa. Contate o administrador");
                }

                // Determina a data da última alteração da senha
                TimeSpan tempoUltimaAlteracaoSenha = DateTime.Now - (usuario.DataUltimaAlteracaoSenha == null ? DateTime.MinValue : (DateTime)usuario.DataUltimaAlteracaoSenha);

                // Verifica se a última alteração foi feita há mais de 90 dias
                if (tempoUltimaAlteracaoSenha > TimeSpan.FromDays(90))
                {
                    MensagemStatus = "Sua senha expirou. Por favor, defina uma nova senha";
                    Messenger.Default.Send<Usuario>(usuario, "UsuarioAlteraSenha");
                    Messenger.Default.Send<int>(0, "PaginaOrigem");
                    Messenger.Default.Send<int>(2, "PaginaAtualLogin");

                    // Habilita todos os controles
                    ControleHabilitado = true;

                    // Determia o progresso como Determinável
                    ProgressoEhIndeterminavel = false;

                    return;
                    //throw new ValorInativoException("Sua senha expirou. Por favor, defina uma nova senha");
                }

                // Se o login for após o uso, altera todos os registros em uso do usuário anterior para o atual
                if (App.LoginAposUso)
                {
                    await Usuario.AlteraIdUsuarioEmUsoAsync(CancellationToken.None, App.Usuario.Id, usuario.Id);
                }

                // Define o usuário da aplicação como o usuário que efetuou o login
                App.Usuario = usuario;

                // Inicia as configurações e define o id do último usuário logado
                App.ConfiguracoesGerais.LoginUltimoUsuarioLogado = LembrarUsuario == true ? Convert.ToString(App.Usuario.Login) : "";
                App.ConfiguracoesGerais.SalvarNoRegistro();

                // Limpa todas os registros que estavam em uso por esse usuário se o login não for após o uso
                if (!App.LoginAposUso)
                {
                    await App.Usuario.LimpaIdUsuarioEmUsoAsync(CancellationToken.None);
                }

                // Define os dados da sessão atual do usuário
                App.Usuario.DataSessao = DateTime.Now;
                App.Usuario.NomeComputadorSessao = Environment.MachineName;
                App.Usuario.UsuarioSessao = Environment.UserName;

                // Altera o tema de acordo com o usuário
                try
                {
                    if (!String.IsNullOrEmpty(usuario.Tema) && !String.IsNullOrEmpty(usuario.Cor))
                    {
                        ThemeManager.Current.ChangeTheme(App.Current, usuario.Tema + "." + usuario.Cor);
                    }
                }
                catch (Exception)
                {
                }

                // Atualiza os dados da sessão atual no servidor
                await App.Usuario.DefineSessaoUsuarioAsync(CancellationToken.None);

                // Altera a mensagem e lança um delay para que ela tenha tempo de ser exibida
                MensagemStatus = "Login efetuado com sucesso. Bem vindo(a), " + App.Usuario.Nome + "!";
                await Task.Delay(500);

                // Cria uma instância de empresa
                Empresa empresa = new();

                // Retorna os dados da empresa da database através da empresa do usuário logado
                await empresa.GetEmpresaDatabaseAsync(App.Usuario.Filial.Empresa.Id, CancellationToken.None);

                // Tenta salvar os arquivos de imagens da empresa na pasta temporária do sistema
                try
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetTempPath() + "Proreports\\SGT\\Imagens");

                    if (empresa.Logo1 != null)
                    {
                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(empresa.Logo1)))
                        {
                            image.Save(System.IO.Path.GetTempPath() + "Proreports\\SGT\\Imagens\\logo_1.png", System.Drawing.Imaging.ImageFormat.Png);  // Or Png
                        }
                    }
                    if (empresa.Logo2 != null)
                    {
                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(empresa.Logo2)))
                        {
                            image.Save(System.IO.Path.GetTempPath() + "Proreports\\SGT\\Imagens\\logo_2.png", System.Drawing.Imaging.ImageFormat.Png);  // Or Png
                        }
                    }
                    if (empresa.Logo3 != null)
                    {
                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(empresa.Logo3)))
                        {
                            image.Save(System.IO.Path.GetTempPath() + "Proreports\\SGT\\Imagens\\logo_3.png", System.Drawing.Imaging.ImageFormat.Png);  // Or Png
                        }
                    }
                }
                catch (Exception)
                {
                }

                Serilog.Context.GlobalLogContext.PushProperty("EmpresaCnpj", App.Usuario?.Filial?.Empresa?.CNPJ);
                Serilog.Context.GlobalLogContext.PushProperty("UsuarioLogado", App.Usuario?.Login);

                // Limpa a mensagem
                MensagemStatus = "";

                // Habilita todos os controles
                ControleHabilitado = true;

                // Determia o progresso como Determinável
                ProgressoEhIndeterminavel = false;

                if (App.LoginAposUso)
                {
                    Messenger.Default.Send<bool>(true, "SessaoRenovada");
                    Messenger.Default.Send<bool>(true, "UsuarioAlterado");
                }
                else
                {
                    // Cria uma nova instância da página principal e envia ao mensageiro
                    Messenger.Default.Send<IPageViewModel>(new PrincipalViewModel(DialogCoordinator.Instance), "PaginaAdicionar");

                    // Envia a página atual
                    Messenger.Default.Send<int>(1, "PaginaAtualGeral");

                    // Define a largura e altura da janela principal
                    Messenger.Default.Send<double?>(720, "AlturaJanela");
                    Messenger.Default.Send<double?>(1280, "LarguraJanela");
                }
            }
            catch (OperationCanceledException)
            {
                // Define a mensagem de status com o retorno da exceção
                MensagemStatus = "Login cancelado";

                // Habilita todos os controles
                ControleHabilitado = true;

                // Oculta o cancelar
                CancelarVisivel = false;

                // Determia o progresso como Determinável
                ProgressoEhIndeterminavel = false;
            }
            catch (ValorNaoExisteException ex)
            {
                // Define a mensagem de status com o retorno da exceção
                MensagemStatus = ex.Message;

                // Habilita todos os controles
                ControleHabilitado = true;

                // Oculta o cancelar
                CancelarVisivel = false;

                // Determia o progresso como Determinável
                ProgressoEhIndeterminavel = false;
            }
            catch (ValorIncorretoException ex)
            {
                // Define a mensagem de status com o retorno da exceção
                MensagemStatus = ex.Message;

                // Habilita todos os controles
                ControleHabilitado = true;

                // Oculta o cancelar
                CancelarVisivel = false;

                // Determia o progresso como Determinável
                ProgressoEhIndeterminavel = false;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao realizar o login");

                // Define a mensagem de status com o retorno da exceção
                MensagemStatus = ex.Message;

                // Habilita todos os controles
                ControleHabilitado = true;

                // Oculta o cancelar
                CancelarVisivel = false;

                // Determia o progresso como Determinável
                ProgressoEhIndeterminavel = false;
            }
        }

        private void Cancelar()
        {
            if (_cts != null)
                _cts.Cancel();
        }

        #endregion Métodos
    }
}