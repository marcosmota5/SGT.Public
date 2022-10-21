using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class Frota : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private string? _nome;
        private Status? _status;
        private Area? _area;

        #endregion Campos

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

        public string? Nome
        {
            get { return _nome; }
            set
            {
                if (value != _nome)
                {
                    _nome = value;
                    OnPropertyChanged(nameof(Nome));
                }
            }
        }

        public Status? Status
        {
            get { return _status; }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public Area? Area
        {
            get { return _area; }
            set
            {
                if (value != _area)
                {
                    _area = value;
                    OnPropertyChanged(nameof(Planta));
                }
            }
        }

        #endregion Propriedades

        #region Construtores

        /// <summary>
        /// Construtor sem parâmetros que cria uma nova instância de todas as classes
        /// </summary>
        public Frota()
        {
            Status = new();
            Area = new();
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados da frota com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id da frota que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetFrotaDatabaseAsync(int? id, CancellationToken ct)
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

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Utilização do comando
                using (var command = db.conexao.CreateCommand())
                {
                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT "
                                          + "frot.id_frota AS IdFrota, "
                                          + "frot.nome AS NomeFrota, "
                                          + "stat.id_status AS IdStatusFrota, "
                                          + "stat.nome AS NomeStatusFrota, "

                                          + "area.id_area AS IdArea, "
                                          + "area.nome AS NomeArea, "
                                          + "stat_area.id_status AS IdStatusArea, "
                                          + "stat_area.nome AS NomeStatusArea, "
                                          + "plan.id_planta AS IdPlanta, "
                                          + "plan.nome AS NomePlanta, "
                                          + "stat_plan.id_status AS IdStatusPlanta, "
                                          + "stat_plan.nome AS NomeStatusPlanta, "
                                          + "clie.id_cliente AS IdCliente, "
                                          + "clie.nome AS NomeCliente, "
                                          + "stat_clie.id_status AS IdStatusCliente, "
                                          + "stat_clie.nome AS NomeStatusCliente, "
                                          + "cida.id_cidade AS IdCidade, "
                                          + "cida.nome AS NomeCidade, "
                                          + "stat_cida.id_status AS IdStatusCidade, "
                                          + "stat_cida.nome AS NomeStatusCidade, "
                                          + "esta.id_estado AS IdEstado, "
                                          + "esta.nome AS NomeEstado, "
                                          + "stat_esta.id_status AS IdStatusEstado, "
                                          + "stat_esta.nome AS NomeStatusEstado, "
                                          + "pais.id_pais AS IdPais, "
                                          + "pais.nome AS NomePais, "
                                          + "stat_pais.id_status AS IdStatusPais, "
                                          + "stat_pais.nome AS NomeStatusPais "
                                          + "FROM tb_frotas AS frot "
                                          + "LEFT JOIN tb_status AS stat ON stat.id_status = frot.id_status "

                                          + "LEFT JOIN tb_areas AS area ON area.id_area = frot.id_area "
                                          + "LEFT JOIN tb_status AS stat_area ON stat_area.id_status = area.id_status "

                                          + "LEFT JOIN tb_plantas AS plan ON plan.id_planta = area.id_planta "
                                          + "LEFT JOIN tb_status AS stat_plan ON stat_plan.id_status = plan.id_status "

                                          + "LEFT JOIN tb_clientes AS clie ON clie.id_cliente = plan.id_cliente "
                                          + "LEFT JOIN tb_status AS stat_clie ON stat_clie.id_status = clie.id_status "
                                          + "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = plan.id_cidade "
                                          + "LEFT JOIN tb_status AS stat_cida ON stat_cida.id_status = cida.id_status "
                                          + "LEFT JOIN tb_estados AS esta ON esta.id_estado = cida.id_estado "
                                          + "LEFT JOIN tb_status AS stat_esta ON stat_esta.id_status = esta.id_status "
                                          + "LEFT JOIN tb_paises AS pais ON pais.id_pais = esta.id_pais "
                                          + "LEFT JOIN tb_status AS stat_pais ON stat_pais.id_status = pais.id_status "
                                          + "WHERE frot.id_frota = @id";

                    command.Parameters.AddWithValue("@id", id);

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync(ct))
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync(ct))
                            {
                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Status == null)
                                {
                                    Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area == null)
                                {
                                    Area = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Status == null)
                                {
                                    Area.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta == null)
                                {
                                    Area.Planta = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Status == null)
                                {
                                    Area.Planta.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cliente == null)
                                {
                                    Area.Planta.Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cliente.Status == null)
                                {
                                    Area.Planta.Cliente.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cidade == null)
                                {
                                    Area.Planta.Cidade = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cidade.Status == null)
                                {
                                    Area.Planta.Cidade.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cidade.Estado == null)
                                {
                                    Area.Planta.Cidade.Estado = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cidade.Estado.Status == null)
                                {
                                    Area.Planta.Cidade.Estado.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cidade.Estado.Pais == null)
                                {
                                    Area.Planta.Cidade.Estado.Pais = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cidade.Estado.Pais.Status == null)
                                {
                                    Area.Planta.Cidade.Estado.Pais.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdFrota"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFrota"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFrota"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFrota"]);
                                Area.Id = FuncoesDeConversao.ConverteParaInt(reader["IdArea"]);
                                Area.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeArea"]);
                                Area.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusArea"]);
                                Area.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusArea"]);
                                Area.Planta.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPlanta"]);
                                Area.Planta.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePlanta"]);
                                Area.Planta.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPlanta"]);
                                Area.Planta.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPlanta"]);
                                Area.Planta.Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Area.Planta.Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                Area.Planta.Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                Area.Planta.Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                                Area.Planta.Cidade.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCidade"]);
                                Area.Planta.Cidade.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCidade"]);
                                Area.Planta.Cidade.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCidade"]);
                                Area.Planta.Cidade.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCidade"]);
                                Area.Planta.Cidade.Estado.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEstado"]);
                                Area.Planta.Cidade.Estado.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeEstado"]);
                                Area.Planta.Cidade.Estado.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEstado"]);
                                Area.Planta.Cidade.Estado.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEstado"]);
                                Area.Planta.Cidade.Estado.Pais.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPais"]);
                                Area.Planta.Cidade.Estado.Pais.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePais"]);
                                Area.Planta.Cidade.Estado.Pais.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPais"]);
                                Area.Planta.Cidade.Estado.Pais.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPais"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados da frota com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetFrotaDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Utilização do comando
                using (var command = db.conexao.CreateCommand())
                {
                    // Define o comando
                    string comando = "SELECT "
                                    + "frot.id_frota AS IdFrota, "
                                    + "frot.nome AS NomeFrota, "
                                    + "stat.id_status AS IdStatusFrota, "
                                    + "stat.nome AS NomeStatusFrota, "

                                    + "area.id_area AS IdArea, "
                                    + "area.nome AS NomeArea, "
                                    + "stat_area.id_status AS IdStatusArea, "
                                    + "stat_area.nome AS NomeStatusArea, "
                                    + "plan.id_planta AS IdPlanta, "
                                    + "plan.nome AS NomePlanta, "
                                    + "stat_plan.id_status AS IdStatusPlanta, "
                                    + "stat_plan.nome AS NomeStatusPlanta, "
                                    + "clie.id_cliente AS IdCliente, "
                                    + "clie.nome AS NomeCliente, "
                                    + "stat_clie.id_status AS IdStatusCliente, "
                                    + "stat_clie.nome AS NomeStatusCliente, "
                                    + "cida.id_cidade AS IdCidade, "
                                    + "cida.nome AS NomeCidade, "
                                    + "stat_cida.id_status AS IdStatusCidade, "
                                    + "stat_cida.nome AS NomeStatusCidade, "
                                    + "esta.id_estado AS IdEstado, "
                                    + "esta.nome AS NomeEstado, "
                                    + "stat_esta.id_status AS IdStatusEstado, "
                                    + "stat_esta.nome AS NomeStatusEstado, "
                                    + "pais.id_pais AS IdPais, "
                                    + "pais.nome AS NomePais, "
                                    + "stat_pais.id_status AS IdStatusPais, "
                                    + "stat_pais.nome AS NomeStatusPais "
                                    + "FROM tb_frotas AS frot "
                                    + "LEFT JOIN tb_status AS stat ON stat.id_status = frot.id_status "

                                    + "LEFT JOIN tb_areas AS area ON area.id_area = frot.id_area "
                                    + "LEFT JOIN tb_status AS stat_area ON stat_area.id_status = area.id_status "

                                    + "LEFT JOIN tb_plantas AS plan ON plan.id_planta = area.id_planta "
                                    + "LEFT JOIN tb_status AS stat_plan ON stat_plan.id_status = plan.id_status "

                                    + "LEFT JOIN tb_clientes AS clie ON clie.id_cliente = plan.id_cliente "
                                    + "LEFT JOIN tb_status AS stat_clie ON stat_clie.id_status = clie.id_status "
                                    + "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = plan.id_cidade "
                                    + "LEFT JOIN tb_status AS stat_cida ON stat_cida.id_status = cida.id_status "
                                    + "LEFT JOIN tb_estados AS esta ON esta.id_estado = cida.id_estado "
                                    + "LEFT JOIN tb_status AS stat_esta ON stat_esta.id_status = esta.id_status "
                                    + "LEFT JOIN tb_paises AS pais ON pais.id_pais = esta.id_pais "
                                    + "LEFT JOIN tb_status AS stat_pais ON stat_pais.id_status = pais.id_status "
                                    + condicoesExtras;

                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = comando;

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
                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync(ct))
                            {
                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Status == null)
                                {
                                    Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area == null)
                                {
                                    Area = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Status == null)
                                {
                                    Area.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta == null)
                                {
                                    Area.Planta = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Status == null)
                                {
                                    Area.Planta.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cliente == null)
                                {
                                    Area.Planta.Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cliente.Status == null)
                                {
                                    Area.Planta.Cliente.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cidade == null)
                                {
                                    Area.Planta.Cidade = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cidade.Status == null)
                                {
                                    Area.Planta.Cidade.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cidade.Estado == null)
                                {
                                    Area.Planta.Cidade.Estado = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cidade.Estado.Status == null)
                                {
                                    Area.Planta.Cidade.Estado.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cidade.Estado.Pais == null)
                                {
                                    Area.Planta.Cidade.Estado.Pais = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Area.Planta.Cidade.Estado.Pais.Status == null)
                                {
                                    Area.Planta.Cidade.Estado.Pais.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdFrota"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFrota"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFrota"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFrota"]);
                                Area.Id = FuncoesDeConversao.ConverteParaInt(reader["IdArea"]);
                                Area.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeArea"]);
                                Area.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusArea"]);
                                Area.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusArea"]);
                                Area.Planta.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPlanta"]);
                                Area.Planta.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePlanta"]);
                                Area.Planta.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPlanta"]);
                                Area.Planta.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPlanta"]);
                                Area.Planta.Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Area.Planta.Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                Area.Planta.Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                Area.Planta.Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                                Area.Planta.Cidade.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCidade"]);
                                Area.Planta.Cidade.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCidade"]);
                                Area.Planta.Cidade.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCidade"]);
                                Area.Planta.Cidade.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCidade"]);
                                Area.Planta.Cidade.Estado.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEstado"]);
                                Area.Planta.Cidade.Estado.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeEstado"]);
                                Area.Planta.Cidade.Estado.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEstado"]);
                                Area.Planta.Cidade.Estado.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEstado"]);
                                Area.Planta.Cidade.Estado.Pais.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPais"]);
                                Area.Planta.Cidade.Estado.Pais.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePais"]);
                                Area.Planta.Cidade.Estado.Pais.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPais"]);
                                Area.Planta.Cidade.Estado.Pais.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPais"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva a frota na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarFrotaDatabaseAsync(CancellationToken ct)
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

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Se o id for nulo, efetua uma inserção, caso contrário efetua uma edição
                if (Id == null)
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_frota", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_id_area", Area.Id);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.Add("p_id_frota", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException("frota", Nome);
                        }

                        // Retorna o id da série
                        Id = FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_frota"].Value);
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_frota", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_frota", Id);
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_id_area", Area.Id);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException("frota", Nome);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta a frota na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarFrotaDatabaseAsync(CancellationToken ct)
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

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Utilização do comando
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_frota", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_frota", Id);
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    try
                    {
                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);
                    }
                    catch (MySqlConnector.MySqlException ex)
                    {
                        // Se o número da exceção for referente à chave estrangeira em uso, lança a exceção referente a isso
                        if (ex.Number == 1451 || ex.Number == 1417)
                        {
                            throw new ChaveEstrangeiraEmUsoException("frota", Nome);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("frota", Nome);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de frotas com os argumentos utilizados
        /// </summary>
        /// <param name="listaFrotas">Representa a lista de frotas que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaFrotasAsync(ObservableCollection<Frota> listaFrotas, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaFrotas == null)
            {
                listaFrotas = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaFrotas.Clear();
            }

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

                // Define o comando
                string comando = "SELECT "
                                + "frot.id_frota AS IdFrota, "
                                + "frot.nome AS NomeFrota, "
                                + "stat.id_status AS IdStatusFrota, "
                                + "stat.nome AS NomeStatusFrota, "

                                + "area.id_area AS IdArea, "
                                + "area.nome AS NomeArea, "
                                + "stat_area.id_status AS IdStatusArea, "
                                + "stat_area.nome AS NomeStatusArea, "
                                + "plan.id_planta AS IdPlanta, "
                                + "plan.nome AS NomePlanta, "
                                + "stat_plan.id_status AS IdStatusPlanta, "
                                + "stat_plan.nome AS NomeStatusPlanta, "
                                + "clie.id_cliente AS IdCliente, "
                                + "clie.nome AS NomeCliente, "
                                + "stat_clie.id_status AS IdStatusCliente, "
                                + "stat_clie.nome AS NomeStatusCliente, "
                                + "cida.id_cidade AS IdCidade, "
                                + "cida.nome AS NomeCidade, "
                                + "stat_cida.id_status AS IdStatusCidade, "
                                + "stat_cida.nome AS NomeStatusCidade, "
                                + "esta.id_estado AS IdEstado, "
                                + "esta.nome AS NomeEstado, "
                                + "stat_esta.id_status AS IdStatusEstado, "
                                + "stat_esta.nome AS NomeStatusEstado, "
                                + "pais.id_pais AS IdPais, "
                                + "pais.nome AS NomePais, "
                                + "stat_pais.id_status AS IdStatusPais, "
                                + "stat_pais.nome AS NomeStatusPais "
                                + "FROM tb_frotas AS frot "
                                + "LEFT JOIN tb_status AS stat ON stat.id_status = frot.id_status "

                                + "LEFT JOIN tb_areas AS area ON area.id_area = frot.id_area "
                                + "LEFT JOIN tb_status AS stat_area ON stat_area.id_status = area.id_status "

                                + "LEFT JOIN tb_plantas AS plan ON plan.id_planta = area.id_planta "
                                + "LEFT JOIN tb_status AS stat_plan ON stat_plan.id_status = plan.id_status "

                                + "LEFT JOIN tb_clientes AS clie ON clie.id_cliente = plan.id_cliente "
                                + "LEFT JOIN tb_status AS stat_clie ON stat_clie.id_status = clie.id_status "
                                + "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = plan.id_cidade "
                                + "LEFT JOIN tb_status AS stat_cida ON stat_cida.id_status = cida.id_status "
                                + "LEFT JOIN tb_estados AS esta ON esta.id_estado = cida.id_estado "
                                + "LEFT JOIN tb_status AS stat_esta ON stat_esta.id_status = esta.id_status "
                                + "LEFT JOIN tb_paises AS pais ON pais.id_pais = esta.id_pais "
                                + "LEFT JOIN tb_status AS stat_pais ON stat_pais.id_status = pais.id_status "
                                + condicoesExtras;

                // Cria e atribui a variável do total de linhas através da função específica para contagem de linhas
                int totalLinhas = await FuncoesDeDatabase.GetQuantidadeLinhasReaderAsync(db, comando, ct, nomesParametrosSeparadosPorVirgulas, valoresParametros);

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Utilização do comando
                using (var command = db.conexao.CreateCommand())
                {
                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = comando;

                    // Cria uma array com os parâmetros passados utilizando vírgula como delimitador
                    string[] nomesParametros = nomesParametrosSeparadosPorVirgulas.Split(",");

                    // Cria um contador para retornar o nome do parametro corretamente
                    int contadorParametros = 0;

                    // Varre o array de parâmetros adicionando-os à consulta
                    foreach (var item in valoresParametros)
                    {
                        command.Parameters.AddWithValue(nomesParametros[contadorParametros].Trim(), item);
                        contadorParametros++;
                    }

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync(ct))
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            // Cria e atribui a variável de contagem de linhas
                            int linhaAtual = 0;

                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync(ct))
                            {
                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Cria um novo item e atribui os valores
                                Frota item = new();

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Area == null)
                                {
                                    item.Area = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Area.Status == null)
                                {
                                    item.Area.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Area.Planta == null)
                                {
                                    item.Area.Planta = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Area.Planta.Status == null)
                                {
                                    item.Area.Planta.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Area.Planta.Cliente == null)
                                {
                                    item.Area.Planta.Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Area.Planta.Cliente.Status == null)
                                {
                                    item.Area.Planta.Cliente.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Area.Planta.Cidade == null)
                                {
                                    item.Area.Planta.Cidade = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Area.Planta.Cidade.Status == null)
                                {
                                    item.Area.Planta.Cidade.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Area.Planta.Cidade.Estado == null)
                                {
                                    item.Area.Planta.Cidade.Estado = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Area.Planta.Cidade.Estado.Status == null)
                                {
                                    item.Area.Planta.Cidade.Estado.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Area.Planta.Cidade.Estado.Pais == null)
                                {
                                    item.Area.Planta.Cidade.Estado.Pais = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Area.Planta.Cidade.Estado.Pais.Status == null)
                                {
                                    item.Area.Planta.Cidade.Estado.Pais.Status = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFrota"]);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFrota"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFrota"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFrota"]);
                                item.Area.Id = FuncoesDeConversao.ConverteParaInt(reader["IdArea"]);
                                item.Area.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeArea"]);
                                item.Area.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusArea"]);
                                item.Area.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusArea"]);
                                item.Area.Planta.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPlanta"]);
                                item.Area.Planta.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePlanta"]);
                                item.Area.Planta.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPlanta"]);
                                item.Area.Planta.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPlanta"]);
                                item.Area.Planta.Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                item.Area.Planta.Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                item.Area.Planta.Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                item.Area.Planta.Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                                item.Area.Planta.Cidade.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCidade"]);
                                item.Area.Planta.Cidade.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCidade"]);
                                item.Area.Planta.Cidade.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCidade"]);
                                item.Area.Planta.Cidade.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCidade"]);
                                item.Area.Planta.Cidade.Estado.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEstado"]);
                                item.Area.Planta.Cidade.Estado.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeEstado"]);
                                item.Area.Planta.Cidade.Estado.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEstado"]);
                                item.Area.Planta.Cidade.Estado.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEstado"]);
                                item.Area.Planta.Cidade.Estado.Pais.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPais"]);
                                item.Area.Planta.Cidade.Estado.Pais.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePais"]);
                                item.Area.Planta.Cidade.Estado.Pais.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPais"]);
                                item.Area.Planta.Cidade.Estado.Pais.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPais"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaFrotas.Add(item);

                                // Incrementa a linha atual
                                linhaAtual++;

                                // Reporta o progresso se o progresso não for nulo
                                if (reportadorProgresso != null)
                                {
                                    reportadorProgresso.Report((double)linhaAtual / (double)totalLinhas * (double)100);
                                }

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();
                            }
                        }
                    }
                }
            }
        }

        #endregion Métodos

        #region Interfaces

        /// <summary>
        /// Método para criar uma cópia da classe já que esse é um tipo de referência que não pode ser atribuído diretamente
        /// </summary>
        public object Clone()
        {
            Frota frotaCopia = new();

            frotaCopia.Id = Id;
            frotaCopia.Nome = Nome;
            frotaCopia.Status = Status == null ? new() : (Status)Status.Clone();
            frotaCopia.Area = Area == null ? new() : (Area)Area.Clone();

            return frotaCopia;
        }

        #endregion Interfaces
    }
}