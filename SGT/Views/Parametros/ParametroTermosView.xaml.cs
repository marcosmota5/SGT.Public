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
    /// Interaction logic for ParametroTermosView.xaml
    /// </summary>
    public partial class ParametroTermosView : UserControl
    {
        public ParametroTermosView()
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
                cboStatus,
                txtNome,
            };

            // Lista com as propriedades a serem verificadas nos elementos obrigatórios
            List<DependencyProperty> listaPropriedadesObrigatorias = new()
            {
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

        private void dtgSetoresDisponiveis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<object> listaSetores = new();

            foreach (var item in dtgSetoresDisponiveis.SelectedItems)
            {
                listaSetores.Add(item);
            }

            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ListaSetoresDisponiveisSelecionados = listaSetores;
            }
        }

        private void dtgSetoresAssociados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<object> listaSetores = new();

            foreach (var item in dtgSetoresAssociados.SelectedItems)
            {
                listaSetores.Add(item);
            }

            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ListaSetoresAssociadosSelecionados = listaSetores;
            }
        }

        private void dtgClientesDisponiveis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<object> listaClientes = new();

            foreach (var item in dtgClientesDisponiveis.SelectedItems)
            {
                listaClientes.Add(item);
            }

            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ListaClientesDisponiveisSelecionados = listaClientes;
            }
        }

        private void dtgClientesAssociados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<object> listaClientes = new();

            foreach (var item in dtgClientesAssociados.SelectedItems)
            {
                listaClientes.Add(item);
            }

            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ListaClientesAssociadosSelecionados = listaClientes;
            }
        }

        //To get the calculated height from GetAutoRowHeight method.
        private double autoHeight;

        private Syncfusion.UI.Xaml.Grid.GridRowSizingOptions gridRowResizingOptions = new Syncfusion.UI.Xaml.Grid.GridRowSizingOptions();

        private void GridResultados_QueryRowHeight(object sender, Syncfusion.UI.Xaml.Grid.QueryRowHeightEventArgs e)
        {
            if (this.GridResultados.GridColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out autoHeight))
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