using GalaSoft.MvvmLight.Messaging;
using SGT.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SGT
{
    /// <summary>
    /// Interaction logic for DockingWindow.xaml
    /// </summary>
    public partial class DockingWindow
    {
        public DockingWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<bool>(this, "FundoPrincipalVisivel", delegate (bool fundoPrincipalVisivel)
            {
                this.grdFundo.Visibility = fundoPrincipalVisivel == true ? Visibility.Visible : Visibility.Collapsed;
                this.grdPrincipal.IsEnabled = !fundoPrincipalVisivel;
            });
            Messenger.Default.Register<string>(this, "TextoFundoPrincipal", delegate (string textoFundoPrincipal)
            {
                this.lblTextoFundo.Content = textoFundoPrincipal;
            });

            Messenger.Default.Register<IPageViewModel>(this, "PrincipalPaginaRemover", delegate (IPageViewModel paginaPropostaRemover)
            {
                this.Close();
            });

            Storyboard sb = Resources["FadeInContentAnim"] as Storyboard;
            sb?.Begin();
        }
    }
}