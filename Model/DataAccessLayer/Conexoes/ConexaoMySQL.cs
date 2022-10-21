using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.Conexoes
{
    internal class ConexaoMySQL : IDisposable
    {
        public readonly MySqlConnection conexao;

        public ConexaoMySQL()
        {
            // Definição da string de conexão, considerando a execução em ambiente de teste ou produção
            string stringConexao;
#if DEBUG
            stringConexao = "server=localhost;user id=root;password=Test@12345;persistsecurityinfo=True;database=db;port=3306;";
#else
           stringConexao = "server=localhost;user id=root;password=Test@12345;persistsecurityinfo=True;database=db;port=3306;";
#endif
            conexao = new MySqlConnection(stringConexao);
        }

        public async Task AbreConexaoAsync()
        {
            if (conexao.State != ConnectionState.Open)
            {
                // Tenta abrir a conexão e retorna exceção caso não consiga
                try
                {
                    await conexao.OpenAsync();
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
        }

        public async void Dispose()
        {
            await conexao.DisposeAsync();
        }
    }
}