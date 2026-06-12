//using BankingSystem.Commands;
//using BankingSystem.Data;
//using BankingSystem.Data.Services;
//using BankingSystem.Model.Contracts;
//using BankingSystem.Model.Domain;
//using BankingSystem.Model.Logging;
//using BankingSystem.Views;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Text;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Input;

//namespace BankingSystem.ViewModel
//{
//    public class CustomerDashboardViewModel : ViewModelBase
//    {
//        private int _id;
//        private readonly IReportingService _reportingService;
//        private UserControl? _currentView;
//        private IEnumerable _currentGridData;
//        private ObservableCollection<Transaction> _transactions;
//        private ObservableCollection<Account> _accounts;
//        private ObservableCollection<BankService> _bankServices;

//        public ICommand LoadTransactionsCommand { get; }
//        public ICommand LoadAccountsCommand { get; }
//        public ICommand LoadServicesCommand { get; }
//        public ICommand LoadTotalBalanceCommand { get; }
//        public ICommand LoadTotalTransactionsCommand { get; }
//        public ICommand LoadHomeViewCommand { get; }


//        public int Id
//        {
//            get => _id;
//            set => SetProperty(ref _id, value);
//        }

//        public IEnumerable CurrentGridData
//        {
//            get => _currentGridData;
//            set => SetProperty(ref _currentGridData, value);
//        }

//        public ObservableCollection<Transaction> Transactions
//        {
//            get => _transactions;
//            set => SetProperty(ref _transactions, value);
//        }

//        public ObservableCollection<Account> Accounts
//        {
//            get => _accounts;
//            set => SetProperty(ref _accounts, value);
//        }
//        public ObservableCollection<BankService> BankServices
//        {
//            get => _bankServices;
//            set => SetProperty(ref _bankServices, value);
//        }

//        public UserControl CurrentView
//        {
//            get => _currentView;
//            set => SetProperty(ref _currentView, value);
//        }

//        public class TableSet
//        {
//            public string? Category { get; set; }
//            public decimal RecordCounts { get; set; }
//        }

//        public CustomerDashboardViewModel(IReportingService reportingService)
//        {
//            _reportingService = reportingService;
//            _transactions = new ObservableCollection<Transaction>();
//            _accounts = new ObservableCollection<Account>();
//            LoadTransactionsCommand = new RelayCommand(ExecuteLoadTransactions);
//            LoadAccountsCommand = new RelayCommand(ExecuteLoadAccounts);
//            LoadServicesCommand = new RelayCommand(ExecuteLoadServices);
//            LoadTotalBalanceCommand = new RelayCommand(ExecuteLoadTotalBalance);
//            LoadTotalTransactionsCommand = new RelayCommand(ExecuteLoadTotalTransactions);
//            LoadHomeViewCommand = new RelayCommand(ExecuteLoadHomeView);
//        }

//        private void ExecuteLoadTransactions(object o)
//        {
//            Transactions = new ObservableCollection<Transaction>(_reportingService.GetCustomerTransactions(Id));
//            CurrentGridData = Transactions;
//        }

//        private void ExecuteLoadAccounts(object o)
//        {
//            Accounts = new ObservableCollection<Account>(_reportingService.GetCustomerAccounts(Id));
//            CurrentGridData = Accounts;
//        }
//        private void ExecuteLoadServices(object o)
//        {
//            BankServices = new ObservableCollection<BankService>(_reportingService.GetCustomerServices(Id));
//            CurrentGridData = BankServices;
//        }
//        private void ExecuteLoadTotalBalance(object o)
//        {
//            var TotalBalance = new List<TableSet>
//            {
//                new TableSet { Category="Total Balance", RecordCounts= _reportingService.GetCustomerTotalBalance(Id)}
//            };

//            CurrentGridData = TotalBalance;
//        }

//        private void ExecuteLoadTotalTransactions(object o)
//        {
//            var TotalTransactions = new List<TableSet>
//            {
//                new TableSet { Category="Total Transactions", RecordCounts= _reportingService.GetTotalTransactionPerCustomer(Id)}
//            };

//            CurrentGridData = TotalTransactions;
//        }

//        private void ExecuteLoadHomeView(object o)
//        {
//            CurrentView = new HomeView();
//        }
//    }
//}
