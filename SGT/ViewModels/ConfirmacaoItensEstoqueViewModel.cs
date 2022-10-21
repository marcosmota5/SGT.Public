using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class ConfirmacaoItensEstoqueViewModel : ObservableObject
    {
        #region Campos

        private bool _carregamentoVisivel = true;
        private bool _controlesHabilitados;
        private ICommand _comandoConfirmar;
        private ObservableCollection<ValidacaoItemPropostaEstoque> _listaValidacaoItemPropostaEstoque = new();

        #endregion Campos

        #region Propriedades/Comandos

        public ICommand ComandoFechar { get; }

        public ICommand ComandoConfirmar
        {
            get
            {
                if (_comandoConfirmar == null)
                {
                    _comandoConfirmar = new RelayCommand(
                        param => Confirmar(),
                        param => true
                    );
                }

                return _comandoConfirmar;
            }
        }

        public ObservableCollection<ValidacaoItemPropostaEstoque> ListaValidacaoItemPropostaEstoque
        {
            get { return _listaValidacaoItemPropostaEstoque; }
            set
            {
                if (value != _listaValidacaoItemPropostaEstoque)
                {
                    _listaValidacaoItemPropostaEstoque = value;
                    OnPropertyChanged(nameof(ListaValidacaoItemPropostaEstoque));
                }
            }
        }

        public bool ControlesHabilitados
        {
            get { return _controlesHabilitados; }
            set
            {
                if (value != _controlesHabilitados)
                {
                    _controlesHabilitados = value;
                    OnPropertyChanged(nameof(ControlesHabilitados));
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

        #endregion Propriedades/Comandos

        #region Construtores

        public ConfirmacaoItensEstoqueViewModel(ObservableCollection<ValidacaoItemPropostaEstoque> listaValidacaoItemPropostaEstoque, Action<ConfirmacaoItensEstoqueViewModel> closeHandler)
        {
            try
            {
                ListaValidacaoItemPropostaEstoque = listaValidacaoItemPropostaEstoque;

                // Atribui o método de limpar listas e a ação de fechar a caixa de diálogo ao comando
                this.ComandoFechar = new SimpleCommand(o => true, o =>
                {
                    closeHandler(this);
                });

                ControlesHabilitados = true;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Erro ao carregar dados");
                ControlesHabilitados = false;
            }
            CarregamentoVisivel = false;
        }

        #endregion Construtores

        #region Métodos

        private void Confirmar()
        {
            ControlesHabilitados = false;

            try
            {
                foreach (var item in ListaValidacaoItemPropostaEstoque)
                {
                    if (item.IsSelected)
                    {
                        item.ItemProposta.PrazoFinalItem = ExcelClasses.RetornaPrazo(item.ItemProposta.QuantidadeItem == null ? 0 : (decimal)item.ItemProposta.QuantidadeItem, item.ItemProposta.PrazoInicialItem == null ? "" : item.ItemProposta.PrazoInicialItem.Replace("*", "").Trim(),
                                item.QuantidadeEstoqueAbreviado, App.Usuario.Setor.PrazoAdicional == null ? 0 : (int)App.Usuario.Setor.PrazoAdicional);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                ControlesHabilitados = true;
            }

            ComandoFechar.Execute(null);
        }

        #endregion Métodos
    }
}