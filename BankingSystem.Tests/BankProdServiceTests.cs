//using BankingSystem.Data;
//using BankingSystem.Data.Services;
//using BankingSystem.Model.Contracts;
//using BankingSystem.Model.CrossCutting;
//using BankingSystem.Model.Domain;
//using BankingSystem.Model.Logging;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq.Expressions;
//using System.Security.Principal;
//using System.Text;

////CreditCard IssueCreditCard(int customerId, decimal cashLimit);
////Certificate IssueCertificate(int customerId, int period, decimal principalAmount);
////void UpdateCreditCardLimit(int customerId, decimal newCreditCardLimit);
////void ModifyCertificate(int customerId, int certificateId, int newPeriod, decimal newPrice);
////void deleteCertificate(int customerId, int certificateId);

//namespace BankingSystem.Tests
//{
//    public class BankProdServiceTests
//    {
//        private Mock<AppDbContext> _mockContext;
//        private Mock<ILogger> _mockLogger;
//        private BankProdService _service;

//        //Setup
//        public BankProdServiceTests()
//        {
//            _mockContext = new Mock<AppDbContext>();
//            _mockLogger = new Mock<ILogger>();
//            _service = new BankProdService(_mockContext.Object, _mockLogger.Object);
//        }

//        // Helper method to turn a standard List into an EF6 Mock DbSet
//        private Mock<DbSet<T>> CreateMockDbSet<T>(List<T> sourceList) where T : class
//        {
//            var fakeQueryableTable = sourceList.AsQueryable();
//            var mockSet = new Mock<DbSet<T>>();

//            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(fakeQueryableTable.Provider);
//            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(fakeQueryableTable.Expression);
//            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(fakeQueryableTable.ElementType);
//            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(fakeQueryableTable.GetEnumerator());

//            return mockSet;
//        }

//        //CreditCards Unit tests
//        [Fact]
//        public void IssueCreditCard_WithValidLimit_ShouldSucceed()
//        {
//            //Arrange
//            List<BankService> fakeBankServices = new List<BankService>() {};
//            Mock<DbSet<BankService>> mockDbSet = CreateMockDbSet(fakeBankServices);

//            _mockContext.Setup(c => c.BankServices).Returns(mockDbSet.Object);

//            //Act
//            var creditCard = _service.IssueCreditCard(1, 100000);

//            //Assert
//            Assert.Equal(100000, creditCard.CashLimit);
//            mockDbSet.Verify(m => m.Add(It.IsAny<CreditCard>()), Times.Once);
//            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [Fact]
//        public void IssueCreditCard_WithInvalidLimit_ShouldThrow()
//        {
//            //Arrange
//            List<BankService> fakeBankingServices = new List<BankService>() { };
//            Mock<DbSet<BankService>>  mockDbSet = CreateMockDbSet(fakeBankingServices);

//            _mockContext.Setup(c => c.BankServices).Returns(mockDbSet.Object);

//            //Act + Assert
//            var exception = Assert.Throws<ArgumentException>(() => _service.IssueCreditCard(1, 5000m));

//            Assert.Contains("Cash limit must be between 50k and 250k.", exception.Message);
//        }

//        [Fact]
//        public void UpdateCreditCardLimit_WithValidNewLimit_ShouldSucceed()
//        {
//            //Arrange
//            int customerId = 5;
//            var baseCreditCard = new CreditCard { CustomerId = customerId, CashLimit = 60000, IssueDate = DateTime.Now};

//            var fakeBankServices = new List<BankService>() { baseCreditCard };
//            var fakeCreditCards = new List<CreditCard>() { baseCreditCard };

//            _mockContext.Setup(c => c.BankServices).Returns(CreateMockDbSet(fakeBankServices).Object);
//            _mockContext.Setup(c => c.CreditCards).Returns(CreateMockDbSet(fakeCreditCards).Object);

//            //Act
//            var updatedCreditCard = _service.UpdateCreditCardLimit(5, 70000);

//            //Assert
//            Assert.Equal(70000, updatedCreditCard.CashLimit);
//            _mockContext.Verify(m => m.SaveChanges(), Times.Once);

//            _mockLogger.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("updated successfully"))), Times.Once);
//        }

//        [Fact]
//        public void UpdateCreditCardLimit_WithInvalidNewLimit_ShouldThrow()
//        {
//            //Arrange
//            int customerId = 3;
//            var baseCreditCard = new CreditCard { CustomerId = customerId, CashLimit = 60000}; 
            
//            var fakeBankServices = new List<BankService>() {baseCreditCard};
//            var fakeCreditCards = new List<CreditCard>() {baseCreditCard};
            
//            _mockContext.Setup(c => c.BankServices).Returns(CreateMockDbSet(fakeBankServices).Object);
//            _mockContext.Setup(c => c.CreditCards).Returns(CreateMockDbSet(fakeCreditCards).Object);

//            //Assert + Act
//            var exception = Assert.Throws<ArgumentException>(() => _service.UpdateCreditCardLimit(3, 400000));
//            Assert.Contains(exception.Message, "Cash limit must be between 50k and 250k.");
//        }

//        [Fact]
//        public void UpdateCreditCardLimit_WithNonExistingCustomer_ShouldThrow()
//        {
//            //Arrange
//            var fakeBankServices = new List<BankService>() {  };

//            _mockContext.Setup(c => c.BankServices).Returns(CreateMockDbSet(fakeBankServices).Object);

//            //Assert + Act
//            var exception = Assert.Throws<ArgumentException>(() => _service.UpdateCreditCardLimit(3, 400000));
//            Assert.Contains("is not found! abort!", exception.Message);
//        }

//        //Certificates Unit tests
//        [Fact]
//        public void IssueCertificate_WithValidArgs_ShouldSucceed()
//        {
//            // Arrange
//            int customerId = 9;
//            var baseCustomer = new Customer
//            {
//                Id = customerId,
//                Name = "Seifovic",
//                Address = "Russia",
//                Age = 30,
//                Gender = Gender.Male,
//                PhoneNumber = "01134598234",
//                NationalId = "5469234986"
//            };

//            //Create fake tables for Customers and BankServices
//            var fakeCustomers = new List<Customer> { baseCustomer };
//            var fakeBankServices = new List<BankService>(); 

//            //Create mocked DbSets for both tables
//            var mockCustomerSet = CreateMockDbSet(fakeCustomers);
//            var mockBankServicesSet = CreateMockDbSet(fakeBankServices);

//            //Setup DbSets on mocked DbContext
//            _mockContext.Setup(m => m.Customers).Returns(mockCustomerSet.Object);
//            _mockContext.Setup(m => m.BankServices).Returns(mockBankServicesSet.Object);

//            // Act
//            Certificate issuedCertificate = _service.IssueCertificate(customerId, 3, 10000m);

//            // Assert
//            Assert.Equal(10000m, issuedCertificate.PrincipalAmount);
//            Assert.Equal(3, issuedCertificate.PeriodInYears);
//            Assert.Equal(0.15m, issuedCertificate.InterestRate);

//            mockBankServicesSet.Verify(m => m.Add(It.IsAny<Certificate>()), Times.Once);
//            _mockContext.Verify(m => m.SaveChanges(), Times.Once);

//            _mockLogger.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("Certificate issued for CustomerId:"))), Times.Once);
//        }

//        [Fact]
//        public void IssueCertificate_WithNonExistingCustomer_ShouldThrow()
//        {
//            //Arrange
//            int customerId = 4;
//            Customer baseCustomer = new Customer
//            {
//                Id = customerId,
//                Name = "Soha",
//                Gender = Gender.Female,
//                Address = "Cairo",
//                Age = 45,
//                NationalId = "12567983",
//                PhoneNumber = "01010435987"
//            };

//            List<BankService> fakeBankServices = new List<BankService>() { };
//            List<Customer> fakeCustomers = new List<Customer>() { baseCustomer };

//            //Create Dbset for this table
//            Mock<DbSet<BankService>> mockBankServicesSet = CreateMockDbSet(fakeBankServices);
//            Mock<DbSet<Customer>> mockCustomersSet = CreateMockDbSet(fakeCustomers);

//            //Setup it on mock DbContext
//            _mockContext.Setup(m => m.BankServices).Returns(mockBankServicesSet.Object);
//            _mockContext.Setup(m => m.Customers).Returns(mockCustomersSet.Object);

//            //Assert + Act
//            var exception = Assert.Throws<ArgumentException>(() => _service.IssueCertificate(1, 5, 1000000));
//            Assert.Contains("There's no customer with ID: 1!", exception.Message);
//            _mockLogger.Verify(l => l.LogError(It.Is<string>(s => s.Contains("There's no customer"))), Times.Once);
//        }

//        [Fact]
//        public void IssueCertificate_WithInvalidPrincipalAmount_ShouldThrow()
//        {
//            //Arrange
//            int customerId = 4;
//            Customer baseCustomer = new Customer
//            {
//                Id = customerId,
//                Name = "Salma",
//                Gender = Gender.Female,
//                Address = "Auxor",
//                Age = 45,
//                NationalId = "12567983",
//                PhoneNumber = "01010785987"
//            };

//            //Create fake tables
//            List<Customer> fakeCustomers = new List<Customer>() { baseCustomer };
//            List<BankService> fakeBankServices = new List<BankService>() { };

//            //Create mock DbSet for them
//            Mock<DbSet<BankService>> mockBankingServiceDbSet = CreateMockDbSet(fakeBankServices);
//            Mock<DbSet<Customer>> mockCustomerDbSet = CreateMockDbSet(fakeCustomers);

//            //Setup them on MockContext
//            _mockContext.Setup(m => m.Customers).Returns(mockCustomerDbSet.Object);
//            _mockContext.Setup(m => m.BankServices).Returns(mockBankingServiceDbSet.Object);

//            //Assert + Act
//            Exception exception = Assert.Throws<ArgumentException>(() => _service.IssueCertificate(customerId, 1, 300));
//            Assert.Contains("Prinipal amount should be min. of 1000 and it's multiple", exception.Message);
//            _mockLogger.Verify(l => l.LogError(It.Is<string>(s => s.Contains("Prinipal amount should be min. of 1000 and it's multiple"))), Times.Once);
//        }

//        [Fact]
//        public void ModifyCertificate_WithValidArgs_ShouldSucceed()
//        {
//            //Arrange
//            int customerId = 4;
//            Certificate baseCertificate = new Certificate { CustomerId = customerId, Id = 2, PeriodInYears = 10, PrincipalAmount = 1000 };

//            //Create fake tables
//            List<Certificate> fakeCertificates = new List<Certificate>() { baseCertificate };
//            List<BankService> fakeBankServices = new List<BankService>() { baseCertificate };

//            //Create DbSet for both tables
//            Mock<DbSet<Certificate>> mockCertificateSet = CreateMockDbSet(fakeCertificates);
//            Mock<DbSet<BankService>> mockBankingServiceSet = CreateMockDbSet(fakeBankServices);

//            //Setup them on MockContext
//            _mockContext.Setup(m => m.Certificates).Returns(mockCertificateSet.Object);
//            _mockContext.Setup(m => m.BankServices).Returns(mockBankingServiceSet.Object);

//            //Act
//            Certificate modifiedCertificate  = _service.ModifyCertificate(customerId: 4, certificateId: 2, newPeriod: 5, newPrice: 100000);

//            //Assert
//            Assert.Equal(100000, modifiedCertificate.PrincipalAmount);
//            Assert.Equal(5, modifiedCertificate.PeriodInYears);
//            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
//            _mockLogger.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("is modified successfully!"))), Times.Once);
//        }

//        [Fact]
//        public void ModifyCertificate_WithCustomerDoesNotHaveCertificate_ShouldThrow()
//        {
//            //Arrange
//            var fakeBankServices = new List<BankService>();

//            Mock<DbSet<BankService>> mockBankSet = CreateMockDbSet(fakeBankServices);

//            _mockContext.Setup(m => m.BankServices).Returns(mockBankSet.Object);

//            //Assert + Act
//            Exception exception = Assert.Throws<ArgumentException>(() => _service.ModifyCertificate(customerId: 1, 0, 3, 1000));
//            Assert.Contains("Customer with 1 doesn't have certificates!", exception.Message);
//            _mockLogger.Verify(l => l.LogError(It.Is<string>(s => s.Contains("doesn't have certificates!"))), Times.Once);
//        }

//        [Fact]
//        public void ModifyCertificate_WithNotFoundCertificate_ShouldThrow()
//        {
//            //Arrange
//            int customerId = 3;
//            Customer baseCustomer = new Customer
//            {
//                Id = customerId,
//                Name = "Rayan",
//                Gender = Gender.Male,
//                Address = "Aswan",
//                Age = 19,
//                NationalId = "12567983",
//                PhoneNumber = "0124976345"
//            };

//            Certificate anotherCertificate = new Certificate { CustomerId = customerId, Id = 5, PeriodInYears = 3, PrincipalAmount = 10000 };

//            List<Customer> fakeCustomers = new List<Customer>() { baseCustomer };
//            List<BankService> fakeBankServices = new List<BankService>() { anotherCertificate };
//            List<Certificate> fakeCertificates = new List<Certificate>() { anotherCertificate };

//            Mock<DbSet<Customer>> mockCustomerSet = CreateMockDbSet(fakeCustomers);
//            Mock<DbSet<BankService>> mockBankServiceSet = CreateMockDbSet(fakeBankServices);
//            Mock<DbSet<Certificate>> mockCertificateSet = CreateMockDbSet(fakeCertificates);

//            _mockContext.Setup(m => m.Customers).Returns(mockCustomerSet.Object);
//            _mockContext.Setup(m => m.BankServices).Returns(mockBankServiceSet.Object);
//            _mockContext.Setup(m => m.Certificates).Returns(mockCertificateSet.Object);

//            //Assert + Act
//            Exception exception = Assert.Throws<ArgumentException>(() => _service.ModifyCertificate(customerId: 3, certificateId: 7, newPeriod: 3, newPrice: 1000));
//            Assert.Contains("Customer with CustomerId: 3 doesn't have a certificate with CertificateId: 7", exception.Message);
//            _mockLogger.Verify(l => l.LogError(It.Is<string>(s => s.Contains("doesn't have a certificate with CertificateId"))), Times.Once);
//        }

//        [Fact]
//        public void ModifyCertificate_WithInvalidNewPrice_ShouldThrow()
//        {
//            //Arrange
//            int customerId = 3;
//            Customer baseCustomer = new Customer
//            {
//                Id = customerId,
//                Name = "Rayan",
//                Gender = Gender.Male,
//                Address = "Aswan",
//                Age = 19,
//                NationalId = "12567983",
//                PhoneNumber = "0124976345"
//            };

//            Certificate certificate = new Certificate { CustomerId = customerId, Id = 5, PeriodInYears = 3, PrincipalAmount = 10000 };

//            List<BankService> fakeBankServices = new List<BankService>() { certificate };
//            List<Certificate> fakeCertificates = new List<Certificate>() { certificate };

//            Mock<DbSet<BankService>> mockBankServiceSet = CreateMockDbSet(fakeBankServices);
//            Mock<DbSet<Certificate>> mockCertificateSet = CreateMockDbSet(fakeCertificates);

//            _mockContext.Setup(m => m.BankServices).Returns(mockBankServiceSet.Object);
//            _mockContext.Setup(m => m.Certificates).Returns(mockCertificateSet.Object);

//            //Assert + Act
//            Exception exception = Assert.Throws<ArgumentException>(() => _service.ModifyCertificate(customerId: 3, certificateId: 5, newPeriod: 3, newPrice: 500));
//            Assert.Contains("Certificate price should be min of 1000 and its multiple", exception.Message);
//            _mockLogger.Verify(l => l.LogError(It.Is<string>(s => s.Contains("Certificate price should be min of 1000 and its multiple"))), Times.Once);
//        }

//        [Fact]
//        public void ModifyCertificate_WithInvalidNewPeriod_ShouldThrow()
//        {
//            //Arrange
//            int customerId = 3;
//            Customer baseCustomer = new Customer
//            {
//                Id = customerId,
//                Name = "Rayan",
//                Gender = Gender.Male,
//                Address = "Aswan",
//                Age = 19,
//                NationalId = "12567983",
//                PhoneNumber = "0124976345"
//            };

//            Certificate certificate = new Certificate { CustomerId = customerId, Id = 5, PeriodInYears = 3, PrincipalAmount = 10000 };

//            List<BankService> fakeBankServices = new List<BankService>() { certificate };
//            List<Certificate> fakeCertificates = new List<Certificate>() { certificate };

//            Mock<DbSet<BankService>> mockBankServiceSet = CreateMockDbSet(fakeBankServices);
//            Mock<DbSet<Certificate>> mockCertificateSet = CreateMockDbSet(fakeCertificates);

//            _mockContext.Setup(m => m.BankServices).Returns(mockBankServiceSet.Object);
//            _mockContext.Setup(m => m.Certificates).Returns(mockCertificateSet.Object);

//            //Assert + Act
//            Exception exception = Assert.Throws<ArgumentException>(() => _service.ModifyCertificate(customerId: 3, certificateId: 5, newPeriod: 4, newPrice: 500));
//            Assert.Contains("Certificate period should be 1, 3 or 5 years", exception.Message);
//            _mockLogger.Verify(l => l.LogError(It.Is<string>(s => s.Contains("Certificate period should be 1, 3 or 5 years"))), Times.Once);
//        }
//    }
//}
