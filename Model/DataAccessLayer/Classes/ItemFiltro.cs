using Model.DataAccessLayer.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DataAccessLayer.Conexoes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.Funcoes;

namespace Model.DataAccessLayer.Classes
{
    public class ItemFiltro : ObservableObject
    {
        private int? _id;
        private string? _nome;
        private bool _selecionado;

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

        public bool Selecionado
        {
            get { return _selecionado; }
            set
            {
                if (value != _selecionado)
                {
                    _selecionado = value;
                    OnPropertyChanged(nameof(Selecionado));
                }
            }
        }

        /// <summary>
        /// Método assíncrono que preenche uma lista itens com os argumentos utilizados
        /// </summary>
        /// <param name="listaItemFiltro">Representa a lista de status que deseja preencher</param>
        /// <param name="comando">Representa a opção de limpar a lista antes de preenchê-la. Verdadeiro por padrão</param>
        public static async Task PreencheListaItemFiltroAsync(ObservableCollection<ItemFiltro> listaItemFiltro, string comando)
        {
            // Verifica se a lista não foi instanciada e, caso verdadeiro, cria nova instância da lista
            if (listaItemFiltro == null)
            {
                listaItemFiltro = new();
            }

            // Limpa a lista caso verdadeiro
            listaItemFiltro.Clear();

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

                // Utilização do comando
                using (var command = db.conexao.CreateCommand())
                {
                    // Definição do tipo, texto e parâmetros do comando
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = comando;

                    // Utilização do reader para retornar os dados asíncronos
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Verifica se o reader possui linhas
                        if (reader.HasRows)
                        {
                            // Enquanto o reader possuir linhas, define os valores
                            while (await reader.ReadAsync())
                            {
                                // Cria um novo item e atribui os valores
                                ItemFiltro item = new();

                                // Define as propriedades
                                item.Id = FuncoesDeConversao.ConverteParaInt(reader["Id"]);
                                item.Nome = FuncoesDeConversao.ConverteParaString(reader["Nome"]);
                                item.Selecionado = false;

                                // Adiciona o item à coleção
                                listaItemFiltro.Add(item);
                            }
                        }
                    }
                }
            }
        }
    }
}