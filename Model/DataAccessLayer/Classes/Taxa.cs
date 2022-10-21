using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class Taxa : ObservableObject, ICloneable
    {
        #region Campos

        int? _id;
        int? _idRetornado;
        string? _nome;
        DateTime? _dataInicio;
        DateTime? _dataFim;
        decimal? _percentualAliquotaExternaIcmsItemNacional;
        decimal? _percentualAliquotaExternaIcmsItemImportado;
        decimal? _percentualMvaItemNacional;
        decimal? _percentualMvaItemImportado;
        decimal? _percentualAliquotaInternaIcmsItemNacional;
        decimal? _percentualAliquotaInternaIcmsItemImportado;
        decimal? _impostosFederaisItemNacional;
        decimal? _impostosFederaisItemImportado;
        decimal? _rateioDespesaFixaItemNacional;
        decimal? _rateioDespesaFixaItemImportado;
        decimal? _percentualLucroNecessarioItemRevendaStNacional;
        decimal? _percentualLucroNecessarioItemOutrosNacional;
        decimal? _percentualLucroNecessarioItemRevendaStImportado;
        decimal? _percentualLucroNecessarioItemOutrosImportado;
        decimal? _percentualAcrescimoItemNacional;
        decimal? _percentualAcrescimoItemImportado;

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
        
        public decimal? PercentualAliquotaExternaIcmsItemNacional 
        {
            get { return _percentualAliquotaExternaIcmsItemNacional; } 
            set 
            {
                if (value != _percentualAliquotaExternaIcmsItemNacional)
                {
                    _percentualAliquotaExternaIcmsItemNacional = value;
                    OnPropertyChanged(nameof(PercentualAliquotaExternaIcmsItemNacional));
                }
            }
        }
        
        public decimal? PercentualAliquotaExternaIcmsItemImportado
        { 
            get { return _percentualAliquotaExternaIcmsItemImportado; } 
            set
            {
                if (value != _percentualAliquotaExternaIcmsItemImportado)
                {
                    _percentualAliquotaExternaIcmsItemImportado = value;
                    OnPropertyChanged(nameof(PercentualAliquotaExternaIcmsItemImportado));
                }
            }
        }
        
        public decimal? PercentualMvaItemNacional
        {
            get { return _percentualMvaItemNacional; } 
            set 
            {
                if (value != _percentualMvaItemNacional)
                {
                    _percentualMvaItemNacional = value;
                    OnPropertyChanged(nameof(PercentualMvaItemNacional));
                }
            }
        }
        
        public decimal? PercentualMvaItemImportado 
        {
            get { return _percentualMvaItemImportado; }
            set 
            {
                if (value != _percentualMvaItemImportado)
                {
                    _percentualMvaItemImportado = value;
                    OnPropertyChanged(nameof(PercentualMvaItemImportado));
                }
            }
        }
        
        public decimal? PercentualAliquotaInternaIcmsItemNacional 
        {
            get { return _percentualAliquotaInternaIcmsItemNacional; } 
            set
            {
                if (value != _percentualAliquotaInternaIcmsItemNacional)
                {
                    _percentualAliquotaInternaIcmsItemNacional = value;
                    OnPropertyChanged(nameof(PercentualAliquotaInternaIcmsItemNacional));
                }
            }
        }
        
        public decimal? PercentualAliquotaInternaIcmsItemImportado
        { 
            get { return _percentualAliquotaInternaIcmsItemImportado; }
            set 
            {
                if (value != _percentualAliquotaInternaIcmsItemImportado)
                {
                    _percentualAliquotaInternaIcmsItemImportado = value;
                    OnPropertyChanged(nameof(PercentualAliquotaInternaIcmsItemImportado));
                }
            }
        }
        
        public decimal? ImpostosFederaisItemNacional 
        {
            get { return _impostosFederaisItemNacional; } 
            set 
            {
                if (value != _impostosFederaisItemNacional)
                {
                    _impostosFederaisItemNacional = value;
                    OnPropertyChanged(nameof(ImpostosFederaisItemNacional));
                }
            }
        }
        
        public decimal? ImpostosFederaisItemImportado 
        {
            get { return _impostosFederaisItemImportado; } 
            set
            {
                if (value != _impostosFederaisItemImportado)
                {
                    _impostosFederaisItemImportado = value;
                    OnPropertyChanged(nameof(ImpostosFederaisItemImportado));
                }
            }
        }
        
        public decimal? RateioDespesaFixaItemNacional 
        {
            get { return _rateioDespesaFixaItemNacional; } 
            set
            {
                if (value != _rateioDespesaFixaItemNacional)
                {
                    _rateioDespesaFixaItemNacional = value;
                    OnPropertyChanged(nameof(RateioDespesaFixaItemNacional));
                }
            }
        }
        
        public decimal? RateioDespesaFixaItemImportado 
        {
            get { return _rateioDespesaFixaItemImportado; } 
            set 
            {
                if (value != _rateioDespesaFixaItemImportado)
                {
                    _rateioDespesaFixaItemImportado = value;
                    OnPropertyChanged(nameof(RateioDespesaFixaItemImportado));
                }
            }
        }
        
        public decimal? PercentualLucroNecessarioItemRevendaStNacional 
        {
            get { return _percentualLucroNecessarioItemRevendaStNacional; } 
            set
            {
                if (value != _percentualLucroNecessarioItemRevendaStNacional)
                {
                    _percentualLucroNecessarioItemRevendaStNacional = value;
                    OnPropertyChanged(nameof(PercentualLucroNecessarioItemRevendaStNacional));
                }
            }
        }
        
        public decimal? PercentualLucroNecessarioItemOutrosNacional 
        {
            get { return _percentualLucroNecessarioItemOutrosNacional; } 
            set 
            {
                if (value != _percentualLucroNecessarioItemOutrosNacional)
                {
                    _percentualLucroNecessarioItemOutrosNacional = value;
                    OnPropertyChanged(nameof(PercentualLucroNecessarioItemOutrosNacional));
                }
            }
        }
        
        public decimal? PercentualLucroNecessarioItemRevendaStImportado 
        {
            get { return _percentualLucroNecessarioItemRevendaStImportado; }
            set
            {
                if (value != _percentualLucroNecessarioItemRevendaStImportado)
                {
                    _percentualLucroNecessarioItemRevendaStImportado = value;
                    OnPropertyChanged(nameof(PercentualLucroNecessarioItemRevendaStImportado));
                }
            }
        }
        
        public decimal? PercentualLucroNecessarioItemOutrosImportado 
        {
            get { return _percentualLucroNecessarioItemOutrosImportado; }
            set
            {
                if (value != _percentualLucroNecessarioItemOutrosImportado)
                {
                    _percentualLucroNecessarioItemOutrosImportado = value;
                    OnPropertyChanged(nameof(PercentualLucroNecessarioItemOutrosImportado));
                }
            }
        }
        
        public decimal? PercentualAcrescimoItemNacional 
        {
            get { return _percentualAcrescimoItemNacional; }
            set
            {
                if (value != _percentualAcrescimoItemNacional)
                {
                    _percentualAcrescimoItemNacional = value;
                    OnPropertyChanged(nameof(PercentualAcrescimoItemNacional));
                }
            }
        }
        
        public decimal? PercentualAcrescimoItemImportado 
        { 
            get { return _percentualAcrescimoItemImportado; } 
            set
            {
                if (value != _percentualAcrescimoItemImportado)
                {
                    _percentualAcrescimoItemImportado = value;
                    OnPropertyChanged(nameof(PercentualAcrescimoItemImportado));
                }
            }
        }

        #endregion // Propriedades

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados da taxa através do id
        /// </summary>
        /// <param name="id">Representa o id da taxa que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetTaxaDatabaseAsync(int? id, CancellationToken ct)
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
                                          + "id_taxa AS IdTaxa, "
                                          + "nome AS NomeTaxa, "
                                          + "data_inicio AS DataInicioTaxa, "
                                          + "data_fim AS DataFimTaxa, "
                                          + "percentual_aliquota_externa_icms_item_nacional AS PercentualAliquotaExternaIcmsItemNacional, "
                                          + "percentual_aliquota_externa_icms_item_importado AS PercentualAliquotaExternaIcmsItemImportado, "
                                          + "percentual_mva_item_nacional AS PercentualMvaItemNacional, "
                                          + "percentual_mva_item_importado AS PercentualMvaItemImportado, "
                                          + "percentual_aliquota_interna_icms_item_nacional AS PercentualAliquotaInternaIcmsItemNacional, "
                                          + "percentual_aliquota_interna_icms_item_importado AS PercentualAliquotaInternaIcmsItemImportado, "
                                          + "impostos_federais_item_nacional AS ImpostosFederaisItemNacional, "
                                          + "impostos_federais_item_importado AS ImpostosFederaisItemImportado, "
                                          + "rateio_despesa_fixa_item_nacional AS RateioDespesaFixaItemNacional, "
                                          + "rateio_despesa_fixa_item_importado AS RateioDespesaFixaItemImportado, "
                                          + "percentual_lucro_necessario_item_revenda_st_nacional AS PercentualLucroNecessarioItemRevendaStNacional, "
                                          + "percentual_lucro_necessario_item_outros_nacional AS PercentualLucroNecessarioItemOutrosNacional, "
                                          + "percentual_lucro_necessario_item_revenda_st_importado AS PercentualLucroNecessarioItemRevendaStImportado, "
                                          + "percentual_lucro_necessario_item_outros_importado AS PercentualLucroNecessarioItemOutrosImportado, "
                                          + "percentual_acrescimo_item_nacional AS PercentualAcrescimoItemNacional, "
                                          + "percentual_acrescimo_item_importado AS PercentualAcrescimoItemImportado "
                                          + "FROM tb_taxas "
                                          + "WHERE id_taxa = @id";
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
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdTaxa"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTaxa"]);
                                DataInicio = FuncoesDeConversao.ConverteParaDateTime(reader["DataInicioTaxa"]);
                                DataFim = FuncoesDeConversao.ConverteParaDateTime(reader["DataFimTaxa"]);
                                PercentualAliquotaExternaIcmsItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaExternaIcmsItemNacional"]);
                                PercentualAliquotaExternaIcmsItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaExternaIcmsItemImportado"]);
                                PercentualMvaItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualMvaItemNacional"]);
                                PercentualMvaItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualMvaItemImportado"]);
                                PercentualAliquotaInternaIcmsItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaInternaIcmsItemNacional"]);
                                PercentualAliquotaInternaIcmsItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaInternaIcmsItemImportado"]);
                                ImpostosFederaisItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["ImpostosFederaisItemNacional"]);
                                ImpostosFederaisItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["ImpostosFederaisItemImportado"]);
                                RateioDespesaFixaItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["RateioDespesaFixaItemNacional"]);
                                RateioDespesaFixaItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["RateioDespesaFixaItemImportado"]);
                                PercentualLucroNecessarioItemRevendaStNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItemRevendaStNacional"]);
                                PercentualLucroNecessarioItemOutrosNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItemOutrosNacional"]);
                                PercentualLucroNecessarioItemRevendaStImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItemRevendaStImportado"]);
                                PercentualLucroNecessarioItemOutrosImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItemOutrosImportado"]);
                                PercentualAcrescimoItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoItemNacional"]);
                                PercentualAcrescimoItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoItemImportado"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados da taxa através de condições
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetTaxaDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                                     + "id_taxa AS IdTaxa, "
                                     + "nome AS NomeTaxa, "
                                     + "data_inicio AS DataInicioTaxa, "
                                     + "data_fim AS DataFimTaxa, "
                                     + "percentual_aliquota_externa_icms_item_nacional AS PercentualAliquotaExternaIcmsItemNacional, "
                                     + "percentual_aliquota_externa_icms_item_importado AS PercentualAliquotaExternaIcmsItemImportado, "
                                     + "percentual_mva_item_nacional AS PercentualMvaItemNacional, "
                                     + "percentual_mva_item_importado AS PercentualMvaItemImportado, "
                                     + "percentual_aliquota_interna_icms_item_nacional AS PercentualAliquotaInternaIcmsItemNacional, "
                                     + "percentual_aliquota_interna_icms_item_importado AS PercentualAliquotaInternaIcmsItemImportado, "
                                     + "impostos_federais_item_nacional AS ImpostosFederaisItemNacional, "
                                     + "impostos_federais_item_importado AS ImpostosFederaisItemImportado, "
                                     + "rateio_despesa_fixa_item_nacional AS RateioDespesaFixaItemNacional, "
                                     + "rateio_despesa_fixa_item_importado AS RateioDespesaFixaItemImportado, "
                                     + "percentual_lucro_necessario_item_revenda_st_nacional AS PercentualLucroNecessarioItemRevendaStNacional, "
                                     + "percentual_lucro_necessario_item_outros_nacional AS PercentualLucroNecessarioItemOutrosNacional, "
                                     + "percentual_lucro_necessario_item_revenda_st_importado AS PercentualLucroNecessarioItemRevendaStImportado, "
                                     + "percentual_lucro_necessario_item_outros_importado AS PercentualLucroNecessarioItemOutrosImportado, "
                                     + "percentual_acrescimo_item_nacional AS PercentualAcrescimoItemNacional, "
                                     + "percentual_acrescimo_item_importado AS PercentualAcrescimoItemImportado "
                                     + "FROM tb_taxas "
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
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdTaxa"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTaxa"]);
                                DataInicio = FuncoesDeConversao.ConverteParaDateTime(reader["DataInicioTaxa"]);
                                DataFim = FuncoesDeConversao.ConverteParaDateTime(reader["DataFimTaxa"]);
                                PercentualAliquotaExternaIcmsItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaExternaIcmsItemNacional"]);
                                PercentualAliquotaExternaIcmsItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaExternaIcmsItemImportado"]);
                                PercentualMvaItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualMvaItemNacional"]);
                                PercentualMvaItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualMvaItemImportado"]);
                                PercentualAliquotaInternaIcmsItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaInternaIcmsItemNacional"]);
                                PercentualAliquotaInternaIcmsItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaInternaIcmsItemImportado"]);
                                ImpostosFederaisItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["ImpostosFederaisItemNacional"]);
                                ImpostosFederaisItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["ImpostosFederaisItemImportado"]);
                                RateioDespesaFixaItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["RateioDespesaFixaItemNacional"]);
                                RateioDespesaFixaItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["RateioDespesaFixaItemImportado"]);
                                PercentualLucroNecessarioItemRevendaStNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItemRevendaStNacional"]);
                                PercentualLucroNecessarioItemOutrosNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItemOutrosNacional"]);
                                PercentualLucroNecessarioItemRevendaStImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItemRevendaStImportado"]);
                                PercentualLucroNecessarioItemOutrosImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItemOutrosImportado"]);
                                PercentualAcrescimoItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoItemNacional"]);
                                PercentualAcrescimoItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoItemImportado"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva a taxa na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarTaxaDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_taxa", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_data_inicio", DataInicio);
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_percentual_aliquota_externa_icms_item_nacional", PercentualAliquotaExternaIcmsItemNacional);
                        command.Parameters.AddWithValue("p_percentual_aliquota_externa_icms_item_importado", PercentualAliquotaExternaIcmsItemImportado);
                        command.Parameters.AddWithValue("p_percentual_mva_item_nacional", PercentualMvaItemNacional);
                        command.Parameters.AddWithValue("p_percentual_mva_item_importado", PercentualMvaItemImportado);
                        command.Parameters.AddWithValue("p_percentual_aliquota_interna_icms_item_nacional", PercentualAliquotaInternaIcmsItemNacional);
                        command.Parameters.AddWithValue("p_percentual_aliquota_interna_icms_item_importado", PercentualAliquotaInternaIcmsItemImportado);
                        command.Parameters.AddWithValue("p_impostos_federais_item_nacional", ImpostosFederaisItemNacional);
                        command.Parameters.AddWithValue("p_impostos_federais_item_importado", ImpostosFederaisItemImportado);
                        command.Parameters.AddWithValue("p_rateio_despesa_fixa_item_nacional", RateioDespesaFixaItemNacional);
                        command.Parameters.AddWithValue("p_rateio_despesa_fixa_item_importado", RateioDespesaFixaItemImportado);
                        command.Parameters.AddWithValue("p_percentual_lucro_necessario_item_revenda_st_nacional", PercentualLucroNecessarioItemRevendaStNacional);
                        command.Parameters.AddWithValue("p_percentual_lucro_necessario_item_outros_nacional", PercentualLucroNecessarioItemOutrosNacional);
                        command.Parameters.AddWithValue("p_percentual_lucro_necessario_item_revenda_st_importado", PercentualLucroNecessarioItemRevendaStImportado);
                        command.Parameters.AddWithValue("p_percentual_lucro_necessario_item_outros_importado", PercentualLucroNecessarioItemOutrosImportado);
                        command.Parameters.AddWithValue("p_percentual_acrescimo_item_nacional", PercentualAcrescimoItemNacional);
                        command.Parameters.AddWithValue("p_percentual_acrescimo_item_importado", PercentualAcrescimoItemImportado);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add("p_id_taxa", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException(nameof(Taxa).ToLower(), Nome);
                        }

                        // Retorna o id da série
                        IdRetornado = FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_taxa"].Value);
                    }

                    // Utilização do comando para lançar a data fim das outras taxas
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_taxa_data_fim", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_taxa", IdRetornado);
                        command.Parameters.AddWithValue("p_data_fim", Convert.ToDateTime(DataInicio).AddDays(-1));
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_taxa", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_taxa", Id);
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException(nameof(Taxa).ToLower(), Nome);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta a taxa na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarTaxaDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_taxa", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_taxa", Id);
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
                            throw new ChaveEstrangeiraEmUsoException(nameof(Taxa).ToLower(), Nome);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException(nameof(Taxa).ToLower(), Nome);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de taxas com os argumentos utilizados
        /// </summary>
        /// <param name="listaTaxas">Representa a lista de taxas que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaTaxasAsync(ObservableCollection<Taxa> listaTaxas, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaTaxas == null)
            {
                listaTaxas = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaTaxas.Clear();
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
                                 + "id_taxa AS IdTaxa, "
                                 + "nome AS NomeTaxa, "
                                 + "data_inicio AS DataInicioTaxa, "
                                 + "data_fim AS DataFimTaxa, "
                                 + "percentual_aliquota_externa_icms_item_nacional AS PercentualAliquotaExternaIcmsItemNacional, "
                                 + "percentual_aliquota_externa_icms_item_importado AS PercentualAliquotaExternaIcmsItemImportado, "
                                 + "percentual_mva_item_nacional AS PercentualMvaItemNacional, "
                                 + "percentual_mva_item_importado AS PercentualMvaItemImportado, "
                                 + "percentual_aliquota_interna_icms_item_nacional AS PercentualAliquotaInternaIcmsItemNacional, "
                                 + "percentual_aliquota_interna_icms_item_importado AS PercentualAliquotaInternaIcmsItemImportado, "
                                 + "impostos_federais_item_nacional AS ImpostosFederaisItemNacional, "
                                 + "impostos_federais_item_importado AS ImpostosFederaisItemImportado, "
                                 + "rateio_despesa_fixa_item_nacional AS RateioDespesaFixaItemNacional, "
                                 + "rateio_despesa_fixa_item_importado AS RateioDespesaFixaItemImportado, "
                                 + "percentual_lucro_necessario_item_revenda_st_nacional AS PercentualLucroNecessarioItemRevendaStNacional, "
                                 + "percentual_lucro_necessario_item_outros_nacional AS PercentualLucroNecessarioItemOutrosNacional, "
                                 + "percentual_lucro_necessario_item_revenda_st_importado AS PercentualLucroNecessarioItemRevendaStImportado, "
                                 + "percentual_lucro_necessario_item_outros_importado AS PercentualLucroNecessarioItemOutrosImportado, "
                                 + "percentual_acrescimo_item_nacional AS PercentualAcrescimoItemNacional, "
                                 + "percentual_acrescimo_item_importado AS PercentualAcrescimoItemImportado "
                                 + "FROM tb_taxas "
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
                                Taxa item = new();

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTaxa"]);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTaxa"]);
                                item.DataInicio = FuncoesDeConversao.ConverteParaDateTime(reader["DataInicioTaxa"]);
                                item.DataFim = FuncoesDeConversao.ConverteParaDateTime(reader["DataFimTaxa"]);
                                item.PercentualAliquotaExternaIcmsItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaExternaIcmsItemNacional"]);
                                item.PercentualAliquotaExternaIcmsItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaExternaIcmsItemImportado"]);
                                item.PercentualMvaItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualMvaItemNacional"]);
                                item.PercentualMvaItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualMvaItemImportado"]);
                                item.PercentualAliquotaInternaIcmsItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaInternaIcmsItemNacional"]);
                                item.PercentualAliquotaInternaIcmsItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAliquotaInternaIcmsItemImportado"]);
                                item.ImpostosFederaisItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["ImpostosFederaisItemNacional"]);
                                item.ImpostosFederaisItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["ImpostosFederaisItemImportado"]);
                                item.RateioDespesaFixaItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["RateioDespesaFixaItemNacional"]);
                                item.RateioDespesaFixaItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["RateioDespesaFixaItemImportado"]);
                                item.PercentualLucroNecessarioItemRevendaStNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItemRevendaStNacional"]);
                                item.PercentualLucroNecessarioItemOutrosNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItemOutrosNacional"]);
                                item.PercentualLucroNecessarioItemRevendaStImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItemRevendaStImportado"]);
                                item.PercentualLucroNecessarioItemOutrosImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualLucroNecessarioItemOutrosImportado"]);
                                item.PercentualAcrescimoItemNacional = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoItemNacional"]);
                                item.PercentualAcrescimoItemImportado = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoItemImportado"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaTaxas.Add(item);

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
            Taxa taxaCopia = new();
            taxaCopia.Id = Id;
            taxaCopia.Nome = Nome;
            taxaCopia.DataInicio = DataInicio;
            taxaCopia.DataFim = DataFim;
            taxaCopia.PercentualAliquotaExternaIcmsItemNacional = PercentualAliquotaExternaIcmsItemNacional;
            taxaCopia.PercentualAliquotaExternaIcmsItemImportado = PercentualAliquotaExternaIcmsItemImportado;
            taxaCopia.PercentualMvaItemNacional = PercentualMvaItemNacional;
            taxaCopia.PercentualMvaItemImportado = PercentualMvaItemImportado;
            taxaCopia.PercentualAliquotaInternaIcmsItemNacional = PercentualAliquotaInternaIcmsItemNacional;
            taxaCopia.PercentualAliquotaInternaIcmsItemImportado = PercentualAliquotaInternaIcmsItemImportado;
            taxaCopia.ImpostosFederaisItemNacional = ImpostosFederaisItemNacional;
            taxaCopia.ImpostosFederaisItemImportado = ImpostosFederaisItemImportado;
            taxaCopia.RateioDespesaFixaItemNacional = RateioDespesaFixaItemNacional;
            taxaCopia.RateioDespesaFixaItemImportado = RateioDespesaFixaItemImportado;
            taxaCopia.PercentualLucroNecessarioItemRevendaStNacional = PercentualLucroNecessarioItemRevendaStNacional;
            taxaCopia.PercentualLucroNecessarioItemOutrosNacional = PercentualLucroNecessarioItemOutrosNacional;
            taxaCopia.PercentualLucroNecessarioItemRevendaStImportado = PercentualLucroNecessarioItemRevendaStImportado;
            taxaCopia.PercentualLucroNecessarioItemOutrosImportado = PercentualLucroNecessarioItemOutrosImportado;
            taxaCopia.PercentualAcrescimoItemNacional = PercentualAcrescimoItemNacional;
            taxaCopia.PercentualAcrescimoItemImportado = PercentualAcrescimoItemImportado;

            return taxaCopia;
        }

        #endregion // Interfaces
    }
}
