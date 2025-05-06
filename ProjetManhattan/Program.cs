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
using Microsoft.Data.SqlClient;
using System.CommandLine.Builder;
using Unity;
using ProjetManhattan.Sources;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Annexes_Traitements;

namespace ProjetManhattan
{
    internal class Program
    {
        private static IUnityContainer _container = new UnityContainer();
        private static string fichierConfig;
        private static BaseConfig? importConfig = null;
        private static Option<string> configFileName;
        private static Regex dBFilenamePattern = new Regex(@".*\.db$", RegexOptions.Compiled);
        private const string RESSOURCEHELP = "ProjetManhattan.ReadMe - CommandesCLI.txt";
  
        
        public static void MiniMenu(string nomTraitement, BaseConfig importConfig, string typeOutput, string? nomBD)
        {
            ITraitement? traitement = null;
            object[] parametreTraitement = { importConfig };

            GenerationNomTraitement generationNomTraitement = _container.Resolve<GenerationNomTraitement>();
            bool isTraitementDone = false;
            foreach (var traitementInstance in generationNomTraitement.AllTreatments)
            {
                if (nomTraitement.ToLower() == "all" || traitementInstance.Key.Contains(nomTraitement.ToLower()))
                {
                    try
                    {
                        traitement = traitementInstance.Value;
                        traitement?.InitialisationConfig(importConfig);
                        traitement?.Execute();
                        traitement?.Display(typeOutput, nomBD);
                        isTraitementDone = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERREUR : {ex.Message}");
                        Console.WriteLine(ex.InnerException);
                        Console.WriteLine($"Traitement {traitementInstance.Key} non instancié. Ignoré.");
                    }
                }
            }

            if (!isTraitementDone)
            {
                Console.WriteLine($"Oops, le traitement {nomTraitement} ne fonctionne pas.");
                Console.WriteLine("Liste des traitements existants : ");
                foreach (var oneTraitement in generationNomTraitement.AllTreatments)
                {
                    Console.WriteLine($"- {oneTraitement.Key}");
                }
            }
        }
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Les Commandes Disponibles sur Projet Manhattan :");
                Assembly assembly = Assembly.GetExecutingAssembly();

                using (Stream stream = assembly.GetManifestResourceStream(RESSOURCEHELP))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }
            }
            else
            {
                RootCommand rootCommand = GetRootCommandProjetManhattan();
                Command effectuerTraitement = GetCommandTraitement(configFileName);
                Command exporterVersZabbix = GetCommandExportToZabbix();
                Command menuAide = GetHelp(configFileName);
                Command getValue = GetCommandGetValue();
                Command getTargetInfo = GetCommandTargetInfo(configFileName);

                rootCommand.Add(getValue);
                rootCommand.Add(effectuerTraitement);
                rootCommand.Add(exporterVersZabbix);
                rootCommand.Add(menuAide);
                rootCommand.Add(getTargetInfo);
                rootCommand.Invoke(args);
            }
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
                InitConfig(configFileNameValue, DateTime.Now);
            }, configFileName);

            return rootCommand;
        }

        private static void InitConfig(string configFileNameValue, DateTime DateTraitement)
        {

            fichierConfig = configFileNameValue;
            importConfig = new BaseConfig(fichierConfig);
            importConfig.DateTraitement = DateTraitement;
            _container.RegisterInstance<BaseConfig>(importConfig);

            IAccesBDD source = new AccesBDD(importConfig);
            IFormatage sortie = new OutputDisplay();

            _container.RegisterInstance<IAccesBDD>(source);
            _container.RegisterInstance<IFormatage>(sortie);
            _container.RegisterFactory<IFichierDeLog>((container) =>
            {
                return new FichierDeLogIIS(importConfig);
            });
            GenerationNomTraitement generationNomTraitement = new GenerationNomTraitement(_container, importConfig);
            _container.RegisterInstance<GenerationNomTraitement>(generationNomTraitement);
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

                

                if (Path.GetExtension(nomBDorigneValue) != ".db")
                {
                    nomBDorigneValue += ".db";
                }

                SQLiteToZabbix transfertVersZabbix;

                //Si les 2 paramètres --date ET --debutPeriode sont entrés OU aucun des 2
                if ((dateOnlyValue != default(DateTime) && dateDebutExportValue != default(DateTime)) || (dateOnlyValue == default(DateTime) && dateDebutExportValue == default(DateTime)))
                {
                    Console.WriteLine("Erreur : entrer une date OU une periode pour l'extraction des informations");
                    Console.WriteLine("--date : pour entrer une seule journée");
                    Console.WriteLine("--debutPeriode : pour entrer le debut de la periode");
                    Console.WriteLine("--finPeriode : pour entrer la fin de la periode (date par defaut = aujourd'hui)");
                    return;
                }
                //Option RANGE de DATES
                else if (dateDebutExportValue != default(DateTime))
                {
                    //Console.WriteLine($"date debut export entree : {dateDebutExportValue}");
                    
                    InitConfig(configFileNameValue, dateDebutExportValue);


                    if (dateFinExportValue == default(DateTime))
                    {
                        dateFinExportValue = DateTime.Now;
                    }

                    if (dateFinExportValue > DateTime.Now)
                    {
                        Console.WriteLine("Erreur : Faille Spatio Temporelle");
                        Console.WriteLine($"La date de fin {dateFinExportValue} est dans le futur");
                        return;
                    }
                    if (dateDebutExportValue > DateTime.Now)
                    {
                        Console.WriteLine("Erreur : Faille Spatio Temporelle");
                        Console.WriteLine($"La date {dateDebutExportValue} est dans le futur");
                        return;
                    }

                    if (dateDebutExportValue >= dateFinExportValue)
                    {
                        Console.WriteLine("Erreur : Faille Spatio Temporelle");
                        Console.WriteLine($"La date de début {dateDebutExportValue} est incompatible avec la date de fin {dateFinExportValue}");
                        return;
                    }
                    transfertVersZabbix = new SQLiteToZabbix(nomBDorigneValue, dateDebutExportValue, dateFinExportValue);
                }
                //Option date seule rentrée
                else if (dateOnlyValue != default(DateTime))
                {
                    if (dateOnlyValue > DateTime.Now)
                    {
                        Console.WriteLine("Erreur : Faille Spatio Temporelle");
                        Console.WriteLine($"La date demandée {dateOnlyValue} est dans le futur");
                        return;
                    }
                    //Console.WriteLine($"date only entree : {dateOnlyValue}");
                    importConfig.DateTraitement = dateOnlyValue;
                    InitConfig(configFileNameValue, dateOnlyValue);
                    transfertVersZabbix = new SQLiteToZabbix(nomBDorigneValue, DateOnly.FromDateTime(dateOnlyValue));
                }
                else
                {
                    Console.WriteLine("Erreur : entrer une date OU une periode pour l'extraction des informations");
                    Console.WriteLine("--date : pour entrer une seule journée");
                    Console.WriteLine("--debutPeriode : pour entrer le debut de la periode");
                    Console.WriteLine("--finPeriode : pour entrer la fin de la periode (date par defaut = aujourd'hui)");
                    return;
                }

                //Affichage du JSON en console
                Console.WriteLine(transfertVersZabbix.GetJSONToZabbix(traitementChoisiValue, _container));
            }, nomBDorigine, traitementChoisi, dateDebutExport, dateFinExport, configFileName, dateOnly);

            return exporterVersZabbix;
        }

        private static Command GetCommandGetValue()
        {
            Command getValue = new Command(name: "getValue", description: "recuperer la derniere valeur d'un ensemble Traitement-Target-PropertyName");
            Option<string> nomTraitement = new Option<string>(
                name: "--traitement",
                description: "nom du traitement dont la value est recherchee",
                getDefaultValue: () => "");
            getValue.AddOption(nomTraitement);

            Option<string> nomPropertyName = new Option<string>(
                name: "--propertyName",
                description: "nom de la PropertyName dont la value est recherchée",
                getDefaultValue:()=> "");
            getValue.AddOption(nomPropertyName);

            Option<string> nomTarget = new Option<string>(
                name: "--target",
                description: "nom de la Target dont la value est recherchée");
            nomTarget.IsRequired = true;
            getValue.AddOption(nomTarget);

            Option<DateTime> dateValue = new Option<DateTime>(
                name: "--date",
                description: "Date a laquelle la Value est a rechercher",
                getDefaultValue: () => DateTime.Now);
            getValue.AddOption(dateValue);

            Option<string> nomBDOrigine = new Option<string>(
                name: "--input",
                description: "Nom de la base de donnée dont il faut exporter les resultats");
            nomBDOrigine.IsRequired = true;
            getValue.AddOption(nomBDOrigine);

            getValue.SetHandler((nomTraitementValue, nomTargetValue, nomPropertyNameValue, nomBDOrigineValue, dateValueValue, configFileNameValue) =>
            {
                if (nomBDOrigineValue == null) 
                {
                    Console.WriteLine("Base de données non indiquée");
                    Console.WriteLine("Infos à entrer en CLI : toZabbix -input nomDeLaBaseDeDonnees GetValue --date dateDeRechercheDeLaValue --target nomTarget [--traitement nomTraitement OU --propertyName nomPropertyName]");
                    return;
                } 
                else
                {
                    if (Path.GetExtension(nomBDOrigineValue) != ".db")
                    {
                        nomBDOrigineValue += ".db";
                    }

                    SQLiteToZabbix transfertToZabbix = new SQLiteToZabbix(nomBDOrigineValue, DateOnly.FromDateTime(dateValueValue));
                    InitConfig(configFileNameValue, dateValueValue);
                    
                    importConfig.DateTraitement = dateValueValue;
                    //Console.WriteLine($"Base de Donnees consultee={nomBDOrigineValue}"); 

                    if(nomTraitementValue == null && nomPropertyNameValue == null)
                    {
                        Console.WriteLine("Traitement ou PropertyName non indiqué");
                        Console.WriteLine("Infos à entrer en CLI : toZabbix -input nomDeLaBaseDeDonnees GetValue --date dateDeRechercheDeLaValue --target nomTarget [--traitement nomTraitement OU --propertyName nomPropertyName]");
                        return;
                    }

                    Console.WriteLine(transfertToZabbix.GetValueFromTraitementTargetPropertyName(nomTraitementValue, nomTargetValue, nomPropertyNameValue, _container));
                }
            }, nomTraitement, nomTarget, nomPropertyName, nomBDOrigine, dateValue, configFileName);

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
                InitConfig(configFileNameValue, dateDebutTraitementsValue);

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

        private static Command GetHelp(Option<string> configFileName)
        {
            Command help = new Command(
                name: "helpMe",
                description : "Informations pour utiliser ProjetManhattanCLI"
            );

            Option<DateTime> date = new Option<DateTime>(
                name: "--date",
                description: "Liste de tous les traitements existants disponibles a la date demandee",
                getDefaultValue:()=> DateTime.Now
                //ajouter le jour même par default une fois en utilisation
                );
            date.IsRequired = true; ;
            help.AddOption(date);

            help.SetHandler((configFileNameValue, dateValue) =>
            {
                InitConfig(configFileNameValue, dateValue);              
                importConfig.DateTraitement = dateValue;

                GenerationNomTraitement generationNomTraitement = _container.Resolve<GenerationNomTraitement>();
                Console.WriteLine("Liste des traitements disponibles");
                foreach (string nomTraitement in generationNomTraitement.AllTreatments.Keys)
                {
                    Console.WriteLine($"- {nomTraitement}");
                }
            }, configFileName, date);

            return help;
        }

        private static Command GetCommandTargetInfo(Option<string> configFileName)
        {
            Command view = new Command(
                name: "views",
                description: "retourne les valeurs d'une vue qui sera définie par une requête SQL"
                );

            Option<string> getView = new Option<string>(
                name: "--view",
                description: "choix de la vue à utiliser");
            getView.IsRequired = true;
            view.AddOption(getView);

            Option<string> getTarget = new Option<string>(
                name:"--target",
                description: "cible de la recherche d'informations");
            getTarget.IsRequired = true;
            view.AddOption(getTarget);

            Option<DateTime> dateDebut = new Option<DateTime>(
                name: "--dateDebut",
                description: "date pour laquelle on souhaite aggréger les informations");
            view.AddOption(dateDebut);

            Option<DateTime> dateFin = new Option<DateTime>(
                name: "--dateFin",
                description: "date pour laquelle on souhaite finir d'aggréger les informations");
            view.AddOption(dateFin);

            Option<string> fromDatabase = new Option<string>(
                name: "--input",
                description: "base de données dans laquelle la requete sera effectuée");
            fromDatabase.IsRequired = true;
            view.AddOption(fromDatabase);


            view.SetHandler((configFileNameValue, fromDatabaseValue, getViewValue, getTargetValue, dateDebutValue, dateFinValue) =>
            {
                if (dateDebutValue == default(DateTime))
                {
                    Console.WriteLine("Erreur : Entrer une date pour demarrer l'agregation des informations");
                    Console.WriteLine("--dateDebut : pour entrer le debut de la période souhaitée");
                    Console.WriteLine("--dateFin : Pour entrer la fin de la période souhaitée.");
                    return;
                }

                if (dateDebutValue > DateTime.Now)
                {
                    Console.WriteLine("Erreur : Faille Spatio Temporelle");
                    Console.WriteLine($"La date de Debut {dateDebutValue} est dans le futur");
                    return;
                }

                if (dateFinValue != default(DateTime))
                {
                    if (dateDebutValue >= dateFinValue)
                    {
                        Console.WriteLine("Erreur : Faille Spatio Temporelle");
                        Console.WriteLine($"La date de début {dateDebutValue} est incompatible avec la date de fin {dateFinValue}");
                        return;
                    }

                    if (dateFinValue > DateTime.Now)
                    {
                        Console.WriteLine("Erreur : Faille Spatio Temporelle");
                        Console.WriteLine($"La date de Fin {dateFinValue} est dans le futur");
                        return;
                    }
                } 
                else
                {
                    dateFinValue = DateTime.Now;
                }

                if (Path.GetExtension(fromDatabaseValue) != ".db")
                {
                    fromDatabaseValue += ".db";
                    
                }

                //Retrait de la casse si existante
                getViewValue = getViewValue.ToLower();
                getTargetValue = getTargetValue.ToLower();

                BaseConfig config = new BaseConfig(configFileNameValue);

                IList<JToken> listeViews = config._jConfig["views"].ToList();
                IList<string> views = new List<string>();

                foreach (JProperty view in listeViews)
                {
                    string viewItem = view.Name;
                    views.Add(viewItem);
                }

                bool isViewExist = false;
                foreach (string viewName in views)
                {
                    if (viewName.Contains(getViewValue))
                    {
                        AnalyseTargetInfo traitementTarget = new AnalyseTargetInfo(config, getViewValue, getTargetValue, dateDebutValue, fromDatabaseValue, dateFinValue);

                        traitementTarget.Execute();
                        Console.WriteLine(traitementTarget.TargetInfoToJSON());
                        isViewExist = true;
                    }
                   
                }
                if (!isViewExist)
                {
                    Console.WriteLine($"La vue : {getViewValue} n'existe pas dans le fichier de configuration.");
                    Console.WriteLine("Liste des vues disponibles");
                    foreach (string viewName in views)
                    {
                        Console.WriteLine($" - {viewName}");
                    }
                }
            }, configFileName, fromDatabase, getView, getTarget, dateDebut, dateFin);

            return view;
        }
    }
}