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

        //Passer la Query dans le fichier de config json
        private const string QUERY = "SELECT country_name FROM dbo.ip2location_db1 WHERE @ip BETWEEN ip_from AND ip_to;";

        private string regexIPv4 = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$";

        public TraitementLocalisationIp(BaseConfig config) : base(config)
        {
            ConnectionStringIPLocator = config.connectionStringIPLocator;
            _listingIp = new HashSet<IpClient>();
        }

        public override void Execute()
        {
            //Recuperation des adresse IP dans le HashSet (moins les adresse IP validées sur liste blanche)
            while (_source.HasLines())
            {
                LigneDeLog? ligne = _source.ReadLine();
                if (ligne != null && _filtre.Needed(ligne) && Regex.IsMatch(ligne.IpClient, regexIPv4))                    
                {
                    IpClient nouvelleIp = new IpClient(ligne.IpClient);
                    _listingIp.Add(nouvelleIp);
                }
            }
            
            //Connexion à la BD localisation IP
            using (SqlConnection connect = new SqlConnection(ConnectionStringIPLocator))
            {
                connect.Open();
                //Conversion adresse IP en numeroIP
                foreach (IpClient item in _listingIp)
                {
                    /*string[] numIp = item.adresseIP.Split('.');
                    int numIpTotal = 0;
                    for (int i = 0; i < _conversionIpNumbers.Length; i++)
                    {
                        numIpTotal += (int.Parse(numIp[i]) * _conversionIpNumbers[i]);
                    }
                    item.NumeroIp = numIpTotal;*/

                    using (SqlCommand requete = new SqlCommand(QUERY, connect))
                    {
                        requete.Parameters.AddWithValue("@ip", item.NumeroIp);
                        using (SqlDataReader reader = requete.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int colCountry = reader.GetOrdinal("country_name");
                                item.PaysOrigineIp = reader.GetString(colCountry);
                            }
                        }
                    }
                }
            }

            //Ajouter un record pour les IP hors france
            foreach (IpClient item in _listingIp)
            {
                if (!item.PaysOrigineIp.Equals("France"))
                {
                    Record record = new Record()
                    {
                        Traitement = nameof(TraitementLocalisationIp),
                        Date = DateTime.Now,
                        Target = item.adresseIP,
                        PropertyName = $"OrigineGeographique : {item.PaysOrigineIp}",
                        Value = 0
                    };
                    this.AddItem(record);
                }
            }
        }
    }
}
