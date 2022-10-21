using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.HelperClasses;
using Ookii.Dialogs.Wpf;
using SGT.HelperClasses;
using SGT.Views;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class ExportarOrdemServicoViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private ObservableCollection<FiltroExportacao> _filtroExportacao;
        public ObservableCollection<object> _filtrosSelecionados;
        private bool _controlesHabilitados;
        private bool _carregamentoVisivel = true;

        private readonly IDialogCoordinator _dialogCoordinator;
        private CancellationTokenSource _cts;
        private double _valorProgresso;
        private bool _progressoEhIndeterminavel;
        private bool _progressoVisivel = false;
        private string _mensagemStatus;

        private bool _formatarExportacao = true;
        private bool _apenasRegistrosAtivos = true;

        private DateTime? _dataInsercaoDe;
        private DateTime? _dataInsercaoAte;
        private DateTime? _dataAtendimentoDe;
        private DateTime? _dataAtendimentoAte;

        private ICommand _comandoVerificaDataInsercaoDe;
        private ICommand _comandoVerificaDataInsercaoAte;
        private ICommand _comandoVerificaDataAtendimentoDe;
        private ICommand _comandoVerificaDataAtendimentoAte;

        private ICommand _comandoCancelar;
        private ICommand _comandoExportar;

        #endregion Campos

        #region Construtores

        public ExportarOrdemServicoViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;

            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Propriedades

        public string Name
        {
            get
            {
                return "Exportar ordens de serviço";
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
            }
            catch (Exception)
            {
            }
        }

        public ObservableCollection<FiltroExportacao> FiltroExportacao
        {
            get { return _filtroExportacao; }
            set
            {
                if (value != _filtroExportacao)
                {
                    _filtroExportacao = value;
                    OnPropertyChanged(nameof(FiltroExportacao));
                }
            }
        }

        public ObservableCollection<object> FiltrosSelecionados
        {
            get { return _filtrosSelecionados; }
            set
            {
                if (value != _filtrosSelecionados)
                {
                    _filtrosSelecionados = value;
                    OnPropertyChanged(nameof(FiltrosSelecionados));
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
            get
            {
                return _valorProgresso;
            }
            set
            {
                if (value != _valorProgresso)
                {
                    _valorProgresso = value;
                    if (ProgressoEhIndeterminavel)
                    {
                        ProgressoVisivel = true;
                    }
                    else
                    {
                        ProgressoVisivel = ValorProgresso > 0 && ValorProgresso < 100;
                    }
                    OnPropertyChanged(nameof(ValorProgresso));
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
                    if (ProgressoEhIndeterminavel)
                    {
                        ProgressoVisivel = true;
                    }
                    else
                    {
                        ProgressoVisivel = ValorProgresso > 0 && ValorProgresso < 100;
                    }
                    OnPropertyChanged(nameof(ProgressoEhIndeterminavel));
                }
            }
        }

        public bool FormatarExportacao
        {
            get { return _formatarExportacao; }
            set
            {
                if (value != _formatarExportacao)
                {
                    _formatarExportacao = value;
                    OnPropertyChanged(nameof(FormatarExportacao));
                }
            }
        }

        public bool ApenasRegistrosAtivos
        {
            get { return _apenasRegistrosAtivos; }
            set
            {
                if (value != _apenasRegistrosAtivos)
                {
                    _apenasRegistrosAtivos = value;
                    OnPropertyChanged(nameof(ApenasRegistrosAtivos));
                }
            }
        }

        public DateTime? DataInsercaoDe
        {
            get { return _dataInsercaoDe; }
            set
            {
                if (value != _dataInsercaoDe)
                {
                    _dataInsercaoDe = value;
                    OnPropertyChanged(nameof(DataInsercaoDe));
                }
            }
        }

        public DateTime? DataInsercaoAte
        {
            get { return _dataInsercaoAte; }
            set
            {
                if (value != _dataInsercaoAte)
                {
                    _dataInsercaoAte = value;
                    OnPropertyChanged(nameof(DataInsercaoAte));
                }
            }
        }

        public DateTime? DataAtendimentoDe
        {
            get { return _dataAtendimentoDe; }
            set
            {
                if (value != _dataAtendimentoDe)
                {
                    _dataAtendimentoDe = value;
                    OnPropertyChanged(nameof(DataAtendimentoDe));
                }
            }
        }

        public DateTime? DataAtendimentoAte
        {
            get { return _dataAtendimentoAte; }
            set
            {
                if (value != _dataAtendimentoAte)
                {
                    _dataAtendimentoAte = value;
                    OnPropertyChanged(nameof(DataAtendimentoAte));
                }
            }
        }

        public ICommand ComandoVerificaDataInsercaoDe
        {
            get
            {
                if (_comandoVerificaDataInsercaoDe == null)
                {
                    _comandoVerificaDataInsercaoDe = new RelayCommand(
                        param => VerificaDataInsercaoDe(),
                        param => true
                    );
                }
                return _comandoVerificaDataInsercaoDe;
            }
        }

        public ICommand ComandoVerificaDataInsercaoAte
        {
            get
            {
                if (_comandoVerificaDataInsercaoAte == null)
                {
                    _comandoVerificaDataInsercaoAte = new RelayCommand(
                        param => VerificaDataInsercaoAte(),
                        param => true
                    );
                }
                return _comandoVerificaDataInsercaoAte;
            }
        }

        public ICommand ComandoVerificaDataAtendimentoDe
        {
            get
            {
                if (_comandoVerificaDataAtendimentoDe == null)
                {
                    _comandoVerificaDataAtendimentoDe = new RelayCommand(
                        param => VerificaDataAtendimentoDe(),
                        param => true
                    );
                }
                return _comandoVerificaDataAtendimentoDe;
            }
        }

        public ICommand ComandoVerificaDataAtendimentoAte
        {
            get
            {
                if (_comandoVerificaDataAtendimentoAte == null)
                {
                    _comandoVerificaDataAtendimentoAte = new RelayCommand(
                        param => VerificaDataAtendimentoAte(),
                        param => true
                    );
                }
                return _comandoVerificaDataAtendimentoAte;
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

        public ICommand ComandoExportar
        {
            get
            {
                if (_comandoExportar == null)
                {
                    _comandoExportar = new RelayCommand(
                        param => Exportar().Await(),
                        param => true
                    );
                }
                return _comandoExportar;
            }
        }

        #endregion Propriedades

        #region Métodos

        private async Task ConstrutorAsync()
        {
            FiltrosSelecionados = new ObservableCollection<object>();
            ObservableCollection<FiltroExportacao> filtros = new ObservableCollection<FiltroExportacao>();

            try
            {
                DataTable dataTable = await ExportacaoOrdemServico.GetDataTableAsync(CancellationToken.None, true, "", "");

                foreach (System.Data.DataColumn coluna in dataTable.Columns)
                {
                    filtros.Add(new FiltroExportacao() { Nome = coluna.ColumnName });

                    DataView view = new(dataTable);

                    FiltroExportacao filtroExportacao = filtros.First(x => x.Nome == coluna.ColumnName);

                    filtroExportacao.Filtros = new ObservableCollection<FiltroExportacao>();

                    foreach (System.Data.DataRow linha in view.ToTable(true, coluna.ColumnName).Rows)
                    {
                        if (linha[0] != null)
                        {
                            filtroExportacao.Filtros.Add(new FiltroExportacao() { Nome = linha[0].ToString(), Coluna = coluna.ColumnName });
                        }
                    }

                    filtroExportacao.Filtros = new ObservableCollection<FiltroExportacao>(filtroExportacao.Filtros.OrderBy(x => x.Nome));
                }
            }
            catch (Exception)
            {
            }

            FiltroExportacao = filtros;

            ControlesHabilitados = true;

            //FiltroExportacao = new ObservableCollection<FiltroExportacao>();

            //var continental = new FiltroExportacao() { Nome = "Continental" };
            //var leBiscuit = new FiltroExportacao() { Nome = "Le Biscuit" };

            //var marcos = new FiltroExportacao() { Nome = "Marcos Mota" };
            //var patricia = new FiltroExportacao() { Nome = "Patrícia Rosa" };
            //var robson = new FiltroExportacao() { Nome = "Robson Lessa" };

            //cliente.Filtros = new ObservableCollection<FiltroExportacao>
            //{
            //    continental,
            //    leBiscuit
            //};

            //usuario.Filtros = new ObservableCollection<FiltroExportacao>
            //{
            //    marcos,
            //    patricia,
            //    robson
            //};

            //FiltroExportacao.Add(cliente);
            //FiltroExportacao.Add(usuario);

            //try
            //{
            //    // Preenche as listas com as classes necessárias
            //    await Cliente.PreencheListaClientesAsync(ListaClientes, true, null, CancellationToken.None, "ORDER BY clie.nome ASC", "");
            //    await Planta.PreencheListaPlantasAsync(ListaPlantas, true, null, CancellationToken.None, "ORDER BY plan.nome ASC", "");
            //    await Area.PreencheListaAreasAsync(ListaAreas, true, null, CancellationToken.None, "ORDER BY area.nome ASC", "");
            //    await Frota.PreencheListaFrotasAsync(ListaFrotas, true, null, CancellationToken.None, "ORDER BY frot.nome ASC", "");
            //    await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");
            //    await Serie.PreencheListaSeriesAsync(ListaSeries, true, null, CancellationToken.None, "", "");
            //    await Fabricante.PreencheListaFabricantesAsync(ListaFabricantes, true, null, CancellationToken.None, "ORDER BY fabr.nome ASC", "");
            //    await Categoria.PreencheListaCategoriasAsync(ListaCategorias, true, null, CancellationToken.None, "ORDER BY cate.nome ASC", "");
            //    await TipoEquipamento.PreencheListaTiposEquipamentoAsync(ListaTiposEquipamento, true, null, CancellationToken.None, "ORDER BY tieq.nome ASC", "");
            //    await Classe.PreencheListaClassesAsync(ListaClasses, true, null, CancellationToken.None, "ORDER BY clas.nome ASC", "");
            //    await Modelo.PreencheListaModelosAsync(ListaModelos, true, null, CancellationToken.None, "ORDER BY mode.nome ASC", "");
            //    await Ano.PreencheListaAnosAsync(ListaAnos, true, null, CancellationToken.None, "WHERE ano <= @ano", "@ano", DateTime.Now.Year + 1);

            //    await TipoOrdemServico.PreencheListaTiposOrdemServicoAsync(ListaTiposOrdemServico, true, null, CancellationToken.None, "ORDER BY tios.nome ASC", "");
            //    await StatusEquipamentoAposManutencao.PreencheListaStatusEquipamentoAposManutencaoAsync(ListaStatusEquipamentoAposManutencao, true, null, CancellationToken.None, "ORDER BY seam.nome ASC", "");
            //    await TipoManutencao.PreencheListaTiposManutencaoAsync(ListaTipoManutencao, true, null, CancellationToken.None, "ORDER BY tima.id_tipo_manutencao ASC", "");
            //    await UsoIndevido.PreencheListaUsosIndevidosAsync(ListaUsoIndevido, true, null, CancellationToken.None, "ORDER BY usoi.nome ASC", "");
            //    await ExecutanteServico.PreencheListaExecutantesServicoAsync(ListaExecutanteServico, true, null, CancellationToken.None, "ORDER BY exse.nome ASC", "");

            //}
            //catch (Exception ex)
            //{
            //    // Escreve no log a exceção e uma mensagem de erro
            //    Serilog.Log.Error(ex, "Erro ao carregar dados");

            //    Messenger.Default.Send<string>("Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log", "MensagemStatus");

            //    ControlesHabilitados = false;
            //}
            //CarregamentoVisivel = false;
        }

        private void VerificaDataInsercaoDe()
        {
            if (DataInsercaoDe != null && DataInsercaoAte != null)
            {
                if (DataInsercaoDe > DataInsercaoAte)
                {
                    DataInsercaoDe = null;
                }
            }
        }

        private void VerificaDataInsercaoAte()
        {
            if (DataInsercaoDe != null && DataInsercaoAte != null)
            {
                if (DataInsercaoAte < DataInsercaoDe)
                {
                    DataInsercaoAte = null;
                }
            }
        }

        private void VerificaDataAtendimentoDe()
        {
            if (DataAtendimentoDe != null && DataAtendimentoAte != null)
            {
                if (DataAtendimentoDe > DataAtendimentoAte)
                {
                    DataAtendimentoDe = null;
                }
            }
        }

        private void VerificaDataAtendimentoAte()
        {
            if (DataAtendimentoDe != null && DataAtendimentoAte != null)
            {
                if (DataAtendimentoAte < DataAtendimentoDe)
                {
                    DataAtendimentoAte = null;
                }
            }
        }

        private void Cancelar()
        {
            if (_cts != null)
                _cts.Cancel();
        }

        private async Task Exportar()
        {
            _cts = new();

            ControlesHabilitados = false;

            string condicoes = "";
            List<string> listaParametros = new();
            List<object> listaValores = new();
            string nomeParametro = "";
            string valorFormatado = "";
            int contagem = 1;

            ObservableCollection<FiltroExportacao> listaFiltros = new();

            foreach (FiltroExportacao item in FiltrosSelecionados)
            {
                if (!String.IsNullOrEmpty(item.Coluna))
                {
                    if (listaFiltros.Where(x => x.Nome == item.Coluna).Count() == 0)
                    {
                        listaFiltros.Add(new FiltroExportacao() { Nome = item.Coluna });
                    }

                    FiltroExportacao filtro = listaFiltros.Where(x => x.Nome == item.Coluna).First();

                    if (filtro.Filtros == null)
                    {
                        filtro.Filtros = new();
                    }

                    filtro.Filtros.Add(item);
                }
            }

            foreach (var filtro in listaFiltros)
            {
                if (String.IsNullOrEmpty(condicoes))
                {
                    condicoes = "`" + filtro.Nome + "` IN (";
                }
                else
                {
                    condicoes += ") AND `" + filtro.Nome + "` IN (";
                }

                foreach (var subFiltro in filtro.Filtros)
                {
                    nomeParametro = "@parametro" + contagem;
                    valorFormatado = subFiltro.Nome;

                    listaParametros.Add(nomeParametro);
                    listaValores.Add(valorFormatado);

                    if (condicoes.EndsWith("("))
                    {
                        condicoes += nomeParametro;
                    }
                    else
                    {
                        condicoes += ", " + nomeParametro;
                    }

                    contagem++;
                }
            }

            if (!String.IsNullOrEmpty(condicoes))
            {
                condicoes += ")";
            }

            if (ApenasRegistrosAtivos)
            {
                listaParametros.Add("@status");
                listaValores.Add("Ativo");

                if (!String.IsNullOrEmpty(condicoes))
                {
                    condicoes += " AND `Status` = @status";
                }
                else
                {
                    condicoes = "`Status` = @status";
                }
            }

            if (DataInsercaoDe != null && DataInsercaoAte != null)
            {
                listaParametros.Add("@data_insercao_de");
                listaParametros.Add("@data_insercao_ate");
                listaValores.Add(DataInsercaoDe);
                listaValores.Add(DataInsercaoAte);

                if (!String.IsNullOrEmpty(condicoes))
                {
                    condicoes += " AND DATE(`Data da digitação`) >= @data_insercao_de AND DATE(`Data da digitação`) <= @data_insercao_ate";
                }
                else
                {
                    condicoes = "DATE(`Data da digitação`) >= @data_insercao_de AND DATE(`Data da digitação`) <= @data_insercao_ate";
                }
            }

            if (DataInsercaoDe != null && DataInsercaoAte == null)
            {
                listaParametros.Add("@data_insercao_de");
                listaValores.Add(DataInsercaoDe);

                if (!String.IsNullOrEmpty(condicoes))
                {
                    condicoes += " AND DATE(`Data da digitação`) >= @data_insercao_de";
                }
                else
                {
                    condicoes = "DATE(`Data da digitação`) >= @data_insercao_de";
                }
            }

            if (DataInsercaoDe == null && DataInsercaoAte != null)
            {
                listaParametros.Add("@data_insercao_ate");
                listaValores.Add(DataInsercaoAte);

                if (!String.IsNullOrEmpty(condicoes))
                {
                    condicoes += " AND DATE(`Data da digitação`) <= @data_insercao_ate";
                }
                else
                {
                    condicoes = "DATE(`Data da digitação`) <= @data_insercao_ate";
                }
            }
            //

            if (DataAtendimentoDe != null && DataAtendimentoAte != null)
            {
                listaParametros.Add("@data_atendimento_de");
                listaParametros.Add("@data_atendimento_ate");
                listaValores.Add(DataAtendimentoDe);
                listaValores.Add(DataAtendimentoAte);

                if (!String.IsNullOrEmpty(condicoes))
                {
                    condicoes += " AND DATE(`Data do atendimento`) >= @data_atendimento_de AND DATE(`Data do atendimento`) <= @data_atendimento_ate";
                }
                else
                {
                    condicoes = "DATE(`Data do atendimento`) >= @data_atendimento_de AND DATE(`Data do atendimento`) <= @data_atendimento_ate";
                }
            }

            if (DataAtendimentoDe != null && DataAtendimentoAte == null)
            {
                listaParametros.Add("@data_atendimento_de");
                listaValores.Add(DataAtendimentoDe);

                if (!String.IsNullOrEmpty(condicoes))
                {
                    condicoes += " AND DATE(`Data do atendimento`) >= @data_atendimento_de";
                }
                else
                {
                    condicoes = "DATE(`Data do atendimento`) >= @data_atendimento_de";
                }
            }

            if (DataAtendimentoDe == null && DataAtendimentoAte != null)
            {
                listaParametros.Add("@data_atendimento_ate");
                listaValores.Add(DataAtendimentoAte);

                if (!String.IsNullOrEmpty(condicoes))
                {
                    condicoes += " AND DATE(`Data do atendimento`) <= @data_atendimento_ate";
                }
                else
                {
                    condicoes = "DATE(`Data do atendimento`) <= @data_atendimento_ate";
                }
            }

            DataTable dataTable = await ExportacaoOrdemServico.GetDataTableAsync(CancellationToken.None, false, condicoes + " ORDER BY `Data da digitação` DESC",
                String.Join(",", listaParametros.ToArray()), listaValores.ToArray());

            if (dataTable.Rows.Count == 0)
            {
                var mySettings2 = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var resp = await _dialogCoordinator.ShowMessageAsync(this,
                        "Tabela vazia", "Não há dados a serem exportados. Verifique os filtros aplicados.", MessageDialogStyle.Affirmative, mySettings2);

                ControlesHabilitados = true;

                return;
            }

            VistaSaveFileDialog sfd = new VistaSaveFileDialog()
            {
                Filter = "Arquivo do Excel (*.xlsx)|*.xlsx",
                Title = "Informe o local e o nome do arquivo",
                FileName = "Registro_Servico_" + DateTime.Now.ToString("yyyyMMddhhmmss"),
                AddExtension = true
            };

            bool houveErro = false;

            try
            {
                if (sfd.ShowDialog() == true)
                {
                    if (!sfd.FileName.EndsWith(".xlsx"))
                    {
                        sfd.FileName = sfd.FileName + ".xlsx";
                    }

                    //Create an instance of ExcelEngine
                    using (ExcelEngine excelEngine = new ExcelEngine())
                    {
                        //Initialize application object
                        IApplication application = excelEngine.Excel;

                        //Set the default application version as Excel 2016
                        application.DefaultVersion = ExcelVersion.Excel2016;

                        //Create a new workbook
                        IWorkbook workbook = application.Workbooks.Create(1);

                        //Access first worksheet from the workbook instance
                        IWorksheet worksheet = workbook.Worksheets[0];

                        //Exporting DataTable to worksheet
                        worksheet.ImportDataTable(dataTable, true, 1, 1);

                        if (!FormatarExportacao)
                        {
                            foreach (var item in worksheet.Columns)
                            {
                                item.NumberFormat = "@";
                            }
                        }
                        else
                        {
                            worksheet.Columns[8].NumberFormat = "@";
                        }

                        worksheet.UsedRange.AutofitColumns();

                        //Save the workbook to disk in xlsx format
                        workbook.SaveAs(sfd.FileName);
                    }
                }
                else
                {
                    ControlesHabilitados = true;

                    return;
                }
            }
            catch (Exception ex)
            {
                houveErro = true;

                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao exportar dados");

                MensagemStatus = "Falha na eportação dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
            }

            if (!houveErro)
            {
                try
                {
                    var mySettings = new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Sim",
                        NegativeButtonText = "Não"
                    };

                    var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                        "Database exportada", "Deseja abrir o arquivo?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);

                    if (respostaMensagem == MessageDialogResult.Affirmative)
                    {
                        ProcessStartInfo psInfo = new ProcessStartInfo
                        {
                            FileName = sfd.FileName,
                            UseShellExecute = true
                        };

                        Process.Start(psInfo);
                    }
                }
                catch (Exception ex)
                {
                    // Escreve no log a exceção e uma mensagem de erro
                    Serilog.Log.Error(ex, "Erro ao abrir o arquivo");

                    MensagemStatus = "Falha na abertura do arquivo. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                }
            }

            ControlesHabilitados = true;
        }

        #endregion Métodos
    }

    public class FiltroExportacao : ObservableObject
    {
        #region Campos

        private string _nome;
        private string _coluna;
        private ObservableCollection<FiltroExportacao> _filtros;

        #endregion Campos

        #region Propriedades

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

        public string Coluna
        {
            get { return _coluna; }
            set
            {
                if (value != _coluna)
                {
                    _coluna = value;
                    OnPropertyChanged(nameof(Coluna));
                }
            }
        }

        public ObservableCollection<FiltroExportacao> Filtros
        {
            get { return _filtros; }
            set
            {
                if (value != _filtros)
                {
                    _filtros = value;
                    OnPropertyChanged(nameof(Filtros));
                }
            }
        }

        #endregion Propriedades
    }
}