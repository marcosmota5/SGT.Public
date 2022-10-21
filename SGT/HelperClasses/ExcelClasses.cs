using Microsoft.Office.Interop.Excel;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.Funcoes;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SGT.HelperClasses
{
    public class ExcelClasses
    {
        public async Task ImportarCotacao(string caminhoArquivoCotacao, Fornecedor fornecedor, Cliente cliente, ObservableCollection<ItemProposta> listaItensProposta, IProgress<double>? reportadorProgresso, CancellationToken ct)
        {
            ObservableCollection<ItemProposta> listaItensImportados = new();

            await Task.Run(async () =>
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                string colunaItem = "";
                ObservableCollection<ColunaImportacaoCotacao> listaColunasImportacaoCotacao = new();

                await ColunaImportacaoCotacao.PreencheListaColunasImportacaoCotacaoAsync(listaColunasImportacaoCotacao, true, reportadorProgresso, ct,
                    "WHERE forn.id_fornecedor = @id_fornecedor", "@id_fornecedor", fornecedor.Id);

                ColunaImportacaoCotacao colunaSistema = new();
                ColunaImportacaoCotacao colunaReferencia = new();
                string colunaReferenciaReal = "";

                Application elApp = new() { Visible = false };
                Workbook elWorkbook;
                Worksheet elWorksheet;
                Microsoft.Office.Interop.Excel.Range elRange;
                Microsoft.Office.Interop.Excel.Range elRangeHeaders;
                QueryTable elQueryTable;

                switch (fornecedor.Id)
                {
                    case 1:
                        elWorkbook = elApp.Workbooks.Open(caminhoArquivoCotacao, false, false);
                        elWorksheet = elWorkbook.Worksheets[1] as Worksheet;
                        elRangeHeaders = elWorksheet.Range["A22:IV22"];
                        colunaReferencia = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "codigo_item").First();
                        colunaReferenciaReal = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaReferencia.NomeColunaCotacao));
                        elRange = elWorksheet.Range["A23:IV" + elWorksheet.Range[colunaReferenciaReal + "65536"].End[XlDirection.xlUp].Row.ToString()];

                        break;

                    case 2:
                        elWorkbook = elApp.Workbooks.Add();
                        elWorksheet = elWorkbook.Worksheets[1] as Worksheet;

                        elQueryTable = elWorksheet.QueryTables.Add("URL;" + caminhoArquivoCotacao, elWorksheet.Range["A1"]);
                        elQueryTable.Name = "PedidoPeca";
                        elQueryTable.FieldNames = true;
                        elQueryTable.RowNumbers = false;
                        elQueryTable.EnableEditing = true;
                        elQueryTable.FillAdjacentFormulas = true;
                        elQueryTable.PreserveFormatting = true;
                        elQueryTable.RefreshOnFileOpen = false;
                        elQueryTable.BackgroundQuery = true;
                        elQueryTable.RefreshStyle = XlCellInsertionMode.xlInsertDeleteCells;
                        elQueryTable.SavePassword = false;
                        elQueryTable.SaveData = true;
                        elQueryTable.AdjustColumnWidth = true;
                        elQueryTable.RefreshPeriod = 0;
                        elQueryTable.WebSelectionType = XlWebSelectionType.xlAllTables;
                        elQueryTable.WebFormatting = XlWebFormatting.xlWebFormattingAll;
                        elQueryTable.WebPreFormattedTextToColumns = true;
                        elQueryTable.WebConsecutiveDelimitersAsOne = true;
                        elQueryTable.WebSingleBlockTextImport = false;
                        elQueryTable.WebDisableDateRecognition = false;
                        elQueryTable.WebDisableRedirections = false;
                        elQueryTable.Refresh(false);

                        elRangeHeaders = elWorksheet.Range["A17:IV17"];

                        foreach (Microsoft.Office.Interop.Excel.Range elColHeader in elRangeHeaders.Columns)
                        {
                            ct.ThrowIfCancellationRequested();
                            elColHeader.Formula = "=TRIM(TRIM(" + ConverterColunaNumericaParaLetra(elColHeader.Column) + "19)&\" \"&TRIM(" + ConverterColunaNumericaParaLetra(elColHeader.Column) +
                                "20)&\" \"&TRIM(" + ConverterColunaNumericaParaLetra(elColHeader.Column) + "21))";
                        }

                        elWorksheet.Calculate();
                        elRangeHeaders.Copy();
                        elRangeHeaders.PasteSpecial(XlPasteType.xlPasteValues);
                        elWorksheet.Application.CutCopyMode = 0;

                        colunaReferencia = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "codigo_item").First();
                        colunaReferenciaReal = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaReferencia.NomeColunaCotacao));

                        elRange = elWorksheet.Range["A22:IV" + elWorksheet.Range[colunaReferenciaReal + "65536"].End[XlDirection.xlUp].Row.ToString()];

                        break;

                    default:
                        elWorkbook = elApp.Workbooks.Open(caminhoArquivoCotacao, false, false);
                        elWorksheet = elWorkbook.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                        elRangeHeaders = elWorksheet.Range["A22:IV22"];
                        colunaReferencia = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "codigo_item").First();
                        colunaReferenciaReal = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaReferencia.NomeColunaCotacao));
                        elRange = elWorksheet.Range["A23:IV" + elWorksheet.Range[colunaReferenciaReal + "65536"].End[XlDirection.xlUp].Row.ToString()];

                        break;
                }

                try
                {
                    // Cria e atribui a variável do total de linhas através da função específica para contagem de linhas
                    int totalLinhas = elRange.Rows.Count;

                    // Cria e atribui a variável de contagem de linhas
                    int linhaAtual = 0;

                    foreach (Microsoft.Office.Interop.Excel.Range elRow in elRange.Rows)
                    {
                        // Lança exceção de cancelamento caso ela tenha sido efetuada
                        ct.ThrowIfCancellationRequested();

                        ItemProposta itemProposta = new(inicializarDemaisItens: true);

                        itemProposta.Id = null;
                        itemProposta.DataInsercao = DateTime.Now;

                        colunaSistema = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "codigo_item").First();
                        colunaItem = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaSistema.NomeColunaCotacao));

                        if ((bool)colunaSistema.ColunaExiste)
                        {
                            itemProposta.CodigoItem = Convert.ToString(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value).Trim();
                        }
                        else
                        {
                            itemProposta.CodigoItem = "";
                        }

                        colunaSistema = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "descricao_item").First();
                        colunaItem = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaSistema.NomeColunaCotacao));

                        if ((bool)colunaSistema.ColunaExiste)
                        {
                            itemProposta.DescricaoItem = Convert.ToString(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value).Trim();
                        }
                        else
                        {
                            itemProposta.DescricaoItem = "";
                        }

                        colunaSistema = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "quantidade_item").First();
                        colunaItem = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaSistema.NomeColunaCotacao));

                        if ((bool)colunaSistema.ColunaExiste)
                        {
                            itemProposta.QuantidadeItem = Convert.ToDecimal(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value);
                        }
                        else
                        {
                            itemProposta.QuantidadeItem = 0;
                        }

                        colunaSistema = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "preco_unitario_inicial_item").First();
                        colunaItem = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaSistema.NomeColunaCotacao));

                        if ((bool)colunaSistema.ColunaExiste)
                        {
                            itemProposta.PrecoUnitarioInicialItem = Convert.ToDecimal(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value);
                        }
                        else
                        {
                            itemProposta.PrecoUnitarioInicialItem = 0;
                        }

                        colunaSistema = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "percentual_ipi_item").First();
                        colunaItem = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaSistema.NomeColunaCotacao));

                        if ((bool)colunaSistema.ColunaExiste)
                        {
                            if (elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value.ToString().Contains("."))
                            {
                                decimal percentualIpiItem = Convert.ToDecimal(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value.ToString().Replace(".", ",").Replace("%", ""));
                                itemProposta.PercentualIpiItem = FuncoesMatematicas.DividirPorZeroDecimal(percentualIpiItem, percentualIpiItem >= 1 ? 100 : 1);
                            }
                            else
                            {
                                itemProposta.PercentualIpiItem = FuncoesMatematicas.DividirPorZeroDecimal(Convert.ToDecimal(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value), fornecedor.Id == 1 ? 100 : 1);
                            }

                            //itemProposta.PercentualIpiItem = FuncoesMatematicas.DividirPorZeroDecimal(Convert.ToDecimal(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value), fornecedor.Id == 1 ? 100 : 1);
                        }
                        else
                        {
                            itemProposta.PercentualIpiItem = 0;
                        }

                        colunaSistema = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "percentual_icms_item").First();
                        colunaItem = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaSistema.NomeColunaCotacao));

                        if ((bool)colunaSistema.ColunaExiste)
                        {
                            if (elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value.ToString().Contains("."))
                            {
                                decimal percentualIcmsItem = Convert.ToDecimal(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value.ToString().Replace(".", ",").Replace("%", ""));
                                itemProposta.PercentualIcmsItem = FuncoesMatematicas.DividirPorZeroDecimal(percentualIcmsItem, percentualIcmsItem >= 1 ? 100 : 1);
                            }
                            else
                            {
                                itemProposta.PercentualIcmsItem = FuncoesMatematicas.DividirPorZeroDecimal(Convert.ToDecimal(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value), fornecedor.Id == 1 ? 100 : 1);
                            }

                            //itemProposta.PercentualIcmsItem = FuncoesMatematicas.DividirPorZeroDecimal(Convert.ToDecimal(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value), fornecedor.Id == 1 ? 100 : 1);
                        }
                        else
                        {
                            itemProposta.PercentualIcmsItem = 0;
                        }

                        colunaSistema = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "ncm_item").First();
                        colunaItem = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaSistema.NomeColunaCotacao));

                        if ((bool)colunaSistema.ColunaExiste)
                        {
                            itemProposta.NcmItem = Convert.ToString(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value).Trim();
                        }
                        else
                        {
                            itemProposta.NcmItem = "";
                        }

                        colunaSistema = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "mva_item").First();
                        colunaItem = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaSistema.NomeColunaCotacao));

                        if ((bool)colunaSistema.ColunaExiste)
                        {
                            itemProposta.MvaItem = Convert.ToDecimal(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value);
                        }
                        else
                        {
                            itemProposta.MvaItem = 0;
                        }

                        colunaSistema = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "valor_st_item").First();
                        colunaItem = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaSistema.NomeColunaCotacao));

                        if ((bool)colunaSistema.ColunaExiste)
                        {
                            itemProposta.ValorStItem = Convert.ToDecimal(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value);
                        }
                        else
                        {
                            itemProposta.ValorStItem = 0;
                        }

                        colunaSistema = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "valor_total_inicial_item").First();
                        colunaItem = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaSistema.NomeColunaCotacao));

                        if ((bool)colunaSistema.ColunaExiste)
                        {
                            itemProposta.ValorTotalInicialItem = Convert.ToDecimal(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value);
                        }
                        else
                        {
                            itemProposta.ValorTotalInicialItem = 0;
                        }

                        colunaSistema = listaColunasImportacaoCotacao.Where(coluna => coluna.NomeColunaSistema == "prazo_inicial_item").First();
                        colunaItem = ConverterColunaNumericaParaLetra(RetornaColunaExcel(elRangeHeaders, colunaSistema.NomeColunaCotacao));

                        if ((bool)colunaSistema.ColunaExiste)
                        {
                            switch (fornecedor.Id)
                            {
                                case 1:
                                    itemProposta.PrazoInicialItem = ((string)elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value).Trim();
                                    break;

                                case 2:
                                    if (Convert.ToDateTime(Convert.ToString(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value).Trim()) == Convert.ToDateTime(Convert.ToString(elWorksheet.Range["G18"].Value).Trim()))
                                    {
                                        itemProposta.PrazoInicialItem = "Imediato";
                                    }
                                    else
                                    {
                                        itemProposta.PrazoInicialItem = "Qtd: " + Convert.ToInt32(itemProposta.QuantidadeItem).ToString() + " - " + Convert.ToDateTime(Convert.ToString(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value).Trim()).ToString("dd/MM/yyyy");
                                    }
                                    break;

                                default:
                                    itemProposta.PrazoInicialItem = Convert.ToString(elWorksheet.Range[colunaItem + elRow.Row.ToString()].Value).Trim();
                                    break;
                            }
                        }
                        else
                        {
                            itemProposta.PrazoInicialItem = "";
                        }

                        itemProposta.FreteUnitarioItem = 0;
                        itemProposta.DescontoInicialItem = null;
                        itemProposta.PrecoAposDescontoInicialItem = null;
                        itemProposta.PrecoComIpiEIcmsItem = null;
                        itemProposta.PercentualAliquotaExternaIcmsItem = null;
                        itemProposta.ValorIcmsCreditoItem = null;
                        itemProposta.PercentualMvaItem = null;
                        itemProposta.ValorMvaItem = null;
                        itemProposta.PrecoComMvaItem = null;
                        itemProposta.PercentualAliquotaInternaIcmsItem = null;
                        itemProposta.ValorIcmsDebitoItem = null;
                        itemProposta.SaldoIcmsItem = null;
                        itemProposta.PrecoUnitarioParaRevendedorItem = null;
                        itemProposta.ImpostosFederaisItem = null;
                        itemProposta.RateioDespesaFixaItem = null;
                        itemProposta.PercentualLucroNecessarioItem = null;
                        itemProposta.PrecoListaVendaItem = null;
                        itemProposta.DescontoFinalItem = null;
                        itemProposta.PrecoUnitarioFinalItem = null;
                        itemProposta.PrecoTotalFinalItem = null;
                        itemProposta.QuantidadeEstoqueCodigoCompletoItem = null;
                        itemProposta.QuantidadeEstoqueCodigoAbreviadoItem = null;
                        itemProposta.InformacaoEstoqueCodigoCompletoItem = null;
                        itemProposta.InformacaoEstoqueCodigoAbreviadoItem = null;
                        itemProposta.PrazoFinalItem = null;
                        itemProposta.ComentariosItem = null;
                        itemProposta.DataAprovacaoItem = null;
                        itemProposta.DataEdicaoItem = null;

                        itemProposta.Usuario = (Usuario)App.Usuario.Clone();

                        await itemProposta.Status.GetStatusDatabaseAsync(1, ct);
                        await itemProposta.StatusAprovacao.GetStatusAprovacaoDatabaseAsync(3, ct);
                        await itemProposta.JustificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(1, ct);
                        await itemProposta.TipoItem.GetTipoItemDatabaseAsync(1, ct);
                        await itemProposta.Fornecedor.GetFornecedorDatabaseAsync(fornecedor.Id, ct);
                        await itemProposta.TipoSubstituicaoTributaria.GetTipoSubstituicaoTributariaDatabaseAsync(1, ct);

                        switch (fornecedor.Id)
                        {
                            case 1:
                                if (itemProposta.CodigoItem.StartsWith("I"))
                                {
                                    await itemProposta.Origem.GetOrigemDatabaseAsync(2, ct);
                                }
                                else
                                {
                                    await itemProposta.Origem.GetOrigemDatabaseAsync(1, ct);
                                }
                                break;

                            case 2:
                                if (itemProposta.PercentualIcmsItem == Convert.ToDecimal(0.04))
                                {
                                    await itemProposta.Origem.GetOrigemDatabaseAsync(2, ct);
                                }
                                else
                                {
                                    await itemProposta.Origem.GetOrigemDatabaseAsync(1, ct);
                                }
                                break;

                            default:
                                if (itemProposta.CodigoItem.StartsWith("I"))
                                {
                                    await itemProposta.Origem.GetOrigemDatabaseAsync(2, ct);
                                }
                                else
                                {
                                    await itemProposta.Origem.GetOrigemDatabaseAsync(1, ct);
                                }
                                break;
                        }

                        await itemProposta.CalculaValoresItemPropostaAsync(cliente);

                        listaItensImportados.Add(itemProposta);

                        // Incrementa a linha atual
                        linhaAtual++;

                        // Reporta o progresso se o progresso não for nulo
                        if (reportadorProgresso != null)
                        {
                            reportadorProgresso.Report((double)linhaAtual / (double)totalLinhas * (double)100);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    elWorkbook.Close(false);
                    elApp.Quit();

                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(elApp);
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(elWorkbook);

                    Process[] excelProcesses = Process.GetProcessesByName("excel");
                    foreach (Process p in excelProcesses)
                    {
                        if (string.IsNullOrEmpty(p.MainWindowTitle)) // use MainWindowTitle to distinguish this excel process with other excel processes
                        {
                            p.Kill();
                        }
                    }
                }

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();
            }, ct);

            foreach (ItemProposta itemProposta in listaItensImportados)
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                listaItensProposta.Add(itemProposta);
            }
        }

        public enum TipoInformacaoEstoque
        {
            CodigoCompleto,
            CodigoAbreviado,
            PorPeca
        }

        public async Task<ObservableCollection<ValidacaoItemPropostaEstoque>> AtualizarEstoque(TipoInformacaoEstoque tipoInformacaoEstoque, ObservableCollection<ItemProposta> listaItensProposta, string arquivoEstoque,
            IProgress<double>? reportadorProgresso, CancellationToken ct)
        {
            // Declara a lista de itens da proposta a serem validados
            ObservableCollection<ValidacaoItemPropostaEstoque> listaValidacaoItemPropostaEstoque = new();

            await Task.Run(() =>
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                Application elApp = new() { Visible = false, EnableAnimations = false, ScreenUpdating = false, DisplayAlerts = false };
                Workbook elWorkbook;
                Worksheet elWorksheet;
                Microsoft.Office.Interop.Excel.Range elRange;
                Microsoft.Office.Interop.Excel.Range elRangeFind;

                try
                {
                    elWorkbook = elApp.Workbooks.Open(arquivoEstoque, false, true, Password: App.ConfiguracoesGerais.SenhaArquivoEstoque);
                }
                catch (Exception)
                {
                    throw;
                }

                elWorksheet = elWorkbook.Worksheets[1];
                foreach (Worksheet tempWorksheet in elWorkbook.Worksheets)
                {
                    // Lança exceção de cancelamento caso ela tenha sido efetuada
                    ct.ThrowIfCancellationRequested();

                    if (tempWorksheet.Name.Contains(App.ConfiguracoesGerais.NomeAbaArquivoEstoque))
                    {
                        elWorksheet = tempWorksheet;
                        break;
                    }
                }

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                try
                {
                    if (!elWorksheet.Name.Contains(App.ConfiguracoesGerais.NomeAbaArquivoEstoque))
                    {
                        throw new System.IO.FileNotFoundException();
                    }

                    elRange = elWorksheet.Range["A1:IV65536"];

                    try
                    {
                        elWorksheet.ShowAllData();
                    }
                    catch (Exception)
                    {
                    }

                    // Lança exceção de cancelamento caso ela tenha sido efetuada
                    ct.ThrowIfCancellationRequested();

                    string primeiroEnderecoEncontrado = "";
                    int? idFornecedorEncontrado = 0;
                    decimal quantidadeEstoqueCodigoCompleto = 0;
                    decimal quantidadeEstoqueCodigoAbreviado = 0;
                    string informacaoEstoqueCodigoCompleto = "";
                    string informacaoEstoqueCodigoAbreviado = "";
                    string textoEncontradoCodigoAbreviado = "";
                    decimal quantidadeUsadaParaOCalculoDoPrazo = 0;

                    // Cria e atribui a variável do total de linhas
                    int totalLinhas = listaItensProposta.Count;

                    // Cria e atribui a variável de contagem de linhas
                    int linhaAtual = 0;

                    foreach (ItemProposta itemProposta in listaItensProposta)
                    {
                        // Lança exceção de cancelamento caso ela tenha sido efetuada
                        ct.ThrowIfCancellationRequested();

                        if (itemProposta.TipoItem.Id == 1)
                        {
                            // Retorna quantidade em estoque pelo código completo ou na opção decidir por peça
                            if (tipoInformacaoEstoque != TipoInformacaoEstoque.CodigoAbreviado)
                            {
                                quantidadeEstoqueCodigoCompleto = 0;
                                informacaoEstoqueCodigoCompleto = "-";
                                elRangeFind = elRange.Find(itemProposta.CodigoItem.Trim(), LookIn: XlFindLookIn.xlValues, LookAt: XlLookAt.xlPart);

                                if (elRangeFind != null)
                                {
                                    primeiroEnderecoEncontrado = elRangeFind.Address;
                                    do
                                    {
                                        switch (FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["D" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim())
                                        {
                                            case "STILL":
                                                idFornecedorEncontrado = 1;
                                                break;

                                            case "LINDE":
                                                idFornecedorEncontrado = 1;
                                                break;

                                            case "TVH":
                                                idFornecedorEncontrado = 2;
                                                break;

                                            default:
                                                idFornecedorEncontrado = 0;
                                                break;
                                        }

                                        if (idFornecedorEncontrado == itemProposta.Fornecedor.Id)
                                        {
                                            bool conversaoComSucesso = decimal.TryParse(FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["H" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim(), out decimal quantidadeEncontrada);
                                            quantidadeEstoqueCodigoCompleto += quantidadeEncontrada;
                                        }

                                        if (String.IsNullOrEmpty(informacaoEstoqueCodigoCompleto) || informacaoEstoqueCodigoCompleto == "-")
                                        {
                                            informacaoEstoqueCodigoCompleto = FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["D" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim() + ": " +
                                            FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["H" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim() + ";";
                                        }
                                        else
                                        {
                                            informacaoEstoqueCodigoCompleto = informacaoEstoqueCodigoCompleto + "\n" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["D" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim() + ": " +
                                            FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["H" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim() + ";";
                                        }

                                        elRangeFind.FindNext(elRangeFind);
                                    } while (elRangeFind != null && elRangeFind.Address != primeiroEnderecoEncontrado);
                                }
                            }

                            // Retorna quantidade em estoque pelo código abreviado ou na opção decidir por peça
                            if (tipoInformacaoEstoque != TipoInformacaoEstoque.CodigoCompleto)
                            {
                                quantidadeEstoqueCodigoAbreviado = 0;
                                informacaoEstoqueCodigoAbreviado = "-";
                                elRangeFind = elRange.Find(Regex.Match(itemProposta.CodigoItem.Trim(), @"[1-9]+[0-9]*").Value, LookIn: XlFindLookIn.xlValues, LookAt: XlLookAt.xlPart);

                                if (elRangeFind != null)
                                {
                                    primeiroEnderecoEncontrado = elRangeFind.Address;
                                    do
                                    {
                                        switch (FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["D" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim())
                                        {
                                            case "STILL":
                                                idFornecedorEncontrado = 1;
                                                break;

                                            case "LINDE":
                                                idFornecedorEncontrado = 1;
                                                break;

                                            case "TVH":
                                                idFornecedorEncontrado = 2;
                                                break;

                                            default:
                                                idFornecedorEncontrado = 0;
                                                break;
                                        }

                                        if (idFornecedorEncontrado == itemProposta.Fornecedor.Id)
                                        {
                                            bool conversaoComSucesso = decimal.TryParse(FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["H" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim(), out decimal quantidadeEncontrada);
                                            quantidadeEstoqueCodigoAbreviado += quantidadeEncontrada;

                                            if (ConverterColunaNumericaParaLetra(elRangeFind.Column) == "L")
                                            {
                                                textoEncontradoCodigoAbreviado = FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["D" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim() + " - " +
                                                FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Value2).Trim();
                                            }
                                            else
                                            {
                                                textoEncontradoCodigoAbreviado = FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["D" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim() + " - " +
                                                FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Value2).Trim() + " - " + FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["L" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim();
                                            }
                                        }

                                        if (String.IsNullOrEmpty(informacaoEstoqueCodigoAbreviado) || informacaoEstoqueCodigoAbreviado == "-")
                                        {
                                            informacaoEstoqueCodigoAbreviado = FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["D" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim() + ": " +
                                            FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["H" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim() + ";";
                                        }
                                        else
                                        {
                                            informacaoEstoqueCodigoAbreviado = informacaoEstoqueCodigoAbreviado + "\n" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["D" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim() + ": " +
                                            FuncoesDeConversao.ConverteValorParaStringFailSafe(elWorksheet.Range["H" + FuncoesDeConversao.ConverteValorParaStringFailSafe(elRangeFind.Row)].Value2).Trim() + ";";
                                        }

                                        elRangeFind.FindNext(elRangeFind);
                                    } while (elRangeFind != null && elRangeFind.Address != primeiroEnderecoEncontrado);
                                }
                            }

                            if (tipoInformacaoEstoque == TipoInformacaoEstoque.CodigoAbreviado)
                            {
                                quantidadeUsadaParaOCalculoDoPrazo = quantidadeEstoqueCodigoAbreviado;
                            }
                            else
                            {
                                if (quantidadeEstoqueCodigoCompleto > 0)
                                {
                                    quantidadeUsadaParaOCalculoDoPrazo = quantidadeEstoqueCodigoCompleto;
                                }
                                else
                                {
                                    quantidadeUsadaParaOCalculoDoPrazo = quantidadeEstoqueCodigoCompleto;

                                    if (quantidadeEstoqueCodigoAbreviado > 0)
                                    {
                                        // Adiciona o item encontrado a lista de itens a serem validados posteriormente
                                        listaValidacaoItemPropostaEstoque.Add(new ValidacaoItemPropostaEstoque(itemProposta,
                                            String.Join(" - ", itemProposta.Fornecedor.Nome, itemProposta.CodigoItem, itemProposta.DescricaoItem), textoEncontradoCodigoAbreviado,
                                            quantidadeEstoqueCodigoAbreviado, false));
                                    }
                                }
                            }

                            itemProposta.QuantidadeEstoqueCodigoCompletoItem = quantidadeEstoqueCodigoCompleto;
                            itemProposta.QuantidadeEstoqueCodigoAbreviadoItem = quantidadeEstoqueCodigoAbreviado;
                            itemProposta.InformacaoEstoqueCodigoCompletoItem = informacaoEstoqueCodigoCompleto;
                            itemProposta.InformacaoEstoqueCodigoAbreviadoItem = informacaoEstoqueCodigoAbreviado;
                            itemProposta.PrazoFinalItem = ExcelClasses.RetornaPrazo(itemProposta.QuantidadeItem == null ? 0 : (decimal)itemProposta.QuantidadeItem, itemProposta.PrazoInicialItem == null ? "" : itemProposta.PrazoInicialItem.Replace("*", "").Trim(),
                                quantidadeUsadaParaOCalculoDoPrazo, App.Usuario.Setor.PrazoAdicional == null ? 0 : (int)App.Usuario.Setor.PrazoAdicional);
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(itemProposta.PrazoFinalItem))
                            {
                                itemProposta.PrazoFinalItem = itemProposta.PrazoInicialItem;
                            }
                        }

                        //await System.Windows.Application.Current.Dispatcher.BeginInvoke((System.Action)(() =>
                        //{
                        // Incrementa a linha atual
                        linhaAtual++;

                        // Reporta o progresso se o progresso não for nulo
                        if (reportadorProgresso != null)
                        {
                            reportadorProgresso.Report((double)linhaAtual / (double)totalLinhas * (double)100);
                        }

                        // Lança exceção de cancelamento caso ela tenha sido efetuada
                        ct.ThrowIfCancellationRequested();

                        //}));
                        //await AtualizaProgresso(progresso);
                    }
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    elWorkbook.Close(false);

                    elApp.EnableAnimations = true;
                    elApp.ScreenUpdating = true;
                    elApp.Quit();

                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(elApp);
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(elWorkbook);

                    Process[] excelProcesses = Process.GetProcessesByName("excel");
                    foreach (Process p in excelProcesses)
                    {
                        if (string.IsNullOrEmpty(p.MainWindowTitle)) // use MainWindowTitle to distinguish this excel process with other excel processes
                        {
                            p.Kill();
                        }
                    }
                }
            });

            return listaValidacaoItemPropostaEstoque;
            //}));
        }

        public enum TipoExportacaoItensProposta
        {
            FormatoFluig,
            TabelaCompleta
        }

        public static async Task ExportarItensProposta(TipoExportacaoItensProposta tipoExportacaoItensProposta, Proposta proposta, ObservableCollection<ItemProposta> listaItensProposta, IProgress<double>? reportadorProgresso, CancellationToken ct)
        {
            await Task.Run(async () =>
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                string nomeInicial = "";
                string nomeWorksheet = "";
                string caminhoArquivo = "";

                if (tipoExportacaoItensProposta == TipoExportacaoItensProposta.FormatoFluig)
                {
                    nomeInicial = "importacao";
                    nomeWorksheet = "importacao1";
                }
                else
                {
                    nomeInicial = "Itens_Proposta_" + proposta.CodigoProposta + "_" + proposta.Cliente.Nome;
                    nomeWorksheet = "Itens_Proposta";
                }

                VistaSaveFileDialog vistaSaveFileDialog = new VistaSaveFileDialog()
                {
                    Filter = "Arquivo do Excel (*.xlsx)|*.xlsx",
                    Title = "Informe o local e o nome do arquivo",
                    FileName = nomeInicial
                };

                if ((bool)vistaSaveFileDialog.ShowDialog())
                {
                    caminhoArquivo = vistaSaveFileDialog.FileName;
                }

                if (String.IsNullOrEmpty(caminhoArquivo))
                {
                    throw new OperationCanceledException();
                }

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                Application elApp = new() { Visible = false, DisplayAlerts = false };
                Workbook elWorkbook;
                Worksheet elWorksheet;
                elWorkbook = elApp.Workbooks.Add();
                elWorksheet = elWorkbook.Worksheets[1];
                elWorksheet.Name = nomeWorksheet;

                try
                {
                    if (tipoExportacaoItensProposta == TipoExportacaoItensProposta.FormatoFluig)
                    {
                        elWorksheet.Range["A1"].Value = "item";
                        elWorksheet.Range["B1"].Value = "quantidade";
                    }
                    else
                    {
                        elWorksheet.Range["A1"].Value = "Data de inserção";
                        elWorksheet.Range["B1"].Value = "Tipo do item";
                        elWorksheet.Range["C1"].Value = "Fornecedor";
                        elWorksheet.Range["D1"].Value = "Código";
                        elWorksheet.Range["E1"].Value = "Descrição";
                        elWorksheet.Range["F1"].Value = "Quantidade";
                        elWorksheet.Range["G1"].Value = "Preço unitário inicial";
                        elWorksheet.Range["H1"].Value = "% IPI";
                        elWorksheet.Range["I1"].Value = "% ICMS";
                        elWorksheet.Range["J1"].Value = "NCM";
                        elWorksheet.Range["K1"].Value = "MVA";
                        elWorksheet.Range["L1"].Value = "Valor ST";
                        elWorksheet.Range["M1"].Value = "Preço total inicial";
                        elWorksheet.Range["N1"].Value = "Prazo inicial";
                        elWorksheet.Range["O1"].Value = "Frete unitário";
                        elWorksheet.Range["P1"].Value = "Desconto no custo";
                        elWorksheet.Range["Q1"].Value = "Preço unitário após desconto inicial";
                        elWorksheet.Range["R1"].Value = "Valor com IPI e ICMS";
                        elWorksheet.Range["S1"].Value = "Alíquota externa ICMS";
                        elWorksheet.Range["T1"].Value = "Valor do ICMS crédito";
                        elWorksheet.Range["U1"].Value = "Percentual do MVA";
                        elWorksheet.Range["V1"].Value = "Valor do MVA";
                        elWorksheet.Range["W1"].Value = "Valor do item + MVA";
                        elWorksheet.Range["X1"].Value = "Alíquota interna do ICMS";
                        elWorksheet.Range["Y1"].Value = "Valor do ICMS débito";
                        elWorksheet.Range["Z1"].Value = "Saldo do ICMS";
                        elWorksheet.Range["AA1"].Value = "Total com imposto para o revendedor";
                        elWorksheet.Range["AB1"].Value = "Impostos federais";
                        elWorksheet.Range["AC1"].Value = "Rateio despesa fixa";
                        elWorksheet.Range["AD1"].Value = "% Lucro necessário";
                        elWorksheet.Range["AE1"].Value = "Preço lista de venda";
                        elWorksheet.Range["AF1"].Value = "Desconto final";
                        elWorksheet.Range["AG1"].Value = "Preço unitário final com desconto";
                        elWorksheet.Range["AH1"].Value = "Preço total final com desconto";
                        elWorksheet.Range["AI1"].Value = "Quantidade em estoque (código completo)";
                        elWorksheet.Range["AJ1"].Value = "Quantidade em estoque (código abreviado)";
                        elWorksheet.Range["AK1"].Value = "Informação do estoque (código completo)";
                        elWorksheet.Range["AL1"].Value = "Informação do estoque (código abreviado)";
                        elWorksheet.Range["AM1"].Value = "Prazo para o cliente";
                        elWorksheet.Range["AN1"].Value = "Data da aprovação";
                        elWorksheet.Range["AO1"].Value = "Data de edição";
                        elWorksheet.Range["AP1"].Value = "Usuário";
                        elWorksheet.Range["AQ1"].Value = "Tipo de substituição tributária";
                        elWorksheet.Range["AR1"].Value = "Origem";
                        elWorksheet.Range["AS1"].Value = "Status da aprovação";
                        elWorksheet.Range["AT1"].Value = "Justificativa da aprovação";
                        elWorksheet.Range["AU1"].Value = "Conjunto";
                        elWorksheet.Range["AV1"].Value = "Especificação";
                        elWorksheet.Range["AW1"].Value = "Comentários";
                        elWorksheet.Range["AX1"].Value = "Status";

                        elWorksheet.Range["F:F"].NumberFormat = "#,##0";
                        elWorksheet.Range["G:G"].NumberFormat = "$ #,##0.00";
                        elWorksheet.Range["H:I"].NumberFormat = "0.00%";
                        elWorksheet.Range["K:K"].NumberFormat = "#,##0.00";
                        elWorksheet.Range["L:O"].NumberFormat = "$ #,##0.00";
                        elWorksheet.Range["P:P"].NumberFormat = "0.00%";
                        elWorksheet.Range["Q:R"].NumberFormat = "$ #,##0.00";
                        elWorksheet.Range["S:S"].NumberFormat = "0.00%";
                        elWorksheet.Range["T:T"].NumberFormat = "$ #,##0.00";
                        elWorksheet.Range["U:U"].NumberFormat = "0.00%";
                        elWorksheet.Range["V:W"].NumberFormat = "$ #,##0.00";
                        elWorksheet.Range["X:X"].NumberFormat = "0.00%";
                        elWorksheet.Range["Y:Z"].NumberFormat = "$ #,##0.00";
                        elWorksheet.Range["AA:AA"].NumberFormat = "$ #,##0.00";
                        elWorksheet.Range["AB:AD"].NumberFormat = "0.00%";
                        elWorksheet.Range["AE:AE"].NumberFormat = "$ #,##0.00";
                        elWorksheet.Range["AF:AF"].NumberFormat = "0.00%";
                        elWorksheet.Range["AG:AH"].NumberFormat = "$ #,##0.00";
                        elWorksheet.Range["AI:AJ"].NumberFormat = "#,##0";
                    }

                    int linhaItem = 2;

                    // Cria e atribui a variável de contagem de linhas
                    int linhaAtual = 0;

                    // Cria e atribui a variável do total de linhas
                    int totalLinhas = listaItensProposta.Count;

                    // Lança exceção de cancelamento caso ela tenha sido efetuada
                    ct.ThrowIfCancellationRequested();

                    foreach (ItemProposta itemProposta in listaItensProposta)
                    {
                        if (tipoExportacaoItensProposta == TipoExportacaoItensProposta.FormatoFluig)
                        {
                            if (itemProposta.Fornecedor.Id == 1)
                            {
                                elWorksheet.Range["A" + linhaItem.ToString()].Value = itemProposta.CodigoItem == null ? "" : itemProposta.CodigoItem;
                                elWorksheet.Range["B" + linhaItem.ToString()].Value = itemProposta.QuantidadeItem == null ? 0 : itemProposta.QuantidadeItem;
                                linhaItem++;
                            }
                        }
                        else
                        {
                            elWorksheet.Range["A" + linhaItem.ToString()].Value = itemProposta.DataInsercao == null ? "" : itemProposta.DataInsercao;
                            elWorksheet.Range["B" + linhaItem.ToString()].Value = itemProposta.TipoItem == null ? "" : itemProposta.TipoItem.Nome;
                            elWorksheet.Range["C" + linhaItem.ToString()].Value = itemProposta.Fornecedor == null ? "" : itemProposta.Fornecedor.Nome;
                            elWorksheet.Range["D" + linhaItem.ToString()].Value = itemProposta.DescricaoItem == null ? "" : itemProposta.DescricaoItem;
                            elWorksheet.Range["E" + linhaItem.ToString()].Value = itemProposta.CodigoItem == null ? "" : itemProposta.CodigoItem;
                            elWorksheet.Range["F" + linhaItem.ToString()].Value = itemProposta.QuantidadeItem == null ? 0 : itemProposta.QuantidadeItem;
                            elWorksheet.Range["G" + linhaItem.ToString()].Value = itemProposta.PrecoUnitarioInicialItem == null ? 0 : itemProposta.PrecoUnitarioInicialItem;
                            elWorksheet.Range["H" + linhaItem.ToString()].Value = itemProposta.PercentualIpiItem == null ? 0 : itemProposta.PercentualIpiItem;
                            elWorksheet.Range["I" + linhaItem.ToString()].Value = itemProposta.PercentualIcmsItem == null ? 0 : itemProposta.PercentualIcmsItem;
                            elWorksheet.Range["J" + linhaItem.ToString()].Value = itemProposta.NcmItem == null ? "" : itemProposta.NcmItem;
                            elWorksheet.Range["K" + linhaItem.ToString()].Value = itemProposta.MvaItem == null ? 0 : itemProposta.MvaItem;
                            elWorksheet.Range["L" + linhaItem.ToString()].Value = itemProposta.ValorStItem == null ? 0 : itemProposta.ValorStItem;
                            elWorksheet.Range["M" + linhaItem.ToString()].Value = itemProposta.ValorTotalInicialItem == null ? 0 : itemProposta.ValorTotalInicialItem;
                            elWorksheet.Range["N" + linhaItem.ToString()].Value = itemProposta.PrazoInicialItem == null ? "" : itemProposta.PrazoInicialItem;
                            elWorksheet.Range["O" + linhaItem.ToString()].Value = itemProposta.FreteUnitarioItem == null ? 0 : itemProposta.FreteUnitarioItem;
                            elWorksheet.Range["P" + linhaItem.ToString()].Value = itemProposta.DescontoInicialItem == null ? 0 : itemProposta.DescontoInicialItem;
                            elWorksheet.Range["Q" + linhaItem.ToString()].Value = itemProposta.PrecoAposDescontoInicialItem == null ? 0 : itemProposta.PrecoAposDescontoInicialItem;
                            elWorksheet.Range["R" + linhaItem.ToString()].Value = itemProposta.PrecoComIpiEIcmsItem == null ? 0 : itemProposta.PrecoComIpiEIcmsItem;
                            elWorksheet.Range["S" + linhaItem.ToString()].Value = itemProposta.PercentualAliquotaExternaIcmsItem == null ? 0 : itemProposta.PercentualAliquotaExternaIcmsItem;
                            elWorksheet.Range["T" + linhaItem.ToString()].Value = itemProposta.ValorIcmsCreditoItem == null ? 0 : itemProposta.ValorIcmsCreditoItem;
                            elWorksheet.Range["U" + linhaItem.ToString()].Value = itemProposta.PercentualMvaItem == null ? 0 : itemProposta.PercentualMvaItem;
                            elWorksheet.Range["V" + linhaItem.ToString()].Value = itemProposta.ValorMvaItem == null ? 0 : itemProposta.ValorMvaItem;
                            elWorksheet.Range["W" + linhaItem.ToString()].Value = itemProposta.PrecoComMvaItem == null ? 0 : itemProposta.PrecoComMvaItem;
                            elWorksheet.Range["X" + linhaItem.ToString()].Value = itemProposta.PercentualAliquotaInternaIcmsItem == null ? 0 : itemProposta.PercentualAliquotaInternaIcmsItem;
                            elWorksheet.Range["Y" + linhaItem.ToString()].Value = itemProposta.ValorIcmsDebitoItem == null ? 0 : itemProposta.ValorIcmsDebitoItem;
                            elWorksheet.Range["Z" + linhaItem.ToString()].Value = itemProposta.SaldoIcmsItem == null ? 0 : itemProposta.SaldoIcmsItem;
                            elWorksheet.Range["AA" + linhaItem.ToString()].Value = itemProposta.PrecoUnitarioParaRevendedorItem == null ? 0 : itemProposta.PrecoUnitarioParaRevendedorItem;
                            elWorksheet.Range["AB" + linhaItem.ToString()].Value = itemProposta.ImpostosFederaisItem == null ? 0 : itemProposta.ImpostosFederaisItem;
                            elWorksheet.Range["AC" + linhaItem.ToString()].Value = itemProposta.RateioDespesaFixaItem == null ? 0 : itemProposta.RateioDespesaFixaItem;
                            elWorksheet.Range["AD" + linhaItem.ToString()].Value = itemProposta.PercentualLucroNecessarioItem == null ? 0 : itemProposta.PercentualLucroNecessarioItem;
                            elWorksheet.Range["AE" + linhaItem.ToString()].Value = itemProposta.PrecoListaVendaItem == null ? 0 : itemProposta.PrecoListaVendaItem;
                            elWorksheet.Range["AF" + linhaItem.ToString()].Value = itemProposta.DescontoFinalItem == null ? 0 : itemProposta.DescontoFinalItem;
                            elWorksheet.Range["AG" + linhaItem.ToString()].Value = itemProposta.PrecoUnitarioFinalItem == null ? 0 : itemProposta.PrecoUnitarioFinalItem;
                            elWorksheet.Range["AH" + linhaItem.ToString()].Value = itemProposta.PrecoTotalFinalItem == null ? 0 : itemProposta.PrecoTotalFinalItem;
                            elWorksheet.Range["AI" + linhaItem.ToString()].Value = itemProposta.QuantidadeEstoqueCodigoCompletoItem == null ? 0 : itemProposta.QuantidadeEstoqueCodigoCompletoItem;
                            elWorksheet.Range["AJ" + linhaItem.ToString()].Value = itemProposta.QuantidadeEstoqueCodigoAbreviadoItem == null ? 0 : itemProposta.QuantidadeEstoqueCodigoAbreviadoItem;
                            elWorksheet.Range["AK" + linhaItem.ToString()].Value = itemProposta.InformacaoEstoqueCodigoCompletoItem == null ? "" : itemProposta.InformacaoEstoqueCodigoCompletoItem;
                            elWorksheet.Range["AL" + linhaItem.ToString()].Value = itemProposta.InformacaoEstoqueCodigoAbreviadoItem == null ? "" : itemProposta.InformacaoEstoqueCodigoAbreviadoItem;
                            elWorksheet.Range["AM" + linhaItem.ToString()].Value = itemProposta.PrazoFinalItem == null ? "" : itemProposta.PrazoFinalItem;
                            elWorksheet.Range["AN" + linhaItem.ToString()].Value = itemProposta.DataAprovacaoItem == null ? "" : itemProposta.DataAprovacaoItem;
                            elWorksheet.Range["AO" + linhaItem.ToString()].Value = itemProposta.DataEdicaoItem == null ? "" : itemProposta.DataEdicaoItem;
                            elWorksheet.Range["AP" + linhaItem.ToString()].Value = itemProposta.Usuario == null ? "" : itemProposta.Usuario.Nome;
                            elWorksheet.Range["AQ" + linhaItem.ToString()].Value = itemProposta.TipoSubstituicaoTributaria == null ? "" : itemProposta.TipoSubstituicaoTributaria.Nome;
                            elWorksheet.Range["AR" + linhaItem.ToString()].Value = itemProposta.Origem == null ? "" : itemProposta.Origem.Nome;
                            elWorksheet.Range["AS" + linhaItem.ToString()].Value = itemProposta.StatusAprovacao == null ? "" : itemProposta.StatusAprovacao.Nome;
                            elWorksheet.Range["AT" + linhaItem.ToString()].Value = itemProposta.JustificativaAprovacao == null ? "" : itemProposta.JustificativaAprovacao.Nome;
                            elWorksheet.Range["AU" + linhaItem.ToString()].Value = itemProposta.Conjunto == null ? "" : itemProposta.Conjunto.Nome;
                            elWorksheet.Range["AV" + linhaItem.ToString()].Value = itemProposta.Especificacao == null ? "" : itemProposta.Especificacao.Nome;
                            elWorksheet.Range["AW" + linhaItem.ToString()].Value = itemProposta.ComentariosItem == null ? "" : itemProposta.ComentariosItem;
                            elWorksheet.Range["AX" + linhaItem.ToString()].Value = itemProposta.Status == null ? "" : itemProposta.Status.Nome;
                            linhaItem++;
                        }

                        // Incrementa a linha atual
                        linhaAtual++;

                        // Reporta o progresso se o progresso não for nulo
                        if (reportadorProgresso != null)
                        {
                            reportadorProgresso.Report((double)linhaAtual / (double)totalLinhas * (double)100);
                        }

                        // Lança exceção de cancelamento caso ela tenha sido efetuada
                        ct.ThrowIfCancellationRequested();
                    }

                    //elWorksheet.Range["A:AAA"].AutoFit();
                    elWorkbook.SaveAs2(caminhoArquivo);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.StackTrace);
                    throw;
                }
                finally
                {
                    elWorkbook.Close(false);
                    elApp.Quit();

                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(elApp);
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(elWorkbook);

                    Process[] excelProcesses = Process.GetProcessesByName("excel");
                    foreach (Process p in excelProcesses)
                    {
                        if (string.IsNullOrEmpty(p.MainWindowTitle)) // use MainWindowTitle to distinguish this excel process with other excel processes
                        {
                            p.Kill();
                        }
                    }
                }
            }, ct);
        }

        public static async Task ExportarPesquisaProposta(ObservableCollection<ResultadoPesquisaProposta> listaResultadoPesquisaProposta, IProgress<double>? reportadorProgresso, CancellationToken ct)
        {
            await Task.Run(async () =>
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                string nomeInicial = "Pesquisa_Propostas_" + DateTime.Now.ToString("yyyyMMddhhmmss");
                string nomeWorksheet = "pesquisa_propostas";
                string caminhoArquivo = "";

                VistaSaveFileDialog vistaSaveFileDialog = new VistaSaveFileDialog()
                {
                    Filter = "Arquivo do Excel (*.xlsx)|*.xlsx",
                    Title = "Informe o local e o nome do arquivo",
                    FileName = nomeInicial
                };

                if ((bool)vistaSaveFileDialog.ShowDialog())
                {
                    caminhoArquivo = vistaSaveFileDialog.FileName;
                }

                if (String.IsNullOrEmpty(caminhoArquivo))
                {
                    throw new OperationCanceledException();
                }

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                Application elApp = new() { Visible = false, DisplayAlerts = false };
                Workbook elWorkbook;
                Worksheet elWorksheet;
                elWorkbook = elApp.Workbooks.Add();
                elWorksheet = elWorkbook.Worksheets[1];
                elWorksheet.Name = nomeWorksheet;

                try
                {
                    elWorksheet.Range["A1"].Value = "Usuário";
                    elWorksheet.Range["B1"].Value = "Cliente";
                    elWorksheet.Range["C1"].Value = "Código da proposta";
                    elWorksheet.Range["D1"].Value = "Data da solicitação";
                    elWorksheet.Range["E1"].Value = "Data de envio";
                    elWorksheet.Range["F1"].Value = "Tempo de resposta";
                    elWorksheet.Range["G1"].Value = "Contato";
                    elWorksheet.Range["H1"].Value = "Série";
                    elWorksheet.Range["I1"].Value = "Quantidade de itens";
                    elWorksheet.Range["J1"].Value = "Valor de peças";
                    elWorksheet.Range["K1"].Value = "Valor de serviços";
                    elWorksheet.Range["L1"].Value = "Valor total";
                    elWorksheet.Range["M1"].Value = "Status da aprovação";
                    elWorksheet.Range["N1"].Value = "Valor faturado";
                    elWorksheet.Range["O1"].Value = "Data de envio para fat.";
                    elWorksheet.Range["P1"].Value = "Data do faturamento";
                    elWorksheet.Range["Q1"].Value = "Tempo para faturamento";
                    elWorksheet.Range["R1"].Value = "Nota fiscal";

                    elWorksheet.Range["I:I"].NumberFormat = "#,##0";
                    elWorksheet.Range["J:L"].NumberFormat = "$ #,##0.00";
                    elWorksheet.Range["N:N"].NumberFormat = "$ #,##0.00";

                    int linhaItem = 2;

                    // Cria e atribui a variável de contagem de linhas
                    int linhaAtual = 0;

                    // Cria e atribui a variável do total de linhas
                    int totalLinhas = listaResultadoPesquisaProposta.Count;

                    // Lança exceção de cancelamento caso ela tenha sido efetuada
                    ct.ThrowIfCancellationRequested();

                    foreach (ResultadoPesquisaProposta resultadoPesquisaProposta in listaResultadoPesquisaProposta)
                    {
                        elWorksheet.Range["A" + linhaItem.ToString()].Value = resultadoPesquisaProposta.NomeUsuario == null ? "" : resultadoPesquisaProposta.NomeUsuario;
                        elWorksheet.Range["B" + linhaItem.ToString()].Value = resultadoPesquisaProposta.NomeCliente == null ? "" : resultadoPesquisaProposta.NomeCliente;
                        elWorksheet.Range["C" + linhaItem.ToString()].Value = resultadoPesquisaProposta.CodigoProposta == null ? "" : resultadoPesquisaProposta.CodigoProposta;
                        elWorksheet.Range["D" + linhaItem.ToString()].Value = resultadoPesquisaProposta.DataSolicitacao == null ? "" : resultadoPesquisaProposta.DataSolicitacao;
                        elWorksheet.Range["E" + linhaItem.ToString()].Value = resultadoPesquisaProposta.DataEnvio == null ? "" : resultadoPesquisaProposta.DataEnvio;
                        elWorksheet.Range["F" + linhaItem.ToString()].Value = resultadoPesquisaProposta.TempoResposta == null ? 0 : resultadoPesquisaProposta.TempoResposta;
                        elWorksheet.Range["G" + linhaItem.ToString()].Value = resultadoPesquisaProposta.NomeContato == null ? "" : resultadoPesquisaProposta.NomeContato;
                        elWorksheet.Range["H" + linhaItem.ToString()].Value = resultadoPesquisaProposta.NomeSerie == null ? "" : resultadoPesquisaProposta.NomeSerie;
                        elWorksheet.Range["I" + linhaItem.ToString()].Value = resultadoPesquisaProposta.QuantidadeTotal == null ? 0 : resultadoPesquisaProposta.QuantidadeTotal;
                        elWorksheet.Range["J" + linhaItem.ToString()].Value = resultadoPesquisaProposta.ValorPecas == null ? 0 : resultadoPesquisaProposta.ValorPecas;
                        elWorksheet.Range["K" + linhaItem.ToString()].Value = resultadoPesquisaProposta.ValorServicos == null ? 0 : resultadoPesquisaProposta.ValorServicos;
                        elWorksheet.Range["L" + linhaItem.ToString()].Value = resultadoPesquisaProposta.ValorTotal == null ? 0 : resultadoPesquisaProposta.ValorTotal;
                        elWorksheet.Range["M" + linhaItem.ToString()].Value = resultadoPesquisaProposta.NomeStatusAprovacao == null ? "" : resultadoPesquisaProposta.NomeStatusAprovacao;
                        elWorksheet.Range["N" + linhaItem.ToString()].Value = resultadoPesquisaProposta.ValorFaturamento == null ? 0 : resultadoPesquisaProposta.ValorFaturamento;
                        elWorksheet.Range["O" + linhaItem.ToString()].Value = resultadoPesquisaProposta.DataEnvioFaturamento == null ? "" : resultadoPesquisaProposta.DataEnvioFaturamento;
                        elWorksheet.Range["P" + linhaItem.ToString()].Value = resultadoPesquisaProposta.DataFaturamento == null ? "" : resultadoPesquisaProposta.DataFaturamento;
                        elWorksheet.Range["Q" + linhaItem.ToString()].Value = resultadoPesquisaProposta.TempoFaturamento == null ? 0 : resultadoPesquisaProposta.TempoFaturamento;
                        elWorksheet.Range["R" + linhaItem.ToString()].Value = resultadoPesquisaProposta.NotaFiscal == null ? 0 : resultadoPesquisaProposta.NotaFiscal;

                        linhaItem++;

                        // Incrementa a linha atual
                        linhaAtual++;

                        // Reporta o progresso se o progresso não for nulo
                        if (reportadorProgresso != null)
                        {
                            reportadorProgresso.Report((double)linhaAtual / (double)totalLinhas * (double)100);
                        }

                        // Lança exceção de cancelamento caso ela tenha sido efetuada
                        ct.ThrowIfCancellationRequested();
                    }

                    //elWorksheet.Range["A:AAA"].AutoFit();
                    elWorkbook.SaveAs2(caminhoArquivo);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.StackTrace);
                    throw;
                }
                finally
                {
                    elWorkbook.Close(false);
                    elApp.Quit();

                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(elApp);
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(elWorkbook);

                    Process[] excelProcesses = Process.GetProcessesByName("excel");
                    foreach (Process p in excelProcesses)
                    {
                        if (string.IsNullOrEmpty(p.MainWindowTitle)) // use MainWindowTitle to distinguish this excel process with other excel processes
                        {
                            p.Kill();
                        }
                    }
                }
            }, ct);
        }

        public static async Task ExportarPesquisaRegistroManifestacao(ObservableCollection<ResultadoPesquisaRegistroManifestacao> listaResultadoPesquisaRegistroManifestacao, IProgress<double>? reportadorProgresso, CancellationToken ct)
        {
            await Task.Run(async () =>
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                string nomeInicial = "Pesquisa_Requisicoes_" + DateTime.Now.ToString("yyyyMMddhhmmss");
                string nomeWorksheet = "pesquisa_requisicoes";
                string caminhoArquivo = "";

                VistaSaveFileDialog vistaSaveFileDialog = new VistaSaveFileDialog()
                {
                    Filter = "Arquivo do Excel (*.xlsx)|*.xlsx",
                    Title = "Informe o local e o nome do arquivo",
                    FileName = nomeInicial
                };

                if ((bool)vistaSaveFileDialog.ShowDialog())
                {
                    caminhoArquivo = vistaSaveFileDialog.FileName;
                }

                if (String.IsNullOrEmpty(caminhoArquivo))
                {
                    throw new OperationCanceledException();
                }

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                Application elApp = new() { Visible = false, DisplayAlerts = false };
                Workbook elWorkbook;
                Worksheet elWorksheet;
                elWorkbook = elApp.Workbooks.Add();
                elWorksheet = elWorkbook.Worksheets[1];
                elWorksheet.Name = nomeWorksheet;

                try
                {
                    elWorksheet.Range["A1"].Value = "Código";
                    elWorksheet.Range["B1"].Value = "Prioridade";
                    elWorksheet.Range["C1"].Value = "Tipo";
                    elWorksheet.Range["D1"].Value = "Status";
                    elWorksheet.Range["E1"].Value = "Data de abertura";
                    elWorksheet.Range["F1"].Value = "Solicitante";
                    elWorksheet.Range["G1"].Value = "Descrição";
                    elWorksheet.Range["H1"].Value = "Tempo em aberto";

                    //elWorksheet.Range["E:E"].NumberFormat = "#,##0";

                    int linhaItem = 2;

                    // Cria e atribui a variável de contagem de linhas
                    int linhaAtual = 0;

                    // Cria e atribui a variável do total de linhas
                    int totalLinhas = listaResultadoPesquisaRegistroManifestacao.Count;

                    // Lança exceção de cancelamento caso ela tenha sido efetuada
                    ct.ThrowIfCancellationRequested();

                    foreach (ResultadoPesquisaRegistroManifestacao resultadoPesquisaRegistroManifestacao in listaResultadoPesquisaRegistroManifestacao)
                    {
                        elWorksheet.Range["A" + linhaItem.ToString()].Value = resultadoPesquisaRegistroManifestacao.CodigoManifestacao == null ? "" : resultadoPesquisaRegistroManifestacao.CodigoManifestacao;
                        elWorksheet.Range["B" + linhaItem.ToString()].Value = resultadoPesquisaRegistroManifestacao.Prioridade == null ? "" : resultadoPesquisaRegistroManifestacao.Prioridade;
                        elWorksheet.Range["C" + linhaItem.ToString()].Value = resultadoPesquisaRegistroManifestacao.Tipo == null ? "" : resultadoPesquisaRegistroManifestacao.Tipo;
                        elWorksheet.Range["D" + linhaItem.ToString()].Value = resultadoPesquisaRegistroManifestacao.Status == null ? "" : resultadoPesquisaRegistroManifestacao.Status;
                        elWorksheet.Range["E" + linhaItem.ToString()].Value = resultadoPesquisaRegistroManifestacao.DataAbertura == null ? "" : resultadoPesquisaRegistroManifestacao.DataAbertura;
                        elWorksheet.Range["F" + linhaItem.ToString()].Value = resultadoPesquisaRegistroManifestacao.PessoaAbertura == null ? "" : resultadoPesquisaRegistroManifestacao.PessoaAbertura;
                        elWorksheet.Range["G" + linhaItem.ToString()].Value = resultadoPesquisaRegistroManifestacao.DescricaoAbertura == null ? "" : resultadoPesquisaRegistroManifestacao.DescricaoAbertura;
                        elWorksheet.Range["H" + linhaItem.ToString()].Value = resultadoPesquisaRegistroManifestacao.TempoEmAberto == null ? 0 : resultadoPesquisaRegistroManifestacao.TempoEmAberto;
                        linhaItem++;

                        // Incrementa a linha atual
                        linhaAtual++;

                        // Reporta o progresso se o progresso não for nulo
                        if (reportadorProgresso != null)
                        {
                            reportadorProgresso.Report((double)linhaAtual / (double)totalLinhas * (double)100);
                        }

                        // Lança exceção de cancelamento caso ela tenha sido efetuada
                        ct.ThrowIfCancellationRequested();
                    }

                    //elWorksheet.Range["A:AAA"].AutoFit();
                    elWorkbook.SaveAs2(caminhoArquivo);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.StackTrace);
                    throw;
                }
                finally
                {
                    elWorkbook.Close(false);
                    elApp.Quit();

                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(elApp);
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(elWorkbook);

                    Process[] excelProcesses = Process.GetProcessesByName("excel");
                    foreach (Process p in excelProcesses)
                    {
                        if (string.IsNullOrEmpty(p.MainWindowTitle)) // use MainWindowTitle to distinguish this excel process with other excel processes
                        {
                            p.Kill();
                        }
                    }
                }
            }, ct);
        }

        private int RetornaColunaExcel(Microsoft.Office.Interop.Excel.Range targetRange, string valorProcurado)
        {
            try
            {
                return Convert.ToInt32(targetRange.Application.WorksheetFunction.Match(valorProcurado, targetRange, 0));
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        public static string ConverterColunaNumericaParaLetra(int numeroColuna)
        {
            string letracoluna = "";

            while (numeroColuna > 0)
            {
                int modulo = (numeroColuna - 1) % 26;
                letracoluna = Convert.ToChar('A' + modulo) + letracoluna;
                numeroColuna = (numeroColuna - modulo) / 26;
            }

            return letracoluna;
        }

        public static string RetornaPrazo(decimal quantidadeInicial, string prazoFornecedor, decimal quantidadeEstoque, int quantidadeDiasParaCliente)
        {
            string[] prazos;
            string novoTextoPrazo = "";
            string[] quantidadeData;
            //string novoTextoQuantidadeData = "";
            decimal quantidadeFornecedor = 0;
            //string textoDias = "";
            string textoPrazo = "";
            int diasPrazo = 0;
            string[] tmparr;

            // Código para retornar o texto do prazo de cada item da proposta

            // Tenta fazer retornar o prazo, caso contrário retorna "-"
            try
            {
                if (quantidadeEstoque >= quantidadeInicial)
                {
                    return "Imediato";
                }

                if (quantidadeEstoque <= 0)
                {
                    prazos = prazoFornecedor.Trim().Split("|");

                    foreach (string prazo in prazos)
                    {
                        if (!prazo.Trim().Contains('-'))
                        {
                            if (prazo.Trim().ToLower() == "ordem em atraso")
                            {
                                novoTextoPrazo = "Prazo indefinido";
                            }
                            else
                            {
                                switch (quantidadeDiasParaCliente)
                                {
                                    case 0:
                                        novoTextoPrazo = "Imediato";
                                        break;

                                    case 1:
                                        novoTextoPrazo = quantidadeDiasParaCliente.ToString() + " dia";
                                        break;

                                    default:
                                        novoTextoPrazo = quantidadeDiasParaCliente.ToString() + " dias";
                                        break;
                                }
                            }
                        }
                        else
                        {
                            quantidadeData = prazo.Trim().Split("-");

                            //Regex.Match(itemProposta.CodigoItem.Trim(), "[0-9]+(\.[0-9]+)?")

                            bool conversaoComSucesso = decimal.TryParse(Regex.Match(quantidadeData.First().Trim(), @"[0-9]+(\.[0-9]+)?").Value, out quantidadeFornecedor);

                            if (DateTime.TryParse(quantidadeData.Last().Trim(), out DateTime dataPrazo))
                            {
                                diasPrazo = (dataPrazo - DateTime.Today).Days + quantidadeDiasParaCliente;
                                switch (diasPrazo)
                                {
                                    case 0:
                                        textoPrazo = "Imediato";
                                        break;

                                    case 1:
                                        textoPrazo = diasPrazo.ToString() + " dia";
                                        break;

                                    default:
                                        textoPrazo = diasPrazo.ToString() + " dias";
                                        break;
                                }
                            }
                            else if (quantidadeData.Last().Trim().ToLower() == "ordem em atraso")
                            {
                                textoPrazo = "Prazo indefinido";
                            }
                            else
                            {
                                switch (quantidadeDiasParaCliente)
                                {
                                    case 0:
                                        textoPrazo = "Imediato";
                                        break;

                                    case 1:
                                        textoPrazo = quantidadeDiasParaCliente.ToString() + " dia";
                                        break;

                                    default:
                                        textoPrazo = quantidadeDiasParaCliente.ToString() + " dias";
                                        break;
                                }
                            }

                            if (novoTextoPrazo == "")
                            {
                                novoTextoPrazo = "Qtd: " + quantidadeFornecedor.ToString() + " - " + textoPrazo;
                            }
                            else
                            {
                                novoTextoPrazo = novoTextoPrazo + " | Qtd: " + quantidadeFornecedor.ToString() + " - " + textoPrazo;
                            }
                        }
                    }

                    novoTextoPrazo = novoTextoPrazo.Replace("- -", "-");

                    tmparr = novoTextoPrazo.Split("-");

                    if (tmparr.Length <= 2)
                    {
                        novoTextoPrazo = tmparr.Last().Trim();
                    }

                    return novoTextoPrazo;
                }

                // Caso a quantidade em estoque seja maior que zero mas seja menor que a quantidade solicitada
                // Verifica se o texto do prazo possui um "-", indicando que o prazo está quebrado em mais de 1

                prazos = prazoFornecedor.Split("|");

                novoTextoPrazo = "Qtd: " + quantidadeEstoque.ToString() + " - Imediato";

                decimal totalQuantidade = quantidadeInicial - quantidadeEstoque;

                foreach (string prazo in prazos)
                {
                    if (!prazo.Trim().Contains('-'))
                    {
                        if (prazo.Trim().ToLower() == "ordem em atraso")
                        {
                            //novoTextoPrazo = "Prazo indefinido";
                            if (novoTextoPrazo == "")
                            {
                                novoTextoPrazo = "Prazo indefinido";
                            }
                            else
                            {
                                novoTextoPrazo = novoTextoPrazo + " | Qtd: " + totalQuantidade.ToString() + " - Prazo indefinido";
                            }
                        }
                        else
                        {
                            switch (quantidadeDiasParaCliente)
                            {
                                case 0:
                                    novoTextoPrazo = "Imediato";
                                    break;

                                case 1:
                                    //novoTextoPrazo = quantidadeDiasParaCliente.ToString() + " dia";
                                    if (novoTextoPrazo == "")
                                    {
                                        novoTextoPrazo = quantidadeDiasParaCliente.ToString() + " dia";
                                    }
                                    else
                                    {
                                        novoTextoPrazo = novoTextoPrazo + " | Qtd: " + totalQuantidade.ToString() + " - " + quantidadeDiasParaCliente.ToString() + " dia";
                                    }
                                    break;

                                default:
                                    //novoTextoPrazo = quantidadeDiasParaCliente.ToString() + " dias";
                                    if (novoTextoPrazo == "")
                                    {
                                        novoTextoPrazo = quantidadeDiasParaCliente.ToString() + " dia";
                                    }
                                    else
                                    {
                                        novoTextoPrazo = novoTextoPrazo + " | Qtd: " + totalQuantidade.ToString() + " - " + quantidadeDiasParaCliente.ToString() + " dias";
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        quantidadeData = prazo.Trim().Split("-");

                        decimal quantidadeTeste = 0;
                        bool conversaoComSucesso = decimal.TryParse(Regex.Match(quantidadeData.First().Trim(), @"[0-9]+(\.[0-9]+)?").Value, out quantidadeTeste);

                        if (quantidadeTeste >= totalQuantidade)
                        {
                            quantidadeFornecedor = totalQuantidade;
                        }
                        else
                        {
                            quantidadeFornecedor = totalQuantidade - quantidadeTeste;
                        }

                        if (DateTime.TryParse(quantidadeData.Last().Trim(), out DateTime dataPrazo))
                        {
                            diasPrazo = (dataPrazo - DateTime.Today).Days + quantidadeDiasParaCliente;
                            switch (diasPrazo)
                            {
                                case 0:
                                    textoPrazo = "Imediato";
                                    break;

                                case 1:
                                    textoPrazo = diasPrazo.ToString() + " dia";
                                    break;

                                default:
                                    textoPrazo = diasPrazo.ToString() + " dias";
                                    break;
                            }
                        }
                        else if (quantidadeData.Last().Trim().ToLower() == "ordem em atraso")
                        {
                            textoPrazo = "Prazo indefinido";
                        }
                        else
                        {
                            switch (quantidadeDiasParaCliente)
                            {
                                case 0:
                                    textoPrazo = "Imediato";
                                    break;

                                case 1:
                                    textoPrazo = quantidadeDiasParaCliente.ToString() + " dia";
                                    break;

                                default:
                                    textoPrazo = quantidadeDiasParaCliente.ToString() + " dias";
                                    break;
                            }
                        }

                        if (novoTextoPrazo == "")
                        {
                            novoTextoPrazo = "Qtd: " + quantidadeFornecedor.ToString() + " - " + textoPrazo;
                        }
                        else
                        {
                            novoTextoPrazo = novoTextoPrazo + " | Qtd: " + quantidadeFornecedor.ToString() + " - " + textoPrazo;
                        }

                        if (quantidadeFornecedor + quantidadeEstoque >= quantidadeInicial)
                        {
                            break;
                        }
                    }
                }

                novoTextoPrazo = novoTextoPrazo.Replace("- -", "-");

                tmparr = novoTextoPrazo.Split("-");

                if (tmparr.Length <= 2)
                {
                    novoTextoPrazo = novoTextoPrazo = tmparr.Last().Trim();
                }

                return novoTextoPrazo;
            }
            catch (Exception)
            {
                return "-";
            }
        }
    }

    public class ValidacaoItemPropostaEstoque
    {
        public ValidacaoItemPropostaEstoque(ItemProposta itemProposta, string textoItemProposta, string textoEncontrado, decimal quantidadeEstoqueAbreviado, bool isSelected)
        {
            ItemProposta = itemProposta;
            TextoItemProposta = textoItemProposta;
            TextoEncontrado = textoEncontrado;
            IsSelected = isSelected;
            QuantidadeEstoqueAbreviado = quantidadeEstoqueAbreviado;
        }

        public ItemProposta ItemProposta { get; set; }
        public string TextoItemProposta { get; set; }
        public string TextoEncontrado { get; set; }
        public decimal QuantidadeEstoqueAbreviado { get; set; }
        public bool IsSelected { get; set; }
    }
}