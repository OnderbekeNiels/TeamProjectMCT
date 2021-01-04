using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TeamProjectFunction.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;
using TeamProjectFunction.Repository;

namespace TeamProjectFunction
{
    public static class Function1
    {
        [FunctionName("CreateAccount")]
        public static async Task<IActionResult> CreateAccount(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "gebruikers/create")] HttpRequest req,
            ILogger log)
        {
            

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Gebruiker gebruiker = JsonConvert.DeserializeObject<Gebruiker>(requestBody);
            gebruiker.GebruikerId = Guid.NewGuid();         

            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using(SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "INSERT INTO Gebruiker VALUES (@GebruikerId, @Naam, @Voornaam,@Email, @Wachtwoord)";
                    sqlCommand.Parameters.AddWithValue("@GebruikerId", gebruiker.GebruikerId);
                    sqlCommand.Parameters.AddWithValue("@Naam", gebruiker.Naam);
                    sqlCommand.Parameters.AddWithValue("@Voornaam", gebruiker.Voornaam);
                    sqlCommand.Parameters.AddWithValue("@Email", gebruiker.Email);
                    sqlCommand.Parameters.AddWithValue("@Wachtwoord", gebruiker.Wachtwoord);

                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }


            return new OkObjectResult(gebruiker);
        }
    }
}
