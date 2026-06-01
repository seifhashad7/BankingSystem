using BankingSystem.Model.Contracts;
using BankingSystem.Model.CrossCutting;
using BankingSystem.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Data.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger _logger;

        public CustomerService(AppDbContext appDbContext, ILogger logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public Customer RegisterCustomer(string name, int age, Gender gender, string address, string nationalId, string phoneNumber)
        {
            Customer newCustomer = new Customer
            {
                Name = name,
                Age = age,
                Gender = gender,
                Address = address,
                NationalId = nationalId,
                PhoneNumber = phoneNumber
            };

            _appDbContext.Customers.Add(newCustomer);
            _appDbContext.SaveChanges();

            return newCustomer;
        }

        public void EditCustomerInfo(int customerId, string newName, int? newAge, Gender? newGender, string newAddress, string newNationalId, string newPhoneNumber)
        {
            try
            {
                var customer = _appDbContext.Customers.Find(customerId);

                if (customer == null)
                {
                    throw new Exception($"Customer with CustomerId: {customerId} is not found!");
                }

                if(newName != null) customer.Name = newName;
                if(newAge.HasValue) customer.Age = newAge.Value;
                if(newGender.HasValue) customer.Gender = newGender.Value;
                if(newAddress != null) customer.Address = newAddress;
                if(newNationalId != null) customer.NationalId = newNationalId;
                if(newPhoneNumber != null) customer.PhoneNumber = newPhoneNumber;

                _appDbContext.SaveChanges();
                _logger.LogInfo($"Customer with ID: {customerId} info is updated!");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error updating Customer info for CustomerID: {customerId}!", ex);
            }
        }
    }
}
