using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using ProjetManhattan.Filtres;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;

namespace ProjetManhattan.Traitements
{
    abstract class BaseTraitement<TSource> where TSource: ISource
    {
        protected TSource _source;
        protected IFiltre _filtre;
        protected IFormatage _sortie;

        public BaseTraitement(BaseConfig config)
        {
            //_source = new FichierDeLogIIS(config);
            _sortie = new OutputDisplay();         
        }

        public abstract void Execute();
        //{
        //    while (_source.HasLines())
        //    {
        //        LigneDeLog? ligne = _source.ReadLine();
        //        if (ligne != null && _filtre.Needed(ligne))
        //        {
        //            this.AddLine(ligne);
        //        }
        //    }
        //}
        protected abstract void AddLine(Record line); 

        public abstract void Display();

        public IFiltre Filtre
        {
            get { return _filtre; }
            set { _filtre = value; }
        }

    }
}
