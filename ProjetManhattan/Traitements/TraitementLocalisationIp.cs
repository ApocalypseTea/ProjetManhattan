using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;
using ProjetManhattan.Filtres;
using ProjetManhattan.Sources;

namespace ProjetManhattan.Traitements
{
    class TraitementLocalisationIp : TraitementStatIP, ITraitement
    {
        private HashSet<IpClient> _listingIp;
        public string ConnectionStringIPLocator { get; init; }

        private string regexIPv4 = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$";

        public TraitementLocalisationIp(BaseConfig config) : base(config)
        {
            ConnectionStringIPLocator = config.connectionStringIPLocator;
            _listingIp = new HashSet<IpClient>();
        }

        public override void Execute()
        {
            while (_source.HasLines())
            {
                LigneDeLog? ligne = _source.ReadLine();
                if (ligne != null && _filtre.Needed(ligne) && Regex.IsMatch(ligne.IpClient, regexIPv4))                    
                {
                    IpClient nouvelleIp = new IpClient(ligne.IpClient, ConnectionStringIPLocator);
                    _listingIp.Add(nouvelleIp);
                }
            }
         
            foreach (IpClient item in _listingIp)
            {
                if (!item.PaysOrigine.Equals("France"))
                {
                    Record record = new Record()
                    {
                        Traitement = "LocalisationIp",
                        Date = DateTime.Now,
                        Target = item.adresseIP,
                        PropertyName = $"OrigineGeographique : {item.PaysOrigine}",
                        Value = " "
                    };
                    this.AddItem(record);
                }
            }
        }
    }
}
