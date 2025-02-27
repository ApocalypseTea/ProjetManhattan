using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan.Sources
{
    internal class FichierDeLogIIS : IFichierDeLog
    {
        private string _cheminDeFichier;
        private string[] _lignes;
        private int _currentLine;
        public FichierDeLogIIS(Config config)
        {
            _cheminDeFichier = config.cheminFichierLog;
            _lignes = File.ReadAllLines(_cheminDeFichier);
            _currentLine = 0;
        }

        public bool HasLines()
        {
            return _currentLine < _lignes.Length;
        }

        public LigneDeLog? ReadLine()
        {
            string line = _lignes[_currentLine];
            _currentLine++;

            if (line[0] != '#')
            {
                string[] champs = line.Split(' ');
                LigneDeLog parsedLine = new LigneDeLog(champs);
                return parsedLine;
            }
            else
            {
                return null;
            }
        }
    }
}
