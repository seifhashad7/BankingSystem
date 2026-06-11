//using BankingSystem.Model.Contracts;
//using BankingSystem.Model.Domain;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity.Core;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BankingSystem.Data.Services
//{
//    public class BankProdService : IBankProdService
//    {
//        private readonly AppDbContext _appDbContext;
//        private readonly ILogger _logger;

//        public BankProdService(AppDbContext appDbContext, ILogger logger)
//        {
//            _appDbContext = appDbContext;
//            _logger = logger;
//        }

//        public CreditCard IssueCreditCard(int customerId, decimal cashLimit)
//        {
//            bool hasCreditCard = _appDbContext.BankServices.OfType<CreditCard>().Any(c => c.CustomerId == customerId);
//            var errorMsg = string.Empty;

//            if (hasCreditCard)
//            {
//                errorMsg = $"The Customer with ID {customerId} already has a credit card! limit is one per user!";
//                _logger.LogError(errorMsg);
//                throw new InvalidOperationException(errorMsg);
//            }

//            if ((cashLimit < 50000) || (cashLimit > 250000))
//            {
//                errorMsg = "Cash limit must be between 50k and 250k.";
//                _logger.LogError(errorMsg);
//                throw new ArgumentException(errorMsg);
//            }

//            try
//            {
//                var newCreditCard = new CreditCard
//                {
//                    CustomerId = customerId,
//                    CashLimit = cashLimit,
//                    IssueDate = DateTime.Now,
//                };

//                _appDbContext.BankServices.Add(newCreditCard);
//                _appDbContext.SaveChanges();

//                _logger.LogInfo($"Credit card issued for CustomerID: {customerId}, Limit: {cashLimit}");

//                return newCreditCard;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Database write failure while saving new credit card.", ex);
//                return null;
//            }            
//        }
//        public Certificate IssueCertificate(int customerId, int period, decimal prinicipalAmount)
//        {
//            bool isCustomerExists = _appDbContext.Customers.Any(c => c.Id == customerId);
//            string errorMsg = string.Empty;

//            if(!isCustomerExists)
//            {
//                errorMsg = $"There's no customer with ID: {customerId}!";
//                _logger.LogError(errorMsg);
//                throw new ArgumentException(errorMsg);
//            }

//            if ((prinicipalAmount < 1000) || (prinicipalAmount % 1000 != 0))
//            {
//                errorMsg = "Prinipal amount should be min. of 1000 and it's multiple";
//                _logger.LogError(errorMsg);
//                throw new ArgumentException(errorMsg);
//            }

//            decimal interestRate;

//            switch (period)
//            {
//                case 1:
//                    interestRate = 0.10m;
//                    break;
//                case 3:
//                    interestRate = 0.15m;
//                    break;
//                case 5:
//                    interestRate = 0.20m;
//                    break;
//                default:
//                    errorMsg = "Certificate period should be 1, 3 or 5 years";
//                    _logger.LogError(errorMsg);
//                    throw new ArgumentException(errorMsg);
//            }

//            try
//            {
//                var newCertificate = new Certificate
//                {
//                    CustomerId = customerId,
//                    InterestRate = interestRate,
//                    PeriodInYears = period,
//                    PrincipalAmount = prinicipalAmount,
//                    IssueDate = DateTime.Now,
//                };

//                _appDbContext.BankServices.Add(newCertificate);
//                _appDbContext.SaveChanges();

//                _logger.LogInfo($"Certificate issued for CustomerId: {customerId}, PrincipalAmount: {prinicipalAmount}, Period: {period}");

//                return newCertificate;
//            }
//            catch(Exception ex)
//            {
//                _logger.LogError("Database Error issue new certificate", ex);
//                return null;
//            }
//        }

//        public CreditCard UpdateCreditCardLimit(int customerId, decimal newCreditCardLimit)
//        {
//            bool hasCreditCard = _appDbContext.BankServices.OfType<CreditCard>().Any(c => c.CustomerId == customerId);
//            var errorMsg = string.Empty;

//            if (!hasCreditCard)
//            {
//                errorMsg = $"Customer with ID: {customerId} is not found! abort!";
//                _logger.LogError(errorMsg);
//                throw new ArgumentException(errorMsg);
//            }

//            var creditCard = _appDbContext.CreditCards.FirstOrDefault(c => c.CustomerId == customerId);

//            if (creditCard == null)
//            {
//                errorMsg = $"No Credit card is found for customerId: {customerId}";
//                _logger.LogError(errorMsg);
//                throw new InvalidOperationException(errorMsg);
//            }

//            if ((newCreditCardLimit < 50000) || (newCreditCardLimit > 250000))
//            {
//                errorMsg = "Cash limit must be between 50k and 250k.";
//                _logger.LogError(errorMsg);
//                throw new ArgumentException(errorMsg);
//            }

//            try
//            {
//                creditCard.CashLimit = newCreditCardLimit;
//                _appDbContext.SaveChanges();

//                _logger.LogInfo($"Credit card's limit for CustomerId {customerId} is updated successfully!");
//                return creditCard;
//            }
//            catch(Exception ex)
//            {
//                _logger.LogError("Database write failure while updating the credit card.", ex);
//                return null;
//            }

//        }
//        public Certificate ModifyCertificate(int customerId, int certificateId, int newPeriod, decimal newPrice)
//        {
//            bool hasCertificate = _appDbContext.BankServices.OfType<Certificate>().Any(c => c.CustomerId == customerId);
//            string errorMsg = string.Empty;

//            if (!hasCertificate)
//            {
//                errorMsg = $"Customer with {customerId} doesn't have certificates!";
//                _logger.LogError(errorMsg);
//                throw new ArgumentException(errorMsg);
//            }

//            var certificate = _appDbContext.Certificates.FirstOrDefault(c => c.CustomerId == customerId && c.Id == certificateId);

//            if (certificate == null)
//            {
//                errorMsg = $"Customer with CustomerId: {customerId} doesn't have a certificate with CertificateId: {certificateId}";
//                _logger.LogError(errorMsg);
//                throw new ArgumentException(errorMsg);
//            }

//            if ((newPeriod % 2 != 0) && (newPeriod <= 5))
//            {
//                decimal newInterestRate;

//                switch (newPeriod)
//                {
//                    case 1:
//                        newInterestRate = 0.10m;
//                        break;
//                    case 3:
//                        newInterestRate = 0.15m;
//                        break;
//                    case 5:
//                        newInterestRate = 0.20m;
//                        break;
//                    default:
//                        throw new ArgumentException("Certificate period should be 1, 3 or 5 years");
//                }
//                certificate.PeriodInYears = newPeriod;
//                certificate.InterestRate = newInterestRate;
//            }
//            else
//            {
//                errorMsg = "Certificate period should be 1, 3 or 5 years!";
//                _logger.LogError(errorMsg);
//                throw new ArgumentException(errorMsg);
//            }

//            if ((newPrice >= 1000) && (newPrice % 1000 == 0))
//            {
//                certificate.PrincipalAmount = newPrice;
//            }
//            else
//            {
//                errorMsg = "Certificate price should be min of 1000 and its multiple";
//                _logger.LogError(errorMsg);
//                throw new ArgumentException(errorMsg);
//            }

//            try
//            {
//                _appDbContext.SaveChanges();
//                _logger.LogInfo($"Certificate with CertificateId: {certificateId}, CustomerId: {customerId} is modified successfully!");
//                return certificate;
//            }
//            catch(Exception ex)
//            {
//                _logger.LogError("Database Error modifing certificate data!", ex);
//                return null;
//            }
//        }
//        public void deleteCertificate(int customerId, int certificateId)
//        {
//            try
//            {
//                bool hasCertificate = _appDbContext.BankServices.OfType<Certificate>().Any(c => c.CustomerId == customerId);

//                if (!hasCertificate)
//                {
//                    throw new Exception($"Customer with CustomerId: {customerId} doesn't have certificated to be deleted!");
//                }

//                var certificate = _appDbContext.Certificates.FirstOrDefault(c => c.CustomerId == customerId && c.Id == certificateId);

//                if(certificate == null)
//                {
//                    throw new NullReferenceException($"Certificate with ID: {certificateId} is not found for customer with CustomerId: {customerId}");
//                }

//                _appDbContext.Certificates.Remove(certificate);
//                _appDbContext.SaveChanges();

//                _logger.LogInfo($"Certificate with certificate ID: {certificateId} for customer with CustomerId {customerId} is deleted!");
//            }
//            catch(Exception ex)
//            {
//                _logger.LogError("Error deleting certificate!", ex);
//            }
//        }
//    }
//}
