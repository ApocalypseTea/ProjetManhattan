using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using ProjetManhattan.Filtres;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;
using Unity;

namespace ProjetManhattan.Traitements
{
    internal class TraitementStatIP : BaseTraitement<IFichierDeLog>, ITraitement
    {
        protected Dictionary<string, IpClient> _listingIPJournalieres;
        private int _seuilAlerte;
        private DateTime _dateTraitement;
        public string Name
        {
            get
            {
                return "StatIp";
            }
        }
        public TraitementStatIP(IUnityContainer container) : base(container)
        {
            BaseConfig config = container.Resolve<BaseConfig>();
            ConfigStatsIP c = config.GetConfigTraitement<ConfigStatsIP>(nameof(TraitementStatIP));
            this.Filtre = new IgnoreWhiteList(c.AdressesIPValides);
            _seuilAlerte = c.SeuilAlerteRequetesParIp; 
            _listingIPJournalieres = new Dictionary<string, IpClient>();
            _dateTraitement = config.DateTraitement;
        }
        public override void Execute()
        {
            _source = this.Container.Resolve<FichierDeLogIIS>();

            while (_source.HasLines())
            {
                LigneDeLog? ligne = _source.ReadLine();
                if (ligne != null && _filtre.Needed(ligne))
                {
                    this.UpdateStat(ligne);
                }
            }

            foreach (var item in _listingIPJournalieres.Values)
            {
                if (item._nbConnexionJournaliere > _seuilAlerte)
                {
                    Record record = new Record()
                    {
                        Traitement = "StatIp",
                        Date = _dateTraitement,
                        Target = item.AdresseIP,
                        PropertyName = "NbRequetes",
                        Value = item._nbConnexionJournaliere.ToString(),
                        Description = ""
                    };
                    this.AddRecord(record);
                }
            }
        }       
        protected virtual void UpdateStat(LigneDeLog line)
        {
            string numIpClient = line.IpClient;

            if (!_listingIPJournalieres.ContainsKey(numIpClient))
            {
                IpClient nouvelleIp = new IpClient(numIpClient);
                _listingIPJournalieres.Add(numIpClient, nouvelleIp);
            }
            _listingIPJournalieres[numIpClient]._nbConnexionJournaliere++;
        }
    }
}
