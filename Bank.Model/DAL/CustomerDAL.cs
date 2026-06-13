using Bank.Model.Entities;
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
    public class CustomerDAL
    {
        private readonly string _connectionString;
        public CustomerDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        /*
         * Customer Operations
         */
        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();
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
        public int GetTotalCustomer()
        {
            string query = "SELECT COUNT(*) FROM Customers";
            int counts;

            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                object obj = cmd.ExecuteScalar();
                counts = Convert.ToInt32(obj);
            }

            return counts;
        }
        public int InsertCustomer(Customer customer)
        {
            string query = @"INSERT into customers (Name, Age, Gender, Address, PhoneNumber, NationalId) " +
                "           values (@name, @age, @gender, @address, @phonenumber, @nationalid);" +
                "SELECT last_insert_id();";

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@name", customer.Name);
                cmd.Parameters.AddWithValue("@age", customer.Age);
                cmd.Parameters.AddWithValue("@gender", customer.Gender);
                cmd.Parameters.AddWithValue("@address", customer.Address);
                cmd.Parameters.AddWithValue("@phonenumber", customer.PhoneNumber);
                cmd.Parameters.AddWithValue("@nationalid", customer.NationalId);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        public bool UpdateCustomer(Customer customer)
        {
            string query = "UPDATE Customers SET Name=@name, Age=@age, Gender=@age, Address=@address, PhoneNumber=@phonenumber, NationalId=@nationalid WHERE Id=@id";

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                
                cmd.Parameters.AddWithValue("@id", customer.Id);
                cmd.Parameters.AddWithValue("@name", customer.Name);
                cmd.Parameters.AddWithValue("@age", customer.Age);
                cmd.Parameters.AddWithValue("@address", customer.Address);
                cmd.Parameters.AddWithValue("@phonenumber", customer.PhoneNumber);
                cmd.Parameters.AddWithValue("@nationalid", customer.NationalId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public void DeleteCustomer(int customerId)
        {
            string query = "DELETE FROM Customers where Id=@id";

            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@id", customerId);
                cmd.ExecuteNonQuery();
            }      
        }
    }
}
