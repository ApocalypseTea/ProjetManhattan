using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ProjetManhattan.Configuration;
using ProjetManhattan.Traitements;
using Microsoft.Data.Sqlite;
using System.CommandLine;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    internal class Program
    {
        public static void miniMenu(string nomTraitement, BaseConfig importConfig, string typeOutput, string nomBD)
        {
            ITraitement? traitement = null;

            switch (nomTraitement)
            {
                case "StatIp":
                    traitement = new TraitementStatIP(importConfig);
                    break;
                case "TempsRequete":
                    traitement = new TraitementTempsRequete(importConfig);
                    break;
                case "URL":
                    traitement = new TraitementURL(importConfig);
                    break;
                case "BrisGlace":
                    traitement = new TraitementBrisDeGlaceSQL(importConfig);
                    break;
                case "ValidationInterne":
                    traitement = new TraitementValidationParInterneSQL(importConfig);
                    break;
                case "ChangementIdentite":
                    traitement = new TraitementChangementIdentiteUserSQL(importConfig);
                    break;
                case "ValidateurAbsent":
                    traitement = new TraitementValidateurRCPnonPresentSQL(importConfig);
                    break;
            }

            traitement?.Execute();
            // if (traitement != null) { traitement.Execute(); }
            
            traitement?.Display(typeOutput, nomBD);
        }
        static void Main(string[] args)
        {
           string fichierConfig;
            //string fichierConfig = @"C:\Users\Adelas\source\repos\ApocalypseTea\ProjetManhattan\ProjetManhattan\Ressources\config.json";
           BaseConfig? importConfig=null;

           RootCommand rootCommand = new RootCommand("K-Projet Manhattan");
            Option<string> configFileName = new Option<string>(
                name: "--configFile", 
                description: "emplacement du nouveau fichier config JSON", 
                getDefaultValue:() => @"C:\Users\Adelas\source\repos\ApocalypseTea\ProjetManhattan\ProjetManhattan\Ressources\config.json");
            configFileName.IsRequired = true;
            rootCommand.AddGlobalOption(configFileName);

            rootCommand.SetHandler((configFileNameValue) =>
            {
                fichierConfig = configFileNameValue;
                //Console.WriteLine($"Fichier config utilisé : {fichierConfig}");
                importConfig = new BaseConfig(fichierConfig);
            }, configFileName);

            Command effectuerTraitement = new Command("ttt", "Effectuer un traitement");
            rootCommand.Add(effectuerTraitement);

            Option<string> choixTraitement = new Option<string>(
                name: "--nomTraitement", 
                description: "nom du traitement choisi");
            choixTraitement.IsRequired = true;
            effectuerTraitement.AddOption(choixTraitement);

            Option<string> choixOutput = new Option<string>(
                name: "--nomOutput", 
                description: "nom du type d'export des données traitées", 
                getDefaultValue: () => "bd");
            effectuerTraitement.AddOption(choixOutput);

            Argument<string> nomBDResult = new Argument<string>(
                name: "nomBaseDonnee", 
                description: "Nom de la base de donnée qui recevra les resultats du traitement",
                getDefaultValue: () => "resultatTraitement");
            effectuerTraitement.AddArgument(nomBDResult);

            //Option pour tous les traitements à la fois 
            effectuerTraitement.SetHandler((choixTraitementValue, choixOutputValue, nomBDresultValue, configFileNameValue) =>
            {
                fichierConfig = configFileNameValue;
                importConfig = new BaseConfig(fichierConfig);
                if (choixTraitementValue.Equals("all"))
                {
                    foreach (var traitement in Enum.GetValues(typeof(NomsTraitements)))
                    { 
                        miniMenu(traitement.ToString(), importConfig!, choixOutputValue, nomBDresultValue);
                    }
                }
                else
                {
                    miniMenu(choixTraitementValue, importConfig, choixOutputValue, nomBDresultValue);
                }
            }, choixTraitement, choixOutput, nomBDResult, configFileName);

            //Command importerFichierConfig = new Command("config", "importer un fichier JSON de configuration");
            //rootCommand.Add(importerFichierConfig);
                 

            rootCommand.Invoke(args);
        }
    }
}

    
