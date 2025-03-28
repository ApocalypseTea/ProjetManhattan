using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ProjetManhattan.Configuration;
using ProjetManhattan.Traitements;
using Microsoft.Data.Sqlite;
using System.CommandLine;
using System.Threading.Tasks;
using ProjetManhattan.Formatages;
using System.Reflection;

namespace ProjetManhattan
{
    internal class Program
    {
        public static void miniMenu(string nomTraitement, BaseConfig importConfig, string typeOutput, string nomBD)
        {
            ITraitement? traitement = null;
            BaseConfig[] parametreTraitement = { importConfig };
            Action<ITraitement> executeTraitement = (traitement) =>
            {
                traitement?.Execute();
                traitement?.Display(typeOutput, nomBD);
            };
            Dictionary<string, ITraitement> allTreatmentInstances = new Dictionary<string, ITraitement>();

            var traitementsQuiImplemententItraitement = Assembly.GetExecutingAssembly().GetTypes().Where(l => l.IsClass && l.IsAssignableTo(typeof(ITraitement)));

            //Instancier tous les traitements
            foreach (Type traitementQuiImplementeITraitement in traitementsQuiImplemententItraitement)
            {
                ConstructorInfo[] constructeur = traitementQuiImplementeITraitement.GetConstructors();

                foreach (ConstructorInfo constructor in constructeur)
                {
                    ITraitement instanceDeTraitement = (ITraitement)constructor.Invoke(parametreTraitement);
                    string nomTraitementRaccourci = instanceDeTraitement.Name;
                    allTreatmentInstances.Add(nomTraitementRaccourci, instanceDeTraitement);
                }

                foreach(var traitementInstance in allTreatmentInstances)
                {
                    if (nomTraitement == "all" || traitementInstance.Key.Contains(nomTraitement))
                    {
                        traitement = traitementInstance.Value;
                        traitement?.Execute();
                        traitement?.Display(typeOutput, nomBD);
                    }
                }
            }            
        }
        static void Main(string[] args)
        {
           string fichierConfig;
           BaseConfig? importConfig=null;
           RootCommand rootCommand = new RootCommand("K-Projet Manhattan");
           Option<string> configFileName = new Option<string>(
                name: "--configFile", 
                description: "emplacement du nouveau fichier config JSON", 
                getDefaultValue:() => @"..\..\..\Ressources\config.json");
            configFileName.IsRequired = true;
            rootCommand.AddGlobalOption(configFileName);

            rootCommand.SetHandler((configFileNameValue) =>
            {
                fichierConfig = configFileNameValue;
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

            Option<DateTime> dateDebutTraitements = new Option<DateTime>(
                name:"--date",
                description:"date à laquelle effectuer le ou les traitements",
                getDefaultValue: () => DateTime.Now
                );
            effectuerTraitement.AddOption(dateDebutTraitements);

            effectuerTraitement.SetHandler((choixTraitementValue, choixOutputValue, nomBDresultValue, configFileNameValue, dateDebutTraitementsValue) =>
            {
                fichierConfig = configFileNameValue;
                importConfig = new BaseConfig(fichierConfig);
                importConfig.dateTraitement = dateDebutTraitementsValue;
                Console.WriteLine(importConfig.dateTraitement);
                    miniMenu(choixTraitementValue, importConfig, choixOutputValue, nomBDresultValue);
            }, choixTraitement, choixOutput, nomBDResult, configFileName, dateDebutTraitements);

            Command exporterVersZabbix = new Command("toZabbix", "exporter le resultat d'un traitement en bd");
                rootCommand.Add(exporterVersZabbix);
                Argument<string> nomBDorigine = new Argument<string>(
                    name: "nomBaseDonnee",
                    description: "Nom de la base de donnée dont il faut exporter les resultats",
                    getDefaultValue: () => "resultatTraitement.db");
                    exporterVersZabbix.AddArgument(nomBDorigine);

                Option<string> traitementChoisi = new Option<string>(
                    name: "--nomTraitement",
                    description: "Traitement à exporter en JSON");
                traitementChoisi.IsRequired = true;
            exporterVersZabbix.AddOption(traitementChoisi);

                Option<DateTime> dateDebutExport = new Option<DateTime>(
                        name:"--debutPeriode",
                        description:"Date de debut de periode de traitement a exporter"                
                    );
                dateDebutExport.IsRequired = true;
            exporterVersZabbix.AddOption(dateDebutExport);

            Option<DateTime> dateFinExport = new Option<DateTime>(
                        name: "--finPeriode",
                        description: "Date de fin de periode de traitement a exporter",
                        getDefaultValue : () => DateTime.Now
                    );
            dateFinExport.IsRequired = true;
            exporterVersZabbix.AddOption(dateFinExport);

            exporterVersZabbix.SetHandler((nomBDorigneValue, traitementChoisiValue, dateDebutExportValue, dateFinExportValue) => 
                {
                   SQLiteToZabbix transfertVersZabbix = new SQLiteToZabbix(nomBDorigneValue, dateDebutExportValue, dateFinExportValue);
                   Console.WriteLine(transfertVersZabbix.GetJSONToZabbix(traitementChoisiValue));
                }, nomBDorigine, traitementChoisi, dateDebutExport, dateFinExport);

            Command getValue = new Command(name: "getValue", description:"recuperer la derniere valeur d'un ensemble Traitement-Target-PropertyName");
                exporterVersZabbix.Add(getValue);
                Option<string> nomTraitement = new Option<string>(
                    name: "--nomTraitement",
                    description: "nom du traitement dont la value est recherchee");
                nomTraitement.IsRequired = true;
                getValue.AddOption(nomTraitement);
                Option<string> nomTarget = new Option<string>(
                    name: "--nomTarget",
                    description: "nom du Target dont la value est recherchée");
                nomTarget.IsRequired = true;
                getValue.AddOption(nomTarget);
                Option<string> nomPropertyName = new Option<string>(
                    name: "--nomPropertyName",
                    description: "nom de la PropertyName dont la value est recherchée");
                nomPropertyName.IsRequired = true;
                getValue.AddOption(nomPropertyName);

                Option<DateTime> dateValue = new Option<DateTime>(
                    name: "--date",
                    description: "Date a laquelle la Value est a rechercher",
                    getDefaultValue: () => DateTime.Now);
                getValue.AddOption(dateValue);

                getValue.SetHandler((nomTraitementValue, nomTargetValue, nomPropertyNameValue, nomBDOrigineValue, dateValueValue) =>
                {
                    SQLiteToZabbix transfertToZabbix = new SQLiteToZabbix(nomBDOrigineValue, dateValueValue, dateValueValue);
                    Console.WriteLine(transfertToZabbix.GetValueFromTraitementTargetPropertyName(nomTraitementValue, nomTargetValue, nomPropertyNameValue));

                }, nomTraitement, nomTarget, nomPropertyName, nomBDorigine, dateValue);
            rootCommand.Invoke(args);
        }
    }
}