using Bank.Model.Entities;
using Bank.Model.Enums;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Model.DAL
{
    public class BankRepository
    {
        private readonly string _connectionString;
        public BankRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /*
         * Customer Operations
         */
        public ObservableCollection<Customer> GetCustomers()
        {
            ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
            string query = "Select * from Customers";

            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            using (MySqlCommand command = new MySqlCommand(query, conn))
            {
                conn.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string name = reader["Name"].ToString();
                        int age = (int)reader["Age"];
                        Gender gender = (Gender)reader["Gender"];
                        string address = reader["Address"].ToString();
                        string phoneNumber = reader["PhoneNumber"].ToString();
                        string nationalId = reader["NationalId"].ToString();

                        Customer customer = new Customer { Id = id, Name = name, Age = age, Address = address, PhoneNumber = phoneNumber, NationalId = nationalId };
                        customers.Add(customer);
                    }
                }
            }
            return customers;
        }
    }
}
