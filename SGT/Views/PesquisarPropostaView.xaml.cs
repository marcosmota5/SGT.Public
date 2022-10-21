using System;
using System.Windows;
using System.Windows.Controls;

namespace SGT.Views
{
    /// <summary>
    /// Interaction logic for PesquisarPropostaView.xaml
    /// </summary>
    public partial class PesquisarPropostaView : UserControl
    {
        public PesquisarPropostaView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.DataContext != null)
                {
                    ((dynamic)this.DataContext).DataGrid = GridPesquisa;
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao carregar pesquisa para exportação");
            }
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            if (((dynamic)this.DataContext).PesquisaBasica)
            {
                foreach (var item in ((dynamic)this.DataContext).ListaObjetoSelecionavelSetores)
                {
                    item.Selecionado = false;
                }

                foreach (var item in sfdSetor.SelectedItems)
                {
                    ((dynamic)item).Selecionado = true;
                }

                foreach (var item in ((dynamic)this.DataContext).ListaObjetoSelecionavelUsuariosInsercao)
                {
                    item.Selecionado = false;
                }

                foreach (var item in sfdUsuario.SelectedItems)
                {
                    ((dynamic)item).Selecionado = true;
                }

                foreach (var item in ((dynamic)this.DataContext).ListaObjetoSelecionavelClientes)
                {
                    item.Selecionado = false;
                }

                foreach (var item in sfdCliente.SelectedItems)
                {
                    ((dynamic)item).Selecionado = true;
                }

                foreach (var item in ((dynamic)this.DataContext).ListaObjetoSelecionavelStatusAprovacao)
                {
                    item.Selecionado = false;
                }

                foreach (var item in sfdStatusAprovacao.SelectedItems)
                {
                    ((dynamic)item).Selecionado = true;
                }

                foreach (var item in ((dynamic)this.DataContext).ListaObjetoSelecionavelJustificativaAprovacao)
                {
                    item.Selecionado = false;
                }

                foreach (var item in sfdJustificativaAprovacao.SelectedItems)
                {
                    ((dynamic)item).Selecionado = true;
                }

                foreach (var item in ((dynamic)this.DataContext).ListaObjetoSelecionavelPrioridades)
                {
                    item.Selecionado = false;
                }

                foreach (var item in sfdPrioridade.SelectedItems)
                {
                    ((dynamic)item).Selecionado = true;
                }
            }
        }
    }
}