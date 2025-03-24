using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Record ToRecord()
        {
            return new Record()
            {
                Traitement = "BrisGlace",
                Date = this.Date,
                Target = $"ProfilID={this.Profil}",
                PropertyName = "NbBrisGlace",
                Value = this.NbPatientBrisGlace.ToString(),
                Description = $"{this.Nom} {this.Prenom} {this.Value}"
            };
        }
    }
}
