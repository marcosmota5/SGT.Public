using System;
using System.Windows;
using System.Windows.Controls;

namespace SGT.Views
{
    /// <summary>
    /// Interaction logic for VisualizarPropostaView.xaml
    /// </summary>
    public partial class VisualizarPropostaView : UserControl
    {
        public VisualizarPropostaView()
        {
            InitializeComponent();
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.DataContext != null)
                {
                    ((dynamic)this.DataContext).ReportViewer = reportViewer;
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao carregar a visualização da proposta");
            }
            
        }
    }
}
