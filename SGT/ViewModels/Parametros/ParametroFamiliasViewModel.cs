using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class ParametroFamiliasViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private Familia _familiaSelecionada = new();
        private Familia _familiaAlterar = new();
        private CancellationTokenSource _cts;

        private Fabricante? _fabricante;
        private Categoria? _categoria;
        private TipoEquipamento? _tipoEquipamento;
        private Classe? _classe;

        private bool _controlesHabilitados;
        private bool _listaHabilitada = true;
        private bool _progressoEhIndeterminavel = true;
        private bool _progressoVisivel = false;
        private double _valorProgresso = 0;
        private string _textoProgresso;
        private string _mensagemStatus;

        private bool _salvarVisivel = false;
        private bool _deletarVisivel;
        private bool _cancelarVisivel;

        private bool _permiteSalvar;
        private bool _permiteAdicionar;
        private bool _permiteEditar;
        private bool _permiteDeletar;
        private bool _permiteCancelar;

        private bool _carregamentoVisivel = true;

        private readonly IDialogCoordinator _dialogCoordinator;

        private ObservableCollection<Familia> _listaFamilias = new();
        private ObservableCollection<Modelo> _listaModelos = new();
        private ObservableCollection<Fabricante> _listaFabricantes = new();
        private ObservableCollection<Categoria> _listaCategorias = new();
        private ObservableCollection<TipoEquipamento> _listaTiposEquipamentos = new();
        private ObservableCollection<Classe> _listaClasses = new();
        private ObservableCollection<Status> _listaStatus = new();

        private ICommand _comandoSalvar;
        private ICommand _comandoAdicionar;
        private ICommand _comandoEditar;
        private ICommand _comandoDeletar;
        private ICommand _comandoCancelar;

        #endregion Campos

        #region Construtores

        public ParametroFamiliasViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;

            DeletarVisivel = App.Usuario.Perfil.Id == 1;

            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Família";
            }
        }

        public string Icone
        {
            get
            {
                return "FamilyTree";
            }
        }

        public void LimparViewModel()
        {
            try
            {
                _familiaSelecionada = null;
                _fabricante = null;
                _tipoEquipamento = null;
                _cts = null;
                _listaFamilias = null;
                _listaModelos = null;
                _listaFabricantes = null;
                _listaTiposEquipamentos = null;
                _listaStatus = null;
                _comandoSalvar = null;
                _comandoAdicionar = null;
                _comandoEditar = null;
                _comandoDeletar = null;
                _comandoCancelar = null;
            }
            catch (Exception)
            {
            }
        }

        public bool ExistemCamposVazios { private get; set; }

        public Familia FamiliaSelecionada
        {
            get { return _familiaSelecionada; }
            set
            {
                _familiaSelecionada = value;
                OnPropertyChanged(nameof(FamiliaSelecionada));
                if (FamiliaSelecionada == null)
                {
                    FamiliaAlterar = null;
                }
                else
                {
                    FamiliaAlterar = (Familia)FamiliaSelecionada.Clone();
                    PermiteEditar = FamiliaAlterar != null;
                    PermiteDeletar = FamiliaAlterar != null;
                    // Define valores
                    try
                    {
                        Fabricante = ListaFabricantes.First(fabr => fabr.Id == FamiliaSelecionada.Modelo.Fabricante.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        Categoria = ListaCategorias.First(cate => cate.Id == FamiliaSelecionada.Modelo.Categoria.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        TipoEquipamento = ListaTiposEquipamentos.First(tieq => tieq.Id == FamiliaSelecionada.Modelo.TipoEquipamento.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        Classe = ListaClasses.First(clas => clas.Id == FamiliaSelecionada.Modelo.Classe.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        FamiliaAlterar.Modelo = ListaModelos.First(mode => mode.Id == FamiliaSelecionada.Modelo.Id);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        FamiliaAlterar.Status = ListaStatus.First(stat => stat.Id == FamiliaSelecionada.Status.Id);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public Familia FamiliaAlterar
        {
            get { return _familiaAlterar; }
            set
            {
                _familiaAlterar = value;
                OnPropertyChanged(nameof(FamiliaAlterar));
            }
        }

        public Fabricante? Fabricante
        {
            get { return _fabricante; }
            set
            {
                if (value != _fabricante)
                {
                    _fabricante = value;
                    OnPropertyChanged(nameof(Fabricante));
                    ListaModelosView.Filter = FiltraModelos;
                    CollectionViewSource.GetDefaultView(ListaModelos).Refresh();
                }
            }
        }

        public Categoria? Categoria
        {
            get { return _categoria; }
            set
            {
                if (value != _categoria)
                {
                    _categoria = value;
                    OnPropertyChanged(nameof(Categoria));
                    ListaModelosView.Filter = FiltraModelos;
                    CollectionViewSource.GetDefaultView(ListaModelos).Refresh();
                }
            }
        }

        public TipoEquipamento? TipoEquipamento
        {
            get { return _tipoEquipamento; }
            set
            {
                if (value != _tipoEquipamento)
                {
                    _tipoEquipamento = value;
                    OnPropertyChanged(nameof(TipoEquipamento));
                    ListaModelosView.Filter = FiltraModelos;
                    CollectionViewSource.GetDefaultView(ListaModelos).Refresh();
                }
            }
        }

        public Classe? Classe
        {
            get { return _classe; }
            set
            {
                if (value != _classe)
                {
                    _classe = value;
                    OnPropertyChanged(nameof(Classe));
                    ListaModelosView.Filter = FiltraModelos;
                    CollectionViewSource.GetDefaultView(ListaModelos).Refresh();
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

        public bool PermiteAdicionar
        {
            get { return _permiteAdicionar; }
            set
            {
                if (value != _permiteAdicionar)
                {
                    _permiteAdicionar = value;
                    OnPropertyChanged(nameof(PermiteAdicionar));
                }
            }
        }

        public bool PermiteEditar
        {
            get { return _permiteEditar; }
            set
            {
                if (value != _permiteEditar)
                {
                    _permiteEditar = value;
                    OnPropertyChanged(nameof(PermiteEditar));
                }
            }
        }

        public bool PermiteDeletar
        {
            get { return _permiteDeletar; }
            set
            {
                if (value != _permiteDeletar)
                {
                    _permiteDeletar = value;
                    OnPropertyChanged(nameof(PermiteDeletar));
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

        public bool DeletarVisivel
        {
            get { return _deletarVisivel; }
            set
            {
                if (value != _deletarVisivel)
                {
                    _deletarVisivel = value;
                    OnPropertyChanged(nameof(DeletarVisivel));
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

        public ObservableCollection<Familia> ListaFamilias
        {
            get { return _listaFamilias; }
            set
            {
                if (value != _listaFamilias)
                {
                    _listaFamilias = value;
                    OnPropertyChanged(nameof(ListaFamilias));
                }
            }
        }

        public ObservableCollection<Modelo> ListaModelos
        {
            get { return _listaModelos; }
            set
            {
                if (value != _listaModelos)
                {
                    _listaModelos = value;
                    OnPropertyChanged(nameof(ListaModelos));
                }
            }
        }

        public ObservableCollection<Categoria> ListaCategorias
        {
            get { return _listaCategorias; }
            set
            {
                if (value != _listaCategorias)
                {
                    _listaCategorias = value;
                    OnPropertyChanged(nameof(ListaCategorias));
                }
            }
        }

        public ObservableCollection<TipoEquipamento> ListaTiposEquipamentos
        {
            get { return _listaTiposEquipamentos; }
            set
            {
                if (value != _listaTiposEquipamentos)
                {
                    _listaTiposEquipamentos = value;
                    OnPropertyChanged(nameof(ListaTiposEquipamentos));
                }
            }
        }

        public ObservableCollection<Classe> ListaClasses
        {
            get { return _listaClasses; }
            set
            {
                if (value != _listaClasses)
                {
                    _listaClasses = value;
                    OnPropertyChanged(nameof(ListaClasses));
                }
            }
        }

        public ObservableCollection<Fabricante> ListaFabricantes
        {
            get { return _listaFabricantes; }
            set
            {
                if (value != _listaFabricantes)
                {
                    _listaFabricantes = value;
                    OnPropertyChanged(nameof(ListaFabricantes));
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

        private CollectionView _listaModelosView;

        public CollectionView ListaModelosView
        {
            get { return _listaModelosView; }
            set
            {
                if (_listaModelosView != value)
                {
                    _listaModelosView = value;
                    OnPropertyChanged(nameof(ListaModelosView));
                }
            }
        }

        //public CollectionView ListaModelosView { get; private set; }

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

        public ICommand ComandoAdicionar
        {
            get
            {
                if (_comandoAdicionar == null)
                {
                    _comandoAdicionar = new RelayCommand(
                        param => Adicionar(),
                        param => true
                    );
                }
                return _comandoAdicionar;
            }
        }

        public ICommand ComandoEditar
        {
            get
            {
                if (_comandoEditar == null)
                {
                    _comandoEditar = new RelayCommand(
                        param => Editar(),
                        param => true
                    );
                }
                return _comandoEditar;
            }
        }

        public ICommand ComandoDeletar
        {
            get
            {
                if (_comandoDeletar == null)
                {
                    _comandoDeletar = new RelayCommand(
                        param => Deletar().Await(),
                        param => true
                    );
                }
                return _comandoDeletar;
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
                CarregamentoVisivel = true;

                // Preenche as listas com as classes necessárias
                await Familia.PreencheListaFamiliasAsync(ListaFamilias, true, null, CancellationToken.None, "ORDER BY fami.familia ASC", "");
                await Modelo.PreencheListaModelosAsync(ListaModelos, true, null, CancellationToken.None, "ORDER BY mode.nome ASC", "");
                await Fabricante.PreencheListaFabricantesAsync(ListaFabricantes, true, null, CancellationToken.None, "ORDER BY fabr.nome ASC", "");
                await Categoria.PreencheListaCategoriasAsync(ListaCategorias, true, null, CancellationToken.None, "ORDER BY cate.nome ASC", "");
                await TipoEquipamento.PreencheListaTiposEquipamentoAsync(ListaTiposEquipamentos, true, null, CancellationToken.None, "ORDER BY tieq.nome ASC", "");
                await Classe.PreencheListaClassesAsync(ListaClasses, true, null, CancellationToken.None, "ORDER BY clas.nome ASC", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");

                ListaModelosView = GetModeloCollectionView(ListaModelos);

                ListaModelosView.Filter = LimpaListaModelos;
                CollectionViewSource.GetDefaultView(ListaModelos).Refresh();

                // Redefine as permissões
                PermiteSalvar = false;
                PermiteAdicionar = true;
                PermiteEditar = false;
                PermiteDeletar = false;
                PermiteCancelar = false;
                CancelarVisivel = false;
                SalvarVisivel = false;
                ListaHabilitada = true;
                ControlesHabilitados = false;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");

                MensagemStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                PermiteSalvar = false;
                PermiteAdicionar = false;
                PermiteEditar = false;
                PermiteDeletar = false;
                PermiteCancelar = false;
                CancelarVisivel = false;
                SalvarVisivel = false;
                ListaHabilitada = false;
                ControlesHabilitados = false;
            }

            CarregamentoVisivel = false;
        }

        private void Adicionar()
        {
            FamiliaAlterar = new Familia();
            FamiliaAlterar.Status = null;
            Fabricante = null;
            Categoria = null;
            TipoEquipamento = null;
            Classe = null;
            FamiliaAlterar.Modelo = null;
            ControlesHabilitados = true;
            ListaHabilitada = false;
            SalvarVisivel = true;
            CancelarVisivel = true;
            PermiteAdicionar = false;
            PermiteEditar = false;
            PermiteCancelar = true;
            PermiteSalvar = true;
            PermiteDeletar = false;
        }

        private void Editar()
        {
            ControlesHabilitados = true;
            ListaHabilitada = false;
            SalvarVisivel = true;
            CancelarVisivel = true;
            PermiteAdicionar = false;
            PermiteEditar = false;
            PermiteCancelar = true;
            PermiteSalvar = true;
            PermiteDeletar = false;
        }

        private void Cancelar()
        {
            if (_cts != null)
                _cts.Cancel();

            ControlesHabilitados = false;
            ListaHabilitada = true;
            SalvarVisivel = false;
            CancelarVisivel = false;
            PermiteAdicionar = true;
            PermiteEditar = FamiliaAlterar != null && FamiliaAlterar?.Id != null; ;
            PermiteCancelar = false;
            PermiteSalvar = false;
            PermiteDeletar = FamiliaAlterar != null && FamiliaAlterar?.Id != null; ;
        }

        private async Task SalvarAsync()
        {
            if (ExistemCamposVazios)
            {
                MensagemStatus = "Campos obrigatórios vazios/inválidos";
                return;
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(false, "SelecaoParametrosHabilitado");

            _cts = new();

            ValorProgresso = 0;
            ListaHabilitada = false;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = true;
            MensagemStatus = "Salvando dados da família '" + FamiliaAlterar.Nome + "', aguarde...";
            CancelarVisivel = true;
            PermiteCancelar = true;
            PermiteSalvar = false;
            PermiteAdicionar = false;
            PermiteEditar = false;
            PermiteDeletar = false;

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
                // Define os dados do modelo
                FamiliaAlterar.Modelo.Fabricante = Fabricante;
                FamiliaAlterar.Modelo.Categoria = Categoria;
                FamiliaAlterar.Modelo.TipoEquipamento = TipoEquipamento;
                FamiliaAlterar.Modelo.Classe = Classe;

                // Salva os dados do modelo
                await FamiliaAlterar.Modelo.SalvarModeloDatabaseAsync(_cts.Token);

                // Verifica se foi solicitado o cancelamento
                _cts.Token.ThrowIfCancellationRequested();

                // Salva a família
                await FamiliaAlterar.SalvarFamiliaDatabaseAsync(CancellationToken.None);
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
                PermiteAdicionar = false;
                PermiteEditar = false;
                PermiteCancelar = true;
                PermiteSalvar = true;
                PermiteDeletar = false;

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

            int? idFamiliaAlterado = FamiliaAlterar.Id;

            try
            {
                await Familia.PreencheListaFamiliasAsync(ListaFamilias, true, null, CancellationToken.None, "ORDER BY fami.familia ASC", "");
            }
            catch (Exception)
            {
            }

            try
            {
                FamiliaSelecionada = ListaFamilias.First(fami => fami.Id == idFamiliaAlterado);
            }
            catch (Exception)
            {
            }

            ValorProgresso = 0;
            ListaHabilitada = true;
            ControlesHabilitados = false;
            ProgressoVisivel = false;
            ProgressoEhIndeterminavel = false;
            SalvarVisivel = false;
            CancelarVisivel = false;
            PermiteCancelar = false;
            PermiteSalvar = false;
            PermiteAdicionar = true;
            PermiteEditar = true;
            PermiteDeletar = true;
            MensagemStatus = "Dados salvos com sucesso!";
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
        }

        private async Task Deletar()
        {
            // Questiona ao usuário se deseja mesmo excluir a proposta
            var mySettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Sim",
                NegativeButtonText = "Não"
            };

            var respostaMensagem = await _dialogCoordinator.ShowMessageAsync(this,
                "Atenção", "Tem certeza que deseja excluir a família '" + FamiliaAlterar.Nome + "'? " +
                "O processo não poderá ser desfeito.",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (respostaMensagem != MessageDialogResult.Affirmative)
            {
                return;
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(false, "SelecaoParametrosHabilitado");

            _cts = new();

            ValorProgresso = 0;
            ListaHabilitada = false;
            ControlesHabilitados = false;
            ProgressoVisivel = true;
            ProgressoEhIndeterminavel = true;
            MensagemStatus = "Deletando família '" + FamiliaAlterar.Nome + "', aguarde...";
            CancelarVisivel = true;
            PermiteCancelar = true;
            PermiteSalvar = false;
            PermiteAdicionar = false;
            PermiteEditar = false;
            PermiteDeletar = false;

            try
            {
                await Task.Delay(3000, _cts.Token);
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
                await FamiliaAlterar.DeletarFamiliaDatabaseAsync(_cts.Token);
            }
            catch (Exception ex)
            {
                ValorProgresso = 0;
                ListaHabilitada = true;
                ControlesHabilitados = false;
                ProgressoVisivel = false;
                ProgressoEhIndeterminavel = true;

                CancelarVisivel = false;
                PermiteCancelar = false;
                PermiteSalvar = false;
                PermiteAdicionar = true;
                PermiteEditar = true;
                PermiteDeletar = true;

                if (ex is OperationCanceledException)
                {
                    MensagemStatus = "Operação cancelada";
                }
                else
                {
                    if (ex is ValorNaoExisteException || ex is ChaveEstrangeiraEmUsoException)
                    {
                        MensagemStatus = ex.Message;
                    }
                    else
                    {
                        Serilog.Log.Error(ex, "Erro ao excluir dados");
                        MensagemStatus = "Falha ao excluir os dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                    }
                }

                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
                return;
            }

            FamiliaSelecionada = null;
            ValorProgresso = 0;
            ListaHabilitada = true;
            ControlesHabilitados = false;
            ProgressoVisivel = false;
            ProgressoEhIndeterminavel = false;
            SalvarVisivel = false;
            CancelarVisivel = false;
            PermiteCancelar = false;
            PermiteSalvar = false;
            PermiteAdicionar = true;
            PermiteEditar = false;
            PermiteDeletar = false;
            MensagemStatus = "Família excluída com sucesso!";

            try
            {
                await Familia.PreencheListaFamiliasAsync(ListaFamilias, true, null, CancellationToken.None, "ORDER BY fami.familia ASC", "");
            }
            catch (Exception)
            {
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<bool>(true, "SelecaoParametrosHabilitado");
        }

        public bool FiltraModelos(object item)
        {
            if (Fabricante == null || TipoEquipamento == null)
            {
                return false;
            }
            if (item is Modelo modelo)
            {
                return modelo.Fabricante.Id == Fabricante.Id && modelo.TipoEquipamento.Id == TipoEquipamento.Id;
            }
            return true;
        }

        public bool LimpaListaModelos(object item)
        {
            return false;
        }

        #endregion Métodos

        #region Classes

        public CollectionView GetModeloCollectionView(ObservableCollection<Modelo> modeloList)
        {
            return (CollectionView)CollectionViewSource.GetDefaultView(modeloList);
        }

        #endregion Classes
    }
}