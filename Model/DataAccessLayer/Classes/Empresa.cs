﻿using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class Empresa : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private string? _cnpj;
        private string? _razaoSocial;
        private string? _nomeFantasia;
        private string? _site;
        private byte[]? _logo1;
        private byte[]? _logo2;
        private byte[]? _logo3;
        private Status? _status;

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
        
        public string? CNPJ 
        {
            get { return _cnpj; } 
            set
            {
                if(value != _cnpj)
                {
                    _cnpj = value;
                    OnPropertyChanged(nameof(CNPJ));
                }
            }
        }
        
        public string? RazaoSocial
        { 
            get { return _razaoSocial; } 
            set
            {
                if(value != _razaoSocial)
                {
                    _razaoSocial = value;
                    OnPropertyChanged(nameof(RazaoSocial));
                }
            }
        }
        
        public string? NomeFantasia 
        {
            get { return _nomeFantasia; } 
            set 
            {
                if(value != _nomeFantasia)
                {
                    _nomeFantasia = value;
                    OnPropertyChanged(nameof(NomeFantasia));
                }
            }
        }
        
        public string? Site 
        {
            get { return _site; } 
            set
            {
                if(value != _site)
                {
                    _site = value;
                    OnPropertyChanged(nameof(Site));
                }
            }
        }

        public byte[]? Logo1
        {
            get { return _logo1; }
            set
            {
                if (value != _logo1)
                {
                    _logo1 = value;
                    OnPropertyChanged(nameof(Logo1));
                }
            }
        }

        public byte[]? Logo2
        {
            get { return _logo2; }
            set
            {
                if (value != _logo2)
                {
                    _logo2 = value;
                    OnPropertyChanged(nameof(Logo2));
                }
            }
        }

        public byte[]? Logo3
        {
            get { return _logo3; }
            set
            {
                if (value != _logo3)
                {
                    _logo3 = value;
                    OnPropertyChanged(nameof(Logo3));
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

        #endregion // Propriedades

        #region Construtores

        /// <summary>
        /// Construtor sem parâmetros que cria uma nova instância de todas as classes
        /// </summary>
        public Empresa()
        {
            Status = new();
        }

        #endregion // Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados da empresa com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id da empresa que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetEmpresaDatabaseAsync(int? id, CancellationToken ct)
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
                                          + "empr.id_empresa AS IdEmpresa, "
                                          + "empr.cnpj AS CNPJEmpresa, "
                                          + "empr.razao_social AS RazaoSocialEmpresa, "
                                          + "empr.nome_fantasia AS NomeFantasiaEmpresa, "
                                          + "empr.site AS SiteEmpresa, "
                                          + "empr.logo_1 AS Logo1, "
                                          + "empr.logo_2 AS Logo2, "
                                          + "empr.logo_3 AS Logo3, "
                                          + "stat.id_status AS IdStatusEmpresa, "
                                          + "stat.nome AS NomeStatusEmpresa "
                                          + "FROM tb_empresas AS empr " 
                                          + "LEFT JOIN tb_status AS stat ON stat.id_status = empr.id_status "
                                          + "WHERE empr.id_empresa = @id";

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
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdEmpresa"]);
                                CNPJ = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CNPJEmpresa"]));
                                RazaoSocial = FuncoesDeConversao.ConverteParaString(reader["RazaoSocialEmpresa"]);
                                NomeFantasia = FuncoesDeConversao.ConverteParaString(reader["NomeFantasiaEmpresa"]);
                                Site = FuncoesDeConversao.ConverteParaString(reader["SiteEmpresa"]);
                                Logo1 = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Logo1"]);
                                Logo2 = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Logo2"]);
                                Logo3 = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Logo3"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEmpresa"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEmpresa"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados da empresa com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetEmpresaDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                      + "empr.id_empresa AS IdEmpresa, "
                      + "empr.cnpj AS CNPJEmpresa, "
                      + "empr.razao_social AS RazaoSocialEmpresa, "
                      + "empr.nome_fantasia AS NomeFantasiaEmpresa, "
                      + "empr.site AS SiteEmpresa, "
                      + "empr.logo_1 AS Logo1, "
                      + "empr.logo_2 AS Logo2, "
                      + "empr.logo_3 AS Logo3, "
                      + "stat.id_status AS IdStatusEmpresa, "
                      + "stat.nome AS NomeStatusEmpresa "
                      + "FROM tb_empresas AS empr "
                      + "LEFT JOIN tb_status AS stat ON stat.id_status = empr.id_status "
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
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdEmpresa"]);
                                CNPJ = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CNPJEmpresa"]));
                                RazaoSocial = FuncoesDeConversao.ConverteParaString(reader["RazaoSocialEmpresa"]);
                                NomeFantasia = FuncoesDeConversao.ConverteParaString(reader["NomeFantasiaEmpresa"]);
                                Site = FuncoesDeConversao.ConverteParaString(reader["SiteEmpresa"]);
                                Logo1 = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Logo1"]);
                                Logo2 = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Logo2"]);
                                Logo3 = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Logo3"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEmpresa"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEmpresa"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva a empresa na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarEmpresaDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_empresa", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_CNPJ", CNPJ);
                        command.Parameters.AddWithValue("p_razao_social", RazaoSocial);
                        command.Parameters.AddWithValue("p_nome_fantasia", NomeFantasia);
                        command.Parameters.AddWithValue("p_site", Site);
                        command.Parameters.AddWithValue("p_logo_1", Logo1);
                        command.Parameters.AddWithValue("p_logo_2", Logo2);
                        command.Parameters.AddWithValue("p_logo_3", Logo3);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException(nameof(Empresa).ToLower(), NomeFantasia);
                        }
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_empresa", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_empresa", Id);
                        command.Parameters.AddWithValue("p_CNPJ", CNPJ);
                        command.Parameters.AddWithValue("p_razao_social", RazaoSocial);
                        command.Parameters.AddWithValue("p_nome_fantasia", NomeFantasia);
                        command.Parameters.AddWithValue("p_site", Site);
                        command.Parameters.AddWithValue("p_logo_1", Logo1);
                        command.Parameters.AddWithValue("p_logo_2", Logo2);
                        command.Parameters.AddWithValue("p_logo_3", Logo3);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException(nameof(Empresa).ToLower(), NomeFantasia);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta a empresa na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarEmpresaDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_empresa", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_empresa", Id);
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
                            throw new ChaveEstrangeiraEmUsoException(nameof(Empresa).ToLower(), NomeFantasia);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException(nameof(Empresa).ToLower(), NomeFantasia);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de empresas com os argumentos utilizados
        /// </summary>
        /// <param name="listaEmpresas">Representa a lista de empresas que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaEmpresasAsync(ObservableCollection<Empresa> listaEmpresas, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaEmpresas == null)
            {
                listaEmpresas = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaEmpresas.Clear();
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
                  + "empr.id_empresa AS IdEmpresa, "
                  + "empr.cnpj AS CNPJEmpresa, "
                  + "empr.razao_social AS RazaoSocialEmpresa, "
                  + "empr.nome_fantasia AS NomeFantasiaEmpresa, "
                  + "empr.site AS SiteEmpresa, "
                  + "empr.logo_1 AS Logo1, "
                  + "empr.logo_2 AS Logo2, "
                  + "empr.logo_3 AS Logo3, "
                  + "stat.id_status AS IdStatusEmpresa, "
                  + "stat.nome AS NomeStatusEmpresa "
                  + "FROM tb_empresas AS empr "
                  + "LEFT JOIN tb_status AS stat ON stat.id_status = empr.id_status "
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
                                Empresa item = new();

                                // Verifica se a classe não foi instanciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEmpresa"]);
                                item.CNPJ = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CNPJEmpresa"]));
                                item.RazaoSocial = FuncoesDeConversao.ConverteParaString(reader["RazaoSocialEmpresa"]);
                                item.NomeFantasia = FuncoesDeConversao.ConverteParaString(reader["NomeFantasiaEmpresa"]);
                                item.Logo1 = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Logo1"]);
                                item.Logo2 = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Logo2"]);
                                item.Logo3 = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Logo3"]);
                                item.Site = FuncoesDeConversao.ConverteParaString(reader["SiteEmpresa"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEmpresa"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEmpresa"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaEmpresas.Add(item);

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
            Empresa empresaCopia = new();

            empresaCopia.Id = Id;
            empresaCopia.CNPJ = CNPJ;
            empresaCopia.RazaoSocial = RazaoSocial;
            empresaCopia.NomeFantasia = NomeFantasia;
            empresaCopia.Site = Site;
            empresaCopia.Logo1 = Logo1;
            empresaCopia.Logo2 = Logo2;
            empresaCopia.Logo3 = Logo3;
            empresaCopia.Status = Status == null ? new() : (Status)Status.Clone();

            return empresaCopia;
        }

        #endregion // Interfaces
    }
}
