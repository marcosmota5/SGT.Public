using Model.DataAccessLayer.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for PropostaView.xaml
    /// </summary>
    public partial class PropostaView : UserControl
    {
        public PropostaView()
        {
            InitializeComponent();

            grdOpcoesTermos.Children.Remove(cboCopiarDeClienteAtual);
            grdOpcoesTermos.Children.Remove(cboCopiarDeClienteETermoCliente);
            grdOpcoesTermos.Children.Remove(grdCopiarDeClienteETermo);
            grdOpcoesTermos.Children.Remove(cboCopiarDeClienteEPropostaCliente);
            grdOpcoesTermos.Children.Remove(grdCopiarDeClienteEProposta);
            grdOpcoesTermos.Children.Remove(cboCopiarDeSetorETermoSetor);
            grdOpcoesTermos.Children.Remove(grdCopiarDeSetorETermo);
            grdOpcoesTermos.Children.Remove(cboCopiarTermo);
        }

        // Evento para limpar o formato do telefone quando o usuário focar nele
        private void nudTelefone_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).FormatoTelefone = ""; }
        }

        // Evento para inserir o formato do telefone quando o controle perder o foco
        private void nudTelefone_LostFocus(object sender, RoutedEventArgs e)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            string telefone = Regex.Replace(Convert.ToString(nudTelefone.Value), @"[^\d]", "");
#pragma warning restore CS8604 // Possible null reference argument.

            if (this.DataContext != null)
            {
                if (telefone.Length > 10)
                {
                    ((dynamic)this.DataContext).FormatoTelefone = @"\(00\)\ 00000\-0000";
                }
                else
                {
                    ((dynamic)this.DataContext).FormatoTelefone = @"\(00\)\ 0000\-0000";
                }
            }
        }

        // Evento para permitir que o usuário digite no máximo 11 digitos
        private void nudTelefone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Tab)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                string telefone = Regex.Replace(Convert.ToString(nudTelefone.Value), @"[^\d]", "");
#pragma warning restore CS8604 // Possible null reference argument.

                if (telefone.Length > 10)
                {
                    e.Handled = true;
                }
            }
        }

        private void cboCopiarDe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnConfirmarCopia.Visibility = Visibility.Visible;

            switch (cboCopiarDe.SelectedIndex)
            {
                case 0:
                    cboCopiarDeClienteAtual.Visibility = Visibility.Visible;
                    tccTransicaoTermos.Content = cboCopiarDeClienteAtual;
                    break;

                case 1:
                    grdCopiarDeClienteETermo.Visibility = Visibility.Visible;
                    tccTransicaoTermos.Content = grdCopiarDeClienteETermo;
                    break;

                case 2:
                    grdCopiarDeClienteEProposta.Visibility = Visibility.Visible;
                    tccTransicaoTermos.Content = grdCopiarDeClienteEProposta;
                    break;

                case 3:
                    grdCopiarDeSetorETermo.Visibility = Visibility.Visible;
                    tccTransicaoTermos.Content = grdCopiarDeSetorETermo;
                    break;

                case 4:
                    cboCopiarTermo.Visibility = Visibility.Visible;
                    tccTransicaoTermos.Content = cboCopiarTermo;
                    break;

                default:
                    cboCopiarTermo.Visibility = Visibility.Visible;
                    tccTransicaoTermos.Content = cboCopiarTermo;
                    break;
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
                cboCliente,
                cboContato,
                cboPrioridade,
                cboStatus,
                cboCidade,
                datDataSolicitacao,
                txtCodigoProposta,
                txtEmail
            };

            // Lista com as propriedades a serem verificadas nos elementos obrigatórios
            List<DependencyProperty> listaPropriedadesObrigatorias = new()
            {
                ComboBox.SelectedItemProperty,
                ComboBox.TextProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                MahApps.Metro.Controls.DateTimePicker.SelectedDateTimeProperty,
                TextBox.TextProperty,
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.DataContext != null)
                {
                    ((dynamic)this.DataContext).DataGrid = GridItens;
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao carregar pesquisa para exportação");
            }
        }

        private void mniStatusAprovacaoApenasItemSelecionado_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).IdStatusAprovacaoUnico = (int)((MenuItem)sender).Tag;
                ((dynamic)this.DataContext).ComandoAlteraStatusAprovacaoUnico.Execute(false);
            }
        }

        private void mniStatusAprovacaoTodosOsItens_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).IdStatusAprovacaoTodos = (int)((MenuItem)sender).Tag;
                ((dynamic)this.DataContext).ComandoAlteraStatusAprovacaoTodos.Execute(false);
            }
        }

        private void mniJustificativaAprovacaoApenasItemSelecionado_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).IdJustificativaAprovacaoUnico = (int)((MenuItem)sender).Tag;
                ((dynamic)this.DataContext).ComandoAlteraJustificativaAprovacaoUnico.Execute(false);
            }
        }

        private void mniJustificativaAprovacaoTodosOsItens_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).IdJustificativaAprovacaoTodos = (int)((MenuItem)sender).Tag;
                ((dynamic)this.DataContext).ComandoAlteraJustificativaAprovacaoTodos.Execute(false);
            }
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

        //To get the calculated height from GetAutoRowHeight method.
        private double autoHeight;

        private Syncfusion.UI.Xaml.Grid.GridRowSizingOptions gridRowResizingOptions = new Syncfusion.UI.Xaml.Grid.GridRowSizingOptions();

        private void dtgComentarios_QueryRowHeight(object sender, Syncfusion.UI.Xaml.Grid.QueryRowHeightEventArgs e)
        {
            if (this.GridItens.GridColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out autoHeight))
            {
                if (autoHeight > 24)
                {
                    if (e.RowIndex == 0)
                    {
                        e.Height = 25;
                    }
                    else
                    {
                        e.Height = autoHeight;
                    }
                    e.Handled = true;
                }
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
            ((dynamic)this.DataContext).ListaItensPropostaSelecionados.Clear();

            foreach (var item in GridItens.SelectedItems)
            {
                ((dynamic)this.DataContext).ListaItensPropostaSelecionados.Add((ItemProposta)item);
            }
        }
    }
}