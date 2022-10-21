using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class Contato : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private int? _idRetornado;
        private string? _nome;
        private string? _telefone;
        private string? _email;
        private Status? _status;
        private Cliente? _cliente;
        private Cidade? _cidade;

        #endregion // Campos

        #region Propriedades

        public int? Id 
        {
            get { return _id; } 
            set
            {
                if(value != _id)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }
        
        public int? IdRetornado
        { 
            get { return _idRetornado; }
            set
            { 
                if(value != _idRetornado)
                {
                    _idRetornado = value;
                    OnPropertyChanged(nameof(IdRetornado));
                }
            }
        }
        
        public string? Nome 
        {
            get { return _nome; } 
            set
            {
                if(value != _nome)
                {
                    _nome = value;
                    OnPropertyChanged(nameof(Nome));
                }
            }
        }
        
        public string? Telefone 
        {
            get { return _telefone; } 
            set
            { 
                if(value != _telefone)
                {
                    _telefone = value;
                    OnPropertyChanged(nameof(Telefone));
                }
            }
        }
        
        public string? Email 
        {
            get { return _email; }
            set
            {
                if(value != _email)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }
        
        public Status? Status 
        {
            get { return _status; } 
            set 
            {
                if(value != _status)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }
        
        public Cliente? Cliente
        {
            get { return _cliente; }
            set 
            {
                if(value != _cliente)
                {
                    _cliente = value;
                    OnPropertyChanged(nameof(Cliente));
                }
            }
        }
        
        public Cidade? Cidade
        {
            get { return _cidade; }
            set 
            {
                if(value != _cidade)
                {
                    _cidade = value;
                    OnPropertyChanged(nameof(Cidade));
                }
            }
        }

        #endregion // Propriedades

        #region Construtores

        /// <summary>
        /// Construtor sem parâmetros que cria uma nova instância de todas as classes
        /// </summary>
        public Contato()
        {
            Status = new();
            Cliente = new();
            Cidade = new();
        }

        #endregion // Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados do contato com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id do contato que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetContatoDatabaseAsync(int? id, CancellationToken ct)
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
                                          + "cont.id_contato AS IdContato, "
                                          + "cont.nome AS NomeContato, "
                                          + "cont.telefone AS Telefone, "
                                          + "cont.email AS Email, "
                                          + "stat.id_status AS IdStatusContato, "
                                          + "stat.nome AS NomeStatusContato, "
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
                                          + "FROM tb_contatos AS cont "
                                          + "LEFT JOIN tb_status AS stat ON stat.id_status = cont.id_status "
                                          + "LEFT JOIN tb_clientes AS clie ON clie.id_cliente = cont.id_cliente "
                                          + "LEFT JOIN tb_status AS stat_clie ON stat_clie.id_status = clie.id_status "
                                          + "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = cont.id_cidade "
                                          + "LEFT JOIN tb_status AS stat_cida ON stat_cida.id_status = cida.id_status "
                                          + "LEFT JOIN tb_estados AS esta ON esta.id_estado = cida.id_estado "
                                          + "LEFT JOIN tb_status AS stat_esta ON stat_esta.id_status = esta.id_status "
                                          + "LEFT JOIN tb_paises AS pais ON pais.id_pais = esta.id_pais "
                                          + "LEFT JOIN tb_status AS stat_pais ON stat_pais.id_status = pais.id_status "
                                          + "WHERE cont.id_contato = @id";

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
                                if (Cliente == null)
                                {
                                    Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cliente.Status == null)
                                {
                                    Cliente.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cidade == null)
                                {
                                    Cidade = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cidade.Status == null)
                                {
                                    Cidade.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cidade.Estado == null)
                                {
                                    Cidade.Estado = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cidade.Estado.Status == null)
                                {
                                    Cidade.Estado.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cidade.Estado.Pais == null)
                                {
                                    Cidade.Estado.Pais = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cidade.Estado.Pais.Status == null)
                                {
                                    Cidade.Estado.Pais.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdContato"]);
                                IdRetornado = FuncoesDeConversao.ConverteParaInt(reader["IdContato"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeContato"]);
                                Telefone = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["Telefone"]));
                                Email = FuncoesDeConversao.ConverteParaString(reader["Email"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusContato"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusContato"]);
                                Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                                Cidade.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCidade"]);
                                Cidade.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCidade"]);
                                Cidade.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCidade"]);
                                Cidade.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCidade"]);
                                Cidade.Estado.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEstado"]);
                                Cidade.Estado.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeEstado"]);
                                Cidade.Estado.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEstado"]);
                                Cidade.Estado.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEstado"]);
                                Cidade.Estado.Pais.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPais"]);
                                Cidade.Estado.Pais.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePais"]);
                                Cidade.Estado.Pais.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPais"]);
                                Cidade.Estado.Pais.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPais"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados do contato com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetContatoDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                      + "cont.id_contato AS IdContato, "
                      + "cont.nome AS NomeContato, "
                      + "cont.telefone AS Telefone, "
                      + "cont.email AS Email, "
                      + "stat.id_status AS IdStatusContato, "
                      + "stat.nome AS NomeStatusContato, "
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
                      + "FROM tb_contatos AS cont "
                      + "LEFT JOIN tb_status AS stat ON stat.id_status = cont.id_status "
                      + "LEFT JOIN tb_clientes AS clie ON clie.id_cliente = cont.id_cliente "
                      + "LEFT JOIN tb_status AS stat_clie ON stat_clie.id_status = clie.id_status "
                      + "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = cont.id_cidade "
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
                                if (Cliente == null)
                                {
                                    Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cliente.Status == null)
                                {
                                    Cliente.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cidade == null)
                                {
                                    Cidade = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cidade.Status == null)
                                {
                                    Cidade.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cidade.Estado == null)
                                {
                                    Cidade.Estado = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cidade.Estado.Status == null)
                                {
                                    Cidade.Estado.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cidade.Estado.Pais == null)
                                {
                                    Cidade.Estado.Pais = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cidade.Estado.Pais.Status == null)
                                {
                                    Cidade.Estado.Pais.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdContato"]);
                                IdRetornado = FuncoesDeConversao.ConverteParaInt(reader["IdContato"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeContato"]);
                                Telefone = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["Telefone"]));
                                Email = FuncoesDeConversao.ConverteParaString(reader["Email"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusContato"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusContato"]);
                                Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                                Cidade.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCidade"]);
                                Cidade.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCidade"]);
                                Cidade.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCidade"]);
                                Cidade.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCidade"]);
                                Cidade.Estado.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEstado"]);
                                Cidade.Estado.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeEstado"]);
                                Cidade.Estado.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEstado"]);
                                Cidade.Estado.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEstado"]);
                                Cidade.Estado.Pais.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPais"]);
                                Cidade.Estado.Pais.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePais"]);
                                Cidade.Estado.Pais.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPais"]);
                                Cidade.Estado.Pais.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPais"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva o contato na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarContatoDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_contato", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_telefone", Telefone);
                        command.Parameters.AddWithValue("p_email", Email);
                        command.Parameters.AddWithValue("p_id_pais", Cidade.Estado.Pais.Id);
                        command.Parameters.AddWithValue("p_id_estado", Cidade.Estado.Id);
                        command.Parameters.AddWithValue("p_id_cidade", Cidade.Id);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_id_cliente", Cliente.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add("p_id_contato", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException("O contato " + Nome + " já existe para o cliente " + Cliente.Nome);
                        }

                        // Retorna o id do contato
                        Id = FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_contato"].Value);
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_contato", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_contato", Id);
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_telefone", Telefone);
                        command.Parameters.AddWithValue("p_email", Email);
                        command.Parameters.AddWithValue("p_id_pais", Cidade.Estado.Pais.Id);
                        command.Parameters.AddWithValue("p_id_estado", Cidade.Estado.Id);
                        command.Parameters.AddWithValue("p_id_cidade", Cidade.Id);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_id_cliente", Cliente.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException(nameof(Contato).ToLower(), Nome);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta o contato na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarContatoDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_contato", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_contato", Id);
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
                            throw new ChaveEstrangeiraEmUsoException(nameof(Contato).ToLower(), Nome);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException(nameof(Contato).ToLower(), Nome);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de contatos com os argumentos utilizados
        /// </summary>
        /// <param name="listaContatos">Representa a lista de contatos que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaContatosAsync(ObservableCollection<Contato> listaContatos, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaContatos == null)
            {
                listaContatos = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaContatos.Clear();
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
                                      + "cont.id_contato AS IdContato, "
                                      + "cont.nome AS NomeContato, "
                                      + "cont.telefone AS Telefone, "
                                      + "cont.email AS Email, "
                                      + "stat.id_status AS IdStatusContato, "
                                      + "stat.nome AS NomeStatusContato, "
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
                                      + "FROM tb_contatos AS cont "
                                      + "LEFT JOIN tb_status AS stat ON stat.id_status = cont.id_status "
                                      + "LEFT JOIN tb_clientes AS clie ON clie.id_cliente = cont.id_cliente "
                                      + "LEFT JOIN tb_status AS stat_clie ON stat_clie.id_status = clie.id_status "
                                      + "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = cont.id_cidade "
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
                                Contato item = new();

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Cliente == null)
                                {
                                    item.Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Cliente.Status == null)
                                {
                                    item.Cliente.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Cidade == null)
                                {
                                    item.Cidade = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Cidade.Status == null)
                                {
                                    item.Cidade.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Cidade.Estado == null)
                                {
                                    item.Cidade.Estado = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Cidade.Estado.Status == null)
                                {
                                    item.Cidade.Estado.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Cidade.Estado.Pais == null)
                                {
                                    item.Cidade.Estado.Pais = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Cidade.Estado.Pais.Status == null)
                                {
                                    item.Cidade.Estado.Pais.Status = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdContato"]);
                                item.IdRetornado = FuncoesDeConversao.ConverteParaInt(reader["IdContato"]);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeContato"]);
                                item.Telefone = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["Telefone"]));
                                item.Email = FuncoesDeConversao.ConverteParaString(reader["Email"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusContato"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusContato"]);
                                item.Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                item.Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                item.Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                item.Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                                item.Cidade.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCidade"]);
                                item.Cidade.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCidade"]);
                                item.Cidade.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCidade"]);
                                item.Cidade.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCidade"]);
                                item.Cidade.Estado.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEstado"]);
                                item.Cidade.Estado.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeEstado"]);
                                item.Cidade.Estado.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEstado"]);
                                item.Cidade.Estado.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEstado"]);
                                item.Cidade.Estado.Pais.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPais"]);
                                item.Cidade.Estado.Pais.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePais"]);
                                item.Cidade.Estado.Pais.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPais"]);
                                item.Cidade.Estado.Pais.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPais"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaContatos.Add(item);

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

        #endregion // Métodos

        #region Interfaces

        /// <summary>
        /// Método para criar uma cópia da classe já que esse é um tipo de referência que não pode ser atribuído diretamente
        /// </summary>
        public object Clone()
        {
            Contato contatoCopia = new();

            contatoCopia.Id = Id;
            contatoCopia.Nome = Nome;
            contatoCopia.Telefone = Telefone;
            contatoCopia.Email = Email;
            contatoCopia.Status = Status == null ? new() : (Status)Status.Clone();
            contatoCopia.Cliente = Cliente == null ? new() : (Cliente)Cliente.Clone();
            contatoCopia.Cidade = Cidade == null ? new() : (Cidade)Cidade.Clone();

            return contatoCopia;
        }

        #endregion // Interfaces
    }
}
