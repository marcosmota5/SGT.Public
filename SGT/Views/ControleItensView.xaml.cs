using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SGT.Views
{
    /// <summary>
    /// Interaction logic for ControleItensView.xaml
    /// </summary>
    public partial class ControleItensView : UserControl
    {
        public ControleItensView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento apenas para mudar a visibilidade dos itens de acordo com o tipo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboTipoItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cboTipoItem.SelectedIndex == 0)
            {

                cboFornecedor.Visibility = Visibility.Visible;
                cboCodigo.Visibility = Visibility.Visible;
                nudQuantidade.Visibility = Visibility.Visible;
                grdPercentuais.Visibility = Visibility.Visible;
                nudValorST.Visibility = Visibility.Visible;
                nudPrecoTotal.Visibility = Visibility.Visible;
                nudFreteUnitario.Visibility = Visibility.Visible;
                nudDescontoCusto.Visibility = Visibility.Visible;
                cboTipoSubstituicaoTributaria.Visibility = Visibility.Visible;
                cboOrigem.Visibility = Visibility.Visible;
                cboConjunto.Visibility = Visibility.Visible;
                cboEspecificacao.Visibility = Visibility.Visible;

                txtDescricao.SetValue(Grid.ColumnProperty, 2);
                txtDescricao.SetValue(Grid.ColumnSpanProperty, 3);
                nudPrecoUnitario.SetValue(Grid.ColumnProperty, 1);
                nudDescontoFinal.SetValue(Grid.ColumnProperty, 4);
                nudDescontoFinal.SetValue(Grid.RowProperty, 2);
                grdPrazos.SetValue(Grid.RowProperty, 3);
                txtComentarios.SetValue(Grid.ColumnProperty, 2);
                txtComentarios.SetValue(Grid.ColumnSpanProperty, 1);

                GridLengthConverter gridLengthConverter = new GridLengthConverter();
                rowItem2.Height = new GridLength(1.0, GridUnitType.Star);
                rowItem3.Height = new GridLength(1.0, GridUnitType.Star);

                rowControleItem1.Height = new GridLength(3.5, GridUnitType.Star);
                this.Height = 500;
            }
            else
            {
                cboFornecedor.Visibility = Visibility.Collapsed;
                cboCodigo.Visibility = Visibility.Collapsed;
                nudQuantidade.Visibility = Visibility.Collapsed;
                grdPercentuais.Visibility = Visibility.Collapsed;
                nudValorST.Visibility = Visibility.Collapsed;
                nudPrecoTotal.Visibility = Visibility.Collapsed;
                nudFreteUnitario.Visibility = Visibility.Collapsed;
                nudDescontoCusto.Visibility = Visibility.Collapsed;
                cboTipoSubstituicaoTributaria.Visibility = Visibility.Collapsed;
                cboOrigem.Visibility = Visibility.Collapsed;
                cboConjunto.Visibility = Visibility.Collapsed;
                cboEspecificacao.Visibility = Visibility.Collapsed;

                txtDescricao.SetValue(Grid.ColumnProperty, 0);
                txtDescricao.SetValue(Grid.ColumnSpanProperty, 5);
                nudPrecoUnitario.SetValue(Grid.ColumnProperty, 0);
                nudDescontoFinal.SetValue(Grid.ColumnProperty, 1);
                nudDescontoFinal.SetValue(Grid.RowProperty, 1);
                grdPrazos.SetValue(Grid.RowProperty, 1);
                txtComentarios.SetValue(Grid.ColumnProperty, 0);
                txtComentarios.SetValue(Grid.ColumnSpanProperty, 3);

                GridLengthConverter gridLengthConverter = new GridLengthConverter();
                rowItem2.Height = new GridLength(0.0, GridUnitType.Star);
                rowItem3.Height = new GridLength(0.0, GridUnitType.Star);

                rowControleItem1.Height = new GridLength(2.0, GridUnitType.Star);
                this.Height = 450;
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
                cboTipoItem,
                cboStatus,
                cboStatusAprovacao,
                cboJustificativaAprovacao,
                cboFornecedor,
                cboTipoSubstituicaoTributaria,
                cboOrigem,
                nudQuantidade,
                nudPrecoUnitario,
                txtDescricao,
                txtPrazoInicial
            };

            // Lista com as propriedades a serem verificadas nos elementos obrigatórios
            List<DependencyProperty> listaPropriedadesObrigatorias = new()
            {
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
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

        /// <summary>
        /// Evento que passa a verificação de campos vazios para o ViewModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {

            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ExistemCamposVazios = ExistemCamposVazios();
            }

            if (!ExistemCamposVazios())
            {
                bdgSalvar.Badge = "";
            }
            else
            {
                bdgSalvar.Badge = "Campos obrigatórios vazios";
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            int numLines = textBox.Text.Split('\n').Length;
            if (numLines > 1)
            {
                textBox.FontSize = 9;
            }
            else
            {
                textBox.FontSize = 12;
            }
        }
    }
}