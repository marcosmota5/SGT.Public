using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class Modelo : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private string? _nome;
        private Status? _status;
        private Fabricante? _fabricante;
        private TipoEquipamento? _tipoEquipamento;
        private Categoria? _categoria;
        private Classe? _classe;

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

        public Fabricante? Fabricante
        {
            get { return _fabricante; }
            set
            {
                if (value != _fabricante)
                {
                    _fabricante = value;
                    OnPropertyChanged(nameof(Fabricante));
                }
            }
        }

        public TipoEquipamento? TipoEquipamento
        {
            get { return _tipoEquipamento; }
            set
            {
                if (value != _tipoEquipamento)
                {
                    _tipoEquipamento = value;
                    OnPropertyChanged(nameof(TipoEquipamento));
                }
            }
        }

        public Categoria? Categoria
        {
            get { return _categoria; }
            set
            {
                if (value != _categoria)
                {
                    _categoria = value;
                    OnPropertyChanged(nameof(Categoria));
                }
            }
        }

        public Classe? Classe
        {
            get { return _classe; }
            set
            {
                if (value != _classe)
                {
                    _classe = value;
                    OnPropertyChanged(nameof(Classe));
                }
            }
        }

        #endregion Propriedades

        #region Construtores

        /// <summary>
        /// Construtor sem parâmetros que cria uma nova instância de todas as classes
        /// </summary>
        public Modelo()
        {
            Status = new();
            Fabricante = new();
            TipoEquipamento = new();
            Categoria = new();
            Classe = new();
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados do modelo com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id do modelo que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetModeloDatabaseAsync(int? id, CancellationToken ct)
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
                                          + "mode.id_modelo AS IdModelo, "
                                          + "mode.nome AS NomeModelo, "
                                          + "stat.id_status AS IdStatusModelo, "
                                          + "stat.nome AS NomeStatusModelo, "
                                          + "fabr.id_fabricante AS IdFabricante, "
                                          + "fabr.nome AS NomeFabricante, "
                                          + "stat_fabr.id_status AS IdStatusFabricante, "
                                          + "stat_fabr.nome AS NomeStatusFabricante, "
                                          + "tieq.id_tipo_equipamento AS IdTipoEquipamento, "
                                          + "tieq.nome AS NomeTipoEquipamento, "
                                          + "stat_tieq.id_status AS IdStatusTipoEquipamento, "
                                          + "stat_tieq.nome AS NomeStatusTipoEquipamento, "
                                          + "cate.id_categoria AS IdCategoria, "
                                          + "cate.nome AS NomeCategoria, "
                                          + "stat_cate.id_status AS IdStatusCategoria, "
                                          + "stat_cate.nome AS NomeStatusCategoria, "
                                          + "clas.id_classe AS IdClasse, "
                                          + "clas.nome AS NomeClasse, "
                                          + "stat_clas.id_status AS IdStatusClasse, "
                                          + "stat_clas.nome AS NomeStatusClasse "
                                          + "FROM tb_modelos AS mode "
                                          + "LEFT JOIN tb_status AS stat ON stat.id_status = mode.id_status "
                                          + "LEFT JOIN tb_fabricantes AS fabr ON fabr.id_fabricante = mode.id_fabricante "
                                          + "LEFT JOIN tb_status AS stat_fabr ON stat_fabr.id_status = fabr.id_status "
                                          + "LEFT JOIN tb_tipos_equipamento AS tieq ON tieq.id_tipo_equipamento = mode.id_tipo_equipamento "
                                          + "LEFT JOIN tb_status AS stat_tieq ON stat_tieq.id_status = tieq.id_status "
                                          + "LEFT JOIN tb_categorias AS cate ON cate.id_categoria = mode.id_categoria "
                                          + "LEFT JOIN tb_status AS stat_cate ON stat_cate.id_status = cate.id_status "
                                          + "LEFT JOIN tb_classes AS clas ON clas.id_classe = mode.id_classe "
                                          + "LEFT JOIN tb_status AS stat_clas ON stat_clas.id_status = clas.id_status "
                                          + "WHERE mode.id_modelo = @id";

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
                                if (Fabricante == null)
                                {
                                    Fabricante = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Fabricante.Status == null)
                                {
                                    Fabricante.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoEquipamento == null)
                                {
                                    TipoEquipamento = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoEquipamento.Status == null)
                                {
                                    TipoEquipamento.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Categoria == null)
                                {
                                    Categoria = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Categoria.Status == null)
                                {
                                    Categoria.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Classe == null)
                                {
                                    Classe = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Classe.Status == null)
                                {
                                    Classe.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdModelo"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeModelo"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusModelo"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusModelo"]);
                                Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);
                                TipoEquipamento.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoEquipamento"]);
                                TipoEquipamento.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTipoEquipamento"]);
                                TipoEquipamento.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTipoEquipamento"]);
                                TipoEquipamento.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTipoEquipamento"]);
                                Categoria.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCategoria"]);
                                Categoria.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCategoria"]);
                                Categoria.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCategoria"]);
                                Categoria.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCategoria"]);
                                Classe.Id = FuncoesDeConversao.ConverteParaInt(reader["IdClasse"]);
                                Classe.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeClasse"]);
                                Classe.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusClasse"]);
                                Classe.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusClasse"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados do modelo com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetModeloDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                      + "mode.id_modelo AS IdModelo, "
                      + "mode.nome AS NomeModelo, "
                      + "stat.id_status AS IdStatusModelo, "
                      + "stat.nome AS NomeStatusModelo, "
                      + "fabr.id_fabricante AS IdFabricante, "
                      + "fabr.nome AS NomeFabricante, "
                      + "stat_fabr.id_status AS IdStatusFabricante, "
                      + "stat_fabr.nome AS NomeStatusFabricante, "
                      + "tieq.id_tipo_equipamento AS IdTipoEquipamento, "
                      + "tieq.nome AS NomeTipoEquipamento, "
                      + "stat_tieq.id_status AS IdStatusTipoEquipamento, "
                      + "stat_tieq.nome AS NomeStatusTipoEquipamento, "
                      + "cate.id_categoria AS IdCategoria, "
                      + "cate.nome AS NomeCategoria, "
                      + "stat_cate.id_status AS IdStatusCategoria, "
                      + "stat_cate.nome AS NomeStatusCategoria, "
                      + "clas.id_classe AS IdClasse, "
                      + "clas.nome AS NomeClasse, "
                      + "stat_clas.id_status AS IdStatusClasse, "
                      + "stat_clas.nome AS NomeStatusClasse "
                      + "FROM tb_modelos AS mode "
                      + "LEFT JOIN tb_status AS stat ON stat.id_status = mode.id_status "
                      + "LEFT JOIN tb_fabricantes AS fabr ON fabr.id_fabricante = mode.id_fabricante "
                      + "LEFT JOIN tb_status AS stat_fabr ON stat_fabr.id_status = fabr.id_status "
                      + "LEFT JOIN tb_tipos_equipamento AS tieq ON tieq.id_tipo_equipamento = mode.id_tipo_equipamento "
                      + "LEFT JOIN tb_status AS stat_tieq ON stat_tieq.id_status = tieq.id_status "
                      + "LEFT JOIN tb_categorias AS cate ON cate.id_categoria = mode.id_categoria "
                      + "LEFT JOIN tb_status AS stat_cate ON stat_cate.id_status = cate.id_status "
                      + "LEFT JOIN tb_classes AS clas ON clas.id_classe = mode.id_classe "
                      + "LEFT JOIN tb_status AS stat_clas ON stat_clas.id_status = clas.id_status "
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
                                if (Fabricante == null)
                                {
                                    Fabricante = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Fabricante.Status == null)
                                {
                                    Fabricante.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoEquipamento == null)
                                {
                                    TipoEquipamento = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (TipoEquipamento.Status == null)
                                {
                                    TipoEquipamento.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Categoria == null)
                                {
                                    Categoria = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Categoria.Status == null)
                                {
                                    Categoria.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Classe == null)
                                {
                                    Classe = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Classe.Status == null)
                                {
                                    Classe.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdModelo"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeModelo"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusModelo"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusModelo"]);
                                Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);
                                TipoEquipamento.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoEquipamento"]);
                                TipoEquipamento.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTipoEquipamento"]);
                                TipoEquipamento.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTipoEquipamento"]);
                                TipoEquipamento.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTipoEquipamento"]);
                                Categoria.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCategoria"]);
                                Categoria.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCategoria"]);
                                Categoria.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCategoria"]);
                                Categoria.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCategoria"]);
                                Classe.Id = FuncoesDeConversao.ConverteParaInt(reader["IdClasse"]);
                                Classe.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeClasse"]);
                                Classe.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusClasse"]);
                                Classe.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusClasse"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva o modelo na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarModeloDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_modelo", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_id_fabricante", Fabricante.Id);
                        command.Parameters.AddWithValue("p_id_tipo_equipamento", TipoEquipamento.Id);
                        command.Parameters.AddWithValue("p_id_categoria", Categoria.Id);
                        command.Parameters.AddWithValue("p_id_classe", Classe.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException(nameof(Modelo).ToLower(), Nome);
                        }
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_modelo", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_modelo", Id);
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_id_fabricante", Fabricante.Id);
                        command.Parameters.AddWithValue("p_id_tipo_equipamento", TipoEquipamento.Id);
                        command.Parameters.AddWithValue("p_id_categoria", Categoria.Id);
                        command.Parameters.AddWithValue("p_id_classe", Classe.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException(nameof(Modelo).ToLower(), Nome);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta o modelo na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarModeloDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_modelo", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_modelo", Id);
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
                            throw new ChaveEstrangeiraEmUsoException(nameof(Modelo).ToLower(), Nome);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException(nameof(Modelo).ToLower(), Nome);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de modelos com os argumentos utilizados
        /// </summary>
        /// <param name="listaModelos">Representa a lista de modelos que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaModelosAsync(ObservableCollection<Modelo> listaModelos, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaModelos == null)
            {
                listaModelos = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaModelos.Clear();
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
                  + "mode.id_modelo AS IdModelo, "
                  + "mode.nome AS NomeModelo, "
                  + "stat.id_status AS IdStatusModelo, "
                  + "stat.nome AS NomeStatusModelo, "
                  + "fabr.id_fabricante AS IdFabricante, "
                  + "fabr.nome AS NomeFabricante, "
                  + "stat_fabr.id_status AS IdStatusFabricante, "
                  + "stat_fabr.nome AS NomeStatusFabricante, "
                  + "tieq.id_tipo_equipamento AS IdTipoEquipamento, "
                  + "tieq.nome AS NomeTipoEquipamento, "
                  + "stat_tieq.id_status AS IdStatusTipoEquipamento, "
                  + "stat_tieq.nome AS NomeStatusTipoEquipamento, "
                  + "cate.id_categoria AS IdCategoria, "
                  + "cate.nome AS NomeCategoria, "
                  + "stat_cate.id_status AS IdStatusCategoria, "
                  + "stat_cate.nome AS NomeStatusCategoria, "
                  + "clas.id_classe AS IdClasse, "
                  + "clas.nome AS NomeClasse, "
                  + "stat_clas.id_status AS IdStatusClasse, "
                  + "stat_clas.nome AS NomeStatusClasse "
                  + "FROM tb_modelos AS mode "
                  + "LEFT JOIN tb_status AS stat ON stat.id_status = mode.id_status "
                  + "LEFT JOIN tb_fabricantes AS fabr ON fabr.id_fabricante = mode.id_fabricante "
                  + "LEFT JOIN tb_status AS stat_fabr ON stat_fabr.id_status = fabr.id_status "
                  + "LEFT JOIN tb_tipos_equipamento AS tieq ON tieq.id_tipo_equipamento = mode.id_tipo_equipamento "
                  + "LEFT JOIN tb_status AS stat_tieq ON stat_tieq.id_status = tieq.id_status "
                  + "LEFT JOIN tb_categorias AS cate ON cate.id_categoria = mode.id_categoria "
                  + "LEFT JOIN tb_status AS stat_cate ON stat_cate.id_status = cate.id_status "
                  + "LEFT JOIN tb_classes AS clas ON clas.id_classe = mode.id_classe "
                  + "LEFT JOIN tb_status AS stat_clas ON stat_clas.id_status = clas.id_status "
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
                                Modelo item = new();

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Fabricante == null)
                                {
                                    item.Fabricante = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Fabricante.Status == null)
                                {
                                    item.Fabricante.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.TipoEquipamento == null)
                                {
                                    item.TipoEquipamento = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.TipoEquipamento.Status == null)
                                {
                                    item.TipoEquipamento.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Categoria == null)
                                {
                                    item.Categoria = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Categoria.Status == null)
                                {
                                    item.Categoria.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Classe == null)
                                {
                                    item.Classe = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Classe.Status == null)
                                {
                                    item.Classe.Status = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdModelo"]);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeModelo"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusModelo"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusModelo"]);
                                item.Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                item.Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                item.Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                item.Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);
                                item.TipoEquipamento.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoEquipamento"]);
                                item.TipoEquipamento.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTipoEquipamento"]);
                                item.TipoEquipamento.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTipoEquipamento"]);
                                item.TipoEquipamento.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTipoEquipamento"]);
                                item.Categoria.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCategoria"]);
                                item.Categoria.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCategoria"]);
                                item.Categoria.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCategoria"]);
                                item.Categoria.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCategoria"]);
                                item.Classe.Id = FuncoesDeConversao.ConverteParaInt(reader["IdClasse"]);
                                item.Classe.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeClasse"]);
                                item.Classe.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusClasse"]);
                                item.Classe.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusClasse"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaModelos.Add(item);

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
            Modelo modeloCopia = new();

            modeloCopia.Id = Id;
            modeloCopia.Nome = Nome;
            modeloCopia.Status = Status == null ? new() : (Status)Status.Clone();
            modeloCopia.Fabricante = Fabricante == null ? new() : (Fabricante)Fabricante.Clone();
            modeloCopia.TipoEquipamento = TipoEquipamento == null ? new() : (TipoEquipamento)TipoEquipamento.Clone();
            modeloCopia.Categoria = Categoria == null ? new() : (Categoria)Categoria.Clone();
            modeloCopia.Classe = Classe == null ? new() : (Classe)Classe.Clone();

            return modeloCopia;
        }

        #endregion Interfaces
    }
}