using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.Classes;
using Model.DataAccessLayer.DataAccessExceptions;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class MensagemComTextBoxViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private string _titulo;
        private string _mensagem;
        private string _tituloTextBox;
        private string _valorTextBox;

        #endregion Campos

        #region Construtores

        public MensagemComTextBoxViewModel(string titulo, string mensagem, string tituloTextBox, string valorTextBox, Action<MensagemComTextBoxViewModel> closeHandler)
        {
            Titulo = titulo.ToUpper();
            Mensagem = mensagem;
            TituloTextBox = tituloTextBox;
            ValorTextBox = valorTextBox;

            // Atribui o método de limpar listas e a ação de fechar a caixa de diálogo ao comando
            this.ComandoFechar = new SimpleCommand(o => true, o =>
            {
                closeHandler(this);
            });
        }

        #endregion Construtores

        #region Propriedades/Comandos

        public string Name
        {
            get
            {
                return "Mensagem";
            }
        }

        public string Icone
        {
            get
            {
                return "ProgressClose";
            }
        }

        public void LimparViewModel()
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        public string Titulo
        {
            get { return _titulo; }
            set
            {
                _titulo = value;
                OnPropertyChanged(nameof(Titulo));
            }
        }

        public string Mensagem
        {
            get { return _mensagem; }
            set
            {
                _mensagem = value;
                OnPropertyChanged(nameof(Mensagem));
            }
        }

        public string TituloTextBox
        {
            get { return _tituloTextBox; }
            set
            {
                _tituloTextBox = value;
                OnPropertyChanged(nameof(TituloTextBox));
            }
        }

        public string ValorTextBox
        {
            get { return _valorTextBox; }
            set
            {
                _valorTextBox = value;
                OnPropertyChanged(nameof(ValorTextBox));
            }
        }

        public ICommand ComandoFechar { get; }

        #endregion Propriedades/Comandos
    }
}