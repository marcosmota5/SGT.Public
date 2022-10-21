using Humanizer;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;
using System.Collections.ObjectModel;

namespace Model.DataAccessLayer.Classes
{
    public class ResultadoPesquisaOrdemServico : ObservableObject, ICloneable
    {
        #region Campos

        private DateTime? _dataInsercao;
        private string? _nomeUsuario;
        private int? _idOrdemServico;
        private int? _numeroOrdemServicoAtual;
        private int? _numeroOrdemServicoPrimaria;
        private int? _numeroChamado;
        private DateTime? _dataChamado;
        private DateTime? _dataAtendimento;
        private string? _mastro;
        private string? _codigoFalha;
        private int? _etapasConcluidas;
        private decimal? _horimetro;
        private DateTime? _dataSaida;
        private DateTime? _dataChegada;
        private DateTime? _dataRetorno;
        private string? _nomeTipoOS;
        private string? _nomeCliente;
        private string? _nomeExecutanteServico;
        private string? _nomeSerie;
        private string? _nomeFabricante;
        private string? _nomeTipoEquipamento;
        private string? _nomeModelo;
        private string? _nomeEquipamentoAposManutencao;
        private bool? _equipamentoOperacional;
        private string? _nomeTipoManutencao;
        private decimal? _quantidadeItens;
        private decimal? _quantidadeEventos;
        private string? _tempoResposta;
        private string? _tempoAtendimento;

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

        public int? IdOrdemServico
        {
            get { return _idOrdemServico; }
            set
            {
                if (value != _idOrdemServico)
                {
                    _idOrdemServico = value;
                    OnPropertyChanged(nameof(IdOrdemServico));
                }
            }
        }

        public int? NumeroOrdemServicoAtual
        {
            get { return _numeroOrdemServicoAtual; }
            set
            {
                if (value != _numeroOrdemServicoAtual)
                {
                    _numeroOrdemServicoAtual = value;
                    OnPropertyChanged(nameof(NumeroOrdemServicoAtual));
                }
            }
        }

        public int? NumeroOrdemServicoPrimaria
        {
            get { return _numeroOrdemServicoPrimaria; }
            set
            {
                if (value != _numeroOrdemServicoPrimaria)
                {
                    _numeroOrdemServicoPrimaria = value;
                    OnPropertyChanged(nameof(NumeroOrdemServicoPrimaria));
                }
            }
        }

        public int? NumeroChamado
        {
            get { return _numeroChamado; }
            set
            {
                if (value != _numeroChamado)
                {
                    _numeroChamado = value;
                    OnPropertyChanged(nameof(NumeroChamado));
                }
            }
        }

        public DateTime? DataChamado
        {
            get { return _dataChamado; }
            set
            {
                if (value != _dataChamado)
                {
                    _dataChamado = value;
                    OnPropertyChanged(nameof(DataChamado));
                }
            }
        }

        public DateTime? DataAtendimento
        {
            get { return _dataAtendimento; }
            set
            {
                if (value != _dataAtendimento)
                {
                    _dataAtendimento = value;
                    OnPropertyChanged(nameof(DataAtendimento));
                }
            }
        }

        public string? Mastro
        {
            get { return _mastro; }
            set
            {
                if (value != _mastro)
                {
                    _mastro = value;
                    OnPropertyChanged(nameof(Mastro));
                }
            }
        }

        public string? CodigoFalha
        {
            get { return _codigoFalha; }
            set
            {
                if (value != _codigoFalha)
                {
                    _codigoFalha = value;
                    OnPropertyChanged(nameof(CodigoFalha));
                }
            }
        }

        public int? EtapasConcluidas
        {
            get { return _etapasConcluidas; }
            set
            {
                if (value != _etapasConcluidas)
                {
                    _etapasConcluidas = value;
                    OnPropertyChanged(nameof(EtapasConcluidas));
                }
            }
        }

        public decimal? Horimetro
        {
            get { return _horimetro; }
            set
            {
                if (value != _horimetro)
                {
                    _horimetro = value;
                    OnPropertyChanged(nameof(Horimetro));
                }
            }
        }

        public DateTime? DataSaida
        {
            get { return _dataSaida; }
            set
            {
                if (value != _dataSaida)
                {
                    _dataSaida = value;
                    OnPropertyChanged(nameof(DataSaida));
                }
            }
        }

        public DateTime? DataChegada
        {
            get { return _dataChegada; }
            set
            {
                if (value != _dataChegada)
                {
                    _dataChegada = value;
                    OnPropertyChanged(nameof(DataChegada));
                }
            }
        }

        public DateTime? DataRetorno
        {
            get { return _dataRetorno; }
            set
            {
                if (value != _dataRetorno)
                {
                    _dataRetorno = value;
                    OnPropertyChanged(nameof(DataRetorno));
                }
            }
        }

        public string? NomeTipoOS
        {
            get { return _nomeTipoOS; }
            set
            {
                if (value != _nomeTipoOS)
                {
                    _nomeTipoOS = value;
                    OnPropertyChanged(nameof(NomeTipoOS));
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

        public string? NomeExecutanteServico
        {
            get { return _nomeExecutanteServico; }
            set
            {
                if (value != _nomeExecutanteServico)
                {
                    _nomeExecutanteServico = value;
                    OnPropertyChanged(nameof(NomeExecutanteServico));
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

        public string? NomeFabricante
        {
            get { return _nomeFabricante; }
            set
            {
                if (value != _nomeFabricante)
                {
                    _nomeFabricante = value;
                    OnPropertyChanged(nameof(NomeFabricante));
                }
            }
        }

        public string? NomeTipoEquipamento
        {
            get { return _nomeTipoEquipamento; }
            set
            {
                if (value != _nomeTipoEquipamento)
                {
                    _nomeTipoEquipamento = value;
                    OnPropertyChanged(nameof(NomeTipoEquipamento));
                }
            }
        }

        public string? NomeModelo
        {
            get { return _nomeModelo; }
            set
            {
                if (value != _nomeModelo)
                {
                    _nomeModelo = value;
                    OnPropertyChanged(nameof(NomeModelo));
                }
            }
        }

        public string? NomeEquipamentoAposManutencao
        {
            get { return _nomeEquipamentoAposManutencao; }
            set
            {
                if (value != _nomeEquipamentoAposManutencao)
                {
                    _nomeEquipamentoAposManutencao = value;
                    OnPropertyChanged(nameof(NomeEquipamentoAposManutencao));
                }
            }
        }

        public bool? EquipamentoOperacional
        {
            get { return _equipamentoOperacional; }
            set
            {
                if (value != _equipamentoOperacional)
                {
                    _equipamentoOperacional = value;
                    OnPropertyChanged(nameof(EquipamentoOperacional));
                }
            }
        }

        public string? NomeTipoManutencao
        {
            get { return _nomeTipoManutencao; }
            set
            {
                if (value != _nomeTipoManutencao)
                {
                    _nomeTipoManutencao = value;
                    OnPropertyChanged(nameof(NomeTipoManutencao));
                }
            }
        }

        public decimal? QuantidadeItens
        {
            get { return _quantidadeItens; }
            set
            {
                if (value != _quantidadeItens)
                {
                    _quantidadeItens = value;
                    OnPropertyChanged(nameof(QuantidadeItens));
                }
            }
        }

        public decimal? QuantidadeEventos
        {
            get { return _quantidadeEventos; }
            set
            {
                if (value != _quantidadeEventos)
                {
                    _quantidadeEventos = value;
                    OnPropertyChanged(nameof(QuantidadeEventos));
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

        public string? TempoAtendimento
        {
            get { return _tempoAtendimento; }
            set
            {
                if (value != _tempoAtendimento)
                {
                    _tempoAtendimento = value;
                    OnPropertyChanged(nameof(TempoAtendimento));
                }
            }
        }

        #endregion Propriedades

        #region Métodos

        /// <summary>
        /// Método assíncrono que preenche uma lista de resultados de pesquisa de ordens de serviço com os argumentos utilizados
        /// </summary>
        /// <param name="listaResultadosPesquisaOrdemServico">Representa a lista de resultados de pesquisa de ordens de serviço que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="stringComando">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaResultadosPesquisaOrdemServicoAsync(ObservableCollection<ResultadoPesquisaOrdemServico> listaResultadosPesquisaOrdemServico, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string stringComando, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaResultadosPesquisaOrdemServico == null)
            {
                listaResultadosPesquisaOrdemServico = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaResultadosPesquisaOrdemServico.Clear();
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
                                ResultadoPesquisaOrdemServico item = new();

                                // Define as propriedades
                                item.DataInsercao = FuncoesDeConversao.ConverteParaDateTime(reader["DataInsercao"]);
                                item.NomeUsuario = FuncoesDeConversao.ConverteParaString(reader["NomeUsuario"]);
                                item.IdOrdemServico = FuncoesDeConversao.ConverteParaInt(reader["IdOrdemServico"]);
                                item.NumeroOrdemServicoAtual = FuncoesDeConversao.ConverteParaInt(reader["NumeroOrdemServicoAtual"]);
                                item.NumeroOrdemServicoPrimaria = FuncoesDeConversao.ConverteParaInt(reader["NumeroOrdemServicoPrimaria"]);
                                item.NumeroChamado = FuncoesDeConversao.ConverteParaInt(reader["NumeroChamado"]);
                                item.DataChamado = FuncoesDeConversao.ConverteParaDateTime(reader["DataChamado"]);
                                item.DataAtendimento = FuncoesDeConversao.ConverteParaDateTime(reader["DataAtendimento"]);
                                item.Mastro = FuncoesDeConversao.ConverteParaString(reader["Mastro"]);
                                item.CodigoFalha = FuncoesDeConversao.ConverteParaString(reader["CodigoFalha"]);
                                item.EtapasConcluidas = FuncoesDeConversao.ConverteParaInt(reader["EtapasConcluidas"]);
                                item.Horimetro = FuncoesDeConversao.ConverteParaDecimal(reader["Horimetro"]);
                                item.DataSaida = FuncoesDeConversao.ConverteParaDateTime(reader["DataSaida"]);
                                item.DataChegada = FuncoesDeConversao.ConverteParaDateTime(reader["DataChegada"]);
                                item.DataRetorno = FuncoesDeConversao.ConverteParaDateTime(reader["DataRetorno"]);
                                item.NomeTipoOS = FuncoesDeConversao.ConverteParaString(reader["NomeTipoOS"]);
                                item.NomeCliente = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                item.NomeExecutanteServico = FuncoesDeConversao.ConverteParaString(reader["NomeExecutanteServico"]);
                                item.NomeSerie = FuncoesDeConversao.ConverteParaString(reader["NomeSerie"]);
                                item.NomeFabricante = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                item.NomeTipoEquipamento = FuncoesDeConversao.ConverteParaString(reader["NomeTipoEquipamento"]);
                                item.NomeModelo = FuncoesDeConversao.ConverteParaString(reader["NomeModelo"]);
                                item.NomeEquipamentoAposManutencao = FuncoesDeConversao.ConverteParaString(reader["NomeEquipamentoAposManutencao"]);
                                item.EquipamentoOperacional = FuncoesDeConversao.ConverteParaBool(reader["EquipamentoOperacional"]);
                                item.NomeTipoManutencao = FuncoesDeConversao.ConverteParaString(reader["NomeTipoManutencao"]);
                                item.QuantidadeItens = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeItens"]);
                                item.QuantidadeEventos = FuncoesDeConversao.ConverteParaDecimal(reader["QuantidadeEventos"]);

                                try
                                {
                                    DateTime dataInicioResposta = item.DataChamado == null ? DateTime.Now : (DateTime)item.DataChamado;
                                    DateTime dataFimResposta = item.DataAtendimento == null ? DateTime.Now : (DateTime)item.DataAtendimento;

                                    TimeSpan tempoResposta = dataFimResposta - dataInicioResposta;

                                    item.TempoResposta = tempoResposta.Humanize(2);
                                }
                                catch (Exception)
                                {
                                }

                                try
                                {
                                    DateTime dataSaida = item.DataSaida == null ? DateTime.Now : (DateTime)item.DataSaida;
                                    DateTime dataChegada = item.DataChegada == null ? DateTime.Now : (DateTime)item.DataChegada;
                                    DateTime dataRetorno = item.DataRetorno == null ? DateTime.Now : (DateTime)item.DataRetorno;

                                    TimeSpan tempoChegadaRetorno = dataRetorno - dataChegada;
                                    TimeSpan tempoSaidaChegada = dataChegada - dataSaida;
                                    TimeSpan tempoTotal = tempoChegadaRetorno - tempoSaidaChegada;

                                    item.TempoAtendimento = tempoTotal.Humanize(2);
                                }
                                catch (Exception)
                                {
                                }

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaResultadosPesquisaOrdemServico.Add(item);

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
            ResultadoPesquisaOrdemServico resultadoPesquisaOrdemServicoCopia = new();

            resultadoPesquisaOrdemServicoCopia.NomeUsuario = NomeUsuario;
            resultadoPesquisaOrdemServicoCopia.IdOrdemServico = IdOrdemServico;
            resultadoPesquisaOrdemServicoCopia.NumeroOrdemServicoAtual = NumeroOrdemServicoAtual;
            resultadoPesquisaOrdemServicoCopia.NumeroOrdemServicoPrimaria = NumeroOrdemServicoPrimaria;
            resultadoPesquisaOrdemServicoCopia.NumeroChamado = NumeroChamado;
            resultadoPesquisaOrdemServicoCopia.DataChamado = DataChamado;
            resultadoPesquisaOrdemServicoCopia.DataAtendimento = DataAtendimento;
            resultadoPesquisaOrdemServicoCopia.Mastro = Mastro;
            resultadoPesquisaOrdemServicoCopia.CodigoFalha = CodigoFalha;
            resultadoPesquisaOrdemServicoCopia.EtapasConcluidas = EtapasConcluidas;
            resultadoPesquisaOrdemServicoCopia.Horimetro = Horimetro;
            resultadoPesquisaOrdemServicoCopia.DataSaida = DataSaida;
            resultadoPesquisaOrdemServicoCopia.DataChegada = DataChegada;
            resultadoPesquisaOrdemServicoCopia.DataRetorno = DataRetorno;
            resultadoPesquisaOrdemServicoCopia.NomeTipoOS = NomeTipoOS;
            resultadoPesquisaOrdemServicoCopia.NomeCliente = NomeCliente;
            resultadoPesquisaOrdemServicoCopia.NomeExecutanteServico = NomeExecutanteServico;
            resultadoPesquisaOrdemServicoCopia.NomeSerie = NomeSerie;
            resultadoPesquisaOrdemServicoCopia.NomeTipoEquipamento = NomeTipoEquipamento;
            resultadoPesquisaOrdemServicoCopia.NomeModelo = NomeModelo;
            resultadoPesquisaOrdemServicoCopia.NomeEquipamentoAposManutencao = NomeEquipamentoAposManutencao;
            resultadoPesquisaOrdemServicoCopia.EquipamentoOperacional = EquipamentoOperacional;
            resultadoPesquisaOrdemServicoCopia.NomeTipoManutencao = NomeTipoManutencao;
            resultadoPesquisaOrdemServicoCopia.QuantidadeItens = QuantidadeItens;
            resultadoPesquisaOrdemServicoCopia.QuantidadeEventos = QuantidadeEventos;
            resultadoPesquisaOrdemServicoCopia.TempoResposta = TempoResposta;
            resultadoPesquisaOrdemServicoCopia.TempoAtendimento = TempoAtendimento;

            return resultadoPesquisaOrdemServicoCopia;
        }

        #endregion Interfaces
    }
}