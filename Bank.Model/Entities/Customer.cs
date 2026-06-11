using Bank.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Model.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }

        public void RegisterCustomer(string name, int age, Gender gender, string address, string nationalId, string phoneNumber)
        {

        }
        public void EditCustomerInfo(int customerId, string newName, int? newAge, Gender? newGender, string newAddress, string newNationalId, string newPhoneNumber)
        {

        }
        public override string ToString()
        {
            return Name;
        }
    }
}
