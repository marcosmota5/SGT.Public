using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.HelperClasses;
using SGT.ViewModels;
using SGT.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SGT.HelperClasses
{
    public static class ValidaSessao
    {
        //private readonly IDialogCoordinator _dialogCoordinator;

        //public ValidaSessao(IDialogCoordinator dialogCoordinator)
        //{
        //    _dialogCoordinator = dialogCoordinator;
        //}

        public static async Task ValidaSessaoUsuarioAsync(IDialogCoordinator dialogCoordinator, object instance)
        {
            try
            {
                await App.Usuario.ComparaSessaoUsuarioAsync(CancellationToken.None);
            }
            catch (Model.DataAccessLayer.DataAccessExceptions.SessaoInvalidaException ex)
            {
                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Ok"
                };

                var respostaMensagem = await dialogCoordinator.ShowMessageAsync(instance,
                        "Sessão", ex.Message, MessageDialogStyle.Affirmative, mySettings);

                var pagina = new RenovarSessaoViewModel(false);

                MetroWindow? testWindow;

                testWindow = new MetroWindow
                {
                    Owner = App.Current.MainWindow,
                    Title = pagina.Name
                };

                testWindow.Tag = "";
                testWindow.Closed += (o, args) => testWindow = null;
                testWindow.Closed += (o, args) => Messenger.Default.Send<bool>(false, "FundoPrincipalVisivel");
                testWindow.Closed += (o, args) => Messenger.Default.Send<string>("", "TextoFundoPrincipal");
                testWindow.Closing += (o, args) =>
                        {
                            if (testWindow.Tag.ToString() != "")
                                return;

                            args.Cancel = true;
                            var resultado = MessageBox.Show("O login é obrigatório, portanto o sistema será encerrado. Deseja continuar?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                            if (resultado == MessageBoxResult.Yes)
                            {
                                App.Usuario.LimpaIdUsuarioEmUsoAsync(CancellationToken.None).Await();
                                Application.Current.Shutdown();
                            }
                        };
                Messenger.Default.Send<bool>(true, "FundoPrincipalVisivel");
                Messenger.Default.Send<string>("Aguardando a operação na janela " + pagina.Name, "TextoFundoPrincipal");

                var w = testWindow;

                pagina.ComandoFechar = new RelayCommand(
                            param => { w.Tag = "Executado"; w.Close(); },
                            param => true
                        );

                w.ShowInTaskbar = true;
                w.Height = 335;
                w.Width = 300;
                w.ResizeMode = System.Windows.ResizeMode.NoResize;
                w.Content = pagina;
                w.ShowDialogsOverTitleBar = false;
                w.ShowDialog();
            }
        }
    }
}