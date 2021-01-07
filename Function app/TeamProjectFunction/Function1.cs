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
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", "get", Route = "gebruikers/login")] HttpRequest req,
            ILogger log)
        {

            //Account login:
            //naar api sturen email json gebruikersnaam niet verplicht bij inloggen, wel verplicht als de account voor het eerst aangemaakt wordt , vb:
            //{
            //"email": "test1@email.com",
            //"gebruikersnaam": "test1"
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
                    sqlCommand.CommandText = "SELECT * FROM Gebruikers WHERE Email = @Email";
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
                        //gebruiker bestond nog niet
                        gebruikerLogin.GebruikerId = Guid.NewGuid();
                        using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionString))
                        {
                            await sqlConnectionInsert.OpenAsync();
                            using (SqlCommand sqlCommandInsert = new SqlCommand())
                            {
                                sqlCommandInsert.Connection = sqlConnectionInsert;
                                sqlCommandInsert.CommandText = "INSERT INTO Gebruikers VALUES (@GebruikerId, @GebruikersNaam, @Email)";
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

        [FunctionName("AddDeelnemerToRonde")]
        public static async Task<IActionResult> AddDeelnemerToRonde(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "deelnemer/add")] HttpRequest req,
          ILogger log)
        {

            //{
            //    "invitecode": "4cc9e0df-1ded-4dc6-8000-b4d27fd3027a",
            //    "gebruikerId": "986c5f65-cafc-43ce-8fa5-bf4f97921e92"
            //}


            //mogelijke returns:
            //als ronde gemaakt is wordt het model deelnemer met alle params terug gestuurd


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Deelnemer deelnemer = JsonConvert.DeserializeObject<Deelnemer>(requestBody);
            deelnemer.DeelnemerId = Guid.NewGuid();
            Ronde ronde = JsonConvert.DeserializeObject<Ronde>(requestBody);

            //controleren als invitecode bestaat
            string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnection = new SqlConnection(connectionStringInsert))
            {
                await sqlConnection.OpenAsync();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "SELECT * FROM Rondes WHERE InviteCode = @InviteCode";
                    sqlCommand.Parameters.AddWithValue("@InviteCode", ronde.InviteCode);

                    SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
                    Ronde rondeDb = new Ronde();
                    while (reader.Read())
                    {

                        rondeDb.RondeId = Guid.Parse(reader["RondeId"].ToString());

                    }
                    if (rondeDb.RondeId != null && rondeDb.RondeId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    {
                        //invitecode bestaat
                        ronde.RondeId = rondeDb.RondeId;

                        using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
                        {
                            await sqlConnectionInsert.OpenAsync();
                            using (SqlCommand sqlCommandInsert = new SqlCommand())
                            {
                                sqlCommandInsert.Connection = sqlConnectionInsert;
                                sqlCommandInsert.CommandText = "INSERT INTO Deelnemers VALUES(@DeelnemerId, @GebruikerId, @RondeId)";
                                sqlCommandInsert.Parameters.AddWithValue("@DeelnemerId", deelnemer.DeelnemerId);
                                sqlCommandInsert.Parameters.AddWithValue("@RondeId", ronde.RondeId);
                                sqlCommandInsert.Parameters.AddWithValue("@GebruikerId", deelnemer.GebruikerId);

                                //deelnemer.RondeId = ronde.RondeId;

                                await sqlCommandInsert.ExecuteNonQueryAsync();

                                return new OkObjectResult(deelnemer);

                            }
                        }


                    }

                    else
                    {
                        CustomResponse customResponse = new CustomResponse();
                        customResponse.Succes = false;
                        customResponse.Message = "Invitecode betsaat niet";
                        return new OkObjectResult(customResponse);
                    }



                }
            }




        }


        [FunctionName("DelDeelnemerfromRonde")]
        public static async Task<IActionResult> DelDeelnemerfromRonde(
          [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "deelnemer/del")] HttpRequest req,
          ILogger log)
        {

            //{
            //    "deelnemerId": "f0429da0-f5cd-4f39-bb88-cc46d14bae20"
            //}


            //mogelijke returns:
            //als ronde verwijderd is wordt het model deelnemer met alle params terug gestuurd


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Deelnemer deelnemer = JsonConvert.DeserializeObject<Deelnemer>(requestBody);


            string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
            {
                await sqlConnectionInsert.OpenAsync();
                using (SqlCommand sqlCommandInsert = new SqlCommand())
                {
                    sqlCommandInsert.Connection = sqlConnectionInsert;
                    sqlCommandInsert.CommandText = "DELETE FROM Deelnemers WHERE deelnemerId = @deelnemerId";
                    sqlCommandInsert.Parameters.AddWithValue("@deelnemerId", deelnemer.DeelnemerId);



                    await sqlCommandInsert.ExecuteNonQueryAsync();

                    return new OkObjectResult(deelnemer);

                }
            }

        }



        [FunctionName("CreateEtappe")]
        public static async Task<IActionResult> CreateEtappe(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "etappes/create")] HttpRequest req,
           ILogger log)
        {

            //Account create:
            //naar api sturen email en gebruikersnaam via json gebruikersnaam is optioneel, vb:
            //{
            //"laps" : 3,
            //"rondeid": "55c83dc3-8bc6-43de-b1ae-50fdf9a10590",
            //"lapafstand": 102.33,
            //"starttijd": "2021-01-05T15:00:00"
            //}

            //mogelijke returns:
            //als etappe gemaakt is wordt het model etappe met alle params terug gestuurd


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Etappe etappe = JsonConvert.DeserializeObject<Etappe>(requestBody);
            etappe.EtappeId = Guid.NewGuid();
            

            string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
            {
                await sqlConnectionInsert.OpenAsync();
                using (SqlCommand sqlCommandInsert = new SqlCommand())
                {
                    sqlCommandInsert.Connection = sqlConnectionInsert;
                    sqlCommandInsert.CommandText = "INSERT INTO Etappes VALUES(@EtappeId, @Laps, @RondeId, @StartTijd, @LapAfstand)";
                    sqlCommandInsert.Parameters.AddWithValue("@EtappeId", etappe.EtappeId);
                    sqlCommandInsert.Parameters.AddWithValue("@Laps", etappe.Laps);
                    sqlCommandInsert.Parameters.AddWithValue("@RondeId", etappe.RondeId);
                    sqlCommandInsert.Parameters.AddWithValue("@LapAfstand", etappe.LapAfstand);
                    sqlCommandInsert.Parameters.AddWithValue("@StartTijd", etappe.StartTijd);



                    await sqlCommandInsert.ExecuteNonQueryAsync();

                    return new OkObjectResult(etappe);

                }
            }




        }


        [FunctionName("UpdateEtappe")]
        public static async Task<IActionResult> UpdateEtappe(
          [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "etappe/update")] HttpRequest req,
          ILogger log)
        {

            //Account create:
            //naar api sturen email en gebruikersnaam via json gebruikersnaam is optioneel, vb:
            //{
            //"laps" : 3,
            //"rondeid": "55c83dc3-8bc6-43de-b1ae-50fdf9a10590",
            //"lapafstand": 105.33,
            //"starttijd": "2021-01-05T15:00:00",
            //"EtappeId" : "6B2C2BE0-C30C-4742-8ED2-83A14B4FE066"
            //}

            //mogelijke returns:
            //als etappe aangepast is wordt het model etappe met alle params terug gestuurd


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Etappe etappe = JsonConvert.DeserializeObject<Etappe>(requestBody);


            string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
            {
                await sqlConnectionInsert.OpenAsync();
                using (SqlCommand sqlCommandInsert = new SqlCommand())
                {
                    sqlCommandInsert.Connection = sqlConnectionInsert;
                    sqlCommandInsert.CommandText = "UPDATE Etappes SET Laps = @Laps, StartTijd = @StartTijd, LapAfstand = @LapAfstand WHERE EtappeId = @EtappeId";
                    sqlCommandInsert.Parameters.AddWithValue("@EtappeId", etappe.EtappeId);
                    sqlCommandInsert.Parameters.AddWithValue("@Laps", etappe.Laps);
                    sqlCommandInsert.Parameters.AddWithValue("@StartTijd", etappe.StartTijd);
                    sqlCommandInsert.Parameters.AddWithValue("@LapAfstand", etappe.LapAfstand);



                    await sqlCommandInsert.ExecuteNonQueryAsync();

                    return new OkObjectResult(etappe);

                }
            }




        }


        [FunctionName("AddLaptijd")]
        public static async Task<IActionResult> AddLaptijd(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "laptijden/add")] HttpRequest req,
           ILogger log)
        {

            //Account create:
            //naar api sturen email en gebruikersnaam via json gebruikersnaam is optioneel, vb:
            //{
            //"etappeId": "b3a5f165-3011-4fdb-bba3-8c888e7a15d9",
            //"gebruikerid": "f35d476e-55c8-484a-8b12-fb52da1c1413",
            //"tijdlap": 95,
            //"lapnummer": 1
            //}

            //mogelijke returns:
            //als laptijd gemaakt is wordt het model laptijd met alle params terug gestuurd


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            LapTijd lapTijd = JsonConvert.DeserializeObject<LapTijd>(requestBody);
            lapTijd.LapTijdId = Guid.NewGuid();


            string connectionStringInsert = Environment.GetEnvironmentVariable("ConnectionString");
            using (SqlConnection sqlConnectionInsert = new SqlConnection(connectionStringInsert))
            {
                await sqlConnectionInsert.OpenAsync();
                using (SqlCommand sqlCommandInsert = new SqlCommand())
                {
                    sqlCommandInsert.Connection = sqlConnectionInsert;
                    sqlCommandInsert.CommandText = "INSERT INTO LapTijden VALUES(@LapTijdId, @EtappeId, @GebruikerId, @TijdLap, @LapNummer)";
                    sqlCommandInsert.Parameters.AddWithValue("@LapTijdId", lapTijd.LapTijdId);
                    sqlCommandInsert.Parameters.AddWithValue("@EtappeId", lapTijd.EtappeId);
                    sqlCommandInsert.Parameters.AddWithValue("@GebruikerId", lapTijd.GebruikerId);
                    sqlCommandInsert.Parameters.AddWithValue("@TijdLap", lapTijd.TijdLap);
                    sqlCommandInsert.Parameters.AddWithValue("@LapNummer", lapTijd.LapNummer);



                    await sqlCommandInsert.ExecuteNonQueryAsync();

                    return new OkObjectResult(lapTijd);

                }
            }




        }

        [FunctionName("DelLapTijd")]
        public static async Task<IActionResult> DelLapTijd(
          [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "laptijden/del")] HttpRequest req,
          ILogger log)
        {

            //{
            //    "laptijdid": "f0429da0-f5cd-4f39-bb88-cc46d14bae20"
            //}


            //mogelijke returns:
            //als laptijd verwijderd is wordt het model laptijd met alle params terug gestuurd


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            LapTijd lapTijd = JsonConvert.DeserializeObject<LapTijd>(requestBody);

            CustomResponse customResponse = await DeleteFunctions.DelLapTijdFunction(lapTijd);

            return new OkObjectResult(customResponse);




        }

    }
}
