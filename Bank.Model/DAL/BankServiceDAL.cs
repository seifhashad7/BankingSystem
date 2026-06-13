using Bank.Model.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Bank.Model.DAL
{
    public class BankServiceDAL
    {
        private readonly string _connectionString;

        public BankServiceDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<BankService> GetServices()
        {
            var list = new List<BankService>();
            using(MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                using(MySqlCommand cmd = new MySqlCommand("SELECT * FROM Certificates", conn))
                using(MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Certificate certificate = new Certificate
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CustomerId = Convert.ToInt32(reader["CustomerId"]),
                            PeriodInYears = Convert.ToInt32(reader["PeriodYears"]),
                            PrincipalAmount = Convert.ToDecimal(reader["PrincipalAmount"]),
                            InterestRate = Convert.ToDecimal(reader["InterestRate"]),
                            IssueDate = Convert.ToDateTime(reader["IssueDate"])
                        };

                        list.Add(certificate);
                    }
                }

                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM CreditCards", conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CreditCard creditCard = new CreditCard
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CustomerId = Convert.ToInt32(reader["CustomerId"]),
                            CashLimit = Convert.ToDecimal(reader["CashLimit"]),
                            IssueDate = Convert.ToDateTime(reader["IssueDate"])
                        };

                        list.Add(creditCard);
                    }
                }
            }
            return list;
        }

        public int InsertCertificate(Certificate certificate)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(@"INSERT INTO Certificates (CustomerId, PeriodYears, PrincipalAmount, InterestRate, IssueDate) 
                                                    VALUES (@customerid, @periodyears, @principalamount, @interestrate, @issuedate);
                                                    SELECT LAST_INSERT_ID();", conn))
                {
                    cmd.Parameters.AddWithValue("@customerid", certificate.CustomerId);
                    cmd.Parameters.AddWithValue("@periodyears", certificate.PeriodInYears);
                    cmd.Parameters.AddWithValue("@principalamount", certificate.PrincipalAmount);
                    cmd.Parameters.AddWithValue("@interestrate", certificate.InterestRate);
                    cmd.Parameters.AddWithValue("@issuedate", certificate.IssueDate);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void UpdateCertificate(Certificate certificate)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(@"UPDATE Certificates 
                                                    SET PeriodYears=@periodyears, PrincipalAmount=@principalamount, InterestRate=@interestrate 
                                                    WHERE Id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", certificate.Id);
                    cmd.Parameters.AddWithValue("@periodyears", certificate.PeriodInYears);
                    cmd.Parameters.AddWithValue("@principalamount", certificate.PrincipalAmount);
                    cmd.Parameters.AddWithValue("@interestrate", certificate.InterestRate);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCertificate(int certificateId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("DELETE FROM Certificates WHERE Id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", certificateId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int InsertCreditCard(CreditCard creditCard)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(@"INSERT INTO CreditCards (CustomerId, CashLimit, IssueDate) 
                                                    VALUES (@customerid, @cashlimit, @issuedate);
                                                    SELECT LAST_INSERT_ID();", conn))
                {
                    cmd.Parameters.AddWithValue("@customerid", creditCard.CustomerId);
                    cmd.Parameters.AddWithValue("@cashlimit", creditCard.CashLimit);
                    cmd.Parameters.AddWithValue("@issuedate", creditCard.IssueDate);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void UpdateCreditCard(CreditCard creditCard)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(@"UPDATE CreditCards 
                                                    SET CashLimit=@cachlimit 
                                                    WHERE Id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", creditCard.Id);
                    cmd.Parameters.AddWithValue("@cachlimit", creditCard.CashLimit);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
