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
using Unity;

namespace ProjetManhattan.Traitements
{
    class TraitementValidationParInterneSQL : BaseTraitementParRequeteSQL<LigneRequeteSQLValidationParInterne>, ITraitement
    {
        private const string RESOURCENAME = "ProjetManhattan.Configuration.QueryValidationParInterne.txt";

        private string [] _titreValidateur;
        private DateTime _dateTraitement;
        public string Name => "ValidationInterne";
          
        public TraitementValidationParInterneSQL(BaseConfig config, IUnityContainer container) : base(container)
        {
        }
        protected override IDbCommand GetSQLCommand(IDbConnection connection)
        {
            IDbCommand requete = connection.CreateCommand();
            requete.CommandText = GetSQLQuery(RESOURCENAME);
            requete.CommandType = CommandType.Text;
            DataTable table = new DataTable("toto");
            DataColumn column = new DataColumn("value");
            column.DataType = typeof(string);
            table.Columns.Add(column);

            foreach(string titre in _titreValidateur)
            {
                table.Rows.Add(titre);
            }

            SqlParameter pTitre = requete.AddParameterWithValue("@Titre", table);
            pTitre.SqlDbType = SqlDbType.Structured;
            pTitre.TypeName = "dbo.StringArray";
            requete.AddParameterWithValue("@dateTraitement", _dateTraitement);
            return requete;
        }

        protected override LigneRequeteSQLValidationParInterne ReadItem(IDataReader reader)
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

            int colDateValidationFiche = reader.GetOrdinal("date_validation");
            DateTime dateValidationFiche = reader.GetDateTime(colDateValidationFiche);

            int colDateReunionRCP = reader.GetOrdinal("dateRCP");
            DateTime dateReunionRCP = reader.GetDateTime(colDateReunionRCP);
            
            int colIdSpecialiteMedicale = reader.GetOrdinal("id_specialite");
            long specialiteID = reader.GetInt32(colIdSpecialiteMedicale);

            int colLieuReunion = reader.GetOrdinal("salle_reunion");
            string lieuReunionRCP = reader.GetString(colLieuReunion);

            int colSpecialiteMedicale = reader.GetOrdinal("specialite_med");
            string specialite = reader.GetString(colSpecialiteMedicale);

            int colProfilValidateur = reader.GetOrdinal("profil_validateur");
            string profiValidateur = reader.GetString(colProfilValidateur);


            LigneRequeteSQLValidationParInterne ligne = new LigneRequeteSQLValidationParInterne(numeroFicheRCP, idPatient, idValidateur, nomValidateur, prenomValidateur, numeroRCP, dateValidationFiche, dateReunionRCP, specialiteID, lieuReunionRCP, specialite, profiValidateur);

            return ligne;
        }

        public void InitialisationConfig(BaseConfig config)
        {
            ConfigRCPValideParInterne c = config.GetConfigTraitement<ConfigRCPValideParInterne>(nameof(TraitementValidationParInterneSQL));
            _titreValidateur = c.TitreValidateur;
            _lines = new List<LigneRequeteSQLValidationParInterne>();
            _source = new AccesBDD(config);
            _sortie = new OutputDisplay();
            _dateTraitement = config.DateTraitement;
        }
    }
}
