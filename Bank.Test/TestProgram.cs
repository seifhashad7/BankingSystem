using Bank.Model.Managers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Model.Entities;

namespace Bank.Test
{
    public class TestProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==== Banking System ====");

            Console.WriteLine("== Establishing a connection with db Started.. ==");
            string connectionString = ConfigurationManager.ConnectionStrings["BankDbConnection"].ConnectionString;
            BankSystem.Initialize(connectionString);
            BankSystem bankSystem = BankSystem.Instance;
            Console.WriteLine("== Connection established successfully.. ==");

            Console.WriteLine("## Registering or Fetching a new Customer ##");
            Customer registeredCustomer = bankSystem.GetAllCustomers().FirstOrDefault(c => c.NationalId == "3091985923456");
            if (registeredCustomer == null)
            {
                registeredCustomer = bankSystem.RegisterCustomer("Dr. Shereef Hamdy", 45, Gender.Male, "Mohandseen Giza", "3091985923456", "0100100234876");
            }
            Console.WriteLine($"==> Customer with Name {registeredCustomer.Name} and Id {registeredCustomer.Id} is registered successfully!");

            Console.WriteLine("## Creating saving and salart accounts for him ##");
            Account salaryAccount = bankSystem.OpenAccount(registeredCustomer.Id, AccountType.Salary, 100000, DateTime.Now);
            Account savingAccount = bankSystem.OpenAccount(registeredCustomer.Id, AccountType.Saving, 10000, DateTime.Now);
            Console.WriteLine($"==> Successfully opened saving account with ID: {savingAccount.Id} and salary account with ID: {salaryAccount.Id} for {registeredCustomer.Name}");

            Console.WriteLine("## Performing some Transactions ##");
            bankSystem.Deposit(salaryAccount.Id, 1000);
            bankSystem.Withdraw(salaryAccount.Id, 200);

            Console.WriteLine($"==> Salary Account with ID: {salaryAccount.Id} Balance should be 100800, Actual balance is {salaryAccount.Balance}");

            bankSystem.Deposit(savingAccount.Id, 1000);
            bankSystem.Withdraw(savingAccount.Id, 300);

            Console.WriteLine($"==> Saving Account with ID: {savingAccount.Id} Balance should be 10700, Actual balance is {savingAccount.Balance}");

            Console.WriteLine("## Issuing some services for him##");

            var existingCard = bankSystem.GetCreditCards().FirstOrDefault(c => c.CustomerId == registeredCustomer.Id);
            if (existingCard == null)
            {
                existingCard = bankSystem.IssueCreditCard(registeredCustomer.Id, 50000);
            }
            
            Certificate certificate;
            try 
            {
                certificate = bankSystem.IssueCertificate(registeredCustomer.Id, 3, 200);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"==> Successfully caught expected validation error: {ex.Message}");
            }
            
            Console.WriteLine($"==> Should issue a credit card successfully with ID: {existingCard.Id} and throws argument exception for Issue certificate because principal amount < 1000, Check log file.");


            Console.WriteLine("## Confirming that only credit card creation is succeed and certificate fails ##");
            IReadOnlyList<BankService> list = bankSystem.GetBankServicesPerCustomer(registeredCustomer.Id);

            foreach(var l in list)
            {
                Console.WriteLine($"Service Id: {l.Id}");
            }

            Console.WriteLine("## Reporting ##");
            var accounts = bankSystem.GetAllAccounts();
            foreach(var acc in accounts)
            {
                Console.WriteLine($"AccountId: {acc.Id} for CustomerId: {acc.CustomerId} is created at: {acc.CreatedAt}");
            }

            Console.WriteLine("================================================");
            var services = bankSystem.GetServices();
            foreach (var ser in services)
            {
                Console.WriteLine($"ServiceId: {ser.Id} for CustomerId: {ser.CustomerId} is issued in {ser.IssueDate}");
            }

            Console.WriteLine("================================================");
        }
    }
}
