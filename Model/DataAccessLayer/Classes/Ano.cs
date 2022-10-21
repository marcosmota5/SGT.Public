using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;
using System.Collections.ObjectModel;

namespace Model.DataAccessLayer.Classes
{
    public class Ano : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private int? _posicaoInicioCaracteres;
        private string? _caracteres;
        private int? _anoValor;
        private Status? _status;
        private Fabricante? _fabricante;

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

        public int? PosicaoInicioCaracteres
        {
            get { return _posicaoInicioCaracteres; }
            set
            {
                if (value != _posicaoInicioCaracteres)
                {
                    _posicaoInicioCaracteres = value;
                    OnPropertyChanged(nameof(PosicaoInicioCaracteres));
                }
            }
        }

        public string? Caracteres
        {
            get { return _caracteres; }
            set
            {
                if (value != _caracteres)
                {
                    _caracteres = value;
                    OnPropertyChanged(nameof(Caracteres));
                }
            }
        }

        public int? AnoValor
        {
            get { return _anoValor; }
            set
            {
                if (value != _anoValor)
                {
                    _anoValor = value;
                    OnPropertyChanged(nameof(AnoValor));
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

        #endregion Propriedades

        #region Construtores

        /// <summary>
        /// Construtor sem parâmetros que cria uma nova instância de todas as classes
        /// </summary>
        public Ano()
        {
            // Cria uma nova instância das classes
            Status = new();
            Fabricante = new();
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados do ano com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id do ano que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetAnoDatabaseAsync(int? id, CancellationToken ct)
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
                                          + "t_ano.id_ano AS IdAno, "
                                          + "t_ano.posicao_inicio_caracteres AS PosicaoInicioCaracteres, "
                                          + "t_ano.caracteres AS Caracteres, "
                                          + "t_ano.ano AS AnoValor, "
                                          + "stat.id_status AS IdStatusAno, "
                                          + "stat.nome AS NomeStatusAno, "
                                          + "fabr.id_fabricante AS IdFabricante, "
                                          + "fabr.nome AS NomeFabricante, "
                                          + "stat_fabr.id_status AS IdStatusFabricante, "
                                          + "stat_fabr.nome AS NomeStatusFabricante "
                                          + "FROM tb_anos AS t_ano "
                                          + "LEFT JOIN tb_status AS stat ON stat.id_status = t_ano.id_status "
                                          + "LEFT JOIN tb_fabricantes AS fabr ON fabr.id_fabricante = t_ano.id_fabricante "
                                          + "LEFT JOIN tb_status AS stat_fabr ON stat_fabr.id_status = fabr.id_status "
                                          + "WHERE t_ano.id_ano = @id";

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

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdAno"]);
                                PosicaoInicioCaracteres = FuncoesDeConversao.ConverteParaInt(reader["PosicaoInicioCaracteres"]);
                                Caracteres = FuncoesDeConversao.ConverteParaString(reader["Caracteres"]);
                                AnoValor = FuncoesDeConversao.ConverteParaInt(reader["AnoValor"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusAno"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusAno"]);
                                Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados do ano com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetAnoDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                      + "t_ano.id_ano AS IdAno, "
                      + "t_ano.posicao_inicio_caracteres AS PosicaoInicioCaracteres, "
                      + "t_ano.caracteres AS Caracteres, "
                      + "t_ano.ano AS AnoValor, "
                      + "stat.id_status AS IdStatusAno, "
                      + "stat.nome AS NomeStatusAno, "
                      + "fabr.id_fabricante AS IdFabricante, "
                      + "fabr.nome AS NomeFabricante, "
                      + "stat_fabr.id_status AS IdStatusFabricante, "
                      + "stat_fabr.nome AS NomeStatusFabricante "
                      + "FROM tb_anos AS t_ano "
                      + "LEFT JOIN tb_status AS stat ON stat.id_status = t_ano.id_status "
                      + "LEFT JOIN tb_fabricantes AS fabr ON fabr.id_fabricante = t_ano.id_fabricante "
                      + "LEFT JOIN tb_status AS stat_fabr ON stat_fabr.id_status = fabr.id_status "
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

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdAno"]);
                                PosicaoInicioCaracteres = FuncoesDeConversao.ConverteParaInt(reader["PosicaoInicioCaracteres"]);
                                Caracteres = FuncoesDeConversao.ConverteParaString(reader["Caracteres"]);
                                AnoValor = FuncoesDeConversao.ConverteParaInt(reader["AnoValor"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusAno"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusAno"]);
                                Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva o ano na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarAnoDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_ano", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_id_fabricante", Fabricante.Id);
                        command.Parameters.AddWithValue("p_posicao_inicio_caracteres", PosicaoInicioCaracteres);
                        command.Parameters.AddWithValue("p_caracteres", Caracteres);
                        command.Parameters.AddWithValue("p_ano", AnoValor);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException(nameof(Ano).ToLower(), AnoValor);
                        }
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_ano", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_ano", Id);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_id_fabricante", Fabricante.Id);
                        command.Parameters.AddWithValue("p_posicao_inicio_caracteres", PosicaoInicioCaracteres);
                        command.Parameters.AddWithValue("p_caracteres", Caracteres);
                        command.Parameters.AddWithValue("p_ano", AnoValor);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException(nameof(Ano).ToLower(), AnoValor);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta o ano na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarAnoDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_ano", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_ano", Id);
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
                            throw new ChaveEstrangeiraEmUsoException(nameof(Ano).ToLower(), AnoValor);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException(nameof(Ano).ToLower(), AnoValor);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de anos com os argumentos utilizados
        /// </summary>
        /// <param name="listaAnos">Representa a lista de anos que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaAnosAsync(ObservableCollection<Ano> listaAnos, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaAnos == null)
            {
                listaAnos = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaAnos.Clear();
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
                  + "t_ano.id_ano AS IdAno, "
                  + "t_ano.posicao_inicio_caracteres AS PosicaoInicioCaracteres, "
                  + "t_ano.caracteres AS Caracteres, "
                  + "t_ano.ano AS AnoValor, "
                  + "stat.id_status AS IdStatusAno, "
                  + "stat.nome AS NomeStatusAno, "
                  + "fabr.id_fabricante AS IdFabricante, "
                  + "fabr.nome AS NomeFabricante, "
                  + "stat_fabr.id_status AS IdStatusFabricante, "
                  + "stat_fabr.nome AS NomeStatusFabricante "
                  + "FROM tb_anos AS t_ano "
                  + "LEFT JOIN tb_status AS stat ON stat.id_status = t_ano.id_status "
                  + "LEFT JOIN tb_fabricantes AS fabr ON fabr.id_fabricante = t_ano.id_fabricante "
                  + "LEFT JOIN tb_status AS stat_fabr ON stat_fabr.id_status = fabr.id_status "
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
                                Ano item = new();

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

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdAno"]);
                                item.PosicaoInicioCaracteres = FuncoesDeConversao.ConverteParaInt(reader["PosicaoInicioCaracteres"]);
                                item.Caracteres = FuncoesDeConversao.ConverteParaString(reader["Caracteres"]);
                                item.AnoValor = FuncoesDeConversao.ConverteParaInt(reader["AnoValor"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusAno"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusAno"]);
                                item.Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                item.Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                item.Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                item.Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaAnos.Add(item);

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
            Ano anoCopia = new();

            anoCopia.Id = Id;
            anoCopia.PosicaoInicioCaracteres = PosicaoInicioCaracteres;
            anoCopia.Caracteres = Caracteres;
            anoCopia.AnoValor = AnoValor;
            anoCopia.Status = Status == null ? new() : (Status)Status.Clone();
            anoCopia.Fabricante = Fabricante == null ? new() : (Fabricante)Fabricante.Clone();

            return anoCopia;
        }

        #endregion Interfaces
    }
}