using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
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
    public class CadastrarViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private Usuario _novoUsuario = new();
        private CancellationTokenSource _cts;
        private bool _sexoEhMasculino;
        private bool _sexoEhFeminino;
        private string _formatoTelefone;
        private string _senha;
        private bool _tamanhoNovaSenhaVisivel;
        private bool _maiusculasNovaSenhaVisivel;
        private bool _minusculasNovaSenhaVisivel;
        private bool _numerosNovaSenhaVisivel;
        private bool _caractereEspecialNovaSenhaVisivel;
        private bool _senhaInvalida;
        private bool _controlesHabilitados;
        private bool _progressoEhIndeterminavel = true;
        private bool _progressoVisivel = false;
        private double _valorProgresso = 0;
        private string _textoProgresso;
        private string _mensagemStatus;

        private bool _cancelarVisivel;

        private bool _permiteSalvar;
        private bool _permiteCancelar;

        private bool _carregamentoVisivel = true;

        private ObservableCollection<Filial> _listaFiliais = new();
        private ObservableCollection<Setor> _listaSetores = new();
        private ObservableCollection<Perfil> _listaPerfis = new();
        private ObservableCollection<Status> _listaStatus = new();

        private ICommand _comandoSalvar;
        private ICommand _comandoCancelar;

        #endregion Campos

        #region Construtores

        public CadastrarViewModel()
        {
            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Cadastrar";
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
                _listaFiliais = null;
                _listaSetores = null;
                _listaPerfis = null;
                _listaStatus = null;

                _cts = null;
                _novoUsuario = null;
                _comandoSalvar = null;
                _comandoCancelar = null;
            }
            catch (Exception)
            {
            }
        }

        public Action CloseAction { get; set; }

        public bool ExistemCamposVazios { private get; set; }

        public string Senha
        {
            get { return _senha; }
            set
            {
                if (value != _senha)
                {
                    _senha = value;
                    TamanhoNovaSenhaVisivel = !FuncoesDeSenha.SenhaPossuiTamanhoMinimo(Senha);
                    MaiusculasNovaSenhaVisivel = !FuncoesDeSenha.SenhaPossuiMaiusculas(Senha);
                    MinusculasNovaSenhaVisivel = !FuncoesDeSenha.SenhaPossuiMinusculas(Senha);
                    NumerosNovaSenhaVisivel = !FuncoesDeSenha.SenhaPossuiNumeros(Senha);
                    CaractereEspecialNovaSenhaVisivel = !FuncoesDeSenha.SenhaPossuiCaraceteresEspeciais(Senha);
                    SenhaInvalida = !FuncoesDeSenha.SenhaEhValida(Senha);
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

        public bool SenhaInvalida
        {
            get { return _senhaInvalida; }
            set
            {
                if (value != _senhaInvalida)
                {
                    _senhaInvalida = value;
                    OnPropertyChanged(nameof(SenhaInvalida));
                }
            }
        }

        public string ConfirmacaoSenha { private get; set; }

        public Usuario NovoUsuario
        {
            get { return _novoUsuario; }
            set
            {
                _novoUsuario = value;
                OnPropertyChanged(nameof(NovoUsuario));
            }
        }

        public bool SexoEhMasculino
        {
            get { return _sexoEhMasculino; }
            set
            {
                _sexoEhMasculino = value;
                OnPropertyChanged(nameof(SexoEhMasculino));
                NovoUsuario.Sexo = SexoEhMasculino ? "M" : "F";
            }
        }

        public bool SexoEhFeminino
        {
            get { return _sexoEhFeminino; }
            set
            {
                _sexoEhFeminino = value;
                OnPropertyChanged(nameof(SexoEhFeminino));
                NovoUsuario.Sexo = SexoEhFeminino ? "F" : "M";
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

        #endregion Propriedades/Comandos

        #region Métodos

        private async Task ConstrutorAsync()
        {
            try
            {
                SexoEhMasculino = true;

                // Preenche as listas com as classes necessárias
                await Filial.PreencheListaFiliaisAsync(ListaFiliais, true, null, CancellationToken.None, "", "");
                await Setor.PreencheListaSetoresAsync(ListaSetores, true, null, CancellationToken.None, "", "");
                await Perfil.PreencheListaPerfisAsync(ListaPerfis, true, null, CancellationToken.None, "", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");

                // Define valores
                try
                {
                    NovoUsuario.Filial = ListaFiliais.First(fili => fili.Id == App.Usuario.Filial.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    NovoUsuario.Setor = ListaSetores.First(seto => seto.Id == App.Usuario.Setor.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    NovoUsuario.Perfil = ListaPerfis.First(perf => perf.Id == 2);
                }
                catch (Exception)
                {
                }
                try
                {
                    NovoUsuario.Status = ListaStatus.First(stat => stat.Id == 1);
                }
                catch (Exception)
                {
                }
                ControlesHabilitados = true;
                PermiteSalvar = true;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");

                MensagemStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                PermiteSalvar = false;
                ControlesHabilitados = false;
            }

            CarregamentoVisivel = false;
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
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = true;
            MensagemStatus = "Cadastrando usuário, aguarde...";

            CancelarVisivel = true;
            PermiteCancelar = true;
            PermiteSalvar = false;

            NovoUsuario.TextoRespostaEmail = "Conforme vossa solicitação, envio a proposta referente à(s) peça(s) e/ou serviço(s) solicitado(s). Aguardo vossa análise e aprovação.";
            NovoUsuario.DataCadastro = DateTime.Now;
            NovoUsuario.CPF = Convert.ToInt64(NovoUsuario.CPF).ToString("00000000000");

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
                await NovoUsuario.SalvarUsuarioDatabaseAsync(_cts.Token);
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

                return;
            }

            MensagemStatus = "Usuário cadastrado com sucesso. Encerrando...";

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