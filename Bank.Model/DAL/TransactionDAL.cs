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
    public class TransactionDAL
    {
        private readonly string _connectionString;
        public TransactionDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        /*
         * Transaction Operations
         */
        public List<Transaction> GetTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();
            string query = "Select * from Transactions";

            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            using (MySqlCommand command = new MySqlCommand(query, conn))
            {
                conn.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int id = Convert.ToInt32(reader["Id"]);
                        int accountId = Convert.ToInt32(reader["AccountId"]);
                        TransactionType transactionType = (TransactionType)reader["TransactionType"];
                        decimal amount = Convert.ToDecimal(reader["Amount"]);
                        DateTime transactionDate = Convert.ToDateTime(reader["TransactionDate"]);

                        Transaction transaction = new Transaction { Id = id, AccountId = accountId, TransactionType = transactionType, Amount = amount, TransactionDate = transactionDate};
                        transactions.Add(transaction);
                    }
                }
            }
            return transactions;
        }
        public int InsertTransaction(Transaction transaction)
        {
            string query = "INSERT into Transactions (Id, AccountId, TransactionType, Amount, TransactionDate) " +
                "           values (@id, @accountid, @transactiontype, @amount, @transactiondate);" +
                "SELECT last_insert_id();";

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            using(MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@id", transaction.Id);
                cmd.Parameters.AddWithValue("@accountid", transaction.AccountId);
                cmd.Parameters.AddWithValue("@transactiontype", transaction.TransactionType);
                cmd.Parameters.AddWithValue("@amount", transaction.Amount);
                cmd.Parameters.AddWithValue("@transactiondate", transaction.TransactionDate);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
