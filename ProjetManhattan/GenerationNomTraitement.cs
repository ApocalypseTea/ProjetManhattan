using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Configuration;
using ProjetManhattan.Traitements;

namespace ProjetManhattan
{
    internal class GenerationNomTraitement
    {
        public Dictionary<string, ITraitement> AllTreatments { get; }
        private object[] _parametreTraitement;

        public GenerationNomTraitement(BaseConfig importConfig)
        {
            AllTreatments = new Dictionary<string, ITraitement>();
            object[] parametreTraitement = {importConfig};
            _parametreTraitement = parametreTraitement;

            //var traitementsQuiImplemententItraitement = Assembly.GetExecutingAssembly().GetTypes().Where(l => l.IsClass && l.IsAssignableTo(typeof(ITraitement)));
            //syntaxe Linq "officielle":

            var traitementsQuiImplemententItraitement = from l in Assembly.GetExecutingAssembly().GetTypes()
                                                        where l.IsClass && l.IsAssignableTo(typeof(ITraitement))
                                                        select l;
            //Instanciation de tous les traitements existants
            foreach (Type typeTraitement in traitementsQuiImplemententItraitement)
            {
                ConstructorInfo[] constructors = typeTraitement.GetConstructors();

                foreach (ConstructorInfo constructor in constructors)
                {
                    try
                    {
                        ITraitement instanceDeTraitement = (ITraitement)constructor.Invoke(_parametreTraitement);
                        string nomTraitementRaccourci = instanceDeTraitement.Name;
                        AllTreatments.Add(nomTraitementRaccourci.ToLower(), instanceDeTraitement);
                    }
                    catch (TargetInvocationException ex)
                    {
                        if (ex.InnerException.GetType() == typeof(TraitementExecutionException))
                        {
                            Console.WriteLine(ex.InnerException.Message);
                            Console.WriteLine(ex.InnerException.InnerException);
                            Console.WriteLine($"Traitement {typeTraitement.Name} non instancié. Ignoré.");
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }
    }
}
