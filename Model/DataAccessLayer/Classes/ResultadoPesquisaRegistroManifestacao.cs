using Humanizer;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataAccessLayer.Classes
{
    public class ResultadoPesquisaRegistroManifestacao : ObservableObject, ICloneable
    {
        #region Campos

        private int? _idRegistroManifestacao;
        private string? _codigoManifestacao;
        private string? _prioridade;
        private string? _tipo;
        private string? _status;
        private DateTime? _dataAbertura;
        private string? _pessoaAbertura;
        private string? _descricaoAbertura;
        private string? _tempoEmAberto;

        #endregion // Campos

        #region Propriedades

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

        public string? CodigoManifestacao
        {
            get { return _codigoManifestacao; }
            set
            {
                if (value != _codigoManifestacao)
                {
                    _codigoManifestacao = value;
                    OnPropertyChanged(nameof(CodigoManifestacao));
                }
            }
        }

        public string? Prioridade
        {
            get { return _prioridade; }
            set
            {
                if (value != _prioridade)
                {
                    _prioridade = value;
                    OnPropertyChanged(nameof(Prioridade));
                }
            }
        }

        public string? Tipo
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

        public string? Status
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

        public DateTime? DataAbertura
        {
            get { return _dataAbertura; }
            set
            {
                if (value != _dataAbertura)
                {
                    _dataAbertura = value;
                    OnPropertyChanged(nameof(DataAbertura));
                }
            }
        }

        public string? PessoaAbertura
        {
            get { return _pessoaAbertura; }
            set
            {
                if (value != _pessoaAbertura)
                {
                    _pessoaAbertura = value;
                    OnPropertyChanged(nameof(PessoaAbertura));
                }
            }
        }

        public string? DescricaoAbertura
        {
            get { return _descricaoAbertura; }
            set
            {
                if (value != _descricaoAbertura)
                {
                    _descricaoAbertura = value;
                    OnPropertyChanged(nameof(DescricaoAbertura));
                }
            }
        }

        public string? TempoEmAberto
        {
            get { return _tempoEmAberto; }
            set
            {
                if (value != _tempoEmAberto)
                {
                    _tempoEmAberto = value;
                    OnPropertyChanged(nameof(TempoEmAberto));
                }
            }
        }

        #endregion // Propriedades

        #region Métodos

        /// <summary>
        /// Método assíncrono que preenche uma lista de resultados de pesquisa de registro de manifestação com os argumentos utilizados
        /// </summary>
        /// <param name="listaResultadosPesquisaRegistroManifestacao">Representa a lista de resultados de pesquisa de registro de manifestação que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="stringComando">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaResultadosPesquisaRegistroManifestacaoAsync(ObservableCollection<ResultadoPesquisaRegistroManifestacao> listaResultadosPesquisaRegistroManifestacao, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaResultadosPesquisaRegistroManifestacao == null)
            {
                listaResultadosPesquisaRegistroManifestacao = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaResultadosPesquisaRegistroManifestacao.Clear();
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
                                + "rema.id_registro_manifestacao AS IdRegistroManifestacao, "
                                + "rema.descricao_abertura AS DescricaoAbertura, "
                                + "rema.nome_pessoa_abertura AS PessoaAbertura, "
                                + "rema.data_abertura AS DataAbertura, "
                                + "rema.data_fechamento AS DataFechamento, "
                                + "prma.nome AS Prioridade, "
                                + "tima.nome AS Tipo, "
                                + "stma.nome AS Status "
                                + "FROM tb_registro_manifestacoes AS rema "
                                + "LEFT JOIN tb_prioridades_manifestacoes AS prma ON prma.id_prioridade_manifestacao = rema.id_prioridade_manifestacao "
                                + "LEFT JOIN tb_tipos_manifestacoes AS tima ON tima.id_tipo_manifestacao = rema.id_tipo_manifestacao "
                                + "LEFT JOIN tb_status_manifestacoes AS stma ON stma.id_status_manifestacao = rema.id_status_manifestacao "
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
                                ResultadoPesquisaRegistroManifestacao item = new();

                                // Define as propriedades
                                item.IdRegistroManifestacao = FuncoesDeConversao.ConverteParaInt(reader["IdRegistroManifestacao"]);
                                item.CodigoManifestacao = "REQN" + ((int)item.IdRegistroManifestacao).ToString("00000000");
                                item.Prioridade = FuncoesDeConversao.ConverteParaString(reader["Prioridade"]);
                                item.Tipo = FuncoesDeConversao.ConverteParaString(reader["Tipo"]);
                                item.Status = FuncoesDeConversao.ConverteParaString(reader["Status"]);
                                item.DataAbertura = FuncoesDeConversao.ConverteParaDateTime(reader["DataAbertura"]);
                                item.PessoaAbertura = FuncoesDeConversao.ConverteParaString(reader["PessoaAbertura"]);
                                item.DescricaoAbertura = FuncoesDeConversao.ConverteParaString(reader["DescricaoAbertura"]);

                                DateTime? dataFechamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataFechamento"]);

                                DateTime dataInicio = item.DataAbertura == null ? DateTime.Now : (DateTime)item.DataAbertura;
                                DateTime dataFim = dataFechamento == null ? DateTime.Now : (DateTime)dataFechamento;

                                TimeSpan tempoAberto = dataFim - dataInicio;

                                item.TempoEmAberto = tempoAberto.Humanize(2);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaResultadosPesquisaRegistroManifestacao.Add(item);

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
            ResultadoPesquisaRegistroManifestacao resultadoPesquisaRegistroManifestacaoCopia = new();

            resultadoPesquisaRegistroManifestacaoCopia.CodigoManifestacao = CodigoManifestacao;
            resultadoPesquisaRegistroManifestacaoCopia.IdRegistroManifestacao = IdRegistroManifestacao;
            resultadoPesquisaRegistroManifestacaoCopia.Prioridade = Prioridade;
            resultadoPesquisaRegistroManifestacaoCopia.DataAbertura = DataAbertura;
            resultadoPesquisaRegistroManifestacaoCopia.Tipo = Tipo;
            resultadoPesquisaRegistroManifestacaoCopia.Status = Status;
            resultadoPesquisaRegistroManifestacaoCopia.PessoaAbertura = PessoaAbertura;
            resultadoPesquisaRegistroManifestacaoCopia.DescricaoAbertura = DescricaoAbertura;
            resultadoPesquisaRegistroManifestacaoCopia.TempoEmAberto = TempoEmAberto;

            return resultadoPesquisaRegistroManifestacaoCopia;
        }

        #endregion // Interfaces
    }
}
