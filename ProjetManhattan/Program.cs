using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ProjetManhattan.Configuration;
using ProjetManhattan.Traitements;

namespace ProjetManhattan
{
    internal class Program
    {
        public static void miniMenu(int numero, BaseConfig importConfig)
        {
            ITraitement? traitement = null;

            switch (numero)
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
                    traitement = new TraitementBrisDeGlace(importConfig);
                    break;
                case 5:
                    traitement = new TraitementValidationParInterneSQL(importConfig);
                    break;
            }

            traitement?.Execute();

            // if (traitement != null) { traitement.Execute(); }
            traitement?.Display();
        }
        static void Main(string[] args)
        {
            string fichierConfig = @"C:\Users\AdeLas\source\repos\ProjetManhattan\ProjetManhattan\Ressources\config.json";
            //string importJSONConfig = File.ReadAllText(fichierConfig);
            //BaseConfig importConfig = JsonConvert.DeserializeObject<BaseConfig>(importJSONConfig);
            BaseConfig importConfig = new BaseConfig(fichierConfig);


            //miniMenu(1, importConfig);
            //miniMenu(2, importConfig);
            //miniMenu(3, importConfig);
            //miniMenu(4, importConfig);
            miniMenu(5, importConfig);





        }
    }
}

    
