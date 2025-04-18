﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using ProjetManhattan.Configuration;
using ProjetManhattan.Traitements;

namespace ProjetManhattan
{
    internal class IpClient 
    {
        //Passer la Query dans le fichier de config json
        private const string QUERY = "SELECT country_name FROM dbo.ip2location_db1 WHERE @ip BETWEEN ip_from AND ip_to;";
        
        public string AdresseIP { get; init; }
        public int _nbConnexionJournaliere = 0;
        public string ConnectionStringIPLocator { get; init; }
        public IpClient(string adresseIP)
        {
            this.AdresseIP = adresseIP;
        }

        public IpClient(string adresseIP, string connectionStringIPLocator)
        {
            this.ConnectionStringIPLocator = connectionStringIPLocator;
            this.AdresseIP = adresseIP;
        }

        public long NumeroIp
        {
            get
            {
                long[] _conversionIpNumbers = { 16777216, 65536, 256, 1 };

                string[] numIp = AdresseIP.Split('.');
                long numIpTotal = 0;
                for (int i = 0; i < 4; i++)
                {
                    numIpTotal += (int.Parse(numIp[i]) * _conversionIpNumbers[i]);
                }
                return numIpTotal;
            }
        }

        public string PaysOrigine
        {
            get
            {
                using (SqlConnection connect = new SqlConnection(ConnectionStringIPLocator))
                {
                    connect.Open();
                    using (SqlCommand requete = new SqlCommand(QUERY, connect))
                    {
                        requete.Parameters.AddWithValue("@ip", NumeroIp);
                        using (SqlDataReader reader = requete.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int colCountry = reader.GetOrdinal("country_name");
                                string pays = reader.GetString(colCountry);
                                return pays;
                            }
                            return "";
                        }
                    }
                }
            }
        }

        public override bool Equals(object? obj)
        {
            IpClient ipClient = obj as IpClient;
            if (ipClient != null)
            {
                return this.AdresseIP.Equals(ipClient.AdresseIP);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.AdresseIP.GetHashCode();
        }
    }
}
