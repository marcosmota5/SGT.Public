using ControlzEx.Theming;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.HelperClasses;
using Ookii.Dialogs.Wpf;
using SGT.HelperClasses;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class PerfilViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private readonly IDialogCoordinator _dialogCoordinator;

        private Usuario usuarioInicial;
        private Usuario _usuario = new();
        private CancellationTokenSource _cts;
        private bool _sexoEhMasculino;
        private bool _sexoEhFeminino;
        private bool _imagemPadraoVisivel;
        private bool _bloqueiaAlterarLogin;
        private bool _bloqueiaAlterarEmail;
        private string? _toolTipAlterarLogin;
        private string? _toolTipAlterarEmail;
        private string _formatoTelefone;
        private bool _controlesHabilitados;
        private bool _progressoEhIndeterminavel = true;
        private bool _progressoVisivel = false;
        private double _valorProgresso = 0;
        private string _textoProgresso;
        private string _mensagemStatus;
        private ClasseTema _temaSelecionado;
        private ClasseEsquemaCor _esquemaCorSelecionado;

        private bool _cancelarVisivel;

        private bool _permiteSalvar;
        private bool _permiteCancelar;

        private bool _carregamentoVisivel = true;

        private ObservableCollection<ClasseTema> _listaTemas = new();
        private ObservableCollection<ClasseEsquemaCor> _listaEsquemaCores = new();

        private ICommand _comandoSelecionarImagem;
        private ICommand _comandoRemoverImagem;
        private ICommand _comandoSalvar;
        private ICommand _comandoCancelar;

        #endregion Campos

        #region Construtores

        public PerfilViewModel(IDialogCoordinator dialogCoordinator, Usuario usuarioLogado)
        {
            _dialogCoordinator = dialogCoordinator;

            usuarioInicial = usuarioLogado;

            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Perfil";
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
                _usuario = null;
                _cts = null;
                _comandoSelecionarImagem = null;
                _comandoSalvar = null;
                _comandoCancelar = null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Action CloseAction { get; set; }

        public bool ExistemCamposVazios { private get; set; }

        public Usuario UsuarioLogado
        {
            get { return _usuario; }
            set
            {
                _usuario = value;
                OnPropertyChanged(nameof(UsuarioLogado));
                ImagemPadraoVisivel = UsuarioLogado.Imagem == null;
            }
        }

        public bool SexoEhMasculino
        {
            get { return _sexoEhMasculino; }
            set
            {
                _sexoEhMasculino = value;
                OnPropertyChanged(nameof(SexoEhMasculino));
                UsuarioLogado.Sexo = SexoEhMasculino ? "M" : "F";
            }
        }

        public bool SexoEhFeminino
        {
            get { return _sexoEhFeminino; }
            set
            {
                _sexoEhFeminino = value;
                OnPropertyChanged(nameof(SexoEhFeminino));
                UsuarioLogado.Sexo = SexoEhFeminino ? "F" : "M";
            }
        }

        public bool ImagemPadraoVisivel
        {
            get { return _imagemPadraoVisivel; }
            set
            {
                _imagemPadraoVisivel = value;
                OnPropertyChanged(nameof(ImagemPadraoVisivel));
            }
        }

        public bool BloqueiaAlterarLogin
        {
            get { return _bloqueiaAlterarLogin; }
            set
            {
                _bloqueiaAlterarLogin = value;
                OnPropertyChanged(nameof(BloqueiaAlterarLogin));
            }
        }

        public bool BloqueiaAlterarEmail
        {
            get { return _bloqueiaAlterarEmail; }
            set
            {
                _bloqueiaAlterarEmail = value;
                OnPropertyChanged(nameof(BloqueiaAlterarEmail));
            }
        }

        public string? ToolTipAlterarLogin
        {
            get { return _toolTipAlterarLogin; }
            set
            {
                _toolTipAlterarLogin = value;
                OnPropertyChanged(nameof(ToolTipAlterarLogin));
            }
        }

        public string? ToolTipAlterarEmail
        {
            get { return _toolTipAlterarEmail; }
            set
            {
                _toolTipAlterarEmail = value;
                OnPropertyChanged(nameof(ToolTipAlterarEmail));
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

        public Window Janela { private get; set; }

        public ClasseTema TemaSelecionado
        {
            get { return _temaSelecionado; }
            set
            {
                if (value != _temaSelecionado)
                {
                    _temaSelecionado = value;
                    OnPropertyChanged(nameof(TemaSelecionado));
                    if (TemaSelecionado != null && EsquemaCorSelecionado != null)
                    {
                        ThemeManager.Current.ChangeTheme(Janela, TemaSelecionado?.NomeReal + "." + EsquemaCorSelecionado?.NomeReal);
                    }
                }
            }
        }

        public ClasseEsquemaCor EsquemaCorSelecionado
        {
            get { return _esquemaCorSelecionado; }
            set
            {
                if (value != _esquemaCorSelecionado)
                {
                    _esquemaCorSelecionado = value;
                    OnPropertyChanged(nameof(EsquemaCorSelecionado));
                    if (TemaSelecionado != null && EsquemaCorSelecionado != null)
                    {
                        ThemeManager.Current.ChangeTheme(Janela, TemaSelecionado?.NomeReal + "." + EsquemaCorSelecionado?.NomeReal);
                    }
                }
            }
        }

        public ObservableCollection<ClasseTema> ListaTemas
        {
            get { return _listaTemas; }
            set
            {
                if (value != _listaTemas)
                {
                    _listaTemas = value;
                    OnPropertyChanged(nameof(ListaTemas));
                }
            }
        }

        public ObservableCollection<ClasseEsquemaCor> ListaEsquemaCores
        {
            get { return _listaEsquemaCores; }
            set
            {
                if (value != _listaEsquemaCores)
                {
                    _listaEsquemaCores = value;
                    OnPropertyChanged(nameof(ListaEsquemaCores));
                }
            }
        }

        public ICommand ComandoSelecionarImagem
        {
            get
            {
                if (_comandoSelecionarImagem == null)
                {
                    _comandoSelecionarImagem = new RelayCommand(
                        param => AlteraImagem(),
                        param => true
                    );
                }
                return _comandoSelecionarImagem;
            }
        }

        public ICommand ComandoRemoverImagem
        {
            get
            {
                if (_comandoRemoverImagem == null)
                {
                    _comandoRemoverImagem = new RelayCommand(
                        param => RemoverImagem(),
                        param => true
                    );
                }
                return _comandoRemoverImagem;
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

        private void AlteraImagem()
        {
            string caminhoArquivo = "";

            VistaOpenFileDialog vistaOpenFileDialog = new VistaOpenFileDialog()
            {
                Filter = "Arquivos de imagem (*.jpg;*.bmp;*.png)|*.jpg;*.bmp;*.png",
                Title = "Selecione um arquivo de imagem",
                //DefaultExt = "jpg",
                //AddExtension = true,
                FilterIndex = 0
            };

            if ((bool)vistaOpenFileDialog.ShowDialog())
            {
                caminhoArquivo = vistaOpenFileDialog.FileName;
            }

            if (String.IsNullOrEmpty(caminhoArquivo))
            {
                return;
            }

            Image img = Image.FromFile(caminhoArquivo);
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }

            UsuarioLogado.Imagem = arr;
            OnPropertyChanged(nameof(UsuarioLogado));
        }

        private void RemoverImagem()
        {
            UsuarioLogado.Imagem = null;
            ImagemPadraoVisivel = true;
        }

        private async Task ConstrutorAsync()
        {
            try
            {
                try
                {
                    await UsuarioLogado.GetUsuarioDatabaseAsync(usuarioInicial.Id, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    // Escreve no log a exceção e uma mensagem de erro
                    Serilog.Log.Error(ex, "Erro ao carregar dados");

                    MensagemStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";

                    ControlesHabilitados = false;
                    PermiteSalvar = false;
                    PermiteCancelar = false;
                    CarregamentoVisivel = false;
                    return;
                }

                ListaTemas = new();
                ListaEsquemaCores = new();

                ListaTemas.Add(new ClasseTema { Cor = "White", NomeReal = "Light", Nome = "Claro" });
                ListaTemas.Add(new ClasseTema { Cor = "Black", NomeReal = "Dark", Nome = "Escuro" });

                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FFE51400", NomeReal = "Red", Nome = "Vermelho" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FF60A917", NomeReal = "Green", Nome = "Verde" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FF0078D7", NomeReal = "Blue", Nome = "Azul" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FF6459DF", NomeReal = "Purple", Nome = "Roxo" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FFFA6800", NomeReal = "Orange", Nome = "Laranja" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FFA4C400", NomeReal = "Lime", Nome = "Lima" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FF008A00", NomeReal = "Emerald", Nome = "Esmeralda" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FF00ABA9", NomeReal = "Teal", Nome = "Cerceta" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FF1BA1E2", NomeReal = "Cyan", Nome = "Ciano" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FF0050EF", NomeReal = "Cobalt", Nome = "Cobalto" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FF6A00FF", NomeReal = "Indigo", Nome = "Índigo" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FFAA00FF", NomeReal = "Violet", Nome = "Tolet" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FFF472D0", NomeReal = "Pink", Nome = "Rosa" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FFD80073", NomeReal = "Magenta", Nome = "Magenta" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FFA20025", NomeReal = "Crimson", Nome = "Carmesim" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FFF0A30A", NomeReal = "Amber", Nome = "Âmbar" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FFFEDE06", NomeReal = "Yellow", Nome = "Amarelo" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FF825A2C", NomeReal = "Brown", Nome = "Marrom" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FF6D8764", NomeReal = "Olive", Nome = "Oliva" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FF647687", NomeReal = "Steel", Nome = "Aço" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FF76608A", NomeReal = "Mauve", Nome = "Mauva" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FF87794E", NomeReal = "Taupe", Nome = "Taupe" });
                ListaEsquemaCores.Add(new ClasseEsquemaCor { Cor = "#FFA0522D", NomeReal = "Sienna", Nome = "Siena" });

                ListaEsquemaCores = new ObservableCollection<ClasseEsquemaCor>(ListaEsquemaCores.OrderBy(x => x.Nome));

                try
                {
                    if (String.IsNullOrEmpty(UsuarioLogado.Tema))
                    {
                        TemaSelecionado = ListaTemas.First(tema => tema.NomeReal == "Light");
                    }
                    else
                    {
                        TemaSelecionado = ListaTemas.First(tema => tema.NomeReal == UsuarioLogado.Tema);
                    }
                }
                catch (Exception)
                {
                }

                try
                {
                    if (String.IsNullOrEmpty(UsuarioLogado.Cor))
                    {
                        EsquemaCorSelecionado = ListaEsquemaCores.First(esqu => esqu.NomeReal == "Cobalt");
                    }
                    else
                    {
                        EsquemaCorSelecionado = ListaEsquemaCores.First(esqu => esqu.NomeReal == UsuarioLogado.Cor);
                    }
                }
                catch (Exception)
                {
                }

                if (UsuarioLogado.Telefone != null)
                {
                    FormatoTelefone = UsuarioLogado.Telefone.Length > 10 ? @"\(00\)\ 00000\-0000" : @"\(00\)\ 0000\-0000";
                }

                SexoEhMasculino = UsuarioLogado.Sexo == "M";
                SexoEhFeminino = UsuarioLogado.Sexo == "F";

                if (UsuarioLogado.DataUltimaAlteracaoLogin == null)
                {
                    BloqueiaAlterarLogin = false;
                    ToolTipAlterarLogin = null;
                }
                else
                {
                    TimeSpan timeSpanLogin = DateTime.Now - Convert.ToDateTime(UsuarioLogado.DataUltimaAlteracaoLogin);
                    BloqueiaAlterarLogin = timeSpanLogin.Days < 60;
                    ToolTipAlterarLogin = BloqueiaAlterarLogin ? "O login foi alterado há menos de 60 dias. Você só poderá alterar o seu login a partir de " + DateTime.Now.AddDays(60 - timeSpanLogin.Days).ToString("d") : null;
                }

                if (UsuarioLogado.DataUltimaAlteracaoEmail == null)
                {
                    BloqueiaAlterarEmail = false;
                    ToolTipAlterarEmail = null;
                }
                else
                {
                    TimeSpan timeSpanEmail = DateTime.Now - Convert.ToDateTime(UsuarioLogado.DataUltimaAlteracaoEmail);
                    BloqueiaAlterarEmail = timeSpanEmail.Days < 60;
                    ToolTipAlterarEmail = BloqueiaAlterarEmail ? "O e-mail foi alterado há menos de 60 dias. Você só poderá alterar o seu e-mail a partir de " + DateTime.Now.AddDays(60 - timeSpanEmail.Days).ToString("d") : null;
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

            if (TemaSelecionado.NomeReal != UsuarioLogado.Tema || EsquemaCorSelecionado.NomeReal != UsuarioLogado.Cor)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Entendi"
                };

                var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Tema alterado", "Alguns elementos só atualizarão para o novo tema após a reinicialização do sistema", MessageDialogStyle.Affirmative, mySettings);
            }

            _cts = new();

            ValorProgresso = 0;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = true;
            MensagemStatus = "Salvando dados, aguarde...";

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

            usuarioInicial.Nome = UsuarioLogado.Nome;
            usuarioInicial.CPF = UsuarioLogado.CPF;
            usuarioInicial.Sexo = UsuarioLogado.Sexo;
            usuarioInicial.Telefone = UsuarioLogado.Telefone;
            usuarioInicial.Imagem = UsuarioLogado.Imagem;
            usuarioInicial.Cor = EsquemaCorSelecionado?.NomeReal;
            usuarioInicial.Tema = TemaSelecionado?.NomeReal;
            usuarioInicial.LimiteResultados = UsuarioLogado.LimiteResultados;
            usuarioInicial.TextoRespostaEmail = UsuarioLogado.TextoRespostaEmail;
            usuarioInicial.EmailsEmCopia = UsuarioLogado.EmailsEmCopia;

            if (UsuarioLogado.Login != usuarioInicial.Login)
            {
                usuarioInicial.Login = UsuarioLogado.Login;
                usuarioInicial.DataUltimaAlteracaoLogin = DateTime.Now;
            }

            if (UsuarioLogado.Email != usuarioInicial.Email)
            {
                usuarioInicial.Email = UsuarioLogado.Email;
                usuarioInicial.DataUltimaAlteracaoEmail = DateTime.Now;
            }

            try
            {
                await usuarioInicial.SalvarUsuarioDatabaseAsync(_cts.Token);
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

            ThemeManager.Current.ChangeTheme(App.Current, TemaSelecionado?.NomeReal + "." + EsquemaCorSelecionado?.NomeReal);

            MensagemStatus = "Dados salvos com sucesso. Encerrando...";

            OnPropertyChanged(nameof(usuarioInicial));

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

    #region Classes

    public class ClasseTema
    {
        public string Cor { get; set; }
        public string NomeReal { get; set; }
        public string Nome { get; set; }
    }

    public class ClasseEsquemaCor
    {
        public string Cor { get; set; }
        public string NomeReal { get; set; }
        public string Nome { get; set; }
    }

    #endregion Classes
}