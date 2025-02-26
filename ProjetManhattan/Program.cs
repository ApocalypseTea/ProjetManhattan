using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ProjetManhattan
{
    internal class Program
    {
        public static void miniMenu(int numero, Config importConfig)
        {
            switch (numero)
            {
                case 1:
                    //Recuperateur d'adresses IP par nombre de requetes
                    //Import Liste Blanche
                    IPAutorisees listeBlancheAdressesIPv4 = new IPAutorisees(importConfig.adressesIPValides);

                    RecuperateurIPDuLog adressesIPDuJour = new RecuperateurIPDuLog(importConfig.cheminFichierLog, listeBlancheAdressesIPv4);

                    adressesIPDuJour.TrierAdressesIPParConnexion(importConfig.seuilAlerteRequetesParIp);
                    break;
                case 2:
                    //Analyse de longueur de requete
                    AnalyseurTempsDeRequete infosTempsRequetesLog = new AnalyseurTempsDeRequete();
                    infosTempsRequetesLog.RecupererInfosTempsRequeteDuLog(importConfig.cheminFichierLog);
                    //infosTempsRequetesLog.AnalyserListeTempsDeRequete(1);
                    Console.WriteLine(infosTempsRequetesLog);
                    break;

            }
        }


        static void Main(string[] args)
        {
            string fichierConfig = @"C:\Users\AdeLas\source\repos\ProjetManhattan\ProjetManhattan\Ressources\config.json";
            string importJSONConfig = File.ReadAllText(fichierConfig);
            Config importConfig = JsonConvert.DeserializeObject<Config>(importJSONConfig);


            //miniMenu(2, importConfig);

            //Analyse de longueur de requete
            AnalyseurTempsDeRequete infosTempsRequetesLog = new AnalyseurTempsDeRequete();
            infosTempsRequetesLog.RecupererInfosTempsRequeteDuLog(importConfig.cheminFichierLog);
            infosTempsRequetesLog.AnalyserListeTempsDeRequete(2000);
            

        }
    }
}

    
