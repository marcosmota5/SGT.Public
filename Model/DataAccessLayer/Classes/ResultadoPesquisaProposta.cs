using Humanizer;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;
using System.Collections.ObjectModel;

namespace Model.DataAccessLayer.Classes
{
    public class ResultadoPesquisaProposta : ObservableObject, ICloneable
    {
        #region Campos

        private DateTime? _dataInsercao;
        private string? _nomeUsuario;
        private int? _idItemProposta;
        private int? _idProposta;
        private string? _codigoProposta;
        private DateTime? _dataSolicitacao;
        private string? _nomeCliente;
        private string? _nomeContato;
        private DateTime? _dataEnvio;
        private decimal? _quantidadeTotal;
        private decimal? _valorPecas;
        private decimal? _valorServicos;
        private string? _nomeStatusAprovacao;
        private string? _nomeJustificativaAprovacao;
        private decimal? _valorTotal;
        private string? _nomeSerie;
        private decimal? _valorFaturamento;
        private DateTime? _dataEnvioFaturamento;
        private DateTime? _dataFaturamento;
        private DateTime? _dataAprovacao;
        private int? _notaFiscal;
        private string? _tempoResposta;
        private string? _tempoFaturamento;

        #endregion Campos

        #region Propriedades

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

        public string? NomeUsuario
        {
            get { return _nomeUsuario; }
            set
            {
                if (value != _nomeUsuario)
                {
                    _nomeUsuario = value;
                    OnPropertyChanged(nameof(NomeUsuario));
                }
            }
        }

        public int? IdItemProposta
        {
            get { return _idItemProposta; }
            set
            {
                if (value != _idItemProposta)
                {
                    _idItemProposta = value;
                    OnPropertyChanged(nameof(IdItemProposta));
                }
            }
        }

        public int? IdProposta
        {
            get { return _idProposta; }
            set
            {
                if (value != _idProposta)
                {
                    _idProposta = value;
                    OnPropertyChanged(nameof(IdProposta));
                }
            }
        }

        public string? CodigoProposta
        {
            get { return _codigoProposta; }
            set
            {
                if (value != _codigoProposta)
                {
                    _codigoProposta = value;
                    OnPropertyChanged(nameof(CodigoProposta));
                }
            }
        }

        public DateTime? DataSolicitacao
        {
            get { return _dataSolicitacao; }
            set
            {
                if (value != _dataSolicitacao)
                {
                    _dataSolicitacao = value;
                    OnPropertyChanged(nameof(DataSolicitacao));
                }
            }
        }

        public string? NomeCliente
        {
            get { return _nomeCliente; }
            set
            {
                if (value != _nomeCliente)
                {
                    _nomeCliente = value;
                    OnPropertyChanged(nameof(NomeCliente));
                }
            }
        }

        public string? NomeContato
        {
            get { return _nomeContato; }
            set
            {
                if (value != _nomeContato)
                {
                    _nomeContato = value;
                    OnPropertyChanged(nameof(NomeContato));
                }
            }
        }

        public DateTime? DataEnvio
        {
            get { return _dataEnvio; }
            set
            {
                if (value != _dataEnvio)
                {
                    _dataEnvio = value;
                    OnPropertyChanged(nameof(DataEnvio));
                }
            }
        }

        public decimal? QuantidadeTotal
        {
            get { return _quantidadeTotal; }
            set
            {
                if (value != _quantidadeTotal)
                {
                    _quantidadeTotal = value;
                    OnPropertyChanged(nameof(QuantidadeTotal));
                }
            }
        }

        public decimal? ValorPecas
        {
            get { return _valorPecas; }
            set
            {
                if (value != _valorPecas)
                {
                    _valorPecas = value;
                    OnPropertyChanged(nameof(ValorPecas));
                }
            }
        }

        public decimal? ValorServicos
        {
            get { return _valorServicos; }
            set
            {
                if (value != _valorServicos)
                {
                    _valorServicos = value;
                    OnPropertyChanged(nameof(ValorServicos));
                }
            }
        }

        public string? NomeStatusAprovacao
        {
            get { return _nomeStatusAprovacao; }
            set
            {
                if (value != _nomeStatusAprovacao)
                {
                    _nomeStatusAprovacao = value;
                    OnPropertyChanged(nameof(NomeStatusAprovacao));
                }
            }
        }

        public string? NomeJustificativaAprovacao
        {
            get { return _nomeJustificativaAprovacao; }
            set
            {
                if (value != _nomeJustificativaAprovacao)
                {
                    _nomeJustificativaAprovacao = value;
                    OnPropertyChanged(nameof(NomeJustificativaAprovacao));
                }
            }
        }

        public decimal? ValorTotal
        {
            get { return _valorTotal; }
            set
            {
                if (value != _valorTotal)
                {
                    _valorTotal = value;
                    OnPropertyChanged(nameof(ValorTotal));
                }
            }
        }

        public string? NomeSerie
        {
            get { return _nomeSerie; }
            set
            {
                if (value != _nomeSerie)
                {
                    _nomeSerie = value;
                    OnPropertyChanged(nameof(NomeSerie));
                }
            }
        }

        public decimal? ValorFaturamento
        {
            get { return _valorFaturamento; }
            set
            {
                if (value != _valorFaturamento)
                {
                    _valorFaturamento = value;
                    OnPropertyChanged(nameof(ValorFaturamento));
                }
            }
        }

        public DateTime? DataEnvioFaturamento
        {
            get { return _dataEnvioFaturamento; }
            set
            {
                if (value != _dataEnvioFaturamento)
                {
                    _dataEnvioFaturamento = value;
                    OnPropertyChanged(nameof(DataEnvioFaturamento));
                }
            }
        }

        public DateTime? DataFaturamento
        {
            get { return _dataFaturamento; }
            set
            {
                if (value != _dataFaturamento)
                {
                    _dataFaturamento = value;
                    OnPropertyChanged(nameof(DataFaturamento));
                }
            }
        }

        public DateTime? DataAprovacao
        {
            get { return _dataAprovacao; }
            set
            {
                if (value != _dataAprovacao)
                {
                    _dataAprovacao = value;
                    OnPropertyChanged(nameof(DataAprovacao));
                }
            }
        }

        public int? NotaFiscal
        {
            get { return _notaFiscal; }
            set
            {
                if (value != _notaFiscal)
                {
                    _notaFiscal = value;
                    OnPropertyChanged(nameof(NotaFiscal));
                }
            }
        }

        public string? TempoResposta
        {
            get { return _tempoResposta; }
            set
            {
                if (value != _tempoResposta)
                {
                    _tempoResposta = value;
                    OnPropertyChanged(nameof(TempoResposta));
                }
            }
        }

        public string? TempoFaturamento
        {
            get { return _tempoFaturamento; }
            set
            {
                if (value != _tempoFaturamento)
                {
                    _tempoFaturamento = value;
                    OnPropertyChanged(nameof(TempoFaturamento));
                }
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Método assíncrono que preenche uma lista de resultados de pesquisa de proposta com os argumentos utilizados
        /// </summary>
        /// <param name="listaResultadosPesquisaProposta">Representa a lista de resultados de pesquisa de proposta que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="stringComando">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaResultadosPesquisaPropostaAsync(ObservableCollection<ResultadoPesquisaProposta> listaResultadosPesquisaProposta, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string stringComando, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaResultadosPesquisaProposta == null)
            {
                listaResultadosPesquisaProposta = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaResultadosPesquisaProposta.Clear();
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
                string comando = stringComando;

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
                                ResultadoPesquisaProposta item = new();

                                // Define as propriedades
                                item.DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                item.NomeUsuario = FuncoesDeConversao.ConverteParaString(reader["NomeUsuario"]);
                                item.IdItemProposta = FuncoesDeConversao.ConverteParaInt(reader["IdItemProposta"]);
                                item.IdProposta = FuncoesDeConversao.ConverteParaInt(reader["IdProposta"]);
                                item.CodigoProposta = FuncoesDeConversao.ConverteParaString(reader["CodigoProposta"]);
                                item.DataSolicitacao = FuncoesDeConversao.ConverteParaDateTime(reader["DataSolicitacao"]);
                                item.NomeCliente = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                item.NomeContato = FuncoesDeConversao.ConverteParaString(reader["NomeContato"]);
                                item.DataEnvio = FuncoesDeConversao.ConverteParaDateTime(reader["DataEnvio"]);
                                item.QuantidadeTotal = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeTotal"]);
                                item.ValorPecas = FuncoesDeConversao.ConverteParaDecimal(reader["ValorPecas"]);
                                item.ValorServicos = FuncoesDeConversao.ConverteParaDecimal(reader["ValorServicos"]);
                                item.NomeStatusAprovacao = FuncoesDeConversao.ConverteParaString(reader["NomeStatusAprovacao"]);
                                item.NomeJustificativaAprovacao = FuncoesDeConversao.ConverteParaString(reader["NomeJustificativaAprovacao"]);
                                item.ValorTotal = FuncoesDeConversao.ConverteParaDecimal(reader["ValorTotal"]);
                                item.NomeSerie = FuncoesDeConversao.ConverteParaString(reader["NomeSerie"]);
                                item.ValorFaturamento = FuncoesDeConversao.ConverteParaDecimal(reader["ValorFaturamento"]);
                                item.DataEnvioFaturamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataEnvioFaturamento"]);
                                item.DataFaturamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataFaturamento"]);
                                item.DataAprovacao = FuncoesDeConversao.ConverteParaDateTime(reader["DataAprovacao"]);
                                item.NotaFiscal = FuncoesDeConversao.ConverteParaInt(reader["NotaFiscal"]);

                                try
                                {
                                    DateTime dataInicioResposta = item.DataSolicitacao == null ? DateTime.Now : (DateTime)item.DataSolicitacao;
                                    DateTime dataFimResposta = item.DataEnvio == null ? DateTime.Now : (DateTime)item.DataEnvio;

                                    TimeSpan tempoResposta = dataFimResposta - dataInicioResposta;

                                    item.TempoResposta = tempoResposta.Humanize(2);
                                }
                                catch (Exception)
                                {
                                }

                                try
                                {
                                    if (item.DataEnvioFaturamento != null)
                                    {
                                        DateTime dataInicioFaturamento = item.DataEnvioFaturamento == null ? DateTime.Now : (DateTime)item.DataEnvioFaturamento;
                                        DateTime dataFimFaturamento = item.DataFaturamento == null ? DateTime.Now : (DateTime)item.DataFaturamento;

                                        TimeSpan tempoFaturamento = dataInicioFaturamento - dataFimFaturamento;

                                        item.TempoFaturamento = tempoFaturamento.Humanize(2);
                                    }
                                }
                                catch (Exception)
                                {
                                }

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaResultadosPesquisaProposta.Add(item);

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
            ResultadoPesquisaProposta resultadoPesquisaPropostaCopia = new();

            resultadoPesquisaPropostaCopia.NomeUsuario = NomeUsuario;
            resultadoPesquisaPropostaCopia.IdItemProposta = IdItemProposta;
            resultadoPesquisaPropostaCopia.IdProposta = IdProposta;
            resultadoPesquisaPropostaCopia.CodigoProposta = CodigoProposta;
            resultadoPesquisaPropostaCopia.DataSolicitacao = DataSolicitacao;
            resultadoPesquisaPropostaCopia.NomeCliente = NomeCliente;
            resultadoPesquisaPropostaCopia.NomeContato = NomeContato;
            resultadoPesquisaPropostaCopia.DataEnvio = DataEnvio;
            resultadoPesquisaPropostaCopia.QuantidadeTotal = QuantidadeTotal;
            resultadoPesquisaPropostaCopia.ValorPecas = ValorPecas;
            resultadoPesquisaPropostaCopia.ValorServicos = ValorServicos;
            resultadoPesquisaPropostaCopia.NomeStatusAprovacao = NomeStatusAprovacao;
            resultadoPesquisaPropostaCopia.NomeJustificativaAprovacao = NomeJustificativaAprovacao;
            resultadoPesquisaPropostaCopia.ValorTotal = ValorTotal;
            resultadoPesquisaPropostaCopia.NomeSerie = NomeSerie;
            resultadoPesquisaPropostaCopia.ValorFaturamento = ValorFaturamento;
            resultadoPesquisaPropostaCopia.DataEnvioFaturamento = DataEnvioFaturamento;
            resultadoPesquisaPropostaCopia.DataFaturamento = DataFaturamento;
            resultadoPesquisaPropostaCopia.DataAprovacao = DataAprovacao;
            resultadoPesquisaPropostaCopia.NotaFiscal = NotaFiscal;

            return resultadoPesquisaPropostaCopia;
        }

        #endregion Interfaces
    }
}