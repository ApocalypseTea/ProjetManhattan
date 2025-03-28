using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using ProjetManhattan.Configuration;
using ProjetManhattan.Traitements;

namespace ProjetManhattan
{
    internal class TestsReflexivite
    {
        public BaseConfig[] parametreTraitement = { new BaseConfig(@"..\..\..\Ressources\config.json") }; 
        public TestsReflexivite()
        {           
        }
        
        public void GetTest()
        {
            string nomTraitement = "URL";

            Dictionary<string, ITraitement> allTreatmentInstances = new Dictionary<string, ITraitement>();

            var traitementsQuiImplemententItraitement = Assembly.GetExecutingAssembly().GetTypes().Where(l => l.IsClass && l.IsAssignableTo(typeof(ITraitement)));
            
            //Instancier tous les traitements
            foreach (Type traitementQuiImplementeITraitement in traitementsQuiImplemententItraitement)
            {
                ConstructorInfo[] constructeur = traitementQuiImplementeITraitement.GetConstructors();
                foreach (ConstructorInfo constructor in constructeur)
                {
                    //Recuperer le nom raccourci
                    ITraitement instanceDeTraitement = (ITraitement)constructor.Invoke(parametreTraitement);                    
                    string nomTraitementRaccourci = instanceDeTraitement.Name; 
                    //Ajouter au dictionnaire
                    allTreatmentInstances.Add(nomTraitementRaccourci, instanceDeTraitement);
                }

            //Selectionner uniquement le traitement qui correspond à la demande
            //Chercher parmi les instances du dictionnaire
                if (allTreatmentInstances.Keys.Contains(nomTraitement))
                {
                    ITraitement traitement = allTreatmentInstances[nomTraitement];
                    traitement.Execute();
                    //traitement.Display(typeOutput, nomBD);
                }
            }
        }
    }
}
