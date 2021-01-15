using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using Newtonsoft.Json;

namespace TempoTrack.Repositories
{
    public class EtappeRepository
    {
        private const string _BASEURI = "https://temptrackingfunction.azurewebsites.net/api";
        private const string _FUNCTIONKEY = "WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==";
        public static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            return client;
        }

        public static async Task<Etappe> CreateEtappe(Etappe etappe)
        {
            string url = $"{_BASEURI}/etappes?code={_FUNCTIONKEY}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    //Creeren van een json
                    string json = JsonConvert.SerializeObject(etappe);
                    //aanduiden dat het geen dat we willen doorsturen van het type application json moet zijn
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    //request doen naar api
                    var response = await client.PostAsync(url, content);
                    //controleren of de put gelukt is
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Unsuccesful POST to url: {url}, object; {json}");
                        return null;
                    }
                    else
                    {
                        //Extra check of de status code success is
                        response.EnsureSuccessStatusCode();
                        //Bode van de response inlezen
                        var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        //Nieuw object ronde aanmaken met de data van de response
                        Etappe etappeResponse = JsonConvert.DeserializeObject<Etappe>(Convert.ToString(responseBody));

                        //return object ronde met invite code etc
                        return etappeResponse;

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<List<EtappesRonde>> GetEtappesRonde(Guid rondeId, Guid gebruikerId)
        {
            string url = $"{_BASEURI}/gebruiker/ronde/etappes/{rondeId}/{gebruikerId}?code={_FUNCTIONKEY}";
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

        public static async Task<int> RemoveDeelnemerRonde(Guid gebruikersId,Guid rondeId)
        {
            string url = $"{_BASEURI}/deelnemer/{gebruikersId}/{rondeId}?code={_FUNCTIONKEY}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    HttpContent content = null;
                    var response = await client.PutAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}

