using libPassword;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace libData
{
    public class PasswordConfiguration : DbConfiguration
    {
        public PasswordConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient",
                        () => new PasswordStrategy());



        }
    }
}
