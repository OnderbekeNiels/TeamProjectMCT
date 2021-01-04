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

        [FunctionName("AccountLogin")]
        public static async Task<IActionResult> AccountLogin(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gebruikers/login")] HttpRequest req,
            ILogger log)
        {

            //Account login:
            //naar api sturen email en ww via json, vb:
            //{
            //"email": "test1@email.com",
            //"wachtwoord": "test1efe"
            //}

            //mogelijke returns:
            //ww correct: Wachtwoord correct
            //ww incorrect: Wachtwoord incorrect
            //geen account gevonde: Geen gebruiker gevonden
            //de return values kunnen aangepast worden natuurlijk, deze zijn als vb


        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Gebruiker gebruikerLogin = JsonConvert.DeserializeObject<Gebruiker>(requestBody);
            

            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "SELECT * FROM Gebruiker WHERE Email = @Email";
                    sqlCommand.Parameters.AddWithValue("@Email", gebruikerLogin.Email);

                    SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
                    Gebruiker gebruikerDb = new Gebruiker();
                    while (reader.Read())
                    {
                        gebruikerDb.Email = reader["Email"].ToString();
                        gebruikerDb.Wachtwoord = reader["Wachtwoord"].ToString();

                    }

                    if (gebruikerDb.Email != null)
                    {
                        if (gebruikerDb.Wachtwoord == gebruikerLogin.Wachtwoord)
                        {
                            // psswd correct
                            return new OkObjectResult("Wachtwoord correct");
                        }

                        else
                        {
                            // psswd incorrect
                            return new OkObjectResult("Wachtwoord onjuist");
                        }
                    }

                    else
                    {
                        // no user in db with this email
                        return new OkObjectResult("Geen gebruiker gevonden");
                    }
                }
            }



        }
    }
}
