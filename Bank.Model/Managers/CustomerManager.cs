using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Model.Entities;
using Bank.Model.DAL;
using Bank.Model.Logging;

namespace Bank.Model.Managers
{
    public class CustomerManager
    {
        private readonly CustomerDAL _dal;
        private readonly Logger _logger;
        private List<Customer> _customerCache;

        public CustomerManager(string connectionString, Logger logger)
        {
            _dal = new CustomerDAL(connectionString);
            _logger = logger;
            _customerCache = new List<Customer>();
        }

        public void Initialize()
        {
            _logger.LogInfo("Initializing Customer cache from Database.");
            _customerCache = _dal.GetCustomers();
        }

        public IReadOnlyList<Customer> GetCustomers() => _customerCache.AsReadOnly();
        public int GetTotalCustomers() => _customerCache.Count();

        public Customer RegisterCustomer(Customer customer)
        {
            if(_customerCache.Any(c => c.NationalId == customer.NationalId))
            {
                string errorMsg = $"Registeration failed! Customer with NationalId: {customer.NationalId} already exists!";
                _logger.LogError(errorMsg);
                throw new InvalidOperationException(errorMsg);
            }

            try
            {
                customer.Id = _dal.InsertCustomer(customer);
                _customerCache.Add(customer);
                _logger.LogInfo($"Registered New Customer with Name:{customer.Name} Id: {customer.Id}");
                return customer;
            }
            catch(Exception ex)
            {
                _logger.LogError("Database write failure while saving new customer.", ex);
                throw;
            }
        }

        public Customer EditCustomerInfo(Customer customer)
        {
            Customer cachedCustomer = _customerCache.FirstOrDefault(c => c.Id == customer.Id);
            if (cachedCustomer == null)
            {
                string errorMsg = $"Edit failed. Customer {customer.Id} does not exist in the cache.";
                _logger.LogError(errorMsg);
                throw new InvalidOperationException(errorMsg);
            }

            try
            {
                bool isSuccess = _dal.UpdateCustomer(customer);

                if (!isSuccess)
                {
                    string errorMsg = $"Edit failed. Customer {customer.Id} does not exist in the database.";
                    _logger.LogError(errorMsg);
                    throw new InvalidOperationException(errorMsg);
                }

                cachedCustomer.Name = customer.Name;
                cachedCustomer.Age = customer.Age;
                cachedCustomer.Gender = customer.Gender;
                cachedCustomer.Address = customer.Address;
                cachedCustomer.PhoneNumber = customer.PhoneNumber;
                cachedCustomer.NationalId = customer.NationalId;

                _logger.LogInfo($"Successfully edited data for customer with id: {customer.Id}");
                return cachedCustomer;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Database error in editing customer's data with CustomerId: {customer.Id}", ex);
                throw;
            }
        }
        
        public void DeleteCustomer(int customerId)
        {
            if(!_customerCache.Any(c => c.Id == customerId))
            {
                string errorMsg = $"Deletion failed. Customer {customerId} does not exist in the cache.";
                _logger.LogError(errorMsg);
                throw new ArgumentException(errorMsg);
            }

            try
            {
                _dal.DeleteCustomer(customerId);
                _customerCache.Remove(_customerCache.FirstOrDefault(c => c.Id == customerId));
                _logger.LogInfo($"Successfully deleted customer with CustomerId {customerId}");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to delete customer with Id: {customerId}", ex);
            }
        }
    }
}
