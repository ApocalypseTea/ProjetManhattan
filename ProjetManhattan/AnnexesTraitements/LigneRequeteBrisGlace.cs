using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Formatages;

namespace ProjetManhattan
{
    class LigneRequeteBrisGlace : IToRecordable
    {
        public long Profil { get; init; }
        public string Nom { get; init; }
        public string Prenom { get; init; }
        public string Label { get; init; }
        public string Value { get; init; }
        public int NbPatientBrisGlace { get; init; }
        public DateTime Date { get; init; }

        public LigneRequeteBrisGlace(long _colProfil, string _colNom, string _colPrenom, string _colLabel, string _colValue, int _colNbPatientBrisGlace, DateTime _colDate)
        {
            Profil = _colProfil;
            Nom = _colNom;
            Prenom = _colPrenom;
            Label = _colLabel;
            Value = _colValue;
            NbPatientBrisGlace = _colNbPatientBrisGlace;
            Date = _colDate;
        }

        public Record[] ToRecords()
        {
            JObject jObject = new JObject();
            jObject.Add("nom", this.Nom);
            jObject.Add("prenom", this.Prenom);
            jObject.Add("profilID", this.Profil);
            jObject.Add("profilType", this.Value);

            Record record =  new Record()
            {
                Traitement = "BrisGlace",
                Date = this.Date,
                Target = $"ProfilID={this.Profil}",
                PropertyName = "NbBrisGlace",
                Value = this.NbPatientBrisGlace.ToString(),
                Description = jObject.ToString()
            };
            Record[] tableauRecord = { record };
            return tableauRecord;
        }
    }
}