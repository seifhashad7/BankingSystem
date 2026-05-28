using BankingSystem.Data;
using BankingSystem.Data.Services;
using BankingSystem.Model.Contracts;
using BankingSystem.Model.Logging;
using BankingSystem.ViewModel;
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
            var dbContext = new AppDbContext();
            var logger = new Logger();
            IAccountService accountService = new AccountService(dbContext, logger);
            IBankProdService bankProdService = new BankProdService(dbContext, logger);
            IReportingService reportingService = new ReportingService(dbContext, logger);
            DataContext = new MainViewModel(dbContext, accountService, bankProdService, reportingService);
        }
    }
}