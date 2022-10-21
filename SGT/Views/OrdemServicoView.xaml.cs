using Model.DataAccessLayer.Classes;
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
    /// Interaction logic for OrdemServicoView.xaml
    /// </summary>
    public partial class OrdemServicoView : UserControl
    {
        public OrdemServicoView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.DataContext != null)
                {
                    ((dynamic)this.DataContext).DataGrid = GridItens;
                    ((dynamic)this.DataContext).DataGridEventos = GridEventos;
                    ((dynamic)this.DataContext).DataGridInconsistencias = GridInconsistencias;
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao carregar pesquisa para exportação");
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
                cboTipo,
                cboStatus,
                cboCliente,
                cboPlanta,
                cboArea,
                cboFrota,
                cboSerie,
                cboFabricante,
                cboCategoria,
                cboTipoEquipamento,
                cboClasse,
                cboModelo,
                nudOrdemServicoAtual,
                nudNumeroChamado,
                datDataChamado,
                datDataAtendimento
            };

            // Lista com as propriedades a serem verificadas nos elementos obrigatórios
            List<DependencyProperty> listaPropriedadesObrigatorias = new()
            {
                ComboBox.TagProperty,
                ComboBox.TagProperty,
                ComboBox.TagProperty,
                ComboBox.TagProperty,
                ComboBox.TagProperty,
                ComboBox.TextProperty,
                ComboBox.TextProperty,
                ComboBox.TagProperty,
                ComboBox.TagProperty,
                ComboBox.TagProperty,
                ComboBox.TagProperty,
                ComboBox.TagProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.DateTimePicker.SelectedDateTimeProperty,
                MahApps.Metro.Controls.DateTimePicker.SelectedDateTimeProperty
            };

            if (tbiPagina2.Visibility == Visibility.Visible)
            {
                listaElementosObrigatorios.Add(grdEquipamentoOperacional);
                listaElementosObrigatorios.Add(nudHorimetro);
                listaElementosObrigatorios.Add(cboEquipamentoAposManutencao);
                listaElementosObrigatorios.Add(cboTipoManutencao);
                listaElementosObrigatorios.Add(txtMotivoAtendimento);
                listaElementosObrigatorios.Add(txtEntrevistaInicial);
                listaElementosObrigatorios.Add(txtIntervencao);
                listaPropriedadesObrigatorias.Add(Grid.TagProperty);
                listaPropriedadesObrigatorias.Add(MahApps.Metro.Controls.NumericUpDown.ValueProperty);
                listaPropriedadesObrigatorias.Add(ComboBox.TagProperty);
                listaPropriedadesObrigatorias.Add(ComboBox.TagProperty);
                listaPropriedadesObrigatorias.Add(TextBox.TextProperty);
                listaPropriedadesObrigatorias.Add(TextBox.TextProperty);
                listaPropriedadesObrigatorias.Add(TextBox.TextProperty);

                if (cboUsoIndevido.Visibility == Visibility.Visible)
                {
                    listaElementosObrigatorios.Add(cboUsoIndevido);
                    listaPropriedadesObrigatorias.Add(ComboBox.TagProperty);
                }

                if (nudHorasPreventiva.Visibility == Visibility.Visible)
                {
                    listaElementosObrigatorios.Add(nudHorasPreventiva);
                    listaPropriedadesObrigatorias.Add(MahApps.Metro.Controls.NumericUpDown.ValueProperty);
                }

                if (txtOutro.Visibility == Visibility.Visible)
                {
                    listaElementosObrigatorios.Add(txtOutro);
                    listaPropriedadesObrigatorias.Add(TextBox.TextProperty);
                }
            }

            if (tbiPagina4.Visibility == Visibility.Visible)
            {
                listaElementosObrigatorios.Add(cboExecutante);
                listaPropriedadesObrigatorias.Add(ComboBox.TagProperty);
            }

            // Define a verificação da existência de campos vazios como falso
            bool existemCamposVazios = false;

            // Laço para varrer os itens e verificar se existem campos vazios
            for (int i = 0; i < listaElementosObrigatorios.Count; i++)
            {
                // Atualiza as validações
                if (listaElementosObrigatorios[i] == grdEquipamentoOperacional && grdEquipamentoOperacional.Tag != null)
                {
                    //System.Diagnostics.Trace.WriteLine(listaElementosObrigatorios[i].Tag);
                    continue;
                }

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

        private void txbConjuntoEspecificacaoApenasItemSelecionado_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (((TextBlock)sender).Tag.GetType() == typeof(Conjunto) && ((Conjunto)((TextBlock)sender).Tag).ListaEspecificacoes.Count == 0)
            {
                ((dynamic)this.DataContext).ConjuntoUnico = (Conjunto)((TextBlock)sender).Tag;
                ((dynamic)this.DataContext).ComandoAlteraConjuntoUnico.Execute(false);
            }
            else if (((TextBlock)sender).Tag.GetType() == typeof(Especificacao))
            {
                ((dynamic)this.DataContext).EspecificacaoUnico = (Especificacao)((TextBlock)sender).Tag;
                ((dynamic)this.DataContext).ComandoAlteraEspecificacaoUnico.Execute(false);
            }
        }

        private void txbConjuntoEspecificacaoTodosOsItens_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (((TextBlock)sender).Tag.GetType() == typeof(Conjunto) && ((Conjunto)((TextBlock)sender).Tag).ListaEspecificacoes.Count == 0)
            {
                ((dynamic)this.DataContext).ConjuntoTodos = (Conjunto)((TextBlock)sender).Tag;
                ((dynamic)this.DataContext).ComandoAlteraConjuntoTodos.Execute(false);
            }
            else if (((TextBlock)sender).Tag.GetType() == typeof(Especificacao))
            {
                ((dynamic)this.DataContext).EspecificacaoTodos = (Especificacao)((TextBlock)sender).Tag;
                ((dynamic)this.DataContext).ComandoAlteraEspecificacaoTodos.Execute(false);
            }
        }

        private void ctl_Unloaded(object sender, RoutedEventArgs e)
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

        private void GridItens_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            ((dynamic)this.DataContext).ListaItensOrdemServicoSelecionados.Clear();

            foreach (var item in GridItens.SelectedItems)
            {
                ((dynamic)this.DataContext).ListaItensOrdemServicoSelecionados.Add((ItemOrdemServico)item);
            }
        }

        private void GridEventos_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            ((dynamic)this.DataContext).ListaEventosOrdemServicoSelecionados.Clear();

            foreach (var item in GridEventos.SelectedItems)
            {
                ((dynamic)this.DataContext).ListaEventosOrdemServicoSelecionados.Add((EventoOrdemServico)item);
            }
        }

        private void GridInconsistencias_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            ((dynamic)this.DataContext).ListaInconsistenciasOrdemServicoSelecionados.Clear();

            foreach (var item in GridInconsistencias.SelectedItems)
            {
                ((dynamic)this.DataContext).ListaInconsistenciasOrdemServicoSelecionados.Add((InconsistenciaOrdemServico)item);
            }
        }
    }
}