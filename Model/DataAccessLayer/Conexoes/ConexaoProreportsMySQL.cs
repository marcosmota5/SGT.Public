using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.Conexoes
{
    internal class ConexaoProreportsMySQL : IDisposable
    {
        public readonly MySqlConnection conexaoProreports;

        public ConexaoProreportsMySQL()
        {
            // Definição da string de conexão, considerando a execução em ambiente de teste ou produção
            string stringConexaoProreports;

#if DEBUG
            stringConexaoProreports = "server=localhost;user id=root;password=Test@12345;persistsecurityinfo=True;database=db_cloud;port=3306;";
#else
            stringConexaoProreports = "server=localhost;user id=root;password=Test@12345;persistsecurityinfo=True;database=db_cloud;port=3306;";
#endif

            conexaoProreports = new MySqlConnection(stringConexaoProreports);
        }

        public async Task AbreConexaoAsync()
        {
            if (conexaoProreports.State != ConnectionState.Open)
            {
                // Tenta abrir a conexão e retorna exceção caso não consiga
                try
                {
                    await conexaoProreports.OpenAsync();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }

        public async void Dispose()
        {
            await conexaoProreports.DisposeAsync();
        }
    }
}