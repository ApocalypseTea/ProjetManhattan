using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;

namespace ProjetManhattan.Traitements
{
    public class TraitementDureeMoyenneRequeteSQL : BaseTraitementParRequeteSQL<LigneRequeteDureeMoyenneSQL>, ITraitement
    {
        private List<object> _items;
        public string Name => "DureeTraitementRequeteSQL";

        public TraitementDureeMoyenneRequeteSQL(BaseConfig config, List<object> items) : base(config)
        {
            _items = items;
        }

        public void Display(string exportDataMethod, string nomDB)
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
          
        }

        protected override LigneRequeteDureeMoyenneSQL ReadItem(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }

        protected override SqlCommand GetSQLCommand(SqlConnection connection)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<object> Items => _items;
    }
}
