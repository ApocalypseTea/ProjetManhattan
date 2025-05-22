using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Configuration;
using ProjetManhattan.Filtres;
using ProjetManhattan.Formatages;
using Unity;

namespace ProjetManhattan.Traitements
{
    internal class TraitementCheckHTTPErrors : BaseTraitementParLigne, ITraitement
    {
        public TraitementCheckHTTPErrors(IUnityContainer container) : base(container)
        {
        }

        public string Name => "HTTPErrors";

        public void InitialisationConfig(BaseConfig config)
        {
            ConfigHTTPErrors c = config.GetConfigTraitement<ConfigHTTPErrors>(nameof(TraitementCheckHTTPErrors));
            this.Filtre = new OnlyURLWhiteListAndHTTPError4xx(c.PatternURLValide);
        }

        protected override void FillRecord(Record[] records, LigneDeLog ligne)
        {
            JObject jobject = new JObject();

            string patternTiret = "^-$";
            string patternTime = @"\+time=";
            Match matchTiret = Regex.Match(ligne.CsUriQuery, patternTiret);
            Match matchTime = Regex.Match(ligne.CsUriQuery, patternTime);

            if (matchTiret.Success || matchTime.Success)
            {
                jobject.Add("queryParams", "");
            } 
            else
            {
                jobject.Add("queryParams", ligne.CsUriQuery);
            }
                jobject.Add("userAgent", ligne.UserAgentHTTP);
            jobject.Add("ip", ligne.IpClient);

            foreach (Record record in records)
            {
                record.Traitement = "HTTPErrors";
                record.Target = ligne.CsUriStem;
                record.PropertyName = "ErrorCode";
                record.Value = ligne.CsStatutHTTP.ToString();
                record.Description = jobject.ToString();
                record.Date = ligne.DateHeure;
            }
        }
    }
}
