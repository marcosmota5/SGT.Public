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
using System.Windows.Data;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class CopiarPropostaViewModel : ObservableObject, IPageViewModel
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

        private Proposta _proposta;

        private bool _ehParaProposta;
        private bool _dadosSolicitante;
        private bool _dadosProposta;
        private bool _dadosFaturamento;
        private bool _dadosEquipamento;
        private bool _dadosTermo;
        private bool _dadosComentario;
        private ObservableCollection<ItemProposta> _listaItensPropostaDisponiveis = new();
        private ObservableCollection<ItemProposta> _listaItensPropostaACopiar = new();

        private ICommand _comandoConfirmar;
        private ICommand _comandoPassarTodosItensDireita;
        private ICommand _comandoPassarUmItemDireita;
        private ICommand _comandoPassarUmItemEsquerda;
        private ICommand _comandoPassarTodosItensEsquerda;

        #endregion Campos

        #region Construtores

        /// <summary>
        /// Construtor do controle do item
        /// </summary>
        /// <param name="ordemServico">Ordem de serviço que terá o item inserido ou editado</param>
        /// <param name="itemOrdemServico">Item da proposta que será inserido ou editado</param>
        /// <param name="ehNovoItem">Valor booleano informando se o item é novo ou existente</param>
        /// <param name="closeHandler">Ação para fechar a caixa de diálogo</param>
        public CopiarPropostaViewModel(Proposta proposta, TipoCopia tipoCopia, Action<CopiarPropostaViewModel> closeHandler)
        {
            EhParaProposta = tipoCopia == TipoCopia.ParaProposta;

            // Atribui o método de limpar listas e a ação de fechar a caixa de diálogo ao comando
            this.ComandoFechar = new SimpleCommand(o => true, o =>
            {
                closeHandler(this);
            });

            _proposta = proposta;

            foreach (var item in _proposta.ListaItensProposta)
            {
                ListaItensPropostaDisponiveis.Add(item);
            }
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Copiar proposta";
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
                _listaItensPropostaDisponiveis = null;
                _listaItensPropostaACopiar = null;

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

        public bool EhParaProposta
        {
            get { return _ehParaProposta; }
            set
            {
                if (value != _ehParaProposta)
                {
                    _ehParaProposta = value;
                    OnPropertyChanged(nameof(EhParaProposta));
                }
            }
        }

        public bool DadosSolicitante
        {
            get { return _dadosSolicitante; }
            set
            {
                if (value != _dadosSolicitante)
                {
                    _dadosSolicitante = value;
                    OnPropertyChanged(nameof(DadosSolicitante));
                }
            }
        }

        public bool DadosProposta
        {
            get { return _dadosProposta; }
            set
            {
                if (value != _dadosProposta)
                {
                    _dadosProposta = value;
                    OnPropertyChanged(nameof(DadosProposta));
                }
            }
        }

        public bool DadosFaturamento
        {
            get { return _dadosFaturamento; }
            set
            {
                if (value != _dadosFaturamento)
                {
                    _dadosFaturamento = value;
                    OnPropertyChanged(nameof(DadosFaturamento));
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

        public bool DadosTermo
        {
            get { return _dadosTermo; }
            set
            {
                if (value != _dadosTermo)
                {
                    _dadosTermo = value;
                    OnPropertyChanged(nameof(DadosTermo));
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

        public ObservableCollection<ItemProposta> ListaItensPropostaDisponiveis
        {
            get { return _listaItensPropostaDisponiveis; }
            set
            {
                if (value != _listaItensPropostaDisponiveis)
                {
                    _listaItensPropostaDisponiveis = value;
                    OnPropertyChanged(nameof(ListaItensPropostaDisponiveis));
                }
            }
        }

        public List<object> ListaItensPropostaDisponiveisSelecionados { private get; set; }

        public List<object> ListaItensPropostaCopiadosSelecionados { private get; set; }

        public ObservableCollection<ItemProposta> ListaItensPropostaACopiar
        {
            get { return _listaItensPropostaACopiar; }
            set
            {
                if (value != _listaItensPropostaACopiar)
                {
                    _listaItensPropostaACopiar = value;
                    OnPropertyChanged(nameof(ListaItensPropostaACopiar));
                }
            }
        }

        #endregion Propriedades/Comandos

        #region Métodos

        private async Task ConfirmarCopia()
        {
            if (EhParaProposta)
            {
                await ConfirmarCopiaParaProposta();
            }
            else
            {
                await ConfirmarCopiaParaOrdemServico();
            }
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

            if (DadosSolicitante)
            {
                proposta.Cliente = (Cliente)_proposta.Cliente.Clone();
                proposta.Contato = (Contato)_proposta.Contato.Clone();
            }

            if (DadosProposta)
            {
                proposta.DataSolicitacao = _proposta.DataSolicitacao;
                proposta.Prioridade = (Prioridade)_proposta.Prioridade.Clone();
                proposta.Status = (Status)_proposta.Status.Clone();
            }

            if (DadosFaturamento)
            {
                proposta.DataEnvioFaturamento = _proposta.DataEnvioFaturamento;
                proposta.DataFaturamento = _proposta.DataFaturamento;
                proposta.NotaFiscal = _proposta.NotaFiscal;
                proposta.ValorFaturamento = _proposta.ValorFaturamento;
            }

            if (DadosEquipamento)
            {
                proposta.Serie = (Serie)_proposta.Serie.Clone();
                proposta.Horimetro = _proposta.Horimetro;
                proposta.OrdemServico = _proposta.OrdemServico;
            }

            if (DadosTermo)
            {
                proposta.TextoPadrao = _proposta.TextoPadrao;
                proposta.Observacoes = _proposta.Observacoes;
                proposta.PrazoEntrega = _proposta.PrazoEntrega;
                proposta.CondicaoPagamento = _proposta.CondicaoPagamento;
                proposta.Garantia = _proposta.Garantia;
                proposta.Validade = _proposta.Validade;
            }

            if (DadosComentario)
            {
                proposta.Comentarios = _proposta.Comentarios;
            }

            foreach (var item in ListaItensPropostaACopiar)
            {
                item.Id = null;
                item.Usuario = (Usuario)App.Usuario.Clone();
                item.DataInsercao = DateTime.Now;
                item.DataEdicaoItem = null;
                item.DataAprovacaoItem = null;
                item.QuantidadeEstoqueCodigoAbreviadoItem = null;
                item.QuantidadeEstoqueCodigoCompletoItem = null;
                item.InformacaoEstoqueCodigoAbreviadoItem = null;
                item.InformacaoEstoqueCodigoCompletoItem = null;
                await item.Status.GetStatusDatabaseAsync(1, CancellationToken.None);
                await item.StatusAprovacao.GetStatusAprovacaoDatabaseAsync(3, CancellationToken.None);
                await item.JustificativaAprovacao.GetJustificativaAprovacaoDatabaseAsync(1, CancellationToken.None);

                proposta.ListaItensProposta.Add(item);
            }

            Messenger.Default.Send<Proposta>(proposta, "PropostaCopiar");

            // Executa o comando que fecha a caixa de diálogo
            ComandoFechar.Execute(null);
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

            if (DadosSolicitante)
            {
                ordemServico.Cliente = (Cliente)_proposta.Cliente.Clone();
            }

            if (DadosProposta)
            {
                ordemServico.Status = (Status)_proposta.Status.Clone();
                ordemServico.DataChamado = _proposta.DataSolicitacao;
            }

            if (DadosEquipamento)
            {
                ordemServico.Serie = (Serie)_proposta.Serie.Clone();
            }

            if (DadosComentario)
            {
                ordemServico.Comentarios = _proposta.Comentarios;
            }

            foreach (var item in ListaItensPropostaACopiar)
            {
                ItemOrdemServico itemOrdemServico = new(true, true);

                itemOrdemServico.Id = null;
                itemOrdemServico.Usuario = (Usuario)App.Usuario.Clone();
                itemOrdemServico.DataInsercao = DateTime.Now;
                itemOrdemServico.DataEdicaoItem = null;
                itemOrdemServico.CodigoItem = item.CodigoItem;
                itemOrdemServico.DescricaoItem = item.DescricaoItem;
                itemOrdemServico.QuantidadeItem = item.QuantidadeItem;
                itemOrdemServico.Conjunto = (Conjunto)item.Conjunto.Clone();
                itemOrdemServico.Especificacao = (Especificacao)item.Especificacao.Clone();
                await itemOrdemServico.Status.GetStatusDatabaseAsync(1, CancellationToken.None);

                ordemServico.ListaItensOrdemServico.Add(itemOrdemServico);
            }

            Messenger.Default.Send<OrdemServico>(ordemServico, "OrdemServicoCopiar");

            // Executa o comando que fecha a caixa de diálogo
            ComandoFechar.Execute(null);
        }

        private void PassarTodosItensDireita()
        {
            foreach (var item in ListaItensPropostaDisponiveis)
            {
                ListaItensPropostaACopiar.Add(item);
            }
            ListaItensPropostaDisponiveis.Clear();
        }

        private void PassarUmItemDireita()
        {
            foreach (var item in ListaItensPropostaDisponiveisSelecionados)
            {
                ListaItensPropostaACopiar.Add((ItemProposta)item);

                ItemProposta itemPropostaARemover = ListaItensPropostaDisponiveis.First(d => d.Id == ((ItemProposta)item).Id);

                ListaItensPropostaDisponiveis.Remove(itemPropostaARemover);
            }
        }

        private void PassarUmItemEsquerda()
        {
            foreach (var item in ListaItensPropostaCopiadosSelecionados)
            {
                ListaItensPropostaDisponiveis.Add((ItemProposta)item);

                ItemProposta itemPropostaARemover = ListaItensPropostaACopiar.First(d => d.Id == ((ItemProposta)item).Id);

                ListaItensPropostaACopiar.Remove(itemPropostaARemover);
            }
        }

        private void PassarTodosItensEsquerda()
        {
            foreach (var item in ListaItensPropostaACopiar)
            {
                ListaItensPropostaDisponiveis.Add(item);
            }
            ListaItensPropostaACopiar.Clear();
        }

        #endregion Métodos
    }
}