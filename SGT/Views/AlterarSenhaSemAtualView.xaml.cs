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
    /// Interaction logic for AlterarSenhaSemAtualView.xaml
    /// </summary>
    public partial class AlterarSenhaSemAtualView : UserControl
    {
        public AlterarSenhaSemAtualView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento para guardar o valor digitado na caixa de senha já que ele não pode ser acessado por bindings comuns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NovaSenha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).NovaSenha = ((PasswordBox)sender).Password; }
        }

        /// <summary>
        /// Evento para guardar o valor digitado na caixa de senha já que ele não pode ser acessado por bindings comuns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmacaoSenha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).ConfirmacaoSenha = ((PasswordBox)sender).Password; }
        }

        private void btnAlterarASenha_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ExistemCamposVazios = ExistemCamposVazios();
            }
        }

        /// <summary>
        /// Método que verifica se existem campos vazios
        /// </summary>
        /// <returns>Valor booleano indicando se existem campos vazios</returns>
        private bool ExistemCamposVazios()
        {

            // Define a verificação da existência de campos vazios como falso
            bool existemCamposVazios = false;

            if (String.IsNullOrEmpty(pboNovaSenha.Password))
            {
                bdgNovaSenha.Badge = "Campo obrigatório";
                pboNovaSenha.BorderBrush = Brushes.Red;
                pboNovaSenha.BorderThickness = new Thickness(1);
                existemCamposVazios = true;
            }
            else
            {
                bdgNovaSenha.Badge = "";
                pboNovaSenha.BorderBrush = Brushes.LightGray;
                pboNovaSenha.BorderThickness = new Thickness(1);
            }

            if (String.IsNullOrEmpty(pboConfirmarNovaSenha.Password))
            {
                bdgConfirmarNovaSenha.Badge = "Campo obrigatório";
                pboConfirmarNovaSenha.BorderBrush = Brushes.Red;
                pboConfirmarNovaSenha.BorderThickness = new Thickness(1);
                existemCamposVazios = true;
            }
            else
            {
                if (pboNovaSenha.Password != pboConfirmarNovaSenha.Password)
                {
                    bdgConfirmarNovaSenha.Badge = "Confirmação não confere";
                    pboConfirmarNovaSenha.BorderBrush = Brushes.Red;
                    pboConfirmarNovaSenha.BorderThickness = new Thickness(1);
                    existemCamposVazios = true;
                }
                else
                {
                    bdgConfirmarNovaSenha.Badge = "";
                    pboConfirmarNovaSenha.BorderBrush = Brushes.LightGray;
                    pboConfirmarNovaSenha.BorderThickness = new Thickness(1);
                }
            }

            return existemCamposVazios;
        }
    }
}
