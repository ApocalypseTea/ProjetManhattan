using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;

namespace ProjetManhattan.Traitements
{
    public class TraitementTargetInfo : ITraitement
    {
        public BaseConfig Config { get; set; }

        public string Name => "TargetInfo";

        private string _view;
        private string _target;
        private DateTime _dateDebut;
        private DateTime _dateFin;
        public TraitementTargetInfo(BaseConfig fakeConfig, string view, string target, DateTime dateDebut, DateTime dateFin = default)
        {
            Config = fakeConfig;
            _view = view;
            _target = target;
            _dateDebut = dateDebut;
            _dateFin = dateFin;
        }

        public void Display(string exportDataMethod, string nomDB)
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
