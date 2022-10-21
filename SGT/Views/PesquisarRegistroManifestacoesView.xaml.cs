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
    /// Interaction logic for PesquisarRegistroManifestacoesView.xaml
    /// </summary>
    public partial class PesquisarRegistroManifestacoesView : UserControl
    {
        public PesquisarRegistroManifestacoesView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.DataContext != null)
                {
                    ((dynamic)this.DataContext).Janela = Window.GetWindow(this);
                    ((dynamic)this.DataContext).DataGrid = GridPesquisa;
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao carregar pesquisa para exportação");
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
        private double autoHeight;

        private GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();

        private void dtgComentarios_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            if (this.GridPesquisa.GridColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out autoHeight))
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