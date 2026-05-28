using BankingSystem.Commands;
using BankingSystem.Data;
using BankingSystem.Data.Services;
using BankingSystem.Model.Contracts;
using BankingSystem.Model.Domain;
using BankingSystem.Model.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BankingSystem.ViewModel
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IReportingService _reportingService;
        private readonly ILogger _logger;

        public ICommand LoadCustomersDataCommand { get; }

        public ObservableCollection<Customer> _customers;

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => SetProperty(ref _customers, value);
        }

        public DashboardViewModel(IReportingService reportingService)
        {
            _reportingService = reportingService;
            _logger = new Logger();
            _customers = new ObservableCollection<Customer>();
            //Customers = new ObservableCollection<Customer>();
            LoadCustomersDataCommand = new RelayCommand(ExecuteLoadCustomersData); 
        }

        private void ExecuteLoadCustomersData(object o)
        {
            //Customers.Clear();

            //foreach(Customer c in _reportingService.GetCustomers())
            //{
            //    Customers.Add(c);
            //}
            Customers = new ObservableCollection<Customer>(_reportingService.GetCustomers());
        }
    }
}
