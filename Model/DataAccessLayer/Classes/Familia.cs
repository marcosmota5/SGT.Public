using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class Familia : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private int? _idRetornado;
        private string? _nome;
        private Status? _status;
        private Modelo? _modelo;

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

        public int? IdRetornado
        {
            get { return _idRetornado; }
            set
            {
                if (value != _idRetornado)
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

        public Modelo? Modelo
        {
            get { return _modelo; }
            set
            {
                if (value != _modelo)
                {
                    _modelo = value;
                    OnPropertyChanged(nameof(Modelo));
                }
            }
        }

        #endregion Propriedades

        #region Construtores

        /// <summary>
        /// Construtor sem parâmetros que cria uma nova instância de todas as classes
        /// </summary>
        public Familia()
        {
            Status = new();
            Modelo = new();
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados da família com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id da família que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetFamiliaDatabaseAsync(int? id, CancellationToken ct)
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
                                   + "fami.id_familia AS IdFamilia, "
                                   + "fami.familia AS NomeFamilia, "
                                   + "stat.id_status AS IdStatusFamilia, "
                                   + "stat.nome AS NomeStatusFamilia, "
                                   + "mode.id_modelo AS IdModelo, "
                                   + "mode.nome AS NomeModelo, "
                                   + "stat_mode.id_status AS IdStatusModelo, "
                                   + "stat_mode.nome AS NomeStatusModelo, "
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
                                   + "FROM tb_familias AS fami "
                                   + "LEFT JOIN tb_status AS stat ON stat.id_status = fami.id_status "
                                   + "LEFT JOIN tb_modelos AS mode ON mode.id_modelo = fami.id_modelo "
                                   + "LEFT JOIN tb_status AS stat_mode ON stat_mode.id_status = mode.id_status "
                                   + "LEFT JOIN tb_fabricantes AS fabr ON fabr.id_fabricante = mode.id_fabricante "
                                   + "LEFT JOIN tb_status AS stat_fabr ON stat_fabr.id_status = fabr.id_status "
                                   + "LEFT JOIN tb_tipos_equipamento AS tieq ON tieq.id_tipo_equipamento = mode.id_tipo_equipamento "
                                   + "LEFT JOIN tb_status AS stat_tieq ON stat_tieq.id_status = tieq.id_status "
                                   + "LEFT JOIN tb_categorias AS cate ON cate.id_categoria = mode.id_categoria "
                                   + "LEFT JOIN tb_status AS stat_cate ON stat_cate.id_status = cate.id_status "
                                   + "LEFT JOIN tb_classes AS clas ON clas.id_classe = mode.id_classe "
                                   + "LEFT JOIN tb_status AS stat_clas ON stat_clas.id_status = clas.id_status "
                                   + "WHERE fami.id_familia = @id";

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
                                if (Modelo == null)
                                {
                                    Modelo = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Status == null)
                                {
                                    Modelo.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Fabricante == null)
                                {
                                    Modelo.Fabricante = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Fabricante.Status == null)
                                {
                                    Modelo.Fabricante.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.TipoEquipamento == null)
                                {
                                    Modelo.TipoEquipamento = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.TipoEquipamento.Status == null)
                                {
                                    Modelo.TipoEquipamento.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Categoria == null)
                                {
                                    Modelo.Categoria = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Categoria.Status == null)
                                {
                                    Modelo.Categoria.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Classe == null)
                                {
                                    Modelo.Classe = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Classe.Status == null)
                                {
                                    Modelo.Classe.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdFamilia"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFamilia"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFamilia"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFamilia"]);
                                Modelo.Id = FuncoesDeConversao.ConverteParaInt(reader["IdModelo"]);
                                Modelo.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeModelo"]);
                                Modelo.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusModelo"]);
                                Modelo.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusModelo"]);
                                Modelo.Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                Modelo.Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                Modelo.Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                Modelo.Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);
                                Modelo.TipoEquipamento.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoEquipamento"]);
                                Modelo.TipoEquipamento.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTipoEquipamento"]);
                                Modelo.TipoEquipamento.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTipoEquipamento"]);
                                Modelo.TipoEquipamento.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTipoEquipamento"]);
                                Modelo.Categoria.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCategoria"]);
                                Modelo.Categoria.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCategoria"]);
                                Modelo.Categoria.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCategoria"]);
                                Modelo.Categoria.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCategoria"]);
                                Modelo.Classe.Id = FuncoesDeConversao.ConverteParaInt(reader["IdClasse"]);
                                Modelo.Classe.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeClasse"]);
                                Modelo.Classe.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusClasse"]);
                                Modelo.Classe.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusClasse"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados da família com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetFamiliaDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                                   + "fami.id_familia AS IdFamilia, "
                                   + "fami.familia AS NomeFamilia, "
                                   + "stat.id_status AS IdStatusFamilia, "
                                   + "stat.nome AS NomeStatusFamilia, "
                                   + "mode.id_modelo AS IdModelo, "
                                   + "mode.nome AS NomeModelo, "
                                   + "stat_mode.id_status AS IdStatusModelo, "
                                   + "stat_mode.nome AS NomeStatusModelo, "
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
                                   + "FROM tb_familias AS fami "
                                   + "LEFT JOIN tb_status AS stat ON stat.id_status = fami.id_status "
                                   + "LEFT JOIN tb_modelos AS mode ON mode.id_modelo = fami.id_modelo "
                                   + "LEFT JOIN tb_status AS stat_mode ON stat_mode.id_status = mode.id_status "
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
                                if (Modelo == null)
                                {
                                    Modelo = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Status == null)
                                {
                                    Modelo.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Fabricante == null)
                                {
                                    Modelo.Fabricante = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Fabricante.Status == null)
                                {
                                    Modelo.Fabricante.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.TipoEquipamento == null)
                                {
                                    Modelo.TipoEquipamento = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.TipoEquipamento.Status == null)
                                {
                                    Modelo.TipoEquipamento.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Categoria == null)
                                {
                                    Modelo.Categoria = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Categoria.Status == null)
                                {
                                    Modelo.Categoria.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Classe == null)
                                {
                                    Modelo.Classe = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Classe.Status == null)
                                {
                                    Modelo.Classe.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdFamilia"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFamilia"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFamilia"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFamilia"]);
                                Modelo.Id = FuncoesDeConversao.ConverteParaInt(reader["IdModelo"]);
                                Modelo.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeModelo"]);
                                Modelo.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusModelo"]);
                                Modelo.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusModelo"]);
                                Modelo.Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                Modelo.Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                Modelo.Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                Modelo.Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);
                                Modelo.TipoEquipamento.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoEquipamento"]);
                                Modelo.TipoEquipamento.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTipoEquipamento"]);
                                Modelo.TipoEquipamento.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTipoEquipamento"]);
                                Modelo.TipoEquipamento.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTipoEquipamento"]);
                                Modelo.Categoria.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCategoria"]);
                                Modelo.Categoria.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCategoria"]);
                                Modelo.Categoria.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCategoria"]);
                                Modelo.Categoria.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCategoria"]);
                                Modelo.Classe.Id = FuncoesDeConversao.ConverteParaInt(reader["IdClasse"]);
                                Modelo.Classe.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeClasse"]);
                                Modelo.Classe.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusClasse"]);
                                Modelo.Classe.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusClasse"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados da família com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id da família que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetFamiliaDatabaseAsync(string? serie, CancellationToken ct)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Declaração das variáveis a serem utilizadas para preencher os dados da classe
            string? familiaRetornada;

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
                using (MySqlConnector.MySqlCommand command = new("sp_verifica_familia", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_serie", serie);
                    command.Parameters.Add("p_familia", MySqlConnector.MySqlDbType.VarChar, 10).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("série", serie);
                    }

                    familiaRetornada = command.Parameters["p_familia"].Value.ToString();
                }
            }

            // Se for retornada alguma família, preenche os dados
            if (!String.IsNullOrEmpty(familiaRetornada))
            {
                await GetFamiliaDatabaseAsync(ct, "WHERE fami.familia = @familia", "@familia", familiaRetornada);
            }
        }

        /// <summary>
        /// Método assíncrono que salva a família na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarFamiliaDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_familia", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_familia", Nome);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_id_fabricante", Modelo.Fabricante.Id);
                        command.Parameters.AddWithValue("p_id_categoria", Modelo.Categoria.Id);
                        command.Parameters.AddWithValue("p_id_tipo_equipamento", Modelo.TipoEquipamento.Id);
                        command.Parameters.AddWithValue("p_id_classe", Modelo.Classe.Id);
                        command.Parameters.AddWithValue("p_id_modelo", Modelo.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException("A família " + Nome + " já existe");
                        }
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_familia", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_familia", Id);
                        command.Parameters.AddWithValue("p_familia", Nome);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_id_fabricante", Modelo.Fabricante.Id);
                        command.Parameters.AddWithValue("p_id_categoria", Modelo.Categoria.Id);
                        command.Parameters.AddWithValue("p_id_tipo_equipamento", Modelo.TipoEquipamento.Id);
                        command.Parameters.AddWithValue("p_id_classe", Modelo.Classe.Id);
                        command.Parameters.AddWithValue("p_id_modelo", Modelo.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException("família", Nome);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta a família na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarFamiliaDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_familia", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_familia", Id);
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
                            throw new ChaveEstrangeiraEmUsoException("família", Nome);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("família", Nome);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de famílias com os argumentos utilizados
        /// </summary>
        /// <param name="listaFamilias">Representa a lista de famílias que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaFamiliasAsync(ObservableCollection<Familia> listaFamilias, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaFamilias == null)
            {
                listaFamilias = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaFamilias.Clear();
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
                                   + "fami.id_familia AS IdFamilia, "
                                   + "fami.familia AS NomeFamilia, "
                                   + "stat.id_status AS IdStatusFamilia, "
                                   + "stat.nome AS NomeStatusFamilia, "
                                   + "mode.id_modelo AS IdModelo, "
                                   + "mode.nome AS NomeModelo, "
                                   + "stat_mode.id_status AS IdStatusModelo, "
                                   + "stat_mode.nome AS NomeStatusModelo, "
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
                                   + "FROM tb_familias AS fami "
                                   + "LEFT JOIN tb_status AS stat ON stat.id_status = fami.id_status "
                                   + "LEFT JOIN tb_modelos AS mode ON mode.id_modelo = fami.id_modelo "
                                   + "LEFT JOIN tb_status AS stat_mode ON stat_mode.id_status = mode.id_status "
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
                                Familia item = new();

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo == null)
                                {
                                    item.Modelo = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Status == null)
                                {
                                    item.Modelo.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Fabricante == null)
                                {
                                    item.Modelo.Fabricante = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Fabricante.Status == null)
                                {
                                    item.Modelo.Fabricante.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.TipoEquipamento == null)
                                {
                                    item.Modelo.TipoEquipamento = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.TipoEquipamento.Status == null)
                                {
                                    item.Modelo.TipoEquipamento.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Categoria == null)
                                {
                                    item.Modelo.Categoria = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Categoria.Status == null)
                                {
                                    item.Modelo.Categoria.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Classe == null)
                                {
                                    item.Modelo.Classe = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Classe.Status == null)
                                {
                                    item.Modelo.Classe.Status = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFamilia"]);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFamilia"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFamilia"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFamilia"]);
                                item.Modelo.Id = FuncoesDeConversao.ConverteParaInt(reader["IdModelo"]);
                                item.Modelo.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeModelo"]);
                                item.Modelo.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusModelo"]);
                                item.Modelo.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusModelo"]);
                                item.Modelo.Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                item.Modelo.Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                item.Modelo.Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                item.Modelo.Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);
                                item.Modelo.TipoEquipamento.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoEquipamento"]);
                                item.Modelo.TipoEquipamento.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTipoEquipamento"]);
                                item.Modelo.TipoEquipamento.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTipoEquipamento"]);
                                item.Modelo.TipoEquipamento.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTipoEquipamento"]);
                                item.Modelo.Categoria.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCategoria"]);
                                item.Modelo.Categoria.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCategoria"]);
                                item.Modelo.Categoria.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCategoria"]);
                                item.Modelo.Categoria.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCategoria"]);
                                item.Modelo.Classe.Id = FuncoesDeConversao.ConverteParaInt(reader["IdClasse"]);
                                item.Modelo.Classe.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeClasse"]);
                                item.Modelo.Classe.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusClasse"]);
                                item.Modelo.Classe.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusClasse"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaFamilias.Add(item);

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
            Familia familiaCopia = new();

            familiaCopia.Id = Id;
            familiaCopia.Nome = Nome;
            familiaCopia.Status = Status == null ? new() : (Status)Status.Clone();
            familiaCopia.Modelo = Modelo == null ? new() : (Modelo)Modelo.Clone();

            return familiaCopia;
        }

        #endregion Interfaces
    }
}