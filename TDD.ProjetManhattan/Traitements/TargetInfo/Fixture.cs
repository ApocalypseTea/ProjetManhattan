using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ProjetManhattan.Configuration;
using ProjetManhattan.Traitements;

namespace TDD.ProjetManhattan.Traitements.TargetInfo
{
    internal class Fixture
    {
        public TraitementTargetInfo? _traitement;
        private static readonly string FILENAME = "C:\\Users\\Adelas\\source\\repos\\ApocalypseTea\\ProjetManhattan\\ProjetManhattan\\Ressources\\config.json";

        private BaseConfig _fakeConfig;
        private string _fakeView;
        private string _fakeTarget;
        private DateTime _fakeDateDebut;
        private DateTime _fakeDateFin;


        public Fixture() {
           _fakeConfig = new BaseConfig(FILENAME);
        }

        public TraitementTargetInfo WhenStartingTraitementTargetInfo()
        {
            _traitement = new TraitementTargetInfo(this._fakeConfig, this._fakeView, this._fakeTarget, this._fakeDateDebut, this._fakeDateFin);
            return _traitement;
        }

        public void GivingTraitementParameters(string fakeView, string fakeTarget, DateTime fakeDateDebut, DateTime fakeDateFin)
        {
            this._fakeView = fakeView;
            this._fakeTarget = fakeTarget;
            this._fakeDateDebut = fakeDateDebut;
            this._fakeDateFin = fakeDateFin;            
        }

        public BaseConfig ThenConfigFile()
        {
            return _traitement.Config;        
        }

        public TraitementTargetInfo ThenTraitementInstance()
        {
            return _traitement;
        }
    }
}
