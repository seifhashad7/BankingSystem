using Bank.Model.Entities;
using Bank.Model.Logging;
using Bank.Model.Managers;
using BankingSystem.ViewModel;
using System.IO;
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
using Path = System.IO.Path;

namespace BankingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Manual Dependency injection of needed services
            var logger = new Logger();
            string connectionString = "server=localhost;database=BankSystemDB;user=deskUser;password=1234;";
            BankSystem.Initialize(connectionString);
            DataContext = new MainViewModel(BankSystem.Instance);
        }
    }
}