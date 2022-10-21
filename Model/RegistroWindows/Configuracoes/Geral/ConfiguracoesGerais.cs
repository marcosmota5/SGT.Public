using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model.RegistroWindows.Configuracoes.Geral
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public class ConfiguracoesGerais : INotifyPropertyChanged, ICloneable
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private static readonly RegistryKey registryKey = Registry.CurrentUser;
        private const string subKey = @"Software\Proreports\SGT\Settings\General";

        private string _loginUltimoUsuarioLogado;
        private string _caminhoInstalacao;
        private int _idVersao;
        private string _caminhoAssinaturaEmail;
        private string _caminhoPastaEstoque;
        private string _caminhoUltimaPastaSelecionada;
        private string _caminhoUltimaPastaEmailSelecionadaCotacaoFornecedor1;
        private string _caminhoUltimaPastaEmailSelecionadaCotacaoFornecedor2;
        private string _caminhoUltimaPastaEmailSelecionadaCotacaoFornecedor3;
        private string _nomeArquivoEstoque;
        private string _nomeAbaArquivoEstoque;
        private string _senhaArquivoEstoque;
        private int _quantidadeDiasAdicionaisPrazo;
        private string _caminhoUltimaPastaEmailSelecionadaResposta;
        private bool _atualizarDashboardAutomaticamente;
        private int _segundosAtualizacaoAutomaticaDashboard;
        private int _limiteResultadosPesquisa;
        private string _tema;
        private string _esquemaCor;
        private DateTime? _dataInicioImportar;
        private DateTime? _dataFimImportar;
        private DateTime? _dataInicioResponder;
        private DateTime? _dataFimResponder;

        public string LoginUltimoUsuarioLogado { get { return _loginUltimoUsuarioLogado; } set { _loginUltimoUsuarioLogado = value; OnPropertyChanged(nameof(LoginUltimoUsuarioLogado)); } }
        public string CaminhoInstalacao { get { return _caminhoInstalacao; } set { _caminhoInstalacao = value; OnPropertyChanged(nameof(CaminhoInstalacao)); } }
        public int IdVersao { get { return _idVersao; } set { _idVersao = value; OnPropertyChanged(nameof(IdVersao)); } }
        public string CaminhoAssinaturaEmail { get { return _caminhoAssinaturaEmail; } set { _caminhoAssinaturaEmail = value; OnPropertyChanged(nameof(CaminhoAssinaturaEmail)); } }
        public string CaminhoPastaEstoque { get { return _caminhoPastaEstoque; } set { _caminhoPastaEstoque = value; OnPropertyChanged(nameof(CaminhoPastaEstoque)); } }
        public string CaminhoUltimaPastaSelecionada { get { return _caminhoUltimaPastaSelecionada; } set { _caminhoUltimaPastaSelecionada = value; OnPropertyChanged(nameof(CaminhoUltimaPastaSelecionada)); } }
        public string CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor1 { get { return _caminhoUltimaPastaEmailSelecionadaCotacaoFornecedor1; } set { _caminhoUltimaPastaEmailSelecionadaCotacaoFornecedor1 = value; OnPropertyChanged(nameof(CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor1)); } }
        public string CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor2 { get { return _caminhoUltimaPastaEmailSelecionadaCotacaoFornecedor2; } set { _caminhoUltimaPastaEmailSelecionadaCotacaoFornecedor2 = value; OnPropertyChanged(nameof(CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor2)); } }
        public string CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor3 { get { return _caminhoUltimaPastaEmailSelecionadaCotacaoFornecedor3; } set { _caminhoUltimaPastaEmailSelecionadaCotacaoFornecedor3 = value; OnPropertyChanged(nameof(CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor3)); } }
        public string NomeArquivoEstoque { get { return _nomeArquivoEstoque; } set { _nomeArquivoEstoque = value; OnPropertyChanged(nameof(NomeArquivoEstoque)); } }
        public string NomeAbaArquivoEstoque { get { return _nomeAbaArquivoEstoque; } set { _nomeAbaArquivoEstoque = value; OnPropertyChanged(nameof(NomeAbaArquivoEstoque)); } }
        public string SenhaArquivoEstoque { get { return _senhaArquivoEstoque; } set { _senhaArquivoEstoque = value; OnPropertyChanged(nameof(SenhaArquivoEstoque)); } }
        public int QuantidadeDiasAdicionaisPrazo { get { return _quantidadeDiasAdicionaisPrazo; } set { _quantidadeDiasAdicionaisPrazo = value; OnPropertyChanged(nameof(QuantidadeDiasAdicionaisPrazo)); } }
        public string CaminhoUltimaPastaEmailSelecionadaResposta { get { return _caminhoUltimaPastaEmailSelecionadaResposta; } set { _caminhoUltimaPastaEmailSelecionadaResposta = value; OnPropertyChanged(nameof(CaminhoUltimaPastaEmailSelecionadaResposta)); } }
        public bool AtualizarDashboardAutomaticamente { get { return _atualizarDashboardAutomaticamente; } set { _atualizarDashboardAutomaticamente = value; OnPropertyChanged(nameof(AtualizarDashboardAutomaticamente)); } }
        public int SegundosAtualizacaoAutomaticaDashboard { get { return _segundosAtualizacaoAutomaticaDashboard; } set { _segundosAtualizacaoAutomaticaDashboard = value; OnPropertyChanged(nameof(SegundosAtualizacaoAutomaticaDashboard)); } }
        public int LimiteResultadosPesquisa { get { return _limiteResultadosPesquisa; } set { _limiteResultadosPesquisa = value; OnPropertyChanged(nameof(LimiteResultadosPesquisa)); } }
        public string Tema { get { return _tema; } set { _tema = value; OnPropertyChanged(nameof(Tema)); } }
        public string EsquemaCor { get { return _esquemaCor; } set { _esquemaCor = value; OnPropertyChanged(nameof(EsquemaCor)); } }
        public DateTime? DataInicioImportar { get { return _dataInicioImportar; } set { _dataInicioImportar = value; OnPropertyChanged(nameof(DataInicioImportar)); } }
        public DateTime? DataFimImportar { get { return _dataFimImportar; } set { _dataFimImportar = value; OnPropertyChanged(nameof(DataFimImportar)); } }
        public DateTime? DataInicioResponder { get { return _dataInicioResponder; } set { _dataInicioResponder = value; OnPropertyChanged(nameof(DataInicioResponder)); } }
        public DateTime? DataFimResponder { get { return _dataFimResponder; } set { _dataFimResponder = value; OnPropertyChanged(nameof(DataFimResponder)); } }

        /// <summary>
        /// Construtor das configurações gerais sem parâmetros
        /// </summary>
        public ConfiguracoesGerais()
        {
            GetValoresRegistro();
        }

#pragma warning disable CS8603 // Possible null reference return.
        /// <summary>
        /// Retorna um valor do registro do Windows com os parâmetros utilizados
        /// </summary>
        /// <param name="keyName">Nome da chave a ser buscada no registro</param>
        /// <param name="defaultValue">Valor padrão a ser retornado caso não seja encontrado</param>
        /// <param name="keyKind">Tipo do valor a ser retornado</param>
        /// <returns>Objeto proveniente do registro do windows</returns>
        public object RetornaValorRegistroWindows(string keyName, object defaultValue, RegistryValueKind keyKind)
        {
            // Tenta retornar o valor do registro e, caso não consiga, retorna o valor padrão
            try
            {
                using (RegistryKey key = registryKey.CreateSubKey(subKey, true))
                {
                    if (key.GetValue(keyName) == null)
                    {
                        key.SetValue(keyName, defaultValue, keyKind);
                    }

                    return key.GetValue(keyName, defaultValue);

                }
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
#pragma warning restore CS8603 // Possible null reference return.

        /// <summary>
        /// Define o valor do registro do Windows coms os parâmetros utilizados
        /// </summary>
        /// <param name="keyName">Nome da chave a ser buscada no registro</param>
        /// <param name="defaultValue">Valor padrão a ser retornado caso não seja encontrado</param>
        /// <param name="keyKind">Tipo do valor a ser retornado</param>
        public void DefineValorRegistroWindows(string keyName, object keyValue, RegistryValueKind keyKind)
        {
            try
            {
                using (RegistryKey key = registryKey.CreateSubKey(subKey, true))
                {
                    key.SetValue(keyName, keyValue, keyKind);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método para salvar os valores no registro do Windows
        /// </summary>
        public void SalvarNoRegistro()
        {
            // Tenta definir os valores no registro e retorna exceção caso não consiga
            try
            {
                DefineValorRegistroWindows("LastLoggedUserLogin", LoginUltimoUsuarioLogado, RegistryValueKind.String);
                DefineValorRegistroWindows("InstallPath", CaminhoInstalacao, RegistryValueKind.String);
                DefineValorRegistroWindows("VersionId", IdVersao, RegistryValueKind.DWord);
                DefineValorRegistroWindows("SelectedSignaturePath", CaminhoAssinaturaEmail, RegistryValueKind.String);
                DefineValorRegistroWindows("StockFileFolderPath", CaminhoPastaEstoque, RegistryValueKind.String);
                DefineValorRegistroWindows("LastSelectedFolder", CaminhoUltimaPastaSelecionada, RegistryValueKind.String);
                DefineValorRegistroWindows("LastSelectedQuotationsFolderSupplier1", CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor1, RegistryValueKind.String);
                DefineValorRegistroWindows("LastSelectedQuotationsFolderSupplier2", CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor2, RegistryValueKind.String);
                DefineValorRegistroWindows("LastSelectedQuotationsFolderSupplier3", CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor3, RegistryValueKind.String);
                DefineValorRegistroWindows("StockFileName", NomeArquivoEstoque, RegistryValueKind.String);
                DefineValorRegistroWindows("StockFileSheetName", NomeAbaArquivoEstoque, RegistryValueKind.String);
                DefineValorRegistroWindows("StockFilePassword", SenhaArquivoEstoque, RegistryValueKind.String);
                DefineValorRegistroWindows("DeadlineDaysAmount", QuantidadeDiasAdicionaisPrazo, RegistryValueKind.DWord);
                DefineValorRegistroWindows("LastSelectedReplyEmailFolder", CaminhoUltimaPastaEmailSelecionadaResposta, RegistryValueKind.String);
                DefineValorRegistroWindows("AutoRefreshDashboard", AtualizarDashboardAutomaticamente == true ? 1 : 0, RegistryValueKind.DWord);
                DefineValorRegistroWindows("AutoRefreshDashboardSeconds", SegundosAtualizacaoAutomaticaDashboard, RegistryValueKind.DWord);
                DefineValorRegistroWindows("SearchResultsLimit", LimiteResultadosPesquisa, RegistryValueKind.DWord);
                DefineValorRegistroWindows("Theme", Tema, RegistryValueKind.String);
                DefineValorRegistroWindows("AccentColor", EsquemaCor, RegistryValueKind.String);
                DefineValorRegistroWindows("DateStartImport", DataInicioImportar == null ? "" : ((DateTime)DataInicioImportar).ToString(), RegistryValueKind.String);
                DefineValorRegistroWindows("DateEndImport", DataFimImportar == null ? "" : ((DateTime)DataFimImportar).ToString(), RegistryValueKind.String);
                DefineValorRegistroWindows("DateStartReply", DataInicioResponder == null ? "" : ((DateTime)DataInicioResponder).ToString(), RegistryValueKind.String);
                DefineValorRegistroWindows("DateEndReply", DataFimResponder == null ? "" : ((DateTime)DataFimResponder).ToString(), RegistryValueKind.String);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método para retornar os valores do registro do Windows
        /// </summary>
        public void GetValoresRegistro()
        {
            LoginUltimoUsuarioLogado = (string)RetornaValorRegistroWindows("LastLoggedUserLogin", "", RegistryValueKind.String);
            CaminhoInstalacao = (string)RetornaValorRegistroWindows("InstallPath", "", RegistryValueKind.String);
            IdVersao = (int)RetornaValorRegistroWindows("VersionId", 1, RegistryValueKind.DWord);

            try
            {
                CaminhoAssinaturaEmail = (string)RetornaValorRegistroWindows("SelectedSignaturePath",
                                Directory.GetFiles(Environment.GetEnvironmentVariable("AppData") + "\\Microsoft\\Signatures", "*.htm").First(), RegistryValueKind.String);
            }
            catch (Exception)
            {
                CaminhoAssinaturaEmail = (string)RetornaValorRegistroWindows("SelectedSignaturePath", "", RegistryValueKind.String);
            }

            try
            {
                CaminhoPastaEstoque = (string)RetornaValorRegistroWindows("StockFileFolderPath",
                                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), RegistryValueKind.String);
            }
            catch (Exception)
            {
                CaminhoPastaEstoque = (string)RetornaValorRegistroWindows("StockFileFolderPath", "", RegistryValueKind.String);
            }

            NomeArquivoEstoque = (string)RetornaValorRegistroWindows("StockFileName", "CONTROLE DE ESTOQUE", RegistryValueKind.String);
            NomeAbaArquivoEstoque = (string)RetornaValorRegistroWindows("StockFileSheetName", "ESTOQUE DE REVENDA", RegistryValueKind.String);
            SenhaArquivoEstoque = (string)RetornaValorRegistroWindows("StockFilePassword", "", RegistryValueKind.String);

            try
            {
                CaminhoUltimaPastaSelecionada = (string)RetornaValorRegistroWindows("LastSelectedFolder", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), RegistryValueKind.String);

            }
            catch (Exception)
            {
                CaminhoUltimaPastaSelecionada = (string)RetornaValorRegistroWindows("LastSelectedFolder", "", RegistryValueKind.String);
            }

            CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor1 = (string)RetornaValorRegistroWindows("LastSelectedQuotationsFolderSupplier1", "", RegistryValueKind.String);
            CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor2 = (string)RetornaValorRegistroWindows("LastSelectedQuotationsFolderSupplier2", "", RegistryValueKind.String);
            CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor3 = (string)RetornaValorRegistroWindows("LastSelectedQuotationsFolderSupplier3", "", RegistryValueKind.String);
            QuantidadeDiasAdicionaisPrazo = (int)RetornaValorRegistroWindows("DeadlineDaysAmount", 4, RegistryValueKind.DWord);
            CaminhoUltimaPastaEmailSelecionadaResposta = (string)RetornaValorRegistroWindows("LastSelectedReplyEmailFolder", "", RegistryValueKind.String);
            AtualizarDashboardAutomaticamente = (int)RetornaValorRegistroWindows("AutoRefreshDashboard", 1, RegistryValueKind.DWord) != 0;
            SegundosAtualizacaoAutomaticaDashboard = (int)RetornaValorRegistroWindows("AutoRefreshDashboardSeconds", 300, RegistryValueKind.DWord);
            LimiteResultadosPesquisa = (int)RetornaValorRegistroWindows("SearchResultsLimit", 500, RegistryValueKind.DWord);
            Tema = (string)RetornaValorRegistroWindows("Theme", "Light", RegistryValueKind.String);
            EsquemaCor = (string)RetornaValorRegistroWindows("AccentColor", "Blue", RegistryValueKind.String);
            DataInicioImportar = String.IsNullOrEmpty((string)RetornaValorRegistroWindows("DateStartImport", "", RegistryValueKind.String)) ? null : Convert.ToDateTime(RetornaValorRegistroWindows("DateStartImport", "", RegistryValueKind.String));
            DataFimImportar = String.IsNullOrEmpty((string)RetornaValorRegistroWindows("DateEndImport", "", RegistryValueKind.String)) ? null : Convert.ToDateTime(RetornaValorRegistroWindows("DateEndImport", "", RegistryValueKind.String));
            DataInicioResponder = String.IsNullOrEmpty((string)RetornaValorRegistroWindows("DateStartReply", "", RegistryValueKind.String)) ? null : Convert.ToDateTime(RetornaValorRegistroWindows("DateStartReply", "", RegistryValueKind.String));
            DataFimResponder = String.IsNullOrEmpty((string)RetornaValorRegistroWindows("DateEndReply", "", RegistryValueKind.String)) ? null : Convert.ToDateTime(RetornaValorRegistroWindows("DateEndReply", "", RegistryValueKind.String));
        }

        protected void OnPropertyChanged([CallerMemberName] string memberName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(memberName));
            }
        }

        public object Clone()
        {
            ConfiguracoesGerais configuracoesGeraisCopia = new();

            configuracoesGeraisCopia.LoginUltimoUsuarioLogado = LoginUltimoUsuarioLogado;
            configuracoesGeraisCopia.CaminhoInstalacao = CaminhoInstalacao;
            configuracoesGeraisCopia.IdVersao = IdVersao;
            configuracoesGeraisCopia.CaminhoAssinaturaEmail = CaminhoAssinaturaEmail;
            configuracoesGeraisCopia.CaminhoPastaEstoque = CaminhoPastaEstoque;
            configuracoesGeraisCopia.CaminhoUltimaPastaSelecionada = CaminhoUltimaPastaSelecionada;
            configuracoesGeraisCopia.CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor1 = CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor1;
            configuracoesGeraisCopia.CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor2 = CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor2;
            configuracoesGeraisCopia.CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor3 = CaminhoUltimaPastaEmailSelecionadaCotacaoFornecedor3;
            configuracoesGeraisCopia.NomeArquivoEstoque = NomeArquivoEstoque;
            configuracoesGeraisCopia.NomeAbaArquivoEstoque = NomeAbaArquivoEstoque;
            configuracoesGeraisCopia.SenhaArquivoEstoque = SenhaArquivoEstoque;
            configuracoesGeraisCopia.QuantidadeDiasAdicionaisPrazo = QuantidadeDiasAdicionaisPrazo;
            configuracoesGeraisCopia.CaminhoUltimaPastaEmailSelecionadaResposta = CaminhoUltimaPastaEmailSelecionadaResposta;
            configuracoesGeraisCopia.AtualizarDashboardAutomaticamente = AtualizarDashboardAutomaticamente;
            configuracoesGeraisCopia.SegundosAtualizacaoAutomaticaDashboard = SegundosAtualizacaoAutomaticaDashboard;
            configuracoesGeraisCopia.LimiteResultadosPesquisa = LimiteResultadosPesquisa;
            configuracoesGeraisCopia.Tema = Tema;
            configuracoesGeraisCopia.EsquemaCor = EsquemaCor;
            configuracoesGeraisCopia.DataInicioImportar = DataInicioImportar;
            configuracoesGeraisCopia.DataFimImportar = DataFimImportar;
            configuracoesGeraisCopia.DataInicioResponder = DataInicioResponder;
            configuracoesGeraisCopia.DataFimResponder = DataFimResponder;

            return configuracoesGeraisCopia;
        }
    }
}
