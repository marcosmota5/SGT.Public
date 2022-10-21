using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class ParametroColunasImportacaoViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private Fornecedor _fornecedorSelecionado = new();
        private CancellationTokenSource _cts;

        private bool _controlesHabilitados;
        private bool _listaHabilitada = true;
        private bool _progressoEhIndeterminavel = true;
        private bool _progressoVisivel = false;
        private double _valorProgresso = 0;
        private string _textoProgresso;
        private string _mensagemStatus;

        private bool _salvarVisivel = false;
        private bool _cancelarVisivel;

        private bool _permiteSalvar;
        private bool _permiteCancelar;

        private bool _carregamentoVisivel = true;

        private readonly IDialogCoordinator _dialogCoordinator;

        private ObservableCollection<Fornecedor> _listaFornecedores = new();
        private ObservableCollection<ColunaImportacaoCotacao> _listaColunasImportacao = new();

        private ICommand _comandoSalvar;
        private ICommand _comandoCancelar;

        #endregion Campos

        #region Construtores

        public ParametroColunasImportacaoViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;

            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Colunas para importação";
            }
        }

        public string Icone
        {
            get
            {
                return "FormatColumns";
            }
        }

        public void LimparViewModel()
        {
            try
            {
                _fornecedorSelecionado = null;
                _cts = null;
                _listaFornecedores = null;
                _listaColunasImportacao = null;
                _comandoSalvar = null;
                _comandoCancelar = null;
            }
            catch (Exception)
            {
            }
        }

        public bool ExistemCamposVazios { private get; set; }

        public Fornecedor FornecedorSelecionado
        {
            get { return _fornecedorSelecionado; }
            set
            {
                _fornecedorSelecionado = value;
                OnPropertyChanged(nameof(FornecedorSelecionado));

                PermiteSalvar = FornecedorSelecionado != null;

                PreencheListaColunas().Await();
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

        public bool ListaHabilitada
        {
            get { return _listaHabilitada; }
            set
            {
                if (value != _listaHabilitada)
                {
                    _listaHabilitada = value;
                    OnPropertyChanged(nameof(ListaHabilitada));
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

        public bool SalvarVisivel
        {
            get { return _salvarVisivel; }
            set
            {
                if (value != _salvarVisivel)
                {
                    _salvarVisivel = value;
                    OnPropertyChanged(nameof(SalvarVisivel));
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

        public ObservableCollection<ColunaImportacaoCotacao> ListaColunasImportacao
        {
            get { return _listaColunasImportacao; }
            set
            {
                if (value != _listaColunasImportacao)
                {
                    _listaColunasImportacao = value;
                    OnPropertyChanged(nameof(ListaColunasImportacao));
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

        public async Task ConstrutorAsync()
        {
            try
            {
                // Preenche as listas com as classes necessárias
                await Fornecedor.PreencheListaFornecedoresAsync(ListaFornecedores, true, null, CancellationToken.None, "ORDER BY forn.nome ASC", "");

                // Redefine as permissões
                PermiteSalvar = false;
                PermiteCancelar = false;
                CancelarVisivel = false;
                SalvarVisivel = true;
                ListaHabilitada = true;
                ControlesHabilitados = true;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");

                MensagemStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                PermiteSalvar = false;
                PermiteCancelar = false;
                CancelarVisivel = false;
                SalvarVisivel = false;
                ListaHabilitada = false;
                ControlesHabilitados = false;
            }

            CarregamentoVisivel = false;
        }

        private void Cancelar()
        {
            if (_cts != null)
                _cts.Cancel();

            ControlesHabilitados = true;
            ListaHabilitada = true;
            SalvarVisivel = true;
            CancelarVisivel = false;
            PermiteCancelar = false;
            PermiteSalvar = FornecedorSelecionado != null;
        }

        private async Task PreencheListaColunas()
        {
            try
            {
                await ColunaImportacaoCotacao.PreencheListaColunasImportacaoCotacaoAsync(ListaColunasImportacao, true, null, CancellationToken.None, "WHERE coic.id_fornecedor = @id_fornecedor ORDER BY coic.nome_coluna_sistema ASC", "@id_fornecedor", FornecedorSelecionado?.Id);
            }
            catch (Exception)
            {
            }

            if (ListaColunasImportacao.Count == 0)
            {
                ListaColunasImportacao.Add(new ColunaImportacaoCotacao
                {
                    Id = null,
                    NomeColunaSistema = "codigo_item",
                    NomeColunaSistemaFormatado = "Código",
                    ColunaExiste = false,
                    NomeColunaCotacao = "",
                    Fornecedor = FornecedorSelecionado
                });

                ListaColunasImportacao.Add(new ColunaImportacaoCotacao
                {
                    Id = null,
                    NomeColunaSistema = "descricao_item",
                    NomeColunaSistemaFormatado = "Descrição",
                    ColunaExiste = false,
                    NomeColunaCotacao = "",
                    Fornecedor = FornecedorSelecionado
                });

                ListaColunasImportacao.Add(new ColunaImportacaoCotacao
                {
                    Id = null,
                    NomeColunaSistema = "quantidade_item",
                    NomeColunaSistemaFormatado = "Quantidade",
                    ColunaExiste = false,
                    NomeColunaCotacao = "",
                    Fornecedor = FornecedorSelecionado
                });

                ListaColunasImportacao.Add(new ColunaImportacaoCotacao
                {
                    Id = null,
                    NomeColunaSistema = "preco_unitario_inicial_item",
                    NomeColunaSistemaFormatado = "Preço unitário inicial",
                    ColunaExiste = false,
                    NomeColunaCotacao = "",
                    Fornecedor = FornecedorSelecionado
                });

                ListaColunasImportacao.Add(new ColunaImportacaoCotacao
                {
                    Id = null,
                    NomeColunaSistema = "percentual_ipi_item",
                    NomeColunaSistemaFormatado = "% IPI",
                    ColunaExiste = false,
                    NomeColunaCotacao = "",
                    Fornecedor = FornecedorSelecionado
                });

                ListaColunasImportacao.Add(new ColunaImportacaoCotacao
                {
                    Id = null,
                    NomeColunaSistema = "percentual_icms_item",
                    NomeColunaSistemaFormatado = "% ICMS",
                    ColunaExiste = false,
                    NomeColunaCotacao = "",
                    Fornecedor = FornecedorSelecionado
                });

                ListaColunasImportacao.Add(new ColunaImportacaoCotacao
                {
                    Id = null,
                    NomeColunaSistema = "ncm_item",
                    NomeColunaSistemaFormatado = "NCM",
                    ColunaExiste = false,
                    NomeColunaCotacao = "",
                    Fornecedor = FornecedorSelecionado
                });

                ListaColunasImportacao.Add(new ColunaImportacaoCotacao
                {
                    Id = null,
                    NomeColunaSistema = "mva_item",
                    NomeColunaSistemaFormatado = "MVA",
                    ColunaExiste = false,
                    NomeColunaCotacao = "",
                    Fornecedor = FornecedorSelecionado
                });

                ListaColunasImportacao.Add(new ColunaImportacaoCotacao
                {
                    Id = null,
                    NomeColunaSistema = "valor_st_item",
                    NomeColunaSistemaFormatado = "Valor ST",
                    ColunaExiste = false,
                    NomeColunaCotacao = "",
                    Fornecedor = FornecedorSelecionado
                });

                ListaColunasImportacao.Add(new ColunaImportacaoCotacao
                {
                    Id = null,
                    NomeColunaSistema = "valor_total_inicial_item",
                    NomeColunaSistemaFormatado = "Preço total inicial",
                    ColunaExiste = false,
                    NomeColunaCotacao = "",
                    Fornecedor = FornecedorSelecionado
                });

                ListaColunasImportacao.Add(new ColunaImportacaoCotacao
                {
                    Id = null,
                    NomeColunaSistema = "prazo_inicial_item",
                    NomeColunaSistemaFormatado = "Prazo inicial",
                    ColunaExiste = false,
                    NomeColunaCotacao = "",
                    Fornecedor = FornecedorSelecionado
                });
            }
        }

        private async Task SalvarAsync()
        {
            if (FornecedorSelecionado == null)
            {
                MensagemStatus = "Selecione o fornecedor";
                return;
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(false, "SelecaoParametrosHabilitado");

            _cts = new();

            ValorProgresso = 0;
            ListaHabilitada = false;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = true;
            MensagemStatus = "Salvando dados das colunas para o fornecedor '" + FornecedorSelecionado.Nome + "', aguarde...";
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
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = true;
                MensagemStatus = "Operação cancelada";
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
                return;
            }

            try
            {
                foreach (var item in ListaColunasImportacao)
                {
                    await item.SalvarColunaImportacaoCotacaoDatabaseAsync(CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                ValorProgresso = 0;
                ControlesHabilitados = true;
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = true;

                ListaHabilitada = false;
                SalvarVisivel = true;
                CancelarVisivel = true;
                PermiteCancelar = true;
                PermiteSalvar = FornecedorSelecionado != null;

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

                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
                return;
            }

            ValorProgresso = 0;
            ListaHabilitada = true;
            ControlesHabilitados = true;
            ProgressoVisivel = false;
            ProgressoEhIndeterminavel = false;
            SalvarVisivel = true;
            CancelarVisivel = false;
            PermiteCancelar = false;
            PermiteSalvar = FornecedorSelecionado != null;
            MensagemStatus = "Dados salvos com sucesso!";
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
        }

        #endregion Métodos
    }
}