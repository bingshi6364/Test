using StackExchange.Redis;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using PLMSide.Common;
using Microsoft.Extensions.Configuration;

namespace PLMSide.Data
{
    public class DataBaseConfig
    {
        #region SqlServer链接配置


        public string te = "";
        // private static string DefaultSqlConnectionString= @"Data Source=DEVSHRSQLBJI1A,1433;Initial Catalog=PLM_DEV;User ID=PLM_DEV_OWNER;Password=Plmdev@dmin456;";
        //private static string DefaultSqlConnectionString = @"Data Source=TSTSHRSQLBJI1A;Initial Catalog=PLM_QA;User ID=PLM_QA_OWNER;Password=p!m@qa#23ql;";
        public static string DefaultSqlConnectionString = Appsettings.app(new string[] { "SqlServer", "ConnectionString" });
        private static string DefaultRedisString = "localhost, abortConnect=false";
        private static ConnectionMultiplexer redis;


        public DataBaseConfig(IConfiguration configuration)
        {
        }

        public static IDbConnection GetSqlConnection(string sqlConnectionString = null)
        {
            if (string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                sqlConnectionString = DefaultSqlConnectionString;
            }
            IDbConnection conn = new SqlConnection(sqlConnectionString);
            conn.Open();
            return conn;
        }

        #endregion

        #region Redis链接配置

        private static ConnectionMultiplexer GetRedis(string redisString = null)
        {
            if (string.IsNullOrWhiteSpace(redisString))
            {
                redisString = DefaultRedisString;
            }
            if (redis == null || redis.IsConnected)
            {
                redis = ConnectionMultiplexer.Connect(redisString);
            }
            return redis;
        }

        #endregion
    }
}
