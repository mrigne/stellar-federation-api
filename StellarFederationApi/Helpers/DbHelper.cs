using Microsoft.Extensions.Configuration;
using StellarFederationApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace StellarFederationApi.Helpers
{
    public class DbHelper : IDbHelper
    {
        private readonly IConfiguration _configuration;
        private SqlConnection connection;
        public DbHelper(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public List<User> GetUsers()
        {
            var usersList = new List<User>();
            this.Connect();
            string query = "select Name, PasswordHash from Users";
            using (SqlCommand command = new SqlCommand(query, this.connection))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    usersList.Add(new User
                    {
                        Name = (string)reader["Name"],
                        PasswordHash = (string)reader["PasswordHash"],
                    });
                }
            }
            this.Disconnect();
            return usersList;
        }

        public List<Account> GetAccounts()
        {
            var accountsList = new List<Account>();
            this.Connect();
            string query = "select * from Accounts";
            using (SqlCommand command = new SqlCommand(query, this.connection))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    accountsList.Add(new Account
                    {
                        Federation = (string)reader["Federation"],
                        Address = (string)reader["Address"],
                        MemoType = (MemoType)Enum.Parse(typeof(MemoType), (string)(reader["MemoType"] ?? "None")),
                        Memo = (string)reader["Memo"]
                    });
                }
            }
            this.Disconnect();
            return accountsList;
        }

        public void CreateAccount(Account account)
        {
            this.Connect();
            SqlCommand cmd = new SqlCommand(@"insert Into Accounts (Federation,Address,MemoType,Memo)
                                            VALUES(@Federation,@Address,@MemoType,@Memo)", this.connection);
            cmd.Parameters.Add(new SqlParameter("@Federation", account.Federation));
            cmd.Parameters.Add(new SqlParameter("@Address", account.Address));
            cmd.Parameters.Add(new SqlParameter("@MemoType", account.MemoType.ToString()));
            cmd.Parameters.Add(new SqlParameter("@Memo", account.Memo == null ? DBNull.Value : account.Memo));
            cmd.ExecuteNonQuery();
            this.Disconnect();
        }

        public void UpdateAccount(Account account)
        {
            var existingAccounts = this.GetAccounts();
            if (existingAccounts.Exists(existingAccount => existingAccount.Federation == account.Federation))
            {
                this.Connect();
                SqlCommand cmd = new SqlCommand(@"update Accounts
                                                set Federation=@Federation, Address=@Address, MemoType=@MemoType, Memo=@Memo
                                                where Federation=@Federation)", this.connection);
                cmd.Parameters.Add(new SqlParameter("@Federation", account.Federation));
                cmd.Parameters.Add(new SqlParameter("@Address", account.Address));
                cmd.Parameters.Add(new SqlParameter("@MemoType", account.MemoType.ToString()));
                cmd.Parameters.Add(new SqlParameter("@Memo", account.Memo == null ? DBNull.Value : account.Memo));
                cmd.ExecuteNonQuery();
                this.Disconnect();
            }
        }

        public void DeleteAccount(string federation)
        {
            var existingAccounts = this.GetAccounts();
            if (existingAccounts.Exists(existingAccount => existingAccount.Federation == federation))
            {
                this.Connect();
                SqlCommand cmd = new SqlCommand(@"delete from Accounts
                                                where Federation=@Federation", this.connection);
                cmd.Parameters.Add(new SqlParameter("@Federation", federation));
                cmd.ExecuteNonQuery();
                this.Disconnect();
            }
        }

        private void Connect()
        {
            if (this.connection == null)
            {
                string connectionString = _configuration.GetConnectionString("StellarFederationApiDb");
                this.connection = new SqlConnection(connectionString);
            }
            if (this.connection.State != ConnectionState.Open)
            {
                this.connection.Open();
            }
        }

        private void Disconnect()
        {
            if (this.connection != null && this.connection.State == ConnectionState.Open)
            {
                this.connection.Close();
            }
        }
    }
}