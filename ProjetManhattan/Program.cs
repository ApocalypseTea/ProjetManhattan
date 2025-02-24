using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ProjetManhattan
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPAutorisees listeBlancheAdressesIPv4 = new IPAutorisees(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ProjetManhattan\\listeIPAutorisees.txt");

            //Test De serialisation de la liste des adresses IP autorisées
            string infosListeIPAutorisees = JsonConvert.SerializeObject(listeBlancheAdressesIPv4);
            Console.WriteLine(infosListeIPAutorisees);


            RecuperateurIPDuLog adressesIPDuJour = new RecuperateurIPDuLog(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ProjetManhattan\\u_ex250217.log", listeBlancheAdressesIPv4);
            
           adressesIPDuJour.TrierAdressesIPParConnexion();

            

        }
    }
}

    
