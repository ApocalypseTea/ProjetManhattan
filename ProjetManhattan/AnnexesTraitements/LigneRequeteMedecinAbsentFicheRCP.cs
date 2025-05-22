using Newtonsoft.Json.Linq;
using ProjetManhattan.Formatages;

namespace ProjetManhattan
{
    public class LigneRequeteMedecinAbsentFicheRCP : IToRecordable
    {
        public long IdReunion { get; init; }
        public long IdMedecin { get; init; }
        public long IdFicheRCp { get; init; }
        public DateTime DateValidation { get; init; }

        public LigneRequeteMedecinAbsentFicheRCP(long idReunion, long idMedecin, DateTime dateValidation, long idFicheRCp)
        {
            IdReunion = idReunion;
            IdMedecin = idMedecin;
            IdFicheRCp = idFicheRCp;
            DateValidation = dateValidation;
        }
        public Record[] ToRecords()
        {
            JObject jobject = new JObject();
            jobject.Add("reunionID", this.IdReunion);
            
            Record record = new Record
            {
                Traitement = "MedecinParticipantAbsent",
                Date = this.DateValidation,
                Target = $"MedecinID={this.IdMedecin}",
                PropertyName = "ParticipantAbsent",
                Value = $"FicheRCP={this.IdFicheRCp}",
                Description = jobject.ToString()
            };
            Record[] tableauRecord = { record };
            return tableauRecord;
        }
    }
}