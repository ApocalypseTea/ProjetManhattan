using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using ProjetManhattan.Sources;
using Microsoft.Data.SqlClient;
using System.Data;
using ProjetManhattan.Formatages;

namespace ProjetManhattan.Traitements
{
    class TraitementBrisDeGlace : ITraitement
    {
        //Voir le type reel de retour de la requete SQL
        private List<LigneRequeteBrisGlace> _resultatsRequeteSQL;
        private int _seuilAlerteBrisGlace;
        private AccesBDD _source;
        private string _queryBrisGlace;
        private IFormatage _sortie;

        public TraitementBrisDeGlace(BaseConfig config)
        {
            ConfigBrisGlace c = config.GetConfigTraitement<ConfigBrisGlace>(nameof(TraitementBrisDeGlace));
            _seuilAlerteBrisGlace = c.SeuilAlerteBrisDeGlaceJournalierParUtilisateur;

            _resultatsRequeteSQL = new List<LigneRequeteBrisGlace> ();
            _source = new AccesBDD(config);
            _sortie = new OutputDisplay();
            _queryBrisGlace = "SELECT BG.profil_ref, \t\r\n\tU.nom, \r\n\tU.prenom,\t \r\n\tPPSS.label,\r\n\tPT.value,\r\n\tCOUNT(BG.patient_ref) AS nb_patient_brise_glace, \r\n\tCONVERT(DATE, BG.creation_date) AS date\r\nFROM account.bris_glace AS BG\r\nJOIN account.ZT_profil AS P ON BG.profil_ref = P.id \r\nJOIN account.profil_type_enum AS PT ON P.type_ref = PT.id\r\nJOIN account.ZT_user AS U ON P.user_ref = U.id\r\nLEFT JOIN account.profil_professionnel_sante AS PPS ON PPS.profil_id=P.id\r\nLEFT JOIN account.profil_professionnel_sante_specialite_enum AS PPSS ON PPS.specialite_ref=PPSS.id\r\nGROUP BY BG.profil_ref,\t\r\n\tPT.value,\r\n\tU.nom, \r\n\tU.prenom,\t\r\n\tPPSS.label,\r\n\tCONVERT(DATE, BG.creation_date)\r\n\tHAVING COUNT(BG.patient_ref)>@Seuil\r\nORDER BY CONVERT(DATE, BG.creation_date) DESC\r\n;";
        }

        public void Execute()
        {
            using (SqlConnection connexion = _source.ConnexionBD())
            using (SqlCommand requete = new SqlCommand(_queryBrisGlace, connexion))
            {
                requete.Parameters.AddWithValue("@Seuil", _seuilAlerteBrisGlace);
                using (SqlDataReader reader = requete.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //reader.GetName(2);
                        int colProfilRef = reader.GetOrdinal("profil_ref");
                        int colPrenom = reader.GetOrdinal("prenom");
                        int colNom = reader.GetOrdinal("nom");
                        int colLabel = reader.GetOrdinal("label");
                        int colValue = reader.GetOrdinal("value");
                        int colNbPatient = reader.GetOrdinal("nb_patient_brise_glace");
                        int colDate = reader.GetOrdinal("date");

                        long profilRef = reader.GetInt64(colProfilRef);
                        string prenom = reader.GetString(colPrenom);
                        string nom = reader.GetString(colNom);
                        string label = reader.GetString(colLabel);
                        string value = reader.GetString(colValue);
                        int nbPatientBrisGlace = reader.GetInt32(colNbPatient);
                        DateTime date = reader.GetDateTime(colDate);

                        LigneRequeteBrisGlace ligne = new LigneRequeteBrisGlace(profilRef, prenom, nom, label, value, nbPatientBrisGlace, date);

                        AddLine(ligne);
                    }
                }
            }
        }

        public void AddLine(LigneRequeteBrisGlace ligne)
        {
            _resultatsRequeteSQL.Add(ligne);
        }

        public void Display()
        {
            List<Notification> notifications = new List<Notification>();
            foreach (LigneRequeteBrisGlace ligneRequete in _resultatsRequeteSQL)
            {
                Notification notification = new Notification($"L'utilisateur {ligneRequete.Nom} {ligneRequete.Prenom} a effectué {ligneRequete.NbPatientBrisGlace} bris de glace le {ligneRequete.Date.ToString("dd/MM/yyyy")}");

                notifications.Add(notification);

            }
            _sortie.Display(notifications);
        }

    }
}
