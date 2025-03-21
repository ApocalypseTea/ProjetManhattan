﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    class LigneRequeteSQLValidateurNonPresent : IToRecordable
    {
        private long _validateur;
        private long _idFicheRCP;
        private long _idReunionRCP;
        private DateTime _dateFicheRCP;
        public LigneRequeteSQLValidateurNonPresent(long validateur, long idFicheRCP, long idReunionRCP, DateTime dateReunionRCP)
        {
            _validateur = validateur;
            _idFicheRCP = idFicheRCP;
            _idReunionRCP = idReunionRCP;
            _dateFicheRCP = dateReunionRCP;
        }
        public Record ToRecord()
        {
            return new Record()
            {
                Traitement = "ValidateurAbsent",
                Date = this._dateFicheRCP,
                Target = $"Validateur:{this._validateur}",
                PropertyName = "ValidateurAbsent",
                Description = $"Concerne la RCP {this._idReunionRCP}",
                Value = _idFicheRCP.ToString(),
            };
        }
    }
}
