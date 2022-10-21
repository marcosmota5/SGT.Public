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
    public class AlterarSenhaComAtualViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private Usuario _usuario = new();
        private CancellationTokenSource _cts;
        private string _novaSenha;
        private bool _tamanhoNovaSenhaVisivel = true;
        private bool _maiusculasNovaSenhaVisivel = true;
        private bool _minusculasNovaSenhaVisivel = true;
        private bool _numerosNovaSenhaVisivel = true;
        private bool _caractereEspecialNovaSenhaVisivel = true;
        private bool _controlesHabilitados = true;
        private bool _progressoEhIndeterminavel = true;
        private bool _progressoVisivel = false;
        private double _valorProgresso = 0;
        private string _textoProgresso;
        private string _mensagemStatus;

        private bool _cancelarVisivel;

        private bool _permiteSalvar = true;
        private bool _permiteCancelar;

        private ICommand _comandoSalvar;
        private ICommand _comandoCancelar;

        #endregion Campos

        #region Construtores

        public AlterarSenhaComAtualViewModel(Usuario usuarioLogado)
        {
            UsuarioLogado = usuarioLogado;
        }

        #endregion Construtores

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
                _cts = null;
                _comandoSalvar = null;
                _comandoCancelar = null;
            }
            catch (Exception)
            {
            }
        }

        public Action CloseAction { get; set; }

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

        public string SenhaAtual { private get; set; }
        public string ConfirmacaoSenha { private get; set; }

        public Usuario UsuarioLogado
        {
            get { return _usuario; }
            set
            {
                _usuario = value;
                OnPropertyChanged(nameof(UsuarioLogado));
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

        #endregion Propriedades/Comandos

        #region Métodos

        private async Task SalvarAsync()
        {
            if (ExistemCamposVazios)
            {
                MensagemStatus = "Campos obrigatórios vazios/inválidos";
                return;
            }

            _cts = new();

            ValorProgresso = 0;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = true;
            MensagemStatus = "Alterando senha, aguarde...";

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
                ControlesHabilitados = true;
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = true;

                CancelarVisivel = false;
                PermiteCancelar = false;
                PermiteSalvar = true;

                MensagemStatus = "Operação cancelada";
                return;
            }

            try
            {
                var senhaCorreta = await Usuario.SenhaEstaCorretaDatabaseAsync(UsuarioLogado.Id, SenhaAtual, _cts.Token);

                if (!senhaCorreta)
                {
                    throw new ValorIncorretoException("Senha atual errada");
                }

                if (NovaSenha != ConfirmacaoSenha)
                {
                    throw new ValorIncorretoException("A confirmação não confere");
                }

                await Usuario.AlterarSenhaUsuarioDatabaseAsync(UsuarioLogado.Id, NovaSenha, _cts.Token, DateTime.Now);
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

                return;
            }

            MensagemStatus = "Senha alterada com sucesso. Encerrando...";

            await Task.Delay(1000, CancellationToken.None);

            CloseAction();
        }

        private void Cancelar()
        {
            if (_cts != null)
                _cts.Cancel();

            CancelarVisivel = false;
            PermiteCancelar = false;
            PermiteSalvar = false;
        }

        #endregion Métodos
    }
}