using BankingSystem.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Model.Contracts
{
    public interface IBankProdService
    {
        CreditCard IssueCreditCard(int customerId, decimal cashLimit);
        Certificate IssueCertificate(int customerId, int period, decimal principalAmount);
        CreditCard UpdateCreditCardLimit(int customerId, decimal newCreditCardLimit);
        void ModifyCertificate(int customerId, int certificateId, int newPeriod, decimal newPrice);
        void deleteCertificate(int customerId, int certificateId);
    }
}
