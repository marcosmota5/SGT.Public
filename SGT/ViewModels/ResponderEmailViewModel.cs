using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class ResponderEmailViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private ICommand _comandoCancelar;
        private ICommand _comandoFiltrar;
        private ICommand _comandoResponderEmail;
        private CancellationTokenSource _cts;
        private DateTime? _dataDe;
        private DateTime? _dataAte;
        private bool _cancelarVisivel = false;
        private bool _controlesHabilitados = true;
        private bool _progressoEhIndeterminavel = true;
        private bool _progressoVisivel = false;
        private double _valorProgresso = 0;
        private string _textoProgresso;
        private PastaEmail _pastaSelecionada;
        private Proposta _proposta;
        private MailItemComIsSelected _itemEmailSelecionado = new();
        private ObservableCollection<MailItemComIsSelected> _listaItensEmail = new();
        private ObservableCollection<PastaEmail> _listaPastasEmail = new();
        private string _mensagemStatus;
        private string _textoBadgeImportar;
        private bool _visibilidadeColunas;

        private string _caminhoArquivo;
        private string _textoInicial;
        private string _emailsEmCopia;
        private string _assunto;

        private bool _controlesHabilitadosCarregamento;
        private bool _carregamentoVisivel = true;

        private ICommand _comandoVerificaDataDe;
        private ICommand _comandoVerificaDataAte;

        #endregion Campos

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Responder e-mail";
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
                _comandoCancelar = null;
                _comandoFiltrar = null;
                _comandoResponderEmail = null;
                _cts = null;

                _itemEmailSelecionado = null;
                _listaItensEmail = null;
                _listaPastasEmail = null;

                _comandoVerificaDataDe = null;
                _comandoVerificaDataAte = null;
            }
            catch (System.Exception)
            {
            }
        }

        public object ObjetoPastaSelecionada { private get; set; }

        public MailItemComIsSelected ItemEmailSelecionado
        {
            get { return _itemEmailSelecionado; }
            set
            {
                if (value != _itemEmailSelecionado)
                {
                    _itemEmailSelecionado = value;
                    OnPropertyChanged(nameof(ItemEmailSelecionado));
                }
            }
        }

        public ICommand ComandoFechar { get; }

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

        public ICommand ComandoFiltrar
        {
            get
            {
                if (_comandoFiltrar == null)
                {
                    _comandoFiltrar = new RelayCommand(
                        param => FiltrarAsync().Await(),
                        param => true
                    );
                }
                return _comandoFiltrar;
            }
        }

        public ICommand ComandoResponderEmail
        {
            get
            {
                if (_comandoResponderEmail == null)
                {
                    _comandoResponderEmail = new RelayCommand(
                        param => ResponderEmailAsync().Await(),
                        param => true
                    );
                }
                return _comandoResponderEmail;
            }
        }

        public DateTime? DataDe
        {
            get { return _dataDe; }
            set
            {
                if (value != _dataDe)
                {
                    _dataDe = value;
                    OnPropertyChanged(nameof(DataDe));
                }
            }
        }

        public DateTime? DataAte
        {
            get { return _dataAte; }
            set
            {
                if (value != _dataAte)
                {
                    _dataAte = value;
                    OnPropertyChanged(nameof(DataAte));
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

        public bool ControlesHabilitadosCarregamento
        {
            get { return _controlesHabilitadosCarregamento; }
            set
            {
                if (value != _controlesHabilitadosCarregamento)
                {
                    _controlesHabilitadosCarregamento = value;
                    OnPropertyChanged(nameof(ControlesHabilitadosCarregamento));
                }
            }
        }

        public bool CarregamentoVisivel
        {
            get { return _carregamentoVisivel; }
            set
            {
                if (value != _carregamentoVisivel)
                {
                    _carregamentoVisivel = value;
                    OnPropertyChanged(nameof(CarregamentoVisivel));
                }
            }
        }

        public PastaEmail PastaSelecionada
        {
            get { return _pastaSelecionada; }
            set
            {
                if (value != _pastaSelecionada)
                {
                    _pastaSelecionada = value;
                    OnPropertyChanged(nameof(PastaSelecionada));
                }
            }
        }

        public ObservableCollection<MailItemComIsSelected> ListaItensEmail
        {
            get { return _listaItensEmail; }
            set
            {
                if (value != _listaItensEmail)
                {
                    _listaItensEmail = value;
                    OnPropertyChanged(nameof(ListaItensEmail));
                }
            }
        }

        public ObservableCollection<PastaEmail> ListaPastasEmail
        {
            get { return _listaPastasEmail; }
            set
            {
                if (value != _listaPastasEmail)
                {
                    _listaPastasEmail = value;
                    OnPropertyChanged(nameof(ListaPastasEmail));
                }
            }
        }

        public IEnumerable<PastaEmail> EnumerablePastasEmail
        {
            get { return ListaPastasEmail; }
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

        public string TextoBadgeImportar
        {
            get { return _textoBadgeImportar; }
            set
            {
                if (value != _textoBadgeImportar)
                {
                    _textoBadgeImportar = value;
                    OnPropertyChanged(nameof(TextoBadgeImportar));
                }
            }
        }

        public bool VisibilidadeColunas
        {
            get { return _visibilidadeColunas; }
            set
            {
                if (value != _visibilidadeColunas)
                {
                    _visibilidadeColunas = value;
                    OnPropertyChanged(nameof(VisibilidadeColunas));
                }
            }
        }

        public ICommand ComandoVerificaDataDe
        {
            get
            {
                if (_comandoVerificaDataDe == null)
                {
                    _comandoVerificaDataDe = new RelayCommand(
                        param => VerificaDataDe(),
                        param => true
                    );
                }
                return _comandoVerificaDataDe;
            }
        }

        public ICommand ComandoVerificaDataAte
        {
            get
            {
                if (_comandoVerificaDataAte == null)
                {
                    _comandoVerificaDataAte = new RelayCommand(
                        param => VerificaDataAte(),
                        param => true
                    );
                }
                return _comandoVerificaDataAte;
            }
        }

        #endregion Propriedades/Comandos

        #region Construtores

        public ResponderEmailViewModel(string caminhoArquivo, string textoInicial, string emailsEmCopia, string assunto, Action<ResponderEmailViewModel> closeHandler)
        {
            // Define as propriedades
            _caminhoArquivo = caminhoArquivo;
            _textoInicial = textoInicial;
            _emailsEmCopia = emailsEmCopia;
            _assunto = assunto;

            // Atribui o método de limpar listas e a ação de fechar a caixa de diálogo ao comando
            this.ComandoFechar = new SimpleCommand(o => true, o =>
            {
                closeHandler(this);
            });

            // Executa o construtor
            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Métodos

        private void Cancelar()
        {
            if (_cts != null)
                _cts.Cancel();
        }

        private async Task FiltrarAsync()
        {
            PastaSelecionada = new();
            PastaSelecionada = (PastaEmail)ObjetoPastaSelecionada;
            ValorProgresso = 0;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = false;

            try
            {
                Progress<double> progresso = new(dbl =>
                {
                    ValorProgresso = dbl;
                });

                await OutlookClasses.PreencheListaEmailsAsync(PastaSelecionada, DataDe, DataAte, ListaItensEmail, progresso, CancellationToken.None);
            }
            catch (System.Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao atualizar os e-mails");

                MensagemStatus = "Erro ao importar cotação. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
            }
            finally
            {
                ControlesHabilitados = true;
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = false;
                ValorProgresso = 0;
            }
        }

        private async Task ResponderEmailAsync()
        {
            _cts = new();
            CancelarVisivel = true;
            ValorProgresso = 0;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = false;

            try
            {
                await OutlookClasses.ResponderEmailPropostaAsync(ItemEmailSelecionado.MailItem, _caminhoArquivo, _textoInicial, _emailsEmCopia, _assunto, _cts.Token);

                App.ConfiguracoesGerais.DataInicioResponder = DataDe;
                App.ConfiguracoesGerais.DataFimResponder = DataAte;

                App.ConfiguracoesGerais.SalvarNoRegistro();
            }
            catch (System.Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    MensagemStatus = "Operação cancelada";
                }
                else
                {
                    Serilog.Log.Error(ex, "Erro ao enviar e-mail");
                    MensagemStatus = "Falha ao responder o e-mail. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                }
            }
            finally
            {
                CancelarVisivel = false;
                ControlesHabilitados = true;
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = false;
                ValorProgresso = 0;
            }
        }

        private async Task ConstrutorAsync()
        {
            try
            {
                // Desabilita os controles
                //ControlesHabilitados = false;

                // Preenche as listas com as classes necessárias
                await OutlookClasses.PreenchePastasEmailAsync(ListaPastasEmail);

                DataDe = App.ConfiguracoesGerais.DataInicioResponder;
                DataAte = App.ConfiguracoesGerais.DataFimResponder;

                VerificaDataDe();
                VerificaDataAte();

                // Habilita os controles
                //ControlesHabilitados = true;
                ControlesHabilitadosCarregamento = true;


            }
            catch (System.Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");

                MensagemStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";

                ControlesHabilitadosCarregamento = false;
            }
            CarregamentoVisivel = false;
        }

        private void VerificaDataDe()
        {
            if (DataDe != null && DataAte != null)
            {
                if (DataDe > DataAte)
                {
                    DataDe = null;
                }
            }
        }

        private void VerificaDataAte()
        {
            if (DataDe != null && DataAte != null)
            {
                if (DataAte < DataDe)
                {
                    DataAte = null;
                }
            }
        }

        #endregion Métodos
    }
}