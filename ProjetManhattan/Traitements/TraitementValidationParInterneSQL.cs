﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;

namespace ProjetManhattan.Traitements
{
    class TraitementValidationParInterneSQL : BaseTraitementParRequeteSQL<LigneRequeteSQLValidationParInterne>, ITraitement
    {
        private const string RESOURCENAME = "ProjetManhattan.Configuration.QueryValidationParInterne.txt";

        private string [] _titreValidateur;
        public TraitementValidationParInterneSQL(BaseConfig config) : base(config)
        {
            ConfigRCPValideParInterne c = config.GetConfigTraitement<ConfigRCPValideParInterne>(nameof(TraitementValidationParInterneSQL));

            _titreValidateur = c.TitreValidateur;
            _items = new List<LigneRequeteSQLValidationParInterne>();
            _source = new AccesBDD(config);
            _sortie = new OutputDisplay();
        }

        protected override SqlCommand GetSQLCommand(SqlConnection connection)
        {
            SqlCommand requete = new SqlCommand(GetSQLQuery(RESOURCENAME), connection);
            DataTable table = new DataTable("toto");
            DataColumn column = new DataColumn("value");
            column.DataType = typeof(string);
            table.Columns.Add(column);

            foreach(string titre in _titreValidateur)
            {
                table.Rows.Add(titre);
            }

            SqlParameter pTitre = requete.Parameters.AddWithValue("@Titre", table);
            pTitre.SqlDbType = SqlDbType.Structured;
            pTitre.TypeName = "dbo.StringArray";
            return requete;
        }

        protected override LigneRequeteSQLValidationParInterne ReadItem(SqlDataReader reader)
        {
            int colNumeroFiche = reader.GetOrdinal("numeroFiche");
            long numeroFicheRCP = reader.GetInt64(colNumeroFiche);

            int colPatient = reader.GetOrdinal("patient");
            long idPatient = reader.GetInt64(colPatient);

            int colValidateur = reader.GetOrdinal("validateur");
            long idValidateur = reader.GetInt64(colValidateur);

            int colNom = reader.GetOrdinal("nom");
            string nomValidateur = reader.GetString(colNom);

            int colPrenom = reader.GetOrdinal("prenom");
            string prenomValidateur = reader.GetString(colPrenom);

            int colRCP = reader.GetOrdinal("numeroRCP");
            long numeroRCP = reader.GetInt64(colRCP);

            int colDate = reader.GetOrdinal("date_validation");
            DateTime date = reader.GetDateTime(colDate);

            LigneRequeteSQLValidationParInterne ligne = new LigneRequeteSQLValidationParInterne(numeroFicheRCP, idPatient, idValidateur, nomValidateur, prenomValidateur, numeroRCP, date);

            return ligne;
        }
    }
}
