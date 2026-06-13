using Bank.Model.Entities;
using Bank.Model.Logging;
using BankingSystem.Commands;
using BankingSystem.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bank.Model.Managers;

namespace BankingSystem.ViewModel
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly BankSystem _bankSystem;
        private readonly Logger _logger;
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
            public decimal RecordCounts { get; set; }
        }

        public DashboardViewModel(BankSystem bankSystem)
        {
            _bankSystem = bankSystem;
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
            Customers = new ObservableCollection<Customer>(_bankSystem.GetAllCustomers());
            CurrentGridData = Customers;
        }

        private void ExecuteLoadAccountsData(object o)
        {
            Accounts = new ObservableCollection<Account>(_bankSystem.GetAllAccounts());
            CurrentGridData = Accounts;
        }

        private void ExecuteLoadSalaryAccData(object o)
        {
            Accounts = new ObservableCollection<Account>(_bankSystem.GetSalaryAccounts());
            CurrentGridData = Accounts;
        }
        private void ExecuteLoadSavingAccData(object o)
        {
            Accounts = new ObservableCollection<Account>(_bankSystem.GetSavingAccounts());
            CurrentGridData = Accounts;
        }
        private void ExecuteLoadServicesData(object o)
        {
            Services = new ObservableCollection<BankService>(_bankSystem.GetServices());
            CurrentGridData = Services;
        }
        private void ExecuteLoadCreditCardsData(object o)
        {
            Services = new ObservableCollection<BankService>(_bankSystem.GetCreditCards());
            CurrentGridData = Services;
        }
        private void ExecuteLoadCertificatesData(object o)
        {
            Services = new ObservableCollection<BankService>(_bankSystem.GetCertificates());
            CurrentGridData = Services;
        }
        private void ExecuteLoadStats(object o)
        {
            var stats = new List<TableSet>
            {
                new TableSet { Category="Customers", RecordCounts= _bankSystem.GetTotalCustomers()},
                new TableSet { Category="Salary Accounts", RecordCounts= _bankSystem.GetTotalSalaryAccounts()},
                new TableSet { Category="Saving Accounts", RecordCounts= _bankSystem.GetTotalSavingAccounts()},
                new TableSet { Category="Credit Cards", RecordCounts= _bankSystem.GetTotalCreditCards()},
                new TableSet { Category="Certificates", RecordCounts= _bankSystem.GetTotalCertificates()},
                new TableSet { Category="Transactions", RecordCounts= _bankSystem.GetTotalTransactions()},
                //TODO: new TableSet { Category="Assets", RecordCounts= _bankSystem.GetTotalAssets()}
            };

            CurrentGridData = stats;
        }
        private void ExecuteLoadTransactions(object o)
        {
            Transactions = new ObservableCollection<Transaction>(_bankSystem.GetTransactions());
            CurrentGridData = Transactions;
        }

        private void ExecuteLoadHomeView(object o)
        {
            CurrentView = new HomeView();
        }
    }
}
