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
            string fichierConfig = @"C:\Users\AdeLas\source\repos\ProjetManhattan\ProjetManhattan\Ressources\config.json";
            string importJSONConfig = File.ReadAllText(fichierConfig);
            Config importConfig = JsonConvert.DeserializeObject<Config>(importJSONConfig);

            IPAutorisees listeBlancheAdressesIPv4 = new IPAutorisees(importConfig.adressesIPValides);

            RecuperateurIPDuLog adressesIPDuJour = new RecuperateurIPDuLog(importConfig.cheminFichierLog, listeBlancheAdressesIPv4);
            
            adressesIPDuJour.TrierAdressesIPParConnexion(importConfig.seuilAlerteRequetesParIp);
            
        }
    }
}

    
