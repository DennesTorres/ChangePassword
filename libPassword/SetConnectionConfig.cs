using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Configuration;


namespace libPassword
{
    public class SetConnectionConfig
    {
        public static void SetConnection(string ConnectionStringName,string ConnectionString)
        {
            Configuration config;
            if (HttpRuntime.AppDomainAppId != null)
            {
                HttpContext.Current.Trace.Warn("webconiguration");
                try
                {
                    config = WebConfigurationManager.OpenWebConfiguration("~");
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Deu erro no webconfiguration");
                }
            }
            else
            {
                HttpContext.Current.Trace.Warn("configuration");
                try
                {
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                }
                catch (Exception ex)
                {

                    throw new ApplicationException("Deu erro no configuration manager");
                }
            }
            HttpContext.Current.Trace.Warn("passou");
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            HttpContext.Current.Trace.Warn("leu");
            connectionStringsSection.ConnectionStrings[ConnectionStringName].ConnectionString = ConnectionString;
            HttpContext.Current.Trace.Warn("alterou");
            config.Save();
            HttpContext.Current.Trace.Warn("salvou");
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
