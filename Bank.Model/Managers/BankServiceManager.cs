using Bank.Model.DAL;
using Bank.Model.Entities;
using Bank.Model.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Model.Managers
{
    public class BankServiceManager
    {
        private readonly Logger _logger;
        private readonly BankServiceDAL _dal;
        private List<BankService> _servicesCache;

        public BankServiceManager(string connectionString, Logger logger)
        {
            _logger = logger;
            _dal = new BankServiceDAL(connectionString);
            _servicesCache = new List<BankService>();
        }

        public void Initialize()
        {
            _logger.LogInfo("Initializing BankService cache from Database.");
            _servicesCache = _dal.GetServices();
        }

        public IReadOnlyList<BankService> GetServices() => _servicesCache.AsReadOnly();
        public IReadOnlyList<CreditCard> GetCreditCards() => _servicesCache.OfType<CreditCard>().ToList().AsReadOnly();
        public IReadOnlyList<Certificate> GetCertificates() => _servicesCache.OfType<Certificate>().ToList().AsReadOnly();

        public decimal GetTotalServiceAssets()
        {
            decimal totalCashLimit = _servicesCache.OfType<CreditCard>().Sum(c => c.CashLimit);
            decimal totalPrincipalAmount = _servicesCache.OfType<Certificate>().Sum(c => c.PrincipalAmount);
            return totalCashLimit + totalPrincipalAmount;
        }

        public IReadOnlyList<BankService> GetBankServicesPerCustomer(int customerId) => _servicesCache.Where(s => s.CustomerId == customerId).ToList().AsReadOnly();
        public IReadOnlyList<BankService> GetCertificatesPerCustomer(int customerId) => _servicesCache.OfType<Certificate>().Where(s => s.CustomerId == customerId).ToList().AsReadOnly();
        public int  GetTotalServices() => _servicesCache.Count();
        public int  GetTotalCreditCards() => _servicesCache.OfType<CreditCard>().Count();
        public int  GetTotalCertificates() => _servicesCache.OfType<Certificate>().Count();
        public CreditCard IssueCreditCard(int customerId, decimal cashLimit)
        {
            if (_servicesCache.OfType<CreditCard>().Any(c => c.CustomerId == customerId))
            {
                string errorMsg = $"Customer {customerId} already has a Credit Card. Only one card is allowed.";
                _logger.LogError(errorMsg);
                throw new InvalidOperationException(errorMsg);
            }

            if(cashLimit < 50000 || cashLimit > 250000)
            {
                string errorMsg = "Cash limit is out of range, min CashLimit is 50000 L.E., Max CashLimit is 250000 L.E.";
                _logger.LogError(errorMsg);
                throw new ArgumentOutOfRangeException(errorMsg);
            }

            var cc = new CreditCard
            {
                CustomerId = customerId,
                IssueDate = DateTime.Now,
                CashLimit = cashLimit
                // PeriodInYears is set to 10 by default in constructor
            };

            try
            {
                int newId = _dal.InsertCreditCard(cc);
                cc.Id = newId;
                _servicesCache.Add(cc);
                _logger.LogInfo($"Successfully issued CreditCard for Customer {customerId} (Service ID: {newId})");
                return cc;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Database error in issuing CreditCard for Customer {customerId}", ex);
                throw;
            }
        }

        public Certificate IssueCertificate(int customerId, int period, decimal principalAmount)
        {
            if(principalAmount < 1000 || principalAmount % 1000 != 0)
            {
                string errorMsg = "Principal amount should be min. of 1000 L.E. and its multiple";
                _logger.LogError(errorMsg);
                throw new ArgumentOutOfRangeException(errorMsg);
            }

            if(period != 1 && period != 3 && period != 5)
            {
                string errorMsg = "Period should be 1 year, 3 years or 5 years";
                _logger.LogError(errorMsg);
                throw new ArgumentOutOfRangeException(errorMsg);
            }

            var cert = new Certificate
            {
                CustomerId = customerId,
                PeriodInYears = period,
                IssueDate = DateTime.Now,
                PrincipalAmount = principalAmount,
                InterestRate = CalculateInterestRate(period)
            };

            try
            {
                int newId = _dal.InsertCertificate(cert);
                cert.Id = newId;
                _servicesCache.Add(cert);
                _logger.LogInfo($"Successfully issued Certificate for Customer {customerId} (Service ID: {newId})");
                return cert;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Database error in issuing Certificate for Customer {customerId}", ex);
                throw;
            }
        }

        public CreditCard UpdateCreditCardLimit(int customerId, decimal newCreditCardLimit)
        {
            if(newCreditCardLimit < 50000 || newCreditCardLimit > 250000)
            {
                string errorMsg = "Cash limit is out of range, min CashLimit is 50000 L.E., Max CashLimit is 250000 L.E.";
                _logger.LogError(errorMsg);
                throw new ArgumentOutOfRangeException(errorMsg);
            }

            var cc = _servicesCache.OfType<CreditCard>().FirstOrDefault(c => c.CustomerId == customerId);
            if (cc == null)
            {
                string errorMsg = $"No CreditCard found for Customer Id: {customerId}";
                _logger.LogError(errorMsg);
                throw new ArgumentException(errorMsg);
            }

            try
            {
                cc.CashLimit = newCreditCardLimit;
                _dal.UpdateCreditCard(cc);
                _logger.LogInfo($"Successfully updated CreditCard limit for Customer {customerId}");
                return cc;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Database error updating CreditCard limit for Customer {customerId}", ex);
                throw;
            }
        }

        public Certificate ModifyCertificate(int customerId, int certificateId, int newPeriod, decimal newPrice)
        {
            if(newPrice < 1000 || newPrice % 1000 != 0)
            {
                string errorMsg = "Principal amount should be min. of 1000 L.E. and its multiple";
                _logger.LogError(errorMsg);
                throw new ArgumentOutOfRangeException(errorMsg);
            }

            if(newPeriod != 1 && newPeriod != 3 && newPeriod != 5)
            {
                string errorMsg = "Period should be 1 year, 3 years or 5 years";
                _logger.LogError(errorMsg);
                throw new ArgumentOutOfRangeException(errorMsg);
            }

            var cert = _servicesCache.OfType<Certificate>().FirstOrDefault(c => c.Id == certificateId && c.CustomerId == customerId);
            if (cert == null)
            {
                string errorMsg = $"No Certificate found with Id: {certificateId} for Customer Id: {customerId}";
                _logger.LogError(errorMsg);
                throw new ArgumentException(errorMsg);
            }

            decimal newRate = CalculateInterestRate(newPeriod);

            try
            {
                cert.PeriodInYears = newPeriod;
                cert.PrincipalAmount = newPrice;
                cert.InterestRate = newRate;
                _dal.UpdateCertificate(cert);
                
                _logger.LogInfo($"Successfully modified Certificate {certificateId} for Customer {customerId}");
                return cert;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Database error modifying Certificate {certificateId} for Customer {customerId}", ex);
                throw;
            }
        }

        public void deleteCertificate(int customerId, int certificateId)
        {
            var cert = _servicesCache.OfType<Certificate>().FirstOrDefault(c => c.Id == certificateId && c.CustomerId == customerId);
            if (cert == null)
            {
                string errorMsg = $"No Certificate found with Id: {certificateId} for Customer Id: {customerId}";
                _logger.LogError(errorMsg);
                throw new ArgumentException(errorMsg);
            }

            try
            {
                _dal.DeleteCertificate(certificateId);
                _servicesCache.Remove(cert);
                _logger.LogInfo($"Successfully deleted Certificate {certificateId} for Customer {customerId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Database error deleting Certificate {certificateId} for Customer {customerId}", ex);
                throw;
            }
        }

        private decimal CalculateInterestRate(int periodInYears)
        {
            // 10% for 1 year, 15% for 3 years and 20% for 5 years
            if (periodInYears >= 5) return 0.20m;
            if (periodInYears >= 3) return 0.15m;
            if (periodInYears >= 1) return 0.10m;
            return 0.0m;
        }
    }
}
