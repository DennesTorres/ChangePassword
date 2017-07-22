using System;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;




namespace libPassword
{

    public class PasswordStrategy : DbExecutionStrategy
    {
        const int PasswordExpired = 18487;
        const int MustChangePassword = 18488;

        #region Sync

        private static Object lck = new object();
        private static bool ChangingPassword = false;

        #endregion

        #region Properties

        [ThreadStatic]
        private static string _ConnectionStringName;
        public static string ConnectionStringName
        {
            get
            {
                return _ConnectionStringName;
            }
            set
            {
                _ConnectionStringName = value;
            }
        }

        [ThreadStatic]
        private static DbContext _Context;
        public static DbContext Context
        {
            get
            {
                return _Context;
            }
            set
            {
                _Context = value;
            }
        }


        private string ConStr
        {
            get
            {
                    return ConfigurationManager.ConnectionStrings
                        [ConnectionStringName]?.ConnectionString;
            }
        }
        #endregion

        private string ExtractSQLString(string ConnectionString)
        {
            var efbuilder = new EntityConnectionStringBuilder(ConnectionString);
            return efbuilder.ProviderConnectionString;
        }

        private string BuildEntityString(string ProviderString)
        {
            var efBuilder= new EntityConnectionStringBuilder(ConStr);
            efBuilder.ProviderConnectionString = ProviderString;
            return efBuilder.ConnectionString;
        }

        protected override bool ShouldRetryOn(Exception exception)
        {
            if (!(exception is SqlException))
                return false;

            var number = ((SqlException)exception).Number;

            if ((number != PasswordExpired) && (number != MustChangePassword))
                return false;

            if (ChangingPassword)
                return false;

            try
            {

                ChangingPassword = true; /* Starting the password change */

                if (string.IsNullOrEmpty(ConStr))
                    return false;

                var provStr = ExtractSQLString(ConStr);

                var newPassword = System.Guid.NewGuid();

                provStr =
                    SQLChangePassword.ChangePwd(provStr, newPassword.ToString());

                SetConnectionConfig.SetConnection(ConnectionStringName, BuildEntityString(provStr));

                Context.Database.Connection.ConnectionString = provStr;
            }
            finally
            {
                ChangingPassword = false; /* Finishing the password change */
            }
            return true;
        }

    }
}
