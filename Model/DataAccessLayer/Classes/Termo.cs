using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class Termo : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private int? _idRetornado;
        private string? _nome;
        private string? _textoPadrao;
        private string? _observacoes;
        private string? _prazoEntrega;
        private string? _condicaoPagamento;
        private string? _garantia;
        private string? _validade;
        private string? _nomeAdicional;
        private Status? _status;

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

        public string? TextoPadrao
        {
            get { return _textoPadrao; }
            set
            {
                if (value != _textoPadrao)
                {
                    _textoPadrao = value;
                    OnPropertyChanged(nameof(TextoPadrao));
                }
            }
        }

        public string? Observacoes
        {
            get { return _observacoes; }
            set
            {
                if (value != _observacoes)
                {
                    _observacoes = value;
                    OnPropertyChanged(nameof(Observacoes));
                }
            }
        }

        public string? PrazoEntrega
        {
            get { return _prazoEntrega; }
            set
            {
                if (value != _prazoEntrega)
                {
                    _prazoEntrega = value;
                    OnPropertyChanged(nameof(PrazoEntrega));
                }
            }
        }

        public string? CondicaoPagamento
        {
            get { return _condicaoPagamento; }
            set
            {
                if (value != _condicaoPagamento)
                {
                    _condicaoPagamento = value;
                    OnPropertyChanged(nameof(CondicaoPagamento));
                }
            }
        }

        public string? Garantia
        {
            get { return _garantia; }
            set
            {
                if (value != _garantia)
                {
                    _garantia = value;
                    OnPropertyChanged(nameof(Garantia));
                }
            }
        }

        public string? Validade
        {
            get { return _validade; }
            set
            {
                if (value != _validade)
                {
                    _validade = value;
                    OnPropertyChanged(nameof(Validade));
                }
            }
        }
        
        public string? NomeAdicional 
        {
            get { return _nomeAdicional; } 
            set
            {
                if(value != _nomeAdicional)
                {
                    _nomeAdicional = value;
                    OnPropertyChanged(nameof(NomeAdicional));
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

        #endregion // Propriedades

        #region Construtores

        /// <summary>
        /// Construtor sem parâmetros que cria uma nova instância de todas as classes
        /// </summary>
        public Termo()
        {
            Status = new();
        }

        #endregion // Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados do termo com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id do termo que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetTermoDatabaseAsync(int? id, CancellationToken ct)
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
                                          + "term.id_termo AS IdTermo, "
                                          + "term.nome AS NomeTermo, "
                                          + "term.texto_padrao AS TextoPadrao, "
                                          + "term.observacoes AS Observacoes, "
                                          + "term.prazo_entrega AS PrazoEntrega, "
                                          + "term.condicao_pagamento AS CondicaoPagamento, "
                                          + "term.garantia AS Garantia, "
                                          + "term.validade AS Validade, "
                                          + "term.nome AS NomeAdicional, "
                                          + "stat.id_status AS IdStatusTermo, "
                                          + "stat.nome AS NomeStatusTermo "
                                          + "FROM tb_termos AS term "
                                          + "LEFT JOIN tb_status AS stat ON stat.id_status = term.id_status "
                                          + "WHERE term.id_termo = @id";

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

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdTermo"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTermo"]);
                                TextoPadrao = FuncoesDeConversao.ConverteParaString(reader["TextoPadrao"]);
                                Observacoes = FuncoesDeConversao.ConverteParaString(reader["Observacoes"]);
                                PrazoEntrega = FuncoesDeConversao.ConverteParaString(reader["PrazoEntrega"]);
                                CondicaoPagamento = FuncoesDeConversao.ConverteParaString(reader["CondicaoPagamento"]);
                                Garantia = FuncoesDeConversao.ConverteParaString(reader["Garantia"]);
                                Validade = FuncoesDeConversao.ConverteParaString(reader["Validade"]);
                                NomeAdicional = FuncoesDeConversao.ConverteParaString(reader["NomeAdicional"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTermo"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTermo"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados do termo com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetTermoDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                      + "term.id_termo AS IdTermo, "
                      + "term.nome AS NomeTermo, "
                      + "term.texto_padrao AS TextoPadrao, "
                      + "term.observacoes AS Observacoes, "
                      + "term.prazo_entrega AS PrazoEntrega, "
                      + "term.condicao_pagamento AS CondicaoPagamento, "
                      + "term.garantia AS Garantia, "
                      + "term.validade AS Validade, "
                      + "term.nome AS NomeAdicional, "
                      + "stat.id_status AS IdStatusTermo, "
                      + "stat.nome AS NomeStatusTermo "
                      + "FROM tb_termos AS term "
                      + "LEFT JOIN tb_status AS stat ON stat.id_status = term.id_status "
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

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdTermo"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTermo"]);
                                TextoPadrao = FuncoesDeConversao.ConverteParaString(reader["TextoPadrao"]);
                                Observacoes = FuncoesDeConversao.ConverteParaString(reader["Observacoes"]);
                                PrazoEntrega = FuncoesDeConversao.ConverteParaString(reader["PrazoEntrega"]);
                                CondicaoPagamento = FuncoesDeConversao.ConverteParaString(reader["CondicaoPagamento"]);
                                Garantia = FuncoesDeConversao.ConverteParaString(reader["Garantia"]);
                                Validade = FuncoesDeConversao.ConverteParaString(reader["Validade"]);
                                NomeAdicional = FuncoesDeConversao.ConverteParaString(reader["NomeAdicional"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTermo"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTermo"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva o termo na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarTermoDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_termo", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_texto_padrao", TextoPadrao);
                        command.Parameters.AddWithValue("p_observacoes", Observacoes);
                        command.Parameters.AddWithValue("p_prazo_entrega", PrazoEntrega);
                        command.Parameters.AddWithValue("p_condicao_pagamento", CondicaoPagamento);
                        command.Parameters.AddWithValue("p_garantia", Garantia);
                        command.Parameters.AddWithValue("p_validade", Validade);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add("p_id_termo", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException(nameof(Termo).ToLower(), Nome);
                        }

                        // Retorna o id da série
                        IdRetornado = FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_termo"].Value);
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_termo", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_termo", Id);
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_texto_padrao", TextoPadrao);
                        command.Parameters.AddWithValue("p_observacoes", Observacoes);
                        command.Parameters.AddWithValue("p_prazo_entrega", PrazoEntrega);
                        command.Parameters.AddWithValue("p_condicao_pagamento", CondicaoPagamento);
                        command.Parameters.AddWithValue("p_garantia", Garantia);
                        command.Parameters.AddWithValue("p_validade", Validade);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException(nameof(Termo).ToLower(), Nome);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta o termo na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarTermoDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_termo", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_termo", Id);
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
                            throw new ChaveEstrangeiraEmUsoException(nameof(Termo).ToLower(), Nome);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException(nameof(Termo).ToLower(), Nome);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de termos com os argumentos utilizados
        /// </summary>
        /// <param name="listaTermos">Representa a lista de termos que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaTermosAsync(ObservableCollection<Termo> listaTermos, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaTermos == null)
            {
                listaTermos = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaTermos.Clear();
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
                  + "term.id_termo AS IdTermo, "
                  + "term.nome AS NomeTermo, "
                  + "term.texto_padrao AS TextoPadrao, "
                  + "term.observacoes AS Observacoes, "
                  + "term.prazo_entrega AS PrazoEntrega, "
                  + "term.condicao_pagamento AS CondicaoPagamento, "
                  + "term.garantia AS Garantia, "
                  + "term.validade AS Validade, "
                  + "term.nome AS NomeAdicional, "
                  + "stat.id_status AS IdStatusTermo, "
                  + "stat.nome AS NomeStatusTermo "
                  + "FROM tb_termos AS term "
                  + "LEFT JOIN tb_status AS stat ON stat.id_status = term.id_status "
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
                                Termo item = new();

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTermo"]);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTermo"]);
                                item.TextoPadrao = FuncoesDeConversao.ConverteParaString(reader["TextoPadrao"]);
                                item.Observacoes = FuncoesDeConversao.ConverteParaString(reader["Observacoes"]);
                                item.PrazoEntrega = FuncoesDeConversao.ConverteParaString(reader["PrazoEntrega"]);
                                item.CondicaoPagamento = FuncoesDeConversao.ConverteParaString(reader["CondicaoPagamento"]);
                                item.Garantia = FuncoesDeConversao.ConverteParaString(reader["Garantia"]);
                                item.Validade = FuncoesDeConversao.ConverteParaString(reader["Validade"]);
                                item.NomeAdicional = FuncoesDeConversao.ConverteParaString(reader["NomeAdicional"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTermo"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTermo"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaTermos.Add(item);

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

        /// <summary>
        /// Método assíncrono que preenche uma lista de termos com argumentos genéricos
        /// </summary>
        /// <param name="listaTermos">Representa a lista de termos que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="querySelecao">Representa a query completa de seleção</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaTermosArgumentosGenericosAsync(ObservableCollection<Termo> listaTermos, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string querySelecao, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaTermos == null)
            {
                listaTermos = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaTermos.Clear();
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
                string comando = querySelecao;

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
                                Termo item = new();

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTermo"]);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTermo"]);
                                item.TextoPadrao = FuncoesDeConversao.ConverteParaString(reader["TextoPadrao"]);
                                item.Observacoes = FuncoesDeConversao.ConverteParaString(reader["Observacoes"]);
                                item.PrazoEntrega = FuncoesDeConversao.ConverteParaString(reader["PrazoEntrega"]);
                                item.CondicaoPagamento = FuncoesDeConversao.ConverteParaString(reader["CondicaoPagamento"]);
                                item.Garantia = FuncoesDeConversao.ConverteParaString(reader["Garantia"]);
                                item.Validade = FuncoesDeConversao.ConverteParaString(reader["Validade"]);
                                item.NomeAdicional = FuncoesDeConversao.ConverteParaString(reader["NomeAdicional"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTermo"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTermo"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaTermos.Add(item);

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
            Termo termoCopia = new();
            termoCopia.Id = Id;
            termoCopia.Nome = Nome;
            termoCopia.TextoPadrao = TextoPadrao;
            termoCopia.Observacoes = Observacoes;
            termoCopia.PrazoEntrega = PrazoEntrega;
            termoCopia.CondicaoPagamento = CondicaoPagamento;
            termoCopia.Garantia = Garantia;
            termoCopia.Validade = Validade;
            termoCopia.NomeAdicional = NomeAdicional;
            termoCopia.Status = Status == null ? new() : (Status)Status.Clone();

            return termoCopia;
        }

        #endregion // Interfaces
    }
}
