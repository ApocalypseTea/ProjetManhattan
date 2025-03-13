using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ProjetManhattan.Configuration;
using ProjetManhattan.Traitements;
using Microsoft.Data.Sqlite;

namespace ProjetManhattan
{
    internal class Program
    {
        public static void miniMenu(int numeroTraitement, BaseConfig importConfig, int typeOutput)
        {
            ITraitement? traitement = null;

            switch (numeroTraitement)
            {
                case 1:
                    traitement = new TraitementStatIP(importConfig);
                    break;
                case 2:
                    traitement = new TraitementTempsRequete(importConfig);
                    break;
                case 3:
                    traitement = new TraitementURL(importConfig);
                    break;
                case 4:
                    traitement = new TraitementBrisDeGlaceSQL(importConfig);
                    break;
                case 5:
                    traitement = new TraitementValidationParInterneSQL(importConfig);
                    break;
                case 6:
                    traitement = new TraitementChangementIdentiteUserSQL(importConfig);
                    break;
                case 7:
                    traitement = new TraitementValidateurRCPnonPresentSQL(importConfig);
                    break;
            }

            traitement?.Execute();

            // if (traitement != null) { traitement.Execute(); }
            
            //0 : Afficahge resultat sur Console
            //1 : Envoi resultats dans BD SQLite
            traitement?.Display(typeOutput);
        }
        static void Main(string[] args)
        {
            string fichierConfig = @"C:\Users\AdeLas\source\repos\ProjetManhattan\ProjetManhattan\Ressources\config.json";
            //string importJSONConfig = File.ReadAllText(fichierConfig);
            //BaseConfig importConfig = JsonConvert.DeserializeObject<BaseConfig>(importJSONConfig);
            BaseConfig importConfig = new BaseConfig(fichierConfig);

            //miniMenu(1, importConfig,1);
            //miniMenu(2, importConfig,1);
            //miniMenu(3, importConfig,1);
            //miniMenu(4, importConfig,1);
            //miniMenu(5, importConfig,1);
            //miniMenu(6, importConfig,1);
            //miniMenu(7, importConfig,1);

            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }





        }
    }
}

    
