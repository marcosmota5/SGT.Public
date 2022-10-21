using ControlzEx.Theming;
using GalaSoft.MvvmLight.Messaging;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.HelperClasses;
using Model.RegistroWindows.Configuracoes.Geral;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace SGT
{
    /// <summary>
    /// Dados do aplicativo
    /// </summary>
    public partial class App : Application
    {
        private DispatcherTimer _timer;

        public App()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("LICENSE_KEY_HIDDEN");

            //Register Bold license online
            //Bold.Licensing.BoldLicenseProvider.RegisterLicense("LICENSE_KEY_HIDDEN");

            //Register Bold license offline
            string licenseKey = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"Resources\boldreports_licensekey.lic"));
            Bold.Licensing.BoldLicenseProvider.RegisterLicense(licenseKey, isOfflineValidation: true);
        }

        public static Usuario Usuario { get; set; }
        public static ConfiguracoesGerais ConfiguracoesGerais { get; set; }
        public static bool EdicaoEhEnterprise { get; set; }
        public static bool LoginAposUso { get; set; }

        /// <summary>
        /// Método que sobrescreve o evento de inicialização do sistema
        /// </summary>
        /// <param name="e">Argumentos de inicialização</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Declara a pasta do log
            string pastaLog = "";

            // Tenta definir a pasta do log
            try
            {
                // Define a pasta do log como meus documentos
                pastaLog = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Proreports\\SGT\\logs";

                // Tenta criar o diretório de logs
                Directory.CreateDirectory(pastaLog);
            }
            catch (Exception)
            {
            }

            // Cria o logger adicionando o registro em database apenas caso não seja uma seção debug

#if DEBUG
            var log = new LoggerConfiguration()
                .Enrich.WithMemoryUsage()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithAssemblyName()
                .Enrich.WithAssemblyVersion()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.FromGlobalLogContext()
                .WriteTo.File(pastaLog + "\\log.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [" + Environment.OSVersion.ToString() + "] [{MachineName}] [{EnvironmentUserName}] " +
                "[{AssemblyName}] [{AssemblyVersion}] [{ProcessId}] [{ProcessName}] [{MemoryUsage}] [{EmpresaInstancia}] [{EmpresaCnpj}] [{UsuarioLogado}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
#else
                        var log = new LoggerConfiguration()
                            .Enrich.WithMemoryUsage()
                            .Enrich.WithMachineName()
                            .Enrich.WithEnvironmentUserName()
                            .Enrich.WithAssemblyName()
                            .Enrich.WithAssemblyVersion()
                            .Enrich.WithProcessId()
                            .Enrich.WithProcessName()
                            .Enrich.FromGlobalLogContext()
                            .WriteTo.File(pastaLog + "\\log.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [" + Environment.OSVersion.ToString() + "] [{MachineName}] [{EnvironmentUserName}] " +
                            "[{AssemblyName}] [{AssemblyVersion}] [{ProcessId}] [{ProcessName}] [{MemoryUsage}] [{EmpresaInstancia}] [{EmpresaCnpj}] [{UsuarioLogado}] {Message:lj}{NewLine}{Exception}")
                            .WriteTo.MySQL("server=localhost;user id=root;password=Test@12345;persistsecurityinfo=True;database=db_cloud;port=3306;", "tb_logs")
                            .CreateLogger();
#endif

            // Tenta iniciar o sistema e retorna um erro caso não consiga
            try
            {
                // Definição do logger estático que será usado por toda a aplicação
                Log.Logger = log;

                // Escreve no log um texto informando dados do sistema e do usuário atual do Windows
                Log.Information("Sistema iniciado");

                // Inicializar as configurações gerais (obtidas através do registro do Windows)
                ConfiguracoesGerais = new();

                // Definição que sobrescreve a inicialização
                base.OnStartup(e);

                // Registra a mensagem
                Messenger.Default.Register<bool>(this, "LoginAposUso", delegate (bool loginAposUso) { LoginAposUso = loginAposUso; });

                // Definições e exibição do sistema
                MainWindow app = new();
                MainWindowViewModel context = new();
                app.DataContext = context;
                app.ShowDialogsOverTitleBar = false;
                app.Show();
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Log.Fatal(ex, "Erro ao inicializar o sistema");

                string local_log = "";

                try
                {
                    local_log = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "logs\\log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                }
                catch (Exception)
                {
                    local_log = "Falha ao obter arquivo de log";
                }

                MessageBox.Show("Houve um erro fatal ao iniciar o sistema. Por favor, contate o desenvolvedor e envie-o o arquivo de log disponível em " + local_log + "\n\nDetalhes do erro: " + ex.Message, "Erro fatal não tratado", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Definição do timer que irá gravar no log a quantidade de memória em uso a cada 1 hora
            _timer = new DispatcherTimer(new TimeSpan(0, 30, 0), DispatcherPriority.Normal, delegate
            {
                // Escreve no log um texto informando a quantidade de memória em uso
                Log.Information("Registro de uso periódico");
            }, Application.Current.Dispatcher);

            // Inicia o timer
            _timer.Start();
        }

        /// <summary>
        /// Método estático que retorna a quantidade atual de memória utilizada pelo sistema
        /// </summary>
        /// <returns>Long contendo a quantidade de memória</returns>
        public static long GetMemoriaUsada()
        {
            // Tenta retornar a quantidade de memória utilizada e retorna um erro caso não consiga
            try
            {
                // Obtém o processo atual do sistema
                Process currentProcess = Process.GetCurrentProcess();

                // Retorna a quantidade de memória utilizada atualmente pelo sistema
                return currentProcess.PrivateMemorySize64;
            }
            catch (Exception ex)
            {
                // Escreve no log uma mensagem informando a falha ao obter a memória atual passando a exceção que gerou o erro
                Log.Error(ex, "Falha ao obter a memória utilizada");
            }

            // Retorno obrigatório
            return 0;
        }

        /// <summary>
        /// Evento que lida com o encerramento do sistema e fecha o log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Log.CloseAndFlush();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                if (App.Usuario != null)
                {
                    App.Usuario.LimpaIdUsuarioEmUsoAsync(System.Threading.CancellationToken.None).Await();
                }
            }
            catch (Exception)
            {
            }

            // Escreve no log a exceção e uma mensagem de erro
            Log.Fatal(e.Exception, "Erro fatal não tratado");

            string local_log = "";

            try
            {
                local_log = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "logs\\log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            }
            catch (Exception)
            {
                local_log = "Falha ao obter arquivo de log";
            }

            MessageBox.Show("Houve um erro fatal não tratado no sistema. Por favor, contate o desenvolvedor e envie-o o arquivo de log disponível em " + local_log + "\n\nDetalhes do erro: " + e.Exception.Message, "Erro fatal não tratado", MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;
        }
    }
}