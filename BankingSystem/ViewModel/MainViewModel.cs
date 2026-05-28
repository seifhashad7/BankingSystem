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
        private readonly IAccountService _accountService;
        private readonly IBankProdService _bankProdService;
        private readonly IReportingService _reportingService;
        private UserControl? _currentView;

        public ObservableCollection<Customer> Customers { get; set; }
        public ICommand LoadDashboardViewCommand { get; }
        public ICommand LoadRegisterViewCommand { get; }
        public ICommand LoadLoginViewCommand { get; }

        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public MainViewModel(AppDbContext appDbContext, IAccountService accountService, IBankProdService bankProdService, IReportingService reportingService)
        {
            _appDbContext = appDbContext;
            _accountService = accountService;
            _bankProdService = bankProdService;
            _reportingService = reportingService;
            Customers = new ObservableCollection<Customer>();
            LoadDashboardViewCommand = new RelayCommand(o => CurrentView = new DashboardView(reportingService));
            LoadRegisterViewCommand = new RelayCommand(o => CurrentView = new RegisterView());
            LoadLoginViewCommand = new RelayCommand(o => CurrentView = new LoginView());
        }
    }
}
