using Bank.Model.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Model.DAL
{
    public class AccountDAL
    {
        private readonly string _connectionString;

        public AccountDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        /*
         * Account Operations        
        */
        public List<Account> GetAccounts()
        {
            string query = "SELECT * from Accounts";
            List<Account> accounts = new List<Account>();

            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using(MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Account account;

                        AccountType accountType = (AccountType)Convert.ToInt32(reader["AccountType"]);
                        if(accountType == AccountType.Salary)
                        {
                            account = new SalaryAccount();
                        }
                        else
                        {
                            account = new SavingAccount();
                        }

                        account.Id = Convert.ToInt32(reader["Id"]);
                        account.Balance = Convert.ToDecimal(reader["Balance"]);
                        account.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                        account.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                        account.AccountType = accountType;

                        accounts.Add(account);
                    }
                }
            }    
            
            return accounts;
        }

        public int OpenAccount(Account account)
        {
            string query = @"INSERT INTO accounts (CustomerId, AccountType, Balance, CreatedAt) VALUES (@customerid, @accounttype, @balance, @createdat); " +
                "SELECT last_insert_id();";

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@customerid", account.CustomerId);
                cmd.Parameters.AddWithValue("@accounttype", account.AccountType);
                cmd.Parameters.AddWithValue("@balance", account.Balance);
                cmd.Parameters.AddWithValue("@createdat", account.CreatedAt);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void DeleteAccount(int accountId)
        {
            string query = "DELETE FROM Accounts where Id=@id";

            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@id", accountId);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateBalance(int accountId, decimal newBalance)
        {
            string query = "UPDATE Accounts SET Balance=@balance where Id=@id";

            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@id", accountId);
                cmd.Parameters.AddWithValue("@balance", newBalance);

                cmd.ExecuteNonQuery();
            }
        }

        //public decimal GetBalance(int accountId)
        //{
        //    string query = "SELECT Balance FROM Accounts where Id=@id";

        //    using(MySqlConnection conn = new MySqlConnection(_connectionString))
        //    using(MySqlCommand cmd = new MySqlCommand(query, conn))
        //    {
        //        conn.Open();

        //        return Convert.ToDecimal(cmd.ExecuteScalar());
        //    }
        //}

        //public List<Account> GetAccountsByType(AccountType accountType)
        //{
        //    string query = "SELECT * from Accounts where AccountType=@type";
        //    List<Account> list = new List<Account>();

        //    using(MySqlConnection conn = new MySqlConnection(_connectionString))
        //    using(MySqlCommand cmd = new MySqlCommand(query, conn))
        //    {
        //        conn.Open();
        //        Account account;

        //        cmd.Parameters.AddWithValue("@type", accountType);

        //        MySqlDataReader reader = cmd.ExecuteReader();
        //        while(reader.Read())
        //        {
        //            if(accountType == AccountType.Salary)
        //            {
        //                account = new SalaryAccount();
        //            }
        //            else
        //            {
        //                account = new SavingAccount();
        //            }

        //            account.Id = Convert.ToInt32(reader["Id"]);
        //            account.Balance = Convert.ToDecimal(reader["Balance"]);
        //            account.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
        //            account.CustomerId = Convert.ToInt32(reader["CustomerId"]);
        //            account.AccountType = accountType;

        //            list.Add(account);
        //        }
        //    }
        //    return list;
        //}

        //public List<Account> GetAccountsPerCustomer(int customerId)
        //{
        //    string query = "SELECT * from Accounts where CustomerId=@customerid";
        //    List<Account> list = new List<Account>();

        //    using (MySqlConnection conn = new MySqlConnection(_connectionString))
        //    using (MySqlCommand cmd = new MySqlCommand(query, conn))
        //    {
        //        conn.Open();
        //        Account account;

        //        cmd.Parameters.AddWithValue("@customerid", customerId);

        //        MySqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            AccountType accountType = (AccountType)Convert.ToInt32(reader["AccountType"]);
        //            if (accountType == AccountType.Salary)
        //            {
        //                account = new SalaryAccount();
        //            }
        //            else
        //            {
        //                account = new SavingAccount();
        //            }

        //            account.Id = Convert.ToInt32(reader["Id"]);
        //            account.Balance = Convert.ToDecimal(reader["Balance"]);
        //            account.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
        //            account.CustomerId = Convert.ToInt32(reader["CustomerId"]);
        //            account.AccountType = accountType;

        //            list.Add(account);
        //        }
        //    }
        //    return list;
        //}

        //public int GetTotalAccountsByType(AccountType accountType)
        //{
        //    string query = "SELECT COUNT(*) FROM Accounts where AccountType=@accounttype";

        //    using (MySqlConnection conn = new MySqlConnection(_connectionString))
        //    using (MySqlCommand cmd = new MySqlCommand(query, conn))
        //    {
        //        conn.Open();

        //        cmd.Parameters.AddWithValue("@accounttype", accountType);
        //        return Convert.ToInt32(cmd.ExecuteScalar());
        //    }
        //}

        //public int GetTotalAccounts()
        //{
        //    string query = "SELECT COUNT(*) FROM Accounts";

        //    using(MySqlConnection conn = new MySqlConnection(_connectionString))
        //    using(MySqlCommand cmd = new MySqlCommand(query, conn))
        //    {
        //        conn.Open();

        //        return Convert.ToInt32(cmd.ExecuteScalar());
        //    }
        //}
    }
}
