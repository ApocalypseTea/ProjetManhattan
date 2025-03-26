using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    class LigneRequeteSQLChgtIdentiteUser : IToRecordable
    {
        private long _idUser;
        private string _nomUser;
        private string _prenomUser;
        private string _previousNomUser;
        private string _previousPrenomUser;
        private DateTime dateModificationNom;
        private long _idModificateur;
        private string _nomModificateur;
        private string _prenomModificateur;
        private string _typeModificateur;

        public LigneRequeteSQLChgtIdentiteUser(long idUser, string nomUser, string prenomUser, string previousNomUser, string previousPrenomUser, DateTime dateModificationNom, long idModificateur, string nomModificateur, string prenomModificateur, string typeModificateur)
        {
            _idUser = idUser;
            _nomUser = nomUser;
            _prenomUser = prenomUser;
            _previousNomUser = previousNomUser;
            _previousPrenomUser = previousPrenomUser;
            this.dateModificationNom = dateModificationNom;
            _idModificateur = idModificateur;
            _nomModificateur = nomModificateur;
            _prenomModificateur = prenomModificateur;
            _typeModificateur = typeModificateur;
        }
        public Record ToRecord()
        {
            return new Record()
            {
                Traitement = "ChangementIdentite",
                Date = this.dateModificationNom,
                Target = $"UserID={this._idUser}",
                PropertyName = $"Modificateur",
                Description = $"PreviousName={this._previousNomUser} {this._previousPrenomUser} /ModificateurID={this._idModificateur.ToString()} {this._nomModificateur} {this._prenomModificateur} {this._typeModificateur}",
                Value =  $"{this._prenomUser} {this._nomUser}"
            };
        }
    }
}
