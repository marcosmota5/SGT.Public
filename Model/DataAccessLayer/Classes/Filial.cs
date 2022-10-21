using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class Filial : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private string? _nome;
        private string? _endereco;
        private string? _cep;
        private string? _telefoneGeral;
        private string? _telefonePecas;
        private string? _telefoneServicos;
        private string? _emailGeral;
        private string? _emailPecas;
        private string? _emailServicos;
        private Status? _status;
        private Empresa? _empresa;
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
        
        public string? Endereco 
        { 
            get { return _endereco; } 
            set
            {
                if(value != _endereco)
                {
                    _endereco = value;
                    OnPropertyChanged(nameof(Endereco));
                }
            } 
        }
        
        public string? CEP 
        { 
            get { return _cep; } 
            set
            {
                if(value != _cep)
                {
                    _cep = value;
                    OnPropertyChanged(nameof(CEP));
                }
            }
        }
        
        public string? TelefoneGeral 
        { 
            get { return _telefoneGeral; } 
            set 
            {
                if(value != _telefoneGeral)
                {
                    _telefoneGeral = value;
                    OnPropertyChanged(nameof(TelefoneGeral));
                }
            }
        }
        
        public string? TelefonePecas 
        { 
            get { return _telefonePecas; } 
            set
            { 
                if(value != _telefonePecas)
                {
                    _telefonePecas = value;
                    OnPropertyChanged(nameof(TelefonePecas));
                }
            }
        }
        
        public string? TelefoneServicos
        { 
            get { return _telefoneServicos; } 
            set
            { 
                if(value != _telefoneServicos)
                {
                    _telefoneServicos = value;
                    OnPropertyChanged(nameof(TelefoneServicos));
                }
            }
        }
        
        public string? EmailGeral 
        {
            get { return _emailGeral; } 
            set 
            {
                if(value != _emailGeral)
                {
                    _emailGeral = value;
                    OnPropertyChanged(nameof(EmailGeral));
                }
            }
        }
        
        public string? EmailPecas
        { 
            get { return _emailPecas; }
            set 
            {
                if(value != _emailPecas)
                {
                    _emailPecas = value;
                    OnPropertyChanged(nameof(EmailPecas));
                }
            }
        }
        
        public string? EmailServicos 
        {
            get { return _emailServicos; }
            set 
            {
                if(value != _emailServicos)
                {
                    _emailServicos = value;
                    OnPropertyChanged(nameof(EmailServicos));
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
        
        public Empresa? Empresa 
        {
            get { return _empresa; } 
            set
            {
                if(value != _empresa)
                {
                    _empresa = value;
                    OnPropertyChanged(nameof(Empresa));
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
        public Filial()
        {
            Status = new();
            Empresa = new();
            Cidade = new();
        }

        #endregion // Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados da filial com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id da filial que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetFilialDatabaseAsync(int? id, CancellationToken ct)
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
                                          + "fili.id_filial AS IdFilial, "
                                          + "fili.nome AS NomeFilial, "
                                          + "fili.endereco AS EnderecoFilial, "
                                          + "fili.CEP AS CEPFilial, "
                                          + "fili.telefone_geral AS TelefoneGeralFilial, "
                                          + "fili.telefone_pecas AS TelefonePecasFilial, "
                                          + "fili.telefone_servicos AS TelefoneServicosFilial, "
                                          + "fili.email_geral AS EmailGeralFilial, "
                                          + "fili.email_pecas AS EmailPecasFilial, "
                                          + "fili.email_servicos AS EmailServicosFilial, "
                                          + "stat.id_status AS IdStatusFilial, "
                                          + "stat.nome AS NomeStatusFilial, "
                                          + "empr.id_empresa AS IdEmpresa, "
                                          + "empr.CNPJ AS CNPJEmpresa, "
                                          + "empr.razao_social AS RazaoSocialEmpresa, "
                                          + "empr.nome_fantasia AS NomeFantasiaEmpresa, "
                                          + "empr.site AS SiteEmpresa, "
                                          + "stat_empr.id_status AS IdStatusEmpresa, "
                                          + "stat_empr.nome AS NomeStatusEmpresa, "
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
                                          + "FROM tb_filiais AS fili "
                                          + "LEFT JOIN tb_status AS stat ON stat.id_status = fili.id_status "
                                          + "LEFT JOIN tb_empresas AS empr ON empr.id_empresa = fili.id_empresa "
                                          + "LEFT JOIN tb_status AS stat_empr ON stat_empr.id_status = empr.id_status "
                                          + "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = fili.id_cidade "
                                          + "LEFT JOIN tb_status AS stat_cida ON stat_cida.id_status = cida.id_status "
                                          + "LEFT JOIN tb_estados AS esta ON esta.id_estado = cida.id_estado "
                                          + "LEFT JOIN tb_status AS stat_esta ON stat_esta.id_status = esta.id_status "
                                          + "LEFT JOIN tb_paises AS pais ON pais.id_pais = esta.id_pais "
                                          + "LEFT JOIN tb_status AS stat_pais ON stat_pais.id_status = pais.id_status "
                                          + "WHERE fili.id_filial = @id";

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
                                if (Empresa == null)
                                {
                                    Empresa = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Empresa.Status == null)
                                {
                                    Empresa.Status = new();
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
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdFilial"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFilial"]);
                                Endereco = FuncoesDeConversao.ConverteParaString(reader["EnderecoFilial"]);
                                CEP = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CEPFilial"]));
                                TelefoneGeral = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneGeralFilial"]));
                                TelefonePecas = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefonePecasFilial"]));
                                TelefoneServicos = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneServicosFilial"]));
                                EmailGeral = FuncoesDeConversao.ConverteParaString(reader["EmailGeralFilial"]);
                                EmailPecas = FuncoesDeConversao.ConverteParaString(reader["EmailPecasFilial"]);
                                EmailServicos = FuncoesDeConversao.ConverteParaString(reader["EmailServicosFilial"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFilial"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFilial"]);
                                Empresa.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEmpresa"]);
                                Empresa.CNPJ = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CNPJEmpresa"]));
                                Empresa.RazaoSocial = FuncoesDeConversao.ConverteParaString(reader["RazaoSocialEmpresa"]);
                                Empresa.NomeFantasia = FuncoesDeConversao.ConverteParaString(reader["NomeFantasiaEmpresa"]);
                                Empresa.Site = FuncoesDeConversao.ConverteParaString(reader["SiteEmpresa"]);
                                Empresa.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEmpresa"]);
                                Empresa.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEmpresa"]);
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
        /// Método assíncrono que retorna os dados da filial com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetFilialDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                      + "fili.id_filial AS IdFilial, "
                      + "fili.nome AS NomeFilial, "
                      + "fili.endereco AS EnderecoFilial, "
                      + "fili.CEP AS CEPFilial, "
                      + "fili.telefone_geral AS TelefoneGeralFilial, "
                      + "fili.telefone_pecas AS TelefonePecasFilial, "
                      + "fili.telefone_servicos AS TelefoneServicosFilial, "
                      + "fili.email_geral AS EmailGeralFilial, "
                      + "fili.email_pecas AS EmailPecasFilial, "
                      + "fili.email_servicos AS EmailServicosFilial, "
                      + "stat.id_status AS IdStatusFilial, "
                      + "stat.nome AS NomeStatusFilial, "
                      + "empr.id_empresa AS IdEmpresa, "
                      + "empr.CNPJ AS CNPJEmpresa, "
                      + "empr.razao_social AS RazaoSocialEmpresa, "
                      + "empr.nome_fantasia AS NomeFantasiaEmpresa, "
                      + "empr.site AS SiteEmpresa, "
                      + "stat_empr.id_status AS IdStatusEmpresa, "
                      + "stat_empr.nome AS NomeStatusEmpresa, "
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
                      + "FROM tb_filiais AS fili "
                      + "LEFT JOIN tb_status AS stat ON stat.id_status = fili.id_status "
                      + "LEFT JOIN tb_empresas AS empr ON empr.id_empresa = fili.id_empresa "
                      + "LEFT JOIN tb_status AS stat_empr ON stat_empr.id_status = empr.id_status "
                      + "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = fili.id_cidade "
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
                                if (Empresa == null)
                                {
                                    Empresa = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Empresa.Status == null)
                                {
                                    Empresa.Status = new();
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
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdFilial"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFilial"]);
                                Endereco = FuncoesDeConversao.ConverteParaString(reader["EnderecoFilial"]);
                                CEP = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CEPFilial"]));
                                TelefoneGeral = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneGeralFilial"]));
                                TelefonePecas = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefonePecasFilial"]));
                                TelefoneServicos = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneServicosFilial"]));
                                EmailGeral = FuncoesDeConversao.ConverteParaString(reader["EmailGeralFilial"]);
                                EmailPecas = FuncoesDeConversao.ConverteParaString(reader["EmailPecasFilial"]);
                                EmailServicos = FuncoesDeConversao.ConverteParaString(reader["EmailServicosFilial"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFilial"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFilial"]);
                                Empresa.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEmpresa"]);
                                Empresa.CNPJ = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CNPJEmpresa"]));
                                Empresa.RazaoSocial = FuncoesDeConversao.ConverteParaString(reader["RazaoSocialEmpresa"]);
                                Empresa.NomeFantasia = FuncoesDeConversao.ConverteParaString(reader["NomeFantasiaEmpresa"]);
                                Empresa.Site = FuncoesDeConversao.ConverteParaString(reader["SiteEmpresa"]);
                                Empresa.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEmpresa"]);
                                Empresa.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEmpresa"]);
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
        /// Método assíncrono que salva a filial na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarFilialDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_filial", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_id_empresa", Empresa.Id);
                        command.Parameters.AddWithValue("p_id_pais", Cidade.Estado.Pais.Id);
                        command.Parameters.AddWithValue("p_id_estado", Cidade.Estado.Id);
                        command.Parameters.AddWithValue("p_id_cidade", Cidade.Id);
                        command.Parameters.AddWithValue("p_endereco", Endereco);
                        command.Parameters.AddWithValue("p_CEP", CEP);
                        command.Parameters.AddWithValue("p_telefone_geral", TelefoneGeral);
                        command.Parameters.AddWithValue("p_telefone_pecas", TelefonePecas);
                        command.Parameters.AddWithValue("p_telefone_servicos", TelefoneServicos);
                        command.Parameters.AddWithValue("p_email_geral", EmailGeral);
                        command.Parameters.AddWithValue("p_email_pecas", EmailPecas);
                        command.Parameters.AddWithValue("p_email_servicos", EmailServicos);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException(nameof(Filial).ToLower(), Nome);
                        }
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_filial", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_filial", Id);
                        command.Parameters.AddWithValue("p_id_empresa", Empresa.Id);
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_id_pais", Cidade.Estado.Pais.Id);
                        command.Parameters.AddWithValue("p_id_estado", Cidade.Estado.Id);
                        command.Parameters.AddWithValue("p_id_cidade", Cidade.Id);
                        command.Parameters.AddWithValue("p_endereco", Endereco);
                        command.Parameters.AddWithValue("p_CEP", CEP);
                        command.Parameters.AddWithValue("p_telefone_geral", TelefoneGeral);
                        command.Parameters.AddWithValue("p_telefone_pecas", TelefonePecas);
                        command.Parameters.AddWithValue("p_telefone_servicos", TelefoneServicos);
                        command.Parameters.AddWithValue("p_email_geral", EmailGeral);
                        command.Parameters.AddWithValue("p_email_pecas", EmailPecas);
                        command.Parameters.AddWithValue("p_email_servicos", EmailServicos);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException(nameof(Filial).ToLower(), Nome);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta a filial na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarFilialDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_filial", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_filial", Id);
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
                            throw new ChaveEstrangeiraEmUsoException(nameof(Filial).ToLower(), Nome);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException(nameof(Filial).ToLower(), Nome);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de filiais com os argumentos utilizados
        /// </summary>
        /// <param name="listaFiliais">Representa a lista de filiais que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaFiliaisAsync(ObservableCollection<Filial> listaFiliais, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaFiliais == null)
            {
                listaFiliais = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaFiliais.Clear();
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
                  + "fili.id_filial AS IdFilial, "
                  + "fili.nome AS NomeFilial, "
                  + "fili.endereco AS EnderecoFilial, "
                  + "fili.CEP AS CEPFilial, "
                  + "fili.telefone_geral AS TelefoneGeralFilial, "
                  + "fili.telefone_pecas AS TelefonePecasFilial, "
                  + "fili.telefone_servicos AS TelefoneServicosFilial, "
                  + "fili.email_geral AS EmailGeralFilial, "
                  + "fili.email_pecas AS EmailPecasFilial, "
                  + "fili.email_servicos AS EmailServicosFilial, "
                  + "stat.id_status AS IdStatusFilial, "
                  + "stat.nome AS NomeStatusFilial, "
                  + "empr.id_empresa AS IdEmpresa, "
                  + "empr.CNPJ AS CNPJEmpresa, "
                  + "empr.razao_social AS RazaoSocialEmpresa, "
                  + "empr.nome_fantasia AS NomeFantasiaEmpresa, "
                  + "empr.site AS SiteEmpresa, "
                  + "stat_empr.id_status AS IdStatusEmpresa, "
                  + "stat_empr.nome AS NomeStatusEmpresa, "
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
                  + "FROM tb_filiais AS fili "
                  + "LEFT JOIN tb_status AS stat ON stat.id_status = fili.id_status "
                  + "LEFT JOIN tb_empresas AS empr ON empr.id_empresa = fili.id_empresa "
                  + "LEFT JOIN tb_status AS stat_empr ON stat_empr.id_status = empr.id_status "
                  + "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = fili.id_cidade "
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
                                Filial item = new();

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Empresa == null)
                                {
                                    item.Empresa = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Empresa.Status == null)
                                {
                                    item.Empresa.Status = new();
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
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFilial"]);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFilial"]);
                                item.Endereco = FuncoesDeConversao.ConverteParaString(reader["EnderecoFilial"]);
                                item.CEP = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CEPFilial"]));
                                item.TelefoneGeral = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneGeralFilial"]));
                                item.TelefonePecas = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefonePecasFilial"]));
                                item.TelefoneServicos = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneServicosFilial"]));
                                item.EmailGeral = FuncoesDeConversao.ConverteParaString(reader["EmailGeralFilial"]);
                                item.EmailPecas = FuncoesDeConversao.ConverteParaString(reader["EmailPecasFilial"]);
                                item.EmailServicos = FuncoesDeConversao.ConverteParaString(reader["EmailServicosFilial"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFilial"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFilial"]);
                                item.Empresa.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEmpresa"]);
                                item.Empresa.CNPJ = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CNPJEmpresa"]));
                                item.Empresa.RazaoSocial = FuncoesDeConversao.ConverteParaString(reader["RazaoSocialEmpresa"]);
                                item.Empresa.NomeFantasia = FuncoesDeConversao.ConverteParaString(reader["NomeFantasiaEmpresa"]);
                                item.Empresa.Site = FuncoesDeConversao.ConverteParaString(reader["SiteEmpresa"]);
                                item.Empresa.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEmpresa"]);
                                item.Empresa.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEmpresa"]);
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
                                listaFiliais.Add(item);

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
            Filial filialCopia = new();

            filialCopia.Id = Id;
            filialCopia.Nome = Nome;
            filialCopia.Endereco = Endereco;
            filialCopia.CEP = CEP;
            filialCopia.TelefoneGeral = TelefoneGeral;
            filialCopia.TelefonePecas = TelefonePecas;
            filialCopia.TelefoneServicos = TelefoneServicos;
            filialCopia.EmailGeral = EmailGeral;
            filialCopia.EmailPecas = EmailPecas;
            filialCopia.EmailServicos = EmailServicos;
            filialCopia.Status = Status == null ? new() : (Status)Status.Clone();
            filialCopia.Empresa = Empresa == null ? new() : (Empresa)Empresa.Clone();
            filialCopia.Cidade = Cidade == null ? new() : (Cidade)Cidade.Clone();

            return filialCopia;
        }

        #endregion // Interfaces
    }
}
