using System;
using System.Data;
using System.Data.SqlClient;

namespace iSqlCmd
{
    public class LoginDetails : IDisposable
    {
        public LoginDetails()
        {
            TrustedConnection = true;
            DatabaseName = "master";
            Server = @".\sqlexpress";
            Connection = new Lazy<SqlConnection>(BuildConnection);
        }

        private SqlConnection BuildConnection()
        {
            var sqlConnection = new SqlConnection(BuildConnectionString(this));
            sqlConnection.Open();
            return sqlConnection;
        }

        public Lazy<SqlConnection> Connection { get; private set; }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                if(!string.IsNullOrWhiteSpace(userName))
                {
                    TrustedConnection = false;
                }
            }
        }

        public string Password { get; set; }
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public bool TrustedConnection { get; set; }

        private static string BuildConnectionString(LoginDetails loginDetails)
        {
            if (loginDetails.TrustedConnection)
            {
                return string.Format("Server={0};Database={1};Trusted_Connection=true;", loginDetails.Server, loginDetails.DatabaseName);
            }
            return string.Format("Server={0}; Database{1}; User={2}; Password={3};",
                loginDetails.Server, loginDetails.DatabaseName, loginDetails.UserName, loginDetails.Password);
        }

        public void Dispose()
        {
            if (!Connection.IsValueCreated) return;
            if(Connection.Value.State == ConnectionState.Open) Connection.Value.Close();
            Connection.Value.Dispose();
        }
    }
}