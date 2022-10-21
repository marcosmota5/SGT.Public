using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class SobreViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private Instancia _instancia;
        private string _versao;
        private bool _exibeMensagemErro;
        private string _mensagemErro;
        private int? _quantidadeUsuariosAtual;
        private ICommand _comandoFechar;

        private bool _controlesHabilitados;
        private bool _carregamentoVisivel = true;

        #endregion Campos

        public SobreViewModel()
        {
            ConstrutorAsync().Await();
        }

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Sobre";
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
                _instancia = null;
                _comandoFechar = null;
            }
            catch (Exception)
            {
            }
        }

        public Action CloseAction { get; set; }

        public string Versao
        {
            get { return _versao; }
            set
            {
                if (value != _versao)
                {
                    _versao = value;
                    OnPropertyChanged(nameof(Versao));
                }
            }
        }

        public Instancia Instancia
        {
            get { return _instancia; }
            set
            {
                if (value != _instancia)
                {
                    _instancia = value;
                    OnPropertyChanged(nameof(Instancia));
                }
            }
        }

        public bool ExibeMensagemErro
        {
            get { return _exibeMensagemErro; }
            set
            {
                if (value != _exibeMensagemErro)
                {
                    _exibeMensagemErro = value;
                    OnPropertyChanged(nameof(ExibeMensagemErro));
                }
            }
        }

        public string MensagemErro
        {
            get { return _mensagemErro; }
            set
            {
                if (value != _mensagemErro)
                {
                    _mensagemErro = value;
                    OnPropertyChanged(nameof(MensagemErro));
                }
            }
        }

        public int? QuantidadeUsuariosAtual
        {
            get { return _quantidadeUsuariosAtual; }
            set
            {
                if (value != _quantidadeUsuariosAtual)
                {
                    _quantidadeUsuariosAtual = value;
                    OnPropertyChanged(nameof(QuantidadeUsuariosAtual));
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

        public ICommand ComandoFechar
        {
            get
            {
                if (_comandoFechar == null)
                {
                    _comandoFechar = new RelayCommand(
                        param => CloseAction(),
                        param => true
                    );
                }
                return _comandoFechar;
            }
        }

        #endregion Propriedades/Comandos

        #region Métodos

        private async Task ConstrutorAsync()
        {
            Instancia = new();
            InstanciaLocal instanciaLocal = new();

            try
            {
                await instanciaLocal.GetInstanciaLocal(CancellationToken.None);
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro na verificação de instância");

                MensagemErro = "Falha na verificação de instância local. Se o problema persistir, contate o desenvolvedor e envie o log";
                ExibeMensagemErro = true;
                ControlesHabilitados = false;
                CarregamentoVisivel = false;
                return;
            }

            try
            {
                QuantidadeUsuariosAtual = await Usuario.GetQuantidadeUsuariosAtivos(CancellationToken.None);
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro na verificação de usuários");

                MensagemErro = "Falha na verificação de usuários. Se o problema persistir, contate o desenvolvedor e envie o log";
                ExibeMensagemErro = true;
                ControlesHabilitados = false;
                CarregamentoVisivel = false;
                return;
            }

            try
            {
                Versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                await Instancia.GetInstanciaDatabaseAsync(instanciaLocal.CodigoInstancia, CancellationToken.None);

                try
                {
                    await InstanciaLocal.AtualizaDataInstancia(CancellationToken.None);
                }
                catch (Exception)
                {
                }

                if (Instancia.Id == null)
                {
                    MensagemErro = "Instância de login inválida. Contate o desenvolvedor";
                    ExibeMensagemErro = true;
                    ControlesHabilitados = false;
                    CarregamentoVisivel = false;
                    return;
                }
                else
                {
                    if (Instancia.DataFim != null)
                    {
                        if (DateTime.Now > (DateTime)Instancia.DataFim)
                        {
                            MensagemErro = "Instância expirada. Contate o desenvolvedor";
                            ExibeMensagemErro = true;
                            ControlesHabilitados = false;
                            CarregamentoVisivel = false;
                            return;
                        }
                    }
                }
                MensagemErro = "";
                ExibeMensagemErro = false;
            }
            catch (Exception ex)
            {
                DateTime dataAtualizacao = instanciaLocal.DataAtualizacao == null ? DateTime.Now : (DateTime)instanciaLocal.DataAtualizacao;
                TimeSpan diasVerificacao = DateTime.Now - dataAtualizacao;

                if (diasVerificacao.Days > 15)
                {
                    // Escreve no log a exceção e uma mensagem de erro
                    Serilog.Log.Error(ex, "Erro na autenticação de instância (limite de 15 dias ultrapassado)");

                    MensagemErro = "Falha na autenticação de instância (limite de 15 dias ultrapassado). Contate o desenvolvedor";
                    ExibeMensagemErro = true;
                    ControlesHabilitados = false;
                    CarregamentoVisivel = false;
                    return;
                }
            }
            ControlesHabilitados = true;
            CarregamentoVisivel = false;
        }

        #endregion Métodos
    }
}