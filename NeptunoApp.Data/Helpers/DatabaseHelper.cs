using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NeptunoApp.Data.Helpers
{
    public static class DatabaseHelper
    {
        private static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["NeptunoDB"].ConnectionString;

        public static async Task<DataSet> ExecuteQueryAsync(string queryOrSpName, SqlParameter[] parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryOrSpName, conn))
                {
                    cmd.CommandType = commandType; // <-- AQUÍ ESTÁ EL CAMBIO CLAVE
                    if (parameters != null) cmd.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        await Task.Run(() => adapter.Fill(ds));
                        return ds;
                    }
                }
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(string spName, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null) cmd.Parameters.AddRange(parameters);

                    await conn.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}