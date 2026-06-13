using Bank.Model.Managers;
using BankingSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BankingSystem.Views
{
    /// <summary>
    /// Interaction logic for CustomerDashboardView.xaml
    /// </summary>
    public partial class CustomerDashboardView : UserControl
    {
        public CustomerDashboardView(BankSystem bankSystem)
        {
            InitializeComponent();
            DataContext = new CustomerDashboardViewModel(bankSystem);
        }
    }
}
