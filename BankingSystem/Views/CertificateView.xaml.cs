using BankingSystem.Model.Contracts;
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
    /// Interaction logic for CertificateView.xaml
    /// </summary>
    public partial class CertificateView : UserControl
    {
        public CertificateView(IBankProdService bankProdService) 
        {
            InitializeComponent();
            DataContext = new CertificateViewModel(bankProdService);
        }
    }
}
