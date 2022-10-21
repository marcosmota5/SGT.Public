using System;
using System.Collections.Generic;
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
    /// Interaction logic for ParametroFiliaisView.xaml
    /// </summary>
    public partial class ParametroFiliaisView : UserControl
    {
        public ParametroFiliaisView()
        {
            InitializeComponent();
        }

        // Evento para limpar o formato do telefone quando o usuário focar nele
        private void nudTelefoneGeral_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).FormatoTelefoneGeral = ""; }
        }

        // Evento para inserir o formato do TelefoneGeral quando o controle perder o foco
        private void nudTelefoneGeral_LostFocus(object sender, RoutedEventArgs e)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            string TelefoneGeral = Regex.Replace(Convert.ToString(nudTelefoneGeral.Value), @"[^\d]", "");
#pragma warning restore CS8604 // Possible null reference argument.


            if (this.DataContext != null)
            {
                if (TelefoneGeral.Length > 10)
                {
                    ((dynamic)this.DataContext).FormatoTelefoneGeral = @"\(00\)\ 00000\-0000";
                }
                else
                {
                    ((dynamic)this.DataContext).FormatoTelefoneGeral = @"\(00\)\ 0000\-0000";
                }
            }
        }

        // Evento para permitir que o usuário digite no máximo 11 digitos
        private void nudTelefoneGeral_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Tab)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                string TelefoneGeral = Regex.Replace(Convert.ToString(nudTelefoneGeral.Value), @"[^\d]", "");
#pragma warning restore CS8604 // Possible null reference argument.

                if (TelefoneGeral.Length > 10)
                {
                    e.Handled = true;
                }
            }
        }

        // Evento para limpar o formato do telefone quando o usuário focar nele
        private void nudTelefonePecas_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).FormatoTelefonePecas = ""; }
        }

        // Evento para inserir o formato do TelefonePecas quando o controle perder o foco
        private void nudTelefonePecas_LostFocus(object sender, RoutedEventArgs e)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            string TelefonePecas = Regex.Replace(Convert.ToString(nudTelefonePecas.Value), @"[^\d]", "");
#pragma warning restore CS8604 // Possible null reference argument.


            if (this.DataContext != null)
            {
                if (TelefonePecas.Length > 10)
                {
                    ((dynamic)this.DataContext).FormatoTelefonePecas = @"\(00\)\ 00000\-0000";
                }
                else
                {
                    ((dynamic)this.DataContext).FormatoTelefonePecas = @"\(00\)\ 0000\-0000";
                }
            }
        }

        // Evento para permitir que o usuário digite no máximo 11 digitos
        private void nudTelefonePecas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Tab)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                string TelefonePecas = Regex.Replace(Convert.ToString(nudTelefonePecas.Value), @"[^\d]", "");
#pragma warning restore CS8604 // Possible null reference argument.

                if (TelefonePecas.Length > 10)
                {
                    e.Handled = true;
                }
            }
        }

        // Evento para limpar o formato do telefone quando o usuário focar nele
        private void nudTelefoneServicos_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).FormatoTelefoneServicos = ""; }
        }

        // Evento para inserir o formato do TelefoneServicos quando o controle perder o foco
        private void nudTelefoneServicos_LostFocus(object sender, RoutedEventArgs e)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            string TelefoneServicos = Regex.Replace(Convert.ToString(nudTelefoneServicos.Value), @"[^\d]", "");
#pragma warning restore CS8604 // Possible null reference argument.


            if (this.DataContext != null)
            {
                if (TelefoneServicos.Length > 10)
                {
                    ((dynamic)this.DataContext).FormatoTelefoneServicos = @"\(00\)\ 00000\-0000";
                }
                else
                {
                    ((dynamic)this.DataContext).FormatoTelefoneServicos = @"\(00\)\ 0000\-0000";
                }
            }
        }

        // Evento para permitir que o usuário digite no máximo 11 digitos
        private void nudTelefoneServicos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Tab)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                string TelefoneServicos = Regex.Replace(Convert.ToString(nudTelefoneServicos.Value), @"[^\d]", "");
#pragma warning restore CS8604 // Possible null reference argument.

                if (TelefoneServicos.Length > 10)
                {
                    e.Handled = true;
                }
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
                cboEmpresa,
                cboStatus,
                cboPais,
                cboEstado,
                cboCidade,
                txtNome,
                txtEndereco,
                txtEmailGeral,
                txtEmailPecas,
                txtEmailServicos,
                nudCEP
            };

            // Lista com as propriedades a serem verificadas nos elementos obrigatórios
            List<DependencyProperty> listaPropriedadesObrigatorias = new()
            {
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                ComboBox.SelectedItemProperty,
                TextBox.TextProperty,
                TextBox.TextProperty,
                TextBox.TextProperty,
                TextBox.TextProperty,
                TextBox.TextProperty,
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
