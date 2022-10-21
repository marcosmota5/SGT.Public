using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class MensagemOrdensServicoIncompletasViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private ConfiguracaoSistema _configuracaoSistema = new();
        private OrdemServico _ordemServicoSelecionada = new();
        private CancellationTokenSource _cts;

        private bool _controlesHabilitados;
        private bool _listaHabilitada = true;
        private bool _progressoEhIndeterminavel = true;
        private bool _progressoVisivel = false;
        private double _valorProgresso = 0;
        private string _textoProgresso;
        private string _mensagemStatus;

        private bool _carregamentoVisivel = true;

        private ObservableCollection<OrdemServico> _listaOrdensServicoIncompletas = new();

        private ICommand _comandoLemprarDepois;
        private ICommand _comandoContinuarPreenchimento;

        #endregion Campos

        #region Construtores

        public MensagemOrdensServicoIncompletasViewModel(Action<MensagemOrdensServicoIncompletasViewModel> closeHandler)
        {
            // Atribui o método de limpar listas e a ação de fechar a caixa de diálogo ao comando
            this.ComandoFechar = new SimpleCommand(o => true, o =>
            {
                closeHandler(this);
            });

            ConstrutorAsync().Await();
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Ordens de serviço incompletas";
            }
        }

        public string Icone
        {
            get
            {
                return "ProgressClose";
            }
        }

        public void LimparViewModel()
        {
            try
            {
                _ordemServicoSelecionada = null;
                _cts = null;
                _listaOrdensServicoIncompletas = null;
                _comandoLemprarDepois = null;
                _comandoContinuarPreenchimento = null;
            }
            catch (Exception)
            {
            }
        }

        public ConfiguracaoSistema ConfiguracaoSistema
        {
            get { return _configuracaoSistema; }
            set
            {
                _configuracaoSistema = value;
                OnPropertyChanged(nameof(ConfiguracaoSistema));
            }
        }

        public OrdemServico OrdemServicoSelecionada
        {
            get { return _ordemServicoSelecionada; }
            set
            {
                _ordemServicoSelecionada = value;
                OnPropertyChanged(nameof(OrdemServicoSelecionada));
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

        public ObservableCollection<OrdemServico> ListaOrdensServicoIncompletas
        {
            get { return _listaOrdensServicoIncompletas; }
            set
            {
                if (value != _listaOrdensServicoIncompletas)
                {
                    _listaOrdensServicoIncompletas = value;
                    OnPropertyChanged(nameof(ListaOrdensServicoIncompletas));
                }
            }
        }

        public ICommand ComandoFechar { get; }

        public ICommand ComandoLemprarDepois
        {
            get
            {
                if (_comandoLemprarDepois == null)
                {
                    _comandoLemprarDepois = new RelayCommand(
                        param => LembrarDepois(),
                        param => true
                    );
                }
                return _comandoLemprarDepois;
            }
        }

        public ICommand ComandoContinuarPreenchimento
        {
            get
            {
                if (_comandoContinuarPreenchimento == null)
                {
                    _comandoContinuarPreenchimento = new RelayCommand(
                        param => ContinuarPreenchimento().Await(),
                        param => true
                    );
                }
                return _comandoContinuarPreenchimento;
            }
        }

        #endregion Propriedades/Comandos

        #region Métodos

        public async Task ConstrutorAsync()
        {
            try
            {
                CarregamentoVisivel = true;

                ListaHabilitada = false;
                ControlesHabilitados = false;

                await ConfiguracaoSistema.GetConfiguracaoSistemaDatabaseAsync(1, CancellationToken.None);

                // Preenche as listas com as classes necessárias
                await OrdemServico.PreencheListaOrdensServicoAsync(ListaOrdensServicoIncompletas, true, false, false,
                    false, null, CancellationToken.None, "WHERE orse.etapas_concluidas < 4 ORDER BY orse.ordem_servico_atual ASC", "");

                // Redefine as permissões
                ListaHabilitada = true;
                ControlesHabilitados = true;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");

                MensagemStatus = "Falha no carregamento dos dados. Caso o problema persista, contate o desenvolvedor e envie o arquivo de log";
                ListaHabilitada = false;
                ControlesHabilitados = false;
            }

            CarregamentoVisivel = false;
        }

        private async Task ContinuarPreenchimento()
        {
            if (OrdemServicoSelecionada != null)
            {
                try
                {
                    if (!await OrdemServico.OrdemServicoExiste(CancellationToken.None, OrdemServicoSelecionada.Id))
                    {
                        MensagemStatus = "A ordem de serviço selecionada foi excluída da database. Não será possível abri-la";

                        await Task.Delay(1000);

                        if (!await OrdemServico.ExistemOrdensServicoIncompletas(CancellationToken.None))
                        {
                            MensagemStatus = "Não há mais ordens de serviço incompletas. Fechando...";

                            await Task.Delay(1000);

                            ComandoFechar.Execute(null);
                        }

                        MensagemStatus = "Atualizando lista";

                        await Task.Delay(1000);

                        ListaHabilitada = false;
                        ControlesHabilitados = false;

                        // Preenche as listas com as classes necessárias
                        await OrdemServico.PreencheListaOrdensServicoAsync(ListaOrdensServicoIncompletas, true, false, false,
                            false, null, CancellationToken.None, "WHERE orse.etapas_concluidas < 4 ORDER BY orse.ordem_servico_atual ASC", "");

                        // Redefine as permissões
                        ListaHabilitada = true;
                        ControlesHabilitados = true;

                        return;
                    }

                    Messenger.Default.Send<OrdemServico>(OrdemServicoSelecionada, "OrdemServicoAdicionar");

                    ComandoFechar.Execute(null);
                }
                catch (Exception)
                {
                    MensagemStatus = "Não foi possível verificar a existência da ordem de serviço. Não será possível abri-la";

                    return;
                }
            }
        }

        private void LembrarDepois()
        {
            ComandoFechar.Execute(null);
            Messenger.Default.Send<bool>(true, "ClicouLembrarDepoisOS");
        }

        #endregion Métodos
    }
}