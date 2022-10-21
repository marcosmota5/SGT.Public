using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;
using System.Collections.ObjectModel;

namespace Model.DataAccessLayer.Classes
{
    public class Serie : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private int? _idRetornado;
        private string? _nome;
        private Status? _status;
        private Cliente? _cliente;
        private Modelo? _modelo;
        private Ano? _ano;
        private Frota? _frota;

        #endregion Campos

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

        public Status? Status
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

        public Cliente? Cliente
        {
            get { return _cliente; }
            set
            {
                if (value != _cliente)
                {
                    _cliente = value;
                    OnPropertyChanged(nameof(Cliente));
                }
            }
        }

        public Modelo? Modelo
        {
            get { return _modelo; }
            set
            {
                if (value != _modelo)
                {
                    _modelo = value;
                    OnPropertyChanged(nameof(Modelo));
                }
            }
        }

        public Ano? Ano
        {
            get { return _ano; }
            set
            {
                if (value != _ano)
                {
                    _ano = value;
                    OnPropertyChanged(nameof(Ano));
                }
            }
        }

        public Frota? Frota
        {
            get { return _frota; }
            set
            {
                if (value != _frota)
                {
                    _frota = value;
                    OnPropertyChanged(nameof(Frota));
                }
            }
        }

        #endregion Propriedades

        #region Construtores

        /// <summary>
        /// Construtor sem parâmetros que cria uma nova instância de todas as classes
        /// </summary>
        public Serie()
        {
            Status = new();
            Cliente = new();
            Modelo = new();
            Ano = new();
            Frota = new();
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados da série com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id da série que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetSerieDatabaseAsync(int? id, CancellationToken ct)
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
                                          + "seri.id_serie AS IdSerie, "
                                          + "seri.nome AS NomeSerie, "
                                          + "stat.id_status AS IdStatusSerie, "
                                          + "stat.nome AS NomeStatusSerie, "
                                          + "clie.id_cliente AS IdCliente, "
                                          + "clie.nome AS NomeCliente, "
                                          + "clie.considerar_percentuais_tabela_kion AS ConsiderarPercentuaisTabelaKion, "
                                          + "clie.percentual_tabela_kion_1 AS PercentualTabelaKion1, "
                                          + "clie.percentual_tabela_kion_2 AS PercentualTabelaKion2, "
                                          + "clie.percentual_tabela_kion_3 AS PercentualTabelaKion3, "
                                          + "clie.considerar_acrescimo_especifico AS ConsiderarAcrescimoEspecifico, "
                                          + "clie.percentual_acrescimo_especifico AS PercentualAcrescimoEspecifico, "
                                          + "stat_clie.id_status AS IdStatusCliente, "
                                          + "stat_clie.nome AS NomeStatusCliente, "
                                          + "mode.id_modelo AS IdModelo, "
                                          + "mode.nome AS NomeModelo, "
                                          + "stat_mode.id_status AS IdStatusModelo, "
                                          + "stat_mode.nome AS NomeStatusModelo, "
                                          + "fabr.id_fabricante AS IdFabricante, "
                                          + "fabr.nome AS NomeFabricante, "
                                          + "stat_fabr.id_status AS IdStatusFabricante, "
                                          + "stat_fabr.nome AS NomeStatusFabricante, "
                                          + "tieq.id_tipo_equipamento AS IdTipoEquipamento, "
                                          + "tieq.nome AS NomeTipoEquipamento, "
                                          + "stat_tieq.id_status AS IdStatusTipoEquipamento, "
                                          + "stat_tieq.nome AS NomeStatusTipoEquipamento, "

                                          + "cate.id_categoria AS IdCategoria, "
                                          + "cate.nome AS NomeCategoria, "
                                          + "stat_cate.id_status AS IdStatusCategoria, "
                                          + "stat_cate.nome AS NomeStatusCategoria, "
                                          + "clas.id_classe AS IdClasse, "
                                          + "clas.nome AS NomeClasse, "
                                          + "stat_clas.id_status AS IdStatusClasse, "
                                          + "stat_clas.nome AS NomeStatusClasse, "

                                          + "t_ano.id_ano AS IdAno, "
                                          + "t_ano.posicao_inicio_caracteres AS PosicaoInicioCaracteres, "
                                          + "t_ano.caracteres AS Caracteres, "
                                          + "t_ano.ano AS AnoValor, "
                                          + "stat_t_ano.id_status AS IdStatusAno, "
                                          + "stat_t_ano.nome AS NomeStatusAno, "
                                          + "frot.id_frota AS IdFrota, "
                                          + "frot.nome AS NomeFrota, "
                                          + "stat_frot.id_status AS IdStatusFrota, "
                                          + "stat_frot.nome AS NomeStatusFrota, "
                                          + "area.id_area AS IdArea, "
                                          + "area.nome AS NomeArea, "
                                          + "stat_area.id_status AS IdStatusArea, "
                                          + "stat_area.nome AS NomeStatusArea, "
                                          + "plan.id_planta AS IdPlanta, "
                                          + "plan.nome AS NomePlanta, "
                                          + "stat_plan.id_status AS IdStatusPlanta, "
                                          + "stat_plan.nome AS NomeStatusPlanta, "
                                          + "stat_clie.id_status AS IdStatusCliente, "
                                          + "stat_clie.nome AS NomeStatusCliente, "
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
                                          + "FROM tb_series AS seri "
                                          + "LEFT JOIN tb_status AS stat ON stat.id_status = seri.id_status "
                                          + "LEFT JOIN tb_clientes AS clie ON clie.id_cliente = seri.id_cliente "
                                          + "LEFT JOIN tb_status AS stat_clie ON stat_clie.id_status = clie.id_status "
                                          + "LEFT JOIN tb_modelos AS mode ON mode.id_modelo = seri.id_modelo "
                                          + "LEFT JOIN tb_status AS stat_mode ON stat_mode.id_status = mode.id_status "

                                          + "LEFT JOIN tb_categorias AS cate ON cate.id_categoria = mode.id_categoria "
                                          + "LEFT JOIN tb_status AS stat_cate ON stat_cate.id_status = cate.id_status "
                                          + "LEFT JOIN tb_classes AS clas ON clas.id_classe = mode.id_classe "
                                          + "LEFT JOIN tb_status AS stat_clas ON stat_clas.id_status = clas.id_status "

                                          + "LEFT JOIN tb_fabricantes AS fabr ON fabr.id_fabricante = mode.id_fabricante "
                                          + "LEFT JOIN tb_status AS stat_fabr ON stat_fabr.id_status = fabr.id_status "
                                          + "LEFT JOIN tb_tipos_equipamento AS tieq ON tieq.id_tipo_equipamento = mode.id_tipo_equipamento "
                                          + "LEFT JOIN tb_status AS stat_tieq ON stat_tieq.id_status = tieq.id_status "
                                          + "LEFT JOIN tb_anos AS t_ano ON t_ano.id_ano = seri.id_ano "
                                          + "LEFT JOIN tb_status AS stat_t_ano ON stat_t_ano.id_status = t_ano.id_status "
                                          + "LEFT JOIN tb_frotas AS frot ON frot.id_frota = seri.id_frota "
                                          + "LEFT JOIN tb_status AS stat_frot ON stat_frot.id_status = frot.id_status "
                                          + "LEFT JOIN tb_areas AS area ON area.id_area = frot.id_area "
                                          + "LEFT JOIN tb_status AS stat_area ON stat_area.id_status = area.id_status "
                                          + "LEFT JOIN tb_plantas AS plan ON plan.id_planta = area.id_planta "
                                          + "LEFT JOIN tb_status AS stat_plan ON stat_plan.id_status = plan.id_status "
                                          + "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = plan.id_cidade "
                                          + "LEFT JOIN tb_status AS stat_cida ON stat_cida.id_status = cida.id_status "
                                          + "LEFT JOIN tb_estados AS esta ON esta.id_estado = cida.id_estado "
                                          + "LEFT JOIN tb_status AS stat_esta ON stat_esta.id_status = esta.id_status "
                                          + "LEFT JOIN tb_paises AS pais ON pais.id_pais = esta.id_pais "
                                          + "LEFT JOIN tb_status AS stat_pais ON stat_pais.id_status = pais.id_status "
                                          + "WHERE seri.id_serie = @id";

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
                                if (Cliente == null)
                                {
                                    Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cliente.Status == null)
                                {
                                    Cliente.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo == null)
                                {
                                    Modelo = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Status == null)
                                {
                                    Modelo.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Fabricante == null)
                                {
                                    Modelo.Fabricante = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Fabricante.Status == null)
                                {
                                    Modelo.Fabricante.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.TipoEquipamento == null)
                                {
                                    Modelo.TipoEquipamento = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.TipoEquipamento.Status == null)
                                {
                                    Modelo.TipoEquipamento.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Categoria == null)
                                {
                                    Modelo.Categoria = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Categoria.Status == null)
                                {
                                    Modelo.Categoria.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Classe == null)
                                {
                                    Modelo.Classe = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Classe.Status == null)
                                {
                                    Modelo.Classe.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Ano == null)
                                {
                                    Ano = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Ano.Status == null)
                                {
                                    Ano.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Ano.Fabricante == null)
                                {
                                    Ano.Fabricante = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Ano.Fabricante.Status == null)
                                {
                                    Ano.Fabricante.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota == null)
                                {
                                    Frota = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Status == null)
                                {
                                    Frota.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area == null)
                                {
                                    Frota.Area = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Status == null)
                                {
                                    Frota.Area.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta == null)
                                {
                                    Frota.Area.Planta = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Status == null)
                                {
                                    Frota.Area.Planta.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cliente == null)
                                {
                                    Frota.Area.Planta.Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cliente.Status == null)
                                {
                                    Frota.Area.Planta.Cliente.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cidade == null)
                                {
                                    Frota.Area.Planta.Cidade = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cidade.Status == null)
                                {
                                    Frota.Area.Planta.Cidade.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cidade.Estado == null)
                                {
                                    Frota.Area.Planta.Cidade.Estado = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cidade.Estado.Status == null)
                                {
                                    Frota.Area.Planta.Cidade.Estado.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cidade.Estado.Pais == null)
                                {
                                    Frota.Area.Planta.Cidade.Estado.Pais = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cidade.Estado.Pais.Status == null)
                                {
                                    Frota.Area.Planta.Cidade.Estado.Pais.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdSerie"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeSerie"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSerie"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSerie"]);
                                Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                Cliente.ConsiderarPercentuaisTabelaKion = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarPercentuaisTabelaKion"]);
                                Cliente.PercentualTabelaKion1 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion1"]);
                                Cliente.PercentualTabelaKion2 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion2"]);
                                Cliente.PercentualTabelaKion3 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion3"]);
                                Cliente.ConsiderarAcrescimoEspecifico = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarAcrescimoEspecifico"]);
                                Cliente.PercentualAcrescimoEspecifico = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoEspecifico"]);
                                Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                                Modelo.Id = FuncoesDeConversao.ConverteParaInt(reader["IdModelo"]);
                                Modelo.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeModelo"]);
                                Modelo.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusModelo"]);
                                Modelo.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusModelo"]);
                                Modelo.Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                Modelo.Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                Modelo.Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                Modelo.Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);
                                Modelo.TipoEquipamento.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoEquipamento"]);
                                Modelo.TipoEquipamento.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTipoEquipamento"]);
                                Modelo.TipoEquipamento.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTipoEquipamento"]);
                                Modelo.TipoEquipamento.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTipoEquipamento"]);

                                Modelo.Categoria.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCategoria"]);
                                Modelo.Categoria.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCategoria"]);
                                Modelo.Categoria.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCategoria"]);
                                Modelo.Categoria.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCategoria"]);
                                Modelo.Classe.Id = FuncoesDeConversao.ConverteParaInt(reader["IdClasse"]);
                                Modelo.Classe.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeClasse"]);
                                Modelo.Classe.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusClasse"]);
                                Modelo.Classe.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusClasse"]);

                                Ano.Id = FuncoesDeConversao.ConverteParaInt(reader["IdAno"]);
                                Ano.PosicaoInicioCaracteres = FuncoesDeConversao.ConverteParaInt(reader["PosicaoInicioCaracteres"]);
                                Ano.Caracteres = FuncoesDeConversao.ConverteParaString(reader["Caracteres"]);
                                Ano.AnoValor = FuncoesDeConversao.ConverteParaInt(reader["AnoValor"]);
                                Ano.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusAno"]);
                                Ano.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusAno"]);
                                Ano.Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                Ano.Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                Ano.Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                Ano.Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);

                                Frota.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFrota"]);
                                Frota.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFrota"]);
                                Frota.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFrota"]);
                                Frota.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFrota"]);
                                Frota.Area.Id = FuncoesDeConversao.ConverteParaInt(reader["IdArea"]);
                                Frota.Area.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeArea"]);
                                Frota.Area.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusArea"]);
                                Frota.Area.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusArea"]);
                                Frota.Area.Planta.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPlanta"]);
                                Frota.Area.Planta.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePlanta"]);
                                Frota.Area.Planta.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPlanta"]);
                                Frota.Area.Planta.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPlanta"]);
                                Frota.Area.Planta.Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Frota.Area.Planta.Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                Frota.Area.Planta.Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                Frota.Area.Planta.Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                                Frota.Area.Planta.Cidade.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCidade"]);
                                Frota.Area.Planta.Cidade.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCidade"]);
                                Frota.Area.Planta.Cidade.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCidade"]);
                                Frota.Area.Planta.Cidade.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCidade"]);
                                Frota.Area.Planta.Cidade.Estado.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEstado"]);
                                Frota.Area.Planta.Cidade.Estado.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeEstado"]);
                                Frota.Area.Planta.Cidade.Estado.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEstado"]);
                                Frota.Area.Planta.Cidade.Estado.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEstado"]);
                                Frota.Area.Planta.Cidade.Estado.Pais.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPais"]);
                                Frota.Area.Planta.Cidade.Estado.Pais.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePais"]);
                                Frota.Area.Planta.Cidade.Estado.Pais.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPais"]);
                                Frota.Area.Planta.Cidade.Estado.Pais.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPais"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados da série com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetSerieDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                                    + "seri.id_serie AS IdSerie, "
                                    + "seri.nome AS NomeSerie, "
                                    + "stat.id_status AS IdStatusSerie, "
                                    + "stat.nome AS NomeStatusSerie, "
                                    + "clie.id_cliente AS IdCliente, "
                                    + "clie.nome AS NomeCliente, "
                                    + "clie.considerar_percentuais_tabela_kion AS ConsiderarPercentuaisTabelaKion, "
                                    + "clie.percentual_tabela_kion_1 AS PercentualTabelaKion1, "
                                    + "clie.percentual_tabela_kion_2 AS PercentualTabelaKion2, "
                                    + "clie.percentual_tabela_kion_3 AS PercentualTabelaKion3, "
                                    + "clie.considerar_acrescimo_especifico AS ConsiderarAcrescimoEspecifico, "
                                    + "clie.percentual_acrescimo_especifico AS PercentualAcrescimoEspecifico, "
                                    + "stat_clie.id_status AS IdStatusCliente, "
                                    + "stat_clie.nome AS NomeStatusCliente, "
                                    + "mode.id_modelo AS IdModelo, "
                                    + "mode.nome AS NomeModelo, "
                                    + "stat_mode.id_status AS IdStatusModelo, "
                                    + "stat_mode.nome AS NomeStatusModelo, "
                                    + "fabr.id_fabricante AS IdFabricante, "
                                    + "fabr.nome AS NomeFabricante, "
                                    + "stat_fabr.id_status AS IdStatusFabricante, "
                                    + "stat_fabr.nome AS NomeStatusFabricante, "
                                    + "tieq.id_tipo_equipamento AS IdTipoEquipamento, "
                                    + "tieq.nome AS NomeTipoEquipamento, "
                                    + "stat_tieq.id_status AS IdStatusTipoEquipamento, "
                                    + "stat_tieq.nome AS NomeStatusTipoEquipamento, "

                                    + "cate.id_categoria AS IdCategoria, "
                                    + "cate.nome AS NomeCategoria, "
                                    + "stat_cate.id_status AS IdStatusCategoria, "
                                    + "stat_cate.nome AS NomeStatusCategoria, "
                                    + "clas.id_classe AS IdClasse, "
                                    + "clas.nome AS NomeClasse, "
                                    + "stat_clas.id_status AS IdStatusClasse, "
                                    + "stat_clas.nome AS NomeStatusClasse, "

                                    + "t_ano.id_ano AS IdAno, "
                                    + "t_ano.posicao_inicio_caracteres AS PosicaoInicioCaracteres, "
                                    + "t_ano.caracteres AS Caracteres, "
                                    + "t_ano.ano AS AnoValor, "
                                    + "stat_t_ano.id_status AS IdStatusAno, "
                                    + "stat_t_ano.nome AS NomeStatusAno, "
                                    + "frot.id_frota AS IdFrota, "
                                    + "frot.nome AS NomeFrota, "
                                    + "stat_frot.id_status AS IdStatusFrota, "
                                    + "stat_frot.nome AS NomeStatusFrota, "
                                    + "area.id_area AS IdArea, "
                                    + "area.nome AS NomeArea, "
                                    + "stat_area.id_status AS IdStatusArea, "
                                    + "stat_area.nome AS NomeStatusArea, "
                                    + "plan.id_planta AS IdPlanta, "
                                    + "plan.nome AS NomePlanta, "
                                    + "stat_plan.id_status AS IdStatusPlanta, "
                                    + "stat_plan.nome AS NomeStatusPlanta, "
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
                                    + "FROM tb_series AS seri "
                                    + "LEFT JOIN tb_status AS stat ON stat.id_status = seri.id_status "
                                    + "LEFT JOIN tb_clientes AS clie ON clie.id_cliente = seri.id_cliente "
                                    + "LEFT JOIN tb_status AS stat_clie ON stat_clie.id_status = clie.id_status "
                                    + "LEFT JOIN tb_modelos AS mode ON mode.id_modelo = seri.id_modelo "
                                    + "LEFT JOIN tb_status AS stat_mode ON stat_mode.id_status = mode.id_status "

                                    + "LEFT JOIN tb_categorias AS cate ON cate.id_categoria = mode.id_categoria "
                                    + "LEFT JOIN tb_status AS stat_cate ON stat_cate.id_status = cate.id_status "
                                    + "LEFT JOIN tb_classes AS clas ON clas.id_classe = mode.id_classe "
                                    + "LEFT JOIN tb_status AS stat_clas ON stat_clas.id_status = clas.id_status "

                                    + "LEFT JOIN tb_fabricantes AS fabr ON fabr.id_fabricante = mode.id_fabricante "
                                    + "LEFT JOIN tb_status AS stat_fabr ON stat_fabr.id_status = fabr.id_status "
                                    + "LEFT JOIN tb_tipos_equipamento AS tieq ON tieq.id_tipo_equipamento = mode.id_tipo_equipamento "
                                    + "LEFT JOIN tb_status AS stat_tieq ON stat_tieq.id_status = tieq.id_status "
                                    + "LEFT JOIN tb_anos AS t_ano ON t_ano.id_ano = seri.id_ano "
                                    + "LEFT JOIN tb_status AS stat_t_ano ON stat_t_ano.id_status = t_ano.id_status "
                                    + "LEFT JOIN tb_frotas AS frot ON frot.id_frota = seri.id_frota "
                                    + "LEFT JOIN tb_status AS stat_frot ON stat_frot.id_status = frot.id_status "
                                    + "LEFT JOIN tb_areas AS area ON area.id_area = frot.id_area "
                                    + "LEFT JOIN tb_status AS stat_area ON stat_area.id_status = area.id_status "
                                    + "LEFT JOIN tb_plantas AS plan ON plan.id_planta = area.id_planta "
                                    + "LEFT JOIN tb_status AS stat_plan ON stat_plan.id_status = plan.id_status "
                                    + "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = plan.id_cidade "
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
                                if (Cliente == null)
                                {
                                    Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Cliente.Status == null)
                                {
                                    Cliente.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo == null)
                                {
                                    Modelo = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Status == null)
                                {
                                    Modelo.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Fabricante == null)
                                {
                                    Modelo.Fabricante = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Fabricante.Status == null)
                                {
                                    Modelo.Fabricante.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.TipoEquipamento == null)
                                {
                                    Modelo.TipoEquipamento = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.TipoEquipamento.Status == null)
                                {
                                    Modelo.TipoEquipamento.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Categoria == null)
                                {
                                    Modelo.Categoria = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Categoria.Status == null)
                                {
                                    Modelo.Categoria.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Classe == null)
                                {
                                    Modelo.Classe = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Modelo.Classe.Status == null)
                                {
                                    Modelo.Classe.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Ano == null)
                                {
                                    Ano = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Ano.Status == null)
                                {
                                    Ano.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Ano.Fabricante == null)
                                {
                                    Ano.Fabricante = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Ano.Fabricante.Status == null)
                                {
                                    Ano.Fabricante.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota == null)
                                {
                                    Frota = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Status == null)
                                {
                                    Frota.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area == null)
                                {
                                    Frota.Area = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Status == null)
                                {
                                    Frota.Area.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta == null)
                                {
                                    Frota.Area.Planta = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Status == null)
                                {
                                    Frota.Area.Planta.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cliente == null)
                                {
                                    Frota.Area.Planta.Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cliente.Status == null)
                                {
                                    Frota.Area.Planta.Cliente.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cidade == null)
                                {
                                    Frota.Area.Planta.Cidade = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cidade.Status == null)
                                {
                                    Frota.Area.Planta.Cidade.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cidade.Estado == null)
                                {
                                    Frota.Area.Planta.Cidade.Estado = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cidade.Estado.Status == null)
                                {
                                    Frota.Area.Planta.Cidade.Estado.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cidade.Estado.Pais == null)
                                {
                                    Frota.Area.Planta.Cidade.Estado.Pais = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Frota.Area.Planta.Cidade.Estado.Pais.Status == null)
                                {
                                    Frota.Area.Planta.Cidade.Estado.Pais.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdSerie"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeSerie"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSerie"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSerie"]);
                                Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                Cliente.ConsiderarPercentuaisTabelaKion = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarPercentuaisTabelaKion"]);
                                Cliente.PercentualTabelaKion1 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion1"]);
                                Cliente.PercentualTabelaKion2 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion2"]);
                                Cliente.PercentualTabelaKion3 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion3"]);
                                Cliente.ConsiderarAcrescimoEspecifico = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarAcrescimoEspecifico"]);
                                Cliente.PercentualAcrescimoEspecifico = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoEspecifico"]);
                                Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                                Modelo.Id = FuncoesDeConversao.ConverteParaInt(reader["IdModelo"]);
                                Modelo.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeModelo"]);
                                Modelo.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusModelo"]);
                                Modelo.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusModelo"]);
                                Modelo.Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                Modelo.Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                Modelo.Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                Modelo.Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);
                                Modelo.TipoEquipamento.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoEquipamento"]);
                                Modelo.TipoEquipamento.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTipoEquipamento"]);
                                Modelo.TipoEquipamento.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTipoEquipamento"]);
                                Modelo.TipoEquipamento.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTipoEquipamento"]);

                                Modelo.Categoria.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCategoria"]);
                                Modelo.Categoria.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCategoria"]);
                                Modelo.Categoria.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCategoria"]);
                                Modelo.Categoria.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCategoria"]);
                                Modelo.Classe.Id = FuncoesDeConversao.ConverteParaInt(reader["IdClasse"]);
                                Modelo.Classe.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeClasse"]);
                                Modelo.Classe.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusClasse"]);
                                Modelo.Classe.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusClasse"]);

                                Ano.Id = FuncoesDeConversao.ConverteParaInt(reader["IdAno"]);
                                Ano.PosicaoInicioCaracteres = FuncoesDeConversao.ConverteParaInt(reader["PosicaoInicioCaracteres"]);
                                Ano.Caracteres = FuncoesDeConversao.ConverteParaString(reader["Caracteres"]);
                                Ano.AnoValor = FuncoesDeConversao.ConverteParaInt(reader["AnoValor"]);
                                Ano.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusAno"]);
                                Ano.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusAno"]);
                                Ano.Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                Ano.Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                Ano.Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                Ano.Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);

                                Frota.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFrota"]);
                                Frota.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFrota"]);
                                Frota.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFrota"]);
                                Frota.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFrota"]);
                                Frota.Area.Id = FuncoesDeConversao.ConverteParaInt(reader["IdArea"]);
                                Frota.Area.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeArea"]);
                                Frota.Area.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusArea"]);
                                Frota.Area.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusArea"]);
                                Frota.Area.Planta.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPlanta"]);
                                Frota.Area.Planta.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePlanta"]);
                                Frota.Area.Planta.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPlanta"]);
                                Frota.Area.Planta.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPlanta"]);
                                Frota.Area.Planta.Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                Frota.Area.Planta.Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                Frota.Area.Planta.Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                Frota.Area.Planta.Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                                Frota.Area.Planta.Cidade.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCidade"]);
                                Frota.Area.Planta.Cidade.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCidade"]);
                                Frota.Area.Planta.Cidade.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCidade"]);
                                Frota.Area.Planta.Cidade.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCidade"]);
                                Frota.Area.Planta.Cidade.Estado.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEstado"]);
                                Frota.Area.Planta.Cidade.Estado.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeEstado"]);
                                Frota.Area.Planta.Cidade.Estado.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEstado"]);
                                Frota.Area.Planta.Cidade.Estado.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEstado"]);
                                Frota.Area.Planta.Cidade.Estado.Pais.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPais"]);
                                Frota.Area.Planta.Cidade.Estado.Pais.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePais"]);
                                Frota.Area.Planta.Cidade.Estado.Pais.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPais"]);
                                Frota.Area.Planta.Cidade.Estado.Pais.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPais"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva a série na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarSerieDatabaseAsync(CancellationToken ct)
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_serie", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_id_cliente", Cliente.Id);
                        command.Parameters.AddWithValue("p_id_fabricante", Modelo.Fabricante.Id);
                        command.Parameters.AddWithValue("p_id_tipo_equipamento", Modelo.TipoEquipamento.Id);
                        command.Parameters.AddWithValue("p_id_modelo", Modelo.Id);
                        command.Parameters.AddWithValue("p_id_ano", Ano == null ? null : Ano.Id);
                        command.Parameters.AddWithValue("p_id_frota", Frota.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add("p_id_serie", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException("A série " + Nome + " já existe para o cliente " + Cliente.Nome);
                        }

                        // Retorna o id da série
                        Id = FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_serie"].Value);
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_serie", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_serie", Id);
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_id_cliente", Cliente.Id);
                        command.Parameters.AddWithValue("p_id_fabricante", Modelo.Fabricante.Id);
                        command.Parameters.AddWithValue("p_id_tipo_equipamento", Modelo.TipoEquipamento.Id);
                        command.Parameters.AddWithValue("p_id_modelo", Modelo.Id);
                        command.Parameters.AddWithValue("p_id_ano", Ano == null ? null : Ano.Id);
                        command.Parameters.AddWithValue("p_id_frota", Frota.Id);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException("série", Nome);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta a série na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarSerieDatabaseAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_serie", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_serie", Id);
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
                            throw new ChaveEstrangeiraEmUsoException("série", Nome);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("série", Nome);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de séries com os argumentos utilizados
        /// </summary>
        /// <param name="listaSeries">Representa a lista de séries que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaSeriesAsync(ObservableCollection<Serie> listaSeries, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaSeries == null)
            {
                listaSeries = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaSeries.Clear();
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
                                + "seri.id_serie AS IdSerie, "
                                + "seri.nome AS NomeSerie, "
                                + "stat.id_status AS IdStatusSerie, "
                                + "stat.nome AS NomeStatusSerie, "
                                + "clie.id_cliente AS IdCliente, "
                                + "clie.nome AS NomeCliente, "
                                + "clie.considerar_percentuais_tabela_kion AS ConsiderarPercentuaisTabelaKion, "
                                + "clie.percentual_tabela_kion_1 AS PercentualTabelaKion1, "
                                + "clie.percentual_tabela_kion_2 AS PercentualTabelaKion2, "
                                + "clie.percentual_tabela_kion_3 AS PercentualTabelaKion3, "
                                + "clie.considerar_acrescimo_especifico AS ConsiderarAcrescimoEspecifico, "
                                + "clie.percentual_acrescimo_especifico AS PercentualAcrescimoEspecifico, "
                                + "stat_clie.id_status AS IdStatusCliente, "
                                + "stat_clie.nome AS NomeStatusCliente, "
                                + "mode.id_modelo AS IdModelo, "
                                + "mode.nome AS NomeModelo, "
                                + "stat_mode.id_status AS IdStatusModelo, "
                                + "stat_mode.nome AS NomeStatusModelo, "
                                + "fabr.id_fabricante AS IdFabricante, "
                                + "fabr.nome AS NomeFabricante, "
                                + "stat_fabr.id_status AS IdStatusFabricante, "
                                + "stat_fabr.nome AS NomeStatusFabricante, "
                                + "tieq.id_tipo_equipamento AS IdTipoEquipamento, "
                                + "tieq.nome AS NomeTipoEquipamento, "
                                + "stat_tieq.id_status AS IdStatusTipoEquipamento, "
                                + "stat_tieq.nome AS NomeStatusTipoEquipamento, "

                                + "cate.id_categoria AS IdCategoria, "
                                + "cate.nome AS NomeCategoria, "
                                + "stat_cate.id_status AS IdStatusCategoria, "
                                + "stat_cate.nome AS NomeStatusCategoria, "
                                + "clas.id_classe AS IdClasse, "
                                + "clas.nome AS NomeClasse, "
                                + "stat_clas.id_status AS IdStatusClasse, "
                                + "stat_clas.nome AS NomeStatusClasse, "

                                + "t_ano.id_ano AS IdAno, "
                                + "t_ano.posicao_inicio_caracteres AS PosicaoInicioCaracteres, "
                                + "t_ano.caracteres AS Caracteres, "
                                + "t_ano.ano AS AnoValor, "
                                + "stat_t_ano.id_status AS IdStatusAno, "
                                + "stat_t_ano.nome AS NomeStatusAno, "
                                + "frot.id_frota AS IdFrota, "
                                + "frot.nome AS NomeFrota, "
                                + "stat_frot.id_status AS IdStatusFrota, "
                                + "stat_frot.nome AS NomeStatusFrota, "
                                + "area.id_area AS IdArea, "
                                + "area.nome AS NomeArea, "
                                + "stat_area.id_status AS IdStatusArea, "
                                + "stat_area.nome AS NomeStatusArea, "
                                + "plan.id_planta AS IdPlanta, "
                                + "plan.nome AS NomePlanta, "
                                + "stat_plan.id_status AS IdStatusPlanta, "
                                + "stat_plan.nome AS NomeStatusPlanta, "
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
                                + "FROM tb_series AS seri "
                                + "LEFT JOIN tb_status AS stat ON stat.id_status = seri.id_status "
                                + "LEFT JOIN tb_clientes AS clie ON clie.id_cliente = seri.id_cliente "
                                + "LEFT JOIN tb_status AS stat_clie ON stat_clie.id_status = clie.id_status "
                                + "LEFT JOIN tb_modelos AS mode ON mode.id_modelo = seri.id_modelo "
                                + "LEFT JOIN tb_status AS stat_mode ON stat_mode.id_status = mode.id_status "

                                + "LEFT JOIN tb_categorias AS cate ON cate.id_categoria = mode.id_categoria "
                                + "LEFT JOIN tb_status AS stat_cate ON stat_cate.id_status = cate.id_status "
                                + "LEFT JOIN tb_classes AS clas ON clas.id_classe = mode.id_classe "
                                + "LEFT JOIN tb_status AS stat_clas ON stat_clas.id_status = clas.id_status "

                                + "LEFT JOIN tb_fabricantes AS fabr ON fabr.id_fabricante = mode.id_fabricante "
                                + "LEFT JOIN tb_status AS stat_fabr ON stat_fabr.id_status = fabr.id_status "
                                + "LEFT JOIN tb_tipos_equipamento AS tieq ON tieq.id_tipo_equipamento = mode.id_tipo_equipamento "
                                + "LEFT JOIN tb_status AS stat_tieq ON stat_tieq.id_status = tieq.id_status "
                                + "LEFT JOIN tb_anos AS t_ano ON t_ano.id_ano = seri.id_ano "
                                + "LEFT JOIN tb_status AS stat_t_ano ON stat_t_ano.id_status = t_ano.id_status "
                                + "LEFT JOIN tb_frotas AS frot ON frot.id_frota = seri.id_frota "
                                + "LEFT JOIN tb_status AS stat_frot ON stat_frot.id_status = frot.id_status "
                                + "LEFT JOIN tb_areas AS area ON area.id_area = frot.id_area "
                                + "LEFT JOIN tb_status AS stat_area ON stat_area.id_status = area.id_status "
                                + "LEFT JOIN tb_plantas AS plan ON plan.id_planta = area.id_planta "
                                + "LEFT JOIN tb_status AS stat_plan ON stat_plan.id_status = plan.id_status "
                                + "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = plan.id_cidade "
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
                                Serie item = new();

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Cliente == null)
                                {
                                    item.Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Cliente.Status == null)
                                {
                                    item.Cliente.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo == null)
                                {
                                    item.Modelo = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Status == null)
                                {
                                    item.Modelo.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Fabricante == null)
                                {
                                    item.Modelo.Fabricante = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Fabricante.Status == null)
                                {
                                    item.Modelo.Fabricante.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.TipoEquipamento == null)
                                {
                                    item.Modelo.TipoEquipamento = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.TipoEquipamento.Status == null)
                                {
                                    item.Modelo.TipoEquipamento.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Categoria == null)
                                {
                                    item.Modelo.Categoria = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Categoria.Status == null)
                                {
                                    item.Modelo.Categoria.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Classe == null)
                                {
                                    item.Modelo.Classe = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Modelo.Classe.Status == null)
                                {
                                    item.Modelo.Classe.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Ano == null)
                                {
                                    item.Ano = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Ano.Status == null)
                                {
                                    item.Ano.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Ano.Fabricante == null)
                                {
                                    item.Ano.Fabricante = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Ano.Fabricante.Status == null)
                                {
                                    item.Ano.Fabricante.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota == null)
                                {
                                    item.Frota = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota.Status == null)
                                {
                                    item.Frota.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota.Area == null)
                                {
                                    item.Frota.Area = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota.Area.Status == null)
                                {
                                    item.Frota.Area.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota.Area.Planta == null)
                                {
                                    item.Frota.Area.Planta = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota.Area.Planta.Status == null)
                                {
                                    item.Frota.Area.Planta.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota.Area.Planta.Cliente == null)
                                {
                                    item.Frota.Area.Planta.Cliente = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota.Area.Planta.Cliente.Status == null)
                                {
                                    item.Frota.Area.Planta.Cliente.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota.Area.Planta.Cidade == null)
                                {
                                    item.Frota.Area.Planta.Cidade = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota.Area.Planta.Cidade.Status == null)
                                {
                                    item.Frota.Area.Planta.Cidade.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota.Area.Planta.Cidade.Estado == null)
                                {
                                    item.Frota.Area.Planta.Cidade.Estado = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota.Area.Planta.Cidade.Estado.Status == null)
                                {
                                    item.Frota.Area.Planta.Cidade.Estado.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota.Area.Planta.Cidade.Estado.Pais == null)
                                {
                                    item.Frota.Area.Planta.Cidade.Estado.Pais = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Frota.Area.Planta.Cidade.Estado.Pais.Status == null)
                                {
                                    item.Frota.Area.Planta.Cidade.Estado.Pais.Status = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdSerie"]);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeSerie"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSerie"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSerie"]);
                                item.Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                item.Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                item.Cliente.ConsiderarPercentuaisTabelaKion = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarPercentuaisTabelaKion"]);
                                item.Cliente.PercentualTabelaKion1 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion1"]);
                                item.Cliente.PercentualTabelaKion2 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion2"]);
                                item.Cliente.PercentualTabelaKion3 = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualTabelaKion3"]);
                                item.Cliente.ConsiderarAcrescimoEspecifico = FuncoesDeConversao.ConverteParaBool(reader["ConsiderarAcrescimoEspecifico"]);
                                item.Cliente.PercentualAcrescimoEspecifico = FuncoesDeConversao.ConverteParaDecimal(reader["PercentualAcrescimoEspecifico"]);
                                item.Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                item.Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                                item.Modelo.Id = FuncoesDeConversao.ConverteParaInt(reader["IdModelo"]);
                                item.Modelo.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeModelo"]);
                                item.Modelo.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusModelo"]);
                                item.Modelo.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusModelo"]);
                                item.Modelo.Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                item.Modelo.Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                item.Modelo.Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                item.Modelo.Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);
                                item.Modelo.TipoEquipamento.Id = FuncoesDeConversao.ConverteParaInt(reader["IdTipoEquipamento"]);
                                item.Modelo.TipoEquipamento.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeTipoEquipamento"]);
                                item.Modelo.TipoEquipamento.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusTipoEquipamento"]);
                                item.Modelo.TipoEquipamento.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusTipoEquipamento"]);

                                item.Modelo.Categoria.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCategoria"]);
                                item.Modelo.Categoria.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCategoria"]);
                                item.Modelo.Categoria.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCategoria"]);
                                item.Modelo.Categoria.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCategoria"]);
                                item.Modelo.Classe.Id = FuncoesDeConversao.ConverteParaInt(reader["IdClasse"]);
                                item.Modelo.Classe.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeClasse"]);
                                item.Modelo.Classe.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusClasse"]);
                                item.Modelo.Classe.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusClasse"]);

                                item.Ano.Id = FuncoesDeConversao.ConverteParaInt(reader["IdAno"]);
                                item.Ano.PosicaoInicioCaracteres = FuncoesDeConversao.ConverteParaInt(reader["PosicaoInicioCaracteres"]);
                                item.Ano.Caracteres = FuncoesDeConversao.ConverteParaString(reader["Caracteres"]);
                                item.Ano.AnoValor = FuncoesDeConversao.ConverteParaInt(reader["AnoValor"]);
                                item.Ano.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusAno"]);
                                item.Ano.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusAno"]);
                                item.Ano.Fabricante.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFabricante"]);
                                item.Ano.Fabricante.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFabricante"]);
                                item.Ano.Fabricante.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFabricante"]);
                                item.Ano.Fabricante.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFabricante"]);

                                item.Frota.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFrota"]);
                                item.Frota.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFrota"]);
                                item.Frota.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFrota"]);
                                item.Frota.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFrota"]);
                                item.Frota.Area.Id = FuncoesDeConversao.ConverteParaInt(reader["IdArea"]);
                                item.Frota.Area.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeArea"]);
                                item.Frota.Area.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusArea"]);
                                item.Frota.Area.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusArea"]);
                                item.Frota.Area.Planta.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPlanta"]);
                                item.Frota.Area.Planta.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePlanta"]);
                                item.Frota.Area.Planta.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPlanta"]);
                                item.Frota.Area.Planta.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPlanta"]);
                                item.Frota.Area.Planta.Cliente.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCliente"]);
                                item.Frota.Area.Planta.Cliente.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCliente"]);
                                item.Frota.Area.Planta.Cliente.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCliente"]);
                                item.Frota.Area.Planta.Cliente.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCliente"]);
                                item.Frota.Area.Planta.Cidade.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCidade"]);
                                item.Frota.Area.Planta.Cidade.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCidade"]);
                                item.Frota.Area.Planta.Cidade.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCidade"]);
                                item.Frota.Area.Planta.Cidade.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCidade"]);
                                item.Frota.Area.Planta.Cidade.Estado.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEstado"]);
                                item.Frota.Area.Planta.Cidade.Estado.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeEstado"]);
                                item.Frota.Area.Planta.Cidade.Estado.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEstado"]);
                                item.Frota.Area.Planta.Cidade.Estado.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEstado"]);
                                item.Frota.Area.Planta.Cidade.Estado.Pais.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPais"]);
                                item.Frota.Area.Planta.Cidade.Estado.Pais.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePais"]);
                                item.Frota.Area.Planta.Cidade.Estado.Pais.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPais"]);
                                item.Frota.Area.Planta.Cidade.Estado.Pais.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPais"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaSeries.Add(item);

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
            Serie serieCopia = new();

            serieCopia.Id = Id;
            serieCopia.Nome = Nome;
            serieCopia.Status = Status == null ? new() : (Status)Status.Clone();
            serieCopia.Cliente = Cliente == null ? new() : (Cliente)Cliente.Clone();
            serieCopia.Modelo = Modelo == null ? new() : (Modelo)Modelo.Clone();
            serieCopia.Ano = Ano == null ? new() : (Ano)Ano.Clone();
            serieCopia.Frota = Frota == null ? new() : (Frota)Frota.Clone();

            return serieCopia;
        }

        #endregion Interfaces
    }
}