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
using System.Data;
using TeamProjectFunction.Repository;

namespace TeamProjectFunction
{
    public static class Function1
    {
        //[FunctionName("CreateAccount")]
        //public static async Task<IActionResult> CreateAccount(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "gebruikers/create")] HttpRequest req,
        //    ILogger log)
        //{

        //    //Account create:
        //    //naar api sturen email, ww, voornaam, achternaam via json, vb:
        //    //{
        //    //"naam": "test6",
        //    //"voornaam": "test1",
        //    //"email": "test1@email.com",
        //    //"wachtwoord": "test1efe"
        //    //}

        //    //mogelijke returns:
        //    //account aangemaakt gebruiker met alle params
        //    //gebruiker bestaat al: Gebruiker bestaat al
        //    //de return values kunnen aangepast worden natuurlijk, deze zijn als vb


        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    Gebruiker gebruiker = JsonConvert.DeserializeObject<Gebruiker>(requestBody);

        //    gebruiker.GebruikerId = Guid.NewGuid();         

        //    string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        //    // controleren als het email nog niet gebruikt is
        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        await sqlConnection.OpenAsync();
        //        using (SqlCommand sqlCommand = new SqlCommand())
        //        {
        //            sqlCommand.Connection = sqlConnection;
        //            sqlCommand.CommandText = "SELECT * FROM Gebruiker WHERE Email = @Email";
        //            sqlCommand.Parameters.AddWithValue("@Email", gebruiker.Email);

        //            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
        //            Gebruiker gebruikerDb = new Gebruiker();
        //            while (reader.Read())
        //            {
        //                gebruikerDb.Email = reader["Email"].ToString();
        //                gebruikerDb.Wachtwoord = reader["Wachtwoord"].ToString();

        //            }

        //            if (gebruikerDb.Email != gebruiker.Email)
        //            {
        //                using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionString))
        //                {
        //                    await sqlConnectionInsert.OpenAsync();
        //                    using (SqlCommand sqlCommandInsert = new SqlCommand())
        //                    {
        //                        sqlCommandInsert.Connection = sqlConnectionInsert;
        //                        sqlCommandInsert.CommandText = "INSERT INTO Gebruiker VALUES (@GebruikerId, @Naam, @Voornaam,@Email, @Wachtwoord)";
        //                        sqlCommandInsert.Parameters.AddWithValue("@GebruikerId", gebruiker.GebruikerId);
        //                        sqlCommandInsert.Parameters.AddWithValue("@Naam", gebruiker.Naam);
        //                        sqlCommandInsert.Parameters.AddWithValue("@Voornaam", gebruiker.Voornaam);
        //                        sqlCommandInsert.Parameters.AddWithValue("@Email", gebruiker.Email);
        //                        sqlCommandInsert.Parameters.AddWithValue("@Wachtwoord", gebruiker.Wachtwoord);

        //                        await sqlCommandInsert.ExecuteNonQueryAsync();
        //                    }
        //                    // return gemaakte gebruiker
        //                    return new OkObjectResult(gebruiker);
        //                }
        //            }
        //            else
        //            {
        //                // gebruiker bestaat al
        //                return new OkObjectResult("Gebruiker bestaat al");
        //            }
        //        }
        //    }




        //}

        //[FunctionName("AccountLogin")]
        //public static async Task<IActionResult> AccountLogin(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gebruikers/login")] HttpRequest req,
        //    ILogger log)
        //{

        //    //Account login:
        //    //naar api sturen email en ww via json, vb:
        //    //{
        //    //"email": "test1@email.com",
        //    //"wachtwoord": "test1efe"
        //    //}

        //    //mogelijke returns:
        //    //ww correct: Wachtwoord correct
        //    //ww incorrect: Wachtwoord incorrect
        //    //geen account gevonde: Geen gebruiker gevonden
        //    //de return values kunnen aangepast worden natuurlijk, deze zijn als vb


        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    Gebruiker gebruikerLogin = JsonConvert.DeserializeObject<Gebruiker>(requestBody);


        //    string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        await sqlConnection.OpenAsync();
        //        using (SqlCommand sqlCommand = new SqlCommand())
        //        {
        //            sqlCommand.Connection = sqlConnection;
        //            sqlCommand.CommandText = "SELECT * FROM Gebruiker WHERE Email = @Email";
        //            sqlCommand.Parameters.AddWithValue("@Email", gebruikerLogin.Email);

        //            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
        //            Gebruiker gebruikerDb = new Gebruiker();
        //            while (reader.Read())
        //            {
        //                gebruikerDb.Email = reader["Email"].ToString();
        //                gebruikerDb.Wachtwoord = reader["Wachtwoord"].ToString();

        //            }

        //            if (gebruikerDb.Email != null)
        //            {
        //                if (gebruikerDb.Wachtwoord == gebruikerLogin.Wachtwoord)
        //                {
        //                    // psswd correct
        //                    return new OkObjectResult("Wachtwoord correct");
        //                }

        //                else
        //                {
        //                    // psswd incorrect
        //                    return new OkObjectResult("Wachtwoord onjuist");
        //                }
        //            }

        //            else
        //            {
        //                // no user in db with this email
        //                return new OkObjectResult("Geen gebruiker gevonden");
        //            }
        //        }
        //    }



        //}

        //[FunctionName("CreateAccount")]
        //public static async Task<IActionResult> CreateAccount(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "gebruikers/create")] HttpRequest req,
        //    ILogger log)
        //{

        //    //Account create:
        //    //naar api sturen email en gebruikersnaam via json gebruikersnaam is optioneel, vb:
        //    //{
        //    //"email": "test1@email.com",
        //    //"gebruikersnaam": "test1"
        //    //}

        //    //mogelijke returns:
        //    //account aangemaakt: gebruikerv2 met alle params
        //    //gebruiker bestaat al: custom response zie model
        //    //de return values kunnen aangepast worden natuurlijk, deze zijn als vb


        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    GebruikerV2 gebruiker = JsonConvert.DeserializeObject<GebruikerV2>(requestBody);

        //    gebruiker.GebruikerId = Guid.NewGuid();

        //    string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        //    // controleren als het email nog niet gebruikt is
        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        await sqlConnection.OpenAsync();
        //        using (SqlCommand sqlCommand = new SqlCommand())
        //        {
        //            sqlCommand.Connection = sqlConnection;
        //            sqlCommand.CommandText = "SELECT * FROM Gebruiker WHERE Email = @Email";
        //            sqlCommand.Parameters.AddWithValue("@Email", gebruiker.Email);

        //            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
        //            Gebruiker gebruikerDb = new Gebruiker();
        //            while (reader.Read())
        //            {
        //                gebruikerDb.Email = reader["Email"].ToString();

        //            }

        //            if (gebruikerDb.Email != gebruiker.Email)
        //            {
        //                using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionString))
        //                {
        //                    await sqlConnectionInsert.OpenAsync();
        //                    using (SqlCommand sqlCommandInsert = new SqlCommand())
        //                    {
        //                        sqlCommandInsert.Connection = sqlConnectionInsert;
        //                        sqlCommandInsert.CommandText = "INSERT INTO Gebruiker VALUES (@GebruikerId, @GebruikersNaam, @Email)";
        //                        sqlCommandInsert.Parameters.AddWithValue("@GebruikerId", gebruiker.GebruikerId);
        //                        sqlCommandInsert.Parameters.AddWithValue("@GebruikersNaam", gebruiker.GebruikersNaam);
        //                        sqlCommandInsert.Parameters.AddWithValue("@Email", gebruiker.Email);

        //                        await sqlCommandInsert.ExecuteNonQueryAsync();
        //                    }
        //                    // return gemaakte gebruiker
        //                    return new OkObjectResult(gebruiker);
        //                }
        //            }
        //            else
        //            {
        //                // gebruiker bestaat al
        //                CustomResponse customResponse = new CustomResponse();
        //                customResponse.Succes = false;
        //                customResponse.Message = "Gebruiker bestaat al";
        //                return new OkObjectResult(customResponse);
        //            }
        //        }
        //    }




        //}



        //[FunctionName("AccountLogin")]
        //public static async Task<IActionResult> AccountLogin(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gebruikers/login")] HttpRequest req,
        //    ILogger log)
        //{

        //    //Account login:
        //    //naar api sturen email json, vb:
        //    //{
        //    //"email": "test1@email.com"
        //    //}

        //    //mogelijke returns:
        //    //gebruiker gevonden: gebruikerv2 met alle params
        //    //geen account gevonden: custom response zie model
        //    //de return values kunnen aangepast worden natuurlijk, deze zijn als vb


        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    GebruikerV2 gebruikerLogin = JsonConvert.DeserializeObject<GebruikerV2>(requestBody);


        //    string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        await sqlConnection.OpenAsync();
        //        using (SqlCommand sqlCommand = new SqlCommand())
        //        {
        //            sqlCommand.Connection = sqlConnection;
        //            sqlCommand.CommandText = "SELECT * FROM Gebruiker WHERE Email = @Email";
        //            sqlCommand.Parameters.AddWithValue("@Email", gebruikerLogin.Email);

        //            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
        //            GebruikerV2 gebruikerDb = new GebruikerV2();
        //            while (reader.Read())
        //            {
        //                gebruikerDb.Email = reader["Email"].ToString();
        //                gebruikerDb.GebruikerId = Guid.Parse(reader["GebruikersId"].ToString());
        //                gebruikerDb.GebruikersNaam = reader["GebruikersNaam"].ToString();

        //            }

        //            if (gebruikerDb.Email != null)
        //            {
        //                gebruikerLogin.GebruikerId = gebruikerDb.GebruikerId;
        //                gebruikerLogin.GebruikersNaam = gebruikerDb.GebruikersNaam;

        //                return new OkObjectResult(gebruikerLogin);
        //            }

        //            else
        //            {
        //                // no user in db with this email
        //                CustomResponse customResponse = new CustomResponse();
        //                customResponse.Succes = false;
        //                customResponse.Message = "Geen gebruiker gevonden in de db";
        //                return new OkObjectResult(customResponse);
        //            }
        //        }
        //    }



        //}

        [FunctionName("AccountLogin")]
        public static async Task<IActionResult> AccountLogin(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", "get", Route = "gebruikers/login")] HttpRequest req,
            ILogger log)
        {

            //Account login:
            //naar api sturen email json, vb:
            //{
            //"email": "test1@email.com"
            //}

            //mogelijke returns:
            //gebruiker gevonden: gebruikerv2 met alle params
            //geen account gevonden: account wordt aangemaakt en returnd gebruikerv2 met alle params
            //de return values kunnen aangepast worden natuurlijk, deze zijn als vb


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            GebruikerV2 gebruikerLogin = JsonConvert.DeserializeObject<GebruikerV2>(requestBody);


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
                    GebruikerV2 gebruikerDb = new GebruikerV2();
                    while (reader.Read())
                    {
                        gebruikerDb.Email = reader["Email"].ToString();
                        gebruikerDb.GebruikerId = Guid.Parse(reader["GebruikersId"].ToString());
                        gebruikerDb.GebruikersNaam = reader["GebruikersNaam"].ToString();

                    }

                    if (gebruikerDb.Email != null)
                    {
                        gebruikerLogin.GebruikerId = gebruikerDb.GebruikerId;
                        gebruikerLogin.GebruikersNaam = gebruikerDb.GebruikersNaam;

                        return new OkObjectResult(gebruikerLogin);
                    }

                    else
                    {
                        gebruikerLogin.GebruikerId = Guid.NewGuid();
                        using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionString))
                        {
                            await sqlConnectionInsert.OpenAsync();
                            using (SqlCommand sqlCommandInsert = new SqlCommand())
                            {
                                sqlCommandInsert.Connection = sqlConnectionInsert;
                                sqlCommandInsert.CommandText = "INSERT INTO Gebruiker VALUES (@GebruikerId, @GebruikersNaam, @Email)";
                                sqlCommandInsert.Parameters.AddWithValue("@GebruikerId", gebruikerLogin.GebruikerId);
                                sqlCommandInsert.Parameters.AddWithValue("@GebruikersNaam", gebruikerLogin.GebruikersNaam);
                                sqlCommandInsert.Parameters.AddWithValue("@Email", gebruikerLogin.Email);

                                await sqlCommandInsert.ExecuteNonQueryAsync();
                            }
                            // return gemaakte gebruiker
                            return new OkObjectResult(gebruikerLogin);
                        }

                    }
                }
            }



        }


        [FunctionName("CreateRonde")]
        public static async Task<IActionResult> CreateRonde(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "rondes/create")] HttpRequest req,
           ILogger log)
        {

            //Account create:
            //naar api sturen email en gebruikersnaam via json gebruikersnaam is optioneel, vb:
            //{
            //"Admin": "ee6c128e-0b7e-4d8f-b139-6a40308e112d",
            //"naam": "testronde1",
            //"startdatum" : "2021-01-05T15:00:00"
            //}

            //mogelijke returns:
            //als ronde gemaakt is wordt het model ronde met alle params terug gestuurd


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Ronde ronde = JsonConvert.DeserializeObject<Ronde>(requestBody);

            // genereer nieuwe invite code
            ronde.RondeId = Guid.NewGuid();
            ronde.EindDatum = DateTime.ParseExact("9999-12-31 11:59:59", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            string inviteCode = RandomCharGen.RandomString(8);
            bool inviteIsValid = false;

            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            // check if invitecode doesn't already exist in db
            while (inviteIsValid is false)
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "SELECT * FROM Rondes WHERE InviteCode = @InviteCode";
                        sqlCommand.Parameters.AddWithValue("@InviteCode", inviteCode);

                        SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
                        Ronde rondeDb = new Ronde();
                        while (reader.Read())
                        {
                            rondeDb.InviteCode = reader["InviteCode"].ToString();
                            
                        }

                        if (rondeDb.InviteCode == null)
                        {
                            ronde.InviteCode = inviteCode;
                            inviteIsValid = true;
                        }

                    }
                }
            }



            string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
            {
                await sqlConnectionInsert.OpenAsync();
                using (SqlCommand sqlCommandInsert = new SqlCommand())
                {
                    sqlCommandInsert.Connection = sqlConnectionInsert;
                    sqlCommandInsert.CommandText = "INSERT INTO Rondes VALUES(@RondeId, @Naam, @Admin , @InviteCode, @StartDatum, @EindDatum)";
                    sqlCommandInsert.Parameters.AddWithValue("@RondeId", ronde.RondeId);
                    sqlCommandInsert.Parameters.AddWithValue("@Naam", ronde.Naam);
                    sqlCommandInsert.Parameters.AddWithValue("@Admin", ronde.Admin);
                    sqlCommandInsert.Parameters.AddWithValue("@InviteCode", ronde.InviteCode);
                    sqlCommandInsert.Parameters.AddWithValue("@StartDatum", ronde.StartDatum);
                    sqlCommandInsert.Parameters.AddWithValue("@EindDatum", ronde.EindDatum);



                    await sqlCommandInsert.ExecuteNonQueryAsync();

                    return new OkObjectResult(ronde);
                   
                }
            }




        }


        [FunctionName("UpdateRonde")]
        public static async Task<IActionResult> UpdateRonde(
           [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "rondes/update")] HttpRequest req,
           ILogger log)
        {

            //Account create:
            //naar api sturen email en gebruikersnaam via json gebruikersnaam is optioneel, vb:
            //{
            //"rondeId": "4dca58c9-7e62-4bcb-8b05-b81b15a1ba75",
            //"naam": "testronde1",
            //"startDatum": "2021-01-05T15:00:00",
            //"eindDatum": "2020-12-31T11:59:59"
            //}

            //mogelijke returns:
            //als ronde gemaakt is wordt het model ronde met alle params terug gestuurd


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Ronde ronde = JsonConvert.DeserializeObject<Ronde>(requestBody);


            string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
            {
                await sqlConnectionInsert.OpenAsync();
                using (SqlCommand sqlCommandInsert = new SqlCommand())
                {
                    sqlCommandInsert.Connection = sqlConnectionInsert;
                    sqlCommandInsert.CommandText = "UPDATE Rondes SET Naam = @Naam,StartDatum = @StartDatum,EindDatum = @EindDatum WHERE RondeId = @RondeId";
                    sqlCommandInsert.Parameters.AddWithValue("@RondeId", ronde.RondeId);
                    sqlCommandInsert.Parameters.AddWithValue("@Naam", ronde.Naam);
                    sqlCommandInsert.Parameters.AddWithValue("@StartDatum", ronde.StartDatum);
                    sqlCommandInsert.Parameters.AddWithValue("@EindDatum", ronde.EindDatum);



                    await sqlCommandInsert.ExecuteNonQueryAsync();

                    return new OkObjectResult(ronde);

                }
            }




        }

        [FunctionName("AddDeelenmerToRonde")]
        public static async Task<IActionResult> AddDeelenmerToRonde(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "deelnemer/add")] HttpRequest req,
          ILogger log)
        {

            //{
            //    "rondeid": "4cc9e0df-1ded-4dc6-8000-b4d27fd3027a",
            //    "gebruikerId": "986c5f65-cafc-43ce-8fa5-bf4f97921e92"
            //}


            //mogelijke returns:
            //als ronde gemaakt is wordt het model deelnemer met alle params terug gestuurd


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Deelnemer deelnemer = JsonConvert.DeserializeObject<Deelnemer>(requestBody);


            string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
            {
                await sqlConnectionInsert.OpenAsync();
                using (SqlCommand sqlCommandInsert = new SqlCommand())
                {
                    sqlCommandInsert.Connection = sqlConnectionInsert;
                    sqlCommandInsert.CommandText = "INSERT INTO Deelnemers VALUES(@GebruikerId, @RondeId)";
                    sqlCommandInsert.Parameters.AddWithValue("@RondeId", deelnemer.RondeId);
                    sqlCommandInsert.Parameters.AddWithValue("@GebruikerId", deelnemer.GebruikerId);



                    await sqlCommandInsert.ExecuteNonQueryAsync();

                    return new OkObjectResult(deelnemer);

                }
            }




        }

    }
}
