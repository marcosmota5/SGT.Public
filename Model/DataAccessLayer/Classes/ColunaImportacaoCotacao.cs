using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class ColunaImportacaoCotacao : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private string? _nomeColunaSistema;
        private string? _nomeColunaSistemaFormatado;
        private bool? _colunaExiste;
        private string? _nomeColunaCotacao;
        private Fornecedor? _fornecedor;

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
        
        public string? NomeColunaSistema
        {
            get { return _nomeColunaSistema; } 
            set
            {
                if(value != _nomeColunaSistema)
                {
                    _nomeColunaSistema = value;
                    OnPropertyChanged(nameof(NomeColunaSistema));
                }
            }
        }
        
        public string? NomeColunaSistemaFormatado 
        {
            get { return _nomeColunaSistemaFormatado; }
            set 
            {
                if(value != _nomeColunaSistemaFormatado)
                {
                    _nomeColunaSistemaFormatado = value;
                    OnPropertyChanged(nameof(NomeColunaSistemaFormatado));
                }
            }
        }
        
        public bool? ColunaExiste
        {
            get { return _colunaExiste; }
            set 
            {
                if(value != _colunaExiste)
                {
                    _colunaExiste = value;
                    OnPropertyChanged(nameof(ColunaExiste));
                }
            }
        }
        
        public string? NomeColunaCotacao 
        {
            get { return _nomeColunaCotacao; }
            set 
            {
                if(value != _nomeColunaCotacao)
                {
                    _nomeColunaCotacao = value;
                    OnPropertyChanged(nameof(NomeColunaCotacao));
                }
            }
        }
        
        public Fornecedor? Fornecedor 
        {
            get { return _fornecedor; } 
            set 
            {
                if(value != _fornecedor)
                {
                    _fornecedor = value;
                    OnPropertyChanged(nameof(Fornecedor));
                }
            }
        }

        #endregion // Propriedades

        #region Construtores

        /// <summary>
        /// Construtor sem parâmetros que cria uma nova instância de todas as classes
        /// </summary>
        public ColunaImportacaoCotacao()
        {
            Fornecedor = new();
        }

        #endregion // Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados da coluna de importação com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id da coluna de importação que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetColunaImportacaoCotacaoDatabaseAsync(int? id, CancellationToken ct)
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
                                          + "coic.id_coluna_importacao_cotacao AS IdColunaImportacaoCotacao, "
                                          + "coic.nome_coluna_sistema AS NomeColunaSistema, "
                                          + "coic.nome_coluna_sistema_formatado AS NomeColunaSistemaFormatado, "
                                          + "coic.coluna_existe AS ColunaExiste, "
                                          + "coic.nome_coluna_cotacao AS NomeColunaCotacao, "
                                          + "forn.id_fornecedor AS IdFornecedor, "
                                          + "forn.nome AS NomeFornecedor, "
                                          + "stat_forn.id_status AS IdStatusFornecedor, "
                                          + "stat_forn.nome AS NomeStatusFornecedor "
                                          + "FROM tb_colunas_importacao_cotacoes AS coic "
                                          + "LEFT JOIN tb_fornecedores AS forn ON forn.id_fornecedor = coic.id_fornecedor "
                                          + "LEFT JOIN tb_status AS stat_forn ON stat_forn.id_status = forn.id_status "
                                          + "WHERE coic.id_coluna_importacao_cotacao = @id";

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
                                if (Fornecedor == null)
                                {
                                    Fornecedor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Fornecedor.Status == null)
                                {
                                    Fornecedor.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdColunaImportacaoCotacao"]);
                                NomeColunaSistema = FuncoesDeConversao.ConverteParaString(reader["NomeColunaSistema"]);
                                NomeColunaSistemaFormatado = FuncoesDeConversao.ConverteParaString(reader["NomeColunaSistemaFormatado"]);
                                ColunaExiste = FuncoesDeConversao.ConverteParaBool(reader["ColunaExiste"]);
                                NomeColunaCotacao = FuncoesDeConversao.ConverteParaString(reader["NomeColunaCotacao"]);
                                Fornecedor.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFornecedor"]);
                                Fornecedor.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFornecedor"]);
                                Fornecedor.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFornecedor"]);
                                Fornecedor.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFornecedor"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados da coluna de importação com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetColunaImportacaoCotacaoDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Utilização da conexão
            using (var db = new ConexaoMySQL())
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

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
                      + "coic.id_coluna_importacao_cotacao AS IdColunaImportacaoCotacao, "
                      + "coic.nome_coluna_sistema AS NomeColunaSistema, "
                      + "coic.nome_coluna_sistema_formatado AS NomeColunaSistemaFormatado, "
                      + "coic.coluna_existe AS ColunaExiste, "
                      + "coic.nome_coluna_cotacao AS NomeColunaCotacao, "
                      + "forn.id_fornecedor AS IdFornecedor, "
                      + "forn.nome AS NomeFornecedor, "
                      + "stat_forn.id_status AS IdStatusFornecedor, "
                      + "stat_forn.nome AS NomeStatusFornecedor "
                      + "FROM tb_colunas_importacao_cotacoes AS coic "
                      + "LEFT JOIN tb_fornecedores AS forn ON forn.id_fornecedor = coic.id_fornecedor "
                      + "LEFT JOIN tb_status AS stat_forn ON stat_forn.id_status = forn.id_status "
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
                                if (Fornecedor == null)
                                {
                                    Fornecedor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Fornecedor.Status == null)
                                {
                                    Fornecedor.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdColunaImportacaoCotacao"]);
                                NomeColunaSistema = FuncoesDeConversao.ConverteParaString(reader["NomeColunaSistema"]);
                                NomeColunaSistemaFormatado = FuncoesDeConversao.ConverteParaString(reader["NomeColunaSistemaFormatado"]);
                                ColunaExiste = FuncoesDeConversao.ConverteParaBool(reader["ColunaExiste"]);
                                NomeColunaCotacao = FuncoesDeConversao.ConverteParaString(reader["NomeColunaCotacao"]);
                                Fornecedor.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFornecedor"]);
                                Fornecedor.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFornecedor"]);
                                Fornecedor.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFornecedor"]);
                                Fornecedor.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFornecedor"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva a coluna de importação na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarColunaImportacaoCotacaoDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_coluna_importacao_cotacao", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_nome_coluna_sistema", NomeColunaSistema);
                        command.Parameters.AddWithValue("p_nome_coluna_sistema_formatado", NomeColunaSistemaFormatado);
                        command.Parameters.AddWithValue("p_coluna_existe", ColunaExiste);
                        command.Parameters.AddWithValue("p_nome_coluna_cotacao", NomeColunaCotacao);
                        command.Parameters.AddWithValue("p_id_fornecedor", Fornecedor.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException("A coluna " + NomeColunaSistema + " já existe para o fornecedor " + Fornecedor.Nome + " na database");
                        }
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_coluna_importacao_cotacao", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_coluna_importacao_cotacao", Id);
                        command.Parameters.AddWithValue("p_nome_coluna_sistema", NomeColunaSistema);
                        command.Parameters.AddWithValue("p_nome_coluna_sistema_formatado", NomeColunaSistemaFormatado);
                        command.Parameters.AddWithValue("p_coluna_existe", ColunaExiste);
                        command.Parameters.AddWithValue("p_nome_coluna_cotacao", NomeColunaCotacao);
                        command.Parameters.AddWithValue("p_id_fornecedor", Fornecedor.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException("coluna", NomeColunaSistema);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta a coluna de importação na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarColunaImportacaoCotacaoDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_coluna_importacao_cotacao", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_coluna_importacao_cotacao", Id);
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
                            throw new ChaveEstrangeiraEmUsoException("coluna", NomeColunaSistema);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("coluna", NomeColunaSistema);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de colunas de importação com os argumentos utilizados
        /// </summary>
        /// <param name="listaColunasImportacaoCotacao">Representa a lista de colunas de importação que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaColunasImportacaoCotacaoAsync(ObservableCollection<ColunaImportacaoCotacao> listaColunasImportacaoCotacao, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaColunasImportacaoCotacao == null)
            {
                listaColunasImportacaoCotacao = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaColunasImportacaoCotacao.Clear();
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
                  + "coic.id_coluna_importacao_cotacao AS IdColunaImportacaoCotacao, "
                  + "coic.nome_coluna_sistema AS NomeColunaSistema, "
                  + "coic.nome_coluna_sistema_formatado AS NomeColunaSistemaFormatado, "
                  + "coic.coluna_existe AS ColunaExiste, "
                  + "coic.nome_coluna_cotacao AS NomeColunaCotacao, "
                  + "forn.id_fornecedor AS IdFornecedor, "
                  + "forn.nome AS NomeFornecedor, "
                  + "stat_forn.id_status AS IdStatusFornecedor, "
                  + "stat_forn.nome AS NomeStatusFornecedor "
                  + "FROM tb_colunas_importacao_cotacoes AS coic "
                  + "LEFT JOIN tb_fornecedores AS forn ON forn.id_fornecedor = coic.id_fornecedor "
                  + "LEFT JOIN tb_status AS stat_forn ON stat_forn.id_status = forn.id_status "
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
                                ColunaImportacaoCotacao item = new();

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Fornecedor == null)
                                {
                                    item.Fornecedor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Fornecedor.Status == null)
                                {
                                    item.Fornecedor.Status = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdColunaImportacaoCotacao"]);
                                item.NomeColunaSistema = FuncoesDeConversao.ConverteParaString(reader["NomeColunaSistema"]);
                                item.NomeColunaSistemaFormatado = FuncoesDeConversao.ConverteParaString(reader["NomeColunaSistemaFormatado"]);
                                item.ColunaExiste = FuncoesDeConversao.ConverteParaBool(reader["ColunaExiste"]);
                                item.NomeColunaCotacao = FuncoesDeConversao.ConverteParaString(reader["NomeColunaCotacao"]);
                                item.Fornecedor.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFornecedor"]);
                                item.Fornecedor.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFornecedor"]);
                                item.Fornecedor.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFornecedor"]);
                                item.Fornecedor.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFornecedor"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaColunasImportacaoCotacao.Add(item);

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
            ColunaImportacaoCotacao colunasImportacaoCotacoesCopia = new();
            colunasImportacaoCotacoesCopia.Id = Id;
            colunasImportacaoCotacoesCopia.NomeColunaSistema = NomeColunaSistema;
            colunasImportacaoCotacoesCopia.NomeColunaSistemaFormatado = NomeColunaSistemaFormatado;
            colunasImportacaoCotacoesCopia.ColunaExiste = ColunaExiste;
            colunasImportacaoCotacoesCopia.NomeColunaCotacao = NomeColunaCotacao;
            colunasImportacaoCotacoesCopia.Fornecedor = Fornecedor == null ? new() : (Fornecedor)Fornecedor.Clone();

            return colunasImportacaoCotacoesCopia;
        }

        #endregion // Interfaces
    }
}
