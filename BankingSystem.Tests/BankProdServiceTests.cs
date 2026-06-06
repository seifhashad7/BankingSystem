using BankingSystem.Data;
using BankingSystem.Data.Services;
using BankingSystem.Model.Contracts;
using BankingSystem.Model.Domain;
using BankingSystem.Model.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

//CreditCard IssueCreditCard(int customerId, decimal cashLimit);
//Certificate IssueCertificate(int customerId, int period, decimal principalAmount);
//void UpdateCreditCardLimit(int customerId, decimal newCreditCardLimit);
//void ModifyCertificate(int customerId, int certificateId, int newPeriod, decimal newPrice);
//void deleteCertificate(int customerId, int certificateId);

namespace BankingSystem.Tests
{
    public class BankProdServiceTests
    {
        private Mock<AppDbContext> _mockContext;
        private Mock<ILogger> _mockLogger;
        private BankProdService _service;

        //Setup
        public BankProdServiceTests()
        {
            _mockContext = new Mock<AppDbContext>();
            _mockLogger = new Mock<ILogger>();
            _service = new BankProdService(_mockContext.Object, _mockLogger.Object);
        }

        // Helper method to turn a standard List into an EF6 Mock DbSet
        private Mock<DbSet<T>> CreateMockDbSet<T>(List<T> sourceList) where T : class
        {
            var fakeQueryableTable = sourceList.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(fakeQueryableTable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(fakeQueryableTable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(fakeQueryableTable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(fakeQueryableTable.GetEnumerator());

            return mockSet;
        }

        [Fact]
        public void IssueCreditCard_WithValidLimit_ShouldSucceed()
        {
            //Arrange
            List<BankService> fakeBankServices = new List<BankService>() {};
            Mock<DbSet<BankService>> mockDbSet = CreateMockDbSet(fakeBankServices);

            _mockContext.Setup(c => c.BankServices).Returns(mockDbSet.Object);

            //Act
            var creditCard = _service.IssueCreditCard(1, 100000);

            //Assert
            Assert.Equal(100000, creditCard.CashLimit);
            mockDbSet.Verify(m => m.Add(It.IsAny<CreditCard>()), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void IssueCreditCard_WithInvalidLimit_ShouldThrow()
        {
            //Arrange
            List<BankService> fakeBankingServices = new List<BankService>() { };
            Mock<DbSet<BankService>>  mockDbSet = CreateMockDbSet(fakeBankingServices);

            _mockContext.Setup(c => c.BankServices).Returns(mockDbSet.Object);

            //Act + Assert
            var exception = Assert.Throws<ArgumentException>(() => _service.IssueCreditCard(1, 5000m));

            Assert.Contains("Cash limit must be between 50k and 250k.", exception.Message);
        }

        [Fact]
        public void UpdateCreditCardLimit_WithValidNewLimit_ShouldSucceed()
        {
            //Arrange
            int customerId = 5;
            var baseCreditCard = new CreditCard { CustomerId = customerId, CashLimit = 60000, IssueDate = DateTime.Now};

            var fakeBankServices = new List<BankService>() { baseCreditCard };
            var fakeCreditCards = new List<CreditCard>() { baseCreditCard };

            _mockContext.Setup(c => c.BankServices).Returns(CreateMockDbSet(fakeBankServices).Object);
            _mockContext.Setup(c => c.CreditCards).Returns(CreateMockDbSet(fakeCreditCards).Object);

            //Act
            var updatedCreditCard = _service.UpdateCreditCardLimit(5, 70000);

            //Assert
            Assert.Equal(70000, updatedCreditCard.CashLimit);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);

            _mockLogger.Verify(l => l.LogInfo(It.Is<string>(s => s.Contains("updated successfully"))), Times.Once);
        }

        [Fact]
        public void UpdateCreditCardLimit_WithInvalidNewLimit_ShouldThrow()
        {
            //Arrange
            int customerId = 3;
            var baseCreditCard = new CreditCard { CustomerId = customerId, CashLimit = 60000}; 
            
            var fakeBankServices = new List<BankService>() {baseCreditCard};
            var fakeCreditCards = new List<CreditCard>() {baseCreditCard};
            
            _mockContext.Setup(c => c.BankServices).Returns(CreateMockDbSet(fakeBankServices).Object);
            _mockContext.Setup(c => c.CreditCards).Returns(CreateMockDbSet(fakeCreditCards).Object);

            //Assert + Act
            var exception = Assert.Throws<ArgumentException>(() => _service.UpdateCreditCardLimit(3, 400000));
            Assert.Contains(exception.Message, "Cash limit must be between 50k and 250k.");
        }

        [Fact]
        public void UpdateCreditCardLimit_WithNonExistingCustomer_ShouldThrow()
        {
            //Arrange
            var fakeBankServices = new List<BankService>() {  };

            _mockContext.Setup(c => c.BankServices).Returns(CreateMockDbSet(fakeBankServices).Object);

            //Assert + Act
            var exception = Assert.Throws<ArgumentException>(() => _service.UpdateCreditCardLimit(3, 400000));
            Assert.Contains("is not found! abort!", exception.Message);
        }
    }
}
