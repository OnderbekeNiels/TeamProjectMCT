using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TempoTrack.Models;
using Newtonsoft.Json;


namespace TempoTrack.Repositories
{
    class LoginRepository
    {
        private const string _BASEURI = "https://temptrackingfunction.azurewebsites.net/api/gebruikers/login";
        private const string _FUNCTIONKEY = "WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==";
        public static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            return client;
        }

        public static async Task<GebruikerV2> CheckLogin(GoogleLogin googleLogin)
        {
            string url = $"{_BASEURI}?code={_FUNCTIONKEY}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    HttpClient httpClient = GetHttpClient();
                    //httpClient.DefaultRequestHeaders.Add("email", googleLogin.email); 
                    //httpClient.DefaultRequestHeaders.Add("gebruikersnaam", googleLogin.name);
                    string data = JsonConvert.SerializeObject(googleLogin);
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(url),
                        Content = new StringContent(data, Encoding.UTF8, "application/json"),
                    };

                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);


                    //string json = await client.GetStringAsync(url);
                    GebruikerV2 gebruikerInfo = JsonConvert.DeserializeObject<GebruikerV2>(Convert.ToString(responseBody));
                    return gebruikerInfo;
                }
                catch (Exception ex)
                {
                    return null;
                    //throw ex;
                }
            }
        }

    }
}
