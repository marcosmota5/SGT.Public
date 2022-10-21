using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class InstanciaLocal : ObservableObject
    {
        #region Campos

        private int? _id;
        private string? _codigoInstancia;
        private DateTime? _dataAtualizacao;

        #endregion // Campos

        #region Propriedades

        public int? Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string? CodigoInstancia
        {
            get { return _codigoInstancia; }
            set
            {
                if (value != _codigoInstancia)
                {
                    _codigoInstancia = value;
                    OnPropertyChanged(nameof(CodigoInstancia));
                }
            }
        }

        public DateTime? DataAtualizacao
        {
            get { return _dataAtualizacao; }
            set
            {
                if (value != _dataAtualizacao)
                {
                    _dataAtualizacao = value;
                    OnPropertyChanged(nameof(DataAtualizacao));
                }
            }
        }

        #endregion // Propriedades

        #region Métodos

        public async Task GetInstanciaLocal(CancellationToken ct)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Utilização da conexão
            using (var db = new ConexaoMySQL())
            {
                // Tenta abrir a conexão e retorna um erro caso não consiga
                try
                {
                    // Abre a conexão
                    await db.AbreConexaoAsync();
                }
                catch (MySqlConnector.MySqlException)
                {
                    throw;
                }

                // Utilização do comando
                using (var command = db.conexao.CreateCommand())
                {
                    // Define o comando
                    string comando = "SELECT codigo_instancia AS CodigoInstancia, data_atualizacao AS DataAtualizacao FROM tb_instancia WHERE id_instancia = 1";

                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = comando;

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync(ct))
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync(ct))
                            {
                                CodigoInstancia = FuncoesDeConversao.ConverteParaString(reader["CodigoInstancia"]);
                                DataAtualizacao = FuncoesDeConversao.ConverteParaDateTime(reader["DataAtualizacao"]);
                            }
                        }
                    }
                }
            }
        }

        public static async Task AtualizaDataInstancia(CancellationToken ct)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Utilização da conexão
            using (var db = new ConexaoMySQL())
            {
                // Tenta abrir a conexão e retorna um erro caso não consiga
                try
                {
                    // Abre a conexão
                    await db.AbreConexaoAsync();
                }
                catch (MySqlConnector.MySqlException)
                {
                    throw;
                }

                // Utilização do comando
                using (var command = db.conexao.CreateCommand())
                {
                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "UPDATE tb_instancia SET data_atualizacao = @data_atualizacao WHERE id_instancia = 1";
                    command.Parameters.AddWithValue("@data_atualizacao", DateTime.Now);

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);
                }
            }
        }


        #endregion // Métodos
    }
}