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
    /// Interaction logic for PesquisarOrdemServicoView.xaml
    /// </summary>
    public partial class PesquisarOrdemServicoView : UserControl
    {
        public PesquisarOrdemServicoView()
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

                foreach (var item in ((dynamic)this.DataContext).ListaObjetoSelecionavelTipoOrdemServico)
                {
                    item.Selecionado = false;
                }

                foreach (var item in sfdTipoOrdemServico.SelectedItems)
                {
                    ((dynamic)item).Selecionado = true;
                }

                foreach (var item in ((dynamic)this.DataContext).ListaObjetoSelecionavelEquipamentoAposManutencao)
                {
                    item.Selecionado = false;
                }

                foreach (var item in sfdEquipamentoAposManutencao.SelectedItems)
                {
                    ((dynamic)item).Selecionado = true;
                }

                foreach (var item in ((dynamic)this.DataContext).ListaObjetoSelecionavelTipoManutencao)
                {
                    item.Selecionado = false;
                }

                foreach (var item in sfdTipoManutencao.SelectedItems)
                {
                    ((dynamic)item).Selecionado = true;
                }

                foreach (var item in ((dynamic)this.DataContext).ListaObjetoSelecionavelExecutanteServico)
                {
                    item.Selecionado = false;
                }

                foreach (var item in sfdExecutanteServico.SelectedItems)
                {
                    ((dynamic)item).Selecionado = true;
                }

                foreach (var item in ((dynamic)this.DataContext).ListaObjetoSelecionavelPassosExecutados)
                {
                    item.Selecionado = false;
                }

                foreach (var item in sfdPassosExecutados.SelectedItems)
                {
                    ((dynamic)item).Selecionado = true;
                }
            }
        }
    }
}