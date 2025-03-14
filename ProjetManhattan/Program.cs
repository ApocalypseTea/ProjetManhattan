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

            var rootCommand = new RootCommand("K-Projet Manhattan");

            Command effectuerTraitement = new Command("ttt", "Effectuer un traitement");            
            rootCommand.Add(effectuerTraitement);

                Option<int> choixTraitement = new Option<int>(name: "--numeroTraitement", description: "numero du traitement choisi");
                choixTraitement.IsRequired = true;
                effectuerTraitement.AddOption(choixTraitement);

                Option<int> choixOutput = new Option<int>(name: "--numeroOutput", description: "numero du type d'export des données traitées");
                effectuerTraitement.AddOption(choixOutput);

                //Faire une option pour tous les traitements à la fois ?

                effectuerTraitement.SetHandler((choixTraitementValue, choixOutputValue) =>
                {
                    miniMenu(choixTraitementValue, importConfig, choixOutputValue);
                }, choixTraitement, choixOutput);

            Command importerFichierConfig = new Command("config", "importer un fichier JSON de configuration");
            rootCommand.Add(importerFichierConfig);

                //Type <FileInfo> ?
                Option<string> configFileName = new Option<string>(name: "--configFile", description: "emplacement du fichier config JSON");
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

    
