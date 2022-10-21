using GalaSoft.MvvmLight.Messaging;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.HelperClasses;
using Model.Email;
using SGT.HelperClasses;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SGT.ViewModels
{
    public class EsqueciASenhaViewModel : ObservableObject, IPageViewModel
    {
        #region Constantes

        private const int TEMPO_ESPERA_SEGUNDOS = 60;

        #endregion Constantes

        #region Campos

        private string _enderecoEmail;
        private string _mensagemStatus;
        private double _valorProgresso;
        private bool _progressoEhIndeterminavel;
        private bool _controleHabilitado = true;
        private bool _tempoEsperaVisivel;
        private bool _codigoRecuperacaoVisivel;
        private string _textoAguarde;
        private string _codigoRecuperacaoGerado;
        private string _codigoRecuperacaoInformado;
        private TimeSpan _time;

        private DispatcherTimer _timer;
        private ICommand _comandoEnviarEmailCodigoRecuperacao;
        private ICommand _comandoConfirmarCodigoRecuperacao;
        private ICommand _comandoVoltar;
        private Usuario usuario;

        #endregion Campos

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Esqueci a senha";
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
                _timer = null;
                _comandoEnviarEmailCodigoRecuperacao = null;
                _comandoConfirmarCodigoRecuperacao = null;
                _comandoVoltar = null;
                usuario = null;
            }
            catch (Exception)
            {
            }
        }

        public string EnderecoEmail
        {
            get { return _enderecoEmail; }
            set
            {
                if (_enderecoEmail != value)
                {
                    _enderecoEmail = value;
                    OnPropertyChanged(nameof(EnderecoEmail));
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

        public ICommand ComandoEnviarEmailCodigoRecuperacao
        {
            get
            {
                if (_comandoEnviarEmailCodigoRecuperacao == null)
                {
                    _comandoEnviarEmailCodigoRecuperacao = new RelayCommand(
                        param => EnviarEmailRecuperacaoAsync().Await(),
                        param => true
                    );
                }
                return _comandoEnviarEmailCodigoRecuperacao;
            }
        }

        public bool TempoEsperaVisivel
        {
            get { return _tempoEsperaVisivel; }
            set
            {
                if (value != _tempoEsperaVisivel)
                {
                    _tempoEsperaVisivel = value;
                    OnPropertyChanged(nameof(TempoEsperaVisivel));
                }
            }
        }

        public bool CodigoRecuperacaoVisivel
        {
            get { return _codigoRecuperacaoVisivel; }
            set
            {
                if (value != _codigoRecuperacaoVisivel)
                {
                    _codigoRecuperacaoVisivel = value;
                    OnPropertyChanged(nameof(CodigoRecuperacaoVisivel));
                }
            }
        }

        public string TextoAguarde
        {
            get { return _textoAguarde; }
            set
            {
                if (value != _textoAguarde)
                {
                    _textoAguarde = value;
                    OnPropertyChanged(nameof(TextoAguarde));
                }
            }
        }

        public string CodigoRecuperacaoInformado
        {
            get { return _codigoRecuperacaoInformado; }
            set
            {
                if (value != _codigoRecuperacaoInformado)
                {
                    _codigoRecuperacaoInformado = value;
                    OnPropertyChanged(nameof(CodigoRecuperacaoInformado));
                }
            }
        }

        public ICommand ComandoConfirmarCodigoRecuperacao
        {
            get
            {
                if (_comandoConfirmarCodigoRecuperacao == null)
                {
                    _comandoConfirmarCodigoRecuperacao = new RelayCommand(
                        param => ConfirmarCodigoRecuperacao().Await(),
                        param => true
                    );
                }
                return _comandoConfirmarCodigoRecuperacao;
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

        #endregion Propriedades/Comandos

        #region Métodos

        private void Voltar()
        {
            Messenger.Default.Send<int>(0, "PaginaAtualLogin");
        }

        private async Task ConfirmarCodigoRecuperacao()
        {
            // Verifica se o código está correto e, caso contrário retorna mensagem
            if (CodigoRecuperacaoInformado == _codigoRecuperacaoGerado)
            {
                // Altera a mensagem e lança um delay para que ela tenha tempo de ser exibida
                MensagemStatus = "Verificando código informado. Aguarde...";
                await Task.Delay(500);

                MensagemStatus = "Código confirmado. Informe a nova senha";
                Messenger.Default.Send<Usuario>(usuario, "UsuarioAlteraSenha");
                Messenger.Default.Send<int>(1, "PaginaOrigem");
                Messenger.Default.Send<int>(2, "PaginaAtualLogin");
            }
            else
            {
                MensagemStatus = "Código inválido";
            }
        }

        private async Task EnviarEmailRecuperacaoAsync()
        {
            // Tenta retornar o usuário e enviar o e-mail
            try
            {
                // Verifica se o e-mail é válido e registra uma mensagem caso não seja
                if (!Email.EmailEhValido(EnderecoEmail))
                {
                    MensagemStatus = "E-mail inválido";
                    return;
                }

                // Desabilita todos os controles
                ControleHabilitado = false;

                // Determia o progresso como indeterminável
                ProgressoEhIndeterminavel = true;

                // Altera a mensagem e lança um delay para que ela tenha tempo de ser exibida
                MensagemStatus = "Enviando e-mail de recuperação. Aguarde...";
                await Task.Delay(500);

                // Instancia o usuário
                usuario = new();

                // Define o usuário através do e-mail
                await usuario.GetUsuarioDatabaseAsync(CancellationToken.None, "WHERE email = @email", "@email", EnderecoEmail);

                // Gera o código de recuperação
                _codigoRecuperacaoGerado = Email.GerarCodigoRecuperacaoSenha();

                // Envia o e-mail para o usuário retornado passando o código de recuperação gerado
                await Email.EnviarEmailRecuperacaoSenha(usuario, _codigoRecuperacaoGerado);

                // Altera a mensagem informando que o código foi enviado
                MensagemStatus = "Código de recuperação enviado";
                ProgressoEhIndeterminavel = false;

                // Torna o campo de visibilidade de tempo de espera visível
                TempoEsperaVisivel = true;

                // Torna o campo de codigo de recuperação visível
                CodigoRecuperacaoVisivel = true;

                // Definição do tempo que deve ser aguardado de acordo com a constante
                _time = TimeSpan.FromSeconds(TEMPO_ESPERA_SEGUNDOS);

                // Definição do timer
                _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                {
                    TextoAguarde = "Aguarde " + _time.ToString(@"mm\:ss") + " para enviar novamente";
                    if (_time == TimeSpan.Zero)
                    {
                        // Habilita todos os controles
                        ControleHabilitado = true;
                        TempoEsperaVisivel = false;
                        _timer.Stop();
                    }
                    _time = _time.Add(TimeSpan.FromSeconds(-1));
                }, Application.Current.Dispatcher);

                // Inicia o tempo
                _timer.Start();
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao enviar e-mail");

                // Define a mensagem de status com o retorno da exceção
                MensagemStatus = "Falha no no envio do e-mail. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";

                // Habilita todos os controles
                ControleHabilitado = true;

                // Determia o progresso como Determinável
                ProgressoEhIndeterminavel = false;

                // Torna o campo de visibilidade de tempo de espera invisível
                TempoEsperaVisivel = false;

                // Torna o campo de codigo de recuperação invisível
                CodigoRecuperacaoVisivel = false;
            }
        }

        #endregion Métodos
    }
}