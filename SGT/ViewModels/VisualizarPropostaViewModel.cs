using BoldReports.UI.Xaml;
using BoldReports.Windows;
using BoldReports.Writer;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;
using Ookii.Dialogs.Wpf;
using SGT.HelperClasses;
using SGT.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class VisualizarPropostaViewModel : ObservableObject
    {
        #region Campos

        private Parametro _parametroNCM;
        private Parametro _parametroConjunto;
        private Parametro _parametroExibicaoCodigos;
        private Parametro _parametroExibicaoDescricao;
        private CancellationTokenSource _cts;
        private double _valorProgresso;
        private bool _progressoEhIndeterminavel;
        private bool _progressoVisivel = false;
        private string _textoStatus;
        private double _alturaGrid = 25;
        private Proposta propostaInicial;
        private Proposta _proposta = new();

        private bool _visibilidadeConjunto;
        private int _tamanhoColunaItem = 398;
        private int _tamanhoColunaCodigo;
        private int _tamanhoColunaNCM;
        private bool _controlesHabilitados;
        private bool _carregamentoVisivel = true;
        private bool _edicaoHabilitada = true;

        private string _caminhoLogo1;
        private string _caminhoLogo2;
        private string _caminhoLogo3;

        private readonly IDialogCoordinator _dialogCoordinator;
        private ObservableCollection<ItemPropostaExibir> _listaItensPecas = new();
        private ObservableCollection<ItemPropostaExibir> _listaItensPecasComConjunto = new();
        private ObservableCollection<ItemPropostaExibir> _listaItensPecasExibir = new();
        private ObservableCollection<ItemPropostaExibir> _listaItensServicosEDeslocamentos = new();
        private ObservableCollection<Parametro> _listaParametroNCM = new();
        private ObservableCollection<Parametro> _listaParametroConjunto = new();
        private ObservableCollection<Parametro> _listaParametroExibicaoCodigos = new();
        private ObservableCollection<Parametro> _listaParametroExibicaoDescricao = new();

        private ICommand _comandoEnviarEmail;
        private ICommand _comandoResponderEmail;
        private ICommand _comandoSalvar;
        private ICommand _comandoAlteraParametros;

        #endregion Campos

        #region Construtores

        public VisualizarPropostaViewModel(IDialogCoordinator dialogCoordinator, Proposta proposta)
        {
            _dialogCoordinator = dialogCoordinator;

            Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(DescricaoLocal));
            Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(TextoLocalDataAtual));
            Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(TextoDataAprovacao));
            Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(TelefoneContatoFormatado));

            Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(TextoNomeEmpresa));
            Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(TextoEndereco1));
            Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(TextoEndereco2));
            Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(TextoContatos));
            Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(TextoSite));

            Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(PermiteEdicao));
            Proposta.PropertyChanged += (s, e) => OnPropertyChanged(nameof(TestePadding));

            propostaInicial = proposta;
            Proposta = (Proposta)proposta.Clone();

            try
            {
                CaminhoLogo1 = Path.GetTempPath() + "Proreports\\SGT\\Imagens\\logo_1.png";
                CaminhoLogo2 = Path.GetTempPath() + "Proreports\\SGT\\Imagens\\logo_2.png";
                CaminhoLogo3 = Path.GetTempPath() + "Proreports\\SGT\\Imagens\\logo_3.png";
            }
            catch (Exception)
            {
                TextoStatus = "Falha ao retornar os logotipos. Caso o problema persista, reinicie o sistema";
            }

            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public ReportViewer ReportViewer { get; set; }

        public bool ContemPecas => ListaItensPecas.Count > 0;
        public bool ContemServicosEDesligamentos => ListaItensServicosEDeslocamentos.Count > 0;
        public decimal TotalPecas => Convert.ToDecimal(ListaItensPecas.Sum(prec => prec.PrecoTotalFinalItem));
        public decimal TotalServicosEDeslocamentos => Convert.ToDecimal(ListaItensServicosEDeslocamentos.Sum(prec => prec.PrecoTotalFinalItem));
        public string DescricaoLocal => Convert.ToString(Proposta?.Contato?.Cidade?.Nome) + " - " + Convert.ToString(Proposta?.Contato?.Cidade?.Estado?.Nome == null ? "" : Proposta?.Contato?.Cidade?.Estado?.Nome).ToUpper();
        public string TextoLocalDataAtual => App.Usuario?.Filial?.Cidade?.Nome + " - " + App.Usuario?.Filial?.Cidade?.Estado?.Nome.ToUpper() + ", " + DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
        public string TextoDataAprovacao => Proposta.ListaItensProposta.Max(d => d.DataAprovacaoItem) == null ? "" : "Data da aprovação: " + Convert.ToDateTime(Proposta.ListaItensProposta.Max(d => d.DataAprovacaoItem)).ToString("d");
        public string? TextoNomeEmpresa => App.Usuario?.Filial?.Empresa?.RazaoSocial;
        public string? TextoEndereco1 => App.Usuario?.Filial?.Endereco;
        public string? TextoEndereco2 => App.Usuario?.Filial?.Cidade?.Nome + " - " + App.Usuario?.Filial?.Cidade?.Estado?.Nome.ToUpper() + ", " + Convert.ToInt64(App.Usuario?.Filial?.CEP).ToString("00000-000");
        public string? TextoContatos => "Contato: " + FuncoesDeTexto.NomeSobrenome(App.Usuario?.Nome) + " | Telefone: " + Convert.ToInt64(App.Usuario?.Telefone == null ? 0 : App.Usuario?.Telefone).ToString(App.Usuario?.Telefone?.Length > 10 ? "(00) 00000-0000" : "(00) 0000-0000") + " | E-mail: " + App.Usuario?.Email;
        public string? TextoSite => App.Usuario?.Filial?.Empresa?.Site;
        public bool PermiteEdicao => Proposta.DataEnvio == null;
        public string TestePadding => Proposta.DataEnvio == null ? "0" : "-4,0";
        public string TelefoneContatoFormatado => Proposta?.Contato?.Telefone == null ? "" : Convert.ToInt64(Proposta?.Contato?.Telefone).ToString(Proposta?.Contato?.Telefone.Length > 10 ? "(00) 00000-0000" : "(00) 0000-0000");

        public Proposta Proposta
        {
            get { return _proposta; }
            set
            {
                if (value != _proposta)
                {
                    _proposta = value;
                    OnPropertyChanged(nameof(Proposta));
                }
            }
        }

        public bool VisibilidadeConjunto
        {
            get { return _visibilidadeConjunto; }
            set
            {
                if (value != _visibilidadeConjunto)
                {
                    _visibilidadeConjunto = value;
                    OnPropertyChanged(nameof(VisibilidadeConjunto));
                }
            }
        }

        public int TamanhoColunaItem
        {
            get { return _tamanhoColunaItem; }
            set
            {
                if (value != _tamanhoColunaItem)
                {
                    _tamanhoColunaItem = value;
                    OnPropertyChanged(nameof(TamanhoColunaItem));
                }
            }
        }

        public int TamanhoColunaCodigo
        {
            get { return _tamanhoColunaCodigo; }
            set
            {
                if (value != _tamanhoColunaCodigo)
                {
                    _tamanhoColunaCodigo = value;
                    OnPropertyChanged(nameof(TamanhoColunaCodigo));

                    TamanhoColunaItem = 398 - TamanhoColunaCodigo - TamanhoColunaNCM;
                }
            }
        }

        public int TamanhoColunaNCM
        {
            get { return _tamanhoColunaNCM; }
            set
            {
                if (value != _tamanhoColunaNCM)
                {
                    _tamanhoColunaNCM = value;
                    OnPropertyChanged(nameof(TamanhoColunaNCM));

                    TamanhoColunaItem = 398 - TamanhoColunaCodigo - TamanhoColunaNCM;
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

        public Parametro ParametroNCM
        {
            get { return _parametroNCM; }
            set
            {
                if (value != _parametroNCM)
                {
                    _parametroNCM = value;
                    OnPropertyChanged(nameof(ParametroNCM));

                    AtualizaParametros();
                }
            }
        }

        public Parametro ParametroConjunto
        {
            get { return _parametroConjunto; }
            set
            {
                if (value != _parametroConjunto)
                {
                    _parametroConjunto = value;
                    OnPropertyChanged(nameof(ParametroConjunto));

                    AtualizaParametros();
                }
            }
        }

        public Parametro ParametroExibicaoCodigos
        {
            get { return _parametroExibicaoCodigos; }
            set
            {
                if (value != _parametroExibicaoCodigos)
                {
                    _parametroExibicaoCodigos = value;
                    OnPropertyChanged(nameof(ParametroExibicaoCodigos));

                    AtualizaParametros();
                }
            }
        }

        public Parametro ParametroExibicaoDescricao
        {
            get { return _parametroExibicaoDescricao; }
            set
            {
                if (value != _parametroExibicaoDescricao)
                {
                    _parametroExibicaoDescricao = value;
                    OnPropertyChanged(nameof(ParametroExibicaoDescricao));

                    AtualizaParametros();
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
                    Messenger.Default.Send<double>(ValorProgresso, "ValorProgresso2");
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

        public bool ProgressoVisivel
        {
            get { return _progressoVisivel; }
            set
            {
                if (value != _progressoVisivel)
                {
                    _progressoVisivel = value;
                    if (_progressoVisivel)
                    {
                        AlturaGrid = 35;
                    }
                    else
                    {
                        AlturaGrid = 25;
                    }
                    OnPropertyChanged(nameof(ProgressoVisivel));
                }
            }
        }

        public double AlturaGrid
        {
            get
            {
                return _alturaGrid;
            }
            set
            {
                if (value != _alturaGrid)
                {
                    _alturaGrid = value;
                    OnPropertyChanged(nameof(AlturaGrid));
                }
            }
        }

        public string TextoStatus
        {
            get { return _textoStatus; }
            set
            {
                if (value != _textoStatus)
                {
                    _textoStatus = value;
                    OnPropertyChanged(nameof(TextoStatus));
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

        public bool EdicaoHabilitada
        {
            get { return _edicaoHabilitada; }
            set
            {
                if (_edicaoHabilitada != value)
                {
                    _edicaoHabilitada = value;
                    OnPropertyChanged(nameof(EdicaoHabilitada));
                }
            }
        }

        public string CaminhoLogo1
        {
            get { return _caminhoLogo1; }
            set
            {
                if (_caminhoLogo1 != value)
                {
                    _caminhoLogo1 = value;
                    OnPropertyChanged(nameof(CaminhoLogo1));
                }
            }
        }

        public string CaminhoLogo2
        {
            get { return _caminhoLogo2; }
            set
            {
                if (_caminhoLogo2 != value)
                {
                    _caminhoLogo2 = value;
                    OnPropertyChanged(nameof(CaminhoLogo2));
                }
            }
        }

        public string CaminhoLogo3
        {
            get { return _caminhoLogo3; }
            set
            {
                if (_caminhoLogo3 != value)
                {
                    _caminhoLogo3 = value;
                    OnPropertyChanged(nameof(CaminhoLogo3));
                }
            }
        }

        public ObservableCollection<ItemPropostaExibir> ListaItensPecas
        {
            get { return _listaItensPecas; }
            set
            {
                if (value != _listaItensPecas)
                {
                    _listaItensPecas = value;
                    OnPropertyChanged(nameof(ListaItensPecas));
                }
            }
        }

        public ObservableCollection<ItemPropostaExibir> ListaItensPecasComConjuntos
        {
            get { return _listaItensPecasComConjunto; }
            set
            {
                if (value != _listaItensPecasComConjunto)
                {
                    _listaItensPecasComConjunto = value;
                    OnPropertyChanged(nameof(ListaItensPecasComConjuntos));
                }
            }
        }

        public ObservableCollection<ItemPropostaExibir> ListaItensPecasExibir
        {
            get { return _listaItensPecasExibir; }
            set
            {
                if (value != _listaItensPecasExibir)
                {
                    _listaItensPecasExibir = value;
                    OnPropertyChanged(nameof(ListaItensPecasExibir));
                }
            }
        }

        public ObservableCollection<ItemPropostaExibir> ListaItensServicosEDeslocamentos
        {
            get { return _listaItensServicosEDeslocamentos; }
            set
            {
                if (value != _listaItensServicosEDeslocamentos)
                {
                    _listaItensServicosEDeslocamentos = value;
                    OnPropertyChanged(nameof(ListaItensServicosEDeslocamentos));
                }
            }
        }

        public ObservableCollection<Parametro> ListaParametroNCM
        {
            get { return _listaParametroNCM; }
            set
            {
                if (value != _listaParametroNCM)
                {
                    _listaParametroNCM = value;
                    OnPropertyChanged(nameof(ListaParametroNCM));
                }
            }
        }

        public ObservableCollection<Parametro> ListaParametroConjunto
        {
            get { return _listaParametroConjunto; }
            set
            {
                if (value != _listaParametroConjunto)
                {
                    _listaParametroConjunto = value;
                    OnPropertyChanged(nameof(ListaParametroConjunto));
                }
            }
        }

        public ObservableCollection<Parametro> ListaParametroExibicaoCodigos
        {
            get { return _listaParametroExibicaoCodigos; }
            set
            {
                if (value != _listaParametroExibicaoCodigos)
                {
                    _listaParametroExibicaoCodigos = value;
                    OnPropertyChanged(nameof(ListaParametroExibicaoCodigos));
                }
            }
        }

        public ObservableCollection<Parametro> ListaParametroExibicaoDescricao
        {
            get { return _listaParametroExibicaoDescricao; }
            set
            {
                if (value != _listaParametroExibicaoDescricao)
                {
                    _listaParametroExibicaoDescricao = value;
                    OnPropertyChanged(nameof(ListaParametroExibicaoDescricao));
                }
            }
        }

        public ICommand ComandoEnviarEmail
        {
            get
            {
                if (_comandoEnviarEmail == null)
                {
                    _comandoEnviarEmail = new RelayCommand(
                        param => EnviarEmailAsync().Await(),
                        param => true
                    );
                }
                return _comandoEnviarEmail;
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

        public ICommand ComandoSalvar
        {
            get
            {
                if (_comandoSalvar == null)
                {
                    _comandoSalvar = new RelayCommand(
                        param => SalvarArquivoAsync(@"G:\Marcos Mota\Área de Trabalho\ReportViewerRDLC", WriterFormat.PDF).Await(),
                        param => true
                    );
                }
                return _comandoSalvar;
            }
        }

        public ICommand ComandoAlteraParametros
        {
            get
            {
                if (_comandoAlteraParametros == null)
                {
                    _comandoAlteraParametros = new RelayCommand(
                        param => AtualizaParametros(),
                        param => true
                    );
                }
                return _comandoAlteraParametros;
            }
        }

        #endregion Propriedades/Comandos

        #region Métodos

        /// <summary>
        /// Método assíncrono que serve como construtor da proposta já que construtores não podem ser assíncronos
        /// </summary>
        private async Task ConstrutorAsync()
        {
            try
            {
                ListaItensPecas.CollectionChanged += (s, e) => OnPropertyChanged(nameof(ContemPecas));
                ListaItensServicosEDeslocamentos.CollectionChanged += (s, e) => OnPropertyChanged(nameof(ContemServicosEDesligamentos));
                ListaItensPecas.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalPecas));
                ListaItensServicosEDeslocamentos.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalServicosEDeslocamentos));

                ListaParametroNCM.Add(new Parametro("Exibir NCM", "exibir", "string", "", "Eye", "SteelBlue"));
                ListaParametroNCM.Add(new Parametro("Ocultar NCM", "ocultar", "string", "", "EyeOff", "Gray"));

                ListaParametroConjunto.Add(new Parametro("Exibir conjunto", "exibir", "string", "", "Eye", "SteelBlue"));
                ListaParametroConjunto.Add(new Parametro("Ocultar conjunto", "ocultar", "string", "", "EyeOff", "Gray"));

                ListaParametroExibicaoCodigos.Add(new Parametro("Exibir códigos atuais abreviados", "exibir_abreviado", "string", "", "Eye", "SteelBlue"));
                ListaParametroExibicaoCodigos.Add(new Parametro("Exibir códigos atuais completos", "exibir_completo", "string", "", "Eye", "SteelBlue"));
                ListaParametroExibicaoCodigos.Add(new Parametro("Ocultar códigos atuais", "ocultar", "string", "", "EyeOff", "Gray"));

                ListaParametroExibicaoDescricao.Add(new Parametro("Exibir descrição completa", "exibir", "string", "", "Eye", "SteelBlue"));
                ListaParametroExibicaoDescricao.Add(new Parametro("Ocultar códigos anteriores da descrição", "ocultar", "string", "", "EyeOff", "Gray"));

                ParametroNCM = ListaParametroNCM.Where(p => p.Valor == "ocultar").First();
                ParametroConjunto = ListaParametroConjunto.Where(p => p.Valor == "ocultar").First();
                ParametroExibicaoCodigos = ListaParametroExibicaoCodigos.Where(p => p.Valor == "ocultar").First();
                ParametroExibicaoDescricao = ListaParametroExibicaoDescricao.Where(p => p.Valor == "ocultar").First();

                var dtProposta = await FuncoesDeDatabase.GetDataTable("Propostas", "SELECT " +
                                                              "clie.nome AS Cliente,  " +
                                                              "IF(cida.nome IS NULL, '', CONCAT(cida.nome, '-' ,esta.nome)) AS Local, " +
                                                              "cont.nome AS Contato, " +
                                                              "cont.telefone AS TelefoneContato, " +
                                                              "cont.email AS EmailContato, " +
                                                              "prop.codigo_proposta AS CodigoProposta, " +
                                                              "prop.data_solicitacao AS DataSolicitacao, " +
                                                              "mode.nome AS Modelo, " +
                                                              "seri.nome AS Serie, " +
                                                              "prop.horimetro AS Horimetro, " +
                                                              "prop.texto_padrao AS TextoPadrao, " +
                                                              "prop.observacoes AS Observacoes, " +
                                                              "prop.prazo_entrega AS PrazoEntrega, " +
                                                              "prop.condicao_pagamento AS CondicaoPagamento, " +
                                                              "prop.garantia AS Garantia, " +
                                                              "prop.validade AS Validade, " +
                                                              "prop.ordem_servico AS OrdemServico, " +
                                                              "(SELECT MAX(data_aprovacao_item) FROM tb_itens_propostas WHERE id_proposta = prop.id_proposta) AS DataAprovacao " +
                                                              "FROM tb_propostas AS prop " +
                                                              "LEFT JOIN tb_clientes AS clie ON clie.id_cliente = prop.id_cliente " +
                                                              "LEFT JOIN tb_contatos AS cont ON cont.id_contato = prop.id_contato " +
                                                              "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = cont.id_cidade " +
                                                              "LEFT JOIN tb_estados AS esta ON esta.id_estado = cida.id_estado " +
                                                              "LEFT JOIN tb_series AS seri ON seri.id_serie = prop.id_serie " +
                                                              "LEFT JOIN tb_modelos AS mode ON mode.id_modelo = prop.id_modelo " +
                                                              "WHERE id_proposta = @id_proposta; ", CancellationToken.None, "@id_proposta", Proposta?.Id);

                var dtUsuario = await FuncoesDeDatabase.GetDataTable("Usuario", "SELECT " +
                                                                "cida.nome AS Cidade, " +
                                                                "esta.nome AS Estado, " +
                                                                "empr.razao_social AS RazaoSocial, " +
                                                                "empr.site AS Site, " +
                                                                "empr.logo_1 AS Logo1, " +
                                                                "empr.logo_2 AS Logo2, " +
                                                                "empr.logo_3 AS Logo3, " +
                                                                "fili.endereco AS Endereco, " +
                                                                "fili.CEP AS CEP, " +
                                                                "usua.nome AS NomeUsuario, " +
                                                                "usua.telefone AS TelefoneUsuario, " +
                                                                "usua.email AS EmailUsuario " +
                                                                "FROM tb_usuarios AS usua " +
                                                                "LEFT JOIN tb_filiais AS fili ON fili.id_filial = usua.id_filial " +
                                                                "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = fili.id_cidade " +
                                                                "LEFT JOIN tb_estados AS esta ON esta.id_estado = cida.id_estado " +
                                                                "LEFT JOIN tb_empresas AS empr ON empr.id_empresa = fili.id_empresa " +
                                                                "WHERE id_usuario = @id_usuario; ", CancellationToken.None, "@id_usuario", App.Usuario?.Id);

                var dtPecas = await FuncoesDeDatabase.GetDataTable("Pecas", "SELECT " +
                                                                "itpr.descricao_item AS Descricao, " +
                                                                "itpr.codigo_item AS Codigo, " +
                                                                "itpr.ncm_item AS NCM, " +
                                                                "itpr.preco_unitario_final_item AS PrecoUnitarioFinal, " +
                                                                "itpr.quantidade_item AS Quantidade, " +
                                                                "itpr.preco_total_final_item AS PrecoTotalFinal, " +
                                                                "itpr.prazo_final_item AS PrazoFinal, " +
                                                                "itpr.id_tipo_item AS IdTipo, " +
                                                                "IF(itpr.id_conjunto IS NULL, 'CONJUNTO NÃO INFORMADO', conj.nome) AS Conjunto " +
                                                                "FROM tb_itens_propostas AS itpr  " +
                                                                "LEFT JOIN tb_conjuntos AS conj ON conj.id_conjunto = itpr.id_conjunto " +
                                                                "WHERE itpr.id_proposta = @id_proposta AND itpr.id_tipo_item = 1 ORDER BY itpr.id_item_proposta ASC; ", CancellationToken.None, "@id_proposta", Proposta?.Id);

                var dtServicos = await FuncoesDeDatabase.GetDataTable("Servicos", "SELECT " +
                                                                "itpr.descricao_item AS Descricao, " +
                                                                "itpr.preco_unitario_final_item AS PrecoUnitarioFinal, " +
                                                                "itpr.prazo_final_item AS PrazoFinal " +
                                                                "FROM tb_itens_propostas AS itpr  " +
                                                                "LEFT JOIN tb_conjuntos AS conj ON conj.id_conjunto = itpr.id_conjunto " +
                                                                "WHERE itpr.id_proposta = @id_proposta AND itpr.id_tipo_item <> 1 ORDER BY itpr.id_item_proposta ASC;", CancellationToken.None, "@id_proposta", Proposta?.Id);

                ReportViewer.ReportPath = Path.Combine(Environment.CurrentDirectory, @"Resources\RelatorioProposta.rdlc");
                ReportViewer.ProcessingMode = BoldReports.UI.Xaml.ProcessingMode.Local;

                ReportViewer.DataSources.Clear();
                ReportViewer.DataSources.Add(new ReportDataSource { Name = dtProposta.TableName, Value = dtProposta });
                ReportViewer.DataSources.Add(new ReportDataSource { Name = dtUsuario.TableName, Value = dtUsuario });
                ReportViewer.DataSources.Add(new ReportDataSource { Name = dtPecas.TableName, Value = dtPecas });
                ReportViewer.DataSources.Add(new ReportDataSource { Name = dtServicos.TableName, Value = dtServicos });

                ReportViewer.ExportSettings = new()
                {
                    FileName = Proposta?.Cliente?.Nome + " - " + Proposta?.CodigoProposta
                };

                ReportViewer.ViewMode = ViewMode.Print;

                //ReportViewer.ExportSettings.FileName = Proposta?.Cliente?.Nome + " - " +  Proposta?.CodigoProposta;

                //ReportViewer.RefreshReport();

                AtualizaParametros();

                EdicaoHabilitada = Proposta.DataEnvio == null;

                ControlesHabilitados = true;
            }
            catch (Exception)
            {
                TextoStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor";
                ControlesHabilitados = false;
            }
            CarregamentoVisivel = false;
        }

        private async Task ResponderEmailAsync()
        {
            // Atualiza os parâmetros
            AtualizaParametros();

            // Define a pasta a ser salva a proposta temporária
            string pastaTemporaria = System.IO.Path.GetTempPath() + "Proreports\\SGT\\Proposta_PDF";

            // Tenta criar a pasta
            System.IO.Directory.CreateDirectory(pastaTemporaria);

            // Define o caminho do arquivo
            string caminhoArquivo = pastaTemporaria + "\\" + Proposta?.CodigoProposta + ".pdf";

            // Tenta gerar o arquivo da proposta e salva na pasta temporária
            try
            {
                //await GeraArquivoPropostaAsync(caminhoArquivo);
                await SalvarArquivoAsync(caminhoArquivo, WriterFormat.PDF);
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    TextoStatus = "Operação cancelada";
                }
                else
                {
                    Serilog.Log.Error(ex, "Erro ao salvar dados");
                    TextoStatus = "Falha na geração. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                }
                return;
            }

            // Abre a caixa de diálogo de resposta de e-mail
            var customDialog = new CustomDialog();

            var dataContext = new ResponderEmailViewModel(caminhoArquivo, App.Usuario.TextoRespostaEmail, App.Usuario.EmailsEmCopia, "Proposta de Venda - " + Proposta.CodigoProposta, instance =>
            {
                _dialogCoordinator.HideMetroDialogAsync(this, customDialog);

                // Tenta salvar os dados da proposta na database
                try
                {
                    SalvaDadosPropostaAsync().Await();
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error(ex, "Erro ao salvar dados");
                    TextoStatus = "Falha ao atualizar a data de envio e demais informações. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                }

                // Tenta deletar o arquivo após a geração
                try
                {
                    System.IO.File.Delete(caminhoArquivo);
                }
                catch (Exception)
                {
                }
            });

            customDialog.Content = new ResponderEmailView { DataContext = dataContext };

            await _dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        private async Task EnviarEmailAsync()
        {
            // Atualiza o texto
            TextoStatus = "Gerando e-mail, aguarde...";

            // Atualiza os parâmetros
            AtualizaParametros();

            // Define a pasta a ser salva a proposta temporária
            string pastaTemporaria = Path.GetTempPath() + "Proreports\\SGT\\Proposta_PDF";

            // Tenta criar a pasta
            Directory.CreateDirectory(pastaTemporaria);

            // Define o caminho do arquivo
            string caminhoArquivo = pastaTemporaria + "\\" + Proposta?.CodigoProposta + ".pdf";

            // Tenta gerar o arquivo da proposta e salva na pasta temporária
            try
            {
                await SalvarArquivoAsync(caminhoArquivo, WriterFormat.PDF);
            }
            catch (OperationCanceledException)
            {
                TextoStatus = "Operação cancelada";
                return;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao gerar e-mail");

                TextoStatus = "Falha na geração. Detalhes: " + ex.Message;
                return;
            }

            // Gera o e-mail
            await OutlookClasses.GeraEmailEnvioPropostaAsync(caminhoArquivo, App.Usuario.TextoRespostaEmail, App.Usuario.EmailsEmCopia, "Proposta de Venda - " + Proposta.CodigoProposta, CancellationToken.None);

            // Tenta salvar os dados da proposta na database
            try
            {
                await SalvaDadosPropostaAsync();
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao salvar dados");

                TextoStatus = "O arquivo foi gerado, mas houve uma falha ao salvar a data de envio e demais informações na database";
            }

            // Tenta deletar o arquivo após a geração
            try
            {
                File.Delete(caminhoArquivo);
            }
            catch (Exception)
            {
            }

            // Atualiza o texto
            TextoStatus = "E-mail gerado";
        }

        private async Task SalvaDadosPropostaAsync()
        {
            try
            {
                Proposta.DataEnvio = DateTime.Now;
                await Proposta.SalvaDadosEnvioPropostaAsync(CancellationToken.None);
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                propostaInicial.TextoPadrao = Proposta.TextoPadrao;
                propostaInicial.Observacoes = Proposta.Observacoes;
                propostaInicial.PrazoEntrega = Proposta.PrazoEntrega;
                propostaInicial.CondicaoPagamento = Proposta.CondicaoPagamento;
                propostaInicial.Garantia = Proposta.Garantia;
                propostaInicial.Validade = Proposta.Validade;
                propostaInicial.DataEnvio = Proposta.DataEnvio;

                EdicaoHabilitada = Proposta.DataEnvio == null;

                OnPropertyChanged(nameof(PermiteEdicao));
                OnPropertyChanged(nameof(TestePadding));
                OnPropertyChanged(nameof(propostaInicial));
            }
            catch (Exception)
            {
            }
        }

        private async Task SalvarArquivoAsync(string local, WriterFormat writerFormat)
        {
            try
            {
                ReportWriter reportWriter = new(Path.Combine(Environment.CurrentDirectory, @"Resources\RelatorioProposta.rdlc"), ReportViewer.DataSources);

                List<ReportParameter> parameters = new List<ReportParameter>();

                if (ParametroNCM != null)
                {
                    ReportParameter parameterExibeNCM = new ReportParameter();
                    parameterExibeNCM.Name = "exibe_ncm";
                    parameterExibeNCM.Values = new List<string>() { ParametroNCM.Valor == "exibir" ? "True" : "False" };
                    parameters.Add(parameterExibeNCM);
                }

                if (ParametroConjunto != null)
                {
                    ReportParameter parameterExibeConjunto = new ReportParameter();
                    parameterExibeConjunto.Name = "exibe_conjunto";
                    parameterExibeConjunto.Values = new List<string>() { ParametroConjunto.Valor == "exibir" ? "True" : "False" };
                    parameters.Add(parameterExibeConjunto);
                }

                if (ParametroExibicaoCodigos != null)
                {
                    ReportParameter parameterExibeCodigo = new ReportParameter();
                    parameterExibeCodigo.Name = "exibicao_codigo";
                    parameterExibeCodigo.Values = new List<string>() { ParametroExibicaoCodigos.Valor };
                    parameters.Add(parameterExibeCodigo);
                }

                if (ParametroExibicaoDescricao != null)
                {
                    ReportParameter parameterExibeDescricao = new ReportParameter();
                    parameterExibeDescricao.Name = "exibe_descricao_completa";
                    parameterExibeDescricao.Values = new List<string>() { ParametroExibicaoDescricao.Valor == "exibir" ? "True" : "False" };
                    parameters.Add(parameterExibeDescricao);
                }

                ReportParameter parameterTextoCabecalho = new ReportParameter();
                parameterTextoCabecalho.Name = "texto_cabecalho";
                parameterTextoCabecalho.Values = new List<string>() { Proposta?.TextoPadrao == null ? "" : Proposta?.TextoPadrao };
                parameters.Add(parameterTextoCabecalho);

                ReportParameter parameterTextoObservacoes = new ReportParameter();
                parameterTextoObservacoes.Name = "texto_observacoes";
                parameterTextoObservacoes.Values = new List<string>() { Proposta?.Observacoes == null ? "" : Proposta?.Observacoes };
                parameters.Add(parameterTextoObservacoes);

                ReportParameter parameterTextoPrazoEntrega = new ReportParameter();
                parameterTextoPrazoEntrega.Name = "texto_prazo_entrega";
                parameterTextoPrazoEntrega.Values = new List<string>() { Proposta?.PrazoEntrega == null ? "" : Proposta?.PrazoEntrega };
                parameters.Add(parameterTextoPrazoEntrega);

                ReportParameter parameterTextoCondicaoPagamento = new ReportParameter();
                parameterTextoCondicaoPagamento.Name = "texto_condicao_pagamento";
                parameterTextoCondicaoPagamento.Values = new List<string>() { Proposta?.CondicaoPagamento == null ? "" : Proposta?.CondicaoPagamento };
                parameters.Add(parameterTextoCondicaoPagamento);

                ReportParameter parameterTextoGarantia = new ReportParameter();
                parameterTextoGarantia.Name = "texto_garantia";
                parameterTextoGarantia.Values = new List<string>() { Proposta?.Garantia == null ? "" : Proposta?.Garantia };
                parameters.Add(parameterTextoGarantia);

                ReportParameter parameterTextoValidade = new ReportParameter();
                parameterTextoValidade.Name = "texto_validade";
                parameterTextoValidade.Values = new List<string>() { Proposta?.Validade == null ? "" : Proposta?.Validade };
                parameters.Add(parameterTextoValidade);

                reportWriter.SetParameters(parameters);

                reportWriter.Save(local, writerFormat);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao salvar arquivo");
                return;
            }

            try
            {
                await SalvaDadosPropostaAsync();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao salvar dados");
                TextoStatus = "O arquivo foi salvo, porém houve falha ao atualizar a data de envio e demais informações. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                return;
            }

            // Atualiza o texto
            TextoStatus = "Arquivo gerado";
        }

        public void LimparViewModel()
        {
            try
            {
                _parametroNCM = null;
                _parametroConjunto = null;
                _parametroExibicaoCodigos = null;
                _parametroExibicaoDescricao = null;
                _cts = null;
                _proposta = null;

                _listaItensPecas = null;
                _listaItensPecasComConjunto = null;
                _listaItensPecasExibir = null;
                _listaItensServicosEDeslocamentos = null;
                _listaParametroNCM = null;
                _listaParametroConjunto = null;
                _listaParametroExibicaoCodigos = null;
                _listaParametroExibicaoDescricao = null;

                _comandoEnviarEmail = null;
                _comandoResponderEmail = null;
                _comandoSalvar = null;
            }
            catch (Exception)
            {
            }
        }

        private void AtualizaParametros()
        {
            if (ReportViewer != null)
            {
                List<ReportParameter> parameters = new List<ReportParameter>();

                if (ParametroNCM != null)
                {
                    ReportParameter parameterExibeNCM = new ReportParameter();
                    parameterExibeNCM.Name = "exibe_ncm";
                    parameterExibeNCM.Values = new List<string>() { ParametroNCM.Valor == "exibir" ? "True" : "False" };
                    parameters.Add(parameterExibeNCM);
                }

                if (ParametroConjunto != null)
                {
                    ReportParameter parameterExibeConjunto = new ReportParameter();
                    parameterExibeConjunto.Name = "exibe_conjunto";
                    parameterExibeConjunto.Values = new List<string>() { ParametroConjunto.Valor == "exibir" ? "True" : "False" };
                    parameters.Add(parameterExibeConjunto);
                }

                if (ParametroExibicaoCodigos != null)
                {
                    ReportParameter parameterExibeCodigo = new ReportParameter();
                    parameterExibeCodigo.Name = "exibicao_codigo";
                    parameterExibeCodigo.Values = new List<string>() { ParametroExibicaoCodigos.Valor };
                    parameters.Add(parameterExibeCodigo);
                }

                if (ParametroExibicaoDescricao != null)
                {
                    ReportParameter parameterExibeDescricao = new ReportParameter();
                    parameterExibeDescricao.Name = "exibe_descricao_completa";
                    parameterExibeDescricao.Values = new List<string>() { ParametroExibicaoDescricao.Valor == "exibir" ? "True" : "False" };
                    parameters.Add(parameterExibeDescricao);
                }

                ReportParameter parameterTextoCabecalho = new ReportParameter();
                parameterTextoCabecalho.Name = "texto_cabecalho";
                parameterTextoCabecalho.Values = new List<string>() { Proposta?.TextoPadrao == null ? "" : Proposta?.TextoPadrao };
                parameters.Add(parameterTextoCabecalho);

                ReportParameter parameterTextoObservacoes = new ReportParameter();
                parameterTextoObservacoes.Name = "texto_observacoes";
                parameterTextoObservacoes.Values = new List<string>() { Proposta?.Observacoes == null ? "" : Proposta?.Observacoes };
                parameters.Add(parameterTextoObservacoes);

                ReportParameter parameterTextoPrazoEntrega = new ReportParameter();
                parameterTextoPrazoEntrega.Name = "texto_prazo_entrega";
                parameterTextoPrazoEntrega.Values = new List<string>() { Proposta?.PrazoEntrega == null ? "" : Proposta?.PrazoEntrega };
                parameters.Add(parameterTextoPrazoEntrega);

                ReportParameter parameterTextoCondicaoPagamento = new ReportParameter();
                parameterTextoCondicaoPagamento.Name = "texto_condicao_pagamento";
                parameterTextoCondicaoPagamento.Values = new List<string>() { Proposta?.CondicaoPagamento == null ? "" : Proposta?.CondicaoPagamento };
                parameters.Add(parameterTextoCondicaoPagamento);

                ReportParameter parameterTextoGarantia = new ReportParameter();
                parameterTextoGarantia.Name = "texto_garantia";
                parameterTextoGarantia.Values = new List<string>() { Proposta?.Garantia == null ? "" : Proposta?.Garantia };
                parameters.Add(parameterTextoGarantia);

                ReportParameter parameterTextoValidade = new ReportParameter();
                parameterTextoValidade.Name = "texto_validade";
                parameterTextoValidade.Values = new List<string>() { Proposta?.Validade == null ? "" : Proposta?.Validade };
                parameters.Add(parameterTextoValidade);

                ReportViewer.SetParameters(parameters);

                ReportViewer.RefreshReport();
            }
        }

        #endregion Métodos
    }

    public class ItemPropostaExibir : ObservableObject, ICloneable
    {
        private string? _codigoItem;
        private string? _descricaoItem;
        private string? _NCMItem;
        private decimal? _quantidadeItem;
        private decimal? _precoUnitarioFinalItem;
        private decimal? _precoTotalFinalItem;
        private string? _prazoFinalItem;
        private string? _codigoItemAlterado;
        private string? _descricaoItemAlterada;
        private string? _codigoItemExibido;
        private string? _descricaoItemExibida;
        private string? _NCMItemExibido;
        private string? _nomeConjunto;
        private string? _corFundo;
        private string? _tipoTexto;
        private string? _margemTexto;
        private bool _ehConjunto;

        public string? CodigoItem
        {
            get { return _codigoItem; }
            set
            {
                if (value != _codigoItem)
                {
                    _codigoItem = value;
                    OnPropertyChanged(nameof(CodigoItem));
                }
            }
        }

        public string? DescricaoItem
        {
            get { return _descricaoItem; }
            set
            {
                if (value != _descricaoItem)
                {
                    _descricaoItem = value;
                    OnPropertyChanged(nameof(DescricaoItem));
                }
            }
        }

        public string? NCMItem
        {
            get { return _NCMItem; }
            set
            {
                if (value != _NCMItem)
                {
                    _NCMItem = value;
                    OnPropertyChanged(nameof(NCMItem));
                }
            }
        }

        public decimal? QuantidadeItem
        {
            get { return _quantidadeItem; }
            set
            {
                if (value != _quantidadeItem)
                {
                    _quantidadeItem = value;
                    OnPropertyChanged(nameof(QuantidadeItem));
                }
            }
        }

        public decimal? PrecoUnitarioFinalItem
        {
            get { return _precoUnitarioFinalItem; }
            set
            {
                if (value != _precoUnitarioFinalItem)
                {
                    _precoUnitarioFinalItem = value;
                    OnPropertyChanged(nameof(PrecoUnitarioFinalItem));
                }
            }
        }

        public decimal? PrecoTotalFinalItem
        {
            get { return _precoTotalFinalItem; }
            set
            {
                if (value != _precoTotalFinalItem)
                {
                    _precoTotalFinalItem = value;
                    OnPropertyChanged(nameof(PrecoTotalFinalItem));
                }
            }
        }

        public string? PrazoFinalItem
        {
            get { return _prazoFinalItem; }
            set
            {
                if (value != _prazoFinalItem)
                {
                    _prazoFinalItem = value;
                    OnPropertyChanged(nameof(PrazoFinalItem));
                }
            }
        }

        public string? DescricaoItemAlterada
        {
            get { return _descricaoItemAlterada; }
            set
            {
                if (value != _descricaoItemAlterada)
                {
                    _descricaoItemAlterada = value;
                    OnPropertyChanged(nameof(DescricaoItemAlterada));
                }
            }
        }

        public string? CodigoItemAlterado
        {
            get { return _codigoItemAlterado; }
            set
            {
                if (value != _codigoItemAlterado)
                {
                    _codigoItemAlterado = value;
                    OnPropertyChanged(nameof(CodigoItemAlterado));
                }
            }
        }

        public string? DescricaoItemExibida
        {
            get { return _descricaoItemExibida; }
            set
            {
                if (value != _descricaoItemExibida)
                {
                    _descricaoItemExibida = value;
                    OnPropertyChanged(nameof(DescricaoItemExibida));
                }
            }
        }

        public string? CodigoItemExibido
        {
            get { return _codigoItemExibido; }
            set
            {
                if (value != _codigoItemExibido)
                {
                    _codigoItemExibido = value;
                    OnPropertyChanged(nameof(CodigoItemExibido));
                }
            }
        }

        public string? NCMItemExibido
        {
            get { return _NCMItemExibido; }
            set
            {
                if (value != _NCMItemExibido)
                {
                    _NCMItemExibido = value;
                    OnPropertyChanged(nameof(NCMItemExibido));
                }
            }
        }

        public string? NomeConjunto
        {
            get { return _nomeConjunto; }
            set
            {
                if (value != _nomeConjunto)
                {
                    _nomeConjunto = value;
                    OnPropertyChanged(nameof(NomeConjunto));
                }
            }
        }

        public string? CorFundo
        {
            get { return _corFundo; }
            set
            {
                if (value != _corFundo)
                {
                    _corFundo = value;
                    OnPropertyChanged(nameof(CorFundo));
                }
            }
        }

        public string? TipoTexto
        {
            get { return _tipoTexto; }
            set
            {
                if (value != _tipoTexto)
                {
                    _tipoTexto = value;
                    OnPropertyChanged(nameof(TipoTexto));
                }
            }
        }

        public string? MargemTexto
        {
            get { return _margemTexto; }
            set
            {
                if (value != _margemTexto)
                {
                    _margemTexto = value;
                    OnPropertyChanged(nameof(MargemTexto));
                }
            }
        }

        public bool EhConjunto
        {
            get { return _ehConjunto; }
            set
            {
                if (value != _ehConjunto)
                {
                    _ehConjunto = value;
                    OnPropertyChanged(nameof(EhConjunto));
                }
            }
        }

        /// <summary>
        /// Método para criar uma cópia da classe já que esse é um tipo de referência que não pode ser atribuído diretamente
        /// </summary>
        public object Clone()
        {
            ItemPropostaExibir itemPropostaExibirCopia = new();
            itemPropostaExibirCopia.CodigoItem = CodigoItem;
            itemPropostaExibirCopia.CodigoItemAlterado = CodigoItemAlterado;
            itemPropostaExibirCopia.CodigoItemExibido = CodigoItemExibido;
            itemPropostaExibirCopia.DescricaoItem = DescricaoItem;
            itemPropostaExibirCopia.DescricaoItemAlterada = DescricaoItemAlterada;
            itemPropostaExibirCopia.DescricaoItemExibida = DescricaoItemExibida;
            itemPropostaExibirCopia.NCMItem = NCMItem;
            itemPropostaExibirCopia.NCMItemExibido = NCMItemExibido;
            itemPropostaExibirCopia.QuantidadeItem = QuantidadeItem;
            itemPropostaExibirCopia.PrecoUnitarioFinalItem = PrecoUnitarioFinalItem;
            itemPropostaExibirCopia.PrecoTotalFinalItem = PrecoTotalFinalItem;
            itemPropostaExibirCopia.NomeConjunto = NomeConjunto;
            itemPropostaExibirCopia.TipoTexto = TipoTexto;
            itemPropostaExibirCopia.CorFundo = CorFundo;
            itemPropostaExibirCopia.MargemTexto = MargemTexto;

            return itemPropostaExibirCopia;
        }
    }
}