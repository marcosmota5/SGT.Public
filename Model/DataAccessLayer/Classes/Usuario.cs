using System.Collections.ObjectModel;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using Model.DataAccessLayer.HelperClasses;

namespace Model.DataAccessLayer.Classes
{
    public class Usuario : ObservableObject, ICloneable
    {
        #region Campos

        private int? _id;
        private int? _idRetornado;
        private string? _nome;
        private string? _sexo;
        private string? _cpf;
        private string? _telefone;
        private string? _email;
        private string? _login;
        private DateTime? _dataCadastro;
        private DateTime? _dataUltimaAlteracaoSenha;
        private DateTime? _dataUltimaAlteracaoLogin;
        private DateTime? _dataUltimaAlteracaoEmail;
        private byte[]? _imagem;
        private string? _textoRespostaEmail;
        private string? _emailsEmCopia;
        private Status? _status;
        private Perfil? _perfil;
        private Setor? _setor;
        private Filial? _filial;
        private DateTime? _dataSessao;
        private string? _nomeComputadorSessao;
        private string? _usuarioSessao;
        private string? _cor;
        private string? _tema;
        private int? _limiteResultados;

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

        public string? Sexo
        {
            get { return _sexo; }
            set
            {
                if (value != _sexo)
                {
                    _sexo = value;
                    OnPropertyChanged(nameof(Sexo));
                }
            }
        }

        public string? CPF
        {
            get { return _cpf; }
            set
            {
                if (value != _cpf)
                {
                    _cpf = value;
                    OnPropertyChanged(nameof(CPF));
                }
            }
        }

        public string? Telefone
        {
            get { return _telefone; }
            set
            {
                if (value != _telefone)
                {
                    _telefone = value;
                    OnPropertyChanged(nameof(Telefone));
                }
            }
        }

        public string? Email
        {
            get { return _email; }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string? Login
        {
            get { return _login; }
            set
            {
                if (value != _login)
                {
                    _login = value;
                    OnPropertyChanged(nameof(Login));
                }
            }
        }

        public DateTime? DataCadastro
        {
            get { return _dataCadastro; }
            set
            {
                if (value != _dataCadastro)
                {
                    _dataCadastro = value;
                    OnPropertyChanged(nameof(DataCadastro));
                }
            }
        }

        public DateTime? DataUltimaAlteracaoSenha
        {
            get { return _dataUltimaAlteracaoSenha; }
            set
            {
                if (value != _dataUltimaAlteracaoSenha)
                {
                    _dataUltimaAlteracaoSenha = value;
                    OnPropertyChanged(nameof(DataUltimaAlteracaoSenha));
                }
            }
        }

        public DateTime? DataUltimaAlteracaoLogin
        {
            get { return _dataUltimaAlteracaoLogin; }
            set
            {
                if (value != _dataUltimaAlteracaoLogin)
                {
                    _dataUltimaAlteracaoLogin = value;
                    OnPropertyChanged(nameof(DataUltimaAlteracaoLogin));
                }
            }
        }

        public DateTime? DataUltimaAlteracaoEmail
        {
            get { return _dataUltimaAlteracaoEmail; }
            set
            {
                if (value != _dataUltimaAlteracaoEmail)
                {
                    _dataUltimaAlteracaoEmail = value;
                    OnPropertyChanged(nameof(DataUltimaAlteracaoEmail));
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

        public string? TextoRespostaEmail
        {
            get { return _textoRespostaEmail; }
            set
            {
                if (value != _textoRespostaEmail)
                {
                    _textoRespostaEmail = value;
                    OnPropertyChanged(nameof(TextoRespostaEmail));
                }
            }
        }

        public string? EmailsEmCopia
        {
            get { return _emailsEmCopia; }
            set
            {
                if (value != _emailsEmCopia)
                {
                    _emailsEmCopia = value;
                    OnPropertyChanged(nameof(EmailsEmCopia));
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

        public Perfil? Perfil
        {
            get { return _perfil; }
            set
            {
                if (value != _perfil)
                {
                    _perfil = value;
                    OnPropertyChanged(nameof(Perfil));
                }
            }
        }

        public Setor? Setor
        {
            get { return _setor; }
            set
            {
                if (value != _setor)
                {
                    _setor = value;
                    OnPropertyChanged(nameof(Setor));
                }
            }
        }

        public Filial? Filial
        {
            get { return _filial; }
            set
            {
                if (value != _filial)
                {
                    _filial = value;
                    OnPropertyChanged(nameof(Filial));
                }
            }
        }

        public DateTime? DataSessao
        {
            get { return _dataSessao; }
            set
            {
                if (value != _dataSessao)
                {
                    _dataSessao = value;
                    OnPropertyChanged(nameof(DataSessao));
                }
            }
        }

        public string? NomeComputadorSessao
        {
            get { return _nomeComputadorSessao; }
            set
            {
                if (value != _nomeComputadorSessao)
                {
                    _nomeComputadorSessao = value;
                    OnPropertyChanged(nameof(NomeComputadorSessao));
                }
            }
        }

        public string? UsuarioSessao
        {
            get { return _usuarioSessao; }
            set
            {
                if (value != _usuarioSessao)
                {
                    _usuarioSessao = value;
                    OnPropertyChanged(nameof(UsuarioSessao));
                }
            }
        }

        public string? Cor
        {
            get { return _cor; }
            set
            {
                if (value != _cor)
                {
                    _cor = value;
                    OnPropertyChanged(nameof(Cor));
                }
            }
        }

        public string? Tema
        {
            get { return _tema; }
            set
            {
                if (value != _tema)
                {
                    _tema = value;
                    OnPropertyChanged(nameof(Tema));
                }
            }
        }

        public int? LimiteResultados
        {
            get { return _limiteResultados; }
            set
            {
                if (value != _limiteResultados)
                {
                    _limiteResultados = value;
                    OnPropertyChanged(nameof(LimiteResultados));
                }
            }
        }

        #endregion Propriedades

        #region Construtores

        /// <summary>
        /// Construtor sem parâmetros que cria uma nova instância de todas as classes
        /// </summary>
        public Usuario()
        {
            Status = new();
            Perfil = new();
            Setor = new();
            Filial = new();
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Método assíncrono que retorna os dados do usuário com os argumentos utilizados
        /// </summary>
        /// <param name="id">Representa o id do usuário que deseja retornar</param>
        /// <param name="ct">Token de cancelamento</param>
        public async Task GetUsuarioDatabaseAsync(int? id, CancellationToken ct)
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
                                          + "usua.id_usuario AS IdUsuario, "
                                          + "usua.nome AS NomeUsuario, "
                                          + "usua.sexo AS SexoUsuario, "
                                          + "usua.cpf AS CPFUsuario, "
                                          + "usua.telefone AS TelefoneUsuario, "
                                          + "usua.email AS EmailUsuario, "
                                          + "usua.login AS LoginUsuario, "
                                          + "usua.data_cadastro AS DataCadastroUsuario, "
                                          + "usua.data_ultima_alteracao_senha AS DataUltimaAlteracaoSenhaUsuario, "
                                          + "usua.data_ultima_alteracao_login AS DataUltimaAlteracaoLoginUsuario, "
                                          + "usua.data_ultima_alteracao_email AS DataUltimaAlteracaoEmailUsuario, "
                                          + "usua.imagem AS ImagemUsuario, "
                                          + "usua.texto_resposta_email AS TextoRespostaEmailUsuario, "
                                          + "usua.emails_copia AS EmailsEmCopiaUsuario, "
                                          + "usua.data_sessao AS DataSessaoUsuario, "
                                          + "usua.nome_computador_sessao AS NomeComputadorSessaoUsuario, "
                                          + "usua.usuario_sessao AS UsuarioSessaoUsuario, "
                                          + "usua.cor AS CorUsuario, "
                                          + "usua.tema AS TemaUsuario, "
                                          + "usua.limite_resultados AS LimiteResultadosUsuario, "
                                          + "stat.id_status AS IdStatusUsuario, "
                                          + "stat.nome AS NomeStatusUsuario, "
                                          + "perf.id_perfil AS IdPerfil, "
                                          + "perf.nome AS NomePerfil, "
                                          + "stat_perf.id_status AS IdStatusPerfil, "
                                          + "stat_perf.nome AS NomeStatusPerfil, "
                                          + "seto.id_setor AS IdSetor, "
                                          + "seto.nome AS NomeSetor, "
                                          + "seto.prazo_adicional AS PrazoAdicional, "
                                          + "stat_seto.id_status AS IdStatusSetor, "
                                          + "stat_seto.nome AS NomeStatusSetor, "
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
                                          + "stat_fili.id_status AS IdStatusFilial, "
                                          + "stat_fili.nome AS NomeStatusFilial, "
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
                                          + "FROM tb_usuarios AS usua "
                                          + "LEFT JOIN tb_status AS stat ON stat.id_status = usua.id_status "
                                          + "LEFT JOIN tb_perfis AS perf ON perf.id_perfil = usua.id_perfil "
                                          + "LEFT JOIN tb_status AS stat_perf ON stat_perf.id_status = perf.id_status "
                                          + "LEFT JOIN tb_setores AS seto ON seto.id_setor = usua.id_setor "
                                          + "LEFT JOIN tb_status AS stat_seto ON stat_seto.id_status = seto.id_status "
                                          + "LEFT JOIN tb_filiais AS fili ON fili.id_filial = usua.id_filial "
                                          + "LEFT JOIN tb_status AS stat_fili ON stat_fili.id_status = fili.id_status "
                                          + "LEFT JOIN tb_empresas AS empr ON empr.id_empresa = fili.id_empresa "
                                          + "LEFT JOIN tb_status AS stat_empr ON stat_empr.id_status = empr.id_status "
                                          + "LEFT JOIN tb_cidades AS cida ON cida.id_cidade = fili.id_cidade "
                                          + "LEFT JOIN tb_status AS stat_cida ON stat_cida.id_status = cida.id_status "
                                          + "LEFT JOIN tb_estados AS esta ON esta.id_estado = cida.id_estado "
                                          + "LEFT JOIN tb_status AS stat_esta ON stat_esta.id_status = esta.id_status "
                                          + "LEFT JOIN tb_paises AS pais ON pais.id_pais = esta.id_pais "
                                          + "LEFT JOIN tb_status AS stat_pais ON stat_pais.id_status = pais.id_status "
                                          + "WHERE usua.id_usuario = @id";

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
                                if (Perfil == null)
                                {
                                    Perfil = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Perfil.Status == null)
                                {
                                    Perfil.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Setor == null)
                                {
                                    Setor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Setor.Status == null)
                                {
                                    Setor.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial == null)
                                {
                                    Filial = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Status == null)
                                {
                                    Filial.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Empresa == null)
                                {
                                    Filial.Empresa = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Empresa.Status == null)
                                {
                                    Filial.Empresa.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Cidade == null)
                                {
                                    Filial.Cidade = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Cidade.Status == null)
                                {
                                    Filial.Cidade.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Cidade.Estado == null)
                                {
                                    Filial.Cidade.Estado = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Cidade.Estado.Status == null)
                                {
                                    Filial.Cidade.Estado.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Cidade.Estado.Pais == null)
                                {
                                    Filial.Cidade.Estado.Pais = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Cidade.Estado.Pais.Status == null)
                                {
                                    Filial.Cidade.Estado.Pais.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdUsuario"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeUsuario"]);
                                Sexo = FuncoesDeConversao.ConverteParaString(reader["SexoUsuario"]);
                                CPF = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CPFUsuario"]));
                                Telefone = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneUsuario"]));
                                Email = FuncoesDeConversao.ConverteParaString(reader["EmailUsuario"]);
                                Login = FuncoesDeConversao.ConverteParaString(reader["LoginUsuario"]);
                                DataCadastro = FuncoesDeConversao.ConverteParaDateTime(reader["DataCadastroUsuario"]);
                                DataUltimaAlteracaoSenha = FuncoesDeConversao.ConverteParaDateTime(reader["DataUltimaAlteracaoSenhaUsuario"]);
                                DataUltimaAlteracaoLogin = FuncoesDeConversao.ConverteParaDateTime(reader["DataUltimaAlteracaoLoginUsuario"]);
                                DataUltimaAlteracaoEmail = FuncoesDeConversao.ConverteParaDateTime(reader["DataUltimaAlteracaoEmailUsuario"]);
                                Imagem = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["ImagemUsuario"]);
                                TextoRespostaEmail = FuncoesDeConversao.ConverteParaString(reader["TextoRespostaEmailUsuario"]);
                                EmailsEmCopia = FuncoesDeConversao.ConverteParaString(reader["EmailsEmCopiaUsuario"]);
                                DataSessao = FuncoesDeConversao.ConverteParaDateTime(reader["DataSessaoUsuario"]);
                                NomeComputadorSessao = FuncoesDeConversao.ConverteParaString(reader["NomeComputadorSessaoUsuario"]);
                                UsuarioSessao = FuncoesDeConversao.ConverteParaString(reader["UsuarioSessaoUsuario"]);
                                Cor = FuncoesDeConversao.ConverteParaString(reader["CorUsuario"]);
                                Tema = FuncoesDeConversao.ConverteParaString(reader["TemaUsuario"]);
                                LimiteResultados = FuncoesDeConversao.ConverteParaInt(reader["LimiteResultadosUsuario"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusUsuario"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusUsuario"]);
                                Perfil.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPerfil"]);
                                Perfil.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePerfil"]);
                                Perfil.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPerfil"]);
                                Perfil.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPerfil"]);
                                Setor.Id = FuncoesDeConversao.ConverteParaInt(reader["IdSetor"]);
                                Setor.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeSetor"]);
                                Setor.PrazoAdicional = FuncoesDeConversao.ConverteParaInt(reader["PrazoAdicional"]);
                                Setor.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSetor"]);
                                Setor.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSetor"]);
                                Filial.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFilial"]);
                                Filial.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFilial"]);
                                Filial.Endereco = FuncoesDeConversao.ConverteParaString(reader["EnderecoFilial"]);
                                Filial.CEP = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CEPFilial"]));
                                Filial.TelefoneGeral = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneGeralFilial"]));
                                Filial.TelefonePecas = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefonePecasFilial"]));
                                Filial.TelefoneServicos = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneServicosFilial"]));
                                Filial.EmailGeral = FuncoesDeConversao.ConverteParaString(reader["EmailGeralFilial"]);
                                Filial.EmailPecas = FuncoesDeConversao.ConverteParaString(reader["EmailPecasFilial"]);
                                Filial.EmailServicos = FuncoesDeConversao.ConverteParaString(reader["EmailServicosFilial"]);
                                Filial.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFilial"]);
                                Filial.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFilial"]);
                                Filial.Empresa.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEmpresa"]);
                                Filial.Empresa.CNPJ = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CNPJEmpresa"]));
                                Filial.Empresa.RazaoSocial = FuncoesDeConversao.ConverteParaString(reader["RazaoSocialEmpresa"]);
                                Filial.Empresa.NomeFantasia = FuncoesDeConversao.ConverteParaString(reader["NomeFantasiaEmpresa"]);
                                Filial.Empresa.Site = FuncoesDeConversao.ConverteParaString(reader["SiteEmpresa"]);
                                Filial.Empresa.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEmpresa"]);
                                Filial.Empresa.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEmpresa"]);
                                Filial.Cidade.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCidade"]);
                                Filial.Cidade.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCidade"]);
                                Filial.Cidade.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCidade"]);
                                Filial.Cidade.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCidade"]);
                                Filial.Cidade.Estado.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEstado"]);
                                Filial.Cidade.Estado.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeEstado"]);
                                Filial.Cidade.Estado.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEstado"]);
                                Filial.Cidade.Estado.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEstado"]);
                                Filial.Cidade.Estado.Pais.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPais"]);
                                Filial.Cidade.Estado.Pais.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePais"]);
                                Filial.Cidade.Estado.Pais.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPais"]);
                                Filial.Cidade.Estado.Pais.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPais"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna os dados do usuário com os argumentos utilizados
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public async Task GetUsuarioDatabaseAsync(CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
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
                      + "usua.id_usuario AS IdUsuario, "
                      + "usua.nome AS NomeUsuario, "
                      + "usua.sexo AS SexoUsuario, "
                      + "usua.cpf AS CPFUsuario, "
                      + "usua.telefone AS TelefoneUsuario, "
                      + "usua.email AS EmailUsuario, "
                      + "usua.login AS LoginUsuario, "
                      + "usua.data_cadastro AS DataCadastroUsuario, "
                      + "usua.data_ultima_alteracao_senha AS DataUltimaAlteracaoSenhaUsuario, "
                      + "usua.data_ultima_alteracao_login AS DataUltimaAlteracaoLoginUsuario, "
                      + "usua.data_ultima_alteracao_email AS DataUltimaAlteracaoEmailUsuario, "
                      + "usua.imagem AS ImagemUsuario, "
                      + "usua.texto_resposta_email AS TextoRespostaEmailUsuario, "
                      + "usua.emails_copia AS EmailsEmCopiaUsuario, "
                      + "usua.data_sessao AS DataSessaoUsuario, "
                      + "usua.nome_computador_sessao AS NomeComputadorSessaoUsuario, "
                      + "usua.usuario_sessao AS UsuarioSessaoUsuario, "
                      + "usua.cor AS CorUsuario, "
                      + "usua.tema AS TemaUsuario, "
                      + "usua.limite_resultados AS LimiteResultadosUsuario, "
                      + "stat.id_status AS IdStatusUsuario, "
                      + "stat.nome AS NomeStatusUsuario, "
                      + "perf.id_perfil AS IdPerfil, "
                      + "perf.nome AS NomePerfil, "
                      + "stat_perf.id_status AS IdStatusPerfil, "
                      + "stat_perf.nome AS NomeStatusPerfil, "
                      + "seto.id_setor AS IdSetor, "
                      + "seto.nome AS NomeSetor, "
                      + "seto.prazo_adicional AS PrazoAdicional, "
                      + "stat_seto.id_status AS IdStatusSetor, "
                      + "stat_seto.nome AS NomeStatusSetor, "
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
                      + "stat_fili.id_status AS IdStatusFilial, "
                      + "stat_fili.nome AS NomeStatusFilial, "
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
                      + "FROM tb_usuarios AS usua "
                      + "LEFT JOIN tb_status AS stat ON stat.id_status = usua.id_status "
                      + "LEFT JOIN tb_perfis AS perf ON perf.id_perfil = usua.id_perfil "
                      + "LEFT JOIN tb_status AS stat_perf ON stat_perf.id_status = perf.id_status "
                      + "LEFT JOIN tb_setores AS seto ON seto.id_setor = usua.id_setor "
                      + "LEFT JOIN tb_status AS stat_seto ON stat_seto.id_status = seto.id_status "
                      + "LEFT JOIN tb_filiais AS fili ON fili.id_filial = usua.id_filial "
                      + "LEFT JOIN tb_status AS stat_fili ON stat_fili.id_status = fili.id_status "
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
                                if (Perfil == null)
                                {
                                    Perfil = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Perfil.Status == null)
                                {
                                    Perfil.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Setor == null)
                                {
                                    Setor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Setor.Status == null)
                                {
                                    Setor.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial == null)
                                {
                                    Filial = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Status == null)
                                {
                                    Filial.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Empresa == null)
                                {
                                    Filial.Empresa = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Empresa.Status == null)
                                {
                                    Filial.Empresa.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Cidade == null)
                                {
                                    Filial.Cidade = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Cidade.Status == null)
                                {
                                    Filial.Cidade.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Cidade.Estado == null)
                                {
                                    Filial.Cidade.Estado = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Cidade.Estado.Status == null)
                                {
                                    Filial.Cidade.Estado.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Cidade.Estado.Pais == null)
                                {
                                    Filial.Cidade.Estado.Pais = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (Filial.Cidade.Estado.Pais.Status == null)
                                {
                                    Filial.Cidade.Estado.Pais.Status = new();
                                }

                                // Define as propriedades
                                Id = FuncoesDeConversao.ConverteParaInt(reader["IdUsuario"]);
                                Nome = FuncoesDeConversao.ConverteParaString(reader["NomeUsuario"]);
                                Sexo = FuncoesDeConversao.ConverteParaString(reader["SexoUsuario"]);
                                CPF = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CPFUsuario"]));
                                Telefone = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneUsuario"]));
                                Email = FuncoesDeConversao.ConverteParaString(reader["EmailUsuario"]);
                                Login = FuncoesDeConversao.ConverteParaString(reader["LoginUsuario"]);
                                DataCadastro = FuncoesDeConversao.ConverteParaDateTime(reader["DataCadastroUsuario"]);
                                DataUltimaAlteracaoSenha = FuncoesDeConversao.ConverteParaDateTime(reader["DataUltimaAlteracaoSenhaUsuario"]);
                                DataUltimaAlteracaoLogin = FuncoesDeConversao.ConverteParaDateTime(reader["DataUltimaAlteracaoLoginUsuario"]);
                                DataUltimaAlteracaoEmail = FuncoesDeConversao.ConverteParaDateTime(reader["DataUltimaAlteracaoEmailUsuario"]);
                                Imagem = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["ImagemUsuario"]);
                                TextoRespostaEmail = FuncoesDeConversao.ConverteParaString(reader["TextoRespostaEmailUsuario"]);
                                EmailsEmCopia = FuncoesDeConversao.ConverteParaString(reader["EmailsEmCopiaUsuario"]);
                                DataSessao = FuncoesDeConversao.ConverteParaDateTime(reader["DataSessaoUsuario"]);
                                NomeComputadorSessao = FuncoesDeConversao.ConverteParaString(reader["NomeComputadorSessaoUsuario"]);
                                UsuarioSessao = FuncoesDeConversao.ConverteParaString(reader["UsuarioSessaoUsuario"]);
                                Cor = FuncoesDeConversao.ConverteParaString(reader["CorUsuario"]);
                                Tema = FuncoesDeConversao.ConverteParaString(reader["TemaUsuario"]);
                                LimiteResultados = FuncoesDeConversao.ConverteParaInt(reader["LimiteResultadosUsuario"]);
                                Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusUsuario"]);
                                Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusUsuario"]);
                                Perfil.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPerfil"]);
                                Perfil.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePerfil"]);
                                Perfil.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPerfil"]);
                                Perfil.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPerfil"]);
                                Setor.Id = FuncoesDeConversao.ConverteParaInt(reader["IdSetor"]);
                                Setor.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeSetor"]);
                                Setor.PrazoAdicional = FuncoesDeConversao.ConverteParaInt(reader["PrazoAdicional"]);
                                Setor.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSetor"]);
                                Setor.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSetor"]);
                                Filial.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFilial"]);
                                Filial.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFilial"]);
                                Filial.Endereco = FuncoesDeConversao.ConverteParaString(reader["EnderecoFilial"]);
                                Filial.CEP = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CEPFilial"]));
                                Filial.TelefoneGeral = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneGeralFilial"]));
                                Filial.TelefonePecas = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefonePecasFilial"]));
                                Filial.TelefoneServicos = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneServicosFilial"]));
                                Filial.EmailGeral = FuncoesDeConversao.ConverteParaString(reader["EmailGeralFilial"]);
                                Filial.EmailPecas = FuncoesDeConversao.ConverteParaString(reader["EmailPecasFilial"]);
                                Filial.EmailServicos = FuncoesDeConversao.ConverteParaString(reader["EmailServicosFilial"]);
                                Filial.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFilial"]);
                                Filial.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFilial"]);
                                Filial.Empresa.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEmpresa"]);
                                Filial.Empresa.CNPJ = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CNPJEmpresa"]));
                                Filial.Empresa.RazaoSocial = FuncoesDeConversao.ConverteParaString(reader["RazaoSocialEmpresa"]);
                                Filial.Empresa.NomeFantasia = FuncoesDeConversao.ConverteParaString(reader["NomeFantasiaEmpresa"]);
                                Filial.Empresa.Site = FuncoesDeConversao.ConverteParaString(reader["SiteEmpresa"]);
                                Filial.Empresa.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEmpresa"]);
                                Filial.Empresa.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEmpresa"]);
                                Filial.Cidade.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCidade"]);
                                Filial.Cidade.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCidade"]);
                                Filial.Cidade.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCidade"]);
                                Filial.Cidade.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCidade"]);
                                Filial.Cidade.Estado.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEstado"]);
                                Filial.Cidade.Estado.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeEstado"]);
                                Filial.Cidade.Estado.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEstado"]);
                                Filial.Cidade.Estado.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEstado"]);
                                Filial.Cidade.Estado.Pais.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPais"]);
                                Filial.Cidade.Estado.Pais.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePais"]);
                                Filial.Cidade.Estado.Pais.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPais"]);
                                Filial.Cidade.Estado.Pais.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPais"]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna o id do usuário através do método de login
        /// </summary>
        /// <param name="loginOuEmail">Representa o login ou e-mail do usuário que deseja retornar o id</param>
        /// <param name="senha">Representa a senha do usuário</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorInativoException">Exceção que será lançada quando o usuário estiver inativo</exception>
        /// <exception cref="ValorIncorretoException">Exceção que será lançada quando a senha informada estiver errada</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task EfetuarLoginUsuarioDatabaseAsync(string loginOuEmail, string senha, CancellationToken ct)
        {
            // Retorna o id do usuário através do método correspondente
            int? idUsuario = await RetornarIdUsuarioDatabaseAsync(loginOuEmail, senha, ct);

            // Se o usuário for nulo, retorna a exceção de valor inexistente
            if (idUsuario == null)
            {
                throw new ValorNaoExisteException("Login e/ou e-mail não existe");
            }

            // Define os dados do usuário utilizando o id retornado
            await GetUsuarioDatabaseAsync(idUsuario, ct);
        }

        /// <summary>
        /// Método assíncrono que retorna o id do usuário através do método de login
        /// </summary>
        /// <param name="loginOuEmail">Representa o login ou e-mail do usuário que deseja retornar o id</param>
        /// <param name="senha">Representa a senha do usuário</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorInativoException">Exceção que será lançada quando o usuário estiver inativo</exception>
        /// <exception cref="ValorIncorretoException">Exceção que será lançada quando a senha informada estiver errada</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        private async Task<int?> RetornarIdUsuarioDatabaseAsync(string loginOuEmail, string senha, CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_efetua_login", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_login", loginOuEmail);
                    command.Parameters.AddWithValue("p_email", loginOuEmail);
                    command.Parameters.AddWithValue("p_senha", senha);
                    command.Parameters.Add("p_id_usuario", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("po_login", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("po_email", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_nome", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_cor", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_tema", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_limite_resultados", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_id_perfil", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_nome_perfil", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_id_setor", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_nome_setor", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_id_filial", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_nome_filial", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_id_pais", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_nome_pais", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_id_estado", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_nome_estado", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_id_cidade", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_nome_cidade", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_imagem", MySqlConnector.MySqlDbType.LongBlob).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_data_ultima_alteracao_senha", MySqlConnector.MySqlDbType.DateTime).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_texto_resposta_email", MySqlConnector.MySqlDbType.Text).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_emails_copia", MySqlConnector.MySqlDbType.Text).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);

                    switch (command.Parameters["p_mensagem"].Value.ToString())
                    {
                        case "Logado com sucesso":
                            return FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_usuario"].Value);

                        case "Valor inativo":
                            throw new ValorInativoException("O usuário informado está inativo");
                        case "Senha inválida":
                            throw new ValorIncorretoException("Senha incorreta");
                        case "Login e/ou e-mail não existe":
                            throw new ValorNaoExisteException("Login e/ou e-mail não existe");
                        default:
                            return null;
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que salva o usuário na database, inserindo um novo se o id for nulo ou editando se o id não for nulo
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task SalvarUsuarioDatabaseAsync(CancellationToken ct, string senha = "")
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
                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_usuario", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_sexo", Sexo);
                        command.Parameters.AddWithValue("p_cpf", CPF);
                        command.Parameters.AddWithValue("p_telefone", Telefone);
                        command.Parameters.AddWithValue("p_email", Email);
                        command.Parameters.AddWithValue("p_login", Login);
                        command.Parameters.AddWithValue("p_senha", senha);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_id_filial", Filial.Id);
                        command.Parameters.AddWithValue("p_id_perfil", Perfil.Id);
                        command.Parameters.AddWithValue("p_id_setor", Setor.Id);
                        command.Parameters.AddWithValue("p_data_cadastro", DataCadastro);
                        command.Parameters.AddWithValue("p_imagem", Imagem);
                        command.Parameters.AddWithValue("p_texto_resposta_email", TextoRespostaEmail);
                        command.Parameters.AddWithValue("p_emails_copia", EmailsEmCopia);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add("p_id_usuario", MySqlConnector.MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        switch (command.Parameters["p_mensagem"].Value.ToString())
                        {
                            case "Valor já existe 1":
                                throw new ValorJaExisteException("O login " + Login.ToString() + " já existe na database");
                            case "Valor já existe 2":
                                throw new ValorJaExisteException("O CPF " + CPF.ToString() + " já existe na database");
                            case "Valor já existe 3":
                                throw new ValorJaExisteException("O e-mail " + Email.ToString() + " já existe na database");
                            default:
                                break;
                        }

                        // Retorna o id da série
                        IdRetornado = FuncoesDeConversao.ConverteParaInt(command.Parameters["p_id_usuario"].Value);
                    }

                    using (MySqlConnector.MySqlCommand command = new("sp_inserir_senha_usuario", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_usuario", IdRetornado);
                        command.Parameters.AddWithValue("p_senha", senha);
                        command.Parameters.AddWithValue("p_data_definicao", DataCadastro);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                        {
                            throw new ValorJaExisteException("Senha já definida anteriormente para o mesmo usuário");
                        }
                    }
                }
                else
                {
                    // Utilização do comando
                    using (MySqlConnector.MySqlCommand command = new("sp_editar_usuario", db.conexao))
                    {
                        // Definição do tipo do comando e dos parâmetros
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("p_id_usuario", Id);
                        command.Parameters.AddWithValue("p_nome", Nome);
                        command.Parameters.AddWithValue("p_sexo", Sexo);
                        command.Parameters.AddWithValue("p_cpf", CPF);
                        command.Parameters.AddWithValue("p_telefone", Telefone);
                        command.Parameters.AddWithValue("p_email", Email);
                        command.Parameters.AddWithValue("p_login", Login);
                        command.Parameters.AddWithValue("p_id_status", Status.Id);
                        command.Parameters.AddWithValue("p_id_filial", Filial.Id);
                        command.Parameters.AddWithValue("p_id_perfil", Perfil.Id);
                        command.Parameters.AddWithValue("p_id_setor", Setor.Id);
                        command.Parameters.AddWithValue("p_data_ultima_alteracao_login", DataUltimaAlteracaoLogin);
                        command.Parameters.AddWithValue("p_data_ultima_alteracao_email", DataUltimaAlteracaoEmail);
                        command.Parameters.AddWithValue("p_imagem", Imagem);
                        command.Parameters.AddWithValue("p_texto_resposta_email", TextoRespostaEmail);
                        command.Parameters.AddWithValue("p_emails_copia", EmailsEmCopia);
                        command.Parameters.AddWithValue("p_cor", Cor);
                        command.Parameters.AddWithValue("p_tema", Tema);
                        command.Parameters.AddWithValue("p_limite_resultados", LimiteResultados);
                        command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                        // Executa o comando asíncrono passando o cancellation token
                        await command.ExecuteNonQueryAsync(ct);

                        // Retorna exceção de acordo com a mensagem retornada pela database
                        if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                        {
                            throw new ValorNaoExisteException("usuário", Login);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que deleta o usuário na database, desde que não exista tabelas se referenciando a esse item
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        /// <exception cref="ChaveEstrangeiraEmUsoException">Exceção que será lançada quando uma exclusão falhar por conta do valor já estar sendo utilizado em outras tabelas</exception>
        public async Task DeletarUsuarioDatabaseAsync(CancellationToken ct)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Define a variável de verificação de deleção
            bool permiteDeletar = await FuncoesDeDatabase.GetPermiteDeletar("tb_usuarios", Id, ct, "tb_senhas_usuarios");

            // Verifica se a deleção é permitida e, caso falso, retorna exceção
            if (!permiteDeletar)
            {
                throw new ChaveEstrangeiraEmUsoException("usuário", Login);
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

                // Utilização do comando
                using (var command = db.conexao.CreateCommand())
                {
                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "DELETE FROM tb_senhas_usuarios WHERE id_usuario = @id_usuario";
                    command.Parameters.AddWithValue("@id_usuario", Id);

                    // Executa o comando para delação das senhas
                    await command.ExecuteNonQueryAsync(ct);
                }

                // Utilização do comando
                using (MySqlConnector.MySqlCommand command = new("sp_excluir_usuario", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario", Id);
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
                            throw new ChaveEstrangeiraEmUsoException("usuário", Login);
                        }

                        // Lança a exceção original
                        throw;
                    }

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("usuário", Login);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que verifica se a senha do usuário está correta na database
        /// </summary>
        /// <param name="id">Id do usuário para verificar a senha</param>
        /// <param name="senha">Senha a ser verificada</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando a validação falhar por conta do id não existir na database</exception>
        public static async Task<bool> SenhaEstaCorretaDatabaseAsync(int? id, string senha, CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_valida_senha", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario", id);
                    command.Parameters.AddWithValue("p_senha", senha);
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);

                    // Verifica se a senha esta´correta e retorna um valor booleano representando isso, ou retorna uma exceção caso o usuário não exista
                    switch (command.Parameters["p_mensagem"].Value.ToString())
                    {
                        case "Senha correta":
                            return true;

                        case "Senha errada":
                            return false;

                        case "Usuario nao encontrado":
                            throw new ValorNaoExisteException("Usuário não encontrado na database");
                        default:
                            return false;
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna a quantidade de usuários ativos
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        public static async Task<int> GetQuantidadeUsuariosAtivos(CancellationToken ct)
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
                      + "COUNT(id_usuario) AS Contagem "
                      + "FROM tb_usuarios WHERE id_status = 1";

                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = comando;

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync(ct))
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync(ct))
                            {
                                // Define as propriedades
                                return (int)FuncoesDeConversao.ConverteParaInt(reader["Contagem"]);
                            }
                        }
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Método assíncrono que altera a senha do usuário
        /// </summary>
        /// <param name="id">Id do usuário para alterar a senha</param>
        /// <param name="senha">Senha a ser definida</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma exclusão falhar por conta do id não existir na database</exception>
        public static async Task AlterarSenhaUsuarioDatabaseAsync(int? id, string senha, CancellationToken ct, DateTime? dataAlteracao = null)
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
                using (MySqlConnector.MySqlCommand command = new("sp_valida_historico_senha", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario", id);
                    command.Parameters.AddWithValue("p_senha", senha);
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Senha existe")
                    {
                        throw new ValorJaExisteException("A senha precisa ser diferente das 5 últimas");
                    }
                }

                // Utilização do comando
                using (MySqlConnector.MySqlCommand command = new("sp_inserir_senha_usuario", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario", id);
                    command.Parameters.AddWithValue("p_senha", senha);
                    command.Parameters.AddWithValue("p_data_definicao", dataAlteracao == null ? DateTime.Now : dataAlteracao);
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor já existe")
                    {
                        throw new ValorJaExisteException("A senha precisa ser diferente das 5 últimas");
                    }
                }

                // Utilização do comando
                using (MySqlConnector.MySqlCommand command = new("sp_altera_senha", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario", id);
                    command.Parameters.AddWithValue("p_senha", senha);
                    command.Parameters.AddWithValue("p_data_ultima_alteracao_senha", dataAlteracao == null ? DateTime.Now : dataAlteracao);
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("Usuário não encontrado na database");
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista de usuários com os argumentos utilizados
        /// </summary>
        /// <param name="listaUsuarios">Representa a lista de usuários que deseja preencher</param>
        /// <param name="limparLista">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        /// <param name="reportadorProgresso">Progresso a ser reportado na ação de preenchimento</param>
        /// <param name="ct">Token de cancelamento</param>
        /// <param name="condicoesExtras">Representa as condições extras como WHERE, GROUP BY, ORDER BY, LIMIT e etc</param>
        /// <param name="nomesParametrosSeparadosPorVirgulas">Representa o nome dos parâmetros passados nas condições extras. Devem começar com @ e ser separados por vírgulas</param>
        /// <param name="valoresParametros">Reresenta um array de parâmetros com os valores dos parâmetros definidos. Devem seguir a mesma ordem do parâmetro <paramref name="nomesParametrosSeparadosPorVirgulas"/></param>
        public static async Task PreencheListaUsuariosAsync(ObservableCollection<Usuario> listaUsuarios, bool limparLista, IProgress<double>? reportadorProgresso, CancellationToken ct, string condicoesExtras, string nomesParametrosSeparadosPorVirgulas, params object?[] valoresParametros)
        {
            // Lança exceção de cancelamento caso ela tenha sido efetuada
            ct.ThrowIfCancellationRequested();

            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaUsuarios == null)
            {
                listaUsuarios = new();
            }

            // Limpa a lista caso verdadeiro
            if (limparLista)
            {
                listaUsuarios.Clear();
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
                  + "usua.id_usuario AS IdUsuario, "
                  + "usua.nome AS NomeUsuario, "
                  + "usua.sexo AS SexoUsuario, "
                  + "usua.cpf AS CPFUsuario, "
                  + "usua.telefone AS TelefoneUsuario, "
                  + "usua.email AS EmailUsuario, "
                  + "usua.login AS LoginUsuario, "
                  + "usua.senha AS SenhaUsuario, "
                  + "usua.data_cadastro AS DataCadastroUsuario, "
                  + "usua.data_ultima_alteracao_senha AS DataUltimaAlteracaoSenhaUsuario, "
                  + "usua.data_ultima_alteracao_login AS DataUltimaAlteracaoLoginUsuario, "
                  + "usua.data_ultima_alteracao_email AS DataUltimaAlteracaoEmailUsuario, "
                  + "usua.imagem AS ImagemUsuario, "
                  + "usua.texto_resposta_email AS TextoRespostaEmailUsuario, "
                  + "usua.emails_copia AS EmailsEmCopiaUsuario, "
                  + "usua.data_sessao AS DataSessaoUsuario, "
                  + "usua.nome_computador_sessao AS NomeComputadorSessaoUsuario, "
                  + "usua.usuario_sessao AS UsuarioSessaoUsuario, "
                  + "usua.cor AS CorUsuario, "
                  + "usua.tema AS TemaUsuario, "
                  + "usua.limite_resultados AS LimiteResultadosUsuario, "
                  + "stat.id_status AS IdStatusUsuario, "
                  + "stat.nome AS NomeStatusUsuario, "
                  + "perf.id_perfil AS IdPerfil, "
                  + "perf.nome AS NomePerfil, "
                  + "stat_perf.id_status AS IdStatusPerfil, "
                  + "stat_perf.nome AS NomeStatusPerfil, "
                  + "seto.id_setor AS IdSetor, "
                  + "seto.nome AS NomeSetor, "
                  + "seto.prazo_adicional AS PrazoAdicional, "
                  + "stat_seto.id_status AS IdStatusSetor, "
                  + "stat_seto.nome AS NomeStatusSetor, "
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
                  + "stat_fili.id_status AS IdStatusFilial, "
                  + "stat_fili.nome AS NomeStatusFilial, "
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
                  + "FROM tb_usuarios AS usua "
                  + "LEFT JOIN tb_status AS stat ON stat.id_status = usua.id_status "
                  + "LEFT JOIN tb_perfis AS perf ON perf.id_perfil = usua.id_perfil "
                  + "LEFT JOIN tb_status AS stat_perf ON stat_perf.id_status = perf.id_status "
                  + "LEFT JOIN tb_setores AS seto ON seto.id_setor = usua.id_setor "
                  + "LEFT JOIN tb_status AS stat_seto ON stat_seto.id_status = seto.id_status "
                  + "LEFT JOIN tb_filiais AS fili ON fili.id_filial = usua.id_filial "
                  + "LEFT JOIN tb_status AS stat_fili ON stat_fili.id_status = fili.id_status "
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
                                Usuario item = new();

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Status == null)
                                {
                                    item.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Perfil == null)
                                {
                                    item.Perfil = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Perfil.Status == null)
                                {
                                    item.Perfil.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Setor == null)
                                {
                                    item.Setor = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Setor.Status == null)
                                {
                                    item.Setor.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Filial == null)
                                {
                                    item.Filial = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Filial.Status == null)
                                {
                                    item.Filial.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Filial.Empresa == null)
                                {
                                    item.Filial.Empresa = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Filial.Empresa.Status == null)
                                {
                                    item.Filial.Empresa.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Filial.Cidade == null)
                                {
                                    item.Filial.Cidade = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Filial.Cidade.Status == null)
                                {
                                    item.Filial.Cidade.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Filial.Cidade.Estado == null)
                                {
                                    item.Filial.Cidade.Estado = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Filial.Cidade.Estado.Status == null)
                                {
                                    item.Filial.Cidade.Estado.Status = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Filial.Cidade.Estado.Pais == null)
                                {
                                    item.Filial.Cidade.Estado.Pais = new();
                                }

                                // Verifica se a classe não foi instânciada e, caso verdadeiro, cria nova instância da classe
                                if (item.Filial.Cidade.Estado.Pais.Status == null)
                                {
                                    item.Filial.Cidade.Estado.Pais.Status = new();
                                }

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["IdUsuario"]);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeUsuario"]);
                                item.Sexo = FuncoesDeConversao.ConverteParaString(reader["SexoUsuario"]);
                                item.CPF = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CPFUsuario"]));
                                item.Telefone = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneUsuario"]));
                                item.Email = FuncoesDeConversao.ConverteParaString(reader["EmailUsuario"]);
                                item.Login = FuncoesDeConversao.ConverteParaString(reader["LoginUsuario"]);
                                item.DataCadastro = FuncoesDeConversao.ConverteParaDateTime(reader["DataCadastroUsuario"]);
                                item.DataUltimaAlteracaoSenha = FuncoesDeConversao.ConverteParaDateTime(reader["DataUltimaAlteracaoSenhaUsuario"]);
                                item.DataUltimaAlteracaoLogin = FuncoesDeConversao.ConverteParaDateTime(reader["DataUltimaAlteracaoLoginUsuario"]);
                                item.DataUltimaAlteracaoEmail = FuncoesDeConversao.ConverteParaDateTime(reader["DataUltimaAlteracaoEmailUsuario"]);
                                item.Imagem = FuncoesDeConversao.ConverteParaArrayDeBytes(reader["ImagemUsuario"]);
                                item.TextoRespostaEmail = FuncoesDeConversao.ConverteParaString(reader["TextoRespostaEmailUsuario"]);
                                item.EmailsEmCopia = FuncoesDeConversao.ConverteParaString(reader["EmailsEmCopiaUsuario"]);
                                item.DataSessao = FuncoesDeConversao.ConverteParaDateTime(reader["DataSessaoUsuario"]);
                                item.NomeComputadorSessao = FuncoesDeConversao.ConverteParaString(reader["NomeComputadorSessaoUsuario"]);
                                item.UsuarioSessao = FuncoesDeConversao.ConverteParaString(reader["UsuarioSessaoUsuario"]);
                                item.Cor = FuncoesDeConversao.ConverteParaString(reader["CorUsuario"]);
                                item.Tema = FuncoesDeConversao.ConverteParaString(reader["TemaUsuario"]);
                                item.LimiteResultados = FuncoesDeConversao.ConverteParaInt(reader["LimiteResultadosUsuario"]);
                                item.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusUsuario"]);
                                item.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusUsuario"]);
                                item.Perfil.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPerfil"]);
                                item.Perfil.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePerfil"]);
                                item.Perfil.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPerfil"]);
                                item.Perfil.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPerfil"]);
                                item.Setor.Id = FuncoesDeConversao.ConverteParaInt(reader["IdSetor"]);
                                item.Setor.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeSetor"]);
                                item.Setor.PrazoAdicional = FuncoesDeConversao.ConverteParaInt(reader["PrazoAdicional"]);
                                item.Setor.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusSetor"]);
                                item.Setor.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusSetor"]);
                                item.Filial.Id = FuncoesDeConversao.ConverteParaInt(reader["IdFilial"]);
                                item.Filial.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeFilial"]);
                                item.Filial.Endereco = FuncoesDeConversao.ConverteParaString(reader["EnderecoFilial"]);
                                item.Filial.CEP = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CEPFilial"]));
                                item.Filial.TelefoneGeral = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneGeralFilial"]));
                                item.Filial.TelefonePecas = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefonePecasFilial"]));
                                item.Filial.TelefoneServicos = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["TelefoneServicosFilial"]));
                                item.Filial.EmailGeral = FuncoesDeConversao.ConverteParaString(reader["EmailGeralFilial"]);
                                item.Filial.EmailPecas = FuncoesDeConversao.ConverteParaString(reader["EmailPecasFilial"]);
                                item.Filial.EmailServicos = FuncoesDeConversao.ConverteParaString(reader["EmailServicosFilial"]);
                                item.Filial.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusFilial"]);
                                item.Filial.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusFilial"]);
                                item.Filial.Empresa.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEmpresa"]);
                                item.Filial.Empresa.CNPJ = FuncoesDeTexto.MantemApenasNumeros(FuncoesDeConversao.ConverteParaString(reader["CNPJEmpresa"]));
                                item.Filial.Empresa.RazaoSocial = FuncoesDeConversao.ConverteParaString(reader["RazaoSocialEmpresa"]);
                                item.Filial.Empresa.NomeFantasia = FuncoesDeConversao.ConverteParaString(reader["NomeFantasiaEmpresa"]);
                                item.Filial.Empresa.Site = FuncoesDeConversao.ConverteParaString(reader["SiteEmpresa"]);
                                item.Filial.Empresa.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEmpresa"]);
                                item.Filial.Empresa.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEmpresa"]);
                                item.Filial.Cidade.Id = FuncoesDeConversao.ConverteParaInt(reader["IdCidade"]);
                                item.Filial.Cidade.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeCidade"]);
                                item.Filial.Cidade.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusCidade"]);
                                item.Filial.Cidade.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusCidade"]);
                                item.Filial.Cidade.Estado.Id = FuncoesDeConversao.ConverteParaInt(reader["IdEstado"]);
                                item.Filial.Cidade.Estado.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeEstado"]);
                                item.Filial.Cidade.Estado.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusEstado"]);
                                item.Filial.Cidade.Estado.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusEstado"]);
                                item.Filial.Cidade.Estado.Pais.Id = FuncoesDeConversao.ConverteParaInt(reader["IdPais"]);
                                item.Filial.Cidade.Estado.Pais.Nome = FuncoesDeConversao.ConverteParaString(reader["NomePais"]);
                                item.Filial.Cidade.Estado.Pais.Status.Id = FuncoesDeConversao.ConverteParaInt(reader["IdStatusPais"]);
                                item.Filial.Cidade.Estado.Pais.Status.Nome = FuncoesDeConversao.ConverteParaString(reader["NomeStatusPais"]);

                                // Lança exceção de cancelamento caso ela tenha sido efetuada
                                ct.ThrowIfCancellationRequested();

                                // Adiciona o item à coleção
                                listaUsuarios.Add(item);

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

        /// <summary>
        /// Método assíncrono que limpa todas as ordens de serviço em uso pelo usuárioi
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public static async Task AlteraIdUsuarioEmUsoAsync(CancellationToken ct, int? idUsuarioAnterior, int? idNovoUsuario)
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
                using (MySqlConnector.MySqlCommand command = new("sp_altera_id_usuario_em_uso_todos_registros", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario_em_uso_anterior", idUsuarioAnterior);
                    command.Parameters.AddWithValue("p_id_usuario_em_uso_novo", idNovoUsuario);

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que limpa todas as ordens de serviço em uso pelo usuárioi
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task LimpaIdUsuarioEmUsoAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_limpa_id_usuario_em_uso_todos_registros", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario_em_uso", Id);

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);
                }
            }
        }

        /// <summary>
        /// Método assíncrono que define a sessão atual do usuário
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task DefineSessaoUsuarioAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_define_sessao_usuario", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario", Id);
                    command.Parameters.AddWithValue("p_data_sessao ", DataSessao);
                    command.Parameters.AddWithValue("p_nome_computador_sessao ", NomeComputadorSessao);
                    command.Parameters.AddWithValue("p_usuario_sessao ", UsuarioSessao);
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("usuário", Login);
                    }
                }
            }
        }

        /// <summary>
        /// Método assíncrono que retorna o id do usuário em uso
        /// </summary>
        /// <param name="ct">Token de cancelamento</param>
        /// <exception cref="ValorJaExisteException">Exceção que será lançada quando uma inserção falhar por já existir um mesmo valor na database. Não utilizada para essa classe</exception>
        /// <exception cref="ValorNaoExisteException">Exceção que será lançada quando uma edição falhar por conta do id não existir na database</exception>
        public async Task ComparaSessaoUsuarioAsync(CancellationToken ct)
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
                using (MySqlConnector.MySqlCommand command = new("sp_retorna_sessao_usuario", db.conexao))
                {
                    // Definição do tipo do comando e dos parâmetros
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_id_usuario", Id);
                    command.Parameters.Add("p_data_sessao", MySqlConnector.MySqlDbType.DateTime).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_nome_computador_sessao", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_usuario_sessao", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add("p_mensagem", MySqlConnector.MySqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;

                    // Executa o comando asíncrono passando o cancellation token
                    await command.ExecuteNonQueryAsync(ct);

                    // Retorna exceção de acordo com a mensagem retornada pela database
                    if (command.Parameters["p_mensagem"].Value.ToString() == "Valor não existe")
                    {
                        throw new ValorNaoExisteException("usuário", Login);
                    }

                    if (FuncoesDeConversao.ConverteParaDateTime(command.Parameters["p_data_sessao"].Value) == null ||
                        FuncoesDeConversao.ConverteParaString(command.Parameters["p_nome_computador_sessao"].Value) == null ||
                        FuncoesDeConversao.ConverteParaString(command.Parameters["p_usuario_sessao"].Value) == null)
                    {
                        throw new SessaoInvalidaException("O seu usuário foi deslogado por um administrador. Por favor, efetue o login novamente");
                    }

                    if (DataSessao.ToString() != FuncoesDeConversao.ConverteParaDateTime(command.Parameters["p_data_sessao"].Value).ToString() ||
                        NomeComputadorSessao != FuncoesDeConversao.ConverteParaString(command.Parameters["p_nome_computador_sessao"].Value) ||
                        UsuarioSessao != FuncoesDeConversao.ConverteParaString(command.Parameters["p_usuario_sessao"].Value))
                    {
                        throw new SessaoInvalidaException("O usuário efetuou o login em outra máquina conforme dados abaixo.\n" +
                            "\nData/hora do login: " + FuncoesDeConversao.ConverteParaDateTime(command.Parameters["p_data_sessao"].Value).ToString() +
                            "\nComputador: " + FuncoesDeConversao.ConverteParaString(command.Parameters["p_nome_computador_sessao"].Value).ToString() +
                            "\nUsuário de rede: " + FuncoesDeConversao.ConverteParaString(command.Parameters["p_usuario_sessao"].Value).ToString() +
                            "\n\nPor favor, efetue o login novamente");
                    }
                }
            }
        }

        public static string GeraSenhaAleatoria(int length = 8)
        {
            // Create a string of characters, numbers, special characters that allowed in the password
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Select one random character at a time from the string
            // and create an array of chars
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        #endregion Métodos

        #region Interfaces

        /// <summary>
        /// Método para criar uma cópia da classe já que esse é um tipo de referência que não pode ser atribuído diretamente
        /// </summary>
        public object Clone()
        {
            Usuario usuarioCopia = new();

            usuarioCopia.Id = Id;
            usuarioCopia.Nome = Nome;
            usuarioCopia.Sexo = Sexo;
            usuarioCopia.CPF = CPF;
            usuarioCopia.Telefone = Telefone;
            usuarioCopia.Email = Email;
            usuarioCopia.Login = Login;
            usuarioCopia.DataCadastro = DataCadastro;
            usuarioCopia.DataUltimaAlteracaoSenha = DataUltimaAlteracaoSenha;
            usuarioCopia.DataUltimaAlteracaoLogin = DataUltimaAlteracaoLogin;
            usuarioCopia.DataUltimaAlteracaoEmail = DataUltimaAlteracaoEmail;
            usuarioCopia.Imagem = Imagem;
            usuarioCopia.TextoRespostaEmail = TextoRespostaEmail;
            usuarioCopia.EmailsEmCopia = EmailsEmCopia;
            usuarioCopia.DataSessao = DataSessao;
            usuarioCopia.NomeComputadorSessao = NomeComputadorSessao;
            usuarioCopia.UsuarioSessao = UsuarioSessao;
            usuarioCopia.Cor = Cor;
            usuarioCopia.Tema = Tema;
            usuarioCopia.LimiteResultados = LimiteResultados;
            usuarioCopia.Perfil = Perfil == null ? new() : (Perfil)Perfil.Clone();
            usuarioCopia.Setor = Setor == null ? new() : (Setor)Setor.Clone();
            usuarioCopia.Filial = Filial == null ? new() : (Filial)Filial.Clone();
            usuarioCopia.Status = Status == null ? new() : (Status)Status.Clone();

            return usuarioCopia;
        }

        #endregion Interfaces
    }
}