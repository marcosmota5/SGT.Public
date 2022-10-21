using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class ParametroFiltroPesquisa : ObservableObject, ICloneable
    {
        #region Campos

        private int _idFiltroUsuario;
        private string _tituloColuna;
        private string _textoFiltro;
        private string _coluna;
        private string _tipo;
        private string _operador1;
        private string _parametro1;
        private object _valor1;
        private string _operador2;
        private string _parametro2;
        private object _valor2;
        private string _tituloOperador;
        private string _operador;

        #endregion Campos

        #region Propriedades

        public int IdFiltroUsuario
        {
            get { return _idFiltroUsuario; }
            set
            {
                if (value != _idFiltroUsuario)
                {
                    _idFiltroUsuario = value;
                    OnPropertyChanged(nameof(IdFiltroUsuario));
                }
            }
        }

        public string TituloColuna
        {
            get { return _tituloColuna; }
            set
            {
                if (value != _tituloColuna)
                {
                    _tituloColuna = value;
                    OnPropertyChanged(nameof(TituloColuna));
                }
            }
        }

        public string TextoFiltro
        {
            get { return _textoFiltro; }
            set
            {
                if (value != _textoFiltro)
                {
                    _textoFiltro = value;
                    OnPropertyChanged(nameof(TextoFiltro));
                }
            }
        }

        public string Coluna
        {
            get { return _coluna; }
            set
            {
                if (value != _coluna)
                {
                    _coluna = value;
                    OnPropertyChanged(nameof(Coluna));
                }
            }
        }

        public string Tipo
        {
            get { return _tipo; }
            set
            {
                if (value != _tipo)
                {
                    _tipo = value;
                    OnPropertyChanged(nameof(Tipo));
                }
            }
        }

        public string Operador1
        {
            get { return _operador1; }
            set
            {
                if (value != _operador1)
                {
                    _operador1 = value;
                    OnPropertyChanged(nameof(Operador1));
                }
            }
        }

        public string Parametro1
        {
            get { return _parametro1; }
            set
            {
                if (value != _parametro1)
                {
                    _parametro1 = value;
                    OnPropertyChanged(nameof(Parametro1));
                }
            }
        }

        public object Valor1
        {
            get { return _valor1; }
            set
            {
                if (value != _valor1)
                {
                    _valor1 = value;
                    OnPropertyChanged(nameof(Valor1));
                }
            }
        }

        public string Operador2
        {
            get { return _operador2; }
            set
            {
                if (value != _operador2)
                {
                    _operador2 = value;
                    OnPropertyChanged(nameof(Operador2));
                }
            }
        }

        public string Parametro2
        {
            get { return _parametro2; }
            set
            {
                if (value != _parametro2)
                {
                    _parametro2 = value;
                    OnPropertyChanged(nameof(Parametro2));
                }
            }
        }

        public object Valor2
        {
            get { return _valor2; }
            set
            {
                if (value != _valor2)
                {
                    _valor2 = value;
                    OnPropertyChanged(nameof(Valor2));
                }
            }
        }

        public string TituloOperador
        {
            get { return _tituloOperador; }
            set
            {
                if (value != _tituloOperador)
                {
                    _tituloOperador = value;
                    OnPropertyChanged(nameof(TituloOperador));
                }
            }
        }

        public string Operador
        {
            get { return _operador; }
            set
            {
                if (value != _operador)
                {
                    _operador = value;
                    OnPropertyChanged(nameof(Operador));
                }
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Método assíncrono que salva o parâmetro do filtro da pesquisa de proposta na database
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public async Task SalvarParametroFiltroPesquisaPropostaDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_inserir_parametro_filtro_usuario", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_filtro_usuario", IdFiltroUsuario);
                    command.Parameters.AddWithValue("p_titulo_coluna", TituloColuna);
                    command.Parameters.AddWithValue("p_texto_filtro", TextoFiltro);
                    command.Parameters.AddWithValue("p_coluna", Coluna);
                    command.Parameters.AddWithValue("p_tipo", Tipo);
                    command.Parameters.AddWithValue("p_operador_1", Operador1);
                    command.Parameters.AddWithValue("p_parametro_1", Parametro1);
                    command.Parameters.AddWithValue("p_valor_1", Valor1);
                    command.Parameters.AddWithValue("p_operador_2", Operador2);
                    command.Parameters.AddWithValue("p_parametro_2", Parametro2);
                    command.Parameters.AddWithValue("p_valor_2", Valor2);
                    command.Parameters.AddWithValue("p_titulo_operador", TituloOperador);
                    command.Parameters.AddWithValue("p_operador", Operador);
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista com os argumentos utilizados
        /// </summary>
        /// <param name="listaParametroFiltroPesquisaProposta">Representa a lista deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaParametroFiltroPesquisaPropostaAsync(ObservableCollection<ParametroFiltroPesquisa> listaParametroFiltroPesquisaProposta, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaParametroFiltroPesquisaProposta == null)
            {
                listaParametroFiltroPesquisaProposta = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaParametroFiltroPesquisaProposta.Clear();
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
                  + "id_filtro_usuario AS IdFiltroUsuario, "
                  + "titulo_coluna AS TituloColuna, "
                  + "texto_filtro AS TextoFiltro, "
                  + "coluna AS Coluna, "
                  + "tipo AS Tipo, "
                  + "operador_1 AS Operador1, "
                  + "parametro_1 AS Parametro1, "
                  + "valor_1 AS Valor1, "
                  + "operador_2 AS Operador2, "
                  + "parametro_2 AS Parametro2, "
                  + "valor_2 AS Valor2, "
                  + "titulo_operador AS TituloOperador, "
                  + "operador AS Operador "
                  + "FROM tb_parametros_filtros_usuarios "
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
                                ParametroFiltroPesquisa item = new();

                                // Define as propriedades
                                item.IdFiltroUsuario = FuncoesDeConversao.ConverteParaInt(reader["IdFiltroUsuario"]).GetValueOrDefault(0);
                                item.TituloColuna = FuncoesDeConversao.ConverteParaString(reader["TituloColuna"]).ToString();
                                item.TextoFiltro = FuncoesDeConversao.ConverteParaString(reader["TextoFiltro"]).ToString();
                                item.Coluna = FuncoesDeConversao.ConverteParaString(reader["Coluna"]).ToString();
                                item.Tipo = FuncoesDeConversao.ConverteParaString(reader["Tipo"]).ToString();
                                item.Operador1 = FuncoesDeConversao.ConverteParaString(reader["Operador1"]).ToString();
                                item.Parametro1 = FuncoesDeConversao.ConverteParaString(reader["Parametro1"]).ToString();
                                item.Valor1 = FuncoesDeConversao.ConverteParaString(reader["Valor1"]).ToString();
                                item.Operador2 = FuncoesDeConversao.ConverteParaString(reader["Operador2"]).ToString();
                                item.Parametro2 = FuncoesDeConversao.ConverteParaString(reader["Parametro2"]).ToString();
                                item.Valor2 = FuncoesDeConversao.ConverteParaString(reader["Valor2"]).ToString();
                                item.TituloOperador = FuncoesDeConversao.ConverteParaString(reader["TituloOperador"]).ToString();
                                item.Operador = FuncoesDeConversao.ConverteParaString(reader["Operador"]).ToString();

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaParametroFiltroPesquisaProposta.Add(item);

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
            ParametroFiltroPesquisa parametroFiltroPesquisaPropostaClone = new();

            parametroFiltroPesquisaPropostaClone.IdFiltroUsuario = IdFiltroUsuario;
            parametroFiltroPesquisaPropostaClone.TituloColuna = TituloColuna;
            parametroFiltroPesquisaPropostaClone.TextoFiltro = TextoFiltro;
            parametroFiltroPesquisaPropostaClone.Coluna = Coluna;
            parametroFiltroPesquisaPropostaClone.Tipo = Tipo;
            parametroFiltroPesquisaPropostaClone.Operador1 = Operador1;
            parametroFiltroPesquisaPropostaClone.Parametro1 = Parametro1;
            parametroFiltroPesquisaPropostaClone.Valor1 = Valor1;
            parametroFiltroPesquisaPropostaClone.Operador2 = Operador2;
            parametroFiltroPesquisaPropostaClone.Parametro2 = Parametro2;
            parametroFiltroPesquisaPropostaClone.Valor2 = Valor2;
            parametroFiltroPesquisaPropostaClone.TituloOperador = TituloOperador;
            parametroFiltroPesquisaPropostaClone.Operador = Operador;

            return parametroFiltroPesquisaPropostaClone;
        }

        #endregion Interfaces
    }
}