using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using MahApps.Metro.Controls;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;

namespace SGT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ////if (this.WindowState == WindowState.Maximized)
            ////{
            //    tccConteudo.Width = e.NewSize.Width;
            //    tccConteudo.Height = e.NewSize.Height;
            //}
        }

        /// <summary>
        /// Evento que lida com o fechamento da janela principal e registra no log o encerramento do sistema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                if (App.Usuario != null)
                {
                    App.Usuario.LimpaIdUsuarioEmUsoAsync(System.Threading.CancellationToken.None).Await();
                }
            }
            catch (Exception)
            {
            }

            Serilog.Log.Information("Sistema encerrado");
            Serilog.Log.CloseAndFlush();
        }
    }
}