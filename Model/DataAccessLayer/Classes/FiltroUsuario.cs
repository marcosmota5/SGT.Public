using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class FiltroUsuario : ObservableObject, ICloneable
    {
        #region Campos

        private int _id;
        private int _idUsuario;
        private string _nome = string.Empty;
        private DateTime _dataCriacao = DateTime.Now;
        private int _idModulo;
        private ObservableCollection<ParametroFiltroPesquisa> _listaParametroFiltroPesquisaProposta;

        #endregion Campos

        #region Construtores

        public FiltroUsuario()
        {
            ListaParametroFiltroPesquisaProposta = new();
        }

        #endregion Construtores

        #region Propriedades

        public int Id
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

        public int IdUsuario
        {
            get { return _idUsuario; }
            set
            {
                if (value != _idUsuario)
                {
                    _idUsuario = value;
                    OnPropertyChanged(nameof(IdUsuario));
                }
            }
        }

        public string Nome
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

        public DateTime DataCriacao
        {
            get { return _dataCriacao; }
            set
            {
                if (value != _dataCriacao)
                {
                    _dataCriacao = value;
                    OnPropertyChanged(nameof(DataCriacao));
                }
            }
        }

        public int IdModulo
        {
            get { return _idModulo; }
            set
            {
                if (value != _idModulo)
                {
                    _idModulo = value;
                    OnPropertyChanged(nameof(IdModulo));
                }
            }
        }

        public ObservableCollection<ParametroFiltroPesquisa> ListaParametroFiltroPesquisaProposta
        {
            get { return _listaParametroFiltroPesquisaProposta; }
            set
            {
                if (value != _listaParametroFiltroPesquisaProposta)
                {
                    _listaParametroFiltroPesquisaProposta = value;
                    OnPropertyChanged(nameof(ListaParametroFiltroPesquisaProposta));
                }
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Método assíncrono que salva o parâmetro
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public async Task SalvarFiltroUsuarioDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_inserir_filtro_usuario", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario", IdUsuario);
                    command.Parameters.AddWithValue("p_nome", Nome);
                    command.Parameters.AddWithValue("p_data_criacao", DataCriacao);
                    command.Parameters.AddWithValue("p_id_modulo", IdModulo);
                    command.Parameters.Add("p_id_filtro_usuario", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);

                    Id = FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_filtro_usuario"].Value).GetValueOrDefault(1);
                }

                // Varre os filtros e insere-os na database
                foreach (var item in ListaParametroFiltroPesquisaProposta)
                {
                    item.IdFiltroUsuario = Id;
                    await item.SalvarParametroFiltroPesquisaPropostaDatabaseAsync(CancellationToken.None);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta o parâmetro
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public async Task DeletarFiltroUsuarioDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_filtro_usuario", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_filtro_usuario", Id);
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando
                    await command.ExecuteNonQueryAsync(ct);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista com os argumentos utilizados
        /// </summary>
        /// <param name="listaFiltroUsuario">Representa a lista que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaFiltroUsuarioAsync(ObservableCollection<FiltroUsuario> listaFiltroUsuario, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaFiltroUsuario == null)
            {
                listaFiltroUsuario = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaFiltroUsuario.Clear();
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
                  + "id_filtro_usuario AS Id, "
                  + "id_usuario AS IdUsuario, "
                  + "nome AS Nome, "
                  + "data_criacao AS DataCriacao, "
                  + "id_modulo AS IdModulo "
                  + "FROM tb_filtros_usuarios "
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
                                FiltroUsuario item = new();

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]).GetValueOrDefault(1);
                                item.IdUsuario = FuncoesDeConversao.ConverteParaInt(reader["IdUsuario"]).GetValueOrDefault(1);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["Nome"]).ToString();
                                item.DataCriacao = FuncoesDeConversao.ConverteParaDateTime(reader["DataCriacao"]).GetValueOrDefault(DateTime.Now);
                                item.IdModulo = FuncoesDeConversao.ConverteParaInt(reader["IdModulo"]).GetValueOrDefault(1);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaFiltroUsuario.Add(item);

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

            foreach (var item in listaFiltroUsuario)
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                await ParametroFiltroPesquisa.PreencheListaParametroFiltroPesquisaPropostaAsync(item.ListaParametroFiltroPesquisaProposta, true, null,
                    ct, "WHERE id_filtro_usuario = @id_filtro_usuario", "@id_filtro_usuario", item.Id);
            }
        }

        #endregion Métodos

        #region Interfaces

        /// <summary>
        /// Método para criar uma cópia da classe já que esse é um tipo de referência que não pode ser atribuído diretamente
        /// </summary>
        public object Clone()
        {
            FiltroUsuario filtroUsuarioClone = new();

            filtroUsuarioClone.Id = Id;
            filtroUsuarioClone.IdUsuario = IdUsuario;
            filtroUsuarioClone.Nome = Nome;
            filtroUsuarioClone.DataCriacao = DataCriacao;
            filtroUsuarioClone.IdModulo = IdModulo;

            if (ListaParametroFiltroPesquisaProposta != null)
            {
                foreach (var item in ListaParametroFiltroPesquisaProposta)
                {
                    filtroUsuarioClone.ListaParametroFiltroPesquisaProposta.Add(item);
                }
            }

            return filtroUsuarioClone;
        }

        #endregion Interfaces
    }
}