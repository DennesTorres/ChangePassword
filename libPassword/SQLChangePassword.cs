using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace libPassword
{
    public class SQLChangePassword
    {
        public static string ChangePwd(string ConnectionString, string newPassword)
        {
            var builder = new SqlConnectionStringBuilder(ConnectionString);
            ServerConnection srvConn = new ServerConnection();

            srvConn.ServerInstance = builder.DataSource;
            srvConn.LoginSecure = false;
            srvConn.Login = builder.UserID;
            srvConn.Password = builder.Password;

            srvConn.ChangePassword(newPassword);

            builder.Password = newPassword.ToString();
            return builder.ConnectionString;
        }
    }
}
