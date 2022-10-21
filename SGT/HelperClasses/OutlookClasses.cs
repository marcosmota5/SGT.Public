using Microsoft.Office.Interop.Outlook;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.HelperClasses;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace SGT.HelperClasses
{
    public class OutlookClasses
    {
        #region Métodos

        /// <summary>
        /// Método assíncrono para retornar os e-mails de cotações com os parâmetros utilizados
        /// </summary>
        /// <param name="pastaSelecionada">Pasta de e-mail a ser verificada</param>
        /// <param name="fornecedor">Fornecedor a ser verificado</param>
        /// <param name="dataDe">Data inicial do filtro</param>
        /// <param name="dataAte">Data final do filtro</param>
        /// <param name="listaItensEmail">Lista de itens de e-mail a ser populada</param>
        /// <param name="reportadorProgresso">Progresso a se reportado</param>
        /// <param name="ct">Token de cancelamento</param>
        public static async Task PreencheListaEmailsCotacoesAsync(PastaEmail pastaSelecionada, Fornecedor fornecedor, DateTime? dataDe, DateTime? dataAte,
    ObservableCollection<MailItemComIsSelected> listaItensEmail, IProgress<double>? reportadorProgresso, CancellationToken ct)
        {
            try
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Se não houver pasta selecionada, encerra o méotod
                if (pastaSelecionada is null)
                {
                    return;
                }

                // Declara a lista de itens de email
                ObservableCollection<MailItemComIsSelected> listaItensEmailGerada = new();

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                await Task.Run(() =>
                {
                    // Define o texto a ser procurado no assunto do e-mail de cotação
                    string textoFornecedor = "";

                    switch (fornecedor.Id)
                    {
                        case 1:
                            textoFornecedor = "COTACAO-";
                            break;

                        case 2:
                            textoFornecedor = "TVH - Orçamento";
                            break;

                        default:
                            break;
                    }

                    // Declaração das variáveis
                    Application olApp = new();
                    NameSpace olNameSpace = olApp.GetNamespace("MAPI");
                    MAPIFolder olFolder = olNameSpace.GetFolderFromID(pastaSelecionada.Id);
                    MailItem olMail;
                    Items olItems;
                    Items olItemsRestrict;
                    DateTime dataDeNaoNula = DateTime.MinValue;
                    DateTime dataAteNaoNula = DateTime.MaxValue;

                    if (dataDe is not null)
                    {
                        dataDeNaoNula = (DateTime)dataDe;
                    }

                    if (dataAte is not null)
                    {
                        dataAteNaoNula = ((DateTime)dataAte).AddDays(1);
                    }

                    try
                    {
                        listaItensEmailGerada.Clear();
                        olItems = olFolder.Items;
                        olItemsRestrict = olItems.Restrict("[ReceivedTime] >= '" + dataDeNaoNula.ToShortDateString() + "' And [ReceivedTime] < '" + dataAteNaoNula.ToShortDateString() + "'");
                        olItemsRestrict.Sort("[ReceivedTime]", true);

                        // Cria e atribui a variável de contagem de linhas
                        int linhaAtual = 0;

                        // Cria e atribui a variável do total de linhas
                        int totalLinhas = olItemsRestrict.Count;

                        // Lança exceção de cancelamento caso ela tenha sido efetuada
                        ct.ThrowIfCancellationRequested();

                        foreach (var olItem in olItemsRestrict)
                        {
                            if (olItem is MailItem)
                            {
                                olMail = (MailItem)olItem;
                                if (olMail.Subject is not null)
                                {
                                    if (olMail.Subject.Contains(textoFornecedor))
                                    {
                                        if (fornecedor.Id == 1)
                                        {
                                            bool contemProposta = false;
                                            foreach (Attachment anexo in olMail.Attachments)
                                            {
                                                if (anexo.FileName.EndsWith("xls"))
                                                {
                                                    contemProposta = true;
                                                    break;
                                                }
                                            }
                                            if (contemProposta)
                                            {
                                                MailItemComIsSelected mailItemComIsSelected = new MailItemComIsSelected { IsSelected = false, MailItem = olMail };
                                                listaItensEmailGerada.Add(mailItemComIsSelected);
                                            }
                                        }
                                        else
                                        {
                                            MailItemComIsSelected mailItemComIsSelected = new MailItemComIsSelected { IsSelected = false, MailItem = olMail };
                                            listaItensEmailGerada.Add(mailItemComIsSelected);
                                        }
                                    }
                                }
                            }

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
                    catch (System.Exception)
                    {
                        throw;
                    }
                });

                listaItensEmail.Clear();

                foreach (MailItemComIsSelected mailItem in listaItensEmailGerada)
                {
                    listaItensEmail.Add(mailItem);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método assíncrono para retornar os e-mails com os parâmetros utilizados
        /// </summary>
        /// <param name="pastaSelecionada">Pasta de e-mail a ser verificada</param>
        /// <param name="dataDe">Data inicial do filtro</param>
        /// <param name="dataAte">Data final do filtro</param>
        /// <param name="listaItensEmail">Lista de itens de e-mail a ser populada</param>
        /// <param name="reportadorProgresso">Progresso a se reportado</param>
        /// <param name="ct">Token de cancelamento</param>
        public static async Task PreencheListaEmailsAsync(PastaEmail pastaSelecionada, DateTime? dataDe, DateTime? dataAte,
    ObservableCollection<MailItemComIsSelected> listaItensEmail, IProgress<double>? reportadorProgresso, CancellationToken ct)
        {
            try
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                // Se não houver pasta selecionada, encerra o méotod
                if (pastaSelecionada is null)
                {
                    return;
                }

                // Declara a lista de itens de email
                ObservableCollection<MailItemComIsSelected> listaItensEmailGerada = new();

                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                await Task.Run(() =>
                {
                    // Declaração das variáveis
                    Application olApp = new();
                    NameSpace olNameSpace = olApp.GetNamespace("MAPI");
                    MAPIFolder olFolder = olNameSpace.GetFolderFromID(pastaSelecionada.Id);
                    MailItem olMail;
                    Items olItems;
                    Items olItemsRestrict;
                    DateTime dataDeNaoNula = DateTime.MinValue;
                    DateTime dataAteNaoNula = DateTime.MaxValue;

                    if (dataDe is not null)
                    {
                        dataDeNaoNula = (DateTime)dataDe;
                    }

                    if (dataAte is not null)
                    {
                        dataAteNaoNula = ((DateTime)dataAte).AddDays(1);
                    }

                    try
                    {
                        listaItensEmailGerada.Clear();
                        olItems = olFolder.Items;
                        olItemsRestrict = olItems.Restrict("[ReceivedTime] >= '" + dataDeNaoNula.ToShortDateString() + "' And [ReceivedTime] < '" + dataAteNaoNula.ToShortDateString() + "'");
                        olItemsRestrict.Sort("[ReceivedTime]", true);

                        // Cria e atribui a variável de contagem de linhas
                        int linhaAtual = 0;

                        // Cria e atribui a variável do total de linhas
                        int totalLinhas = olItemsRestrict.Count;

                        // Lança exceção de cancelamento caso ela tenha sido efetuada
                        ct.ThrowIfCancellationRequested();

                        // Varre a lista de e-mails filtrada e preenche a lista
                        foreach (var olItem in olItemsRestrict)
                        {
                            if (olItem is MailItem)
                            {
                                olMail = (MailItem)olItem;
                                if (olMail.Subject is not null)
                                {
                                    MailItemComIsSelected mailItemComIsSelected = new MailItemComIsSelected { IsSelected = false, MailItem = olMail };
                                    listaItensEmailGerada.Add(mailItemComIsSelected);
                                }
                            }

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
                    catch (System.Exception)
                    {
                        throw;
                    }
                });

                listaItensEmail.Clear();

                foreach (MailItemComIsSelected mailItem in listaItensEmailGerada)
                {
                    listaItensEmail.Add(mailItem);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método assíncrono para retornar as pastas de e-mail
        /// </summary>
        /// <returns>Lista com as pastas de e-mail</returns>
        public static async Task PreenchePastasEmailAsync(ObservableCollection<PastaEmail> listaPastaEmails)
        {
            try
            {
                // Declaração da lista temporária
                ObservableCollection<PastaEmail> tempListaPastasEmail = new();

                await Task.Run(() =>
                {
                    Application olApp = new();
                    NameSpace olNameSpace = olApp.GetNamespace("MAPI");

                    foreach (MAPIFolder olFolder in olNameSpace.Folders)
                    {
                        PastaEmail pastaPrincipal = new();
                        pastaPrincipal.Id = olFolder.EntryID;
                        pastaPrincipal.Nome = olFolder.Name;
                        foreach (MAPIFolder olSubFolder1 in olFolder.Folders)
                        {
                            PastaEmail subPasta1 = new();
                            subPasta1.Id = olSubFolder1.EntryID;
                            subPasta1.Nome = olSubFolder1.Name;
                            pastaPrincipal.ListaPastas.Add(subPasta1);
                            foreach (MAPIFolder olSubFolder2 in olSubFolder1.Folders)
                            {
                                PastaEmail subPasta2 = new();
                                subPasta2.Id = olSubFolder2.EntryID;
                                subPasta2.Nome = olSubFolder2.Name;
                                subPasta1.ListaPastas.Add(subPasta2);
                                foreach (MAPIFolder olSubFolder3 in olSubFolder2.Folders)
                                {
                                    PastaEmail subPasta3 = new();
                                    subPasta3.Id = olSubFolder3.EntryID;
                                    subPasta3.Nome = olSubFolder3.Name;
                                    subPasta2.ListaPastas.Add(subPasta3);
                                    foreach (MAPIFolder olSubFolder4 in olSubFolder3.Folders)
                                    {
                                        PastaEmail subPasta4 = new();
                                        subPasta4.Id = olSubFolder4.EntryID;
                                        subPasta4.Nome = olSubFolder4.Name;
                                        subPasta3.ListaPastas.Add(subPasta4);
                                        foreach (MAPIFolder olSubFolder5 in olSubFolder4.Folders)
                                        {
                                            PastaEmail subPasta5 = new();
                                            subPasta5.Id = olSubFolder5.EntryID;
                                            subPasta5.Nome = olSubFolder5.Name;
                                            subPasta4.ListaPastas.Add(subPasta5);
                                        }
                                    }
                                }
                            }
                        }
                        tempListaPastasEmail.Add(pastaPrincipal);
                    }
                });

                // Limpa a lista passada
                listaPastaEmails.Clear();

                // Preenche a lista passada com a lista temporária
                foreach (var item in tempListaPastasEmail)
                {
                    listaPastaEmails.Add(item);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            if (listaPastaEmails == null)
            {
                listaPastaEmails = new();
            }            
        }

        /// <summary>
        /// Método assíncrono que gera o email de envio da proposta
        /// </summary>
        /// <param name="caminhoArquivo"></param>
        /// <param name="textoInicial"></param>
        /// <param name="emailsEmCopia"></param>
        /// <param name="assunto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task GeraEmailEnvioPropostaAsync(string caminhoArquivo, string textoInicial, string emailsEmCopia, string assunto, CancellationToken ct)
        {
            try
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                await Task.Run(() =>
                {
                    // Declaração das variáveis
                    Application olApp = new();
                    NameSpace olNameSpace = olApp.GetNamespace("MAPI");
                    MailItem olMail;

                    try
                    {
                        // Lança exceção de cancelamento caso ela tenha sido efetuada
                        ct.ThrowIfCancellationRequested();

                        // Cria o item de e-mail
                        olMail = olApp.CreateItem(OlItemType.olMailItem);

                        olMail.Display();
                        olMail.Subject = assunto;
                        olMail.CC = emailsEmCopia;
                        olMail.Attachments.Add(caminhoArquivo);
                        string assinatura = olMail.HTMLBody;
                        olMail.HTMLBody = textoInicial + "<br>" + assinatura;
                    }
                    catch (System.Exception)
                    {
                        throw;
                    }
                });
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método assíncrono que gera o email de envio da proposta
        /// </summary>
        /// <param name="mailItemOrigem"></param>
        /// <param name="caminhoArquivo"></param>
        /// <param name="textoInicial"></param>
        /// <param name="emailsEmCopia"></param>
        /// <param name="assunto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task ResponderEmailPropostaAsync(MailItem mailItemOrigem, string caminhoArquivo, string textoInicial, string emailsEmCopia, string assunto, CancellationToken ct)
        {
            try
            {
                // Lança exceção de cancelamento caso ela tenha sido efetuada
                ct.ThrowIfCancellationRequested();

                await Task.Run(() =>
                {
                    try
                    {
                        // Lança exceção de cancelamento caso ela tenha sido efetuada
                        ct.ThrowIfCancellationRequested();

                        // Cria o item de e-mail
                        MailItem olMail = mailItemOrigem.ReplyAll();

                        olMail.Display();
                        olMail.Subject = olMail.Subject + " - " + assunto;
                        olMail.CC = olMail.CC + ";" + emailsEmCopia;
                        olMail.Attachments.Add(caminhoArquivo);
                        string assinatura = olMail.HTMLBody;
                        olMail.HTMLBody = textoInicial + "<br>" + assinatura;
                    }
                    catch (System.Exception)
                    {
                        throw;
                    }
                });
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        #endregion Métodos
    }

    public class MailItemComIsSelected : ObservableObject
    {
        #region Campos

        private MailItem _mailItem;
        private bool? _isSelected;

        #endregion Campos

        #region Propriedades

        public MailItem MailItem
        {
            get { return _mailItem; }
            set
            {
                if (value != _mailItem)
                {
                    _mailItem = value;
                    OnPropertyChanged(nameof(MailItem));
                }
            }
        }

        public bool? IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != IsSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        #endregion Propriedades
    }

    public class PastaEmail : ObservableObject, ICloneable
    {
        #region Campos

        private string _id;
        private string _nome;
        private ObservableCollection<PastaEmail> _listaPastas;

        #endregion Campos

        #region Propriedades

        public string Id
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

        public ObservableCollection<PastaEmail> ListaPastas
        {
            get { return _listaPastas; }
            set
            {
                if (value != _listaPastas)
                {
                    _listaPastas = value;
                    OnPropertyChanged(nameof(ListaPastas));
                }
            }
        }

        #endregion Propriedades

        #region Construtores

        public PastaEmail()
        {
            ListaPastas = new();
        }

        #endregion Construtores

        #region Interfaces

        public object Clone()
        {
            PastaEmail pastaEmailCopia = new();

            pastaEmailCopia.Id = Id;
            pastaEmailCopia.Nome = Nome;
            foreach (var pasta in ListaPastas)
            {
                pastaEmailCopia.ListaPastas.Add(pasta);
            }

            return pastaEmailCopia;
        }

        #endregion Interfaces
    }
}