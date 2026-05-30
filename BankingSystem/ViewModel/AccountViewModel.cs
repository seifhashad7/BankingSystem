using BankingSystem.Commands;
using BankingSystem.Data.Services;
using BankingSystem.Model.Contracts;
using BankingSystem.Model.CrossCutting;
using BankingSystem.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BankingSystem.ViewModel
{
    public class AccountViewModel : ViewModelBase
    {
        private ICustomerService _customerService;
        private IAccountService _accountService;

        private string? _name;
        private int _age;
        private string? _nationalId;
        private Gender _gender;
        private string? _address;
        private string? _phoneNumber;
        private AccountType _accountType;
        private int _initialBalance;
        private UserControl? _currentView;
        private bool _isMaleSelected;
        private bool _isFemaleSelected;
        private bool _isSalaryAccSelected;
        private bool _isSavingAccSelected;

        public ICommand SubmitCommand { get; }
        public ICommand BackCommand { get; }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }
        public string NationalId
        {
            get => _name;
            set => SetProperty(ref _nationalId, value);
        }
        public Gender Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }
        public AccountType AccountType
        {
            get => _accountType;
            set => SetProperty(ref _accountType, value);
        }
        public int InitialBalance
        {
            get => _initialBalance;
            set => SetProperty(ref _initialBalance, value);
        }
        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }
        public bool IsMaleSelected
        {
            get => _isMaleSelected;
            set => SetProperty(ref _isMaleSelected, value);
        }
        public bool IsFemaleSelected
        {
            get => _isFemaleSelected;
            set => SetProperty(ref _isFemaleSelected, value);
        }
        public bool IsSalaryAccSelected
        {
            get => _isSalaryAccSelected;
            set => SetProperty(ref _isSalaryAccSelected, value);
        }
        public bool isSavingAccSelected
        {
            get => _isSavingAccSelected;
            set => SetProperty(ref _isSavingAccSelected, value);
        }

        public AccountViewModel(ICustomerService customerService, IAccountService accountService)
        {
            _customerService = customerService;
            _accountService = accountService;
            SubmitCommand = new RelayCommand(ExecuteSubmitButton);
            BackCommand = new RelayCommand(ExecuteBackButton);
            IsMaleSelected = true;
            IsSalaryAccSelected = true;
        }

        private void ExecuteSubmitButton(object o)
        {
            if (IsMaleSelected) Gender = Gender.Male;
            else Gender = Gender.Female;

            if (IsSalaryAccSelected) AccountType = AccountType.Salary;
            else AccountType = AccountType.Saving;

            var newCustomer = _customerService.RegisterCustomer(Name, Age, Gender, Address, NationalId, PhoneNumber);

            _accountService.OpenAccount(newCustomer.Id, AccountType, InitialBalance);

            MessageBox.Show("Account Created successfully!");
        }
        private void ExecuteBackButton(object o)
        {
            CurrentView = new HomeView();
        }
    }
}
