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
            string fichierConfig = @"C:\Users\AdeLas\source\repos\ProjetManhattan\ProjetManhattan\Ressources\config.json";
            //string importJSONConfig = File.ReadAllText(fichierConfig);
            //BaseConfig importConfig = JsonConvert.DeserializeObject<BaseConfig>(importJSONConfig);
            BaseConfig importConfig = new BaseConfig(fichierConfig);

            var rootCommand = new RootCommand("K-Projet Manhattan");

            Command effectuerTraitement = new Command("ttt", "Effectuer un traitement");            
            rootCommand.Add(effectuerTraitement);

                Option<string> choixTraitement = new Option<string>(name: "--nomTraitement", description: "nom du traitement choisi");
                choixTraitement.IsRequired = true;
                effectuerTraitement.AddOption(choixTraitement);

                Option<string> choixOutput = new Option<string>(name: "--nomOutput", description: "nom du type d'export des données traitées", getDefaultValue: () => "bd");
                effectuerTraitement.AddOption(choixOutput);

                    Argument<string> nomBDResult = new Argument<string>(name: "nomBaseDonnee", description: "Nom de la base de donnée qui recevra les resultats du traitement", getDefaultValue: () => "resultatTraitement");
                    effectuerTraitement.AddArgument(nomBDResult);

                //Faire une option pour tous les traitements à la fois ?

                effectuerTraitement.SetHandler((choixTraitementValue, choixOutputValue, nomBDresultValue) =>
                {
                    miniMenu(choixTraitementValue, importConfig, choixOutputValue, nomBDresultValue);
                }, choixTraitement, choixOutput, nomBDResult);

            Command importerFichierConfig = new Command("config", "importer un fichier JSON de configuration");
            rootCommand.Add(importerFichierConfig);

                //Type <FileInfo> ?
                Option<string> configFileName = new Option<string>(name: "--configFile", description: "emplacement du nouveau fichier config JSON");
                importerFichierConfig.AddOption(configFileName);

                importerFichierConfig.SetHandler((configFileNameValue) => 
                {
                    //Verifications a faire avant ??
                    fichierConfig = configFileNameValue;
                }, configFileName);

            rootCommand.Invoke(args);
        }
    }
}

    
