using BankingSystem.Commands;
using BankingSystem.Data;
using BankingSystem.Data.Services;
using BankingSystem.Model.Contracts;
using BankingSystem.Model.Domain;
using BankingSystem.Model.Logging;
using BankingSystem.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BankingSystem.ViewModel
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IReportingService _reportingService;
        private readonly ILogger _logger;
        private UserControl? _currentView;
        private IEnumerable _currentGridData;
        private ObservableCollection<Customer> _customers;
        private ObservableCollection<Account> _accounts;
        private ObservableCollection<BankService> _services;
        private ObservableCollection<Transaction>  _transactions;

        public ICommand LoadCustomersDataCommand { get; }
        public ICommand LoadAccountsDataCommand { get; }
        public ICommand LoadSalaryAccDataCommand { get; }
        public ICommand LoadSavingAccDataCommand { get; }
        public ICommand LoadServicesDataCommand { get; }
        public ICommand LoadCreditCardsDataCommand { get; }
        public ICommand LoadACertificatesDataCommand { get; }
        public ICommand LoadStatsCommand { get; }
        public ICommand LoadTransactionsCommand { get; }
        public ICommand LoadHomeViewCommand { get; }
        
        public IEnumerable CurrentGridData
        {
            get => _currentGridData;
            set => SetProperty(ref _currentGridData, value);
        }

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => SetProperty(ref _customers, value);
        }

        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }

        public ObservableCollection<BankService> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }

        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set => SetProperty(ref _transactions, value);
        }

        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public class TableSet
        {
            public string? Category { get; set; }
            public int RecordCounts { get; set; }
        }

        public DashboardViewModel(IReportingService reportingService)
        {
            _reportingService = reportingService;
            _logger = new Logger();
            _customers = new ObservableCollection<Customer>();
            //Customers = new ObservableCollection<Customer>();
            LoadCustomersDataCommand = new RelayCommand(ExecuteLoadCustomersData);
            LoadAccountsDataCommand = new RelayCommand(ExecuteLoadAccountsData);
            LoadSalaryAccDataCommand = new RelayCommand(ExecuteLoadSalaryAccData);
            LoadSavingAccDataCommand = new RelayCommand(ExecuteLoadSavingAccData);
            LoadServicesDataCommand = new RelayCommand(ExecuteLoadServicesData);
            LoadCreditCardsDataCommand = new RelayCommand(ExecuteLoadCreditCardsData);
            LoadACertificatesDataCommand = new RelayCommand(ExecuteLoadCertificatesData);
            LoadStatsCommand = new RelayCommand(ExecuteLoadStats);
            LoadHomeViewCommand = new RelayCommand(ExecuteLoadHomeView);
            LoadTransactionsCommand = new RelayCommand(ExecuteLoadTransactions);
        }

        private void ExecuteLoadCustomersData(object o)
        {
            //Customers.Clear();

            //foreach(Customer c in _reportingService.GetCustomers())
            //{
            //    Customers.Add(c);
            //}
            Customers = new ObservableCollection<Customer>(_reportingService.GetCustomers());
            CurrentGridData = Customers;
        }

        private void ExecuteLoadAccountsData(object o)
        {
            Accounts = new ObservableCollection<Account>(_reportingService.GetAccounts());
            CurrentGridData = Accounts;
        }

        private void ExecuteLoadSalaryAccData(object o)
        {
            Accounts = new ObservableCollection<Account>(_reportingService.GetSalaryAccounts());
            CurrentGridData = Accounts;
        }
        private void ExecuteLoadSavingAccData(object o)
        {
            Accounts = new ObservableCollection<Account>(_reportingService.GetSavingAccounts());
            CurrentGridData = Accounts;
        }
        private void ExecuteLoadServicesData(object o)
        {
            Services = new ObservableCollection<BankService>(_reportingService.GetServices());
            CurrentGridData = Services;
        }
        private void ExecuteLoadCreditCardsData(object o)
        {
            Services = new ObservableCollection<BankService>(_reportingService.GetCreditCards());
            CurrentGridData = Services;
        }
        private void ExecuteLoadCertificatesData(object o)
        {
            Services = new ObservableCollection<BankService>(_reportingService.GetCertificates());
            CurrentGridData = Services;
        }
        private void ExecuteLoadStats(object o)
        {
            var stats = new List<TableSet>
            {
                new TableSet { Category="Customers", RecordCounts= _reportingService.GetTotalCustomers()},
                new TableSet { Category="Salary Accounts", RecordCounts= _reportingService.GetTotalSalaryAccounts()},
                new TableSet { Category="Saving Accounts", RecordCounts= _reportingService.GetTotalSavingAccounts()},
                new TableSet { Category="Credit Cards", RecordCounts= _reportingService.GetTotalCreditCards()},
                new TableSet { Category="Certificates", RecordCounts= _reportingService.GetTotalCertificates()},
                new TableSet { Category="Transactions", RecordCounts= _reportingService.GetTotalTransactions()}
            };

            CurrentGridData = stats;
        }
        private void ExecuteLoadTransactions(object o)
        {
            Transactions = new ObservableCollection<Transaction>(_reportingService.GetAllTransactions());
            CurrentGridData = Transactions;
        }

        private void ExecuteLoadHomeView(object o)
        {
            CurrentView = new HomeView();
        }
    }
}
