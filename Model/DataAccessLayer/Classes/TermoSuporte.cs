using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class TermoSuporte : ObservableObject, ICloneable
    {
        #region Campos

        private Termo? _termo;
        private Cliente? _cliente;
        private Setor? _setor;
        private Status? _status;

        #endregion // Campos

        #region Propriedades

        public Termo? Termo
        {
            get { return _termo; }
            set
            {
                if (value != _termo)
                {
                    _termo = value;
                    OnPropertyChanged(nameof(Termo));
                }
            }
        }

        public Cliente? Cliente
        {
            get { return _cliente; }
            set
            {
                if (value != _cliente)
                {
                    _cliente = value;
                    OnPropertyChanged(nameof(Cliente));
                }
            }
        }

        public Setor? Setor
        {
            get { return _setor; }
            set
            {
                if (value != _setor)
                {
                    _setor = value;
                    OnPropertyChanged(nameof(Setor));
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
        public TermoSuporte()
        {
            Termo = new();
            Cliente = new();
            Setor = new();
            Status = new();
        }

        #endregion // Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados do termo suporte com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id do termo suporte que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetTermoSuporteDatabaseAsync(int? id, CancellationToken ct)
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
                                          + "stat_term.id_status AS IdStatusTermo, "
                                          + "stat_term.nome AS NomeStatusTermo, "
                                          + "clie.id_cliente AS IdCliente, "
                                          + "clie.nome AS NomeCliente, "
                                          + "clie.considerar_percentuais_tabela_kion AS ConsiderarPercentuaisTabelaKion, "
                                          + "clie.percentual_tabela_kion_1 AS PercentualTabelaKion1, "
                                          + "clie.percentual_tabela_kion_2 AS PercentualTabelaKion2, "
                                          + "clie.percentual_tabela_kion_3 AS PercentualTabelaKion3, "
                                          + "clie.considerar_acrescimo_especifico AS ConsiderarAcrescimoEspecifico, "
                                          + "clie.percentual_acrescimo_especifico AS PercentualAcrescimoEspecifico, "
                                          + "stat_clie.id_status AS IdStatusCliente, "
                                          + "stat_clie.nome AS NomeStatusCliente, "
                                          + "seto.id_setor AS IdSetor, "
                                          + "seto.nome AS NomeSetor, "
                                          + "seto.prazo_adicional AS PrazoAdicional, "
                                          + "stat_seto.id_status AS IdStatusSetor, "
                                          + "stat_seto.nome AS NomeStatusSetor "
                                          + "FROM tb_termos_suporte AS term_sup "
                                          + "LEFT JOIN tb_termos AS term ON term_sup.id_termo = term.id_termo "
                                          + "LEFT JOIN tb_status AS stat_term ON stat_term.id_status = term.id_status "
                                          + "LEFT JOIN tb_clientes AS clie ON term_sup.id_cliente = clie.id_cliente "
                                          + "LEFT JOIN tb_status AS stat_clie ON stat_clie.id_status = clie.id_status "
                                          + "LEFT JOIN tb_setores AS seto ON term_sup.id_setor = seto.id_setor "
                                          + "LEFT JOIN tb_status AS stat_seto ON stat_seto.id_status = seto.id_status "
                                          + "LEFT JOIN tb_status AS stat_term_sup ON term_sup.id_status = stat_term_sup.id_status "
                                          + "WHERE term_sup.id_termo = @id";

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
                                if (Termo == null)
                                {
                                    Termo = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cliente == null)
                                {
                                    Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Setor == null)
                                {
                                    Setor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Status == null)
                                {
                                    Status = new();
                                }

                                // Define as propriedades
                                Termo.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTermo"]);
                                Termo.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTermo"]);
                                Termo.TextoPadrao = FuncoesDeConversao.ConverteParaString(reader["TextoPadrao"]);
                                Termo.Observacoes = FuncoesDeConversao.ConverteParaString(reader["Observacoes"]);
                                Termo.PrazoEntrega = FuncoesDeConversao.ConverteParaString(reader["PrazoEntrega"]);
                                Termo.CondicaoPagamento = FuncoesDeConversao.ConverteParaString(reader["CondicaoPagamento"]);
                                Termo.Garantia = FuncoesDeConversao.ConverteParaString(reader["Garantia"]);
                                Termo.Validade = FuncoesDeConversao.ConverteParaString(reader["Validade"]);
                                Termo.NomeAdicional = FuncoesDeConversao.ConverteParaString(reader["NomeAdicional"]);
                                Termo.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTermo"]);
                                Termo.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTermo"]);

                                Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                Cliente.ConsiderarPercentuaisTabelaKion = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarPercentuaisTabelaKion"]);
                                Cliente.PercentualTabelaKion1 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion1"]);
                                Cliente.PercentualTabelaKion2 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion2"]);
                                Cliente.PercentualTabelaKion3 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion3"]);
                                Cliente.ConsiderarAcrescimoEspecifico = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarAcrescimoEspecifico"]);
                                Cliente.PercentualAcrescimoEspecifico = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoEspecifico"]);
                                Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);

                                Setor.Id = FuncoesDeConversao.ConverteParaInt(reader["IdSetor"]);
                                Setor.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeSetor"]);
                                Setor.PrazoAdicional = FuncoesDeConversao.ConverteParaInt(reader["PrazoAdicional"]);
                                Setor.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSetor"]);
                                Setor.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSetor"]);

                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSetor"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSetor"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados do termo suporte com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetTermoSuporteDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                                     + "stat_term.id_status AS IdStatusTermo, "
                                     + "stat_term.nome AS NomeStatusTermo, "
                                     + "clie.id_cliente AS IdCliente, "
                                     + "clie.nome AS NomeCliente, "
                                     + "clie.considerar_percentuais_tabela_kion AS ConsiderarPercentuaisTabelaKion, "
                                     + "clie.percentual_tabela_kion_1 AS PercentualTabelaKion1, "
                                     + "clie.percentual_tabela_kion_2 AS PercentualTabelaKion2, "
                                     + "clie.percentual_tabela_kion_3 AS PercentualTabelaKion3, "
                                     + "clie.considerar_acrescimo_especifico AS ConsiderarAcrescimoEspecifico, "
                                     + "clie.percentual_acrescimo_especifico AS PercentualAcrescimoEspecifico, "
                                     + "stat_clie.id_status AS IdStatusCliente, "
                                     + "stat_clie.nome AS NomeStatusCliente, "
                                     + "seto.id_setor AS IdSetor, "
                                     + "seto.nome AS NomeSetor, "
                                     + "seto.prazo_adicional AS PrazoAdicional, "
                                     + "stat_seto.id_status AS IdStatusSetor, "
                                     + "stat_seto.nome AS NomeStatusSetor "
                                     + "FROM tb_termos_suporte AS term_sup "
                                     + "LEFT JOIN tb_termos AS term ON term_sup.id_termo = term.id_termo "
                                     + "LEFT JOIN tb_status AS stat_term ON stat_term.id_status = term.id_status "
                                     + "LEFT JOIN tb_clientes AS clie ON term_sup.id_cliente = clie.id_cliente "
                                     + "LEFT JOIN tb_status AS stat_clie ON stat_clie.id_status = clie.id_status "
                                     + "LEFT JOIN tb_setores AS seto ON term_sup.id_setor = seto.id_setor "
                                     + "LEFT JOIN tb_status AS stat_seto ON stat_seto.id_status = seto.id_status "
                                     + "LEFT JOIN tb_status AS stat_term_sup ON term_sup.id_status = stat_term_sup.id_status "
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
                                if (Termo == null)
                                {
                                    Termo = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cliente == null)
                                {
                                    Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Setor == null)
                                {
                                    Setor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Status == null)
                                {
                                    Status = new();
                                }

                                // Define as propriedades
                                Termo.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTermo"]);
                                Termo.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTermo"]);
                                Termo.TextoPadrao = FuncoesDeConversao.ConverteParaString(reader["TextoPadrao"]);
                                Termo.Observacoes = FuncoesDeConversao.ConverteParaString(reader["Observacoes"]);
                                Termo.PrazoEntrega = FuncoesDeConversao.ConverteParaString(reader["PrazoEntrega"]);
                                Termo.CondicaoPagamento = FuncoesDeConversao.ConverteParaString(reader["CondicaoPagamento"]);
                                Termo.Garantia = FuncoesDeConversao.ConverteParaString(reader["Garantia"]);
                                Termo.Validade = FuncoesDeConversao.ConverteParaString(reader["Validade"]);
                                Termo.NomeAdicional = FuncoesDeConversao.ConverteParaString(reader["NomeAdicional"]);
                                Termo.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTermo"]);
                                Termo.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTermo"]);

                                Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                Cliente.ConsiderarPercentuaisTabelaKion = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarPercentuaisTabelaKion"]);
                                Cliente.PercentualTabelaKion1 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion1"]);
                                Cliente.PercentualTabelaKion2 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion2"]);
                                Cliente.PercentualTabelaKion3 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion3"]);
                                Cliente.ConsiderarAcrescimoEspecifico = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarAcrescimoEspecifico"]);
                                Cliente.PercentualAcrescimoEspecifico = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoEspecifico"]);
                                Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);

                                Setor.Id = FuncoesDeConversao.ConverteParaInt(reader["IdSetor"]);
                                Setor.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeSetor"]);
                                Setor.PrazoAdicional = FuncoesDeConversao.ConverteParaInt(reader["PrazoAdicional"]);
                                Setor.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSetor"]);
                                Setor.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSetor"]);

                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSetor"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSetor"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva o termo suporte na database
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public async Task SalvarTermoSuporteDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_inserir_termo_suporte", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_termo", Termo.Id);
                    command.Parameters.AddWithValue("p_id_cliente", Cliente?.Id);
                    command.Parameters.AddWithValue("p_id_setor", Setor?.Id);
                    command.Parameters.AddWithValue("p_id_status", Status.Id);
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta o termo suporte na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarTermoSuporteDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_termo_suporte", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_termo", Termo.Id);
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
                            throw new ChaveEstrangeiraEmUsoException(nameof(TermoSuporte).ToLower(), Termo.Nome);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    //if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    //{
                    //    throw new ValorNaoExisteException(nameof(TermoSuporte).ToLower(), Termo.Nome);
                    //}
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de termos suporte com os argumentos utilizados
        /// </summary>
        /// <param name="listaTermosSuporte">Representa a lista de termos suporte que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaTermosSuporteAsync(ObservableCollection<TermoSuporte> listaTermosSuporte, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaTermosSuporte == null)
            {
                listaTermosSuporte = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaTermosSuporte.Clear();
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
                                 + "stat_term.id_status AS IdStatusTermo, "
                                 + "stat_term.nome AS NomeStatusTermo, "
                                 + "clie.id_cliente AS IdCliente, "
                                 + "clie.nome AS NomeCliente, "
                                 + "clie.considerar_percentuais_tabela_kion AS ConsiderarPercentuaisTabelaKion, "
                                 + "clie.percentual_tabela_kion_1 AS PercentualTabelaKion1, "
                                 + "clie.percentual_tabela_kion_2 AS PercentualTabelaKion2, "
                                 + "clie.percentual_tabela_kion_3 AS PercentualTabelaKion3, "
                                 + "clie.considerar_acrescimo_especifico AS ConsiderarAcrescimoEspecifico, "
                                 + "clie.percentual_acrescimo_especifico AS PercentualAcrescimoEspecifico, "
                                 + "stat_clie.id_status AS IdStatusCliente, "
                                 + "stat_clie.nome AS NomeStatusCliente, "
                                 + "seto.id_setor AS IdSetor, "
                                 + "seto.nome AS NomeSetor, "
                                 + "seto.prazo_adicional AS PrazoAdicional, "
                                 + "stat_seto.id_status AS IdStatusSetor, "
                                 + "stat_seto.nome AS NomeStatusSetor "
                                 + "FROM tb_termos_suporte AS term_sup "
                                 + "LEFT JOIN tb_termos AS term ON term_sup.id_termo = term.id_termo "
                                 + "LEFT JOIN tb_status AS stat_term ON stat_term.id_status = term.id_status "
                                 + "LEFT JOIN tb_clientes AS clie ON term_sup.id_cliente = clie.id_cliente "
                                 + "LEFT JOIN tb_status AS stat_clie ON stat_clie.id_status = clie.id_status "
                                 + "LEFT JOIN tb_setores AS seto ON term_sup.id_setor = seto.id_setor "
                                 + "LEFT JOIN tb_status AS stat_seto ON stat_seto.id_status = seto.id_status "
                                 + "LEFT JOIN tb_status AS stat_term_sup ON term_sup.id_status = stat_term_sup.id_status "
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
                                TermoSuporte item = new();

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Termo == null)
                                {
                                    item.Termo = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Cliente == null)
                                {
                                    item.Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Setor == null)
                                {
                                    item.Setor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Define as propriedades
                                item.Termo.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTermo"]);
                                item.Termo.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTermo"]);
                                item.Termo.TextoPadrao = FuncoesDeConversao.ConverteParaString(reader["TextoPadrao"]);
                                item.Termo.Observacoes = FuncoesDeConversao.ConverteParaString(reader["Observacoes"]);
                                item.Termo.PrazoEntrega = FuncoesDeConversao.ConverteParaString(reader["PrazoEntrega"]);
                                item.Termo.CondicaoPagamento = FuncoesDeConversao.ConverteParaString(reader["CondicaoPagamento"]);
                                item.Termo.Garantia = FuncoesDeConversao.ConverteParaString(reader["Garantia"]);
                                item.Termo.Validade = FuncoesDeConversao.ConverteParaString(reader["Validade"]);
                                item.Termo.NomeAdicional = FuncoesDeConversao.ConverteParaString(reader["NomeAdicional"]);
                                item.Termo.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTermo"]);
                                item.Termo.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTermo"]);

                                item.Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                item.Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                item.Cliente.ConsiderarPercentuaisTabelaKion = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarPercentuaisTabelaKion"]);
                                item.Cliente.PercentualTabelaKion1 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion1"]);
                                item.Cliente.PercentualTabelaKion2 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion2"]);
                                item.Cliente.PercentualTabelaKion3 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion3"]);
                                item.Cliente.ConsiderarAcrescimoEspecifico = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarAcrescimoEspecifico"]);
                                item.Cliente.PercentualAcrescimoEspecifico = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoEspecifico"]);
                                item.Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                item.Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);

                                item.Setor.Id = FuncoesDeConversao.ConverteParaInt(reader["IdSetor"]);
                                item.Setor.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeSetor"]);
                                item.Setor.PrazoAdicional = FuncoesDeConversao.ConverteParaInt(reader["PrazoAdicional"]);
                                item.Setor.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSetor"]);
                                item.Setor.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSetor"]);

                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSetor"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSetor"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaTermosSuporte.Add(item);

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
        /// Método assíncrono que preenche uma lista de termos suporte com argumentos genéricos
        /// </summary>
        /// <param name="listaTermosSuporte">Representa a lista de termos suporte que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="querySelecao">Representa a query completa de seleção</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaTermosSuporteArgumentosGenericosAsync(ObservableCollection<TermoSuporte> listaTermosSuporte, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string querySelecao, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaTermosSuporte == null)
            {
                listaTermosSuporte = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaTermosSuporte.Clear();
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
                                TermoSuporte item = new();

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Termo == null)
                                {
                                    item.Termo = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Cliente == null)
                                {
                                    item.Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Setor == null)
                                {
                                    item.Setor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Define as propriedades
                                item.Termo.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTermo"]);
                                item.Termo.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTermo"]);
                                item.Termo.TextoPadrao = FuncoesDeConversao.ConverteParaString(reader["TextoPadrao"]);
                                item.Termo.Observacoes = FuncoesDeConversao.ConverteParaString(reader["Observacoes"]);
                                item.Termo.PrazoEntrega = FuncoesDeConversao.ConverteParaString(reader["PrazoEntrega"]);
                                item.Termo.CondicaoPagamento = FuncoesDeConversao.ConverteParaString(reader["CondicaoPagamento"]);
                                item.Termo.Garantia = FuncoesDeConversao.ConverteParaString(reader["Garantia"]);
                                item.Termo.Validade = FuncoesDeConversao.ConverteParaString(reader["Validade"]);
                                item.Termo.NomeAdicional = FuncoesDeConversao.ConverteParaString(reader["NomeAdicional"]);
                                item.Termo.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTermo"]);
                                item.Termo.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTermo"]);

                                item.Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                item.Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                item.Cliente.ConsiderarPercentuaisTabelaKion = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarPercentuaisTabelaKion"]);
                                item.Cliente.PercentualTabelaKion1 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion1"]);
                                item.Cliente.PercentualTabelaKion2 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion2"]);
                                item.Cliente.PercentualTabelaKion3 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion3"]);
                                item.Cliente.ConsiderarAcrescimoEspecifico = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarAcrescimoEspecifico"]);
                                item.Cliente.PercentualAcrescimoEspecifico = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoEspecifico"]);
                                item.Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                item.Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);

                                item.Setor.Id = FuncoesDeConversao.ConverteParaInt(reader["IdSetor"]);
                                item.Setor.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeSetor"]);
                                item.Setor.PrazoAdicional = FuncoesDeConversao.ConverteParaInt(reader["PrazoAdicional"]);
                                item.Setor.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSetor"]);
                                item.Setor.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSetor"]);

                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSetor"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSetor"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaTermosSuporte.Add(item);

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
            TermoSuporte termoSuporteCopia = new();
            termoSuporteCopia.Termo = Termo == null ? new() : (Termo)Termo.Clone();
            termoSuporteCopia.Cliente = Cliente == null ? new() : (Cliente)Cliente.Clone();
            termoSuporteCopia.Setor = Setor == null ? new() : (Setor)Setor.Clone();
            termoSuporteCopia.Status = Status == null ? new() : (Status)Status.Clone();

            return termoSuporteCopia;
        }

        #endregion // Interfaces
    }
}
