﻿using System;
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
        public TraitementStatIP(BaseConfig config) : base(config)
        {
            ConfigStatsIP c = config.GetConfigTraitement<ConfigStatsIP>(nameof(TraitementStatIP));
            this.Filtre = new IgnoreWhiteList(c.AdressesIPValides);
            _source = new FichierDeLogIIS(config);
            _seuilAlerte = c.SeuilAlerteRequetesParIp; 
            _listingIPJournalieres = new Dictionary<string, IpClient>();
            _dateTraitement = config.DateTraitement;
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
                    this.AddItem(record);
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
