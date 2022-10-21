using Model.DataAccessLayer.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGT.HelperClasses
{
    public class ObjetoSelecionavel : ObservableObject
    {
        private object _objeto;
        private bool _selecionado;

        public object Objeto
        {
            get { return _objeto; }
            set
            {
                if (value != _objeto)
                {
                    _objeto = value;
                    OnPropertyChanged(nameof(Objeto));
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
    }
}