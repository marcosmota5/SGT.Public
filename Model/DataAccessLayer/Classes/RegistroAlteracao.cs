﻿using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class RegistroAlteracao : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private string? _descricao;
        private Versao? _versao;

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

        public string? Descricao
        {
            get { return _descricao; }
            set
            {
                if (value != _descricao)
                {
                    _descricao = value;
                    OnPropertyChanged(nameof(Descricao));
                }
            }
        }

        public Versao? Versao
        {
            get { return _versao; }
            set
            {
                if (value != _versao)
                {
                    _versao = value;
                    OnPropertyChanged(nameof(Versao));
                }
            }
        }

        #endregion // Propriedades

        #region Construtores
        /// <summary>
        /// Construtor do registro da alteração com os parâmetros utilizados
        /// </summary>
        /// <param name="inicializaVersao">Indica se a classe deve ser inicializada. Deve-se ter cuidado e levar em consideração loops infinitos</param>
        public RegistroAlteracao(bool inicializaVersao = false)
        {
            if (inicializaVersao)
            {
                Versao = new();
            }
        }

        #endregion // Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados do registro da alteração através do id
        /// </summary>
        /// <param name="id">Representa o id do registro da alteração que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetRegistroAlteracaoDatabaseAsync(int? id, CancellationToken ct, bool retornaVersao = false)
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
                                          + "realt.id_registro_alteracao AS IdRegistroAlteracao, "
                                          + "realt.descricao AS Descricao, "
                                          + "vers.id_versao AS IdVersao, "
                                          + "vers.nome AS NomeVersao, "
                                          + "vers.data_lancamento AS DataLancamento, "
                                          + "vers.e_critica AS EhCritica "
                                          + "FROM tb_registro_alteracoes AS realt "
                                          + "LEFT JOIN tb_versoes AS vers ON vers.id_versao = realt.id_versao "
                                          + "WHERE realt.id_registro_alteracao = @id";
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
                                if (retornaVersao)
                                {
                                    if (Versao == null)
                                    {
                                        Versao = new();
                                    }
                                }

                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdRegistroAlteracao"]);
                                Descricao = FuncoesDeConversao.ConverteParaString(reader["Descricao"]);

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaVersao)
                                {
                                    Versao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdVersao"]);
                                    Versao.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeVersao"]);
                                    Versao.DataLancamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataLancamento"]);
                                    Versao.EhCritica = FuncoesDeConversao.ConverteParaBool(reader["EhCritica"]);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados do registro da alteração através de condições
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetRegistroAlteracaoDatabaseAsync(CancellationToken ct, bool retornaVersao, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                                    + "realt.id_registro_alteracao AS IdRegistroAlteracao, "
                                    + "realt.descricao AS Descricao, "
                                    + "vers.id_versao AS IdVersao, "
                                    + "vers.nome AS NomeVersao, "
                                    + "vers.data_lancamento AS DataLancamento, "
                                    + "vers.e_critica AS EhCritica "
                                    + "FROM tb_registro_alteracoes AS realt "
                                    + "LEFT JOIN tb_versoes AS vers ON vers.id_versao = realt.id_versao "
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
                                if (retornaVersao)
                                {
                                    if (Versao == null)
                                    {
                                        Versao = new();
                                    }
                                }

                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdRegistroAlteracao"]);
                                Descricao = FuncoesDeConversao.ConverteParaString(reader["Descricao"]);

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaVersao)
                                {
                                    Versao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdVersao"]);
                                    Versao.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeVersao"]);
                                    Versao.DataLancamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataLancamento"]);
                                    Versao.EhCritica = FuncoesDeConversao.ConverteParaBool(reader["EhCritica"]);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de registros da alteração com os argumentos utilizados
        /// </summary>
        /// <param name="listaRegistrosAlteracao">Representa a lista de registros da alteração que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaRegistrosAlteracaoAsync(ObservableCollection<RegistroAlteracao> listaRegistrosAlteracao, bool limparLista, bool retornaVersao, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaRegistrosAlteracao == null)
            {
                listaRegistrosAlteracao = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaRegistrosAlteracao.Clear();
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
                                + "realt.id_registro_alteracao AS IdRegistroAlteracao, "
                                + "realt.descricao AS Descricao, "
                                + "vers.id_versao AS IdVersao, "
                                + "vers.nome AS NomeVersao, "
                                + "vers.data_lancamento AS DataLancamento, "
                                + "vers.e_critica AS EhCritica "
                                + "FROM tb_registro_alteracoes AS realt "
                                + "LEFT JOIN tb_versoes AS vers ON vers.id_versao = realt.id_versao "
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
                                RegistroAlteracao item = new();

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaVersao)
                                {
                                    if (item.Versao == null)
                                    {
                                        item.Versao = new();
                                    }
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdRegistroAlteracao"]);
                                item.Descricao = FuncoesDeConversao.ConverteParaString(reader["Descricao"]);

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (retornaVersao)
                                {
                                    item.Versao.Id = FuncoesDeConversao.ConverteParaInt(reader["IdVersao"]);
                                    item.Versao.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeVersao"]);
                                    item.Versao.DataLancamento = FuncoesDeConversao.ConverteParaDateTime(reader["DataLancamento"]);
                                    item.Versao.EhCritica = FuncoesDeConversao.ConverteParaBool(reader["EhCritica"]);
                                }

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaRegistrosAlteracao.Add(item);

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
            RegistroAlteracao registroAlteracaoCopia = new();
            registroAlteracaoCopia.Id = Id;
            registroAlteracaoCopia.Descricao = Descricao;

            return registroAlteracaoCopia;
        }

        #endregion // Interfaces
    }
}
