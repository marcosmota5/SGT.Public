using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class Instancia : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private DateTime? _dataInicio;
        private DateTime? _dataFim;
        private string? _nomeInstancia;
        private string? _codigoInstancia;
        private int? _quantidadeMaximaUsuariosAtivos;
        private string? _nomeEdicao;

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

        public DateTime? DataInicio
        {
            get { return _dataInicio; }
            set
            {
                if (value != _dataInicio)
                {
                    _dataInicio = value;
                    OnPropertyChanged(nameof(DataInicio));
                }
            }
        }

        public DateTime? DataFim
        {
            get { return _dataFim; }
            set
            {
                if (value != _dataFim)
                {
                    _dataFim = value;
                    OnPropertyChanged(nameof(DataFim));
                }
            }
        }

        public string? NomeInstancia
        {
            get { return _nomeInstancia; }
            set
            {
                if (value != _nomeInstancia)
                {
                    _nomeInstancia = value;
                    OnPropertyChanged(nameof(NomeInstancia));
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

        public int? QuantidadeMaximaUsuariosAtivos
        {
            get { return _quantidadeMaximaUsuariosAtivos; }
            set
            {
                if (value != _quantidadeMaximaUsuariosAtivos)
                {
                    _quantidadeMaximaUsuariosAtivos = value;
                    OnPropertyChanged(nameof(QuantidadeMaximaUsuariosAtivos));
                }
            }
        }

        public string? NomeEdicao
        {
            get { return _nomeEdicao; }
            set
            {
                if (value != _nomeEdicao)
                {
                    _nomeEdicao = value;
                    OnPropertyChanged(nameof(NomeEdicao));
                }
            }
        }

        #endregion // Propriedades

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados da instância através do id
        /// </summary>
        /// <param name="id">Representa o id da instância que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetInstanciaDatabaseAsync(string? codigoInstancia, CancellationToken ct)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Utilização da conexão
            using (var db = new ConexaoProreportsMySQL())
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
                using (var command = db.conexaoProreports.CreateCommand())
                {
                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT "
                                         + "inst.id_instancia AS IdInstancia, "
                                         + "inst.data_inicio AS DataInicio, "
                                         + "inst.data_fim AS DataFim, "
                                         + "inst.nome_instancia AS NomeInstancia, "
                                         + "inst.codigo_instancia AS CodigoInstancia, "
                                         + "inst.quantidade_maxima_usuarios_ativos AS QuantidadeMaximaUsuariosAtivos, "
                                         + "inst.nome_edicao AS NomeEdicao "
                                         + "FROM tb_instancias AS inst "
                                         + "WHERE inst.codigo_instancia = @codigo_instancia";
                    command.Parameters.AddWithValue("@codigo_instancia", codigoInstancia);

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync(ct))
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync(ct))
                            {
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdInstancia"]);
                                DataInicio = FuncoesDeConversao.ConverteParaDateTime(reader["DataInicio"]);
                                DataFim = FuncoesDeConversao.ConverteParaDateTime(reader["DataFim"]);
                                NomeInstancia = FuncoesDeConversao.ConverteParaString(reader["NomeInstancia"]);
                                CodigoInstancia = FuncoesDeConversao.ConverteParaString(reader["CodigoInstancia"]);
                                QuantidadeMaximaUsuariosAtivos = FuncoesDeConversao.ConverteParaInt(reader["QuantidadeMaximaUsuariosAtivos"]);
                                NomeEdicao = FuncoesDeConversao.ConverteParaString(reader["NomeEdicao"]);
                            }
                        }
                    }
                }
            }
        }

        #endregion // Métodos

        #region Interfaces

        /// <summary>
        /// Método para criar uma cópia da classe já que esse é um tipo de referência que não pode ser atribuído diretamente
        /// </summary>
        public object Clone()
        {
            Instancia instanciaCopia = new();
            instanciaCopia.Id = Id;
            instanciaCopia.DataInicio = DataInicio;
            instanciaCopia.DataFim = DataFim;
            instanciaCopia.NomeInstancia = NomeInstancia;
            instanciaCopia.CodigoInstancia = CodigoInstancia;
            instanciaCopia.QuantidadeMaximaUsuariosAtivos = QuantidadeMaximaUsuariosAtivos;
            instanciaCopia.NomeEdicao = NomeEdicao;

            return instanciaCopia;
        }

        #endregion // Interfaces
    }
}
