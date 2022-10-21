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
    /// Interaction logic for AlterarSenhaComAtualView.xaml
    /// </summary>
    public partial class AlterarSenhaComAtualView : UserControl
    {
        public AlterarSenhaComAtualView()
        {
            InitializeComponent();
        }

        private void pboSenhaAtual_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).SenhaAtual = ((PasswordBox)sender).Password; }

            if (String.IsNullOrEmpty(pboSenhaAtual.Password))
            {
                bdgSenhaAtual.Badge = "Campo obrigatório";
                pboSenhaAtual.BorderBrush = Brushes.Red;
                pboSenhaAtual.BorderThickness = new Thickness(1);
            }
            else
            {
                bdgSenhaAtual.Badge = "";
                pboSenhaAtual.BorderBrush = Brushes.LightGray;
                pboSenhaAtual.BorderThickness = new Thickness(1);
            }
        }

        private void pboNovaSenha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).NovaSenha = ((PasswordBox)sender).Password; }

            if (String.IsNullOrEmpty(pboNovaSenha.Password))
            {
                bdgNovaSenha.Badge = "Campo obrigatório";
                pboNovaSenha.BorderBrush = Brushes.Red;
                pboNovaSenha.BorderThickness = new Thickness(1);
            }
            else
            {
                bdgNovaSenha.Badge = "";
                pboNovaSenha.BorderBrush = Brushes.LightGray;
                pboNovaSenha.BorderThickness = new Thickness(1);
            }
        }

        private void pboSenhaConfirmacao_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).ConfirmacaoSenha = ((PasswordBox)sender).Password; }

            if (String.IsNullOrEmpty(pboSenhaConfirmacao.Password))
            {
                bdgConfirmacaoSenha.Badge = "Campo obrigatório";
                pboSenhaConfirmacao.BorderBrush = Brushes.Red;
                pboSenhaConfirmacao.BorderThickness = new Thickness(1);
            }
            else
            {
                if (pboNovaSenha.Password != pboSenhaConfirmacao.Password)
                {
                    bdgConfirmacaoSenha.Badge = "Confirmação não confere";
                    pboSenhaConfirmacao.BorderBrush = Brushes.Red;
                    pboSenhaConfirmacao.BorderThickness = new Thickness(1);
                }
                else
                {
                    bdgConfirmacaoSenha.Badge = "";
                    pboSenhaConfirmacao.BorderBrush = Brushes.LightGray;
                    pboSenhaConfirmacao.BorderThickness = new Thickness(1);
                }
            }
        }

        private void mniSalvar_Click(object sender, RoutedEventArgs e)
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

            if (String.IsNullOrEmpty(pboSenhaAtual.Password))
            {
                bdgSenhaAtual.Badge = "Campo obrigatório";
                pboSenhaAtual.BorderBrush = Brushes.Red;
                pboSenhaAtual.BorderThickness = new Thickness(1);
                existemCamposVazios = true;
            }
            else
            {
                bdgSenhaAtual.Badge = "";
                pboSenhaAtual.BorderBrush = Brushes.LightGray;
                pboSenhaAtual.BorderThickness = new Thickness(1);
            }

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

            if (String.IsNullOrEmpty(pboSenhaConfirmacao.Password))
            {
                bdgConfirmacaoSenha.Badge = "Campo obrigatório";
                pboSenhaConfirmacao.BorderBrush = Brushes.Red;
                pboSenhaConfirmacao.BorderThickness = new Thickness(1);
                existemCamposVazios = true;
            }
            else
            {
                if (pboNovaSenha.Password != pboSenhaConfirmacao.Password)
                {
                    bdgConfirmacaoSenha.Badge = "Confirmação não confere";
                    pboSenhaConfirmacao.BorderBrush = Brushes.Red;
                    pboSenhaConfirmacao.BorderThickness = new Thickness(1);
                    existemCamposVazios = true;
                }
                else
                {
                    bdgConfirmacaoSenha.Badge = "";
                    pboSenhaConfirmacao.BorderBrush = Brushes.LightGray;
                    pboSenhaConfirmacao.BorderThickness = new Thickness(1);
                }
            }

            return existemCamposVazios;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).CloseAction = new Action(Window.GetWindow(this).Close); }
        }
    }
}
