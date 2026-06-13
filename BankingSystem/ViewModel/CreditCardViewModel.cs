using Bank.Model.Managers;
using BankingSystem.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BankingSystem.ViewModel
{
    public class CreditCardViewModel : ViewModelBase
    {
        private BankSystem _bankSystem;
        private int _id;
        private decimal _cashLimit;

        public ICommand IssueCreditCardCommand { get; }
        public ICommand UpdateCardLimitCommand { get; }
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public decimal CashLimit
        {
            get => _cashLimit;
            set => SetProperty(ref _cashLimit, value);
        }

        public CreditCardViewModel(BankSystem bankSystem)
        {
            _bankSystem= bankSystem;
            IssueCreditCardCommand = new RelayCommand(ExecuteIssueCreditCard);
            UpdateCardLimitCommand = new RelayCommand(ExecuteUpdateCashLimit);
        }

        private void ExecuteIssueCreditCard(object o)
        {
            var CreditCardIssued = _bankSystem.IssueCreditCard(Id, CashLimit);
            if (CreditCardIssued != null) MessageBox.Show("New Credit is issued successfully", "Operation success", MessageBoxButton.OK, MessageBoxImage.Information);   
        }

        private void ExecuteUpdateCashLimit(object o)
        {
            _bankSystem.UpdateCreditCardLimit(Id, CashLimit);
            MessageBox.Show("Cash limit is updated successfully");
        }
    }
}
