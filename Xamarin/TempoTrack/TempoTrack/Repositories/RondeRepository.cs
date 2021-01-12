using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;

namespace TempoTrack.Repositories
{
    public class RondeRepository
    {
        private const string _BASEURI = "https://temptrackingfunction.azurewebsites.net/api/rondes";
        private const string _BASEURI2 = "https://temptrackingfunction.azurewebsites.net/api/ronde/klassement/";
        private const string _BASEURI3 = "https://temptrackingfunction.azurewebsites.net/api/gebruikers/ronde/";
        private const string _FUNCTIONKEY = "WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==";
        public static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            return client;
        }

        public static async Task<Ronde> CreateRonde(Ronde ronde)
        {
            string url = $"{_BASEURI}?code={_FUNCTIONKEY}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    //Creeren van een json
                    string json = JsonConvert.SerializeObject(ronde);
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
                        Ronde rondeResponse = JsonConvert.DeserializeObject<Ronde>(Convert.ToString(responseBody));

                        //return object ronde met invite code etc
                        return rondeResponse;

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<List<RondesGebruiker>> GetRondesGebruiker(Guid gebruikerId)
        {
            string url = $"{_BASEURI3}{gebruikerId}?code={_FUNCTIONKEY}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);
                    List<RondesGebruiker> list = JsonConvert.DeserializeObject<List<RondesGebruiker>>(json);
                    return list;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public static async Task<List<RondeKlassement>> GetRondeKlassement(Guid rondeId)
        {
            string url = $"{_BASEURI2}{rondeId}?code={_FUNCTIONKEY}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);
                    List<RondeKlassement> list = JsonConvert.DeserializeObject<List<RondeKlassement>>(json);
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
