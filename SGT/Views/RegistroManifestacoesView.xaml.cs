using Syncfusion.UI.Xaml.Grid;
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
    /// Interaction logic for RegistroManifestacoesView.xaml
    /// </summary>
    public partial class RegistroManifestacoesView : UserControl
    {
        public RegistroManifestacoesView()
        {
            InitializeComponent();
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
                cboPrioridadeManifestacao,
                cboTipoManifestacao,
                cboStatusManifestacao,
                txtDescricaoAbertura,
                txtNomePessoaAbertura,
                txtEmailPessoaAbertura,
                txtDescricaoFechamento,
                txtNomePessoaFechamento,
                txtEmailPessoaFechamento,
                datDataAbertura,
                datDataFechamento
            };

            // Lista com as propriedades a serem verificadas nos elementos obrigatórios
            List<DependencyProperty> listaPropriedadesObrigatorias = new()
            {
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                TextBox.TextProperty,
                TextBox.TextProperty,
                TextBox.TextProperty,
                TextBox.TextProperty,
                TextBox.TextProperty,
                TextBox.TextProperty,
                MahApps.Metro.Controls.DateTimePicker.SelectedDateTimeProperty,
                MahApps.Metro.Controls.DateTimePicker.SelectedDateTimeProperty
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).Janela = Window.GetWindow(this);
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

        //To get the calculated height from GetAutoRowHeight method.
        double autoHeight;
        GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();

        private void dtgComentarios_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            if (this.dtgComentarios.GridColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out autoHeight))
            {
                if (autoHeight > 24)
                {
                    e.Height = autoHeight;
                    e.Handled = true;
                }
            }
        }
    }
}
