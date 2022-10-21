using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SGT.ViewModels
{
    public class LogAlteracoesViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private bool _carregamentoVisivel = true;
        private ObservableCollection<RegistroAlteracao> _listaRegistrosAlteracao = new();

        #endregion Campos

        #region Construtores

        public LogAlteracoesViewModel()
        {
            ConstrutorAsync().Await();
        }

        public LogAlteracoesViewModel(Versao versaoAtual)
        {
            ConstrutorAsync(versaoAtual).Await();
        }

        #endregion Construtores

        #region Propriedades / Comandos

        public string Name
        {
            get
            {
                return "Log de alterações";
            }
        }

        public string Icone
        {
            get
            {
                return "";
            }
        }

        public void LimparViewModel()
        {
            try
            {
                _listaRegistrosAlteracao = null;
            }
            catch (Exception)
            {
            }
        }

        public ObservableCollection<RegistroAlteracao> ListaRegistrosAlteracao
        {
            get
            {
                return _listaRegistrosAlteracao;
            }
            set
            {
                if (_listaRegistrosAlteracao != value)
                {
                    _listaRegistrosAlteracao = value;
                    OnPropertyChanged(nameof(ListaRegistrosAlteracao));
                }
            }
        }

        public bool CarregamentoVisivel
        {
            get { return _carregamentoVisivel; }
            set
            {
                if (_carregamentoVisivel != value)
                {
                    _carregamentoVisivel = value;
                    OnPropertyChanged(nameof(CarregamentoVisivel));
                }
            }
        }

        #endregion Propriedades / Comandos

        #region Métodos

        public async Task ConstrutorAsync()
        {
            try
            {
                ObservableCollection<Versao> listaVersoes = new();
                await Versao.PreencheListaVersoesAsync(listaVersoes, true, false, null, CancellationToken.None, "ORDER BY data_lancamento DESC", "");

                ObservableCollection<RegistroAlteracao> listaTemporaria = new();
                await RegistroAlteracao.PreencheListaRegistrosAlteracaoAsync(listaTemporaria, true, true, null, CancellationToken.None, "ORDER BY vers.data_lancamento DESC", "");

                foreach (var item in listaTemporaria)
                {
                    item.Versao = listaVersoes.First(vers => vers.Id == item.Versao.Id);
                    ListaRegistrosAlteracao.Add(item);
                }

                listaVersoes.Clear();
                listaVersoes = null;

                listaTemporaria.Clear();
                listaTemporaria = null;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");
            }
            CarregamentoVisivel = false;
        }

        public async Task ConstrutorAsync(Versao versaoAtual)
        {
            try
            {
                ObservableCollection<Versao> listaVersoes = new();
                await Versao.PreencheListaVersoesAsync(listaVersoes, true, false, null, CancellationToken.None, "WHERE id_versao > @id_versao ORDER BY id_versao DESC", "@id_versao", versaoAtual.Id);

                ObservableCollection<RegistroAlteracao> listaTemporaria = new();
                await RegistroAlteracao.PreencheListaRegistrosAlteracaoAsync(listaTemporaria, true, true, null, CancellationToken.None, "WHERE vers.id_versao > @id_versao ORDER BY vers.id_versao DESC", "@id_versao", versaoAtual.Id);

                foreach (var item in listaTemporaria)
                {
                    item.Versao = listaVersoes.First(vers => vers.Id == item.Versao.Id);
                    ListaRegistrosAlteracao.Add(item);
                }

                listaVersoes.Clear();
                listaVersoes = null;

                listaTemporaria.Clear();
                listaTemporaria = null;
            }
            catch (Exception ex)
            {
                // Escreve no log a exceção e uma mensagem de erro
                Serilog.Log.Error(ex, "Erro ao carregar dados");
            }
            CarregamentoVisivel = false;
        }

        #endregion Métodos
    }
}