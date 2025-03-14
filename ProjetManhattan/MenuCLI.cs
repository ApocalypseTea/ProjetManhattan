using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using ProjetManhattan.Traitements;

namespace ProjetManhattan
{
    class MenuCLI
    {
        private string _fichierConfig = @"C:\Users\AdeLas\source\repos\ProjetManhattan\ProjetManhattan\Ressources\config.json";

        public MenuCLI(string fichierConfig) 
        {
            this._fichierConfig = fichierConfig;
            BaseConfig importConfig = new BaseConfig(this._fichierConfig);
        }

        public void CommandLineMenu(RootCommand command, string [] args)
        {
            var rootCommand = command;

            Command effectuerTraitement = new Command("ttt", "Effectuer un traitement");
            rootCommand.Add(effectuerTraitement);

            Option<string> choixTraitement = new Option<string>(name: "--nomTraitement", description: "nom du traitement choisi");
            choixTraitement.IsRequired = true;
            effectuerTraitement.AddOption(choixTraitement);

            Option<string> choixOutput = new Option<string>(name: "--nomOutput", description: "nom du type d'export des données traitées", getDefaultValue: () => "bd");
            effectuerTraitement.AddOption(choixOutput);

            Argument<string> nomBDResult = new Argument<string>(name: "nomBaseDonnee", description: "Nom de la base de donnée qui recevra les resultats du traitement", getDefaultValue: () => "resultatTraitement");
            effectuerTraitement.AddArgument(nomBDResult);

            //Faire une option pour tous les traitements à la fois 
            effectuerTraitement.SetHandler((choixTraitementValue, choixOutputValue, nomBDresultValue) =>
            {
                if (choixTraitementValue.Equals("all"))
                {
                    foreach (var traitement in Enum.GetValues(typeof(NomsTraitements)))
                    {
                        miniMenu(traitement.ToString(), importConfig, choixOutputValue, nomBDresultValue);
                    }
                }
                else
                {
                    miniMenu(choixTraitementValue, importConfig, choixOutputValue, nomBDresultValue);
                }
            }, choixTraitement, choixOutput, nomBDResult);

            Command importerFichierConfig = new Command("config", "importer un fichier JSON de configuration");
            rootCommand.Add(importerFichierConfig);

            //Type <FileInfo> ?
            Option<string> configFileName = new Option<string>(name: "--configFile", description: "emplacement du nouveau fichier config JSON");
            importerFichierConfig.AddOption(configFileName);

            importerFichierConfig.SetHandler((configFileNameValue) =>
            {
                fichierConfig = configFileNameValue;
            }, configFileName);

            rootCommand.Invoke(args);

        }

    }
}
