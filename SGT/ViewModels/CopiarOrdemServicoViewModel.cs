using GalaSoft.MvvmLight.Messaging;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class CopiarOrdemServicoViewModel : ObservableObject, IPageViewModel
    {
        #region Enums

        public enum TipoCopia
        {
            ParaOrdemServico,
            ParaProposta
        }

        #endregion Enums

        #region Campos

        private string _mensagemStatus;
        private bool _controlesHabilitados;
        private bool _carregamentoVisivel = true;

        private OrdemServico _ordemServico;

        private bool _ehParaOrdemServico;
        private bool _dadosOrdemServico;
        private bool _usarComoPrimaria;
        private bool _dadosChamado;
        private bool _dadosCliente;
        private bool _dadosEquipamento;
        private bool _dadosStatusEquipamento;
        private bool _dadosDescricaoAtendimento;
        private bool _dadosAtendimento;
        private bool _dadosComentario;
        private ObservableCollection<ItemOrdemServico> _listaItensOrdemServicoDisponiveis = new();
        private ObservableCollection<ItemOrdemServico> _listaItensOrdemServicoACopiar = new();
        private ObservableCollection<EventoOrdemServico> _listaEventosOrdemServicoDisponiveis = new();
        private ObservableCollection<EventoOrdemServico> _listaEventosOrdemServicoACopiar = new();
        private ObservableCollection<InconsistenciaOrdemServico> _listaInconsistenciasOrdemServicoDisponiveis = new();
        private ObservableCollection<InconsistenciaOrdemServico> _listaInconsistenciasOrdemServicoACopiar = new();

        private ICommand _comandoConfirmar;
        private ICommand _comandoPassarTodosItensDireita;
        private ICommand _comandoPassarUmItemDireita;
        private ICommand _comandoPassarUmItemEsquerda;
        private ICommand _comandoPassarTodosItensEsquerda;
        private ICommand _comandoPassarTodosEventosDireita;
        private ICommand _comandoPassarUmEventoDireita;
        private ICommand _comandoPassarUmEventoEsquerda;
        private ICommand _comandoPassarTodosEventosEsquerda;
        private ICommand _comandoPassarTodosInconsistenciasDireita;
        private ICommand _comandoPassarUmInconsistenciaDireita;
        private ICommand _comandoPassarUmInconsistenciaEsquerda;
        private ICommand _comandoPassarTodosInconsistenciasEsquerda;

        #endregion Campos

        #region Construtores

        /// <summary>
        /// Construtor do controle do item
        /// </summary>
        /// <param name="ordemServico">Ordem de serviço que terá o item inserido ou editado</param>
        /// <param name="closeHandler">Ação para fechar a caixa de diálogo</param>
        public CopiarOrdemServicoViewModel(OrdemServico ordemServico, TipoCopia tipoCopia, Action<CopiarOrdemServicoViewModel> closeHandler)
        {
            EhParaOrdemServico = tipoCopia == TipoCopia.ParaOrdemServico;

            // Atribui o método de limpar listas e a ação de fechar a caixa de diálogo ao comando
            this.ComandoFechar = new SimpleCommand(o => true, o =>
            {
                closeHandler(this);
            });

            _ordemServico = ordemServico;

            foreach (var item in _ordemServico.ListaItensOrdemServico)
            {
                ListaItensOrdemServicoDisponiveis.Add(item);
            }

            foreach (var item in _ordemServico.ListaEventosOrdemServico)
            {
                ListaEventosOrdemServicoDisponiveis.Add(item);
            }

            foreach (var item in _ordemServico.ListaInconsistenciasOrdemServico)
            {
                ListaInconsistenciasOrdemServicoDisponiveis.Add(item);
            }
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Copiar ordem de serviço";
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
                _listaItensOrdemServicoDisponiveis = null;
                _listaItensOrdemServicoACopiar = null;
                _listaEventosOrdemServicoDisponiveis = null;
                _listaEventosOrdemServicoACopiar = null;
                _listaInconsistenciasOrdemServicoDisponiveis = null;
                _listaInconsistenciasOrdemServicoACopiar = null;

                _comandoConfirmar = null;
                _comandoPassarTodosItensDireita = null;
                _comandoPassarUmItemDireita = null;
                _comandoPassarUmItemEsquerda = null;
                _comandoPassarTodosItensEsquerda = null;
            }
            catch (Exception)
            {
            }
        }

        public ICommand ComandoFechar { get; }

        public ICommand ComandoConfirmar
        {
            get
            {
                if (_comandoConfirmar == null)
                {
                    _comandoConfirmar = new RelayCommand(
                        param => ConfirmarCopia().Await(),
                        param => true
                    );
                }

                return _comandoConfirmar;
            }
        }

        public ICommand ComandoPassarTodosItensDireita
        {
            get
            {
                if (_comandoPassarTodosItensDireita == null)
                {
                    _comandoPassarTodosItensDireita = new RelayCommand(
                        param => PassarTodosItensDireita(),
                        param => true
                    );
                }
                return _comandoPassarTodosItensDireita;
            }
        }

        public ICommand ComandoPassarUmItemDireita
        {
            get
            {
                if (_comandoPassarUmItemDireita == null)
                {
                    _comandoPassarUmItemDireita = new RelayCommand(
                        param => PassarUmItemDireita(),
                        param => true
                    );
                }
                return _comandoPassarUmItemDireita;
            }
        }

        public ICommand ComandoPassarTodosItensEsquerda
        {
            get
            {
                if (_comandoPassarTodosItensEsquerda == null)
                {
                    _comandoPassarTodosItensEsquerda = new RelayCommand(
                        param => PassarTodosItensEsquerda(),
                        param => true
                    );
                }
                return _comandoPassarTodosItensEsquerda;
            }
        }

        public ICommand ComandoPassarUmItemEsquerda
        {
            get
            {
                if (_comandoPassarUmItemEsquerda == null)
                {
                    _comandoPassarUmItemEsquerda = new RelayCommand(
                        param => PassarUmItemEsquerda(),
                        param => true
                    );
                }
                return _comandoPassarUmItemEsquerda;
            }
        }

        public ICommand ComandoPassarTodosEventosDireita
        {
            get
            {
                if (_comandoPassarTodosEventosDireita == null)
                {
                    _comandoPassarTodosEventosDireita = new RelayCommand(
                        param => PassarTodosEventosDireita(),
                        param => true
                    );
                }
                return _comandoPassarTodosEventosDireita;
            }
        }

        public ICommand ComandoPassarUmEventoDireita
        {
            get
            {
                if (_comandoPassarUmEventoDireita == null)
                {
                    _comandoPassarUmEventoDireita = new RelayCommand(
                        param => PassarUmEventoDireita(),
                        param => true
                    );
                }
                return _comandoPassarUmEventoDireita;
            }
        }

        public ICommand ComandoPassarTodosEventosEsquerda
        {
            get
            {
                if (_comandoPassarTodosEventosEsquerda == null)
                {
                    _comandoPassarTodosEventosEsquerda = new RelayCommand(
                        param => PassarTodosEventosEsquerda(),
                        param => true
                    );
                }
                return _comandoPassarTodosEventosEsquerda;
            }
        }

        public ICommand ComandoPassarUmEventoEsquerda
        {
            get
            {
                if (_comandoPassarUmEventoEsquerda == null)
                {
                    _comandoPassarUmEventoEsquerda = new RelayCommand(
                        param => PassarUmEventoEsquerda(),
                        param => true
                    );
                }
                return _comandoPassarUmEventoEsquerda;
            }
        }

        public ICommand ComandoPassarTodosInconsistenciasDireita
        {
            get
            {
                if (_comandoPassarTodosInconsistenciasDireita == null)
                {
                    _comandoPassarTodosInconsistenciasDireita = new RelayCommand(
                        param => PassarTodosInconsistenciasDireita(),
                        param => true
                    );
                }
                return _comandoPassarTodosInconsistenciasDireita;
            }
        }

        public ICommand ComandoPassarUmInconsistenciaDireita
        {
            get
            {
                if (_comandoPassarUmInconsistenciaDireita == null)
                {
                    _comandoPassarUmInconsistenciaDireita = new RelayCommand(
                        param => PassarUmInconsistenciaDireita(),
                        param => true
                    );
                }
                return _comandoPassarUmInconsistenciaDireita;
            }
        }

        public ICommand ComandoPassarTodosInconsistenciasEsquerda
        {
            get
            {
                if (_comandoPassarTodosInconsistenciasEsquerda == null)
                {
                    _comandoPassarTodosInconsistenciasEsquerda = new RelayCommand(
                        param => PassarTodosInconsistenciasEsquerda(),
                        param => true
                    );
                }
                return _comandoPassarTodosInconsistenciasEsquerda;
            }
        }

        public ICommand ComandoPassarUmInconsistenciaEsquerda
        {
            get
            {
                if (_comandoPassarUmInconsistenciaEsquerda == null)
                {
                    _comandoPassarUmInconsistenciaEsquerda = new RelayCommand(
                        param => PassarUmInconsistenciaEsquerda(),
                        param => true
                    );
                }
                return _comandoPassarUmInconsistenciaEsquerda;
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

        public bool EhParaOrdemServico
        {
            get { return _ehParaOrdemServico; }
            set
            {
                if (value != _ehParaOrdemServico)
                {
                    _ehParaOrdemServico = value;
                    OnPropertyChanged(nameof(EhParaOrdemServico));
                }
            }
        }

        public bool DadosOrdemServico
        {
            get { return _dadosOrdemServico; }
            set
            {
                if (value != _dadosOrdemServico)
                {
                    _dadosOrdemServico = value;
                    OnPropertyChanged(nameof(DadosOrdemServico));
                }
            }
        }

        public bool UsarComoPrimaria
        {
            get { return _usarComoPrimaria; }
            set
            {
                if (value != _usarComoPrimaria)
                {
                    _usarComoPrimaria = value;
                    OnPropertyChanged(nameof(UsarComoPrimaria));
                }
            }
        }

        public bool DadosChamado
        {
            get { return _dadosChamado; }
            set
            {
                if (value != _dadosChamado)
                {
                    _dadosChamado = value;
                    OnPropertyChanged(nameof(DadosChamado));
                }
            }
        }

        public bool DadosCliente
        {
            get { return _dadosCliente; }
            set
            {
                if (value != _dadosCliente)
                {
                    _dadosCliente = value;
                    OnPropertyChanged(nameof(DadosCliente));
                }
            }
        }

        public bool DadosEquipamento
        {
            get { return _dadosEquipamento; }
            set
            {
                if (value != _dadosEquipamento)
                {
                    _dadosEquipamento = value;
                    OnPropertyChanged(nameof(DadosEquipamento));
                }
            }
        }

        public bool DadosStatusEquipamento
        {
            get { return _dadosStatusEquipamento; }
            set
            {
                if (value != _dadosStatusEquipamento)
                {
                    _dadosStatusEquipamento = value;
                    OnPropertyChanged(nameof(DadosStatusEquipamento));
                }
            }
        }

        public bool DadosDescricaoAtendimento
        {
            get { return _dadosDescricaoAtendimento; }
            set
            {
                if (value != _dadosDescricaoAtendimento)
                {
                    _dadosDescricaoAtendimento = value;
                    OnPropertyChanged(nameof(DadosDescricaoAtendimento));
                }
            }
        }

        public bool DadosAtendimento
        {
            get { return _dadosAtendimento; }
            set
            {
                if (value != _dadosAtendimento)
                {
                    _dadosAtendimento = value;
                    OnPropertyChanged(nameof(DadosAtendimento));
                }
            }
        }

        public bool DadosComentario
        {
            get { return _dadosComentario; }
            set
            {
                if (value != _dadosComentario)
                {
                    _dadosComentario = value;
                    OnPropertyChanged(nameof(DadosComentario));
                }
            }
        }

        public ObservableCollection<ItemOrdemServico> ListaItensOrdemServicoDisponiveis
        {
            get { return _listaItensOrdemServicoDisponiveis; }
            set
            {
                if (value != _listaItensOrdemServicoDisponiveis)
                {
                    _listaItensOrdemServicoDisponiveis = value;
                    OnPropertyChanged(nameof(ListaItensOrdemServicoDisponiveis));
                }
            }
        }

        public List<object> ListaItensOrdemServicoDisponiveisSelecionados { private get; set; }

        public List<object> ListaItensOrdemServicoCopiadosSelecionados { private get; set; }

        public ObservableCollection<ItemOrdemServico> ListaItensOrdemServicoACopiar
        {
            get { return _listaItensOrdemServicoACopiar; }
            set
            {
                if (value != _listaItensOrdemServicoACopiar)
                {
                    _listaItensOrdemServicoACopiar = value;
                    OnPropertyChanged(nameof(ListaItensOrdemServicoACopiar));
                }
            }
        }

        public ObservableCollection<EventoOrdemServico> ListaEventosOrdemServicoDisponiveis
        {
            get { return _listaEventosOrdemServicoDisponiveis; }
            set
            {
                if (value != _listaEventosOrdemServicoDisponiveis)
                {
                    _listaEventosOrdemServicoDisponiveis = value;
                    OnPropertyChanged(nameof(ListaEventosOrdemServicoDisponiveis));
                }
            }
        }

        public List<object> ListaEventosOrdemServicoDisponiveisSelecionados { private get; set; }

        public List<object> ListaEventosOrdemServicoCopiadosSelecionados { private get; set; }

        public ObservableCollection<EventoOrdemServico> ListaEventosOrdemServicoACopiar
        {
            get { return _listaEventosOrdemServicoACopiar; }
            set
            {
                if (value != _listaEventosOrdemServicoACopiar)
                {
                    _listaEventosOrdemServicoACopiar = value;
                    OnPropertyChanged(nameof(ListaEventosOrdemServicoACopiar));
                }
            }
        }

        public ObservableCollection<InconsistenciaOrdemServico> ListaInconsistenciasOrdemServicoDisponiveis
        {
            get { return _listaInconsistenciasOrdemServicoDisponiveis; }
            set
            {
                if (value != _listaInconsistenciasOrdemServicoDisponiveis)
                {
                    _listaInconsistenciasOrdemServicoDisponiveis = value;
                    OnPropertyChanged(nameof(ListaInconsistenciasOrdemServicoDisponiveis));
                }
            }
        }

        public List<object> ListaInconsistenciasOrdemServicoDisponiveisSelecionados { private get; set; }

        public List<object> ListaInconsistenciasOrdemServicoCopiadosSelecionados { private get; set; }

        public ObservableCollection<InconsistenciaOrdemServico> ListaInconsistenciasOrdemServicoACopiar
        {
            get { return _listaInconsistenciasOrdemServicoACopiar; }
            set
            {
                if (value != _listaInconsistenciasOrdemServicoACopiar)
                {
                    _listaInconsistenciasOrdemServicoACopiar = value;
                    OnPropertyChanged(nameof(ListaInconsistenciasOrdemServicoACopiar));
                }
            }
        }

        #endregion Propriedades/Comandos

        #region Métodos

        private async Task ConfirmarCopia()
        {
            if (EhParaOrdemServico)
            {
                await ConfirmarCopiaParaOrdemServico();
            }
            else
            {
                await ConfirmarCopiaParaProposta();
            }
        }

        /// <summary>
        /// Método assíncrono para salvar o item da proposta
        /// </summary>
        private async Task ConfirmarCopiaParaOrdemServico()
        {
            OrdemServico ordemServico = new(true, true, true);

            ordemServico.IdUsuarioEmUso = App.Usuario.Id;

            ordemServico.Filial = App.Usuario.Filial;
            ordemServico.EtapasConcluidas = 1;
            ordemServico.UsuarioInsercao = (Usuario)App.Usuario.Clone();

            if (DadosOrdemServico)
            {
                ordemServico.TipoOrdemServico = (TipoOrdemServico)_ordemServico.TipoOrdemServico.Clone();
                ordemServico.Status = (Status)_ordemServico.Status.Clone();
            }

            if (UsarComoPrimaria)
            {
                ordemServico.OrdemServicoPrimaria = _ordemServico.OrdemServicoAtual;
            }

            if (DadosChamado)
            {
                ordemServico.NumeroChamado = _ordemServico.NumeroChamado;
                ordemServico.DataChamado = _ordemServico.DataChamado;
            }

            if (DadosCliente)
            {
                ordemServico.Cliente = (Cliente)_ordemServico.Cliente.Clone();
                ordemServico.Frota = (Frota)_ordemServico.Frota.Clone();
                ordemServico.DataAtendimento = _ordemServico.DataAtendimento;
            }

            if (DadosEquipamento)
            {
                ordemServico.Serie = (Serie)_ordemServico.Serie.Clone();
                ordemServico.Mastro = _ordemServico.Mastro;
                ordemServico.CodigoFalha = _ordemServico.CodigoFalha;
            }

            if (DadosStatusEquipamento)
            {
                ordemServico.EquipamentoOperacional = _ordemServico.EquipamentoOperacional;
                ordemServico.Horimetro = _ordemServico.Horimetro;
                ordemServico.StatusEquipamentoAposManutencao = (StatusEquipamentoAposManutencao)_ordemServico.StatusEquipamentoAposManutencao.Clone();
                ordemServico.TipoManutencao = (TipoManutencao)_ordemServico.TipoManutencao.Clone();
                ordemServico.UsoIndevido = (UsoIndevido)_ordemServico.UsoIndevido.Clone();
                ordemServico.HorasPreventiva = _ordemServico.HorasPreventiva;
                ordemServico.OutroTipoManutencao = _ordemServico.OutroTipoManutencao;
            }

            if (DadosDescricaoAtendimento)
            {
                ordemServico.MotivoAtendimento = _ordemServico.MotivoAtendimento;
                ordemServico.EntrevistaInicial = _ordemServico.EntrevistaInicial;
                ordemServico.Intervencao = _ordemServico.Intervencao;
            }

            if (DadosAtendimento)
            {
                ordemServico.DataSaida = _ordemServico.DataSaida;
                ordemServico.DataChegada = _ordemServico.DataChegada;
                ordemServico.DataRetorno = _ordemServico.DataRetorno;
                ordemServico.ExecutanteServico = (ExecutanteServico)_ordemServico.ExecutanteServico.Clone();
            }

            if (DadosComentario)
            {
                ordemServico.Comentarios = _ordemServico.Comentarios;
            }

            foreach (var item in ListaItensOrdemServicoACopiar)
            {
                item.Id = null;
                item.Usuario = (Usuario)App.Usuario.Clone();
                item.DataInsercao = DateTime.Now;
                item.DataEdicaoItem = null;
                await item.Status.GetStatusDatabaseAsync(1, CancellationToken.None);

                ordemServico.ListaItensOrdemServico.Add(item);
            }

            foreach (var item in ListaEventosOrdemServicoACopiar)
            {
                item.Id = null;
                item.Usuario = (Usuario)App.Usuario.Clone();
                item.DataInsercao = DateTime.Now;
                item.DataEdicaoItem = null;
                await item.Status.GetStatusDatabaseAsync(1, CancellationToken.None);

                ordemServico.ListaEventosOrdemServico.Add(item);
            }

            foreach (var item in ListaInconsistenciasOrdemServicoACopiar)
            {
                item.Id = null;
                item.Usuario = (Usuario)App.Usuario.Clone();
                item.DataInsercao = DateTime.Now;
                item.DataEdicaoItem = null;
                await item.Status.GetStatusDatabaseAsync(1, CancellationToken.None);

                ordemServico.ListaInconsistenciasOrdemServico.Add(item);
            }

            Messenger.Default.Send<OrdemServico>(ordemServico, "OrdemServicoCopiar");

            // Executa o comando que fecha a caixa de diálogo
            ComandoFechar.Execute(null);
        }

        /// <summary>
        /// Método assíncrono para salvar o item da proposta
        /// </summary>
        private async Task ConfirmarCopiaParaProposta()
        {
            Proposta proposta = new(true, true, true);

            proposta.IdUsuarioEmUso = App.Usuario.Id;

            proposta.Filial = App.Usuario.Filial;
            proposta.UsuarioInsercao = (Usuario)App.Usuario.Clone();

            if (DadosChamado)
            {
                proposta.DataSolicitacao = _ordemServico.DataChamado;
            }

            if (DadosCliente)
            {
                proposta.Cliente = (Cliente)_ordemServico.Cliente.Clone();
                proposta.Status = (Status)_ordemServico.Status.Clone();
            }

            if (DadosEquipamento)
            {
                proposta.Serie = (Serie)_ordemServico.Serie.Clone();
                proposta.Horimetro = _ordemServico.Horimetro;
                proposta.OrdemServico = _ordemServico.OrdemServicoAtual;
            }

            if (DadosComentario)
            {
                proposta.Comentarios = _ordemServico.Comentarios;
            }

            foreach (var item in ListaItensOrdemServicoACopiar)
            {
                ItemProposta itemProposta = new(true, true);

                itemProposta.CodigoItem = item.CodigoItem;
                itemProposta.TipoItem = new();
                itemProposta.Fornecedor = new();
                itemProposta.QuantidadeItem = item.QuantidadeItem;

                await itemProposta.GetItemPropostaDatabaseAsync(false, CancellationToken.None, false, "WHERE itpr.codigo_item LIKE @codigo_item ORDER BY itpr.data_insercao DESC LIMIT 1",
                          "@codigo_item", "%" + item.CodigoItem + "%");

                itemProposta.Id = null;
                itemProposta.Usuario = (Usuario)App.Usuario.Clone();
                itemProposta.DataInsercao = DateTime.Now;
                itemProposta.DataEdicaoItem = null;
                itemProposta.DataAprovacaoItem = null;

                if (String.IsNullOrEmpty(itemProposta.DescricaoItem))
                {
                    itemProposta.DescricaoItem = item.DescricaoItem;
                }

                itemProposta.ValorTotalInicialItem = itemProposta.PrecoUnitarioInicialItem * item.QuantidadeItem;

                itemProposta.QuantidadeEstoqueCodigoAbreviadoItem = null;
                itemProposta.QuantidadeEstoqueCodigoCompletoItem = null;
                itemProposta.InformacaoEstoqueCodigoAbreviadoItem = null;
                itemProposta.InformacaoEstoqueCodigoCompletoItem = null;
                await itemProposta.TipoItem.GetTipoItemDatabaseAsync(1, CancellationToken.None);
                await itemProposta.Status.GetStatusDatabaseAsync(1, CancellationToken.None);
                await itemProposta.StatusAprovacao.GetStatusAprovacaoDatabaseAsync(3, CancellationToken.None);
                await itemProposta.JustificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(1, CancellationToken.None);

                if (itemProposta.Fornecedor.Id != null)
                {
                    proposta.ListaItensProposta.Add(itemProposta);
                }
            }

            if (ListaItensOrdemServicoACopiar.Count > 0)
            {
                if (proposta.ListaItensProposta.Count == 0)
                {
                    MessageBox.Show("Nenhum item encontrado na database. Por favor, adicione-os manualmente", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    string textoAtencao = "Lembre-se de revisar os itens já que a busca na database é feita apenas pelo código abreviado, o que pode ocasionar imprecisões";
                    textoAtencao += "\n\nÉ altamente recomendado que os preços e prazos sejam verificados com o fornecedor pois estes podem estar desatualizados";

                    if (proposta.ListaItensProposta.Count < ListaItensOrdemServicoACopiar.Count)
                    {
                        textoAtencao += "\n\nAlguns itens não foram encontrados na database. Por favor, adicione-os manualmente";
                    }
                    MessageBox.Show("Atente-se aos pontos abaixo referentes aos itens adicionados:\n\n" + textoAtencao, "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            Messenger.Default.Send<Proposta>(proposta, "PropostaCopiar");

            // Executa o comando que fecha a caixa de diálogo
            ComandoFechar.Execute(null);
        }

        private void PassarTodosItensDireita()
        {
            foreach (var item in ListaItensOrdemServicoDisponiveis)
            {
                ListaItensOrdemServicoACopiar.Add(item);
            }
            ListaItensOrdemServicoDisponiveis.Clear();
        }

        private void PassarUmItemDireita()
        {
            foreach (var item in ListaItensOrdemServicoDisponiveisSelecionados)
            {
                ListaItensOrdemServicoACopiar.Add((ItemOrdemServico)item);

                ItemOrdemServico itemOrdemServicoARemover = ListaItensOrdemServicoDisponiveis.First(d => d.Id == ((ItemOrdemServico)item).Id);

                ListaItensOrdemServicoDisponiveis.Remove(itemOrdemServicoARemover);
            }
        }

        private void PassarUmItemEsquerda()
        {
            foreach (var item in ListaItensOrdemServicoCopiadosSelecionados)
            {
                ListaItensOrdemServicoDisponiveis.Add((ItemOrdemServico)item);

                ItemOrdemServico itemOrdemServicoARemover = ListaItensOrdemServicoACopiar.First(d => d.Id == ((ItemOrdemServico)item).Id);

                ListaItensOrdemServicoACopiar.Remove(itemOrdemServicoARemover);
            }
        }

        private void PassarTodosItensEsquerda()
        {
            foreach (var item in ListaItensOrdemServicoACopiar)
            {
                ListaItensOrdemServicoDisponiveis.Add(item);
            }
            ListaItensOrdemServicoACopiar.Clear();
        }

        private void PassarTodosEventosDireita()
        {
            foreach (var item in ListaEventosOrdemServicoDisponiveis)
            {
                ListaEventosOrdemServicoACopiar.Add(item);
            }
            ListaEventosOrdemServicoDisponiveis.Clear();
        }

        private void PassarUmEventoDireita()
        {
            foreach (var item in ListaEventosOrdemServicoDisponiveisSelecionados)
            {
                ListaEventosOrdemServicoACopiar.Add((EventoOrdemServico)item);

                EventoOrdemServico itemOrdemServicoARemover = ListaEventosOrdemServicoDisponiveis.First(d => d.Id == ((EventoOrdemServico)item).Id);

                ListaEventosOrdemServicoDisponiveis.Remove(itemOrdemServicoARemover);
            }
        }

        private void PassarUmEventoEsquerda()
        {
            foreach (var item in ListaEventosOrdemServicoCopiadosSelecionados)
            {
                ListaEventosOrdemServicoDisponiveis.Add((EventoOrdemServico)item);

                EventoOrdemServico itemOrdemServicoARemover = ListaEventosOrdemServicoACopiar.First(d => d.Id == ((EventoOrdemServico)item).Id);

                ListaEventosOrdemServicoACopiar.Remove(itemOrdemServicoARemover);
            }
        }

        private void PassarTodosEventosEsquerda()
        {
            foreach (var item in ListaEventosOrdemServicoACopiar)
            {
                ListaEventosOrdemServicoDisponiveis.Add(item);
            }
            ListaEventosOrdemServicoACopiar.Clear();
        }

        private void PassarTodosInconsistenciasDireita()
        {
            foreach (var item in ListaInconsistenciasOrdemServicoDisponiveis)
            {
                ListaInconsistenciasOrdemServicoACopiar.Add(item);
            }
            ListaInconsistenciasOrdemServicoDisponiveis.Clear();
        }

        private void PassarUmInconsistenciaDireita()
        {
            foreach (var item in ListaInconsistenciasOrdemServicoDisponiveisSelecionados)
            {
                ListaInconsistenciasOrdemServicoACopiar.Add((InconsistenciaOrdemServico)item);

                InconsistenciaOrdemServico itemOrdemServicoARemover = ListaInconsistenciasOrdemServicoDisponiveis.First(d => d.Id == ((InconsistenciaOrdemServico)item).Id);

                ListaInconsistenciasOrdemServicoDisponiveis.Remove(itemOrdemServicoARemover);
            }
        }

        private void PassarUmInconsistenciaEsquerda()
        {
            foreach (var item in ListaInconsistenciasOrdemServicoCopiadosSelecionados)
            {
                ListaInconsistenciasOrdemServicoDisponiveis.Add((InconsistenciaOrdemServico)item);

                InconsistenciaOrdemServico itemOrdemServicoARemover = ListaInconsistenciasOrdemServicoACopiar.First(d => d.Id == ((InconsistenciaOrdemServico)item).Id);

                ListaInconsistenciasOrdemServicoACopiar.Remove(itemOrdemServicoARemover);
            }
        }

        private void PassarTodosInconsistenciasEsquerda()
        {
            foreach (var item in ListaInconsistenciasOrdemServicoACopiar)
            {
                ListaInconsistenciasOrdemServicoDisponiveis.Add(item);
            }
            ListaInconsistenciasOrdemServicoACopiar.Clear();
        }

        #endregion Métodos
    }
}