using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    internal class AnalyseurTempsDeRequete
    {
        private List<TempsRequete> _infosTempsRequetes;
        public AnalyseurTempsDeRequete()
        {
            this._infosTempsRequetes = new List<TempsRequete>();
        }

        public void RecupererInfosTempsRequeteDuLog(string cheminFichier)
        {
            var lignes = File.ReadAllLines(cheminFichier);
            foreach (string ligne in lignes)
            {
                if (ligne[0] == '#')
                {
                    continue;
                }
                else
                {
                    string[] champs = ligne.Split(' ');
                    LigneDeLog ligneLog = new LigneDeLog(champs);
                    //Console.WriteLine(ligneLog);

                    ligneLog.AjouterInfosTempsDeRequetes(_infosTempsRequetes);
                }

            }            
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach( var ligne in _infosTempsRequetes)
            {
                sb.Append($"Requete de l'IP {ligne.ipClient.numeroIP} vers {ligne.url} a pris {ligne.timeTaken}ms dont {ligne.timeQuery}ms hors reseau");
            }

            return sb.ToString();
        }

        public void AnalyserListeTempsDeRequete(int seuilAlerte)
        {
            foreach (var requete in _infosTempsRequetes) 
            {
                if ((requete.timeTaken) >= seuilAlerte)
                {
                    Console.WriteLine($"La requete de l'IP {requete.ipClient.numeroIP} vers l'adresse {requete.url} a duré {requete.timeTaken} ms dont {requete.timeQuery} ms hors reseau");
                }                
            }
        }

    }
}
