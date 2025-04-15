using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using ProjetManhattan.Traitements;

namespace TDD.ProjetManhattan.Traitements.DureeMoyenneRequeteSQLTests
{
    [TestClass]
    public class DureeMoyenneRequeteSQLTests
    {
        private static readonly string FILENAME = "C:\\Users\\Adelas\\source\\repos\\ApocalypseTea\\ProjetManhattan\\ProjetManhattan\\Ressources\\config.json";

        [TestMethod]
        public void CanCreateInstance()
        {
            BaseConfig config = new BaseConfig(FILENAME);
            ITraitement traitement = new TraitementDureeMoyenneRequeteSQL(config, new List<object>());

            Assert.IsNotNull(traitement);
        }

        [TestMethod]
        public void ShouldHaveAName()
        {
            BaseConfig config = new BaseConfig(FILENAME);
            ITraitement traitement = new TraitementDureeMoyenneRequeteSQL(config, new List<object>());
            Assert.AreEqual("DureeTraitementRequeteSQL", traitement.Name);
        }

        [TestMethod]
        public void ShouldExecute()
        {
            BaseConfig config = new BaseConfig(FILENAME);
            ITraitement traitement = new TraitementDureeMoyenneRequeteSQL(config, new List<object>());
            traitement.Execute();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ShouldExecuteAndReturnOneResult()
        {
            BaseConfig config = new BaseConfig(FILENAME);
            TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(config, new List<object>() { new object() });
            traitement.Execute();
            Assert.IsTrue(traitement.Items.Count == 1);
        }

        [TestMethod]
        public void ShouldExecuteAndReturnTwoResults()
        {
            BaseConfig config = new BaseConfig(FILENAME);
            TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(
                config,
                new List<object>() { 
                    new object(), 
                    new object() 
                }
            );
            traitement.Execute();
            Assert.IsTrue(traitement.Items.Count == 2);
        }

        [TestMethod]
        public void ExecuteShouldInterrogateDatabase()
        {
            BaseConfig config = new BaseConfig(FILENAME);
            TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(
                config, new List<object>()
            );
            traitement.Execute();
            traitement.AccesBDD.
        }

        [TestMethod]
        public void ExecuteShouldInterrogateSpecificTable()
        {

        }

        [TestMethod]
        public void ResultShouldHaveDuree()
        {

        }

        [TestMethod]
        public void ResultSHouldHaveStoredProcedureName()
        {

        }

        

    }
}
