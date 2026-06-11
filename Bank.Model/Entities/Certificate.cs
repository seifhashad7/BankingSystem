using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Model.Entities
{
    public class Certificate : BankService
    {
        public decimal PrincipalAmount { get; set; } = 1000; //Multiples of 1000
        public decimal InterestRate { get; set; } //10% for 1 year, 15% for 3 years and 20% for 5 years

        public void UpdateTerms(int period, decimal amount)
        {

        }
        public void IssueCertificate(int customerId, int period, decimal principalAmount)
        {

        }
        public void ModifyCertificate(int customerId, int certificateId, int newPeriod, decimal newPrice)
        { 
        
        }
        public void deleteCertificate(int customerId, int certificateId)
        {

        }
    }
}
