using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ProjetManhattan.Traitements;

namespace ProjetManhattan
{
    internal class Program
    {
        public static void miniMenu(int numero, Config importConfig)
        {
            switch (numero)
            {
                case 1:
                    
                    ITraitement traitement1 = new TraitementStatIP(importConfig);
                    traitement1.Execute();
                    traitement1.Display();

                    break;
                case 2:

                    ITraitement traitement2 = new TraitementTempsRequete(importConfig);
                    traitement2.Execute();
                    traitement2.Display();
                    break;


            }
        }
        static void Main(string[] args)
        {
            string fichierConfig = @"C:\Users\AdeLas\source\repos\ProjetManhattan\ProjetManhattan\Ressources\config.json";
            string importJSONConfig = File.ReadAllText(fichierConfig);
            Config importConfig = JsonConvert.DeserializeObject<Config>(importJSONConfig);

            //miniMenu(1, importConfig);
            miniMenu(2, importConfig);

            
            

        }
    }
}

    
