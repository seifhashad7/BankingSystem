using BankingSystem.Commands;
using BankingSystem.Model.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BankingSystem.ViewModel
{
    public class CertificateViewModel : ViewModelBase
    {
        private IBankProdService _bankProdService;
        private int _customerId;
        private int _certificateId;
        private int _period;
        private decimal _principalAMount;

        public ICommand IssueCertificateCommand { get; }
        public ICommand ModifyCertificateCommand { get; }
        public ICommand DeleteCertificateCommand { get; }
        public int CustomerId
        {
            get => _customerId;
            set => SetProperty(ref _customerId, value);
        }
        public int CertificateId
        {
            get => _certificateId;
            set => SetProperty(ref _certificateId, value);
        }

        public int Period
        {
            get => _period;
            set => SetProperty(ref _period, value);
        }

        public decimal PrincipalAmount
        {
            get => _principalAMount;
            set => SetProperty(ref _principalAMount, value);
        }

        public CertificateViewModel(IBankProdService bankProdService)
        {
            _bankProdService = bankProdService;
            IssueCertificateCommand = new RelayCommand(ExecuteIssueCertificate);
            ModifyCertificateCommand = new RelayCommand(ExecuteModifyCertificate);
            DeleteCertificateCommand = new RelayCommand(ExecuteDeleteCertificate);
        }

        private void ExecuteIssueCertificate(object o)
        {
            var IssuedCertificate = _bankProdService.IssueCertificate(CustomerId, Period, PrincipalAmount);
            if (IssuedCertificate != null) MessageBox.Show("Certificate is issued successfully!");
            else MessageBox.Show("Certificate Issue operation is failed!");
        }

        private void ExecuteModifyCertificate(object o)
        {
            _bankProdService.ModifyCertificate(CustomerId, CertificateId, Period, PrincipalAmount);
            MessageBox.Show("Certificate Data is modified successfully!");
        }

        private void ExecuteDeleteCertificate(object o)
        {
            _bankProdService.deleteCertificate(CustomerId, CertificateId);
            MessageBox.Show("Certificate deletion is done successfully!");
        }
    }
}
