using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Model.DAL;

namespace Bank.Model.Entities
{
    public class BankSystem
    {
        static string connectionString = "server=localhost;database=BankSystemDB;user=deskUser;password=1234;";
        
        BankRepository repo = new BankRepository(connectionString);

        public ObservableCollection<Customer> GetCustomers()
        {
            return repo.GetCustomers();
        }
    }
}
