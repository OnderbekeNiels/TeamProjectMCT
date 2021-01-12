using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;

namespace TempoTrack.Repositories
{
    class EtappesRepository
    {
        private const string _BASEURI = "https://temptrackingfunction.azurewebsites.net/api/gebruikers/ronde";
        private const string _FUNCTIONKEY = "WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==";

        public static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            return client;
        }

        public static async Task<List<EtappesRonde>> GetEtappesRonde(Guid gebruikerId, Guid rondeId)
        {
            string url = $"{_BASEURI}/{gebruikerId}/{rondeId}?code={_FUNCTIONKEY}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);
                    List<EtappesRonde> list = JsonConvert.DeserializeObject<List<EtappesRonde>>(json);
                    return list;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}
