using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using ProjetManhattan.Filtres;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;

namespace ProjetManhattan.Traitements
{
    internal class TraitementStatIP : BaseTraitement<FichierDeLogIIS>, ITraitement
    {
        private Dictionary<string, IpClient> _listingIPJournalieres;
        private int _seuilAlerte;
        public TraitementStatIP(BaseConfig config) : base(config)
        {
            ConfigStatsIP c = config.GetConfigTraitement<ConfigStatsIP>(nameof(TraitementStatIP));
            this.Filtre = new IgnoreWhiteList(c.adressesIPValides);
            _source = new FichierDeLogIIS(config);
            _seuilAlerte = c.seuilAlerteRequetesParIp; 
            _listingIPJournalieres = new Dictionary<string, IpClient>();
        }
        public override void Execute()
        {
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
                if (item.nbConnexionJournaliere > _seuilAlerte)
                {
                    Record record = new Record()
                    {
                        Traitement = nameof(TraitementStatIP),
                        Date = DateTime.Now,
                        Target = item.numeroIP,
                        PropertyName = "NbRequetes",
                        Value = item.nbConnexionJournaliere
                    };
                    this.AddItem(record);
                }
            }
        }       
        protected void UpdateStat(LigneDeLog line)
        {
            string numIpClient = line.IpClient;

            if (!_listingIPJournalieres.ContainsKey(numIpClient))
            {
                IpClient nouvelleIp = new IpClient(numIpClient);
                _listingIPJournalieres.Add(numIpClient, nouvelleIp);
            }
            _listingIPJournalieres[numIpClient].nbConnexionJournaliere++;
        }
    }
}
