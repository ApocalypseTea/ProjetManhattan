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
        public HashSet<string> adressesIPValides { get; init; }
        public DateTime dateDeCreationListe { get; init; }
            

        public IPAutorisees(HashSet<string> adressesIPValides)
        {
            HashSet<string> IPVerifiees = new HashSet<string>();
            foreach (var ligne in adressesIPValides)
            {
                string regexIPv4 = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$";

                if (String.IsNullOrEmpty(ligne))
                {
                    break;
                }

                if (Regex.IsMatch(ligne, regexIPv4))
                {
                    IPVerifiees.Add(ligne);
                }

            }
            this.adressesIPValides = IPVerifiees;
            this.dateDeCreationListe = DateTime.Now;            
        }

        public override string ToString()
        {
            StringBuilder infosIPAutorisees = new StringBuilder();
            infosIPAutorisees.AppendLine($"Adresses IP Autorisées :");
            foreach (string ligne in adressesIPValides)
            {
                infosIPAutorisees.AppendLine(ligne);
            }
            infosIPAutorisees.AppendLine($"Date de Création de la liste d'IP valides : {dateDeCreationListe}");
            return infosIPAutorisees.ToString();

        }

    }    
}
