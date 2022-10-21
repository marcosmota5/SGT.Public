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
    public class ControleInconsistenciaOrdemServicoViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private ObservableCollection<Inconsistencia> _listaInconsistencias = new();
        private ObservableCollection<Status> _listaStatus = new();
        private InconsistenciaOrdemServico _inconsistenciaOrdemServico;
        private InconsistenciaOrdemServico _inconsistenciaOrdemServicoInicial;
        private OrdemServico _ordemServico;
        private bool _ehNovoItem;
        private string _mensagemStatus;
        private bool _controlesHabilitados;
        private bool _carregamentoVisivel = true;

        private ICommand _comandoSalvar;

        #endregion Campos

        #region Construtores

        /// <summary>
        /// Construtor do controle do item
        /// </summary>
        /// <param name="ordemServico">Ordem de serviço que terá o item inserido ou editado</param>
        /// <param name="inconsistenciaOrdemServico">Item da proposta que será inserido ou editado</param>
        /// <param name="ehNovoItem">Valor booleano informando se o item é novo ou existente</param>
        /// <param name="closeHandler">Ação para fechar a caixa de diálogo</param>
        public ControleInconsistenciaOrdemServicoViewModel(OrdemServico ordemServico, InconsistenciaOrdemServico? inconsistenciaOrdemServico, bool ehNovoItem, Action<ControleInconsistenciaOrdemServicoViewModel> closeHandler)
        {
            // Define os itens internos como o informado no construtor
            _ordemServico = ordemServico;
            _ehNovoItem = ehNovoItem;

            // Verifica se é um novo item
            if (_ehNovoItem)
            {
                // Define o item da proposta
                InconsistenciaOrdemServico = inconsistenciaOrdemServico;
            }
            else
            {
                // Salva o item inicial que foi passado
                _inconsistenciaOrdemServicoInicial = inconsistenciaOrdemServico;

                // Define o item da proposta a ser utilizado como uma cópia do item existente
                InconsistenciaOrdemServico = (InconsistenciaOrdemServico)inconsistenciaOrdemServico.Clone();
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
                return "Inconsistência da ordem de serviço";
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
                _listaInconsistencias = null;
                _listaStatus = null;
                _comandoSalvar = null;
            }
            catch (Exception)
            {
            }
        }

        public bool ExistemCamposVazios { private get; set; }

        public ICommand ComandoFechar { get; }

        public ICommand ComandoSalvar
        {
            get
            {
                if (_comandoSalvar == null)
                {
                    _comandoSalvar = new RelayCommand(
                        param => SalvarInconsistenciaOrdemServicoAsync().Await(),
                        param => true
                    );
                }

                return _comandoSalvar;
            }
        }

        public ObservableCollection<Inconsistencia> ListaInconsistencias
        {
            get { return _listaInconsistencias; }
            set
            {
                if (value != _listaInconsistencias)
                {
                    _listaInconsistencias = value;
                    OnPropertyChanged(nameof(ListaInconsistencias));
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

        public InconsistenciaOrdemServico InconsistenciaOrdemServico
        {
            get { return _inconsistenciaOrdemServico; }
            set
            {
                if (value != _inconsistenciaOrdemServico)
                {
                    _inconsistenciaOrdemServico = value;
                    OnPropertyChanged(nameof(InconsistenciaOrdemServico));
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
            ListaInconsistencias = null;
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
                await Inconsistencia.PreencheListaInconsistenciasAsync(ListaInconsistencias, true, null, CancellationToken.None, "WHERE inco.id_status = 1", "");
                await Status.PreencheListaStatusAsync(ListaStatus, true, null, CancellationToken.None, "", "");

                try
                {
                    if (InconsistenciaOrdemServico?.Inconsistencia?.Id > 0 && !ListaInconsistencias.Any(x => x.Id == InconsistenciaOrdemServico?.Inconsistencia?.Id))
                    {
                        Inconsistencia inconsistencia = new();

                        await inconsistencia.GetInconsistenciaDatabaseAsync(InconsistenciaOrdemServico?.Inconsistencia?.Id, CancellationToken.None);

                        ListaInconsistencias.Add(inconsistencia);
                    }
                }
                catch (Exception)
                {
                }

                // Define as classes da proposta como classes com a mesma referência das listas
                try
                {
                    InconsistenciaOrdemServico.Inconsistencia = ListaInconsistencias.First(tiip => tiip.Id == InconsistenciaOrdemServico?.Inconsistencia?.Id);
                }
                catch (Exception)
                {
                }
                try
                {
                    InconsistenciaOrdemServico.Status = ListaStatus.First(tiip => tiip.Id == InconsistenciaOrdemServico?.Status?.Id);
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
        private async Task SalvarInconsistenciaOrdemServicoAsync()
        {
            // Verifica se existem campos vazios e, caso verdadeiro, encerra a execução do método
            if (ExistemCamposVazios)
            {
                return;
            }

            // Verifica se é um novo item e, caso verdadeiro, adiciona-o a proposta. Caso contrário, apenas altera-o
            if (_ehNovoItem)
            {
                InconsistenciaOrdemServico.DataInsercao = DateTime.Now;
                InconsistenciaOrdemServico.Usuario = App.Usuario == null ? null : (Usuario)App.Usuario.Clone();

                _ordemServico.ListaInconsistenciasOrdemServico.Add(InconsistenciaOrdemServico);

                ComandoFechar.Execute(null);
            }
            else
            {
                _inconsistenciaOrdemServicoInicial.ComentariosItem = InconsistenciaOrdemServico.ComentariosItem;

                if (_inconsistenciaOrdemServicoInicial.Id != null)
                {
                    _inconsistenciaOrdemServicoInicial.DataEdicaoItem = DateTime.Now;
                }

                _inconsistenciaOrdemServicoInicial.Usuario = App.Usuario == null ? null : (Usuario)App.Usuario.Clone();
                _inconsistenciaOrdemServicoInicial.Inconsistencia = InconsistenciaOrdemServico.Inconsistencia == null ? null : (Inconsistencia)InconsistenciaOrdemServico.Inconsistencia.Clone();
                _inconsistenciaOrdemServicoInicial.Status = InconsistenciaOrdemServico.Status == null ? null : (Status)InconsistenciaOrdemServico.Status.Clone();

                // Executa o comando que fecha a caixa de diálogo
                ComandoFechar.Execute(null);

                OnPropertyChanged(nameof(_inconsistenciaOrdemServicoInicial));
            }
        }

        #endregion Métodos
    }
}