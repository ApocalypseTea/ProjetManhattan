using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    internal class IPAutorisees
    {
        public string cheminDeFichier { get; init; }
        public string nomDeFichier { get; init; }
        public HashSet<string> adressesIPValides { get; init; }
        public DateTime dateDeCreationListe { get; init; }


        public IPAutorisees(string cheminDeFichier, string nomDeFichier) 
        {
            this.cheminDeFichier = cheminDeFichier;
            this.nomDeFichier = nomDeFichier;
            this.dateDeCreationListe = DateTime.Now; //A voir si utile de mettre une duree de validite de la liste
            this.adressesIPValides = new HashSet<string>();
            string cheminFichierFinal = Path.Combine(cheminDeFichier, nomDeFichier);
            
            var ligneListeBlanche = File.ReadAllLines(cheminFichierFinal);
            foreach (var ligne in ligneListeBlanche)
            {
                string regexIPv4 = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$";

                if (String.IsNullOrEmpty(ligne))
                {
                    break;
                }

                if (Regex.IsMatch(ligne, regexIPv4))
                {
                    adressesIPValides.Add(ligne);
                }
            }
        }


    }



    
}
