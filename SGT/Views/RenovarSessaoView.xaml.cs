﻿using System;
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
    /// Interaction logic for RenovarSessaoView.xaml
    /// </summary>
    public partial class RenovarSessaoView : UserControl
    {
        public RenovarSessaoView()
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
    }
}