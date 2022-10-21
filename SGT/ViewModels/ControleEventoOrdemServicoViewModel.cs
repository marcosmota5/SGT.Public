using Model.DataAccessLayer.Classes;
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
    public class ControleEventoOrdemServicoViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private ObservableCollection<Evento> _listaEventos = new();
        private ObservableCollection<Status> _listaStatus = new();
        private EventoOrdemServico _eventoOrdemServico;
        private EventoOrdemServico _eventoOrdemServicoInicial;
        private OrdemServico _ordemServico;
        private bool _ehNovoItem;
        private string _mensagemStatus;
        private bool _controlesHabilitados;
        private bool _carregamentoVisivel = true;

        private ICommand _comandoVerificaDataInicio;
        private ICommand _comandoVerificaDataFim;

        private ICommand _comandoSalvar;

        #endregion Campos

        #region Construtores

        /// <summary>
        /// Construtor do controle do item
        /// </summary>
        /// <param name="ordemServico">Ordem de serviço que terá o item inserido ou editado</param>
        /// <param name="eventoOrdemServico">Item da proposta que será inserido ou editado</param>
        /// <param name="ehNovoItem">Valor booleano informando se o item é novo ou existente</param>
        /// <param name="closeHandler">Ação para fechar a caixa de diálogo</param>
        public ControleEventoOrdemServicoViewModel(OrdemServico ordemServico, EventoOrdemServico? eventoOrdemServico, bool ehNovoItem, Action<ControleEventoOrdemServicoViewModel> closeHandler)
        {
            // Define os itens internos como o informado no construtor
            _ordemServico = ordemServico;
            _ehNovoItem = ehNovoItem;

            // Verifica se é um novo item
            if (_ehNovoItem)
            {
                // Define o item da proposta
                EventoOrdemServico = eventoOrdemServico;
            }
            else
            {
                // Salva o item inicial que foi passado
                _eventoOrdemServicoInicial = eventoOrdemServico;

                // Define o item da proposta a ser utilizado como uma cópia do item existente
                EventoOrdemServico = (EventoOrdemServico)eventoOrdemServico.Clone();
            }

            // Atribui o método de limpar listas e a ação de fechar a caixa de diálogo ao comando
            this.ComandoFechar = new SimpleCommand(o => true, o =>
            {
                LimparListas();
                closeHandler(this);
            });

            // Chama o méodo assíncrono para construir o controle de itens
            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Evento da ordem de serviço";
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
                _listaEventos = null;
                _listaStatus = null;
                _comandoSalvar = null;
            }
            catch (Exception)
            {
            }
        }

        public bool ExistemCamposVazios { private get; set; }

        public ICommand ComandoFechar { get; }

        public ICommand ComandoVerificaDataInicio
        {
            get
            {
                if (_comandoVerificaDataInicio == null)
                {
                    _comandoVerificaDataInicio = new RelayCommand(
                        param => VerificaDataInicio(),
                        param => true
                    );
                }
                return _comandoVerificaDataInicio;
            }
        }

        public ICommand ComandoVerificaDataFim
        {
            get
            {
                if (_comandoVerificaDataFim == null)
                {
                    _comandoVerificaDataFim = new RelayCommand(
                        param => VerificaDataFim(),
                        param => true
                    );
                }
                return _comandoVerificaDataFim;
            }
        }

        public ICommand ComandoSalvar
        {
            get
            {
                if (_comandoSalvar == null)
                {
                    _comandoSalvar = new RelayCommand(
                        param => SalvarEventoOrdemServicoAsync().Await(),
                        param => true
                    );
                }

                return _comandoSalvar;
            }
        }

        public ObservableCollection<Evento> ListaEventos
        {
            get { return _listaEventos; }
            set
            {
                if (value != _listaEventos)
                {
                    _listaEventos = value;
                    OnPropertyChanged(nameof(ListaEventos));
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

        public EventoOrdemServico EventoOrdemServico
        {
            get { return _eventoOrdemServico; }
            set
            {
                if (value != _eventoOrdemServico)
                {
                    _eventoOrdemServico = value;
                    OnPropertyChanged(nameof(EventoOrdemServico));
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

        #endregion Propriedades/Comandos

        #region Métodos

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

        /// <summary>
        /// Método para limpar as listas e salvar memória
        /// </summary>
        private void LimparListas()
        {
            ListaEventos = null;
            ListaStatus = null;
        }

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        /// <summary>
        /// Método assíncrono que preenche e define os itens, deve ser chamado no construtor
        /// </summary>
        private async Task ConstrutorAsync()
        {
            try
            {
                // Preenche as listas com as classes necessárias
                await Evento.PreencheListaEventosAsync(ListaEventos, true, null, CancellationToken.None, "WHERE even.id_status = 1", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");

                try
                {
                    if (EventoOrdemServico?.Evento?.Id > 0 && !ListaEventos.Any(x => x.Id == EventoOrdemServico?.Evento?.Id))
                    {
                        Evento evento = new();

                        await evento.GetEventoDatabaseAsync(EventoOrdemServico?.Evento?.Id, CancellationToken.None);

                        ListaEventos.Add(evento);
                    }
                }
                catch (Exception)
                {
                }

                // Define as classes da proposta como classes com a mesma referência das listas
                try
                {
                    EventoOrdemServico.Evento = ListaEventos.First(tiip => tiip.Id == EventoOrdemServico?.Evento?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    EventoOrdemServico.Status = ListaStatus.First(tiip => tiip.Id == EventoOrdemServico?.Status?.Id);
                }
                catch (Exception)
                {
                }
                ControlesHabilitados = true;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");

                MensagemStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                ControlesHabilitados = false;
            }
            CarregamentoVisivel = false;
        }

        /// <summary>
        /// Método assíncrono para salvar o item da proposta
        /// </summary>
        private async Task SalvarEventoOrdemServicoAsync()
        {
            // Verifica se existem campos vazios e, caso verdadeiro, encerra a execução do método
            if (ExistemCamposVazios)
            {
                return;
            }

            // Verifica se é um novo item e, caso verdadeiro, adiciona-o a proposta. Caso contrário, apenas altera-o
            if (_ehNovoItem)
            {
                EventoOrdemServico.DataInsercao = DateTime.Now;
                EventoOrdemServico.Usuario = App.Usuario == null ? null : (Usuario)App.Usuario.Clone();

                _ordemServico.ListaEventosOrdemServico.Add(EventoOrdemServico);

                ComandoFechar.Execute(null);
            }
            else
            {
                _eventoOrdemServicoInicial.DataInicio = EventoOrdemServico.DataInicio;
                _eventoOrdemServicoInicial.DataFim = EventoOrdemServico.DataFim;
                _eventoOrdemServicoInicial.ComentariosItem = EventoOrdemServico.ComentariosItem;

                if (_eventoOrdemServicoInicial.Id != null)
                {
                    _eventoOrdemServicoInicial.DataEdicaoItem = DateTime.Now;
                }

                _eventoOrdemServicoInicial.Usuario = App.Usuario == null ? null : (Usuario)App.Usuario.Clone();
                _eventoOrdemServicoInicial.Evento = EventoOrdemServico.Evento == null ? null : (Evento)EventoOrdemServico.Evento.Clone();
                _eventoOrdemServicoInicial.Status = EventoOrdemServico.Status == null ? null : (Status)EventoOrdemServico.Status.Clone();

                // Executa o comando que fecha a caixa de diálogo
                ComandoFechar.Execute(null);

                OnPropertyChanged(nameof(_eventoOrdemServicoInicial));
            }
        }

        private void VerificaDataInicio()
        {
            if (EventoOrdemServico.DataInicio != null && EventoOrdemServico.DataFim != null)
            {
                if (EventoOrdemServico.DataInicio > EventoOrdemServico.DataFim)
                {
                    EventoOrdemServico.DataInicio = null;
                }
            }
        }

        private void VerificaDataFim()
        {
            if (EventoOrdemServico.DataInicio != null && EventoOrdemServico.DataFim != null)
            {
                if (EventoOrdemServico.DataFim < EventoOrdemServico.DataInicio)
                {
                    EventoOrdemServico.DataFim = null;
                }
            }
        }

        #endregion Métodos
    }
}