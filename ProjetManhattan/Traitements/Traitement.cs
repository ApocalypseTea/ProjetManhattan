using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Filtres;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;

namespace ProjetManhattan.Traitements
{
    abstract class Traitement
    {
        protected IFichierDeLog _source;
        protected IFiltre _filtre;
        protected IFormatage _sortie;

        public Traitement(Config config, IFiltre filtre)
        {
            _source = new FichierDeLogIIS(config);
            _sortie = new OutputDisplay();
            _filtre = filtre;
        }

        public void Execute()
        {
            while (_source.HasLines())
            {
                LigneDeLog? ligne = _source.ReadLine();
                if (ligne != null && _filtre.Needed(ligne))
                {
                    this.AddLine(ligne);
                }

            }
        }
        protected abstract void AddLine(LigneDeLog line);

        public abstract void Display();

    }
}
