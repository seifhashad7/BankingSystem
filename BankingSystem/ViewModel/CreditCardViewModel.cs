using BankingSystem.Commands;
using BankingSystem.Model.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BankingSystem.ViewModel
{
    public class CreditCardViewModel : ViewModelBase
    {
        private IBankProdService _bankProdService;
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

        public CreditCardViewModel(IBankProdService bankProdService)
        {
            _bankProdService = bankProdService;
            IssueCreditCardCommand = new RelayCommand(ExecuteIssueCreditCard);
            UpdateCardLimitCommand = new RelayCommand(ExecuteUpdateCashLimit);
        }

        private void ExecuteIssueCreditCard(object o)
        {
            var CreditCardIssued = _bankProdService.IssueCreditCard(Id, CashLimit);
            if (CreditCardIssued != null) MessageBox.Show("New Credit is issued successfully");
            else MessageBox.Show("Issue Credit card operation is failed");
        }

        private void ExecuteUpdateCashLimit(object o)
        {
            _bankProdService.UpdateCreditCardLimit(Id, CashLimit);
            MessageBox.Show("Cash limit is updated successfully");
        }


    }
}
