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
    /// Interaction logic for ControleEventoOrdemServicoView.xaml
    /// </summary>
    public partial class ControleEventoOrdemServicoView : UserControl
    {
        public ControleEventoOrdemServicoView()
        {
            InitializeComponent();
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
                datDataInicio,
                datDataFim,
                cboEvento,
                cboStatus
            };

            // Lista com as propriedades a serem verificadas nos elementos obrigatórios
            List<DependencyProperty> listaPropriedadesObrigatorias = new()
            {
                MahApps.Metro.Controls.DateTimePicker.SelectedDateTimeProperty,
                MahApps.Metro.Controls.DateTimePicker.SelectedDateTimeProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty
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

        /// <summary>
        /// Evento que passa a verificação de campos vazios para o ViewModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ExistemCamposVazios = ExistemCamposVazios();
            }

            if (!ExistemCamposVazios())
            {
                bdgSalvar.Badge = "";
            }
            else
            {
                bdgSalvar.Badge = "Campos obrigatórios vazios";
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((dynamic)this.DataContext).LimparViewModel();
                this.DataContext = null;
            }
            catch (Exception)
            {
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            int numLines = textBox.Text.Split('\n').Length;
            if (numLines > 1)
            {
                textBox.FontSize = 9;
            }
            else
            {
                textBox.FontSize = 12;
            }
        }
    }
}