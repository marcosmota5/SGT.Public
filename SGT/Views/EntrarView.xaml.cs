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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SGT.Views
{
    /// <summary>
    /// Interaction logic for EntrarView.xaml
    /// </summary>
    public partial class EntrarView : UserControl
    {
        public EntrarView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento para guardar o valor digitado na caixa de senha já que ele não pode ser acessado por bindings comuns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Senha = ((PasswordBox)sender).Password; }
        }
    }
}
