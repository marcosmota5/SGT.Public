using Model.DataAccessLayer.HelperClasses;

namespace SGT.HelperClasses
{
    public class Parametro : ObservableObject
    {
        private string _nome;
        private string _valor;
        private string _tipo;
        private string _formato;
        private string _icone;
        private string _cor;

        public Parametro(string nome, string valor, string tipo, string formato, string icone, string cor)
        {
            Nome = nome;
            Valor = valor;
            Tipo = tipo;
            Formato = formato;
            Icone = icone;
            Cor = cor;
        }

        public string Valor
        {
            get { return _valor; }
            set
            {
                if (value != _valor)
                {
                    _valor = value;
                    OnPropertyChanged(nameof(Valor));
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

        public string Tipo
        {
            get { return _tipo; }
            set
            {
                if (value != _tipo)
                {
                    _tipo = value;
                    OnPropertyChanged(nameof(Tipo));
                }
            }
        }

        public string Formato
        {
            get { return _formato; }
            set
            {
                if (value != _formato)
                {
                    _formato = value;
                    OnPropertyChanged(nameof(Formato));
                }
            }
        }

        public string Icone
        {
            get { return _icone; }
            set
            {
                if (value != _icone)
                {
                    _icone = value;
                    OnPropertyChanged(nameof(Icone));
                }
            }
        }

        public string Cor
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
    }
}