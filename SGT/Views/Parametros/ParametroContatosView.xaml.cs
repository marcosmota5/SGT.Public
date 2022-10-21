using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ParametroContatosView.xaml
    /// </summary>
    public partial class ParametroContatosView : UserControl
    {
        public ParametroContatosView()
        {
            InitializeComponent();
        }


        // Evento para limpar o formato do telefone quando o usuário focar nele
        private void nudTelefone_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).FormatoTelefone = ""; }
        }

        // Evento para inserir o formato do telefone quando o controle perder o foco
        private void nudTelefone_LostFocus(object sender, RoutedEventArgs e)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            string telefone = Regex.Replace(Convert.ToString(nudTelefone.Value), @"[^\d]", "");
#pragma warning restore CS8604 // Possible null reference argument.


            if (this.DataContext != null)
            {
                if (telefone.Length > 10)
                {
                    ((dynamic)this.DataContext).FormatoTelefone = @"\(00\)\ 00000\-0000";
                }
                else
                {
                    ((dynamic)this.DataContext).FormatoTelefone = @"\(00\)\ 0000\-0000";
                }
            }
        }

        // Evento para permitir que o usuário digite no máximo 11 digitos
        private void nudTelefone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Tab)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                string telefone = Regex.Replace(Convert.ToString(nudTelefone.Value), @"[^\d]", "");
#pragma warning restore CS8604 // Possible null reference argument.

                if (telefone.Length > 10)
                {
                    e.Handled = true;
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
            // Lista dos elementos obrigatórios
            List<FrameworkElement> listaElementosObrigatorios = new()
            {
                cboCliente,
                cboStatus,
                cboPais,
                cboEstado,
                cboCidade,
                txtNome
            };

            // Lista com as propriedades a serem verificadas nos elementos obrigatórios
            List<DependencyProperty> listaPropriedadesObrigatorias = new()
            {
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                TextBox.TextProperty
            };

            // Define a verificação da existência de campos vazios como falso
            bool existemCamposVazios = false;

            // Laço para varrer os itens e verificar se existem campos vazios
            for (int i = 0; i < listaElementosObrigatorios.Count; i++)
            {
                // Atualiza as validações
                listaElementosObrigatorios[i].GetBindingExpression(listaPropriedadesObrigatorias[i]).UpdateSource();

                if (listaElementosObrigatorios[i].Visibility == Visibility.Visible)
                {
                    if (Validation.GetHasError(listaElementosObrigatorios[i]))
                    {
                        existemCamposVazios = true;
                    }
                }
            }

            return existemCamposVazios;
        }
    }
}
