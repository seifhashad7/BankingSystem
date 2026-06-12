using BankingSystem.Commands;
using BankingSystem.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bank.Model.Entities;
using Bank.Model.Managers;

namespace BankingSystem.ViewModel
{
    public class AccountViewModel : ViewModelBase
    {
        private readonly BankSystem _bankSystem;

        private int _id;
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

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
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
            get => _nationalId;
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

        public AccountViewModel(BankSystem bankSystem)
        {
            _bankSystem = bankSystem;
            SubmitCommand = new RelayCommand(ExecuteSubmitButton);
            BackCommand = new RelayCommand(ExecuteBackButton);
            IsMaleSelected = true;
            IsSalaryAccSelected = true;
        }

        private void ExecuteSubmitButton(object o)
        {
            string activeView = o as string;
            if (activeView == "CreateMode")
            {
                if (IsMaleSelected) Gender = Gender.Male;
                else Gender = Gender.Female;

                if (IsSalaryAccSelected) AccountType = AccountType.Salary;
                else AccountType = AccountType.Saving;

                var newCustomer = _bankSystem.RegisterCustomer(Name, Age, Gender, Address, NationalId, PhoneNumber);

                //TODO
                //_accountService.OpenAccount(newCustomer.Id, AccountType, InitialBalance);

                MessageBox.Show("Account Created successfully!");
            }
            else if (activeView == "EditMode")
            {
                _bankSystem.UpdateCustomer(Id, Name, Age, Gender, Address, NationalId, PhoneNumber);
                MessageBox.Show("Customer info edited successfully");
            }
            else if (activeView == "CloseMode")
            {
                _bankSystem.CloseCustomer(Id);
                MessageBox.Show("Account Closed successfully");
            }
        }
        private void ExecuteBackButton(object o)
        {
            CurrentView = new HomeView();
        }
    }
}
