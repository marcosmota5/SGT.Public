using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class AlterarSenhaSemAtualViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private string _mensagemStatus;
        private double _valorProgresso;
        private bool _progressoEhIndeterminavel;
        private bool _controleHabilitado = true;
        private bool _tamanhoNovaSenhaVisivel;
        private bool _maiusculasNovaSenhaVisivel;
        private bool _minusculasNovaSenhaVisivel;
        private bool _numerosNovaSenhaVisivel;
        private bool _caractereEspecialNovaSenhaVisivel;
        private string _novaSenha;
        private ICommand _comandoConfirmarAlteracao;
        private ICommand _comandoVoltar;
        private Usuario _usuarioAlterarSenha;
        private int _paginaOrigem;

        #endregion Campos

        public AlterarSenhaSemAtualViewModel()
        {
            Messenger.Default.Register<int>(this, "PaginaOrigem", delegate (int mensagemPaginaOrigem) { _paginaOrigem = mensagemPaginaOrigem; });
            Messenger.Default.Register<Usuario>(this, "UsuarioAlteraSenha", delegate (Usuario mensagemUsuarioRecebida) { UsuarioAlterarSenha = mensagemUsuarioRecebida; });
        }

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Alterar senha";
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
                _comandoConfirmarAlteracao = null;
                _comandoVoltar = null;
            }
            catch (Exception)
            {
            }
        }

        //public string NovaSenha { private get; set; }

        public bool ExistemCamposVazios { private get; set; }

        public string NovaSenha
        {
            get { return _novaSenha; }
            set
            {
                if (value != _novaSenha)
                {
                    _novaSenha = value;
                    TamanhoNovaSenhaVisivel = !FuncoesDeSenha.SenhaPossuiTamanhoMinimo(NovaSenha);
                    MaiusculasNovaSenhaVisivel = !FuncoesDeSenha.SenhaPossuiMaiusculas(NovaSenha);
                    MinusculasNovaSenhaVisivel = !FuncoesDeSenha.SenhaPossuiMinusculas(NovaSenha);
                    NumerosNovaSenhaVisivel = !FuncoesDeSenha.SenhaPossuiNumeros(NovaSenha);
                    CaractereEspecialNovaSenhaVisivel = !FuncoesDeSenha.SenhaPossuiCaraceteresEspeciais(NovaSenha);
                }
            }
        }

        public string ConfirmacaoSenha { private get; set; }

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

        public bool TamanhoNovaSenhaVisivel
        {
            get { return _tamanhoNovaSenhaVisivel; }
            set
            {
                if (value != _tamanhoNovaSenhaVisivel)
                {
                    _tamanhoNovaSenhaVisivel = value;
                    OnPropertyChanged(nameof(TamanhoNovaSenhaVisivel));
                }
            }
        }

        public bool MaiusculasNovaSenhaVisivel
        {
            get { return _maiusculasNovaSenhaVisivel; }
            set
            {
                if (value != _maiusculasNovaSenhaVisivel)
                {
                    _maiusculasNovaSenhaVisivel = value;
                    OnPropertyChanged(nameof(MaiusculasNovaSenhaVisivel));
                }
            }
        }

        public bool MinusculasNovaSenhaVisivel
        {
            get { return _minusculasNovaSenhaVisivel; }
            set
            {
                if (value != _minusculasNovaSenhaVisivel)
                {
                    _minusculasNovaSenhaVisivel = value;
                    OnPropertyChanged(nameof(MinusculasNovaSenhaVisivel));
                }
            }
        }

        public bool NumerosNovaSenhaVisivel
        {
            get { return _numerosNovaSenhaVisivel; }
            set
            {
                if (value != _numerosNovaSenhaVisivel)
                {
                    _numerosNovaSenhaVisivel = value;
                    OnPropertyChanged(nameof(NumerosNovaSenhaVisivel));
                }
            }
        }

        public bool CaractereEspecialNovaSenhaVisivel
        {
            get { return _caractereEspecialNovaSenhaVisivel; }
            set
            {
                if (value != _caractereEspecialNovaSenhaVisivel)
                {
                    _caractereEspecialNovaSenhaVisivel = value;
                    OnPropertyChanged(nameof(CaractereEspecialNovaSenhaVisivel));
                }
            }
        }

        public ICommand ComandoConfirmarAlteracao
        {
            get
            {
                if (_comandoConfirmarAlteracao == null)
                {
                    _comandoConfirmarAlteracao = new RelayCommand(
                        param => ConfirmarAlteracao().Await(),
                        param => true
                    );
                }
                return _comandoConfirmarAlteracao;
            }
        }

        public ICommand ComandoVoltar
        {
            get
            {
                if (_comandoVoltar == null)
                {
                    _comandoVoltar = new RelayCommand(
                        param => Voltar(),
                        param => true
                    );
                }
                return _comandoVoltar;
            }
        }

        public Usuario UsuarioAlterarSenha
        {
            get { return _usuarioAlterarSenha; }
            set
            {
                if (value != _usuarioAlterarSenha)
                {
                    _usuarioAlterarSenha = value;
                    OnPropertyChanged(nameof(UsuarioAlterarSenha));
                }
            }
        }

        #endregion Propriedades/Comandos

        #region Métodos

        private void Voltar()
        {
            Messenger.Default.Send<int>(_paginaOrigem, "PaginaAtualLogin");
        }

        private async Task ConfirmarAlteracao()
        {
            if (ExistemCamposVazios)
            {
                MensagemStatus = "Campos obrigatórios vazios/inválidos";
                return;
            }

            // Tenta alterar a senha
            try
            {
                if (NovaSenha != ConfirmacaoSenha)
                {
                    throw new ValorIncorretoException("A confirmação da senha não confere com a senha digitada");
                }

                if (!FuncoesDeSenha.SenhaEhValida(NovaSenha))
                {
                    throw new ValorIncorretoException("A senha não atende aos requisitos de segurança");
                }

                // Desabilita todos os controles
                ControleHabilitado = false;

                // Determia o progresso como indeterminável
                ProgressoEhIndeterminavel = true;

                // Verifica se a filial está ativa e, caso contrário, lança uma exceção
                if (UsuarioAlterarSenha.Filial.Status.Id != 1)
                {
                    throw new ValorInativoException("Filial inativa. Contate o administrador");
                }

                // Verifica se a empresa está ativa e, caso contrário, lança uma exceção
                if (UsuarioAlterarSenha.Filial.Empresa.Status.Id != 1)
                {
                    throw new ValorInativoException("Empresa inativa. Contate o administrador");
                }

                // Altera a mensagem e lança um delay para que ela tenha tempo de ser exibida
                MensagemStatus = "Alterando senha. Aguarde...";
                await Task.Delay(500);

                // Altera a senha
                await Usuario.AlterarSenhaUsuarioDatabaseAsync(UsuarioAlterarSenha.Id, NovaSenha, CancellationToken.None);

                // Altera a mensagem e lança um delay para que ela tenha tempo de ser exibida
                MensagemStatus = "Senha alterada. Efetuando login...";
                await Task.Delay(500);

                // Se o login for após o uso, altera todos os registros em uso do usuário anterior para o atual
                if (App.LoginAposUso)
                {
                    await Usuario.AlteraIdUsuarioEmUsoAsync(CancellationToken.None, App.Usuario.Id, UsuarioAlterarSenha.Id);
                }

                // Define o usuário da aplicação como o usuário que alterou a senha
                App.Usuario = UsuarioAlterarSenha;

                // Inicia as configurações e define o id do último usuário logado
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
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    MensagemStatus = "Operação cancelada";
                }
                else
                {
                    if (ex is ValorJaExisteException || ex is ValorNaoExisteException || ex is ValorIncorretoException)
                    {
                        MensagemStatus = ex.Message;
                    }
                    else
                    {
                        Serilog.Log.Error(ex, "Erro ao salvar dados");
                        MensagemStatus = "Falha ao salvar os dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                    }
                }

                // Habilita todos os controles
                ControleHabilitado = true;

                // Determia o progresso como Determinável
                ProgressoEhIndeterminavel = false;
            }
        }

        #endregion Métodos
    }
}