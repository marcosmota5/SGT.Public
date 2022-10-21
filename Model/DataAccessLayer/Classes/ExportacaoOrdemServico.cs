using System.Collections.ObjectModel;
using System.Data;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class ExportacaoOrdemServico : ObservableObject
    {
        private DataTable _dataTable;

        public DataTable DataTable
        {
            get { return _dataTable; }
            set
            {
                if (value != _dataTable)
                {
                    _dataTable = value;
                    OnPropertyChanged(nameof(DataTable));
                }
            }
        }

        public static async Task<DataTable> GetDataTableAsync(CancellationToken ct, bool apenasFiltros, string condicoes, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            DataSet ds = new();
            DataTable dataTable = new();

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

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Utilização do comando
                using (var command = db.conexao.CreateCommand())
                {
                    string comando;

                    if (apenasFiltros)
                    {
                        comando = "SELECT " +
                                    "`Local`, " +
                                    "`Tipo da OS`, " +
                                    "`Cliente`, " +
                                    "`Planta`, " +
                                    "`Área`, " +
                                    "`Frota`, " +
                                    "`Executante do serviço`, " +
                                    "`Equipamento`, " +
                                    "`Modelo`, " +
                                    "`Equipamento após a manutenção`, " +
                                    "`Equipamento operacional`, " +
                                    "`Tipo da manutenção`, " +
                                    "`Ano`, " +
                                    "`Fabricante`, " +
                                    "`Categoria`, " +
                                    "`Classe`, " +
                                    "`Tipo do equipamento`, " +
                                    "`Passos executados` " +
                                    " FROM `vw_registro_servico` ";
                    }
                    else
                    {
                        comando = "SELECT * FROM `vw_registro_servico` ";
                    }

                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = CommandType.Text;

                    if (!String.IsNullOrEmpty(condicoes))
                    {
                        command.CommandText = comando + " WHERE " + condicoes;
                    }
                    else
                    {
                        command.CommandText = comando;
                    }

                    // Cria uma array com os parâmetros passados utilizando vírgula como delimitador
                    string[] nomesParametros = nomesParametrosSeparadosPorVirgulas.Split(",");

                    // Cria um contador para retornar o nome do parametro corretamente
                    int contadorParametros = 0;

                    // Varre o array de parâmetros adicionando-os à consulta
                    foreach (var item in valoresParametros)
                    {
                        // Lança exceção de cancelamento caso ela tenha sido efetuada
                        ct.ThrowIfCancellationRequested();

                        command.Parameters.AddWithValue(nomesParametros[contadorParametros].Trim(), item);
                        contadorParametros++;
                    }

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync(ct))
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            ds.Tables.Add(dataTable);
                            ds.EnforceConstraints = false;
                            dataTable.Load(reader);
                            reader.Close();
                        }
                    }
                }
            }
            return dataTable;
        }
    }
}