using Microsoft.Office.Interop.Outlook;
using Model.DataAccessLayer.Classes;
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
    public class ImportarPecasViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private CancellationTokenSource _cts;
        private DateTime? _dataDe;
        private DateTime? _dataAte;
        private bool _cancelarVisivel = false;
        private bool _importarVisivel = true;
        private bool _controlesHabilitados = true;
        private bool _progressoEhIndeterminavel = true;
        private bool _progressoVisivel = false;
        private double _valorProgresso = 0;
        private string _textoProgresso;
        private string _mensagemStatus;
        private string _textoBadgeImportar;
        private bool _visibilidadeColunas;

        private bool _controlesHabilitadosCarregamento;
        private bool _carregamentoVisivel = true;

        private PastaEmail _pastaSelecionada;
        private Proposta _proposta;
        private Fornecedor _fornecedor;
        private TipoImportacao _tipoImportacao;
        private MailItemComIsSelected _itemEmailSelecionado = new();
        private ObservableCollection<Fornecedor> _listaFornecedores = new();
        private ObservableCollection<TipoImportacao> _listaTipoImportacao;
        private ObservableCollection<MailItemComIsSelected> _listaItensEmail = new();
        private ObservableCollection<PastaEmail> _listaPastasEmail = new();

        private ICommand _comandoCancelar;
        private ICommand _comandoFiltrar;
        private ICommand _comandoImportarUnicoSubstituir;
        private ICommand _comandoImportarUnicoAdicionar;
        private ICommand _comandoImportarMultiplos;

        private ICommand _comandoVerificaDataDe;
        private ICommand _comandoVerificaDataAte;

        #endregion Campos

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Importar peças";
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
                _fornecedor = null;
                _tipoImportacao = null;
                _itemEmailSelecionado = null;
                _listaFornecedores = null;
                _listaTipoImportacao = null;
                _listaItensEmail = null;
                _listaPastasEmail = null;

                _comandoCancelar = null;
                _comandoFiltrar = null;
                _comandoImportarUnicoSubstituir = null;
                _comandoImportarUnicoAdicionar = null;
                _comandoImportarMultiplos = null;

                _comandoVerificaDataDe = null;
                _comandoVerificaDataAte = null;
            }
            catch (System.Exception)
            {
            }
        }

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

        public Fornecedor Fornecedor
        {
            get { return _fornecedor; }
            set
            {
                if (value != _fornecedor)
                {
                    _fornecedor = value;
                    OnPropertyChanged(nameof(Fornecedor));

                    try
                    {
                        foreach (var item in ListaPastasEmail)
                        {
                            foreach (var item2 in item.ListaPastas)
                            {
                                switch (Fornecedor.Id)
                                {
                                    case 1:
                                        if (item2.Id == App.ConfiguracoesGerais.CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor1)
                                        {
                                            PastaSelecionada = item2;
                                            OnPropertyChanged(nameof(PastaSelecionada));
                                            break;
                                        }
                                        break;

                                    case 2:
                                        if (item2.Id == App.ConfiguracoesGerais.CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor2)
                                        {
                                            PastaSelecionada = item2;
                                            OnPropertyChanged(nameof(PastaSelecionada));
                                            break;
                                        }
                                        break;

                                    case 3:
                                        if (item2.Id == App.ConfiguracoesGerais.CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor3)
                                        {
                                            PastaSelecionada = item2;
                                            OnPropertyChanged(nameof(PastaSelecionada));
                                            break;
                                        }
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                        //PastaSelecionada = ListaPastasEmail.First(
                        //    past => past.Id == App.ConfiguracoesGerais.CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor1
                        //    );
                    }
                    catch (System.Exception)
                    {
                    }
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
                        param => this.Cancelar(),
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
                        param => this.Filtrar().Await(),
                        param => true
                    );
                }
                return _comandoFiltrar;
            }
        }

        public ICommand ComandoImportarUnicoSubstituir
        {
            get
            {
                if (_comandoImportarUnicoSubstituir == null)
                {
                    _comandoImportarUnicoSubstituir = new RelayCommand(
                        param => this.ImportarUnico(true).Await(),
                        param => true
                    );
                }
                return _comandoImportarUnicoSubstituir;
            }
        }

        public ICommand ComandoImportarUnicoAdicionar
        {
            get
            {
                if (_comandoImportarUnicoAdicionar == null)
                {
                    _comandoImportarUnicoAdicionar = new RelayCommand(
                        param => this.ImportarUnico(false).Await(),
                        param => true
                    );
                }
                return _comandoImportarUnicoAdicionar;
            }
        }

        public ICommand ComandoImportarMultiplos
        {
            get
            {
                if (_comandoImportarMultiplos == null)
                {
                    _comandoImportarMultiplos = new RelayCommand(
                        param => this.ImportarMultiplos().Await(),
                        param => true
                    );
                }
                return _comandoImportarMultiplos;
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

        public bool ImportarVisivel
        {
            get { return _importarVisivel; }
            set
            {
                if (value != _importarVisivel)
                {
                    _importarVisivel = value;
                    OnPropertyChanged(nameof(ImportarVisivel));
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

        public TipoImportacao TipoImportacao
        {
            get { return _tipoImportacao; }
            set
            {
                if (value != _tipoImportacao)
                {
                    _tipoImportacao = value;
                    OnPropertyChanged(nameof(TipoImportacao));
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

        public ObservableCollection<TipoImportacao> ListaTipoImportacao
        {
            get { return _listaTipoImportacao; }
            set
            {
                if (value != _listaTipoImportacao)
                {
                    _listaTipoImportacao = value;
                    OnPropertyChanged(nameof(ListaTipoImportacao));
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

        public ObservableCollection<Fornecedor> ListaFornecedores
        {
            get { return _listaFornecedores; }
            set
            {
                if (value != _listaFornecedores)
                {
                    _listaFornecedores = value;
                    OnPropertyChanged(nameof(ListaFornecedores));
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

        public ImportarPecasViewModel(Proposta proposta, Fornecedor fornecedor, Action<ImportarPecasViewModel> closeHandler)
        {
            TipoImportacao = new();
            _proposta = proposta;

            // Atribui o método de limpar listas e a ação de fechar a caixa de diálogo ao comando
            this.ComandoFechar = new SimpleCommand(o => true, o =>
            {
                closeHandler(this);
            });

            ConstrutorAsync(fornecedor).Await();
        }

        #endregion Construtores

        #region Métodos

        private async Task SalvaProposta(MailItem olMailItem, CancellationToken ct)
        {

            try
            {

                string pastaCotacao = "";
                ExcelClasses excel = new();

                switch (Fornecedor.Id)
                {
                    case 1:
                        pastaCotacao = System.IO.Path.GetTempPath() + "Proreports\\SGT\\Cotacoes_Kion";
                        break;

                    case 2:
                        pastaCotacao = System.IO.Path.GetTempPath() + "Proreports\\SGT\\Cotacoes_TVH";
                        break;

                    default:
                        break;
                }

                System.IO.Directory.CreateDirectory(pastaCotacao);

                Progress<double> progresso = new(dbl =>
                {
                    ValorProgresso = dbl;
                });

                switch (Fornecedor.Id)
                {
                    case 1:
                        foreach (Attachment olAttachment in olMailItem.Attachments)
                        {
                            if (System.IO.Path.GetExtension(olAttachment.FileName.ToString()) == ".xls")
                            {
                                olAttachment.SaveAsFile(pastaCotacao + "\\Cotacao_Kion" + System.IO.Path.GetExtension(olAttachment.FileName.ToString()));
                                await excel.ImportarCotacao(pastaCotacao + "\\Cotacao_Kion" + System.IO.Path.GetExtension(olAttachment.FileName.ToString()),
                                    Fornecedor, _proposta.Cliente, _proposta.ListaItensProposta, progresso, ct);
                            }
                        }
                        break;

                    case 2:
                        olMailItem.SaveAs(pastaCotacao + "\\Cotacao_TVH.html", OlSaveAsType.olHTML);
                        await excel.ImportarCotacao(pastaCotacao + "\\Cotacao_TVH.html",
                                    Fornecedor, _proposta.Cliente, _proposta.ListaItensProposta, progresso, ct);
                        break;

                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao importar cotação");

                MensagemStatus = "Erro ao importar cotação. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private async Task ImportarPecas(MailItem itemImportar, CancellationToken ct)
        {
            if (itemImportar is null)
            {
                return;
            }
            await SalvaProposta(itemImportar, ct);
        }

        private void Cancelar()
        {
            if (_cts != null)
                _cts.Cancel();
        }

        private async Task Filtrar()
        {
            try
            {
                //PastaSelecionada = new();
                //PastaSelecionada = (PastaEmail)ObjetoPastaSelecionada;
                ValorProgresso = 0;
                ControlesHabilitados = false;
                ProgressoVisivel = true;
                ProgressoEhIndeterminavel = false;

                Progress<double> progresso = new(dbl =>
                {
                    ValorProgresso = dbl;
                });

                await OutlookClasses.PreencheListaEmailsCotacoesAsync(PastaSelecionada, Fornecedor, DataDe, DataAte, ListaItensEmail, progresso, CancellationToken.None);
            }
            catch (System.Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao atualizar e-mails");

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

        private async Task ImportarUnico(bool substituir)
        {
            _cts = new();
            ImportarVisivel = false;
            CancelarVisivel = true;
            ValorProgresso = 0;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = false;

            if (substituir)
            {
                _proposta.ListaItensProposta.Clear();
            }

            try
            {
                await ImportarPecas(ItemEmailSelecionado.MailItem, _cts.Token);
                //pagPropostaOrigem.DefineTotais();
            }
            catch (OperationCanceledException)
            {
                MensagemStatus = "Operação cancelada";
            }
            finally
            {
                ImportarVisivel = true;
                CancelarVisivel = false;
                ControlesHabilitados = true;
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = false;
                ValorProgresso = 0;
            }

            if (!_cts.Token.IsCancellationRequested)
            {
                switch (Fornecedor.Id)
                {
                    case 1:
                        App.ConfiguracoesGerais.CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor1 = PastaSelecionada.Id;
                        break;

                    case 2:
                        App.ConfiguracoesGerais.CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor2 = PastaSelecionada.Id;
                        break;

                    case 3:
                        App.ConfiguracoesGerais.CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor3 = PastaSelecionada.Id;
                        break;

                    default:
                        break;
                }

                App.ConfiguracoesGerais.DataInicioImportar = DataDe;
                App.ConfiguracoesGerais.DataFimImportar = DataAte;

                App.ConfiguracoesGerais.SalvarNoRegistro();

                ComandoFechar.Execute(null);
            }
        }

        private async Task ImportarMultiplos()
        {
            bool itemSelecionado = ListaItensEmail.Where(iss => iss.IsSelected == true).Count() > 0;

            if (!itemSelecionado)
            {
                TextoBadgeImportar = "Selecione ao menos uma cotação";
                return;
            }
            else
            {
                TextoBadgeImportar = "";
            }

            _cts = new();
            ImportarVisivel = false;
            CancelarVisivel = true;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = false;
            ValorProgresso = 0;

            try
            {
                if (TipoImportacao.Id == 2)
                {
                    _proposta.ListaItensProposta.Clear();
                }

                foreach (MailItemComIsSelected item in ListaItensEmail)
                {
                    if ((bool)item.IsSelected)
                    {
                        await ImportarPecas(item.MailItem, _cts.Token);
                    }
                }
                //pagPropostaOrigem.DefineTotais();
            }
            catch (OperationCanceledException)
            {
                MensagemStatus = "Operação cancelada";
            }
            finally
            {
                ImportarVisivel = true;
                CancelarVisivel = false;
                ControlesHabilitados = true;
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = false;
                ValorProgresso = 0;
            }

            if (!_cts.Token.IsCancellationRequested)
            {
                switch (Fornecedor.Id)
                {
                    case 1:
                        App.ConfiguracoesGerais.CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor1 = PastaSelecionada.Id;
                        break;

                    case 2:
                        App.ConfiguracoesGerais.CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor2 = PastaSelecionada.Id;
                        break;

                    case 3:
                        App.ConfiguracoesGerais.CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor3 = PastaSelecionada.Id;
                        break;

                    default:
                        break;
                }

                App.ConfiguracoesGerais.DataInicioImportar = DataDe;
                App.ConfiguracoesGerais.DataFimImportar = DataAte;

                App.ConfiguracoesGerais.SalvarNoRegistro();

                ComandoFechar.Execute(null);
            }
        }

        private async Task ConstrutorAsync(Fornecedor fornecedor)
        {
            try
            {
                //ControlesHabilitados = false;

                // Preenche as listas com as classes necessárias
                await Fornecedor.PreencheListaFornecedoresAsync(ListaFornecedores, true, null, CancellationToken.None, "WHERE forn.id_fornecedor IN (1, 2)", "");
                await OutlookClasses.PreenchePastasEmailAsync(ListaPastasEmail);

                ListaTipoImportacao = new()
                {
                    new TipoImportacao { Id = 1, Nome = "Adicionar" },
                    new TipoImportacao { Id = 2, Nome = "Substituir" }
                };

                TipoImportacao = ListaTipoImportacao[0];

                // Define as classes da proposta como classes com a mesma referência das listas

                try
                {
                    Fornecedor = ListaFornecedores.First(forn => forn.Id == fornecedor?.Id);
                }
                catch (System.Exception)
                {
                }

                DataDe = App.ConfiguracoesGerais.DataInicioImportar;
                DataAte = App.ConfiguracoesGerais.DataFimImportar;

                VerificaDataDe();
                VerificaDataAte();

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

    public class TipoImportacao : ObservableObject
    {
        private int _id;
        private string _nome;

        public int Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Nome
        {
            get { return _nome; }
            set
            {
                if (value != _nome)
                {
                    _nome = value;
                    OnPropertyChanged(nameof(Nome));
                }
            }
        }
    }
}