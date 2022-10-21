using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Model.Email
{
    public class Email
    {
        #region Constantes

        private const string EMAIL_ENVIO = "sgt@euroliftempilhadeiras.com.br";
        private const string USUARIO_ENVIO = "sgt@euroliftempilhadeiras.com.br";
        private const string SENHA_ENVIO = "U74sH,;_f@{y";
        private const string HOST_ENVIO = "mail.euroliftempilhadeiras.com.br";
        private const int PORTA_ENVIO = 587;

        #endregion Constantes

        /// <summary>
        /// Função estática que gera um código de recuperação aleatório com os parâmetros utilizados
        /// </summary>
        /// <param name="tamanhoCodigo">Tamanho que o código de recuperação terá</param>
        /// <returns>String contendo o código de recuperação gerado</returns>
        public static string GerarCodigoRecuperacaoSenha(int tamanhoCodigo = 6)
        {
            string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int indiceInicio;
            var aleatorio = new Random();
            var construtorString = new StringBuilder();

            for (int i = 1; i <= tamanhoCodigo; i++)
            {
                indiceInicio = aleatorio.Next(0, 35);
                construtorString.Append(caracteres.Substring(indiceInicio, 1));
            }
            return construtorString.ToString();
        }

        /// <summary>
        /// Função estática que verifica se um endereço de e-mail informado é válido
        /// </summary>
        /// <param name="enderecoEmail">Endereço de e-mail a ser verificado</param>
        /// <returns>Valor booleano indicando se o endereço de e-mail é válido ou não</returns>
        public static bool EmailEhValido(string enderecoEmail)
        {
            try
            {
                var endereco = new MailAddress(enderecoEmail);
                return endereco.Address == enderecoEmail;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Função estática assíncrona para enviar o e-mail de recuperação ao usuário
        /// </summary>
        /// <param name="usuario">Usuário para o qual o e-mail será enviado</param>
        /// <param name="codigoRecuperacao">Código de recuperação a ser enviado</param>
        /// <exception cref="ValorNaoExisteException">Exceção que será retornada caso o usuário não exista</exception>
        public static async Task EnviarEmailRecuperacaoSenha(Usuario usuario, string codigoRecuperacao)
        {
            // Verifica se o endereço do e-mail do usuário é nulo e, caso verdadeiro, lança exceção
            if (usuario.Email == null)
            {
                throw new ValorNaoExisteException("Usuário não encontrado");
            }

            string corpoEmail = "<html><head><style>header {    background-color:darkblue;    color:white;    text-align:left;    padding:10px;    width:700px;    height:20px;    line-height:2px} section {    width:500px;    float:left;    padding:1px;    line-height:15px;}" +
                    "footer {    background-color:darkblue;    color:white;    clear:both;    text-align:center;    width:700px;    height:1px;    padding:10px;}</style></head><body>" +
                    "<header><font size='4'><b>Proreports SGT</b></font><section><p align='left'><font size='4' color='black'><br><b>Prezado(a) " +
                    usuario.Nome + ",</b></font><font size='3' color='black'><br><br>Segue o código de recuperação de senha. Copie esse código e cole na caixa correspondente na ferramenta:" +
                    "<br><br>" + codigoRecuperacao + "<br><br>" +
                    "</section><footer></footer><section><font size='2' color='black'><center>SGT - Sistema de Gerenciamento Total<br>2020-" + DateTime.Now.Year.ToString() + ". Proreports. Todos os direitos reservados.</center></section></body></html>";
            try
            {
                await EnviarEmailAsync(usuario.Email, "SGT - Recuperação de senha", corpoEmail);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Função estática assíncrona para enviar o e-mail de recuperação ao usuário
        /// </summary>
        /// <exception cref="ValorNaoExisteException">Exceção que será retornada caso o usuário não exista</exception>
        public static async Task EnviarEmailManifestacao(bool ehNova, string versao, string nomePessoa, string emailPessoa, RegistroManifestacao registroManifestacao)
        {
            string codigoManifestacao = "REQN" + ((int)registroManifestacao.Id).ToString("00000000");
            string data = DateTime.Now.ToString("d");
            string hora = DateTime.Now.ToString("t");
            string prioridade = registroManifestacao.PrioridadeManifestacao.Nome;
            string tipo = registroManifestacao.TipoManifestacao.Nome;
            string status = registroManifestacao.StatusManifestacao.Nome;
            string emailDesenvolvedor = "marcos.mota@proreports.com.br";
            string assunto = "SGT - Requisição " + codigoManifestacao + " - " + status;
            string textoInicial;

            if (ehNova)
            {
                textoInicial = "Uma nova requisição foi criada no SGT. Detalhes seguem abaixo. Para mais informações, verifique no sistema.";
            }
            else
            {
                textoInicial = "A requisição abaixo foi alterada no SGT. Para mais informações, verifique no sistema.";
            }

            string corpoEmail = "<html><head><style>header {    background-color: darkblue;    color:white;    text-align:left;    padding:10px;    width:700px;    height:20px;    line-height:2px} section {    width:500px;    float:left;    padding:1px;    line-height:15px;}" +
                  "footer {    background-color:darkblue;    color:white;    clear:both;    text-align:center;    width:700px;    height:1px;    padding:10px;}</style></head><body>" +
                  "<header><font size='4'><b>Proreports SGT</b></font><section><p align='left'><font size='4' color='black'><br><br><b>Prezado(a) " + FuncoesDeTexto.PrimeiroNome(nomePessoa) + ", </b></font><font size='3' color='black'><br><br>" + textoInicial +
                  "<br><br><b>Código:</b> " + codigoManifestacao +
                  "<br><b>Data:</b> " + data +
                  "<br><b>Hora:</b> " + hora +
                  "<br><b>Tipo:</b> " + tipo +
                  "<br><b>Prioridade:</b> " + prioridade +
                  "<br><b>Status:</b> " + status +
                  "<br><b>Versão do sistema:</b> " + versao +
                  "<br><b>Nome do usuário:</b> " + nomePessoa +
                  "<br><b>E-mail do usuário:</b> " + emailPessoa +
                  "</section><footer></footer><section><font size='2' color='black'><center>SGT - Sistema de Gerenciamento Total<br>2020-" + DateTime.Now.Year.ToString() + ". Proreports. Todos os direitos reservados.</center></section></body></html>";

            try
            {
                await EnviarEmailAsync(registroManifestacao.EmailPessoaAbertura!, assunto, corpoEmail);

                if (registroManifestacao.EmailPessoaAbertura! != emailDesenvolvedor)
                {
                    corpoEmail = "<html><head><style>header {    background-color: darkblue;    color:white;    text-align:left;    padding:10px;    width:700px;    height:20px;    line-height:2px} section {    width:500px;    float:left;    padding:1px;    line-height:15px;}" +
                          "footer {    background-color:darkblue;    color:white;    clear:both;    text-align:center;    width:700px;    height:1px;    padding:10px;}</style></head><body>" +
                          "<header><font size='4'><b>Proreports SGT</b></font><section><p align='left'><font size='4' color='black'><br><br><b>Prezado(a) Desenvolvedor, </b></font><font size='3' color='black'><br><br>" + textoInicial +
                          "<br><br><b>Código:</b> " + codigoManifestacao +
                          "<br><b>Data:</b> " + data +
                          "<br><b>Hora:</b> " + hora +
                          "<br><b>Tipo:</b> " + tipo +
                          "<br><b>Prioridade:</b> " + prioridade +
                          "<br><b>Status:</b> " + status +
                          "<br><b>Versão do sistema:</b> " + versao +
                          "<br><b>Nome do usuário:</b> " + nomePessoa +
                          "<br><b>E-mail do usuário:</b> " + emailPessoa +
                          "</section><footer></footer><section><font size='2' color='black'><center>SGT - Sistema de Gerenciamento Total<br>2020-" + DateTime.Now.Year.ToString() + ". Proreports. Todos os direitos reservados.</center></section></body></html>";

                    await EnviarEmailAsync(emailDesenvolvedor, assunto, corpoEmail);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Função estática assíncrona para enviar um e-mail de acordo com os parâmetros utilizados
        /// </summary>
        /// <param name="emailsPara">Endereços de e-mail 'Para'. Devem estar separados por ponto e vírgula ';'</param>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="htmlBody">Corpo do e-mail escrito em html</param>
        /// <param name="emailsCopia">Endereços de e-mail em cópia. Devem estar separados por ponto e vírgula ';'</param>
        /// <param name="listaDeAnexos">Lista de strings contendo o caminho dos anexos</param>
        public static async Task EnviarEmailAsync(string emailsPara, string assunto, string htmlBody, string emailsCopia = "", List<string>? listaDeAnexos = null)
        {
            var mensagemEmail = new MailMessage();

            if (!String.IsNullOrEmpty(emailsPara))
            {
                mensagemEmail.To.Add(emailsPara);
            }

            mensagemEmail.From = new MailAddress(EMAIL_ENVIO);
            mensagemEmail.Subject = assunto;
            mensagemEmail.Body = htmlBody;
            mensagemEmail.IsBodyHtml = true;

            if (!String.IsNullOrEmpty(emailsCopia))
            {
                mensagemEmail.CC.Add(emailsCopia);
            }

            if (listaDeAnexos != null)
            {
                foreach (string anexo in listaDeAnexos)
                {
                    mensagemEmail.Attachments.Add(new Attachment(anexo));
                }
            }

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Credentials = new NetworkCredential(USUARIO_ENVIO, SENHA_ENVIO);
                smtpClient.Host = HOST_ENVIO;
                smtpClient.Port = PORTA_ENVIO;

                try
                {
                    await smtpClient.SendMailAsync(mensagemEmail);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}