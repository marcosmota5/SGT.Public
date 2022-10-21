using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;
using System.Collections.ObjectModel;

namespace Model.DataAccessLayer.Classes
{
    public class ImagemRegistroManifestacao : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private int? _idRegistroManifestacao;
        private DateTime? _dataInsercao;
        private byte[]? _miniatura;
        private byte[]? _imagem;
        private int? _tamanhoImagem;
        private string? _nomePessoaInsercao;
        private string? _emailPessoaInsercao;

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

        public int? IdRegistroManifestacao
        {
            get { return _idRegistroManifestacao; }
            set
            {
                if (value != _idRegistroManifestacao)
                {
                    _idRegistroManifestacao = value;
                    OnPropertyChanged(nameof(IdRegistroManifestacao));
                }
            }
        }

        public DateTime? DataInsercao
        {
            get { return _dataInsercao; }
            set
            {
                if (value != _dataInsercao)
                {
                    _dataInsercao = value;
                    OnPropertyChanged(nameof(DataInsercao));
                }
            }
        }

        public int? TamanhoImagem
        {
            get { return _tamanhoImagem; }
            set
            {
                if (value != _tamanhoImagem)
                {
                    _tamanhoImagem = value;
                    OnPropertyChanged(nameof(TamanhoImagem));
                }
            }
        }

        public byte[]? Miniatura
        {
            get { return _miniatura; }
            set
            {
                if (value != _miniatura)
                {
                    _miniatura = value;
                    OnPropertyChanged(nameof(Miniatura));
                }
            }
        }

        public byte[]? Imagem
        {
            get { return _imagem; }
            set
            {
                if (value != _imagem)
                {
                    _imagem = value;
                    OnPropertyChanged(nameof(Imagem));
                }
            }
        }

        public string? NomePessoaInsercao
        {
            get { return _nomePessoaInsercao; }
            set
            {
                if (value != _nomePessoaInsercao)
                {
                    _nomePessoaInsercao = value;
                    OnPropertyChanged(nameof(NomePessoaInsercao));
                }
            }
        }

        public string? EmailPessoaInsercao
        {
            get { return _emailPessoaInsercao; }
            set
            {
                if (value != _emailPessoaInsercao)
                {
                    _emailPessoaInsercao = value;
                    OnPropertyChanged(nameof(EmailPessoaInsercao));
                }
            }
        }

        #endregion // Propriedades

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados do imagem da manifestação através do id
        /// </summary>
        /// <param name="id">Representa o id do imagem da manifestação que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetImagemManifestacaoDatabaseAsync(int? id, CancellationToken ct)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Utilização da conexão
            using (var db = new ConexaoProreportsMySQL())
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
                using (var command = db.conexaoProreports.CreateCommand())
                {
                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT "
                                         + "imrm.id_imagem_registro_manifestacao AS IdImagemRegistroManifestacao, "
                                         + "imrm.data_insercao AS DataInsercao, "
                                         + "imrm.miniatura AS Miniatura, "
                                         + "imrm.imagem AS Imagem, "
                                         + "imrm.tamanho_imagem AS TamanhoImagem, "
                                         + "imrm.nome_pessoa_insercao AS NomePessoaInsercao, "
                                         + "imrm.email_pessoa_insercao AS EmailPessoaInsercao, "
                                         + "imrm.id_registro_manifestacao AS IdRegistroManifestacao "
                                         + "FROM tb_imagens_registro_manifestacoes AS imrm "
                                         + "WHERE imrm.id_imagem_registro_manifestacao = @id";
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
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdImagemRegistroManifestacao"]);
                                DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                Miniatura = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Miniatura"]);
                                Imagem = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Imagem"]);
                                TamanhoImagem = FuncoesDeConversao.ConverteParaInt(reader["TamanhoImagem"]);
                                NomePessoaInsercao = FuncoesDeConversao.ConverteParaString(reader["NomePessoaInsercao"]);
                                EmailPessoaInsercao = FuncoesDeConversao.ConverteParaString(reader["EmailPessoaInsercao"]);
                                IdRegistroManifestacao = FuncoesDeConversao.ConverteParaInt(reader["IdRegistroManifestacao"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados da imagem da manifestação através de condições
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetImagemManifestacaoDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Utilização da conexão
            using (var db = new ConexaoProreportsMySQL())
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
                using (var command = db.conexaoProreports.CreateCommand())
                {
                    // Define o comando
                    string comando = "SELECT "
                                    + "imrm.id_imagem_registro_manifestacao AS IdImagemRegistroManifestacao, "
                                    + "imrm.data_insercao AS DataInsercao, "
                                    + "imrm.miniatura AS Miniatura, "
                                    + "imrm.imagem AS Imagem, "
                                    + "imrm.tamanho_imagem AS TamanhoImagem, "
                                    + "imrm.nome_pessoa_insercao AS NomePessoaInsercao, "
                                    + "imrm.email_pessoa_insercao AS EmailPessoaInsercao, "
                                    + "imrm.id_registro_manifestacao AS IdRegistroManifestacao "
                                    + "FROM tb_imagens_registro_manifestacoes AS imrm "
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
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdImagemRegistroManifestacao"]);
                                DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                Miniatura = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Miniatura"]);
                                Imagem = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Imagem"]);
                                TamanhoImagem = FuncoesDeConversao.ConverteParaInt(reader["TamanhoImagem"]);
                                NomePessoaInsercao = FuncoesDeConversao.ConverteParaString(reader["NomePessoaInsercao"]);
                                EmailPessoaInsercao = FuncoesDeConversao.ConverteParaString(reader["EmailPessoaInsercao"]);
                                IdRegistroManifestacao = FuncoesDeConversao.ConverteParaInt(reader["IdRegistroManifestacao"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que insere o imagem da manifestação na database
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarImagemManifestacaoDatabaseAsync(CancellationToken ct)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Utilização da conexão
            using (var db = new ConexaoProreportsMySQL())
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_imagem_manifestacao", db.conexaoProreports))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_imagem", Imagem);
                        command.Parameters.AddWithValue("p_tamanho_imagem", TamanhoImagem);
                        command.Parameters.AddWithValue("p_nome_pessoa_insercao", NomePessoaInsercao);
                        command.Parameters.AddWithValue("p_email_pessoa_insercao", EmailPessoaInsercao);
                        command.Parameters.AddWithValue("p_id_registro_manifestacao", IdRegistroManifestacao);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException("A imagem da manifestação já existe");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta a imagem na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletaImagemManifestacaoDatabaseAsync(CancellationToken ct, string nomePessoaAlteracao, string emailPessoaAlteracao)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Utilização da conexão
            using (var db = new ConexaoProreportsMySQL())
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_imagem_manifestacao", db.conexaoProreports))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_imagem_registro_manifestacao", Id);
                    command.Parameters.AddWithValue("p_nome_pessoa_insercao", nomePessoaAlteracao);
                    command.Parameters.AddWithValue("p_email_pessoa_insercao", emailPessoaAlteracao);
                    command.Parameters.AddWithValue("p_id_registro_manifestacao", IdRegistroManifestacao);
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
                            throw new ChaveEstrangeiraEmUsoException(nameof(ImagemRegistroManifestacao).ToLower(), "imagem");
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException(nameof(ImagemRegistroManifestacao).ToLower(), "imagem");
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de imagens de manifestação com os argumentos utilizados
        /// </summary>
        /// <param name="listaImagensManifestacao">Representa a lista de imagens de manifestação que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaImagensManifestacaoAsync(ObservableCollection<ImagemRegistroManifestacao> listaImagensManifestacao, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaImagensManifestacao == null)
            {
                listaImagensManifestacao = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaImagensManifestacao.Clear();
            }

            // Utilização da conexão
            using (var db = new ConexaoProreportsMySQL())
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
                                  + "imrm.id_imagem_registro_manifestacao AS IdImagemRegistroManifestacao, "
                                  + "imrm.data_insercao AS DataInsercao, "
                                  + "imrm.miniatura AS Miniatura, "
                                  + "imrm.imagem AS Imagem, "
                                  + "imrm.tamanho_imagem AS TamanhoImagem, "
                                  + "imrm.nome_pessoa_insercao AS NomePessoaInsercao, "
                                  + "imrm.email_pessoa_insercao AS EmailPessoaInsercao, "
                                  + "imrm.id_registro_manifestacao AS IdRegistroManifestacao "
                                  + "FROM tb_imagens_registro_manifestacoes AS imrm "
                                  + condicoesExtras;

                // Cria e atribui a variável do total de linhas através da função específica para contagem de linhas
                int totalLinhas = await FuncoesDeDatabase.GetQuantidadeLinhasReaderAsync(db, comando, ct, nomesParametrosSeparadosPorVirgulas, valoresParametros);

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Utilização do comando
                using (var command = db.conexaoProreports.CreateCommand())
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
                                ImagemRegistroManifestacao item = new();

                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdImagemRegistroManifestacao"]);
                                item.DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                item.Miniatura = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Miniatura"]);
                                item.Imagem = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["Imagem"]);
                                item.TamanhoImagem = FuncoesDeConversao.ConverteParaInt(reader["TamanhoImagem"]);
                                item.NomePessoaInsercao = FuncoesDeConversao.ConverteParaString(reader["NomePessoaInsercao"]);
                                item.EmailPessoaInsercao = FuncoesDeConversao.ConverteParaString(reader["EmailPessoaInsercao"]);
                                item.IdRegistroManifestacao = FuncoesDeConversao.ConverteParaInt(reader["IdRegistroManifestacao"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaImagensManifestacao.Add(item);

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
            ImagemRegistroManifestacao imagemManifestacaoCopia = new();
            imagemManifestacaoCopia.Id = Id;
            imagemManifestacaoCopia.DataInsercao = DataInsercao;
            imagemManifestacaoCopia.Miniatura = Miniatura;
            imagemManifestacaoCopia.Imagem = Imagem;
            imagemManifestacaoCopia.TamanhoImagem = TamanhoImagem;
            imagemManifestacaoCopia.NomePessoaInsercao = NomePessoaInsercao;
            imagemManifestacaoCopia.EmailPessoaInsercao = EmailPessoaInsercao;
            imagemManifestacaoCopia.IdRegistroManifestacao = IdRegistroManifestacao;

            return imagemManifestacaoCopia;
        }

        #endregion // Interfaces
    }
}