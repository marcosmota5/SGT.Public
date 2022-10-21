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
    /// Interaction logic for CopiarPropostaView.xaml
    /// </summary>
    public partial class CopiarPropostaView : UserControl
    {
        public CopiarPropostaView()
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
                ((dynamic)this.DataContext).ListaItensPropostaDisponiveisSelecionados = lista;
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
                ((dynamic)this.DataContext).ListaItensPropostaCopiadosSelecionados = lista;
            }
        }
    }
}