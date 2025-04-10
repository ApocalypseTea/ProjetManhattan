﻿using System;
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
using Microsoft.Data.SqlClient;

namespace ProjetManhattan
{
    internal class Program
    {
        private static string fichierConfig;
        private static BaseConfig? importConfig = null;
        private static Option<string> configFileName;
        private static Regex dBFilenamePattern = new Regex(@".*\.db$", RegexOptions.Compiled);
        public static void MiniMenu(string nomTraitement, BaseConfig importConfig, string typeOutput, string? nomBD)
        {
            ITraitement? traitement = null;
            object[] parametreTraitement = { importConfig };
            
            Action<ITraitement> executeTraitement = (traitement) =>
            {
                traitement?.Execute();
                traitement?.Display(typeOutput, nomBD);
            };
            /*
            //Dictionary<string, ITraitement> allTreatments = new Dictionary<string, ITraitement>();

            //var traitementsQuiImplemententItraitement = from l in Assembly.GetExecutingAssembly().GetTypes()
            //                                            where l.IsClass && l.IsAssignableTo(typeof(ITraitement))
            //                                            select l;

            ////Instancier tous les traitements
            //foreach (Type typeTraitement in traitementsQuiImplemententItraitement)
            //{
            //    ConstructorInfo[] constructors = typeTraitement.GetConstructors();

            //    foreach (ConstructorInfo constructor in constructors)
            //    {
            //        try
            //        {
            //            ITraitement instanceDeTraitement = (ITraitement)constructor.Invoke(parametreTraitement);
            //            string nomTraitementRaccourci = instanceDeTraitement.Name;
            //            allTreatments.Add(nomTraitementRaccourci.ToLower(), instanceDeTraitement);
            //        }
            //        catch (TargetInvocationException ex)
            //        {
            //            if (ex.InnerException.GetType() == typeof(TraitementExecutionException))
            //            {
            //                Console.WriteLine(ex.InnerException.Message);
            //                Console.WriteLine(ex.InnerException.InnerException);
            //                Console.WriteLine($"Traitement {typeTraitement.Name} non instancié. Ignoré.");
            //            }
            //            else
            //            {
            //                throw;
            //            }
            //        }
            //    }              
            //}
            */

            GenerationNomTraitement generationNomTraitement = new GenerationNomTraitement(importConfig);
            bool isTraitementDone = false;
            foreach (var traitementInstance in generationNomTraitement.AllTreatments)
            {
                if (nomTraitement.ToLower() == "all" || traitementInstance.Key.Contains(nomTraitement.ToLower()))
                {
                    try
                    {
                        traitement = traitementInstance.Value;
                        traitement?.Execute();
                        traitement?.Display(typeOutput, nomBD);
                        isTraitementDone = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.InnerException);
                        Console.WriteLine($"Traitement {traitementInstance.Key} non instancié. Ignoré.");
                    }
                } 
            }

            if (!isTraitementDone)
            {
                Console.WriteLine($"Oops, le traitement {nomTraitement} n'existe pas.");
                Console.WriteLine("Par contre, vous pouvez choisir parmi les traitements : ");
                foreach (var oneTraitement in generationNomTraitement.AllTreatments)
                {
                    Console.WriteLine($"- {oneTraitement.Key}");
                }
            }
        }
        static void Main(string[] args)
        {
            RootCommand rootCommand = GetRootCommandProjetManhattan();
            Command effectuerTraitement = GetCommandTraitement(configFileName);
            Command exporterVersZabbix = GetCommandExportToZabbix();

            rootCommand.Add(effectuerTraitement);
            rootCommand.Add(exporterVersZabbix);
            rootCommand.Invoke(args);
        }

        private static RootCommand GetRootCommandProjetManhattan()
        {
            RootCommand rootCommand = new RootCommand("K-Projet Manhattan");
            configFileName = new Option<string>(
                 name: "--configFile",
                 description: "emplacement du nouveau fichier config JSON",
                 getDefaultValue: () => @"..\..\..\Ressources\config.json");
            configFileName.IsRequired = true;
            rootCommand.AddGlobalOption(configFileName);

            rootCommand.SetHandler((configFileNameValue) =>
            {
                fichierConfig = configFileNameValue;
                importConfig = new BaseConfig(fichierConfig);
            }, configFileName);

            return rootCommand;
        }

        private static Command GetCommandExportToZabbix()
        {
            Command exporterVersZabbix = new Command("toZabbix", "exporter le resultat d'un traitement en bd");
            Option<string> nomBDorigine = new Option<string>(
                name: "--input",
                description: "Nom de la base de donnée dont il faut exporter les resultats");
                nomBDorigine.IsRequired = true;
            exporterVersZabbix.AddOption(nomBDorigine);

            Option<string> traitementChoisi = new Option<string>(
                name: "--traitement",
                description: "Traitement à exporter en JSON");
            traitementChoisi.IsRequired = true;
            exporterVersZabbix.AddOption(traitementChoisi);

            Option<DateTime> dateDebutExport = new Option<DateTime>(
                name: "--debutPeriode",
                description: "Date de debut de periode de traitement a exporter"
                );
            exporterVersZabbix.AddOption(dateDebutExport);

            Option<DateTime> dateFinExport = new Option<DateTime>(
                name: "--finPeriode",
                description: "Date de fin de periode de traitement a exporter"
                );
            exporterVersZabbix.AddOption(dateFinExport);

            Option<DateTime> dateOnly = new Option<DateTime>(
                name: "--date",
                description: "Date exacte du traitement à exporter"
                );
            exporterVersZabbix.AddOption(dateOnly);

            exporterVersZabbix.SetHandler((nomBDorigneValue, traitementChoisiValue, dateDebutExportValue, dateFinExportValue, configFileNameValue, dateOnlyValue) =>
            {
                fichierConfig = configFileNameValue;
                importConfig = new BaseConfig(fichierConfig);
                
                if (Path.GetExtension(nomBDorigneValue) != ".db")                
                {
                    nomBDorigneValue += ".db";
                }
               
                SQLiteToZabbix transfertVersZabbix;
                if (dateDebutExportValue != default(DateTime))
                {
                    Console.WriteLine($"date debut export entree : {dateDebutExportValue}");
                    importConfig.DateTraitement = dateDebutExportValue;
                    if (dateFinExportValue == default(DateTime))
                    {
                        Console.WriteLine("Option Date de fin non remplie. La periode s'etend donc jusqu'a aujourd'hui");
                        dateFinExportValue = DateTime.Now;
                    }
                    if (dateDebutExportValue >= dateFinExportValue)
                    {
                        Console.WriteLine("Erreur : Faille Spatio Temporelle");
                        Console.WriteLine($"La date de début {dateDebutExportValue} est incompatible avec la date de fin {dateFinExportValue}");
                        return;
                    }


                        transfertVersZabbix = new SQLiteToZabbix(nomBDorigneValue, dateDebutExportValue, dateFinExportValue);
                } 
                else if (dateOnlyValue != default(DateTime))
                {
                    //Console.WriteLine($"date only entree : {dateOnlyValue}");
                    importConfig.DateTraitement = dateOnlyValue;
                    transfertVersZabbix = new SQLiteToZabbix(nomBDorigneValue, dateOnlyValue);
                } 
                //SI les 2 paramètres --date ET --debutPeriode sont entrés OU aucun des 2
                else if((dateOnlyValue != default(DateTime) && dateDebutExportValue !=default(DateTime)) || (dateOnlyValue == default(DateTime) && dateDebutExportValue == default(DateTime))) {
                {
                    Console.WriteLine("Erreur : entrer une date OU une periode pour l'extraction des informations");
                    Console.WriteLine("--date : pour entrer une seule journée");
                    Console.WriteLine("--debutPeriode : pour entrer le debut de la periode");
                    Console.WriteLine("--finPeriode : pour entrer la fin de la periode (date par defaut = aujourd'hui)");
                    return;
                }

                    //Affichage du JSON en console
                    Console.WriteLine(transfertVersZabbix.GetJSONToZabbix(traitementChoisiValue, importConfig));
            }, nomBDorigine, traitementChoisi, dateDebutExport, dateFinExport, configFileName, dateOnly);

            Command getValue = GetSubCommandGetValue(nomBDorigine);

            exporterVersZabbix.Add(getValue);

            return exporterVersZabbix;
        }

        private static Command GetSubCommandGetValue(Option<string> nomBDorigine)
        {
            Command getValue = new Command(name: "getValue", description: "recuperer la derniere valeur d'un ensemble Traitement-Target-PropertyName");
            Option<string> nomTraitement = new Option<string>(
                name: "--traitement",
                description: "nom du traitement dont la value est recherchee");
            nomTraitement.IsRequired = true;
            getValue.AddOption(nomTraitement);
            Option<string> nomTarget = new Option<string>(
                name: "--target",
                description: "nom de la Target dont la value est recherchée");
            nomTarget.IsRequired = true;
            getValue.AddOption(nomTarget);
            Option<string> nomPropertyName = new Option<string>(
                name: "--propertyName",
                description: "nom de la PropertyName dont la value est recherchée");
            nomPropertyName.IsRequired = true;
            getValue.AddOption(nomPropertyName);
            Option<DateTime> dateValue = new Option<DateTime>(
                name: "--date",
                description: "Date a laquelle la Value est a rechercher",
                getDefaultValue: () => DateTime.Now);
            getValue.AddOption(dateValue);

            getValue.SetHandler((nomTraitementValue, nomTargetValue, nomPropertyNameValue, nomBDOrigineValue, dateValueValue, configFileNameValue) =>
            {
                SQLiteToZabbix transfertToZabbix = new SQLiteToZabbix(nomBDOrigineValue, dateValueValue, dateValueValue);
                fichierConfig = configFileNameValue;
                importConfig = new BaseConfig(fichierConfig);
                importConfig.DateTraitement = dateValueValue;

                Console.WriteLine(transfertToZabbix.GetValueFromTraitementTargetPropertyName(nomTraitementValue, nomTargetValue, nomPropertyNameValue, importConfig));

            }, nomTraitement, nomTarget, nomPropertyName, nomBDorigine, dateValue, configFileName);

            return getValue;
        }

        private static Command GetCommandTraitement(Option<string> configFileName)
        {
            Command effectuerTraitement = new Command("traitement", "Effectuer un traitement");
            
            Option<string> choixTraitement = new Option<string>(
                name: "--traitement",
                description: "nom du traitement choisi");
            choixTraitement.IsRequired = true;
            effectuerTraitement.AddOption(choixTraitement);

            Option<string> choixOutput = new Option<string>(
                name: "--outputFormat",
                description: "nom du type d'export des données traitées"
                ).FromAmong("bd", "console");
            choixOutput.IsRequired = true;
            effectuerTraitement.AddOption(choixOutput);
            
            //Trouver une solution pour créer une conditionnelle SEULEMENT si le type de traitement est "bd"
            Option<string> nomBDResult = new Option<string>(
                name: "--output",
                description: "Nom de la base de donnée qui recevra les resultats du traitement"
                );            
            effectuerTraitement.AddOption(nomBDResult);
            
            Option<DateTime> dateDebutTraitements = new Option<DateTime>(
                name: "--date",
                description: "date à laquelle effectuer le ou les traitements",
                getDefaultValue: () => DateTime.Now
                );
            effectuerTraitement.AddOption(dateDebutTraitements);

            effectuerTraitement.SetHandler((choixTraitementValue, choixOutputValue, nomBDresultValue, configFileNameValue, dateDebutTraitementsValue) =>
            {
                fichierConfig = configFileNameValue;
                importConfig = new BaseConfig(fichierConfig);
                importConfig.DateTraitement = dateDebutTraitementsValue;
                //Console.WriteLine(importConfig.DateTraitement);
                if (choixOutputValue.Equals("bd"))
                {
                    if (nomBDresultValue == null)
                    {
                        Console.WriteLine("Erreur : le nom de la base de donnée n'est pas renseigné.");
                        Console.WriteLine("Veuillez le renseigner avec l'option --output.");
                        return;
                    }

                    if (!dBFilenamePattern.IsMatch(nomBDresultValue))
                    {
                        nomBDresultValue += ".db";
                    }

                }
                else
                {
                    nomBDresultValue = null;
                }

                MiniMenu(choixTraitementValue, importConfig, choixOutputValue, nomBDresultValue);
            }, choixTraitement, choixOutput, nomBDResult, configFileName, dateDebutTraitements);

            return effectuerTraitement;
        }
    }
}