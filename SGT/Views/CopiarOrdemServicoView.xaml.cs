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
    /// Interaction logic for CopiarOrdemServicoView.xaml
    /// </summary>
    public partial class CopiarOrdemServicoView : UserControl
    {
        public CopiarOrdemServicoView()
        {
            InitializeComponent();
        }

        private void ItensDisponiveis_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            List<object> lista = new();

            foreach (var item in sfdgItensDisponiveis.SelectedItems)
            {
                lista.Add(item);
            }

            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ListaItensOrdemServicoDisponiveisSelecionados = lista;
            }
        }

        private void ItensACopiar_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            List<object> lista = new();

            foreach (var item in sfdgItensACopiar.SelectedItems)
            {
                lista.Add(item);
            }

            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ListaItensOrdemServicoCopiadosSelecionados = lista;
            }
        }

        private void EventosDisponiveis_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            List<object> lista = new();

            foreach (var item in sfdgEventosDisponiveis.SelectedItems)
            {
                lista.Add(item);
            }

            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ListaEventosOrdemServicoDisponiveisSelecionados = lista;
            }
        }

        private void EventosACopiar_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            List<object> lista = new();

            foreach (var item in sfdgEventosACopiar.SelectedItems)
            {
                lista.Add(item);
            }

            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ListaEventosOrdemServicoCopiadosSelecionados = lista;
            }
        }

        private void InconsistenciasDisponiveis_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            List<object> lista = new();

            foreach (var item in sfdgInconsistenciasDisponiveis.SelectedItems)
            {
                lista.Add(item);
            }

            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ListaInconsistenciasOrdemServicoDisponiveisSelecionados = lista;
            }
        }

        private void InconsistenciasACopiar_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            List<object> lista = new();

            foreach (var item in sfdgInconsistenciasACopiar.SelectedItems)
            {
                lista.Add(item);
            }

            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ListaInconsistenciasOrdemServicoCopiadosSelecionados = lista;
            }
        }
    }
}