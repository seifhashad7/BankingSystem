using Bank.Model.Managers;
using BankingSystem.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BankingSystem.ViewModel
{
    public class CertificateViewModel : ViewModelBase
    {
        private BankSystem _bankSystem;
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

        public CertificateViewModel(BankSystem bankSystem)
        {
            _bankSystem = bankSystem;
            IssueCertificateCommand = new RelayCommand(ExecuteIssueCertificate);
            ModifyCertificateCommand = new RelayCommand(ExecuteModifyCertificate);
            DeleteCertificateCommand = new RelayCommand(ExecuteDeleteCertificate);
        }

        private void ExecuteIssueCertificate(object o)
        {
            var IssuedCertificate = _bankSystem.IssueCertificate(CustomerId, Period, PrincipalAmount);
            if (IssuedCertificate != null) MessageBox.Show("Certificate is issued successfully!");
        }

        private void ExecuteModifyCertificate(object o)
        {
            _bankSystem.ModifyCertificate(CustomerId, CertificateId, Period, PrincipalAmount);
            MessageBox.Show("Certificate Data is modified successfully!");
        }

        private void ExecuteDeleteCertificate(object o)
        {
            _bankSystem.deleteCertificate(CustomerId, CertificateId);
            MessageBox.Show("Certificate deletion is done successfully!");
        }
    }
}
