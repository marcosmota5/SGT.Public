using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class ConfiguracaoSistema : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private string? _localArquivoEstoque;
        private string? _nomeArquivoEstoque;
        private string? _abaArquivoEstoque;
        private string? _senhaArquivoEstoque;
        private string? _localOrdensServico;
        private bool? _permiteLembrarPreenchimentoDepois;

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

        public string? LocalArquivoEstoque
        {
            get { return _localArquivoEstoque; }
            set
            {
                if (value != _localArquivoEstoque)
                {
                    _localArquivoEstoque = value;
                    OnPropertyChanged(nameof(LocalArquivoEstoque));
                }
            }
        }

        public string? NomeArquivoEstoque
        {
            get { return _nomeArquivoEstoque; }
            set
            {
                if (value != _nomeArquivoEstoque)
                {
                    _nomeArquivoEstoque = value;
                    OnPropertyChanged(nameof(NomeArquivoEstoque));
                }
            }
        }

        public string? AbaArquivoEstoque
        {
            get { return _abaArquivoEstoque; }
            set
            {
                if (value != _abaArquivoEstoque)
                {
                    _abaArquivoEstoque = value;
                    OnPropertyChanged(nameof(AbaArquivoEstoque));
                }
            }
        }

        public string? SenhaArquivoEstoque
        {
            get { return _senhaArquivoEstoque; }
            set
            {
                if (value != _senhaArquivoEstoque)
                {
                    _senhaArquivoEstoque = value;
                    OnPropertyChanged(nameof(SenhaArquivoEstoque));
                }
            }
        }

        public string? LocalOrdensServico
        {
            get { return _localOrdensServico; }
            set
            {
                if (value != _localOrdensServico)
                {
                    _localOrdensServico = value;
                    OnPropertyChanged(nameof(LocalOrdensServico));
                }
            }
        }

        public bool? PermiteLembrarPreenchimentoDepois
        {
            get { return _permiteLembrarPreenchimentoDepois; }
            set
            {
                if (value != _permiteLembrarPreenchimentoDepois)
                {
                    _permiteLembrarPreenchimentoDepois = value;
                    OnPropertyChanged(nameof(PermiteLembrarPreenchimentoDepois));
                }
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados da configuração do sistema com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id da configuração do sistema que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetConfiguracaoSistemaDatabaseAsync(int? id, CancellationToken ct)
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
                                          + "cosi.id_configuracao AS IdConfiguracaoSistema, "
                                          + "cosi.local_arquivo_estoque AS LocalArquivoEstoqueConfiguracaoSistema, "
                                          + "cosi.nome_arquivo_estoque AS NomeArquivoEstoqueConfiguracaoSistema, "
                                          + "cosi.aba_arquivo_estoque AS AbaArquivoEstoqueConfiguracaoSistema, "
                                          + "cosi.senha_arquivo_estoque AS SenhaArquivoEstoqueConfiguracaoSistema, "
                                          + "cosi.local_ordens_servico AS LocalOrdensServicoConfiguracaoSistema, "
                                          + "cosi.permite_lembrar_preenchimento_depois AS PermiteLembrarPreenchimentoDepoisConfiguracaoSistema "
                                          + "FROM tb_configuracoes_sistema AS cosi "
                                          + "WHERE cosi.id_configuracao = @id";

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
                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdConfiguracaoSistema"]);
                                LocalArquivoEstoque = FuncoesDeConversao.ConverteParaString(reader["LocalArquivoEstoqueConfiguracaoSistema"]);
                                NomeArquivoEstoque = FuncoesDeConversao.ConverteParaString(reader["NomeArquivoEstoqueConfiguracaoSistema"]);
                                AbaArquivoEstoque = FuncoesDeConversao.ConverteParaString(reader["AbaArquivoEstoqueConfiguracaoSistema"]);
                                SenhaArquivoEstoque = FuncoesDeConversao.ConverteParaString(reader["SenhaArquivoEstoqueConfiguracaoSistema"]);
                                LocalOrdensServico = FuncoesDeConversao.ConverteParaString(reader["LocalOrdensServicoConfiguracaoSistema"]);
                                PermiteLembrarPreenchimentoDepois = FuncoesDeConversao.ConverteParaBool(reader["PermiteLembrarPreenchimentoDepoisConfiguracaoSistema"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados da configuração do sistema com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetConfiguracaoSistemaDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                                    + "cosi.id_configuracao AS IdConfiguracaoSistema, "
                                    + "cosi.local_arquivo_estoque AS LocalArquivoEstoqueConfiguracaoSistema, "
                                    + "cosi.nome_arquivo_estoque AS NomeArquivoEstoqueConfiguracaoSistema, "
                                    + "cosi.aba_arquivo_estoque AS AbaArquivoEstoqueConfiguracaoSistema, "
                                    + "cosi.senha_arquivo_estoque AS SenhaArquivoEstoqueConfiguracaoSistema, "
                                    + "cosi.local_ordens_servico AS LocalOrdensServicoConfiguracaoSistema, "
                                    + "cosi.permite_lembrar_preenchimento_depois AS PermiteLembrarPreenchimentoDepoisConfiguracaoSistema "
                                    + "FROM tb_configuracoes_sistema AS cosi "
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
                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdConfiguracaoSistema"]);
                                LocalArquivoEstoque = FuncoesDeConversao.ConverteParaString(reader["LocalArquivoEstoqueConfiguracaoSistema"]);
                                NomeArquivoEstoque = FuncoesDeConversao.ConverteParaString(reader["NomeArquivoEstoqueConfiguracaoSistema"]);
                                AbaArquivoEstoque = FuncoesDeConversao.ConverteParaString(reader["AbaArquivoEstoqueConfiguracaoSistema"]);
                                SenhaArquivoEstoque = FuncoesDeConversao.ConverteParaString(reader["SenhaArquivoEstoqueConfiguracaoSistema"]);
                                LocalOrdensServico = FuncoesDeConversao.ConverteParaString(reader["LocalOrdensServicoConfiguracaoSistema"]);
                                PermiteLembrarPreenchimentoDepois = FuncoesDeConversao.ConverteParaBool(reader["PermiteLembrarPreenchimentoDepoisConfiguracaoSistema"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva a configuração do sistema na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarConfiguracaoSistemaDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_editar_configuracao_sistema", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_configuracao", Id);
                    command.Parameters.AddWithValue("p_local_arquivo_estoque", LocalArquivoEstoque);
                    command.Parameters.AddWithValue("p_nome_arquivo_estoque", NomeArquivoEstoque);
                    command.Parameters.AddWithValue("p_aba_arquivo_estoque", AbaArquivoEstoque);
                    command.Parameters.AddWithValue("p_senha_arquivo_estoque", SenhaArquivoEstoque);
                    command.Parameters.AddWithValue("p_local_ordens_servico", LocalOrdensServico);
                    command.Parameters.AddWithValue("p_permite_lembrar_preenchimento_depois", PermiteLembrarPreenchimentoDepois);
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException(nameof(ConfiguracaoSistema).ToLower(), LocalArquivoEstoque);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de configurações do sistema com os argumentos utilizados
        /// </summary>
        /// <param name="listaConfiguracaoSistemas">Representa a lista de configurações do sistema que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaConfiguracaoSistemasAsync(ObservableCollection<ConfiguracaoSistema> listaConfiguracaoSistemas, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaConfiguracaoSistemas == null)
            {
                listaConfiguracaoSistemas = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaConfiguracaoSistemas.Clear();
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
                                    + "cosi.id_configuracao AS IdConfiguracaoSistema, "
                                    + "cosi.local_arquivo_estoque AS LocalArquivoEstoqueConfiguracaoSistema, "
                                    + "cosi.nome_arquivo_estoque AS NomeArquivoEstoqueConfiguracaoSistema, "
                                    + "cosi.aba_arquivo_estoque AS AbaArquivoEstoqueConfiguracaoSistema, "
                                    + "cosi.senha_arquivo_estoque AS SenhaArquivoEstoqueConfiguracaoSistema, "
                                    + "cosi.local_ordens_servico AS LocalOrdensServicoConfiguracaoSistema, "
                                    + "cosi.permite_lembrar_preenchimento_depois AS PermiteLembrarPreenchimentoDepoisConfiguracaoSistema "
                                    + "FROM tb_configuracoes_sistema AS cosi "
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
                                ConfiguracaoSistema item = new();

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdConfiguracaoSistema"]);
                                item.LocalArquivoEstoque = FuncoesDeConversao.ConverteParaString(reader["LocalArquivoEstoqueConfiguracaoSistema"]);
                                item.NomeArquivoEstoque = FuncoesDeConversao.ConverteParaString(reader["NomeArquivoEstoqueConfiguracaoSistema"]);
                                item.AbaArquivoEstoque = FuncoesDeConversao.ConverteParaString(reader["AbaArquivoEstoqueConfiguracaoSistema"]);
                                item.SenhaArquivoEstoque = FuncoesDeConversao.ConverteParaString(reader["SenhaArquivoEstoqueConfiguracaoSistema"]);
                                item.LocalOrdensServico = FuncoesDeConversao.ConverteParaString(reader["LocalOrdensServicoConfiguracaoSistema"]);
                                item.PermiteLembrarPreenchimentoDepois = FuncoesDeConversao.ConverteParaBool(reader["PermiteLembrarPreenchimentoDepoisConfiguracaoSistema"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaConfiguracaoSistemas.Add(item);

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
            ConfiguracaoSistema ConfiguracaoSistemaCopia = new();
            ConfiguracaoSistemaCopia.Id = Id;
            ConfiguracaoSistemaCopia.LocalArquivoEstoque = LocalArquivoEstoque;
            ConfiguracaoSistemaCopia.NomeArquivoEstoque = NomeArquivoEstoque;
            ConfiguracaoSistemaCopia.AbaArquivoEstoque = AbaArquivoEstoque;
            ConfiguracaoSistemaCopia.SenhaArquivoEstoque = SenhaArquivoEstoque;
            ConfiguracaoSistemaCopia.LocalOrdensServico = LocalOrdensServico;
            ConfiguracaoSistemaCopia.PermiteLembrarPreenchimentoDepois = PermiteLembrarPreenchimentoDepois;

            return ConfiguracaoSistemaCopia;
        }

        #endregion Interfaces
    }
}