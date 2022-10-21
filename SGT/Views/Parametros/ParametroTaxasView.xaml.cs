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
    /// Interaction logic for ParametroTaxasView.xaml
    /// </summary>
    public partial class ParametroTaxasView : UserControl
    {
        public ParametroTaxasView()
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
                txtNome,
                datDataInicio,
                nudAliquotaExternaICMSNacional,
                nudAliquotaInternaICMSNacional,
                nudRateioDespesaFixaItemNacional,
                nudPercentualMvaItemNacional,
                nudImpostosFederaisItemNacional,
                nudPercentualAcrescimoItemNacional,
                nudPercentualLucroNecessarioItemRevendaStNacional,
                nudPercentualLucroNecessarioItemOutrosNacional,
                nudAliquotaExternaICMSImportado,
                nudAliquotaInternaICMSImportado,
                nudRateioDespesaFixaItemImportado,
                nudPercentualMvaItemImportado,
                nudImpostosFederaisItemImportado,
                nudPercentualAcrescimoItemImportado,
                nudPercentualLucroNecessarioItemRevendaStImportado,
                nudPercentualLucroNecessarioItemOutrosImportado
            };

            // Lista com as propriedades a serem verificadas nos elementos obrigatórios
            List<DependencyProperty> listaPropriedadesObrigatorias = new()
            {
                TextBox.TextProperty,
                DatePicker.SelectedDateProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty,
                MahApps.Metro.Controls.NumericUpDown.ValueProperty
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
    }
}
