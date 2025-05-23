﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;
using Unity;

namespace ProjetManhattan.Traitements
{
    class TraitementValidateurRCPnonPresentSQL : BaseTraitementParRequeteSQL<LigneRequeteSQLValidateurNonPresent>, ITraitement
    {
        private const string RESOURCENAME = "ProjetManhattan.Configuration.QueryValidateurRCPAbsent.txt";
        private DateTime _dateTraitement;
        public string Name => "ValidateurAbsent";
        
        public TraitementValidateurRCPnonPresentSQL(BaseConfig config, IUnityContainer container) : base(container)
        { 
        }
        protected override IDbCommand GetSQLCommand(IDbConnection connection)
        {
            IDbCommand requete = connection.CreateCommand();
            requete.CommandText = GetSQLQuery(RESOURCENAME);
            requete.CommandType = CommandType.Text;

            requete.AddParameterWithValue("@dateTraitement", _dateTraitement);
            return requete;
        }
        protected override LigneRequeteSQLValidateurNonPresent ReadItem(IDataReader reader)
        {
            int _colValidateur = reader.GetOrdinal("Validateur");
            int _colIdFicheRCP = reader.GetOrdinal("FicheRCP");
            int _colDateFicheRCP = reader.GetOrdinal("DateValidationRCP");
            int _colIdReunionRCP = reader.GetOrdinal("ReunionRCP");
            int _colDateReunionRCP = reader.GetOrdinal("date_reunion");
            int _colIdSpecialiteMedicale = reader.GetOrdinal("specialite_ref");
            int _colLieuReunion = reader.GetOrdinal("label");
            int _colNomValidateur = reader.GetOrdinal("validateur_nom");
            int _colPrenomValidateur = reader.GetOrdinal("validateur_prenom");
            int _colSpecialiteMedicale = reader.GetOrdinal("specialite_med");
            int _colProfilUser = reader.GetOrdinal("profil");
            int _colProfilTitre = reader.GetOrdinal("titre");

            long _validateur = reader.GetInt64(_colValidateur);
            long _idFicheRCP = reader.GetInt64(_colIdFicheRCP);
            long _idReunionRCP = reader.GetInt64(_colIdReunionRCP);
            DateTime _dateFicheRCP = reader.GetDateTime(_colDateFicheRCP);
            DateTime _dateReunionRCP = reader.GetDateTime(_colDateReunionRCP);
            long _idSpecialiteMed = reader.GetInt32(_colIdSpecialiteMedicale);
            string _lieuReunion = reader.GetString(_colLieuReunion);
            string _nomValidateur = reader.GetString(_colNomValidateur);
            string _prenomValidateur = reader.GetString(_colPrenomValidateur);
            string _specialiteMedicale = reader.GetString(_colSpecialiteMedicale);
            string _profilUser = reader.GetString(_colProfilUser);
            string _titreUser = reader.GetString(_colProfilTitre);

            return new LigneRequeteSQLValidateurNonPresent(_validateur, _idFicheRCP, _idReunionRCP, _dateFicheRCP, _dateReunionRCP, _colIdSpecialiteMedicale, _lieuReunion, _nomValidateur, _prenomValidateur, _specialiteMedicale, _profilUser, _titreUser);
        }

        public void InitialisationConfig(BaseConfig config)
        {
            _dateTraitement = config.DateTraitement;
        }
    }
}
