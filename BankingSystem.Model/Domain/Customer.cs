using BankingSystem.Model.CrossCutting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Model.Domain
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

        public override string ToString()
        {
            return Name;
        }
    }
}
