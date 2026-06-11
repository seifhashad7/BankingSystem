//using BankingSystem.Commands;
//using BankingSystem.Model.Contracts;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Windows;
//using System.Windows.Input;

//namespace BankingSystem.ViewModel
//{
//    public class AccountOperationViewModel : ViewModelBase
//    {
//        private IAccountService _accountService;
//        private int _id;
//        private decimal _amount;
//        private decimal _balance;

//        public ICommand DepoistCommand { get; }
//        public ICommand WithdrawCommand { get; }
//        public ICommand GetBalanceCommand { get; }

//        public int Id
//        {
//            get => _id;
//            set => SetProperty(ref _id, value);
//        }

//        public decimal Amount
//        {
//            get => _amount;
//            set => SetProperty(ref _amount, value);
//        }

//        public decimal Balance
//        {
//            get => _balance;
//            set => SetProperty(ref _balance, value);
//        }

//        public AccountOperationViewModel(IAccountService accountService)
//        {
//            _accountService = accountService;
//            DepoistCommand = new RelayCommand(ExecuteDepoistCommand);
//            WithdrawCommand = new RelayCommand(ExecuteWithdrawCommand);
//            GetBalanceCommand = new RelayCommand(ExecuteGetBalanceCommand);
//        }

//        private void ExecuteDepoistCommand(object o)
//        {
//            _accountService.Depoist(Id, Amount);
//            MessageBox.Show("Depoist operation done successfully!");
//        }
//        private void ExecuteWithdrawCommand(object o)
//        {
//            _accountService.Withdraw(Id, Amount);
//            MessageBox.Show("Withdraw operation done successfully!");
//        }
//        private void ExecuteGetBalanceCommand(object o)
//        {
//            Balance = _accountService.GetBalance(Id);
//        }
//    }
//}
