using BankingSystem.Commands;
using BankingSystem.Data;
using BankingSystem.Model.Contracts;
using BankingSystem.Model.CrossCutting;
using BankingSystem.Model.Domain;
using BankingSystem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace BankingSystem.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly IBankProdService _bankProdService;
        private readonly IReportingService _reportingService;
        private UserControl? _currentView;

        public ICommand LoadDashboardViewCommand { get; }
        public ICommand LoadCustomerDashboardViewCommand { get; }
        public ICommand LoadCreateAccountViewCommand { get; }
        public ICommand LoadEditAccountViewCommand { get; }
        public ICommand LoadCloseAccountViewCommand { get; }
        public ICommand LoadAccountOperationViewCommand { get; }
        public ICommand LoadCreditCardViewCommand { get; }
        public ICommand LoadCertificateViewCommand { get; }

        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public MainViewModel(AppDbContext appDbContext, ICustomerService customerService, IAccountService accountService, IBankProdService bankProdService, IReportingService reportingService)
        {
            _appDbContext = appDbContext;
            _accountService = accountService;
            _bankProdService = bankProdService;
            _reportingService = reportingService;
            LoadDashboardViewCommand = new RelayCommand(o => CurrentView = new DashboardView(reportingService));
            LoadCustomerDashboardViewCommand = new RelayCommand(o => CurrentView = new CustomerDashboardView(reportingService));
            LoadCreateAccountViewCommand = new RelayCommand(o => CurrentView = new CreateAccountView(customerService, accountService));
            LoadEditAccountViewCommand = new RelayCommand(o => CurrentView = new EditCustomerView(customerService, accountService));
            LoadCloseAccountViewCommand = new RelayCommand(o => CurrentView = new CloseAccountView(customerService, accountService));
            LoadAccountOperationViewCommand = new RelayCommand(o => CurrentView = new AccountOperationView(accountService));
            LoadCreditCardViewCommand = new RelayCommand(o => CurrentView = new CreditCardView(bankProdService));
            LoadCertificateViewCommand = new RelayCommand(o => CurrentView = new CertificateView(bankProdService));
            CurrentView = new HomeView();
        }
    }
}
