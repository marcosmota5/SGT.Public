using GalaSoft.MvvmLight.Messaging;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Threading;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class CustomProgressViewModel : ObservableObject
    {
        #region Campos

        private ICommand _comandoCancelar;
        private CancellationTokenSource _cts;
        private bool _cancelarVisivel = false;
        private bool _progressoEhIndeterminavel = true;
        private bool _cancelarHabilitado = true;
        private double _valorProgresso = 0;
        private string _titulo;
        private string _mensagem;
        private string _textoProgresso;

        #endregion Campos

        #region Propriedades/Comandos

        public ICommand ComandoFechar { get; }

        public ICommand ComandoCancelar
        {
            get
            {
                if (_comandoCancelar == null)
                {
                    _comandoCancelar = new RelayCommand(
                        param => this.Cancelar(),
                        param => true
                    );
                }
                return _comandoCancelar;
            }
        }

        public bool CancelarVisivel
        {
            get { return _cancelarVisivel; }
            set
            {
                if (value != _cancelarVisivel)
                {
                    _cancelarVisivel = value;
                    OnPropertyChanged(nameof(CancelarVisivel));
                }
            }
        }

        public bool ProgressoEhIndeterminavel
        {
            get { return _progressoEhIndeterminavel; }
            set
            {
                if (value != _progressoEhIndeterminavel)
                {
                    _progressoEhIndeterminavel = value;
                    OnPropertyChanged(nameof(ProgressoEhIndeterminavel));
                }
            }
        }

        public bool CancelarHabilitado
        {
            get { return _cancelarHabilitado; }
            set
            {
                if (value != _cancelarHabilitado)
                {
                    _cancelarHabilitado = value;
                    OnPropertyChanged(nameof(CancelarHabilitado));
                }
            }
        }

        public double ValorProgresso
        {
            get { return _valorProgresso; }
            set
            {
                if (value != _valorProgresso)
                {
                    _valorProgresso = value;
                    if (!ProgressoEhIndeterminavel)
                    {
                        TextoProgresso = (value / 100).ToString("P1");
                    }
                    else
                    {
                        TextoProgresso = "";
                    }
                    OnPropertyChanged(nameof(ValorProgresso));
                }
            }
        }

        public string Titulo
        {
            get { return _titulo; }
            set
            {
                if (value != _titulo)
                {
                    _titulo = value;
                    OnPropertyChanged(nameof(Titulo));
                }
            }
        }

        public string Mensagem
        {
            get { return _mensagem; }
            set
            {
                if (value != _mensagem)
                {
                    _mensagem = value;
                    OnPropertyChanged(nameof(Mensagem));
                }
            }
        }

        public string TextoProgresso
        {
            get { return _textoProgresso; }
            set
            {
                if (value != _textoProgresso)
                {
                    _textoProgresso = value;
                    OnPropertyChanged(nameof(TextoProgresso));
                }
            }
        }

        #endregion Propriedades/Comandos

        #region Construtores

        public CustomProgressViewModel(string titulo, string mensagem, bool progressoEhIndeterminavel, bool cancelarVisivel, CancellationTokenSource cts, Action<CustomProgressViewModel> closeHandler)
        {
            Titulo = titulo;
            Mensagem = mensagem;
            ProgressoEhIndeterminavel = progressoEhIndeterminavel;
            CancelarVisivel = cancelarVisivel;
            _cts = cts;

            Messenger.Default.Register<double>(this, "ValorProgresso2", delegate (double valorProgressoRecebido) { ValorProgresso = valorProgressoRecebido; });

            // Atribui o método de limpar listas e a ação de fechar a caixa de diálogo ao comando
            this.ComandoFechar = new SimpleCommand(o => true, o =>
            {
                closeHandler(this);
            });
        }

        #endregion Construtores

        #region Métodos

        private void Cancelar()
        {
            if (_cts != null)
            {
                Mensagem = "Cancelando operação...";
                CancelarHabilitado = false;
                _cts.Cancel();
            }
        }

        #endregion Métodos
    }
}