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
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;
using Unity;

namespace ProjetManhattan.Traitements
{
    class TraitementLocalisationIp : TraitementStatIP, ITraitement
    {
        private HashSet<IpClient> _listingIp;
        public string ConnectionStringIPLocator { get; set; }
        public string Name => "LocalisationIp"; 
       
        private string _regexIPv4 = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$";
        private DateTime _dateTraitement;

        public TraitementLocalisationIp(IUnityContainer container) : base(container)
        {
            //BaseConfig config = container.Resolve<BaseConfig>();
        }

        public override void Execute()
        {
            _source = this.Container.Resolve<IFichierDeLog>();
            while (_source.HasLines())
            {
                LigneDeLog? ligne = _source.ReadLine();
                if (ligne != null && _filtre.Needed(ligne) && Regex.IsMatch(ligne.IpClient, _regexIPv4))                    
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
                        Date = _dateTraitement,
                        Target = item.AdresseIP,
                        PropertyName = "OrigineGeographique",
                        Value = $"{item.PaysOrigine}",
                        Description = ""
                    };
                    Record[] tableauRecord = new Record[] { record };
                    this.AddRecord(tableauRecord);
                }
            }
        }

        public override void InitialisationConfig(BaseConfig config)
        {
            this.ConnectionStringIPLocator = config.ConnectionStringIPLocator;
            _listingIp = new HashSet<IpClient>();
            _dateTraitement = config.DateTraitement;
        }
    }
}
